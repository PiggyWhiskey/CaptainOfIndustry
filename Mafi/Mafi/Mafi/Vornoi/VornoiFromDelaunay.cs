// Decompiled with JetBrains decompiler
// Type: Mafi.Vornoi.VornoiFromDelaunay
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Vornoi
{
  public class VornoiFromDelaunay
  {
    /// <summary>
    /// Computes Vornoi diagram from given delaunay triangulation.
    /// </summary>
    /// <param name="points">Cell centers, vertices of delaunay triangulation.</param>
    /// <param name="delaunayTriangulation">Triangulation.</param>
    /// <param name="minEdgeLength">Minimum edge length of the vornoi diagram. Shorter edges will be collapsed to a single point.</param>
    /// <param name="vornoiPoints">Output list of Vornoi diagram vertices, referenced from <see cref="T:Mafi.Vornoi.VornoiCell" />.</param>
    /// <returns>Resulting vornoi cells.</returns>
    public static Lyst<VornoiCell> ComputeVornoi(
      IIndexable<Vector2i> points,
      IIndexable<DelaunayTriangle2i> delaunayTriangulation,
      int minEdgeLength,
      out Lyst<Vector2i> vornoiPoints)
    {
      Dict<int, Lyst<int>> neighbors = new Dict<int, Lyst<int>>();
      Set<Vector2i> edges = new Set<Vector2i>();
      foreach (DelaunayTriangle2i delaunayTriangle2i in delaunayTriangulation)
      {
        VornoiFromDelaunay.addNeighbor(neighbors, delaunayTriangle2i.E0);
        VornoiFromDelaunay.addNeighbor(neighbors, delaunayTriangle2i.E1);
        VornoiFromDelaunay.addNeighbor(neighbors, delaunayTriangle2i.E2);
        VornoiFromDelaunay.addEdgeRemoveIfExists(edges, delaunayTriangle2i.E0Canonic);
        VornoiFromDelaunay.addEdgeRemoveIfExists(edges, delaunayTriangle2i.E1Canonic);
        VornoiFromDelaunay.addEdgeRemoveIfExists(edges, delaunayTriangle2i.E2Canonic);
      }
      Set<int> set = new Set<int>();
      foreach (Vector2i vector2i in edges)
      {
        neighbors.Remove(vector2i.X);
        neighbors.Remove(vector2i.Y);
        set.Add(vector2i.X);
        set.Add(vector2i.Y);
      }
      edges.Clear();
      Lyst<VornoiCell> vornoi = new Lyst<VornoiCell>(neighbors.Count);
      vornoiPoints = new Lyst<Vector2i>(neighbors.Count);
      Dict<Vector3i, int> vornoiPointsLookup = new Dict<Vector3i, int>();
      long minEdgeLengthSqr = (long) (minEdgeLength * minEdgeLength);
      foreach (KeyValuePair<int, Lyst<int>> keyValuePair in neighbors)
      {
        int key = keyValuePair.Key;
        Vector2i pt = points[key];
        Lyst<int> lyst1 = keyValuePair.Value.OrderBy<int, AngleDegrees1f>((Func<int, AngleDegrees1f>) (i => (points[i] - pt).AnglePositive)).ToLyst<int>();
        bool isOnBoundary = false;
        Lyst<int> lyst2 = new Lyst<int>(lyst1.Count);
        int prevIndex = lyst1.Last;
        foreach (int thisIndex in lyst1)
        {
          isOnBoundary |= set.Contains(thisIndex);
          int vornoiPtIndex = VornoiFromDelaunay.getVornoiPtIndex(points, vornoiPoints, vornoiPointsLookup, key, prevIndex, thisIndex, minEdgeLengthSqr);
          LystAssertionExtensions.IsValidIndexFor<Vector2i>(Assert.That<int>(vornoiPtIndex), vornoiPoints);
          lyst2.Add(vornoiPtIndex);
          prevIndex = thisIndex;
        }
        for (int index = 0; index < lyst1.Count; ++index)
        {
          if (lyst2[index] == lyst2[(index + 1) % lyst1.Count])
          {
            lyst1.RemoveAt(index);
            lyst2.RemoveAt(index);
            --index;
          }
        }
        vornoi.Add(new VornoiCell(key, lyst2.ToImmutableArrayAndClear(), lyst1.ToImmutableArray(), isOnBoundary));
      }
      return vornoi;
    }

    private static int getVornoiPtIndex(
      IIndexable<Vector2i> points,
      Lyst<Vector2i> vornoiPoints,
      Dict<Vector3i, int> vornoiPointsLookup,
      int ptIndex,
      int prevIndex,
      int thisIndex,
      long minEdgeLengthSqr)
    {
      Assert.That<int>(ptIndex).IsNotEqualTo(prevIndex);
      Assert.That<int>(ptIndex).IsNotEqualTo(thisIndex);
      Assert.That<int>(prevIndex).IsNotEqualTo(thisIndex);
      Vector3i sortedComponents = new Vector3i(ptIndex, prevIndex, thisIndex).SortedComponents;
      int vornoiPtIndex;
      if (vornoiPointsLookup.TryGetValue(sortedComponents, out vornoiPtIndex))
        return vornoiPtIndex;
      Vector2i point1 = points[ptIndex];
      Vector2i point2 = points[prevIndex];
      Vector2f pt1 = (point2 + point1).Vector2f / (Fix32) 2;
      Vector2i vector2i = point2 - point1;
      Vector2f vector2f = vector2i.Vector2f;
      Vector2f orthogonalVector1 = vector2f.RightOrthogonalVector;
      Line2f line2f = Line2f.FromPointDirection(pt1, orthogonalVector1);
      Vector2i point3 = points[thisIndex];
      vector2i = point3 + point1;
      Vector2f pt2 = vector2i.Vector2f / (Fix32) 2;
      vector2i = point3 - point1;
      vector2f = vector2i.Vector2f;
      Vector2f orthogonalVector2 = vector2f.RightOrthogonalVector;
      Line2f otherLine = Line2f.FromPointDirection(pt2, orthogonalVector2);
      Vector2f? nullable = line2f.Intersect(otherLine);
      Assert.That<Vector2f?>(nullable).IsNotNull<Vector2f>();
      vector2f = nullable.Value;
      Vector2i intersectPt = vector2f.RoundedVector2i;
      if (vornoiPoints.IsNotEmpty)
      {
        int index = vornoiPoints.MinIndex<Vector2i, long>((Func<Vector2i, long>) (p => (p - intersectPt).LengthSqr));
        Assert.That<int>(index).IsNotNegative();
        vector2i = vornoiPoints[index] - intersectPt;
        if (vector2i.LengthSqr <= minEdgeLengthSqr)
        {
          vornoiPointsLookup.Add(sortedComponents, index);
          return index;
        }
      }
      int count = vornoiPoints.Count;
      vornoiPointsLookup.Add(sortedComponents, count);
      vornoiPoints.Add(intersectPt);
      return count;
    }

    private static void addNeighbor(Dict<int, Lyst<int>> neighbors, Vector2i edge)
    {
      Lyst<int> lyst;
      if (!neighbors.TryGetValue(edge.X, out lyst))
      {
        lyst = new Lyst<int>();
        neighbors.Add(edge.X, lyst);
      }
      lyst.Add(edge.Y);
    }

    private static void addEdgeRemoveIfExists(Set<Vector2i> edges, Vector2i canonicalEdge)
    {
      if (edges.Add(canonicalEdge))
        return;
      edges.Remove(canonicalEdge);
    }

    public VornoiFromDelaunay()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
