// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.ToggleBtn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class ToggleBtn : IUiElement
  {
    private readonly Btn m_onBtn;
    private readonly Btn m_offBtn;
    internal Tooltip OnTooltip;
    internal Tooltip OffTooltip;
    private Option<UiElementListener> m_listener;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public bool IsOn { get; private set; }

    public ToggleBtn(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GameObject = builder.GetClonedGo(name, parent);
      this.RectTransform = this.GameObject.GetComponent<RectTransform>();
      this.m_onBtn = builder.NewBtn("On", (IUiElement) this).PutTo<Btn>((IUiElement) this);
      this.m_offBtn = builder.NewBtn("Off", (IUiElement) this).PutTo<Btn>((IUiElement) this);
      this.SetIsOn(false);
    }

    public ToggleBtn AddTooltip(LocStrFormatted text) => this.AddTooltip(text, 0.0f);

    internal ToggleBtn AddTooltip(LocStrFormatted text, float extraOffsetFromBottom)
    {
      this.OnTooltip = this.m_onBtn.AddToolTipAndReturn();
      this.OffTooltip = this.m_offBtn.AddToolTipAndReturn();
      this.OnTooltip.SetText(text.Value);
      this.OffTooltip.SetText(text.Value);
      this.OnTooltip.SetExtraOffsetFromBottom(extraOffsetFromBottom);
      this.OffTooltip.SetExtraOffsetFromBottom(extraOffsetFromBottom);
      return this;
    }

    public ToggleBtn SetButtonStyleWhenOn(BtnStyle buttonStyle)
    {
      this.m_onBtn.SetButtonStyle(buttonStyle);
      return this;
    }

    public ToggleBtn SetButtonStyleWhenOff(BtnStyle buttonStyle)
    {
      this.m_offBtn.SetButtonStyle(buttonStyle);
      return this;
    }

    public ToggleBtn SetBtnIcon(string iconAssetPath, Offset? offset = null)
    {
      this.m_onBtn.SetIcon(iconAssetPath, offset);
      this.m_offBtn.SetIcon(iconAssetPath, offset);
      return this;
    }

    public ToggleBtn SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<UiElementListener>) this.GameObject.AddComponent<UiElementListener>();
      this.m_listener.Value.MouseEnterAction = (Option<Action>) enterAction.CheckNotNull<Action>();
      this.m_listener.Value.MouseLeaveAction = (Option<Action>) leaveAction.CheckNotNull<Action>();
      return this;
    }

    public ToggleBtn SetText(string text)
    {
      this.m_onBtn.SetText(text);
      this.m_offBtn.SetText(text);
      return this;
    }

    public ToggleBtn SetOnText(string text)
    {
      this.m_onBtn.SetText(text);
      return this;
    }

    public ToggleBtn SetOffText(string text)
    {
      this.m_offBtn.SetText(text);
      return this;
    }

    public ToggleBtn SetGameObjectWhenOn(IUiElement element, Graphic targetGraphic = null)
    {
      if ((UnityEngine.Object) targetGraphic != (UnityEngine.Object) null)
        this.m_onBtn.SetTargetGraphic(targetGraphic);
      element.PutTo<IUiElement>((IUiElement) this.m_onBtn);
      return this;
    }

    public ToggleBtn SetGameObjectWhenOff(IUiElement element, Graphic targetGraphic = null)
    {
      if ((UnityEngine.Object) targetGraphic != (UnityEngine.Object) null)
        this.m_offBtn.SetTargetGraphic(targetGraphic);
      element.PutTo<IUiElement>((IUiElement) this.m_offBtn);
      return this;
    }

    public ToggleBtn SetOnToggleAction(
      Action<bool> action,
      AudioSource toggleSound = null,
      bool toggleMuted = false)
    {
      this.m_onBtn.OnClick((Action) (() => action(!this.IsOn)), toggleSound, toggleMuted);
      this.m_offBtn.OnClick((Action) (() => action(!this.IsOn)), toggleSound, toggleMuted);
      return this;
    }

    public ToggleBtn SetIsOn(bool isOn)
    {
      this.IsOn = isOn;
      this.m_onBtn.SetVisibility<Btn>(this.IsOn);
      this.m_offBtn.SetVisibility<Btn>(!this.IsOn);
      return this;
    }

    public void Toggle() => this.SetIsOn(!this.IsOn);

    public void SetEnabled(bool isEnabled)
    {
      this.m_onBtn.SetEnabled(isEnabled);
      this.m_offBtn.SetEnabled(isEnabled);
    }
  }
}
