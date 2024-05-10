// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.VehicleJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  public abstract class VehicleJob : 
    IEquatable<VehicleJob>,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey
  {
    private readonly VehicleJobId m_id;
    private Duration m_cancelDeadline;
    private Option<LystMutableDuringIter<IVehicleJobObserver>> m_observers;

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsBeingCancelled);
      writer.WriteBool(this.IsDestroyed);
      Duration.Serialize(this.m_cancelDeadline, writer);
      VehicleJobId.Serialize(this.m_id, writer);
      Option<LystMutableDuringIter<IVehicleJobObserver>>.Serialize(this.m_observers, writer);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsBeingCancelled = reader.ReadBool();
      this.IsDestroyed = reader.ReadBool();
      this.m_cancelDeadline = Duration.Deserialize(reader);
      reader.SetField<VehicleJob>(this, "m_id", (object) VehicleJobId.Deserialize(reader));
      this.m_observers = Option<LystMutableDuringIter<IVehicleJobObserver>>.Deserialize(reader);
    }

    public VehicleJobId Id => this.m_id;

    public abstract LocStrFormatted JobInfo { get; }

    public bool IsDestroyed { get; private set; }

    /// <summary>
    /// Whether this job is "true" job and entity should not be seeking for another job. An example of non-true job
    /// is returning to depot where a vehicle can get another job instead.
    /// </summary>
    public abstract bool IsTrueJob { get; }

    /// <summary>How much of fuel is the job currently consuming.</summary>
    public abstract VehicleFuelConsumption CurrentFuelConsumption { get; }

    /// <summary>Whether this job is being cancelled.</summary>
    public bool IsBeingCancelled { get; private set; }

    public bool IsCancelled => this.IsBeingCancelled && this.m_cancelDeadline.IsNotPositive;

    public virtual bool SkipNoMovementMonitoring => false;

    protected VehicleJob(VehicleJobId id)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_id = id;
    }

    /// <summary>Performs one step of the job.</summary>
    /// <returns>True if there is more work, false if all work was done.</returns>
    public bool DoJob()
    {
      if (this.IsDestroyed)
      {
        Log.Error("Calling DoJob on destroyed job!");
        return false;
      }
      Assert.That<bool>(!this.IsBeingCancelled || this.m_cancelDeadline.IsPositive).IsTrue("Cancelled job is being called.");
      bool flag = this.DoJobInternal();
      if (this.IsBeingCancelled & flag)
      {
        this.m_cancelDeadline -= Duration.OneTick;
        if (this.m_cancelDeadline <= Duration.Zero)
        {
          Log.Error(string.Format("Job #{0} '{1}' did not get cancelled in time. Force-cancelling. Info:", (object) this.m_id, (object) this.GetType().Name) + this.JobInfo.Value);
          return false;
        }
      }
      return flag;
    }

    /// <summary>Performs one step of the job.</summary>
    /// <returns>True if there is more work, false if all work was done.</returns>
    protected abstract bool DoJobInternal();

    public virtual bool RequestCancel()
    {
      if (this.IsDestroyed)
      {
        Log.Error("CancelJob on destroyed instance.");
        return true;
      }
      this.IsBeingCancelled = true;
      this.m_cancelDeadline = this.RequestCancelReturnDeadline();
      return this.m_cancelDeadline.IsNotPositive;
    }

    /// <summary>
    /// Default implementation declared the job cancelled immediately. Override this if you need to do some extra
    /// work in order to cancel the job and return <c>false</c>.
    /// </summary>
    protected virtual Duration RequestCancelReturnDeadline() => Duration.Zero;

    public void Destroy()
    {
      if (this.IsDestroyed)
      {
        Log.Error("Destroying a destroyed job");
      }
      else
      {
        this.IsBeingCancelled = false;
        this.m_cancelDeadline = Duration.Zero;
        this.IsDestroyed = true;
        this.m_observers = (Option<LystMutableDuringIter<IVehicleJobObserver>>) Option.None;
        try
        {
          this.OnDestroy();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown when destroying a job");
        }
      }
    }

    public void AddObserver(IVehicleJobObserver observer)
    {
      if (this.m_observers.IsNone)
        this.m_observers = (Option<LystMutableDuringIter<IVehicleJobObserver>>) new LystMutableDuringIter<IVehicleJobObserver>();
      if (this.m_observers.Value.Contains<IVehicleJobObserver>(observer))
        Log.Error("Observer already added!");
      else
        this.m_observers.Value.Add(observer);
    }

    public void RemoveObserver(IVehicleJobObserver observer)
    {
      if (this.m_observers.IsNone)
        Log.Error("Observer not set!");
      else
        Assert.That<bool>(this.m_observers.Value.Remove(observer)).IsTrue("Observer is not registered!");
    }

    protected void InvokeJobDone()
    {
      if (this.m_observers.IsNone)
        return;
      foreach (IVehicleJobObserver vehicleJobObserver in this.m_observers.Value)
        vehicleJobObserver.OnJobDone((IVehicleJob) this);
    }

    protected abstract void OnDestroy();

    public bool Equals(VehicleJob other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (!this.m_id.Equals(other.m_id))
        return false;
      Log.Error(string.Format("Vehicle jobs that are not equal references have equal IDs: {0}", (object) this.m_id));
      return true;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      if (!(obj is VehicleJob vehicleJob) || !this.m_id.Equals(vehicleJob.m_id))
        return false;
      Log.Error(string.Format("Vehicle jobs that are not equal references have equal IDs: {0}", (object) this.m_id));
      return true;
    }

    public override int GetHashCode() => this.m_id.GetHashCode();

    public override string ToString()
    {
      return string.Format("Job #{0}: {1}", (object) this.Id, (object) this.JobInfo.Value);
    }
  }
}
