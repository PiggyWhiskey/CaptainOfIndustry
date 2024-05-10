// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.BorderDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class BorderDecorator : IBorderDecorator
  {
    private static readonly BorderDecorator s_instance;
    private VisualElement m_element;

    public static BorderDecorator GetSharedInstance(VisualElement element)
    {
      BorderDecorator.s_instance.m_element = element;
      return BorderDecorator.s_instance;
    }

    public void SetBorderLeft(int left, ColorRgba? color = null)
    {
      this.m_element.style.borderLeftWidth = (StyleFloat) (float) left;
      if (!color.HasValue)
        return;
      this.m_element.style.borderLeftColor = (StyleColor) color.Value.ToColor();
    }

    public void SetBorderRight(int right, ColorRgba? color = null)
    {
      this.m_element.style.borderRightWidth = (StyleFloat) (float) right;
      if (!color.HasValue)
        return;
      this.m_element.style.borderRightColor = (StyleColor) color.Value.ToColor();
    }

    public void SetBorderTop(int top, ColorRgba? color = null)
    {
      this.m_element.style.borderTopWidth = (StyleFloat) (float) top;
      if (!color.HasValue)
        return;
      this.m_element.style.borderTopColor = (StyleColor) color.Value.ToColor();
    }

    public void SetBorderBottom(int bottom, ColorRgba? color = null)
    {
      this.m_element.style.borderBottomWidth = (StyleFloat) (float) bottom;
      if (!color.HasValue)
        return;
      this.m_element.style.borderBottomColor = (StyleColor) color.Value.ToColor();
    }

    public void SetBorderRadius(
      float? topLeft,
      float? topRight,
      float? bottomRight,
      float? bottomLeft)
    {
      if (topLeft.HasValue)
        this.m_element.style.borderTopLeftRadius = (StyleLength) topLeft.Value;
      if (topRight.HasValue)
        this.m_element.style.borderTopRightRadius = (StyleLength) topRight.Value;
      if (bottomRight.HasValue)
        this.m_element.style.borderBottomRightRadius = (StyleLength) bottomRight.Value;
      if (!bottomLeft.HasValue)
        return;
      this.m_element.style.borderBottomLeftRadius = (StyleLength) bottomLeft.Value;
    }

    public void SetBorder(int all = -2147483648, ColorRgba? color = null)
    {
      if (all != int.MinValue)
      {
        this.m_element.style.borderTopWidth = (StyleFloat) (float) all;
        this.m_element.style.borderRightWidth = (StyleFloat) (float) all;
        this.m_element.style.borderBottomWidth = (StyleFloat) (float) all;
        this.m_element.style.borderLeftWidth = (StyleFloat) (float) all;
      }
      if (!color.HasValue)
        return;
      Color color1 = color.Value.ToColor();
      this.m_element.style.borderTopColor = (StyleColor) color1;
      this.m_element.style.borderRightColor = (StyleColor) color1;
      this.m_element.style.borderBottomColor = (StyleColor) color1;
      this.m_element.style.borderLeftColor = (StyleColor) color1;
    }

    public BorderDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static BorderDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BorderDecorator.s_instance = new BorderDecorator();
    }
  }
}
