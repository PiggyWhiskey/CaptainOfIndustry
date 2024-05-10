// Decompiled with JetBrains decompiler
// Type: Mafi.Curves.CubicBezierCurve3f
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Curves
{
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public class CubicBezierCurve3f
  {
    [OnlyForSaveCompatibility(null)]
    private readonly Lyst<Vector3f> m_controlPoints;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Number of control points of this curve. It is always 1 + 3 * <see cref="P:Mafi.Curves.CubicBezierCurve3f.SegmentsCount" />. Curve with only one
    /// initial control point is considered empty.
    /// </summary>
    public int ControlPointsCount => this.m_controlPoints.Count;

    /// <summary>Number of Cubic Bezier segments.</summary>
    public int SegmentsCount => this.m_controlPoints.Count / 3;

    /// <summary>
    /// Whether curve is empty, that's when <see cref="P:Mafi.Curves.CubicBezierCurve3f.SegmentsCount" /> is zero. Note that a curve with only one
    /// initial control point is considered empty.
    /// </summary>
    public bool IsEmpty => this.m_controlPoints.Count < 4;

    /// <summary>
    /// Returns last control point of the curve. Every curve should have at least one point.
    /// </summary>
    public Vector3f LastControlPoint => this.m_controlPoints.Last;

    public IIndexable<Vector3f> ControlPoints => (IIndexable<Vector3f>) this.m_controlPoints;

    public Vector3f StartPoint => this.m_controlPoints.First;

    public Vector3f EndPoint => this.m_controlPoints.Last;

    public Vector3f StartDirectionNotNormalized
    {
      get => this.m_controlPoints[1] - this.m_controlPoints.First;
    }

    public Vector3f EndDirectionNotNormalized
    {
      get => this.m_controlPoints.Last - this.m_controlPoints.PreLast;
    }

    public CubicBezierCurve3f(IEnumerable<Vector3f> controlPoints)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_controlPoints = new Lyst<Vector3f>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_controlPoints.AddRange(controlPoints);
    }

    public Vector3f this[int i] => this.m_controlPoints[i];

    public Fix32 ApproximateCurveLength(int samplesPerSegment)
    {
      if (this.m_controlPoints.Count <= 1)
        return Fix32.Zero;
      Fix32 zero = Fix32.Zero;
      Vector3f vector3f = this.m_controlPoints.First;
      int basePtIndex = 0;
      for (int index1 = this.m_controlPoints.Count - 3; basePtIndex < index1; basePtIndex += 3)
      {
        for (int index2 = 0; index2 < samplesPerSegment; ++index2)
        {
          Vector3f other = this.sampleSegment(basePtIndex, Percent.FromRatio(index2 + 1, samplesPerSegment));
          zero += vector3f.DistanceTo(other);
          vector3f = other;
        }
      }
      return zero;
    }

    public CubicBezierCurve3fSampler GetUniformSampler(int subSamplesPerSegment)
    {
      int segmentsCount = this.SegmentsCount;
      Fix32[] arcLengths = new Fix32[subSamplesPerSegment * segmentsCount + 1];
      Vector3f vector3f = this.m_controlPoints.First;
      Fix32 zero = Fix32.Zero;
      for (int index1 = 0; index1 < segmentsCount; ++index1)
      {
        int num = 1 + index1 * subSamplesPerSegment;
        int basePtIndex = index1 * 3;
        for (int index2 = 0; index2 < subSamplesPerSegment; ++index2)
        {
          Vector3f other = this.sampleSegment(basePtIndex, Percent.FromRatio(index2 + 1, subSamplesPerSegment));
          zero += vector3f.DistanceTo(other);
          arcLengths[num + index2] = zero;
          vector3f = other;
        }
      }
      return new CubicBezierCurve3fSampler(this, arcLengths);
    }

    public CubicBezierCurve3fSamplerCustom GetUniformSamplerCustom(
      int subSamplesPerSegment,
      Func<CubicBezierCurve3f, int, Percent, Vector3f> sampleSegment)
    {
      int segmentsCount = this.SegmentsCount;
      Fix32[] arcLengths = new Fix32[subSamplesPerSegment * segmentsCount + 1];
      Vector3f vector3f1 = sampleSegment(this, 0, Percent.Zero);
      Fix32 zero = Fix32.Zero;
      for (int index1 = 0; index1 < segmentsCount; ++index1)
      {
        int num = 1 + index1 * subSamplesPerSegment;
        for (int index2 = 0; index2 < subSamplesPerSegment; ++index2)
        {
          Vector3f vector3f2 = sampleSegment(this, index1, Percent.FromRatio(index2 + 1, subSamplesPerSegment));
          zero += (vector3f2 - vector3f1).Length;
          arcLengths[num + index2] = zero;
          vector3f1 = vector3f2;
        }
      }
      return new CubicBezierCurve3fSamplerCustom(this, arcLengths, sampleSegment);
    }

    /// <summary>
    /// Returns position on the curve at given <paramref name="t" />.
    /// </summary>
    public Vector3f Sample(Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return this.sampleSegment(this.GetSegmentIndex(ref t) * 3, t);
    }

    /// <summary>
    /// Returns position on the curve within segment <paramref name="segmentIndex" /> at given <paramref name="t" />.
    /// </summary>
    public Vector3f SampleSegment(int segmentIndex, Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      Assert.That<int>(segmentIndex).IsValidIndex(this.SegmentsCount);
      return this.sampleSegment(segmentIndex * 3, t);
    }

    /// <summary>
    /// Returns an accurate position on the curve within segment <paramref name="segmentIndex" /> at given <paramref name="t" />.
    /// </summary>
    public Vector3f SampleSegment64(int segmentIndex, Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      Assert.That<int>(segmentIndex).IsValidIndex(this.SegmentsCount);
      return this.sampleSegment64(segmentIndex * 3, t);
    }

    /// <summary>http://en.wikipedia.org/wiki/Bezier_curve</summary>
    private Vector3f sampleSegment(int basePtIndex, Percent t)
    {
      Percent percent = Percent.Hundred - t;
      Vector3f controlPoint = this.m_controlPoints[basePtIndex];
      return controlPoint + +(3 * t * percent) * (percent * (this.m_controlPoints[basePtIndex + 1] - controlPoint) + t * (this.m_controlPoints[basePtIndex + 2] - controlPoint)) + t * t * t * (this.m_controlPoints[basePtIndex + 3] - controlPoint);
    }

    /// <summary>
    /// http://en.wikipedia.org/wiki/Bezier_curve
    /// Uses 64 bit for internal calculations. Important if the control point magnitudes are &gt;1000
    /// </summary>
    private Vector3f sampleSegment64(int basePtIndex, Percent tPercent)
    {
      Fix64 fix64_1 = tPercent.ToFix64();
      Fix64 fix64_2 = 1 - fix64_1;
      Fix64 fix64_3 = fix64_2 * fix64_2 * fix64_2;
      Fix64 fix64_4 = 3 * fix64_1 * fix64_2 * fix64_2;
      Fix64 fix64_5 = 3 * fix64_1 * fix64_1 * fix64_2;
      Fix64 fix64_6 = fix64_1 * fix64_1 * fix64_1;
      Vector3f controlPoint1 = this.m_controlPoints[basePtIndex];
      Vector3f controlPoint2 = this.m_controlPoints[basePtIndex + 1];
      Vector3f controlPoint3 = this.m_controlPoints[basePtIndex + 2];
      Vector3f controlPoint4 = this.m_controlPoints[basePtIndex + 3];
      Vector3f vector3f;
      ref Vector3f local = ref vector3f;
      Fix64 fix64_7 = fix64_3 * controlPoint1.X.ToFix64() + fix64_4 * controlPoint2.X.ToFix64() + fix64_5 * controlPoint3.X.ToFix64() + fix64_6 * controlPoint4.X.ToFix64();
      Fix32 fix32_1 = fix64_7.ToFix32();
      fix64_7 = fix64_3 * controlPoint1.Y.ToFix64() + fix64_4 * controlPoint2.Y.ToFix64() + fix64_5 * controlPoint3.Y.ToFix64() + fix64_6 * controlPoint4.Y.ToFix64();
      Fix32 fix32_2 = fix64_7.ToFix32();
      fix64_7 = fix64_3 * controlPoint1.Z.ToFix64() + fix64_4 * controlPoint2.Z.ToFix64() + fix64_5 * controlPoint3.Z.ToFix64() + fix64_6 * controlPoint4.Z.ToFix64();
      Fix32 fix32_3 = fix64_7.ToFix32();
      local = new Vector3f(fix32_1, fix32_2, fix32_3);
      return vector3f;
    }

    /// <summary>
    /// Returns derivative (tangent) of the curve at given <paramref name="t" />.
    /// </summary>
    public Vector3f SampleDerivative(Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return this.sampleSegmentDerivative(this.GetSegmentIndex(ref t) * 3, t);
    }

    /// <summary>
    /// Returns derivative (tangent) of the curve within segment <paramref name="segmentIndex" /> at given <paramref name="t" />.
    /// </summary>
    public Vector3f SampleSegmentDerivative(int segmentIndex, Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      Assert.That<int>(segmentIndex).IsValidIndex(this.SegmentsCount);
      return this.sampleSegmentDerivative(segmentIndex * 3, t);
    }

    /// <summary>http://en.wikipedia.org/wiki/Bezier_curve#Derivative</summary>
    private Vector3f sampleSegmentDerivative(int basePtIndex, Percent t)
    {
      Percent percent = Percent.Hundred - t;
      return 3 * percent * percent * (this.m_controlPoints[basePtIndex + 1] - this.m_controlPoints[basePtIndex]) + 6 * t * percent * (this.m_controlPoints[basePtIndex + 2] - this.m_controlPoints[basePtIndex + 1]) + 3 * t * t * (this.m_controlPoints[basePtIndex + 3] - this.m_controlPoints[basePtIndex + 2]);
    }

    public Vector3f SampleSecondDerivative(Percent t)
    {
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return this.sampleSegmentSecondDerivative(this.GetSegmentIndex(ref t) * 3, t);
    }

    /// <summary>http://en.wikipedia.org/wiki/Bezier_curve#Derivative</summary>
    private Vector3f sampleSegmentSecondDerivative(int basePtIndex, Percent t)
    {
      return 6 * (Percent.Hundred - t) * (this.m_controlPoints[basePtIndex + 2] - 2 * this.m_controlPoints[basePtIndex + 1] + this.m_controlPoints[basePtIndex]) + 6 * t * (this.m_controlPoints[basePtIndex + 3] - 2 * this.m_controlPoints[basePtIndex + 2] + this.m_controlPoints[basePtIndex + 1]);
    }

    /// <summary>
    /// Whether given <paramref name="segmentIndex" /> is just a straight line.
    /// </summary>
    public bool IsStraightSegment(int segmentIndex)
    {
      Assert.That<int>(segmentIndex).IsValidIndex(this.SegmentsCount);
      int index = segmentIndex * 3;
      Vector3f vector3f = this.m_controlPoints[index + 3] - this.m_controlPoints[index];
      return vector3f.Cross(this.m_controlPoints[index] - this.m_controlPoints[index + 1]).IsNearZero() && vector3f.Cross(this.m_controlPoints[index + 3] - this.m_controlPoints[index + 2]).IsNearZero();
    }

    /// <summary>
    /// Returns segment number based on given parameter <paramref name="t" /> and transforms to a parameter of
    /// position within th segment. Segments are assumed to be uniformly distributed with regards to <paramref name="t" />.
    /// </summary>
    public int GetSegmentIndex(ref Percent t)
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

    [MustUseReturnValue]
    public CubicBezierCurve3f CreatedTranslatedCopy(Vector2f offset)
    {
      return new CubicBezierCurve3f((IEnumerable<Vector3f>) this.m_controlPoints.ToArray<Vector3f>((Func<Vector3f, Vector3f>) (x => x + offset)));
    }

    [MustUseReturnValue]
    public CubicBezierCurve3f ReverseControlPoints()
    {
      Vector3f[] array = this.m_controlPoints.ToArray();
      Array.Reverse((Array) array);
      return new CubicBezierCurve3f((IEnumerable<Vector3f>) array);
    }

    public static void Serialize(CubicBezierCurve3f value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CubicBezierCurve3f>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CubicBezierCurve3f.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<Vector3f>.Serialize(this.m_controlPoints, writer);
    }

    public static CubicBezierCurve3f Deserialize(BlobReader reader)
    {
      CubicBezierCurve3f cubicBezierCurve3f;
      if (reader.TryStartClassDeserialization<CubicBezierCurve3f>(out cubicBezierCurve3f))
        reader.EnqueueDataDeserialization((object) cubicBezierCurve3f, CubicBezierCurve3f.s_deserializeDataDelayedAction);
      return cubicBezierCurve3f;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<CubicBezierCurve3f>(this, "m_controlPoints", (object) Lyst<Vector3f>.Deserialize(reader));
    }

    static CubicBezierCurve3f()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      CubicBezierCurve3f.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((CubicBezierCurve3f) obj).SerializeData(writer));
      CubicBezierCurve3f.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((CubicBezierCurve3f) obj).DeserializeData(reader));
    }
  }
}
