// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.VehicleJobsSequence
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  public sealed class VehicleJobsSequence
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Queueue<VehicleJob> m_jobs;
    /// <summary>Flag that is set when this sequence is cancelling.</summary>
    private bool m_canceling;
    /// <summary>The vehicle this job queue operates on.</summary>
    private readonly Vehicle m_vehicle;

    public static void Serialize(VehicleJobsSequence value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleJobsSequence>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleJobsSequence.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.m_canceling);
      Queueue<VehicleJob>.Serialize(this.m_jobs, writer);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
    }

    public static VehicleJobsSequence Deserialize(BlobReader reader)
    {
      VehicleJobsSequence vehicleJobsSequence;
      if (reader.TryStartClassDeserialization<VehicleJobsSequence>(out vehicleJobsSequence))
        reader.EnqueueDataDeserialization((object) vehicleJobsSequence, VehicleJobsSequence.s_deserializeDataDelayedAction);
      return vehicleJobsSequence;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.m_canceling = reader.ReadBool();
      reader.SetField<VehicleJobsSequence>(this, "m_jobs", (object) Queueue<VehicleJob>.Deserialize(reader));
      reader.SetField<VehicleJobsSequence>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
      reader.RegisterInitAfterLoad<VehicleJobsSequence>(this, "initSelf", InitPriority.Lowest);
    }

    public bool IsEmpty => this.m_jobs.IsEmpty;

    public bool IsNotEmpty => this.m_jobs.IsNotEmpty;

    public Option<IVehicleJob> CurrentJob
    {
      get
      {
        return !this.m_jobs.IsEmpty ? Option<IVehicleJob>.Some((IVehicleJob) this.m_jobs.First) : Option<IVehicleJob>.None;
      }
    }

    public VehicleFuelConsumption CurrentFuelConsumption
    {
      get
      {
        return !this.m_jobs.IsEmpty ? this.m_jobs.First.CurrentFuelConsumption : VehicleFuelConsumption.None;
      }
    }

    public bool ContainsTrueJob
    {
      get
      {
        if (this.m_jobs.IsEmpty)
          return false;
        foreach (VehicleJob job in this.m_jobs)
        {
          if (job.IsTrueJob)
            return true;
        }
        return false;
      }
    }

    public int Count => this.m_jobs.Count;

    internal IEnumerable<IVehicleJobReadOnly> AllJobs
    {
      get => (IEnumerable<IVehicleJobReadOnly>) this.m_jobs.AsEnumerable<VehicleJob>();
    }

    public IVehicleJob this[int index] => (IVehicleJob) this.m_jobs[index];

    public VehicleJobsSequence(Vehicle vehicle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_jobs = new Queueue<VehicleJob>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_vehicle = vehicle;
    }

    [InitAfterLoad(InitPriority.Lowest)]
    private void initSelf(int saveVersion)
    {
      if (this.m_canceling && this.m_vehicle != null)
      {
        this.m_canceling = false;
        Log.Error("Clearing incorrect jobs cancellation state for " + this.m_vehicle.GetTitle() + ", " + string.Format("save version: {0}", (object) saveVersion));
        this.CancelAllAndClear();
      }
      else
      {
        for (int index = 0; index < this.m_jobs.Count; ++index)
        {
          if (this.m_jobs[index] == null)
          {
            this.m_jobs.PopAt(index);
            --index;
            Log.Error("Found and removed null job from queue.");
          }
        }
      }
    }

    public bool ContainsJobOfType<T>()
    {
      if (this.m_jobs.IsEmpty)
        return false;
      foreach (VehicleJob job in this.m_jobs)
      {
        if (job is T)
          return true;
      }
      return false;
    }

    /// <summary>Adds given job to the end of the sequence.</summary>
    public void EnqueueJob(VehicleJob job)
    {
      if (job == null)
        Log.Error("Enqueueing null job to a queue.");
      else
        this.m_jobs.Enqueue(job);
    }

    /// <summary>Adds given job at the front of the sequence.</summary>
    public void AddFirst(VehicleJob job)
    {
      if (job == null)
        Log.Error("AddingFirst null job to a queue.");
      else
        this.m_jobs.EnqueueFirst(job);
    }

    /// <summary>
    /// Moves job at given index (0 being front of the queue) to the back of the queue.
    /// </summary>
    public void MoveToBack(int index)
    {
      if ((uint) index >= (uint) this.m_jobs.Count)
        Log.Error(string.Format("MoveToBack called with invalid index {0} (count={1}).", (object) index, (object) this.m_jobs.Count));
      else
        this.m_jobs.Enqueue(this.m_jobs.PopAt(index));
    }

    /// <summary>Performs a first job in the sequence.</summary>
    /// <returns>True if there is more work, false if all work is done.</returns>
    public bool DoJob()
    {
      VehicleJob vehicleJob1 = (VehicleJob) null;
      while (this.m_jobs.IsNotEmpty)
      {
        VehicleJob vehicleJob2 = this.m_jobs.Peek();
        if (vehicleJob2.IsCancelled)
        {
          this.m_jobs.Dequeue().Destroy();
        }
        else
        {
          vehicleJob1 = vehicleJob2;
          break;
        }
      }
      if (vehicleJob1 == null)
        return false;
      this.m_vehicle.ConsumeFuelPerUpdate();
      if (vehicleJob1.DoJob())
      {
        Assert.That<Queueue<VehicleJob>>(this.m_jobs).IsNotEmpty<VehicleJob>("Job reported not done but jobs queue is empty.");
        return true;
      }
      if (this.m_jobs.IsEmpty)
        return false;
      this.m_jobs.TryRemove(vehicleJob1).AssertTrue();
      vehicleJob1.Destroy();
      return !this.m_jobs.IsEmpty;
    }

    /// <summary>
    /// Cancels and removes all cancellable jobs in the sequence.
    /// </summary>
    public void CancelAll() => this.cancelAll(Option<VehicleJob>.None);

    public void CancelAllExcept(VehicleJob caller) => this.cancelAll((Option<VehicleJob>) caller);

    public void CancelAllAndClear()
    {
      this.cancelAll(Option<VehicleJob>.None);
      if (!this.m_jobs.IsNotEmpty)
        return;
      Log.Warning("Some jobs were not cancelled during clear: " + this.m_jobs.Select<VehicleJob, string>((Func<VehicleJob, string>) (x => x.GetType().Name)).JoinStrings(", "));
      foreach (VehicleJob job in this.m_jobs)
      {
        Assert.That<bool>(job.IsBeingCancelled).IsTrue();
        job.Destroy();
      }
      this.m_jobs.Clear();
    }

    /// <summary>
    /// Requests cancellation for all jobs and removes ones that were cancelled immediately.
    /// </summary>
    private void cancelAll(Option<VehicleJob> caller)
    {
      if (this.m_canceling)
        return;
      this.m_canceling = true;
      for (int index = 0; index < this.m_jobs.Count; ++index)
      {
        VehicleJob job = this.m_jobs[index];
        if (!(job == caller))
        {
          if (!job.IsBeingCancelled)
          {
            try
            {
              if (!job.RequestCancel())
                continue;
            }
            catch (Exception ex)
            {
              Log.Exception(ex, "Exception thrown when cancelling a job");
            }
            job.Destroy();
            this.m_jobs.PopAt(index);
            --index;
          }
        }
      }
      this.m_canceling = false;
    }

    static VehicleJobsSequence()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleJobsSequence.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJobsSequence) obj).SerializeData(writer));
      VehicleJobsSequence.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJobsSequence) obj).DeserializeData(reader));
    }
  }
}
