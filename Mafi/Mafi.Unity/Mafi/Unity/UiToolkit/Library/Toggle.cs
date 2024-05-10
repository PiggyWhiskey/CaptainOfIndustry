// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Toggle
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
  public class Toggle : ButtonRow, IComponentWithLabel
  {
    private Option<Action<bool>> m_valueChanged;
    private readonly Label m_label;
    private readonly Button m_box;
    private readonly Row m_checkboxRow;
    private bool m_value;

    /// <summary>
    /// Standalone looks like this:
    /// [ ] Some label
    /// + it's text is clickable, hovers and also gets highlighted when active
    /// 
    /// Non standalone is used in editors a looks like this:
    /// Some label  [ ]
    /// </summary>
    /// <param name="standalone"></param>
    public Toggle(bool standalone = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<Toggle>(Cls.inputToggle, Cls.clickable).Class<Toggle>(standalone ? Cls.standalone : Cls.notStandalone).Gap<Toggle>(new Px?(Theme.InputLabelGap)).OnClick<Toggle>(new Action(this.clicked));
      Row component1 = new Row();
      component1.Add<Row>((Action<Row>) (c => c.IgnoreInputPicking<Row>().Class<Row>(Cls.inputToggle_btnHolder)));
      Button component2 = new Button(new Action(this.clicked), inner: Inner.GlowAll);
      component2.Add<Button>((Action<Button>) (c => c.Class<Button>(Cls.glowOnHover, Cls.inputToggle_btn)));
      Button child = component2;
      this.m_box = component2;
      component1.Add((UiComponent) child);
      this.m_checkboxRow = component1;
      this.m_label = new Label();
      if (standalone)
        this.SetChildren((UiComponent) this.m_checkboxRow, (UiComponent) this.m_label);
      else
        this.SetChildren((UiComponent) this.m_label, (UiComponent) this.m_checkboxRow);
    }

    public Toggle SetValid(bool valid) => this.ClassIff<Toggle>(Cls.invalid, !valid);

    private void clicked()
    {
      this.Value(!this.m_value);
      Action<bool> valueOrNull = this.m_valueChanged.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull(this.m_value);
    }

    public override void SetTooltipOrCreate(LocStrFormatted tooltip)
    {
      this.m_label.SetTooltipOrCreate(tooltip);
    }

    void IComponentWithLabel.SetLabel(LocStrFormatted text) => this.m_label.Text<Label>(text);

    void IComponentWithLabel.SetLabelWidth(Percent width) => this.m_label.Width<Label>(width);

    void IComponentWithLabel.SetLabelWidth(Px width) => this.m_label.Width<Label>(new Px?(width));

    public Toggle Value(bool value)
    {
      this.m_value = value;
      this.Selected<Toggle>(value);
      this.m_box.Selected<Button>(value);
      return this;
    }

    public bool GetValue() => this.m_value;

    public Toggle OnValueChanged(Action<bool> onChange)
    {
      this.m_valueChanged = (Option<Action<bool>>) onChange;
      return this;
    }

    public void ForEditorSetWidth(Px width) => this.m_checkboxRow.Width<Row>(new Px?(width));
  }
}
