// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutTokenSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public readonly struct LayoutTokenSpec
  {
    public readonly ThicknessTilesI HeightFrom;
    public readonly ThicknessTilesI HeightToExcl;
    public readonly LayoutTileConstraint Constraint;
    public readonly ThicknessTilesI? TerrainSurfaceHeight;
    public readonly ThicknessTilesI? MinTerrainHeight;
    public readonly ThicknessTilesI? MaxTerrainHeight;
    public readonly ThicknessTilesF? VehicleHeight;
    public readonly Proto.ID? TerrainMaterialId;
    public readonly Proto.ID? SurfaceId;
    public readonly bool IsRamp;
    public readonly bool IsPort;
    public readonly int PortHeight;

    /// <param name="heightFrom">Starting height. Value 0 is on the ground.</param>
    /// <param name="heightToExcl">Final height (exclusive). Value 5 will occupy tiles up to and including
    /// height 4.</param>
    /// <param name="constraint"></param>
    /// <param name="terrainSurfaceHeight">When specified, terrain surface will be set to this value during
    /// construction. The <paramref name="minTerrainHeight" /> and <paramref name="maxTerrainHeight" /> will be
    /// also set to this value and <see cref="F:Mafi.Core.Entities.Static.Layout.LayoutTileConstraint.Ground" /> will be added to
    /// <paramref name="constraint" />.</param>
    /// <param name="minTerrainHeight">When set, entity will be in danger of collapse when terrain below the tile
    /// is lower than specified value.</param>
    /// <param name="maxTerrainHeight">When set, entity will be in danger of collapse when terrain below the tile
    /// is higher than specified value.</param>
    /// <param name="vehicleHeight">Height of vehicle surface.</param>
    /// <param name="terrainMaterialId">When specified, one tile of terrain will be set to this material.</param>
    /// <param name="surfaceId">When specified, surface of specified ID will be placed on this tile.</param>
    /// <param name="isRamp"></param>
    /// <param name="isPort"></param>
    /// <param name="portHeight">Sets a custom specced port's height.</param>
    public LayoutTokenSpec(
      int heightFrom = 0,
      int heightToExcl = 0,
      LayoutTileConstraint constraint = LayoutTileConstraint.None,
      int? terrainSurfaceHeight = null,
      int? minTerrainHeight = null,
      int? maxTerrainHeight = null,
      Fix32? vehicleHeight = null,
      Proto.ID? terrainMaterialId = null,
      Proto.ID? surfaceId = null,
      bool isRamp = false,
      bool isPort = false,
      int portHeight = 0)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.HeightFrom = heightFrom.TilesThick();
      this.HeightToExcl = heightToExcl.TilesThick();
      this.Constraint = constraint;
      this.TerrainSurfaceHeight = !terrainSurfaceHeight.HasValue ? new ThicknessTilesI?() : new ThicknessTilesI?(new ThicknessTilesI(terrainSurfaceHeight.Value));
      this.MinTerrainHeight = !minTerrainHeight.HasValue ? new ThicknessTilesI?() : new ThicknessTilesI?(new ThicknessTilesI(minTerrainHeight.Value));
      this.MaxTerrainHeight = !maxTerrainHeight.HasValue ? new ThicknessTilesI?() : new ThicknessTilesI?(new ThicknessTilesI(maxTerrainHeight.Value));
      this.VehicleHeight = !vehicleHeight.HasValue ? new ThicknessTilesF?() : new ThicknessTilesF?(new ThicknessTilesF(vehicleHeight.Value));
      this.TerrainMaterialId = terrainMaterialId;
      this.SurfaceId = surfaceId;
      this.IsRamp = isRamp;
      this.IsPort = isPort;
      this.PortHeight = portHeight;
    }
  }
}
