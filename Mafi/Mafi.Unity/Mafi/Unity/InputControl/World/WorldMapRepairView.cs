// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldMapRepairView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  public class WorldMapRepairView : IUiElementWithUpdater, IUiElement, IDynamicSizeElement
  {
    private readonly Btn m_repairBtn;
    private readonly PricePanel m_repairPrice;
    private readonly StackContainer m_container;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public event Action<IUiElement> SizeChanged;

    public WorldMapRepairView(
      UiBuilder builder,
      IInputScheduler inputScheduler,
      Func<WorldMapRepairableEntity> entityProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      WorldMapRepairView worldMapRepairView = this;
      this.m_container = builder.NewStackContainer("Trades container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic);
      this.m_container.SizeChanged += (Action<IUiElement>) (x =>
      {
        Action<IUiElement> sizeChanged = worldMapRepairView.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) worldMapRepairView);
      });
      this.m_repairPrice = new PricePanel(builder, builder.Style.PricePanel.VehiclesBuyPricePanelStyle, (Option<IAvailableProductsProvider>) Option.None);
      this.m_repairPrice.AppendTo<PricePanel>(this.m_container, new Vector2?(new Vector2(120f, this.m_repairPrice.PreferredHeight)), ContainerPosition.MiddleOrCenter);
      this.m_repairBtn = builder.NewBtnPrimary("Start repairs").EnableDynamicSize().SetText((LocStrFormatted) TrCore.StartRepairs).OnClick(new Action(startRepair)).AppendTo<Btn>(this.m_container, new Vector2?(), ContainerPosition.MiddleOrCenter, Offset.Top(5f));
      ConstructionProgressView repairView = new ConstructionProgressView((IUiElement) this.m_container, builder, (Func<Option<IConstructionProgress>>) (() => entityProvider().ConstructionProgress));
      repairView.AppendTo<ConstructionProgressView>(this.m_container, new float?(95f)).SetBackground(builder.Style.Panel.ItemOverlay);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().IsBeingRepaired)).Do((Action<bool>) (isBeingRepaired =>
      {
        closure_0.m_container.SetItemVisibility((IUiElement) repairView, isBeingRepaired);
        closure_0.m_container.SetItemVisibility((IUiElement) closure_0.m_repairPrice, !isBeingRepaired);
        closure_0.m_container.SetItemVisibility((IUiElement) closure_0.m_repairBtn, !isBeingRepaired);
      }));
      updaterBuilder.Observe<AssetValue>((Func<AssetValue>) (() => entityProvider().CostToRepair)).Do((Action<AssetValue>) (price => worldMapRepairView.m_repairPrice.SetPrice(price)));
      this.Updater = updaterBuilder.Build();
      this.Updater.AddChildUpdater(repairView.Updater);

      void startRepair()
      {
        inputScheduler.ScheduleInputCmd<WorldMapEntityStartRepairCmd>(new WorldMapEntityStartRepairCmd(entityProvider().Id));
      }
    }

    public void SetButtonText(LocStr btnText, LocStr? toolTip = null)
    {
      this.m_repairBtn.SetText((LocStrFormatted) btnText);
      if (!toolTip.HasValue)
        return;
      this.m_repairBtn.AddToolTip(toolTip.Value);
    }
  }
}
