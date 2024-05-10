// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldMapEditorUi
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Fleet;
using Mafi.Core.Prototypes;
using Mafi.Core.World;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  /// <summary>
  /// Simple world map editor.
  /// 
  /// * Usage
  /// * New node: [Alt] + [LMB]
  /// * Move node: arrows, use [Shift], [Ctrl], and [Alt] for distance multipliers.
  /// * Remove node: Select it, then [Alt] + [RMB] (this removes all of its connections too).
  /// * Add/remove connection: Select start node, then [Ctrl] + [LMB] on goal node.
  /// </summary>
  internal class WorldMapEditorUi : IUiElement
  {
    private readonly WorldMapManager m_mapManager;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly IUiElement m_mapElement;
    private readonly WorldMapView m_mapView;
    private readonly ProtosDb m_protosDb;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public WorldMapEditorUi(
      UiBuilder builder,
      string name,
      WorldMapManager mapManager,
      TravelingFleetManager fleetManager,
      IUiElement mapElement,
      WorldMapView mapView,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_mapManager = mapManager;
      this.m_fleetManager = fleetManager;
      this.m_mapElement = mapElement;
      this.m_mapView = mapView;
      this.m_protosDb = protosDb;
      this.GameObject = new GameObject(name);
      this.RectTransform = this.GameObject.AddComponent<RectTransform>();
      this.GameObject.AddComponent<CanvasRenderer>();
      Btn objectToPlace1 = builder.NewBtnPrimary("GeneratePos").SetText("GenPos").OnClick(new Action(this.genCode));
      objectToPlace1.PutToCenterTopOf<Btn>((IUiElement) this, objectToPlace1.GetOptimalSize());
      Btn objectToPlace2 = builder.NewBtn("ToggleVis").SetText("Toggle Vis").OnClick((Action) (() => this.m_mapView.ToggleEntireMapVisibility())).SetButtonStyle(builder.Style.Global.GeneralBtn);
      objectToPlace2.PutToCenterBottomOf<Btn>((IUiElement) this, objectToPlace2.GetOptimalSize());
    }

    private void genCode()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Map code below");
      stringBuilder1.AppendLine();
      Dict<WorldMapLocation, int> dict1 = new Dict<WorldMapLocation, int>();
      List<FleetEnginePartProto> list = this.m_protosDb.All<FleetEnginePartProto>().OrderBy<FleetEnginePartProto, Quantity>((Func<FleetEnginePartProto, Quantity>) (x => x.FuelCapacity)).ToList<FleetEnginePartProto>();
      Lyst<Tuple<int, WorldMapLocation>> lyst = new Lyst<Tuple<int, WorldMapLocation>>();
      foreach (WorldMapLocation location in (IEnumerable<WorldMapLocation>) this.m_mapManager.Map.Locations)
      {
        if (this.m_mapManager.Map.HomeLocation == location)
        {
          lyst.Add(new Tuple<int, WorldMapLocation>(0, location));
        }
        else
        {
          int travelDistanceBetween = this.m_fleetManager.ComputeTravelDistanceBetween(this.m_mapManager.Map.HomeLocation.Id, location.Id, false);
          lyst.Add(new Tuple<int, WorldMapLocation>(travelDistanceBetween, location));
        }
      }
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.AppendLine("Dict<WorldMapLocation, int> distances = new Dict<WorldMapLocation, int>();");
      Dict<FleetEnginePartProto, int> dict2 = new Dict<FleetEnginePartProto, int>();
      Dict<FleetEnginePartProto, AssetValue> dict3 = new Dict<FleetEnginePartProto, AssetValue>();
      foreach (Tuple<int, WorldMapLocation> tuple in (IEnumerable<Tuple<int, WorldMapLocation>>) lyst.OrderBy<Tuple<int, WorldMapLocation>, int>((Func<Tuple<int, WorldMapLocation>, int>) (x => x.Item1)))
      {
        int count = dict1.Count;
        WorldMapLocation key1 = tuple.Item2;
        int distance = tuple.Item1;
        stringBuilder1.AppendLine(string.Format("var loc{0} = new WorldMapLocation(\"{1}\", new Vector2i({2}, {3}));", (object) count, (object) key1.Name, (object) key1.Position.X, (object) key1.Position.Y));
        stringBuilder1.AppendLine(string.Format("map.AddLocation(loc{0});", (object) count));
        dict1.Add(key1, count);
        FleetEnginePartProto key2 = list.FirstOrDefault<FleetEnginePartProto>((Func<FleetEnginePartProto, bool>) (x => x.GetFuelCostFromDistance((Fix32) (distance * 2 + 550)) <= x.FuelCapacity));
        stringBuilder2.Append(string.Format("distances.Add(loc{0}, {1});", (object) count, (object) distance));
        if ((Proto) key2 != (Proto) null)
        {
          stringBuilder2.AppendLine(string.Format(" // ({0}: {1} fuel)", (object) key2.Id.Value, (object) key2.GetFuelCostFromDistance((Fix32) (distance * 2 + 550))));
          if (key1.Loot.HasValue)
          {
            dict2[key2] = dict2.GetOrCreate<FleetEnginePartProto, int>(key2, (Func<int>) (() => 0)) + key1.Loot.Value.People;
            dict3[key2] = dict3.GetOrCreate<FleetEnginePartProto, AssetValue>(key2, (Func<AssetValue>) (() => AssetValue.Empty)) + key1.Loot.Value.Products;
          }
        }
        else
          stringBuilder2.AppendLine(" // (unreachable)");
      }
      stringBuilder1.AppendLine();
      foreach (WorldMapConnection connection in (IEnumerable<WorldMapConnection>) this.m_mapManager.Map.Connections)
        stringBuilder1.AppendLine(string.Format("map.AddConnection(loc{0}, loc{1});", (object) dict1[connection.Location1], (object) dict1[connection.Location2]));
      stringBuilder1.AppendLine();
      stringBuilder1.Append(stringBuilder2);
      stringBuilder1.AppendLine();
      int lastKnownMaxRange = 1;
      foreach (FleetEnginePartProto fleetEnginePartProto in list)
      {
        int range = ((fleetEnginePartProto.FuelCapacity.Value.ToFix32() * fleetEnginePartProto.DistancePerFuel).ToIntFloored() - 550) / 2;
        string lower = fleetEnginePartProto.Id.Value.ToLower();
        stringBuilder1.AppendLine(string.Format("var {0}MaxRange = {1};", (object) lower, (object) range));
        string str = lyst.Where<Tuple<int, WorldMapLocation>>((Func<Tuple<int, WorldMapLocation>, bool>) (x => x.Item1 > lastKnownMaxRange && x.Item1 < range)).OrderBy<Tuple<int, WorldMapLocation>, int>((Func<Tuple<int, WorldMapLocation>, int>) (x => x.Item1)).Select<Tuple<int, WorldMapLocation>, string>((Func<Tuple<int, WorldMapLocation>, string>) (x => x.Item1.ToString())).JoinStrings(", ");
        stringBuilder1.AppendLine("var " + lower + "Locations = ImmutableArray.Create(" + str + ");");
        lastKnownMaxRange = range;
      }
      stringBuilder1.AppendLine();
      stringBuilder1.AppendLine("// Loot analysis");
      foreach (KeyValuePair<FleetEnginePartProto, int> keyValuePair in dict2)
      {
        stringBuilder1.AppendLine(string.Format("// {0} => {1} refugees (without villages)", (object) keyValuePair.Key.Id.Value, (object) keyValuePair.Value));
        foreach (ProductQuantity product in dict3[keyValuePair.Key].Products)
          stringBuilder1.AppendLine(string.Format("// - {0}", (object) product));
      }
      Log.Error(stringBuilder1.ToString());
    }

    public bool InputUpdate()
    {
      if (this.m_mapView.SelectedLocation.HasValue)
      {
        Vector2i vector2i = Vector2i.Zero;
        if (Input.GetKeyDown(KeyCode.UpArrow))
          vector2i = new Vector2i(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
          vector2i = new Vector2i(0, -1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
          vector2i = new Vector2i(-1, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
          vector2i = new Vector2i(1, 0);
        if (vector2i != Vector2i.Zero)
        {
          if (Input.GetKey(KeyCode.LeftShift))
            vector2i *= 2;
          if (Input.GetKey(KeyCode.LeftControl))
            vector2i *= 5;
          if (Input.GetKey(KeyCode.LeftAlt))
            vector2i *= 10;
          WorldMapLocation loc = this.m_mapView.SelectedLocation.Value;
          this.m_mapManager.Map.MoveLocation(loc, loc.Position + vector2i);
          return true;
        }
      }
      Vector3 vector3 = this.m_mapElement.RectTransform.InverseTransformPoint(Input.mousePosition);
      if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0))
      {
        Vector2i position = new Vector2i(vector3.x.RoundToInt(), (this.m_mapElement.RectTransform.rect.height + vector3.y).RoundToInt());
        this.m_mapManager.Map.AddLocation(new WorldMapLocation(string.Format("Location {0}", (object) this.m_mapManager.Map.LocationsCount), position));
        return true;
      }
      if (!Input.GetKey(KeyCode.LeftAlt) || !Input.GetMouseButtonDown(1) || !this.m_mapView.SelectedLocation.HasValue)
        return false;
      this.m_mapManager.Map.RemoveLocation(this.m_mapView.SelectedLocation.Value);
      return true;
    }

    public bool ProcessLocationSelected(WorldMapLocation location)
    {
      if (!Input.GetKey(KeyCode.LeftControl) || this.m_mapView.SelectedLocation.IsNone)
        return false;
      WorldMapLocation loc1 = this.m_mapView.SelectedLocation.Value;
      if (loc1 == location)
        return false;
      if (this.m_mapManager.Map.HasConnection(loc1, location))
        this.m_mapManager.Map.RemoveConnection(loc1, location);
      else
        this.m_mapManager.Map.AddConnection(loc1, location);
      return true;
    }
  }
}
