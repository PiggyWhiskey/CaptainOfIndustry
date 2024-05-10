// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.TombOfCaptains
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  [GenerateSerializer(false, null, 0)]
  public class TombOfCaptains : 
    LayoutEntity,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IMaintainedEntity,
    IEntityWithGeneralPriority,
    IEntityWithPorts
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Quantity FUEL_BUFFER_CAPACITY;
    private static readonly Quantity FLOWERS_BUFFER_CAPACITY;
    private static readonly Percent FIRE_INC_PER_DAY;
    private static readonly Percent FLOWERS_INC_PER_DAY;
    private TombOfCaptainsProto m_proto;
    private Option<ProductBuffer> m_fuelBuffer;
    private PartialQuantity m_partialFuelLeft;
    private Percent m_firePercSum;
    private Option<ProductBuffer> m_flowersBuffer;
    private PartialQuantity m_partialFlowersLeft;
    private Percent m_flowersPercSum;
    private readonly IProductsManager m_productsManager;
    private readonly IUpointsManager m_upointsManager;
    private readonly ICalendar m_calendar;

    public static void Serialize(TombOfCaptains value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TombOfCaptains>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TombOfCaptains.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.FireBurningPerc, writer);
      Percent.Serialize(this.FlowersPerc, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      Percent.Serialize(this.m_firePercSum, writer);
      Option<ProductBuffer>.Serialize(this.m_flowersBuffer, writer);
      Percent.Serialize(this.m_flowersPercSum, writer);
      Option<ProductBuffer>.Serialize(this.m_fuelBuffer, writer);
      PartialQuantity.Serialize(this.m_partialFlowersLeft, writer);
      PartialQuantity.Serialize(this.m_partialFuelLeft, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<TombOfCaptainsProto>(this.m_proto);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      Upoints.Serialize(this.UnityDeltaLastMonth, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static TombOfCaptains Deserialize(BlobReader reader)
    {
      TombOfCaptains tombOfCaptains;
      if (reader.TryStartClassDeserialization<TombOfCaptains>(out tombOfCaptains))
        reader.EnqueueDataDeserialization((object) tombOfCaptains, TombOfCaptains.s_deserializeDataDelayedAction);
      return tombOfCaptains;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.FireBurningPerc = Percent.Deserialize(reader);
      this.FlowersPerc = Percent.Deserialize(reader);
      reader.SetField<TombOfCaptains>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      this.m_firePercSum = Percent.Deserialize(reader);
      this.m_flowersBuffer = Option<ProductBuffer>.Deserialize(reader);
      this.m_flowersPercSum = Percent.Deserialize(reader);
      this.m_fuelBuffer = Option<ProductBuffer>.Deserialize(reader);
      this.m_partialFlowersLeft = PartialQuantity.Deserialize(reader);
      this.m_partialFuelLeft = PartialQuantity.Deserialize(reader);
      reader.SetField<TombOfCaptains>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<TombOfCaptainsProto>();
      reader.SetField<TombOfCaptains>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.UnityDeltaLastMonth = Upoints.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    [DoNotSave(0, null)]
    public TombOfCaptainsProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => false;

    public IUpgrader Upgrader { get; private set; }

    public MaintenanceCosts MaintenanceCosts => this.Prototype.Costs.Maintenance;

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public bool IsIdleForMaintenance => false;

    public Upoints UnityDeltaLastMonth { get; private set; }

    public Option<IProductBuffer> FuelBuffer
    {
      get => (Option<IProductBuffer>) (IProductBuffer) this.m_fuelBuffer.ValueOrNull;
    }

    public Percent FireBurningPerc { get; private set; }

    public Option<IProductBuffer> FlowersBuffer
    {
      get => (Option<IProductBuffer>) (IProductBuffer) this.m_flowersBuffer.ValueOrNull;
    }

    public Percent FlowersPerc { get; private set; }

    public TombOfCaptains(
      EntityId id,
      TombOfCaptainsProto proto,
      TileTransform transform,
      EntityContext context,
      ICalendar calendar,
      IProductsManager productsManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IUpointsManager upointsManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_proto = proto;
      this.m_calendar = calendar;
      this.m_productsManager = productsManager;
      this.m_upointsManager = upointsManager;
      this.Upgrader = upgraderFactory.CreateInstance<TombOfCaptainsProto, TombOfCaptains>(this, this.Prototype);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_fuelBuffer = this.Prototype.FlowersConsumptionPerDay.IsNotEmpty ? (Option<ProductBuffer>) new ProductBuffer(TombOfCaptains.FUEL_BUFFER_CAPACITY, this.Prototype.FuelConsumptionPerDay.Product) : Option<ProductBuffer>.None;
      this.m_flowersBuffer = this.Prototype.FlowersConsumptionPerDay.IsNotEmpty ? (Option<ProductBuffer>) new ProductBuffer(TombOfCaptains.FLOWERS_BUFFER_CAPACITY, this.Prototype.FlowersConsumptionPerDay.Product) : Option<ProductBuffer>.None;
      calendar.NewDay.Add<TombOfCaptains>(this, new Action(this.onNewDay));
      calendar.NewMonth.Add<TombOfCaptains>(this, new Action(this.onNewMonth));
    }

    private void onNewDay()
    {
      this.updateFire();
      this.m_firePercSum += this.FireBurningPerc;
      this.updateFlowers();
      this.m_flowersPercSum += this.FlowersPerc;
    }

    private void onNewMonth()
    {
      Upoints upoints1 = this.Prototype.MinUnityForFirePerMonth.Lerp(this.Prototype.MaxUnityForFirePerMonth, this.m_firePercSum / 30);
      this.m_firePercSum = Percent.Zero;
      Upoints upoints2 = this.Prototype.MinUnityForFlowersPerMonth.Lerp(this.Prototype.MaxUnityForFlowersPerMonth, this.m_firePercSum / 30);
      this.m_firePercSum = Percent.Zero;
      this.UnityDeltaLastMonth = upoints1 + upoints2;
      if (this.UnityDeltaLastMonth.IsPositive)
        this.m_upointsManager.GenerateUnity(IdsCore.UpointsCategories.OtherDecorations, this.UnityDeltaLastMonth);
      else
        this.m_upointsManager.TryConsume(IdsCore.UpointsCategories.OtherDecorations, this.UnityDeltaLastMonth, (Option<IEntity>) (IEntity) this);
    }

    private void updateFire()
    {
      if (this.m_fuelBuffer.IsNone)
        return;
      if (this.IsNotEnabled)
      {
        this.FireBurningPerc = Percent.Zero;
      }
      else
      {
        if (this.m_partialFuelLeft < this.Prototype.FuelConsumptionPerDay.Quantity)
        {
          if (this.m_fuelBuffer.Value.IsEmpty)
          {
            this.FireBurningPerc = Percent.Zero;
            return;
          }
          Quantity quantity = this.m_fuelBuffer.Value.RemoveAsMuchAs(2 * this.Prototype.FuelConsumptionPerDay.Quantity.ToQuantityCeiled());
          this.m_partialFuelLeft += quantity.AsPartial;
          this.m_productsManager.ProductDestroyed(this.m_fuelBuffer.Value.Product, quantity, DestroyReason.General);
          if (this.m_partialFuelLeft < this.Prototype.FuelConsumptionPerDay.Quantity)
          {
            this.FireBurningPerc = Percent.Zero;
            return;
          }
        }
        this.m_partialFuelLeft -= this.Prototype.FuelConsumptionPerDay.Quantity;
        Assert.That<PartialQuantity>(this.m_partialFuelLeft).IsNotNegative();
        this.FireBurningPerc = (this.FireBurningPerc + TombOfCaptains.FIRE_INC_PER_DAY).Min(Percent.Hundred);
      }
    }

    private void updateFlowers()
    {
      if (this.m_flowersBuffer.IsNone)
        return;
      if (this.IsNotEnabled)
      {
        if (!this.FlowersPerc.IsPositive)
          return;
        updateNotEnoughFlowers();
      }
      else
      {
        if (this.m_partialFlowersLeft < this.Prototype.FlowersConsumptionPerDay.Quantity)
        {
          if (this.m_flowersBuffer.Value.IsEmpty)
          {
            updateNotEnoughFlowers();
            return;
          }
          Quantity quantity = this.m_flowersBuffer.Value.RemoveAsMuchAs(2 * this.Prototype.FlowersConsumptionPerDay.Quantity.ToQuantityCeiled());
          this.m_partialFlowersLeft += quantity.AsPartial;
          this.m_productsManager.ProductDestroyed(this.m_flowersBuffer.Value.Product, quantity, DestroyReason.General);
          if (this.m_partialFlowersLeft < this.Prototype.FlowersConsumptionPerDay.Quantity)
          {
            updateNotEnoughFlowers();
            return;
          }
        }
        this.m_partialFlowersLeft -= this.Prototype.FlowersConsumptionPerDay.Quantity;
        Assert.That<PartialQuantity>(this.m_partialFlowersLeft).IsNotNegative();
        if (!(this.FlowersPerc < Percent.Hundred))
          return;
        this.FlowersPerc = (this.FlowersPerc + TombOfCaptains.FLOWERS_INC_PER_DAY).Min(Percent.Hundred);
      }

      void updateNotEnoughFlowers()
      {
        this.FlowersPerc = (this.FlowersPerc - 5 * TombOfCaptains.FLOWERS_INC_PER_DAY).Max(Percent.Zero);
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.m_fuelBuffer.HasValue && (Proto) this.m_fuelBuffer.Value.Product == (Proto) pq.Product)
        return this.m_fuelBuffer.Value.StoreAsMuchAs(pq);
      return this.m_flowersBuffer.HasValue && (Proto) this.m_flowersBuffer.Value.Product == (Proto) pq.Product ? this.m_flowersBuffer.Value.StoreAsMuchAs(pq) : pq.Quantity;
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public void UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Error("Upgrade not available!");
      }
      else
      {
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
        this.FireBurningPerc = Percent.Zero;
        this.FlowersPerc = Percent.Zero;
        if (this.Prototype.FlowersConsumptionPerDay.IsNotEmpty)
        {
          if (this.m_fuelBuffer.IsNone)
            this.m_fuelBuffer = (Option<ProductBuffer>) new ProductBuffer(TombOfCaptains.FUEL_BUFFER_CAPACITY, this.Prototype.FuelConsumptionPerDay.Product);
          else
            Assert.That<ProductProto>(this.m_fuelBuffer.Value.Product).IsEqualTo<ProductProto>(this.Prototype.FuelConsumptionPerDay.Product);
        }
        else
          Assert.That<Option<ProductBuffer>>(this.m_fuelBuffer).IsNone<ProductBuffer>("Buffer removal is not supported.");
        if (this.Prototype.FlowersConsumptionPerDay.IsNotEmpty)
        {
          if (this.m_flowersBuffer.IsNone)
            this.m_flowersBuffer = (Option<ProductBuffer>) new ProductBuffer(TombOfCaptains.FLOWERS_BUFFER_CAPACITY, this.Prototype.FlowersConsumptionPerDay.Product);
          else
            Assert.That<ProductProto>(this.m_flowersBuffer.Value.Product).IsEqualTo<ProductProto>(this.Prototype.FlowersConsumptionPerDay.Product);
        }
        else
          Assert.That<Option<ProductBuffer>>(this.m_flowersBuffer).IsNone<ProductBuffer>("Buffer removal is not supported.");
      }
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewDay.Remove<TombOfCaptains>(this, new Action(this.onNewDay));
      this.m_calendar.NewMonth.Remove<TombOfCaptains>(this, new Action(this.onNewMonth));
      base.OnDestroy();
    }

    static TombOfCaptains()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TombOfCaptains.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      TombOfCaptains.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      TombOfCaptains.FUEL_BUFFER_CAPACITY = 10.Quantity();
      TombOfCaptains.FLOWERS_BUFFER_CAPACITY = 10.Quantity();
      TombOfCaptains.FIRE_INC_PER_DAY = 10.Percent();
      TombOfCaptains.FLOWERS_INC_PER_DAY = 1.Percent();
    }
  }
}
