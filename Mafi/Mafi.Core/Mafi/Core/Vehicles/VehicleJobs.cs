// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleJobs
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class VehicleJobs
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Set<IVehicleJob> m_jobs;

    public static void Serialize(VehicleJobs value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleJobs>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleJobs.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Set<IVehicleJob>.Serialize(this.m_jobs, writer);
    }

    public static VehicleJobs Deserialize(BlobReader reader)
    {
      VehicleJobs vehicleJobs;
      if (reader.TryStartClassDeserialization<VehicleJobs>(out vehicleJobs))
        reader.EnqueueDataDeserialization((object) vehicleJobs, VehicleJobs.s_deserializeDataDelayedAction);
      return vehicleJobs;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehicleJobs>(this, "m_jobs", (object) Set<IVehicleJob>.Deserialize(reader));
    }

    internal IReadOnlySet<IVehicleJob> AllJobs => (IReadOnlySet<IVehicleJob>) this.m_jobs;

    public int Count => this.m_jobs.Count;

    public bool TryAddJob(IVehicleJob job)
    {
      if (this.m_jobs.Add(job))
        return true;
      Log.Error("Failed to add job, it is already reserved.");
      return false;
    }

    public bool TryRemoveJob(IVehicleJob job)
    {
      if (this.m_jobs.Remove(job))
        return true;
      Log.Error("Failed to remove job, it has no reservation.");
      return false;
    }

    public void Destroy()
    {
      foreach (IVehicleJob vehicleJob in this.m_jobs.ToArray())
        vehicleJob.RequestCancel();
    }

    public void ClearAndCancelAllJobs()
    {
      foreach (IVehicleJob vehicleJob in this.m_jobs.ToArray())
        vehicleJob.RequestCancel();
      this.m_jobs.Clear();
    }

    public VehicleJobs()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_jobs = new Set<IVehicleJob>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleJobs()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleJobs.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJobs) obj).SerializeData(writer));
      VehicleJobs.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJobs) obj).DeserializeData(reader));
    }
  }
}
