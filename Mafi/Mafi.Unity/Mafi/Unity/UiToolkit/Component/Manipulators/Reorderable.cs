// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.Manipulators.Reorderable
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component.Manipulators
{
  public class Reorderable : Manipulator
  {
    private Vector2 m_targetStartPos;
    private VisualElement m_container;
    private VisualElement m_lastSwappedElement;
    private readonly VisualElement m_dragHandle;
    private readonly VisualElement m_shadowSpace;
    private readonly VisualElement m_dragTargetContainer;
    private readonly bool m_lockDragToAxis;
    private readonly float m_dragStartThreshold;
    private int m_startingIndex;
    private Vector3 m_pointerStartPos;
    private bool m_isDragging;
    private bool m_isColumn;

    /// <summary>Action(oldIndex, newIndex)</summary>
    public event Action<int, int> OnOrderChanged;

    public Reorderable(VisualElement dragHandle = null, bool lockDragToAxis = true)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_lockDragToAxis = lockDragToAxis;
      this.m_dragHandle = dragHandle ?? this.target;
      this.m_dragStartThreshold = dragHandle == null ? 5f : 1f;
      this.m_shadowSpace = new VisualElement();
      this.m_dragTargetContainer = new VisualElement()
      {
        usageHints = UsageHints.DynamicTransform,
        style = {
          position = (StyleEnum<Position>) Position.Absolute
        }
      };
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.m_dragHandle.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.pointerDownHandler));
      this.m_dragHandle.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.pointerMoveHandler));
      this.m_dragHandle.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.pointerUpHandler));
      this.m_dragHandle.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.mouseUpHandler));
      this.m_dragHandle.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.detachFromPanel));
      if (!(this.m_dragHandle is Button dragHandle))
        return;
      Clickable clickable = dragHandle.clickable;
      dragHandle.RemoveManipulator((IManipulator) clickable);
      dragHandle.AddManipulator((IManipulator) clickable);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.m_dragHandle.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.pointerDownHandler));
      this.m_dragHandle.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.pointerMoveHandler));
      this.m_dragHandle.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.pointerUpHandler));
      this.m_dragHandle.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.mouseUpHandler));
      this.m_dragHandle.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.detachFromPanel));
    }

    private void pointerUpHandler(PointerUpEvent evt)
    {
      this.onPointerUp((EventBase) evt, evt.pointerId);
    }

    private void mouseUpHandler(MouseUpEvent evt)
    {
      this.onPointerUp((EventBase) evt, PointerId.mousePointerId);
    }

    private void onPointerUp(EventBase evt, int pointerId)
    {
      this.m_dragHandle.ReleasePointer(pointerId);
      evt.StopImmediatePropagation();
      this.OnDragEnd(false);
    }

    private void pointerDownHandler(PointerDownEvent evt)
    {
      this.OnDragEnd(true);
      this.m_pointerStartPos = evt.position;
      this.m_dragHandle.CapturePointer(evt.pointerId);
    }

    private void pointerMoveHandler(PointerMoveEvent evt)
    {
      if (!this.m_dragHandle.HasPointerCapture(evt.pointerId))
        return;
      Vector3 pointerDelta = evt.position - this.m_pointerStartPos;
      if (!this.m_isDragging && (double) pointerDelta.magnitude >= (double) this.m_dragStartThreshold)
      {
        this.OnDragStart((IPointerEvent) evt);
        this.m_isDragging = true;
      }
      if (!this.m_isDragging)
        return;
      this.OnDragging((Vector2) pointerDelta);
    }

    private void detachFromPanel(DetachFromPanelEvent evt) => this.OnDragEnd(true);

    private void OnDragStart(IPointerEvent evt)
    {
      this.m_container = this.target.parent.contentContainer;
      this.m_isColumn = this.m_container.resolvedStyle.flexDirection == FlexDirection.Column || this.m_container.resolvedStyle.flexDirection == FlexDirection.ColumnReverse;
      this.m_startingIndex = this.m_container.IndexOf(this.target);
      this.m_targetStartPos = this.target.layout.position;
      this.m_dragTargetContainer.Add(this.target);
      IStyle style1 = this.m_shadowSpace.style;
      IStyle style2 = this.m_dragTargetContainer.style;
      Rect layout1 = this.target.layout;
      StyleLength width;
      StyleLength styleLength1 = width = (StyleLength) layout1.width;
      style2.width = width;
      StyleLength styleLength2 = styleLength1;
      style1.width = styleLength2;
      IStyle style3 = this.m_shadowSpace.style;
      IStyle style4 = this.m_dragTargetContainer.style;
      Rect layout2 = this.target.layout;
      StyleLength height;
      StyleLength styleLength3 = height = (StyleLength) layout2.height;
      style4.height = height;
      StyleLength styleLength4 = styleLength3;
      style3.height = styleLength4;
      this.m_container.Insert(this.m_startingIndex, this.m_shadowSpace);
      this.m_container.Add(this.m_dragTargetContainer);
    }

    private void OnDragEnd(bool wasCanceled)
    {
      if (!this.m_isDragging)
        return;
      this.m_isDragging = false;
      int index = wasCanceled ? this.m_startingIndex : this.m_container.IndexOf(this.m_shadowSpace);
      this.m_shadowSpace.RemoveFromHierarchy();
      this.m_container.Insert(index, this.target);
      this.m_dragTargetContainer.transform.position = Vector3.zero;
      this.m_dragTargetContainer.RemoveFromHierarchy();
      if (this.m_startingIndex == index)
        return;
      Action<int, int> onOrderChanged = this.OnOrderChanged;
      if (onOrderChanged == null)
        return;
      onOrderChanged(this.m_startingIndex, index);
    }

    private void OnDragging(Vector2 pointerDelta)
    {
      this.m_dragTargetContainer.transform.position = (Vector3) this.getNewTargetContainerPosFromCursor(pointerDelta);
      int index;
      if (!this.tryGetNewHierarchyPosition(out index))
        return;
      this.m_container.Insert(index, this.m_shadowSpace);
    }

    private Vector2 getNewTargetContainerPosFromCursor(Vector2 pointerDelta)
    {
      Vector2 vector2 = this.m_targetStartPos - this.m_dragTargetContainer.layout.position;
      float num1 = pointerDelta.x + vector2.x;
      float num2 = pointerDelta.y + vector2.y;
      float x = num1.Clamp(0.0f, this.m_container.layout.width - this.m_dragTargetContainer.resolvedStyle.width);
      float y = num2.Clamp(0.0f, this.m_container.layout.height - this.m_dragTargetContainer.resolvedStyle.height);
      if (this.m_lockDragToAxis)
      {
        x = this.m_isColumn ? this.m_dragTargetContainer.transform.position.x : x;
        y = this.m_isColumn ? y : this.m_dragTargetContainer.transform.position.y;
      }
      return new Vector2(x, y);
    }

    private bool tryGetNewHierarchyPosition(out int index)
    {
      index = -1;
      VisualElement closestElement = this.findClosestElement();
      if (closestElement == null)
        return false;
      bool hierarchyPosition = this.shouldSwapPlacesWith(closestElement);
      if (hierarchyPosition)
        this.m_lastSwappedElement = closestElement;
      index = hierarchyPosition ? closestElement.parent.IndexOf(closestElement) : -1;
      return hierarchyPosition;
    }

    private VisualElement findClosestElement()
    {
      float num = float.MaxValue;
      VisualElement closestElement = (VisualElement) null;
      foreach (VisualElement child in this.m_container.Children())
      {
        if (child != this.target && child != this.m_shadowSpace && child != this.m_dragTargetContainer && this.m_dragTargetContainer.worldBound.Overlaps(child.worldBound))
        {
          float sqrMagnitude = (Reorderable.rootSpaceOfElement(child) - this.m_dragTargetContainer.transform.position).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num)
          {
            num = sqrMagnitude;
            closestElement = child;
          }
        }
      }
      return closestElement;
    }

    private static Vector3 rootSpaceOfElement(VisualElement element)
    {
      Vector2 world = element.parent.LocalToWorld(element.layout.position);
      return (Vector3) element.parent.WorldToLocal(world);
    }

    private bool shouldSwapPlacesWith(VisualElement element)
    {
      Reorderable.AxisRectData axisRectData1 = new Reorderable.AxisRectData(this.m_isColumn, this.m_dragTargetContainer.worldBound);
      Reorderable.AxisRectData axisRectData2 = new Reorderable.AxisRectData(this.m_isColumn, element.worldBound);
      float num = axisRectData2.WidthOrHeight * (element != this.m_lastSwappedElement ? 0.6666667f : 0.5f);
      return (double) axisRectData2.CenterXOrY < (double) axisRectData1.CenterXOrY ? (double) axisRectData2.XMaxOrYMax - (double) axisRectData1.XMinOrYMin > (double) num : (double) axisRectData1.XMaxOrYMax - (double) axisRectData2.XMinOrYMin > (double) num;
    }

    private struct AxisRectData
    {
      public readonly float WidthOrHeight;
      public readonly float XOrY;
      public readonly float XMaxOrYMax;

      public float XMinOrYMin => this.XOrY;

      public float CenterXOrY => this.XOrY + this.WidthOrHeight / 2f;

      public AxisRectData(bool isVertical, Rect rect)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.WidthOrHeight = isVertical ? rect.height : rect.width;
        this.XOrY = isVertical ? rect.y : rect.x;
        this.XMaxOrYMax = isVertical ? rect.yMax : rect.xMax;
      }
    }
  }
}
