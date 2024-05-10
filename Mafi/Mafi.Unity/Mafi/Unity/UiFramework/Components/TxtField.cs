// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.TxtField
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Decorators;
using Mafi.Unity.UserInterface;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class TxtField : IUiElement, IUiElementWithHover<TxtField>
  {
    private readonly UiBuilder m_builder;
    private readonly TMP_InputField m_inputField;
    private readonly Txt m_textView;
    private readonly Txt m_placeholder;
    private Option<UiElementListener> m_listener;
    private readonly GameObject m_textAreaGo;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public TxtField(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.GameObject = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = this.GameObject.GetComponent<RectTransform>();
      this.m_inputField = this.GameObject.AddComponent<TMP_InputField>();
      BackgroundDecorator.DecorateWithBgImage(this.GameObject, builder.AssetsDb.GetSharedSprite(builder.Style.Core.TextFieldBgSprite), this.m_builder, new ColorRgba?(builder.Style.Core.TextFieldBgColor));
      this.m_textAreaGo = this.m_builder.GetClonedGo("TextArea", parent);
      LayoutHelper.Fill(this.GameObject, this.m_textAreaGo, new Mafi.Unity.UiFramework.Offset(10f, 8f, 10f, 8f));
      this.m_textAreaGo.AddComponent<RectMask2D>().padding = new Vector4(-8f, -5f, -8f, -5f);
      this.m_inputField.textViewport = this.m_textAreaGo.GetComponent<RectTransform>();
      this.m_placeholder = this.m_builder.NewTxt("Placeholder", (IUiElement) this).SetAlignment(TextAnchor.MiddleLeft);
      LayoutHelper.Fill(this.m_textAreaGo, this.m_placeholder.GameObject, Mafi.Unity.UiFramework.Offset.Zero);
      this.m_placeholder.SetAsPlaceholderOf(this.m_inputField);
      this.m_textView = builder.NewTxt("Text", (IUiElement) this).SetAlignment(TextAnchor.MiddleLeft);
      LayoutHelper.Fill(this.m_textAreaGo, this.m_textView.GameObject, Mafi.Unity.UiFramework.Offset.Zero);
      this.m_textView.SetAsComponentOf(this.m_inputField);
      this.m_inputField.targetGraphic = (Graphic) this.GameObject.GetComponent<Image>();
      this.m_inputField.selectionColor = 10911509.ToColor();
      this.m_inputField.colors = this.m_inputField.colors with
      {
        normalColor = 16777215.ToColor(),
        highlightedColor = 14540253.ToColor(),
        pressedColor = 12237498.ToColor(),
        selectedColor = 12237498.ToColor(),
        disabledColor = 16777215.ToColorRgba().SetA((byte) 128).ToColor(),
        fadeDuration = 0.1f
      };
    }

    public TxtField SetBackgroundColor(ColorRgba color)
    {
      BackgroundDecorator.DecorateWithBgImage(this.GameObject, this.m_builder.AssetsDb.GetSharedSprite(this.m_builder.Style.Core.TextFieldBgSprite), this.m_builder, new ColorRgba?(color));
      return this;
    }

    public TxtField SetCharLimit(int limit)
    {
      this.m_inputField.characterLimit = limit;
      return this;
    }

    public TxtField MakeMultiline()
    {
      this.m_inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
      this.m_textView.SetAlignment(TextAnchor.UpperLeft);
      this.m_placeholder.SetAlignment(TextAnchor.UpperLeft);
      this.m_inputField.onFocusSelectAll = false;
      return this;
    }

    public TxtField MakeSingleLine()
    {
      this.m_inputField.lineType = TMP_InputField.LineType.SingleLine;
      this.m_textView.SetAlignment(TextAnchor.MiddleLeft);
      this.m_placeholder.SetAlignment(TextAnchor.MiddleLeft);
      this.m_inputField.onFocusSelectAll = true;
      return this;
    }

    public TxtField SetTextAlignment(TextAnchor anchor)
    {
      this.m_textView.SetAlignment(anchor);
      return this;
    }

    public TxtField SetOnValueChangedAction(Action onValueChanged)
    {
      this.m_inputField.onValueChanged.AddListener((UnityAction<string>) (s => onValueChanged()));
      return this;
    }

    public TxtField SetOnSelectAction(Action onSelect)
    {
      this.m_inputField.onSelect.AddListener((UnityAction<string>) (s => onSelect()));
      return this;
    }

    public TxtField SetPlaceholderText(string text)
    {
      this.m_placeholder.SetText(text);
      return this;
    }

    public TxtField SetPlaceholderText(LocStr text)
    {
      this.m_placeholder.SetText(text.TranslatedString);
      return this;
    }

    public TxtField SetText(string text)
    {
      this.m_inputField.SetTextWithoutNotify(text);
      GameObject resultGo;
      if (!this.m_inputField.isFocused && this.m_textAreaGo.TryFindChild("Caret", out resultGo))
      {
        this.m_inputField.MoveTextStart(false);
        this.m_textView.RectTransform.localPosition = Vector3.zero;
        this.m_placeholder.RectTransform.localPosition = Vector3.zero;
        resultGo.GetComponent<RectTransform>().localPosition = Vector3.zero;
      }
      return this;
    }

    public void MoveCaretToEnd() => this.m_inputField.caretPosition = this.m_inputField.text.Length;

    public TxtField OnClick(Action onClick)
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<UiElementListener>) this.GameObject.AddComponent<UiElementListener>();
      this.m_listener.Value.LeftClickAction = (Option<Action>) onClick.CheckNotNull<Action>();
      return this;
    }

    public TxtField OnRightClick(Action onClick)
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<UiElementListener>) this.GameObject.AddComponent<UiElementListener>();
      this.m_listener.Value.RightClickAction = (Option<Action>) onClick.CheckNotNull<Action>();
      return this;
    }

    public string GetText() => this.m_inputField.text;

    internal float GetDynamicHeight()
    {
      string text = this.GetText().Replace("\n\n", "\nxxxx\n");
      if (text.EndsWith("\n"))
        text += "xxxx";
      return this.m_textView.GetPreferedHeight(this.GetWidth() - 20f, text) + 16f;
    }

    public TxtField SetStyle(TxtFieldStyle style)
    {
      this.m_textView.SetTextStyle(style.TextStyle);
      this.m_placeholder.SetTextStyle(style.PlaceHolderStyle);
      return this;
    }

    public TxtField SetTextColor(ColorRgba color)
    {
      this.m_textView.SetColor(color);
      return this;
    }

    public TxtField SetFont(FontAsset font)
    {
      this.m_textView.SetFont(font);
      this.m_placeholder.SetFont(font);
      return this;
    }

    public TxtField SetOnEditEndListener(Action<string> action)
    {
      this.m_inputField.onEndEdit.AddListener((UnityAction<string>) (x => action(x)));
      return this;
    }

    public TxtField SetOnValidateInput(TMP_InputField.OnValidateInput validationFunc)
    {
      this.m_inputField.onValidateInput = validationFunc;
      return this;
    }

    public TxtField SetDelayedOnEditEndListener(Action<string> onEditEndAction)
    {
      this.GameObject.AddComponent<TxtField.DelayedEditEndListener>().SetCallback(onEditEndAction);
      return this;
    }

    public TxtField SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<UiElementListener>) this.GameObject.AddComponent<UiElementListener>();
      this.m_listener.Value.MouseEnterAction = (Option<Action>) enterAction.CheckNotNull<Action>();
      this.m_listener.Value.MouseLeaveAction = (Option<Action>) leaveAction.CheckNotNull<Action>();
      return this;
    }

    public TxtField ClearInput()
    {
      this.m_inputField.text = "";
      return this;
    }

    /// <summary>
    /// NOTE: This elements and all its parents need to be visible in order for this to work.
    /// </summary>
    public TxtField Focus()
    {
      this.m_inputField.ActivateInputField();
      this.m_inputField.Select();
      return this;
    }

    public TxtField SelectAllText()
    {
      this.m_inputField.caretPosition = this.m_inputField.text.Length;
      this.m_inputField.selectionAnchorPosition = 0;
      this.m_inputField.selectionFocusPosition = this.m_inputField.text.Length;
      this.m_inputField.ForceLabelUpdate();
      return this;
    }

    public bool IsFocused() => this.m_inputField.isFocused;

    public void SetReadonly() => this.m_inputField.interactable = false;

    public void SetReadonlyReally(bool isReadonly = true)
    {
      this.m_inputField.readOnly = isReadonly;
    }

    public void SetEnabled(bool isEnabled)
    {
      this.m_inputField.interactable = isEnabled;
      this.m_inputField.readOnly = !isEnabled;
    }

    internal TxtField EnableSelectionOnFocus()
    {
      this.m_inputField.onFocusSelectAll = true;
      return this;
    }

    public TxtField Activate()
    {
      this.m_inputField.ActivateInputField();
      return this;
    }

    private class DelayedEditEndListener : MonoBehaviour
    {
      private Action<string> m_callback;
      private float m_countDownStartSec;
      private TMP_InputField m_field;

      [PublicAPI("Unity MB API")]
      private void Awake()
      {
        this.m_field = this.GetComponent<TMP_InputField>();
        this.m_field.onValueChanged.AddListener((UnityAction<string>) (s => this.valueChanged()));
        this.m_field.onEndEdit.AddListener((UnityAction<string>) (s => this.onEditEnd()));
      }

      public void SetCallback(Action<string> callback)
      {
        this.m_callback = callback.CheckNotNull<Action<string>>();
      }

      [PublicAPI("Unity MB API")]
      private void Update()
      {
        if ((double) this.m_countDownStartSec < 0.0 || (double) this.m_countDownStartSec + 0.5 >= (double) Time.time)
          return;
        this.onEditEnd();
      }

      private void valueChanged() => this.m_countDownStartSec = Time.time;

      private void onEditEnd()
      {
        this.m_countDownStartSec = -1f;
        this.m_callback(this.m_field.text);
      }

      public DelayedEditEndListener()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_countDownStartSec = -1f;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
