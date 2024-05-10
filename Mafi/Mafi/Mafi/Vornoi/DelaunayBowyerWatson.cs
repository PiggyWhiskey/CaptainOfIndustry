// Decompiled with JetBrains decompiler
// Type: Mafi.Vornoi.DelaunayBowyerWatson
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Vornoi
{
  /// <summary>
  /// Computes delaunay triangulation using Bowyer-Watson algorithm.
  /// </summary>
  /// <remarks>https://en.wikipedia.org/wiki/Bowyer%E2%80%93Watson_algorithm</remarks>
  public class DelaunayBowyerWatson
  {
    /// <summary>Returns delaunay triangulation of given points.</summary>
    public static Lyst<DelaunayTriangle2i> ComputeDelaunayTriangulation(IIndexable<Vector2i> points)
    {
      Assert.That<int>(points.Count).IsGreaterOrEqual(3, "Not enough points.");
      Vector2i vector2i1 = Vector2i.MaxValue;
      Vector2i vector2i2 = Vector2i.MinValue;
      foreach (Vector2i point in points)
      {
        vector2i1 = vector2i1.Min(point);
        vector2i2 = vector2i2.Max(point);
      }
      vector2i1 = vector2i1.AddXy(-1);
      vector2i2 = vector2i2.AddXy(1);
      Set<DelaunayTriangle2i> set1 = new Set<DelaunayTriangle2i>();
      int intCeiled1 = ((vector2i2.Y - vector2i1.Y) * 3.Sqrt()).ToIntCeiled();
      int intCeiled2 = ((vector2i2.X - vector2i1.X) * 3.Sqrt() / 2).ToIntCeiled();
      Vector2i[] masterPoints = new Vector2i[3]
      {
        new Vector2i(vector2i1.X - intCeiled1, vector2i1.Y),
        new Vector2i(vector2i2.X + intCeiled1, vector2i1.Y),
        new Vector2i((vector2i1.X + vector2i2.X) / 2, vector2i2.Y + intCeiled2)
      };
      DelaunayBowyerWatson.addNewTriangle(set1, points, masterPoints, -1, -2, -3);
      set1.First<DelaunayTriangle2i>();
      Lyst<DelaunayTriangle2i> lyst = new Lyst<DelaunayTriangle2i>();
      Dict<Vector2i, Vector2i> edges = new Dict<Vector2i, Vector2i>();
      Set<Vector2i> set2 = new Set<Vector2i>();
      for (int index = 0; index < points.Count; ++index)
      {
        Vector2i point = points[index];
        if (!set2.Add(point))
        {
          Log.Warning(string.Format("Duplicate input point detected, ignoring: {0}", (object) point));
        }
        else
        {
          foreach (DelaunayTriangle2i delaunayTriangle2i in set1)
          {
            if (delaunayTriangle2i.IsInCircumcircle(point))
            {
              lyst.Add(delaunayTriangle2i);
              DelaunayBowyerWatson.addEdgeRemoveIfExists(edges, delaunayTriangle2i.E0Canonic, delaunayTriangle2i.E0);
              DelaunayBowyerWatson.addEdgeRemoveIfExists(edges, delaunayTriangle2i.E1Canonic, delaunayTriangle2i.E1);
              DelaunayBowyerWatson.addEdgeRemoveIfExists(edges, delaunayTriangle2i.E2Canonic, delaunayTriangle2i.E2);
            }
          }
          Assert.That<Lyst<DelaunayTriangle2i>>(lyst).IsNotEmpty<DelaunayTriangle2i>();
          lyst.ForEachAndClear<bool>(new Func<DelaunayTriangle2i, bool>(set1.Remove));
          Assert.That<int>(edges.Count).IsGreaterOrEqual(3);
          foreach (Vector2i vector2i3 in edges.Values)
            DelaunayBowyerWatson.addNewTriangle(set1, points, masterPoints, index, vector2i3.X, vector2i3.Y);
          edges.Clear();
        }
      }
      return set1.Where<DelaunayTriangle2i>((Func<DelaunayTriangle2i, bool>) (t => t.I0 >= 0 && t.I1 >= 0 && t.I2 >= 0)).ToLyst<DelaunayTriangle2i>();
    }

    private static void addEdgeRemoveIfExists(
      Dict<Vector2i, Vector2i> edges,
      Vector2i canonicalEdge,
      Vector2i edge)
    {
      if (edges.ContainsKey(canonicalEdge))
        edges.Remove(canonicalEdge);
      else
        edges.Add(canonicalEdge, edge);
    }

    /// <summary>
    /// Helper function that inserts triangle based on point indices.
    /// </summary>
    private static void addNewTriangle(
      Set<DelaunayTriangle2i> triangles,
      IIndexable<Vector2i> points,
      Vector2i[] masterPoints,
      int i0,
      int i1,
      int i2)
    {
      triangles.Add(new DelaunayTriangle2i(i0 >= 0 ? points[i0] : masterPoints[-1 - i0], i0, i1 >= 0 ? points[i1] : masterPoints[-1 - i1], i1, i2 >= 0 ? points[i2] : masterPoints[-1 - i2], i2));
    }

    public DelaunayBowyerWatson()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
