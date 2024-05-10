// Decompiled with JetBrains decompiler
// Type: Mafi.Vector2iExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class Vector2iExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Vector2i NextVector2i(this IRandom random, int minValueIncl, int maxValueExcl)
    {
      return new Vector2i(random.NextInt(minValueIncl, maxValueExcl), random.NextInt(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Vector2i NextVector2i(
      this IRandom random,
      Vector2i minValueIncl,
      Vector2i maxValueExcl)
    {
      return new Vector2i(random.NextInt(minValueIncl.X, maxValueExcl.X), random.NextInt(minValueIncl.Y, maxValueExcl.Y));
    }
  }
}
