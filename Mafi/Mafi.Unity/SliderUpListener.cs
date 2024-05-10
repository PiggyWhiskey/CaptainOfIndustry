// Decompiled with JetBrains decompiler
// Type: SliderUpListener
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
public class SliderUpListener : MonoBehaviour, IPointerUpHandler, IEventSystemHandler
{
  public Option<Action> OnPointerUpAction { get; set; }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left || !this.OnPointerUpAction.HasValue)
      return;
    this.OnPointerUpAction.Value();
  }

  public SliderUpListener()
  {
    xxhJUtQyC9HnIshc6H.OukgcisAbr();
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
