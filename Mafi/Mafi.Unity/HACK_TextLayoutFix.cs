// Decompiled with JetBrains decompiler
// Type: HACK_TextLayoutFix
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
public static class HACK_TextLayoutFix
{
  private static readonly VisualElement HACK_TEXT_LAYOUT_KICKER;

  /// <summary>
  /// Works around a Unity bug with inconsistent text wrapping, especially prevalent in Chinese.
  /// Source: https://forum.unity.com/threads/text-wrap-bug.913379/
  /// Bug reported: https://unity3d.atlassian.net/servicedesk/customer/portal/2/IN-73456
  /// </summary>
  public static void Apply(VisualElement element)
  {
    element.RegisterCallback<GeometryChangedEvent>((EventCallback<GeometryChangedEvent>) (evt =>
    {
      VisualElement currentTarget = (VisualElement) evt.currentTarget;
      currentTarget.Add(HACK_TextLayoutFix.HACK_TEXT_LAYOUT_KICKER);
      currentTarget.Remove(HACK_TextLayoutFix.HACK_TEXT_LAYOUT_KICKER);
    }));
  }

  static HACK_TextLayoutFix()
  {
    xxhJUtQyC9HnIshc6H.OukgcisAbr();
    HACK_TextLayoutFix.HACK_TEXT_LAYOUT_KICKER = new VisualElement();
  }
}
