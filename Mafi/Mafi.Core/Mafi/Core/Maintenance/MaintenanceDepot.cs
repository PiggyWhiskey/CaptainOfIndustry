// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.MaintenanceDepot
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GenerateSerializer(false, null, 0)]
  public class MaintenanceDepot : Machine
  {
    private MaintenanceDepotProto m_proto;
    private readonly MaintenanceManager m_maintenanceManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public MaintenanceDepotProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (MachineProto) value;
      }
    }

    protected override bool UsesVirtualLimitedBuffers => true;

    public MaintenanceDepot(
      EntityId id,
      MaintenanceDepotProto proto,
      TileTransform transform,
      EntityContext context,
      VirtualBuffersMap buffersMap,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IUnityConsumerFactory unityConsumerFactory,
      UnlockedProtosDb unlockedProtosDb,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      MaintenanceManager maintenanceManager,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (MachineProto) proto, transform, context, buffersMap, upgraderFactory, unityConsumerFactory, unlockedProtosDb, vehicleBuffersRegistry, maintenanceProvidersFactory, animationStateFactory);
      this.Prototype = proto;
      this.m_maintenanceManager = maintenanceManager;
    }

    protected override void OnUpgradeDone(IEntityProto oldProto, IEntityProto newProto)
    {
      base.OnUpgradeDone(oldProto, newProto);
      this.m_proto = (MaintenanceDepotProto) newProto;
      this.m_maintenanceManager.ReportUpgradeDone(this, ((MaintenanceDepotProto) newProto).MaintenanceBufferExtraCapacity - ((MaintenanceDepotProto) oldProto).MaintenanceBufferExtraCapacity);
    }

    public static void Serialize(MaintenanceDepot value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MaintenanceDepot>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MaintenanceDepot.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      MaintenanceManager.Serialize(this.m_maintenanceManager, writer);
      writer.WriteGeneric<MaintenanceDepotProto>(this.m_proto);
    }

    public static MaintenanceDepot Deserialize(BlobReader reader)
    {
      MaintenanceDepot maintenanceDepot;
      if (reader.TryStartClassDeserialization<MaintenanceDepot>(out maintenanceDepot))
        reader.EnqueueDataDeserialization((object) maintenanceDepot, MaintenanceDepot.s_deserializeDataDelayedAction);
      return maintenanceDepot;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MaintenanceDepot>(this, "m_maintenanceManager", (object) MaintenanceManager.Deserialize(reader));
      this.m_proto = reader.ReadGenericAs<MaintenanceDepotProto>();
    }

    static MaintenanceDepot()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MaintenanceDepot.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      MaintenanceDepot.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
