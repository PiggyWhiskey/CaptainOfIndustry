// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.LeftRightScrollRect
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.EventSystems;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  /// <summary>
  /// Also allows dragging with right mouse button, to match in-game camera behavior.
  /// </summary>
  public class LeftRightScrollRect : ScrollRect
  {
    public override void OnBeginDrag(PointerEventData eventData)
    {
      if (eventData.button == PointerEventData.InputButton.Right)
        eventData.button = PointerEventData.InputButton.Left;
      base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
      if (eventData.button == PointerEventData.InputButton.Right)
        eventData.button = PointerEventData.InputButton.Left;
      base.OnEndDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
      if (eventData.button == PointerEventData.InputButton.Right)
        eventData.button = PointerEventData.InputButton.Left;
      base.OnDrag(eventData);
    }

    public LeftRightScrollRect()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
