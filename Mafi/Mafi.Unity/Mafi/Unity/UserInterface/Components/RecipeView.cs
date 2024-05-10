// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.RecipeView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  internal class RecipeView : IUiElement
  {
    public readonly IRecipeForUi Recipe;
    private readonly UiBuilder m_builder;
    private readonly UiStyle m_style;
    private readonly StackContainer m_container;
    private readonly Btn m_button;
    private readonly SimpleProgressBar m_progressBar;
    private readonly Lyst<ProductQuantityWithIcon> m_products;
    private TextWithIcon m_duration;
    private string m_durationPrefix;
    private int m_pollutionSuffixWidth;
    private readonly bool m_dynamicVersion;
    private readonly Dict<ProductProto, RecipeView.ProductViewData> m_productsViews;
    private readonly bool m_ignoreNormalizationSettings;
    private readonly ColorRgba DefaultBorderColor;
    private readonly Option<Panel> m_tickBox;
    public Option<Btn> UpArrow;
    public Option<Btn> DownArrow;

    public event Action OnRecipeEnter;

    public event Action OnRecipeLeave;

    public GameObject GameObject => this.m_button.GameObject;

    public RectTransform RectTransform => this.m_button.RectTransform;

    public Option<IUiUpdater> Updater { get; private set; }

    public Option<IUiUpdater> BuffersUpdater { get; private set; }

    public Option<IUiUpdater> DurationUpdater { get; private set; }

    private bool NormalizeDuration
    {
      get => !this.m_ignoreNormalizationSettings && this.Normalizer.IsNormalizationOn;
    }

    private RecipeDurationNormalizer Normalizer => this.m_builder.DurationNormalizer;

    internal RecipeView(
      IUiElement parent,
      UiBuilder builder,
      Option<RecipesBookController> recipesBook,
      IRecipeForUi process,
      Action<IRecipeForUi> clickAction = null,
      bool dynamicVersion = false,
      bool ignoreNormalizationSettings = false,
      bool noBorders = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_products = new Lyst<ProductQuantityWithIcon>();
      this.m_durationPrefix = "";
      this.DefaultBorderColor = ColorRgba.Black;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      RecipeView recipeView = this;
      this.Recipe = process.CheckNotNull<IRecipeForUi>();
      this.m_builder = builder;
      this.m_style = builder.Style;
      this.m_dynamicVersion = dynamicVersion;
      this.m_ignoreNormalizationSettings = ignoreNormalizationSettings;
      this.m_button = builder.NewBtn("RecipeRow", parent).SetOnMouseEnterLeaveActions(new Action(this.onRecipeEnter), new Action(this.onRecipeLeave)).OnRightClick(new Action(rightClick)).SetButtonStyle(new BtnStyle()
      {
        BackgroundClr = new ColorRgba?(this.m_style.Panel.ItemOverlay),
        NormalMaskClr = new ColorRgba?((ColorRgba) 16777215),
        HoveredMaskClr = new ColorRgba?((ColorRgba) (clickAction != null ? 13158600 : 16777215)),
        PressedMaskClr = new ColorRgba?((ColorRgba) 16777215),
        Border = noBorders ? BorderStyle.DEFAULT : new BorderStyle(this.DefaultBorderColor, 2f)
      });
      this.m_productsViews = new Dict<ProductProto, RecipeView.ProductViewData>();
      if (dynamicVersion)
      {
        this.m_progressBar = new SimpleProgressBar((IUiElement) this.m_button, builder).SetBackgroundColor((ColorRgba) 2236962).PutTo<SimpleProgressBar>((IUiElement) this.m_button, Offset.All(2f));
        this.m_button.SetTargetGraphic(this.m_progressBar.RectTransform.GetComponent<Graphic>());
      }
      if (clickAction != null)
      {
        this.m_button.OnClick((Action) (() => clickAction(recipeView.Recipe)));
        this.m_button.SetBorderColor(ColorRgba.Gray);
        this.m_tickBox = (Option<Panel>) builder.NewPanel("TickBox", (IUiElement) this.m_button).SetBackground(this.m_builder.Style.Global.GreenForDark).PutToLeftTopOf<Panel>((IUiElement) this.m_button, 22.Vector2(), Offset.TopLeft(2f, 2f)).Hide<Panel>();
        builder.NewIconContainer("Icon", (IUiElement) this.m_tickBox.Value).SetIcon("Assets/Unity/UserInterface/General/Tick128.png", this.m_style.Panel.ItemOverlay).PutTo<IconContainer>((IUiElement) this.m_tickBox.Value);
      }
      this.m_container = builder.NewStackContainer("RecipeItems", (IUiElement) this.m_button).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutTo<StackContainer>((IUiElement) this.m_button, new Offset(this.m_style.Panel.Padding, this.m_style.Panel.PaddingCompact, this.m_style.Panel.Padding, 0.0f));
      this.buildUiForRecipe();

      void rightClick()
      {
        foreach (RecipeView.ProductViewData productViewData in recipeView.m_productsViews.Values)
        {
          Vector3[] fourCornersArray = new Vector3[4];
          productViewData.IconWithQuantity.GameObject.GetComponent<RectTransform>().GetWorldCorners(fourCornersArray);
          if (new Rect(fourCornersArray[0].x, fourCornersArray[0].y, fourCornersArray[2].x - fourCornersArray[0].x, fourCornersArray[2].y - fourCornersArray[0].y).Contains(Input.mousePosition))
            recipesBook.ValueOrNull?.OpenForProduct(productViewData.RecipeProduct.Product);
        }
      }
    }

    public void BuildMoveArrows(Action<IRecipeForUi, bool> onClick)
    {
      TextStyle? text = new TextStyle?(new TextStyle(ColorRgba.White));
      ColorRgba? nullable1 = new ColorRgba?(new ColorRgba(4371254));
      ColorRgba? nullable2 = new ColorRgba?(new ColorRgba(7075163));
      ColorRgba? nullable3 = new ColorRgba?(new ColorRgba(3050532));
      BorderStyle? border = new BorderStyle?();
      ColorRgba? backgroundClr = new ColorRgba?();
      ColorRgba? normalMaskClr = nullable1;
      ColorRgba? hoveredMaskClr = nullable2;
      ColorRgba? pressedMaskClr = nullable3;
      ColorRgba? disabledMaskClr = new ColorRgba?();
      ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
      ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
      Offset? iconPadding = new Offset?();
      BtnStyle buttonStyle = new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      if (this.UpArrow.IsNone)
        this.UpArrow = (Option<Btn>) this.m_builder.NewBtn("Up").SetButtonStyle(buttonStyle).SetIcon("Assets/Unity/UserInterface/General/MoveUp.svg").PutToLeftTopOf<Btn>((IUiElement) this.m_button, 16.Vector2(), Offset.Top(30f) + Offset.Left(6f)).OnClick((Action) (() => onClick(this.Recipe, true))).AddToolTip(Tr.IncreasePriority).Hide<Btn>();
      if (!this.DownArrow.IsNone)
        return;
      this.DownArrow = (Option<Btn>) this.m_builder.NewBtn("Down").SetButtonStyle(buttonStyle).SetIcon("Assets/Unity/UserInterface/General/MoveDown.svg").PutToLeftTopOf<Btn>((IUiElement) this.m_button, 16.Vector2(), Offset.Top(46f) + Offset.Left(6f)).OnClick((Action) (() => onClick(this.Recipe, false))).AddToolTip(Tr.DecreasePriority).Hide<Btn>();
    }

    public void SetRecipeActive(bool isActive)
    {
      this.m_button.SetBorderColor(isActive ? this.m_builder.Style.Global.GreenForDark : ColorRgba.Gray);
      Panel valueOrNull = this.m_tickBox.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.SetVisibility<Panel>(isActive);
    }

    public IUiUpdater BuildProgressUpdater(Func<IRecipeExecutorForUi> executorProvider)
    {
      Assert.That<bool>(this.m_dynamicVersion).IsTrue();
      if (this.Updater.HasValue)
        return this.Updater.Value;
      this.Updater = UpdaterBuilder.Start().Observe<bool>((Func<bool>) (() => executorProvider().WorkedThisTick)).Observe<Percent>((Func<Percent>) (() => executorProvider().ProgressOnRecipe(this.Recipe))).Do(new Action<bool, Percent>(this.UpdateProgress)).Build().SomeOption<IUiUpdater>();
      return this.Updater.Value;
    }

    public IUiUpdater BuildBufferViewUpdaters(
      Func<IRecipeExecutorForUi> executorProvider,
      Action<IRecipeForUi> cleanProductAction = null,
      IUpointsManager upointsManager = null)
    {
      Assert.That<bool>(this.m_dynamicVersion).IsTrue();
      if (this.BuffersUpdater.HasValue)
        return this.BuffersUpdater.Value;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      foreach (RecipeInput userVisibleInput in this.Recipe.AllUserVisibleInputs)
      {
        RecipeInput input = userVisibleInput;
        updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => executorProvider().GetInputQuantityFor(input.Product))).Observe<Quantity>((Func<Quantity>) (() => executorProvider().GetInputCapacityFor(input.Product))).Observe<Duration>((Func<Duration>) (() => executorProvider().GetTargetDurationFor(this.Recipe))).Observe<ExecutorDurationInfoUi>((Func<ExecutorDurationInfoUi>) (() => executorProvider().GetDurationInfo())).Observe<bool>((Func<bool>) (() => this.NormalizeDuration)).Do((Action<Quantity, Quantity, Duration, ExecutorDurationInfoUi, bool>) ((stored, capacity, duration, durationInfo, normalize) => this.m_productsViews[input.Product].UpdateQuantityInBuffer(this.Normalizer, stored, capacity, duration, durationInfo.DurationMultiplier, durationInfo.IsBoosted, true)));
      }
      foreach (RecipeOutput userVisibleOutput in this.Recipe.AllUserVisibleOutputs)
      {
        RecipeOutput output = userVisibleOutput;
        if (!output.HideInUi)
          updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => executorProvider().GetOutputQuantityFor(output.Product))).Observe<Quantity>((Func<Quantity>) (() => executorProvider().GetOutputCapacityFor(output.Product))).Observe<Duration>((Func<Duration>) (() => executorProvider().GetTargetDurationFor(this.Recipe))).Observe<ExecutorDurationInfoUi>((Func<ExecutorDurationInfoUi>) (() => executorProvider().GetDurationInfo())).Observe<bool>((Func<bool>) (() => this.NormalizeDuration)).Do((Action<Quantity, Quantity, Duration, ExecutorDurationInfoUi, bool>) ((stored, capacity, duration, durationInfo, normalize) => this.m_productsViews[output.Product].UpdateQuantityInBuffer(this.Normalizer, stored, capacity, duration, durationInfo.DurationMultiplier, durationInfo.IsBoosted, false)));
      }
      if (cleanProductAction != null && upointsManager != null)
      {
        Btn trashBtn = this.m_builder.NewBtn("Trash icon").SetButtonStyle(this.m_builder.Style.Global.IconBtnUpointsDanger).SetIcon("Assets/Unity/UserInterface/General/Trash128.png").OnClick((Action) (() => cleanProductAction(this.Recipe))).PutToLeftBottomOf<Btn>((IUiElement) this.m_button, 18.Vector2(), Offset.BottomLeft(10f, 5f)).Hide<Btn>();
        Tooltip trashTooltip = trashBtn.AddToolTipAndReturn();
        string discardCargoTooltip = MachineCommandsProcessor.COST_TO_DISCARD_PRODUCTS.FormatWithUnitySuffix().Value + " - " + Tr.RemoveProductsInBuffers__Tooltip.ToString();
        updaterBuilder.Observe<bool>((Func<bool>) (() => executorProvider().HasClearProductsActionFor(this.Recipe))).Observe<bool>((Func<bool>) (() => upointsManager.CanConsume(MachineCommandsProcessor.COST_TO_DISCARD_PRODUCTS))).Do((Action<bool, bool>) ((hasCleanAction, canConsumeUnity) =>
        {
          trashBtn.SetVisibility<Btn>(hasCleanAction);
          trashBtn.SetEnabled(canConsumeUnity);
          if (!hasCleanAction)
            return;
          trashTooltip.SetText(canConsumeUnity ? discardCargoTooltip : TrCore.TradeStatus__NoUnity.TranslatedString);
        }));
      }
      this.BuffersUpdater = Option.Some<IUiUpdater>(updaterBuilder.Build());
      return this.BuffersUpdater.Value;
    }

    public IUiUpdater BuildDurationUpdater(Func<IRecipeExecutorForUi> executorProvider)
    {
      Assert.That<bool>(this.m_dynamicVersion).IsTrue();
      if (this.DurationUpdater.HasValue)
        return this.DurationUpdater.Value;
      this.DurationUpdater = Option.Some<IUiUpdater>(UpdaterBuilder.Start().Observe<ExecutorDurationInfoUi>((Func<ExecutorDurationInfoUi>) (() => executorProvider().GetDurationInfo())).Observe<Duration>((Func<Duration>) (() => executorProvider().GetTargetDurationFor(this.Recipe))).Observe<bool>((Func<bool>) (() => this.NormalizeDuration)).Do((Action<ExecutorDurationInfoUi, Duration, bool>) ((durationInfo, duration, normalize) =>
      {
        duration = normalize ? 60.Seconds() : duration;
        this.m_duration?.SetPrefixText(this.m_durationPrefix + duration.Seconds.ToStringRounded(0));
        if (durationInfo.DurationMultiplier > Percent.Hundred && this.Normalizer.IsNormalizationOff)
          this.m_duration?.SetColor(this.m_builder.Style.Global.OrangeText);
        else if (durationInfo.IsBoosted && this.Normalizer.IsNormalizationOff)
          this.m_duration?.SetColor((ColorRgba) 12817663);
        else
          this.m_duration?.SetColor(this.m_builder.Style.Global.Title.Color);
      })).Build());
      return this.DurationUpdater.Value;
    }

    public void UpdateNormalization()
    {
      foreach (RecipeProduct userVisibleInput in this.Recipe.AllUserVisibleInputs)
        this.m_productsViews[userVisibleInput.Product].UpdateQuantityOnly(this.Normalizer, this.Recipe.Duration);
      foreach (RecipeProduct userVisibleOutput in this.Recipe.AllUserVisibleOutputs)
        this.m_productsViews[userVisibleOutput.Product].UpdateQuantityOnly(this.Normalizer, this.Recipe.Duration);
      if (!this.m_builder.DurationNormalizer.CanNormalizeRecipe(this.Recipe.Duration))
        return;
      this.m_duration?.SetPrefixText(this.m_durationPrefix + (this.NormalizeDuration ? 60.Seconds() : this.Recipe.Duration).Seconds.ToStringRounded(0));
    }

    public void SetLeftIndent(float left)
    {
      this.m_container.PutTo<StackContainer>((IUiElement) this.m_button, new Offset(this.m_style.Panel.Padding, this.m_style.Panel.PaddingCompact, this.m_style.Panel.Padding, 0.0f) + Offset.Left(left));
    }

    public void SetHighlight(bool enabled)
    {
      this.m_button.SetBorderColor(enabled ? (ColorRgba) 34816 : this.DefaultBorderColor);
    }

    private void onRecipeEnter()
    {
      Action onRecipeEnter = this.OnRecipeEnter;
      if (onRecipeEnter != null)
        onRecipeEnter();
      foreach (ProductQuantityWithIcon product in this.m_products)
        product.ShowProductName();
    }

    private void onRecipeLeave()
    {
      Action onRecipeLeave = this.OnRecipeLeave;
      if (onRecipeLeave != null)
        onRecipeLeave();
      foreach (ProductQuantityWithIcon product in this.m_products)
        product.HideProductName();
    }

    public float GetDynamicWidth()
    {
      return (float) ((double) this.m_container.GetDynamicWidth() + (double) this.m_pollutionSuffixWidth + 10.0);
    }

    public void UpdateProgress(bool workedThisTick, Percent progress)
    {
      if (this.Recipe.Duration <= 1.Seconds() && progress.IsPositive)
        progress = Percent.Hundred;
      this.m_progressBar.SetProgress(progress);
      this.m_progressBar.SetColor((workedThisTick ? this.m_style.Global.GreenForDark : this.m_style.Global.OrangeText).SetA((byte) 64));
    }

    private void buildUiForRecipe()
    {
      int length = this.Recipe.AllUserVisibleInputs.Length;
      ImmutableArray<RecipeOutput> userVisibleOutputs = this.Recipe.AllUserVisibleOutputs;
      int num = userVisibleOutputs.Count((Func<RecipeOutput, bool>) (x => !x.IsPollution));
      if (length == 0 && num >= 1)
      {
        userVisibleOutputs = this.Recipe.AllUserVisibleOutputs;
        this.buildOneProductOnlyRecipeUi(userVisibleOutputs.As<RecipeProduct>(), true);
        this.m_pollutionSuffixWidth = 85;
      }
      else if (length >= 1 && num == 0)
      {
        this.buildOneProductOnlyRecipeUi(this.Recipe.AllUserVisibleInputs.As<RecipeProduct>(), false);
        userVisibleOutputs = this.Recipe.AllUserVisibleOutputs;
        Option<RecipeOutput> option = (Option<RecipeOutput>) userVisibleOutputs.FirstOrDefault((Func<RecipeOutput, bool>) (x => x.IsPollution));
        if (option.HasValue)
          this.addProductIconFor((RecipeProduct) option.Value, this.Recipe.Duration);
        this.m_pollutionSuffixWidth = 85;
      }
      else
        this.buildRecipeUi();
    }

    private void buildOneProductOnlyRecipeUi(ImmutableArray<RecipeProduct> products, bool gives)
    {
      this.m_durationPrefix = "/ ";
      Txt element = this.m_builder.NewTxt("Quantity", (IUiElement) this.m_container).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.m_builder.Style.Global.Title).SetText(gives ? Tr.Provides.TranslatedString.ToUpper(LocalizationManager.CurrentCultureInfo) : Tr.Accepts.TranslatedString.ToUpper(LocalizationManager.CurrentCultureInfo));
      float preferedWidth = element.GetPreferedWidth();
      this.m_container.Append((IUiElement) element, new float?(preferedWidth), Offset.Left(15f));
      bool flag = true;
      foreach (RecipeProduct product in products)
      {
        if (!flag)
          this.appendPlusIcon();
        this.addProductIconFor(product, this.Recipe.Duration);
        flag = false;
      }
      Duration duration = this.NormalizeDuration ? 60.Seconds() : this.Recipe.Duration;
      this.m_duration = new TextWithIcon(this.m_builder, (IUiElement) this.m_container).SetTextStyle(this.m_builder.Style.Global.Title).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").SetPrefixText(this.m_durationPrefix + duration.Seconds.ToStringRounded(0));
      this.m_duration.AppendTo<TextWithIcon>(this.m_container, new float?(this.m_duration.GetWidth()));
    }

    private void buildRecipeUi()
    {
      bool flag1 = true;
      foreach (RecipeInput userVisibleInput in this.Recipe.AllUserVisibleInputs)
      {
        if (!flag1)
          this.appendPlusIcon();
        this.addProductIconFor((RecipeProduct) userVisibleInput, this.Recipe.Duration);
        flag1 = false;
      }
      this.appendTransformIcon();
      bool flag2 = true;
      foreach (RecipeOutput userVisibleOutput in this.Recipe.AllUserVisibleOutputs)
      {
        if (!flag2 && !userVisibleOutput.IsPollution)
          this.appendPlusIcon();
        this.addProductIconFor((RecipeProduct) userVisibleOutput, this.Recipe.Duration);
        if (!userVisibleOutput.IsPollution)
          flag2 = false;
      }
    }

    private void addProductIconFor(RecipeProduct recipeProduct, Duration duration)
    {
      ProductQuantityWithIcon quantityWithIcon;
      if (!recipeProduct.IsPollution)
      {
        quantityWithIcon = new ProductQuantityWithIcon((IUiElement) this.m_container, this.m_builder);
        quantityWithIcon.AppendTo<ProductQuantityWithIcon>(this.m_container, new float?(this.m_style.RecipeDetail.ItemWidth - 5f), Offset.Right(0.0f));
      }
      else
      {
        quantityWithIcon = new ProductQuantityWithIcon((IUiElement) this.m_button, this.m_builder);
        quantityWithIcon.PutToRightMiddleOf<ProductQuantityWithIcon>((IUiElement) this.m_button, new Vector2(60f, 65f), Offset.Right(5f));
        this.m_builder.NewPanel("Divider", (IUiElement) quantityWithIcon).SetBackground((ColorRgba) 7171437).PutToLeftOf<Panel>((IUiElement) quantityWithIcon, 2f, Offset.Left(-5f) + Offset.TopBottom(10f)).SendToBack<Panel>();
        this.m_pollutionSuffixWidth += 85;
      }
      quantityWithIcon.SetProduct(recipeProduct.Product.WithQuantity(recipeProduct.Quantity));
      if (this.NormalizeDuration)
        quantityWithIcon.SetQuantityText(this.Normalizer.NormalizeQuantityAsString(recipeProduct, duration));
      RecipeView.BufferStripe? nullable = new RecipeView.BufferStripe?();
      if (this.m_dynamicVersion)
        nullable = new RecipeView.BufferStripe?(new RecipeView.BufferStripe(this.m_builder, (IUiElement) quantityWithIcon));
      this.m_productsViews[recipeProduct.Product] = new RecipeView.ProductViewData()
      {
        IconWithQuantity = quantityWithIcon,
        Stripe = nullable,
        RecipeProduct = recipeProduct
      };
      quantityWithIcon.HideProductName();
      this.m_products.Add(quantityWithIcon);
    }

    private void appendTransformIcon()
    {
      Panel parent = this.m_builder.NewPanel("Transform icon container", (IUiElement) this.m_container).AppendTo<Panel>(this.m_container, new float?(this.m_style.RecipeDetail.DurationTextWidth));
      if (this.Recipe.Duration >= 1.Years())
        this.m_duration = new TextWithIcon(this.m_builder, (IUiElement) parent, 0).SetTextStyle(this.m_style.Panel.Text).SetPrefixText(TrCore.NumberOfYears.Format(this.Recipe.Duration.Years.ToString(), this.Recipe.Duration.Years.ToFix64()).Value);
      else if (this.Recipe.Duration > Duration.OneTick)
      {
        Duration duration = this.NormalizeDuration ? 60.Seconds() : this.Recipe.Duration;
        this.m_duration = new TextWithIcon(this.m_builder, (IUiElement) parent).SetTextStyle(this.m_style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").SetPrefixText(duration.Seconds.ToStringRounded(0));
      }
      if (this.m_duration != null)
        this.m_duration.PutToMiddleOf<TextWithIcon>((IUiElement) parent, this.m_style.RecipeDetail.DurationTextLineHeight, Offset.Bottom(24f));
      this.m_builder.NewIconContainer("->", (IUiElement) parent).SetIcon(new IconStyle(this.m_style.Icons.Transform, new ColorRgba?(this.m_style.Panel.PlainIconColor))).PutToCenterMiddleOf<IconContainer>((IUiElement) parent, this.m_style.RecipeDetail.TransformIconSize);
    }

    private void appendPlusIcon()
    {
      this.m_builder.NewIconContainer("+", (IUiElement) this.m_container).SetIcon(new IconStyle(this.m_style.Icons.Plus, new ColorRgba?(this.m_style.Panel.PlainIconColor))).AppendTo<IconContainer>(this.m_container, new Vector2?(this.m_style.RecipeDetail.PlusIconSize), ContainerPosition.MiddleOrCenter);
    }

    public class Cache : ViewsCache<IRecipeForUi, RecipeView>
    {
      private readonly IUiElement m_parent;
      private readonly Option<RecipesBookController> m_recipesBookController;
      private readonly Action<IRecipeForUi> m_onClickAction;
      private readonly bool m_isDynamic;

      public Cache(
        IUiElement parent,
        UiBuilder builder,
        Option<RecipesBookController> recipesBookController,
        Action<IRecipeForUi> onClickAction = null,
        bool isDynamic = false)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        this.m_parent = parent;
        this.m_recipesBookController = recipesBookController;
        this.m_onClickAction = onClickAction;
        this.m_isDynamic = isDynamic;
      }

      protected override RecipeView CreateView(UiBuilder builder, IRecipeForUi data)
      {
        return new RecipeView(this.m_parent, builder, this.m_recipesBookController, data, this.m_onClickAction, this.m_isDynamic);
      }
    }

    private class ProductViewData
    {
      public RecipeProduct RecipeProduct;
      public RecipeView.BufferStripe? Stripe;
      public ProductQuantityWithIcon IconWithQuantity;

      public void UpdateQuantityInBuffer(
        RecipeDurationNormalizer normalizer,
        Quantity stored,
        Quantity capacity,
        Duration recipeDuration,
        Percent durationMultiplier,
        bool isBoosted,
        bool isInput)
      {
        Assert.That<bool>(this.Stripe.HasValue).IsTrue();
        ref RecipeView.BufferStripe? local = ref this.Stripe;
        if (local.HasValue)
          local.GetValueOrDefault().Update(this.RecipeProduct.Quantity, stored, capacity, isInput);
        string str1 = "";
        if (stored.IsPositive && !this.RecipeProduct.IsPollution)
          str1 = string.Format(" <size=10>({0})</size>", (object) stored);
        string str2 = normalizer.NormalizeQuantityAsString(this.RecipeProduct, recipeDuration);
        this.IconWithQuantity.SetQuantityText((!(durationMultiplier > Percent.Hundred) || !normalizer.IsNormalizationOn ? (!isBoosted || !normalizer.IsNormalizationOn ? str2.ToString() : "<color=#C394FF>" + str2 + "</color>") : "<color=#FF9900>" + str2 + "</color>") + str1);
      }

      public void UpdateQuantityOnly(RecipeDurationNormalizer normalizer, Duration recipeDuration)
      {
        if (this.RecipeProduct.Quantity.IsZero)
          this.IconWithQuantity.SetQuantityText("?");
        else
          this.IconWithQuantity.SetQuantityText(normalizer.NormalizeQuantityAsString(this.RecipeProduct, recipeDuration));
      }

      public ProductViewData()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private struct BufferStripe
    {
      private readonly Panel m_bar;
      private readonly IconContainer m_warningIcon;
      private static readonly ColorRgba RED;
      private static readonly ColorRgba GRAY;
      private static readonly ColorRgba ORANGE;
      private static readonly ColorRgba GREEN;

      public BufferStripe(UiBuilder builder, IUiElement parent)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_bar = builder.NewPanel("bar", parent).PutToRightBottomOf<Panel>(parent, new Vector2(6f, 0.0f), Offset.Bottom(12f) + Offset.Right(5f)).SetHeight<Panel>(0.0f);
        this.m_warningIcon = builder.NewIconContainer("Warning", (IUiElement) this.m_bar).SetIcon(new IconStyle(builder.Style.Icons.Warning, new ColorRgba?(RecipeView.BufferStripe.RED))).PutToCenterTopOf<IconContainer>((IUiElement) this.m_bar, new Vector2(16f, 16f), Offset.Top(-18f)).Hide<IconContainer>();
      }

      public void Update(
        Quantity recipeQuantity,
        Quantity stored,
        Quantity capacity,
        bool isInput)
      {
        this.m_bar.SetHeight<Panel>((float) (capacity.IsPositive ? Percent.FromRatio(stored.Value, capacity.Value) : Percent.Zero).Clamp0To100().Apply(isInput ? 52 : 40));
        ColorRgba color;
        if (isInput)
        {
          color = stored >= recipeQuantity ? RecipeView.BufferStripe.GREEN : RecipeView.BufferStripe.GRAY;
        }
        else
        {
          bool visibility = capacity.IsPositive && stored + recipeQuantity > capacity;
          this.m_warningIcon.SetVisibility<IconContainer>(visibility);
          color = visibility ? RecipeView.BufferStripe.RED : RecipeView.BufferStripe.ORANGE;
        }
        this.m_bar.SetBackground(color);
      }

      static BufferStripe()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        RecipeView.BufferStripe.RED = (ColorRgba) 9454917;
        RecipeView.BufferStripe.GRAY = (ColorRgba) 9539985;
        RecipeView.BufferStripe.ORANGE = (ColorRgba) 12883266;
        RecipeView.BufferStripe.GREEN = (ColorRgba) 4558930;
      }
    }
  }
}
