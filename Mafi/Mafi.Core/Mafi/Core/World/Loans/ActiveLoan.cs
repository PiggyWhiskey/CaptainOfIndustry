// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Loans.ActiveLoan
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.Loans
{
  [GenerateSerializer(false, null, 0)]
  public class ActiveLoan
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly uint Id;
    public readonly ProductProto Product;
    public readonly Percent RatioToMax;
    private readonly Percent m_interestRate;
    public NotificatorWithProtoParam PaymentDelayedNotif;
    public readonly Quantity StartingDept;
    public readonly GameDate StartDate;
    private readonly Lyst<ActiveLoan.BalanceLogEntry> m_balanceLog;
    public int BufferPriority;
    private readonly LoansManager m_loansManager;

    public static void Serialize(ActiveLoan value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ActiveLoan>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ActiveLoan.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Quantity.Serialize(this.AnnualPayment, writer);
      writer.WriteInt(this.BufferPriority);
      writer.WriteUInt(this.Id);
      Quantity.Serialize(this.InterestAddedToDebt, writer);
      Quantity.Serialize(this.InterestPaid, writer);
      writer.WriteBool(this.InterestRateDisabled);
      Quantity.Serialize(this.LeftToPay, writer);
      Lyst<ActiveLoan.BalanceLogEntry>.Serialize(this.m_balanceLog, writer);
      Percent.Serialize(this.m_interestRate, writer);
      LoansManager.Serialize(this.m_loansManager, writer);
      GameDate.Serialize(this.NextPaymentDate, writer);
      NotificatorWithProtoParam.Serialize(this.PaymentDelayedNotif, writer);
      writer.WriteGeneric<ProductProto>(this.Product);
      Percent.Serialize(this.RatioToMax, writer);
      GameDate.Serialize(this.StartDate, writer);
      Quantity.Serialize(this.StartingDept, writer);
      writer.WriteInt(this.YearsLeft);
      writer.WriteInt(this.YearsPaid);
    }

    public static ActiveLoan Deserialize(BlobReader reader)
    {
      ActiveLoan activeLoan;
      if (reader.TryStartClassDeserialization<ActiveLoan>(out activeLoan))
        reader.EnqueueDataDeserialization((object) activeLoan, ActiveLoan.s_deserializeDataDelayedAction);
      return activeLoan;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AnnualPayment = Quantity.Deserialize(reader);
      this.BufferPriority = reader.ReadInt();
      reader.SetField<ActiveLoan>(this, "Id", (object) reader.ReadUInt());
      this.InterestAddedToDebt = Quantity.Deserialize(reader);
      this.InterestPaid = Quantity.Deserialize(reader);
      this.InterestRateDisabled = reader.ReadBool();
      this.LeftToPay = Quantity.Deserialize(reader);
      reader.SetField<ActiveLoan>(this, "m_balanceLog", (object) Lyst<ActiveLoan.BalanceLogEntry>.Deserialize(reader));
      reader.SetField<ActiveLoan>(this, "m_interestRate", (object) Percent.Deserialize(reader));
      reader.SetField<ActiveLoan>(this, "m_loansManager", (object) LoansManager.Deserialize(reader));
      this.NextPaymentDate = GameDate.Deserialize(reader);
      this.PaymentDelayedNotif = NotificatorWithProtoParam.Deserialize(reader);
      reader.SetField<ActiveLoan>(this, "Product", (object) reader.ReadGenericAs<ProductProto>());
      reader.SetField<ActiveLoan>(this, "RatioToMax", (object) Percent.Deserialize(reader));
      reader.SetField<ActiveLoan>(this, "StartDate", (object) GameDate.Deserialize(reader));
      reader.SetField<ActiveLoan>(this, "StartingDept", (object) Quantity.Deserialize(reader));
      this.YearsLeft = reader.ReadInt();
      this.YearsPaid = reader.ReadInt();
    }

    public Quantity LeftToPay { get; private set; }

    public Quantity AnnualPayment { get; private set; }

    public Percent InterestRate => !this.InterestRateDisabled ? this.m_interestRate : Percent.Zero;

    public int YearsLeft { get; private set; }

    /// <summary>Includes only the years with a proper payment.</summary>
    public int YearsPaid { get; private set; }

    public GameDate NextPaymentDate { get; private set; }

    public Quantity InterestPaid { get; private set; }

    public Quantity InterestAddedToDebt { get; private set; }

    public Quantity RemainingInterest
    {
      get
      {
        return !this.InterestRate.IsZero ? this.AnnualPayment * this.YearsLeft - this.LeftToPay : Quantity.Zero;
      }
    }

    public bool InterestRateDisabled { get; private set; }

    public IIndexable<ActiveLoan.BalanceLogEntry> BalanceLog
    {
      get => (IIndexable<ActiveLoan.BalanceLogEntry>) this.m_balanceLog;
    }

    public ActiveLoan(
      uint loanId,
      LoansManager loansManager,
      ProductProto product,
      Quantity debt,
      GameDate startDate,
      int yearsTotal,
      Percent interestRate,
      Percent ratioToMax,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_balanceLog = new Lyst<ActiveLoan.BalanceLogEntry>();
      this.BufferPriority = 5;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Id = loanId;
      this.m_loansManager = loansManager;
      this.Product = product;
      this.LeftToPay = this.StartingDept = debt;
      this.StartDate = startDate;
      this.YearsLeft = yearsTotal;
      this.m_interestRate = interestRate;
      this.AnnualPayment = LoansManager.CalculateAnnualPayment(this.LeftToPay, this.YearsLeft, this.InterestRate);
      this.NextPaymentDate = this.StartDate + LoansManager.PAYMENT_FREQUENCY;
      this.RatioToMax = ratioToMax;
      this.PaymentDelayedNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.LoanPaymentDelayed);
    }

    private void updateAnnualPayment()
    {
      Quantity annualPayment1 = this.AnnualPayment;
      this.AnnualPayment = LoansManager.CalculateAnnualPayment(this.LeftToPay, this.YearsLeft, this.InterestRate);
      Quantity leftToPay1;
      if (this.AnnualPayment.IsNotPositive)
      {
        leftToPay1 = this.LeftToPay;
        if (leftToPay1.IsPositive)
        {
          Log.Error("No annual pay, yet loan not repaid fully!");
          this.YearsLeft = 1;
          Quantity leftToPay2 = this.LeftToPay;
          leftToPay1 = this.LeftToPay;
          Quantity quantity = leftToPay1.ScaledBy(this.InterestRate);
          this.AnnualPayment = leftToPay2 + quantity;
        }
      }
      leftToPay1 = this.LeftToPay;
      if (leftToPay1.IsPositive)
      {
        Quantity annualPayment2 = this.AnnualPayment;
        leftToPay1 = this.LeftToPay;
        Quantity quantity = leftToPay1.ScaledBy(this.InterestRate);
        if (annualPayment2 <= quantity)
        {
          Log.Error("Annual payment covers only interest and no repayment!");
          leftToPay1 = this.LeftToPay;
          this.AnnualPayment = leftToPay1.ScaledBy(this.InterestRate) + Quantity.One;
        }
      }
      if (!(annualPayment1 != this.AnnualPayment))
        return;
      this.m_loansManager.OnAnnualPaymentChanged(this);
    }

    public void MakeRegularPayment(Quantity toPay, GameDate date)
    {
      Assert.That<Quantity>(toPay).IsNotNegative();
      Assert.That<Quantity>(this.LeftToPay).IsPositive();
      Assert.That<int>(this.YearsLeft).IsPositive();
      this.NextPaymentDate += LoansManager.PAYMENT_FREQUENCY;
      Quantity quantity1 = this.LeftToPay.ScaledBy(this.InterestRate);
      if (toPay < quantity1)
      {
        Quantity diff = quantity1 - toPay;
        this.addToLeftToPay(diff, date);
        this.InterestAddedToDebt += diff;
        this.InterestPaid += toPay;
        if (!this.InterestRateDisabled && this.InterestPaid + this.InterestAddedToDebt > this.StartingDept * 5)
          this.InterestRateDisabled = true;
        this.updateAnnualPayment();
        this.m_loansManager.ReportPaymentFail(this);
      }
      else
      {
        this.InterestPaid += quantity1;
        Quantity quantity2 = (toPay - quantity1).Clamp(0.Quantity(), this.LeftToPay);
        if (quantity2.IsPositive)
          this.addToLeftToPay(-quantity2, date);
        if (this.AnnualPayment < toPay)
        {
          this.updateAnnualPayment();
          this.m_loansManager.ReportPaymentFail(this);
        }
        else
        {
          this.m_loansManager.ReportPaymentSuccess(this);
          this.YearsLeft = (this.YearsLeft - 1).Max(0);
          this.AnnualPayment = this.AnnualPayment.Min(this.LeftToPay + this.LeftToPay.ScaledBy(this.InterestRate));
          ++this.YearsPaid;
        }
      }
    }

    public void MakeOverpayment(Quantity toPay, GameDate date)
    {
      if (this.LeftToPay.IsPositive && this.YearsLeft > 1)
      {
        Percent p = Percent.FromRatio(toPay.Value, this.LeftToPay.Value);
        Fix32 fix32 = this.YearsLeft.ToFix32();
        fix32 = fix32.ScaledBy(p);
        int yearsSavedOnPayment = fix32.ToIntFloored();
        if (yearsSavedOnPayment >= this.YearsLeft)
          yearsSavedOnPayment = this.YearsLeft - 1;
        this.YearsLeft -= yearsSavedOnPayment;
        this.m_loansManager.ReportLoanOverpayment(this, yearsSavedOnPayment);
      }
      this.addToLeftToPay(-this.LeftToPay.Min(toPay), date);
      this.updateAnnualPayment();
    }

    private void addToLeftToPay(Quantity diff, GameDate date)
    {
      this.LeftToPay += diff;
      this.m_balanceLog.Add(new ActiveLoan.BalanceLogEntry(diff, date));
    }

    static ActiveLoan()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ActiveLoan.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ActiveLoan) obj).SerializeData(writer));
      ActiveLoan.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ActiveLoan) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct BalanceLogEntry
    {
      public readonly Quantity Diff;
      public readonly GameDate Date;

      public static void Serialize(ActiveLoan.BalanceLogEntry value, BlobWriter writer)
      {
        Quantity.Serialize(value.Diff, writer);
        GameDate.Serialize(value.Date, writer);
      }

      public static ActiveLoan.BalanceLogEntry Deserialize(BlobReader reader)
      {
        return new ActiveLoan.BalanceLogEntry(Quantity.Deserialize(reader), GameDate.Deserialize(reader));
      }

      [LoadCtor]
      public BalanceLogEntry(Quantity diff, GameDate date)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Diff = diff;
        this.Date = date;
      }
    }
  }
}
