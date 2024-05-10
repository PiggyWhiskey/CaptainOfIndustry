// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.TerrainDetailsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Base.Terrain
{
  public class TerrainDetailsData : IModData
  {
    private static readonly Percent GRASS_POSITION_RNG;
    private static readonly Percent GRASS_BASE_SCALE;
    private static readonly Percent GRASS_SCALE_VARIATION;
    private static readonly Percent FLOWERS_SCALE_VARIATION;

    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.Grass, "Assets/Base/Terrain/Details/Grass.prefab", "Assets/Base/Terrain/Details/Grass.png", 0.5f, 0.5f, 1f, 1f, 1f, new ColorRgba(9677644, 64), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.GRASS_SCALE_VARIATION, new Vector2i(0, 0), 1))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.GrassLush, "Assets/Base/Terrain/Details/Grass.prefab", "Assets/Base/Terrain/Details/GrassLush.png", 0.5f, 0.5f, 1f, 1f, 1f, new ColorRgba(10533476, 64), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, TerrainDetailsData.GRASS_BASE_SCALE + 20.Percent(), TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.GRASS_SCALE_VARIATION, new Vector2i(0, 0), 1))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.ForestGrass, "Assets/Base/Terrain/Details/Grass.prefab", "Assets/Base/Terrain/Details/ForestGrass.png", 0.25f, 0.25f, 1f, 1f, 1f, new ColorRgba(9677644, 0), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.GRASS_SCALE_VARIATION, new Vector2i(0, 0), 2), new DetailLayerSpecProto.DetailVariant(3, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.GRASS_SCALE_VARIATION, new Vector2i(0, 1), 2))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.FlowersWhite, "Assets/Base/Terrain/Details/Flowers.prefab", "Assets/Base/Terrain/Details/FlowersWhite.png", 0.5f, 0.9f, 1f, 1f, 1f, new ColorRgba(16777215, 0), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 0), 2), new DetailLayerSpecProto.DetailVariant(2, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(1, 0), 2), new DetailLayerSpecProto.DetailVariant(4, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 1), 2))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.FlowersYellowLush, "Assets/Base/Terrain/Details/Flowers.prefab", "Assets/Base/Terrain/Details/FlowersYellowLush.png", 0.5f, 0.9f, 1f, 1f, 1f, new ColorRgba(16777215, 0), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 0), 2), new DetailLayerSpecProto.DetailVariant(2, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(1, 0), 2), new DetailLayerSpecProto.DetailVariant(4, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 1), 2))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.FlowersRed, "Assets/Base/Terrain/Details/Flowers.prefab", "Assets/Base/Terrain/Details/FlowersRed.png", 0.5f, 0.9f, 1f, 1f, 1f, new ColorRgba(16777215, 0), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 0), 2), new DetailLayerSpecProto.DetailVariant(2, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(1, 0), 2), new DetailLayerSpecProto.DetailVariant(4, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 1), 2))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.FlowersPurpleLush, "Assets/Base/Terrain/Details/Flowers.prefab", "Assets/Base/Terrain/Details/FlowersPurpleLush.png", 0.5f, 0.9f, 1f, 1f, 1f, new ColorRgba(16777215, 0), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 0), 2), new DetailLayerSpecProto.DetailVariant(2, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(1, 0), 2), new DetailLayerSpecProto.DetailVariant(4, TerrainDetailsData.GRASS_BASE_SCALE, TerrainDetailsData.GRASS_POSITION_RNG, TerrainDetailsData.FLOWERS_SCALE_VARIATION, new Vector2i(0, 1), 2))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.DebrisFlat, "Assets/Base/Terrain/Details/Debris.prefab", "Assets/Base/Terrain/Details/Debris.png", 0.1f, 0.1f, 1f, 0.0f, 0.0f, new ColorRgba(10533476, 0), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, 100.Percent(), 50.Percent(), 10.Percent(), new Vector2i(2, 0), 4), new DetailLayerSpecProto.DetailVariant(1, 100.Percent(), 50.Percent(), 10.Percent(), new Vector2i(2, 1), 4), new DetailLayerSpecProto.DetailVariant(1, 100.Percent(), 50.Percent(), 10.Percent(), new Vector2i(3, 1), 4))));
      registrator.PrototypesDb.Add<DetailLayerSpecProto>(new DetailLayerSpecProto(Ids.TerrainDetails.Rocks, "Assets/Base/Terrain/Details/Rocks.prefab", "Assets/Base/Terrain/Surfaces/Cobblestone/Cobblestone1a-256-albedo.png", 0.5f, 0.5f, 1f, 0.0f, 0.0f, new ColorRgba(16777215, 0), ImmutableArray.Create<DetailLayerSpecProto.DetailVariant>(new DetailLayerSpecProto.DetailVariant(1, Percent.Hundred, 50.Percent(), 30.Percent(), new Vector2i(0, 0), 1))));
    }

    public TerrainDetailsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TerrainDetailsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TerrainDetailsData.GRASS_POSITION_RNG = 70.Percent();
      TerrainDetailsData.GRASS_BASE_SCALE = 110.Percent();
      TerrainDetailsData.GRASS_SCALE_VARIATION = 35.Percent();
      TerrainDetailsData.FLOWERS_SCALE_VARIATION = 25.Percent();
    }
  }
}
