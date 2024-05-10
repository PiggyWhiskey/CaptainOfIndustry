// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMap
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.World.Entities;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  public class WorldMap
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Dict<WorldMapLocId, WorldMapLocation> m_locations;
    private readonly Dict<WorldMapLocId, Lyst<WorldMapLocation>> m_neighbors;
    private readonly Set<WorldMapConnection> m_connections;
    public Vector2i Size;
    private WorldMapManager m_manager;
    private WorldMapLocId m_lastUsedLocationId;

    public static void Serialize(WorldMap value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMap>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMap.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      WorldMapLocation.Serialize(this.HomeLocation, writer);
      Set<WorldMapConnection>.Serialize(this.m_connections, writer);
      WorldMapLocId.Serialize(this.m_lastUsedLocationId, writer);
      Dict<WorldMapLocId, WorldMapLocation>.Serialize(this.m_locations, writer);
      WorldMapManager.Serialize(this.m_manager, writer);
      Dict<WorldMapLocId, Lyst<WorldMapLocation>>.Serialize(this.m_neighbors, writer);
      Vector2i.Serialize(this.Size, writer);
    }

    public static WorldMap Deserialize(BlobReader reader)
    {
      WorldMap worldMap;
      if (reader.TryStartClassDeserialization<WorldMap>(out worldMap))
        reader.EnqueueDataDeserialization((object) worldMap, WorldMap.s_deserializeDataDelayedAction);
      return worldMap;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.HomeLocation = WorldMapLocation.Deserialize(reader);
      reader.SetField<WorldMap>(this, "m_connections", (object) Set<WorldMapConnection>.Deserialize(reader));
      this.m_lastUsedLocationId = WorldMapLocId.Deserialize(reader);
      reader.SetField<WorldMap>(this, "m_locations", (object) Dict<WorldMapLocId, WorldMapLocation>.Deserialize(reader));
      this.m_manager = WorldMapManager.Deserialize(reader);
      reader.SetField<WorldMap>(this, "m_neighbors", (object) Dict<WorldMapLocId, Lyst<WorldMapLocation>>.Deserialize(reader));
      this.Size = Vector2i.Deserialize(reader);
    }

    public WorldMapLocation HomeLocation { get; private set; }

    public IReadOnlyCollection<WorldMapLocation> Locations
    {
      get => (IReadOnlyCollection<WorldMapLocation>) this.m_locations.Values;
    }

    public IReadOnlyDictionary<WorldMapLocId, WorldMapLocation> LocationsDict
    {
      get => (IReadOnlyDictionary<WorldMapLocId, WorldMapLocation>) this.m_locations;
    }

    public int LocationsCount => this.m_locations.Count;

    public IReadOnlySet<WorldMapConnection> Connections
    {
      get => (IReadOnlySet<WorldMapConnection>) this.m_connections;
    }

    public WorldMap()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_locations = new Dict<WorldMapLocId, WorldMapLocation>();
      this.m_neighbors = new Dict<WorldMapLocId, Lyst<WorldMapLocation>>();
      this.m_connections = new Set<WorldMapConnection>();
      this.Size = new Vector2i(4096, 4096);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public Option<WorldMapLocation> this[WorldMapLocId id]
    {
      get => this.m_locations.Get<WorldMapLocId, WorldMapLocation>(id);
    }

    public void SetHomeLocation(WorldMapLocation location)
    {
      Assert.That<WorldMapManager>(this.m_manager).IsNull<WorldMapManager>();
      Assert.That<Dict<WorldMapLocId, Lyst<WorldMapLocation>>>(this.m_neighbors).ContainsKey<WorldMapLocId, Lyst<WorldMapLocation>>(location.Id);
      this.HomeLocation = location;
    }

    public void Initialize(WorldMapManager worldMapManager)
    {
      Assert.That<WorldMapLocation>(this.HomeLocation).IsNotNull<WorldMapLocation>("Home location was not set.");
      this.m_manager = worldMapManager;
      if (this.HomeLocation.Enemy.HasValue)
      {
        Log.Error("Home location has enemy! Removing.");
        this.HomeLocation.MarkEnemyAsDefeated();
      }
      this.Visit(this.HomeLocation, 1, 0);
      WorldMapLocation location = this.m_neighbors[this.HomeLocation.Id].FirstOrDefault<WorldMapLocation>((Predicate<WorldMapLocation>) (x => x.EntityProto.ValueOrNull is WorldMapVillageProto));
      if (location == null)
        return;
      this.Visit(location, 1, 0);
    }

    public void Deactivate() => this.m_manager = (WorldMapManager) null;

    public bool HasConnection(WorldMapLocation loc1, WorldMapLocation loc2)
    {
      return this.m_connections.Contains(new WorldMapConnection(loc1, loc2));
    }

    public void AddLocation(WorldMapLocation location, bool doNotGenerateId = false)
    {
      if (this.m_neighbors.ContainsKey(location.Id))
      {
        Log.Error(string.Format("Map location at {0} is already in the map.", (object) location.Position));
      }
      else
      {
        if (!doNotGenerateId)
          location.SetId(this.getNextLocationId());
        this.m_locations.Add(location.Id, location);
        this.m_neighbors.Add(location.Id, new Lyst<WorldMapLocation>());
        this.m_manager?.OnLocationAdded(location);
      }
    }

    private WorldMapLocId getNextLocationId()
    {
      return this.m_lastUsedLocationId = new WorldMapLocId(this.m_lastUsedLocationId.Value + 1);
    }

    public bool RemoveLocation(WorldMapLocation location)
    {
      if (!this.m_locations.ContainsKey(location.Id))
        return false;
      Lyst<WorldMapConnection> lyst = this.ConnectionsFor(location).ToLyst<WorldMapConnection>();
      this.m_connections.RemoveRange((IEnumerable<WorldMapConnection>) lyst);
      foreach (WorldMapConnection worldMapConnection in lyst)
        this.m_neighbors[worldMapConnection.GetOtherLocation(location).Id].RemoveAndAssert(location);
      this.m_neighbors.RemoveAndAssert(location.Id);
      this.m_locations.RemoveAndAssert(location.Id);
      if (this.m_manager != null)
      {
        foreach (WorldMapConnection connection in lyst)
          this.m_manager?.OnConnectionRemoved(connection);
        this.m_manager?.OnLocationRemoved(location);
      }
      location.ResetId();
      return true;
    }

    public bool AddConnection(WorldMapLocation loc1, WorldMapLocation loc2)
    {
      WorldMapConnection connection = new WorldMapConnection(loc1, loc2);
      if (!this.m_connections.Add(connection))
        return false;
      this.m_neighbors[loc1.Id].Add(loc2);
      this.m_neighbors[loc2.Id].Add(loc1);
      this.m_manager?.OnConnectionAdded(connection);
      return true;
    }

    public bool RemoveConnection(WorldMapLocation loc1, WorldMapLocation loc2)
    {
      WorldMapConnection connection = new WorldMapConnection(loc1, loc2);
      if (!this.m_connections.Remove(connection))
        return false;
      this.m_neighbors[loc1.Id].RemoveAndAssert(loc2);
      this.m_neighbors[loc2.Id].RemoveAndAssert(loc1);
      this.m_manager?.OnConnectionRemoved(connection);
      return true;
    }

    public void MoveLocation(WorldMapLocation loc, Vector2i locPosition)
    {
      loc.SetPosition(locPosition);
      this.m_manager?.OnLocationChanged(loc);
    }

    public IEnumerable<WorldMapLocation> NeighborsOf(WorldMapLocation location)
    {
      return (IEnumerable<WorldMapLocation>) this.m_neighbors[location.Id];
    }

    public IEnumerable<WorldMapConnection> ConnectionsFor(WorldMapLocation location)
    {
      return this.NeighborsOf(location).Select<WorldMapLocation, WorldMapConnection>((Func<WorldMapLocation, WorldMapConnection>) (x => new WorldMapConnection(location, x)));
    }

    public void Visit(
      WorldMapLocation location,
      int neighborDistanceToReveal,
      int distanceToRevealEntities)
    {
      Assert.That<int>(distanceToRevealEntities).IsLessOrEqual(neighborDistanceToReveal);
      if (location.State == WorldMapLocationState.NotExplored || location.State == WorldMapLocationState.Hidden)
      {
        location.SetState(WorldMapLocationState.Explored);
        this.m_manager?.OnLocationExplored(location);
      }
      this.markNeighborsVisible(location, neighborDistanceToReveal, distanceToRevealEntities);
    }

    private void markNeighborsVisible(
      WorldMapLocation location,
      int depthToMakeVisible,
      int depthToRevealEntities)
    {
      if (depthToMakeVisible == 0)
        return;
      foreach (WorldMapLocation location1 in this.NeighborsOf(location))
      {
        if (location1.State == WorldMapLocationState.Hidden)
        {
          location1.SetState(WorldMapLocationState.NotExplored);
          this.m_manager?.OnLocationChanged(location1);
        }
        if (depthToRevealEntities > 0)
        {
          location1.IsEnemyKnown = true;
          location1.IsScannedByRadar = true;
        }
        this.markNeighborsVisible(location1, depthToMakeVisible - 1, depthToRevealEntities - 1);
      }
    }

    public Option<WorldMapLocation> FindClosestLocation(
      Vector2f position,
      Predicate<WorldMapLocation> predicate)
    {
      Fix64 fix64_1 = Fix64.MaxValue;
      Option<WorldMapLocation> closestLocation = Option<WorldMapLocation>.None;
      foreach (WorldMapLocation worldMapLocation in this.m_locations.Values)
      {
        Fix64 fix64_2 = position.DistanceSqrTo(worldMapLocation.Position.Vector2f);
        if (fix64_2 < fix64_1 && predicate(worldMapLocation))
        {
          fix64_1 = fix64_2;
          closestLocation = (Option<WorldMapLocation>) worldMapLocation;
        }
      }
      return closestLocation;
    }

    static WorldMap()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMap.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldMap) obj).SerializeData(writer));
      WorldMap.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldMap) obj).DeserializeData(reader));
    }
  }
}
