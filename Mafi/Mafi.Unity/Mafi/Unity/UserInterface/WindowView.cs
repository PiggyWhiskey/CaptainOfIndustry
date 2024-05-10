// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.WindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.TopStatusBar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  public abstract class WindowView : View, IWindow, IDynamicSizeElement, IUiElement
  {
    private static readonly Offset s_defaultWindowCornerOffset;
    protected Txt m_headerText;
    private Panel m_content;
    private float m_footerHeight;
    private Vector2 m_windowSize;
    private float m_headerRightOffset;
    protected Option<Action> OnCloseButtonClick;
    private PanelWithShadow m_headerContainer;
    protected Panel m_headerHolder;
    private readonly WindowView.FooterStyle m_footer;
    private readonly bool m_noHeader;
    private Panel m_footerPanel;
    private StackContainer m_leftButtonsContainer;

    public event Action<IUiElement> SizeChanged;

    public Vector2 ResolveWindowSize(float? customMargin = null)
    {
      float valueOrDefault = customMargin.GetValueOrDefault(240f);
      float self1 = this.Builder.MainCanvas.GetWidth() - valueOrDefault;
      float self2 = this.Builder.MainCanvas.GetHeight() - valueOrDefault;
      return new Vector2(self1.Min(1600f), self2.Min(1200f));
    }

    internal Vector2 ResolveWindowSizeWithScale(float? customMargin = null)
    {
      float valueOrDefault = customMargin.GetValueOrDefault(240f);
      float scaleFactor = this.Builder.MainCanvas.GameObject.GetComponent<CanvasScaler>().scaleFactor;
      float self1 = this.Builder.MainCanvas.GetWidth() / scaleFactor - valueOrDefault;
      float self2 = this.Builder.MainCanvas.GetHeight() / scaleFactor - valueOrDefault;
      return new Vector2(self1.Min(1600f), self2.Min(1200f));
    }

    protected WindowView(string id, WindowView.FooterStyle footer = WindowView.FooterStyle.Round, bool noHeader = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_windowSize = new Vector2(400f, 400f);
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_footer = footer;
      this.m_noHeader = noHeader;
    }

    protected override void BuildUi()
    {
      this.m_headerContainer = this.Builder.NewPanelWithShadow("Header", (IUiElement) this).SetBackground(this.Style.Panel.Border.Color).SetBorderStyle(this.Style.Panel.Border).AddShadowBottom().PutToTopOf<PanelWithShadow>((IUiElement) this, this.m_noHeader ? 0.0f : (float) this.Style.Panel.HeaderHeight);
      this.m_leftButtonsContainer = this.Builder.NewStackContainer("LeftButtonsContainer", (IUiElement) this.m_headerContainer).SetBackground(this.Style.Panel.Border.Color).SetBorderStyle(this.Style.Panel.Border).SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.RightToLeft).SetItemSpacing(1f).PutToLeftOf<StackContainer>((IUiElement) this.m_headerContainer, 0.0f);
      if (this.OnCloseButtonClick.HasValue)
      {
        this.Builder.NewBtn("CloseButton", (IUiElement) this.m_headerContainer).SetButtonStyle(this.Style.Panel.HeaderCloseButton).SetText("X").OnClick(this.OnCloseButtonClick.Value, muted: true).PutToRightOf<Btn>((IUiElement) this.m_headerContainer, this.Style.Panel.HeadeButtonWidth, Offset.TopBottom(1f));
        this.m_headerRightOffset += this.Style.Panel.HeadeButtonWidth + 1f;
      }
      this.m_headerHolder = this.Builder.NewPanel("TitleHolder", (IUiElement) this.m_headerContainer).SetBackground(this.Style.Panel.HeaderBackground);
      this.updateTitleOffsets();
      this.m_headerText = this.Builder.NewTxt("Title", (IUiElement) this.m_headerHolder).SetTextStyle(this.Style.Panel.WindowTitleText).SetAlignment(TextAnchor.MiddleCenter).EnableRichText().BestFitEnabled(this.Style.Panel.WindowTitleText.FontSize).PutTo<Txt>((IUiElement) this.m_headerHolder);
      if (this.m_footer != WindowView.FooterStyle.None)
      {
        this.m_footerHeight = this.Style.Panel.WindowHeight;
        this.m_footerPanel = this.Builder.NewPanel("Footer", (IUiElement) this).PutToBottomOf<Panel>((IUiElement) this, this.m_footerHeight);
        if (this.m_footer == WindowView.FooterStyle.Round)
          this.m_footerPanel.SetBackground(this.Builder.AssetsDb.GetSharedSprite(this.Style.Icons.Footer), new ColorRgba?(this.Style.Panel.HeaderBackground));
        else
          this.m_footerPanel.SetBackground(this.Style.Panel.HeaderBackground);
        this.m_footerPanel.SetBorderStyle(new BorderStyle((ColorRgba) 0, 0.0f));
      }
      this.m_content = this.Builder.NewPanel("Content", (IUiElement) this).SetBackground(this.Style.Panel.Background).PutTo<Panel>((IUiElement) this, new Offset(0.0f, this.m_headerContainer.GetHeight(), 0.0f, this.m_footerHeight));
      this.BuildWindowContent();
      this.m_content.SetBorderStyle(this.Style.Panel.WindowContentBorder);
    }

    private void updateTitleOffsets()
    {
      this.m_headerHolder.PutTo<Panel>((IUiElement) this.m_headerContainer, Offset.Left(this.m_leftButtonsContainer.GetDynamicWidth()) + Offset.Right(this.m_headerRightOffset) + Offset.All(1f));
    }

    protected abstract void BuildWindowContent();

    protected internal void SetContentBg(ColorRgba bgColor)
    {
      this.m_content.SetBackground(bgColor);
    }

    protected void SetCustomFooter(IUiElement customFooter)
    {
      this.m_footerPanel.Hide<Panel>();
      customFooter.PutToBottomOf<IUiElement>((IUiElement) this, this.m_footerHeight).Show<IUiElement>();
    }

    protected void HideCustomFooter(IUiElement customFooter)
    {
      this.m_footerPanel.Show<Panel>();
      customFooter.Hide<IUiElement>();
    }

    protected void SetContentSize(Vector2 size) => this.SetContentSize(size.x, size.y);

    protected void SetContentSize(float width, float height)
    {
      this.m_windowSize = new Vector2(width, this.getRequiredWindowHeight(height));
      this.SetSize<WindowView>(this.m_windowSize);
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    protected void ClipWindowHeightToScreenSize(float contentWidth, float optimalContentHeight)
    {
      float num = this.m_headerContainer.GetHeight() + this.Style.Panel.Padding + this.m_footerHeight;
      float y = (this.ResolveWindowSize(new float?(20f)).y - num).Min(optimalContentHeight);
      this.SetContentSize(new Vector2(contentWidth, y));
    }

    protected void SetHeaderHeight(float height)
    {
      this.m_headerContainer.SetHeight<PanelWithShadow>(height);
      this.m_content.PutTo<Panel>((IUiElement) this, new Offset(0.0f, this.m_headerContainer.GetHeight(), 0.0f, this.m_footerHeight));
      this.SetContentSize(this.m_content.GetSize());
    }

    protected void SetHeaderAlignment(TextAnchor anchor, Offset offset)
    {
      this.m_headerText.SetAlignment(anchor).PutTo<Txt>((IUiElement) this.m_headerHolder, offset);
    }

    public void MakeMovable(Action<Offset> moveCallback = null, IUiElement elementToMove = null)
    {
      this.m_headerHolder.GameObject.AddComponent<ElementMoveHandler>().Initialize(elementToMove ?? (IUiElement) this, this.Builder.MainCanvas, new Offset(50f, 90f, 50f, -100f), moveCallback ?? (Action<Offset>) (x => { }));
    }

    public void SetOnCloseButtonClickAction(Action onCloseButtonClick)
    {
      this.OnCloseButtonClick = (Option<Action>) onCloseButtonClick.CheckNotNull<Action>();
    }

    internal void CallOnClose()
    {
      Action valueOrNull = this.OnCloseButtonClick.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull();
    }

    private float getRequiredWindowHeight(float contentHeight)
    {
      return contentHeight + this.m_headerContainer.GetHeight() + this.Style.Panel.Padding + this.m_footerHeight;
    }

    protected void AddHeaderButton(IUiElement btnElement)
    {
      this.m_leftButtonsContainer.Append(btnElement, new float?(this.Style.Panel.HeadeButtonWidth));
      this.updateTitleOffsets();
    }

    protected void SetHeaderButtonVisibility(IUiElement btnElement, bool isVisible)
    {
      this.m_leftButtonsContainer.SetItemVisibility(btnElement, isVisible);
      this.updateTitleOffsets();
    }

    protected Panel AddTopButtonsContainer(StackContainer parent)
    {
      return this.Builder.NewPanel("Top buttons container", (IUiElement) parent).AppendTo<Panel>(parent, new float?(this.Style.Panel.TopSquareButtonSize.y));
    }

    protected StackContainer AddLeftVerticalButtonsContainer(IUiElement parent)
    {
      return this.Builder.NewStackContainer("Left buttons container", parent).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(this.Style.Panel.Padding).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutTo<StackContainer>(parent, Offset.Left(this.Style.Panel.Padding));
    }

    protected StackContainer AddRightVerticalButtonsContainer(IUiElement parent)
    {
      return this.Builder.NewStackContainer("Right buttons container", parent).SetStackingDirection(StackContainer.Direction.RightToLeft).SetItemSpacing(this.Style.Panel.Padding).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Right(this.Style.Panel.Padding)).PutToRightOf<StackContainer>(parent, 0.0f);
    }

    protected Txt AddSectionTitle(
      StackContainer parent,
      LocStrFormatted title,
      LocStrFormatted? tooltip = null,
      Offset? extraOffset = null)
    {
      return this.Builder.AddSectionTitle(parent, title, tooltip, extraOffset);
    }

    protected Txt AddSectionTitle(
      StackContainer parent,
      string title,
      string tooltip = null,
      Offset? extraOffset = null)
    {
      return this.Builder.AddSectionTitle(parent, title, tooltip, extraOffset);
    }

    protected Txt CreateSectionTitle(
      IUiElement parent,
      LocStrFormatted title,
      LocStrFormatted? tooltip = null)
    {
      return this.Builder.CreateSectionTitle(parent, title, tooltip);
    }

    public Panel AddOverlayPanel(StackContainer parent, int height = 35, Offset offset = default (Offset))
    {
      return this.Builder.AddOverlayPanel(parent, height, offset);
    }

    protected Txt AddSectionTitleWithIcon(StackContainer parent, string title, string iconPath)
    {
      Panel parent1 = this.Builder.NewPanel("Title", (IUiElement) parent).AppendTo<Panel>(parent, new float?(this.Style.Panel.LineHeight), new Offset(0.0f, this.Style.Panel.SectionTitleTopPadding, this.Style.Panel.Padding, 0.0f));
      this.Builder.NewIconContainer("Icon", (IUiElement) parent1).SetIcon(iconPath, this.Style.Panel.SectionTitle.Color).PutToLeftOf<IconContainer>((IUiElement) parent1, this.Style.Panel.LineHeight - 2f);
      return this.Builder.NewTxt(title, (IUiElement) parent1).SetTextStyle(this.Style.Panel.SectionTitle).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) parent1, Offset.Left((float) ((double) this.Style.Panel.Padding + (double) this.Style.Panel.LineHeight - 2.0)));
    }

    protected Txt AddLabel(StackContainer parent, string text)
    {
      return this.Builder.NewTxt(text, (IUiElement) parent).SetTextStyle(this.Style.Panel.Text).SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(parent, new float?(this.Style.Panel.LineHeight), new Offset(0.0f, 0.0f, this.Style.Panel.Indent, 0.0f));
    }

    protected SwitchBtn AddSwitch(
      StackContainer parent,
      string title,
      Action<bool> action,
      UpdaterBuilder updater,
      Func<bool> provider,
      string tooltip = null)
    {
      return this.Builder.AddSwitch(parent, title, action, updater, provider, tooltip);
    }

    protected SwitchBtn AddSwitch(
      StackContainer parent,
      string title,
      Action<bool> action,
      string tooltip = null)
    {
      return this.Builder.AddSwitch(parent, title, action, tooltip);
    }

    /// <summary>
    /// Used to overlay a window when another window is active above it.
    /// </summary>
    internal Panel AddOverlay(Action onClickAction)
    {
      return this.Builder.NewPanel("Overlay", (IUiElement) this).SetBackground(new ColorRgba(6316128, 125)).OnClick(onClickAction).PutTo<Panel>((IUiElement) this).Hide<Panel>();
    }

    protected TxtField AddSearchBoxToHeader()
    {
      return this.Builder.NewTxtField("Search", (IUiElement) this.m_headerContainer).SetStyle(this.Style.Global.LightTxtFieldStyle).SetPlaceholderText(Tr.Search).SetCharLimit(30).PutToRightOf<TxtField>((IUiElement) this.m_headerContainer, 160f, Offset.TopBottom(5f) + Offset.Right(5f));
    }

    /// <summary>
    /// Positions the window to the center of the screen while setting it the given dimensions.
    /// </summary>
    protected void PositionSelfToCenter(bool gameOverlayParent = false, Offset offset = default (Offset))
    {
      IUiElement parent = this.Parent.ValueOrNull ?? (gameOverlayParent ? (IUiElement) this.Builder.GameOverlay : (IUiElement) this.Builder.MainCanvas);
      if (!gameOverlayParent)
        this.EnableClippingPrevention();
      this.PutToCenterMiddleOf<WindowView>(parent, this.m_windowSize, offset);
    }

    protected void PositionSelfToLeftBottom(bool gameOverlayParent = false)
    {
      this.DisableClippingPrevention();
      this.PutToLeftBottomOf<WindowView>(gameOverlayParent ? (IUiElement) this.Builder.GameOverlay : (IUiElement) this.Builder.MainCanvas, this.m_windowSize, Offset.Bottom((float) this.Builder.Style.EntitiesMenu.MenuHeight + this.Builder.Style.Toolbar.IconsOnlyMenuStripHeight));
    }

    /// <summary>
    /// Positions the window to the center of the screen while setting it the given dimensions.
    /// </summary>
    protected void PositionSelfToFullscreen(bool ignorePaddingForStatusBar = false)
    {
      this.DisableClippingPrevention();
      IUiElement parent1 = this.Parent.ValueOrNull ?? (IUiElement) this.Builder.MainCanvas;
      if (ignorePaddingForStatusBar)
      {
        this.PutTo<WindowView>(parent1);
      }
      else
      {
        IUiElement parent2 = parent1;
        StatusBar valueOrNull = this.Builder.StatusBar.ValueOrNull;
        Offset offset = Offset.Top(valueOrNull != null ? valueOrNull.GetHeight() : 30f);
        this.PutTo<WindowView>(parent2, offset);
      }
    }

    protected Panel GetContentPanel()
    {
      Assert.That<Panel>(this.m_content).IsNotNull<Panel>("You have to build the window using BuildWindow() before accessing its content panel!");
      return this.m_content;
    }

    public void SetTitle(LocStrFormatted text) => this.SetTitle(text.Value);

    public void SetTitle(LocStrFormatted text, string shortcut)
    {
      this.m_headerText.EnableRichText();
      this.SetTitle(text.Value + " [<color=#f0a926>" + shortcut + "</color>]");
    }

    public void SetTitle(string text) => this.m_headerText.SetText(text);

    internal void SetTitleAsBig() => this.m_headerText.BestFitEnabled();

    protected void SetTitleStyle(ColorRgba textColor) => this.m_headerText.SetColor(textColor);

    protected void SetTitleBg(ColorRgba bgColor) => this.m_headerHolder.SetBackground(bgColor);

    static WindowView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WindowView.s_defaultWindowCornerOffset = Offset.TopLeft(300f, 300f);
    }

    protected enum FooterStyle
    {
      None,
      Flat,
      Round,
    }
  }
}
