// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RocketTransporters.RocketTransporterProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.RocketTransporters
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class RocketTransporterProvider : IJobProvider<RocketTransporter>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly IRocketLaunchManager m_rocketLaunchManager;
    private readonly AttachRocketToLaunchPadJob.Factory m_attachRocketJobFactory;

    public static void Serialize(RocketTransporterProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RocketTransporterProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RocketTransporterProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IRocketLaunchManager>(this.m_rocketLaunchManager);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
    }

    public static RocketTransporterProvider Deserialize(BlobReader reader)
    {
      RocketTransporterProvider transporterProvider;
      if (reader.TryStartClassDeserialization<RocketTransporterProvider>(out transporterProvider))
        reader.EnqueueDataDeserialization((object) transporterProvider, RocketTransporterProvider.s_deserializeDataDelayedAction);
      return transporterProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.RegisterResolvedMember<RocketTransporterProvider>(this, "m_attachRocketJobFactory", typeof (AttachRocketToLaunchPadJob.Factory), true);
      reader.SetField<RocketTransporterProvider>(this, "m_rocketLaunchManager", (object) reader.ReadGenericAs<IRocketLaunchManager>());
      reader.SetField<RocketTransporterProvider>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
    }

    public RocketTransporterProvider(
      IVehiclesManager vehiclesManager,
      IRocketLaunchManager rocketLaunchManager,
      AttachRocketToLaunchPadJob.Factory attachRocketJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_vehiclesManager = vehiclesManager;
      this.m_rocketLaunchManager = rocketLaunchManager;
      this.m_attachRocketJobFactory = attachRocketJobFactory;
    }

    public bool TryGetJobFor(RocketTransporter transporter)
    {
      Assert.That<bool>(transporter.IsSpawned).IsTrue();
      if (transporter.HasJobs)
      {
        Assert.Fail("The transporter already has a job assigned!");
        return false;
      }
      return transporter.AttachedRocketBase.IsNone ? transporter.TryRequestScrap() : this.tryGetGoToLaunchPadJob(transporter);
    }

    private bool tryGetGoToLaunchPadJob(RocketTransporter transporter)
    {
      Option<RocketLaunchPad> availableLaunchPad = this.m_rocketLaunchManager.FindClosestAvailableLaunchPad(transporter);
      if (availableLaunchPad.IsNone)
        return false;
      this.m_attachRocketJobFactory.EnqueueJob(transporter, availableLaunchPad.Value);
      return true;
    }

    static RocketTransporterProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RocketTransporterProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RocketTransporterProvider) obj).SerializeData(writer));
      RocketTransporterProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RocketTransporterProvider) obj).DeserializeData(reader));
    }
  }
}
