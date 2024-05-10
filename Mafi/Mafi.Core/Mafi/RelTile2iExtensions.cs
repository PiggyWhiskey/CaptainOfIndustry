// Decompiled with JetBrains decompiler
// Type: Mafi.RelTile2iExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class RelTile2iExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static RelTile2i NextRelTile2i(this IRandom random, int minValueIncl, int maxValueExcl)
    {
      return new RelTile2i(random.NextInt(minValueIncl, maxValueExcl), random.NextInt(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static RelTile2i NextRelTile2i(
      this IRandom random,
      RelTile2i minValueIncl,
      RelTile2i maxValueExcl)
    {
      return new RelTile2i(random.NextInt(minValueIncl.X, maxValueExcl.X), random.NextInt(minValueIncl.Y, maxValueExcl.Y));
    }

    public static RelTile2i ToTileDirection(this Direction90 direction)
    {
      return new RelTile2i(direction.DirectionVector);
    }

    public static RelTile2i Transform(this Matrix2i m, RelTile2i v)
    {
      return new RelTile2i(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y);
    }
  }
}
