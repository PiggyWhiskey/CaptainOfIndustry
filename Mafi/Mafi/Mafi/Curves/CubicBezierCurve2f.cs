// Decompiled with JetBrains decompiler
// Type: Mafi.Curves.CubicBezierCurve2f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Curves
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct CubicBezierCurve2f
  {
    private readonly ImmutableArray<Vector2f> m_controlPoints;

    /// <summary>
    /// Number of control points of this curve. It is always 1 + 3 * <see cref="P:Mafi.Curves.CubicBezierCurve2f.SegmentsCount" />. Curve with only one
    /// initial control point is considered empty.
    /// </summary>
    public int ControlPointsCount => this.m_controlPoints.Length;

    /// <summary>Number of Cubic Bezier segments.</summary>
    public int SegmentsCount => this.m_controlPoints.Length / 3;

    /// <summary>
    /// Whether curve is empty, that's when <see cref="P:Mafi.Curves.CubicBezierCurve2f.SegmentsCount" /> is zero. Note that a curve with only one
    /// initial control point is considered empty.
    /// </summary>
    public bool IsEmpty => this.m_controlPoints.Length < 4;

    /// <summary>
    /// Returns last control point of the curve. Every curve should have at least one point.
    /// </summary>
    public Vector2f LastControlPoint => this.m_controlPoints.Last;

    public CubicBezierCurve2f(ImmutableArray<Vector2f> controlPoints)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_controlPoints = new ImmutableArray<Vector2f>();
      this.m_controlPoints = controlPoints;
    }

    public Vector2f this[int i] => this.m_controlPoints[i];

    public void AddSegment(Vector2f segStartDir, Vector2f segEndDir, Vector2f segEndPt)
    {
      this.m_controlPoints.Add(segStartDir);
      this.m_controlPoints.Add(segEndDir);
      this.m_controlPoints.Add(segEndPt);
    }

    public void AddStraightSegment(Vector2f segEndPt)
    {
      Vector2f lastControlPoint = this.LastControlPoint;
      Vector2f vector2f = (segEndPt - lastControlPoint) / Fix32.FromInt(3);
      this.m_controlPoints.Add(lastControlPoint + vector2f);
      this.m_controlPoints.Add(segEndPt - vector2f);
      this.m_controlPoints.Add(segEndPt);
    }

    public CubicBezierCurve2fSampler GetUniformSampler(int subSamplesPerSegment)
    {
      int segmentsCount = this.SegmentsCount;
      Fix32[] arcLengths = new Fix32[subSamplesPerSegment * segmentsCount + 1];
      Vector2f vector2f1 = this.m_controlPoints[0];
      Fix32 zero = Fix32.Zero;
      for (int index1 = 0; index1 < segmentsCount; ++index1)
      {
        int num = 1 + index1 * subSamplesPerSegment;
        int basePtIndex = index1 * 3;
        for (int index2 = 0; index2 < subSamplesPerSegment; ++index2)
        {
          Vector2f vector2f2 = this.sampleSegment(basePtIndex, Percent.FromRatio(index2 + 1, subSamplesPerSegment));
          zero += (vector2f2 - vector2f1).Length;
          arcLengths[num + index2] = zero;
          vector2f1 = vector2f2;
        }
      }
      return new CubicBezierCurve2fSampler(this, arcLengths);
    }

    /// <summary>
    /// Returns position on the curve at given <paramref name="t" />.
    /// </summary>
    public Vector2f Sample(Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return this.sampleSegment(this.getSegmentIndex(ref t) * 3, t);
    }

    /// <summary>
    /// Returns position on the curve within segment <paramref name="segmentIndex" /> at given <paramref name="t" />.
    /// </summary>
    public Vector2f SampleSegment(int segmentIndex, Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      Assert.That<int>(segmentIndex).IsValidIndex(this.SegmentsCount);
      return this.sampleSegment(segmentIndex * 3, t);
    }

    /// <summary>
    /// Returns an accurate position on the curve within segment <paramref name="segmentIndex" /> at given <paramref name="t" />.
    /// </summary>
    public Vector2f SampleSegment64(int segmentIndex, Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      Assert.That<int>(segmentIndex).IsValidIndex(this.SegmentsCount);
      return this.sampleSegment64(segmentIndex * 3, t);
    }

    /// <summary>http://en.wikipedia.org/wiki/Bezier_curve</summary>
    private Vector2f sampleSegment(int basePtIndex, Percent t)
    {
      Percent percent = Percent.Hundred - t;
      Vector2f controlPoint = this.m_controlPoints[basePtIndex];
      return controlPoint + +(3 * t * percent) * (percent * (this.m_controlPoints[basePtIndex + 1] - controlPoint) + t * (this.m_controlPoints[basePtIndex + 2] - controlPoint)) + t * t * t * (this.m_controlPoints[basePtIndex + 3] - controlPoint);
    }

    /// <summary>
    /// http://en.wikipedia.org/wiki/Bezier_curve
    /// Uses 64 bit for internal calculations. Important if the control point magnitudes are &gt;1000
    /// </summary>
    private Vector2f sampleSegment64(int basePtIndex, Percent tPercent)
    {
      Fix64 fix64_1 = tPercent.ToFix64();
      Fix64 fix64_2 = 1 - fix64_1;
      Fix64 fix64_3 = fix64_2 * fix64_2 * fix64_2;
      Fix64 fix64_4 = 3 * fix64_1 * fix64_2 * fix64_2;
      Fix64 fix64_5 = 3 * fix64_1 * fix64_1 * fix64_2;
      Fix64 fix64_6 = fix64_1 * fix64_1 * fix64_1;
      Vector2f controlPoint1 = this.m_controlPoints[basePtIndex];
      Vector2f controlPoint2 = this.m_controlPoints[basePtIndex + 1];
      Vector2f controlPoint3 = this.m_controlPoints[basePtIndex + 2];
      Vector2f controlPoint4 = this.m_controlPoints[basePtIndex + 3];
      Vector2f vector2f;
      ref Vector2f local = ref vector2f;
      Fix64 fix64_7 = fix64_3 * controlPoint1.X.ToFix64() + fix64_4 * controlPoint2.X.ToFix64() + fix64_5 * controlPoint3.X.ToFix64() + fix64_6 * controlPoint4.X.ToFix64();
      Fix32 fix32_1 = fix64_7.ToFix32();
      fix64_7 = fix64_3 * controlPoint1.Y.ToFix64() + fix64_4 * controlPoint2.Y.ToFix64() + fix64_5 * controlPoint3.Y.ToFix64() + fix64_6 * controlPoint4.Y.ToFix64();
      Fix32 fix32_2 = fix64_7.ToFix32();
      local = new Vector2f(fix32_1, fix32_2);
      return vector2f;
    }

    /// <summary>
    /// Returns derivative (tangent) of the curve at given <paramref name="t" />.
    /// </summary>
    public Vector2f SampleDerivative(Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return this.sampleSegmentDerivative(this.getSegmentIndex(ref t) * 3, t);
    }

    /// <summary>
    /// Returns derivative (tangent) of the curve within segment <paramref name="segmentIndex" /> at given <paramref name="t" />.
    /// </summary>
    public Vector2f SampleSegmentDerivative(int segmentIndex, Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      Assert.That<int>(segmentIndex).IsValidIndex(this.SegmentsCount);
      return this.sampleSegmentDerivative(segmentIndex * 3, t);
    }

    /// <summary>http://en.wikipedia.org/wiki/Bezier_curve#Derivative</summary>
    private Vector2f sampleSegmentDerivative(int basePtIndex, Percent t)
    {
      Percent percent = Percent.Hundred - t;
      return 3 * percent * percent * (this.m_controlPoints[basePtIndex + 1] - this.m_controlPoints[basePtIndex]) + 6 * t * percent * (this.m_controlPoints[basePtIndex + 2] - this.m_controlPoints[basePtIndex + 1]) + 3 * t * t * (this.m_controlPoints[basePtIndex + 3] - this.m_controlPoints[basePtIndex + 2]);
    }

    public Vector2f SampleSecondDerivative(Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return this.sampleSegmentSecondDerivative(this.getSegmentIndex(ref t) * 3, t);
    }

    /// <summary>http://en.wikipedia.org/wiki/Bezier_curve#Derivative</summary>
    private Vector2f sampleSegmentSecondDerivative(int basePtIndex, Percent t)
    {
      return 6 * (Percent.Hundred - t) * (this.m_controlPoints[basePtIndex + 2] - 2 * this.m_controlPoints[basePtIndex + 1] + this.m_controlPoints[basePtIndex]) + 6 * t * (this.m_controlPoints[basePtIndex + 3] - 2 * this.m_controlPoints[basePtIndex + 2] + this.m_controlPoints[basePtIndex + 1]);
    }

    /// <summary>
    /// Returns curvature as a signed reciprocal radius (1 / radius) of a matching circle at that point.
    /// Curvature can be positive or negative, depending to which side is the curve curved. Value zero means
    /// perfectly straight curve (or an inflection point where the curve is momentarily exactly straight).
    /// </summary>
    public Fix32 SampleCurvature(Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      int basePtIndex = 3 * this.getSegmentIndex(ref t);
      Vector2f vector2f = this.sampleSegmentDerivative(basePtIndex, t);
      Fix64 fix64_1 = vector2f.LengthSqr.Sqrt();
      Fix64 fix64_2 = fix64_1 * fix64_1 * fix64_1;
      if (fix64_2.IsNearZero())
        return Fix32.MaxValue;
      Vector2f other = this.sampleSegmentSecondDerivative(basePtIndex, t);
      return vector2f.PseudoCross(other).DivToFix32(fix64_2);
    }

    /// <summary>
    /// Returns segment number based on given parameter <paramref name="t" /> and transforms to a parameter of
    /// position within th segment. Segments are assumed to be uniformly distributed with regards to <paramref name="t" />.
    /// </summary>
    private int getSegmentIndex(ref Percent t)
    {
      int segmentsCount = this.SegmentsCount;
      t *= segmentsCount;
      int segmentIndex = t.IntegerPart;
      if (segmentIndex >= segmentsCount)
      {
        segmentIndex = segmentsCount - 1;
        t = Percent.Hundred;
      }
      else
        t -= (segmentIndex * 100).Percent();
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return segmentIndex;
    }

    public static void Serialize(CubicBezierCurve2f value, BlobWriter writer)
    {
      ImmutableArray<Vector2f>.Serialize(value.m_controlPoints, writer);
    }

    public static CubicBezierCurve2f Deserialize(BlobReader reader)
    {
      return new CubicBezierCurve2f(ImmutableArray<Vector2f>.Deserialize(reader));
    }
  }
}
