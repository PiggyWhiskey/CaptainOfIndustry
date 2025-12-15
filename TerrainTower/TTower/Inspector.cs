using System;
using System.Linq;

using Mafi;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.Mine;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Controllers;
using Mafi.Unity.Ui.Library;
using Mafi.Unity.Ui.Library.Inspectors;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.Utils;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Terrain Tower Inspector controlling GUI and Player Actions for the Terrain Tower Entity.
    /// </summary>
    [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
    internal class TerrainTowerInspector : BaseInspector<TerrainTowerEntity>
    {
        private readonly PolygonAreaSelectionController m_areaSelectionTool;
        private readonly IActivator m_towerAreasAndDesignatorsActivator;
        private readonly TowerAreasRenderer m_towerAreasRenderer;
        private Option<TerrainTowerEntity> m_entityUnderEdit;
        //private readonly ShortcutsManager m_shortcutsManager;

        public object ProductFactory { get; }

        public TerrainTowerInspector(
                                                                            UiContext context,
            TowerAreasRenderer towerAreasRenderer,
            NewInstanceOf<PolygonAreaSelectionController> areaSelectionTool) : base(context)
        {
            TerrainTowerInspector terrainTowerInspector = this;
            m_towerAreasRenderer = towerAreasRenderer;
            m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
            m_areaSelectionTool = areaSelectionTool.Instance;

            Lyst<ProductProto> allDumpableProducts = Context.ProtosDb
                .All<LooseProductProto>()
                .Where(x => x.CanBeOnTerrain)
                .Cast<ProductProto>().ToLyst();

            //m_inputScheduler = inputScheduler;
            //m_shortcutsManager = shortcutsManager;

            //TODO: Save/Use returned Window object for later use?
            _ = WindowSize(800.px(), Px.Auto);
            TopLeftDisplays
                .Add(new ButtonIconText(Button.Primary, "Assets/Unity/UserInterface/Toolbox/SelectArea.svg", (LocStrFormatted)Tr.ManagedArea__EditAction)
                .OnClick(new Action(activateAreaEditing)), new ButtonIcon(Button.General, "Assets/Unity/UserInterface/General/Search.svg")
                .OnClick(() => terrainTowerInspector.Context.CameraController.PanTo(terrainTowerInspector.Entity.Area.BoundingBoxCenter.CenterTile2f))
                .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.FocusManagedAreaTooltip)));

            TopLeftDisplays.Add(new MaxThroughputInfo().ObserveValue(() => Entity.Prototype.SortQuantityPerDuration.AsPartial / Entity.Prototype.SortDuration.Ticks));

            #region STATUS

            EmbedStatusToTheTop();
            this.Observe(() => Entity.CurrentState)
                .Do((state) =>
                {
                    switch (state)
                    {
                        case TerrainTowerEntity.State.Paused: Status.AsPaused(); break;
                        case TerrainTowerEntity.State.Broken: Status.AsBroken(); break;
                        case TerrainTowerEntity.State.Working: Status.AsWorking(); break;
                        case TerrainTowerEntity.State.MissingWorkers: Status.AsNoWorkers(); break;
                        case TerrainTowerEntity.State.NotEnoughPower: Status.AsNoPower(); break;

                        case TerrainTowerEntity.State.CantProcessTerrain:
                            Status.As("Can't Process Terrain".AsLoc(), DisplayState.Warning); break;
                        case TerrainTowerEntity.State.MissingDesignation:
                            Status.As("Missing Designations".AsLoc(), DisplayState.Warning); break;

                        default: Status.As("Unknown State".AsLoc(), DisplayState.Warning); break;
                    }
                });

            #endregion STATUS

            #region MIXED OUTPUT BUFFER

            BufferWithMultipleProductsUi minedProductsBuffer = new BufferWithMultipleProductsUi();
            PanelWithHeader minedProductsBufferPanel = AddPanelWithHeader(minedProductsBuffer);

            Lyst<ProductQuantity> productsBuffer = new Lyst<ProductQuantity>();

            this.ObserveIndexable(
                () =>
                {
                    Entity.GetMixedInputProducts(productsBuffer.ClearAndReturn());
                    return productsBuffer;
                })
                .Observe(() => Entity.Capacity)
                .Do((cargo, capacity) => { minedProductsBuffer.SetProducts(cargo, capacity); });

            #endregion MIXED OUTPUT BUFFER

            #region SORTED OUTPUT BUFFER

            Column component = new Column(2.pt())
            {
                c => c
                .JustifyItemsCenter()
                .AlignItemsCenter()
                .MinHeight(68.px())
                .Class(Cls.group)
            };

            Label startBySettingProducts = new Label("Start by setting products to mine".AsLoc()).Color(new ColorRgba?(Theme.PrimaryColor));
            component.Add(startBySettingProducts);

            ButtonIconText addProductBtn = new ButtonIconText("Assets/Unity/UserInterface/General/PlusThin.svg", "Add Product to sort".AsLoc())
                .Tooltip("Choose products that the Terrain Tower is allowed to Mine. Products will be mined and sorted as needed.".AsLoc()).AlignSelfCenter();
            component.Add(addProductBtn);

            Column addProductPanel = component;

            //Popup to choose a product to mine
            ProtoPickerPopup<ProductProto> protoPickerPopup = new ProtoPickerPopup<ProductProto>(
                () => Context.UnlockedProtosDbForUi.FilterUnlocked(Entity.AllSupportedProducts.Where(x => !Entity.AllowedProducts.Contains(x))),
                (Func<ProductProto, Button>)(ProductFactory ?? (ProductFactory = new Func<ProductProto, Button>(ProtoPickerFactories.ProductFactory))),
                product => ScheduleCommand(new TerrainTowerCommands.AddProductToSortCmd(Entity, product)),
                addProductBtn,
                (LocStrFormatted)Tr.ProductSelectorTitle,
                ProtoPickerConfig.Products);

            Grid buffersGrid = new Grid(2, new Px?(1.pt()), new Px?(2.pt()));

            //Add panel with a grid, 2 columns, 1px gap, 2px padding
            _ = AddPanelWithHeader(new Grid(2, new Px?(1.pt()), new Px?(2.pt())).Component)
                .Title("Products to Mine".AsLoc(), "Choose products that can be Mined in this facility.".AsLoc());

            this
                .ObserveIndexable(() => Entity.OutputBuffers)
                .Observe(() => Entity)
                .DoOnSync((buffers, entity) =>
                {
                    buffersGrid.Clear();
                    foreach (IProductBuffer buffer in buffers)
                    {
                        _ = buffersGrid.AddCached(() => new BufferUi(Context))
                                       .Value(buffer, entity);
                    }

                    if (buffers.Count < 8)
                    {
                        buffersGrid.Add(addProductPanel);
                    }

                    _ = addProductBtn.ClassIff(Cls.btn_general, buffers.Count > 0)
                                     .ClassIff(Cls.btn_primary, buffers.Count == 0);
                    _ = startBySettingProducts.Visible(buffers.Count == 0);
                });

            #endregion SORTED OUTPUT BUFFER

            #region DUMP FILTER

            //This region is for a filter that allows auto-movement from Sorted Output Buffer to the Dump Buffer
            MultipleProductsPickerUi dumpFilter = new MultipleProductsPickerUi(
                () => context.UnlockedProtosDbForUi.FilterUnlocked(allDumpableProducts),
                () => terrainTowerInspector.Entity.DumpableProducts,
                p => terrainTowerInspector.ScheduleCommand(new TerrainTowerCommands.AddProductToDumpCmd(terrainTowerInspector.Entity, p)),
                p => terrainTowerInspector.ScheduleCommand(new TerrainTowerCommands.RemoveProductToDumpCmd(terrainTowerInspector.Entity, p)),
                string.Format("({0})", Tr.DumpingFilter__Empty).AsLoc()
                );

            Row dumpFilterHeader = AddPanelWithHeader(dumpFilter).Header;

            Row dumpFilterRow = new Row
            {
                c => c.Fill().AlignSelfStretch(),
                new Label("What will auto-dump".AsLoc())
                .Tooltip("Configures which materials will be auto-moved from Sorted Buffers to the Dump Buffer".AsLoc())
                .TextCenterMiddle().FontBold()
                .FlexGrow(1f, Percent.Hundred)
            };

            dumpFilterHeader.Add(dumpFilterRow);

            #endregion DUMP FILTER

            #region DUMP BUFFER

            BufferWithMultipleProductsUi dumpBuffer = new BufferWithMultipleProductsUi();
            _ = AddPanelWithHeader(dumpBuffer).Title("Dump Buffer".AsLoc(), "Products that will be auto-dumped from the Sorted Output Buffer".AsLoc());

            //Update Dump Buffers based on Entity.DumpBuffers
            Lyst<ProductQuantity> dumpProductsCache = new Lyst<ProductQuantity>();

            this
                .ObserveIndexable(() =>
                {
                    Entity.GetMixedOutputProducts(dumpProductsCache.ClearAndReturn());
                    return dumpProductsCache;
                })
                .Observe(() => Entity.Capacity)
                .Do((cargo, capacity) =>
                {
                    dumpBuffer.SetProducts(cargo, capacity);
                });

            #endregion DUMP BUFFER
        }

        private void activateAreaEditing()
        {
            m_entityUnderEdit = (Option<TerrainTowerEntity>)Entity;
            m_towerAreasRenderer.MarkAreaUnderEdit((Option<IAreaManagingTower>)Entity);
            m_areaSelectionTool.BeginEdit
            (
                Entity.Area,
                (Fix32)Entity.Prototype.Area.MaxAreaEdgeSize.Value,
                new Action(deactivateEditing),
                new Action(reopen),
                new Action<PolygonTerrainArea2i>(onAreaChanged)
            );
        }

        private void deactivateEditing()
        {
            m_towerAreasAndDesignatorsActivator.DeactivateIfActive();
            m_towerAreasRenderer.MarkAreaUnderEdit(Option<IAreaManagingTower>.None);
        }

        private void onAreaChanged(PolygonTerrainArea2i newArea)
        {
            if (m_entityUnderEdit.HasValue)
            {
                _ = ScheduleCommand(new TerrainTowerCommands.TerrainTowerAreaChangeCmd(m_entityUnderEdit.Value.Id, newArea));
            }
        }

        private void reopen()
        {
            if (m_entityUnderEdit.HasValue)
            {
                Context.InspectorsManager.TryActivateFor(m_entityUnderEdit.Value);
            }
            m_entityUnderEdit = Option<TerrainTowerEntity>.None;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            m_towerAreasRenderer.HighlightTowerArea((Option<IAreaManagingTower>)Entity);
            m_towerAreasAndDesignatorsActivator.ActivateIfNotActive();
            m_entityUnderEdit = Option<TerrainTowerEntity>.None;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (m_entityUnderEdit.IsNone) m_towerAreasAndDesignatorsActivator.DeactivateIfActive();
            m_towerAreasRenderer.HighlightTowerArea((Option<IAreaManagingTower>)Option.None);
        }

        private class BufferUi : Column
        {
            private IProductBuffer Buffer { get; set; }

            private TerrainTowerEntity Entity { get; set; }

            public BufferUi(UiContext context) : base(gap: new Px?())
            {
                Px? gap = new Px?();
                BufferUi bufferUi = this.Class(Cls.hoverContainer)
                                        .StyleGroup()
                                        .PaddingTopBottom(1.pt())
                                        .PaddingLeftRight(2.pt());

                ProductBufferUi buffer = AddAndReturn(new ProductBufferUi().Fill());
                Icon icon = buffer.Icon;
                Icon component = new Icon("Assets/Unity/UserInterface/EntityIcons/Warning.png").Medium();
                gap = new Px?(-4.px());
                Px? nullable = new Px?(-4.px());
                Px? top = new Px?();
                Px? right = gap;
                Px? bottom = nullable;
                Px? left = new Px?();
                Icon child = component.AbsolutePosition(top, right, bottom, left);
                Icon errorIcon = icon.AddAndReturn(child);
                DisplayWithIcon producedDisplay;

                this.Observe(() => bufferUi.Buffer.Product)
                    .Observe(() => !bufferUi.Entity.IsPortSetForProduct(bufferUi.Buffer.Product))
                    .Do((product, isPortNotSet) =>
                {
                    _ = isPortNotSet
                        ? buffer.Icon.Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.OreSorter_ProductNotMapped), isError: true)
                        : buffer.Icon.Tooltip(new LocStrFormatted?((LocStrFormatted)product.Strings.Name));
                    _ = errorIcon.Visible(isPortNotSet);
                });
                _ = buffer.HideTitle();
                _ = buffer.LeftRow.Gap(new Px?(0.pt()));
                _ = buffer
                    .RightRow
                    .AddAndReturn(
                        producedDisplay = new DisplayWithIcon("Assets/Unity/UserInterface/General/Calendar_Time.svg")
                                                .MinDigits(3)
                                                .SuperCompact()
                                                .Tooltip(new LocStrFormatted?(Tr.ProducedLastMonth.AppendAs60())));

                this.Observe(() => bufferUi.Entity.GetSortedLastMonth(bufferUi.Buffer.Product))
                    .Do(produced =>
                    {
                        _ = producedDisplay.Value(produced)
                                           .State(produced.IsPositive ? DisplayState.Positive : DisplayState.Inactive);
                    });

                buffer.LeftRow.Add(new ButtonIcon(Button.IconOnly, "Assets/Unity/UserInterface/General/Bell128.png")
                    .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.OreSorter_BlockedAlert__Tooltip))
                    .Toggleable()
                    .MarginRight(1.pt())
                    .OnClick(
                        () => context.InputScheduler.ScheduleInputCmd(
                            new TerrainTowerCommands.SetBlockedProductAlertCmd(bufferUi.Entity.Id, bufferUi.Buffer.Product.Id, !bufferUi.Entity.IsProductBlockedAlertEnabled(bufferUi.Buffer.Product))))
                    .ObserveSelected(() => bufferUi.Entity.IsProductBlockedAlertEnabled(bufferUi.Buffer.Product)), new ButtonIcon(Button.IconOnlyDanger, "Assets/Unity/UserInterface/General/Trash128.png").Class(Cls.showOnParentHover)
                    .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.OreSorter_UnassignProduct))
                    .OnClick(
                        () => context.InputScheduler.ScheduleInputCmd(
                            new TerrainTowerCommands.RemoveProductToSortCmd(bufferUi.Entity, bufferUi.Buffer.Product))));

                this.Observe(() => bufferUi.Buffer.Product)
                    .Observe(() => bufferUi.Buffer.Quantity)
                    .Observe(() => bufferUi.Buffer.Capacity)
                    .Observe(() => !bufferUi.Entity.IsPortSetForProduct(bufferUi.Buffer.Product))
                    .Do((product, quantity, capacity, portNotSet) =>
                {
                    _ = buffer
                    .Values((Option<ProductProto>)product, quantity, capacity, true)
                    .State(portNotSet ? DisplayState.Danger : (quantity >= capacity ? DisplayState.Warning : DisplayState.Neutral));
                });
            }

            public BufferUi Value(
                IProductBuffer buffer,
                TerrainTowerEntity entity)
            {
                Buffer = buffer;
                Entity = entity;
                return this;
            }
        }

        private class SinglePortMap : Column
        {
            private readonly Label m_portNameLabel;
            private int m_portIndex;

            private TerrainTowerEntity Entity { get; set; }

            public SinglePortMap(UiContext context) : base()
            {
                SinglePortMap singlePortMap = this.StyleGroup()
                                                  .MarginRightBottom(2.pt());

                m_portNameLabel = AddAndReturn(new Label().Class(Cls.groupHeader)
                                                          .AlignSelfStretch()
                                                          .PaddingTopBottom(2.px())
                                                          .TextCenterMiddle());

                MultipleProductsPickerUi component = AddAndReturn
                (
                    new MultipleProductsPickerUi
                    (
                        () => singlePortMap.Entity.AllowedProducts,
                        () => singlePortMap.Entity.GetPortProducts(singlePortMap.m_portIndex),
                        p => context.InputScheduler.ScheduleInputCmd(new TerrainTowerCommands.SetProductPortCmd(singlePortMap.Entity, p, singlePortMap.m_portIndex, false)),
                        p => context.InputScheduler.ScheduleInputCmd(new TerrainTowerCommands.SetProductPortCmd(singlePortMap.Entity, p, singlePortMap.m_portIndex, true)),
                        LocStrFormatted.Empty,
                        Button.IconOnly,
                        true,
                        true
                    )
                );

                component.MarginLeftRight(1.pt())
                    .InsertAt(0, new Icon("Assets/Unity/UserInterface/General/Empty128.png")
                    .Size(32.px())
                    .Color(new ColorRgba?(Theme.InactiveColor))
                    .ObserveVisible(this, () => singlePortMap.Entity.GetPortProducts(singlePortMap.m_portIndex).IsEnumerableEmpty())
                    );

                component.InsertAt(2, new VerticalDivider().MarginTopBottom(6.px()));
            }

            public SinglePortMap Value(
                TerrainTowerEntity entity,
                int portIndex,
                char portId)
            {
                Entity = entity;
                m_portIndex = portIndex;
                _ = m_portNameLabel.Value(portId.ToString().AsLoc());
                return this;
            }
        }
    }
}