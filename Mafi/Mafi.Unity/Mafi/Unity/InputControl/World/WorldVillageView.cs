// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldVillageView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Contracts;
using Mafi.Core.World.Entities;
using Mafi.Core.World.QuickTrade;
using Mafi.Localization;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class WorldVillageView : ItemDetailWindowView
  {
    private static float WIDTH;
    private readonly InspectorContext m_inspectorContext;
    private readonly ContractsManager m_contractsManager;
    private readonly SourceProductsAnalyzer m_sourceProductsAnalyzer;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly TradeWindowController m_tradeWindowController;
    private readonly IAvailableProductsProvider m_availableProductsProvider;
    private readonly Action<WorldMapLocation, LocationVisitReason> m_onGoToClick;

    private WorldMapVillage Entity { get; set; }

    private int ShipSizeForContracts { get; set; }

    public WorldVillageView(
      InspectorContext inspectorContext,
      ContractsManager contractsManager,
      SourceProductsAnalyzer sourceProductsAnalyzer,
      TravelingFleetManager fleetManager,
      TradeWindowController tradeWindowController,
      Action onClose,
      Action<WorldMapLocation, LocationVisitReason> onGoToClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("WorldMapVillageInspector", false);
      this.m_inspectorContext = inspectorContext;
      this.m_contractsManager = contractsManager;
      this.m_sourceProductsAnalyzer = sourceProductsAnalyzer;
      this.m_fleetManager = fleetManager;
      this.m_tradeWindowController = tradeWindowController;
      this.m_availableProductsProvider = (IAvailableProductsProvider) new ProductsAvailableInStorage(inspectorContext.AssetsManager);
      this.m_onGoToClick = onGoToClick;
      this.SetOnCloseButtonClickAction(onClose);
      this.EnableClippingPrevention();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.SetOnHideSound((Option<AudioSource>) Option.None);
      this.SetOnShowSound((Option<AudioSource>) Option.None);
      this.MakeMovable();
      this.AddHelpButton().SetText((LocStrFormatted) Tr.WorldSettlement_NeutralDesc);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<WorldMapVillageProto>((Func<WorldMapVillageProto>) (() => this.Entity.Prototype)).Observe<int>((Func<int>) (() => this.Entity.Reputation)).Observe<bool>((Func<bool>) (() => this.Entity.IsPaused)).Do((Action<WorldMapVillageProto, int, bool>) ((proto, level, isPaused) => this.SetTitle(proto.GetTitleFor(level), isPaused)));
      Txt upgradeTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.ReputationIncrease__Title, new LocStrFormatted?((LocStrFormatted) Tr.ReputationIncrease__Tooltip));
      EntityUpgradeView upgradeView = new EntityUpgradeView(new Action(upgradeAction), this.m_inspectorContext.AssetsManager);
      upgradeView.Build((IUiElement) this.ItemsContainer, this.Builder).AppendTo<EntityUpgradeView>(this.ItemsContainer, new float?(upgradeView.Height));
      upgradeView.SetActionTitle((LocStrFormatted) Tr.ReputationIncrease__DonateAction);
      this.AddUpdater(upgradeView.CreateUpdater((Func<IUpgradableWorldEntity>) (() => (IUpgradableWorldEntity) this.Entity), (Action<bool>) (isVisible =>
      {
        this.ItemsContainer.StartBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) upgradeView, isVisible);
        this.ItemsContainer.SetItemVisibility((IUiElement) upgradeTitle, isVisible);
        this.ItemsContainer.FinishBatchOperation();
      })));
      Txt upgradeProgressTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.ReputationIncrease__Title, new LocStrFormatted?((LocStrFormatted) Tr.ReputationIncrease__Tooltip));
      ConstructionProgressView upgradeProgressView = new ConstructionProgressView((IUiElement) this.ItemsContainer, this.Builder, (Func<Option<IConstructionProgress>>) (() => this.Entity.ConstructionProgress));
      upgradeProgressView.AppendTo<ConstructionProgressView>(this.ItemsContainer, new float?(95f)).SetBackground(this.Builder.Style.Panel.ItemOverlay);
      this.AddUpdater(upgradeProgressView.Updater);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsBeingUpgraded)).Do((Action<bool>) (isBeingUpgraded =>
      {
        this.ItemsContainer.SetItemVisibility((IUiElement) upgradeProgressView, isBeingUpgraded);
        this.ItemsContainer.SetItemVisibility((IUiElement) upgradeProgressTitle, isBeingUpgraded);
      }));
      Txt popAdoptTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.AdoptPops__Title, new LocStrFormatted?((LocStrFormatted) Tr.AdoptPops__Tooltip));
      Panel popsCountHolder = this.AddOverlayPanel(this.ItemsContainer, 50);
      TextWithIcon popsCountTxt = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.TextMediumBold).SetIcon("Assets/Unity/UserInterface/General/PopulationSmall.svg").PutToLeftMiddleOf<TextWithIcon>((IUiElement) popsCountHolder, new Vector2(0.0f, 25f), Offset.Left(15f));
      popsCountTxt.SetPrefixText("10 / 10");
      CostButton adoptBtn = new CostButton(this.Builder, "", "Assets/Unity/UserInterface/General/UnitySmall.svg");
      adoptBtn.SetButtonStyle(this.Builder.Style.Global.UpointsBtn).PlayErrorSoundWhenDisabled().OnClick(new Action(adoptPops)).PutToLeftMiddleOf<Btn>((IUiElement) popsCountHolder, adoptBtn.GetOptimalSize(), Offset.Left(35f + popsCountTxt.GetWidth()));
      Tooltip adoptBtnTooltip = this.Builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) adoptBtn);
      WorldEntityShipOrdersView entityShipOrdersView = new WorldEntityShipOrdersView(this.Builder, this.m_fleetManager, (Func<WorldMapRepairableEntity>) (() => (WorldMapRepairableEntity) this.Entity), this.m_onGoToClick);
      entityShipOrdersView.AppendTo<WorldEntityShipOrdersView>(this.ItemsContainer, new float?(entityShipOrdersView.GetHeight()));
      this.AddUpdater(entityShipOrdersView.Updater);
      Txt loansTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.Loans_ProductsToLend);
      Panel productsPanel = this.AddOverlayPanel(this.ItemsContainer, 55);
      StackContainer productsStack = this.Builder.NewStackContainer("Stack").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f).PutToLeftOf<StackContainer>((IUiElement) productsPanel, 0.0f, Offset.All(10f));
      ViewsCacheHomogeneous<LoanProductView> productsCache = new ViewsCacheHomogeneous<LoanProductView>((Func<LoanProductView>) (() => new LoanProductView(this.Builder)));
      Btn openLoansBtn = this.Builder.NewBtnGeneral("Btn", (IUiElement) productsStack).OnClick((Action) (() => this.m_tradeWindowController.OpenAndShowLoans())).SetIcon("Assets/Unity/UserInterface/General/Loans.svg", 20.Vector2()).SetText((LocStrFormatted) Tr.Loans_Title);
      Lyst<KeyValuePair<ProductProto, bool>> productsToLend = new Lyst<KeyValuePair<ProductProto, bool>>();
      updaterBuilder.Observe<KeyValuePair<ProductProto, bool>>((Func<IIndexable<KeyValuePair<ProductProto, bool>>>) (() =>
      {
        productsToLend.Clear();
        foreach (WorldMapVillageProto.ProductToLend productToLend in this.Entity.Prototype.ProductsToLend)
          productsToLend.Add(Make.Kvp<ProductProto, bool>(productToLend.Product, this.m_inspectorContext.UnlockedProtosDbForUi.IsUnlocked((IProto) productToLend.Product)));
        return (IIndexable<KeyValuePair<ProductProto, bool>>) productsToLend;
      }), (ICollectionComparator<KeyValuePair<ProductProto, bool>, IIndexable<KeyValuePair<ProductProto, bool>>>) CompareFixedOrder<KeyValuePair<ProductProto, bool>>.Instance).Do((Action<Lyst<KeyValuePair<ProductProto, bool>>>) (products =>
      {
        productsCache.ReturnAll();
        productsStack.ClearAll();
        productsStack.StartBatchOperation();
        foreach (KeyValuePair<ProductProto, bool> product in products)
        {
          LoanProductView view = productsCache.GetView();
          view.SetData(product.Key, product.Value);
          view.AppendTo<LoanProductView>(productsStack, new float?(35f));
        }
        openLoansBtn.AppendTo<Btn>(productsStack, new Vector2?(openLoansBtn.GetOptimalSize()), ContainerPosition.MiddleOrCenter);
        productsStack.FinishBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) loansTitle, products.IsNotEmpty);
        this.ItemsContainer.SetItemVisibility((IUiElement) productsPanel, products.IsNotEmpty);
      }));
      QuickTradeView tradesView = new QuickTradeView((IUiElement) this.ItemsContainer, this.Builder, this.m_inspectorContext.InputScheduler, this.m_availableProductsProvider, WorldVillageView.WIDTH, (Func<IIndexable<QuickTradeProvider>>) (() => this.Entity.QuickTradesIndexable));
      tradesView.AppendTo<QuickTradeView>(this.ItemsContainer, new float?(0.0f), Offset.Top(5f));
      this.AddUpdater(tradesView.Updater);
      Txt contractsTitle = this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.Contracts__Title, new LocStrFormatted?((LocStrFormatted) Tr.Contracts__Tooltip), new Offset?(Offset.Bottom(5f)));
      this.ShipSizeForContracts = 4;
      int[] vals = new int[4]{ 2, 4, 6, 8 };
      Dropdwn rightOf = this.Builder.NewDropdown("Dropdown", (IUiElement) contractsTitle).AddOptions(((IEnumerable<int>) vals).Select<int, string>((Func<int, string>) (x => Tr.Contracts__ShipSizeModules.Format(x.ToString(), x).Value)).ToList<string>()).PutToRightOf<Dropdwn>((IUiElement) contractsTitle, 160f, Offset.Right(10f));
      rightOf.SetValueWithoutNotify(Array.IndexOf<int>(vals, this.ShipSizeForContracts));
      Txt objectToPlace = this.Builder.NewTxt("ShipSizeLabel").SetText((LocStrFormatted) Tr.Contracts__ShipSize).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
      objectToPlace.PutToLeftOf<Txt>((IUiElement) rightOf, objectToPlace.GetPreferedWidth(), Offset.Left((float) -((double) objectToPlace.GetPreferedWidth() + 10.0)));
      rightOf.OnValueChange(new Action<int>(onShipSizeChanged));
      GridContainer contractsContainer = this.Builder.NewGridContainer("Container").SetCellSize(ContractView.CompactSize).SetCellSpacing(5f).SetDynamicHeightMode(2).AppendTo<GridContainer>(this.ItemsContainer);
      contractsContainer.SizeChanged += (Action<IUiElement>) (x => this.ItemsContainer.UpdateItemHeight((IUiElement) contractsContainer, contractsContainer.GetRequiredHeight()));
      ViewsCacheTracked<ContractProto, ContractView> contractsCache = new ViewsCacheTracked<ContractProto, ContractView>((Func<ContractProto, ContractView>) (contract =>
      {
        ContractView contractView = new ContractView((IUiElement) contractsContainer, this.Builder, this.m_inspectorContext, this.m_contractsManager, this.m_sourceProductsAnalyzer);
        contractView.SetupForOverview((Func<int>) (() => this.ShipSizeForContracts));
        contractView.SetContract(contract);
        return contractView;
      }));
      this.AddUpdater(contractsCache.Updater);
      updaterBuilder.Observe<WorldMapVillageProto>((Func<WorldMapVillageProto>) (() => this.Entity.Prototype)).Do((Action<WorldMapVillageProto>) (proto =>
      {
        contractsCache.ReturnAll();
        contractsContainer.ClearAll();
        contractsContainer.StartBatchOperation();
        foreach (ContractProto contract in proto.Contracts)
          contractsContainer.Append((IUiElement) contractsCache.GetView(contract));
        contractsContainer.FinishBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) contractsTitle, proto.Contracts.IsNotEmpty);
        this.ItemsContainer.SetItemVisibility((IUiElement) contractsContainer, proto.Contracts.IsNotEmpty);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsPopsAdoptionSupported)).Do((Action<bool>) (isAdoptionVisible =>
      {
        this.ItemsContainer.StartBatchOperation();
        this.ItemsContainer.SetItemVisibility((IUiElement) popAdoptTitle, isAdoptionVisible);
        this.ItemsContainer.SetItemVisibility((IUiElement) popsCountHolder, isAdoptionVisible);
        this.ItemsContainer.FinishBatchOperation();
      }));
      LocStrFormatted error;
      updaterBuilder.Observe<int>((Func<int>) (() => this.Entity.PopsAvailable.Min(10))).Observe<KeyValuePair<bool, LocStrFormatted>>((Func<KeyValuePair<bool, LocStrFormatted>>) (() => Make.Kvp<bool, LocStrFormatted>(this.Entity.CanAdopt(this.Entity.PopsAvailable.Min(10), out error), error))).Observe<int>((Func<int>) (() => this.Entity.PopsAvailable)).Observe<int>((Func<int>) (() => this.Entity.Prototype.PopsToAdoptCapPerReputationLevel(this.Entity.Reputation))).Observe<WorldMapVillageProto>((Func<WorldMapVillageProto>) (() => this.Entity.Prototype)).Do((Action<int, KeyValuePair<bool, LocStrFormatted>, int, int, WorldMapVillageProto>) ((popsToAdopt, canAdoptError, popsInSettlement, popsCap, proto) =>
      {
        popsCountTxt.SetPrefixText(string.Format("{0} / {1}", (object) popsInSettlement, (object) popsCap));
        Upoints upoints = proto.UpointsPerPopToAdopt * popsToAdopt;
        adoptBtn.SetPrefixTextAndCost(Tr.AdoptPopsAction.Format(popsToAdopt.ToString(), popsToAdopt), upoints.FormatForceDigits());
        adoptBtn.SetEnabled(canAdoptError.Key);
        adoptBtnTooltip.SetText(canAdoptError.Value);
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth(WorldVillageView.WIDTH);
      this.OnHide += (Action) (() => tradesView.OnHide());

      void upgradeAction()
      {
        this.m_inspectorContext.InputScheduler.ScheduleInputCmd<WorldMapEntityUpgradeCmd>(new WorldMapEntityUpgradeCmd(this.Entity.Id));
      }

      void adoptPops()
      {
        this.m_inspectorContext.InputScheduler.ScheduleInputCmd<WorldMapSettlementAdoptPopsCmd>(new WorldMapSettlementAdoptPopsCmd(this.Entity.Id, 10.Min(this.Entity.PopsAvailable)));
      }

      void onShipSizeChanged(int index) => this.ShipSizeForContracts = vals[index];
    }

    public void SetEntity(WorldMapVillage village) => this.Entity = village;

    static WorldVillageView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WorldVillageView.WIDTH = (float) (2.0 * (double) ContractView.CompactSize.x + 5.0);
    }
  }
}
