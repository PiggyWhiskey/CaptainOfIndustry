// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.TruckQueue
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks
{
  /// <summary>
  /// Registers trucks arriving for material to an entity. The trucks are either registered as arriving or waiting.
  /// Waiting trucks already arrived and are waiting in the queue behind vehicle before them. Arriving trucks are
  /// currently heading (navigating) to the queue.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class TruckQueue : VehicleQueue<Truck, Vehicle>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(TruckQueue value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TruckQueue>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TruckQueue.s_serializeDataDelayedAction);
    }

    public static TruckQueue Deserialize(BlobReader reader)
    {
      TruckQueue truckQueue;
      if (reader.TryStartClassDeserialization<TruckQueue>(out truckQueue))
        reader.EnqueueDataDeserialization((object) truckQueue, TruckQueue.s_deserializeDataDelayedAction);
      return truckQueue;
    }

    public TruckQueue(Vehicle owner)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(owner);
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (!(this.Owner is ITruckQueueObserver owner))
        return;
      owner.onQueueEnabledChanged(this);
    }

    /// <summary>
    /// Returns first truck compatible with given cargo or none. Releases incompatible trucks.
    /// </summary>
    public Option<Truck> TryGetFirstTruckFor(IVehicleCargo cargo)
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      while (this.WaitingVehicleJobs.Count > 0)
      {
        if (this.WaitingVehicleJobs.First == null)
        {
          Log.Error("Null job in `WaitingVehicleJobs.First`.");
          this.WaitingVehicleJobs.Dequeue();
        }
        else if (this.WaitingVehicleJobs.First.Vehicle == null)
        {
          Log.Error("Null Vehicle in `WaitingVehicleJobs.First`.");
          this.WaitingVehicleJobs.Dequeue();
        }
        else
        {
          Truck vehicle = this.WaitingVehicleJobs.First.Vehicle;
          foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in cargo)
          {
            if (vehicle.Cargo.CanAdd(keyValuePair.Key))
              return (Option<Truck>) vehicle;
          }
          this.ReleaseFirstVehicle();
        }
      }
      return (Option<Truck>) Option.None;
    }

    /// <summary>
    /// Returns first truck compatible with given cargo or none. Releases incompatible trucks.
    /// </summary>
    public Option<Truck> TryGetFirstTruckFor(ProductProto cargoProto)
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      while (this.WaitingVehicleJobs.Count > 0)
      {
        if (this.WaitingVehicleJobs.First == null)
        {
          Log.Error("Null job in `WaitingVehicleJobs.First`.");
          this.WaitingVehicleJobs.Dequeue();
        }
        else if (this.WaitingVehicleJobs.First.Vehicle == null)
        {
          Log.Error("Null Vehicle in `WaitingVehicleJobs.First`.");
          this.WaitingVehicleJobs.Dequeue();
        }
        else
        {
          Truck vehicle = this.WaitingVehicleJobs.First.Vehicle;
          if (vehicle.Cargo.CanAdd(cargoProto))
            return (Option<Truck>) vehicle;
          this.ReleaseFirstVehicle();
        }
      }
      return (Option<Truck>) Option.None;
    }

    public void ReleaseTrucksWithCargo()
    {
      Assert.That<bool>(this.IsEnabled).IsTrue();
      for (int index = 0; index < this.WaitingVehicleJobs.Count; ++index)
      {
        VehicleQueueJob<Truck> waitingVehicleJob = this.WaitingVehicleJobs[index];
        if (!waitingVehicleJob.Vehicle.IsEmpty)
        {
          waitingVehicleJob.Released();
          this.WaitingVehicleJobs.PopAt(index);
          --index;
        }
      }
    }

    static TruckQueue()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TruckQueue.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleQueue<Truck, Vehicle>) obj).SerializeData(writer));
      TruckQueue.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleQueue<Truck, Vehicle>) obj).DeserializeData(reader));
    }
  }
}
