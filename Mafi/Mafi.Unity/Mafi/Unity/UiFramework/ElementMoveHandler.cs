// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.ElementMoveHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  internal class ElementMoveHandler : 
    MonoBehaviour,
    IBeginDragHandler,
    IEventSystemHandler,
    IDragHandler,
    IEndDragHandler
  {
    private IUiElement m_elementToMove;
    private Canvass m_canvas;
    private Offset m_offset;
    private Action<Offset> m_onNewOffsetAction;
    private readonly Vector3[] m_canvasCorners;
    private readonly Vector3[] m_panelCorners;
    private Vector2 m_pointerOffset;

    public void Initialize(
      IUiElement elementToMove,
      Canvass canvas,
      Offset protectedPadding,
      Action<Offset> onNewOffsetAction)
    {
      this.m_elementToMove = elementToMove.CheckNotNull<IUiElement>();
      this.m_canvas = canvas;
      this.m_offset = protectedPadding;
      this.m_onNewOffsetAction = onNewOffsetAction;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_elementToMove.RectTransform, eventData.position, eventData.pressEventCamera, out this.m_pointerOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
      RectTransform rectTransform1 = this.m_canvas.RectTransform;
      RectTransform rectTransform2 = this.m_elementToMove.RectTransform;
      Vector2 localPoint;
      if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform1, eventData.position, eventData.pressEventCamera, out localPoint))
        return;
      rectTransform2.localPosition = (Vector3) (localPoint - this.m_pointerOffset);
      rectTransform1.GetWorldCorners(this.m_canvasCorners);
      rectTransform2.GetWorldCorners(this.m_panelCorners);
      float scaleFactor = this.m_canvas.ScaleFactor;
      Rect rect1 = rectTransform1.rect;
      Rect rect2 = rectTransform2.rect;
      Vector2 pivot = rectTransform2.pivot;
      Vector2 localPosition = (Vector2) rectTransform2.localPosition;
      if ((double) this.m_panelCorners[2].x + (double) this.m_offset.RightOffset * (double) scaleFactor > (double) this.m_canvasCorners[2].x)
        localPosition.x = (float) ((double) rect1.width * 0.5 - (double) rect2.width * (1.0 - (double) pivot.x)) - this.m_offset.RightOffset;
      else if ((double) this.m_panelCorners[0].x - (double) this.m_offset.LeftOffset * (double) scaleFactor < (double) this.m_canvasCorners[0].x)
        localPosition.x = (float) (-(double) rect1.width * 0.5 + (double) rect2.width * (double) pivot.x) + this.m_offset.LeftOffset;
      if ((double) this.m_panelCorners[2].y + (double) this.m_offset.TopOffset * (double) scaleFactor > (double) this.m_canvasCorners[2].y)
        localPosition.y = (float) ((double) rect1.height * 0.5 - (double) rect2.height * (1.0 - (double) pivot.y)) - this.m_offset.TopOffset;
      else if ((double) this.m_panelCorners[0].y - (double) this.m_offset.BottomOffset * (double) scaleFactor < (double) this.m_canvasCorners[0].y)
        localPosition.y = (float) (-(double) rect1.height * 0.5 + (double) rect2.height * (double) pivot.y) + this.m_offset.BottomOffset;
      rectTransform2.localPosition = (Vector3) localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      this.m_onNewOffsetAction(Offset.TopLeft(-this.m_elementToMove.RectTransform.anchoredPosition.y, this.m_elementToMove.RectTransform.anchoredPosition.x));
    }

    public ElementMoveHandler()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_canvasCorners = new Vector3[4];
      this.m_panelCorners = new Vector3[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
