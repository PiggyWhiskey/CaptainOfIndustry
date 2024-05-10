// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.SwitchBtn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class SwitchBtn : IUiElement
  {
    private static readonly ColorRgba CHECK_MARK_CLR;
    private static readonly ColorRgba CHECK_MARK_DISABLED_CLR;
    private readonly UiBuilder m_builder;
    private readonly Toggle m_toggle;
    private readonly Txt m_text;
    private bool m_pauseOnChangeEvent;
    private bool m_hasTooltip;
    private readonly IconContainer m_checkMark;

    public GameObject GameObject { get; private set; }

    public RectTransform RectTransform { get; private set; }

    public bool IsOn => this.m_toggle.isOn;

    public SwitchBtn(UiBuilder builder, IUiElement parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.GameObject = this.m_builder.GetClonedGo("Checkbox", parent?.GameObject);
      this.RectTransform = this.GameObject.GetComponent<RectTransform>();
      this.m_toggle = this.GameObject.AddComponent<Toggle>();
      this.m_toggle.navigation = new Navigation()
      {
        mode = Navigation.Mode.None
      };
      Panel leftMiddleOf = this.m_builder.NewPanel("Background", (IUiElement) this).SetBackground(builder.Style.Icons.GrayBgWhiteBorder, new ColorRgba?(ColorRgba.White)).PutToLeftMiddleOf<Panel>((IUiElement) this, 20.Vector2());
      this.m_checkMark = this.m_builder.NewIconContainer("Checkmark").SetIcon("Assets/Unity/UserInterface/General/Checkmark.svg", SwitchBtn.CHECK_MARK_CLR).PutTo<IconContainer>((IUiElement) leftMiddleOf, Offset.All(3f));
      this.m_toggle.colors = this.m_toggle.colors with
      {
        normalColor = 11119017.ToColor(),
        highlightedColor = 15066597.ToColor(),
        pressedColor = 12961221.ToColor(),
        selectedColor = 11119017.ToColor(),
        disabledColor = 7171437.ToColor(),
        fadeDuration = 0.1f
      };
      this.m_toggle.toggleTransition = Toggle.ToggleTransition.Fade;
      this.m_toggle.targetGraphic = leftMiddleOf.GameObject.GetComponent<Graphic>();
      this.m_toggle.graphic = this.m_checkMark.Graphic;
      this.m_text = builder.NewTxt("Text", (IUiElement) this).PutToLeftOf<Txt>((IUiElement) this, 0.0f, Offset.Left(27f)).SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).IncreaseFontForSymbols();
    }

    public SwitchBtn SetFontSize(int size)
    {
      this.m_text.SetFontSize(size);
      return this;
    }

    public SwitchBtn SetTextColor(ColorRgba color)
    {
      this.m_text.SetColor(color);
      return this;
    }

    public SwitchBtn SetText(LocStrFormatted text)
    {
      this.m_text.SetText(text);
      this.m_text.SetWidth<Txt>(this.m_text.GetPreferedWidth());
      return this;
    }

    public SwitchBtn SetText(string text)
    {
      this.m_text.SetText(text);
      this.m_text.SetWidth<Txt>(this.m_text.GetPreferedWidth());
      return this;
    }

    public SwitchBtn AddTooltip(LocStrFormatted tooltip)
    {
      this.m_builder.AddTooltipForTitle(this.m_text, tooltip);
      this.m_hasTooltip = true;
      return this;
    }

    public SwitchBtn AddTooltip(string tooltip)
    {
      this.m_builder.AddTooltipForTitle(this.m_text, tooltip);
      this.m_hasTooltip = true;
      return this;
    }

    public SwitchBtn SetOnToggleAction(Action<bool> onToggleAction)
    {
      Toggle.ToggleEvent toggleEvent = new Toggle.ToggleEvent();
      toggleEvent.AddListener((UnityAction<bool>) (x =>
      {
        if (this.m_pauseOnChangeEvent)
          return;
        onToggleAction(x);
      }));
      this.m_toggle.onValueChanged = toggleEvent;
      return this;
    }

    public SwitchBtn SetIsOn(bool isOn)
    {
      if (this.m_toggle.isOn == isOn)
        return this;
      this.m_pauseOnChangeEvent = true;
      this.m_toggle.isOn = isOn;
      this.m_pauseOnChangeEvent = false;
      return this;
    }

    public SwitchBtn SetIsEnabled(bool isEnabled)
    {
      this.m_toggle.interactable = isEnabled;
      this.m_checkMark.SetColor(isEnabled ? SwitchBtn.CHECK_MARK_CLR : SwitchBtn.CHECK_MARK_DISABLED_CLR);
      this.m_text.SetColor(isEnabled ? this.m_builder.Style.Global.Text.Color : this.m_builder.Style.Global.Text.Color.SetA((byte) 140));
      return this;
    }

    public float GetWidth()
    {
      return (float) ((double) this.m_text.GetWidth() + 27.0 + (this.m_hasTooltip ? 20.0 : 0.0));
    }

    static SwitchBtn()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SwitchBtn.CHECK_MARK_CLR = (ColorRgba) 14935011;
      SwitchBtn.CHECK_MARK_DISABLED_CLR = (ColorRgba) 6710886;
    }
  }
}
