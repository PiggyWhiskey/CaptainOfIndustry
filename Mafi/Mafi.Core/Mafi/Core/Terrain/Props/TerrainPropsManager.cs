// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Props.TerrainPropsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain.Designation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Props
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TerrainPropsManager : ITerrainPropsManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly TerrainManager m_terrainManager;
    private readonly TerrainOccupancyManager m_occupancyManager;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<TerrainPropId, TerrainPropData> m_props;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<Tile2iSlim, TerrainPropId> m_tileToPropId;
    private readonly Set<TerrainPropId> m_removedProps;
    private readonly Event<Tile2iAndIndex> m_propChangedAt;
    private readonly TileFlagReporter m_flagReporter;
    [DoNotSaveCreateNewOnLoad("new Lyst<Tile2i>(canOmitClearing: true)", 0)]
    private readonly Lyst<Tile2i> m_tmpOccupiedTiles;
    [DoNotSaveCreateNewOnLoad("new Lyst<Tile2i>(canOmitClearing: true)", 0)]
    private readonly Lyst<Tile2i> m_tmpOccupiedTilesForAdd;

    public static void Serialize(TerrainPropsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainPropsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainPropsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      TileFlagReporter.Serialize(this.m_flagReporter, writer);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      Event<Tile2iAndIndex>.Serialize(this.m_propChangedAt, writer);
      Set<TerrainPropId>.Serialize(this.m_removedProps, writer);
      TerrainManager.Serialize(this.m_terrainManager, writer);
    }

    public static TerrainPropsManager Deserialize(BlobReader reader)
    {
      TerrainPropsManager terrainPropsManager;
      if (reader.TryStartClassDeserialization<TerrainPropsManager>(out terrainPropsManager))
        reader.EnqueueDataDeserialization((object) terrainPropsManager, TerrainPropsManager.s_deserializeDataDelayedAction);
      return terrainPropsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainPropsManager>(this, "m_flagReporter", (object) TileFlagReporter.Deserialize(reader));
      reader.SetField<TerrainPropsManager>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.SetField<TerrainPropsManager>(this, "m_propChangedAt", (object) Event<Tile2iAndIndex>.Deserialize(reader));
      reader.SetField<TerrainPropsManager>(this, "m_props", (object) new Dict<TerrainPropId, TerrainPropData>());
      reader.SetField<TerrainPropsManager>(this, "m_removedProps", (object) Set<TerrainPropId>.Deserialize(reader));
      reader.SetField<TerrainPropsManager>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<TerrainPropsManager>(this, "m_tileToPropId", (object) new Dict<Tile2iSlim, TerrainPropId>());
      reader.SetField<TerrainPropsManager>(this, "m_tmpOccupiedTiles", (object) new Lyst<Tile2i>(true));
      reader.SetField<TerrainPropsManager>(this, "m_tmpOccupiedTilesForAdd", (object) new Lyst<Tile2i>(true));
      reader.RegisterInitAfterLoad<TerrainPropsManager>(this, "initSelf", InitPriority.Lowest);
    }

    public IReadOnlyDictionary<TerrainPropId, TerrainPropData> TerrainProps
    {
      get => (IReadOnlyDictionary<TerrainPropId, TerrainPropData>) this.m_props;
    }

    public IReadOnlyDictionary<Tile2iSlim, TerrainPropId> TerrainTileToProp
    {
      get => (IReadOnlyDictionary<Tile2iSlim, TerrainPropId>) this.m_tileToPropId;
    }

    public int PropsCount => this.m_props.Count;

    public int RemovedPropsCount => this.m_removedProps.Count;

    public IEvent<Tile2iAndIndex> PropChangedAt => (IEvent<Tile2iAndIndex>) this.m_propChangedAt;

    public event Action<TerrainPropData> PropRemoved;

    public event Action<TerrainPropData> PropAdded;

    public TerrainPropsManager(
      TerrainManager terrainManager,
      TerrainOccupancyManager occupancyManager,
      SurfaceDesignationsManager surfaceDesignationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_props = new Dict<TerrainPropId, TerrainPropData>();
      this.m_tileToPropId = new Dict<Tile2iSlim, TerrainPropId>();
      this.m_removedProps = new Set<TerrainPropId>();
      this.m_propChangedAt = new Event<Tile2iAndIndex>();
      this.m_tmpOccupiedTiles = new Lyst<Tile2i>(true);
      this.m_tmpOccupiedTilesForAdd = new Lyst<Tile2i>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_occupancyManager = occupancyManager;
      this.m_flagReporter = terrainManager.CreateNewTileFlagReporter("Props", false, true, false);
      terrainManager.HeightChanged.Add<TerrainPropsManager>(this, new Action<Tile2iAndIndex>(this.terrainHeightChanged));
      terrainManager.TerrainGeneratedButNotLoaded.Add<TerrainPropsManager>(this, new Action(this.terrainGeneratedButNotLoaded));
      surfaceDesignationsManager.DesignationFulfilledChanged.Add<TerrainPropsManager>(this, new Action<SurfaceDesignation>(this.surfaceDesignationFulfilledChanged));
    }

    [InitAfterLoad(InitPriority.Lowest)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      LystStruct<TerrainPropData> lystStruct = new LystStruct<TerrainPropData>();
      int num = 0;
      foreach (TerrainPropData propData in this.m_props.Values)
      {
        if (this.shouldRemoveProp(propData))
        {
          lystStruct.Add(propData);
          ++num;
        }
      }
      if (num > 0)
        Log.Warning(string.Format("Removed {0} props as height has changed too much.", (object) num));
      foreach (TerrainPropData terrainPropData in lystStruct)
      {
        if (!this.TryRemoveProp(terrainPropData.Id))
          Log.Warning(string.Format("Failed to remove {0} on game load.", (object) terrainPropData.Id));
      }
      if (saveVersion >= 140)
        return;
      resolver.Resolve<SurfaceDesignationsManager>().DesignationFulfilledChanged.Add<TerrainPropsManager>(this, new Action<SurfaceDesignation>(this.surfaceDesignationFulfilledChanged));
    }

    /// <summary>
    /// Adds prop to the terrain, removing any props that are in the way. This should be only used by
    /// <see cref="T:Mafi.Core.Terrain.Generation.ITerrainPostProcessor" /> during map generation.
    /// </summary>
    public void ForceAddProp(TerrainPropData data, Option<Lyst<TerrainPropId>> removed)
    {
      this.m_tmpOccupiedTilesForAdd.Clear();
      data.CalculateOccupiedTiles(this.m_terrainManager, this.m_tmpOccupiedTilesForAdd);
      foreach (Tile2i tile2i in this.m_tmpOccupiedTilesForAdd)
      {
        TerrainPropId id;
        if (this.m_tileToPropId.TryGetValue(tile2i.AsSlim, out id))
        {
          if (this.TryRemoveProp(id, true))
            removed.ValueOrNull?.Add(id);
          else
            Log.Error(string.Format("Failed to remove prop {0}", (object) id));
        }
      }
      if (!this.m_props.TryAdd(data.Id, data))
      {
        Log.Error(string.Format("Prop already exists with id '{0}'.", (object) data.Id));
      }
      else
      {
        foreach (Tile2i tile in this.m_tmpOccupiedTilesForAdd)
        {
          Tile2iIndex tileIndex = this.m_terrainManager.GetTileIndex(tile);
          if (!data.Proto.DoesNotBlocksVehicles)
            this.m_flagReporter.SetFlagNoEvents(tileIndex);
          if (!this.m_tileToPropId.TryAdd(tile.AsSlim, data.Id))
            Log.WarningOnce(string.Format("Prop overlap between '{0}' and '{1}'.", (object) data.Id, (object) this.m_tileToPropId[tile.AsSlim]));
        }
        Action<TerrainPropData> propAdded = this.PropAdded;
        if (propAdded == null)
          return;
        propAdded(data);
      }
    }

    /// <summary>
    /// Adds prop to the terrain. This should be only used by <see cref="T:Mafi.Core.Terrain.Generation.ITerrainPostProcessor" />
    /// during map generation.
    /// </summary>
    public bool TryAddProp(TerrainPropData data) => this.tryAddProp(data);

    private bool tryAddProp(TerrainPropData data)
    {
      this.m_tmpOccupiedTilesForAdd.Clear();
      data.CalculateOccupiedTiles(this.m_terrainManager, this.m_tmpOccupiedTilesForAdd);
      foreach (Tile2i tile in this.m_tmpOccupiedTilesForAdd)
      {
        if (this.m_tileToPropId.ContainsKey(tile.AsSlim))
        {
          if (this.m_terrainManager.IsGeneratingTerrainLegacy)
            return false;
          this.TryRemovePropAtTile(tile, true);
        }
      }
      if (!this.m_props.TryAdd(data.Id, data))
      {
        Log.Warning(string.Format("Prop already exists with id '{0}'.", (object) data.Id));
        return false;
      }
      foreach (Tile2i tile in this.m_tmpOccupiedTilesForAdd)
      {
        Tile2iIndex tileIndex = this.m_terrainManager.GetTileIndex(tile);
        if (!data.Proto.DoesNotBlocksVehicles)
          this.m_flagReporter.SetFlagNoEvents(tileIndex);
        if (!this.m_tileToPropId.TryAdd(tile.AsSlim, data.Id))
          Log.WarningOnce(string.Format("Prop overlap between '{0}' and '{1}'.", (object) data.Id, (object) this.m_tileToPropId[tile.AsSlim]));
      }
      Action<TerrainPropData> propAdded = this.PropAdded;
      if (propAdded != null)
        propAdded(data);
      return true;
    }

    private void terrainGeneratedButNotLoaded()
    {
      int num = 0;
      foreach (TerrainPropId removedProp in this.m_removedProps)
      {
        TerrainPropData propData;
        if (!this.m_props.TryRemove(removedProp, out propData))
          ++num;
        else
          this.clearPropOccupiedTiles(propData, true);
      }
      if (num <= 0)
        return;
      Log.Warning(string.Format("{0} removed props were not generated", (object) num));
    }

    public bool ContainsPropInDesignation(IDesignation designation)
    {
      TerrainTile plusYneighbor = this.m_terrainManager[designation.OriginTileCoord];
      int sizeTiles = designation.SizeTiles;
      int num1 = 0;
      while (num1 < sizeTiles)
      {
        TerrainTile terrainTile = plusYneighbor;
        int num2 = 0;
        while (num2 < sizeTiles)
        {
          if (this.m_tileToPropId.ContainsKey(terrainTile.TileCoordSlim))
            return true;
          ++num2;
          terrainTile = terrainTile.PlusXNeighbor;
        }
        ++num1;
        plusYneighbor = plusYneighbor.PlusYNeighbor;
      }
      return false;
    }

    public bool TryRemovePropAtTile(Tile2i tile, bool skipSaveRemoved = false)
    {
      return this.TryRemovePropAtTile(tile.AsSlim, skipSaveRemoved);
    }

    public bool TryRemovePropAtTile(Tile2iSlim tile, bool skipSaveRemoved = false)
    {
      TerrainPropId id;
      return this.m_tileToPropId.TryGetValue(tile, out id) && this.TryRemoveProp(id, skipSaveRemoved);
    }

    public bool TryRemoveProp(TerrainPropId id, bool skipSaveRemoved = false)
    {
      TerrainPropData propData;
      if (!this.m_props.TryRemove(id, out propData))
        return false;
      if (!skipSaveRemoved)
        this.m_removedProps.AddAndAssertNew(id);
      this.clearPropOccupiedTiles(propData);
      Action<TerrainPropData> propRemoved = this.PropRemoved;
      if (propRemoved != null)
        propRemoved(propData);
      return true;
    }

    private void surfaceDesignationFulfilledChanged(SurfaceDesignation designation)
    {
      if (!designation.IsPlacing)
        return;
      foreach (TerrainTile terrainTile in designation.AllFulfilled)
      {
        if (this.m_terrainManager.HasTileSurface(terrainTile.DataIndex))
          this.TryRemovePropAtTile(terrainTile.TileCoord, false);
      }
    }

    private void terrainHeightChanged(Tile2iAndIndex tileAndIndex)
    {
      TerrainPropId key;
      if (!this.m_tileToPropId.TryGetValue(tileAndIndex.TileCoordSlim, out key))
        return;
      TerrainPropData propData;
      if (!this.m_props.TryGetValue(key, out propData))
      {
        Log.Warning(string.Format("Not found prop data for {0} on terrain height change.", (object) key));
      }
      else
      {
        if (!this.shouldRemoveProp(propData) || this.TryRemoveProp(propData.Id))
          return;
        Log.Warning(string.Format("Failed to remove {0} on terrain height change.", (object) propData.Id));
      }
    }

    private bool shouldRemoveProp(TerrainPropData propData)
    {
      ThicknessTilesF thicknessTilesF = this.m_terrainManager.GetHeight(propData.Position) - propData.PlacedAtHeight;
      return thicknessTilesF.Value < propData.PlacementHeightOffset.Value || thicknessTilesF > propData.Proto.DespawnBuriedThreshold.ScaledBy(propData.Scale);
    }

    /// <summary>
    /// Clears the saved list of removed props. We don't need this during the editor,
    /// however we do have some runtime asserts in it to make sure it's behaving during normal gameplay.
    /// </summary>
    public void ClearRemovedPropsForTerrainEditor() => this.m_removedProps.Clear();

    /// <summary>
    /// Removes all the props on a chunk. Do not use outside of the terrain editor
    /// as it does not record those removals and so loaded saves won't have them removed.
    /// </summary>
    public void ClearPropsOnChunkForTerrainEditor(Chunk2i chunkCoord)
    {
      List<TerrainPropId> terrainPropIdList = new List<TerrainPropId>();
      foreach (TerrainPropId key in this.m_props.Keys)
      {
        if (!(key.Position.ChunkCoord2i != chunkCoord))
          terrainPropIdList.Add(key);
      }
      foreach (TerrainPropId key in terrainPropIdList)
      {
        TerrainPropData propData;
        if (!this.m_props.TryRemove(key, out propData))
        {
          Log.Warning("Failed to remove prop");
        }
        else
        {
          this.clearPropOccupiedTiles(propData);
          Action<TerrainPropData> propRemoved = this.PropRemoved;
          if (propRemoved != null)
            propRemoved(propData);
        }
      }
    }

    private void clearPropOccupiedTiles(TerrainPropData propData, bool noEvents = false)
    {
      this.m_tmpOccupiedTiles.Clear();
      propData.CalculateOccupiedTiles(this.m_terrainManager, this.m_tmpOccupiedTiles);
      foreach (Tile2i tmpOccupiedTile in this.m_tmpOccupiedTiles)
      {
        Tile2iAndIndex tileAndIndex = this.m_terrainManager.ExtendTileIndex(tmpOccupiedTile);
        if (!propData.Proto.DoesNotBlocksVehicles)
        {
          if (noEvents)
            this.m_flagReporter.ClearFlagNoEvents(tileAndIndex.Index);
          else
            this.m_flagReporter.ClearFlag(tileAndIndex);
        }
        TerrainPropId terrainPropId;
        if (!this.m_tileToPropId.TryRemove(tmpOccupiedTile.AsSlim, out terrainPropId))
          Log.Warning(string.Format("Expected prop '{0}' on '{1}'.", (object) propData.Id, (object) tmpOccupiedTile));
        else if (terrainPropId != propData.Id)
          Log.Warning(string.Format("Expected prop '{0}' on '{1}' but removed '{2}'.", (object) propData.Id, (object) tmpOccupiedTile, (object) terrainPropId));
        if (!noEvents)
          this.m_propChangedAt.Invoke(tileAndIndex);
      }
    }

    public IEnumerable<TerrainPropData> EnumeratePropsInArea(RectangleTerrainArea2i area)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<TerrainPropData>) new TerrainPropsManager.\u003CEnumeratePropsInArea\u003Ed__47(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__area = area
      };
    }

    static TerrainPropsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainPropsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainPropsManager) obj).SerializeData(writer));
      TerrainPropsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainPropsManager) obj).DeserializeData(reader));
    }
  }
}
