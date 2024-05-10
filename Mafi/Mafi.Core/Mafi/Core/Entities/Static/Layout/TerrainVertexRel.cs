// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.TerrainVertexRel
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public readonly struct TerrainVertexRel
  {
    public readonly RelTile2i Coord;
    public readonly ThicknessIRange OccupiedThickness;
    public readonly LayoutTileConstraint Constraint;
    public readonly Option<TerrainMaterialProto> TerrainMaterial;
    public readonly ThicknessTilesI? TerrainHeight;
    public readonly ThicknessTilesI? MinTerrainHeight;
    public readonly ThicknessTilesI? MaxTerrainHeight;
    public readonly ThicknessTilesF? VehicleSurfaceRelHeight;
    public readonly int ContributingTiles;
    public readonly int LowestTileIndex;

    public TerrainVertexRel(
      RelTile2i coord,
      ThicknessIRange occupiedThickness,
      LayoutTileConstraint constraint,
      Option<TerrainMaterialProto> terrainMaterial,
      ThicknessTilesI? terrainHeight,
      ThicknessTilesI? minTerrainHeight,
      ThicknessTilesI? maxTerrainHeight,
      ThicknessTilesF? vehicleSurfaceRelHeight,
      int contributingTiles,
      int lowestTileIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Coord = coord;
      this.OccupiedThickness = occupiedThickness;
      this.Constraint = constraint;
      this.TerrainMaterial = terrainMaterial;
      this.TerrainHeight = terrainHeight;
      this.MinTerrainHeight = minTerrainHeight;
      this.MaxTerrainHeight = maxTerrainHeight;
      this.VehicleSurfaceRelHeight = vehicleSurfaceRelHeight;
      this.ContributingTiles = contributingTiles;
      this.LowestTileIndex = lowestTileIndex;
    }

    [Pure]
    public TerrainVertexRel WithExtraConstraint(LayoutTileConstraint c)
    {
      return new TerrainVertexRel(this.Coord, this.OccupiedThickness, this.Constraint | c, this.TerrainMaterial, this.TerrainHeight, this.MinTerrainHeight, this.MaxTerrainHeight, this.VehicleSurfaceRelHeight, this.ContributingTiles, this.LowestTileIndex);
    }

    [Pure]
    public TerrainVertexRel WithTerrainMaterial(TerrainMaterialProto terrainMaterial)
    {
      return new TerrainVertexRel(this.Coord, this.OccupiedThickness, this.Constraint, (Option<TerrainMaterialProto>) terrainMaterial, this.TerrainHeight, this.MinTerrainHeight, this.MaxTerrainHeight, this.VehicleSurfaceRelHeight, this.ContributingTiles, this.LowestTileIndex);
    }
  }
}
