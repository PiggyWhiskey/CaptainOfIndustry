// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleQueue`2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  /// <summary>
  /// Registers trucks arriving for material to an entity. The trucks are either registered as arriving or waiting.
  /// Waiting trucks already arrived and are waiting in the queue behind vehicle before them. Arriving trucks are
  /// currently heading (navigating) to the queue.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class VehicleQueue<TVehicle, TOwner> : IVehicleQueueFriend<TVehicle>, IVehicleJobObserver
    where TVehicle : Vehicle
    where TOwner : class, IEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>Queueing jobs of trucks waiting in the queue.</summary>
    protected readonly Queueue<VehicleQueueJob<TVehicle>> WaitingVehicleJobs;
    /// <summary>List of trucks arriving to the queue.</summary>
    protected readonly Lyst<VehicleQueueJob<TVehicle>> ArrivingVehicleJobs;
    private Option<IQueueTipJob> m_queueTipJob;
    private Option<TVehicle> m_queueTipJobVehicle;

    public static void Serialize(VehicleQueue<TVehicle, TOwner> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleQueue<TVehicle, TOwner>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleQueue<TVehicle, TOwner>.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<VehicleQueueJob<TVehicle>>.Serialize(this.ArrivingVehicleJobs, writer);
      writer.WriteBool(this.IsEnabled);
      Option<IQueueTipJob>.Serialize(this.m_queueTipJob, writer);
      Option<TVehicle>.Serialize(this.m_queueTipJobVehicle, writer);
      writer.WriteGeneric<TOwner>(this.Owner);
      Queueue<VehicleQueueJob<TVehicle>>.Serialize(this.WaitingVehicleJobs, writer);
    }

    public static VehicleQueue<TVehicle, TOwner> Deserialize(BlobReader reader)
    {
      VehicleQueue<TVehicle, TOwner> vehicleQueue;
      if (reader.TryStartClassDeserialization<VehicleQueue<TVehicle, TOwner>>(out vehicleQueue))
        reader.EnqueueDataDeserialization((object) vehicleQueue, VehicleQueue<TVehicle, TOwner>.s_deserializeDataDelayedAction);
      return vehicleQueue;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehicleQueue<TVehicle, TOwner>>(this, "ArrivingVehicleJobs", (object) Lyst<VehicleQueueJob<TVehicle>>.Deserialize(reader));
      this.IsEnabled = reader.ReadBool();
      this.m_queueTipJob = Option<IQueueTipJob>.Deserialize(reader);
      this.m_queueTipJobVehicle = Option<TVehicle>.Deserialize(reader);
      this.Owner = reader.ReadGenericAs<TOwner>();
      reader.SetField<VehicleQueue<TVehicle, TOwner>>(this, "WaitingVehicleJobs", (object) Queueue<VehicleQueueJob<TVehicle>>.Deserialize(reader));
      reader.RegisterInitAfterLoad<VehicleQueue<TVehicle, TOwner>>(this, "fixStateOnLoad", InitPriority.Lowest);
    }

    public TOwner Owner { get; private set; }

    IEntity IVehicleQueueFriend<TVehicle>.Owner => (IEntity) this.Owner;

    public Option<TVehicle> FirstVehicle
    {
      get
      {
        if (this.m_queueTipJobVehicle.HasValue)
          return (Option<TVehicle>) this.m_queueTipJobVehicle.Value;
        return !this.WaitingVehicleJobs.IsNotEmpty ? (Option<TVehicle>) Option.None : Option.Some<TVehicle>(this.WaitingVehicleJobs.First.Vehicle);
      }
    }

    public int WaitingTrucksCount
    {
      get => this.WaitingVehicleJobs.Count + (this.m_queueTipJob.HasValue ? 1 : 0);
    }

    public int ArrivingTrucksCount => this.ArrivingVehicleJobs.Count;

    public int TrucksCount => this.WaitingTrucksCount + this.ArrivingTrucksCount;

    /// <summary>Whether the queue is enabled and accepts Trucks.</summary>
    public bool IsEnabled { get; private set; }

    public VehicleQueue(TOwner owner)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.WaitingVehicleJobs = new Queueue<VehicleQueueJob<TVehicle>>();
      this.ArrivingVehicleJobs = new Lyst<VehicleQueueJob<TVehicle>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Owner = owner;
    }

    [InitAfterLoad(InitPriority.Lowest)]
    [OnlyForSaveCompatibility(null)]
    private void fixStateOnLoad(int saveVersion)
    {
      if (this.m_queueTipJob.HasValue && this.m_queueTipJob.Value.IsDestroyed)
      {
        Log.Error("Releasing vehicle with destroyed queue tip job on " + this.Owner.GetTitle() + string.Format(", save version: {0}", (object) saveVersion));
        this.ReleaseFirstVehicle();
      }
      if (!this.WaitingVehicleJobs.Any<VehicleQueueJob<TVehicle>>((Predicate<VehicleQueueJob<TVehicle>>) (x => x.IsDestroyed)))
        return;
      Log.Error("Destroyed job in WaitingVehicleJobs on " + this.Owner.GetTitle() + string.Format(", save version: {0}", (object) saveVersion));
      this.Clear();
    }

    protected virtual void OnEnabledChanged()
    {
    }

    public bool IsFirstVehicle(TVehicle vehicle)
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      return this.FirstVehicle == vehicle;
    }

    public bool FirstVehicleReadyAtQueueTip()
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      if (this.m_queueTipJobVehicle.HasValue)
        return true;
      return this.WaitingVehicleJobs.Count > 0 && this.WaitingVehicleJobs.First.IsReadyAtQueueTip();
    }

    /// <summary>Releases the first vehicle from the queue.</summary>
    public void ReleaseFirstVehicle()
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      Option<TVehicle> firstVehicle = this.FirstVehicle;
      if (this.m_queueTipJob.HasValue)
      {
        this.clearQueueTipJob();
      }
      else
      {
        this.releaseFirstWaitingVehicle();
        Assert.That<bool>(this.FirstVehicle != firstVehicle || firstVehicle.IsNone).IsTrue("Releasing first vehicle did not work.");
      }
    }

    private void releaseFirstWaitingVehicle()
    {
      if (this.FirstVehicle.IsNone)
        Assert.Fail("No truck to release.");
      else
        this.WaitingVehicleJobs.Dequeue().Released();
    }

    void IVehicleQueueFriend<TVehicle>.VehicleArrivedAndWaiting(VehicleQueueJob<TVehicle> job)
    {
      if (job == null)
      {
        Log.Error("Null job in `VehicleArrivedAndWaiting`.");
      }
      else
      {
        Assert.That<bool>(this.IsEnabled).IsTrue();
        this.WaitingVehicleJobs.Enqueue(job);
      }
    }

    /// <summary>To be only called by the job itself.</summary>
    void IVehicleQueueFriend<TVehicle>.RemoveWaitingJob(VehicleQueueJob<TVehicle> job)
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      Assert.That<bool>(this.WaitingVehicleJobs.TryRemove(job)).IsTrue();
    }

    void IVehicleQueueFriend<TVehicle>.VehicleArriving(VehicleQueueJob<TVehicle> job)
    {
      if (job == null)
      {
        Log.Error("Null job in `VehicleArriving`.");
      }
      else
      {
        Assert.That<bool>(this.IsEnabled).IsTrue();
        this.ArrivingVehicleJobs.Add(job);
      }
    }

    void IVehicleQueueFriend<TVehicle>.RemoveArrivingVehicle(VehicleQueueJob<TVehicle> job)
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      Assert.That<bool>(this.ArrivingVehicleJobs.Remove(job)).IsTrue();
    }

    /// <summary>
    /// Returns an entity that given truck should be waiting behind. This enables to form lines. Trucks has to first
    /// register as waiting <see cref="!:VehicleArrivedAndWaiting" /> before calling this. If Option.None is returned,
    /// the vehicle should wait behind queue owner.
    /// </summary>
    Option<Vehicle> IVehicleQueueFriend<TVehicle>.GetWaitTargetFor(VehicleQueueJob<TVehicle> job)
    {
      if (job == null)
      {
        Log.Error("Null job in `GetWaitTargetFor`.");
        return Option<Vehicle>.None;
      }
      Assert.That<bool>(this.IsEnabled).IsTrue();
      if (!this.WaitingVehicleJobs.IsEmpty && this.WaitingVehicleJobs.First != job)
        return Option<Vehicle>.None;
      if (!this.m_queueTipJob.HasValue || !this.m_queueTipJob.Value.WaitBehindQueueTipVehicle)
        return (Option<Vehicle>) Option.None;
      Assert.That<bool>(this.m_queueTipJob.Value.IsDestroyed).IsFalse();
      return (Option<Vehicle>) (Vehicle) this.m_queueTipJobVehicle.Value;
    }

    void IVehicleQueueFriend<TVehicle>.ReplaceFirstJobWith(IQueueTipJob newJob)
    {
      if (newJob == null)
      {
        Log.Error("Null job in `ReplaceFirstJobWith`");
      }
      else
      {
        Assert.That<bool>(newJob.IsDestroyed).IsFalse<IQueueTipJob>("Adding destroyed job {0}", newJob);
        Assert.That<Option<IQueueTipJob>>(this.m_queueTipJob).IsNotEqualTo<IQueueTipJob>(newJob, "Replacing job with itself, is that ok?");
        if (this.WaitingVehicleJobs.IsEmpty)
        {
          Log.Error("ReplaceFirstJobWith called while there are no waiting trucks.");
        }
        else
        {
          this.clearQueueTipJob();
          Assert.That<bool>(newJob.IsDestroyed).IsFalse();
          this.m_queueTipJob = Option.Some<IQueueTipJob>(newJob);
          this.m_queueTipJob.Value.AddObserver((IVehicleJobObserver) this);
          this.m_queueTipJobVehicle = (Option<TVehicle>) this.WaitingVehicleJobs.First.Vehicle;
          this.releaseFirstWaitingVehicle();
        }
      }
    }

    private void clearQueueTipJob()
    {
      if (this.m_queueTipJob.IsNone)
        return;
      Assert.That<bool>(this.m_queueTipJob.Value.IsDestroyed).IsFalse<IQueueTipJob>("Cleared tip job is destroyed {0}, already returned to pool?", this.m_queueTipJob.Value);
      this.m_queueTipJob.Value.RemoveObserver((IVehicleJobObserver) this);
      this.m_queueTipJob = (Option<IQueueTipJob>) Option.None;
      this.m_queueTipJobVehicle = (Option<TVehicle>) Option.None;
    }

    public void OnJobDone(IVehicleJob vehicleJob)
    {
      if (this.m_queueTipJob.ValueOrNull == vehicleJob)
        this.clearQueueTipJob();
      else
        Log.Error("Wrong IQueueTipJob informed us that it is done.");
    }

    /// <summary>
    /// Releases trucks from end of the queue to conform to given maximum count of trucks registered.
    /// </summary>
    public void ReleaseVehiclesOverLimit(int truckCountLimit)
    {
      int num;
      for (num = (this.TrucksCount - truckCountLimit).Max(0); num > 0 && this.ArrivingVehicleJobs.IsNotEmpty; --num)
      {
        this.ArrivingVehicleJobs.Last.Released();
        this.ArrivingVehicleJobs.PopLast();
      }
      for (; num > 0 && this.WaitingVehicleJobs.IsNotEmpty; --num)
      {
        this.WaitingVehicleJobs.Last.Released();
        this.WaitingVehicleJobs.PopAt(this.WaitingVehicleJobs.Count - 1);
      }
      Assert.That<int>(num).IsZero();
    }

    /// <summary>Releases all trucks from the queue.</summary>
    public void Clear()
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      this.clearQueueTipJob();
      foreach (VehicleQueueJob<TVehicle> waitingVehicleJob in this.WaitingVehicleJobs)
        waitingVehicleJob.Released();
      foreach (VehicleQueueJob<TVehicle> arrivingVehicleJob in this.ArrivingVehicleJobs)
        arrivingVehicleJob.Released();
      this.WaitingVehicleJobs.Clear();
      this.ArrivingVehicleJobs.Clear();
    }

    /// <summary>
    /// Enables the queue - allows trucks to register with it.
    /// </summary>
    public void Enable()
    {
      if (this.IsEnabled)
        return;
      Assert.That<Queueue<VehicleQueueJob<TVehicle>>>(this.WaitingVehicleJobs).IsEmpty<VehicleQueueJob<TVehicle>>();
      this.IsEnabled = true;
      this.OnEnabledChanged();
    }

    /// <summary>
    /// Releases all trucks from the queue and prevents new trucks from registering with the queue by disbaling it.
    /// </summary>
    public void Disable()
    {
      if (!this.IsEnabled)
        return;
      this.Clear();
      this.IsEnabled = false;
      this.OnEnabledChanged();
    }

    public void CancelJobsAndDisable()
    {
      if (!this.IsEnabled)
        return;
      foreach (VehicleJob vehicleJob in this.WaitingVehicleJobs.ToArray())
        vehicleJob.RequestCancel();
      foreach (VehicleJob vehicleJob in this.ArrivingVehicleJobs.ToArray())
        vehicleJob.RequestCancel();
      this.Disable();
    }

    public void SetEnabled(bool enabled)
    {
      if (enabled)
        this.Enable();
      else
        this.Disable();
    }

    public bool Contains(VehicleQueueJob<TVehicle> job)
    {
      return this.WaitingVehicleJobs.Contains(job) || this.ArrivingVehicleJobs.Contains(job);
    }

    static VehicleQueue()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleQueue<TVehicle, TOwner>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleQueue<TVehicle, TOwner>) obj).SerializeData(writer));
      VehicleQueue<TVehicle, TOwner>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleQueue<TVehicle, TOwner>) obj).DeserializeData(reader));
    }
  }
}
