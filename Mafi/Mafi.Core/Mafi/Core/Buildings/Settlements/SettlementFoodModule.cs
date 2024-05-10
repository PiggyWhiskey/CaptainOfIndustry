// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementFoodModule
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class SettlementFoodModule : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithLogisticsControl,
    IEntityWithPorts,
    IEntityWithMultipleProductsToAssign,
    ILayoutEntity,
    IEntityWithSimUpdate
  {
    private SettlementFoodModuleProto m_proto;
    private readonly Lyst<Option<ProductBuffer>> m_buffersPerSlot;
    private readonly IProductsManager m_productsManager;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private EntityNotificator m_noProductAssignedNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public SettlementFoodModuleProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public Settlement Settlement { get; private set; }

    public SettlementFoodModule.State CurrentState { get; private set; }

    public bool IsOperational => this.CurrentState == SettlementFoodModule.State.Working;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IUpgrader Upgrader { get; private set; }

    public IIndexable<Option<ProductBuffer>> BuffersPerSlot
    {
      get => (IIndexable<Option<ProductBuffer>>) this.m_buffersPerSlot;
    }

    public ImmutableArray<ProductProto> SupportedProducts { get; private set; }

    public SettlementFoodModule(
      EntityId id,
      SettlementFoodModuleProto proto,
      TileTransform transform,
      EntityContext context,
      IProductsManager productsManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_buffersPerSlot = new Lyst<Option<ProductBuffer>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_productsManager = productsManager;
      this.Prototype = proto;
      this.Upgrader = upgraderFactory.CreateInstance<SettlementFoodModuleProto, SettlementFoodModule>(this, this.Prototype);
      this.SupportedProducts = context.ProtosDb.All<FoodProto>().Select<FoodProto, ProductProto>((Func<FoodProto, ProductProto>) (x => x.Product)).Distinct<ProductProto>().ToImmutableArray<ProductProto>();
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), (IOutputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), vehicleBuffersRegistry);
      for (int index = 0; index < this.Prototype.BuffersCount; ++index)
        this.m_buffersPerSlot.Add(Option<ProductBuffer>.None);
      if (proto.StayConnectedToLogisticsByDefault)
        this.m_autoLogisticsHelper.SetLogisticsInputMode(EntityLogisticsMode.On);
      this.m_noProductAssignedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoProductAssignedToEntity);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      this.m_noProductAssignedNotif.NotifyIff(this.CurrentState == SettlementFoodModule.State.NoProductAssigned, (IEntity) this);
    }

    private SettlementFoodModule.State updateState()
    {
      if (!this.IsEnabledIgnoreUpgradeAndDeconstruction)
        return SettlementFoodModule.State.Paused;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return SettlementFoodModule.State.MissingWorkers;
      bool flag1 = true;
      bool flag2 = true;
      foreach (Option<ProductBuffer> option in this.m_buffersPerSlot)
      {
        if (option.HasValue)
        {
          flag2 = false;
          if (option.Value.IsNotEmpty)
          {
            flag1 = false;
            break;
          }
        }
      }
      if (flag2)
        return SettlementFoodModule.State.NoProductAssigned;
      return flag1 ? SettlementFoodModule.State.MissingInput : SettlementFoodModule.State.Working;
    }

    public void SetSettlement(Settlement settlement)
    {
      Assert.That<Settlement>(this.Settlement).IsNull<Settlement>();
      this.Settlement = settlement;
    }

    public void ReplaceSettlement(Settlement settlement)
    {
      Assert.That<Settlement>(this.Settlement).IsNotNull<Settlement>();
      this.Settlement = settlement;
    }

    public void SetProduct(Option<ProductProto> product, int bufferSlot, bool skipIfClearing)
    {
      if (bufferSlot < 0 || bufferSlot >= this.m_buffersPerSlot.Count)
      {
        Assert.Fail(string.Format("Provided buffet slot '{0}' is out of range in {1}!", (object) bufferSlot, (object) this.Prototype.Id));
      }
      else
      {
        Option<ProductBuffer> currentBuffer = this.m_buffersPerSlot[bufferSlot];
        if (product.IsNone)
        {
          if (!currentBuffer.HasValue || skipIfClearing)
            return;
          clearProduct();
        }
        else if (!this.SupportedProducts.Contains(product.Value))
          Assert.Fail(string.Format("Product '{0}' is not supported in {1}", (object) product.Value, (object) this.Prototype.Id));
        else if (currentBuffer.IsNone)
        {
          createNewBuffer();
        }
        else
        {
          if (currentBuffer.Value.Product == product || currentBuffer.Value.Quantity.IsPositive & skipIfClearing)
            return;
          clearProduct();
          createNewBuffer();
        }

        void clearProduct()
        {
          this.m_autoLogisticsHelper.RemoveInputBuffer((IProductBuffer) currentBuffer.Value);
          this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) currentBuffer.Value);
          this.m_buffersPerSlot[bufferSlot] = (Option<ProductBuffer>) Option.None;
        }
      }

      void createNewBuffer()
      {
        GlobalInputBuffer buffer = new GlobalInputBuffer(this.Prototype.CapacityPerBuffer, product.Value, this.Context.ProductsManager, 5, (IEntity) this);
        this.m_buffersPerSlot[bufferSlot] = (Option<ProductBuffer>) (ProductBuffer) buffer;
        this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) buffer);
      }
    }

    public Option<IProductBuffer> GetBuffer(int bufferSlot)
    {
      return bufferSlot < 0 || bufferSlot >= this.m_buffersPerSlot.Count ? Option<IProductBuffer>.None : this.m_buffersPerSlot[bufferSlot].As<IProductBuffer>();
    }

    public Quantity GetCapacity(int bufferSlot) => this.Prototype.CapacityPerBuffer;

    public void Cheat_FillBuffers(ProductProto product, Percent percent = default (Percent))
    {
      this.SetProduct((Option<ProductProto>) product, 0, false);
      Quantity quantity = this.Prototype.CapacityPerBuffer.ScaledBy(percent) - this.m_buffersPerSlot[0].Value.StoreAsMuchAs(this.Prototype.CapacityPerBuffer.ScaledBy(percent));
      this.m_productsManager.ProductCreated(product, quantity, CreateReason.Cheated);
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (!this.IsEnabled)
        return pq.Quantity;
      Quantity quantity = pq.Quantity;
      foreach (Option<ProductBuffer> option in this.m_buffersPerSlot)
      {
        if (!option.IsNone)
        {
          ProductBuffer bufferReceived = option.Value;
          if ((Proto) bufferReceived.Product == (Proto) pq.Product)
          {
            quantity = bufferReceived.StoreAsMuchAs(quantity);
            this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) bufferReceived);
            if (quantity.IsNotPositive)
              break;
          }
        }
      }
      return quantity;
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    protected override void OnDestroy()
    {
      foreach (Option<ProductBuffer> option in this.m_buffersPerSlot)
      {
        if (option.HasValue)
          this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) option.Value);
      }
      base.OnDestroy();
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Error("Upgrade not available!");
      }
      else
      {
        Assert.That<int>(this.Prototype.Upgrade.NextTier.Value.BuffersCount).IsGreaterOrEqual(this.Prototype.BuffersCount);
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
        Lyst<Option<ProductBuffer>> list = new Lyst<Option<ProductBuffer>>();
        for (int index = 0; index < this.Prototype.BuffersCount; ++index)
        {
          if (index < this.m_buffersPerSlot.Count)
          {
            Option<ProductBuffer> option = this.m_buffersPerSlot[index];
            if (option.HasValue)
            {
              option.Value.IncreaseCapacityTo(this.Prototype.CapacityPerBuffer);
              list.Add(option);
              continue;
            }
          }
          list.Add(Option<ProductBuffer>.None);
        }
        this.m_buffersPerSlot.Clear();
        this.m_buffersPerSlot.AddRange(list);
      }
    }

    public bool CanDisableLogisticsInput => true;

    public bool CanDisableLogisticsOutput => false;

    public EntityLogisticsMode LogisticsInputMode => this.m_autoLogisticsHelper.LogisticsInputMode;

    public EntityLogisticsMode LogisticsOutputMode => EntityLogisticsMode.Off;

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsInputMode(mode);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsOutputMode(mode);
    }

    public static void Serialize(SettlementFoodModule value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementFoodModule>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementFoodModule.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt((int) this.CurrentState);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      Lyst<Option<ProductBuffer>>.Serialize(this.m_buffersPerSlot, writer);
      EntityNotificator.Serialize(this.m_noProductAssignedNotif, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<SettlementFoodModuleProto>(this.m_proto);
      Settlement.Serialize(this.Settlement, writer);
      ImmutableArray<ProductProto>.Serialize(this.SupportedProducts, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static SettlementFoodModule Deserialize(BlobReader reader)
    {
      SettlementFoodModule settlementFoodModule;
      if (reader.TryStartClassDeserialization<SettlementFoodModule>(out settlementFoodModule))
        reader.EnqueueDataDeserialization((object) settlementFoodModule, SettlementFoodModule.s_deserializeDataDelayedAction);
      return settlementFoodModule;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentState = (SettlementFoodModule.State) reader.ReadInt();
      reader.SetField<SettlementFoodModule>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      reader.SetField<SettlementFoodModule>(this, "m_buffersPerSlot", (object) Lyst<Option<ProductBuffer>>.Deserialize(reader));
      this.m_noProductAssignedNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<SettlementFoodModule>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<SettlementFoodModuleProto>();
      this.Settlement = Settlement.Deserialize(reader);
      this.SupportedProducts = ImmutableArray<ProductProto>.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    static SettlementFoodModule()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementFoodModule.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      SettlementFoodModule.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Broken,
      Working,
      MissingInput,
      MissingWorkers,
      NoProductAssigned,
    }
  }
}
