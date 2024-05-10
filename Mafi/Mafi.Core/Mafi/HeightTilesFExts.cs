// Decompiled with JetBrains decompiler
// Type: Mafi.HeightTilesFExts
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class HeightTilesFExts
  {
    public static HeightTilesF TilesHigh(this Fix32 value) => new HeightTilesF(value);

    public static HeightTilesF TilesHigh(this Fix64 value) => new HeightTilesF(value.ToFix32());

    public static HeightTilesF TilesHigh(this double value) => new HeightTilesF(value.ToFix32());

    [Pure]
    public static HeightTilesF CubicInterpolate(this HeightTilesF[] data, int baseI, Percent t)
    {
      return new HeightTilesF(MafiMath.MonotoneCubicInterpolate(data[baseI - 1].Value, data[baseI].Value, data[baseI + 1].Value, data[baseI + 2].Value, t));
    }

    [Pure]
    public static HeightTilesF BicubicInterpolate(
      this HeightTilesF[] data,
      int stride,
      int baseI,
      Percent tx,
      Percent ty)
    {
      return new HeightTilesF(MafiMath.MonotoneCubicInterpolate(data.CubicInterpolate(baseI - stride, tx).Value, data.CubicInterpolate(baseI, tx).Value, data.CubicInterpolate(baseI + stride, tx).Value, data.CubicInterpolate(baseI + 2 * stride, tx).Value, ty));
    }
  }
}
