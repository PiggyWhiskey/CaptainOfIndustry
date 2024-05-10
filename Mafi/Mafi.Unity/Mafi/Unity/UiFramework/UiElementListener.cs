// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.UiElementListener
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  /// <summary>Unity listener script to be attached to a button GO.</summary>
  public class UiElementListener : 
    MonoBehaviour,
    IPointerEnterHandler,
    IEventSystemHandler,
    IPointerExitHandler,
    IPointerClickHandler
  {
    public Option<Action> LeftClickAction { get; set; }

    public Option<Action> RightClickAction { get; set; }

    public Option<Action> MouseEnterAction { get; set; }

    public Option<Action> MouseLeaveAction { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (!this.MouseEnterAction.HasValue)
        return;
      this.MouseEnterAction.Value();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      if (!this.MouseLeaveAction.HasValue)
        return;
      this.MouseLeaveAction.Value();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (this.LeftClickAction.HasValue && eventData.button == PointerEventData.InputButton.Left)
        this.LeftClickAction.Value();
      if (!this.RightClickAction.HasValue || eventData.button != PointerEventData.InputButton.Right)
        return;
      this.RightClickAction.Value();
    }

    public void OnDisable()
    {
      if (!this.MouseLeaveAction.HasValue)
        return;
      this.MouseLeaveAction.Value();
    }

    public UiElementListener()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
