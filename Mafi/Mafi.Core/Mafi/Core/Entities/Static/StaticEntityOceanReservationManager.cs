// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.StaticEntityOceanReservationManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Monitors validity of ocean areas and notifies if they are blocked or unblocked.
  /// </summary>
  /// <remarks>
  /// This class groups areas monitoring and makes it way more effective than if we were to just monitor each
  /// area separately.
  /// 
  /// Areas are not saved and state is reconstructed on load.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class StaticEntityOceanReservationManager
  {
    public const int MIN_OCEAN_DEPTH = 2;
    public static readonly HeightTilesF MAX_OCEAN_FLOOR_HEIGHT;
    private readonly TerrainManager m_terrainManager;
    private readonly EntitiesManager m_entitiesManager;
    private readonly TerrainOccupancyManager m_occupancyManager;
    [DoNotSave(0, null)]
    private BitMap m_isValidOceanAreaTile;
    [DoNotSave(0, null)]
    private Lyst<StaticEntityOceanReservationManager.AreaRecord> m_monitoredAreas;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IEnumerable<IOceanAreaRecord> MonitoredAreas
    {
      get => (IEnumerable<IOceanAreaRecord>) this.m_monitoredAreas;
    }

    internal int MonitoredAreasCount => this.m_monitoredAreas.Count;

    internal int SumNonOceanTiles()
    {
      return this.m_monitoredAreas.Sum<StaticEntityOceanReservationManager.AreaRecord>((Func<StaticEntityOceanReservationManager.AreaRecord, int>) (x => x.NonOceanTiles));
    }

    public StaticEntityOceanReservationManager(
      TerrainManager terrainManager,
      EntitiesManager entitiesManager,
      TerrainOccupancyManager occupancyManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_entitiesManager = entitiesManager;
      this.m_occupancyManager = occupancyManager;
      terrainManager.HeightChanged.Add<StaticEntityOceanReservationManager>(this, new Action<Tile2iAndIndex>(this.tileChanged));
      terrainManager.OceanFlagChanged.Add<StaticEntityOceanReservationManager>(this, new Action<Tile2iAndIndex>(this.tileChanged));
      occupancyManager.TileOccupancyChanged.Add<StaticEntityOceanReservationManager>(this, new Action<Tile2iAndIndex>(this.tileChanged));
      entitiesManager.EntityAdded.Add<StaticEntityOceanReservationManager>(this, new Action<IEntity>(this.entityAdded));
      entitiesManager.EntityRemoved.Add<StaticEntityOceanReservationManager>(this, new Action<IEntity>(this.entityRemoved));
      terrainManager.TerrainGenerated.AddNonSaveable<StaticEntityOceanReservationManager>(this, new Action(this.terrainGenerated));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      this.allocateAndInitialize();
      foreach (IStaticEntityWithReservedOcean entityWithArea in this.m_entitiesManager.GetAllEntitiesOfType<IStaticEntityWithReservedOcean>())
        this.registerNewEntity(entityWithArea);
      if (saveVersion >= 140)
        return;
      this.m_terrainManager.TerrainGenerated.Remove<StaticEntityOceanReservationManager>(this, new Action(this.terrainGenerated));
    }

    /// <summary>
    /// Validates ocean area flags and reports number of incorrectly set tiles.
    /// </summary>
    public int ValidateAndFixOceanAreaFlags()
    {
      int num = 0;
      int index = 0;
      for (int terrainTilesCount = this.m_terrainManager.TerrainTilesCount; index < terrainTilesCount; ++index)
      {
        if (this.isValidOceanArea(new Tile2iIndex(index)))
        {
          if (this.m_isValidOceanAreaTile.SetBitReportChanged(index))
            ++num;
        }
        else if (this.m_isValidOceanAreaTile.ClearBitReportChanged(index))
          ++num;
      }
      if (num > 0)
      {
        foreach (StaticEntityOceanReservationManager.AreaRecord monitoredArea in this.m_monitoredAreas)
          this.initArea(monitoredArea);
      }
      return num;
    }

    public void TerrainGeneratedInMapEditor(Chunk64Area area)
    {
      Assert.That<bool>(this.m_monitoredAreas == null || this.m_monitoredAreas.IsEmpty).IsTrue();
      this.allocateAndInitialize();
    }

    private void terrainGenerated()
    {
      if (this.m_monitoredAreas != null)
        return;
      this.allocateAndInitialize();
    }

    private void allocateAndInitialize()
    {
      if (this.m_monitoredAreas == null)
        this.m_monitoredAreas = new Lyst<StaticEntityOceanReservationManager.AreaRecord>();
      this.m_monitoredAreas.Clear();
      if (this.m_isValidOceanAreaTile.Size != this.m_terrainManager.TerrainTilesCount)
        this.m_isValidOceanAreaTile = new BitMap(this.m_terrainManager.TerrainTilesCount);
      else
        this.m_isValidOceanAreaTile.ClearAllBits();
      int index = 0;
      for (int terrainTilesCount = this.m_terrainManager.TerrainTilesCount; index < terrainTilesCount; ++index)
      {
        if (this.isValidOceanArea(new Tile2iIndex(index)))
          this.m_isValidOceanAreaTile.SetBit(index);
      }
    }

    private void tileChanged(Tile2iAndIndex tileAndIndex)
    {
      bool flag = this.isValidOceanArea(tileAndIndex.Index);
      if (flag == this.m_isValidOceanAreaTile.IsSet(tileAndIndex.IndexRaw))
        return;
      Tile2i tileCoord = tileAndIndex.TileCoord;
      if (flag)
      {
        this.m_isValidOceanAreaTile.SetBit(tileAndIndex.IndexRaw);
        foreach (StaticEntityOceanReservationManager.AreaRecord monitoredArea in this.m_monitoredAreas)
        {
          if (monitoredArea.Area.ContainsTile(tileCoord))
            monitoredArea.MarkNonOceanTileAsOcean(tileAndIndex.TileCoord);
        }
      }
      else
      {
        this.m_isValidOceanAreaTile.ClearBit(tileAndIndex.IndexRaw);
        foreach (StaticEntityOceanReservationManager.AreaRecord monitoredArea in this.m_monitoredAreas)
        {
          if (monitoredArea.Area.ContainsTile(tileCoord))
            monitoredArea.MarkOceanTileAsNonOcean(tileAndIndex.TileCoord);
        }
      }
    }

    private bool isValidOceanArea(Tile2iIndex index)
    {
      return this.m_terrainManager.HasAnyTileFlagSet(index, 4U) && this.m_terrainManager.GetHeight(index) <= StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT && !this.m_occupancyManager.IsOccupied(index);
    }

    public bool IsTileValid(Tile2iIndex index) => this.m_isValidOceanAreaTile.IsSet(index.Value);

    public bool IsAreaValid(RectangleTerrainArea2i area)
    {
      if (this.m_monitoredAreas == null)
        return true;
      area = area.ClampToTerrainBounds(this.m_terrainManager);
      foreach (Tile2iAndIndex enumerateTilesAndIndex in area.EnumerateTilesAndIndices(this.m_terrainManager))
      {
        if (!this.m_isValidOceanAreaTile.IsSet(enumerateTilesAndIndex.IndexRaw))
          return false;
      }
      return true;
    }

    public bool IsAreaValid(
      RectangleTerrainArea2i area,
      Lyst<Tile2i> errorTiles,
      out int blockedTilesCount,
      out EntityId blockingEntityId)
    {
      area = area.ClampToTerrainBounds(this.m_terrainManager);
      int count = errorTiles.Count;
      blockingEntityId = new EntityId();
      foreach (Tile2iAndIndex enumerateTilesAndIndex in area.EnumerateTilesAndIndices(this.m_terrainManager))
      {
        if (!this.m_isValidOceanAreaTile.IsSet(enumerateTilesAndIndex.IndexRaw))
        {
          errorTiles.Add(enumerateTilesAndIndex.TileCoord);
          if (blockingEntityId == new EntityId())
            blockingEntityId = this.m_occupancyManager.GetLowestOccupyingEntity(enumerateTilesAndIndex.Index);
        }
      }
      blockedTilesCount = errorTiles.Count - count;
      return blockedTilesCount <= 0;
    }

    public void ExplainAreaValidity(
      RectangleTerrainArea2i area,
      out int totalBlocked,
      out int countBlockedByMissingOcean,
      out int countBlockedByTerrainHeight,
      out int countBlockedByEntity)
    {
      area = area.ClampToTerrainBounds(this.m_terrainManager);
      totalBlocked = 0;
      countBlockedByMissingOcean = 0;
      countBlockedByTerrainHeight = 0;
      countBlockedByEntity = 0;
      foreach (Tile2iIndex enumerateTileIndex in area.EnumerateTileIndices(this.m_terrainManager))
      {
        if (!this.m_isValidOceanAreaTile.IsSet(enumerateTileIndex.Value))
        {
          totalBlocked = 1;
          if (!this.m_terrainManager.HasAnyTileFlagSet(enumerateTileIndex, 4U))
            ++countBlockedByMissingOcean;
          if (this.m_terrainManager.GetHeight(enumerateTileIndex) > StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT)
            ++countBlockedByTerrainHeight;
          if (this.m_occupancyManager.IsOccupied(enumerateTileIndex))
            ++countBlockedByEntity;
        }
      }
    }

    public void GetOceanAreasFor(
      IStaticEntityWithReservedOcean entity,
      Lyst<IOceanAreaRecord> result)
    {
      foreach (StaticEntityOceanReservationManager.AreaRecord monitoredArea in this.m_monitoredAreas)
      {
        if (monitoredArea.Entity == entity)
          result.Add((IOceanAreaRecord) monitoredArea);
      }
    }

    private void entityAdded(IEntity entity)
    {
      if (!(entity is IStaticEntityWithReservedOcean entityWithArea))
        return;
      this.registerNewEntity(entityWithArea);
    }

    private void entityRemoved(IEntity entity)
    {
      if (!(entity is IStaticEntityWithReservedOcean entityWithArea))
        return;
      this.removeRegisteredEntity(entityWithArea);
    }

    private void registerNewEntity(IStaticEntityWithReservedOcean entityWithArea)
    {
      IProtoWithReservedOcean proto = entityWithArea.ReservedOceanAreaState.Proto;
      int length = proto.ReservedOceanAreasSets.Length;
      for (int index = 0; index < length; ++index)
      {
        ImmutableArray<RectangleTerrainArea2iRelative> reservedOceanAreasSet = proto.ReservedOceanAreasSets[index];
        for (int areaIndex = 0; areaIndex < reservedOceanAreasSet.Length; ++areaIndex)
        {
          bool isValid = this.registerNewArea(entityWithArea, index, areaIndex);
          entityWithArea.ReservedOceanAreaState.NotifyOceanAreaStatusChanged(index, areaIndex, isValid);
        }
      }
    }

    private void removeRegisteredEntity(IStaticEntityWithReservedOcean entityWithArea)
    {
      Assert.That<int>(this.m_monitoredAreas.RemoveWhere<IStaticEntityWithReservedOcean>(entityWithArea, (Func<StaticEntityOceanReservationManager.AreaRecord, IStaticEntityWithReservedOcean, bool>) ((x, e) => x.Entity == e))).IsEqualTo(entityWithArea.ReservedOceanAreaState.Proto.ReservedOceanAreasSets.Sum((Func<ImmutableArray<RectangleTerrainArea2iRelative>, int>) (x => x.Length)), "Not all entity areas were removed.");
    }

    private bool registerNewArea(
      IStaticEntityWithReservedOcean entityWithArea,
      int setIndex,
      int areaIndex)
    {
      RectangleTerrainArea2i terrainBounds = entityWithArea.ReservedOceanAreaState.AreasSets[setIndex][areaIndex];
      Assert.That<int>(terrainBounds.AreaTiles).IsPositive();
      terrainBounds = terrainBounds.ClampToTerrainBounds(this.m_terrainManager);
      StaticEntityOceanReservationManager.AreaRecord record = new StaticEntityOceanReservationManager.AreaRecord(entityWithArea, terrainBounds, setIndex, areaIndex);
      this.m_monitoredAreas.Add(record);
      return terrainBounds.AreaTiles <= 0 || this.initArea(record) == 0;
    }

    private int initArea(
      StaticEntityOceanReservationManager.AreaRecord record)
    {
      int num = 0;
      foreach (Tile2iAndIndex enumerateTilesAndIndex in record.Area.EnumerateTilesAndIndices(this.m_terrainManager))
      {
        if (this.m_isValidOceanAreaTile.IsNotSet(enumerateTilesAndIndex.IndexRaw))
        {
          ++num;
          record.NonOceanTilesIndices.SetBit(record.Area.GetTileIndex(enumerateTilesAndIndex.TileCoord));
        }
      }
      record.NonOceanTiles = num;
      return num;
    }

    internal void FixDeclareValid(IOceanAreaRecord area)
    {
      if (!(area is StaticEntityOceanReservationManager.AreaRecord areaRecord))
      {
        Log.Error("Failed to find area to fix.");
      }
      else
      {
        if (areaRecord.NonOceanTiles == 0)
          return;
        areaRecord.NonOceanTiles = 0;
        areaRecord.NonOceanTilesIndices.ClearAllBits();
        ReservedOceanAreaState reservedOceanAreaState = areaRecord.Entity.ReservedOceanAreaState;
        if (areaRecord.NonOceanTiles != 0)
          return;
        reservedOceanAreaState.NotifyOceanAreaStatusChanged(areaRecord.SetIndex, areaRecord.AreaIndex, true);
      }
    }

    public static void Serialize(StaticEntityOceanReservationManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StaticEntityOceanReservationManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StaticEntityOceanReservationManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      TerrainManager.Serialize(this.m_terrainManager, writer);
    }

    public static StaticEntityOceanReservationManager Deserialize(BlobReader reader)
    {
      StaticEntityOceanReservationManager reservationManager;
      if (reader.TryStartClassDeserialization<StaticEntityOceanReservationManager>(out reservationManager))
        reader.EnqueueDataDeserialization((object) reservationManager, StaticEntityOceanReservationManager.s_deserializeDataDelayedAction);
      return reservationManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<StaticEntityOceanReservationManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<StaticEntityOceanReservationManager>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.SetField<StaticEntityOceanReservationManager>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.RegisterInitAfterLoad<StaticEntityOceanReservationManager>(this, "initSelf", InitPriority.Normal);
    }

    static StaticEntityOceanReservationManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT = new HeightTilesF(-2);
      StaticEntityOceanReservationManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StaticEntityOceanReservationManager) obj).SerializeData(writer));
      StaticEntityOceanReservationManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StaticEntityOceanReservationManager) obj).DeserializeData(reader));
    }

    private sealed class AreaRecord : IOceanAreaRecord
    {
      public IStaticEntityWithReservedOcean Entity { get; }

      public RectangleTerrainArea2i Area { get; }

      public int SetIndex { get; }

      public int AreaIndex { get; }

      public BitMap NonOceanTilesIndices { get; }

      public int NonOceanTiles { get; set; }

      public AreaRecord(
        IStaticEntityWithReservedOcean entity,
        RectangleTerrainArea2i area,
        int setIndex,
        int areaIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Area = area;
        this.SetIndex = setIndex;
        this.AreaIndex = areaIndex;
        this.Entity = entity;
        this.NonOceanTilesIndices = new BitMap(area.AreaTiles);
        this.NonOceanTiles = 0;
      }

      public void MarkNonOceanTileAsOcean(Tile2i tile)
      {
        this.NonOceanTilesIndices.ClearBit(this.Area.GetTileIndex(tile));
        --this.NonOceanTiles;
        if (this.NonOceanTiles != 0)
          return;
        this.Entity.ReservedOceanAreaState.NotifyOceanAreaStatusChanged(this.SetIndex, this.AreaIndex, true);
      }

      public void MarkOceanTileAsNonOcean(Tile2i tile)
      {
        this.NonOceanTilesIndices.SetBit(this.Area.GetTileIndex(tile));
        ++this.NonOceanTiles;
        if (this.NonOceanTiles != 1)
          return;
        this.Entity.ReservedOceanAreaState.NotifyOceanAreaStatusChanged(this.SetIndex, this.AreaIndex, false);
      }

      public override string ToString()
      {
        return string.Format("Area {0} for {1} with {2} non-ocean tiles, ", (object) this.Area, (object) this.Entity, (object) this.NonOceanTiles) + string.Format("set #{0}, area #{1}", (object) this.SetIndex, (object) this.AreaIndex);
      }
    }
  }
}
