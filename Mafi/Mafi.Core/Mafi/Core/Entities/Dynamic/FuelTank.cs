// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.FuelTank
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Environment;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  /// <summary>Represents a fuel tank of a vehicle.</summary>
  /// <remarks>
  /// The amount of fuel in the tank is represented as a number of ticks that a vehicle can run on it. Methods
  /// accepting product only accept products allowed in the tank.
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public class FuelTank : IFuelTankReadonly
  {
    private readonly DrivingEntityProto m_drivingEntityProto;
    [NewInSaveVersion(140, null, null, typeof (AirPollutionManager), null)]
    private readonly AirPollutionManager m_airPollutionManager;
    [NewInSaveVersion(140, null, null, typeof (IProductsManager), null)]
    private readonly IProductsManager m_productsManager;
    /// <summary>
    /// Current state of fuel in the tank represented as number of ticks a vehicle can run on it.
    /// </summary>
    private Duration m_remainingFuelDuration;
    /// <summary>
    /// How many percent of a single tick have been consumed. Vehicle consumption in percent is added here, if this
    /// value exceeds 100%, one tick of fuel is consumed. Allows to consume less fuel than one tick (used when a
    /// vehicle is idle).
    /// </summary>
    private Percent m_partialFuelConsumptionBuffer;
    private readonly IProperty<Percent> m_fuelConsumptionMultiplier;
    private readonly IProperty<bool> m_fuelConsumptionDisabled;
    [DoNotSave(0, null)]
    private Duration m_oneQuantityDuration;
    [NewInSaveVersion(140, null, null, null, null)]
    private Duration m_pollutionDuration;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FuelTank.TankInfo Info
    {
      get => new FuelTank.TankInfo(this.Proto, this.m_remainingFuelDuration);
    }

    public Duration RemainingDuration => this.m_remainingFuelDuration;

    public FuelTankProto Proto => this.m_drivingEntityProto.FuelTankProto.Value;

    public bool IsEmpty => this.m_remainingFuelDuration.IsNotPositive;

    public bool IsUnderReserve => this.m_remainingFuelDuration < this.Proto.ReserveDuration;

    public FuelTank(
      DrivingEntityProto drivingEntityProto,
      IPropertiesDb propsDb,
      AirPollutionManager airPollutionManager,
      IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<DrivingEntityProto>(drivingEntityProto).IsNotNull<DrivingEntityProto>();
      Assert.That<Option<FuelTankProto>>(drivingEntityProto.FuelTankProto).HasValue<FuelTankProto>();
      this.m_drivingEntityProto = drivingEntityProto;
      this.m_airPollutionManager = airPollutionManager;
      this.m_productsManager = productsManager;
      this.m_remainingFuelDuration = Duration.Zero;
      this.m_fuelConsumptionMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.VehiclesFuelConsumptionMultiplier);
      this.m_fuelConsumptionDisabled = propsDb.GetProperty<bool>(IdsCore.PropertyIds.FuelConsumptionDisabled);
      this.m_oneQuantityDuration = this.Proto.QuantityToDuration(1.Quantity());
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_oneQuantityDuration = this.Proto.QuantityToDuration(1.Quantity());
    }

    /// <summary>
    /// Lowers amount of fuel in the tank by the given duration.
    /// </summary>
    private void consumeFuel(Duration duration)
    {
      if (this.m_remainingFuelDuration.IsNotPositive)
        return;
      Percent pollutionPercent = this.Proto.PollutionPercent;
      if (pollutionPercent.IsPositive)
        this.m_pollutionDuration += duration;
      if (this.m_pollutionDuration >= this.m_oneQuantityDuration)
      {
        this.m_pollutionDuration -= this.m_oneQuantityDuration;
        this.m_airPollutionManager.EmitVehiclePollution(PartialQuantity.One.ScaledBy(pollutionPercent));
      }
      if (this.m_remainingFuelDuration <= duration)
        this.m_remainingFuelDuration = Duration.Zero;
      else
        this.m_remainingFuelDuration -= duration;
      Assert.That<Duration>(this.m_remainingFuelDuration).IsNotNegative();
    }

    /// <summary>
    /// Expected to be called during vehicle's update. Lowers amount of fuel in the tank according to <paramref name="consumption" />.
    /// </summary>
    public void ConsumeFuelPerUpdate(VehicleFuelConsumption consumption)
    {
      if (this.m_fuelConsumptionDisabled.Value)
      {
        if (!this.IsUnderReserve)
          return;
        this.FillWithFirstProduct();
      }
      else
      {
        switch (consumption)
        {
          case VehicleFuelConsumption.Idle:
            this.m_partialFuelConsumptionBuffer += this.m_fuelConsumptionMultiplier.Value.Apply(this.Proto.IdleFuelConsumption);
            break;
          case VehicleFuelConsumption.Full:
            this.m_partialFuelConsumptionBuffer += this.m_fuelConsumptionMultiplier.Value;
            break;
        }
        if (this.m_partialFuelConsumptionBuffer >= Percent.Hundred)
        {
          Duration duration = this.m_partialFuelConsumptionBuffer.IntegerPart.Ticks();
          Assert.That<Duration>(duration).IsPositive();
          this.consumeFuel(duration);
          this.m_partialFuelConsumptionBuffer = this.m_partialFuelConsumptionBuffer.FractionalPart;
        }
        Assert.That<Percent>(this.m_partialFuelConsumptionBuffer).IsLess(Percent.Hundred);
      }
    }

    /// <returns>Excessive fuel that we could not add.</returns>
    public Quantity AddFuelAsMuchAs(ProductQuantity amount)
    {
      Quantity q = this.GetFreeCapacity().Min(amount.Quantity);
      if (q.IsPositive)
      {
        this.m_remainingFuelDuration += this.Proto.QuantityToDuration(q);
        Assert.That<Duration>(this.m_remainingFuelDuration).IsPositive();
      }
      return amount.Quantity - q;
    }

    [Pure]
    public Quantity GetFreeCapacity()
    {
      return this.m_remainingFuelDuration < this.Proto.Duration ? this.Proto.DurationToQuantity(this.Proto.Duration - this.m_remainingFuelDuration) : Quantity.Zero;
    }

    public void FillWithFirstProduct()
    {
      this.AddFuelAsMuchAs(new ProductQuantity(this.Proto.Product, this.GetFreeCapacity()));
    }

    public static void Serialize(FuelTank value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FuelTank>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FuelTank.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      AirPollutionManager.Serialize(this.m_airPollutionManager, writer);
      writer.WriteGeneric<DrivingEntityProto>(this.m_drivingEntityProto);
      writer.WriteGeneric<IProperty<bool>>(this.m_fuelConsumptionDisabled);
      writer.WriteGeneric<IProperty<Percent>>(this.m_fuelConsumptionMultiplier);
      Percent.Serialize(this.m_partialFuelConsumptionBuffer, writer);
      Duration.Serialize(this.m_pollutionDuration, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Duration.Serialize(this.m_remainingFuelDuration, writer);
    }

    public static FuelTank Deserialize(BlobReader reader)
    {
      FuelTank fuelTank;
      if (reader.TryStartClassDeserialization<FuelTank>(out fuelTank))
        reader.EnqueueDataDeserialization((object) fuelTank, FuelTank.s_deserializeDataDelayedAction);
      return fuelTank;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<FuelTank>(this, "m_airPollutionManager", reader.LoadedSaveVersion >= 140 ? (object) AirPollutionManager.Deserialize(reader) : (object) (AirPollutionManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<FuelTank>(this, "m_airPollutionManager", typeof (AirPollutionManager), true);
      reader.SetField<FuelTank>(this, "m_drivingEntityProto", (object) reader.ReadGenericAs<DrivingEntityProto>());
      reader.SetField<FuelTank>(this, "m_fuelConsumptionDisabled", (object) reader.ReadGenericAs<IProperty<bool>>());
      reader.SetField<FuelTank>(this, "m_fuelConsumptionMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.m_partialFuelConsumptionBuffer = Percent.Deserialize(reader);
      this.m_pollutionDuration = reader.LoadedSaveVersion >= 140 ? Duration.Deserialize(reader) : new Duration();
      reader.SetField<FuelTank>(this, "m_productsManager", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProductsManager>() : (object) (IProductsManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<FuelTank>(this, "m_productsManager", typeof (IProductsManager), true);
      this.m_remainingFuelDuration = Duration.Deserialize(reader);
      reader.RegisterInitAfterLoad<FuelTank>(this, "initSelf", InitPriority.Normal);
    }

    static FuelTank()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FuelTank.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FuelTank) obj).SerializeData(writer));
      FuelTank.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FuelTank) obj).DeserializeData(reader));
    }

    public readonly struct TankInfo
    {
      public static readonly FuelTank.TankInfo NONE;
      public readonly ProductProto Product;
      public readonly Quantity Capacity;
      public readonly Quantity Quantity;
      public readonly Percent PercentFull;

      public bool IsNotNone => this.Product.IsNotPhantom;

      private TankInfo(ProductProto product)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Product = product;
        this.Capacity = Quantity.Zero;
        this.Quantity = Quantity.Zero;
        this.PercentFull = Percent.Zero;
      }

      public TankInfo(FuelTankProto tankProto, Duration remainingFuelDuration)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Product = tankProto.Product;
        this.Capacity = tankProto.Capacity;
        this.Quantity = new Quantity((remainingFuelDuration.Ticks * this.Capacity.Value / tankProto.Duration.Ticks).Clamp(0, this.Capacity.Value));
        this.PercentFull = Percent.FromRatio(remainingFuelDuration.Ticks, tankProto.Duration.Ticks);
      }

      static TankInfo()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        FuelTank.TankInfo.NONE = new FuelTank.TankInfo(ProductProto.Phantom);
      }
    }
  }
}
