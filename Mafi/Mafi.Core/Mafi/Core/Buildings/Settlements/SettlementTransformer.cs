// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementTransformer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GenerateSerializer(false, null, 0)]
  public class SettlementTransformer : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    ISettlementServiceModule,
    IElectricityConsumingEntity,
    IMaintainedEntity,
    IEntityWithSimUpdate
  {
    public readonly SettlementTransformerProto Prototype;
    private readonly IElectricityConsumer m_electricityConsumer;
    private PopNeed m_popNeed;
    [DoNotSave(0, null)]
    private Percent m_consumptionMult;
    private int m_satisfiedTicksCounter;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsIdleForMaintenance => false;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public PopNeedProto ProvidedNeed => this.Prototype.PopsNeed;

    public Settlement Settlement { get; private set; }

    public Electricity PowerRequired { get; private set; }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    public SettlementTransformer(
      EntityId id,
      SettlementTransformerProto proto,
      TileTransform transform,
      EntityContext context,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.updateProperties();
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void updateProperties()
    {
      this.m_consumptionMult = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<Percent>((IEntity) this, IdsCore.PropertyIds.SettlementConsumptionMultiplier);
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    /// <summary>
    /// Returns the amount of pops it did not manage to satisfy.
    /// </summary>
    public int TrySatisfyNeedOnNewDay(int popsToSatisfy)
    {
      if (popsToSatisfy <= 0)
      {
        this.PowerRequired = Electricity.Zero;
        this.m_electricityConsumer.OnPowerRequiredChanged();
        return popsToSatisfy;
      }
      if (this.m_satisfiedTicksCounter > 0)
        return popsToSatisfy;
      this.PowerRequired = Electricity.FromKw(this.Prototype.GetPowerRequired(popsToSatisfy, this.m_consumptionMult).ToIntRounded()).ScaledBy(this.m_popNeed.ConsumptionMultiplier).Max(1.Kw());
      this.m_electricityConsumer.OnPowerRequiredChanged();
      this.m_satisfiedTicksCounter = 1.Days().Ticks;
      return 0;
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.IsNotEnabled() || Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return;
      if (this.PowerRequired.IsPositive)
      {
        if (this.m_satisfiedTicksCounter > 0)
        {
          if (!this.m_electricityConsumer.TryConsume())
            return;
          --this.m_satisfiedTicksCounter;
        }
        else
          this.PowerRequired = Electricity.Zero;
      }
      else
        this.m_satisfiedTicksCounter = 0;
    }

    public Fix32 GetMonthsOfSupply() => (Fix32) 0;

    public void SetSettlement(Settlement settlement)
    {
      Assert.That<Settlement>(this.Settlement).IsNull<Settlement>();
      this.Settlement = settlement;
      this.m_popNeed = this.Settlement.AllNeeds.Single((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) this.Prototype.PopsNeed));
    }

    public void ReplaceSettlement(Settlement settlement)
    {
      Assert.That<Settlement>(this.Settlement).IsNotNull<Settlement>();
      this.Settlement = settlement;
      this.m_popNeed = this.Settlement.AllNeeds.Single((Func<PopNeed, bool>) (x => (Proto) x.Proto == (Proto) this.Prototype.PopsNeed));
    }

    public static void Serialize(SettlementTransformer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementTransformer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementTransformer.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      PopNeed.Serialize(this.m_popNeed, writer);
      writer.WriteInt(this.m_satisfiedTicksCounter);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      Electricity.Serialize(this.PowerRequired, writer);
      writer.WriteGeneric<SettlementTransformerProto>(this.Prototype);
      Settlement.Serialize(this.Settlement, writer);
    }

    public static SettlementTransformer Deserialize(BlobReader reader)
    {
      SettlementTransformer settlementTransformer;
      if (reader.TryStartClassDeserialization<SettlementTransformer>(out settlementTransformer))
        reader.EnqueueDataDeserialization((object) settlementTransformer, SettlementTransformer.s_deserializeDataDelayedAction);
      return settlementTransformer;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SettlementTransformer>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_popNeed = PopNeed.Deserialize(reader);
      this.m_satisfiedTicksCounter = reader.ReadInt();
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.PowerRequired = Electricity.Deserialize(reader);
      reader.SetField<SettlementTransformer>(this, "Prototype", (object) reader.ReadGenericAs<SettlementTransformerProto>());
      this.Settlement = Settlement.Deserialize(reader);
      reader.RegisterInitAfterLoad<SettlementTransformer>(this, "updateProperties", InitPriority.Normal);
    }

    static SettlementTransformer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementTransformer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      SettlementTransformer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
