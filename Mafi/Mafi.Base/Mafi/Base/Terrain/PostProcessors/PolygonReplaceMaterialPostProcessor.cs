// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.PolygonReplaceMaterialPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>
  /// Post processor that replaces material within a polygon.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonReplaceMaterialPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly PolygonReplaceMaterialPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Polygon2fFast m_polygonCache;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, PolygonReplaceMaterialPostProcessor.EditData[]> m_newLayers;
    [DoNotSave(0, null)]
    private Lyst<Lyst<PolygonReplaceMaterialPostProcessor.EditData>> m_newLayersPool;

    public static void Serialize(PolygonReplaceMaterialPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonReplaceMaterialPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonReplaceMaterialPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonReplaceMaterialPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static PolygonReplaceMaterialPostProcessor Deserialize(BlobReader reader)
    {
      PolygonReplaceMaterialPostProcessor materialPostProcessor;
      if (reader.TryStartClassDeserialization<PolygonReplaceMaterialPostProcessor>(out materialPostProcessor))
        reader.EnqueueDataDeserialization((object) materialPostProcessor, PolygonReplaceMaterialPostProcessor.s_deserializeDataDelayedAction);
      return materialPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonReplaceMaterialPostProcessor>(this, "ConfigMutable", (object) PolygonReplaceMaterialPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Replace Material";

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

    public int SortingPriority
    {
      get
      {
        return (int) (this.ConfigMutable.SortingPriorityAdjustment + this.ConfigMutable.ProcessingPhase);
      }
    }

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonReplaceMaterialPostProcessor(
      PolygonReplaceMaterialPostProcessor.Configuration initialConfig)
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
      return new HandleData?(new HandleData(zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length, ColorRgba.Gray, (Option<string>) "Assets/Unity/UserInterface/Toolbar/ReplaceMaterial.svg"));
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
      return new RectangleTerrainArea2i(tile2i - this.ConfigMutable.TransitionDistance.Value, tile2iCeiled - tile2i + 2 * this.ConfigMutable.TransitionDistance.Value);
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
      return this.tryInitialize();
    }

    private bool tryInitialize()
    {
      if (this.m_polygonCache != null)
        return true;
      this.m_newLayers = new Dict<Chunk2i, PolygonReplaceMaterialPostProcessor.EditData[]>();
      this.m_newLayersPool = new Lyst<Lyst<PolygonReplaceMaterialPostProcessor.EditData>>();
      string error;
      if (this.ConfigMutable.Polygon.TryGetFastPolygon(out this.m_polygonCache, out error))
        return true;
      Log.Error("Failed to initialize polygon: " + error);
      return false;
    }

    public void Reset()
    {
      this.m_polygonCache = (Polygon2fFast) null;
      this.m_newLayers = (Dict<Chunk2i, PolygonReplaceMaterialPostProcessor.EditData[]>) null;
      this.m_newLayersPool = (Lyst<Lyst<PolygonReplaceMaterialPostProcessor.EditData>>) null;
    }

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
      lock (this.m_unaffectedChunksCache.BackingArray)
      {
        if (this.m_unaffectedChunksCache.Contains(chunk))
          return;
      }
      if (this.m_polygonCache == null)
      {
        Log.Error("Not initialized.");
      }
      else
      {
        int tileIndex = dataReadOnly.GetTileIndex(chunk.Tile2i - dataOrigin);
        int width = dataReadOnly.Width;
        int num = this.ConfigMutable.TransitionDistance.Value.Squared();
        Fix32 fix32_1 = Fix32.One / this.ConfigMutable.TransitionDistance.Value;
        ThicknessTilesF rhs = this.ConfigMutable.MaxReplacedThickness.IsPositive ? this.ConfigMutable.MaxReplacedThickness : ThicknessTilesF.MaxValue;
        Lyst<PolygonReplaceMaterialPostProcessor.EditData> lyst = (Lyst<PolygonReplaceMaterialPostProcessor.EditData>) null;
        lock (this.m_newLayersPool)
        {
          if (this.m_newLayersPool.IsNotEmpty)
            lyst = this.m_newLayersPool.PopLast();
        }
        if (lyst == null)
          lyst = new Lyst<PolygonReplaceMaterialPostProcessor.EditData>(true);
        TerrainMaterialSlimId slimId = this.ConfigMutable.OldMaterial.SlimId;
        int y = 0;
        while (y < 64)
        {
          for (int x = 0; x < 64; ++x)
          {
            int index = tileIndex + x;
            TileMaterialLayers materialLayer = dataReadOnly.MaterialLayers[index];
            if (materialLayer.Count != 0)
            {
              Vector2f vector2f = (chunk.Tile2i + new RelTile2i(x, y)).Vector2f;
              Fix32 fix32_2;
              if (this.m_polygonCache.Contains(vector2f))
              {
                fix32_2 = Fix32.One;
              }
              else
              {
                Fix64 fix64 = this.m_polygonCache.DistanceSqrTo(vector2f);
                if (!(fix64 > num))
                {
                  Fix32 fix32_3 = fix64.SqrtToFix32() * fix32_1;
                  fix32_2 = Fix32.One - fix32_3;
                }
                else
                  continue;
              }
              ThicknessTilesF thicknessL1 = ThicknessTilesF.Zero;
              ThicknessTilesF thicknessL2 = ThicknessTilesF.Zero;
              ThicknessTilesF thicknessL3 = ThicknessTilesF.Zero;
              ThicknessTilesF thicknessL4 = ThicknessTilesF.Zero;
              if (materialLayer.First.SlimId == slimId)
                thicknessL1 = materialLayer.First.Thickness.Min(rhs) * fix32_2;
              if (materialLayer.Count >= 2 && materialLayer.Second.SlimId == slimId)
                thicknessL2 = materialLayer.Second.Thickness.Min(rhs) * fix32_2;
              if (materialLayer.Count >= 3 && materialLayer.Third.SlimId == slimId)
                thicknessL3 = materialLayer.Third.Thickness.Min(rhs) * fix32_2;
              if (materialLayer.Count >= 4 && materialLayer.Fourth.SlimId == slimId)
                thicknessL4 = materialLayer.Fourth.Thickness.Min(rhs) * fix32_2;
              lyst.Add(new PolygonReplaceMaterialPostProcessor.EditData(new Tile2iIndex(index), thicknessL1, thicknessL2, thicknessL3, thicknessL4));
            }
          }
          ++y;
          tileIndex += width;
        }
        if (lyst.IsEmpty)
        {
          lock (this.m_unaffectedChunksCache.BackingArray)
            this.m_unaffectedChunksCache.Add(chunk);
        }
        else
        {
          lock (this.m_newLayers)
            this.m_newLayers[chunk] = lyst.ToArrayAndClear();
        }
        lock (this.m_newLayersPool)
          this.m_newLayersPool.Add(lyst);
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      PolygonReplaceMaterialPostProcessor.EditData[] editDataArray;
      if (!this.m_newLayers.TryRemove(chunk, out editDataArray))
        return;
      foreach (PolygonReplaceMaterialPostProcessor.EditData editData in editDataArray)
      {
        ref TileMaterialLayers local = ref dataRef.MaterialLayers[editData.TileIndex.Value];
        bool flag = this.ConfigMutable.NewMaterial.HasValue && this.ConfigMutable.ReplacedMaterialThicknessMult.IsPositive;
        if (editData.ThicknessL4.IsPositive)
        {
          ThicknessTilesF thicknessTilesF;
          if (editData.ThicknessL4 >= local.Fourth.Thickness)
          {
            thicknessTilesF = local.Fourth.Thickness;
            dataRef.RemoveFourthLayer_noChecks(ref local);
          }
          else
          {
            thicknessTilesF = editData.ThicknessL4;
            local.Fourth -= thicknessTilesF;
          }
          if (flag)
            dataRef.PushNewFourthLayer(ref local, new TerrainMaterialThicknessSlim(this.ConfigMutable.NewMaterial.Value, thicknessTilesF.ScaledBy(this.ConfigMutable.ReplacedMaterialThicknessMult)));
        }
        if (editData.ThicknessL3.IsPositive)
        {
          ThicknessTilesF thicknessTilesF;
          if (editData.ThicknessL3 >= local.Third.Thickness)
          {
            thicknessTilesF = local.Third.Thickness;
            dataRef.RemoveThirdLayer_noChecks(ref local);
          }
          else
          {
            thicknessTilesF = editData.ThicknessL3;
            local.Third -= thicknessTilesF;
          }
          if (flag)
            dataRef.PushNewThirdLayer(ref local, new TerrainMaterialThicknessSlim(this.ConfigMutable.NewMaterial.Value, thicknessTilesF.ScaledBy(this.ConfigMutable.ReplacedMaterialThicknessMult)));
        }
        if (editData.ThicknessL2.IsPositive)
        {
          ThicknessTilesF thicknessTilesF;
          if (editData.ThicknessL2 >= local.Second.Thickness)
          {
            thicknessTilesF = local.Second.Thickness;
            dataRef.RemoveSecondLayer_noChecks(ref local);
          }
          else
          {
            thicknessTilesF = editData.ThicknessL2;
            local.Second -= thicknessTilesF;
          }
          if (flag)
            dataRef.PushNewSecondLayer(ref local, new TerrainMaterialThicknessSlim(this.ConfigMutable.NewMaterial.Value, thicknessTilesF.ScaledBy(this.ConfigMutable.ReplacedMaterialThicknessMult)));
        }
        if (editData.ThicknessL1.IsPositive)
        {
          ThicknessTilesF thicknessTilesF;
          if (editData.ThicknessL1 >= local.First.Thickness)
          {
            thicknessTilesF = local.First.Thickness;
            dataRef.RemoveFirstLayer_noChecks(ref local);
          }
          else
          {
            thicknessTilesF = editData.ThicknessL1;
            local.First -= thicknessTilesF;
          }
          if (flag)
            dataRef.PushNewFirstLayer(ref local, new TerrainMaterialThicknessSlim(this.ConfigMutable.NewMaterial.Value, thicknessTilesF.ScaledBy(this.ConfigMutable.ReplacedMaterialThicknessMult)));
        }
      }
    }

    static PolygonReplaceMaterialPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonReplaceMaterialPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonReplaceMaterialPostProcessor) obj).SerializeData(writer));
      PolygonReplaceMaterialPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonReplaceMaterialPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        PolygonReplaceMaterialPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonReplaceMaterialPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonReplaceMaterialPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        ThicknessTilesF.Serialize(this.MaxReplacedThickness, writer);
        Option<TerrainMaterialProto>.Serialize(this.NewMaterial, writer);
        writer.WriteGeneric<TerrainMaterialProto>(this.OldMaterial);
        Polygon2fMutable.Serialize(this.Polygon, writer);
        writer.WriteInt((int) this.ProcessingPhase);
        Percent.Serialize(this.ReplacedMaterialThicknessMult, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        RelTile1i.Serialize(this.TransitionDistance, writer);
      }

      public static PolygonReplaceMaterialPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        PolygonReplaceMaterialPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonReplaceMaterialPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonReplaceMaterialPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.MaxReplacedThickness = reader.LoadedSaveVersion >= 155 ? ThicknessTilesF.Deserialize(reader) : new ThicknessTilesF();
        this.NewMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
        this.OldMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
        this.Polygon = Polygon2fMutable.Deserialize(reader);
        this.ProcessingPhase = (TerrainPostProcessorPriorityBase) reader.ReadInt();
        this.ReplacedMaterialThicknessMult = Percent.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.TransitionDistance = RelTile1i.Deserialize(reader);
      }

      [EditorEnforceOrder(41)]
      public Polygon2fMutable Polygon { get; set; }

      [EditorEnforceOrder(45)]
      [EditorRange(0.0, 100.0)]
      public RelTile1i TransitionDistance { get; set; }

      [EditorLabel(null, "Specifies material to be replaced within the area.", false, false)]
      [EditorEnforceOrder(49)]
      public TerrainMaterialProto OldMaterial { get; set; }

      [EditorEnforceOrder(54)]
      [EditorLabel(null, "Specifies an optional material to the old material in the area.If no material is selected, the old material is removed while preserving terrain height.", false, false)]
      public Option<TerrainMaterialProto> NewMaterial { get; set; }

      [EditorRange(0.0, 1000.0)]
      [EditorEnforceOrder(58)]
      [NewInSaveVersion(155, null, null, null, null)]
      public ThicknessTilesF MaxReplacedThickness { get; set; }

      [EditorRange(0.0, 2.0)]
      [EditorEnforceOrder(63)]
      public Percent ReplacedMaterialThicknessMult { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(67)]
      public int SortingPriorityAdjustment { get; set; }

      [EditorEnforceOrder(71)]
      [EditorLabel(null, "Specifies post-processing phase that determines base priority.", false, false)]
      public TerrainPostProcessorPriorityBase ProcessingPhase { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxReplacedThickness\u003Ek__BackingField = 2.0.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CReplacedMaterialThicknessMult\u003Ek__BackingField = Percent.Hundred;
        // ISSUE: reference to a compiler-generated field
        this.\u003CProcessingPhase\u003Ek__BackingField = TerrainPostProcessorPriorityBase.Last;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonReplaceMaterialPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonReplaceMaterialPostProcessor.Configuration) obj).SerializeData(writer));
        PolygonReplaceMaterialPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonReplaceMaterialPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }

    private readonly struct EditData
    {
      public readonly Tile2iIndex TileIndex;
      public readonly ThicknessTilesF ThicknessL1;
      public readonly ThicknessTilesF ThicknessL2;
      public readonly ThicknessTilesF ThicknessL3;
      public readonly ThicknessTilesF ThicknessL4;

      public EditData(
        Tile2iIndex tileIndex,
        ThicknessTilesF thicknessL1,
        ThicknessTilesF thicknessL2,
        ThicknessTilesF thicknessL3,
        ThicknessTilesF thicknessL4)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.TileIndex = tileIndex;
        this.ThicknessL1 = thicknessL1;
        this.ThicknessL2 = thicknessL2;
        this.ThicknessL3 = thicknessL3;
        this.ThicknessL4 = thicknessL4;
      }
    }
  }
}
