// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.PaddingDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class PaddingDecorator : IPaddingDecorator
  {
    private static readonly PaddingDecorator s_instance;
    private VisualElement m_element;

    public static PaddingDecorator GetSharedInstance(VisualElement element)
    {
      PaddingDecorator.s_instance.m_element = element;
      return PaddingDecorator.s_instance;
    }

    public void SetPadding(Px? top = null, Px? right = null, Px? bottom = null, Px? left = null)
    {
      if (top.HasValue)
        this.m_element.style.paddingTop = (StyleLength) top.Value.Pixels;
      if (right.HasValue)
        this.m_element.style.paddingRight = (StyleLength) right.Value.Pixels;
      if (bottom.HasValue)
        this.m_element.style.paddingBottom = (StyleLength) bottom.Value.Pixels;
      if (!left.HasValue)
        return;
      this.m_element.style.paddingLeft = (StyleLength) left.Value.Pixels;
    }

    public PaddingDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static PaddingDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      PaddingDecorator.s_instance = new PaddingDecorator();
    }
  }
}
