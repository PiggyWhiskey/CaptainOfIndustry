// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellInjectionPump
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
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  [GenerateSerializer(false, null, 0)]
  public sealed class WellInjectionPump : Machine
  {
    private WellInjectionPumpProto m_proto;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public WellInjectionPumpProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (MachineProto) value;
      }
    }

    public WellInjectionPump(
      EntityId id,
      WellInjectionPumpProto wellPumpProto,
      TileTransform transform,
      EntityContext context,
      VirtualBuffersMap buffersMap,
      IVirtualResourceManager virtualResourceManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IUnityConsumerFactory unityConsumerFactory,
      UnlockedProtosDb unlockedProtosDb,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (MachineProto) wellPumpProto, transform, context, buffersMap, upgraderFactory, unityConsumerFactory, unlockedProtosDb, vehicleBuffersRegistry, maintenanceProvidersFactory, animationStateFactory);
      this.Prototype = wellPumpProto;
      if (virtualResourceManager.RetrieveAllResourcesAt(transform.Position.Tile2i).Length <= 0)
        return;
      Log.Error("Cannot inject into an existing resource / deposit.");
    }

    public static void Serialize(WellInjectionPump value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WellInjectionPump>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WellInjectionPump.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<WellInjectionPumpProto>(this.m_proto);
    }

    public static WellInjectionPump Deserialize(BlobReader reader)
    {
      WellInjectionPump wellInjectionPump;
      if (reader.TryStartClassDeserialization<WellInjectionPump>(out wellInjectionPump))
        reader.EnqueueDataDeserialization((object) wellInjectionPump, WellInjectionPump.s_deserializeDataDelayedAction);
      return wellInjectionPump;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_proto = reader.ReadGenericAs<WellInjectionPumpProto>();
    }

    static WellInjectionPump()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WellInjectionPump.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      WellInjectionPump.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
