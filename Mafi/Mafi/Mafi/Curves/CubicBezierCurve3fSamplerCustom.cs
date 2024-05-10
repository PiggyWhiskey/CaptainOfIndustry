// Decompiled with JetBrains decompiler
// Type: Mafi.Curves.CubicBezierCurve3fSamplerCustom
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Curves
{
  public class CubicBezierCurve3fSamplerCustom
  {
    private readonly CubicBezierCurve3f m_curve;
    private readonly Fix32[] m_arcLengths;
    private readonly Func<CubicBezierCurve3f, int, Percent, Vector3f> m_sampleSegment;

    public Fix32 CurveLengthApprox => this.m_arcLengths[this.m_arcLengths.Length - 1];

    public CubicBezierCurve3fSamplerCustom(
      CubicBezierCurve3f curve,
      Fix32[] arcLengths,
      Func<CubicBezierCurve3f, int, Percent, Vector3f> sampleSegment)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_curve = curve;
      this.m_arcLengths = arcLengths;
      this.m_sampleSegment = sampleSegment;
    }

    public Percent GetCurveT(Fix32 arcLength)
    {
      int numerator1 = Array.BinarySearch<Fix32>(this.m_arcLengths, arcLength);
      if (numerator1 >= 0)
        return Percent.FromRatio(numerator1, this.m_arcLengths.Length - 1);
      int numerator2 = ~numerator1;
      if (numerator2 <= 0)
        return Percent.Zero;
      if (numerator2 >= this.m_arcLengths.Length)
        return Percent.Hundred;
      Fix32 arcLength1 = this.m_arcLengths[numerator2 - 1];
      Fix32 arcLength2 = this.m_arcLengths[numerator2];
      return Percent.FromRatio(numerator2 - 1, this.m_arcLengths.Length - 1).Lerp(Percent.FromRatio(numerator2, this.m_arcLengths.Length - 1), Percent.FromRatio(arcLength - arcLength1, arcLength2 - arcLength1));
    }

    public Percent GetCurveT(Percent uniformT)
    {
      return this.GetCurveT(this.CurveLengthApprox.ScaledBy(uniformT));
    }

    public Vector3f SampleAtArcLength(Fix32 arcLength)
    {
      Percent curveT = this.GetCurveT(arcLength);
      return this.m_sampleSegment(this.m_curve, this.m_curve.GetSegmentIndex(ref curveT), curveT);
    }

    public Vector3f SampleUniform(Percent t)
    {
      t = this.GetCurveT(t);
      return this.m_sampleSegment(this.m_curve, this.m_curve.GetSegmentIndex(ref t), t);
    }

    public Vector3f SampleCurveT(Percent t)
    {
      return this.m_sampleSegment(this.m_curve, this.m_curve.GetSegmentIndex(ref t), t);
    }
  }
}
