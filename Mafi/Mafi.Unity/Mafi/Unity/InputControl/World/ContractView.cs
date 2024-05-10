// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.ContractView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Economy;
using Mafi.Core.Syncers;
using Mafi.Core.World.Contracts;
using Mafi.Localization;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class ContractView : IUiElementWithUpdater, IUiElement
  {
    public static Vector2 OptimalSize;
    public static Vector2 CompactSize;
    private readonly UiBuilder m_builder;
    private readonly InspectorContext m_context;
    private readonly SourceProductsAnalyzer m_sourceProductsAnalyzer;
    private readonly ContractsManager m_contractsManager;
    private readonly Panel m_panel;
    private readonly ProductQuantityWithIcon m_productToPayWithView;
    private readonly ProductQuantityWithIcon m_productToBuyView;
    private readonly TextWithIcon m_costPerQuantity;
    private readonly TextWithIcon m_costPerMonth;
    private readonly CostTooltipCompact m_costTooltip;
    private readonly Panel m_costIconContainer;
    private readonly Lyst<ProductQuantity> m_valueQueryResultTemp;

    public GameObject GameObject => this.m_panel.GameObject;

    public RectTransform RectTransform => this.m_panel.RectTransform;

    public IUiUpdater Updater { get; private set; }

    public Option<ContractProto> Contract { get; set; }

    public ContractView(
      IUiElement parent,
      UiBuilder builder,
      InspectorContext context,
      ContractsManager contractsManager,
      SourceProductsAnalyzer sourceProductsAnalyzer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_valueQueryResultTemp = new Lyst<ProductQuantity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_contractsManager = contractsManager;
      this.m_builder = builder;
      this.m_context = context;
      this.m_sourceProductsAnalyzer = sourceProductsAnalyzer;
      // ISSUE: method pointer
      // ISSUE: method pointer
      this.m_panel = builder.NewPanel("Product", parent).OnMouseEnter(new Action((object) this, __methodptr(\u003C\u002Ector\u003Eg__onMouseEnter\u007C25_0))).OnMouseLeave(new Action((object) this, __methodptr(\u003C\u002Ector\u003Eg__onMouseLeave\u007C25_1))).SetBackground(new ColorRgba(3092271));
      StackContainer stackContainer = builder.NewStackContainer("Items", (IUiElement) this.m_panel).PutToLeftOf<StackContainer>((IUiElement) this.m_panel, 0.0f).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetInnerPadding(Offset.LeftRight(10f));
      int x = 80;
      int y = 60;
      this.m_productToPayWithView = new ProductQuantityWithIcon((IUiElement) stackContainer, builder);
      this.m_productToPayWithView.HideProductName();
      this.m_productToPayWithView.AppendTo<ProductQuantityWithIcon>(stackContainer, new Vector2?(new Vector2((float) x, (float) y)), ContainerPosition.MiddleOrCenter);
      this.m_costIconContainer = builder.NewPanel("CostIcon", (IUiElement) this.m_productToPayWithView).PutToRightBottomOf<Panel>((IUiElement) this.m_productToPayWithView, 20.Vector2(), Offset.Bottom(4f) + Offset.Right(2f));
      builder.NewIconContainer("Icon", (IUiElement) this.m_costIconContainer).SetIcon("Assets/Unity/UserInterface/General/ValueEstimate.svg", (ColorRgba) 16767744).PutTo<IconContainer>((IUiElement) this.m_costIconContainer);
      this.m_costIconContainer.Hide<Panel>();
      this.m_costTooltip = new CostTooltipCompact(builder);
      this.m_costTooltip.SetText((LocStrFormatted) Tr.ProductionCostEstimate, true);
      this.m_costTooltip.AttachTo<Panel>((IUiElementWithHover<Panel>) this.m_costIconContainer);
      builder.NewIconContainer("arrow", (IUiElement) stackContainer).SetIcon(builder.Style.Icons.Transform, builder.Style.Global.Text.Color).AppendTo<IconContainer>(stackContainer, new Vector2?(new Vector2(25f, 25f)), ContainerPosition.MiddleOrCenter);
      this.m_productToBuyView = new ProductQuantityWithIcon((IUiElement) stackContainer, builder);
      this.m_productToBuyView.HideProductName();
      this.m_productToBuyView.AppendTo<ProductQuantityWithIcon>(stackContainer, new Vector2?(new Vector2((float) x, (float) y)), ContainerPosition.MiddleOrCenter);
      Panel parent1 = builder.NewPanel("InfoPanel", (IUiElement) stackContainer).AppendTo<Panel>(stackContainer, new float?(200f));
      this.m_costPerMonth = new TextWithIcon(builder, (IUiElement) parent1, 16).SetTextStyle(builder.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetColor(builder.Style.Global.UpointsTextColorForDark).PutToLeftTopOf<TextWithIcon>((IUiElement) parent1, new Vector2(0.0f, 20f), Offset.Top(7f) + Offset.Left(5f));
      this.m_costPerMonth.SetSuffixText(string.Format("/ {0}", (object) Tr.OneMonth));
      this.m_costPerQuantity = new TextWithIcon(builder, (IUiElement) parent1, 16).SetTextStyle(builder.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetColor(builder.Style.Global.UpointsTextColorForDark).PutToLeftTopOf<TextWithIcon>((IUiElement) parent1, new Vector2(0.0f, 20f), Offset.Top(33f) + Offset.Left(5f));
      this.m_costPerQuantity.SetPrefixText("0");
      this.m_costPerQuantity.SetSuffixText(string.Format("/ {0}", (object) Tr.UnityPerShip));
      this.Updater = UpdaterBuilder.Start().Build();
      this.SetHeight<ContractView>(60f);
    }

    public void SetContract(ContractProto contract)
    {
      if (this.Contract == contract)
        return;
      this.Contract = (Option<ContractProto>) contract;
      this.m_costPerMonth.SetPrefixText(contract.UpointsPerMonth.Value.ToStringRounded(1));
      this.m_productToPayWithView.SetIcon(contract.ProductToPayWith.Graphics.IconPath);
      this.m_productToBuyView.SetIcon(contract.ProductToBuy.Graphics.IconPath);
    }

    private void setShipSize(int modulesCount, ContractProto contract)
    {
      Quantity maxToImport;
      Quantity maxToExport;
      this.m_contractsManager.GetTradeEstimateForShipSize(contract, modulesCount, out maxToImport, out maxToExport);
      this.m_productToBuyView.SetProduct(contract.ProductToBuy.WithQuantity(maxToImport));
      if (maxToImport.IsZero)
        this.m_productToBuyView.SetQuantityText("?");
      this.m_productToPayWithView.SetProduct(contract.ProductToPayWith.WithQuantity(maxToExport));
      if (maxToExport.IsZero)
        this.m_productToPayWithView.SetQuantityText("?");
      if (maxToExport.IsPositive)
      {
        this.m_sourceProductsAnalyzer.GetSourceProductsFor(contract.ProductToPayWith, maxToExport, this.m_valueQueryResultTemp, true);
        this.m_costTooltip.SetCost((IIndexable<ProductQuantity>) this.m_valueQueryResultTemp);
        this.m_costIconContainer.SetVisibility<Panel>(this.m_valueQueryResultTemp.IsNotEmpty);
      }
      else
        this.m_costIconContainer.Hide<Panel>();
      this.m_costPerQuantity.SetPrefixText(contract.CalculateUpointsForQuantityBought(maxToImport).Value.ToStringRounded(1));
    }

    public void SetupForOverview(Func<int> shipSizeProviderAction)
    {
      int num = 120;
      Btn activateBtn = this.m_builder.NewBtnUpoints("ActivateBtn").OnClick(new Action(doTrade)).PlayErrorSoundWhenDisabled();
      Tooltip activateBtnTooltip = this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) activateBtn);
      Txt btnTxt = this.m_builder.NewTxt("Txt").SetTextStyle(this.m_builder.Style.Global.UpointsBtn.Text).SetAlignment(TextAnchor.MiddleCenter).SetText((LocStrFormatted) Tr.Contract__Establish).BestFitEnabled(this.m_builder.Style.Global.UpointsBtn.Text.FontSize).PutToTopOf<Txt>((IUiElement) activateBtn, 20f, Offset.Top(0.0f));
      TextWithIcon unityCostWithIcon = new TextWithIcon(this.m_builder, 18).SetPrefixText("10").PutToCenterBottomOf<TextWithIcon>((IUiElement) activateBtn, new Vector2(0.0f, 20f), Offset.Bottom(2f));
      unityCostWithIcon.SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg");
      Btn deactivateBtn = this.m_builder.NewBtnDanger("DeactivateBtn").OnClick(new Action(doTrade)).SetText((LocStrFormatted) Tr.Cancel).TextBestFitEnabled(this.m_builder.Style.Global.DangerBtn.Text.FontSize).PlayErrorSoundWhenDisabled();
      Tooltip deactivateBtnTooltip = this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) deactivateBtn);
      float x = btnTxt.GetPreferedWidth().Max(unityCostWithIcon.GetWidth() + 10f).Max(deactivateBtn.GetOptimalWidth() + 10f).Min((float) num);
      activateBtn.PutToRightMiddleOf<Btn>((IUiElement) this, new Vector2(x, 42f), Offset.Right(10f));
      deactivateBtn.PutToRightMiddleOf<Btn>((IUiElement) this, new Vector2(x, 42f), Offset.Right(10f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<ContractProto>>((Func<Option<ContractProto>>) (() => this.Contract)).Observe<int>(shipSizeProviderAction).Observe<Percent>((Func<Percent>) (() => this.m_contractsManager.ProfitMultiplier)).Do((Action<Option<ContractProto>, int, Percent>) ((contract, modulesCount, _) =>
      {
        if (!contract.HasValue)
          return;
        this.setShipSize(modulesCount, contract.Value);
      }));
      updaterBuilder.Observe<Upoints?>((Func<Upoints?>) (() => this.Contract.ValueOrNull?.UpointsToEstablish)).Do((Action<Upoints?>) (costToEstablish => unityCostWithIcon.SetPrefixText(costToEstablish.ToString())));
      updaterBuilder.Observe<Option<ContractProto>>((Func<Option<ContractProto>>) (() => this.Contract)).Observe<bool>((Func<bool>) (() => this.Contract.HasValue && this.m_contractsManager.IsEstablished(this.Contract.Value))).Observe<ContractsManager.EstablishCheckResult>((Func<ContractsManager.EstablishCheckResult>) (() => !this.Contract.HasValue ? ContractsManager.EstablishCheckResult.Ok : this.m_contractsManager.CanEstablish(this.Contract.Value))).Observe<ContractsManager.CancelCheckResult>((Func<ContractsManager.CancelCheckResult>) (() => !this.Contract.HasValue ? ContractsManager.CancelCheckResult.Ok : this.m_contractsManager.CanCancel(this.Contract.Value))).Do((Action<Option<ContractProto>, bool, ContractsManager.EstablishCheckResult, ContractsManager.CancelCheckResult>) ((contract, isActive, canAffordResult, canCancelResult) =>
      {
        if (contract.IsNone)
          return;
        if (isActive)
        {
          LocStrFormatted text = canCancelResult != ContractsManager.CancelCheckResult.HasShipsAssigned ? LocStrFormatted.Empty : (LocStrFormatted) TrCore.ContractCancelStatus__IsAssigned;
          deactivateBtn.SetEnabled(canCancelResult == ContractsManager.CancelCheckResult.Ok);
          deactivateBtnTooltip.SetText(text);
        }
        else
        {
          LocStrFormatted text;
          switch (canAffordResult)
          {
            case ContractsManager.EstablishCheckResult.VillageLevelLow:
              text = TrCore.Status_LowReputation.Format(this.Contract.Value.MinReputationRequired.ToString());
              break;
            case ContractsManager.EstablishCheckResult.ProductLocked:
              text = (LocStrFormatted) TrCore.ContractCancelStatus__ProductNotResearched;
              break;
            case ContractsManager.EstablishCheckResult.LacksUpoints:
              text = (LocStrFormatted) TrCore.TradeStatus__NoUnity;
              break;
            default:
              text = (LocStrFormatted) Tr.Contract__EstablishTooltip;
              break;
          }
          bool enabled = canAffordResult == ContractsManager.EstablishCheckResult.Ok;
          activateBtn.SetEnabled(enabled);
          ColorRgba color = enabled ? this.m_builder.Style.Global.UpointsBtn.Text.Color : (ColorRgba) 10653647;
          btnTxt.SetColor(color);
          unityCostWithIcon.SetColor(color);
          activateBtnTooltip.SetText(text);
        }
        activateBtn.SetVisibility<Btn>(!isActive);
        deactivateBtn.SetVisibility<Btn>(isActive);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());

      void doTrade()
      {
        this.m_context.InputScheduler.ScheduleInputCmd<ToggleContractCmd>(new ToggleContractCmd(this.Contract.ValueOrNull.Id));
      }
    }

    public void SetupForPicker(Action<ContractView> onClick, Func<CargoDepotModule> moduleProvider)
    {
      Btn btn = this.m_builder.NewBtnPrimary("AssignBtn").SetText((LocStrFormatted) Tr.Contract__Assign).OnClick((Action) (() => onClick(this))).PlayErrorSoundWhenDisabled();
      Btn assignBtn = btn;
      btn.PutToRightMiddleOf<Btn>((IUiElement) this, new Vector2(btn.GetOptimalWidth(), 42f), Offset.Right(10f));
      Tooltip assignBtnTooltip = this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) btn);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      LocStrFormatted errorReason;
      updaterBuilder.Observe<Option<ContractProto>>((Func<Option<ContractProto>>) (() => this.Contract)).Observe<int>((Func<int>) (() => moduleProvider().Depot.Value.SlotCount)).Observe<KeyValuePair<bool, LocStrFormatted>?>((Func<KeyValuePair<bool, LocStrFormatted>?>) (() => this.Contract.HasValue ? new KeyValuePair<bool, LocStrFormatted>?(Make.Kvp<bool, LocStrFormatted>(moduleProvider().Depot.Value.CanAssignContract(this.Contract.Value, out errorReason), errorReason)) : new KeyValuePair<bool, LocStrFormatted>?())).Observe<Percent>((Func<Percent>) (() => this.m_contractsManager.ProfitMultiplier)).Do((Action<Option<ContractProto>, int, KeyValuePair<bool, LocStrFormatted>?, Percent>) ((contract, slotCount, canAssign, _) =>
      {
        if (contract.HasValue)
          this.setShipSize(slotCount, contract.Value);
        if (!canAssign.HasValue)
          return;
        assignBtn.SetEnabled(canAssign.Value.Key);
        assignBtnTooltip.SetText(canAssign.Value.Value);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    public void SetupForDepotInspector(Action onUnassign, Func<CargoDepot> depot)
    {
      Btn objectToPlace = this.m_builder.NewBtnGeneral("UnassignBtn").SetText((LocStrFormatted) Tr.Contract__Unassign).OnClick(onUnassign).PlayErrorSoundWhenDisabled();
      objectToPlace.PutToRightMiddleOf<Btn>((IUiElement) this, new Vector2(objectToPlace.GetOptimalWidth(), 42f), Offset.Right(10f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<ContractProto>>((Func<Option<ContractProto>>) (() => this.Contract)).Observe<KeyValuePair<Quantity, Quantity>?>((Func<KeyValuePair<Quantity, Quantity>?>) (() =>
      {
        if (!this.Contract.HasValue)
          return new KeyValuePair<Quantity, Quantity>?();
        Quantity maxExportQuantity;
        Quantity maxImportQuantity;
        this.m_contractsManager.GetContractStatsForDepot(depot(), this.Contract.Value, out maxExportQuantity, out maxImportQuantity);
        return new KeyValuePair<Quantity, Quantity>?(new KeyValuePair<Quantity, Quantity>(maxExportQuantity, maxImportQuantity));
      })).Observe<Percent>((Func<Percent>) (() => this.m_contractsManager.ProfitMultiplier)).Do((Action<Option<ContractProto>, KeyValuePair<Quantity, Quantity>?, Percent>) ((contractMaybe, quantities, _) =>
      {
        if (contractMaybe.IsNone)
          return;
        ContractProto contractProto = contractMaybe.Value;
        KeyValuePair<Quantity, Quantity> valueOrDefault;
        Quantity zero;
        if (!quantities.HasValue)
        {
          zero = Quantity.Zero;
        }
        else
        {
          valueOrDefault = quantities.GetValueOrDefault();
          zero = valueOrDefault.Value;
        }
        Quantity quantity1 = zero;
        Quantity quantity2;
        if (!quantities.HasValue)
        {
          quantity2 = Quantity.Zero;
        }
        else
        {
          valueOrDefault = quantities.GetValueOrDefault();
          quantity2 = valueOrDefault.Key;
        }
        Quantity quantity3 = quantity2;
        this.m_productToBuyView.SetProduct(contractProto.ProductToBuy.WithQuantity(quantity1));
        if (quantity1.IsZero)
          this.m_productToBuyView.SetQuantityText("?");
        this.m_productToPayWithView.SetProduct(contractProto.ProductToPayWith.WithQuantity(quantity3));
        if (quantity3.IsZero)
          this.m_productToPayWithView.SetQuantityText("?");
        this.m_costPerQuantity.SetPrefixText(quantity1.IsPositive ? contractProto.CalculateUpointsForQuantityBought(quantity1).Value.ToStringRounded(1) : "?");
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    static ContractView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ContractView.OptimalSize = new Vector2(460f, 60f);
      ContractView.CompactSize = new Vector2(430f, 60f);
    }
  }
}
