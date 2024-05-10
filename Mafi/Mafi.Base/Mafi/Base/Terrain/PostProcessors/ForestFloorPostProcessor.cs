// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.ForestFloorPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Trees;
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
  public class ForestFloorPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ForestFloorPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, Dict<int, ThicknessTilesF>> m_newFloorThickness;
    [DoNotSave(0, null)]
    private GeneratedTreesData m_treesData;

    public static void Serialize(ForestFloorPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ForestFloorPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ForestFloorPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ForestFloorPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static ForestFloorPostProcessor Deserialize(BlobReader reader)
    {
      ForestFloorPostProcessor floorPostProcessor;
      if (reader.TryStartClassDeserialization<ForestFloorPostProcessor>(out floorPostProcessor))
        reader.EnqueueDataDeserialization((object) floorPostProcessor, ForestFloorPostProcessor.s_deserializeDataDelayedAction);
      return floorPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ForestFloorPostProcessor>(this, "ConfigMutable", (object) ForestFloorPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Forest floor";

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

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 2000 + 500;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public ForestFloorPostProcessor(
      ForestFloorPostProcessor.Configuration configMutable)
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
      if (!extraDataReg.TryGetOrCreateExtraData<GeneratedTreesData>(out this.m_treesData))
      {
        Log.Error("Failed to obtain trees data.");
        return false;
      }
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      this.m_newFloorThickness = new Dict<Chunk2i, Dict<int, ThicknessTilesF>>();
      return true;
    }

    public void Reset()
    {
      this.m_newFloorThickness = (Dict<Chunk2i, Dict<int, ThicknessTilesF>>) null;
      this.m_treesData = (GeneratedTreesData) null;
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
      TerrainMaterialSlimId slimId = this.ConfigMutable.ForestFloorMaterialProto.SlimId;
      ushort[] flags = dataReadOnly.Flags;
      Dict<int, ThicknessTilesF> dict = new Dict<int, ThicknessTilesF>();
      for (int addedY = -1; addedY <= 1; ++addedY)
      {
        for (int addedX = -1; addedX <= 1; ++addedX)
        {
          Chunk2i chunk1 = chunk.AddX(addedX).AddY(addedY);
          if ((long) (uint) (chunk1.X * 64 - dataOrigin.X) < (long) dataReadOnly.Width && (long) (uint) (chunk1.Y * 64 - dataOrigin.Y) < (long) dataReadOnly.Height)
          {
            foreach (KeyValuePair<TreeId, TreeDataBase> tree in this.m_treesData.GetOrCreateChunk(chunk1).Trees)
            {
              TreeId key = tree.Key;
              TreeDataBase treeDataBase = tree.Value;
              if (!treeDataBase.Proto.ForestFloorMaterial.IsNone)
              {
                Tile2i position = (Tile2i) key.Position;
                RelTile2i tile1 = position - dataOrigin;
                dataReadOnly.GetTileIndex(tile1);
                if ((long) (uint) tile1.X < (long) dataReadOnly.Width && (long) (uint) tile1.Y < (long) dataReadOnly.Height)
                {
                  TreeProto proto = treeDataBase.Proto;
                  int num = proto.MaxForestFloorRadius.Value;
                  if (addedX == 0 && addedY == 0 || (int) key.Position.X >= chunk.X - num || (int) key.Position.X < chunk.X + 64 + num || (int) key.Position.Y >= chunk.Y - num || (int) key.Position.Y < chunk.Y + 64 + num)
                  {
                    Fix32 fix32_1 = (Fix32) proto.MinForestFloorRadius.Value;
                    Fix32 denominator = (Fix32) num - fix32_1;
                    Fix32 fix32_2 = fix32_1 * fix32_1;
                    Fix32 fix32_3 = (Fix32) (num * num);
                    for (int index1 = -num + 1; index1 <= num; ++index1)
                    {
                      int y = position.Y + index1;
                      if (y >> 6 == chunk.Y)
                      {
                        Fix32 fix32_4 = treeDataBase.Position.Y - (Fix32) key.Position.Y - (Fix32) index1;
                        Fix32 fix32_5 = fix32_4 * fix32_4;
                        for (int index2 = -num + 1; index2 <= num; ++index2)
                        {
                          int x = position.X + index2;
                          if (x >> 6 == chunk.X)
                          {
                            Tile2i tile2 = new Tile2i(x, y);
                            int tileIndex = dataReadOnly.GetTileIndex(tile2, dataOrigin);
                            if (((int) flags[tileIndex] & 5) == 0)
                            {
                              Fix32 fix32_6 = treeDataBase.Position.X - (Fix32) key.Position.X - (Fix32) index2;
                              Fix32 fix32_7 = fix32_6 * fix32_6 + fix32_5;
                              if (!(fix32_7 > fix32_3))
                              {
                                ThicknessTilesF thicknessTilesF1 = this.ConfigMutable.MaxFloorThicknessFromOneTree;
                                if (fix32_7 > fix32_2)
                                {
                                  Percent scale = Percent.Hundred - Percent.FromRatio(fix32_7.Sqrt() - fix32_1, denominator);
                                  thicknessTilesF1 = thicknessTilesF1.ScaledBy(scale);
                                  if (thicknessTilesF1 < this.ConfigMutable.MinFloorThicknessFromOneTree)
                                    continue;
                                }
                                ThicknessTilesF thicknessTilesF2;
                                dict[tileIndex] = !dict.TryGetValue(tileIndex, out thicknessTilesF2) ? thicknessTilesF1.Min(TreesManager.MAX_FLOOR_THICKNESS_TOTAL) : (thicknessTilesF2 + thicknessTilesF1).Min(TreesManager.MAX_FLOOR_THICKNESS_TOTAL);
                              }
                            }
                          }
                        }
                      }
                    }
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
      Dict<int, ThicknessTilesF> dict;
      if (!this.m_newFloorThickness.TryRemove(chunk, out dict))
        return;
      TerrainMaterialSlimId slimId = this.ConfigMutable.ForestFloorMaterialProto.SlimId;
      foreach (KeyValuePair<int, ThicknessTilesF> keyValuePair in dict)
        dataRef.AppendOrPushFirstLayer(new Tile2iIndex(keyValuePair.Key), keyValuePair.Value.Of(slimId));
    }

    static ForestFloorPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ForestFloorPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ForestFloorPostProcessor) obj).SerializeData(writer));
      ForestFloorPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ForestFloorPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(ForestFloorPostProcessor.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ForestFloorPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ForestFloorPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<TerrainMaterialProto>(this.ForestFloorMaterialProto);
        ThicknessTilesF.Serialize(this.MaxFloorThicknessFromOneTree, writer);
        ThicknessTilesF.Serialize(this.MinFloorThicknessFromOneTree, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
      }

      public static ForestFloorPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        ForestFloorPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<ForestFloorPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, ForestFloorPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.ForestFloorMaterialProto = reader.ReadGenericAs<TerrainMaterialProto>();
        this.MaxFloorThicknessFromOneTree = ThicknessTilesF.Deserialize(reader);
        this.MinFloorThicknessFromOneTree = ThicknessTilesF.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
      }

      [EditorEnforceOrder(43)]
      public TerrainMaterialProto ForestFloorMaterialProto { get; set; }

      [EditorRange(0.0, 1.0)]
      [EditorLabel(null, "Minimum forest floor thickness from one tree.", false, false)]
      [EditorEnforceOrder(48)]
      public ThicknessTilesF MinFloorThicknessFromOneTree { get; set; }

      [EditorRange(0.0, 1.0)]
      [EditorEnforceOrder(53)]
      [EditorLabel(null, "Maximum forest floor thickness from one tree.", false, false)]
      public ThicknessTilesF MaxFloorThicknessFromOneTree { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(57)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMinFloorThicknessFromOneTree\u003Ek__BackingField = 0.05.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxFloorThicknessFromOneTree\u003Ek__BackingField = 0.8.TilesThick();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        ForestFloorPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ForestFloorPostProcessor.Configuration) obj).SerializeData(writer));
        ForestFloorPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ForestFloorPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
