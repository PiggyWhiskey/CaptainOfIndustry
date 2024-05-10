// Decompiled with JetBrains decompiler
// Type: Mafi.Tile3fExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class Tile3fExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Tile3f NextTile3f(this IRandom random, Fix32 minValueIncl, Fix32 maxValueExcl)
    {
      return new Tile3f(random.NextFix32(minValueIncl, maxValueExcl), random.NextFix32(minValueIncl, maxValueExcl), random.NextFix32(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Tile3f NextTile3f(this IRandom random, Tile3f minValueIncl, Tile3f maxValueExcl)
    {
      return new Tile3f(random.NextFix32(minValueIncl.X, maxValueExcl.X), random.NextFix32(minValueIncl.Y, maxValueExcl.Y), random.NextFix32(minValueIncl.Z, maxValueExcl.Z));
    }

    public static Tile3f NextTile3fBetween01(this IRandom random)
    {
      return new Tile3f(random.NextFix32Between01(), random.NextFix32Between01(), random.NextFix32Between01());
    }
  }
}
