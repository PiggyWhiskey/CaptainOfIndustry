// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.TerrainUnderPropsPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Props;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>
  /// Grows grass on rocks as well as spreads incompatible materials such as sand.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class TerrainUnderPropsPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_LAYER_THICKNESS;
    public readonly TerrainUnderPropsPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, Dict<int, TerrainMaterialThicknessSlim>> m_newFloorThickness;
    [DoNotSave(0, null)]
    private GeneratedPropsData m_propsData;

    public static void Serialize(TerrainUnderPropsPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainUnderPropsPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainUnderPropsPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      TerrainUnderPropsPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static TerrainUnderPropsPostProcessor Deserialize(BlobReader reader)
    {
      TerrainUnderPropsPostProcessor propsPostProcessor;
      if (reader.TryStartClassDeserialization<TerrainUnderPropsPostProcessor>(out propsPostProcessor))
        reader.EnqueueDataDeserialization((object) propsPostProcessor, TerrainUnderPropsPostProcessor.s_deserializeDataDelayedAction);
      return propsPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainUnderPropsPostProcessor>(this, "ConfigMutable", (object) TerrainUnderPropsPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Terrain under props";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => true;

    public bool IsImportable => false;

    public bool Is2D => true;

    public bool CanRotate => false;

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.AnalyzeInterleaveAndApply;
    }

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 4000 + 500;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public TerrainUnderPropsPostProcessor(
      TerrainUnderPropsPostProcessor.Configuration configMutable)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = configMutable;
    }

    public HandleData? GetHandleData() => new HandleData?();

    public RectangleTerrainArea2i? GetBoundingBox() => new RectangleTerrainArea2i?();

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors) => true;

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      if (!extraDataReg.TryGetOrCreateExtraData<GeneratedPropsData>(out this.m_propsData))
      {
        Log.Error("Failed to obtain trees data.");
        return false;
      }
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      this.m_newFloorThickness = new Dict<Chunk2i, Dict<int, TerrainMaterialThicknessSlim>>();
      return true;
    }

    public void Reset()
    {
      this.m_newFloorThickness = (Dict<Chunk2i, Dict<int, TerrainMaterialThicknessSlim>>) null;
      this.m_propsData = (GeneratedPropsData) null;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
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
      int width = dataReadOnly.Width;
      Dict<int, TerrainMaterialThicknessSlim> dict = new Dict<int, TerrainMaterialThicknessSlim>();
      for (int addedY = -1; addedY <= 1; ++addedY)
      {
        for (int addedX = -1; addedX <= 1; ++addedX)
        {
          Chunk2i chunk1 = chunk.AddX(addedX).AddY(addedY);
          if ((long) (uint) (chunk1.X * 64 - dataOrigin.X) < (long) dataReadOnly.Width && (long) (uint) (chunk1.Y * 64 - dataOrigin.Y) < (long) dataReadOnly.Height)
          {
            foreach (KeyValuePair<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>> andSurfaceMaterial in this.m_propsData.GetOrCreateChunk(chunk1).PropsAndSurfaceMaterials)
            {
              TerrainPropId key1 = andSurfaceMaterial.Key;
              TerrainPropData first = andSurfaceMaterial.Value.First;
              TerrainMaterialProto second = andSurfaceMaterial.Value.Second;
              Tile2i asFull = key1.Position.AsFull;
              Fix32 fix32_1 = first.Proto.Extents.MaxComponent();
              fix32_1 = fix32_1.ScaledBy(first.Scale);
              int num = fix32_1.ToIntFloored() + 1;
              if (asFull.X + num >= chunk.Tile2i.X && asFull.X - num < chunk.Tile2i.X + 64 && asFull.Y + num >= chunk.Tile2i.Y && asFull.Y - num < chunk.Tile2i.Y + 64)
              {
                int tileIndex = dataReadOnly.GetTileIndex(asFull, dataOrigin);
                Fix32 fix32_2 = (Fix32) (num * num);
                for (int index1 = -num; index1 <= num; ++index1)
                {
                  if (asFull.Y + index1 >= chunk.Tile2i.Y)
                  {
                    if (asFull.Y + index1 < chunk.Tile2i.Y + 64)
                    {
                      fix32_1 = first.PositionWithinTile.Y - (Fix32) index1;
                      Fix32 fix32_3 = fix32_1.SquaredAsFix32();
                      for (int index2 = -num; index2 <= num; ++index2)
                      {
                        if (asFull.X + index2 >= chunk.Tile2i.X)
                        {
                          if (asFull.X + index2 < chunk.Tile2i.X + 64)
                          {
                            fix32_1 = first.PositionWithinTile.X - (Fix32) index2;
                            Fix32 fix32_4 = fix32_1.SquaredAsFix32() + fix32_3;
                            if (!(fix32_4 > fix32_2))
                            {
                              Fix32 fix32_5 = Fix32.One - fix32_4.Sqrt() / num;
                              if (!(fix32_5 <= TerrainUnderPropsPostProcessor.MIN_LAYER_THICKNESS.Value))
                              {
                                int key2 = tileIndex + index2 + index1 * width;
                                TerrainMaterialThicknessSlim materialThicknessSlim;
                                dict[key2] = !dict.TryGetValue(key2, out materialThicknessSlim) ? new TerrainMaterialThicknessSlim(second, new ThicknessTilesF(fix32_5)) : materialThicknessSlim + new ThicknessTilesF(fix32_5);
                              }
                            }
                          }
                          else
                            break;
                        }
                      }
                    }
                    else
                      break;
                  }
                }
              }
            }
          }
        }
      }
      if (dict.IsEmpty)
      {
        lock (this.m_unaffectedChunksCache.BackingArray)
          this.m_unaffectedChunksCache.Add(chunk);
      }
      else
      {
        lock (this.m_newFloorThickness)
          this.m_newFloorThickness[chunk] = dict;
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      Dict<int, TerrainMaterialThicknessSlim> dict;
      if (!this.m_newFloorThickness.TryRemove(chunk, out dict))
        return;
      foreach (KeyValuePair<int, TerrainMaterialThicknessSlim> keyValuePair in dict)
        dataRef.AppendOrPushFirstLayer(new Tile2iIndex(keyValuePair.Key), keyValuePair.Value);
    }

    static TerrainUnderPropsPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TerrainUnderPropsPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainUnderPropsPostProcessor) obj).SerializeData(writer));
      TerrainUnderPropsPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainUnderPropsPostProcessor) obj).DeserializeData(reader));
      TerrainUnderPropsPostProcessor.MIN_LAYER_THICKNESS = 0.1.TilesThick();
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        TerrainUnderPropsPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TerrainUnderPropsPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TerrainUnderPropsPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteInt(this.SortingPriorityAdjustment);
      }

      public static TerrainUnderPropsPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        TerrainUnderPropsPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<TerrainUnderPropsPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, TerrainUnderPropsPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.SortingPriorityAdjustment = reader.ReadInt();
      }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        TerrainUnderPropsPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainUnderPropsPostProcessor.Configuration) obj).SerializeData(writer));
        TerrainUnderPropsPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainUnderPropsPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
