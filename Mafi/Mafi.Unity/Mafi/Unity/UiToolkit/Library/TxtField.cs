// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TxtField
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class TxtField : UiComponent, IComponentWithLabel, IComponentWithText
  {
    private Option<Action<string>> m_editEnd;
    private Option<Action> m_onFocus;
    private Option<Action> m_onReturn;
    private Option<Action> m_onEscape;
    private readonly BetterTextField m_textField;
    private Option<Label> m_label;
    private Option<Icon> m_errorIcon;
    private readonly UiComponent m_inputTooltipTarget;
    protected readonly TextElement m_textElement;

    public bool IsMultiline => this.m_textField.multiline;

    protected event Action<string> m_valueChanged;

    public TxtField()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<TxtField>(Cls.row, Cls.inputTextWrapper);
      this.m_textField = new BetterTextField();
      this.Element.Add((VisualElement) this.m_textField);
      this.m_textField.Add(Inner.GlowAll.ElementToPlace);
      this.m_textField.AddToClassList(Cls.glowOnHover);
      this.m_textField.selectAllOnMouseUp = false;
      this.SelectAllOnFocus(false);
      this.m_textElement = this.m_textField.Q<TextElement>();
      this.m_textElement.parseEscapeSequences = false;
      this.m_inputTooltipTarget = new UiComponent((VisualElement) this.m_textElement);
      this.RunWithBuilder((Action<UiBuilder>) (builder => this.m_inputTooltipTarget.Build(builder)));
      this.m_textElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.checkTextOverflow));
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

    public TxtField SetLabelAlignment(Mafi.Unity.UiToolkit.Component.Align align)
    {
      this.Element.style.alignItems = (StyleEnum<UnityEngine.UIElements.Align>) align.ToUnity();
      return this;
    }

    public TxtField FocusOnShow()
    {
      this.OnShow<TxtField>(new Action(((UiComponent) this).Focus));
      return this;
    }

    public override void SetTooltipOrCreate(LocStrFormatted tooltip)
    {
      this.getOrCreateLabel().SetTooltipOrCreate(tooltip);
    }

    private Label getOrCreateLabel()
    {
      if (this.m_label.IsNone)
      {
        this.m_label = (Option<Label>) new Label().Class<Label>(Cls.inputLabel);
        this.Element.Insert(0, this.m_label.Value.RootElement);
        this.RunWithBuilder((Action<UiBuilder>) (builder => this.m_label.Value.Build(builder)));
      }
      return this.m_label.Value;
    }

    public TxtField SetTextAreaHeight(Px? height)
    {
      IStyle style = this.m_textField.style;
      float? pixels = height?.Pixels;
      StyleLength styleLength = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None);
      style.height = styleLength;
      return this;
    }

    public TxtField SetTextAreaMinHeight(Px? height)
    {
      IStyle style = this.m_textField.style;
      float? pixels = height?.Pixels;
      StyleLength styleLength = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None);
      style.minHeight = styleLength;
      return this;
    }

    public virtual TxtField Text(string text)
    {
      this.m_textField.SetValueWithoutNotify(text);
      if ((double) this.m_textElement.style.translate.value.x.value < 0.0)
        this.m_textElement.style.translate = (StyleTranslate) StyleKeyword.Null;
      return this;
    }

    public TxtField Text(LocStrFormatted text) => this.Text(text.Value);

    protected override void SetColorInternal(ColorRgba? color)
    {
      IStyle style = this.m_textElement.style;
      Color? nullable = color.HasValue ? new Color?(color.GetValueOrDefault().ToColor()) : new Color?();
      StyleColor styleColor = nullable.HasValue ? (StyleColor) nullable.GetValueOrDefault() : new StyleColor(StyleKeyword.None);
      style.color = styleColor;
    }

    public override IBorderDecorator GetBorderDecorator()
    {
      return (IBorderDecorator) BorderDecorator.GetSharedInstance((VisualElement) this.m_textField);
    }

    public string GetText() => this.m_textField.value;

    public TxtField Multiline(bool isMultiline = true, bool doNotScroll = false, bool labelOnTop = false)
    {
      this.m_textField.multiline = isMultiline;
      this.m_textField.EnableInClassList(Cls.multiline, isMultiline);
      if (isMultiline)
      {
        this.m_textField.SetVerticalScrollerVisibility(doNotScroll ? ScrollerVisibility.Hidden : ScrollerVisibility.Auto);
        if (!doNotScroll)
          ScrollBase.ApplyThumbStyles(this.Element.Q((string) null, "unity-scroll-view") as ScrollView);
        this.m_textElement.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.checkTextOverflow));
        this.m_inputTooltipTarget.Tooltip<UiComponent>(new LocStrFormatted?());
      }
      else
        this.m_textElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.checkTextOverflow));
      this.ColumnDirection(labelOnTop);
      return this;
    }

    protected void ColumnDirection(bool column)
    {
      this.ClassIff<TxtField>(Cls.row, !column);
      this.ClassIff<TxtField>(Cls.column, column);
    }

    public TxtField Placeholder(LocStrFormatted placeholder)
    {
      this.m_textField.SetPlaceholder(placeholder);
      return this;
    }

    public TxtField SelectAllOnFocus(bool selectAll = true)
    {
      this.m_textField.selectAllOnFocus = selectAll;
      return this;
    }

    public TxtField Readonly(bool isReadonly)
    {
      this.m_textField.isReadOnly = isReadonly;
      this.m_textField.textEdition.isReadOnly = isReadonly;
      this.m_textField.EnableInClassList(Cls.readonlyCls, isReadonly);
      return this;
    }

    public override void Focus()
    {
      if (this.m_textField.panel.focusController.focusedElement == this.m_textField)
        return;
      this.m_textField.Focus();
    }

    /// <summary>
    /// If isDelayed = true, OnValueChanged gets sent only after commit which is either
    /// enter key or focus lost.
    /// </summary>
    public TxtField OnValueChanged(Action<string> valueChanged, bool isDelayed = false)
    {
      if (this.m_valueChanged == null)
        this.m_textField.RegisterValueChangedCallback<string>((EventCallback<ChangeEvent<string>>) (evt => this.notifyChanged(evt.newValue)));
      this.m_valueChanged += valueChanged;
      this.m_textField.textEdition.isDelayed = isDelayed;
      return this;
    }

    public TxtField OnEditEnd(Action<string> onEditEnd)
    {
      if (this.m_editEnd.IsNone)
      {
        this.m_textField.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.onKeyDown));
        this.m_textField.RegisterCallback<BlurEvent>(new EventCallback<BlurEvent>(this.onBlur));
      }
      this.m_editEnd = (Option<Action<string>>) onEditEnd;
      return this;
    }

    public TxtField OnFocus(Action onFocus)
    {
      if (this.m_onFocus.IsNone)
        this.m_textField.RegisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.onFocusHandler));
      this.m_onFocus = (Option<Action>) onFocus;
      return this;
    }

    public TxtField OnReturn(Action onReturn)
    {
      this.m_textField.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.onKeyDown));
      this.m_onReturn = (Option<Action>) onReturn;
      return this;
    }

    public TxtField OnEscape(Action onEscape)
    {
      this.m_textField.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.onKeyDown));
      this.m_onEscape = (Option<Action>) onEscape;
      return this;
    }

    public TxtField OnRightClick(Action onRightClick)
    {
      this.m_textField.RegisterCallback<MouseDownEvent>((EventCallback<MouseDownEvent>) (evt =>
      {
        if (evt.propagationPhase != PropagationPhase.AtTarget || evt.button != 1)
          return;
        onRightClick();
      }));
      return this;
    }

    protected void notifyChanged(string value)
    {
      Action<string> valueChanged = this.m_valueChanged;
      if (valueChanged == null)
        return;
      valueChanged(value);
    }

    private void onKeyDown(KeyDownEvent evt)
    {
      bool flag = evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return;
      if (flag && !this.m_textField.multiline)
      {
        Action<string> valueOrNull1 = this.m_editEnd.ValueOrNull;
        if (valueOrNull1 != null)
          valueOrNull1(this.GetText());
        Action valueOrNull2 = this.m_onReturn.ValueOrNull;
        if (valueOrNull2 == null)
          return;
        valueOrNull2();
      }
      else
      {
        if (flag && ((evt.modifiers & EventModifiers.Control) != EventModifiers.None || (evt.modifiers & EventModifiers.Command) != EventModifiers.None))
        {
          Action<string> valueOrNull = this.m_editEnd.ValueOrNull;
          if (valueOrNull != null)
            valueOrNull(this.GetText());
        }
        if (evt.keyCode != KeyCode.Escape)
          return;
        Action valueOrNull3 = this.m_onEscape.ValueOrNull;
        if (valueOrNull3 == null)
          return;
        valueOrNull3();
      }
    }

    private void onFocusHandler(FocusEvent evt)
    {
      Action valueOrNull = this.m_onFocus.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull();
    }

    private void onBlur(BlurEvent evt)
    {
      Action<string> valueOrNull = this.m_editEnd.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull(this.GetText());
    }

    private void checkTextOverflow(GeometryChangedEvent evt)
    {
      Rect rect = this.m_textField.localBound;
      double width1 = (double) rect.width;
      rect = evt.newRect;
      double width2 = (double) rect.width;
      if (width1 < width2)
        this.m_inputTooltipTarget.Tooltip<UiComponent>(new LocStrFormatted?(this.m_textField.value.AsLoc()));
      else
        this.m_inputTooltipTarget.Tooltip<UiComponent>(new LocStrFormatted?());
    }

    public TxtField MarkAsError(bool isError, LocStrFormatted tooltip = default (LocStrFormatted))
    {
      this.m_textField.EnableInClassList(Cls.error, isError);
      if (isError && this.m_errorIcon.IsNone)
      {
        this.m_errorIcon = (Option<Icon>) new Icon().Class<Icon>(Cls.errorIcon, "attachedIcon");
        this.m_textField.Add(this.m_errorIcon.Value.RootElement);
        this.RunWithBuilder((Action<UiBuilder>) (builder => this.m_errorIcon.Value.Build(builder)));
      }
      Icon valueOrNull = this.m_errorIcon.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull.Tooltip<Icon>(new LocStrFormatted?(tooltip));
      this.m_errorIcon.ValueOrNull?.SetVisible(isError);
      return this;
    }

    protected TxtField ForFieldAttachIcon(Icon icon)
    {
      icon.Class<Icon>("attachedIcon");
      this.m_textField.Add(icon.RootElement);
      this.RunWithBuilder((Action<UiBuilder>) (builder => icon.Build(builder)));
      return this;
    }

    protected TxtField ForFieldAddComponent(UiComponent component)
    {
      this.m_textField.Add(component.RootElement);
      this.RunWithBuilder((Action<UiBuilder>) (builder => component.Build(builder)));
      return this;
    }

    protected void ForFieldSetEnabled(bool enabled) => this.m_textField.SetEnabled(enabled);

    protected void ForFieldSetMarginTop(Px? top)
    {
      if (this.m_textField.ClassListContains(Cls.column))
        top = new Px?();
      IStyle style = this.m_textField.style;
      float? pixels = top?.Pixels;
      StyleLength styleLength = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None);
      style.marginTop = styleLength;
    }

    protected void ForFieldSetPaddingLeft(Px? left)
    {
      IStyle style = this.m_textField.style;
      float? pixels = left?.Pixels;
      StyleLength styleLength = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None);
      style.paddingLeft = styleLength;
    }

    protected void ForFieldSetWidth(Px? width)
    {
      IStyle style1 = this.m_textField.style;
      float? pixels = width?.Pixels;
      StyleLength styleLength1 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.Null);
      style1.minWidth = styleLength1;
      IStyle style2 = this.m_textField.style;
      pixels = width?.Pixels;
      StyleLength styleLength2 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.Null);
      style2.width = styleLength2;
    }

    public void SetText(LocStrFormatted text) => this.Text(text);

    void IComponentWithText.SetTextOverflow(Mafi.Unity.UiToolkit.Component.TextOverflow overflow)
    {
      overflow.ApplyTo((VisualElement) this.m_textElement);
    }
  }
}
