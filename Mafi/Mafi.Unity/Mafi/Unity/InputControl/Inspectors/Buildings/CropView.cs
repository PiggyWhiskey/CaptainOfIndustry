// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CropView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class CropView : IUiElement
  {
    private readonly ProductProto m_waterProto;
    private readonly Lyst<ProductQuantityWithIcon> m_products;
    private readonly UiStyle m_style;
    private readonly StackContainer m_container;
    private readonly Btn m_button;
    private readonly Txt m_duration;
    private readonly Panel m_transformIcon;
    private readonly ViewsCacheHomogeneous<ProductQuantityWithIcon> m_productIconsCache;
    private readonly ViewsCacheHomogeneous<IconContainer> m_plusIconsCache;
    private readonly Tooltip m_buttonTooltip;
    private readonly IProperty<Percent> m_yieldMultiplier;
    private readonly IProperty<Percent> m_waterConsumptionMultiplier;

    public GameObject GameObject => this.m_button.GameObject;

    public RectTransform RectTransform => this.m_button.RectTransform;

    public CropProto Crop { get; private set; }

    internal CropView(
      UiBuilder builder,
      ProtosDb protosDb,
      IPropertiesDb propsDb,
      Action<CropView> clickAction = null,
      bool alignLeft = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_products = new Lyst<ProductQuantityWithIcon>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      CropView cropView = this;
      this.m_waterProto = protosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.CleanWater);
      this.m_style = builder.Style;
      this.m_yieldMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.FarmYieldMultiplier);
      this.m_waterConsumptionMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.FarmWaterConsumptionMultiplier);
      this.m_productIconsCache = new ViewsCacheHomogeneous<ProductQuantityWithIcon>((Func<ProductQuantityWithIcon>) (() => new ProductQuantityWithIcon((IUiElement) cropView.m_container, builder)));
      this.m_plusIconsCache = new ViewsCacheHomogeneous<IconContainer>((Func<IconContainer>) (() => builder.NewIconContainer("Plus")));
      this.m_button = builder.NewBtn("RecipeRow").SetOnMouseEnterLeaveActions(new Action(this.onRecipeEnter), new Action(this.onRecipeLeave)).SetButtonStyle(new BtnStyle(backgroundClr: new ColorRgba?(this.m_style.Panel.ItemOverlay), normalMaskClr: new ColorRgba?((ColorRgba) (clickAction != null ? 14540253 : 16777215)), hoveredMaskClr: new ColorRgba?((ColorRgba) (clickAction != null ? 15658734 : 16777215)), pressedMaskClr: new ColorRgba?((ColorRgba) 16777215)));
      this.m_buttonTooltip = builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this.m_button).SetErrorTextStyle();
      if (clickAction != null)
        this.m_button.OnClick((Action) (() => clickAction(cropView)));
      this.m_container = builder.NewStackContainer("RecipeItems").SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Top(5f)).SetItemSpacing(2f);
      if (alignLeft)
        this.m_container.PutToLeftOf<StackContainer>((IUiElement) this.m_button, 0.0f, Offset.Left(10f));
      else
        this.m_container.PutToRightOf<StackContainer>((IUiElement) this.m_button, 0.0f);
      this.m_transformIcon = builder.NewPanel("Transform icon container");
      this.m_duration = builder.NewTxt("Duration").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.m_style.Panel.Text).PutToMiddleOf<Txt>((IUiElement) this.m_transformIcon, this.m_style.RecipeDetail.DurationTextLineHeight, Offset.Bottom(24f));
      builder.NewIconContainer("->").SetIcon(new IconStyle(this.m_style.Icons.Transform, new ColorRgba?(this.m_style.Panel.PlainIconColor))).PutToCenterMiddleOf<IconContainer>((IUiElement) this.m_transformIcon, this.m_style.RecipeDetail.TransformIconSize);
      this.SetHeight<CropView>(this.m_style.RecipeDetail.Height);
    }

    private void onRecipeEnter()
    {
      foreach (ProductQuantityWithIcon product in this.m_products)
        product.ShowProductName();
    }

    private void onRecipeLeave()
    {
      foreach (ProductQuantityWithIcon product in this.m_products)
        product.HideProductName();
    }

    public CropView SetCrop(
      CropProto crop,
      FarmProto farmProto,
      Percent? realFertilityConsumedPerMonth = null)
    {
      this.Crop = crop;
      Percent scale = this.m_waterConsumptionMultiplier.Value;
      Percent percent1 = this.m_yieldMultiplier.Value;
      this.m_container.ClearAll();
      this.m_container.StartBatchOperation();
      this.m_productIconsCache.ReturnAll();
      this.m_plusIconsCache.ReturnAll();
      Percent percent2 = crop.GetConsumedFertilityPerDay(farmProto) * 30;
      PartialQuantity partialQuantity = (crop.GetConsumedWaterPerDay(farmProto) * 30).ScaledBy(scale);
      if (partialQuantity.IsPositive)
      {
        ProductQuantityWithIcon quantityWithIcon = this.addProductIconFor();
        string stringRounded = partialQuantity.Value.ToStringRounded(0);
        quantityWithIcon.SetProduct(this.m_waterProto.WithQuantity(0.Quantity()));
        quantityWithIcon.SetQuantityPerDuration(stringRounded, 60);
      }
      if (crop.ProductProduced.IsEmpty)
      {
        if (partialQuantity.IsPositive)
          this.appendPlusIcon();
        ProductQuantityWithIcon quantityWithIcon = this.addProductIconFor();
        quantityWithIcon.SetRawData(crop.Graphics.IconPath, (LocStrFormatted) crop.Strings.Name, "");
        if (percent2.IsNotNegative)
          quantityWithIcon.SetQuantityText(Tr.Empty.TranslatedString);
        else
          quantityWithIcon.SetQuantityText(string.Empty);
      }
      if (percent2.IsPositive)
      {
        this.appendPlusIcon();
        ProductQuantityWithIcon quantityWithIcon = this.addProductIconFor();
        bool flag = false;
        if (realFertilityConsumedPerMonth.HasValue)
        {
          Percent? nullable = realFertilityConsumedPerMonth;
          Percent percent3 = percent2;
          flag = nullable.HasValue && nullable.GetValueOrDefault() > percent3;
          percent2 = realFertilityConsumedPerMonth.Value;
        }
        quantityWithIcon.SetRawData("Assets/Unity/UserInterface/General/Fertility128.png", (LocStrFormatted) Tr.FarmFertility, "");
        quantityWithIcon.SetQuantityPerDuration(percent2.ToStringRounded(1), 60).SetColor(flag ? this.m_style.Global.OrangeText : this.m_style.Global.Text.Color);
      }
      this.m_transformIcon.AppendTo<Panel>(this.m_container, new float?(this.m_style.RecipeDetail.DurationTextWidth + 20f));
      bool flag1 = false;
      if (percent2.IsNegative)
      {
        ProductQuantityWithIcon quantityWithIcon = this.addProductIconFor();
        quantityWithIcon.SetRawData("Assets/Unity/UserInterface/General/Fertility128.png", (LocStrFormatted) Tr.FarmFertility, "");
        quantityWithIcon.SetQuantityPerDuration((-percent2).ToStringRounded(1), 60);
        flag1 = true;
      }
      if (crop.ProductProduced.IsNotEmpty)
      {
        if (flag1)
          this.appendPlusIcon();
        this.addProductIconFor().SetProduct(crop.GetProductProduced(farmProto).ScaledBy(percent1));
      }
      if (crop.ProductProduced.IsEmpty && percent2.IsNotNegative)
        this.addProductIconFor().SetRawData("Assets/Unity/UserInterface/General/Fertility128.png", (LocStrFormatted) Tr.FarmFertility, Tr.FarmFertility__NaturalReplenish.TranslatedString);
      this.m_container.FinishBatchOperation();
      Fix32 fix32 = crop.DaysToGrow.Over(30);
      this.m_duration.SetText(TrCore.NumberOfMonths.Format(fix32.ToStringRounded(0), fix32.ToFix64()));
      bool flag2 = crop.RequiresGreenhouse && !farmProto.IsGreenhouse;
      this.m_button.SetEnabled(!flag2);
      this.m_buttonTooltip.SetText(flag2 ? (LocStrFormatted) Tr.CropRequiresGreenhouse : LocStrFormatted.Empty);
      return this;
    }

    private ProductQuantityWithIcon addProductIconFor()
    {
      ProductQuantityWithIcon quantityWithIcon = this.m_productIconsCache.GetView().AppendTo<ProductQuantityWithIcon>(this.m_container, new float?(this.m_style.RecipeDetail.ItemWidth - 5f), Offset.Right(5f));
      quantityWithIcon.HideProductName();
      this.m_products.Add(quantityWithIcon);
      return quantityWithIcon;
    }

    private void appendPlusIcon()
    {
      this.m_plusIconsCache.GetView().SetIcon(new IconStyle(this.m_style.Icons.Plus, new ColorRgba?(this.m_style.Panel.PlainIconColor))).AppendTo<IconContainer>(this.m_container, new Vector2?(this.m_style.RecipeDetail.PlusIconSize), ContainerPosition.MiddleOrCenter);
    }
  }
}
