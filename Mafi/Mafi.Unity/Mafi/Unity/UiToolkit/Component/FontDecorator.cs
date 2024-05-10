// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.FontDecorator
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
  public class FontDecorator : IFontDecorator
  {
    private static readonly FontDecorator s_instance;
    private VisualElement m_element;

    public static FontDecorator GetSharedInstance(VisualElement element)
    {
      FontDecorator.s_instance.m_element = element;
      return FontDecorator.s_instance;
    }

    public void SetFontStyle(FontStyle style)
    {
      this.m_element.style.unityFontStyleAndWeight = (StyleEnum<UnityEngine.FontStyle>) style.ToUnity();
    }

    public void SetTextAlign(TextAlign style)
    {
      this.m_element.style.unityTextAlign = (StyleEnum<TextAnchor>) style.ToUnity();
    }

    public void SetFontSize(int size) => this.m_element.style.fontSize = (StyleLength) (float) size;

    public FontDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FontDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FontDecorator.s_instance = new FontDecorator();
    }
  }
}
