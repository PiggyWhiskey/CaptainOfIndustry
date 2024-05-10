// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.FluidIndicatorGfxParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory
{
  public readonly struct FluidIndicatorGfxParams
  {
    /// <summary>
    /// Size of the indicator mesh UVs if it would span the entire texture. For example,
    /// if the indicator width is 2 tiles and these two tiles fit 40% of the texture width, the total size per
    /// texture width is 2 / 0.4 = 5 tiles.
    /// </summary>
    public readonly float SizePerTextureWidthMeters;
    /// <summary>Scale of the dot details.</summary>
    public readonly float DetailsScale;
    /// <summary>Scale of movements when the fluid is still.</summary>
    public readonly float StillMovementScale;

    public FluidIndicatorGfxParams(
      float sizePerTextureWidthMeters,
      float detailsScale,
      float stillMovementScale)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.SizePerTextureWidthMeters = sizePerTextureWidthMeters;
      this.DetailsScale = detailsScale;
      this.StillMovementScale = stillMovementScale;
    }
  }
}
