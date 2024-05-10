// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.ActiveLoanView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.World.Loans;
using Mafi.Localization;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class ActiveLoanView : IUiElementWithUpdater, IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly InspectorContext m_context;
    private readonly LoansManager m_loansManager;
    private Quantity m_lastSeenToPay;
    private readonly TxtField m_txtField;
    private readonly Panel m_container;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; private set; }

    public ActiveLoan Loan { get; set; }

    public ActiveLoanView(
      StackContainer parent,
      UiBuilder builder,
      InspectorContext context,
      LoansManager loansManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ActiveLoanView activeLoanView = this;
      this.m_loansManager = loansManager;
      this.m_builder = builder;
      this.m_context = context;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_container = builder.NewPanel(nameof (Loan), (IUiElement) parent).SetHeight<Panel>(50f);
      StackContainer tilesContainer = builder.NewStackContainer("TilesContainer").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).PutToLeftOf<StackContainer>((IUiElement) this.m_container, 0.0f, Offset.Left(10f));
      TileView tile1 = new TileView(builder, (LocStrFormatted) Tr.Loan_Debt, LocStrFormatted.Empty, tilesContainer, new Offset?(Offset.Left(36f)));
      IconContainer icon = new IconContainer(builder, "Icon").PutToLeftMiddleOf<IconContainer>((IUiElement) tile1, 26.Vector2(), Offset.Left(10f));
      Tooltip balanceTooltip = tile1.AddToolTipAndReturn();
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => activeLoanView.Loan.Product)).Observe<Quantity>((Func<Quantity>) (() => activeLoanView.Loan.LeftToPay)).Do((Action<ProductProto, Quantity>) ((product, leftToPay) =>
      {
        icon.SetIcon(product.IconPath);
        tile1.SetValue(string.Format("{0}", (object) leftToPay));
      }));
      StringBuilder sb = new StringBuilder();
      updaterBuilder.Observe<ActiveLoan.BalanceLogEntry>((Func<IIndexable<ActiveLoan.BalanceLogEntry>>) (() => activeLoanView.Loan.BalanceLog), (ICollectionComparator<ActiveLoan.BalanceLogEntry, IIndexable<ActiveLoan.BalanceLogEntry>>) CompareFixedOrder<ActiveLoan.BalanceLogEntry>.Instance).Observe<ProductProto>((Func<ProductProto>) (() => activeLoanView.Loan.Product)).Do((Action<Lyst<ActiveLoan.BalanceLogEntry>, ProductProto>) ((log, product) =>
      {
        if (log.IsEmpty)
        {
          balanceTooltip.SetText((LocStrFormatted) product.Strings.Name);
        }
        else
        {
          int num = log.Count.Min(15);
          sb.AppendLine(product.Strings.Name.TranslatedString);
          sb.AppendLine();
          sb.AppendLine(Tr.Balance_LatestTransactions.TranslatedString);
          for (int index = 0; index < num; ++index)
          {
            ActiveLoan.BalanceLogEntry balanceLogEntry = log[log.Count - index - 1];
            sb.Append("<color=");
            ColorRgba colorRgba;
            if (balanceLogEntry.Diff.IsPositive)
            {
              StringBuilder stringBuilder = sb;
              colorRgba = builder.Style.Global.DangerClr;
              string hex = colorRgba.ToHex();
              stringBuilder.Append(hex);
            }
            else
            {
              StringBuilder stringBuilder = sb;
              colorRgba = builder.Style.Global.GreenForDark;
              string hex = colorRgba.ToHex();
              stringBuilder.Append(hex);
            }
            sb.Append(">");
            sb.Append(balanceLogEntry.Diff.IsPositive ? (object) string.Format("+{0}", (object) balanceLogEntry.Diff) : (object) balanceLogEntry.Diff);
            sb.Append(" | ");
            sb.Append(balanceLogEntry.Date.FormatLong());
            sb.AppendLine("</color>");
          }
          balanceTooltip.SetText(sb.ToString());
          sb.Clear();
        }
      }));
      TileView tile2 = new TileView(builder, (LocStrFormatted) Tr.Loan_TimeLeft, LocStrFormatted.Empty, tilesContainer);
      updaterBuilder.Observe<int>((Func<int>) (() => activeLoanView.Loan.YearsLeft)).Do((Action<int>) (yearsLeft => tile2.SetValue(string.Format("<size=12>{0}</size>", (object) TrCore.NumberOfYears.Format(string.Format("<size=16>{0}</size>", (object) yearsLeft), yearsLeft)))));
      Txt text;
      Panel tile3;
      this.createEmptyTile(builder, tilesContainer, out tile3, out text);
      text.SetText((LocStrFormatted) Tr.Loan_NextPayment);
      TextWithIcon nextPayment = new TextWithIcon(builder, (IUiElement) tile3, 18);
      nextPayment.SetTextStyle(builder.Style.Global.Text);
      nextPayment.PutToLeftTopOf<TextWithIcon>((IUiElement) tile3, new Vector2(0.0f, 25f), Offset.LeftRight(10f) + Offset.Top(5f));
      nextPayment.EnableRichText();
      tile3.AddToolTip(Tr.Loan_NextPayment__Tooltip.TranslatedString);
      float maxWidthSeen1 = 50f;
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => activeLoanView.Loan.Product)).Observe<Quantity>((Func<Quantity>) (() => activeLoanView.Loan.AnnualPayment)).Observe<RelGameDate>((Func<RelGameDate>) (() => activeLoanView.Loan.NextPaymentDate - activeLoanView.m_context.Calendar.CurrentDate)).Do((Action<ProductProto, Quantity, RelGameDate>) ((product, toPay, timeLeft) =>
      {
        nextPayment.SetPrefixText(string.Format("<b><size=16>{0}</size></b>", (object) toPay));
        nextPayment.SetIcon(product.IconPath);
        int totalMonthsRounded = timeLeft.TotalMonthsRounded;
        string upper = (totalMonthsRounded > 1 ? TrCore.NumberOfMonths.Format(string.Format("<size=16>{0}</size>", (object) totalMonthsRounded), totalMonthsRounded).Value : TrCore.NumberOfDays.Format(string.Format("<size=16>{0}</size>", (object) timeLeft.TotalDays), timeLeft.TotalDays).Value).ToUpper(LocalizationManager.CurrentCultureInfo);
        nextPayment.SetSuffixText(string.Format("{0} <b>{1}</b>", (object) Tr.Loan_NextPaymentIn, (object) upper));
        TextStyle textStyle = timeLeft.TotalDays > 0 ? builder.Style.Global.Text : builder.Style.Global.Text.Extend(new ColorRgba?(builder.Style.Global.OrangeText));
        nextPayment.SetTextStyle(textStyle);
        text.SetTextStyle(textStyle);
        maxWidthSeen1 = maxWidthSeen1.Max(nextPayment.GetWidth().Max(text.GetPreferedWidth()) + 20f);
        tilesContainer.UpdateItemWidth((IUiElement) tile3, maxWidthSeen1);
      }));
      StackContainer repayContainer = builder.NewStackContainer("RepayContainer").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetBackground((ColorRgba) 2236962).SetItemSpacing(5f).SetInnerPadding(Offset.LeftRight(10f)).AppendTo<StackContainer>(tilesContainer);
      Btn repayBtn = builder.NewBtnPrimary("Repay").SetText(string.Format("{0}:", (object) Tr.Loan_Repay__Action)).OnClick(new Action(this.repayLoan));
      Tooltip repayBtnTooltip = repayBtn.AddToolTipAndReturn();
      repayBtn.AppendTo<Btn>(repayContainer, new Vector2?(repayBtn.GetOptimalSize()), ContainerPosition.MiddleOrCenter);
      this.m_txtField = builder.NewTxtField("Quantity").SetText("0").SetOnEditEndListener(new Action<string>(this.onEditEnd)).AppendTo<TxtField>(repayContainer, new Vector2?(new Vector2(70f, 35f)), ContainerPosition.MiddleOrCenter);
      bool firstLayoutDone = false;
      updaterBuilder.Observe<LocStrFormatted>((Func<LocStrFormatted>) (() =>
      {
        int result;
        activeLoanView.m_lastSeenToPay = !int.TryParse(activeLoanView.m_txtField.GetText(), out result) ? Quantity.Zero : result.Quantity().Clamp(Quantity.Zero, activeLoanView.Loan.LeftToPay);
        if (activeLoanView.m_lastSeenToPay.IsNotPositive)
          return (LocStrFormatted) Tr.Loan_Repay__InvalidQuantity;
        activeLoanView.m_lastSeenToPay = activeLoanView.m_lastSeenToPay.Min(activeLoanView.Loan.LeftToPay);
        return !activeLoanView.m_context.AssetsManager.CanRemoveProduct(activeLoanView.Loan.Product.WithQuantity(activeLoanView.m_lastSeenToPay)) ? (LocStrFormatted) Tr.Loan_Repay__LackOfProducts : LocStrFormatted.Empty;
      })).Do((Action<LocStrFormatted>) (cannotPayError =>
      {
        repayBtn.SetEnabled(cannotPayError.IsEmptyOrNull);
        repayBtnTooltip.SetText(cannotPayError.IsNotEmpty ? cannotPayError : (LocStrFormatted) Tr.Loan_Repay__Tooltip);
        if (firstLayoutDone)
          return;
        firstLayoutDone = true;
        closure_0.m_txtField.SetEnabled(false);
        closure_0.m_txtField.SetEnabled(true);
      }));
      TextWithIcon availableQuantity = new TextWithIcon(builder, (IUiElement) repayContainer, 18);
      availableQuantity.SetIcon("Assets/Unity/UserInterface/Toolbar/Storages.svg").SetPrefixText(" | ").SetTextStyle(builder.Style.Global.TextMedium).AppendTo<TextWithIcon>(repayContainer, new float?(availableQuantity.GetWidth()));
      float maxWidthSeen2 = availableQuantity.GetWidth();
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => activeLoanView.m_context.AssetsManager.GetAvailableQuantityForRemoval(activeLoanView.Loan.Product))).Do((Action<Quantity>) (available =>
      {
        availableQuantity.SetSuffixText(available.ToString());
        maxWidthSeen2 = maxWidthSeen2.Max(availableQuantity.GetWidth());
        repayContainer.UpdateItemWidth((IUiElement) availableQuantity, maxWidthSeen2);
      }));
      IconContainer element = builder.NewIconContainer("Stats").SetIcon("Assets/Unity/UserInterface/Toolbar/Stats.svg", ColorRgba.White).AppendTo<IconContainer>(tilesContainer, new Vector2?(20.Vector2()), ContainerPosition.MiddleOrCenter, Offset.Left(5f));
      Tooltip statsTooltip = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) element);
      updaterBuilder.Observe<ActiveLoan>((Func<ActiveLoan>) (() => activeLoanView.Loan)).Observe<Quantity>((Func<Quantity>) (() => activeLoanView.Loan.InterestPaid + activeLoanView.Loan.InterestAddedToDebt)).Observe<Quantity>((Func<Quantity>) (() => activeLoanView.Loan.RemainingInterest)).Do((Action<ActiveLoan, Quantity, Quantity>) ((loan, interestAccumulated, remainingInterest) =>
      {
        string[] strArray = new string[5]
        {
          string.Format("{0}: {1}\n", (object) Tr.Loan_StartDate, (object) loan.StartDate.FormatLong()),
          string.Format("{0}: {1}\n", (object) Tr.Loan_StartingLoan, (object) loan.StartingDept),
          string.Format("{0}: {1}\n", (object) Tr.Loan_InterestRate, (object) loan.InterestRate),
          string.Format("{0}: {1}\n", (object) Tr.Loan_InterestSoFar, (object) interestAccumulated),
          string.Format("{0}: {1}", (object) Tr.Loan_RemainingInterest, (object) remainingInterest)
        };
        statsTooltip.SetText(string.Concat(strArray));
      }));
      this.Updater = updaterBuilder.Build();
    }

    private void onEditEnd(string result)
    {
      int result1;
      if (int.TryParse(result, out result1))
      {
        if (result1 < 0)
        {
          this.m_txtField.SetText("0");
        }
        else
        {
          if (result1 <= this.Loan.LeftToPay.Value)
            return;
          this.m_txtField.SetText(this.Loan.LeftToPay.Value.ToString());
        }
      }
      else
        this.m_txtField.SetText("0");
    }

    private void repayLoan()
    {
      this.m_context.InputScheduler.ScheduleInputCmd<MakeLoanOverpaymentCmd>(new MakeLoanOverpaymentCmd(this.Loan.Product.Id, this.Loan.Id, this.m_lastSeenToPay));
    }

    public void SetLoan(ActiveLoan loan)
    {
      if (this.Loan == loan)
        return;
      this.Loan = loan;
      this.m_txtField.Hide<TxtField>();
      this.m_txtField.Show<TxtField>();
    }

    private void createEmptyTile(
      UiBuilder builder,
      StackContainer parent,
      out Panel container,
      out Txt txt)
    {
      container = builder.NewPanel("Tile").SetBackground((ColorRgba) 2236962).AppendTo<Panel>(parent);
      ref Txt local1 = ref txt;
      Txt txt1 = builder.NewTxt("");
      TextStyle text = builder.Style.Global.Text;
      ref TextStyle local2 = ref text;
      bool? nullable = new bool?(true);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = new int?();
      bool? isCapitalized = nullable;
      TextStyle textStyle = local2.Extend(color, fontStyle, fontSize, isCapitalized);
      Txt bottomOf = txt1.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleLeft).EnableRichText().SetText("").PutToBottomOf<Txt>((IUiElement) container, 20f, Offset.LeftRight(10f) + Offset.Bottom(5f));
      local1 = bottomOf;
    }
  }
}
