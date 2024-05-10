// Decompiled with JetBrains decompiler
// Type: RTG.RectEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class RectEx
  {
    /// <summary>
    /// Returns a list with the rectangle's corner points. The elements inside this
    /// list have a one-to-one maping with members of the 'RectCornerPoint' enum.
    /// </summary>
    public static List<Vector2> GetCornerPoints(this Rect rect)
    {
      return new List<Vector2>()
      {
        new Vector2(rect.xMin, rect.yMax),
        new Vector2(rect.xMax, rect.yMax),
        new Vector2(rect.xMax, rect.yMin),
        new Vector2(rect.xMin, rect.yMin)
      };
    }

    /// <summary>
    /// Places 'rect' below 'other' and sets its center to the same center
    /// as 'other' horizontally.
    /// </summary>
    public static Rect PlaceBelowCenterHrz(this Rect rect, Rect other)
    {
      return RectEx.FromCenterAndSize(new Vector2(other.center.x, (float) ((double) other.center.y - (double) other.size.y * 0.5 - (double) rect.size.y * 0.5)), rect.size);
    }

    /// <summary>
    /// Inverts the rectangle's Y coordinates in screen space. Useful when
    /// the rectangle needs to be expressed in coordinate systems with the
    /// Y axis going either up or down.
    /// </summary>
    public static Rect InvertScreenY(this Rect rect)
    {
      Vector2 center = rect.center;
      center.y = (float) (Screen.height - 1) - center.y;
      return RectEx.FromCenterAndSize(center, rect.size);
    }

    public static Rect FromCenterAndSize(Vector2 center, Vector2 size)
    {
      return new Rect(center.x - size.x * 0.5f, center.y - size.y * 0.5f, size.x, size.y);
    }

    public static Rect FromPoints(IEnumerable<Vector2> points)
    {
      Rect rect = new Rect();
      Vector2 rhs1 = Vector2Ex.FromValue(float.MaxValue);
      Vector2 rhs2 = Vector2Ex.FromValue(float.MinValue);
      foreach (Vector2 point in points)
      {
        rhs1 = Vector2.Min(point, rhs1);
        rhs2 = Vector2.Max(point, rhs2);
      }
      rect.xMin = rhs1.x;
      rect.yMin = rhs1.y;
      rect.xMax = rhs2.x;
      rect.yMax = rhs2.y;
      return rect;
    }

    public static Rect FromTexture2D(Texture2D texture2D)
    {
      return new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height);
    }

    public static Rect Inflate(this Rect rect, float inflateAmount)
    {
      float num = inflateAmount;
      float x = (double) rect.size.x >= 0.0 ? rect.size.x + num : rect.size.x - num;
      float y = (double) rect.size.y >= 0.0 ? rect.size.y + num : rect.size.y - num;
      return RectEx.FromCenterAndSize(rect.center, new Vector2(x, y));
    }

    public static bool ContainsAllPoints(this Rect rect, IEnumerable<Vector2> points)
    {
      foreach (Vector2 point in points)
      {
        if (!rect.Contains((Vector3) point, true))
          return false;
      }
      return true;
    }
  }
}
