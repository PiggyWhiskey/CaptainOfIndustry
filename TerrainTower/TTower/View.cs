using Mafi;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;

using System;
using System.Linq;

using TerrainTower.Extras;

using UnityEngine;

using static TerrainTower.TTower.TerrainTowerCommands;

namespace TerrainTower.TTower
{
    internal class TerrainTowerWindowView : StaticEntityInspectorBase<TerrainTowerEntity>
    {
        private const int m_width = 800;
        private readonly TerrainTowerInspector m_controller;
        private StackContainer m_dumpBufferContainer;
        private ViewsCacheHomogeneous<OutputBufferView> m_dumpBuffersCache;
        private Txt m_dumpBufferTitle;
        private Txt m_mixedBufferTitle;
        private BufferWithMultipleProductsView m_mixedBufferView;
        private Lyst<ProductQuantity> m_mixedProductsCache;
        private GridContainer m_sortedBufferContainer;
        private Txt m_sortedBufferTitle;
        private ViewsCacheHomogeneous<OutputSortedProductBufferView> m_sortedProductsCache;

        public TerrainTowerWindowView(TerrainTowerInspector controller) : base(controller)
        {
            m_controller = controller.CheckNotNull();
        }

        protected override TerrainTowerEntity Entity => m_controller.SelectedEntity;

