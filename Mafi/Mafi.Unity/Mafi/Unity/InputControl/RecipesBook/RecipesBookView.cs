// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.RecipesBook.RecipesBookView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Syncers;
using Mafi.Core.UnlockingTree;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Unity.InputControl.Research;
using Mafi.Unity.InputControl.Research.SidePanel;
using Mafi.Unity.InputControl.Statistics;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.RecipesBook
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class RecipesBookView : WindowView
  {
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly StatisticsController m_statisticsController;
    private readonly LazyResolve<ResearchController> m_researchController;
    private readonly Dict<ProductProto, RecipesBookView.ProductTitleView> m_titles;
    private RecipesBookView.ProductRecipesView m_recipesView;
    private LazyResolve<RecipesBookController> m_recipesBookController;
    private StackContainer m_titlesContainer;
    private ScrollableContainer m_scrollableContainer;
    private ProductProto m_selectedProduct;
    private readonly Dict<ProductProto, Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>> m_usedAsInput;
    private readonly Dict<ProductProto, Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>> m_usedAsOutput;
    private readonly Lyst<Proto> m_protosToSearchIn;
    private readonly Set<Proto> m_protosFound;
    private int titlesWidth;
    private TxtField m_searchBox;
    private Txt m_nothingFoundInfo;
    private float m_maxWidth;

    internal RecipesBookView(
      ProtosDb protosDb,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      StatisticsController statisticsController,
      LazyResolve<RecipesBookController> recipesBookController,
      LazyResolve<ResearchController> researchController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_titles = new Dict<ProductProto, RecipesBookView.ProductTitleView>();
      this.m_usedAsInput = new Dict<ProductProto, Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>>();
      this.m_usedAsOutput = new Dict<ProductProto, Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>>();
      this.m_protosToSearchIn = new Lyst<Proto>();
      this.m_protosFound = new Set<Proto>();
      this.titlesWidth = 250;
      this.m_maxWidth = (float) RecipesBookView.ProductRecipesView.MIN_WIDTH;
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (RecipesBookView));
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_shortcutsManager = shortcutsManager;
      this.m_statisticsController = statisticsController;
      this.m_recipesBookController = recipesBookController;
      this.m_researchController = researchController;
      this.ShowAfterSync = true;
      foreach (ProductProto key in this.m_protosDb.All<ProductProto>())
      {
        this.m_usedAsInput.Add(key, new Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>());
        this.m_usedAsOutput.Add(key, new Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>());
      }
      ImmutableArray<ResearchNodeProto> nodes = protosDb.All<ResearchNodeProto>().ToImmutableArray<ResearchNodeProto>();
      foreach (LayoutEntityProto entity in (IEnumerable<LayoutEntityProto>) protosDb.All<LayoutEntityProto>().OrderBy<LayoutEntityProto, int>((Func<LayoutEntityProto, int>) (e => nodes.Where((Func<ResearchNodeProto, bool>) (x => x.Units.OfType<ProtoUnlock>().Any((Func<ProtoUnlock, bool>) (y => y.UnlockedProtos.Contains((IProto) e))))).Select<ResearchNodeProto, int>((Func<ResearchNodeProto, int>) (x => x.Difficulty)).FirstOrDefault<int>())))
      {
        if (entity is IProtoWithRecipes protoWithRecipes)
        {
          foreach (RecipeProto recipe in protoWithRecipes.Recipes)
          {
            if (recipe.IsAvailable)
              add(entity, (IRecipeForUi) recipe);
          }
        }
        if (entity is IProtoWithUiRecipes protoWithUiRecipes)
        {
          foreach (IRecipeForUi recipe in protoWithUiRecipes.Recipes)
            add(entity, recipe);
        }
        if (entity is IProtoWithUiRecipe protoWithUiRecipe)
          add(entity, protoWithUiRecipe.Recipe);
      }

      void add(LayoutEntityProto entity, IRecipeForUi recipe)
      {
        foreach (RecipeProduct userVisibleOutput in recipe.AllUserVisibleOutputs)
          this.m_usedAsOutput[userVisibleOutput.Product].Add<LayoutEntityProto, IRecipeForUi>(entity, recipe);
        foreach (RecipeProduct userVisibleInput in recipe.AllUserVisibleInputs)
          this.m_usedAsInput[userVisibleInput.Product].Add<LayoutEntityProto, IRecipeForUi>(entity, recipe);
      }
    }

    private void setRecipesWindowWidth(float width)
    {
      this.m_maxWidth = this.m_maxWidth.Max(width);
      this.SetWidth<RecipesBookView>((float) this.titlesWidth + this.m_maxWidth);
    }

    protected override void BuildWindowContent()
    {
      string shortcut = this.m_shortcutsManager.ResolveShortcutToString("RecipeBookView", (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleRecipeBook));
      this.SetTitle((LocStrFormatted) Tr.RecipesBook__Title, shortcut);
      this.SetContentSize(new Vector2((float) (this.titlesWidth + RecipesBookView.ProductRecipesView.MIN_WIDTH), this.ResolveWindowSize().y));
      this.PositionSelfToCenter();
      this.MakeMovable();
      this.m_searchBox = this.Builder.NewTxtField("Search", (IUiElement) this.GetContentPanel()).SetStyle(this.Builder.Style.Global.LightTxtFieldStyle).SetPlaceholderText(Tr.Search).SetCharLimit(30).PutToLeftTopOf<TxtField>((IUiElement) this.GetContentPanel(), new Vector2((float) (this.titlesWidth - 20), 30f), Offset.Top(10f) + Offset.Left(10f));
      this.m_searchBox.SetDelayedOnEditEndListener(new Action<string>(this.search));
      ScrollableContainer leftOf = this.Builder.NewScrollableContainer("ScrollableTitles", (IUiElement) this.GetContentPanel()).AddVerticalScrollbar().PutToLeftOf<ScrollableContainer>((IUiElement) this.GetContentPanel(), (float) (this.titlesWidth - 20), Offset.LeftRight(10f) + Offset.Top(50f));
      this.m_titlesContainer = this.Builder.NewStackContainer("TitlesContainer", (IUiElement) this.GetContentPanel()).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(2f).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      leftOf.AddItemTop((IUiElement) this.m_titlesContainer);
      Txt txt = this.Builder.NewTxt("NothingFound", (IUiElement) leftOf).SetAlignment(TextAnchor.MiddleCenter);
      TextStyle text = this.Builder.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable1 = new int?(16);
      FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = nullable2;
      int? fontSize = nullable1;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_nothingFoundInfo = txt.SetTextStyle(textStyle).PutToTopOf<Txt>((IUiElement) leftOf, (float) (this.titlesWidth - 20), Offset.Top(10f)).Hide<Txt>();
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("ScrollableContainer", (IUiElement) this.GetContentPanel()).AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel(), Offset.Left((float) this.titlesWidth));
      this.Builder.NewPanel("ContainerBg", (IUiElement) this.m_scrollableContainer).SetBackground((ColorRgba) 3815994).PutTo<Panel>((IUiElement) this.m_scrollableContainer).SendToBack<Panel>();
      this.m_recipesView = new RecipesBookView.ProductRecipesView((IUiElement) this.m_scrollableContainer, this.Builder, this, this.m_statisticsController);
      this.m_scrollableContainer.AddItemTop((IUiElement) this.m_recipesView);
      this.m_scrollableContainer.SetContentToScroll((IUiElement) this.m_recipesView);
      this.m_recipesView.Show<RecipesBookView.ProductRecipesView>();
      this.AddUpdater(this.m_recipesView.Updater);
      this.m_titlesContainer.StartBatchOperation();
      foreach (ProductProto productProto in this.m_protosDb.All<ProductProto>().OrderBy<ProductProto, string>((Func<ProductProto, string>) (x => x.Strings.Name.TranslatedString)).ToArray<ProductProto>())
      {
        if (!this.m_usedAsInput[productProto].IsEmpty || !this.m_usedAsOutput[productProto].IsEmpty)
        {
          RecipesBookView.ProductTitleView element = new RecipesBookView.ProductTitleView((IUiElement) this.m_titlesContainer, this.Builder, productProto, new Action<ProductProto>(this.SetProduct));
          this.m_titlesContainer.Append((IUiElement) element, new float?(30f));
          this.m_titles.Add(productProto, element);
          bool isVisible = this.m_unlockedProtosDb.IsUnlocked((IProto) element.Product);
          this.m_titlesContainer.SetItemVisibility((IUiElement) element, isVisible);
          if ((Proto) this.m_selectedProduct == (Proto) null & isVisible)
            this.m_selectedProduct = productProto;
        }
      }
      this.m_titlesContainer.FinishBatchOperation();
      this.SetProduct(this.m_selectedProduct);
      this.OnHide += (Action) (() => this.Builder.GameOverlay.Hide<Panel>());
      this.OnShowStart += new Action(this.ClearSearch);
    }

    public void ClearSearch()
    {
      this.m_searchBox.ClearInput();
      this.search("");
    }

    private void search(string text)
    {
      this.m_titlesContainer.StartBatchOperation();
      foreach (RecipesBookView.ProductTitleView productTitleView in this.m_titles.Values)
      {
        if (this.m_unlockedProtosDb.IsUnlocked((IProto) productTitleView.Product))
          this.m_protosToSearchIn.Add((Proto) productTitleView.Product);
      }
      UiSearchUtils.MatchProtos<Proto>(text, (IIndexable<Proto>) this.m_protosToSearchIn, this.m_protosFound);
      this.m_protosToSearchIn.Clear();
      foreach (RecipesBookView.ProductTitleView productTitleView in this.m_titles.Values)
        this.m_titlesContainer.SetItemVisibility((IUiElement) productTitleView, this.m_protosFound.Contains((Proto) productTitleView.Product));
      this.m_titlesContainer.FinishBatchOperation();
      if (this.m_protosFound.Count == 0)
        this.m_nothingFoundInfo.SetText(Tr.NothingFoundFor.Format(text)).Show<Txt>();
      else
        this.m_nothingFoundInfo.Hide<Txt>();
    }

    private bool isLocked(LayoutEntityProto entity, IRecipeForUi recipe)
    {
      if (this.m_unlockedProtosDb.IsLocked((Proto) entity))
        return true;
      return recipe is RecipeProto recipeProto && this.m_unlockedProtosDb.IsLocked((Proto) recipeProto);
    }

    public void SetProduct(ProductProto product)
    {
      if ((Proto) this.m_selectedProduct == (Proto) product || !this.m_titles.ContainsKey(product))
        return;
      this.m_titles[this.m_selectedProduct].Deselect();
      this.m_titles[product].Select();
      this.m_selectedProduct = product;
    }

    private class ProductTitleView : IUiElement
    {
      private readonly UiBuilder m_builder;
      public readonly ProductProto Product;
      private readonly Btn m_btn;

      public GameObject GameObject => this.m_btn.GameObject;

      public RectTransform RectTransform => this.m_btn.RectTransform;

      public ProductTitleView(
        IUiElement parent,
        UiBuilder builder,
        ProductProto product,
        Action<ProductProto> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_builder = builder;
        this.Product = product;
        Btn btn = builder.NewBtn("Title", parent).SetText((LocStrFormatted) product.Strings.Name);
        string iconPath = product.Graphics.IconPath;
        Vector2? nullable = new Vector2?(16.Vector2());
        ColorRgba? color = new ColorRgba?();
        Vector2? size = nullable;
        IconStyle iconStyle = new IconStyle(iconPath, color, size);
        this.m_btn = btn.SetIcon(iconStyle).SetButtonStyle(builder.Style.Global.ListMenuBtn).SetTextAlignment(TextAnchor.MiddleLeft).OnClick((Action) (() => onClick(product)));
      }

      public void Select()
      {
        this.m_btn.SetButtonStyle(this.m_builder.Style.Global.ListMenuBtnSelected);
        this.m_btn.SetEnabled(false);
      }

      public void Deselect()
      {
        this.m_btn.SetButtonStyle(this.m_builder.Style.Global.ListMenuBtn);
        this.m_btn.SetEnabled(true);
      }
    }

    private class ProductRecipesView : IUiElement
    {
      public readonly IUiUpdater Updater;
      public static int MIN_WIDTH;
      private readonly StackContainer m_content;
      private readonly RecipesView m_outputRecipes;
      private readonly RecipesView m_inputRecipes;

      public GameObject GameObject => this.m_content.GameObject;

      public RectTransform RectTransform => this.m_content.RectTransform;

      public ProductRecipesView(
        IUiElement parent,
        UiBuilder builder,
        RecipesBookView mainView,
        StatisticsController statsController)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        RecipesBookView.ProductRecipesView productRecipesView = this;
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = builder.Style;
        this.m_content = builder.NewStackContainer("Content", parent).SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.TopToBottom).SetInnerPadding(Offset.LeftRight(20f) + Offset.Bottom(100f));
        Txt txt = builder.NewTxt("Text", (IUiElement) this.m_content).SetAlignment(TextAnchor.MiddleCenter);
        TextStyle text = builder.Style.Global.Text;
        ref TextStyle local = ref text;
        int? nullable1 = new int?(16);
        FontStyle? nullable2 = new FontStyle?(FontStyle.Bold);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = nullable2;
        int? fontSize = nullable1;
        bool? isCapitalized = new bool?();
        TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
        Txt title = txt.SetTextStyle(textStyle).AppendTo<Txt>(this.m_content, new float?(40f));
        builder.NewTxt("Hint", (IUiElement) this.m_content).SetText((LocStrFormatted) Tr.RecipesBook__OpenHint).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(builder.Style.Global.TextMedium).AppendTo<Txt>(this.m_content, new float?(30f));
        Btn openStatsBtn = builder.NewBtnGeneral("OpenStats", (IUiElement) this.m_content).SetText((LocStrFormatted) Tr.Statistics).SetIcon("Assets/Unity/UserInterface/Toolbar/Stats.svg", 20.Vector2()).AddToolTip(Tr.OpenStats).OnClick((Action) (() => statsController.OpenAndShowProductStats(mainView.m_selectedProduct)));
        openStatsBtn.AppendTo<Btn>(this.m_content, new Vector2?(openStatsBtn.GetOptimalSize() + new Vector2(0.0f, 5f)), ContainerPosition.LeftOrTop, Offset.Bottom(5f));
        Panel radiationContainer = builder.NewPanel("RadioactivityContainer", (IUiElement) this.m_content).SetBackground(style.Panel.ItemOverlay).AppendTo<Panel>(this.m_content, new float?(55f), Offset.Bottom(5f)).AddToolTip(Tr.RadiationLevel__Tooltip.TranslatedString);
        builder.NewIconContainer("RadiationIcon", (IUiElement) radiationContainer).SetIcon("Assets/Unity/UserInterface/General/Radiation.svg", style.Global.OrangeText).PutToLeftOf<IconContainer>((IUiElement) radiationContainer, 35f, Offset.Left(5f));
        Txt radiationLevel = builder.NewTxt("Radiation").SetTextStyle(style.Global.TextBigBold).SetColor(style.Global.OrangeText).SetAlignment(TextAnchor.MiddleLeft).PutToLeftOf<Txt>((IUiElement) radiationContainer, 20f, Offset.Left(50f));
        addTitle((LocStrFormatted) Tr.Production, Offset.Zero);
        Panel miningContainer = builder.NewPanel("MiningContainer", (IUiElement) this.m_content).SetBackground(style.Panel.ItemOverlay).AppendTo<Panel>(this.m_content, new float?(75f), Offset.Bottom(5f));
        IconContainer leftOf1 = builder.NewIconContainer("ExcavatorIcon", (IUiElement) miningContainer).SetIcon("Assets/Unity/UserInterface/Toolbar/Mining.svg").PutToLeftOf<IconContainer>((IUiElement) miningContainer, 45f, Offset.Left(5f));
        builder.NewIconContainer("Transform", (IUiElement) miningContainer).SetIcon(new IconStyle(style.Icons.Transform, new ColorRgba?(style.Panel.PlainIconColor))).PutToLeftMiddleOf<IconContainer>((IUiElement) miningContainer, style.RecipeDetail.TransformIconSize, Offset.Left(leftOf1.GetWidth() + 20f));
        IconContainer minedProductIcon = builder.NewIconContainer("MinedProductIcon", (IUiElement) miningContainer).PutToLeftOf<IconContainer>((IUiElement) miningContainer, 45f, Offset.Bottom(5f) + Offset.Left(leftOf1.GetWidth() + 30f + style.RecipeDetail.TransformIconSize.x));
        this.m_outputRecipes = new RecipesView(builder, mainView.m_recipesBookController.Value, this.m_content, new Action<float>(onSizeChanged), new Action<IRecipeForUi>(onRecipeClick), new Action<LayoutEntityProto>(onMachineClick));
        addTitle((LocStrFormatted) Tr.Consumption, Offset.Top(25f));
        Panel dumpingContainer = builder.NewPanel("DumpingContainer", (IUiElement) this.m_content).SetBackground(style.Panel.ItemOverlay).AppendTo<Panel>(this.m_content, new float?(75f), Offset.Bottom(5f));
        IconContainer dumpedProductIcon = builder.NewIconContainer("DumpedProductIcon", (IUiElement) dumpingContainer).PutToLeftOf<IconContainer>((IUiElement) dumpingContainer, 45f, Offset.Left(5f));
        builder.NewIconContainer("Transform", (IUiElement) dumpingContainer).SetIcon(new IconStyle(style.Icons.Transform, new ColorRgba?(style.Panel.PlainIconColor))).PutToLeftMiddleOf<IconContainer>((IUiElement) dumpingContainer, style.RecipeDetail.TransformIconSize, Offset.Left(dumpedProductIcon.GetWidth() + 20f));
        IconContainer leftOf2 = builder.NewIconContainer("DumpingIcon", (IUiElement) dumpingContainer).SetIcon("Assets/Unity/UserInterface/Toolbar/Dumping.svg").PutToLeftOf<IconContainer>((IUiElement) dumpingContainer, 45f, Offset.Bottom(5f) + Offset.Left(dumpedProductIcon.GetWidth() + 30f + style.RecipeDetail.TransformIconSize.x));
        Txt onlyInMineTowerDumping = builder.NewTxt("OnlyInMineTower", (IUiElement) dumpingContainer).SetTextStyle(style.Global.TextMedium).SetText((LocStrFormatted) Tr.DumpInMineTowerOnly).SetAlignment(TextAnchor.MiddleLeft).PutToLeftMiddleOf<Txt>((IUiElement) dumpingContainer, new Vector2(350f, 40f), Offset.Left((float) ((double) dumpedProductIcon.GetWidth() + 30.0 + (double) style.RecipeDetail.TransformIconSize.x + (double) leftOf2.GetWidth() + 20.0)));
        this.m_inputRecipes = new RecipesView(builder, mainView.m_recipesBookController.Value, this.m_content, new Action<float>(onSizeChanged), new Action<IRecipeForUi>(onRecipeClick), new Action<LayoutEntityProto>(onMachineClick));
        updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => mainView.m_selectedProduct)).Do((Action<ProductProto>) (product =>
        {
          openStatsBtn.SetEnabled(!product.IsExcludedFromStats);
          if (product.DumpableProduct.HasValue)
          {
            minedProductIcon.SetIcon(product.Graphics.IconPath);
            dumpedProductIcon.SetIcon(product.Graphics.IconPath);
            onlyInMineTowerDumping.SetVisibility<Txt>(!product.DumpableProduct.Value.IsDumpedOnTerrainByDefault);
          }
          closure_0.m_content.SetItemVisibility((IUiElement) miningContainer, product.DumpableProduct.HasValue);
          closure_0.m_content.SetItemVisibility((IUiElement) dumpingContainer, product.DumpableProduct.HasValue);
          closure_0.m_content.SetItemVisibility((IUiElement) radiationContainer, product.Radioactivity > 0);
          radiationLevel.SetText(product.Radioactivity.ToStringCached());
          title.SetText((LocStrFormatted) product.Strings.Name);
        }));
        this.Updater = updaterBuilder.Build();
        this.Updater.AddChildUpdater(this.m_outputRecipes.CreateUpdaterForGeneralRecipes((Func<ProductProto>) (() => mainView.m_selectedProduct), (Func<ProductProto, Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>>) (p => mainView.m_usedAsOutput[p]), new Func<LayoutEntityProto, IRecipeForUi, bool>(mainView.isLocked)));
        this.Updater.AddChildUpdater(this.m_inputRecipes.CreateUpdaterForGeneralRecipes((Func<ProductProto>) (() => mainView.m_selectedProduct), (Func<ProductProto, Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>>) (p => mainView.m_usedAsInput[p]), new Func<LayoutEntityProto, IRecipeForUi, bool>(mainView.isLocked)));
        this.Hide<RecipesBookView.ProductRecipesView>();
        this.SetSize<RecipesBookView.ProductRecipesView>(new Vector2(700f, this.m_content.GetDynamicHeight() + 100f));

        void addTitle(LocStrFormatted text, Offset offset)
        {
          builder.NewTxt("Title", (IUiElement) productRecipesView.m_content).SetText(text).SetTextStyle(builder.Style.Global.TitleBig).SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(productRecipesView.m_content, new float?(30f), offset, true);
        }

        void onSizeChanged(float size)
        {
          mainView.setRecipesWindowWidth(productRecipesView.m_outputRecipes.MaxRecipeWidth.Max(productRecipesView.m_inputRecipes.MaxRecipeWidth) + 40f);
        }

        void onRecipeClick(IRecipeForUi recipe)
        {
          mainView.m_researchController.Value.OpenResearchForRecipe(recipe);
        }

        void onMachineClick(LayoutEntityProto machine)
        {
          mainView.m_recipesBookController.Value.StartProtoBuild(machine);
        }
      }

      static ProductRecipesView()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        RecipesBookView.ProductRecipesView.MIN_WIDTH = 700;
      }
    }
  }
}
