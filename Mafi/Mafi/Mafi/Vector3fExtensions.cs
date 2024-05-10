// Decompiled with JetBrains decompiler
// Type: Mafi.Vector3fExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class Vector3fExtensions
  {
    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Vector3f NextVector3f(
      this IRandom random,
      Fix32 minValueIncl,
      Fix32 maxValueExcl)
    {
      return new Vector3f(random.NextFix32(minValueIncl, maxValueExcl), random.NextFix32(minValueIncl, maxValueExcl), random.NextFix32(minValueIncl, maxValueExcl));
    }

    /// <summary>
    /// Returns a vector where each component is pseudo-random uniformly distributed value between
    /// <paramref name="minValueIncl" /> (inclusive) and <paramref name="maxValueExcl" /> (exclusive).
    /// </summary>
    public static Vector3f NextVector3f(
      this IRandom random,
      Vector3f minValueIncl,
      Vector3f maxValueExcl)
    {
      return new Vector3f(random.NextFix32(minValueIncl.X, maxValueExcl.X), random.NextFix32(minValueIncl.Y, maxValueExcl.Y), random.NextFix32(minValueIncl.Z, maxValueExcl.Z));
    }

    public static Vector3f NextVector3fBetween01(this IRandom random)
    {
      return new Vector3f(random.NextFix32Between01(), random.NextFix32Between01(), random.NextFix32Between01());
    }
  }
}
