// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.TradeDockWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.World.Loans;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class TradeDockWindowView : StaticEntityInspectorBase<TradeDock>
  {
    private readonly Action m_onOpenWorldMapWindow;
    private readonly TradeDockInspector m_controller;
    private ProductQuantitiesView m_cargoView;

    protected override TradeDock Entity => this.m_controller.SelectedEntity;

    public TradeDockWindowView(TradeDockInspector controller, Action onOpenWorldMapWindow)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_onOpenWorldMapWindow = onOpenWorldMapWindow;
      this.m_controller = controller.CheckNotNull<TradeDockInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.SetWidth(500f);
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updater = UpdaterBuilder.Start();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoTitle, new LocStrFormatted?((LocStrFormatted) Tr.TradeDockCargo__Tooltip));
      this.AddSwitch(itemContainer, TrCore.ShipyardKeepEmpty.TranslatedString, (Action<bool>) (x => this.m_controller.ScheduleInputCmd<TradeDockToggleUnloadPriorityCmd>(new TradeDockToggleUnloadPriorityCmd(this.Entity.Id))), updater, (Func<bool>) (() => this.Entity.HasHighCargoUnloadPrio), Tr.ShipyardKeepEmpty__Tooltip.TranslatedString);
      this.m_cargoView = new ProductQuantitiesView((IUiElement) itemContainer, this.Builder);
      this.m_cargoView.AppendTo<ProductQuantitiesView>(itemContainer, new float?(0.0f));
      Lyst<ProductQuantity> cargoCache = new Lyst<ProductQuantity>();
      updater.Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        this.Entity.PeekAllCargo(cargoCache);
        return (IIndexable<ProductQuantity>) cargoCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Do((Action<Lyst<ProductQuantity>>) (cargo => this.m_cargoView.SetProducts((IIndexable<ProductQuantity>) cargo)));
      Btn objectToPlace = this.Builder.NewBtnPrimary("NewResearchBtn").SetText((LocStrFormatted) Tr.TradeWithVillage).OnClick(this.m_onOpenWorldMapWindow);
      objectToPlace.AppendTo<Btn>(itemContainer, new Vector2?(objectToPlace.GetOptimalSize()), ContainerPosition.MiddleOrCenter, Offset.TopBottom(10f));
      Txt loansTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Loan_PaymentBuffers__Title, new LocStrFormatted?((LocStrFormatted) Tr.Loan_PaymentBuffers__Tooltip));
      this.Builder.NewStackContainer("Loans").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).AppendTo<StackContainer>(itemContainer);
      StackContainer loanBuffersContainer = this.Builder.NewStackContainer("ActiveLoans").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).AppendTo<StackContainer>(itemContainer);
      ViewsCacheHomogeneous<TradeDockWindowView.LoanBufferView> activeLoansCache = new ViewsCacheHomogeneous<TradeDockWindowView.LoanBufferView>((Func<TradeDockWindowView.LoanBufferView>) (() => new TradeDockWindowView.LoanBufferView((IUiElement) loanBuffersContainer, this.Builder, this.m_controller.Context)));
      updater.Observe<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>((Func<IIndexable<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>>) (() => this.Entity.LoanBuffers), (ICollectionComparator<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>, IIndexable<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>>) CompareFixedOrder<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>.Instance).Do((Action<Lyst<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>>) (activeLoans =>
      {
        activeLoansCache.ReturnAll();
        loanBuffersContainer.ClearAll();
        loanBuffersContainer.StartBatchOperation();
        foreach (KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer> activeLoan in activeLoans)
        {
          TradeDockWindowView.LoanBufferView view = activeLoansCache.GetView();
          view.SetData(activeLoan.Key, activeLoan.Value);
          view.AppendTo<TradeDockWindowView.LoanBufferView>(loanBuffersContainer);
        }
        loanBuffersContainer.FinishBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) loansTitle, activeLoans.IsNotEmpty);
      }));
      this.AddUpdater(updater.Build());
      this.AddUpdater(activeLoansCache.Updater);
      CustomPriorityPanel customPriorityPanel = CustomPriorityPanel.NewForTradeDockCargo((IUiElement) this.m_cargoView, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) this.m_cargoView, customPriorityPanel.GetSize(), Offset.Right((float) (-(double) customPriorityPanel.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel.Updater);
    }

    private class LoanBufferView : IUiElementWithUpdater, IUiElement
    {
      private readonly Panel m_container;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public IUiUpdater Updater { get; private set; }

      private ActiveLoan Loan { get; set; }

      private TradeDock.LoanPaymentBuffer Buffer { get; set; }

      public LoanBufferView(IUiElement parent, UiBuilder builder, InspectorContext context)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        TradeDockWindowView.LoanBufferView loanBufferView = this;
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        this.m_container = builder.NewPanel("Container", parent).SetBackground(new ColorRgba(3092271)).SetHeight<Panel>(builder.Style.BufferView.CompactHeight + 10f);
        BufferView topOf = builder.NewBufferView((IUiElement) this.m_container, isCompact: true).PutToTopOf<BufferView>((IUiElement) this.m_container, builder.Style.BufferView.CompactHeight, Offset.Right(100f) + Offset.Top(2f));
        updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => loanBufferView.Buffer.Product)).Observe<Quantity>((Func<Quantity>) (() => loanBufferView.Buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => loanBufferView.Buffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(topOf.UpdateState));
        List<string> options = new List<string>();
        for (int index = 0; index <= 14; ++index)
          options.Add(string.Format("P{0}", (object) (index + 1)));
        Dropdwn dropdown = builder.NewDropdown("Dropdown", (IUiElement) this.m_container).AddOptions(options).PutToRightTopOf<Dropdwn>((IUiElement) this.m_container, new Vector2(80f, (float) Dropdwn.HEIGHT), Offset.Right(40f) + Offset.Top(3f));
        dropdown.OnValueChange((Action<int>) (newVal => context.InputScheduler.ScheduleInputCmd<SetLoanBufferPriorityCmd>(new SetLoanBufferPriorityCmd(loanBufferView.Loan.Id, newVal))));
        dropdown.AddTooltip(Tr.Loan_PaymentBuffer__PriorityToggle.TranslatedString);
        updaterBuilder.Observe<int>((Func<int>) (() => loanBufferView.Loan.BufferPriority)).Do((Action<int>) (priority => dropdown.SetValueWithoutNotify(priority)));
        Txt nextPayment = builder.NewTxt("NextPayment").SetTextStyle(builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).PutToBottomOf<Txt>((IUiElement) this.m_container, 20f, Offset.Left(95f) + Offset.Bottom(6f));
        updaterBuilder.Observe<GameDate>((Func<GameDate>) (() => loanBufferView.Loan.NextPaymentDate)).Observe<bool>((Func<bool>) (() => loanBufferView.Loan.NextPaymentDate < context.Calendar.CurrentDate)).Do((Action<GameDate, bool>) ((nextPaymentDate, isOverdue) =>
        {
          nextPayment.SetText(string.Format("{0}{1}: {2}", isOverdue ? (object) string.Format("{0}  |  ", (object) Tr.Loan_Overdue) : (object) "", (object) Tr.Loan_NextPayment, (object) nextPaymentDate.FormatLong()));
          nextPayment.SetColor(isOverdue ? builder.Style.Global.OrangeText : builder.Style.Global.Text.Color);
        }));
        Txt bufferClosed = builder.NewTxt("BufferClosed").SetTextStyle(builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).SetText((LocStrFormatted) Tr.Loan_PaymentBuffer__Closed).PutTo<Txt>((IUiElement) topOf.Bar, Offset.Left(10f));
        string monthsOpen = TrCore.NumberOfMonths.Format(LoansManager.PAYMENT_BUFFER_OPENS_BEFORE.Months).Value;
        Tooltip bufferClosedTooltip = builder.AddTooltipForTitle(bufferClosed);
        updaterBuilder.Observe<RelGameDate>((Func<RelGameDate>) (() => loanBufferView.Buffer.TimeUntilBufferOpens())).Do((Action<RelGameDate>) (timeTillOpen =>
        {
          int totalMonthsRounded = timeTillOpen.TotalMonthsRounded;
          string str = totalMonthsRounded > 1 ? TrCore.NumberOfMonths.Format(totalMonthsRounded).Value : TrCore.NumberOfDays.Format(timeTillOpen.TotalDays).Value;
          bufferClosed.SetVisibility<Txt>(timeTillOpen.IsPositive);
          bufferClosedTooltip.SetText(Tr.Loan_PaymentBuffer__ClosedTooltip.Format(monthsOpen, str));
        }));
        this.Updater = updaterBuilder.Build();
      }

      public void SetData(ActiveLoan loan, TradeDock.LoanPaymentBuffer buffer)
      {
        this.Loan = loan;
        this.Buffer = buffer;
      }
    }
  }
}
