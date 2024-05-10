// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Btn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Decorators;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class Btn : IUiElement, IDynamicSizeElement, IUiElementWithHover<Btn>
  {
    public bool DynamicSizeEnabled;
    private static Navigation s_nav;
    private readonly GameObject m_bgContainer;
    private BtnStyle m_buttonStyle;
    private Option<Txt> m_text;
    private Option<GameObject> m_existingBorder;
    private Vector2 m_iconSize;
    private Option<IconContainer> m_icon;
    private Option<Panel> m_shadow;
    private Option<Image> m_bgImage;
    private readonly UiBuilder m_builder;
    private readonly Button m_button;
    private readonly BtnListener m_listener;
    private Option<DragDropHandlerMb> m_dragListener;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_button.gameObject;

    public RectTransform RectTransform { get; }

    public bool IsEnabled => this.m_button.interactable;

    public Btn(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      GameObject clonedGo = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = clonedGo.GetComponent<RectTransform>();
      this.m_button = clonedGo.AddComponent<Button>();
      this.m_buttonStyle = new BtnStyle();
      this.m_bgContainer = this.m_builder.GetClonedGo("bg", clonedGo);
      this.m_bgContainer.PutTo(clonedGo);
      Btn.s_nav.mode = Navigation.Mode.None;
      this.m_button.navigation = Btn.s_nav;
      this.m_listener = this.m_button.gameObject.AddComponent<BtnListener>();
      AudioSource invalidSound = this.m_builder.AudioDb.GetSharedAudio(this.m_builder.Audio.InvalidOp);
      this.m_listener.LeftClickActionWhenDisabled = Option<Action>.Some((Action) (() => invalidSound.Play()));
    }

    public Btn EnableDynamicSize()
    {
      this.DynamicSizeEnabled = true;
      return this;
    }

    public void InvokeLeftClickAction()
    {
      Action valueOrNull = this.m_listener.LeftClickAction.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull();
    }

    public virtual Btn SetButtonStyle(BtnStyle buttonStyle)
    {
      this.m_buttonStyle = buttonStyle;
      if (this.m_buttonStyle.Shadow)
        this.DropShadow();
      this.m_existingBorder = BorderDecorator.DecorateWithBorders(this.m_builder, this.m_existingBorder, this.GameObject, this.m_buttonStyle.Border);
      if (buttonStyle.BackgroundClr.HasValue)
        this.SetBackgroundColor(buttonStyle.BackgroundClr.Value);
      if (this.m_text.HasValue)
      {
        this.m_text.Value.SetTextStyle(this.m_buttonStyle.Text);
        if (LocalizationManager.CurrentLangInfo.UsesSymbols && this.m_buttonStyle.Text.FontSize <= 12)
          this.m_text.Value.SetFontSize(this.m_buttonStyle.Text.FontSize + 1);
      }
      if (this.m_icon.HasValue)
        this.m_icon.Value.SetColor(buttonStyle.Text.Color);
      ColorBlock colors = this.m_button.colors;
      ref ColorBlock local1 = ref colors;
      ref ColorRgba? local2 = ref buttonStyle.NormalMaskClr;
      Color color1 = local2.HasValue ? local2.GetValueOrDefault().ToColor() : colors.normalColor;
      local1.normalColor = color1;
      ref ColorBlock local3 = ref colors;
      ref ColorRgba? local4 = ref buttonStyle.HoveredMaskClr;
      Color color2 = local4.HasValue ? local4.GetValueOrDefault().ToColor() : colors.highlightedColor;
      local3.highlightedColor = color2;
      ref ColorBlock local5 = ref colors;
      ref ColorRgba? local6 = ref buttonStyle.PressedMaskClr;
      Color color3 = local6.HasValue ? local6.GetValueOrDefault().ToColor() : colors.pressedColor;
      local5.pressedColor = color3;
      ref ColorBlock local7 = ref colors;
      ref ColorRgba? local8 = ref buttonStyle.DisabledMaskClr;
      Color color4 = local8.HasValue ? local8.GetValueOrDefault().ToColor() : colors.disabledColor;
      local7.disabledColor = color4;
      colors.fadeDuration = 0.0f;
      this.m_button.colors = colors;
      if (!this.m_button.interactable)
        this.updateVisualsForEnabled(false);
      this.updateSizeAndNotify();
      return this;
    }

    public Btn SetBackgroundColor(ColorRgba color)
    {
      if (this.m_bgImage.IsNone)
        this.m_bgImage = (Option<Image>) this.m_bgContainer.AddComponent<Image>();
      Image image = this.m_bgImage.Value;
      image.color = color.ToColor();
      if ((UnityEngine.Object) this.m_button.targetGraphic != (UnityEngine.Object) image)
        this.m_button.targetGraphic = (Graphic) image;
      return this;
    }

    public Btn DropShadow()
    {
      if (this.m_shadow.HasValue)
        return this;
      Sprite sharedSprite = this.m_builder.AssetsDb.GetSharedSprite(this.m_builder.Style.Icons.BtnShadow);
      Panel element = this.m_builder.NewPanel("Shadow", (IUiElement) this).PutTo<Panel>((IUiElement) this, Offset.BottomRight(-6f, -6f)).SetBackground(sharedSprite, new ColorRgba?(new ColorRgba(0, 150)));
      element.SendToBack<Panel>();
      element.GameObject.GetComponent<Image>().fillCenter = false;
      this.m_shadow = (Option<Panel>) element;
      return this;
    }

    public Btn SetText(LocStrFormatted text) => this.SetText(text.Value);

    public Btn SetText(string text)
    {
      if (this.m_text.IsNone)
      {
        this.m_text = (Option<Txt>) this.m_builder.NewTxt("Text", (IUiElement) this).SetTextStyle(this.m_buttonStyle.Text).SetAlignment(this.m_icon.IsNone ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft).AllowHorizontalOverflow().PutTo<Txt>((IUiElement) this, this.getTextOffset());
        if (LocalizationManager.CurrentLangInfo.UsesSymbols && this.m_buttonStyle.Text.FontSize <= 12)
          this.m_text.Value.SetFontSize(this.m_buttonStyle.Text.FontSize + 1);
      }
      this.m_text.Value.SetText(text);
      this.updateSizeAndNotify();
      return this;
    }

    public Btn UseSmallTextIfNeeded()
    {
      if (this.m_text.HasValue)
        this.m_text.Value.BestFitEnabled(this.m_buttonStyle.Text.FontSize);
      return this;
    }

    public Btn SetTextAlignment(TextAnchor alignment)
    {
      this.m_text.ValueOrNull?.SetAlignment(alignment);
      return this;
    }

    public Btn SetIcon(IconStyle iconStyle, bool isOnRight = false)
    {
      if (!this.m_icon.HasValue)
        return this.SetIcon(this.m_builder.NewIconContainer("Icon").SetIcon(iconStyle), new Vector2?(iconStyle.Size), isOnRight);
      this.m_icon.Value.SetIcon(iconStyle);
      return this;
    }

    /// <summary>
    /// Places icon with specific size, usually to place it next to text
    /// </summary>
    public Btn SetIcon(string iconPath, Vector2 size, bool isOnRight = false)
    {
      if (!this.m_icon.HasValue)
        return this.SetIcon(this.m_builder.NewIconContainer("Icon").SetIcon(iconPath, this.m_buttonStyle.Text.Color), new Vector2?(size), isOnRight);
      this.m_icon.Value.SetIcon(iconPath);
      return this;
    }

    /// <summary>Fills the button with icon</summary>
    public Btn SetIcon(string iconPath, Offset? offset = null)
    {
      if (this.m_icon.HasValue)
      {
        this.m_icon.Value.SetIcon(iconPath);
        return this;
      }
      IconContainer iconContainer = this.m_builder.NewIconContainer("Icon").SetIcon(iconPath, this.m_buttonStyle.Text.Color).PutTo<IconContainer>((IUiElement) this, offset ?? this.m_buttonStyle.IconPadding ?? Offset.Zero);
      this.m_icon = (Option<IconContainer>) iconContainer;
      if (this.m_bgImage.IsNone)
        this.m_button.targetGraphic = iconContainer.Graphic;
      return this;
    }

    private Offset getTextOffset()
    {
      Offset buttonPaddings = this.getButtonPaddings();
      if (this.m_icon.HasValue)
        buttonPaddings += Offset.Left(this.m_iconSize.x) + Offset.Left(6f);
      return buttonPaddings;
    }

    private Offset getButtonPaddings()
    {
      return this.m_buttonStyle.SidePaddings > 0 ? Offset.TopBottom(6f) + Offset.LeftRight((float) this.m_buttonStyle.SidePaddings) : Offset.All(6f);
    }

    public Btn AllowMultilineText()
    {
      this.m_text.ValueOrNull?.AllowVerticalOverflow();
      return this;
    }

    internal Btn SetIcon(IconContainer icon, Vector2? size = null, bool isOnRight = false)
    {
      this.m_icon = (Option<IconContainer>) icon;
      if (size.HasValue)
      {
        this.m_iconSize = size.Value;
        if (isOnRight)
          icon.PutToRightMiddleOf<IconContainer>((IUiElement) this, size.Value, Offset.Right(this.getButtonPaddings().RightOffset));
        else
          icon.PutToLeftMiddleOf<IconContainer>((IUiElement) this, size.Value, Offset.Left(this.getButtonPaddings().LeftOffset));
      }
      else
      {
        this.m_iconSize = Vector2.zero;
        icon.PutTo<IconContainer>((IUiElement) this);
      }
      if (this.m_text.HasValue)
        this.m_text.Value.SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) this, this.getTextOffset());
      this.updateSizeAndNotify();
      return this;
    }

    public Btn SetTargetGraphic(Graphic graphic)
    {
      this.m_button.targetGraphic = graphic;
      return this;
    }

    public Btn MakeTextTargetGraphics()
    {
      this.m_button.targetGraphic = this.m_text.ValueOrNull?.Graphic;
      return this;
    }

    public void SetBorderColor(ColorRgba color)
    {
      if (this.m_existingBorder.IsNone)
        Assert.Fail(string.Format("Button has no border: {0}", (object) this.m_button));
      else
        this.m_existingBorder.Value.GetComponent<Image>().color = color.ToColor();
    }

    public Btn OnClick(Action clickAction, AudioSource clickSound = null, bool muted = false)
    {
      Assert.That<Action>(clickAction).IsNotNull<Action>();
      Action action;
      if ((UnityEngine.Object) clickSound != (UnityEngine.Object) null)
        action = (Action) (() =>
        {
          clickSound.Play();
          clickAction();
        });
      else if (!muted)
      {
        AudioSource defaultClickSound = this.m_builder.AudioDb.GetSharedAudio(this.m_builder.Audio.ButtonClick);
        action = (Action) (() =>
        {
          defaultClickSound.Play();
          clickAction();
        });
      }
      else
        action = clickAction;
      this.m_listener.LeftClickAction = (Option<Action>) action;
      return this;
    }

    public Btn OnRightClick(Action clickAction, AudioSource clickSound = null)
    {
      Assert.That<Action>(clickAction).IsNotNull<Action>();
      this.m_listener.RightClickAction = (Option<Action>) (!((UnityEngine.Object) clickSound != (UnityEngine.Object) null) ? clickAction : (Action) (() =>
      {
        clickAction();
        clickSound.Play();
      }));
      return this;
    }

    public Btn OnDoubleClick(Action clickAction, AudioSource clickSound = null, bool muted = false)
    {
      Assert.That<Action>(clickAction).IsNotNull<Action>();
      Action action;
      if ((UnityEngine.Object) clickSound != (UnityEngine.Object) null)
        action = (Action) (() =>
        {
          clickAction();
          clickSound.Play();
        });
      else if (!muted)
      {
        AudioSource defaultClickSound = this.m_builder.AudioDb.GetSharedAudio(this.m_builder.Audio.ButtonClick);
        action = (Action) (() =>
        {
          clickAction();
          defaultClickSound.Play();
        });
      }
      else
        action = clickAction;
      this.m_listener.DoubleClickAction = (Option<Action>) action;
      return this;
    }

    public Btn SetupDragDrop(Action onBeginDrag, Action onDrag, Action onEndDrag)
    {
      if (this.m_dragListener.IsNone)
      {
        this.m_dragListener = (Option<DragDropHandlerMb>) this.GameObject.AddComponent<DragDropHandlerMb>();
        this.m_dragListener.Value.Initialize((IUiElement) this, this.m_builder.MainCanvas);
      }
      this.m_dragListener.Value.OnBeginDragAction = (Option<Action>) onBeginDrag.CheckNotNull<Action>();
      this.m_dragListener.Value.OnDragAction = (Option<Action>) onDrag.CheckNotNull<Action>();
      this.m_dragListener.Value.OnEndDragAction = (Option<Action>) onEndDrag.CheckNotNull<Action>();
      return this;
    }

    internal Btn SetDragEnabled(bool isEnabled)
    {
      if (this.m_dragListener.HasValue)
        this.m_dragListener.Value.enabled = isEnabled;
      return this;
    }

    internal Btn PlayErrorSoundWhenDisabled() => this;

    /// <summary>
    /// Prevents playing error sound when the player clicks on the button when it is disabled.
    /// </summary>
    public Btn DoNotPlayErrorSoundWhenDisabled()
    {
      this.m_listener.LeftClickActionWhenDisabled = (Option<Action>) Option.None;
      return this;
    }

    public Btn SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      if (this.m_listener.MouseEnterAction.HasValue)
      {
        this.m_listener.MouseEnterAction = (Option<Action>) (this.m_listener.MouseEnterAction.Value + enterAction);
        this.m_listener.MouseLeaveAction = (Option<Action>) (this.m_listener.MouseLeaveAction.Value + leaveAction);
      }
      else
      {
        this.m_listener.MouseEnterAction = (Option<Action>) enterAction.CheckNotNull<Action>();
        this.m_listener.MouseLeaveAction = (Option<Action>) leaveAction.CheckNotNull<Action>();
      }
      return this;
    }

    public virtual Btn SetEnabled(bool enabled)
    {
      if (this.m_button.interactable == enabled)
        return this;
      this.m_button.interactable = enabled;
      this.updateVisualsForEnabled(enabled);
      return this;
    }

    private void updateVisualsForEnabled(bool enabled)
    {
      if (this.m_buttonStyle.BackgroundClrWhenDisabled.HasValue)
      {
        ColorRgba colorRgba = this.m_buttonStyle.BackgroundClrWhenDisabled.Value;
        if (this.m_bgImage.HasValue && this.m_buttonStyle.BackgroundClr.HasValue)
          this.m_bgImage.Value.color = (!enabled ? colorRgba : this.m_buttonStyle.BackgroundClr.Value).ToColor();
      }
      if (!this.m_buttonStyle.ForegroundClrWhenDisabled.HasValue)
        return;
      ColorRgba colorRgba1 = this.m_buttonStyle.ForegroundClrWhenDisabled.Value;
      if (this.m_icon.HasValue)
        this.m_icon.Value.SetColor(!enabled ? colorRgba1 : this.m_buttonStyle.Text.Color);
      if (!this.m_text.HasValue)
        return;
      this.m_text.Value.SetColor(!enabled ? colorRgba1 : this.m_buttonStyle.Text.Color);
    }

    public float GetOptimalWidth()
    {
      if (this.m_buttonStyle.Width != 0)
        return (float) this.m_buttonStyle.Width;
      float leftRightOffset = this.getButtonPaddings().LeftRightOffset;
      if (this.m_text.HasValue)
        leftRightOffset += this.m_text.Value.GetPreferedWidth();
      if (this.m_icon.HasValue)
      {
        leftRightOffset += this.m_iconSize.x;
        if (this.m_text.HasValue)
          leftRightOffset += 6f;
      }
      return leftRightOffset;
    }

    private float getOptimalHeight()
    {
      if (this.m_buttonStyle.Height != 0)
        return (float) this.m_buttonStyle.Height;
      float num1 = 0.0f;
      Offset buttonPaddings;
      if (this.m_text.HasValue)
      {
        buttonPaddings = this.getButtonPaddings();
        num1 = (float) (18.0 + (double) buttonPaddings.TopBottomOffset);
      }
      double self = (double) num1;
      double y = (double) this.m_iconSize.y;
      buttonPaddings = this.getButtonPaddings();
      double topBottomOffset = (double) buttonPaddings.TopBottomOffset;
      double num2 = y + topBottomOffset;
      return ((float) self).Max((float) num2);
    }

    public Vector2 GetOptimalSize() => new Vector2(this.GetOptimalWidth(), this.getOptimalHeight());

    public Btn AddToolTip(string toolTip)
    {
      this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this).SetText(toolTip);
      return this;
    }

    public Btn AddToolTip(LocStr toolTip)
    {
      this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this).SetText((LocStrFormatted) toolTip);
      return this;
    }

    [MustUseReturnValue]
    public Tooltip AddToolTipAndReturn()
    {
      return this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this);
    }

    public Btn TextBestFitEnabled(int maxFontSize = 16)
    {
      this.m_text.ValueOrNull?.BestFitEnabled(maxFontSize);
      return this;
    }

    private void updateSizeAndNotify()
    {
      if (!this.DynamicSizeEnabled)
        return;
      this.SetSize<Btn>(this.GetOptimalSize());
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    static Btn() => xxhJUtQyC9HnIshc6H.OukgcisAbr();
  }
}
