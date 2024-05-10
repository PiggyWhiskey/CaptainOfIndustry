// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.WasteSortingPlantWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Waste;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class WasteSortingPlantWindowView : StaticEntityInspectorBase<WasteSortingPlant>
  {
    private readonly WasteSortingPlantInspector m_controller;
    private readonly ProductsManager m_productsManager;
    private InputProductBufferViewWithUpdater.Cache m_inputBuffersCache;
    private OutputProductBufferViewWithUpdater.Cache m_outputBuffersCache;

    protected override WasteSortingPlant Entity => this.m_controller.SelectedEntity;

    public WasteSortingPlantWindowView(
      WasteSortingPlantInspector controller,
      ProductsManager productsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_productsManager = productsManager;
      this.m_controller = controller.CheckNotNull<WasteSortingPlantInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      int width = 700;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_inputBuffersCache = new InputProductBufferViewWithUpdater.Cache(this.Builder);
      this.AddUpdater(this.m_inputBuffersCache.Updater);
      this.m_outputBuffersCache = new OutputProductBufferViewWithUpdater.Cache(this.Builder);
      this.AddUpdater(this.m_outputBuffersCache.Updater);
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) TrCore.RecyclingEfficiency__Title, new LocStrFormatted?((LocStrFormatted) TrCore.RecyclingEfficiency__Tooltip));
      Panel parent = this.AddOverlayPanel(this.ItemsContainer);
      Txt ratioTxt = this.Builder.NewTxt("RecyclingRatioTxt").SetText("").SetTextStyle(this.Builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) parent, Offset.Left(20f));
      int columnsCount = 2;
      int x1 = (width - 5 * (columnsCount - 1)) / columnsCount;
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle);
      GridContainer inputBuffersContainer = this.Builder.NewGridContainer("InputsGrid").SetCellSize(new Vector2((float) x1, InputProductBufferViewWithUpdater.RequiredHeight)).SetCellSpacing(5f).SetDynamicHeightMode(columnsCount).AppendTo<GridContainer>(itemContainer);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle, new LocStrFormatted?((LocStrFormatted) Tr.WasteSortingOutputs__Tooltip));
      GridContainer outputBuffersContainer = this.Builder.NewGridContainer("OutputGrid").SetCellSize(new Vector2((float) x1, InputProductBufferViewWithUpdater.RequiredHeight)).SetCellSpacing(5f).SetDynamicHeightMode(columnsCount).AppendTo<GridContainer>(itemContainer);
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.m_productsManager.RecyclingRatio)).Do((Action<Percent>) (ratio => ratioTxt.SetText(ratio.ToStringRounded())));
      updaterBuilder.Observe<WasteSortingPlant.State>((Func<WasteSortingPlant.State>) (() => this.Entity.CurrentState)).Do((Action<WasteSortingPlant.State>) (state =>
      {
        switch (state)
        {
          case WasteSortingPlant.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case WasteSortingPlant.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case WasteSortingPlant.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case WasteSortingPlant.State.MissingInput:
            statusInfo.SetStatus(Tr.EntityStatus__WaitingForProducts, StatusPanel.State.Warning);
            break;
          case WasteSortingPlant.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case WasteSortingPlant.State.NotEnoughPower:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          case WasteSortingPlant.State.FullOutput:
            statusInfo.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Critical);
            break;
        }
      }));
      updaterBuilder.Observe<IProductBuffer>((Func<IEnumerable<IProductBuffer>>) (() => (IEnumerable<IProductBuffer>) this.Entity.InputBuffers), (ICollectionComparator<IProductBuffer, IEnumerable<IProductBuffer>>) CompareFixedOrder<IProductBuffer>.Instance).Observe<bool>((Func<bool>) (() => this.Builder.DurationNormalizer.IsNormalizationOn)).Observe<WasteSortingPlantProto>((Func<WasteSortingPlantProto>) (() => this.Entity.Prototype)).Do((Action<Lyst<IProductBuffer>, bool, WasteSortingPlantProto>) ((buffers, isNormalized, proto) =>
      {
        inputBuffersContainer.StartBatchOperation();
        inputBuffersContainer.ClearAll();
        this.m_inputBuffersCache.ReturnAll();
        foreach (IProductBuffer buffer1 in buffers)
        {
          IProductBuffer buffer = buffer1;
          InputProductBufferViewWithUpdater view = this.m_inputBuffersCache.GetView();
          view.Show<InputProductBufferViewWithUpdater>();
          view.AppendTo<InputProductBufferViewWithUpdater>(inputBuffersContainer);
          Quantity quantity = proto.SupportedInputs.FirstOrDefault((Func<ProductQuantity, bool>) (x => (Proto) x.Product == (Proto) buffer.Product)).Quantity;
          Fix32 maxQuantity = this.Builder.DurationNormalizer.NormalizeQuantity(buffer.Product, quantity, proto.Duration);
          Duration normalizedDuration = this.Builder.DurationNormalizer.GetNormalizedDuration(proto.Duration);
          view.SetInputBuffer(buffer, maxQuantity, normalizedDuration);
        }
        inputBuffersContainer.FinishBatchOperation();
        itemContainer.UpdateItemHeight((IUiElement) inputBuffersContainer, inputBuffersContainer.GetRequiredHeight());
      }));
      updaterBuilder.Observe<IProductBuffer>((Func<IIndexable<IProductBuffer>>) (() => this.Entity.OutputBuffers), (ICollectionComparator<IProductBuffer, IIndexable<IProductBuffer>>) CompareFixedOrder<IProductBuffer>.Instance).Do((Action<Lyst<IProductBuffer>>) (buffers =>
      {
        outputBuffersContainer.StartBatchOperation();
        outputBuffersContainer.ClearAll();
        this.m_outputBuffersCache.ReturnAll();
        foreach (IProductBuffer buffer in buffers)
        {
          OutputProductBufferViewWithUpdater view = this.m_outputBuffersCache.GetView();
          view.Show<OutputProductBufferViewWithUpdater>();
          view.AppendTo<OutputProductBufferViewWithUpdater>(outputBuffersContainer);
          view.SetOutputBuffer(buffer, this.Entity);
        }
        outputBuffersContainer.FinishBatchOperation();
        itemContainer.UpdateItemHeight((IUiElement) outputBuffersContainer, outputBuffersContainer.GetRequiredHeight());
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth((float) width);
    }
  }
}
