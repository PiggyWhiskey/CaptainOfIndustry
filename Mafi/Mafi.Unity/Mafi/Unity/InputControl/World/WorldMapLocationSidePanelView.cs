// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldMapLocationSidePanelView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class WorldMapLocationSidePanelView : View
  {
    private StackContainer m_container;
    private readonly WorldMapManager m_worldMapManager;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly Action m_onExploreFinishCheatClick;
    private Txt m_distanceText;
    private Txt m_fleetCargoText;

    public WorldMapLocationSidePanelView(
      WorldMapManager worldMapManager,
      TravelingFleetManager fleetManager,
      Action onExploreFinishCheatClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (WorldMapLocationSidePanelView));
      this.m_worldMapManager = worldMapManager;
      this.m_fleetManager = fleetManager;
      this.m_onExploreFinishCheatClick = onExploreFinishCheatClick;
    }

    protected override void BuildUi()
    {
      TextStyle textStyle = new TextStyle((ColorRgba) 14935011);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_container = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetBackground(new ColorRgba(3684408)).SetItemSpacing(10f).PutToMiddleOf<StackContainer>((IUiElement) this, 0.0f);
      this.m_container.SizeChanged += (Action<IUiElement>) (element => this.SetHeight<WorldMapLocationSidePanelView>(this.m_container.GetDynamicHeight()));
      Panel parent = this.Builder.NewPanel("TitleBar").SetBorderStyle(new BorderStyle(ColorRgba.Black)).AppendTo<Panel>(this.m_container, new Vector2?(new Vector2(300f, 30f)), ContainerPosition.MiddleOrCenter);
      this.Builder.NewTxt("Fleet").SetTextStyle(new TextStyle((ColorRgba) 14935011, fontStyle: new FontStyle?(FontStyle.Bold), isCapitalized: true)).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) parent);
      Txt fleetTargetText = this.Builder.NewTxt("fleet tgt").SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_container, new float?(20f));
      updaterBuilder.Observe<WorldMapLocId?>((Func<WorldMapLocId?>) (() => this.m_fleetManager.TravelingFleet.CurrentLocationId)).Observe<WorldMapLocId>((Func<WorldMapLocId>) (() => this.m_fleetManager.TravelingFleet.NextLocationId)).Do((Action<WorldMapLocId?, WorldMapLocId>) ((curLoc, nextLoc) => fleetTargetText.SetText(curLoc.HasValue ? "At: " + this.m_worldMapManager.Map[curLoc.Value].ValueOrNull?.Name : "Traveling to: " + this.m_worldMapManager.Map[nextLoc].ValueOrNull?.Name)));
      Txt crewCount = this.Builder.NewTxt("crew count").SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_container, new float?(20f));
      updaterBuilder.Observe<int>((Func<int>) (() => this.m_fleetManager.TravelingFleet.CurrentCrew)).Do((Action<int>) (crew => crewCount.SetText(string.Format("Crew: {0}", (object) crew))));
      Txt fleetFuelLeftText = this.Builder.NewTxt("fleet fuel left").SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_container, new float?(20f));
      updaterBuilder.Observe<int>((Func<int>) (() => this.m_fleetManager.TravelingFleet.GetFuelRemainingDistance())).Do((Action<int>) (remDist => fleetFuelLeftText.SetText(string.Format("Fuel reserves: {0} km", (object) remDist))));
      this.m_fleetCargoText = this.Builder.NewTxt("cargo").SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_container, new float?(20f));
      this.m_distanceText = this.Builder.NewTxt("dist dur").SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_container, new float?(20f));
      updaterBuilder.Observe<Option<Tuple<int, RelGameDate>>>((Func<Option<Tuple<int, RelGameDate>>>) (() =>
      {
        if (!this.m_fleetManager.TravelingFleet.Path.IsNotEmpty<WorldMapLocId>())
          return (Option<Tuple<int, RelGameDate>>) Option.None;
        int distance;
        RelGameDate duration;
        this.m_fleetManager.ComputeToGoalPathAndCosts(this.m_fleetManager.TravelingFleet.Path.Last<WorldMapLocId>(), out distance, out duration, out Quantity _);
        return Option.Some<Tuple<int, RelGameDate>>(Tuple.Create<int, RelGameDate>(distance, duration));
      })).Do((Action<Option<Tuple<int, RelGameDate>>>) (arrivalInfo =>
      {
        if (arrivalInfo.HasValue)
          this.m_distanceText.SetText(string.Format("Distance: {0} km, ETA: {1} days", (object) arrivalInfo.Value.Item1, (object) arrivalInfo.Value.Item2));
        this.m_container.SetItemVisibility((IUiElement) this.m_distanceText, arrivalInfo.HasValue);
      }));
      updaterBuilder.Observe<IProductBuffer>((Func<IEnumerable<IProductBuffer>>) (() => this.m_fleetManager.TravelingFleet.Cargo.Values), (ICollectionComparator<IProductBuffer, IEnumerable<IProductBuffer>>) AlwaysRefresh<IProductBuffer>.Instance).Observe<int>((Func<int>) (() => this.m_fleetManager.TravelingFleet.RefugeesCount)).Observe<int>((Func<int>) (() => this.m_fleetManager.TravelingFleet.GetFreeCapacity())).Do((Action<Lyst<IProductBuffer>, int, int>) ((cargo, refugeesCount, freeCapacity) =>
      {
        string text = string.Format("Cargo: pop: {0}", (object) refugeesCount);
        foreach (IProductBuffer productBuffer in cargo)
          text += string.Format(", {0}: {1}", (object) productBuffer.Product.Strings.Name, (object) productBuffer.Quantity.Value);
        this.m_fleetCargoText.SetText(text);
      }));
      this.AddUpdater(updaterBuilder.Build());
    }

    public float GetWidth() => this.RectTransform.rect.width.Max(300f);

    public float GetHeight() => this.m_container.GetDynamicHeight();
  }
}
