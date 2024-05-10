// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ContractPicker
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Economy;
using Mafi.Core.Syncers;
using Mafi.Core.World.Contracts;
using Mafi.Localization;
using Mafi.Unity.InputControl.World;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class ContractPicker : WindowView
  {
    private readonly InspectorContext m_context;
    private readonly ContractsManager m_contractsManager;
    private readonly SourceProductsAnalyzer m_sourceProductsAnalyzer;
    private readonly TradeWindowController m_tradeWindowController;
    private readonly Func<CargoDepotModule> m_depotModuleProvider;
    private readonly Action<ContractProto> m_onContractClicked;
    private StackContainer m_container;
    private ViewsCacheHomogeneous<ContractView> m_contractsCache;
    private static int MAX_HEIGHT;
    private ScrollableContainer m_scrollCont;

    public ContractPicker(
      InspectorContext context,
      ContractsManager contractsManager,
      SourceProductsAnalyzer sourceProductsAnalyzer,
      TradeWindowController tradeWindowController,
      Func<CargoDepotModule> depotModuleProvider,
      Action<ContractProto> onContractClicked)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (ContractPicker));
      this.m_context = context;
      this.m_contractsManager = contractsManager;
      this.m_sourceProductsAnalyzer = sourceProductsAnalyzer;
      this.m_tradeWindowController = tradeWindowController;
      this.m_depotModuleProvider = depotModuleProvider;
      this.m_onContractClicked = onContractClicked;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.EstablishedContracts__Title);
      this.m_scrollCont = this.Builder.NewScrollableContainer("ScrollableCont", (IUiElement) this.GetContentPanel()).PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel(), Offset.TopBottom(10f)).AddVerticalScrollbar().SetScrollSensitivity(this.Builder.Style.RecipeDetail.Height);
      this.m_container = this.Builder.NewStackContainer("Container", (IUiElement) this.m_scrollCont.Viewport).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f);
      this.m_scrollCont.AddItemTop((IUiElement) this.m_container);
      this.m_contractsCache = new ViewsCacheHomogeneous<ContractView>((Func<ContractView>) (() =>
      {
        ContractView contractView = new ContractView((IUiElement) this.m_container, this.Builder, this.m_context, this.m_contractsManager, this.m_sourceProductsAnalyzer);
        contractView.SetupForPicker(new Action<ContractView>(onContractClick), this.m_depotModuleProvider);
        return contractView;
      }));
      this.AddUpdater(this.m_contractsCache.Updater);
      Panel noContractsPanel = this.Builder.AddOverlayPanel(this.m_container);
      this.Builder.NewTxt("Info", (IUiElement) noContractsPanel).SetTextStyle(this.Builder.Style.Global.TextBig).SetText((LocStrFormatted) Tr.EstablishedContracts__NoneInfo).SetAlignment(TextAnchor.MiddleCenter).PutToTopOf<Txt>((IUiElement) noContractsPanel, 22f, Offset.LeftRight(10f) + Offset.Top(5f));
      Btn objectToPlace = this.Builder.NewBtnPrimary("GoToContacts").SetText((LocStrFormatted) Tr.GoToContracts).OnClick((Action) (() => this.m_tradeWindowController.OpenAndShowContracts()));
      objectToPlace.PutToCenterBottomOf<Btn>((IUiElement) noContractsPanel, objectToPlace.GetOptimalSize(), Offset.Bottom(5f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<ContractProto>((Func<IIndexable<ContractProto>>) (() => this.m_contractsManager.ActiveContracts), (ICollectionComparator<ContractProto, IIndexable<ContractProto>>) CompareFixedOrder<ContractProto>.Instance).Do((Action<Lyst<ContractProto>>) (contractsAvailable =>
      {
        this.m_contractsCache.ReturnAll();
        this.m_container.ClearAll();
        this.m_container.StartBatchOperation();
        foreach (ContractProto contract in contractsAvailable)
        {
          ContractView view = this.m_contractsCache.GetView();
          view.SetContract(contract);
          this.m_container.Append((IUiElement) view, new float?(view.GetHeight()));
        }
        if (contractsAvailable.IsEmpty)
        {
          noContractsPanel.Show<Panel>();
          this.m_container.Append((IUiElement) noContractsPanel, new float?(70f));
        }
        else
          noContractsPanel.Hide<Panel>();
        this.m_container.FinishBatchOperation();
        float self = this.m_container.GetDynamicHeight() + 20f;
        float height = self.Min((float) ContractPicker.MAX_HEIGHT);
        if ((double) self > (double) ContractPicker.MAX_HEIGHT)
        {
          this.SetContentSize(416f, height);
          this.m_container.SetInnerPadding(Offset.Right(16f));
        }
        else
        {
          this.SetContentSize(400f, height);
          this.m_container.SetInnerPadding(Offset.Zero);
        }
      }));
      this.AddUpdater(updaterBuilder.Build());

      void onContractClick(ContractView contractView)
      {
        this.m_onContractClicked(contractView.Contract.Value);
      }
    }

    protected override Option<IUiElement> GetParent(UiBuilder builder)
    {
      return (Option<IUiElement>) (IUiElement) builder.MainCanvas;
    }

    static ContractPicker()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ContractPicker.MAX_HEIGHT = 400;
    }
  }
}
