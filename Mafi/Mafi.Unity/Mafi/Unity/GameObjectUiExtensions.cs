// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.GameObjectUiExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  public static class GameObjectUiExtensions
  {
    /// <summary>Fills the given parent with this object.</summary>
    public static GameObject PutTo(this GameObject objectToPlace, GameObject parent, Offset offset = default (Offset))
    {
      LayoutHelper.Fill(parent, objectToPlace, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object vertically and aligns it to the left.
    /// </summary>
    public static GameObject PutToLeftOf(
      this GameObject objectToPlace,
      GameObject parent,
      float size,
      Offset offset = default (Offset))
    {
      LayoutHelper.FillVertically(parent, objectToPlace, size, HorizontalPosition.Left, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object vertically and aligns it to the right.
    /// </summary>
    public static GameObject PutToRightOf(
      this GameObject objectToPlace,
      GameObject parent,
      float size,
      Offset offset = default (Offset))
    {
      LayoutHelper.FillVertically(parent, objectToPlace, size, HorizontalPosition.Right, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object vertically and aligns it to the center.
    /// </summary>
    public static GameObject PutToCenterOf(
      this GameObject objectToPlace,
      GameObject parent,
      float size,
      Offset offset = default (Offset))
    {
      LayoutHelper.FillVertically(parent, objectToPlace, size, HorizontalPosition.Center, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object horizontally and aligns it to the top.
    /// </summary>
    public static GameObject PutToTopOf(
      this GameObject objectToPlace,
      GameObject parent,
      float size,
      Offset offset = default (Offset))
    {
      LayoutHelper.FillHorizontally(parent, objectToPlace, size, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object horizontally and aligns it to the bottom.
    /// </summary>
    public static GameObject PutToBottomOf(
      this GameObject objectToPlace,
      GameObject parent,
      float size,
      Offset offset = default (Offset))
    {
      LayoutHelper.FillHorizontally(parent, objectToPlace, size, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object horizontally and aligns it to the middle.
    /// </summary>
    public static GameObject PutToMiddleOf(
      this GameObject objectToPlace,
      GameObject parent,
      float size,
      Offset offset = default (Offset))
    {
      LayoutHelper.FillHorizontally(parent, objectToPlace, size, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the left top corner of the parent.
    /// </summary>
    public static GameObject PutToLeftTopOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Left, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the left middle corner of the parent.
    /// </summary>
    public static GameObject PutToLeftMiddleOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Left, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the left bottom corner of the parent.
    /// </summary>
    public static GameObject PutToLeftBottomOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Left, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the right top corner of the parent.
    /// </summary>
    public static GameObject PutToRightTopOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Right, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the right middle corner of the parent.
    /// </summary>
    public static GameObject PutToRightMiddleOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Right, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the right bottom corner of the parent.
    /// </summary>
    public static GameObject PutToRightBottomOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Right, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the center top part of the parent.
    /// </summary>
    public static GameObject PutToCenterTopOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Center, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the center and middle of the parent.
    /// </summary>
    public static GameObject PutToCenterMiddleOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Center, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the center bottom part of the parent.
    /// </summary>
    public static GameObject PutToCenterBottomOf(
      this GameObject objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parent, objectToPlace, size, HorizontalPosition.Center, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>
    /// See <see cref="M:Mafi.Unity.UiFramework.LayoutHelper.PositionRelative(UnityEngine.GameObject,UnityEngine.GameObject,UnityEngine.Vector2,Mafi.Unity.UiFramework.HorizontalPosition,Mafi.Unity.UiFramework.VerticalPosition,Mafi.Unity.UiFramework.Offset)" />
    /// </summary>
    public static GameObject PutRelativeTo(
      this GameObject objectToPosition,
      GameObject parentObject,
      Vector2 size,
      HorizontalPosition horizontalPosition,
      VerticalPosition verticalPosition,
      Offset offset = default (Offset))
    {
      LayoutHelper.PositionRelative(parentObject, objectToPosition, size, horizontalPosition, verticalPosition, offset);
      return objectToPosition;
    }
  }
}
