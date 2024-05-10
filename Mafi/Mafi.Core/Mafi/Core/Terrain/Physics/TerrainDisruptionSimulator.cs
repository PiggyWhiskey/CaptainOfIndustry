// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Physics.TerrainDisruptionSimulator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Physics
{
  [GenerateSerializer(false, null, 0)]
  internal class TerrainDisruptionSimulator : ITerrainDisruptionSimulator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// Number of ticks between checking surface disruption of each tile.
    /// </summary>
    public const int TILE_UPDATE_RATE = 64;
    /// <summary>Circular buffer for tiles to process.</summary>
    private readonly LystStruct<Tile2iAndIndex>[] m_updatesQueue;
    /// <summary>
    /// Index into <see cref="F:Mafi.Core.Terrain.Physics.TerrainDisruptionSimulator.m_updatesQueue" /> of currently processed queue.
    /// </summary>
    private int m_currentQueueIndex;
    /// <summary>
    /// Index of a next element to be processed in the current queue.
    /// </summary>
    private int m_currentQueueElementIndex;
    [DoNotSave(0, null)]
    private int m_tilesInQueueCount;
    [DoNotSave(0, null)]
    private ImmutableArray<ThicknessTilesF> m_recoveryPerMaterial;
    [DoNotSave(0, null)]
    private ImmutableArray<ThicknessTilesF> m_recoveryPerMaterialUnderWater;
    private TerrainManager m_terrainManager;
    [DoNotSave(0, null)]
    private BitMap m_isTileInQueue;
    private Option<Event<MaterialConversionResult>>[] m_disruptionRecoveryCallbacks;
    public static readonly ThicknessTilesF MAX_RECOVERY_DEPTH;
    public static readonly ThicknessTilesF MAX_THICKNESS_TO_MERGE;

    public static void Serialize(TerrainDisruptionSimulator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainDisruptionSimulator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainDisruptionSimulator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsDisabled);
      writer.WriteInt(this.m_currentQueueElementIndex);
      writer.WriteInt(this.m_currentQueueIndex);
      writer.WriteArray<Option<Event<MaterialConversionResult>>>(this.m_disruptionRecoveryCallbacks);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      writer.WriteArray<LystStruct<Tile2iAndIndex>>(this.m_updatesQueue);
    }

    public static TerrainDisruptionSimulator Deserialize(BlobReader reader)
    {
      TerrainDisruptionSimulator disruptionSimulator;
      if (reader.TryStartClassDeserialization<TerrainDisruptionSimulator>(out disruptionSimulator))
        reader.EnqueueDataDeserialization((object) disruptionSimulator, TerrainDisruptionSimulator.s_deserializeDataDelayedAction);
      return disruptionSimulator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsDisabled = reader.LoadedSaveVersion >= 140 && reader.ReadBool();
      this.m_currentQueueElementIndex = reader.ReadInt();
      this.m_currentQueueIndex = reader.ReadInt();
      this.m_disruptionRecoveryCallbacks = reader.ReadArray<Option<Event<MaterialConversionResult>>>();
      this.m_terrainManager = TerrainManager.Deserialize(reader);
      reader.SetField<TerrainDisruptionSimulator>(this, "m_updatesQueue", (object) reader.ReadArray<LystStruct<Tile2iAndIndex>>());
      reader.RegisterInitAfterLoad<TerrainDisruptionSimulator>(this, "initSelf", InitPriority.Normal);
    }

    [NewInSaveVersion(140, null, null, null, null)]
    public bool IsDisabled { get; private set; }

    public bool IsProcessingTiles => this.m_tilesInQueueCount > 0;

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      if (this.m_terrainManager.Bedrock.RecoveredMaterialProto.HasValue)
        Log.Warning(string.Format("Recovery of bedrock material '{0}' is not supported, ", (object) this.m_terrainManager.Bedrock) + "please don't make bedrock recoverable!");
      this.m_recoveryPerMaterial = this.m_terrainManager.TerrainMaterials.Map<ThicknessTilesF>((Func<TerrainMaterialProto, ThicknessTilesF>) (x => !x.DisruptionRecoveryTime.IsPositive ? ThicknessTilesF.Zero : (ThicknessTilesF.One * 64 / x.DisruptionRecoveryTime.Ticks).Max(ThicknessTilesF.Epsilon)));
      if (this.m_recoveryPerMaterial[0].IsNotZero)
      {
        Log.Warning("Phantom material should not have any recovery.");
        this.m_recoveryPerMaterial = this.m_recoveryPerMaterial.SetItem(0, ThicknessTilesF.Zero);
      }
      this.m_recoveryPerMaterialUnderWater = this.m_terrainManager.TerrainMaterials.Map<ThicknessTilesF>((Func<TerrainMaterialProto, int, ThicknessTilesF>) ((x, i) => !x.RecoversUnderWater ? ThicknessTilesF.Zero : this.m_recoveryPerMaterial[i]));
      this.m_tilesInQueueCount = 0;
      this.m_isTileInQueue = new BitMap(this.m_terrainManager.TerrainTilesCount);
      foreach (LystStruct<Tile2iAndIndex> updates in this.m_updatesQueue)
      {
        foreach (Tile2iAndIndex tile2iAndIndex in updates)
        {
          this.m_isTileInQueue.SetBit(tile2iAndIndex.IndexRaw);
          ++this.m_tilesInQueueCount;
        }
      }
      int length = this.m_terrainManager.RecoveredMaterialIds.Length;
      if (this.m_disruptionRecoveryCallbacks == null)
      {
        this.m_disruptionRecoveryCallbacks = new Option<Event<MaterialConversionResult>>[length];
      }
      else
      {
        if (this.m_disruptionRecoveryCallbacks.Length == length)
          return;
        Array.Resize<Option<Event<MaterialConversionResult>>>(ref this.m_disruptionRecoveryCallbacks, length);
      }
    }

    public void Initialize(TerrainManager terrainManager)
    {
      this.m_terrainManager = terrainManager;
      this.initSelf();
      terrainManager.HeightChanged.Add<TerrainDisruptionSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
      terrainManager.TileMaterialsOnlyChanged.Add<TerrainDisruptionSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
    }

    public void SetDisabled(bool isDisabled)
    {
      if (this.m_terrainManager == null)
      {
        Log.Error("Not initialized yet.");
      }
      else
      {
        if (isDisabled == this.IsDisabled)
          return;
        this.IsDisabled = isDisabled;
        if (isDisabled)
        {
          this.m_terrainManager.HeightChanged.Remove<TerrainDisruptionSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
          this.m_terrainManager.TileMaterialsOnlyChanged.Remove<TerrainDisruptionSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
        }
        else
        {
          this.m_terrainManager.HeightChanged.Add<TerrainDisruptionSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
          this.m_terrainManager.TileMaterialsOnlyChanged.Add<TerrainDisruptionSimulator>(this, new Action<Tile2iAndIndex>(this.tryQueueNewTile));
        }
      }
    }

    public Event<MaterialConversionResult> GetMaterialRecoveryEvent(TerrainMaterialProto material)
    {
      int index = (int) material.SlimId.Value;
      Event<MaterialConversionResult> materialRecoveryEvent = this.m_disruptionRecoveryCallbacks[index].ValueOrNull;
      if (materialRecoveryEvent == null)
        this.m_disruptionRecoveryCallbacks[index] = (Option<Event<MaterialConversionResult>>) (materialRecoveryEvent = new Event<MaterialConversionResult>());
      return materialRecoveryEvent;
    }

    public int GetQueueSize() => this.m_tilesInQueueCount;

    private void tryQueueNewTile(Tile2iAndIndex tile)
    {
      if (!this.m_isTileInQueue.SetBitReportChanged(tile.IndexRaw))
        return;
      ++this.m_tilesInQueueCount;
      this.m_updatesQueue[(this.m_currentQueueIndex - 1 - (this.m_tilesInQueueCount & 7) + 64) % 64].Add(tile);
    }

    public void Update()
    {
      LystStruct<Tile2iAndIndex> updates = this.m_updatesQueue[this.m_currentQueueIndex];
      Tile2iAndIndex[] backingArray = updates.GetBackingArray();
      int count = updates.Count;
      int self = this.m_currentQueueElementIndex + 2000;
      bool flag;
      if (self >= count)
      {
        flag = true;
        self = self.Min(count);
      }
      else
        flag = false;
      for (int queueElementIndex = this.m_currentQueueElementIndex; queueElementIndex < self; ++queueElementIndex)
      {
        Tile2iAndIndex tileAndIndex = backingArray[queueElementIndex];
        this.m_isTileInQueue.ClearBit(tileAndIndex.IndexRaw);
        --this.m_tilesInQueueCount;
        this.RecoverTile(tileAndIndex);
      }
      if (flag)
      {
        this.m_updatesQueue[this.m_currentQueueIndex].ClearSkipZeroingMemory();
        this.m_currentQueueIndex = (this.m_currentQueueIndex + 1) % 64;
        this.m_currentQueueElementIndex = 0;
      }
      else
        this.m_currentQueueElementIndex = self;
    }

    public void RecoverTile(Tile2iAndIndex tileAndIndex)
    {
      TileMaterialLayers tileMaterialLayers = this.m_terrainManager.TileLayersData[tileAndIndex.IndexRaw];
      ImmutableArray<ThicknessTilesF> immutableArray = this.m_terrainManager.IsOcean(tileAndIndex.Index) ? this.m_recoveryPerMaterialUnderWater : this.m_recoveryPerMaterial;
      ThicknessTilesF maxThickness = immutableArray[(int) tileMaterialLayers.First.SlimId.Value];
      if (maxThickness.IsPositive)
      {
        ThicknessTilesF thicknessTilesF = this.m_terrainManager.ConvertMaterialInFirstLayer(tileAndIndex, this.m_terrainManager.RecoveredMaterialIds[(int) tileMaterialLayers.First.SlimId.Value], maxThickness, TerrainDisruptionSimulator.MAX_THICKNESS_TO_MERGE);
        this.m_disruptionRecoveryCallbacks[tileMaterialLayers.First.SlimIdRaw].ValueOrNull?.Invoke(new MaterialConversionResult(tileAndIndex, tileMaterialLayers.First.WithThickness(maxThickness - thicknessTilesF)));
      }
      else
      {
        if (tileMaterialLayers.Count <= 1 || tileMaterialLayers.First.Thickness >= TerrainDisruptionSimulator.MAX_RECOVERY_DEPTH)
          return;
        maxThickness = immutableArray[(int) tileMaterialLayers.Second.SlimId.Value];
        if (maxThickness.IsPositive)
        {
          ThicknessTilesF thicknessTilesF = this.m_terrainManager.ConvertMaterialInSecondLayer(tileAndIndex, this.m_terrainManager.RecoveredMaterialIds[(int) tileMaterialLayers.Second.SlimId.Value], maxThickness, TerrainDisruptionSimulator.MAX_THICKNESS_TO_MERGE);
          this.m_disruptionRecoveryCallbacks[tileMaterialLayers.Second.SlimIdRaw].ValueOrNull?.Invoke(new MaterialConversionResult(tileAndIndex, tileMaterialLayers.Second.WithThickness(maxThickness - thicknessTilesF)));
        }
        else
        {
          if (tileMaterialLayers.Count == 2 || tileMaterialLayers.First.Thickness + tileMaterialLayers.Second.Thickness >= TerrainDisruptionSimulator.MAX_RECOVERY_DEPTH)
            return;
          maxThickness = immutableArray[(int) tileMaterialLayers.Third.SlimId.Value];
          if (maxThickness.IsPositive)
          {
            ThicknessTilesF thicknessTilesF = this.m_terrainManager.ConvertMaterialInThirdLayer(tileAndIndex, this.m_terrainManager.RecoveredMaterialIds[(int) tileMaterialLayers.Third.SlimId.Value], maxThickness, TerrainDisruptionSimulator.MAX_THICKNESS_TO_MERGE);
            this.m_disruptionRecoveryCallbacks[tileMaterialLayers.Third.SlimIdRaw].ValueOrNull?.Invoke(new MaterialConversionResult(tileAndIndex, tileMaterialLayers.Third.WithThickness(maxThickness - thicknessTilesF)));
          }
          else
          {
            if (tileMaterialLayers.Count == 3 || tileMaterialLayers.First.Thickness + tileMaterialLayers.Second.Thickness + tileMaterialLayers.Third.Thickness >= TerrainDisruptionSimulator.MAX_RECOVERY_DEPTH)
              return;
            maxThickness = immutableArray[(int) tileMaterialLayers.Fourth.SlimId.Value];
            if (!maxThickness.IsPositive)
              return;
            ThicknessTilesF thicknessTilesF = this.m_terrainManager.ConvertMaterialInFourthLayer(tileAndIndex, this.m_terrainManager.RecoveredMaterialIds[(int) tileMaterialLayers.Fourth.SlimId.Value], maxThickness);
            this.m_disruptionRecoveryCallbacks[tileMaterialLayers.Fourth.SlimIdRaw].ValueOrNull?.Invoke(new MaterialConversionResult(tileAndIndex, tileMaterialLayers.Fourth.WithThickness(maxThickness - thicknessTilesF)));
          }
        }
      }
    }

    public void Clear()
    {
      this.m_tilesInQueueCount = 0;
      for (int index = 0; index < this.m_updatesQueue.Length; ++index)
      {
        foreach (Tile2iAndIndex tile2iAndIndex in this.m_updatesQueue[index])
          this.m_isTileInQueue.ClearBitsAround(tile2iAndIndex.IndexRaw);
        this.m_updatesQueue[index].ClearSkipZeroingMemory();
      }
    }

    public TerrainDisruptionSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_updatesQueue = new LystStruct<Tile2iAndIndex>[64];
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TerrainDisruptionSimulator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainDisruptionSimulator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainDisruptionSimulator) obj).SerializeData(writer));
      TerrainDisruptionSimulator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainDisruptionSimulator) obj).DeserializeData(reader));
      TerrainDisruptionSimulator.MAX_RECOVERY_DEPTH = TerrainManager.MAX_DISRUPTION_DEPTH;
      TerrainDisruptionSimulator.MAX_THICKNESS_TO_MERGE = 2 * TerrainDisruptionSimulator.MAX_RECOVERY_DEPTH;
    }
  }
}
