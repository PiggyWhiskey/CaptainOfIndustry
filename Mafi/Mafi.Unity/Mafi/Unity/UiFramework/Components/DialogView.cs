// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.DialogView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  internal class DialogView
  {
    public float Width;
    private readonly UiBuilder m_builder;
    private readonly StackContainer m_itemsContainer;
    private readonly PanelWithShadow m_container;
    private readonly Txt m_text;
    private readonly StackContainer m_btnsContainer;
    private readonly Panel m_btnsHolder;

    protected StackContainer ItemsContainer => this.m_itemsContainer;

    public DialogView(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Width = 450f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      UiStyle style = this.m_builder.Style;
      this.m_container = this.m_builder.NewPanelWithShadow("Dialog", (IUiElement) this.m_builder.GameOverlaySuper).SetBackground((ColorRgba) 2236962).AddShadowRightBottom();
      this.m_container.Hide<PanelWithShadow>();
      this.m_itemsContainer = this.m_builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.All(25f)).SetItemSpacing(10f).PutToTopOf<StackContainer>((IUiElement) this.m_container, 0.0f);
      this.m_text = this.m_builder.NewTxt("Text").SetText("").SetTextStyle(style.Global.TextBig).SetAlignment(TextAnchor.MiddleCenter).EnableRichText().AppendTo<Txt>(this.m_itemsContainer, new float?(35f), Offset.Bottom(10f));
      this.m_btnsHolder = this.m_builder.NewPanel("BtnsHolder").AppendTo<Panel>(this.m_itemsContainer, new float?(35f));
      this.m_btnsContainer = this.m_builder.NewStackContainer("Dialog").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(20f).PutToCenterOf<StackContainer>((IUiElement) this.m_btnsHolder, 0.0f);
    }

    public void HighlightAsDanger()
    {
      this.m_container.SetBorderStyle(new BorderStyle(this.m_builder.Style.Global.DangerClr, 2f));
    }

    public void HighlightAsSuccess()
    {
      this.m_container.SetBorderStyle(new BorderStyle((ColorRgba) 2523182, 2f));
    }

    public void HighlightAsGeneral()
    {
      this.m_container.SetBorderStyle(new BorderStyle((ColorRgba) 10132122, 2f));
    }

    public void HighlightAsSettings()
    {
      this.m_container.SetBorderStyle(new BorderStyle((ColorRgba) 0, 2f));
      this.m_container.SetBackground(this.m_builder.Style.Global.PanelsBg);
    }

    public bool IsVisible() => this.m_container.IsVisible();

    public void SetMessage(LocStrFormatted message)
    {
      this.m_text.SetText(message);
      this.m_itemsContainer.UpdateItemHeight((IUiElement) this.m_text, this.m_text.GetPreferedHeight((float) ((double) this.Width - 50.0 - 30.0)).Max(35f));
    }

    protected void HideMessage() => this.m_itemsContainer.HideItem((IUiElement) this.m_text);

    public void SetLargeMessageOnce(LocStrFormatted message)
    {
      this.Width = 700f;
      this.m_itemsContainer.Remove((IUiElement) this.m_text);
      this.m_text.SetAlignment(TextAnchor.UpperLeft);
      this.m_text.SetText(message);
      this.m_text.SetHeight<Txt>(this.m_text.GetPreferedHeight((float) ((double) this.Width - 50.0 - 30.0)));
      float num = this.m_text.GetHeight().Clamp(40f, 400f);
      this.m_builder.NewScrollableContainer("ScrollableTitles").AddVerticalScrollbar().AppendTo<ScrollableContainer>(this.m_itemsContainer, new float?(num)).AddItemTop((IUiElement) this.m_text);
    }

    protected void AppendCustomElement(IUiElement element)
    {
      this.m_itemsContainer.StartBatchOperation();
      this.m_itemsContainer.Remove((IUiElement) this.m_btnsHolder);
      this.m_itemsContainer.Append(element, new float?(element.GetHeight()));
      this.m_btnsHolder.AppendTo<Panel>(this.m_itemsContainer, new float?(35f));
      this.m_itemsContainer.FinishBatchOperation();
    }

    protected void SetCustomItemVisibility(IUiElement element, bool isVisible)
    {
      this.m_itemsContainer.SetItemVisibility(element, isVisible);
    }

    public Btn AppendBtnPrimary(LocStrFormatted text, Option<string> iconPath = default (Option<string>))
    {
      Btn objectToPlace = this.m_builder.NewBtnPrimaryBig("BtnPrimary").SetText(text).DropShadow();
      if (iconPath.HasValue)
        objectToPlace.SetIcon(iconPath.Value, 16.Vector2());
      objectToPlace.AppendTo<Btn>(this.m_btnsContainer, new Vector2?(objectToPlace.GetOptimalSize()), ContainerPosition.MiddleOrCenter);
      return objectToPlace;
    }

    public Btn AppendBtnDanger(LocStrFormatted text)
    {
      Btn objectToPlace = this.m_builder.NewBtnDangerBig("BtnDanger").SetText(text);
      objectToPlace.AppendTo<Btn>(this.m_btnsContainer, new Vector2?(objectToPlace.GetOptimalSize()), ContainerPosition.MiddleOrCenter);
      return objectToPlace;
    }

    public Btn AppendBtnGeneral(LocStrFormatted text, Option<string> iconPath = default (Option<string>))
    {
      Btn objectToPlace = this.m_builder.NewBtnGeneralBig("BtnGeneral").SetText(text);
      if (iconPath.HasValue)
        objectToPlace.SetIcon(iconPath.Value, 16.Vector2());
      objectToPlace.AppendTo<Btn>(this.m_btnsContainer, new Vector2?(objectToPlace.GetOptimalSize()), ContainerPosition.MiddleOrCenter);
      return objectToPlace;
    }

    public void SetBtnVisibility(Btn btn, bool isVisible)
    {
      this.m_btnsContainer.SetItemVisibility((IUiElement) btn, isVisible);
    }

    public void Show()
    {
      this.m_builder.GameOverlaySuper.Show<Panel>();
      this.m_container.PutToCenterMiddleOf<PanelWithShadow>((IUiElement) this.m_builder.GameOverlaySuper, new Vector2(this.Width, this.m_itemsContainer.GetDynamicHeight()));
      this.m_container.Show<PanelWithShadow>();
    }

    public void Hide()
    {
      this.m_container.Hide<PanelWithShadow>();
      this.m_builder.GameOverlaySuper.Hide<Panel>();
    }

    public void ShowInCustomOverlay(IUiElement overlay)
    {
      overlay.Show<IUiElement>();
      this.m_container.PutToCenterMiddleOf<PanelWithShadow>(overlay, new Vector2(this.Width, this.m_itemsContainer.GetDynamicHeight()));
      this.m_container.Show<PanelWithShadow>();
    }

    public void HideFromCustomOverlay(IUiElement overlay)
    {
      this.m_container.Hide<PanelWithShadow>();
      overlay.Hide<IUiElement>();
    }
  }
}
