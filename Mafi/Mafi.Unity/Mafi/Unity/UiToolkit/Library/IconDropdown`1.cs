// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.IconDropdown`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class IconDropdown<T> : Dropdown<T>, IButtonComponent, IUiComponent, IComponentWithPadding
  {
    public IconDropdown(string iconPath)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((Outer) null, (object) iconPath);
      this.ConstrainMenuWidth(false);
    }

    protected override Button buildButton(Outer outer, object iconPath)
    {
      return (Button) new IconClickable((string) iconPath, new Action(((Dropdown<T>) this).clicked));
    }

    protected override void setValue(T value, int index)
    {
      this.SelectedIndex = index;
      this.SelectedValue = value;
      this.m_dropdownPopup.SetValue(value);
    }

    public IconDropdown<T> Large()
    {
      if (this.m_dropdownBtn is IconClickable dropdownBtn)
        dropdownBtn.Large();
      return this;
    }

    public IconDropdown<T> Medium()
    {
      if (this.m_dropdownBtn is IconClickable dropdownBtn)
        dropdownBtn.Medium();
      return this;
    }

    public IconDropdown<T> Small()
    {
      if (this.m_dropdownBtn is IconClickable dropdownBtn)
        dropdownBtn.Small();
      return this;
    }

    public override void SetTooltipOrCreate(LocStrFormatted tooltip)
    {
      this.m_dropdownBtn.Tooltip<Button>(new LocStrFormatted?(tooltip));
    }

    IButtonDecorator IButtonComponent.GetButtonDecorator()
    {
      return ((IButtonComponent) this.m_dropdownBtn).GetButtonDecorator();
    }

    void IButtonComponent.SetOnClickAction(Action onClick)
    {
      ((IButtonComponent) this.m_dropdownBtn).SetOnClickAction(onClick);
    }

    void IButtonComponent.SetOnDoubleClickAction(Action onClick)
    {
      ((IButtonComponent) this.m_dropdownBtn).SetOnDoubleClickAction(onClick);
    }

    void IButtonComponent.SetCustomClickSound(string pathToSound)
    {
      ((IButtonComponent) this.m_dropdownBtn).SetCustomClickSound(pathToSound);
    }

    void IButtonComponent.SetVariant(ButtonVariant variant)
    {
      ((IButtonComponent) this.m_dropdownBtn).SetVariant(variant);
    }

    IPaddingDecorator IComponentWithPadding.GetPaddingDecorator()
    {
      return this.m_dropdownBtn.GetPaddingDecorator();
    }
  }
}
