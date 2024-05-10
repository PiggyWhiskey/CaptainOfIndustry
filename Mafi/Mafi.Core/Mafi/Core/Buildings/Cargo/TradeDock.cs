// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.TradeDock
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Vehicles;
using Mafi.Core.World.Loans;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [GenerateSerializer(false, null, 0)]
  public class TradeDock : 
    LayoutEntity,
    IEntityWithCustomPriority,
    IEntity,
    IIsSafeAsHashKey,
    IStaticEntityWithReservedOcean,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    private TradeDockProto m_proto;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    public const string CARGO_EXPORT_PRIO_ID = "CargoExportPrio";
    private int m_cargoExportPriority;
    private readonly Dict<ProductProto, ProductBuffer> m_cargo;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private readonly Lyst<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>> m_loanPaymentBuffers;
    private readonly TradeDock.StoredCargoPriorityProvider m_storedCargoPrioProvider;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public TradeDockProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => false;

    public bool CanTrade
    {
      get => this.IsEnabled && this.IsConstructed && this.ReservedOceanAreaState.HasAnyValidAreaSet;
    }

    public bool HasHighCargoUnloadPrio { get; set; }

    public IIndexable<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>> LoanBuffers
    {
      get
      {
        return (IIndexable<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>) this.m_loanPaymentBuffers;
      }
    }

    public IProtoWithReservedOcean ReservedOceanProto { get; private set; }

    public ReservedOceanAreaState ReservedOceanAreaState { get; private set; }

    public TradeDock(
      EntityId id,
      TradeDockProto proto,
      TileTransform transform,
      EntityContext context,
      LoansManager loansManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_cargoExportPriority = 8;
      this.m_cargo = new Dict<ProductProto, ProductBuffer>();
      this.m_loanPaymentBuffers = new Lyst<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.ReservedOceanProto = (IProtoWithReservedOcean) proto;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_storedCargoPrioProvider = new TradeDock.StoredCargoPriorityProvider(this);
      this.ReservedOceanAreaState = new ReservedOceanAreaState((IProtoWithReservedOcean) proto, (IStaticEntityWithReservedOcean) this, new EntityNotificationProto.ID?(IdsCore.Notifications.OceanAccessBlocked), context.NotificationsManager);
      foreach (ActiveLoan activeLoan in loansManager.ActiveLoans)
        this.UpdatePaymentBuffer(activeLoan);
      context.Calendar.NewDay.Add<TradeDock>(this, new Action(this.onNewDay));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      if (saveVersion >= 140)
        return;
      this.Context.Calendar.NewDay.Add<TradeDock>(this, new Action(this.onNewDay));
    }

    private void onNewDay()
    {
      foreach (KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer> loanPaymentBuffer in this.m_loanPaymentBuffers)
      {
        TradeDock.LoanPaymentBuffer buffer = loanPaymentBuffer.Value;
        if (buffer.IsOpenForDelivery)
        {
          Quantity quantity1 = buffer.UsableCapacity;
          ProductBuffer productBuffer;
          if (!quantity1.IsNotPositive && this.m_cargo.TryGetValue(buffer.Product, out productBuffer))
          {
            quantity1 = productBuffer.Quantity;
            if (quantity1.IsPositive)
            {
              Quantity quantity2 = productBuffer.RemoveAsMuchAs(buffer.UsableCapacity);
              buffer.StoreExactly(quantity2);
            }
          }
        }
      }
    }

    public void PeekAllCargo(Lyst<ProductQuantity> cacheToFill)
    {
      cacheToFill.Clear();
      foreach (ProductBuffer productBuffer in this.m_cargo.Values)
      {
        if (productBuffer.Quantity.IsPositive)
          cacheToFill.Add(productBuffer.ProductQuantity);
      }
    }

    public Quantity GetCargoQuantityOf(ProductProto product)
    {
      ProductBuffer productBuffer;
      return this.m_cargo.TryGetValue(product, out productBuffer) ? productBuffer.Quantity : Quantity.Zero;
    }

    protected override void OnDestroy()
    {
      foreach (IProductBuffer buffer in this.m_cargo.Values)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer(buffer);
      foreach (KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer> loanPaymentBuffer in this.m_loanPaymentBuffers)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) loanPaymentBuffer.Value);
      base.OnDestroy();
    }

    private IProductBuffer getOrCreateCargoBufferFor(ProductProto product)
    {
      ProductBuffer cargoBufferFor;
      if (this.m_cargo.TryGetValue(product, out cargoBufferFor))
        return (IProductBuffer) cargoBufferFor;
      ProductBuffer buffer = (ProductBuffer) new GlobalOutputBuffer(Quantity.MaxValue, product, this.Context.ProductsManager, 5, (IEntity) this);
      this.m_cargo.Add(buffer.Product, buffer);
      this.m_vehicleBuffersRegistry.RegisterOutputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IOutputBufferPriorityProvider) this.m_storedCargoPrioProvider);
      this.m_vehicleBuffersRegistry.RegisterInputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IInputBufferPriorityProvider) StaticPriorityProvider.Ignore, true, true);
      return (IProductBuffer) buffer;
    }

    public void StoreProduct(ProductQuantity productQuantity)
    {
      Assert.That<bool>(productQuantity.Product.IsStorable).IsTrue("Trying to store non-storable product in trade dock.");
      this.getOrCreateCargoBufferFor(productQuantity.Product).StoreExactly(productQuantity.Quantity);
    }

    public Quantity RemoveAsMuchAs(ProductQuantity productQuantity)
    {
      ProductBuffer productBuffer;
      return this.m_cargo.TryGetValue(productQuantity.Product, out productBuffer) ? productBuffer.RemoveAsMuchAs(productQuantity.Quantity) : Quantity.Zero;
    }

    public void UpdatePaymentBuffer(ActiveLoan loan)
    {
      TradeDock.LoanPaymentBuffer buffer;
      if (!this.m_loanPaymentBuffers.TryGetValue<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan, out buffer))
      {
        TradeDock.LoanPaymentBuffer loanPaymentBuffer = new TradeDock.LoanPaymentBuffer(this.m_vehicleBuffersRegistry, this, loan);
        this.m_loanPaymentBuffers.Add<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan, loanPaymentBuffer);
      }
      else
      {
        if (buffer.Quantity > loan.AnnualPayment)
        {
          Quantity quantity = buffer.Quantity - loan.AnnualPayment;
          this.getOrCreateCargoBufferFor(buffer.Product).StoreExactly(quantity);
          buffer.RemoveExactly(quantity);
        }
        buffer.SetCapacity(loan.AnnualPayment);
      }
    }

    public void RemovePaymentBuffer(ActiveLoan loan)
    {
      TradeDock.LoanPaymentBuffer loanPaymentBuffer;
      if (!this.m_loanPaymentBuffers.TryGetValue<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan, out loanPaymentBuffer))
        return;
      this.m_loanPaymentBuffers.Remove<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan);
      if (loanPaymentBuffer.Quantity.IsPositive)
        this.getOrCreateCargoBufferFor(loanPaymentBuffer.Product).StoreExactly(loanPaymentBuffer.Quantity);
      loanPaymentBuffer.Destroy();
    }

    public Quantity GetAvailableForRepayment(ActiveLoan loan)
    {
      TradeDock.LoanPaymentBuffer loanPaymentBuffer;
      return this.m_loanPaymentBuffers.TryGetValue<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan, out loanPaymentBuffer) ? loanPaymentBuffer.Quantity : Quantity.Zero;
    }

    public Quantity RemoveAndDestroyAsMuchForRepayment(ActiveLoan loan, Quantity maxQuantity)
    {
      TradeDock.LoanPaymentBuffer loanPaymentBuffer;
      Quantity quantity;
      if (this.m_loanPaymentBuffers.TryGetValue<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan, out loanPaymentBuffer))
      {
        quantity = loanPaymentBuffer.RemoveAsMuchAs(maxQuantity);
        this.Context.ProductsManager.ProductDestroyed(loan.Product, quantity, DestroyReason.LoanPayment);
      }
      else
        quantity = Quantity.Zero;
      return quantity;
    }

    public void OpenPaymentBuffer(ActiveLoan loan)
    {
      TradeDock.LoanPaymentBuffer loanPaymentBuffer;
      if (!this.m_loanPaymentBuffers.TryGetValue<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan, out loanPaymentBuffer))
        return;
      loanPaymentBuffer.OpenBuffer();
    }

    public void ClosePaymentBuffer(ActiveLoan loan)
    {
      TradeDock.LoanPaymentBuffer loanPaymentBuffer;
      if (!this.m_loanPaymentBuffers.TryGetValue<ActiveLoan, TradeDock.LoanPaymentBuffer>(loan, out loanPaymentBuffer))
        return;
      loanPaymentBuffer.CloseBuffer();
    }

    public int GetCustomPriority(string id)
    {
      if (id == "CargoExportPrio")
        return this.m_cargoExportPriority;
      Assert.Fail("Unknown custom priority: " + id);
      return 0;
    }

    public bool IsCustomPriorityVisible(string id)
    {
      return id == "CargoExportPrio" && this.HasHighCargoUnloadPrio;
    }

    public void SetCustomPriority(string id, int priority)
    {
      if (!GeneralPriorities.AssertAssignableRange(priority))
        return;
      if (id == "CargoExportPrio")
        this.m_cargoExportPriority = priority;
      else
        Assert.Fail("Unknown custom priority: " + id);
    }

    public static void Serialize(TradeDock value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TradeDock>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TradeDock.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.HasHighCargoUnloadPrio);
      Dict<ProductProto, ProductBuffer>.Serialize(this.m_cargo, writer);
      writer.WriteInt(this.m_cargoExportPriority);
      Lyst<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>.Serialize(this.m_loanPaymentBuffers, writer);
      writer.WriteGeneric<TradeDockProto>(this.m_proto);
      TradeDock.StoredCargoPriorityProvider.Serialize(this.m_storedCargoPrioProvider, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      ReservedOceanAreaState.Serialize(this.ReservedOceanAreaState, writer);
      writer.WriteGeneric<IProtoWithReservedOcean>(this.ReservedOceanProto);
    }

    public static TradeDock Deserialize(BlobReader reader)
    {
      TradeDock tradeDock;
      if (reader.TryStartClassDeserialization<TradeDock>(out tradeDock))
        reader.EnqueueDataDeserialization((object) tradeDock, TradeDock.s_deserializeDataDelayedAction);
      return tradeDock;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.HasHighCargoUnloadPrio = reader.ReadBool();
      reader.SetField<TradeDock>(this, "m_cargo", (object) Dict<ProductProto, ProductBuffer>.Deserialize(reader));
      this.m_cargoExportPriority = reader.ReadInt();
      reader.SetField<TradeDock>(this, "m_loanPaymentBuffers", reader.LoadedSaveVersion >= 140 ? (object) Lyst<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>.Deserialize(reader) : (object) new Lyst<KeyValuePair<ActiveLoan, TradeDock.LoanPaymentBuffer>>());
      this.m_proto = reader.ReadGenericAs<TradeDockProto>();
      reader.SetField<TradeDock>(this, "m_storedCargoPrioProvider", (object) TradeDock.StoredCargoPriorityProvider.Deserialize(reader));
      reader.SetField<TradeDock>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      this.ReservedOceanAreaState = ReservedOceanAreaState.Deserialize(reader);
      this.ReservedOceanProto = reader.ReadGenericAs<IProtoWithReservedOcean>();
      reader.RegisterInitAfterLoad<TradeDock>(this, "initSelf", InitPriority.Normal);
    }

    static TradeDock()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TradeDock.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      TradeDock.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class StoredCargoPriorityProvider : IOutputBufferPriorityProvider
    {
      private readonly TradeDock m_dock;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public StoredCargoPriorityProvider(TradeDock dock)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_dock = dock;
      }

      public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
      {
        return !this.m_dock.HasHighCargoUnloadPrio ? BufferStrategy.NoQuantityPreference(14) : new BufferStrategy(this.m_dock.m_cargoExportPriority, new Quantity?(request.Buffer.Quantity - request.PendingQuantity));
      }

      public static void Serialize(TradeDock.StoredCargoPriorityProvider value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TradeDock.StoredCargoPriorityProvider>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TradeDock.StoredCargoPriorityProvider.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        TradeDock.Serialize(this.m_dock, writer);
      }

      public static TradeDock.StoredCargoPriorityProvider Deserialize(BlobReader reader)
      {
        TradeDock.StoredCargoPriorityProvider priorityProvider;
        if (reader.TryStartClassDeserialization<TradeDock.StoredCargoPriorityProvider>(out priorityProvider))
          reader.EnqueueDataDeserialization((object) priorityProvider, TradeDock.StoredCargoPriorityProvider.s_deserializeDataDelayedAction);
        return priorityProvider;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<TradeDock.StoredCargoPriorityProvider>(this, "m_dock", (object) TradeDock.Deserialize(reader));
      }

      static StoredCargoPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TradeDock.StoredCargoPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TradeDock.StoredCargoPriorityProvider) obj).SerializeData(writer));
        TradeDock.StoredCargoPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TradeDock.StoredCargoPriorityProvider) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public class LoanPaymentBuffer : ProductBuffer, IInputBufferPriorityProvider
    {
      public readonly ActiveLoan Loan;
      private readonly IVehicleBuffersRegistry m_buffersRegistry;
      private readonly TradeDock m_dock;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public bool IsOpenForDelivery { get; private set; }

      public LoanPaymentBuffer(
        IVehicleBuffersRegistry buffersRegistry,
        TradeDock dock,
        ActiveLoan loan)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(loan.AnnualPayment, loan.Product);
        this.m_buffersRegistry = buffersRegistry;
        this.m_dock = dock;
        this.Loan = loan;
      }

      public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
      {
        return new BufferStrategy(this.Loan.BufferPriority, new Quantity?(buffer.UsableCapacity - pendingQuantity));
      }

      public void OpenBuffer()
      {
        if (this.IsOpenForDelivery)
          return;
        this.IsOpenForDelivery = true;
        this.m_buffersRegistry.RegisterInputBufferAndAssert((IStaticEntity) this.m_dock, (IProductBuffer) this, (IInputBufferPriorityProvider) this);
      }

      public void CloseBuffer()
      {
        if (!this.IsOpenForDelivery)
          return;
        this.IsOpenForDelivery = false;
        this.m_buffersRegistry.UnregisterInputBufferAndAssert((IProductBuffer) this, true);
      }

      public RelGameDate TimeUntilBufferOpens()
      {
        return this.IsOpenForDelivery ? RelGameDate.Zero : this.Loan.NextPaymentDate - this.m_dock.Context.Calendar.CurrentDate - LoansManager.PAYMENT_BUFFER_OPENS_BEFORE;
      }

      public override void Destroy()
      {
        this.CloseBuffer();
        base.Destroy();
      }

      public static void Serialize(TradeDock.LoanPaymentBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TradeDock.LoanPaymentBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TradeDock.LoanPaymentBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        writer.WriteBool(this.IsOpenForDelivery);
        ActiveLoan.Serialize(this.Loan, writer);
        writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_buffersRegistry);
        TradeDock.Serialize(this.m_dock, writer);
      }

      public static TradeDock.LoanPaymentBuffer Deserialize(BlobReader reader)
      {
        TradeDock.LoanPaymentBuffer loanPaymentBuffer;
        if (reader.TryStartClassDeserialization<TradeDock.LoanPaymentBuffer>(out loanPaymentBuffer))
          reader.EnqueueDataDeserialization((object) loanPaymentBuffer, TradeDock.LoanPaymentBuffer.s_deserializeDataDelayedAction);
        return loanPaymentBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        this.IsOpenForDelivery = reader.ReadBool();
        reader.SetField<TradeDock.LoanPaymentBuffer>(this, "Loan", (object) ActiveLoan.Deserialize(reader));
        reader.SetField<TradeDock.LoanPaymentBuffer>(this, "m_buffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
        reader.SetField<TradeDock.LoanPaymentBuffer>(this, "m_dock", (object) TradeDock.Deserialize(reader));
      }

      static LoanPaymentBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TradeDock.LoanPaymentBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        TradeDock.LoanPaymentBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
