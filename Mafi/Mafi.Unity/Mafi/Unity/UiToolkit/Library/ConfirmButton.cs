// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ConfirmButton
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.FloatingPanel;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class ConfirmButton : Button, IComponentWithText, IButtonComponent, IUiComponent
  {
    private LocStrFormatted m_label;
    private LocStrFormatted m_confirmLabel;
    private readonly Action m_onConfirm;
    private Option<FloatingContainer> m_popup;
    private bool m_confirmEnabled;

    private ConfirmButton(
      UiComponent label,
      LocStrFormatted confirm,
      Action onConfirm,
      Outer outer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_confirmEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector(outer: outer);
      this.m_confirmLabel = confirm;
      this.m_onConfirm = onConfirm;
      this.Class<ConfirmButton>("dropdown", Cls.column, Cls.alignItemsCenter);
      this.OnClick<ConfirmButton>(new Action(this.handleOpen));
      this.Add(label.FlexGrow<UiComponent>(1f));
    }

    public ConfirmButton(LocStrFormatted label, LocStrFormatted confirm, Action onConfirm)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector((UiComponent) new Label(label).UpperCase(), confirm, onConfirm, Outer.ShadowCutCorner);
      this.m_label = label;
      this.Variant<ConfirmButton>(ButtonVariant.Default);
    }

    public ConfirmButton(
      string iconAsset,
      LocStrFormatted label,
      LocStrFormatted confirm,
      Action onConfirm)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector((UiComponent) new Icon(iconAsset).Medium(), confirm, onConfirm, Outer.ShadowAll);
      this.m_label = label;
      this.Class<ConfirmButton>(Cls.btn_boxy).Variant<ConfirmButton>(ButtonVariant.Boxy);
    }

    /// <summary>
    /// If confirm is required, shows a menu with the confirmation message on click.
    /// Otherwise, directly invokes the confirm action on click.
    /// </summary>
    public ConfirmButton ConfirmRequired(bool required)
    {
      if (!required)
      {
        this.m_popup.ValueOrNull?.Close();
        this.m_popup = (Option<FloatingContainer>) Option.None;
      }
      this.m_confirmEnabled = required;
      return this;
    }

    public void ShowConfirm()
    {
      if (!this.m_confirmEnabled)
        return;
      this.handleOpen();
      this.m_popup.ValueOrNull?.Focus();
    }

    public ConfirmButton ConfirmLabel(LocStrFormatted label)
    {
      this.m_confirmLabel = label;
      return this;
    }

    void IComponentWithText.SetText(LocStrFormatted text)
    {
      this.SetChildren(new UiComponent[1]
      {
        (UiComponent) new Label(text).UpperCase().FlexGrow<Label>(1f)
      });
    }

    void IComponentWithText.SetTextOverflow(Mafi.Unity.UiToolkit.Component.TextOverflow overflow)
    {
      TextElement element = this.Element.Q<TextElement>();
      if (element == null)
        return;
      overflow.ApplyTo((VisualElement) element);
    }

    private void handleOpen()
    {
      if (this.m_confirmEnabled)
      {
        if (this.m_popup.IsNone)
        {
          FloatingContainer component = new FloatingContainer((UiComponent) this, new Action(this.handleBlur), Inner.GlowAll);
          component.Add<FloatingContainer>((Action<FloatingContainer>) (c => c.MatchTargetWidth(false).Padding<FloatingContainer>(3.pt(), 4.pt())));
          component.Add((UiComponent) new Label(this.m_confirmLabel).MarginBottom<Label>(3.pt()));
          component.Add((UiComponent) new ButtonText(this.m_label, new Action(this.handleConfirm)).MarginLeftRight<ButtonText>(Px.Auto));
          this.m_popup = (Option<FloatingContainer>) component;
        }
        this.m_popup.Value.SetVisible(true);
      }
      else
        this.m_onConfirm();
    }

    private void handleBlur() => this.m_popup = (Option<FloatingContainer>) Option.None;

    private void handleConfirm()
    {
      this.m_onConfirm();
      this.m_popup.ValueOrNull?.Close();
      this.m_popup = (Option<FloatingContainer>) Option.None;
    }
  }
}
