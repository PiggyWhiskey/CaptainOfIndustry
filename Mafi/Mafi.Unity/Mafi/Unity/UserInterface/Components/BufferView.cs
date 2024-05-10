// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.BufferView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class BufferView : IUiElement
  {
    protected UiBuilder Builder;
    protected Option<ProductProto> Product;
    public QuantityBar Bar;
    private Option<Action> m_clickAction;
    private readonly Option<Action> m_trashAction;
    private readonly Option<Action> m_plusBtnAction;
    private readonly bool m_hasSliders;
    private readonly bool m_isCompact;
    private Panel m_container;
    private Option<ProtoWithIcon<ProductProto>> m_productIcon;
    private Panel m_barHolder;
    private Option<Btn> m_plusBtn;
    private Option<Btn> m_trashBtn;
    private Panel m_iconContainer;
    private Panel m_plusBtnHolder;
    private Option<Btn> m_plusBtnWithProduct;
    private Option<IconContainer> m_plusBtnProductIcon;
    private Option<Txt> m_plusBtnProductName;
    private Option<string> m_textToShowWhenEmpty;
    private Txt m_textElementEmpty;
    protected Txt m_label;
    protected Panel m_sliderHandle;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public Option<Btn> TrashBtn => this.m_trashBtn;

    public BufferView(
      IUiElement parent,
      UiBuilder builder,
      Action trashAction,
      Action plusBtnAction,
      bool hasSliders,
      Btn rightButton = null,
      bool isCompact = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_trashAction = (Option<Action>) trashAction;
      this.m_plusBtnAction = (Option<Action>) plusBtnAction;
      this.m_hasSliders = hasSliders;
      this.m_isCompact = isCompact;
      this.build(parent, builder, rightButton);
    }

    private void build(IUiElement parent, UiBuilder builder, Btn rightButton = null)
    {
      this.Builder = builder;
      UiStyle style = builder.Style;
      this.m_container = builder.NewPanel("BufferRow", parent).SetBackground(style.Panel.ItemOverlay);
      Panel parent1 = builder.NewPanel("InnerContainer", (IUiElement) this.m_container).PutTo<Panel>((IUiElement) this.m_container, Offset.LeftRight(style.Panel.Padding));
      this.m_iconContainer = builder.NewPanel("IconContainer", (IUiElement) parent1).PutToLeftOf<Panel>((IUiElement) parent1, style.ProductWithIcon.Size.x);
      if (this.m_plusBtnAction.HasValue)
      {
        this.m_plusBtnHolder = builder.NewPanel("PlusBtnContainer", (IUiElement) this.m_iconContainer).PutToCenterMiddleOf<Panel>((IUiElement) this.m_iconContainer, 38.Vector2());
        this.m_plusBtn = (Option<Btn>) builder.NewBtnPrimary("PlusBtn", (IUiElement) this.m_plusBtnHolder).SetIcon(style.Icons.Plus, new Offset?(Offset.All(8f))).OnClick(this.m_plusBtnAction.Value).PutTo<Btn>((IUiElement) this.m_plusBtnHolder);
        this.m_plusBtnWithProduct = (Option<Btn>) builder.NewBtn("PlusBtn", (IUiElement) this.m_plusBtnHolder).OnClick(this.m_plusBtnAction.Value).SetButtonStyle(style.Global.GeneralBtn).PlayErrorSoundWhenDisabled().PutToTopOf<Btn>((IUiElement) this.m_plusBtnHolder, 38f, Offset.Top(-10f));
        if (this.m_trashAction.HasValue)
          this.m_plusBtnWithProduct.Value.OnRightClick(this.m_trashAction.Value);
        this.m_plusBtnProductIcon = (Option<IconContainer>) builder.NewIconContainer("Icon", (IUiElement) this.m_plusBtnWithProduct.Value).PutTo<IconContainer>((IUiElement) this.m_plusBtnWithProduct.Value, Offset.All(2f));
        this.m_plusBtnProductName = (Option<Txt>) builder.NewTxt("ProductName", (IUiElement) this.m_plusBtnWithProduct.Value).SetTextStyle(style.Global.Text).SetAlignment(TextAnchor.UpperCenter).PutToCenterBottomOf<Txt>((IUiElement) this.m_plusBtnWithProduct.Value, new Vector2(60f, 40f), Offset.Bottom(-40f));
      }
      else
      {
        this.m_productIcon = (Option<ProtoWithIcon<ProductProto>>) new ProtoWithIcon<ProductProto>((IUiElement) this.m_iconContainer, builder).PutTo<ProtoWithIcon<ProductProto>>((IUiElement) this.m_iconContainer);
        if (this.m_isCompact)
          this.ShowProductTitleOnHoverOnly(true, new int?(30));
        this.m_productIcon.Value.SetOnRightClick(new Action(this.onProductRightClick));
      }
      float bottom = !this.m_hasSliders ? (!this.m_isCompact ? 20f : 22f) : style.BufferView.SliderHandleHeight + style.BufferView.SliderBottomOffset - style.BufferView.SliderBarOverflow;
      float num = 0.0f;
      if (this.m_trashAction.HasValue)
      {
        Assert.That<Btn>(rightButton).IsNull<Btn>("Cannot add extra button when there is already trash.");
        this.m_trashBtn = (Option<Btn>) builder.NewBtn("Trash icon", (IUiElement) parent1).SetButtonStyle(style.Global.GeneralBtnToToggle).SetIcon("Assets/Unity/UserInterface/General/Trash128.png").OnClick((Action) (() => this.m_trashAction.Value())).PutToRightTopOf<Btn>((IUiElement) parent1, style.BufferView.TrashIconSize, Offset.TopRight(20f, 6f));
        num += this.m_trashBtn.Value.GetWidth();
      }
      else if (rightButton != null)
      {
        rightButton.PutToRightOf<Btn>((IUiElement) parent1, rightButton.GetWidth(), Offset.Right(6f) + Offset.TopBottom(this.m_isCompact ? 5f : 20f));
        num += rightButton.GetWidth();
      }
      this.m_barHolder = builder.NewPanel("Bar container", (IUiElement) parent1).PutTo<Panel>((IUiElement) parent1, new Offset(style.Panel.Padding + style.BufferView.SliderHandleWidth / 2f + num, 0.0f, this.m_iconContainer.GetWidth(), 0.0f));
      this.m_textElementEmpty = builder.NewTxt("TextWhenEmpty", (IUiElement) this.m_barHolder).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Builder.Style.Global.Text).PutTo<Txt>((IUiElement) this.m_barHolder).Hide<Txt>();
      this.Bar = new QuantityBar(builder, this.m_hasSliders, new Action(this.onClick)).PutTo<QuantityBar>((IUiElement) this.m_barHolder, new Offset(0.0f, this.m_isCompact ? 5f : 20f, style.BufferView.SliderHandleWidth / 2f, bottom));
      this.m_iconContainer.SendToFront<Panel>();
    }

    private void onProductRightClick()
    {
      if (!this.Product.HasValue)
        return;
      this.Builder.GetDependency<RecipesBookController>().ValueOrNull?.OpenForProduct(this.Product.Value);
    }

    public void BindProductPicker(
      WindowView parentWindow,
      Panel overlay,
      ProtoPicker<ProductProto> productPicker)
    {
      productPicker.PutToLeftTopOf<ProtoPicker<ProductProto>>((IUiElement) this.m_plusBtnHolder, productPicker.GetSize(), Offset.Left(this.m_plusBtnHolder.GetWidth() - 1f));
      productPicker.OnShowStart += (Action) (() =>
      {
        overlay.Show<Panel>();
        this.m_plusBtnHolder.SetParent<Panel>((IUiElement) parentWindow);
      });
      productPicker.OnHide += (Action) (() =>
      {
        overlay.Hide<Panel>();
        this.m_plusBtnHolder.PutToCenterMiddleOf<Panel>((IUiElement) this.m_iconContainer, 38.Vector2());
      });
    }

    public void ShowProductTitleOnHoverOnly(bool makeIconTall, int? maxWidthOverflowForTooltip = null)
    {
      this.m_productIcon.Value.ShowTitleOnHoverOnly(maxWidthOverflowForTooltip);
      this.m_container.OnMouseEnter((Action) (() => this.m_productIcon.Value.ShowHoveredProtoName()));
      this.m_container.OnMouseLeave((Action) (() => this.m_productIcon.Value.HideHoveredProductName()));
      if (makeIconTall)
        this.m_productIcon.Value.PutTo<ProtoWithIcon<ProductProto>>((IUiElement) this.m_iconContainer, Offset.All(3f));
      else
        this.m_productIcon.Value.PutTo<ProtoWithIcon<ProductProto>>((IUiElement) this.m_iconContainer, Offset.All(3f) + Offset.Bottom(this.Builder.Style.ProductWithIcon.QuantityLineHeight));
    }

    public BufferView SetAsSuperCompact()
    {
      this.Bar.PutTo<QuantityBar>((IUiElement) this.m_barHolder, new Offset(0.0f, 5f, this.Builder.Style.BufferView.SliderHandleWidth / 2f, 5f));
      return this;
    }

    public BufferView SetOnClickAction(Action clickAction)
    {
      this.m_clickAction = (Option<Action>) clickAction;
      return this;
    }

    /// <summary>
    /// Set the slider to use a negative color for displayed quantity.
    /// </summary>
    public BufferView UseNegativeColor()
    {
      this.Bar.UseNegativeColor();
      return this;
    }

    /// <summary>
    /// Set the slider to use a positive color for displayed quantity.
    /// </summary>
    public BufferView UsePositiveColor()
    {
      this.Bar.UsePositiveColor();
      return this;
    }

    /// <summary>
    /// Set the slider to use a neutral color for displayed quantity.
    /// </summary>
    public BufferView UseNeutralColor()
    {
      this.Bar.UseNeutralColor();
      return this;
    }

    protected Slidder CreateSlider(
      int steps,
      ColorRgba color,
      string handleIcon,
      Action<float> onValueChange,
      Action<float> onValueCommit,
      string labelText,
      bool alignLabelToRight = false)
    {
      UiStyle style = this.Builder.Style;
      this.m_sliderHandle = this.Builder.NewPanel("Slider handle");
      float num = (float) (((double) style.BufferView.SliderHandleWidth - (double) style.BufferView.SliderPointerLineWidth) / 2.0);
      this.Builder.NewPanel("").SetBackground(color).PutTo<Panel>((IUiElement) this.m_sliderHandle, new Offset(num, 19f, num, style.BufferView.SliderHandleHeight));
      this.Builder.NewIconContainer("").SetIcon(handleIcon, color).PutToBottomOf<IconContainer>((IUiElement) this.m_sliderHandle, style.BufferView.SliderHandleHeight);
      this.m_label = this.Builder.NewTxt("Label").SetText(labelText).SetTextStyle(style.BufferView.LabelTextStyle.Extend(new ColorRgba?(color)));
      Vector2 preferedSize = this.m_label.GetPreferedSize();
      if (alignLabelToRight)
        this.m_label.PutToRightTopOf<Txt>((IUiElement) this.m_sliderHandle, preferedSize, Offset.Right(-preferedSize.x + style.BufferView.SliderHandleWidth));
      else
        this.m_label.PutToLeftTopOf<Txt>((IUiElement) this.m_sliderHandle, preferedSize);
      return this.Builder.NewSlider("Slider").SetCustomHandle((IUiElement) this.m_sliderHandle, style.BufferView.SliderHandleWidth).SetCustomLabel(this.m_label).WholeNumbersOnly().SetValuesRange(0.0f, (float) steps).OnValueChange(onValueChange, onValueCommit).PutTo<Slidder>((IUiElement) this.m_barHolder, new Offset(style.BufferView.SliderHandleWidth / 2f, 0.0f, 0.0f, style.BufferView.SliderBottomOffset));
    }

    public void UpdateState(ProductProto product, Quantity capacity, Quantity quantity)
    {
      this.UpdateState(Option.Some<ProductProto>(product), capacity, quantity);
    }

    public virtual void UpdateState(
      Option<ProductProto> product,
      Quantity capacity,
      Quantity quantity)
    {
      Assert.That<Quantity>(capacity).IsNotNegative();
      this.setProduct(product);
      this.Bar.UpdateValues(capacity, quantity);
      this.updatePlusBtn(quantity);
    }

    public void UpdateState(ProductProto product, Quantity quantity, Percent percent)
    {
      this.setProduct((Option<ProductProto>) product);
      if (this.Product.HasValue)
        this.Bar.UpdateValues(percent, string.Format("{0} ({1})", (object) product.WithQuantity(quantity).FormatNumberAndUnitOnly(), (object) percent.ToStringRounded()));
      this.updatePlusBtn(quantity);
    }

    public void UpdateState(
      Option<ProductProto> product,
      Percent percentFull,
      LocStrFormatted text)
    {
      this.setProduct(product);
      this.Bar.UpdateValues(percentFull, text);
    }

    public void UpdateBar(Percent percentFull, LocStrFormatted text)
    {
      this.Bar.UpdateValues(percentFull, text);
    }

    public void UpdateBarOnly(Percent percentFull, LocStrFormatted text)
    {
      this.Bar.UpdateValues(percentFull, text);
    }

    public void UpdateSize() => this.Bar.UpdateSize();

    public void SetTextToShowWhenEmpty(string text)
    {
      this.m_textToShowWhenEmpty = (Option<string>) text;
    }

    private void setProduct(Option<ProductProto> product)
    {
      this.Product = product;
      if (product.IsNone)
      {
        if (this.m_textToShowWhenEmpty.HasValue)
        {
          this.m_textElementEmpty.SetText(this.m_textToShowWhenEmpty.Value);
          this.m_textElementEmpty.Show<Txt>();
          this.Bar.Hide<QuantityBar>();
        }
        this.Bar.SetEmpty();
      }
      else
      {
        this.Bar.Show<QuantityBar>();
        this.m_textElementEmpty.Hide<Txt>();
      }
      Btn valueOrNull = this.m_trashBtn.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull.SetVisibility<Btn>(product.HasValue);
      if (this.m_plusBtn.HasValue)
      {
        this.m_plusBtn.Value.SetVisibility<Btn>(product.IsNone);
        this.m_plusBtnWithProduct.Value.SetVisibility<Btn>(product.HasValue);
        if (!product.HasValue)
          return;
        this.m_plusBtnProductIcon.Value.SetIcon(product.Value.Graphics.IconPath);
        this.m_plusBtnProductName.Value.SetText((LocStrFormatted) product.Value.Strings.Name);
      }
      else
        this.m_productIcon.Value.SetProto(product);
    }

    public void SetCustomIcon(string iconPath) => this.m_productIcon.Value.SetCustomIcon(iconPath);

    public void SetCustomIconText(string text) => this.m_productIcon.Value.SetCustomIconText(text);

    public void ReplaceProductTextWith(IUiElement element)
    {
      this.SetCustomIconText(string.Empty);
      element.PutToBottomOf<IUiElement>((IUiElement) this.m_productIcon.Value, this.Builder.Style.ProductWithIcon.QuantityLineHeight);
    }

    private void updatePlusBtn(Quantity quantity)
    {
      this.m_plusBtnWithProduct.ValueOrNull?.SetEnabled(quantity.IsZero);
    }

    private void onClick()
    {
      if (!this.m_clickAction.HasValue)
        return;
      this.m_clickAction.Value();
    }

    public QuantityBar.Marker AddMarker(Percent position, ColorRgba color)
    {
      return this.Bar.AddMarker(position, color);
    }

    /// <summary>
    /// This is a very simple cache that counts with the fact that buffers can be reused for any product thus they
    /// are all equivalent. So this cache creates a new buffers if needed otherwise it provides the ones that were
    /// previously returned to it.
    /// </summary>
    public class Cache : ViewsCacheHomogeneous<BufferView>
    {
      public Cache(IUiElement parent, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Func<BufferView>) (() => builder.NewBufferView(parent)));
      }
    }
  }
}
