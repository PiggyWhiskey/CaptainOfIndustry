// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SolarElectricityGenerator
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
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  [GenerateSerializer(false, null, 0)]
  public sealed class SolarElectricityGenerator : 
    LayoutEntityBase,
    ISolarPanelEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IMaintainedEntity,
    IEntityWithGeneralPriority,
    IUpgradableEntity,
    IEntityWithGeneralPriorityFriend
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private SolarElectricityGeneratorProto m_proto;

    public static void Serialize(SolarElectricityGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SolarElectricityGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SolarElectricityGenerator.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.GeneralPriority);
      writer.WriteGeneric<SolarElectricityGeneratorProto>(this.m_proto);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static SolarElectricityGenerator Deserialize(BlobReader reader)
    {
      SolarElectricityGenerator electricityGenerator;
      if (reader.TryStartClassDeserialization<SolarElectricityGenerator>(out electricityGenerator))
        reader.EnqueueDataDeserialization((object) electricityGenerator, SolarElectricityGenerator.s_deserializeDataDelayedAction);
      return electricityGenerator;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.GeneralPriority = reader.ReadInt();
      this.m_proto = reader.ReadGenericAs<SolarElectricityGeneratorProto>();
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    bool IMaintainedEntity.IsIdleForMaintenance => false;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    public int GeneralPriority { get; private set; }

    public bool IsCargoAffectedByGeneralPriority => false;

    public bool IsGeneralPriorityVisible => this.IsPriorityVisibleByDefault();

    void IEntityWithGeneralPriorityFriend.SetGeneralPriorityInternal(int priority)
    {
      this.GeneralPriority = priority;
      this.NotifyOnGeneralPriorityChange();
    }

    public Electricity MaxOutputElectricity => this.Prototype.OutputElectricity;

    [DoNotSave(0, null)]
    public SolarElectricityGeneratorProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => false;

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public IUpgrader Upgrader { get; private set; }

    public SolarElectricityGenerator(
      EntityId id,
      SolarElectricityGeneratorProto proto,
      TileTransform transform,
      EntityContext context,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_proto = proto;
      this.Upgrader = upgraderFactory.CreateInstance<SolarElectricityGeneratorProto, SolarElectricityGenerator>(this, proto);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    static SolarElectricityGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      SolarElectricityGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      SolarElectricityGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
