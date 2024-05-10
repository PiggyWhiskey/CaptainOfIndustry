// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.MouseHandlingMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  internal class MouseHandlingMb : MonoBehaviour
  {
    private IMouseHandlingMbListener m_listener;
    private RectTransform m_rectTransform;
    private GraphicRaycaster m_rayCaster;
    private Vector2? m_prevPos;

    public void Initialize(
      IMouseHandlingMbListener listener,
      RectTransform rectTransform,
      GraphicRaycaster raycaster)
    {
      this.m_listener = listener;
      this.m_rectTransform = rectTransform;
      this.m_rayCaster = raycaster;
    }

    private void Update()
    {
      Vector2 localPoint;
      if ((Object) this.m_rayCaster == (Object) null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_rectTransform, (Vector2) Input.mousePosition, this.m_rayCaster.eventCamera, out localPoint))
        return;
      Vector2? prevPos = this.m_prevPos;
      Vector2 vector2 = localPoint;
      if ((prevPos.HasValue ? (prevPos.GetValueOrDefault() == vector2 ? 1 : 0) : 0) != 0)
        return;
      this.m_prevPos = new Vector2?(localPoint);
      if (!this.m_rectTransform.rect.Contains(localPoint))
        return;
      this.m_listener.OnMouseMove(localPoint);
    }

    public MouseHandlingMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
