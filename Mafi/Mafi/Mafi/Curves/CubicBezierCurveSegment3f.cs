// Decompiled with JetBrains decompiler
// Type: Mafi.Curves.CubicBezierCurveSegment3f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Curves
{
  /// <summary>
  /// Single segment of cubic bezier curve specified by 4 points.
  /// </summary>
  /// <remarks>http://en.wikipedia.org/wiki/Bezier_curve</remarks>
  public class CubicBezierCurveSegment3f
  {
    /// <summary>Start point of the curve.</summary>
    public readonly Vector3f P0;
    /// <summary>
    /// Start control point that specifies tangent and "speed" at the start point.
    /// </summary>
    public readonly Vector3f P1;
    /// <summary>
    /// End control point that specifies tangent and "speed" at the end point.
    /// </summary>
    public readonly Vector3f P2;
    /// <summary>End point of the curve.</summary>
    public readonly Vector3f P3;

    public CubicBezierCurveSegment3f(Vector3f p0, Vector3f p1, Vector3f p2, Vector3f p3)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Vector3f>(p0).IsNotNear(p3, Fix32.Epsilon, "Start and end points of a curve should not be identical.");
      this.P0 = p0;
      this.P1 = p1;
      this.P2 = p2;
      this.P3 = p3;
    }

    /// <summary>Creates straight curve.</summary>
    /// <remarks>Keep in mind that when `t == x` it does not correspond to x% among the curve.</remarks>
    public CubicBezierCurveSegment3f(Vector3f p0, Vector3f p3)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Vector3f>(p0).IsNotNear(p3, Fix32.Epsilon, "Start and end points of a curve should not be identical.");
      this.P0 = p0;
      this.P1 = this.P2 = p0.Average(p3);
      this.P3 = p3;
      Assert.That<bool>(this.IsLine).IsTrue();
    }

    /// <summary>Whether this segment is a straight line.</summary>
    public bool IsLine
    {
      get
      {
        Vector3f vector3f = this.P3 - this.P0;
        return vector3f.Cross(this.P0 - this.P1).IsZero && vector3f.Cross(this.P3 - this.P2).IsZero;
      }
    }

    /// <summary>
    /// Returns position on the curve segment at given <paramref name="t" /> ∈ [0, 1]. Note that <paramref name="t" />
    /// is not linear with distance on the curve.
    /// </summary>
    public Vector3f Sample(Percent t)
    {
      Percent percent = Percent.Hundred - t;
      return percent * percent * percent * this.P0 + 3 * t * percent * (percent * this.P1 + t * this.P2) + t * t * t * this.P3;
    }

    /// <summary>
    /// Returns derivative at given <paramref name="t" /> ∈ [0, 1]. Note that <paramref name="t" /> is not linear with
    /// distance on the curve.
    /// </summary>
    public Vector3f SampleDerivative(Percent t)
    {
      Percent percent = Percent.Hundred - t;
      return 3 * percent * percent * (this.P1 - this.P0) + 6 * t * percent * (this.P2 - this.P1) + 3 * t * t * (this.P3 - this.P2);
    }
  }
}
