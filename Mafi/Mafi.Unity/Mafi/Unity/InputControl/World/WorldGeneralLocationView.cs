// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldGeneralLocationView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Fleet;
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
  public class WorldGeneralLocationView : ItemDetailWindowView
  {
    private readonly WorldMapManager m_worldMapManager;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly Action<WorldMapLocation> m_onGoToClick;
    private WorldMapLocation m_location;
    private Btn m_exploreBtn;
    private Txt m_distanceText;
    private Txt m_infoText;
    private Txt m_shipOrdersTitle;
    private Txt m_arrivalInfo;
    private Txt m_enemyInfoTitle;
    private Txt m_enemyInfo;
    private Txt m_entityInfoTitle;
    private Txt m_entityInfo;

    public WorldGeneralLocationView(
      WorldMapManager worldMapManager,
      TravelingFleetManager fleetManager,
      Action onClose,
      Action<WorldMapLocation> onGoToClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("WorldMapLocDetailView", false);
      this.m_worldMapManager = worldMapManager;
      this.m_fleetManager = fleetManager;
      this.m_onGoToClick = onGoToClick;
      this.SetOnCloseButtonClickAction(onClose);
      this.EnableClippingPrevention();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.SetOnHideSound((Option<AudioSource>) Option.None);
      this.SetOnShowSound((Option<AudioSource>) Option.None);
      this.SetWidth(250f);
      this.MakeMovable();
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Txt txt = this.Builder.NewTxt("Info");
      TextStyle text = this.Builder.Style.Panel.Text;
      ref TextStyle local = ref text;
      FontStyle? nullable = new FontStyle?(FontStyle.Italic);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = nullable;
      int? fontSize = new int?();
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_infoText = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.ItemsContainer, new float?(20f), Offset.Bottom(5f));
      this.m_distanceText = this.Builder.NewTxt("Distance").SetTextStyle(this.Builder.Style.Panel.Text).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.ItemsContainer, new float?(20f));
      this.m_enemyInfoTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.WorldLocation_EnemyFound);
      this.m_enemyInfoTitle.SetColor((ColorRgba) 15946543);
      this.m_enemyInfo = this.Builder.NewTxt("EnemyInfo").SetTextStyle(this.Builder.Style.Panel.Text).SetAlignment(TextAnchor.UpperLeft).AppendTo<Txt>(this.ItemsContainer, new float?(60f), Offset.Left(20f));
      this.m_entityInfoTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.WorldLocation_StructureFound);
      this.m_entityInfoTitle.SetColor((ColorRgba) 16765507);
      this.m_entityInfo = this.Builder.NewTxt("EntityInfo").SetTextStyle(this.Builder.Style.Panel.Text).SetAlignment(TextAnchor.UpperLeft).AppendTo<Txt>(this.ItemsContainer, new float?(60f), Offset.Left(20f));
      this.m_shipOrdersTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.WorldLocation_Orders);
      this.m_exploreBtn = this.Builder.NewBtnPrimary("Explore").PlayErrorSoundWhenDisabled().SetText("Explore this location").OnClick((Action) (() => this.m_onGoToClick(this.m_location)));
      this.m_exploreBtn.AppendTo<Btn>(this.ItemsContainer, new Vector2?(this.m_exploreBtn.GetOptimalSize() + new Vector2(0.0f, 8f)), ContainerPosition.MiddleOrCenter, Offset.Top(5f));
      Tooltip visitToolTip = this.Builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this.m_exploreBtn);
      this.m_arrivalInfo = this.Builder.NewTxt("ArrivalInfo").SetTextStyle(this.Builder.Style.Panel.Text).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.ItemsContainer, new float?(20f));
      updaterBuilder.Observe<WorldMapLocationState>((Func<WorldMapLocationState>) (() => this.m_location.State)).Observe<bool>((Func<bool>) (() => this.m_location.IsEnemyKnown && this.m_location.Enemy.HasValue)).Observe<bool>((Func<bool>) (() =>
      {
        if (this.m_fleetManager.TravelingFleet.LocationState != FleetLocationState.ExploreInProgress)
          return false;
        WorldMapLocId? currentLocationId = this.m_fleetManager.TravelingFleet.CurrentLocationId;
        WorldMapLocId id = this.m_location.Id;
        return currentLocationId.HasValue && currentLocationId.GetValueOrDefault() == id;
      })).Observe<bool>((Func<bool>) (() => this.m_fleetManager.TravelingFleet.Path.IsNotEmpty<WorldMapLocId>() && this.m_fleetManager.TravelingFleet.Path.Last<WorldMapLocId>() == this.m_location.Id)).Do((Action<WorldMapLocationState, bool, bool, bool>) ((state, isEnemyKnown, isBeingExplored, isCurrentShipGoal) =>
      {
        if (this.m_location == this.m_worldMapManager.Map.HomeLocation)
        {
          this.m_infoText.SetText((LocStrFormatted) Tr.WorldLocation__Home_Desc);
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__Home_Title);
        }
        else if (state == WorldMapLocationState.NotExplored & isCurrentShipGoal)
        {
          this.m_infoText.SetText((LocStrFormatted) Tr.WorldLocation__UnknownOnWay_Desc);
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__Unknown_Title);
        }
        else if (isEnemyKnown & isCurrentShipGoal)
        {
          this.m_infoText.SetText((LocStrFormatted) Tr.WorldLocation__WithEnemyOnWay_Desc);
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__WithEnemy_Title);
        }
        else if (isBeingExplored)
        {
          this.m_infoText.SetText((LocStrFormatted) Tr.WorldLocation__BeingExplored_Title);
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__BeingExplored_Title);
        }
        else if (isEnemyKnown)
        {
          this.m_infoText.SetText((LocStrFormatted) Tr.WorldLocation__WithEnemy_Desc);
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__WithEnemy_Title);
        }
        else if (state == WorldMapLocationState.NotExplored)
        {
          this.m_infoText.SetText((LocStrFormatted) Tr.WorldLocation__Unknown_Desc);
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__Unknown_Title);
        }
        else if (state == WorldMapLocationState.Explored)
        {
          this.m_infoText.SetText((LocStrFormatted) Tr.WorldLocation__Explored_Desc);
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__Explored_Title);
        }
        else
        {
          this.m_infoText.SetText("");
          this.SetTitle((LocStrFormatted) Tr.WorldLocation__Unknown_Title);
        }
      }));
      updaterBuilder.Observe<Option<BattleFleet>>((Func<Option<BattleFleet>>) (() => this.m_location.Enemy)).Observe<bool>((Func<bool>) (() => this.m_location.IsEnemyKnown)).Do((Action<Option<BattleFleet>, bool>) ((enemyFleet, isEnemyKnown) =>
      {
        bool isVisible = enemyFleet.HasValue & isEnemyKnown;
        if (isVisible)
          this.m_enemyInfo.SetText(EnemyInfoPrinter.GetEnemyInfo(enemyFleet.Value));
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_enemyInfoTitle, isVisible);
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_enemyInfo, isVisible);
      }));
      updaterBuilder.Observe<Option<WorldMapEntityProto>>((Func<Option<WorldMapEntityProto>>) (() => this.m_location.EntityProto)).Observe<bool>((Func<bool>) (() => this.m_location.IsScannedByRadar)).Do((Action<Option<WorldMapEntityProto>, bool>) ((entity, isEntityKnown) =>
      {
        bool isVisible = entity.HasValue & isEntityKnown;
        if (isVisible)
          this.m_entityInfo.SetText(Tr.Location_HasEntity.Format(entity.Value.Strings.Name));
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_entityInfoTitle, isVisible);
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_entityInfo, isVisible);
      }));
      updaterBuilder.Observe<int>((Func<int>) (() =>
      {
        int travelDistanceBetween = this.m_fleetManager.ComputeTravelDistanceBetween(this.m_worldMapManager.Map.HomeLocation.Id, this.m_location.Id);
        if (travelDistanceBetween == 0)
          travelDistanceBetween = this.m_fleetManager.ComputeTravelDistanceBetween(this.m_worldMapManager.Map.HomeLocation.Id, this.m_location.Id, false);
        return travelDistanceBetween;
      })).Do((Action<int>) (distance =>
      {
        this.m_distanceText.SetText(Tr.Location_Distance.Format(distance.ToString()));
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_distanceText, this.m_location != this.m_worldMapManager.Map.HomeLocation);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() =>
      {
        TravelingFleet travelingFleet = this.m_fleetManager.TravelingFleet;
        WorldMapLocId id = this.m_location.Id;
        WorldMapLocId? currentLocationId = travelingFleet.CurrentLocationId;
        return (currentLocationId.HasValue ? (id != currentLocationId.GetValueOrDefault() ? 1 : 0) : 1) != 0 && !travelingFleet.Path.Contains<WorldMapLocId>(this.m_location.Id);
      })).Observe<Option<Tuple<int, RelGameDate>>>((Func<Option<Tuple<int, RelGameDate>>>) (() =>
      {
        if (!this.m_fleetManager.TravelingFleet.Path.IsNotEmpty<WorldMapLocId>() || !(this.m_fleetManager.TravelingFleet.Path.Last<WorldMapLocId>() == this.m_location.Id))
          return (Option<Tuple<int, RelGameDate>>) Option.None;
        int distance;
        RelGameDate duration;
        this.m_fleetManager.ComputeToGoalPathAndCosts(this.m_location.Id, out distance, out duration, out Quantity _);
        return Option.Some<Tuple<int, RelGameDate>>(Tuple.Create<int, RelGameDate>(distance, duration));
      })).Do((Action<bool, Option<Tuple<int, RelGameDate>>>) ((canVisit, arrivalInfo) =>
      {
        if (arrivalInfo.HasValue)
        {
          int num = arrivalInfo.Value.Item1;
          RelGameDate relGameDate = arrivalInfo.Value.Item2;
          this.m_arrivalInfo.SetText((LocStrFormatted) Tr.Location_ShipOnWay);
        }
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_arrivalInfo, arrivalInfo != (Tuple<int, RelGameDate>) null);
        this.ItemsContainer.SetItemVisibility((IUiElement) this.m_shipOrdersTitle, this.m_arrivalInfo.IsVisible() || this.m_exploreBtn.IsVisible());
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_location == this.m_worldMapManager.Map.HomeLocation)).Observe<bool>((Func<bool>) (() => this.m_location.IsEnemyKnown && this.m_location.Enemy.HasValue)).Observe<WorldMapLocationState>((Func<WorldMapLocationState>) (() => this.m_location.State)).Do((Action<bool, bool, WorldMapLocationState>) ((isHome, isEnemyKnown, state) =>
      {
        if (isHome)
          this.m_exploreBtn.SetText((LocStrFormatted) Tr.WorldLocation_Orders__GoHome);
        else if (isEnemyKnown)
          this.m_exploreBtn.SetText((LocStrFormatted) Tr.WorldLocation_Orders__Battle);
        else if (state == WorldMapLocationState.Explored)
          this.m_exploreBtn.SetText((LocStrFormatted) Tr.WorldLocation_Orders__Visit);
        else
          this.m_exploreBtn.SetText((LocStrFormatted) TrCore.WorldLocation_Orders__Explore);
      }));
      updaterBuilder.Observe<TravelingFleetManager.LocationVisitCheckResult>((Func<TravelingFleetManager.LocationVisitCheckResult>) (() => this.m_fleetManager.CanRequestLocationVisit(this.m_location))).Do((Action<TravelingFleetManager.LocationVisitCheckResult>) (visitResult =>
      {
        visitToolTip.SetText((LocStrFormatted) LocationVisitCheckResultHelper.MapToToolTip(visitResult));
        this.m_exploreBtn.SetEnabled(visitResult == TravelingFleetManager.LocationVisitCheckResult.Ok);
      }));
      this.AddUpdater(updaterBuilder.Build());
    }

    public void SetLocation(WorldMapLocation location) => this.m_location = location;
  }
}
