// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldEntityShipOrdersView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Localization;
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
  public class WorldEntityShipOrdersView : IUiElementWithUpdater, IUiElement, IDynamicSizeElement
  {
    private readonly StackContainer m_container;
    private LocationVisitReason m_lastKnownReason;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public event Action<IUiElement> SizeChanged;

    public WorldEntityShipOrdersView(
      UiBuilder builder,
      TravelingFleetManager fleetManager,
      Func<WorldMapRepairableEntity> entityProvider,
      Action<WorldMapLocation, LocationVisitReason> onGoToClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      WorldEntityShipOrdersView entityShipOrdersView = this;
      this.m_container = builder.NewStackContainer("OrdersContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic);
      this.m_container.SizeChanged += (Action<IUiElement>) (x =>
      {
        Action<IUiElement> sizeChanged = entityShipOrdersView.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) entityShipOrdersView);
      });
      builder.NewTxt("ShipOrders").SetText((LocStrFormatted) Tr.WorldLocation_Orders).SetTextStyle(builder.Style.Panel.SectionTitle).SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(this.m_container, new float?(builder.Style.Panel.LineHeight), new Offset(0.0f, builder.Style.Panel.SectionTitleTopPadding, builder.Style.Panel.Padding, 0.0f));
      Btn visitBtn = builder.NewBtnPrimary("Visit").PlayErrorSoundWhenDisabled().SetText((LocStrFormatted) Tr.WorldLocation_Orders__LoadCargo).OnClick((Action) (() => onGoToClick(entityProvider().Location, entityShipOrdersView.m_lastKnownReason)));
      visitBtn.AppendTo<Btn>(this.m_container, new Vector2?(visitBtn.GetOptimalSize() + new Vector2(0.0f, 8f)), ContainerPosition.LeftOrTop, Offset.Top(5f) + Offset.Left(20f));
      Tooltip visitToolTip = builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) visitBtn);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<TravelingFleetManager.LocationVisitCheckResult>((Func<TravelingFleetManager.LocationVisitCheckResult>) (() => fleetManager.CanRequestLocationVisit(entityProvider().Location))).Observe<bool>((Func<bool>) (() =>
      {
        WorldMapRepairableEntity repairableEntity = entityProvider();
        bool flag1 = repairableEntity.IsUnderConstruction && repairableEntity.ConstructionProgress.Value.Buffers.Any((Func<IProductBufferReadOnly, bool>) (x => x.IsNotFull()));
        bool flag2 = false;
        if (flag1)
        {
          foreach (IProductBufferReadOnly buffer in repairableEntity.ConstructionProgress.Value.Buffers)
          {
            IProductBuffer productBuffer;
            if (fleetManager.TravelingFleet.Cargo.TryGetValue(buffer.Product, out productBuffer))
            {
              if (productBuffer.Quantity < buffer.UsableCapacity)
              {
                flag2 = true;
                break;
              }
            }
            else
            {
              flag2 = true;
              break;
            }
          }
        }
        return flag2;
      })).Do((Action<TravelingFleetManager.LocationVisitCheckResult, bool>) ((visitResult, disableForLackOfCargo) =>
      {
        visitToolTip.SetText((LocStrFormatted) LocationVisitCheckResultHelper.MapToToolTip(visitResult));
        visitBtn.SetEnabled(visitResult == TravelingFleetManager.LocationVisitCheckResult.Ok && !disableForLackOfCargo);
      }));
      updaterBuilder.Observe<LocStr>((Func<LocStr>) (() =>
      {
        WorldMapRepairableEntity repairableEntity = entityProvider();
        bool flag = repairableEntity is WorldMapMine worldMapMine2 && worldMapMine2.Buffer.IsNotEmpty();
        if (repairableEntity.IsUnderConstruction && repairableEntity.ConstructionProgress.Value.Buffers.Any((Func<IProductBufferReadOnly, bool>) (x => x.IsNotFull())))
        {
          entityShipOrdersView.m_lastKnownReason = LocationVisitReason.DeliverCargo;
          return Tr.WorldLocation_Orders__DeliverCargo;
        }
        if (flag)
        {
          entityShipOrdersView.m_lastKnownReason = LocationVisitReason.LoadCargo;
          return Tr.WorldLocation_Orders__LoadCargo;
        }
        entityShipOrdersView.m_lastKnownReason = LocationVisitReason.General;
        return Tr.WorldLocation_Orders__Visit;
      })).Do((Action<LocStr>) (text =>
      {
        visitBtn.SetText((LocStrFormatted) text);
        visitBtn.SetWidth<Btn>(visitBtn.GetOptimalWidth());
      }));
      this.Updater = updaterBuilder.Build();
    }
  }
}
