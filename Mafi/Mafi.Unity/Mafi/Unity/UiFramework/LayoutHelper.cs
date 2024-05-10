// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.LayoutHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  /// <summary>Helper class for positioning UI objects.</summary>
  public static class LayoutHelper
  {
    /// <summary>
    /// Positions this object at given absolute coordinate on the main canvas. The position relative from the point
    /// specified by the <paramref name="horizontalPosition" /> and <paramref name="verticalPosition" />.
    /// </summary>
    public static void PositionAbsolute(
      GameObject parentObject,
      GameObject objectToPosition,
      Vector2 position,
      Vector2 size,
      HorizontalPosition horizontalPosition,
      VerticalPosition verticalPosition)
    {
      Offset offset = Offset.Zero;
      switch (horizontalPosition)
      {
        case HorizontalPosition.Left:
          offset = offset.AddLeft(position.x);
          break;
        case HorizontalPosition.Right:
          offset = offset.AddRight(position.x);
          break;
      }
      switch (verticalPosition)
      {
        case VerticalPosition.Bottom:
          offset = offset.AddBottom(position.y);
          break;
        case VerticalPosition.Top:
          offset = offset.AddTop(position.x);
          break;
      }
      LayoutHelper.PositionRelative(parentObject, objectToPosition, size, horizontalPosition, verticalPosition, offset);
    }

    /// <summary>Positions given object to fill given parent.</summary>
    public static void Fill(GameObject parentObject, GameObject objectToPosition, Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parentObject, objectToPosition, Vector2.zero, HorizontalPosition.Stretch, VerticalPosition.Stretch, offset);
    }

    /// <summary>Fills given parent with this object.</summary>
    public static void FillVertically(
      GameObject parentObject,
      GameObject objectToPosition,
      float size,
      HorizontalPosition horizontalPosition,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parentObject, objectToPosition, new Vector2(size, 0.0f), horizontalPosition, VerticalPosition.Stretch, offset);
    }

    /// <summary>Fills given parent with this object.</summary>
    public static void FillHorizontally(
      GameObject parentObject,
      GameObject objectToPosition,
      float size,
      VerticalPosition verticalPosition,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parentObject, objectToPosition, new Vector2(0.0f, size), HorizontalPosition.Stretch, verticalPosition, offset);
    }

    /// <summary>
    /// Positions this object relative to the given parent. The most general method, you can use more specific
    /// methods below.
    /// </summary>
    /// <param name="parentObject">Parent object.</param>
    /// <param name="objectToPosition">Object to position.</param>
    /// <param name="pivot">Normalized pivot point of this object. It is also center of rotation.</param>
    /// <param name="position">Position of the pivot relative to the anchor reference point.</param>
    /// <param name="sizeDelta">Size of this object relative to the distance between anchors.</param>
    /// <param name="anchorLeft">Normalized position from left of parent.</param>
    /// <param name="anchorBottom">Normalized position from bottom of parent.</param>
    /// <param name="anchorRight">Normalized position from right of parent.</param>
    /// <param name="anchorTop">Normalized position from top of parent.</param>
    public static void PositionRelative(
      GameObject parentObject,
      GameObject objectToPosition,
      Vector2 pivot,
      Vector2 position,
      Vector2 sizeDelta,
      float anchorLeft,
      float anchorBottom,
      float anchorRight,
      float anchorTop)
    {
      Assert.That<bool>(objectToPosition.transform is RectTransform).IsTrue("Transform is not `RectTransform`.");
      RectTransform transform = (RectTransform) objectToPosition.transform;
      if ((Object) transform.parent != (Object) parentObject.transform)
        transform.SetParent(parentObject.transform, false);
      transform.pivot = pivot;
      transform.anchorMin = new Vector2(anchorLeft, anchorBottom);
      transform.anchorMax = new Vector2(anchorRight, anchorTop);
      transform.sizeDelta = sizeDelta;
      transform.anchoredPosition = position;
    }

    /// <summary>Positions this object relative to the given parent.</summary>
    /// <param name="parentObject">Parent object.</param>
    /// <param name="objectToPosition">Object to position.</param>
    /// <param name="size">Size of this object. Size is ignored for dimensions that are set to stretch.</param>
    /// <param name="horizontalPosition">Horizontal position</param>
    /// <param name="verticalPosition">Vertical position</param>
    /// <param name="offset">Offset from all sides.</param>
    public static void PositionRelative(
      GameObject parentObject,
      GameObject objectToPosition,
      Vector2 size,
      HorizontalPosition horizontalPosition,
      VerticalPosition verticalPosition,
      Offset offset)
    {
      Vector2 pivot = new Vector2();
      Vector2 position = new Vector2();
      if (verticalPosition == VerticalPosition.Stretch)
        size.y = 0.0f;
      float anchorLeft;
      float anchorRight;
      if (horizontalPosition == HorizontalPosition.Stretch)
      {
        anchorLeft = 0.0f;
        anchorRight = 1f;
        pivot.x = 0.5f;
        size.x = 0.0f;
      }
      else
        anchorLeft = anchorRight = pivot.x = (float) horizontalPosition / 2f;
      switch (verticalPosition)
      {
        case VerticalPosition.Bottom:
          position.y += offset.BottomOffset;
          break;
        case VerticalPosition.Middle:
          position.y += offset.BottomOffset - offset.TopOffset;
          break;
        case VerticalPosition.Top:
          position.y += -offset.TopOffset;
          break;
        default:
          position.y += (float) (((double) offset.BottomOffset - (double) offset.TopOffset) / 2.0);
          size.y -= offset.BottomOffset + offset.TopOffset;
          break;
      }
      float anchorBottom;
      float anchorTop;
      if (verticalPosition == VerticalPosition.Stretch)
      {
        anchorBottom = 0.0f;
        anchorTop = 1f;
        pivot.y = 0.5f;
      }
      else
        anchorBottom = anchorTop = pivot.y = (float) verticalPosition / 2f;
      switch (horizontalPosition)
      {
        case HorizontalPosition.Left:
          position.x += offset.LeftOffset;
          break;
        case HorizontalPosition.Center:
          position.x += offset.LeftOffset - offset.RightOffset;
          break;
        case HorizontalPosition.Right:
          position.x += -offset.RightOffset;
          break;
        default:
          position.x += (float) (((double) offset.LeftOffset - (double) offset.RightOffset) / 2.0);
          size.x -= offset.LeftOffset + offset.RightOffset;
          break;
      }
      LayoutHelper.PositionRelative(parentObject, objectToPosition, pivot, position, size, anchorLeft, anchorBottom, anchorRight, anchorTop);
    }
  }
}
