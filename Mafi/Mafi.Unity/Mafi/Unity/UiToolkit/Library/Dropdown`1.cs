// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Dropdown`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Dropdown<T> : Row, IComponentWithLabel
  {
    private Action<T, int> m_valueChanged;
    private Action<Dropdown<T>> m_onOpen;
    protected Button m_dropdownBtn;
    private Icon m_dropArrow;
    protected readonly DropdownPopup<T> m_dropdownPopup;
    protected LystStruct<T> m_options;
    protected LocStrFormatted m_clearOptionLabel;
    protected T m_clearOptionValue;
    private Option<Label> m_label;
    private Func<T, int, bool, UiComponent> m_optionViewFactory;

    public int SelectedIndex { get; protected set; }

    public T SelectedValue { get; protected set; }

    public Dropdown(Outer outer = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(outer, (object) null);
    }

    protected Dropdown(Outer outer, object forwardArg)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_clearOptionLabel = LocStrFormatted.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<Dropdown<T>>(Cls.inputDropdown).IgnoreInputPicking<Dropdown<T>>().Gap<Dropdown<T>>(new Px?(Theme.InputLabelGap));
      this.Add((UiComponent) (this.m_dropdownBtn = this.buildButton(outer, forwardArg)));
      this.m_dropdownPopup = new DropdownPopup<T>((UiComponent) this.m_dropdownBtn, new Action<T, int>(this.reportValueSelected), new Action(this.onPopupClosed));
      if (!this.updateOptionViewFactory())
        return;
      this.setValue(default (T), -1);
    }

    protected virtual Button buildButton(Outer outer, object forwardArg)
    {
      Inner glowAll = Inner.GlowAll;
      ButtonRow component = new ButtonRow(outer: outer, inner: glowAll);
      component.Add<ButtonRow>((Action<ButtonRow>) (c => c.Class<ButtonRow>(Cls.glowOnHover, Cls.inputDropdown_btn).JustifyItemsSpaceBetween<ButtonRow>().OnClick<ButtonRow>(new Action(this.clicked))));
      component.Add((UiComponent) (this.m_dropArrow = new Icon().Class<Icon>(Cls.inputDropdown_btn_arrow)));
      return (Button) component;
    }

    public Dropdown<T> OnOpen(Action<Dropdown<T>> onOpen)
    {
      this.m_onOpen = onOpen;
      return this;
    }

    public Dropdown<T> InnerDecorationClass(string className)
    {
      this.m_dropdownBtn.WrapClass(className);
      return this;
    }

    /// <summary>
    /// Sets a factory function to construct UI components to display options.
    /// Func arguments
    /// =&gt; T - Option
    /// =&gt; int - Index
    /// =&gt; bool - IsInDropdown
    /// </summary>
    public Dropdown<T> SetOptionViewFactory(Func<T, int, bool, UiComponent> viewFactory)
    {
      this.updateOptionViewFactory(viewFactory);
      this.setValue(this.SelectedValue, this.SelectedIndex);
      return this;
    }

    public Dropdown<T> SetDropdownHeader(UiComponent comp)
    {
      this.m_dropdownPopup.SetHeader(comp);
      return this;
    }

    public Dropdown<T> SetSearchStringLookup(Func<T, string> getSearchString)
    {
      this.m_dropdownPopup.SetSearchStringLookup(getSearchString);
      return this;
    }

    /// <summary>
    /// Adds an entry to the options list to clear the selected value. Set to LocStrFormatted.Empty to clear.
    /// </summary>
    /// <param name="label">Label to use for the clear value option</param>
    /// <param name="value">Value to send when cleared</param>
    /// <returns></returns>
    public Dropdown<T> IncludeClearOption(LocStrFormatted? label = null, T value = null)
    {
      this.m_clearOptionLabel = label ?? (LocStrFormatted) Tr.None;
      this.m_clearOptionValue = value;
      this.m_dropdownPopup.SetClearOptionValue(this.m_clearOptionValue);
      return this;
    }

    public Dropdown<T> HideCheckmarks()
    {
      this.m_dropdownPopup.HideCheckmarks();
      return this;
    }

    private bool updateOptionViewFactory(Func<T, int, bool, UiComponent> viewFactory = null)
    {
      Func<T, int, bool, UiComponent> renderItem = (Func<T, int, bool, UiComponent>) null;
      if (viewFactory != null)
        renderItem = viewFactory;
      else if (typeof (T).IsAssignableTo<LocStrFormatted>())
        renderItem = (Func<T, int, bool, UiComponent>) ((item, _1, _2) => (UiComponent) new Label((LocStrFormatted) (object) item));
      else if (typeof (T).IsAssignableTo<string>())
        renderItem = (Func<T, int, bool, UiComponent>) ((item, _3, _4) => (UiComponent) new Label(((string) (object) item).AsLoc()));
      this.m_optionViewFactory = renderItem == null ? (Func<T, int, bool, UiComponent>) ((_5, _6, _7) =>
      {
        throw new NotImplementedException("Option view factory not found for " + typeof (T).FullName);
      }) : (Func<T, int, bool, UiComponent>) ((opt, idx, isInDropdown) =>
      {
        if (!isInDropdown && ((object) opt == null || this.SelectedIndex < 0))
          return (UiComponent) new Label(this.m_clearOptionLabel.IsNotEmpty ? this.m_clearOptionLabel : (LocStrFormatted) Tr.SelectOption);
        return this.isClearOption(opt) ? (UiComponent) new Label(this.m_clearOptionLabel) : renderItem(opt, idx, isInDropdown);
      });
      this.m_dropdownPopup.SetOptionViewFactory(this.m_optionViewFactory);
      return renderItem != null;
    }

    void IComponentWithLabel.SetLabel(LocStrFormatted text)
    {
      this.getOrCreateLabel().Text<Label>(text);
    }

    void IComponentWithLabel.SetLabelWidth(Percent width)
    {
      Label valueOrNull = this.m_label.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Width<Label>(width);
    }

    void IComponentWithLabel.SetLabelWidth(Px width)
    {
      Label valueOrNull = this.m_label.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Width<Label>(new Px?(width));
    }

    public override void SetTooltipOrCreate(LocStrFormatted tooltip)
    {
      this.getOrCreateLabel().SetTooltipOrCreate(tooltip);
    }

    private Label getOrCreateLabel()
    {
      if (this.m_label.IsNone)
      {
        this.m_label = (Option<Label>) new Label();
        this.InsertAt(0, (UiComponent) this.m_label.Value);
      }
      return this.m_label.Value;
    }

    public Dropdown<T> SetOptions(IEnumerable<T> options)
    {
      this.m_options.Clear();
      this.m_options.AddRange(options);
      this.m_dropdownPopup.SetOptions(this.m_options.AsEnumerable());
      return this;
    }

    public void AddOption(T option) => this.m_options.Add(option);

    private void onPopupClosed() => this.m_dropdownBtn.Selected<Button>(false);

    public void ValueChanged(Action<T, int> onChange) => this.m_valueChanged = onChange;

    public Dropdown<T> OnValueChanged(Action<T, int> onChange)
    {
      this.m_valueChanged = onChange;
      return this;
    }

    public Dropdown<T> OnValueChanged(Action<Dropdown<T>, T, int> onChange)
    {
      this.m_valueChanged = (Action<T, int>) ((v, i) => onChange(this, v, i));
      return this;
    }

    public Dropdown<T> SetValueIndex(int index)
    {
      if (index < 0)
      {
        this.setValue(this.m_clearOptionValue, -1);
      }
      else
      {
        index = index.Clamp(0, this.m_options.Count - 1);
        if (this.m_options.Count > 0)
          this.setValue(this.m_options[index], index);
      }
      return this;
    }

    public Dropdown<T> SetValue(T value)
    {
      if (this.isClearOption(value))
        this.setValue(value, -1);
      else
        this.setValue(value, this.m_options.IndexOf(value));
      return this;
    }

    protected virtual void setValue(T value, int index)
    {
      this.SelectedIndex = index;
      this.SelectedValue = value;
      this.m_dropdownBtn.SetChildren(this.m_optionViewFactory(value, index, false), (UiComponent) this.m_dropArrow);
      this.m_dropdownPopup.SetValue(value);
    }

    private void reportValueSelected(T value, int index)
    {
      this.closePopup();
      this.setValue(value, index);
      Action<T, int> valueChanged = this.m_valueChanged;
      if (valueChanged == null)
        return;
      valueChanged(value, index);
    }

    protected void clicked()
    {
      if (this.m_dropdownPopup.IsVisible())
      {
        this.closePopup();
      }
      else
      {
        Action<Dropdown<T>> onOpen = this.m_onOpen;
        if (onOpen != null)
          onOpen(this);
        this.buildPopup();
        this.m_dropdownBtn.Selected<Button>();
      }
    }

    private void closePopup() => this.m_dropdownPopup.Visible<DropdownPopup<T>>(false);

    private void buildPopup()
    {
      this.m_dropdownPopup.SetOptions(this.m_options.AsEnumerable()).Visible<DropdownPopup<T>>(true);
    }

    public Dropdown<T> ConstrainMenuWidth(bool match = true)
    {
      this.m_dropdownPopup.MatchTargetWidth(match);
      return this;
    }

    public void ForEditorSetWidth(Px width) => this.m_dropdownBtn.Width<Button>(new Px?(width));

    private bool isClearOption(T option)
    {
      return this.m_clearOptionLabel.IsNotEmpty && Dropdown<T>.IsSame(this.m_clearOptionValue, option);
    }

    public static bool IsSame(T left, T right) => EqualityComparer<T>.Default.Equals(left, right);
  }
}
