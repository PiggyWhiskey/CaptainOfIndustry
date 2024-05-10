// Decompiled with JetBrains decompiler
// Type: Mafi.Curves.CubicBezierCurve2fSampler
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Curves
{
  public class CubicBezierCurve2fSampler
  {
    private readonly CubicBezierCurve2f m_curve;
    private readonly Fix32[] m_arcLengths;

    public CubicBezierCurve2fSampler(CubicBezierCurve2f curve, Fix32[] arcLengths)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_curve = curve;
      this.m_arcLengths = arcLengths;
    }

    public Percent GetCurveT(Percent uniformT)
    {
      Fix32 fix32 = this.m_arcLengths.Last<Fix32>().ScaledBy(uniformT);
      int numerator1 = Array.BinarySearch<Fix32>(this.m_arcLengths, fix32);
      if (numerator1 >= 0)
        return Percent.FromRatio(numerator1, this.m_arcLengths.Length - 1);
      int numerator2 = ~numerator1;
      if (numerator2 <= 0)
        return Percent.Zero;
      if (numerator2 >= this.m_arcLengths.Length)
        return Percent.Hundred;
      Fix32 arcLength1 = this.m_arcLengths[numerator2 - 1];
      Fix32 arcLength2 = this.m_arcLengths[numerator2];
      return Percent.FromRatio(numerator2 - 1, this.m_arcLengths.Length - 1).Lerp(Percent.FromRatio(numerator2, this.m_arcLengths.Length - 1), Percent.FromRatio(fix32 - arcLength1, arcLength2 - arcLength1));
    }

    public Vector2f SampleUniform(Percent t) => this.m_curve.Sample(this.GetCurveT(t));

    public Vector2f SampleDerivativeUniform(Percent t)
    {
      return this.m_curve.SampleDerivative(this.GetCurveT(t));
    }
  }
}
