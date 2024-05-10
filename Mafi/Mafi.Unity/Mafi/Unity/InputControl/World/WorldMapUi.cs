// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldMapUi
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class WorldMapUi : IUiElement
  {
    private static readonly ColorRgba CONNECTION_COLOR;
    private static readonly ColorRgba DOT_INNER_COLOR;
    private readonly Action<WorldMapLocation> m_onLocationSelected;
    private readonly GameObject m_iconsGo;
    private readonly GameObject m_islandsGo;
    private readonly UiBuilder m_builder;
    private readonly WorldMapManager m_mapManager;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly IconContainer m_fleetIcon;
    private readonly Dict<WorldMapLocation, WorldMapUi.LocationIcon> m_locations;
    private readonly Dict<WorldMapConnection, UiLine> m_connections;
    private readonly UiLinesRenderer m_connectionsRenderer;
    private readonly Lyst<Action> m_delayedEvents;
    public readonly IUiUpdater Updater;
    private Option<WorldMapUi.LocationIcon> m_selectedLocation;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public bool EntireMapVisible { get; private set; }

    public WorldMapUi(
      IUiElement parent,
      UiBuilder builder,
      string name,
      WorldMapManager mapManager,
      TravelingFleetManager fleetManager,
      Action<WorldMapLocation> onLocationSelected)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_locations = new Dict<WorldMapLocation, WorldMapUi.LocationIcon>();
      this.m_connections = new Dict<WorldMapConnection, UiLine>();
      this.m_delayedEvents = new Lyst<Action>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onLocationSelected = onLocationSelected;
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.m_mapManager = mapManager;
      this.m_fleetManager = fleetManager;
      this.GameObject = new GameObject(name);
      this.RectTransform = this.GameObject.AddComponent<RectTransform>();
      this.GameObject.AddComponent<CanvasRenderer>();
      this.GameObject.transform.SetParent(parent.GameObject.transform, false);
      this.m_islandsGo = new GameObject("islands");
      this.m_islandsGo.AddComponent<RectTransform>();
      this.m_islandsGo.PutTo(this.GameObject);
      GameObject objectToPlace = new GameObject("connections");
      objectToPlace.AddComponent<RectTransform>();
      this.m_connectionsRenderer = objectToPlace.AddComponent<UiLinesRenderer>();
      objectToPlace.PutToLeftBottomOf(this.GameObject, Vector2.zero);
      this.m_iconsGo = new GameObject("icons");
      this.m_iconsGo.AddComponent<RectTransform>();
      this.m_iconsGo.PutTo(this.GameObject);
      this.m_fleetIcon = this.m_builder.NewIconContainer("fleet-icon").SetIcon("Assets/Unity/UserInterface/WorldMap/Battleship-256.png", ColorRgba.White).PutToLeftBottomOf<IconContainer>(this.GameObject, 48.Vector2(), Offset.BottomLeft(0.0f, 0.0f));
      this.Updater = UpdaterBuilder.Start().Build();
    }

    public void SyncUpdate()
    {
      foreach (Action delayedEvent in this.m_delayedEvents)
        delayedEvent();
      this.m_delayedEvents.Clear();
      Vector2f worldPosition = this.m_fleetManager.TravelingFleet.WorldPosition;
      Fix32 fix32 = 48.Over(2);
      this.m_fleetIcon.RectTransform.anchoredPosition = new Vector2f(worldPosition.X - fix32, worldPosition.Y - fix32).ToVector2();
    }

    public Vector2 HomeIslandPosition { get; private set; }

    public WorldMapUi Initialize()
    {
      this.reshowMap();
      this.m_mapManager.LocationAdded += (Action<WorldMapLocation>) (x => this.m_delayedEvents.Add((Action) (() => this.addLocationIfVisible(x))));
      this.m_mapManager.LocationRemoved += (Action<WorldMapLocation>) (x => this.m_delayedEvents.Add((Action) (() => this.tryRemoveLocation(x))));
      this.m_mapManager.LocationChanged += (Action<WorldMapLocation>) (x => this.m_delayedEvents.Add((Action) (() => this.reshowLocation(x))));
      this.m_mapManager.ConnectionAdded += (Action<WorldMapConnection>) (x => this.m_delayedEvents.Add((Action) (() => this.addConnectionIfVisible(x))));
      this.m_mapManager.ConnectionRemoved += (Action<WorldMapConnection>) (x => this.m_delayedEvents.Add((Action) (() => this.tryRemoveConnection(x))));
      this.m_mapManager.MapReplaced += (Action<Mafi.Core.World.WorldMap>) (x => this.m_delayedEvents.Add(new Action(this.reshowMap)));
      return this;
    }

    public void PositionInspectorFor(IUiElement inspector, WorldMapLocation location)
    {
      inspector.PutToLeftBottomOf<IUiElement>((IUiElement) this, inspector.GetSize(), Offset.BottomLeft((float) (location.Position.Y + 40), (float) (location.Position.X + 80)));
    }

    public void SetEntireMapVisibility(bool visible)
    {
      if (this.EntireMapVisible == visible)
        return;
      this.EntireMapVisible = visible;
      this.reshowMap();
    }

    private void reshowMap()
    {
      foreach (KeyValuePair<WorldMapLocation, WorldMapUi.LocationIcon> location in this.m_locations)
        location.Value.Destroy();
      this.m_locations.Clear();
      this.Updater.ClearAllChildUpdaters();
      foreach (KeyValuePair<WorldMapConnection, UiLine> connection in this.m_connections)
        connection.Value.Remove();
      this.m_connections.Clear();
      this.HomeIslandPosition = this.m_mapManager.Map.HomeLocation.Position.ToVector2();
      foreach (WorldMapLocation location in (IEnumerable<WorldMapLocation>) this.m_mapManager.Map.Locations)
        this.addLocationIfVisible(location);
      foreach (WorldMapConnection connection in (IEnumerable<WorldMapConnection>) this.m_mapManager.Map.Connections)
        this.addConnectionIfVisible(connection);
      if (!this.m_selectedLocation.HasValue)
        return;
      WorldMapUi.LocationIcon location1 = this.m_locations[this.m_selectedLocation.Value.Location];
      location1.SetIsSelected(true);
      this.m_selectedLocation = (Option<WorldMapUi.LocationIcon>) location1;
    }

    private void addLocationIfVisible(WorldMapLocation location)
    {
      Assert.That<Dict<WorldMapLocation, WorldMapUi.LocationIcon>>(this.m_locations).NotContainsKey<WorldMapLocation, WorldMapUi.LocationIcon>(location);
      if (!this.EntireMapVisible && location.State == WorldMapLocationState.Hidden)
        return;
      WorldMapUi.LocationIcon locationIcon = new WorldMapUi.LocationIcon(location, this.m_builder, this.m_islandsGo, this.m_iconsGo, this.m_mapManager, new Action<WorldMapUi.LocationIcon>(this.onLocationSelected));
      this.m_locations[location] = locationIcon;
      this.Updater.AddChildUpdater(locationIcon.Updater);
    }

    public void UnselectAnyLocation()
    {
      if (this.m_selectedLocation.HasValue)
        this.m_selectedLocation.Value.SetIsSelected(false);
      this.m_selectedLocation = (Option<WorldMapUi.LocationIcon>) Option.None;
    }

    private void onLocationSelected(WorldMapUi.LocationIcon location)
    {
      if (this.m_selectedLocation == location)
        return;
      if (this.m_selectedLocation.HasValue)
        this.m_selectedLocation.Value.SetIsSelected(false);
      location.SetIsSelected(true);
      this.m_selectedLocation = (Option<WorldMapUi.LocationIcon>) location;
      this.m_onLocationSelected(location.Location);
    }

    private void tryRemoveLocation(WorldMapLocation location)
    {
      WorldMapUi.LocationIcon locationIcon;
      if (!this.m_locations.TryGetValue(location, out locationIcon))
        return;
      locationIcon.Destroy();
      this.Updater.RemoveChildUpdater(locationIcon.Updater);
      this.m_locations.Remove(location);
    }

    private void reshowLocation(WorldMapLocation location)
    {
      this.tryRemoveLocation(location);
      this.addLocationIfVisible(location);
      if (this.m_selectedLocation.HasValue && this.m_selectedLocation.Value.Location == location)
      {
        WorldMapUi.LocationIcon location1 = this.m_locations[this.m_selectedLocation.Value.Location];
        location1.SetIsSelected(true);
        this.m_selectedLocation = (Option<WorldMapUi.LocationIcon>) location1;
      }
      foreach (WorldMapConnection conn in this.m_mapManager.Map.ConnectionsFor(location))
        this.reshowConnection(conn);
    }

    private void addConnectionIfVisible(WorldMapConnection conn)
    {
      Assert.That<Dict<WorldMapConnection, UiLine>>(this.m_connections).NotContainsKey<WorldMapConnection, UiLine>(conn);
      if (!this.EntireMapVisible && (conn.Location1.State == WorldMapLocationState.Hidden || conn.Location2.State == WorldMapLocationState.Hidden))
        return;
      UiLine uiLine = this.m_connectionsRenderer.AddLine(conn.Location1.Position.ToVector2(), conn.Location2.Position.ToVector2(), 2f, WorldMapUi.CONNECTION_COLOR.ToColor());
      this.m_connections.Add(conn, uiLine);
    }

    private void tryRemoveConnection(WorldMapConnection conn)
    {
      UiLine uiLine;
      if (!this.m_connections.TryGetValue(conn, out uiLine))
        return;
      uiLine.Remove();
      this.m_connections.Remove(conn);
    }

    private void reshowConnection(WorldMapConnection conn)
    {
      this.tryRemoveConnection(conn);
      this.addConnectionIfVisible(conn);
    }

    public bool InputUpdate() => false;

    static WorldMapUi()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WorldMapUi.CONNECTION_COLOR = new ColorRgba(11184810);
      WorldMapUi.DOT_INNER_COLOR = new ColorRgba(6776679);
    }

    private class LocationIcon : IUiElement
    {
      public readonly WorldMapLocation Location;
      private readonly UiBuilder m_builder;
      private readonly GameObject m_iconsParent;
      private readonly Btn m_btn;
      private readonly IconContainer m_icon;
      private readonly WorldMapManager m_mapManager;
      private readonly IconContainer m_innerIcon;
      private ColorRgba m_markerColor;
      private ColorRgba m_markerHoverColor;
      private bool m_isSelected;
      private bool m_isHovered;
      public readonly IUiUpdater Updater;
      private Btn m_dot;
      private IconContainer m_pauseIcon;
      private IconContainer m_notifBubble;
      private IconContainer m_islandGfx;

      public GameObject GameObject => this.m_btn.GameObject;

      public RectTransform RectTransform => this.m_btn.RectTransform;

      public LocationIcon(
        WorldMapLocation location,
        UiBuilder builder,
        GameObject islandsParent,
        GameObject iconsParent,
        WorldMapManager mapManager,
        Action<WorldMapUi.LocationIcon> onLocationSelected)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        WorldMapUi.LocationIcon locationIcon = this;
        this.Location = location;
        this.m_builder = builder;
        this.m_iconsParent = iconsParent;
        this.m_mapManager = mapManager;
        if (location.Graphics.HasValue)
        {
          WorldMapLocationGfxProto locationGfxProto = location.Graphics.Value;
          this.m_islandGfx = builder.NewIconContainer("island").SetIcon(locationGfxProto.IconPath).PutToLeftBottomOf<IconContainer>(islandsParent, locationGfxProto.Size.ToVector2(), Offset.BottomLeft((float) this.Location.Position.Y, (float) this.Location.Position.X));
        }
        IconContainer icon = builder.NewIconContainer("circle").SetIcon("Assets/Unity/UserInterface/WorldMap/Circle128.png", WorldMapUi.CONNECTION_COLOR);
        this.m_dot = builder.NewBtn("Dot").SetIcon(icon).OnClick((Action) (() => onLocationSelected(locationIcon))).PutToLeftBottomOf<Btn>(iconsParent, 24.Vector2(), Offset.BottomLeft((float) this.Location.Position.Y - 12f, (float) this.Location.Position.X - 12f));
        builder.NewIconContainer("innerCircle").SetIcon("Assets/Unity/UserInterface/WorldMap/Circle128.png", WorldMapUi.DOT_INNER_COLOR).PutTo<IconContainer>((IUiElement) this.m_dot, Offset.All(3f));
        this.m_icon = builder.NewIconContainer("icon").SetIcon("Assets/Unity/UserInterface/WorldMap/Marker-256.png", ColorRgba.Black);
        this.m_innerIcon = builder.NewIconContainer("innerIcon").PutToCenterTopOf<IconContainer>((IUiElement) this.m_icon, Vector2.zero, Offset.Top(12f));
        this.m_btn = builder.NewBtn(location.Position.ToString()).SetIcon(this.m_icon).OnClick((Action) (() => onLocationSelected(locationIcon))).SetOnMouseEnterLeaveActions(new Action(enterAction), new Action(leaveAction));
        this.m_notifBubble = builder.NewIconContainer("notifBubble").SetIcon("Assets/Unity/UserInterface/General/WhiteGrayBg32.png").Hide<IconContainer>();
        this.m_pauseIcon = builder.NewIconContainer("pausedIcon").SetIcon("Assets/Unity/UserInterface/General/EnableMachine128.png").SetColor((ColorRgba) 10752007).PutTo<IconContainer>((IUiElement) this.m_notifBubble, Offset.All(6f));
        this.m_dot.SetOnMouseEnterLeaveActions(new Action(enterAction), new Action(leaveAction));
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<WorldMapLocationState>((Func<WorldMapLocationState>) (() => locationIcon.Location.State)).Observe<bool>((Func<bool>) (() => locationIcon.Location.Enemy.HasValue)).Observe<bool>((Func<bool>) (() =>
        {
          if (location == locationIcon.m_mapManager.Map.HomeLocation)
            return true;
          return location.Entity.HasValue && location.Entity.Value.IsOwnedByPlayer;
        })).Observe<bool>((Func<bool>) (() => locationIcon.Location.IsEnemyKnown)).Observe<bool>((Func<bool>) (() => locationIcon.Location.Entity.HasValue)).Do((Action<WorldMapLocationState, bool, bool, bool, bool>) ((state, hasEnemy, isOwnedByPlayer, isEnemyKnown, hasEntity) =>
        {
          locationIcon.setLocationColor(state, isOwnedByPlayer, isEnemyKnown, hasEnemy, hasEntity);
          locationIcon.updateColor();
        }));
        updaterBuilder.Observe<WorldMapLocationState>((Func<WorldMapLocationState>) (() => locationIcon.Location.State)).Observe<Option<WorldMapEntityProto>>((Func<Option<WorldMapEntityProto>>) (() => locationIcon.Location.EntityProto)).Observe<bool>((Func<bool>) (() => locationIcon.Location.IsScannedByRadar)).Observe<bool>((Func<bool>) (() => locationIcon.Location.Enemy.HasValue && locationIcon.Location.IsEnemyKnown)).Observe<bool>((Func<bool>) (() => locationIcon.Location.Loot.HasValue && locationIcon.Location.Loot.Value.IsTreasure)).Do((Action<WorldMapLocationState, Option<WorldMapEntityProto>, bool, bool, bool>) ((state, entityProto, isScannedByRadar, showEnemy, isTreasure) => locationIcon.setLocationIcon(state, entityProto, isScannedByRadar, showEnemy, isTreasure)));
        updaterBuilder.Observe<bool>((Func<bool>) (() => locationIcon.Location.Entity.HasValue && locationIcon.Location.Entity.Value.IsPaused)).Do((Action<bool>) (isEntityPaused => locationIcon.m_notifBubble.SetVisibility<IconContainer>(isEntityPaused)));
        this.Updater = updaterBuilder.Build();
        this.positionSelf();

        void enterAction()
        {
          locationIcon.m_isHovered = true;
          locationIcon.updateColor();
        }

        void leaveAction()
        {
          locationIcon.m_isHovered = false;
          locationIcon.updateColor();
        }
      }

      private void setLocationColor(
        WorldMapLocationState state,
        bool isOwnedByPlayer,
        bool isEnemyKnown,
        bool hasEnemy,
        bool hasEntity)
      {
        if (isOwnedByPlayer)
        {
          this.m_markerColor = new ColorRgba(3821012479U);
          this.m_markerHoverColor = new ColorRgba(16766720);
        }
        else if (isEnemyKnown & hasEnemy)
        {
          this.m_markerColor = new ColorRgba(11672861);
          this.m_markerHoverColor = new ColorRgba(12787744);
        }
        else
        {
          switch (state)
          {
            case WorldMapLocationState.NotExplored:
              if (isEnemyKnown)
              {
                this.m_markerColor = new ColorRgba(5873186);
                this.m_markerHoverColor = new ColorRgba(6401569);
                break;
              }
              this.m_markerColor = new ColorRgba(14079702);
              this.m_markerHoverColor = new ColorRgba(16777215);
              break;
            case WorldMapLocationState.Explored:
              if (hasEntity)
              {
                this.m_markerColor = new ColorRgba(4560859);
                this.m_markerHoverColor = new ColorRgba(5220337);
                break;
              }
              this.m_markerColor = new ColorRgba(7508910);
              this.m_markerHoverColor = new ColorRgba(8628939);
              break;
            default:
              this.m_markerColor = new ColorRgba(11184810);
              this.m_markerHoverColor = new ColorRgba(11184810);
              break;
          }
        }
      }

      private void setLocationIcon(
        WorldMapLocationState state,
        Option<WorldMapEntityProto> entityProto,
        bool isScannedByRadar,
        bool showEnemy,
        bool isTreasure)
      {
        bool visibility = true;
        string spriteAssetPath;
        if (this.Location == this.m_mapManager.Map.HomeLocation)
          spriteAssetPath = "Assets/Unity/UserInterface/Toolbar/Settlement.svg";
        else if (isScannedByRadar && entityProto.HasValue)
          spriteAssetPath = entityProto.Value.Graphics.WorldMapIconPath;
        else if (isScannedByRadar & isTreasure)
          spriteAssetPath = "Assets/Unity/UserInterface/WorldMap/Treasure.svg";
        else if (showEnemy)
          spriteAssetPath = "Assets/Unity/UserInterface/WorldMap/PirateIcon128.png";
        else if (state == WorldMapLocationState.NotExplored)
        {
          spriteAssetPath = "Assets/Unity/UserInterface/WorldMap/UnknownLocation256.png";
          if (isScannedByRadar)
            visibility = false;
        }
        else
          spriteAssetPath = !this.Location.Entity.HasValue ? "Assets/Unity/UserInterface/General/Tick128.png" : this.Location.EntityProto.Value.Graphics.WorldMapIconPath;
        this.m_innerIcon.SetIcon(spriteAssetPath);
        this.m_innerIcon.SetVisibility<IconContainer>(visibility);
      }

      private void updateColor()
      {
        ColorRgba color = this.m_markerColor;
        if (this.m_isSelected || this.m_isHovered)
          color = this.m_markerHoverColor;
        this.m_icon.SetColor(color);
        this.m_innerIcon.SetColor(color);
        this.m_notifBubble.SetColor(color);
      }

      private void positionSelf()
      {
        int num = 72;
        if (this.m_isSelected)
          num = num * 4 / 3;
        this.PutToLeftBottomOf<WorldMapUi.LocationIcon>(this.m_iconsParent, new Vector2((float) num, (float) num), Offset.BottomLeft((float) this.Location.Position.Y, (float) this.Location.Position.X - (float) num / 2f));
        if (this.m_isSelected)
        {
          this.m_innerIcon.PutToCenterTopOf<IconContainer>((IUiElement) this.m_icon, 34.Vector2(), Offset.Top(19f));
          this.m_notifBubble.PutToCenterTopOf<IconContainer>((IUiElement) this.m_icon, 28.Vector2(), Offset.Top(-30f));
        }
        else
        {
          this.m_innerIcon.PutToCenterTopOf<IconContainer>((IUiElement) this.m_icon, 28.Vector2(), Offset.Top(13f));
          this.m_notifBubble.PutToCenterTopOf<IconContainer>((IUiElement) this.m_icon, 22.Vector2(), Offset.Top(-24f));
        }
      }

      public void Destroy()
      {
        this.m_dot.Destroy();
        this.GameObject.Destroy();
      }

      public void SetIsSelected(bool isSelected)
      {
        this.m_isSelected = isSelected;
        this.updateColor();
        this.positionSelf();
      }
    }
  }
}
