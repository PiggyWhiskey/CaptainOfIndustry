// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldMapView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Prototypes;
using Mafi.Core.SaveGame;
using Mafi.Core.World;
using Mafi.Core.World.Contracts;
using Mafi.Core.World.Entities;
using Mafi.Unity.InputControl.Fleet.Battle;
using Mafi.Unity.InputControl.Inspectors;
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
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class WorldMapView : WindowView
  {
    public const float WHEEL_ZOOM_SCALE = 1f;
    public const float DECELERATION_RATE = 0.0f;
    public const float ZOOM_SMOOTH_TIME = 0.1f;
    private readonly WorldMapManager m_mapManager;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly SaveManager m_saveManager;
    private readonly TradeWindowController m_tradeWindowController;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly ProtosDb m_protosDb;
    private readonly LazyResolve<WorldMapController> m_controller;
    private readonly UiBuilder m_builder;
    private readonly ExplorationResultView m_explorationView;
    private FleetBattleProgressView m_battleProgressView;
    private Panel m_mapContainer;
    private ScrollableContainer m_scrollableContainer;
    private WorldMapUi m_mapUi;
    private float m_targetScale;
    private float m_zoomVelocity;
    private Vector2 m_zoomPoint;
    private readonly float m_minZoom;
    private readonly float m_maxZoom;
    private Panel m_sideViewContainer;
    private Panel m_container;
    private readonly WorldMapLocationSidePanelView m_locationSidePanel;
    private readonly WorldMineView m_mineViewInspector;
    private readonly WorldCargoShipWreckView m_cargoShipWreckViewInspector;
    private readonly WorldGeneralLocationView m_generalLocationViewInspector;
    private readonly WorldVillageView m_villageInspector;
    private readonly Lyst<Action> m_delayedEvents;

    public ExplorationResultView ExplorationView
    {
      get
      {
        this.BuildIfNeeded(this.m_builder);
        return this.m_explorationView;
      }
    }

    public FleetBattleProgressView BattleProgressView
    {
      get
      {
        this.BuildIfNeeded(this.m_builder);
        return this.m_battleProgressView;
      }
    }

    public Option<WorldMapLocation> SelectedLocation { get; private set; }

    internal WorldMapView(
      WorldMapManager mapManager,
      TravelingFleetManager fleetManager,
      SaveManager saveManager,
      InspectorContext inspectorContext,
      ContractsManager contractsManager,
      TradeWindowController tradeWindowController,
      SourceProductsAnalyzer sourceProductsAnalyzer,
      ShortcutsManager shortcutsManager,
      ProtosDb protosDb,
      LazyResolve<WorldMapController> controller,
      UiBuilder builder,
      NewInstanceOf<ExplorationResultView> explorationView)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_targetScale = 1f;
      this.m_minZoom = 0.5f;
      this.m_maxZoom = 1.5f;
      this.m_delayedEvents = new Lyst<Action>();
      // ISSUE: explicit constructor call
      base.\u002Ector("WorldMap", WindowView.FooterStyle.None, true);
      WorldMapView worldMapView = this;
      this.m_mapManager = mapManager;
      this.m_fleetManager = fleetManager;
      this.m_saveManager = saveManager;
      this.m_tradeWindowController = tradeWindowController;
      this.m_shortcutsManager = shortcutsManager;
      this.m_protosDb = protosDb;
      this.m_controller = controller;
      this.m_builder = builder;
      this.m_explorationView = explorationView.Instance;
      this.m_explorationView.SetOnCloseButtonClickAction((Action) (() => worldMapView.m_explorationView.Hide()));
      this.m_mineViewInspector = new WorldMineView(inspectorContext, this.m_fleetManager, new Action(onInspectorClose), new Action<WorldMapLocation, LocationVisitReason>(onGoToClickWithReason));
      this.m_cargoShipWreckViewInspector = new WorldCargoShipWreckView(inspectorContext.InputScheduler, this.m_fleetManager, new Action(onInspectorClose), new Action<WorldMapLocation, LocationVisitReason>(onGoToClickWithReason));
      this.m_villageInspector = new WorldVillageView(inspectorContext, contractsManager, sourceProductsAnalyzer, this.m_fleetManager, this.m_tradeWindowController, new Action(onInspectorClose), new Action<WorldMapLocation, LocationVisitReason>(onGoToClickWithReason));
      this.m_generalLocationViewInspector = new WorldGeneralLocationView(this.m_mapManager, this.m_fleetManager, new Action(onInspectorClose), new Action<WorldMapLocation>(onGoToClick));
      this.m_locationSidePanel = new WorldMapLocationSidePanelView(mapManager, fleetManager, (Action) (() => inspectorContext.InputScheduler.ScheduleInputCmd<ExploreFinishCheatCmd>(new ExploreFinishCheatCmd())));
      this.m_mapManager.LocationExplored += (Action<WorldMapLocation>) (location => worldMapView.m_delayedEvents.Add((Action) (() =>
      {
        if (!(location == worldMapView.SelectedLocation))
          return;
        worldMapView.unSelectLocation();
      })));
      this.m_mapManager.LocationRemoved += (Action<WorldMapLocation>) (location => worldMapView.m_delayedEvents.Add((Action) (() => worldMapView.unSelectLocation())));
      this.ShowAfterSync = true;

      void onInspectorClose() => worldMapView.unSelectLocation();

      void onGoToClick(WorldMapLocation loc)
      {
        inspectorContext.InputScheduler.ScheduleInputCmd<GoToLocationCmd>(new GoToLocationCmd(loc.Id, LocationVisitReason.General));
      }

      void onGoToClickWithReason(WorldMapLocation loc, LocationVisitReason reason)
      {
        inspectorContext.InputScheduler.ScheduleInputCmd<GoToLocationCmd>(new GoToLocationCmd(loc.Id, reason));
      }
    }

    public void PositionWindowView(IUiElement windowView)
    {
      windowView.PutToCenterMiddleOf<IUiElement>((IUiElement) this.GetContentPanel(), windowView.GetSize());
    }

    public void PositionFullscreen(IUiElement windowView)
    {
      windowView.PutTo<IUiElement>((IUiElement) this.GetContentPanel());
    }

    public void ToggleEntireMapVisibility()
    {
      this.m_mapUi.SetEntireMapVisibility(!this.m_mapUi.EntireMapVisible);
    }

    protected override void BuildWindowContent()
    {
      this.PositionSelfToFullscreen();
      this.m_container = this.Builder.NewPanel("Container").SetBackground(new ColorRgba(4408131)).OnRightClick((Action) (() => this.unSelectLocation())).PutTo<Panel>((IUiElement) this.GetContentPanel());
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("ScrollableContainer").DisableScrollByMouseWheel().SetDecelerationRate(0.0f).SetOnDragAction((Action) (() => this.unSelectLocation())).PutTo<ScrollableContainer>((IUiElement) this.m_container);
      this.m_mapContainer = this.Builder.NewPanel("WorldMapContainer").SetBackground(this.Builder.AssetsDb.GetSharedSprite("Assets/Unity/UserInterface/WorldMap/OceanBg.jpg"), isTiled: true);
      this.m_scrollableContainer.AddItem((IUiElement) this.m_mapContainer);
      Assert.That<Vector2i>(this.m_mapManager.Map.Size).IsNotZero();
      this.m_mapContainer.SetSize<Panel>(this.m_mapManager.Map.Size.ToVector2());
      this.m_mapUi = new WorldMapUi((IUiElement) this.m_mapContainer, this.Builder, "WorldMap2Ui", this.m_mapManager, this.m_fleetManager, new Action<WorldMapLocation>(this.selectLocation)).PutTo<WorldMapUi>((IUiElement) this.m_mapContainer).Initialize();
      this.m_mineViewInspector.BuildUi(this.Builder, (IUiElement) this.m_mapUi);
      this.m_cargoShipWreckViewInspector.BuildUi(this.Builder, (IUiElement) this.m_mapUi);
      this.m_villageInspector.BuildUi(this.Builder, (IUiElement) this.m_mapUi);
      this.m_generalLocationViewInspector.BuildUi(this.Builder, (IUiElement) this.m_mapUi);
      Vector2 homeIslandPosition = this.m_mapUi.HomeIslandPosition;
      this.m_mapContainer.RectTransform.localPosition = new Vector3(-homeIslandPosition.x, (float) this.m_mapManager.Map.Size.Y - homeIslandPosition.y);
      this.m_sideViewContainer = this.Builder.NewPanel("Side").SetBackground(new ColorRgba(4210752)).PutToRightOf<Panel>((IUiElement) this.m_container, 0.0f);
      this.Builder.NewIconContainer("LeftGradient").SetIcon(new IconStyle(this.Style.Icons.GradientToLeft, new ColorRgba?((ColorRgba) 3355443), preserveAspect: false)).PutToLeftOf<IconContainer>((IUiElement) this.m_sideViewContainer, 10f, Offset.Left(-9f));
      this.Builder.NewPanel("LeftBorder").SetBackground(new ColorRgba(2696228)).PutToLeftOf<Panel>((IUiElement) this.m_sideViewContainer, 1f, Offset.Left(-1f));
      this.m_locationSidePanel.BuildUi(this.Builder);
      this.m_locationSidePanel.Show();
      this.setSidePanelWidth(this.m_locationSidePanel.GetWidth());
      this.m_locationSidePanel.PutToMiddleOf<WorldMapLocationSidePanelView>((IUiElement) this.m_sideViewContainer, this.m_locationSidePanel.GetHeight());
      Btn btn = this.Builder.NewReturnBtn().OnClick((Action) (() =>
      {
        Action valueOrNull = this.OnCloseButtonClick.ValueOrNull;
        if (valueOrNull == null)
          return;
        valueOrNull();
      }));
      btn.PutToLeftBottomOf<Btn>((IUiElement) this.m_container, btn.GetSize(), Offset.BottomLeft(20f, 20f));
      this.m_explorationView.BuildUi(this.Builder);
      this.PositionWindowView((IUiElement) this.m_explorationView);
      this.m_battleProgressView = new FleetBattleProgressView(this.Builder, this.m_fleetManager, (Func<bool>) (() => !this.m_controller.Value.IsActive));
      this.m_battleProgressView.SetOnResultAccepted((Action) (() => this.m_battleProgressView.Hide<FleetBattleProgressView>()));
      this.m_battleProgressView.Hide<FleetBattleProgressView>();
      this.PositionFullscreen((IUiElement) this.m_battleProgressView);
    }

    private void setSidePanelWidth(float width)
    {
      this.m_scrollableContainer.PutTo<ScrollableContainer>((IUiElement) this.m_container, Offset.Right((double) width == 0.0 ? 0.0f : width - 1f));
      this.m_sideViewContainer.SetWidth<Panel>(width);
    }

    public bool InputUpdate()
    {
      if (this.m_explorationView.IsVisible && this.m_explorationView.InputUpdate() || Input.GetKeyDown(this.m_shortcutsManager.Cancel) && this.unSelectLocation() || this.m_scrollableContainer.UpdateKeyboardPan(this.m_shortcutsManager))
        return true;
      float num = 1f * Input.GetAxis("MouseScroll");
      if (num.IsNearZero())
        return this.m_mapUi.InputUpdate();
      Vector3 mousePosition = Input.mousePosition;
      if (RectTransformUtility.RectangleContainsScreenPoint(this.m_scrollableContainer.RectTransform, (Vector2) mousePosition))
      {
        this.m_zoomPoint = (Vector2) this.m_mapContainer.RectTransform.InverseTransformPoint(mousePosition);
        this.m_targetScale = (this.m_targetScale + num).Clamp(this.m_minZoom, this.m_maxZoom);
      }
      if ((double) (this.m_mapContainer.RectTransform.localScale.x - this.m_targetScale).Abs() > 0.090000003576278687)
        this.unSelectLocation();
      return true;
    }

    private void selectLocation(WorldMapLocation location)
    {
      this.SelectedLocation = (Option<WorldMapLocation>) location;
      this.hideInspectors();
      if (location.Entity.HasValue && location.Enemy.IsNone)
      {
        if (location.Entity.Value is WorldMapMine entity1)
        {
          this.m_mineViewInspector.SetEntity(entity1);
          this.positionInspector((IUiElement) this.m_mineViewInspector, location);
          this.m_mineViewInspector.Show();
        }
        else if (location.Entity.Value is WorldMapVillage village)
        {
          this.m_villageInspector.SetEntity(village);
          this.positionInspector((IUiElement) this.m_villageInspector, location);
          this.m_villageInspector.Show();
        }
        else
        {
          if (!(location.Entity.Value is WorldMapCargoShipWreck entity))
            return;
          this.m_cargoShipWreckViewInspector.SetEntity(entity);
          this.positionInspector((IUiElement) this.m_cargoShipWreckViewInspector, location);
          this.m_cargoShipWreckViewInspector.Show();
        }
      }
      else
      {
        this.m_generalLocationViewInspector.SetLocation(location);
        this.positionInspector((IUiElement) this.m_generalLocationViewInspector, location);
        this.m_generalLocationViewInspector.Show();
      }
    }

    private void positionInspector(IUiElement inspector, WorldMapLocation location)
    {
      this.m_mapUi.PositionInspectorFor(inspector, location);
      inspector.SetParent<IUiElement>((IUiElement) this.m_container);
      inspector.RectTransform.localScale = Vector3.one;
    }

    private bool unSelectLocation()
    {
      if (this.SelectedLocation.IsNone)
        return false;
      this.hideInspectors();
      this.SelectedLocation = (Option<WorldMapLocation>) Option.None;
      this.m_mapUi.UnselectAnyLocation();
      return true;
    }

    private void hideInspectors()
    {
      this.m_mineViewInspector.Hide();
      this.m_cargoShipWreckViewInspector.Hide();
      this.m_generalLocationViewInspector.Hide();
      this.m_villageInspector.Hide();
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      if (this.m_explorationView.IsVisible)
        this.m_explorationView.RenderUpdate(gameTime);
      if (this.m_battleProgressView.IsVisible())
        this.m_battleProgressView.RenderUpdate(gameTime);
      if (this.m_mineViewInspector.IsVisible)
        this.m_mineViewInspector.RenderUpdate(gameTime);
      if (this.m_cargoShipWreckViewInspector.IsVisible)
        this.m_cargoShipWreckViewInspector.RenderUpdate(gameTime);
      if (this.m_villageInspector.IsVisible)
        this.m_villageInspector.RenderUpdate(gameTime);
      if (this.m_generalLocationViewInspector.IsVisible)
        this.m_generalLocationViewInspector.RenderUpdate(gameTime);
      float x = this.m_mapContainer.RectTransform.localScale.x;
      float num = Mathf.SmoothDamp(x, this.m_targetScale, ref this.m_zoomVelocity, 0.1f);
      this.m_mapContainer.RectTransform.localScale = new Vector3(num, num, 1f);
      Vector2 vector2_1 = this.m_zoomPoint * x;
      Vector2 vector2_2 = this.m_zoomPoint * num - vector2_1;
      RectTransform rectTransform = this.m_mapContainer.RectTransform;
      rectTransform.localPosition = rectTransform.localPosition - new Vector3(vector2_2.x, vector2_2.y, 0.0f);
      this.m_scrollableContainer.FixScroll();
      this.m_locationSidePanel.RenderUpdate(gameTime);
      this.m_mapUi.Updater.RenderUpdate();
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      foreach (Action delayedEvent in this.m_delayedEvents)
        delayedEvent();
      this.m_delayedEvents.Clear();
      if (this.m_explorationView.IsVisible)
        this.m_explorationView.SyncUpdate(gameTime);
      if (this.m_battleProgressView.IsVisible())
        this.m_battleProgressView.SyncUpdate(gameTime);
      if (this.m_mineViewInspector.IsVisible)
        this.m_mineViewInspector.SyncUpdate(gameTime);
      if (this.m_cargoShipWreckViewInspector.IsVisible)
        this.m_cargoShipWreckViewInspector.SyncUpdate(gameTime);
      if (this.m_villageInspector.IsVisible)
        this.m_villageInspector.SyncUpdate(gameTime);
      if (this.m_generalLocationViewInspector.IsVisible)
        this.m_generalLocationViewInspector.SyncUpdate(gameTime);
      this.m_locationSidePanel.SyncUpdate(gameTime);
      this.m_mapUi.Updater.SyncUpdate();
      this.m_mapUi.SyncUpdate();
    }
  }
}
