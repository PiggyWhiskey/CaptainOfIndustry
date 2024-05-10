// Decompiled with JetBrains decompiler
// Type: Mafi.Vector2fExts
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class Vector2fExts
  {
    [Pure]
    public static Vector2f LinearInterpolate(this Vector2f[] data, int baseI, Percent t)
    {
      return data[baseI].Lerp(data[baseI + 1], t);
    }

    [Pure]
    public static Vector2f BilinearInterpolate(
      this Vector2f[] data,
      int stride,
      int baseI,
      Percent tx,
      Percent ty)
    {
      return data.LinearInterpolate(baseI, tx).Lerp(data.LinearInterpolate(baseI + stride, tx), ty);
    }

    [Pure]
    public static Vector2f CubicInterpolate(this Vector2f[] data, int baseI, Percent t)
    {
      Vector2f vector2f1 = data[baseI - 1];
      Vector2f vector2f2 = data[baseI];
      Vector2f vector2f3 = data[baseI + 1];
      Vector2f vector2f4 = data[baseI + 2];
      return new Vector2f(MafiMath.CubicInterpolate(vector2f1.X, vector2f2.X, vector2f3.X, vector2f4.X, t), MafiMath.CubicInterpolate(vector2f1.Y, vector2f2.Y, vector2f3.Y, vector2f4.Y, t));
    }

    [Pure]
    public static Vector2f BicubicInterpolate(
      this Vector2f[] data,
      int stride,
      int baseI,
      Percent tx,
      Percent ty)
    {
      Vector2f vector2f1 = data.CubicInterpolate(baseI - stride, tx);
      Vector2f vector2f2 = data.CubicInterpolate(baseI, tx);
      Vector2f vector2f3 = data.CubicInterpolate(baseI + stride, tx);
      Vector2f vector2f4 = data.CubicInterpolate(baseI + 2 * stride, tx);
      return new Vector2f(MafiMath.CubicInterpolate(vector2f1.X, vector2f2.X, vector2f3.X, vector2f4.X, ty), MafiMath.CubicInterpolate(vector2f1.Y, vector2f2.Y, vector2f3.Y, vector2f4.Y, ty));
    }

    [Pure]
    public static Vector2f MultByUnchecked(this Vector2f v, Fix32 value)
    {
      return new Vector2f(v.X.MultByUnchecked(value), v.Y.MultByUnchecked(value));
    }

    [Pure]
    public static Vector2f DivByPositiveUncheckedUnrounded(this Vector2f v, Fix32 value)
    {
      return new Vector2f(v.X.DivByPositiveUncheckedUnrounded(value), v.Y.DivByPositiveUncheckedUnrounded(value));
    }
  }
}
