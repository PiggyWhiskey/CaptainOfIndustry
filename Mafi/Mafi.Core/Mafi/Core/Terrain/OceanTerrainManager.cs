// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.OceanTerrainManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class OceanTerrainManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public const int MIN_CONNECTED_TILES_FOR_FLOODING = 10;
    /// <summary>
    /// A tile is no longer considered an ocean if the terrain height is at or above this height.
    /// </summary>
    /// <remarks>
    /// Ocean is rendered at height of 0.25 tiles but its surface is moving a little.
    /// The actual threshold is 0.15 tiles so that tiles with ocean have some ocean mesh visible.
    /// The ocean threshold is above zero so that terrain mined to height 0 is flooded.
    /// </remarks>
    public static readonly HeightTilesF OCEAN_THRESHOLD;
    public const int MAX_TILES_PER_TICK = 200;
    private readonly Queueue<Tile2iAndIndex> m_tilesToCheckForFlooding;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<Tile2iIndex> m_connTilesTmp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<IStaticEntity> m_occupiedEntitiesTmp;
    private readonly TerrainManager m_terrainManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly EntityCollapseHelper m_collapseHelper;
    private readonly TerrainOccupancyManager m_occupancyManager;

    public static void Serialize(OceanTerrainManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<OceanTerrainManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, OceanTerrainManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      EntityCollapseHelper.Serialize(this.m_collapseHelper, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      Queueue<Tile2iAndIndex>.Serialize(this.m_tilesToCheckForFlooding, writer);
    }

    public static OceanTerrainManager Deserialize(BlobReader reader)
    {
      OceanTerrainManager oceanTerrainManager;
      if (reader.TryStartClassDeserialization<OceanTerrainManager>(out oceanTerrainManager))
        reader.EnqueueDataDeserialization((object) oceanTerrainManager, OceanTerrainManager.s_deserializeDataDelayedAction);
      return oceanTerrainManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<OceanTerrainManager>(this, "m_collapseHelper", (object) EntityCollapseHelper.Deserialize(reader));
      reader.SetField<OceanTerrainManager>(this, "m_connTilesTmp", (object) new Lyst<Tile2iIndex>());
      reader.SetField<OceanTerrainManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<OceanTerrainManager>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.SetField<OceanTerrainManager>(this, "m_occupiedEntitiesTmp", (object) new Lyst<IStaticEntity>());
      reader.SetField<OceanTerrainManager>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<OceanTerrainManager>(this, "m_tilesToCheckForFlooding", (object) Queueue<Tile2iAndIndex>.Deserialize(reader));
    }

    public int QueuedTilesCount => this.m_tilesToCheckForFlooding.Count;

    public int ProcessedLastTick => 0;

    public OceanTerrainManager(
      TerrainManager terrainManager,
      SimLoopEvents simLoopEvents,
      IEntitiesManager entitiesManager,
      EntityCollapseHelper collapseHelper,
      TerrainOccupancyManager occupancyManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_tilesToCheckForFlooding = new Queueue<Tile2iAndIndex>();
      this.m_connTilesTmp = new Lyst<Tile2iIndex>(true);
      this.m_occupiedEntitiesTmp = new Lyst<IStaticEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_entitiesManager = entitiesManager;
      this.m_collapseHelper = collapseHelper;
      this.m_occupancyManager = occupancyManager;
      terrainManager.HeightChanged.Add<OceanTerrainManager>(this, new Action<Tile2iAndIndex>(this.tileHeightChanged));
      terrainManager.OceanFlagChanged.Add<OceanTerrainManager>(this, new Action<Tile2iAndIndex>(this.oceanChanged));
      simLoopEvents.Update.Add<OceanTerrainManager>(this, new Action(this.simUpdate));
    }

    private void oceanChanged(Tile2iAndIndex tileAndIndex)
    {
      if (!this.m_terrainManager.IsOcean(tileAndIndex.Index))
        return;
      this.m_tilesToCheckForFlooding.Enqueue(tileAndIndex);
      if (!this.m_occupancyManager.IsOccupied(tileAndIndex.Index))
        return;
      this.m_occupiedEntitiesTmp.Clear();
      this.m_occupancyManager.GetAllOccupiedEntitiesAt(tileAndIndex.Index, this.m_occupiedEntitiesTmp);
      foreach (IStaticEntity staticEntity in this.m_occupiedEntitiesTmp)
      {
        if (!staticEntity.IsDestroyed && !(staticEntity is TransportPillar))
        {
          int testedTilesCount;
          if (staticEntity.HasOccTileWithAnyConstraintAt(tileAndIndex.TileCoord, LayoutTileConstraint.Ground, out testedTilesCount))
            this.m_collapseHelper.TryDestroyEntityAndAddRubble(staticEntity);
          Assert.That<int>(testedTilesCount).IsPositive<IStaticEntity, Tile2iSlim>("Entity {0} is in occupied map at {1} but returned no occupied tile.", staticEntity, tileAndIndex.TileCoordSlim);
        }
      }
      this.m_occupiedEntitiesTmp.Clear();
    }

    private void tileHeightChanged(Tile2iAndIndex tileAndIndex)
    {
      HeightTilesF height = this.m_terrainManager.GetHeight(tileAndIndex.Index);
      if (this.m_terrainManager.IsOcean(tileAndIndex.Index))
      {
        if (!(height >= OceanTerrainManager.OCEAN_THRESHOLD))
          return;
        this.m_terrainManager.ClearTileFlags(tileAndIndex, 4U);
      }
      else
      {
        if (height >= OceanTerrainManager.OCEAN_THRESHOLD)
          return;
        if (this.m_terrainManager.IsOnMapBoundary(tileAndIndex.Index))
        {
          this.m_terrainManager.SetTileFlags(tileAndIndex, 4U);
        }
        else
        {
          foreach (Tile2iAndIndexRel sideNeighborsDelta in this.m_terrainManager.FourSideNeighborsDeltas)
          {
            if (this.m_terrainManager.IsOcean((tileAndIndex + sideNeighborsDelta).Index))
            {
              this.m_terrainManager.SetTileFlags(tileAndIndex, 4U);
              break;
            }
          }
        }
      }
    }

    private void simUpdate()
    {
      if (this.m_tilesToCheckForFlooding.IsEmpty)
        return;
      int num = 0;
      while (this.m_tilesToCheckForFlooding.IsNotEmpty && num < 200)
      {
        Tile2iAndIndex tileAndIndex1 = this.m_tilesToCheckForFlooding.Dequeue();
        if (this.m_terrainManager.IsOcean(tileAndIndex1.Index))
        {
          ++num;
          ImmutableArray<Tile2iAndIndexRel> sideNeighborsDeltas;
          if (this.m_terrainManager.IsOnMapBoundary(tileAndIndex1.Index))
          {
            sideNeighborsDeltas = this.m_terrainManager.FourSideNeighborsDeltas;
            foreach (Tile2iAndIndexRel tile2iAndIndexRel in sideNeighborsDeltas)
            {
              Tile2iAndIndex tileAndIndex2 = tileAndIndex1 + tile2iAndIndexRel;
              if (this.m_terrainManager.IsValidCoord(tileAndIndex2.TileCoord) && !this.m_terrainManager.IsOcean(tileAndIndex2.Index) && this.m_terrainManager.GetHeight(tileAndIndex2.Index) < OceanTerrainManager.OCEAN_THRESHOLD)
                this.m_terrainManager.SetTileFlags(tileAndIndex2, 4U);
            }
          }
          else
          {
            this.m_connTilesTmp.Clear();
            if (!isLargeWaterBody(tileAndIndex1.Index, this.m_connTilesTmp))
            {
              if (this.m_connTilesTmp.Count < 10)
              {
                this.m_terrainManager.ClearTileFlags(tileAndIndex1, 4U);
                foreach (Tile2iIndex index in this.m_connTilesTmp)
                  this.m_terrainManager.ClearTileFlags(this.m_terrainManager.ExtendTileCoord_Slow(index), 4U);
              }
              else
                Log.Error("Too many ocean tiles found but `isLargeWaterBody` returned false.");
            }
            else
            {
              sideNeighborsDeltas = this.m_terrainManager.FourSideNeighborsDeltas;
              foreach (Tile2iAndIndexRel tile2iAndIndexRel in sideNeighborsDeltas)
              {
                Tile2iAndIndex tileAndIndex3 = tileAndIndex1 + tile2iAndIndexRel;
                if (!this.m_terrainManager.IsOcean(tileAndIndex3.Index) && this.m_terrainManager.GetHeight(tileAndIndex3.Index) < OceanTerrainManager.OCEAN_THRESHOLD)
                  this.m_terrainManager.SetTileFlags(tileAndIndex3, 4U);
              }
            }
          }
        }
      }

      bool testTile(Tile2iIndex i, Lyst<Tile2iIndex> visited)
      {
        if (this.m_terrainManager.IsOnMapBoundary(i))
          return true;
        if (!this.m_terrainManager.IsOcean(i) || !visited.AddIfNotPresent(i))
          return false;
        return visited.Count >= 10 || isLargeWaterBody(i, visited);
      }

      bool isLargeWaterBody(Tile2iIndex i, Lyst<Tile2iIndex> visited)
      {
        return testTile(i - 1, visited) || testTile(i + 1, visited) || testTile(i - this.m_terrainManager.TerrainWidth, visited) || testTile(i + this.m_terrainManager.TerrainWidth, visited);
      }
    }

    static OceanTerrainManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      OceanTerrainManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((OceanTerrainManager) obj).SerializeData(writer));
      OceanTerrainManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((OceanTerrainManager) obj).DeserializeData(reader));
      OceanTerrainManager.OCEAN_THRESHOLD = new HeightTilesF(0.15.ToFix32());
    }
  }
}
