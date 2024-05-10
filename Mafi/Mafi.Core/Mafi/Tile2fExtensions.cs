// Decompiled with JetBrains decompiler
// Type: Mafi.Tile2fExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class Tile2fExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Tile2f NextTile2f(this IRandom random, Fix32 minValueIncl, Fix32 maxValueExcl)
    {
      return new Tile2f(random.NextFix32(minValueIncl, maxValueExcl), random.NextFix32(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Tile2f NextTile2f(this IRandom random, Tile2f minValueIncl, Tile2f maxValueExcl)
    {
      return new Tile2f(random.NextFix32(minValueIncl.X, maxValueExcl.X), random.NextFix32(minValueIncl.Y, maxValueExcl.Y));
    }

    public static Tile2f NextTile2fBetween01(this IRandom random)
    {
      return new Tile2f(random.NextFix32Between01(), random.NextFix32Between01());
    }
  }
}
