// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.CellSurfacesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Map;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain
{
  public class CellSurfacesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb db = registrator.PrototypesDb;
      db.Add<MapCellSurfaceGeneratorProto>(new MapCellSurfaceGeneratorProto(Ids.CellSurfaces.NoSurface, Proto.Str.Empty, (ImmutableArray<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>>) ImmutableArray.Empty));
      db.Add<MapCellSurfaceGeneratorProto>(new MapCellSurfaceGeneratorProto(Ids.CellSurfaces.Grass, Proto.Str.Empty, ImmutableArray.Create<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>>(createMaterialThickness(Ids.TerrainMaterials.Grass, 5.0))));
      db.Add<MapCellSurfaceGeneratorProto>(new MapCellSurfaceGeneratorProto(Ids.CellSurfaces.GrassAndSand, Proto.Str.Empty, ImmutableArray.Create<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>>(createMaterialThickness(Ids.TerrainMaterials.Grass, 3.0), createMaterialThickness(Ids.TerrainMaterials.Sand, 5.0))));
      db.Add<MapCellSurfaceGeneratorProto>(new MapCellSurfaceGeneratorProto(Ids.CellSurfaces.Rock, Proto.Str.Empty, ImmutableArray.Create<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>>(createMaterialThickness(Ids.TerrainMaterials.Rock, 3.0))));
      db.Add<MapCellSurfaceGeneratorProto>(new MapCellSurfaceGeneratorProto(Ids.CellSurfaces.Sand, Proto.Str.Empty, ImmutableArray.Create<KeyValuePair<TerrainMaterialProto, ThicknessTilesF>>(createMaterialThickness(Ids.TerrainMaterials.Sand, 6.0))));

      KeyValuePair<TerrainMaterialProto, ThicknessTilesF> createMaterialThickness(
        Proto.ID id,
        double thickness)
      {
        return Make.Kvp<TerrainMaterialProto, ThicknessTilesF>(db.GetOrThrow<TerrainMaterialProto>(id), thickness.TilesThick());
      }
    }

    public CellSurfacesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
