// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.LoansTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.World.Loans;
using Mafi.Localization;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
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
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class LoansTab : Tab
  {
    private readonly LoansManager m_loansManager;
    private readonly InspectorContext m_context;
    private ProtoPicker<ProductProto> m_protoPicker;
    private Option<ProductProto> m_currentProduct;
    private int m_yearsDuration;
    private Quantity m_lastParsedQuantity;
    private ProtoWithIcon<ProductProto> m_productIcon;
    private TradeWindow m_tradeWindow;
    private TxtField m_toBorrowField;
    private Quantity m_lastMaxToBorrow;
    private Lyst<ProductProto> m_lastSeenAllowedProducts;

    internal LoansTab(LoansManager loansManager, InspectorContext context)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("Loans");
      this.m_loansManager = loansManager;
      this.m_context = context;
    }

    public void SetTradeWindow(TradeWindow tradeWindow) => this.m_tradeWindow = tradeWindow;

    protected override void BuildUi()
    {
      this.m_protoPicker = new ProtoPicker<ProductProto>(new Action<ProductProto>(this.onProductSelected));
      this.m_protoPicker.BuildUi(this.Builder);
      this.m_protoPicker.SetTitle((LocStrFormatted) Tr.ProductSelectorTitle);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      StackContainer container = this.Builder.NewStackContainer("Loans").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutTo<StackContainer>((IUiElement) this);
      container.SizeChanged += (Action<IUiElement>) (x => this.SetHeight<LoansTab>(container.GetDynamicHeight()));
      Panel overlay = this.m_tradeWindow.AddOverlay((Action) (() => { }));
      StackContainer parent1 = this.Builder.NewStackContainer("TilesContainer").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(5f).AppendTo<StackContainer>(container, new float?(50f), Offset.TopLeft(10f, 10f));
      TileView tile1 = new TileView(this.Builder, (LocStrFormatted) Tr.Loan_CreditScore, (LocStrFormatted) Tr.Loan_CreditScore__Tooltip, parent1);
      updaterBuilder.Observe<Fix32>((Func<Fix32>) (() => this.m_loansManager.CreditScore)).Do((Action<Fix32>) (score => tile1.SetValue(score.ToStringRounded())));
      TileView tile2 = new TileView(this.Builder, (LocStrFormatted) Tr.Loan_InterestRate, (LocStrFormatted) Tr.Loan_InterestRate__Tooltip, parent1);
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.m_loansManager.InterestRate)).Do((Action<Percent>) (interest => tile2.SetValue(interest.ToStringRounded())));
      TileView tile3 = new TileView(this.Builder, (LocStrFormatted) Tr.Loan_Fee, (LocStrFormatted) Tr.Loan_Fee__Tooltip, parent1);
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.m_loansManager.Fee)).Do((Action<Percent>) (fee => tile3.SetValue(fee.ToStringRounded())));
      TileView tile4 = new TileView(this.Builder, (LocStrFormatted) Tr.Loan_Multiplier, (LocStrFormatted) Tr.Loan_Multiplier__Tooltip, parent1);
      updaterBuilder.Observe<Fix32>((Func<Fix32>) (() => this.m_loansManager.LoanLimitMultiplier)).Do((Action<Fix32>) (multiplier => tile4.SetValue(multiplier.ToStringRounded())));
      UiBuilder builder1 = this.Builder;
      LocStrFormatted loanMaxLoans = (LocStrFormatted) Tr.Loan_MaxLoans;
      LocStrFormatted loanMaxLoansTooltip = (LocStrFormatted) Tr.Loan_MaxLoans__Tooltip;
      StackContainer parent2 = parent1;
      Offset? nullable = new Offset?();
      Offset? extraOffset1 = nullable;
      TileView tile5 = new TileView(builder1, loanMaxLoans, loanMaxLoansTooltip, parent2, extraOffset1);
      updaterBuilder.Observe<int>((Func<int>) (() => this.m_loansManager.MaxActiveLoans)).Do((Action<int>) (max => tile5.SetValue(max.ToString())));
      UiBuilder builder2 = this.Builder;
      StackContainer parent3 = container;
      LocStrFormatted loansActive = (LocStrFormatted) Tr.Loans_Active;
      nullable = new Offset?(Offset.Bottom(5f));
      LocStrFormatted? tooltip = new LocStrFormatted?();
      Offset? extraOffset2 = nullable;
      Txt activeLoansTitle = builder2.AddSectionTitle(parent3, loansActive, tooltip, extraOffset2);
      StackContainer activeLoansContainer = this.Builder.NewStackContainer("ActiveLoans").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f).AppendTo<StackContainer>(container);
      ViewsCacheHomogeneous<ActiveLoanView> activeLoansCache = new ViewsCacheHomogeneous<ActiveLoanView>((Func<ActiveLoanView>) (() => new ActiveLoanView(activeLoansContainer, this.Builder, this.m_context, this.m_loansManager)));
      updaterBuilder.Observe<ActiveLoan>((Func<IIndexable<ActiveLoan>>) (() => this.m_loansManager.ActiveLoans), (ICollectionComparator<ActiveLoan, IIndexable<ActiveLoan>>) CompareFixedOrder<ActiveLoan>.Instance).Do((Action<Lyst<ActiveLoan>>) (activeLoans =>
      {
        activeLoansCache.ReturnAll();
        activeLoansContainer.ClearAll();
        activeLoansContainer.StartBatchOperation();
        foreach (ActiveLoan activeLoan in activeLoans)
        {
          ActiveLoanView view = activeLoansCache.GetView();
          view.SetLoan(activeLoan);
          view.AppendTo<ActiveLoanView>(activeLoansContainer);
        }
        activeLoansContainer.FinishBatchOperation();
        container.SetItemVisibility((IUiElement) activeLoansTitle, activeLoans.IsNotEmpty);
      }));
      this.Builder.AddSectionTitle(container, (LocStrFormatted) Tr.Loan_NewLoan, new LocStrFormatted?((LocStrFormatted) Tr.Loans_Title__Tooltip), new Offset?(Offset.Bottom(5f)));
      int num = 30;
      Panel parent4 = this.Builder.AddOverlayPanel(container, 40 + num + 10);
      Btn newProductBtn = this.Builder.NewBtnPrimary("SelectProductFirst").AddToolTip(Tr.ProductSelectorTitle).OnClick(new Action(this.plusBtnClicked)).SetIcon("Assets/Unity/UserInterface/General/Plus.svg", new Offset?(Offset.All(4f)));
      newProductBtn.PutToLeftMiddleOf<Btn>((IUiElement) parent4, 30.Vector2(), Offset.Left(20f));
      Txt newProductBtnHint = this.Builder.NewTxt("").SetTextStyle(this.Builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).SetText(string.Format("<- {0}", (object) Tr.ProductSelectorTitle)).PutTo<Txt>((IUiElement) parent4, Offset.Left(newProductBtn.GetWidth() + 40f));
      StackContainer newLoanRow = this.Builder.NewStackContainer("NewLoanColumn").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(5f).PutToLeftOf<StackContainer>((IUiElement) parent4, 0.0f, Offset.Left(20f));
      this.Builder.NewBtnGeneral("SelectProduct").AddToolTip(Tr.ProductSelectorTitle).OnClick(new Action(this.plusBtnClicked)).SetIcon("Assets/Unity/UserInterface/General/Repeat.svg", new Offset?(Offset.All(4f))).AppendTo<Btn>(newLoanRow, new Vector2?(30.Vector2()), ContainerPosition.MiddleOrCenter);
      this.m_productIcon = new ProtoWithIcon<ProductProto>((IUiElement) parent4, this.Builder);
      this.m_productIcon.AppendTo<ProtoWithIcon<ProductProto>>(newLoanRow, new float?(70f), Offset.Top(10f));
      Panel formContainer = this.Builder.NewPanel("Form").AppendTo<Panel>(newLoanRow, new float?(250f), Offset.Left(10f));
      StackContainer formContainerTop = this.Builder.NewStackContainer("FormTop").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).PutToLeftTopOf<StackContainer>((IUiElement) formContainer, new Vector2(0.0f, 34f), Offset.Top(5f));
      StackContainer formContainerBottom = this.Builder.NewStackContainer("FormBottom").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).PutToLeftBottomOf<StackContainer>((IUiElement) formContainer, new Vector2(0.0f, 34f), Offset.Bottom(5f));
      Txt objectToPlace1 = this.Builder.NewTxt("").SetTextStyle(this.Builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).SetText(string.Format("{0}:", (object) Tr.Loan_BorrowFieldLabel));
      objectToPlace1.AppendTo<Txt>(formContainerTop, new float?(objectToPlace1.GetPreferedWidth()));
      this.m_toBorrowField = this.Builder.NewTxtField("Quantity").SetText("0").SetOnEditEndListener(new Action<string>(this.onEditEnd)).SetBackgroundColor((ColorRgba) 2236962);
      this.m_toBorrowField.AppendTo<TxtField>(formContainerTop, new float?(70f));
      Txt maxAvailable = this.Builder.NewTxt("").SetTextStyle(this.Builder.Style.Global.TextMedium).SetAlignment(TextAnchor.MiddleLeft).SetText("");
      maxAvailable.AppendTo<Txt>(formContainerTop, new float?(70f));
      Panel icon1;
      this.Builder.CreateTooltipIcon(out icon1, Tr.Loan_MaxToBorrowTooltip.TranslatedString);
      icon1.AppendTo<Panel>(formContainerTop, new Vector2?(icon1.GetSize()), ContainerPosition.MiddleOrCenter);
      TextWithIcon annualPaymentPreview = new TextWithIcon(this.Builder, (IUiElement) parent4);
      annualPaymentPreview.SetTextStyle(this.Builder.Style.Global.TextMedium);
      annualPaymentPreview.AppendTo<TextWithIcon>(formContainerBottom, new float?(0.0f));
      Dropdwn objectToPlace2 = this.Builder.NewDropdown("").AddOptions(LoansManager.YEARS_AVAILABLE.Select<string>((Func<int, string>) (x => TrCore.NumberOfYears.Format(x).Value)).ToList<string>()).OnValueChange((Action<int>) (index => this.m_yearsDuration = LoansManager.YEARS_AVAILABLE[index.Clamp(0, LoansManager.YEARS_AVAILABLE.Length - 1)]));
      this.m_yearsDuration = LoansManager.YEARS_AVAILABLE[LoansManager.YEARS_AVAILABLE_DEFAULT_INDEX];
      objectToPlace2.SetValueWithoutNotify(LoansManager.YEARS_AVAILABLE_DEFAULT_INDEX);
      objectToPlace2.AppendTo<Dropdwn>(formContainerBottom, new float?(120f));
      Panel icon2;
      this.Builder.CreateTooltipIcon(out icon2, Tr.Loan_DurationTooltip.TranslatedString);
      icon2.AppendTo<Panel>(formContainerBottom, new Vector2?(icon2.GetSize()), ContainerPosition.MiddleOrCenter);
      Btn takeLoanBtn = this.Builder.NewBtnPrimary("TakeLoan").SetText((LocStrFormatted) Tr.Loan_Borrow__Action).PlayErrorSoundWhenDisabled().OnClick(new Action(this.takeLoan));
      takeLoanBtn.AppendTo<Btn>(newLoanRow, new Vector2?(takeLoanBtn.GetOptimalSize()), ContainerPosition.MiddleOrCenter);
      Tooltip takeLoanBtnTooltip = takeLoanBtn.AddToolTipAndReturn();
      updaterBuilder.Observe<ProductProto>((Func<IEnumerable<ProductProto>>) (() => this.m_loansManager.GetAllProductsToBorrow()), (ICollectionComparator<ProductProto, IEnumerable<ProductProto>>) CompareFixedOrder<ProductProto>.Instance).Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => this.m_currentProduct)).Do((Action<Lyst<ProductProto>, Option<ProductProto>>) ((allProducts, currentProduct) =>
      {
        this.m_lastSeenAllowedProducts = allProducts;
        if (currentProduct.HasValue)
        {
          this.m_productIcon.SetProto((Option<ProductProto>) currentProduct.Value);
          annualPaymentPreview.SetIcon(currentProduct.Value.IconPath);
        }
        newLoanRow.SetVisibility<StackContainer>(currentProduct.HasValue);
        newProductBtn.SetVisibility<Btn>(currentProduct.IsNone);
        newProductBtnHint.SetVisibility<Txt>(currentProduct.IsNone);
      }));
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => !this.m_currentProduct.HasValue ? Quantity.Zero : this.m_loansManager.GetMaxToBorrow(this.m_currentProduct.Value, this.m_yearsDuration))).Do((Action<Quantity>) (max =>
      {
        this.m_lastMaxToBorrow = max;
        maxAvailable.SetText(string.Format("/ {0}", (object) max));
        formContainerTop.UpdateItemWidth((IUiElement) maxAvailable, maxAvailable.GetPreferedWidth());
      }));
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() =>
      {
        int result;
        if (this.m_currentProduct.HasValue && int.TryParse(this.m_toBorrowField.GetText(), out result))
        {
          this.m_lastParsedQuantity = result.Quantity();
          return this.m_loansManager.CalculateNewLoanPayment(this.m_currentProduct.Value, this.m_lastParsedQuantity, this.m_yearsDuration, this.m_loansManager.InterestRate);
        }
        this.m_lastParsedQuantity = Quantity.Zero;
        return Quantity.Zero;
      })).Observe<Percent>((Func<Percent>) (() => this.m_loansManager.Fee)).Observe<LocStrFormatted>((Func<LocStrFormatted>) (() =>
      {
        if (!this.m_currentProduct.HasValue)
          return "No product selected".AsLoc();
        LocStrFormatted error;
        this.m_loansManager.CanBorrow(this.m_currentProduct.Value, this.m_lastParsedQuantity, this.m_yearsDuration, out Quantity _, out error);
        return error;
      })).Do((Action<Quantity, Percent, LocStrFormatted>) ((annualPayment, feePercent, error) =>
      {
        string str1 = annualPayment.IsPositive ? annualPayment.ToString() : "?";
        annualPaymentPreview.SetPrefixText(string.Format("{0} {1}", (object) Tr.Loan_PayPerYear__part1, (object) str1));
        annualPaymentPreview.SetSuffixText(string.Format(" {0}", (object) Tr.Loan_PayPerYear__part2));
        formContainerBottom.UpdateItemWidth((IUiElement) annualPaymentPreview, annualPaymentPreview.GetWidth());
        newLoanRow.UpdateItemWidth((IUiElement) formContainer, formContainerBottom.GetDynamicWidth().Max(formContainerTop.GetDynamicWidth()) + 50f);
        if (error.IsNotEmpty)
        {
          takeLoanBtnTooltip.SetText(error);
          takeLoanBtnTooltip.SetErrorTextStyle();
        }
        else
        {
          Quantity quantity1 = this.m_lastParsedQuantity.ScaledBy(feePercent);
          Quantity quantity2 = this.m_lastParsedQuantity + quantity1;
          string str2 = string.Format("{0}\n\n", (object) Tr.Loan_Borrow__Tooltip);
          string str3 = string.Format("{0}: {1} + {2} ({3})\n", (object) Tr.Loan_Debt, (object) this.m_lastParsedQuantity, (object) quantity1, (object) Tr.Loan_Fee);
          string str4 = string.Format("{0}: {1}\n\n", (object) Tr.Loan_LifetimeInterest, (object) (annualPayment * this.m_yearsDuration - quantity2));
          string str5 = string.Format("{0}", (object) Tr.Loan_ProductsDeliveryTooltip);
          takeLoanBtnTooltip.SetText(str2 + str3 + str4 + str5);
          takeLoanBtnTooltip.SetNormalTextStyle();
        }
        takeLoanBtn.SetEnabled(error.IsEmptyOrNull);
      }));
      this.m_protoPicker.PutToCenterTopOf<ProtoPicker<ProductProto>>((IUiElement) overlay, this.m_protoPicker.GetSize(), Offset.Top(200f));
      this.m_protoPicker.OnShowStart += (Action) (() => overlay.Show<Panel>());
      this.m_protoPicker.OnHide += new Action(onPickerHide);
      overlay.OnClick(new Action(((View) this.m_protoPicker).Hide));
      this.OnHide += (Action) (() =>
      {
        if (!this.m_protoPicker.IsVisible)
          return;
        this.m_protoPicker.Hide();
      });
      this.AddUpdater(updaterBuilder.Build());
      this.AddUpdater(activeLoansCache.Updater);
      this.SetWidth<LoansTab>((float) this.AvailableWidth);

      void onPickerHide() => overlay.Hide<Panel>();
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        if (this.m_lastSeenAllowedProducts == null)
          return;
        this.m_protoPicker.SetVisibleProtos(this.m_lastSeenAllowedProducts.Where<ProductProto>((Func<ProductProto, bool>) (x => x != this.m_currentProduct)));
        this.m_protoPicker.Show();
      }
    }

    private void onProductSelected(ProductProto product)
    {
      this.m_currentProduct = (Option<ProductProto>) product;
      this.m_protoPicker.Hide();
    }

    private void takeLoan()
    {
      if (!this.m_currentProduct.HasValue)
        return;
      this.m_context.InputScheduler.ScheduleInputCmd<AcceptLoanCmd>(new AcceptLoanCmd(this.m_currentProduct.Value.Id, this.m_lastParsedQuantity, this.m_yearsDuration));
    }

    private void onEditEnd(string result)
    {
      int result1;
      if (int.TryParse(result, out result1))
      {
        if (result1 < 0)
        {
          this.m_toBorrowField.SetText("0");
        }
        else
        {
          if (result1 <= this.m_lastMaxToBorrow.Value)
            return;
          this.m_toBorrowField.SetText(this.m_lastMaxToBorrow.Value.ToString());
        }
      }
      else
        this.m_toBorrowField.SetText("0");
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.m_protoPicker.IsVisible || !UnityEngine.Input.GetKey(KeyCode.Escape))
        return false;
      this.m_protoPicker.Hide();
      return true;
    }
  }
}
