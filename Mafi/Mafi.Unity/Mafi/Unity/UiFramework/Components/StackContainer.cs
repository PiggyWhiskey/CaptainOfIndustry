// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.StackContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class StackContainer : IUiElement, IDynamicSizeElement
  {
    /// <summary>Left, bottom, right, top offset mask.</summary>
    private Offset m_itemOffsetMask;
    private HorizontalPosition m_itemHorizontalPosition;
    private VerticalPosition m_itemVerticalPosition;
    /// <summary>Objects in the direction.</summary>
    private readonly SortedList<float, StackContainer.Item> m_items;
    private readonly Panel m_container;
    private readonly UiBuilder m_builder;
    private StackContainer.SizeMode m_sizeMode;
    private Offset m_containerPadding;
    private float m_itemsSpacing;
    private StackContainer.Direction m_direction;
    private Offset m_lastOffset;
    private bool m_postponeRecompute;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public int ItemsCount => this.m_items.Count;

    /// <summary>
    /// Creates new stacking container in given <paramref name="containerObject" />. Objects are stacked in given <see cref="T:Mafi.Unity.UiFramework.Components.StackContainer.Direction" /> and stretched in the other direction.
    /// </summary>
    /// <remarks>
    /// If this container is not dynamic the host container size should be properly set. If this container is
    /// dynamic, only the dynamic size on it will be set (X for horizontal, Y for vertical) and the other will be
    /// preserved.
    /// </remarks>
    public StackContainer(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_itemHorizontalPosition = HorizontalPosition.Stretch;
      this.m_itemVerticalPosition = VerticalPosition.Stretch;
      this.m_items = new SortedList<float, StackContainer.Item>();
      this.m_sizeMode = StackContainer.SizeMode.Dynamic;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.m_container = new Panel(builder, name, parent);
      this.SetStackingDirection(StackContainer.Direction.LeftToRight);
    }

    public bool IsEmpty => this.m_items.Count == 0;

    public StackContainer SetBackground(ColorRgba color)
    {
      this.m_container.SetBackground(color);
      return this;
    }

    public StackContainer SetBorderStyle(BorderStyle borderStyle)
    {
      this.m_container.SetBorderStyle(borderStyle);
      return this;
    }

    /// <summary>Default is left-to-right.</summary>
    public StackContainer SetStackingDirection(StackContainer.Direction direction)
    {
      this.m_direction = direction;
      this.m_itemHorizontalPosition = HorizontalPosition.Stretch;
      this.m_itemVerticalPosition = VerticalPosition.Stretch;
      switch (direction)
      {
        case StackContainer.Direction.LeftToRight:
          this.m_itemOffsetMask = Offset.Left(1f);
          this.m_itemHorizontalPosition = HorizontalPosition.Left;
          break;
        case StackContainer.Direction.RightToLeft:
          this.m_itemOffsetMask = Offset.Right(1f);
          this.m_itemHorizontalPosition = HorizontalPosition.Right;
          break;
        case StackContainer.Direction.TopToBottom:
          this.m_itemOffsetMask = Offset.Top(1f);
          this.m_itemVerticalPosition = VerticalPosition.Top;
          break;
        case StackContainer.Direction.BottomToTop:
          this.m_itemOffsetMask = Offset.Bottom(1f);
          this.m_itemVerticalPosition = VerticalPosition.Bottom;
          break;
        default:
          Log.Error(string.Format("Unknown direction enum '{0}'.", (object) direction));
          break;
      }
      this.recomputeLayout();
      return this;
    }

    /// <summary>Default is dynamic.</summary>
    public StackContainer SetSizeMode(StackContainer.SizeMode sizeMode)
    {
      this.m_sizeMode = sizeMode;
      this.recomputeLayout();
      return this;
    }

    /// <summary>Default is zero.</summary>
    public StackContainer SetInnerPadding(Offset offset)
    {
      this.m_containerPadding = offset;
      this.recomputeLayout();
      return this;
    }

    /// <summary>Default is zero.</summary>
    public StackContainer SetItemSpacing(float itemSpacing)
    {
      this.m_itemsSpacing = itemSpacing;
      this.recomputeLayout();
      return this;
    }

    /// <summary>Adds new item to the end of this container.</summary>
    public void Add(
      float order,
      IUiElement element,
      Vector2? size,
      ContainerPosition? position,
      Offset offset,
      bool noSpacingAfterThis = false)
    {
      if (this.m_items.ContainsKey(order))
      {
        Assert.Fail<float>("Item with order {0} is already in the container.", order);
        for (int index = 0; index < 8 && this.m_items.ContainsKey(order); ++index)
          order += 0.012345f;
      }
      this.m_items.Add(order, new StackContainer.Item()
      {
        Element = element,
        Size = size ?? element.GetSize(),
        SpacingAfterThis = !noSpacingAfterThis,
        Offset = offset,
        HorizontalPosition = position.HasValue ? this.getHorizontalPositionFor(position.Value) : this.m_itemHorizontalPosition,
        VerticalPosition = position.HasValue ? this.getVerticalPositionFor(position.Value) : this.m_itemVerticalPosition
      });
      if (element is IDynamicSizeElement dynamicSizeElement)
        dynamicSizeElement.SizeChanged += new Action<IUiElement>(this.itemSizeChanged);
      this.recomputeLayout();
    }

    public void Add(
      float order,
      IUiElement element,
      float size,
      Offset offset = default (Offset),
      bool noSpacingAfterThis = false)
    {
      this.Add(order, element, new Vector2?(new Vector2(size, size)), new ContainerPosition?(), offset, noSpacingAfterThis);
    }

    public void Append(
      IUiElement element,
      Vector2? size,
      ContainerPosition? position,
      Offset offset = default (Offset),
      bool noSpacingAfterThis = false)
    {
      this.Add(this.getNextOrderIndex(), element, size, position, offset, noSpacingAfterThis);
    }

    public void Append(IUiElement element, float? size, Offset offset = default (Offset), bool noSpacingAfterThis = false)
    {
      this.Append(element, size.HasValue ? new Vector2?(new Vector2(size.Value, size.Value)) : new Vector2?(), new ContainerPosition?(), offset, noSpacingAfterThis);
    }

    /// <summary>Adds a divider of specified size after this item.</summary>
    public void AddDivider(float order, float size, ColorRgba color)
    {
      Panel element = new Panel(this.m_builder, "Divider");
      element.SetBackground(color);
      this.Add(order, (IUiElement) element, size);
    }

    public void AppendDivider(float size, ColorRgba color)
    {
      this.AddDivider(this.getNextOrderIndex(), size, color);
    }

    public void UpdateItemHeight(IUiElement element, float height)
    {
      KeyValuePair<float, StackContainer.Item> keyValuePair = this.m_items.FirstOrDefault<KeyValuePair<float, StackContainer.Item>>((Func<KeyValuePair<float, StackContainer.Item>, bool>) (x => x.Value.Element == element));
      StackContainer.Item obj = keyValuePair.Value;
      obj.Size = new Vector2(obj.Size.x, height);
      this.m_items[keyValuePair.Key] = obj;
      this.recomputeLayout();
    }

    public void UpdateItemWidth(IUiElement element, float width)
    {
      KeyValuePair<float, StackContainer.Item> keyValuePair = this.m_items.FirstOrDefault<KeyValuePair<float, StackContainer.Item>>((Func<KeyValuePair<float, StackContainer.Item>, bool>) (x => x.Value.Element == element));
      StackContainer.Item obj = keyValuePair.Value;
      obj.Size = new Vector2(width, obj.Size.y);
      this.m_items[keyValuePair.Key] = obj;
      this.recomputeLayout();
    }

    public void UpdateItemSize(IUiElement element, Vector2 size)
    {
      KeyValuePair<float, StackContainer.Item> keyValuePair = this.m_items.FirstOrDefault<KeyValuePair<float, StackContainer.Item>>((Func<KeyValuePair<float, StackContainer.Item>, bool>) (x => x.Value.Element == element));
      StackContainer.Item obj = keyValuePair.Value with
      {
        Size = size
      };
      this.m_items[keyValuePair.Key] = obj;
      this.recomputeLayout();
    }

    public void HideItem(IUiElement element) => this.SetItemVisibility(element, false);

    public void ShowItem(IUiElement element) => this.SetItemVisibility(element, true);

    internal void SetItemVisibility(int index, bool isVisible)
    {
      if (index >= this.m_items.Count)
        return;
      this.SetItemVisibility(this.m_items.Values[index].Element, isVisible);
    }

    public void SetItemVisibility(IUiElement item, bool isVisible)
    {
      if (item.IsVisible() == isVisible)
        return;
      item.SetVisibility<IUiElement>(isVisible);
      this.recomputeLayout();
    }

    public void UpdateSizesFromItems()
    {
      IList<float> keys = this.m_items.Keys;
      for (int index = 0; index < keys.Count; ++index)
      {
        StackContainer.Item obj = this.m_items[keys[index]];
        IUiElement element = obj.Element;
        obj.Size = new Vector2(element.GetWidth(), element.GetHeight());
        this.m_items[(float) index] = obj;
      }
      this.recomputeLayout();
    }

    private void itemSizeChanged(IUiElement element)
    {
      this.UpdateItemSize(element, element.GetSize());
    }

    /// <summary>Removes existing item from the container.</summary>
    public void Remove(IUiElement element)
    {
      this.m_items.RemoveAt(this.m_items.IndexOfValue(this.m_items.Values.FirstOrDefault<StackContainer.Item>((Func<StackContainer.Item, bool>) (x => x.Element == element))));
      this.removeCallback(element);
      this.recomputeLayout();
    }

    public void RemoveAt(int index)
    {
      StackContainer.Item obj = this.m_items.Values[index];
      this.m_items.RemoveAt(index);
      obj.Element.Destroy();
      this.removeCallback(obj.Element);
      this.recomputeLayout();
    }

    /// <summary>
    /// Removes the underlying game object of the give view and destroys the view.
    /// </summary>
    public void RemoveAndDestroy(View view)
    {
      this.Remove((IUiElement) view);
      view.Destroy();
    }

    /// <summary>Removes the given game object and destroys it.</summary>
    public void RemoveAndDestroy(IUiElement element)
    {
      this.Remove(element);
      element.Destroy();
    }

    public void ClearAndDestroyAll()
    {
      foreach (StackContainer.Item obj in (IEnumerable<StackContainer.Item>) this.m_items.Values)
      {
        obj.Element.Destroy();
        this.removeCallback(obj.Element);
      }
      this.m_items.Clear();
      this.recomputeLayout();
    }

    public void ClearAll(bool hideItems = false)
    {
      if (hideItems)
      {
        foreach (StackContainer.Item obj in (IEnumerable<StackContainer.Item>) this.m_items.Values)
        {
          obj.Element.Hide<IUiElement>();
          this.removeCallback(obj.Element);
        }
      }
      else
      {
        foreach (StackContainer.Item obj in (IEnumerable<StackContainer.Item>) this.m_items.Values)
          this.removeCallback(obj.Element);
      }
      this.m_items.Clear();
      this.recomputeLayout();
    }

    public void HideAll()
    {
      foreach (StackContainer.Item obj in (IEnumerable<StackContainer.Item>) this.m_items.Values)
        obj.Element.Hide<IUiElement>();
      this.recomputeLayout();
    }

    private void removeCallback(IUiElement element)
    {
      if (!(element is IDynamicSizeElement dynamicSizeElement))
        return;
      dynamicSizeElement.SizeChanged -= new Action<IUiElement>(this.itemSizeChanged);
    }

    /// <summary>
    /// Returns height of the container if used with dynamic vertical grow. Otherwise returns 0.
    /// </summary>
    public float GetDynamicHeight() => this.m_lastOffset.TopBottomOffset;

    /// <summary>
    /// Returns width of the container if used with dynamic horizontal grow. Otherwise returns 0.
    /// </summary>
    public float GetDynamicWidth() => this.m_lastOffset.LeftRightOffset;

    /// <summary>
    /// Use this before running mass changes as it prevents running layout recomputation for every single change.
    /// After you are done don't forget to call <see cref="M:Mafi.Unity.UiFramework.Components.StackContainer.FinishBatchOperation" />
    /// </summary>
    public void StartBatchOperation() => this.m_postponeRecompute = true;

    /// <summary>
    /// Request layout recomputation. Use this after <see cref="M:Mafi.Unity.UiFramework.Components.StackContainer.StartBatchOperation" />.
    /// </summary>
    public void FinishBatchOperation()
    {
      this.m_postponeRecompute = false;
      this.recomputeLayout();
    }

    internal IUiElement GetItemAt(int index) => this.m_items.Values[index].Element;

    private void recomputeLayout()
    {
      if (this.m_postponeRecompute)
        return;
      Offset containerPadding = this.m_containerPadding;
      Offset offset = this.m_itemOffsetMask * this.m_itemsSpacing;
      foreach (StackContainer.Item obj in (IEnumerable<StackContainer.Item>) this.m_items.Values)
      {
        if (obj.Element.IsVisible())
        {
          obj.Element.PutRelativeTo<IUiElement>((IUiElement) this.m_container, obj.Size, obj.HorizontalPosition, obj.VerticalPosition, containerPadding + obj.Offset);
          if (this.m_direction == StackContainer.Direction.LeftToRight || this.m_direction == StackContainer.Direction.RightToLeft)
            containerPadding += this.m_itemOffsetMask * (obj.Size.x + obj.Offset.LeftRightOffset);
          else
            containerPadding += this.m_itemOffsetMask * (obj.Size.y + obj.Offset.TopBottomOffset);
          if (obj.SpacingAfterThis)
            containerPadding += offset;
        }
      }
      if (this.m_items.Count > 0 && this.m_items.Values.Last<StackContainer.Item>().SpacingAfterThis)
        containerPadding -= offset;
      if (this.m_sizeMode == StackContainer.SizeMode.Dynamic)
      {
        if (this.m_direction == StackContainer.Direction.TopToBottom || this.m_direction == StackContainer.Direction.BottomToTop)
          this.m_container.SetHeight<Panel>(containerPadding.TopBottomOffset);
        else
          this.m_container.SetWidth<Panel>(containerPadding.LeftRightOffset);
      }
      this.m_lastOffset = containerPadding;
      if (this.m_container.Border.HasValue)
        this.m_container.Border.Value.transform.SetAsLastSibling();
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    private float getNextOrderIndex()
    {
      return this.m_items.Count <= 0 ? 0.0f : this.m_items.Last<KeyValuePair<float, StackContainer.Item>>().Key + 1f;
    }

    private HorizontalPosition getHorizontalPositionFor(ContainerPosition position)
    {
      if (this.m_direction == StackContainer.Direction.LeftToRight || this.m_direction == StackContainer.Direction.RightToLeft)
        return this.m_itemHorizontalPosition;
      switch (position)
      {
        case ContainerPosition.LeftOrTop:
          return HorizontalPosition.Left;
        case ContainerPosition.MiddleOrCenter:
          return HorizontalPosition.Center;
        case ContainerPosition.RightOrBottom:
          return HorizontalPosition.Right;
        default:
          return this.m_itemHorizontalPosition;
      }
    }

    private VerticalPosition getVerticalPositionFor(ContainerPosition position)
    {
      if (this.m_direction == StackContainer.Direction.TopToBottom || this.m_direction == StackContainer.Direction.BottomToTop)
        return this.m_itemVerticalPosition;
      switch (position)
      {
        case ContainerPosition.LeftOrTop:
          return VerticalPosition.Top;
        case ContainerPosition.MiddleOrCenter:
          return VerticalPosition.Middle;
        case ContainerPosition.RightOrBottom:
          return VerticalPosition.Bottom;
        default:
          return this.m_itemVerticalPosition;
      }
    }

    public StackContainer OnMouseEnter(Action onMouseEnter)
    {
      this.m_container.OnMouseEnter(onMouseEnter);
      return this;
    }

    public StackContainer OnMouseLeave(Action onMouseLeave)
    {
      this.m_container.OnMouseLeave(onMouseLeave);
      return this;
    }

    private struct Item
    {
      public IUiElement Element;
      public Vector2 Size;
      public Offset Offset;
      public bool SpacingAfterThis;
      public VerticalPosition VerticalPosition;
      public HorizontalPosition HorizontalPosition;
    }

    public enum Direction
    {
      LeftToRight,
      RightToLeft,
      TopToBottom,
      BottomToTop,
    }

    public enum SizeMode
    {
      /// <summary>
      /// Size of the container is not changing based on its content and all the items are stacked in the direction
      /// from the start of the container.
      /// </summary>
      StaticDirectionAligned,
      /// <summary>
      /// Size of the container is not changing based on its content and all the items are stacked in the
      /// direction. If there is less items than size of container the items are centered.
      /// </summary>
      StaticCenterAligned,
      /// <summary>
      /// Size of the container is automatically changed based on the items.
      /// </summary>
      Dynamic,
    }
  }
}
