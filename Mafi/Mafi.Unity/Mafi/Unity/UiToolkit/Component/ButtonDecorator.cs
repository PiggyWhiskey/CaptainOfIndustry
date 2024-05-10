// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.ButtonDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class ButtonDecorator : IButtonDecorator
  {
    private static readonly ButtonDecorator s_instance;
    private Button m_element;

    public static ButtonDecorator GetSharedInstance(Button element)
    {
      ButtonDecorator.s_instance.m_element = element;
      return ButtonDecorator.s_instance;
    }

    public bool IsSelected() => this.m_element.ClassListContains(Cls.selected);

    public void SetSelected(bool isSelected)
    {
      this.m_element.EnableInClassList(Cls.selected, isSelected);
    }

    public void SetToggle(bool toggle) => this.m_element.EnableInClassList(Cls.toggle, toggle);

    public ButtonDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ButtonDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ButtonDecorator.s_instance = new ButtonDecorator();
    }
  }
}
