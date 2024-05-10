// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.OceanWaterPump
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  [GenerateSerializer(false, null, 0)]
  public class OceanWaterPump : 
    Machine,
    IStaticEntityWithReservedOcean,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(OceanWaterPump value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<OceanWaterPump>(value))
        return;
      writer.EnqueueDataSerialization((object) value, OceanWaterPump.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ReservedOceanAreaState.Serialize(this.ReservedOceanAreaState, writer);
    }

    public static OceanWaterPump Deserialize(BlobReader reader)
    {
      OceanWaterPump oceanWaterPump;
      if (reader.TryStartClassDeserialization<OceanWaterPump>(out oceanWaterPump))
        reader.EnqueueDataDeserialization((object) oceanWaterPump, OceanWaterPump.s_deserializeDataDelayedAction);
      return oceanWaterPump;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ReservedOceanAreaState = ReservedOceanAreaState.Deserialize(reader);
    }

    public ReservedOceanAreaState ReservedOceanAreaState { get; private set; }

    public OceanWaterPump(
      EntityId id,
      OceanWaterPumpProto proto,
      TileTransform transform,
      EntityContext context,
      VirtualBuffersMap virtualBuffersMap,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IUnityConsumerFactory unityConsumerFactory,
      UnlockedProtosDb unlockedProtosDb,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (MachineProto) proto, transform, context, virtualBuffersMap, upgraderFactory, unityConsumerFactory, unlockedProtosDb, vehicleBuffersRegistry, maintenanceProvidersFactory, animationStateFactory);
      this.ReservedOceanAreaState = new ReservedOceanAreaState((IProtoWithReservedOcean) proto, (IStaticEntityWithReservedOcean) this, new EntityNotificationProto.ID?(IdsCore.Notifications.OceanAccessBlocked), context.NotificationsManager);
    }

    protected override Machine.State TryGetNewWork(out Percent utilization)
    {
      if (this.ReservedOceanAreaState.HasAnyValidAreaSet)
        return base.TryGetNewWork(out utilization);
      utilization = Percent.Zero;
      return Machine.State.InvalidPlacement;
    }

    static OceanWaterPump()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      OceanWaterPump.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      OceanWaterPump.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
