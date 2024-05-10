// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon2fFast
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>
  /// 2D polygon with bunch of stuff precomputed for fast computations.
  /// </summary>
  public class Polygon2fFast
  {
    /// <summary>
    /// Extended array of vertices of this polygon (the first vertex is copied to the end to make some
    /// operations faster).
    /// </summary>
    public readonly ImmutableArray<Vector2f> VerticesExt;
    public readonly int VerticesCount;
    /// <summary>
    /// Vector at i-th index represents edge starting at the i-th vertex.
    /// </summary>
    public readonly ImmutableArray<Vector2f> EdgeVectors;
    public readonly ImmutableArray<Fix64> EdgeLengthsSqr;
    public readonly Vector2f BoundingBoxMin;
    public readonly Vector2f BoundingBoxMax;
    private readonly Aabb m_aabb;

    private Polygon2fFast(ImmutableArray<Vector2f> verticesExt)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Vector2f>(verticesExt.First).IsEqualTo<Vector2f>(verticesExt.Last);
      Assert.That<int>(verticesExt.Length).IsGreaterOrEqual(2);
      this.VerticesExt = verticesExt;
      this.VerticesCount = verticesExt.Length - 1;
      this.BoundingBoxMin = Vector2f.MaxValue;
      this.BoundingBoxMax = Vector2f.MinValue;
      ImmutableArrayBuilder<Vector2f> immutableArrayBuilder1 = new ImmutableArrayBuilder<Vector2f>(this.VerticesCount);
      ImmutableArrayBuilder<Fix64> immutableArrayBuilder2 = new ImmutableArrayBuilder<Fix64>(this.VerticesCount);
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector2f rhs = verticesExt[index];
        Vector2f vector2f = verticesExt[index + 1] - rhs;
        immutableArrayBuilder1[index] = vector2f;
        immutableArrayBuilder2[index] = vector2f.LengthSqr;
        this.BoundingBoxMin = this.BoundingBoxMin.Min(rhs);
        this.BoundingBoxMax = this.BoundingBoxMax.Max(rhs);
      }
      this.m_aabb = new Aabb(this.BoundingBoxMin.ExtendZ((Fix32) 0), this.BoundingBoxMax.ExtendZ((Fix32) 0));
      this.EdgeVectors = immutableArrayBuilder1.GetImmutableArrayAndClear();
      this.EdgeLengthsSqr = immutableArrayBuilder2.GetImmutableArrayAndClear();
    }

    public static bool TryCreateWithoutDegenerateEdges(
      ReadOnlyArraySlice<Vector2f> vertices,
      out Polygon2fFast polygon,
      out string error)
    {
      polygon = (Polygon2fFast) null;
      if (vertices.Length < 2)
      {
        error = string.Format("Degenerate polygon with {0} vertices.", (object) vertices.Length);
        return false;
      }
      Lyst<Vector2f> lyst = new Lyst<Vector2f>(vertices.Length + 1)
      {
        vertices[0]
      };
      for (int index = 1; index < vertices.Length; ++index)
      {
        if (!(vertices[index - 1] == vertices[index]))
          lyst.Add(vertices[index]);
      }
      if (lyst.First != lyst.Last)
        lyst.Add(lyst.First);
      if (lyst.Count < 3)
      {
        error = string.Format("Degenerate polygon with {0} vertices (after filtering duplicates).", (object) (lyst.Count - 1));
        return false;
      }
      polygon = new Polygon2fFast(lyst.ToImmutableArray());
      error = "";
      return true;
    }

    public bool Contains(Vector2f p)
    {
      if (this.VerticesCount <= 2 || !this.m_aabb.Contains(p.ExtendZ((Fix32) 0)))
        return false;
      int num = 0;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector2f vector2f1 = this.VerticesExt[index];
        if (vector2f1 == p)
          return true;
        Vector2f vector2f2 = this.VerticesExt[index + 1];
        Vector2f vector2f3 = p - vector2f1;
        if (vector2f1.Y <= p.Y)
        {
          if (vector2f2.Y > p.Y && vector2f3.PseudoCross(this.EdgeVectors[index]) > 0)
            ++num;
        }
        else if (vector2f2.Y <= p.Y && vector2f3.PseudoCross(this.EdgeVectors[index]) < 0)
          --num;
      }
      return num != 0;
    }

    public Fix64 DistanceSqrTo(Vector2f p)
    {
      if (this.VerticesCount <= 2)
      {
        if (this.VerticesCount == 1)
          return p.DistanceSqrTo(this.VerticesExt.First);
        return this.VerticesCount == 2 ? new Line2f(this.VerticesExt[0], this.VerticesExt[1]).DistanceSqrToLineSegment(p) : Fix64.Zero;
      }
      Fix64 fix64_1 = Fix64.MaxValue;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector2f other = this.VerticesExt[index];
        Fix64 numerator = this.EdgeVectors[index].Dot(p - other);
        Fix64 fix64_2;
        if (numerator.IsNotPositive)
        {
          fix64_2 = p.DistanceSqrTo(other);
        }
        else
        {
          Vector2f vector2f = this.VerticesExt[index + 1];
          Fix64 denominator = this.EdgeLengthsSqr[index];
          fix64_2 = !(numerator >= denominator) ? p.DistanceSqrTo(other.Lerp(vector2f, Percent.FromRatio(numerator, denominator))) : p.DistanceSqrTo(vector2f);
        }
        if (fix64_2 < fix64_1)
          fix64_1 = fix64_2;
      }
      return fix64_1;
    }

    public Fix64 DistanceSqrTo(Vector2f p, out bool closestToVertex, out int closestIndex)
    {
      closestToVertex = true;
      closestIndex = 0;
      if (this.VerticesCount <= 2)
      {
        if (this.VerticesCount == 1)
          return p.DistanceSqrTo(this.VerticesExt.First);
        if (this.VerticesCount != 2)
          return Fix64.Zero;
        Vector2f edgeVector = this.EdgeVectors[0];
        Fix64 numerator = edgeVector.Dot(p - this.VerticesExt[0]);
        if (numerator.IsNegative)
          return p.DistanceSqrTo(this.VerticesExt[0]);
        Fix64 denominator = this.EdgeLengthsSqr[0];
        if (numerator > denominator)
        {
          closestIndex = 1;
          return p.DistanceSqrTo(this.VerticesExt[1]);
        }
        edgeVector = this.VerticesExt[0];
        Vector2f other = edgeVector.Lerp(this.VerticesExt[1], Percent.FromRatio(numerator, denominator));
        closestToVertex = false;
        return p.DistanceSqrTo(other);
      }
      Fix64 fix64_1 = Fix64.MaxValue;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector2f other = this.VerticesExt[index];
        Fix64 numerator = this.EdgeVectors[index].Dot(p - other);
        if (numerator.IsNotPositive)
        {
          Fix64 fix64_2 = p.DistanceSqrTo(other);
          if (fix64_2 < fix64_1)
          {
            closestToVertex = true;
            closestIndex = index;
            fix64_1 = fix64_2;
          }
        }
        else
        {
          Vector2f vector2f = this.VerticesExt[index + 1];
          Fix64 denominator = this.EdgeLengthsSqr[index];
          if (numerator >= denominator)
          {
            Fix64 fix64_3 = p.DistanceSqrTo(vector2f);
            if (fix64_3 < fix64_1)
            {
              closestToVertex = true;
              closestIndex = (index + 1) % this.VerticesCount;
              fix64_1 = fix64_3;
            }
          }
          else
          {
            Fix64 fix64_4 = p.DistanceSqrTo(other.Lerp(vector2f, Percent.FromRatio(numerator, denominator)));
            if (fix64_4 < fix64_1)
            {
              closestToVertex = false;
              closestIndex = index;
              fix64_1 = fix64_4;
            }
          }
        }
      }
      return fix64_1;
    }

    public bool Overlaps(Rect2i rect)
    {
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        if (rect.Contains(this.VerticesExt[index].RoundedVector2i))
          return true;
      }
      Vector2i vector2i = rect.Min;
      if (this.Contains(vector2i.Vector2f))
        return true;
      vector2i = rect.Min;
      vector2i = vector2i.AddX(rect.Size.X);
      if (this.Contains(vector2i.Vector2f))
        return true;
      vector2i = rect.Max;
      if (this.Contains(vector2i.Vector2f))
        return true;
      vector2i = rect.Min;
      vector2i = vector2i.AddX(rect.Size.Y);
      if (this.Contains(vector2i.Vector2f))
        return true;
      vector2i = rect.Min;
      Vector2f vector2f1 = vector2i.Vector2f;
      vector2i = rect.Min;
      vector2i = vector2i.AddY(rect.Size.Y);
      Vector2f vector2f2 = vector2i.Vector2f;
      if (lineIntersectionWith(new Line2f(vector2f1, vector2f2)))
        return true;
      vector2i = rect.Min;
      vector2i = vector2i.AddY(rect.Size.Y);
      Vector2f vector2f3 = vector2i.Vector2f;
      vector2i = rect.Max;
      Vector2f vector2f4 = vector2i.Vector2f;
      if (lineIntersectionWith(new Line2f(vector2f3, vector2f4)))
        return true;
      vector2i = rect.Max;
      Vector2f vector2f5 = vector2i.Vector2f;
      vector2i = rect.Min;
      vector2i = vector2i.AddX(rect.Size.X);
      Vector2f vector2f6 = vector2i.Vector2f;
      if (lineIntersectionWith(new Line2f(vector2f5, vector2f6)))
        return true;
      vector2i = rect.Min;
      vector2i = vector2i.AddX(rect.Size.X);
      Vector2f vector2f7 = vector2i.Vector2f;
      vector2i = rect.Min;
      Vector2f vector2f8 = vector2i.Vector2f;
      return lineIntersectionWith(new Line2f(vector2f7, vector2f8));

      bool lineIntersectionWith(Line2f rectEdge)
      {
        for (int index = 0; index < this.VerticesCount; ++index)
        {
          if (new Line2f(this.VerticesExt[index], this.VerticesExt[index + 1]).IntersectsWithSegment(rectEdge))
            return true;
        }
        return false;
      }
    }
  }
}
