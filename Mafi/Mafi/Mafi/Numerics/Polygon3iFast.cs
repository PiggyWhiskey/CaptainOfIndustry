// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Polygon3iFast
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
  public class Polygon3iFast
  {
    /// <summary>
    /// Extended array of vertices of this polygon (the first vertex is copied to the end to make some
    /// operations faster).
    /// </summary>
    public readonly ImmutableArray<Vector3i> VerticesExt;
    public readonly int VerticesCount;
    /// <summary>
    /// Vector at i-th index represents edge starting at the i-th vertex.
    /// </summary>
    public readonly ImmutableArray<Vector3i> EdgeVectors;
    public readonly ImmutableArray<long> EdgeLengthsSqr;
    public readonly Vector3i BoundingBoxMin;
    public readonly Vector3i BoundingBoxMax;

    private Polygon3iFast(ImmutableArray<Vector3i> verticesExt)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<Vector3i>(verticesExt.First).IsEqualTo<Vector3i>(verticesExt.Last);
      Assert.That<int>(verticesExt.Length).IsGreaterOrEqual(2);
      this.VerticesExt = verticesExt;
      this.VerticesCount = verticesExt.Length - 1;
      this.BoundingBoxMin = Vector3i.MaxValue;
      this.BoundingBoxMax = Vector3i.MinValue;
      ImmutableArrayBuilder<Vector3i> immutableArrayBuilder1 = new ImmutableArrayBuilder<Vector3i>(this.VerticesCount);
      ImmutableArrayBuilder<long> immutableArrayBuilder2 = new ImmutableArrayBuilder<long>(this.VerticesCount);
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3i rhs = verticesExt[index];
        Vector3i vector3i = verticesExt[index + 1] - rhs;
        immutableArrayBuilder1[index] = vector3i;
        immutableArrayBuilder2[index] = vector3i.LengthSqr;
        this.BoundingBoxMin = this.BoundingBoxMin.Min(rhs);
        this.BoundingBoxMax = this.BoundingBoxMax.Max(rhs);
      }
      this.EdgeVectors = immutableArrayBuilder1.GetImmutableArrayAndClear();
      this.EdgeLengthsSqr = immutableArrayBuilder2.GetImmutableArrayAndClear();
    }

    public static bool TryCreateWithoutDegenerateEdges(
      ReadOnlyArraySlice<Vector3i> vertices,
      out Polygon3iFast polygon,
      out string error)
    {
      polygon = (Polygon3iFast) null;
      if (vertices.Length < 2)
      {
        error = string.Format("Degenerate polygon with {0} vertices.", (object) vertices.Length);
        return false;
      }
      Lyst<Vector3i> lyst = new Lyst<Vector3i>(vertices.Length + 1)
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
      polygon = new Polygon3iFast(lyst.ToImmutableArray());
      error = "";
      return true;
    }

    public bool Contains(Vector3i p)
    {
      if (this.VerticesCount <= 2)
        return false;
      int num = 0;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3i vector3i1 = this.VerticesExt[index];
        if (vector3i1 == p)
          return true;
        Vector3i vector3i2 = this.VerticesExt[index + 1];
        Vector3i vector3i3 = p - vector3i1;
        Vector2i xy1;
        Vector3i edgeVector;
        if (vector3i1.Y <= p.Y)
        {
          if (vector3i2.Y > p.Y)
          {
            xy1 = vector3i3.Xy;
            ref Vector2i local = ref xy1;
            edgeVector = this.EdgeVectors[index];
            Vector2i xy2 = edgeVector.Xy;
            if (local.PseudoCross(xy2) > 0L)
              ++num;
          }
        }
        else if (vector3i2.Y <= p.Y)
        {
          xy1 = vector3i3.Xy;
          ref Vector2i local = ref xy1;
          edgeVector = this.EdgeVectors[index];
          Vector2i xy3 = edgeVector.Xy;
          if (local.PseudoCross(xy3) < 0L)
            --num;
        }
      }
      return num != 0;
    }

    public Fix64 DistanceSqrTo(Vector3i p)
    {
      if (this.VerticesCount <= 2)
      {
        if (this.VerticesCount == 1)
          return (Fix64) p.DistanceSqrTo(this.VerticesExt.First);
        return this.VerticesCount == 2 ? new Line3i(this.VerticesExt[0], this.VerticesExt[1]).DistanceSqrToLineSegment(p) : Fix64.MaxValue;
      }
      Fix64 fix64_1 = Fix64.MaxValue;
      for (int index = 0; index < this.VerticesCount; ++index)
      {
        Vector3i other = this.VerticesExt[index];
        long numerator = this.EdgeVectors[index].Dot(p - other);
        Fix64 fix64_2;
        if (numerator <= 0L)
        {
          fix64_2 = (Fix64) p.DistanceSqrTo(other);
        }
        else
        {
          Vector3i vector3i = this.VerticesExt[index + 1];
          long denominator = this.EdgeLengthsSqr[index];
          fix64_2 = numerator < denominator ? p.Vector3f.DistanceSqrTo(other.LerpToFix32(vector3i, Percent.FromRatio(numerator, denominator))) : (Fix64) p.DistanceSqrTo(vector3i);
        }
        if (fix64_2 < fix64_1)
          fix64_1 = fix64_2;
      }
      return fix64_1;
    }
  }
}
