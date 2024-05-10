// Decompiled with JetBrains decompiler
// Type: Mafi.Curves.CubicBezierCurve3fBuilder
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Curves
{
  public class CubicBezierCurve3fBuilder
  {
    private readonly Lyst<Vector3f> m_controlPoints;

    public int ControlPointsCount => this.m_controlPoints.Count;

    public int SegmentsCount => this.m_controlPoints.Count / 3;

    public Vector3f LastControlPoint => this.m_controlPoints.Last;

    public CubicBezierCurve3fBuilder(Vector3f initialPosition)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_controlPoints = new Lyst<Vector3f>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_controlPoints.Add(initialPosition);
    }

    public void AddSegment(Vector3f segStartDir, Vector3f segEndDir, Vector3f segEndPt)
    {
      this.m_controlPoints.Add(segStartDir);
      this.m_controlPoints.Add(segEndDir);
      this.m_controlPoints.Add(segEndPt);
    }

    public void AddStraightSegment(Vector3f segEndPt)
    {
      Vector3f last = this.m_controlPoints.Last;
      Vector3f vector3f = (segEndPt - last) / Fix32.Three;
      this.m_controlPoints.Add(last + vector3f);
      this.m_controlPoints.Add(segEndPt - vector3f);
      this.m_controlPoints.Add(segEndPt);
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
    /// Optimizes the curve by merging consecutive straight segments.
    /// </summary>
    public void Optimize()
    {
      bool flag1 = this.IsStraightSegment(0);
      int segmentIndex = 1;
      while (segmentIndex < this.SegmentsCount)
      {
        bool flag2 = this.IsStraightSegment(segmentIndex);
        if (flag1 && flag2)
        {
          int index = segmentIndex * 3;
          if ((this.m_controlPoints[index] - this.m_controlPoints[index - 3]).HasAlmostSameDirection(this.m_controlPoints[index + 3] - this.m_controlPoints[index]))
          {
            Vector3f vector3f = (this.m_controlPoints[index + 3] - this.m_controlPoints[index - 3]) / Fix32.Three;
            this.m_controlPoints[index - 2] = this.m_controlPoints[index - 3] + vector3f;
            this.m_controlPoints[index + 2] = this.m_controlPoints[index + 3] - vector3f;
            this.m_controlPoints.RemoveRange(index - 1, 3);
            --segmentIndex;
            Assert.That<bool>(this.IsStraightSegment(segmentIndex)).IsTrue();
          }
        }
        ++segmentIndex;
        flag1 = flag2;
      }
    }

    public CubicBezierCurve3f CreateCurve()
    {
      return new CubicBezierCurve3f((IEnumerable<Vector3f>) this.m_controlPoints);
    }
  }
}
