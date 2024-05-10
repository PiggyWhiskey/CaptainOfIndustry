// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.DragDropHandlerMb
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
  /// <summary>Unity script to be attached to a UI GO.</summary>
  public class DragDropHandlerMb : 
    MonoBehaviour,
    IBeginDragHandler,
    IEventSystemHandler,
    IDragHandler,
    IEndDragHandler
  {
    private IUiElement m_elementToMove;
    private Canvass m_canvas;
    private readonly Offset m_protectedPadding;
    private readonly Vector3[] m_corners;
    private Vector3 m_mousePosStart;

    public Option<Action> OnBeginDragAction { get; set; }

    public Option<Action> OnDragAction { get; set; }

    public Option<Action> OnEndDragAction { get; set; }

    public void Initialize(IUiElement elementToMove, Canvass canvas)
    {
      this.m_elementToMove = elementToMove.CheckNotNull<IUiElement>();
      this.m_canvas = canvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      this.m_mousePosStart = Input.mousePosition / this.m_canvas.ScaleFactor;
      this.m_elementToMove.SendToFront<IUiElement>();
      Action valueOrNull = this.OnBeginDragAction.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull();
    }

    public void OnDrag(PointerEventData eventData)
    {
      Vector3 vector3_1 = Input.mousePosition / this.m_canvas.ScaleFactor;
      Vector3 vector3_2 = vector3_1 - this.m_mousePosStart;
      float x = vector3_2.x;
      float y = vector3_2.y;
      this.m_elementToMove.RectTransform.GetWorldCorners(this.m_corners);
      Vector3 vector3_3 = this.m_corners[1] / this.m_canvas.ScaleFactor;
      Vector3 vector3_4 = this.m_corners[3] / this.m_canvas.ScaleFactor;
      if ((double) vector3_4.x + (double) x > (double) this.m_canvas.GetWidth() - (double) this.m_protectedPadding.RightOffset && (double) x > 0.0)
        x = 0.0f;
      if ((double) vector3_3.x + (double) x < (double) this.m_protectedPadding.LeftOffset && (double) x < 0.0)
        x = 0.0f;
      if ((double) vector3_4.y + (double) y < (double) this.m_protectedPadding.BottomOffset && (double) y < 0.0)
        y = 0.0f;
      if ((double) vector3_3.y + (double) y > (double) this.m_canvas.GetHeight() - (double) this.m_protectedPadding.TopOffset && (double) y > 0.0)
        y = 0.0f;
      this.m_elementToMove.RectTransform.anchoredPosition += new Vector2(x, y);
      this.m_mousePosStart = vector3_1;
      Action valueOrNull = this.OnDragAction.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      Action valueOrNull = this.OnEndDragAction.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull();
    }

    public DragDropHandlerMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_protectedPadding = Offset.TopBottom(20f);
      this.m_corners = new Vector3[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
