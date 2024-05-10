// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.CustomPropsPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Utils;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>Places individual custom generated props.</summary>
  [GenerateSerializer(false, null, 0)]
  public class CustomPropsPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly CustomPropsPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private GeneratedPropsData m_propsData;
    [DoNotSave(0, null)]
    private TerrainManager m_terrain;
    [DoNotSave(0, null)]
    private CustomPropsPostProcessor.TerrainInfoForMaterialLookup?[] m_records;

    public static void Serialize(CustomPropsPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CustomPropsPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CustomPropsPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      CustomPropsPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static CustomPropsPostProcessor Deserialize(BlobReader reader)
    {
      CustomPropsPostProcessor propsPostProcessor;
      if (reader.TryStartClassDeserialization<CustomPropsPostProcessor>(out propsPostProcessor))
        reader.EnqueueDataDeserialization((object) propsPostProcessor, CustomPropsPostProcessor.s_deserializeDataDelayedAction);
      return propsPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CustomPropsPostProcessor>(this, "ConfigMutable", (object) CustomPropsPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Manually placed props";

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

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 4000 + 100;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public CustomPropsPostProcessor(
      CustomPropsPostProcessor.Configuration configMutable)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = configMutable;
    }

    public HandleData? GetHandleData() => new HandleData?();

    public RectangleTerrainArea2i? GetBoundingBox()
    {
      if (this.ConfigMutable.CustomPlacedProps.IsEmpty)
        return new RectangleTerrainArea2i?();
      Tile2i origin = Tile2i.MaxValue;
      Tile2i tile2i = Tile2i.MinValue;
      foreach (TerrainPropData customPlacedProp in this.ConfigMutable.CustomPlacedProps)
      {
        origin = origin.Min(customPlacedProp.Position.Tile2i);
        tile2i = tile2i.Max(customPlacedProp.Position.Tile2i);
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
      if (!extraDataReg.TryGetOrCreateExtraData<GeneratedPropsData>(out this.m_propsData))
      {
        Log.Error("Failed to obtain props data.");
        return false;
      }
      ProtosDb db = resolver.Resolve<ProtosDb>();
      this.m_terrain = resolver.Resolve<TerrainManager>();
      IEnumerable<Pair<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup>> pairs = this.ConfigMutable.TerrainInfo.AsEnumerable<KeyValuePair<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial>>().Select<KeyValuePair<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial>, Pair<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup>>((Func<KeyValuePair<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial>, Pair<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup>>) (x =>
      {
        Option<TerrainMaterialProto> option = db.Get<TerrainMaterialProto>(x.Key);
        TerrainMaterialProto valueOrNull1 = option.ValueOrNull;
        option = db.Get<TerrainMaterialProto>(x.Value.PropMaterialOverride.GetValueOrDefault());
        TerrainMaterialProto valueOrNull2 = option.ValueOrNull;
        // ISSUE: explicit non-virtual call
        TerrainMaterialSlimIdOption propMaterialOverride = (TerrainMaterialSlimIdOption) (valueOrNull2 != null ? __nonvirtual (valueOrNull2.SlimId) : new TerrainMaterialSlimId());
        option = db.Get<TerrainMaterialProto>(x.Value.BelowPropMaterial.GetValueOrDefault());
        TerrainMaterialProto valueOrNull3 = option.ValueOrNull;
        // ISSUE: explicit non-virtual call
        TerrainMaterialSlimIdOption belowPropMaterial = (TerrainMaterialSlimIdOption) (valueOrNull3 != null ? __nonvirtual (valueOrNull3.SlimId) : new TerrainMaterialSlimId());
        CustomPropsPostProcessor.TerrainInfoForMaterialLookup second = new CustomPropsPostProcessor.TerrainInfoForMaterialLookup(propMaterialOverride, belowPropMaterial);
        return new Pair<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup>(valueOrNull1, second);
      })).Where<Pair<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup>>((Func<Pair<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup>, bool>) (x => (Proto) x.First != (Proto) null));
      this.m_records = this.m_terrain.TerrainMaterials.MapArray<CustomPropsPostProcessor.TerrainInfoForMaterialLookup?>((Func<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup?>) (_ => new CustomPropsPostProcessor.TerrainInfoForMaterialLookup?()));
      foreach (Pair<TerrainMaterialProto, CustomPropsPostProcessor.TerrainInfoForMaterialLookup> pair in pairs)
        this.m_records[(int) pair.First.SlimId.Value] = new CustomPropsPostProcessor.TerrainInfoForMaterialLookup?(pair.Second);
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      return true;
    }

    public void Reset()
    {
      this.m_propsData = (GeneratedPropsData) null;
      this.m_terrain = (TerrainManager) null;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      for (int index = 0; index < this.ConfigMutable.CustomPlacedProps.Count; ++index)
      {
        TerrainPropData customPlacedProp = this.ConfigMutable.CustomPlacedProps[index];
        this.ConfigMutable.CustomPlacedProps[index] = new TerrainPropData(customPlacedProp.Proto, customPlacedProp.Variant, customPlacedProp.Position + delta.Xy, customPlacedProp.PlacedAtHeight + delta.HeightTiles1f.ThicknessTilesF, customPlacedProp.Scale, customPlacedProp.RotationYaw, customPlacedProp.RotationPitch, customPlacedProp.RotationRoll, customPlacedProp.PlacementHeightOffset);
      }
    }

    public void RotateBy(AngleDegrees1f delta)
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
      Lyst<KeyValuePair<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>>> list = (Lyst<KeyValuePair<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>>>) null;
      GeneratedPropsData.Chunk chunk1 = (GeneratedPropsData.Chunk) null;
      XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 1UL, 2UL);
      foreach (TerrainPropData customPlacedProp in this.ConfigMutable.CustomPlacedProps)
      {
        Tile2f position1 = customPlacedProp.Position;
        Tile2i tile2i = position1.Tile2i;
        if (!(tile2i.ChunkCoord2i != chunk))
        {
          if (!flag1)
          {
            flag1 = true;
            chunk1 = this.m_propsData.GetOrCreateChunk(chunk);
            list = chunk1.PropsAndSurfaceMaterials;
          }
          bool flag2 = false;
          foreach (Polygon2fFast clearedPolygon in chunk1.ClearedPolygons)
          {
            position1 = customPlacedProp.Position;
            Vector2f vector2f = position1.Vector2f;
            if (clearedPolygon.Contains(vector2f))
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
          {
            ref TerrainManager.TerrainData local1 = ref dataReadOnly;
            position1 = customPlacedProp.Position;
            RelTile2i tile = position1.Tile2i - dataOrigin;
            int tileIndex = local1.GetTileIndex(tile);
            TileMaterialLayers materialLayer = dataReadOnly.MaterialLayers[tileIndex];
            TerrainPropId key;
            ref TerrainPropId local2 = ref key;
            position1 = customPlacedProp.Position;
            tile2i = position1.Tile2i;
            Tile2iSlim asSlim = tile2i.AsSlim;
            local2 = new TerrainPropId(asSlim);
            TerrainMaterialSlimId slimId1 = materialLayer.First.SlimId;
            TerrainPropData first = customPlacedProp;
            CustomPropsPostProcessor.TerrainInfoForMaterialLookup? record = this.m_records[(int) materialLayer.First.SlimId.Value];
            if (record.HasValue)
            {
              ref TerrainPropData local3 = ref first;
              TerrainPropProto proto = customPlacedProp.Proto;
              CustomPropsPostProcessor.TerrainInfoForMaterialLookup forMaterialLookup = record.Value;
              TerrainMaterialSlimId materialSlimId;
              if (!forMaterialLookup.PropMaterialOverride.HasValue)
              {
                materialSlimId = slimId1;
              }
              else
              {
                forMaterialLookup = record.Value;
                materialSlimId = forMaterialLookup.PropMaterialOverride.Value;
              }
              double offsetX = (double) random.NextFloat();
              double offsetY = (double) random.NextFloat();
              double uvSize = (double) customPlacedProp.Scale.ToFloat();
              TerrainPropData.PropVariant variant = new TerrainPropData.PropVariant(materialSlimId, (float) offsetX, (float) offsetY, (float) uvSize);
              Tile2f position2 = customPlacedProp.Position;
              HeightTilesF placedAtHeight = customPlacedProp.PlacedAtHeight;
              Percent scale = customPlacedProp.Scale;
              AngleSlim rotationYaw = customPlacedProp.RotationYaw;
              AngleSlim rotationPitch = customPlacedProp.RotationPitch;
              AngleSlim rotationRoll = customPlacedProp.RotationRoll;
              ThicknessTilesF placementHeightOffset = customPlacedProp.PlacementHeightOffset;
              local3 = new TerrainPropData(proto, variant, position2, placedAtHeight, scale, rotationYaw, rotationPitch, rotationRoll, placementHeightOffset);
              forMaterialLookup = record.Value;
              TerrainMaterialSlimId slimId2;
              if (!forMaterialLookup.BelowPropMaterial.HasValue)
              {
                slimId2 = materialLayer.First.SlimId;
              }
              else
              {
                forMaterialLookup = record.Value;
                slimId2 = forMaterialLookup.BelowPropMaterial.Value;
              }
              TerrainMaterialProto full = slimId2.ToFull(this.m_terrain);
              list.Add<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>>(key, new Pair<TerrainPropData, TerrainMaterialProto>(first, full));
            }
            else
              list.Add<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>>(key, new Pair<TerrainPropData, TerrainMaterialProto>(first, materialLayer.First.SlimId.ToFull(this.m_terrain)));
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

    public TerrainPropData GetPropForTerrain(
      TerrainPropData previewPropData,
      TerrainManager terrainManager)
    {
      Tile2iIndex tileIndex = terrainManager.GetTileIndex(previewPropData.Position.Tile2i);
      TileMaterialLayers tileMaterialLayers = terrainManager.TileLayersData[tileIndex.Value];
      CustomPropsPostProcessor.TerrainInfoForMaterialLookup? record = this.m_records[(int) tileMaterialLayers.First.SlimId.Value];
      if (!record.HasValue)
        return previewPropData;
      TerrainMaterialSlimId slimId = tileMaterialLayers.First.SlimId;
      return new TerrainPropData(previewPropData.Proto, new TerrainPropData.PropVariant(record.Value.PropMaterialOverride.HasValue ? record.Value.PropMaterialOverride.Value : slimId, 0.0f, 0.0f, previewPropData.Scale.ToFloat()), previewPropData.Position, previewPropData.PlacedAtHeight, previewPropData.Scale, previewPropData.RotationYaw, previewPropData.RotationPitch, previewPropData.RotationRoll, previewPropData.PlacementHeightOffset);
    }

    public void AddProp(TerrainPropData propData)
    {
      this.ConfigMutable.CustomPlacedProps.Add(propData);
    }

    public void RemoveProp(TerrainPropId previewProp)
    {
      this.ConfigMutable.CustomPlacedProps.RemoveWhere((Predicate<TerrainPropData>) (p => p.Id == previewProp));
    }

    static CustomPropsPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      CustomPropsPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CustomPropsPostProcessor) obj).SerializeData(writer));
      CustomPropsPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CustomPropsPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [EditorIgnore]
      public Lyst<TerrainPropData> CustomPlacedProps;
      [EditorEnforceOrder(48)]
      public Dict<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial> TerrainInfo;

      public static void Serialize(CustomPropsPostProcessor.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<CustomPropsPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, CustomPropsPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Lyst<TerrainPropData>.Serialize(this.CustomPlacedProps, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Dict<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial>.Serialize(this.TerrainInfo, writer);
      }

      public static CustomPropsPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        CustomPropsPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<CustomPropsPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, CustomPropsPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.CustomPlacedProps = Lyst<TerrainPropData>.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.TerrainInfo = Dict<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial>.Deserialize(reader);
      }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(51)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.CustomPlacedProps = new Lyst<TerrainPropData>();
        this.TerrainInfo = new Dict<Proto.ID, CustomPropsPostProcessor.TerrainInfoForMaterial>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        CustomPropsPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CustomPropsPostProcessor.Configuration) obj).SerializeData(writer));
        CustomPropsPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CustomPropsPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct TerrainInfoForMaterial
    {
      [RenamedInVersion(146, "PropMaterialOverride")]
      [DoNotSave(146, null)]
      public Proto.ID PropMaterialOverrideOld;
      [DoNotSave(146, null)]
      [RenamedInVersion(146, "BelowPropMaterial")]
      public Proto.ID BelowPropMaterialOld;
      [NewInSaveVersion(146, null, "obj.PropMaterialOverrideOld", null, null)]
      public Proto.ID? PropMaterialOverride;
      [NewInSaveVersion(146, null, "obj.BelowPropMaterialOld", null, null)]
      public Proto.ID? BelowPropMaterial;

      public static void Serialize(
        CustomPropsPostProcessor.TerrainInfoForMaterial value,
        BlobWriter writer)
      {
        writer.WriteNullableStruct<Proto.ID>(value.BelowPropMaterial);
        writer.WriteNullableStruct<Proto.ID>(value.PropMaterialOverride);
      }

      public static CustomPropsPostProcessor.TerrainInfoForMaterial Deserialize(BlobReader reader)
      {
        CustomPropsPostProcessor.TerrainInfoForMaterial terrainInfoForMaterial = new CustomPropsPostProcessor.TerrainInfoForMaterial();
        if (reader.LoadedSaveVersion < 146)
          terrainInfoForMaterial.BelowPropMaterialOld = Proto.ID.Deserialize(reader);
        terrainInfoForMaterial.BelowPropMaterial = reader.LoadedSaveVersion >= 146 ? reader.ReadNullableStruct<Proto.ID>() : new Proto.ID?(terrainInfoForMaterial.BelowPropMaterialOld);
        if (reader.LoadedSaveVersion < 146)
          terrainInfoForMaterial.PropMaterialOverrideOld = Proto.ID.Deserialize(reader);
        terrainInfoForMaterial.PropMaterialOverride = reader.LoadedSaveVersion >= 146 ? reader.ReadNullableStruct<Proto.ID>() : new Proto.ID?(terrainInfoForMaterial.PropMaterialOverrideOld);
        return terrainInfoForMaterial;
      }
    }

    public readonly struct TerrainInfoForMaterialLookup
    {
      public readonly TerrainMaterialSlimIdOption PropMaterialOverride;
      public readonly TerrainMaterialSlimIdOption BelowPropMaterial;

      public TerrainInfoForMaterialLookup(
        TerrainMaterialSlimIdOption propMaterialOverride,
        TerrainMaterialSlimIdOption belowPropMaterial)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.PropMaterialOverride = propMaterialOverride;
        this.BelowPropMaterial = belowPropMaterial;
      }
    }
  }
}
