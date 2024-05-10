// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.ContractsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.World.Contracts;
using Mafi.Localization;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class ContractsTab : Tab
  {
    private readonly ContractsManager m_contractsManager;
    private readonly SourceProductsAnalyzer m_sourceProductsAnalyzer;
    private readonly InspectorContext m_inspectorContext;

    private int ShipSizeForContracts { get; set; }

    internal ContractsTab(
      ContractsManager contractsManager,
      SourceProductsAnalyzer sourceProductsAnalyzer,
      InspectorContext context)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("Contracts");
      this.m_contractsManager = contractsManager;
      this.m_sourceProductsAnalyzer = sourceProductsAnalyzer;
      this.m_inspectorContext = context;
    }

    protected override void BuildUi()
    {
      StackContainer container = this.Builder.NewStackContainer("Contracts").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutTo<StackContainer>((IUiElement) this);
      container.SizeChanged += (Action<IUiElement>) (x => this.SetHeight<ContractsTab>(container.GetDynamicHeight()));
      Txt parent = this.Builder.AddSectionTitle(container, (LocStrFormatted) Tr.EstablishedContracts__Title, new LocStrFormatted?((LocStrFormatted) Tr.EstablishedContracts__Tooltip), new Offset?(Offset.Bottom(5f)));
      this.ShipSizeForContracts = 4;
      int[] vals = new int[4]{ 2, 4, 6, 8 };
      Dropdwn rightOf = this.Builder.NewDropdown("Dropdown", (IUiElement) parent).AddOptions(((IEnumerable<int>) vals).Select<int, string>((Func<int, string>) (x => Tr.Contracts__ShipSizeModules.Format(x.ToString(), x).Value)).ToList<string>()).PutToRightOf<Dropdwn>((IUiElement) parent, 180f, Offset.Right(10f));
      rightOf.SetValueWithoutNotify(Array.IndexOf<int>(vals, this.ShipSizeForContracts));
      Txt objectToPlace = this.Builder.NewTxt("ShipSizeLabel").SetText((LocStrFormatted) Tr.Contracts__ShipSize).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
      objectToPlace.PutToLeftOf<Txt>((IUiElement) rightOf, objectToPlace.GetPreferedWidth(), Offset.Left((float) -((double) objectToPlace.GetPreferedWidth() + 10.0)));
      rightOf.OnValueChange(new Action<int>(onShipSizeChanged));
      int columnsCount = ((float) (this.AvailableWidth - 25) / (ContractView.OptimalSize.x + 5f)).FloorToInt().Max(1);
      int x1 = (this.AvailableWidth - 25) / columnsCount - 5;
      GridContainer activeContractsContainer = this.Builder.NewGridContainer("Container").SetCellSize(new Vector2((float) x1, ContractView.OptimalSize.y)).SetCellSpacing(5f).SetDynamicHeightMode(columnsCount).SetInnerPadding(Offset.Left(5f) + Offset.Right(20f)).AppendTo<GridContainer>(container);
      Panel noContractsActivePanel = this.Builder.NewPanel("Panel").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(container, new float?(60f), Offset.Left(5f) + Offset.Right(15f));
      this.Builder.NewTxt("Txt").SetTextStyle(this.Builder.Style.Global.TextBig).SetAlignment(TextAnchor.MiddleLeft).SetText((LocStrFormatted) Tr.Contracts__NoneEstablished).PutTo<Txt>((IUiElement) noContractsActivePanel, Offset.LeftRight(10f));
      ViewsCacheTracked<ContractProto, ContractView> activeContractsCache = new ViewsCacheTracked<ContractProto, ContractView>((Func<ContractProto, ContractView>) (contract =>
      {
        ContractView contractView = new ContractView((IUiElement) activeContractsContainer, this.Builder, this.m_inspectorContext, this.m_contractsManager, this.m_sourceProductsAnalyzer);
        contractView.SetContract(contract);
        contractView.SetupForOverview((Func<int>) (() => this.ShipSizeForContracts));
        return contractView;
      }));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<ContractProto>((Func<IIndexable<ContractProto>>) (() => this.m_contractsManager.ActiveContracts), (ICollectionComparator<ContractProto, IIndexable<ContractProto>>) CompareFixedOrder<ContractProto>.Instance).Do((Action<Lyst<ContractProto>>) (contracts =>
      {
        activeContractsCache.ReturnAll();
        activeContractsContainer.ClearAll();
        activeContractsContainer.StartBatchOperation();
        foreach (ContractProto contract in contracts)
          activeContractsContainer.Append((IUiElement) activeContractsCache.GetView(contract));
        activeContractsContainer.FinishBatchOperation();
        container.UpdateItemHeight((IUiElement) activeContractsContainer, activeContractsContainer.GetRequiredHeight());
        container.SetItemVisibility((IUiElement) noContractsActivePanel, contracts.IsEmpty);
      }));
      this.Builder.AddTutorialIconForTitle(this.Builder.AddSectionTitle(container, (LocStrFormatted) Tr.Contracts__Title, new LocStrFormatted?((LocStrFormatted) Tr.Contracts__Tooltip)), this.m_inspectorContext.MessagesCenter, IdsCore.Messages.TutorialOnContracts, true);
      GridContainer unlockedContractsContainer = this.Builder.NewGridContainer("Container").SetCellSize(new Vector2((float) x1, ContractView.OptimalSize.y)).SetCellSpacing(5f).SetDynamicHeightMode(columnsCount).SetInnerPadding(Offset.Left(5f) + Offset.Right(20f)).AppendTo<GridContainer>(container);
      ViewsCacheTracked<ContractProto, ContractView> unlockedContractsCache = new ViewsCacheTracked<ContractProto, ContractView>((Func<ContractProto, ContractView>) (contract =>
      {
        ContractView contractView = new ContractView((IUiElement) unlockedContractsContainer, this.Builder, this.m_inspectorContext, this.m_contractsManager, this.m_sourceProductsAnalyzer);
        contractView.SetContract(contract);
        contractView.SetupForOverview((Func<int>) (() => this.ShipSizeForContracts));
        return contractView;
      }));
      Lyst<ContractProto> tempContracts = new Lyst<ContractProto>();
      updaterBuilder.Observe<ContractProto>((Func<IIndexable<ContractProto>>) (() =>
      {
        this.m_contractsManager.GetUnlockedContractsButNotActive(tempContracts);
        return (IIndexable<ContractProto>) tempContracts;
      }), (ICollectionComparator<ContractProto, IIndexable<ContractProto>>) CompareFixedOrder<ContractProto>.Instance).Do((Action<Lyst<ContractProto>>) (contracts =>
      {
        IEnumerable<ContractProto> contractProtos = contracts.GroupBy<ContractProto, ProductProto>((Func<ContractProto, ProductProto>) (x => x.ProductToBuy)).SelectMany<IGrouping<ProductProto, ContractProto>, ContractProto>((Func<IGrouping<ProductProto, ContractProto>, IEnumerable<ContractProto>>) (x => (IEnumerable<ContractProto>) x));
        unlockedContractsCache.ReturnAll();
        unlockedContractsContainer.ClearAll();
        unlockedContractsContainer.StartBatchOperation();
        foreach (ContractProto data in contractProtos)
          unlockedContractsContainer.Append((IUiElement) unlockedContractsCache.GetView(data));
        unlockedContractsContainer.FinishBatchOperation();
        container.UpdateItemHeight((IUiElement) unlockedContractsContainer, unlockedContractsContainer.GetRequiredHeight());
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.AddUpdater(activeContractsCache.Updater);
      this.AddUpdater(unlockedContractsCache.Updater);
      this.SetWidth<ContractsTab>((float) this.AvailableWidth);

      void onShipSizeChanged(int index) => this.ShipSizeForContracts = vals[index];
    }
  }
}
