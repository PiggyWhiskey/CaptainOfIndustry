using Mafi;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Library;
using Mafi.Unity.Ui.Library.Inspectors;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;

using System;
using System.Linq;

#pragma warning disable

namespace TerrainTower.TTower
{
    internal class OreSortingPlantInspector : BaseInspector<OreSortingPlant>
    {
        public OreSortingPlantInspector(
            UiContext context,
            AssignedBuildingsHighlighter highlighter,
            BuildingsAssigner buildingsAssigner) : base(context)
        {
            WindowSize(800.px(), Px.Auto);
            TopLeftDisplays.Add(new MaxThroughputInfo().ObserveValue(() => Entity.Prototype.QuantityPerDuration.AsPartial / Entity.Prototype.Duration.Ticks));

            this
                .Observe(() => Entity.CurrentState)
                .Observe(() => Entity.CanAllProductsBeAcceptedForUi())
                .Do((state, canAllProductsBeAccepted) =>
            {
                switch (state)
                {
                    case OreSortingPlant.State.Paused:
                        Status.AsPaused();
                        break;

                    case OreSortingPlant.State.Broken:
                        Status.AsBroken();
                        break;

                    case OreSortingPlant.State.Working:
                        Status.AsWorking();
                        break;

                    case OreSortingPlant.State.MissingInput:
                        if (canAllProductsBeAccepted)
                        {
                            Status.As((LocStrFormatted)Tr.EntityStatus__WaitingForProducts, DisplayState.Warning);
                        }
                        else
                        {
                            Status.As((LocStrFormatted)Tr.EntityStatus__PartiallyStuck, DisplayState.Warning);
                        }
                        break;

                    case OreSortingPlant.State.MissingWorkers:
                        Status.AsNoWorkers();
                        break;

                    case OreSortingPlant.State.NotEnoughPower:
                        Status.AsNoPower();
                        break;
                }
            });

            BufferWithMultipleProductsUi cargoBuffer = new BufferWithMultipleProductsUi(true);
            AddPanelWithHeader(cargoBuffer);

            Lyst<ProductQuantity> productsCache = new Lyst<ProductQuantity>();
            this
                .ObserveIndexable(() => { Entity.GetMixedInputProducts(productsCache.ClearAndReturn()); return productsCache; })
                .Observe(() => Entity.Capacity)
                .Observe(() => Entity.ReservedTotal)
                .Do((cargo, capacity, reserved) =>
            {
                cargoBuffer.SetProducts(cargo, capacity);
                cargoBuffer.PendingBarsValues(Percent.FromRatio(reserved.Value, capacity.Value, false), Percent.Zero);
            });

            string str = Context.ProtosDb.First<MineTowerProto>().ValueOrNull?.Strings.Name.TranslatedString ?? "";
            Column component = new Column(2.pt())
            {
                c => c
                .JustifyItemsCenter()
                .AlignItemsCenter()
                .MinHeight(68.px())
                .Class(Cls.group)
            };

            Label startBySettingProducts;

            component.Add(startBySettingProducts = new Label((LocStrFormatted)Tr.OreSorter_HintToAddProduct).Color(new ColorRgba?(Theme.PrimaryColor)));

            ButtonIconText addProductBtn;

            component.Add(
                addProductBtn = new ButtonIconText("Assets/Unity/UserInterface/General/PlusThin.svg", (LocStrFormatted)Tr.OreSorter_AddProduct)
                    .Tooltip(new LocStrFormatted?(Tr.OreSorter_AllowedProducts__Tooltip.Format(str))).AlignSelfCenter()
                );

            Column addProductPanel = component;

            ProtoPickerPopup<ProductProto> protoPickerPopup = new ProtoPickerPopup<ProductProto>(
                    () => Context.UnlockedProtosDbForUi.FilterUnlocked(
                    Entity.AllSupportedProducts.Where(x => !Entity.AllowedProducts.Contains(x))),
                (Func<ProductProto, Button>)(ProductFactory ?? (ProductFactory = new Func<ProductProto, Button>(ProtoPickerFactories.ProductFactory))),
                product => ScheduleCommand(new AddProductToSortCmd(Entity, product)),
                addProductBtn,
                (LocStrFormatted)Tr.ProductSelectorTitle,
                ProtoPickerConfig.Vehicles);

            AddPanelWithHeader(new Grid(2, new Px?(1.pt()), new Px?(2.pt())).Component)
                .Title((LocStrFormatted)Tr.OreSorter_AllowedProducts__Title, Tr.OreSorter_AllowedProducts__Tooltip.Format(str));

            this
                .ObserveIndexable(() => Entity.OutputBuffers)
                .Observe(() => Entity)
                .DoOnSync((buffers, entity) =>
            {
                new Grid(2, new Px?(1.pt()), new Px?(2.pt())).Clear();

                BufferUi createBuffer() => new BufferUi(Context);
                foreach (IProductBuffer buffer in buffers)
                    new Grid(2, new Px?(1.pt()), new Px?(2.pt())).AddCached(createBuffer).Value(buffer, entity);
                if (buffers.Count < 8)
                    new Grid(2, new Px?(1.pt()), new Px?(2.pt())).Add(addProductPanel);
                addProductBtn.ClassIff(Cls.btn_general, buffers.Count > 0);
                addProductBtn.ClassIff(Cls.btn_primary, buffers.Count == 0);
                startBySettingProducts.Visible(buffers.Count == 0);
            });

            UiComponent[] uiComponentArray = new UiComponent[1];
            Px? gap = new Px?();
            Row row;
            Row portsRow = row = new Row(gap: gap).AlignSelfCenter().Wrap();
            uiComponentArray[0] = row;
            AddPanelWithHeader(uiComponentArray)
                .Title((LocStrFormatted)Tr.OreSorter_PortsMap__Title)
                .TitleTooltip((LocStrFormatted)Tr.OreSorter_PortsMap__Tooltip);

            SinglePortMap newPortMap() => new SinglePortMap(Context);
            this
                .Observe(() => Entity)
                .Observe(() => Entity.OutputPortsCount)
                .Do((entity, portsCount) =>
                {
                    portsRow.Clear();
                    for (int portIndex = 0; portIndex < portsCount; ++portIndex)
                    {
                        char portId = (char)(65 + portIndex);
                        portsRow.AddCached(newPortMap).Value(entity, portIndex, portId);
                    }
                });

            this.AddBuildingsAssignerForImport(highlighter, buildingsAssigner, LocStrFormatted.Empty);
        }

