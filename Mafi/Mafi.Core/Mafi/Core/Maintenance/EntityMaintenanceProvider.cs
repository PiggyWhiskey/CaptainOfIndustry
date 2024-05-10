// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.EntityMaintenanceProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Maintenance
{
  /// <summary>
  /// Handles communication between <see cref="T:Mafi.Core.Maintenance.MaintenanceManager" /> and <see cref="T:Mafi.Core.Maintenance.IMaintainedEntity" />.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class EntityMaintenanceProvider : 
    IEntityMaintenanceProvider,
    IEventOwner,
    IComparable<EntityMaintenanceProvider>,
    IEntityObserverForPriority,
    IEntityObserver,
    IEntityObserverForUpgrade
  {
    public readonly IMaintainedEntity Entity;
    private readonly MaintenanceManager m_maintenanceManager;
    private EntityNotificator m_isBrokenNotif;
    private bool m_isBroken;
    private Option<IProperty<Percent>> m_extraMultiplierProperty;
    [NewInSaveVersion(140, null, "Percent.Hundred", null, null)]
    private Percent m_dynamicExtraMultiplier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Unique id of consumer Proto for fast collection of electricity stats.
    /// Set by MaintenanceManager.
    /// </summary>
    public int ProtoToken { get; set; }

    public MaintenanceCosts Costs { get; private set; }

    public MaintenanceStatus Status { get; private set; }

    public bool IsDestroyed => this.Entity.IsDestroyed;

    public int Priority => this.Entity.GeneralPriority;

    public Upoints RepairCost
    {
      get
      {
        return this.m_maintenanceManager.GetQuickRepairCost(this.Costs.Product, this.Status.MissingPointsToFull);
      }
    }

    public EntityMaintenanceProvider(
      IMaintainedEntity entity,
      MaintenanceManager maintenanceManager,
      INotificationsManager notifManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_dynamicExtraMultiplier = Percent.Hundred;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Entity = entity;
      this.m_maintenanceManager = maintenanceManager;
      this.updateCost();
      this.m_isBrokenNotif = entity is IStaticEntity ? notifManager.CreateNotificatorFor(IdsCore.Notifications.MachineIsBroken) : notifManager.CreateNotificatorFor(IdsCore.Notifications.VehicleIsBroken);
      this.Status = this.Costs.MaintenancePerMonth.IsPositive ? this.m_maintenanceManager.AddNewEntityHandler(this) : EntityMaintenanceProvider.createEmptyStatus();
      entity.AddObserver((IEntityObserver) this);
    }

    public bool CanWork() => !this.Status.IsBroken || this.m_maintenanceManager.CanSlowDownIfBroken;

    public bool ShouldSlowDown()
    {
      return this.Status.IsBroken && this.m_maintenanceManager.CanSlowDownIfBroken;
    }

    public void RefreshMaintenanceCost()
    {
      Percent? percentMaintained = new Percent?();
      if (this.Costs.MaintenancePerMonth.IsPositive)
      {
        percentMaintained = new Percent?(Percent.FromRatio(this.Status.MaintenancePointsCurrent.Value, this.Status.MaintenancePointsMax.Value));
        this.m_maintenanceManager.RemoveEntityHandler(this);
      }
      this.updateCost();
      this.Status = this.Costs.MaintenancePerMonth.IsPositive ? this.m_maintenanceManager.AddNewEntityHandler(this, percentMaintained) : EntityMaintenanceProvider.createEmptyStatus();
    }

    void IEntityObserverForUpgrade.OnEntityUpgraded(
      IEntity upgradedEntity,
      IEntityProto previousProto)
    {
      Assert.That<IEntity>(upgradedEntity).IsEqualTo<IEntity>((IEntity) this.Entity);
      this.RefreshMaintenanceCost();
    }

    void IEntityObserverForPriority.OnGeneralPriorityChange(IEntity entity)
    {
      if (this.Costs.MaintenancePerMonth.IsNotPositive)
        return;
      this.m_maintenanceManager.UpdatePriorityFor(this);
    }

    public void OnCostModifierChanged() => this.updateCost();

    private void onExtraMultiplierChange(Percent newVal) => this.updateCost();

    private void updateCost()
    {
      if (this.Entity.MaintenanceCosts.MaintenancePerMonth.IsNotPositive)
      {
        this.Costs = this.Entity.MaintenanceCosts;
      }
      else
      {
        IProperty<Percent> valueOrNull = this.m_extraMultiplierProperty.ValueOrNull;
        Percent scale = valueOrNull != null ? valueOrNull.Value : Percent.Hundred;
        VirtualProductProto product = this.Entity.MaintenanceCosts.Product;
        PartialQuantity partialQuantity = this.Entity.MaintenanceCosts.MaintenancePerMonth.ScaledBy(scale);
        partialQuantity = partialQuantity.ScaledBy(this.m_dynamicExtraMultiplier);
        PartialQuantity maintenancePerMonth1 = partialQuantity.ScaledBy(this.m_maintenanceManager.ConsumptionMultiplier);
        PartialQuantity maintenancePerMonth2 = this.Entity.MaintenanceCosts.MaintenancePerMonth;
        Percent? initialMaintenanceBoost = new Percent?(this.Entity.MaintenanceCosts.InitialMaintenanceBoost);
        this.Costs = new MaintenanceCosts(product, maintenancePerMonth1, maintenancePerMonth2, initialMaintenanceBoost);
      }
    }

    public void SetMaintenanceStatus(MaintenanceStatus newStatus)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse("Setting maintenance on destroyed entity.");
      this.Status = newStatus;
      this.m_isBrokenNotif.NotifyIff(this.Status.IsBroken, (IEntity) this.Entity);
      if (this.m_isBroken == this.Status.IsBroken)
        return;
      this.m_isBroken = this.Status.IsBroken;
      this.Entity.UpdateIsBroken();
    }

    public void SetCurrentMaintenanceTo(Percent percent)
    {
      this.SetMaintenanceStatus(new MaintenanceStatus(this.Status.IsBroken && percent < Percent.Hundred, this.Status.MaintenancePointsMax.ScaledBy(percent), this.Status.MaintenancePointsMax, percent < Percent.Hundred ? this.Status.CurrentBreakdownChance : Percent.Zero, percent < Percent.Hundred ? this.Status.BrokenDurationDays : (Fix32) 0));
    }

    public void SetExtraMultiplierProperty(IProperty<Percent> property)
    {
      Assert.That<Option<IProperty<Percent>>>(this.m_extraMultiplierProperty).IsNone<IProperty<Percent>>();
      this.m_extraMultiplierProperty = property.SomeOption<IProperty<Percent>>();
      property.OnChange.Add<EntityMaintenanceProvider>(this, new Action<Percent>(this.onExtraMultiplierChange));
      this.updateCost();
    }

    public void SetDynamicExtraMultiplier(Percent percent)
    {
      if (percent == this.m_dynamicExtraMultiplier)
        return;
      this.m_dynamicExtraMultiplier = percent;
      this.updateCost();
    }

    public void DecreaseBy(Percent percent)
    {
      this.m_maintenanceManager.ApplyExtraMaintenanceCost(this, Percent.Hundred - percent);
    }

    private static MaintenanceStatus createEmptyStatus()
    {
      return new MaintenanceStatus(false, PartialQuantity.Zero, PartialQuantity.Zero, Percent.Zero, (Fix32) 0);
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
      if (this.Costs.MaintenancePerMonth.IsPositive)
        this.m_maintenanceManager.RemoveEntityHandler(this);
      if (!this.m_extraMultiplierProperty.HasValue)
        return;
      this.m_extraMultiplierProperty.Value.OnChange.Remove<EntityMaintenanceProvider>(this, new Action<Percent>(this.onExtraMultiplierChange));
    }

    public int CompareTo(EntityMaintenanceProvider other)
    {
      return this.Priority.CompareTo(other.Priority);
    }

    public static void Serialize(EntityMaintenanceProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EntityMaintenanceProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EntityMaintenanceProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      MaintenanceCosts.Serialize(this.Costs, writer);
      writer.WriteGeneric<IMaintainedEntity>(this.Entity);
      Percent.Serialize(this.m_dynamicExtraMultiplier, writer);
      Option<IProperty<Percent>>.Serialize(this.m_extraMultiplierProperty, writer);
      writer.WriteBool(this.m_isBroken);
      EntityNotificator.Serialize(this.m_isBrokenNotif, writer);
      MaintenanceManager.Serialize(this.m_maintenanceManager, writer);
      writer.WriteInt(this.ProtoToken);
      MaintenanceStatus.Serialize(this.Status, writer);
    }

    public static EntityMaintenanceProvider Deserialize(BlobReader reader)
    {
      EntityMaintenanceProvider maintenanceProvider;
      if (reader.TryStartClassDeserialization<EntityMaintenanceProvider>(out maintenanceProvider))
        reader.EnqueueDataDeserialization((object) maintenanceProvider, EntityMaintenanceProvider.s_deserializeDataDelayedAction);
      return maintenanceProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Costs = MaintenanceCosts.Deserialize(reader);
      reader.SetField<EntityMaintenanceProvider>(this, "Entity", (object) reader.ReadGenericAs<IMaintainedEntity>());
      this.m_dynamicExtraMultiplier = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : Percent.Hundred;
      this.m_extraMultiplierProperty = Option<IProperty<Percent>>.Deserialize(reader);
      this.m_isBroken = reader.ReadBool();
      this.m_isBrokenNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<EntityMaintenanceProvider>(this, "m_maintenanceManager", (object) MaintenanceManager.Deserialize(reader));
      this.ProtoToken = reader.ReadInt();
      this.Status = MaintenanceStatus.Deserialize(reader);
    }

    static EntityMaintenanceProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityMaintenanceProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EntityMaintenanceProvider) obj).SerializeData(writer));
      EntityMaintenanceProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EntityMaintenanceProvider) obj).DeserializeData(reader));
    }
  }
}
