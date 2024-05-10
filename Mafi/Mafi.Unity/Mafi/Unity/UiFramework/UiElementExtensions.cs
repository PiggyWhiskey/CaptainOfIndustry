// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.UiElementExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public static class UiElementExtensions
  {
    public static float GetWidth(this IUiElement element) => element.RectTransform.rect.width;

    public static float GetHeight(this IUiElement element) => element.RectTransform.rect.height;

    public static Vector2 GetSize(this IUiElement element)
    {
      Rect rect = element.RectTransform.rect;
      double width = (double) rect.width;
      rect = element.RectTransform.rect;
      double height = (double) rect.height;
      return new Vector2((float) width, (float) height);
    }

    public static T SetWidth<T>(this T element, float width) where T : IUiElement
    {
      element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
      return element;
    }

    public static T SetHeight<T>(this T element, float height) where T : IUiElement
    {
      element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
      return element;
    }

    public static T SetSize<T>(this T element, Vector2 size) where T : IUiElement
    {
      element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
      element.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
      return element;
    }

    public static T SetScale<T>(this T element, Vector2 scale) where T : IUiElement
    {
      element.RectTransform.localScale = new Vector3(scale.x, scale.y, 0.0f);
      return element;
    }

    public static T SetPivot<T>(this T element, Vector2 pivot) where T : IUiElement
    {
      element.RectTransform.pivot = pivot;
      return element;
    }

    public static T SetParent<T>(this T element, IUiElement parent, bool worldPositionStays = true) where T : IUiElement
    {
      element.RectTransform.SetParent((Transform) parent.RectTransform, worldPositionStays);
      return element;
    }

    public static T SetPosition<T>(this T element, Vector3 position) where T : IUiElement
    {
      element.RectTransform.position = position;
      return element;
    }

    public static T SetAnchoredPosition<T>(this T element, Vector2 position) where T : IUiElement
    {
      element.RectTransform.anchoredPosition = position;
      return element;
    }

    public static T SetLocalPosition<T>(this T element, Vector3 position) where T : IUiElement
    {
      element.RectTransform.localPosition = position;
      return element;
    }

    public static T Show<T>(this T element) where T : IUiElement
    {
      element.GameObject.SetActive(true);
      return element;
    }

    public static T Hide<T>(this T element) where T : IUiElement
    {
      element.GameObject.SetActive(false);
      return element;
    }

    public static T SendToBack<T>(this T element) where T : IUiElement
    {
      element.GameObject.transform.SetAsFirstSibling();
      return element;
    }

    public static T SendToFront<T>(this T element) where T : IUiElement
    {
      element.GameObject.transform.SetAsLastSibling();
      return element;
    }

    public static T SetVisibility<T>(this T element, bool visibility) where T : IUiElement
    {
      element.GameObject.SetActive(visibility);
      return element;
    }

    public static T ToggleVisibility<T>(this T element) where T : IUiElement
    {
      element.GameObject.SetActive(!element.GameObject.activeSelf);
      return element;
    }

    public static bool IsVisible(this IUiElement element) => element.GameObject.activeSelf;

    public static void Destroy(this IUiElement element) => element.GameObject.Destroy();

    /// <summary>
    /// Fills given parent with this object while keeping its aspect ratio using envelope mode.
    /// 
    /// See https://docs.unity3d.com/Manual/script-AspectRatioFitter.html
    /// </summary>
    /// <param name="aspectRatio">Width divided by height.</param>
    public static T PlaceViaEnvelope<T>(this T objectToPlace, IUiElement parent, float aspectRatio) where T : IUiElement
    {
      LayoutHelper.Fill(parent.GameObject, objectToPlace.GameObject);
      AspectRatioFitter aspectRatioFitter = objectToPlace.GameObject.AddComponent<AspectRatioFitter>();
      aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
      aspectRatioFitter.aspectRatio = aspectRatio;
      return objectToPlace;
    }

    /// <summary>Fills the given parent with this object.</summary>
    public static T PutTo<T>(this T objectToPlace, IUiElement parent, Offset offset = default (Offset)) where T : IUiElement
    {
      LayoutHelper.Fill(parent.GameObject, objectToPlace.GameObject, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object vertically and aligns it to the left.
    /// </summary>
    public static T PutToLeftOf<T>(
      this T objectToPlace,
      IUiElement parent,
      float size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.FillVertically(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object vertically and aligns it to the right.
    /// </summary>
    public static T PutToRightOf<T>(
      this T objectToPlace,
      IUiElement parent,
      float size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.FillVertically(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object vertically and aligns it to the center.
    /// </summary>
    public static T PutToCenterOf<T>(
      this T objectToPlace,
      IUiElement parent,
      float size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.FillVertically(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object horizontally and aligns it to the top.
    /// </summary>
    public static T PutToTopOf<T>(
      this T objectToPlace,
      IUiElement parent,
      float size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.FillHorizontally(parent.GameObject, objectToPlace.GameObject, size, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object horizontally and aligns it to the bottom.
    /// </summary>
    public static T PutToBottomOf<T>(
      this T objectToPlace,
      IUiElement parent,
      float size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.FillHorizontally(parent.GameObject, objectToPlace.GameObject, size, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Fills the given parent with this object horizontally and aligns it to the middle.
    /// </summary>
    public static T PutToMiddleOf<T>(
      this T objectToPlace,
      IUiElement parent,
      float size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.FillHorizontally(parent.GameObject, objectToPlace.GameObject, size, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the left top corner of the parent.
    /// </summary>
    public static T PutToLeftTopOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    public static T PutToLeftTopOf<T>(
      this T objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the left middle corner of the parent.
    /// </summary>
    public static T PutToLeftMiddleOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the left bottom corner of the parent.
    /// </summary>
    public static T PutToLeftBottomOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    public static T PutToLeftBottomOf<T>(
      this T objectToPlace,
      GameObject parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent, objectToPlace.GameObject, size, HorizontalPosition.Left, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the right top corner of the parent.
    /// </summary>
    public static T PutToRightTopOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the right middle corner of the parent.
    /// </summary>
    public static T PutToRightMiddleOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the right bottom corner of the parent.
    /// </summary>
    public static T PutToRightBottomOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Right, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the center top part of the parent.
    /// </summary>
    public static T PutToCenterTopOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, VerticalPosition.Top, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the center and middle of the parent.
    /// </summary>
    public static T PutToCenterMiddleOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, VerticalPosition.Middle, offset);
      return objectToPlace;
    }

    /// <summary>
    /// Position this object to the center bottom part of the parent.
    /// </summary>
    public static T PutToCenterBottomOf<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, HorizontalPosition.Center, VerticalPosition.Bottom, offset);
      return objectToPlace;
    }

    /// <summary>Positions this object relative to the given parent.</summary>
    /// <param name="parent">Parent object.</param>
    /// <param name="size">Size of this object. Size is ignored for dimensions that are set to stretch.</param>
    /// <param name="horizontalPosition">Horizontal position</param>
    /// <param name="verticalPosition">Vertical position</param>
    /// <param name="offset">Offset from sides.</param>
    public static T PutRelativeTo<T>(
      this T objectToPlace,
      IUiElement parent,
      Vector2 size,
      HorizontalPosition horizontalPosition,
      VerticalPosition verticalPosition,
      Offset offset = default (Offset))
      where T : IUiElement
    {
      LayoutHelper.PositionRelative(parent.GameObject, objectToPlace.GameObject, size, horizontalPosition, verticalPosition, offset);
      return objectToPlace;
    }

    public static T AddTo<T>(
      this T objectToPlace,
      StackContainer container,
      float order,
      float size,
      Offset offset = default (Offset),
      bool noSpacingAfterThis = false)
      where T : IUiElement
    {
      container.Add(order, (IUiElement) objectToPlace, size, offset, noSpacingAfterThis);
      return objectToPlace;
    }

    public static T AddTo<T>(
      this T objectToPlace,
      StackContainer container,
      float order,
      Vector2 size,
      ContainerPosition position,
      Offset offset,
      bool noSpacingAfterThis = false)
      where T : IUiElement
    {
      container.Add(order, (IUiElement) objectToPlace, new Vector2?(size), new ContainerPosition?(position), offset, noSpacingAfterThis);
      return objectToPlace;
    }

    public static T AppendTo<T>(
      this T objectToPlace,
      StackContainer container,
      float? size = null,
      Offset offset = default (Offset),
      bool noSpacingAfterThis = false)
      where T : IUiElement
    {
      container.Append((IUiElement) objectToPlace, size, offset, noSpacingAfterThis);
      return objectToPlace;
    }

    public static T AppendTo<T>(
      this T objectToPlace,
      StackContainer container,
      Vector2? size,
      ContainerPosition position,
      Offset offset = default (Offset),
      bool noSpacingAfterThis = false)
      where T : IUiElement
    {
      container.Append((IUiElement) objectToPlace, size, new ContainerPosition?(position), offset, noSpacingAfterThis);
      return objectToPlace;
    }

    public static T AppendTo<T>(this T objectToPlace, GridContainer container) where T : IUiElement
    {
      container.Append((IUiElement) objectToPlace);
      return objectToPlace;
    }
  }
}