        protected override void AddCustomItems(StackContainer itemContainer)
        {
            base.AddCustomItems(itemContainer);
            UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();

            #region STATUS PANEL

            StatusPanel statusInfo = AddStatusInfoPanel();
            statusInfo.SetStatusWorking(); //Default 'Working'

            #endregion STATUS PANEL

            //Status panel fills to the bottom of the region, Spacer added to separate from the next panel
            Panel spacer = new Panel(Builder, "spacer").SetHeight(10).AppendTo(itemContainer);

            #region MANAGEMENT PANEL

            //Build Terrain Control buttons
            TerrainConfigPanel terrainConfigToggle = new TerrainConfigPanel(
                    builder: Builder,
                    width: m_width,
                    onModeClick: state => m_controller.Context.InputScheduler.ScheduleInputCmd(new TerrainTowerConfigToggleCmd(Entity, state)),
                    onEditAreaClick: new Action(m_controller.ToggleAreaEditing))
                .AppendTo(itemContainer);

            #endregion MANAGEMENT PANEL

            #region MIXED OUTPUT BUFFER

            m_mixedBufferTitle = AddSectionTitle(itemContainer, "Mixed Output Buffer");
            m_mixedBufferView = new BufferWithMultipleProductsView(itemContainer, Builder);
            m_mixedBufferView.AppendTo(itemContainer, new float?(m_mixedBufferView.GetHeight()));
            m_mixedProductsCache = new Lyst<ProductQuantity>();

            #endregion MIXED OUTPUT BUFFER

            #region SORTED OUTPUT BUFFER

            int columnsCount = 2;
            // 5 tile buffer between columns
            int x = (m_width - (5 * (columnsCount - 1))) / columnsCount;
            m_sortedProductsCache = new ViewsCacheHomogeneous<OutputSortedProductBufferView>(() => new OutputSortedProductBufferView(Builder, m_controller.Context.InputScheduler));
            m_sortedBufferTitle = AddSectionTitle(itemContainer, "Sorted Outputs");
            m_sortedBufferContainer = Builder
                .NewGridContainer("OutputGrid")
                .SetCellSize(new Vector2(x, OutputSortedProductBufferView.RequiredHeight))
                .SetCellSpacing(5f)
                .SetDynamicHeightMode(columnsCount)
                .AppendTo(itemContainer);
            AddUpdater(m_sortedProductsCache.Updater);

            #endregion SORTED OUTPUT BUFFER

            #region DUMP BUFFER

            //Poached from Farm Output Buffer
            m_dumpBufferTitle = AddSectionTitle(itemContainer, "Dump Buffer");
            m_dumpBufferContainer = new StackContainer(Builder, "DumpContainer")
                .SetStackingDirection(StackContainer.Direction.TopToBottom)
                .SetSizeMode(StackContainer.SizeMode.Dynamic)
                .AppendTo(itemContainer);
            m_dumpBuffersCache = new ViewsCacheHomogeneous<OutputBufferView>(() => new OutputBufferView(m_dumpBufferContainer, Builder));
            AddUpdater(m_dumpBuffersCache.Updater);

            #endregion DUMP BUFFER

            #region UPDATER

            //Status Panel: Update Status
            _ = updaterBuilder
                .Observe(() => Entity.CurrentState)
                .Do((state) =>
                {
                    switch (state)
                    {
                        case TerrainTowerEntity.State.Paused:
                            statusInfo.SetStatus("Paused", StatusPanel.State.Warning);
                            break;

                        case TerrainTowerEntity.State.Broken:
                            statusInfo.SetStatus("Broken", StatusPanel.State.Critical);
                            break;

                        case TerrainTowerEntity.State.MissingDumpResource:
                            statusInfo.SetStatus("Missing Dump Resource", StatusPanel.State.Warning);
                            break;

                        case TerrainTowerEntity.State.MissingDesignation:
                            statusInfo.SetStatus("No Designation", StatusPanel.State.Warning);
                            break;

                        case TerrainTowerEntity.State.MissingWorkers:
                            statusInfo.SetStatus("No Workers", StatusPanel.State.Critical);
                            break;

                        case TerrainTowerEntity.State.NotEnoughPower:
                            statusInfo.SetStatus("Low Power", StatusPanel.State.Critical);
                            break;

                        case TerrainTowerEntity.State.Working:
                        default:
                            statusInfo.SetStatus("Working", StatusPanel.State.Ok);
                            break;
                    }
                });

            //MANAGEMENT BAR: Update - Area Description and Toggle
            updaterBuilder
                .Observe(() => Entity.TerrainConfigState)
                .Do(mode => terrainConfigToggle.SetMode(mode));
            updaterBuilder
                .Observe(() => Entity.Area)
                .Do(new Action<RectangleTerrainArea2i>(terrainConfigToggle.SetAreaDescription));

            //MIXED OUTPUTS: Observe Capacity and Products in m_mixedProductsCache to add new individual product buffers
            updaterBuilder
                .Observe(() => Entity.Capacity)
                .Observe(() =>
                {
                    m_mixedProductsCache.Clear();
                    Entity.GetMixedInputProducts(m_mixedProductsCache);
                    return (IIndexable<ProductQuantity>)m_mixedProductsCache;
                }, CompareFixedOrder<ProductQuantity>.Instance)
                .Do((capacity, cargo) =>
                {
                    m_mixedBufferView.SetProducts(cargo, capacity, false);
                });

            //SORTED OUTPUTS: Update Sorted Output Buffers based on Entity.OutputBuffers
            updaterBuilder
                .Observe(() => Entity.OutputBuffers, CompareFixedOrder<IProductBuffer>.Instance)
                .Do(buffers =>
                {
                    m_sortedBufferContainer.StartBatchOperation();
                    m_sortedBufferContainer.ClearAll();
                    m_sortedProductsCache.ReturnAll();
                    foreach (IProductBuffer buffer in buffers)
                    {
                        OutputSortedProductBufferView view = m_sortedProductsCache.GetView();
                        view.Show();
                        view.AppendTo(m_sortedBufferContainer);
                        view.SetOutputBuffer(buffer, Entity);
                    }
                    m_sortedBufferContainer.FinishBatchOperation();
                    itemContainer.UpdateItemHeight(m_sortedBufferContainer, m_sortedBufferContainer.GetRequiredHeight());
                });

            //Update Dump Buffers based on Entity.DumpBuffers
            updaterBuilder
                .Observe(() => Entity.DumpBuffers.Where(y => y.Quantity.IsPositive), CompareFixedOrder<IProductBuffer>.Instance)
                .Do(buffers =>
                {
                    m_dumpBufferContainer.ClearAll();
                    m_dumpBuffersCache.ReturnAll();
                    m_dumpBufferContainer.StartBatchOperation();
                    foreach (IProductBuffer buffer in buffers)
                    {
                        //TODO: Check if we need to hide 0 quantity products
                        m_dumpBuffersCache
                            .GetView()
                            .SetBuffer(buffer)
                            .AppendTo(m_dumpBufferContainer);
                    }
                    m_dumpBufferContainer.FinishBatchOperation();
                    itemContainer.SetItemVisibility(m_dumpBufferTitle, buffers.IsNotEmpty);
                });

            //Show/Hide panels based on Dump/Flatten/Mine
            updaterBuilder
                .Observe(() => Entity.TerrainConfigState)
                .Do(state =>
                {
                    //Hide items based on the current state
                    itemContainer.SetItemVisibility(m_mixedBufferTitle, state.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Mining));
                    itemContainer.SetItemVisibility(m_mixedBufferView, state.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Mining));
                    itemContainer.SetItemVisibility(m_sortedBufferTitle, state.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Mining));
                    itemContainer.SetItemVisibility(m_sortedBufferContainer, state.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Mining));
                    itemContainer.SetItemVisibility(m_dumpBufferTitle, state.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Dumping));
                    itemContainer.SetItemVisibility(m_dumpBufferContainer, state.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Dumping));
                });

            #endregion UPDATER

            AddUpdater(updaterBuilder.Build());
            SetWidth(m_width);
        }
    }
}