        public object ProductFactory { get; }

        private class BufferUi : Column
        {
            public BufferUi(UiContext context) : base(gap: new Px?())
            {
                BufferUi bufferUi = this;
                _ = this.Class(Cls.hoverContainer).StyleGroup().PaddingTopBottom(1.pt()).PaddingLeftRight(2.pt());

                ProductBufferUi buffer = AddAndReturn(new ProductBufferUi().Fill());

                Icon icon = buffer.Icon;
                Icon component = new Icon("Assets/Unity/UserInterface/EntityIcons/Warning.png").Medium();
                Px? gap = new Px?(-4.px());
                Px? nullable = new Px?(-4.px());
                Px? top = new Px?();
                Px? right = gap;
                Px? bottom = nullable;
                Px? left = new Px?();
                Icon child = component.AbsolutePosition(top, right, bottom, left);
                Icon errorIcon = icon.AddAndReturn(child);
                this
                    .Observe(() => bufferUi.Buffer.Product)
                    .Observe(() => bufferUi.Entity.LogisticsOutputMode)
                    .Observe(() => !bufferUi.Entity.IsPortSetForProduct(bufferUi.Buffer.Product))
                    .Observe(() => !bufferUi.Entity.CanAcceptMoreTrucksForUi(bufferUi.Buffer.Product))
                    .Do((product, logOutputMode, isPortNotSet, isBlocked) =>
                {
                    if (isPortNotSet)
                    {
                        _ = errorIcon.Color(new ColorRgba?(logOutputMode == EntityLogisticsMode.Off ? Theme.DangerColor : Theme.WarningColor));
                        _ = errorIcon.Value(logOutputMode == EntityLogisticsMode.Off ? "Assets/Unity/UserInterface/EntityIcons/Warning.png" : "Assets/Unity/UserInterface/Toolbar/Vehicles.svg");
                        _ = buffer.Icon.Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.OreSorter_ProductNotMapped), isError: true);
                    }
                    else if (isBlocked)
                    {
                        _ = errorIcon.Color(new ColorRgba?(Theme.WarningColor));
                        _ = errorIcon.Value("Assets/Unity/UserInterface/EntityIcons/Warning.png");
                        _ = buffer.Icon.Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.OreSorter_ProductBlocked__Tooltip));
                    }
                    else
                    {
                        _ = buffer.Icon.Tooltip(new LocStrFormatted?((LocStrFormatted)product.Strings.Name));
                    }
                    _ = errorIcon.Visible(isPortNotSet | isBlocked);
                });

                buffer.HideTitle();
                _ = buffer.LeftRow.Gap(new Px?(0.pt()));
                DisplayWithIcon producedDisplay;
                _ = buffer
                    .RightRow
                    .AddAndReturn(producedDisplay = new DisplayWithIcon("Assets/Unity/UserInterface/General/Calendar_Time.svg")
                    .MinDigits(3)
                    .SuperCompact()
                    .Tooltip(new LocStrFormatted?(Tr.ProducedLastMonth.AppendAs60())));

                this
                    .Observe(() => bufferUi.Entity.GetSortedLastMonth(bufferUi.Buffer.Product))
                    .Do(produced =>
                {
                    _ = producedDisplay.Value(produced);
                    _ = producedDisplay.State(produced.IsPositive ? DisplayState.Positive : DisplayState.Inactive);
                });

                buffer
                    .LeftRow
                    .Add(new ButtonIcon(Button.IconOnly, "Assets/Unity/UserInterface/General/Bell128.png")
                    .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.OreSorter_BlockedAlert__Tooltip))
                    .Toggleable()
                    .MarginRight(1.pt())
                    .OnClick
                    (
                        () => context.InputScheduler.ScheduleInputCmd(
                            new SortingPlantSetBlockedProductAlertCmd(
                                bufferUi.Entity.Id,
                                bufferUi.Buffer.Product.Id,
                                !bufferUi.Entity.IsProductBlockedAlertEnabled(bufferUi.Buffer.Product)))
                    )
                    .ObserveSelected(() => bufferUi.Entity.IsProductBlockedAlertEnabled(bufferUi.Buffer.Product)),
                    new ButtonIcon(Button.IconOnlyDanger, "Assets/Unity/UserInterface/General/Trash128.png")
                        .Class(Cls.showOnParentHover)
                        .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.OreSorter_UnassignProduct))
                        .OnClick(() => context.InputScheduler.ScheduleInputCmd(new RemoveProductToSortCmd(bufferUi.Entity, bufferUi.Buffer.Product)))
                        );

                this
                    .Observe(() => bufferUi.Buffer.Product)
                    .Observe(() => bufferUi.Buffer.Quantity)
                    .Observe(() => bufferUi.Buffer.Capacity)
                    .Observe(() => !bufferUi.Entity.IsPortSetForProduct(bufferUi.Buffer.Product))
                    .Do((p, q, c, portNotSet) =>
                {
                    buffer.Values((Option<ProductProto>)p, q, c, true);
                    buffer.State(portNotSet ? DisplayState.Danger : (q >= c ? DisplayState.Warning : DisplayState.Neutral));
                });
            }

            private IProductBuffer Buffer { get; set; }

            private OreSortingPlant Entity { get; set; }

            public BufferUi Value(
                IProductBuffer buffer,
                OreSortingPlant entity)
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

            public SinglePortMap(UiContext context) : base()
            {
                SinglePortMap singlePortMap = this;
                this.StyleGroup().MarginRightBottom(2.pt());
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
                        p => context.InputScheduler.ScheduleInputCmd(new SetProductPortCmd(singlePortMap.Entity, p, singlePortMap.m_portIndex, false)),
                        p => context.InputScheduler.ScheduleInputCmd(new SetProductPortCmd(singlePortMap.Entity, p, singlePortMap.m_portIndex, true)),
                        LocStrFormatted.Empty,
                        Button.IconOnly,
                        true,
                        true
                    )
                );

                component.MarginLeftRight(1.pt());
                component
                    .InsertAt(0, new Icon("Assets/Unity/UserInterface/General/Empty128.png")
                    .Size(32.px())
                    .Color(new ColorRgba?(Theme.InactiveColor))
                    .ObserveVisible(this, () => singlePortMap.Entity.GetPortProducts(singlePortMap.m_portIndex).IsEnumerableEmpty())
                    );

                component.InsertAt(2, new VerticalDivider().MarginTopBottom(6.px()));
            }

            private OreSortingPlant Entity { get; set; }

            public SinglePortMap Value(
                OreSortingPlant entity,
                int portIndex,
                char portId)
            {
                Entity = entity;
                m_portIndex = portIndex;
                m_portNameLabel.Value(portId.ToString().AsLoc());
                return this;
            }
        }
    }
}