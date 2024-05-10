// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.CustomTreesPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Trees;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>Places individual custom generated trees.</summary>
  [GenerateSerializer(false, null, 0)]
  public class CustomTreesPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature,
    ITerrainFeatureWithSimUpdate
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly CustomTreesPostProcessor.Configuration ConfigMutable;
    private Dict<TreeId, TreeDataBase> m_managedTrees;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private GeneratedTreesData m_treesData;
    [DoNotSave(0, null)]
    private bool m_previewGenerated;

    public static void Serialize(CustomTreesPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CustomTreesPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CustomTreesPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      CustomTreesPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      Dict<TreeId, TreeDataBase>.Serialize(this.m_managedTrees, writer);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static CustomTreesPostProcessor Deserialize(BlobReader reader)
    {
      CustomTreesPostProcessor treesPostProcessor;
      if (reader.TryStartClassDeserialization<CustomTreesPostProcessor>(out treesPostProcessor))
        reader.EnqueueDataDeserialization((object) treesPostProcessor, CustomTreesPostProcessor.s_deserializeDataDelayedAction);
      return treesPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CustomTreesPostProcessor>(this, "ConfigMutable", (object) CustomTreesPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_managedTrees = Dict<TreeId, TreeDataBase>.Deserialize(reader);
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Manually placed trees";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => true;

    public bool IsImportable => true;

    public bool Is2D => true;

    public bool CanRotate => false;

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.AnalyzeInterleaveAndApply;
    }

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 2000 + 100;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public CustomTreesPostProcessor(
      CustomTreesPostProcessor.Configuration configMutable)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_managedTrees = new Dict<TreeId, TreeDataBase>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = configMutable;
    }

    public HandleData? GetHandleData() => new HandleData?();

    public RectangleTerrainArea2i? GetBoundingBox()
    {
      if (this.ConfigMutable.CustomTreeList.IsEmpty)
        return new RectangleTerrainArea2i?();
      Tile2i origin = Tile2i.MaxValue;
      Tile2i tile2i = Tile2i.MinValue;
      foreach (TreeDataBase customTree in this.ConfigMutable.CustomTreeList)
      {
        origin = origin.Min(customTree.Position.Tile2i);
        tile2i = tile2i.Max(customTree.Position.Tile2i);
      }
      return new RectangleTerrainArea2i?(new RectangleTerrainArea2i(origin, tile2i - origin));
    }

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
      return true;
    }

    public void Reset()
    {
      this.m_treesData = (GeneratedTreesData) null;
      this.m_previewGenerated = false;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      for (int index = 0; index < this.ConfigMutable.CustomTreeList.Count; ++index)
      {
        TreeDataBase customTree = this.ConfigMutable.CustomTreeList[index];
        this.ConfigMutable.CustomTreeList[index] = new TreeDataBase(customTree.Proto, (customTree.Position + delta.Xy).Tile2i.CenterTile2f, customTree.Rotation, customTree.Scale);
      }
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
      bool flag1 = false;
      Dict<TreeId, TreeDataBase> dict = (Dict<TreeId, TreeDataBase>) null;
      GeneratedTreesData.Chunk chunk1 = (GeneratedTreesData.Chunk) null;
      foreach (TreeDataBase customTree in this.ConfigMutable.CustomTreeList)
      {
        if (!(customTree.Position.Tile2i.ChunkCoord2i != chunk))
        {
          if (!flag1)
          {
            flag1 = true;
            chunk1 = this.m_treesData.GetOrCreateChunk(chunk);
            dict = chunk1.Trees;
          }
          bool flag2 = false;
          foreach (Polygon2fFast clearedPolygon in chunk1.ClearedPolygons)
          {
            if (clearedPolygon.Contains(customTree.Position.Vector2f))
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
          {
            TreeId key = new TreeId(customTree.Position.Tile2i.AsSlim);
            dict[key] = customTree;
          }
        }
      }
      if (flag1)
        return;
      lock (this.m_unaffectedChunksCache.BackingArray)
        this.m_unaffectedChunksCache.Add(chunk);
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
    }

    public void AddManualTree(TreeProto treeProto, Tile2f position)
    {
      this.ConfigMutable.CustomTreeList.Add(new TreeDataBase(treeProto, position, AngleSlim.Zero, Percent.Hundred));
    }

    public void AddManualTree(TreeDataBase treeData)
    {
      this.ConfigMutable.CustomTreeList.Add(treeData);
    }

    public void RemoveManualTree(TreeId treeId)
    {
      for (int index = 0; index < this.ConfigMutable.CustomTreeList.Count; ++index)
      {
        if (this.ConfigMutable.CustomTreeList[index].Position.Tile2i.AsSlim == treeId.Position)
        {
          this.ConfigMutable.CustomTreeList.RemoveAt(index);
          break;
        }
      }
    }

    public bool SimUpdate(IResolver resolver)
    {
      if (this.m_previewGenerated)
        return false;
      this.m_previewGenerated = true;
      TreesManager treesManager = resolver.Resolve<TreesManager>();
      foreach (KeyValuePair<TreeId, TreeDataBase> keyValuePair in this.m_managedTrees.ToArray<KeyValuePair<TreeId, TreeDataBase>>())
      {
        KeyValuePair<TreeId, TreeDataBase> kvp = keyValuePair;
        if (!this.ConfigMutable.CustomTreeList.Any<TreeDataBase>((Predicate<TreeDataBase>) (tdb => tdb.Equals((object) kvp.Value))))
        {
          treesManager.TryRemoveGeneratedTree(kvp.Key);
          this.m_managedTrees.Remove(kvp.Key);
        }
      }
      foreach (TreeDataBase customTree in this.ConfigMutable.CustomTreeList)
      {
        if (!((Proto) customTree.Proto == (Proto) null) && !customTree.Proto.IsPhantom && !customTree.Proto.IsObsolete)
        {
          TreeId treeId = new TreeId(customTree.Position.Tile2i.AsSlim);
          TreeData treeData;
          if (treesManager.Trees.TryGetValue(treeId, out treeData))
          {
            if (!((Proto) customTree.Proto == (Proto) treeData.Proto) || !(customTree.Position == treeData.Position) || !(customTree.Rotation == treeData.Rotation) || !(customTree.Scale == treeData.BaseScale))
            {
              treesManager.TryRemoveGeneratedTree(treeId);
              this.m_managedTrees.TryRemove(treeId, out TreeDataBase _);
            }
            else
              continue;
          }
          treesManager.TryAddGeneratedTree(customTree);
          this.m_managedTrees.Add(treeId, customTree);
        }
      }
      return false;
    }

    static CustomTreesPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      CustomTreesPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CustomTreesPostProcessor) obj).SerializeData(writer));
      CustomTreesPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CustomTreesPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [EditorIgnore]
      public Lyst<TreeDataBase> CustomTreeList;

      public static void Serialize(CustomTreesPostProcessor.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<CustomTreesPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, CustomTreesPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Lyst<TreeDataBase>.Serialize(this.CustomTreeList, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
      }

      public static CustomTreesPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        CustomTreesPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<CustomTreesPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, CustomTreesPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.CustomTreeList = Lyst<TreeDataBase>.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
      }

      [EditorEnforceOrder(45)]
      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.CustomTreeList = new Lyst<TreeDataBase>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        CustomTreesPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CustomTreesPostProcessor.Configuration) obj).SerializeData(writer));
        CustomTreesPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CustomTreesPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
