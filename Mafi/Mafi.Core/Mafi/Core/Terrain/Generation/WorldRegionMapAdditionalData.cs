// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.WorldRegionMapAdditionalData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public class WorldRegionMapAdditionalData : IWorldRegionMapAdditionalData
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly ThicknessTilesF MAX_DEPTH_FOR_EASY_RESOURCES;

    public static void Serialize(WorldRegionMapAdditionalData value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldRegionMapAdditionalData>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldRegionMapAdditionalData.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<MapOtherResourceStats>.Serialize(this.EasyToReachOtherResourcesStats, writer);
      ImmutableArray<MapProductStats>.Serialize(this.EasyToReachProductStats, writer);
      ImmutableArray<MapTerrainResourceStats>.Serialize(this.EasyToReachTerrainResourcesStats, writer);
      writer.WriteInt(this.FlatNonOceanTilesCount);
      writer.WriteInt(this.NonOceanTilesCount);
      ImmutableArray<EncodedImageAndMatrix>.Serialize(this.PreviewImagesData, writer);
      ImmutableArray<MapResourceLocation>.Serialize(this.ResourceLocations, writer);
      ImmutableArray<StartingLocationPreview>.Serialize(this.StartingLocations, writer);
      ImmutableArray<Pair<HeightTilesI, int>>.Serialize(this.TilesAtOrAboveElevationDataSorted, writer);
      ImmutableArray<MapOtherResourceStats>.Serialize(this.TotalOtherResourcesStats, writer);
      ImmutableArray<MapProductStats>.Serialize(this.TotalProductStats, writer);
      ImmutableArray<MapTerrainResourceStats>.Serialize(this.TotalTerrainResourcesStats, writer);
    }

    public static WorldRegionMapAdditionalData Deserialize(BlobReader reader)
    {
      WorldRegionMapAdditionalData mapAdditionalData;
      if (reader.TryStartClassDeserialization<WorldRegionMapAdditionalData>(out mapAdditionalData))
        reader.EnqueueDataDeserialization((object) mapAdditionalData, WorldRegionMapAdditionalData.s_deserializeDataDelayedAction);
      return mapAdditionalData;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.EasyToReachOtherResourcesStats = ImmutableArray<MapOtherResourceStats>.Deserialize(reader);
      this.EasyToReachProductStats = reader.LoadedSaveVersion >= 141 ? ImmutableArray<MapProductStats>.Deserialize(reader) : new ImmutableArray<MapProductStats>();
      this.EasyToReachTerrainResourcesStats = ImmutableArray<MapTerrainResourceStats>.Deserialize(reader);
      this.FlatNonOceanTilesCount = reader.ReadInt();
      this.NonOceanTilesCount = reader.ReadInt();
      this.PreviewImagesData = ImmutableArray<EncodedImageAndMatrix>.Deserialize(reader);
      this.ResourceLocations = ImmutableArray<MapResourceLocation>.Deserialize(reader);
      this.StartingLocations = ImmutableArray<StartingLocationPreview>.Deserialize(reader);
      this.TilesAtOrAboveElevationDataSorted = ImmutableArray<Pair<HeightTilesI, int>>.Deserialize(reader);
      this.TotalOtherResourcesStats = ImmutableArray<MapOtherResourceStats>.Deserialize(reader);
      this.TotalProductStats = reader.LoadedSaveVersion >= 141 ? ImmutableArray<MapProductStats>.Deserialize(reader) : new ImmutableArray<MapProductStats>();
      this.TotalTerrainResourcesStats = ImmutableArray<MapTerrainResourceStats>.Deserialize(reader);
    }

    public int NonOceanTilesCount { get; set; }

    public int FlatNonOceanTilesCount { get; set; }

    public ImmutableArray<StartingLocationPreview> StartingLocations { get; set; }

    public ImmutableArray<MapTerrainResourceStats> EasyToReachTerrainResourcesStats { get; set; }

    public ImmutableArray<MapTerrainResourceStats> TotalTerrainResourcesStats { get; set; }

    [NewInSaveVersion(141, null, "new()", null, null)]
    public ImmutableArray<MapProductStats> EasyToReachProductStats { get; set; }

    [NewInSaveVersion(141, null, "new()", null, null)]
    public ImmutableArray<MapProductStats> TotalProductStats { get; set; }

    public ImmutableArray<MapOtherResourceStats> EasyToReachOtherResourcesStats { get; set; }

    public ImmutableArray<MapOtherResourceStats> TotalOtherResourcesStats { get; set; }

    public ImmutableArray<MapResourceLocation> ResourceLocations { get; set; }

    public ImmutableArray<Pair<HeightTilesI, int>> TilesAtOrAboveElevationDataSorted { get; set; }

    public ImmutableArray<EncodedImageAndMatrix> PreviewImagesData { get; set; }

    public WorldRegionMapAdditionalData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static WorldRegionMapAdditionalData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldRegionMapAdditionalData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldRegionMapAdditionalData) obj).SerializeData(writer));
      WorldRegionMapAdditionalData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldRegionMapAdditionalData) obj).DeserializeData(reader));
      WorldRegionMapAdditionalData.MAX_DEPTH_FOR_EASY_RESOURCES = 10.0.TilesThick();
    }
  }
}
