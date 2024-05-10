// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Loans.LoansManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Game;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.World.Loans
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class LoansManager : 
    ICommandProcessor<AcceptLoanCmd>,
    IAction<AcceptLoanCmd>,
    ICommandProcessor<MakeLoanOverpaymentCmd>,
    IAction<MakeLoanOverpaymentCmd>,
    ICommandProcessor<SetLoanBufferPriorityCmd>,
    IAction<SetLoanBufferPriorityCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly ImmutableArray<int> YEARS_AVAILABLE;
    public static readonly int YEARS_AVAILABLE_DEFAULT_INDEX;
    public static readonly RelGameDate MAX_PAYMENT_DELAY;
    public static readonly RelGameDate PAYMENT_FREQUENCY;
    public static readonly RelGameDate PAYMENT_BUFFER_OPENS_BEFORE;
    public static readonly Fix32 SCORE_PENALTY_ON_MISSED_PAYMENT;
    public static readonly Fix32 SCORE_BONUS_ON_PAYMENT;
    public static readonly Fix32 SCORE_AUTO_RESTORE;
    private static readonly Quantity MIN_PER_YEAR;
    private static readonly Quantity MIN_LOAN;
    private static readonly Quantity MAX_LOAN;
    private readonly Lyst<ActiveLoan> m_activeLoans;
    private uint m_lastLoanId;
    private readonly TradeDockManager m_tradeDockManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly IProductsManager m_productsManager;
    private readonly ICalendar m_calendar;
    private readonly GameDifficultyApplier m_difficultyApplier;
    private readonly IAssetTransactionManager m_assetTransactionManager;
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly INotificationsManager m_notificationsManager;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<ProductProto> m_productsCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<QuantityLarge> m_aggregatedCache;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<QuantityLarge> m_quantityCache;

    public static void Serialize(LoansManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LoansManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LoansManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.CreditScore, writer);
      Lyst<ActiveLoan>.Serialize(this.m_activeLoans, writer);
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetTransactionManager);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      GameDifficultyApplier.Serialize(this.m_difficultyApplier, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      writer.WriteUInt(this.m_lastLoanId);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      TradeDockManager.Serialize(this.m_tradeDockManager, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
    }

    public static LoansManager Deserialize(BlobReader reader)
    {
      LoansManager loansManager;
      if (reader.TryStartClassDeserialization<LoansManager>(out loansManager))
        reader.EnqueueDataDeserialization((object) loansManager, LoansManager.s_deserializeDataDelayedAction);
      return loansManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CreditScore = Fix32.Deserialize(reader);
      reader.SetField<LoansManager>(this, "m_activeLoans", (object) Lyst<ActiveLoan>.Deserialize(reader));
      reader.SetField<LoansManager>(this, "m_aggregatedCache", (object) new Lyst<QuantityLarge>());
      reader.SetField<LoansManager>(this, "m_assetTransactionManager", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<LoansManager>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<LoansManager>(this, "m_difficultyApplier", (object) GameDifficultyApplier.Deserialize(reader));
      reader.SetField<LoansManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      this.m_lastLoanId = reader.ReadUInt();
      reader.SetField<LoansManager>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<LoansManager>(this, "m_productsCache", (object) new Set<ProductProto>());
      reader.SetField<LoansManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.RegisterResolvedMember<LoansManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<LoansManager>(this, "m_quantityCache", (object) new Lyst<QuantityLarge>());
      reader.SetField<LoansManager>(this, "m_tradeDockManager", (object) TradeDockManager.Deserialize(reader));
      reader.SetField<LoansManager>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.RegisterInitAfterLoad<LoansManager>(this, "initSelf", InitPriority.Normal);
    }

    private Quantity MaxLoanWithMultiplier
    {
      get => (LoansManager.MAX_LOAN.Value * this.LoanLimitMultiplier).ToIntRounded().Quantity();
    }

    public Fix32 CreditScore { get; private set; }

    [DoNotSave(0, null)]
    public Percent InterestRate { get; private set; }

    [DoNotSave(0, null)]
    public Fix32 LoanLimitMultiplier { get; private set; }

    [DoNotSave(0, null)]
    public int MaxActiveLoans { get; private set; }

    [DoNotSave(0, null)]
    public Percent Fee { get; private set; }

    public IIndexable<ActiveLoan> ActiveLoans => (IIndexable<ActiveLoan>) this.m_activeLoans;

    public LoansManager(
      TradeDockManager tradeDockManager,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      INotificationsManager notificationsManager,
      IEntitiesManager entitiesManager,
      IProductsManager productsManager,
      IAssetTransactionManager assetTransactionManager,
      ICalendar calendar,
      GameDifficultyApplier difficultyApplier)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCreditScore\u003Ek__BackingField = LoansDifficultyParams.StartingScore;
      this.m_activeLoans = new Lyst<ActiveLoan>();
      this.m_productsCache = new Set<ProductProto>();
      this.m_aggregatedCache = new Lyst<QuantityLarge>();
      this.m_quantityCache = new Lyst<QuantityLarge>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_tradeDockManager = tradeDockManager;
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_notificationsManager = notificationsManager;
      this.m_entitiesManager = entitiesManager;
      this.m_productsManager = productsManager;
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_calendar = calendar;
      this.m_difficultyApplier = difficultyApplier;
      this.m_difficultyApplier.OnDifficultyChanged.Add<LoansManager>(this, new Action(this.updateLoansParams));
      this.m_calendar.NewDay.Add<LoansManager>(this, new Action(this.onNewDay));
      this.m_calendar.NewYear.Add<LoansManager>(this, new Action(this.onNewYear));
      this.updateLoansParams();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf() => this.updateLoansParams();

    private void onNewDay()
    {
      GameDate currentDate = this.m_calendar.CurrentDate;
      foreach (ActiveLoan activeLoan in this.m_activeLoans)
      {
        if (activeLoan.YearsLeft <= 0 || activeLoan.LeftToPay.IsNotPositive)
        {
          Log.Error(string.Format("Found incorrect loan, yearsLeft: '{0}', leftToPay: '{1}'", (object) activeLoan.YearsLeft, (object) activeLoan.LeftToPay));
          this.finishLoanIfNeeded(activeLoan);
          break;
        }
        TradeDock valueOrNull = this.m_tradeDockManager.TradeDock.ValueOrNull;
        if (currentDate + LoansManager.PAYMENT_BUFFER_OPENS_BEFORE >= activeLoan.NextPaymentDate && valueOrNull != null)
          valueOrNull.OpenPaymentBuffer(activeLoan);
        if (!(activeLoan.NextPaymentDate > currentDate))
        {
          if (valueOrNull != null && valueOrNull.GetAvailableForRepayment(activeLoan) >= activeLoan.AnnualPayment)
          {
            Quantity toPay = valueOrNull.RemoveAndDestroyAsMuchForRepayment(activeLoan, activeLoan.AnnualPayment);
            activeLoan.MakeRegularPayment(toPay, currentDate);
            if (this.finishLoanIfNeeded(activeLoan))
              break;
          }
          else if (activeLoan.NextPaymentDate + LoansManager.MAX_PAYMENT_DELAY < currentDate)
          {
            Quantity toPay = valueOrNull != null ? valueOrNull.RemoveAndDestroyAsMuchForRepayment(activeLoan, activeLoan.AnnualPayment) : Quantity.Zero;
            activeLoan.MakeRegularPayment(toPay, currentDate);
          }
          else
            activeLoan.PaymentDelayedNotif.NotifyIff((Proto) activeLoan.Product, activeLoan.NextPaymentDate < currentDate);
        }
      }
    }

    private void onNewYear()
    {
      if (!this.ActiveLoans.IsEmpty<ActiveLoan>() || !(this.CreditScore < LoansDifficultyParams.StartingScore))
        return;
      this.CreditScore += LoansManager.SCORE_AUTO_RESTORE;
    }

    private void borrow(ProductProto product, Quantity toBorrow, int yearsTotal)
    {
      Quantity maxToBorrow;
      if (!this.CanBorrow(product, toBorrow, yearsTotal, out maxToBorrow, out LocStrFormatted _))
        return;
      Quantity debt = toBorrow + toBorrow.ScaledBy(this.Fee);
      Percent ratioToMax = Percent.FromRatio(toBorrow.Value, maxToBorrow.Value / 2).Min(Percent.Hundred);
      ++this.m_lastLoanId;
      ActiveLoan loan = new ActiveLoan(this.m_lastLoanId, this, product, debt, this.m_calendar.CurrentDate, yearsTotal, this.InterestRate, ratioToMax, this.m_notificationsManager);
      this.m_activeLoans.Add(loan);
      this.m_tradeDockManager.TradeDock.ValueOrNull?.UpdatePaymentBuffer(loan);
      ProductQuantity productQuantity = product.WithQuantity(toBorrow);
      this.m_productsManager.ProductCreated(productQuantity, CreateReason.Loan);
      this.m_tradeDockManager.TradeDock.Value.StoreProduct(productQuantity);
    }

    public bool CanBorrow(
      ProductProto product,
      Quantity toBorrow,
      int years,
      out Quantity maxToBorrow,
      out LocStrFormatted error)
    {
      error = LocStrFormatted.Empty;
      maxToBorrow = this.GetMaxToBorrow(product, years);
      if (maxToBorrow < LoansManager.MIN_LOAN)
      {
        error = (LocStrFormatted) TrCore.Loan_NotAvailable__LowProduction;
        return false;
      }
      if (this.m_tradeDockManager.TradeDock.IsNone)
      {
        error = (LocStrFormatted) TrCore.TradeStatus__NoTradeDock;
        return false;
      }
      if (!this.m_tradeDockManager.TradeDock.Value.CanTrade)
      {
        error = (LocStrFormatted) TrCore.TradeStatus__TradeDockNotOperational;
        return false;
      }
      if (this.m_activeLoans.Count >= this.MaxActiveLoans)
      {
        error = (LocStrFormatted) TrCore.Loan_NotAvailable__MaxLoans;
        return false;
      }
      if (toBorrow > maxToBorrow)
      {
        error = TrCore.Loan_NotAvailable__QuantityHigh.Format(maxToBorrow.ToString());
        return false;
      }
      if (!(toBorrow < LoansManager.MIN_LOAN))
        return true;
      error = TrCore.Loan_NotAvailable__QuantityLow.Format(LoansManager.MIN_LOAN.ToString());
      return false;
    }

    public Quantity GetMaxToBorrow(ProductProto product, int years)
    {
      ActiveLoan activeLoan = this.m_activeLoans.FirstOrDefault<ActiveLoan>((Predicate<ActiveLoan>) (x => (Proto) x.Product == (Proto) product));
      Quantity quantity = activeLoan != null ? activeLoan.LeftToPay : Quantity.Zero;
      this.m_aggregatedCache.Clear();
      this.m_aggregatedCache.AddRepeated(QuantityLarge.Zero, 5);
      ProductStats statsFor = this.m_productsManager.GetStatsFor(product);
      addStatsToAggregated(statsFor.CreatedByProduction);
      addStatsToAggregated(statsFor.CreatedByMiningStats);
      addStatsToAggregated(statsFor.CreatedByImportStats);
      this.m_aggregatedCache.RemoveWhere((Predicate<QuantityLarge>) (x => x.IsNotPositive));
      Quantity maxPayment = medianFrom(this.m_aggregatedCache).ToQuantityClamped().ScaledBy(LoansDifficultyParams.MaxAnnualPaymentToProductionRatio);
      if (maxPayment.IsNotPositive)
      {
        if (!this.canBorrowOnStart(product))
          return Quantity.Zero;
        maxPayment = LoansManager.MIN_LOAN.RoundDiv(years) + LoansManager.MIN_PER_YEAR;
      }
      return roundQuantity((LoansManager.GetMaxToBorrowBasedOnPayment(maxPayment, years, this.InterestRate) / (1 + this.Fee.ToFix64()) * this.LoanLimitMultiplier).ToIntRounded().Quantity().Clamp(LoansManager.MIN_LOAN, this.MaxLoanWithMultiplier) - quantity).Max(Quantity.Zero);

      static Quantity roundQuantity(Quantity quantity)
      {
        return quantity < 1000.Quantity() ? quantity.Value.RoundToMultipleOf(10).Quantity() : quantity.Value.RoundToMultipleOf(100).Quantity();
      }

      static QuantityLarge medianFrom(Lyst<QuantityLarge> list)
      {
        if (list.IsEmpty)
          return QuantityLarge.Zero;
        list.Sort();
        int index = list.Count / 2;
        return list.Count % 2 == 0 ? (list[index] + list[index - 1]) / 2 : list[index];
      }

      void addStatsToAggregated(QuantitySumStats stats)
      {
        this.m_quantityCache.Clear();
        stats.GetLastNYears(5, this.m_quantityCache);
        for (int index = 0; index < this.m_quantityCache.Count; ++index)
          this.m_aggregatedCache[index] += this.m_quantityCache[index];
      }
    }

    private bool canBorrowOnStart(ProductProto product)
    {
      foreach (WorldMapVillage worldMapVillage in this.m_entitiesManager.GetAllEntitiesOfType<WorldMapVillage>())
      {
        foreach (WorldMapVillageProto.ProductToLend productToLend in worldMapVillage.Prototype.ProductsToLend)
        {
          if ((Proto) productToLend.Product == (Proto) product)
            return productToLend.BorrowFromStart;
        }
      }
      return false;
    }

    public Quantity CalculateNewLoanPayment(
      ProductProto product,
      Quantity toBorrow,
      int years,
      Percent interestRate)
    {
      Quantity maxToBorrow = this.GetMaxToBorrow(product, years);
      return toBorrow < LoansManager.MIN_LOAN || toBorrow > maxToBorrow ? Quantity.Zero : LoansManager.CalculateAnnualPayment(toBorrow + toBorrow.ScaledBy(this.Fee), years, interestRate);
    }

    internal static Quantity CalculateAnnualPayment(
      Quantity toBorrow,
      int years,
      Percent interestRate)
    {
      if (toBorrow.IsNotPositive || years <= 0)
        return Quantity.Zero;
      if (interestRate.IsNotPositive)
        return (toBorrow.Value.ToFix32() / years).ToIntCeiled().Quantity();
      Quantity quantity = (toBorrow.Value * interestRate.ToFix64() / (1 - (1 + interestRate.ToFix64()).Pow((Fix64) (long) -years))).ToIntCeiled().Quantity();
      return !quantity.IsNotPositive ? quantity : toBorrow;
    }

    internal static Fix64 GetMaxToBorrowBasedOnPayment(
      Quantity maxPayment,
      int years,
      Percent interestRate)
    {
      if (maxPayment.IsNotPositive || years <= 0)
        return Fix64.Zero;
      return interestRate.IsNotPositive ? maxPayment.Value * years.ToFix64() : maxPayment.Value * (1 - (1 + interestRate.ToFix64()).Pow((Fix64) (long) -years)) / interestRate.ToFix64();
    }

    public IEnumerable<ProductProto> GetAllProductsToBorrow()
    {
      this.m_productsCache.Clear();
      foreach (WorldMapVillage worldMapVillage in this.m_entitiesManager.GetAllEntitiesOfType<WorldMapVillage>())
      {
        foreach (WorldMapVillageProto.ProductToLend productToLend in worldMapVillage.Prototype.ProductsToLend)
        {
          if (this.m_unlockedProtosDb.IsUnlocked((Proto) productToLend.Product))
            this.m_productsCache.Add(productToLend.Product);
        }
      }
      return (IEnumerable<ProductProto>) this.m_productsCache;
    }

    private bool finishLoanIfNeeded(ActiveLoan loan)
    {
      if (loan.AnnualPayment.IsPositive && loan.LeftToPay.IsPositive && loan.YearsLeft > 0)
        return false;
      Assert.That<int>(loan.YearsLeft).IsNotNegative("Loan duration got negative!");
      Assert.That<Quantity>(loan.LeftToPay).IsNotNegative("LeftToPay got negative!");
      Assert.That<Quantity>(loan.AnnualPayment).IsNotNegative("AnnualPayment got negative!");
      this.m_activeLoans.Remove(loan);
      this.m_tradeDockManager.TradeDock.ValueOrNull?.RemovePaymentBuffer(loan);
      loan.PaymentDelayedNotif.Deactivate();
      return true;
    }

    internal void OnAnnualPaymentChanged(ActiveLoan loan)
    {
      this.m_tradeDockManager.TradeDock.ValueOrNull?.UpdatePaymentBuffer(loan);
    }

    internal void ReportPaymentSuccess(ActiveLoan loan)
    {
      this.updateCreditScore(LoansManager.SCORE_BONUS_ON_PAYMENT.ScaledBy(loan.RatioToMax));
      loan.PaymentDelayedNotif.Deactivate();
      this.m_tradeDockManager.TradeDock.ValueOrNull?.ClosePaymentBuffer(loan);
    }

    internal void ReportPaymentFail(ActiveLoan loan)
    {
      this.updateCreditScore(-LoansManager.SCORE_PENALTY_ON_MISSED_PAYMENT);
      loan.PaymentDelayedNotif.Deactivate();
      this.m_notificationsManager.NotifyOnce<Proto>(IdsCore.Notifications.LoanPaymentFailed, (Proto) loan.Product);
    }

    internal void ReportLoanOverpayment(ActiveLoan loan, int yearsSavedOnPayment)
    {
      if (yearsSavedOnPayment <= 0)
        return;
      this.updateCreditScore((loan.YearsPaid.Min(yearsSavedOnPayment) * LoansManager.SCORE_BONUS_ON_PAYMENT).ScaledBy(loan.RatioToMax));
    }

    private void updateCreditScore(Fix32 diff)
    {
      this.CreditScore = (this.CreditScore + diff).Clamp(LoansDifficultyParams.MinScore, LoansDifficultyParams.MaxScore);
      this.updateLoansParams();
    }

    private void updateLoansParams()
    {
      LoansDifficulty loansDifficulty = this.m_difficultyApplier.DifficultyConfig.LoansDifficulty;
      this.InterestRate = LoansDifficultyParams.GetInterestRate(loansDifficulty, this.CreditScore);
      this.LoanLimitMultiplier = LoansDifficultyParams.GetMultiplier(loansDifficulty, this.CreditScore);
      this.MaxActiveLoans = LoansDifficultyParams.GetMaxActiveLoans(loansDifficulty, this.CreditScore);
      this.Fee = LoansDifficultyParams.GetFee(loansDifficulty, this.CreditScore);
    }

    public void Invoke(AcceptLoanCmd cmd)
    {
      ProductProto proto;
      if (!this.m_protosDb.TryGetProto<ProductProto>((Proto.ID) cmd.ProductId, out proto))
      {
        cmd.SetResultError(string.Format("Product with id '{0}' not found!", (object) cmd.ProductId));
      }
      else
      {
        this.borrow(proto, cmd.ToBorrow, cmd.YearsTotal);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(MakeLoanOverpaymentCmd cmd)
    {
      ActiveLoan loan = this.m_activeLoans.FirstOrDefault<ActiveLoan>((Predicate<ActiveLoan>) (x => (int) x.Id == (int) cmd.LoanId));
      if (loan == null)
        cmd.SetResultError(string.Format("Loan ID '{0}' does not exist!", (object) cmd.LoanId));
      else if (loan.Product.Id != cmd.ProductId)
        cmd.SetResultError(string.Format("Loan products are not matching '{0}' != '{1}'!", (object) loan.Product.Id, (object) cmd.ProductId));
      else if (cmd.ToPay.IsNotPositive)
      {
        cmd.SetResultError("Payment needs to be positive");
      }
      else
      {
        Quantity quantity = cmd.ToPay.Min(loan.LeftToPay);
        Quantity toPay1 = this.m_assetTransactionManager.RemoveAsMuchAs(loan.Product.WithQuantity(quantity), new DestroyReason?(DestroyReason.LoanPayment));
        if (loan.NextPaymentDate < this.m_calendar.CurrentDate && toPay1 >= loan.AnnualPayment)
        {
          loan.MakeRegularPayment(loan.AnnualPayment, this.m_calendar.CurrentDate);
          Quantity toPay2 = toPay1 - loan.AnnualPayment;
          if (toPay2.IsPositive)
            loan.MakeOverpayment(toPay2, this.m_calendar.CurrentDate);
        }
        else
          loan.MakeOverpayment(toPay1, this.m_calendar.CurrentDate);
        this.finishLoanIfNeeded(loan);
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(SetLoanBufferPriorityCmd cmd)
    {
      ActiveLoan activeLoan = this.m_activeLoans.FirstOrDefault<ActiveLoan>((Predicate<ActiveLoan>) (x => (int) x.Id == (int) cmd.LoanId));
      if (activeLoan == null)
      {
        cmd.SetResultError(string.Format("Loan ID '{0}' does not exist!", (object) cmd.LoanId));
      }
      else
      {
        int num = cmd.Priority.Clamp(0, 14);
        activeLoan.BufferPriority = num;
        cmd.SetResultSuccess();
      }
    }

    static LoansManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LoansManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LoansManager) obj).SerializeData(writer));
      LoansManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LoansManager) obj).DeserializeData(reader));
      LoansManager.YEARS_AVAILABLE = ImmutableArray.Create<int>(5, 10, 15, 20, 30);
      LoansManager.YEARS_AVAILABLE_DEFAULT_INDEX = 1;
      LoansManager.MAX_PAYMENT_DELAY = RelGameDate.FromMonths(2);
      LoansManager.PAYMENT_FREQUENCY = RelGameDate.FromMonths(12);
      LoansManager.PAYMENT_BUFFER_OPENS_BEFORE = RelGameDate.FromMonths(6);
      LoansManager.SCORE_PENALTY_ON_MISSED_PAYMENT = 1.ToFix32();
      LoansManager.SCORE_BONUS_ON_PAYMENT = 0.5.ToFix32();
      LoansManager.SCORE_AUTO_RESTORE = 0.25.ToFix32();
      LoansManager.MIN_PER_YEAR = 30.Quantity();
      LoansManager.MIN_LOAN = 100.Quantity();
      LoansManager.MAX_LOAN = 3000.Quantity();
    }
  }
}
