// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.RayCaster
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Numerics
{
  public class RayCaster
  {
    private static readonly Vector3f[] s_normals;

    public static Vector3f GetNormal(int normalIndex) => RayCaster.s_normals[normalIndex];

    /// <summary>
    /// Computes intersection wit a unit cube (cube with one corner at [0, 0, 0] and other corner at [1, 1, 1]).
    /// </summary>
    /// <remarks>Based on http://www.siggraph.org/education/materials/HyperGraph/raytrace/rtinter3.htm</remarks>
    public static bool IntersectUnitCube(
      Ray3f ray,
      out Percent tMin,
      out int minNormalIndex,
      out Percent tMax,
      out int maxNormalIndex)
    {
      tMin = Percent.MinValue;
      tMax = Percent.MaxValue;
      minNormalIndex = -1;
      maxNormalIndex = -1;
      if (ray.Direction.X.IsNearZero())
      {
        if (ray.Origin.X < 0 || ray.Origin.X > 1)
          return false;
      }
      else
      {
        Percent percent1 = Percent.FromRatio(-ray.Origin.X, ray.Direction.X);
        Percent percent2 = Percent.FromRatio((Fix32) 1 - ray.Origin.X, ray.Direction.X);
        if (ray.Direction.X.IsPositive)
        {
          if (percent1 > tMin)
          {
            tMin = percent1;
            minNormalIndex = 1;
          }
          if (percent2 < tMax)
          {
            tMax = percent2;
            maxNormalIndex = 0;
          }
        }
        else
        {
          if (percent2 > tMin)
          {
            tMin = percent2;
            minNormalIndex = 0;
          }
          if (percent1 < tMax)
          {
            tMax = percent1;
            maxNormalIndex = 1;
          }
        }
        if (tMax.IsNegative || tMin > tMax)
          return false;
      }
      if (ray.Direction.Y.IsNearZero())
      {
        if (ray.Origin.Y < 0 || ray.Origin.Y > 1)
          return false;
      }
      else
      {
        Percent percent3 = Percent.FromRatio(-ray.Origin.Y, ray.Direction.Y);
        Percent percent4 = Percent.FromRatio((Fix32) 1 - ray.Origin.Y, ray.Direction.Y);
        if (ray.Direction.Y.IsPositive)
        {
          if (percent3 > tMin)
          {
            tMin = percent3;
            minNormalIndex = 3;
          }
          if (percent4 < tMax)
          {
            tMax = percent4;
            maxNormalIndex = 2;
          }
        }
        else
        {
          if (percent4 > tMin)
          {
            tMin = percent4;
            minNormalIndex = 2;
          }
          if (percent3 < tMax)
          {
            tMax = percent3;
            maxNormalIndex = 3;
          }
        }
        if (tMax.IsNegative || tMin > tMax)
          return false;
      }
      if (ray.Direction.Z.IsNearZero())
      {
        if (ray.Origin.Z < 0 || ray.Origin.Z > 1)
          return false;
      }
      else
      {
        Percent percent5 = Percent.FromRatio(-ray.Origin.Z, ray.Direction.Z);
        Percent percent6 = Percent.FromRatio((Fix32) 1 - ray.Origin.Z, ray.Direction.Z);
        if (ray.Direction.Z.IsPositive)
        {
          if (percent5 > tMin)
          {
            tMin = percent5;
            minNormalIndex = 5;
          }
          if (percent6 < tMax)
          {
            tMax = percent6;
            maxNormalIndex = 4;
          }
        }
        else
        {
          if (percent6 > tMin)
          {
            tMin = percent6;
            minNormalIndex = 4;
          }
          if (percent5 < tMax)
          {
            tMax = percent5;
            maxNormalIndex = 5;
          }
        }
        if (tMax.IsNegative || tMin > tMax)
          return false;
      }
      Assert.That<int>(minNormalIndex).IsNotNegative("Normal was not set.");
      Assert.That<int>(maxNormalIndex).IsNotNegative("Normal was not set.");
      Assert.That<Percent>(tMin).IsLessOrEqual(tMax);
      return true;
    }

    public static bool IntersectUnitCube(Ray3f ray, out Percent tMin, out Percent tMax)
    {
      return RayCaster.IntersectUnitCube(ray, out tMin, out int _, out tMax, out int _);
    }

    /// <summary>
    /// Computes intersection of given <paramref name="ray" /> and axis-aligned bounding box <paramref name="aabb" />.
    /// </summary>
    public static bool IntersectAabb(
      Ray3f ray,
      Aabb aabb,
      out Percent tMin,
      out int minNormalIndex,
      out Percent tMax,
      out int maxNormalIndex)
    {
      Vector3f direction = ray.Direction / aabb.Size;
      if (!RayCaster.IntersectUnitCube(new Ray3f((ray.Origin - aabb.Min) / aabb.Size, direction), out tMin, out minNormalIndex, out tMax, out maxNormalIndex))
        return false;
      Fix32 length = direction.Length;
      if (length.IsZero)
      {
        Log.Error("Ray direction is zero.");
        return false;
      }
      tMin /= length;
      tMax /= length;
      return true;
    }

    public static bool IntersectPlane(Ray3f ray, Plane plane, out Percent t)
    {
      Fix64 denominator = ray.Direction.Dot(plane.Normal);
      if (denominator.IsZero)
      {
        t = Percent.Zero;
        return false;
      }
      t = Percent.FromRatio(-ray.Origin.Dot(plane.Normal) - plane.DistanceFromOrigin.ToFix64(), denominator);
      return true;
    }

    /// <summary>
    /// Computes intersection of given <paramref name="ray" /> and unit sphere.
    /// </summary>
    public static bool IntersectUnitSphere(Ray3f ray, out Percent tMin, out Percent tMax)
    {
      Fix64 fix64_1 = ray.Origin.Dot(ray.Direction);
      Fix64 fix64_2 = ray.Origin.Dot(ray.Origin);
      Fix64 fix64_3 = fix64_1 * fix64_1 - fix64_2 + Fix64.One;
      if (fix64_3.IsNegative)
      {
        tMin = Percent.Zero;
        tMax = Percent.Zero;
        return false;
      }
      Fix64 fix64_4 = fix64_3.Sqrt();
      tMin = Percent.FromFix64(-fix64_1 - fix64_4);
      tMax = Percent.FromFix64(-fix64_1 + fix64_4);
      return true;
    }

    public RayCaster()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static RayCaster()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      RayCaster.s_normals = new Vector3f[6]
      {
        Vector3f.UnitX,
        -Vector3f.UnitX,
        Vector3f.UnitY,
        -Vector3f.UnitY,
        Vector3f.UnitZ,
        -Vector3f.UnitZ
      };
    }
  }
}
