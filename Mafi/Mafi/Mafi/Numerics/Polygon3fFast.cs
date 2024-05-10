// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon3fFast
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
  public class Polygon3fFast
  {
    /// <summary>
    /// Extended array of vertices of this polygon (the first vertex is copied to the end to make some
    /// operations faster).
    /// </summary>
    public readonly ImmutableArray<Vector3f> VerticesExt;
    public readonly int VerticesCount;
    /// <summary>
    /// Vector at i-th index represents edge starting at the i-th vertex.
    /// </summary>
    public readonly ImmutableArray<Vector3f> EdgeVectors;
    public readonly ImmutableArray<Fix64> EdgeLengthsSqr;
    public readonly ImmutableArray<Fix64> EdgeLengths2DSqr;
    public readonly Vector3f BoundingBoxMin;
    public readonly Vector3f BoundingBoxMax;

    private Polygon3fFast(ImmutableArray<Vector3f> verticesExt)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Vector3f>(verticesExt.First).IsEqualTo<Vector3f>(verticesExt.Last);
      Assert.That<int>(verticesExt.Length).IsGreaterOrEqual(2);
      this.VerticesExt = verticesExt;
      this.VerticesCount = verticesExt.Length - 1;
      this.BoundingBoxMin = Vector3f.MaxValue;
      this.BoundingBoxMax = Vector3f.MinValue;
      ImmutableArrayBuilder<Vector3f> immutableArrayBuilder1 = new ImmutableArrayBuilder<Vector3f>(this.VerticesCount);
      ImmutableArrayBuilder<Fix64> immutableArrayBuilder2 = new ImmutableArrayBuilder<Fix64>(this.VerticesCount);
      ImmutableArrayBuilder<Fix64> immutableArrayBuilder3 = new ImmutableArrayBuilder<Fix64>(this.VerticesCount);
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3f rhs = verticesExt[index];
        Vector3f vector3f = verticesExt[index + 1] - rhs;
        immutableArrayBuilder1[index] = vector3f;
        immutableArrayBuilder2[index] = vector3f.LengthSqr;
        immutableArrayBuilder3[index] = (verticesExt[index + 1].Xy - rhs.Xy).LengthSqr;
        this.BoundingBoxMin = this.BoundingBoxMin.Min(rhs);
        this.BoundingBoxMax = this.BoundingBoxMax.Max(rhs);
      }
      this.EdgeVectors = immutableArrayBuilder1.GetImmutableArrayAndClear();
      this.EdgeLengthsSqr = immutableArrayBuilder2.GetImmutableArrayAndClear();
      this.EdgeLengths2DSqr = immutableArrayBuilder3.GetImmutableArrayAndClear();
    }

    public static bool TryCreateWithoutDegenerateEdges(
      ReadOnlyArraySlice<Vector3f> vertices,
      bool roundHeightsToWholeTiles,
      int clampZMinMax,
      out Polygon3fFast polygon,
      out string error)
    {
      polygon = (Polygon3fFast) null;
      if (vertices.Length < 2)
      {
        error = string.Format("Degenerate polygon with {0} vertices.", (object) vertices.Length);
        return false;
      }
      Lyst<Vector3f> lyst = new Lyst<Vector3f>(vertices.Length + 1)
      {
        getVertex(0)
      };
      for (int index = 1; index < vertices.Length; ++index)
      {
        if (!(vertices[index - 1] == vertices[index]))
          lyst.Add(getVertex(index));
      }
      if (lyst.First != lyst.Last)
        lyst.Add(lyst.First);
      if (lyst.Count < 3)
      {
        error = string.Format("Degenerate polygon with {0} vertices (after filtering duplicates).", (object) (lyst.Count - 1));
        return false;
      }
      polygon = new Polygon3fFast(lyst.ToImmutableArray());
      error = "";
      return true;

      Vector3f getVertex(int i)
      {
        Vector3f vertex = vertices[i];
        if (roundHeightsToWholeTiles)
          vertex = new Vector3f(vertex.X, vertex.Y, vertex.Z.Rounded());
        if (clampZMinMax >= 0)
          vertex = new Vector3f(vertex.X, vertex.Y, vertex.Z.Clamp((Fix32) -clampZMinMax, (Fix32) clampZMinMax));
        return vertex;
      }
    }

    public bool Contains2D(Vector2f p)
    {
      if (this.VerticesCount <= 2)
        return false;
      int num = 0;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3f edgeVector = this.VerticesExt[index];
        Vector2f xy1 = edgeVector.Xy;
        if (xy1 == p)
          return true;
        edgeVector = this.VerticesExt[index + 1];
        Vector2f xy2 = edgeVector.Xy;
        Vector2f vector2f = p - xy1;
        if (xy1.Y <= p.Y)
        {
          if (xy2.Y > p.Y)
          {
            ref Vector2f local = ref vector2f;
            edgeVector = this.EdgeVectors[index];
            Vector2f xy3 = edgeVector.Xy;
            if (local.PseudoCross(xy3) > 0)
              ++num;
          }
        }
        else if (xy2.Y <= p.Y)
        {
          ref Vector2f local = ref vector2f;
          edgeVector = this.EdgeVectors[index];
          Vector2f xy4 = edgeVector.Xy;
          if (local.PseudoCross(xy4) < 0)
            --num;
        }
      }
      return num != 0;
    }

    public Fix64 Distance2DSqrTo(Vector2f p)
    {
      if (this.VerticesCount <= 2)
      {
        if (this.VerticesCount == 1)
          return p.DistanceSqrTo(this.VerticesExt.First.Xy);
        return this.VerticesCount == 2 ? new Line2f(this.VerticesExt[0].Xy, this.VerticesExt[1].Xy).DistanceSqrToLineSegment(p) : Fix64.Zero;
      }
      Fix64 fix64_1 = Fix64.MaxValue;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3f edgeVector = this.VerticesExt[index];
        Vector2f xy1 = edgeVector.Xy;
        edgeVector = this.EdgeVectors[index];
        Fix64 numerator = edgeVector.Xy.Dot(p - xy1);
        Fix64 fix64_2;
        if (numerator.IsNotPositive)
        {
          fix64_2 = p.DistanceSqrTo(xy1);
        }
        else
        {
          edgeVector = this.VerticesExt[index + 1];
          Vector2f xy2 = edgeVector.Xy;
          Fix64 denominator = this.EdgeLengths2DSqr[index];
          fix64_2 = !(numerator >= denominator) ? p.DistanceSqrTo(xy1.Lerp(xy2, Percent.FromRatio(numerator, denominator))) : p.DistanceSqrTo(xy2);
        }
        if (fix64_2 < fix64_1)
          fix64_1 = fix64_2;
      }
      return fix64_1;
    }

    public Fix64 DistanceSqrTo(Vector3f p)
    {
      if (this.VerticesCount <= 2)
      {
        if (this.VerticesCount == 1)
          return p.DistanceSqrTo(this.VerticesExt.First);
        return this.VerticesCount == 2 ? new Line3f(this.VerticesExt[0], this.VerticesExt[1]).DistanceSqrToLineSegment(p) : Fix64.Zero;
      }
      Fix64 fix64_1 = Fix64.MaxValue;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3f other = this.VerticesExt[index];
        Fix64 numerator = this.EdgeVectors[index].Dot(p - other);
        Fix64 fix64_2;
        if (numerator.IsNotPositive)
        {
          fix64_2 = p.DistanceSqrTo(other);
        }
        else
        {
          Vector3f vector3f = this.VerticesExt[index + 1];
          Fix64 denominator = this.EdgeLengthsSqr[index];
          fix64_2 = !(numerator >= denominator) ? p.DistanceSqrTo(other.Lerp(vector3f, Percent.FromRatio(numerator, denominator))) : p.DistanceSqrTo(vector3f);
        }
        if (fix64_2 < fix64_1)
          fix64_1 = fix64_2;
      }
      return fix64_1;
    }

    public Fix64 DistanceSqrTo(Vector3f p, out bool closestToVertex, out int closestIndex)
    {
      closestToVertex = true;
      closestIndex = 0;
      if (this.VerticesCount <= 2)
      {
        if (this.VerticesCount == 1)
          return p.DistanceSqrTo(this.VerticesExt.First);
        if (this.VerticesCount != 2)
          return Fix64.Zero;
        Vector3f edgeVector = this.EdgeVectors[0];
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
        Vector3f other = edgeVector.Lerp(this.VerticesExt[1], Percent.FromRatio(numerator, denominator));
        closestToVertex = false;
        return p.DistanceSqrTo(other);
      }
      Fix64 fix64_1 = Fix64.MaxValue;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3f other = this.VerticesExt[index];
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
          Vector3f vector3f = this.VerticesExt[index + 1];
          Fix64 denominator = this.EdgeLengthsSqr[index];
          if (numerator >= denominator)
          {
            Fix64 fix64_3 = p.DistanceSqrTo(vector3f);
            if (fix64_3 < fix64_1)
            {
              closestToVertex = true;
              closestIndex = (index + 1) % this.VerticesCount;
              fix64_1 = fix64_3;
            }
          }
          else
          {
            Fix64 fix64_4 = p.DistanceSqrTo(other.Lerp(vector3f, Percent.FromRatio(numerator, denominator)));
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
  }
}
