// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutTile
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using System.Text;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  /// <summary>
  /// Represents information about a single relative tile of a layout.
  /// </summary>
  public readonly struct LayoutTile
  {
    /// <summary>Relative coordinate.</summary>
    public readonly RelTile2i Coord;
    /// <summary>
    /// Occupied thickness tiles (from inclusive, to exclusive).
    /// </summary>
    public readonly ThicknessIRange OccupiedThickness;
    public readonly ThicknessTilesI? TerrainHeight;
    public readonly ThicknessTilesI? MinTerrainHeight;
    public readonly ThicknessTilesI? MaxTerrainHeight;
    /// <summary>
    /// Constraint for the current tile. Constraints are validated via <see cref="T:Mafi.Core.Terrain.LayoutEntityTerrainValidator" />
    /// </summary>
    public readonly LayoutTileConstraint Constraint;
    public readonly Option<Mafi.Core.Products.TerrainMaterialProto> TerrainMaterialProto;
    public readonly Option<TerrainTileSurfaceProto> TileSurfaceProto;
    /// <summary>
    /// Whether this tile has vehicle surface. This guarantees that all 4 corners of this tile have defined
    /// vehicle surface.
    /// </summary>
    public readonly bool HasVehicleSurface;

    public LayoutTile(
      RelTile2i coord,
      ThicknessIRange occupiedThickness,
      ThicknessTilesI? terrainHeight = null,
      ThicknessTilesI? minTerrainHeight = null,
      ThicknessTilesI? maxTerrainHeight = null,
      LayoutTileConstraint constraint = LayoutTileConstraint.None,
      Option<Mafi.Core.Products.TerrainMaterialProto> terrainMaterialProto = default (Option<Mafi.Core.Products.TerrainMaterialProto>),
      Option<TerrainTileSurfaceProto> tileSurfaceProto = default (Option<TerrainTileSurfaceProto>),
      bool hasVehicleSurface = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Coord = coord;
      this.OccupiedThickness = occupiedThickness;
      this.TerrainHeight = terrainHeight;
      this.MinTerrainHeight = minTerrainHeight;
      this.MaxTerrainHeight = maxTerrainHeight;
      this.Constraint = constraint;
      this.TerrainMaterialProto = terrainMaterialProto;
      this.TileSurfaceProto = tileSurfaceProto;
      this.HasVehicleSurface = hasVehicleSurface;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(64);
      stringBuilder.Append(this.Coord.ToString());
      stringBuilder.Append(" occ ");
      stringBuilder.Append(this.OccupiedThickness.ToString());
      if (this.Constraint != LayoutTileConstraint.None)
      {
        stringBuilder.Append("; c ");
        stringBuilder.Append(this.Constraint.ToString());
      }
      if (this.TerrainHeight.HasValue)
      {
        stringBuilder.Append("; trn ");
        stringBuilder.Append(this.TerrainHeight.Value.ToString());
      }
      if (this.MinTerrainHeight.HasValue)
      {
        stringBuilder.Append("; min ");
        stringBuilder.Append(this.MinTerrainHeight.Value.ToString());
      }
      if (this.MaxTerrainHeight.HasValue)
      {
        stringBuilder.Append("; max ");
        stringBuilder.Append(this.MaxTerrainHeight.Value.ToString());
      }
      if (this.TerrainMaterialProto.HasValue)
      {
        stringBuilder.Append("; mat ");
        stringBuilder.Append(this.TerrainMaterialProto.Value.Id.Value);
      }
      if (this.TileSurfaceProto.HasValue)
      {
        stringBuilder.Append("; surf ");
        stringBuilder.Append(this.TileSurfaceProto.Value.Id.Value);
      }
      if (this.HasVehicleSurface)
        stringBuilder.Append("; has v-surf ");
      return stringBuilder.ToString();
    }
  }
}
