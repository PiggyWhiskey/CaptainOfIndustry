// Decompiled with JetBrains decompiler
// Type: Mafi.Vector2fExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class Vector2fExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Vector2f NextVector2f(
      this IRandom random,
      Fix32 minValueIncl,
      Fix32 maxValueExcl)
    {
      return new Vector2f(random.NextFix32(minValueIncl, maxValueExcl), random.NextFix32(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Vector2f NextVector2f(
      this IRandom random,
      Vector2f minValueIncl,
      Vector2f maxValueExcl)
    {
      return new Vector2f(random.NextFix32(minValueIncl.X, maxValueExcl.X), random.NextFix32(minValueIncl.Y, maxValueExcl.Y));
    }

    public static Vector2f NextVector2fBetween01(this IRandom random)
    {
      return new Vector2f(random.NextFix32Between01(), random.NextFix32Between01());
    }
  }
}
