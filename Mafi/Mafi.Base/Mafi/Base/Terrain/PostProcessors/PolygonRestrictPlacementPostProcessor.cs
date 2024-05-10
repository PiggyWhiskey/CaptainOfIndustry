// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.PolygonRestrictPlacementPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>
  /// Post processor that clears props and trees within a polygon.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonRestrictPlacementPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly PolygonRestrictPlacementPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Polygon2fFast m_polygonCache;
    [DoNotSave(0, null)]
    private GeneratedPropsData m_propsData;
    [DoNotSave(0, null)]
    private GeneratedTreesData m_treesData;

    public static void Serialize(PolygonRestrictPlacementPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonRestrictPlacementPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonRestrictPlacementPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonRestrictPlacementPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static PolygonRestrictPlacementPostProcessor Deserialize(BlobReader reader)
    {
      PolygonRestrictPlacementPostProcessor placementPostProcessor;
      if (reader.TryStartClassDeserialization<PolygonRestrictPlacementPostProcessor>(out placementPostProcessor))
        reader.EnqueueDataDeserialization((object) placementPostProcessor, PolygonRestrictPlacementPostProcessor.s_deserializeDataDelayedAction);
      return placementPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonRestrictPlacementPostProcessor>(this, "ConfigMutable", (object) PolygonRestrictPlacementPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Restrict Placement";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public bool Is2D => true;

    public bool CanRotate => true;

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.AnalyzeAllThenApply;
    }

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 200;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonRestrictPlacementPostProcessor(
      PolygonRestrictPlacementPostProcessor.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
    }

    public HandleData? GetHandleData()
    {
      Tile2f zero = Tile2f.Zero;
      foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile2f(vertex);
      return new HandleData?(new HandleData(zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length, ColorRgba.Gray, (Option<string>) "Assets/Unity/UserInterface/General/NoTreeCut.svg"));
    }

    RectangleTerrainArea2i? ITerrainFeatureBase.GetBoundingBox()
    {
      return new RectangleTerrainArea2i?(this.GetBoundingBox());
    }

    public RectangleTerrainArea2i GetBoundingBox()
    {
      Vector2f min;
      Vector2f max;
      this.ConfigMutable.Polygon.ComputeBounds(out min, out max);
      Tile2i tile2i = new Tile2f(min).Tile2i;
      Tile2i tile2iCeiled = new Tile2f(max).Tile2iCeiled;
      return new RectangleTerrainArea2i(tile2i, tile2iCeiled - tile2i);
    }

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors) => true;

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      if (!extraDataReg.TryGetOrCreateExtraData<GeneratedTreesData>(out this.m_treesData))
      {
        Log.Error("Failed to obtain trees data.");
        return false;
      }
      if (extraDataReg.TryGetOrCreateExtraData<GeneratedPropsData>(out this.m_propsData))
        return this.tryInitialize();
      Log.Error("Failed to obtain props data.");
      return false;
    }

    private bool tryInitialize()
    {
      string error;
      if (this.m_polygonCache != null || this.ConfigMutable.Polygon.TryGetFastPolygon(out this.m_polygonCache, out error))
        return true;
      Log.Error("Failed to initialize polygon: " + error);
      return false;
    }

    public void Reset() => this.m_polygonCache = (Polygon2fFast) null;

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.ConfigMutable.Polygon.TranslateBy(delta.Xy.Vector2f);
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
      this.Reset();
      this.ConfigMutable.Polygon.RotateBy(rotation.Degrees);
    }

    public void AnalyzeChunkThreadSafe(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly,
      int pass)
    {
      if (this.m_polygonCache == null)
      {
        Log.Error("Not initialized.");
      }
      else
      {
        lock (this.m_unaffectedChunksCache.BackingArray)
        {
          if (this.m_unaffectedChunksCache.Contains(chunk))
            return;
        }
        Rect2i rect;
        ref Rect2i local = ref rect;
        Tile2i tile2i = chunk.Tile2i;
        Vector2i vector2i1 = tile2i.Vector2i;
        tile2i = chunk.Tile2i + new RelTile2i(64, 64);
        Vector2i vector2i2 = tile2i.Vector2i;
        local = new Rect2i(vector2i1, vector2i2);
        if (!this.m_polygonCache.Overlaps(rect))
        {
          lock (this.m_unaffectedChunksCache.BackingArray)
            this.m_unaffectedChunksCache.Add(chunk);
        }
        else
        {
          if (this.ConfigMutable.ClearTrees)
            this.m_treesData.GetOrCreateChunk(chunk).ClearedPolygons.Add(this.m_polygonCache);
          if (!this.ConfigMutable.ClearProps)
            return;
          this.m_propsData.GetOrCreateChunk(chunk).ClearedPolygons.Add(this.m_polygonCache);
        }
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
    }

    static PolygonRestrictPlacementPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonRestrictPlacementPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonRestrictPlacementPostProcessor) obj).SerializeData(writer));
      PolygonRestrictPlacementPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonRestrictPlacementPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        PolygonRestrictPlacementPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonRestrictPlacementPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonRestrictPlacementPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteBool(this.ClearProps);
        writer.WriteBool(this.ClearTrees);
        Polygon2fMutable.Serialize(this.Polygon, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
      }

      public static PolygonRestrictPlacementPostProcessor.Configuration Deserialize(
        BlobReader reader)
      {
        PolygonRestrictPlacementPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonRestrictPlacementPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonRestrictPlacementPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.ClearProps = reader.ReadBool();
        this.ClearTrees = reader.ReadBool();
        this.Polygon = Polygon2fMutable.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
      }

      [EditorEnforceOrder(37)]
      public Polygon2fMutable Polygon { get; set; }

      [EditorEnforceOrder(40)]
      public bool ClearTrees { get; set; }

      [EditorEnforceOrder(43)]
      public bool ClearProps { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(47)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CClearTrees\u003Ek__BackingField = true;
        // ISSUE: reference to a compiler-generated field
        this.\u003CClearProps\u003Ek__BackingField = true;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonRestrictPlacementPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonRestrictPlacementPostProcessor.Configuration) obj).SerializeData(writer));
        PolygonRestrictPlacementPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonRestrictPlacementPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
