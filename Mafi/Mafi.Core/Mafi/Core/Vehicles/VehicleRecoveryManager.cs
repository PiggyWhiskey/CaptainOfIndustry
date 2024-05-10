// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleRecoveryManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class VehicleRecoveryManager : IEntityObserver
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Dict<Vehicle, VehicleRecoveryManager.VehicleRecoveryData> m_recoveryData;

    public static void Serialize(VehicleRecoveryManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleRecoveryManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleRecoveryManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<Vehicle, VehicleRecoveryManager.VehicleRecoveryData>.Serialize(this.m_recoveryData, writer);
    }

    public static VehicleRecoveryManager Deserialize(BlobReader reader)
    {
      VehicleRecoveryManager vehicleRecoveryManager;
      if (reader.TryStartClassDeserialization<VehicleRecoveryManager>(out vehicleRecoveryManager))
        reader.EnqueueDataDeserialization((object) vehicleRecoveryManager, VehicleRecoveryManager.s_deserializeDataDelayedAction);
      return vehicleRecoveryManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehicleRecoveryManager>(this, "m_recoveryData", (object) Dict<Vehicle, VehicleRecoveryManager.VehicleRecoveryData>.Deserialize(reader));
    }

    public void AddVehicleToRecover(Vehicle vehicle)
    {
      if (this.m_recoveryData.TryGetValue(vehicle, out VehicleRecoveryManager.VehicleRecoveryData _))
      {
        Log.Error("Vehicle is already being recovered");
      }
      else
      {
        Option<IEntityAssignedWithVehicles> assignedTo = vehicle.AssignedTo;
        if (vehicle.AssignedTo.HasValue)
          vehicle.UnassignFrom(vehicle.AssignedTo.Value);
        ImmutableArray<Vehicle> immutableArray;
        if (vehicle is IEntityAssignedWithVehicles entity)
        {
          immutableArray = entity.AllSpawnedVehicles().ToImmutableArray<Vehicle>();
          foreach (Vehicle vehicle1 in immutableArray)
            entity.UnassignVehicle(vehicle1);
        }
        else
          immutableArray = ImmutableArray<Vehicle>.Empty;
        this.m_recoveryData.Add(vehicle, new VehicleRecoveryManager.VehicleRecoveryData()
        {
          AssignedTo = assignedTo,
          AssignedVehicles = immutableArray
        });
        vehicle.AddObserver((IEntityObserver) this);
      }
    }

    public void OnVehicleSpawned(Vehicle vehicle)
    {
      VehicleRecoveryManager.VehicleRecoveryData vehicleRecoveryData;
      if (!this.m_recoveryData.TryGetValue(vehicle, out vehicleRecoveryData))
        return;
      this.removeVehicleRegistration(vehicle);
      try
      {
        if (vehicleRecoveryData.AssignedTo.HasValue && !vehicleRecoveryData.AssignedTo.Value.IsDestroyed)
          vehicleRecoveryData.AssignedTo.Value.AssignVehicle(vehicle, true);
        if (!(vehicle is IEntityAssignedWithVehicles assignedWithVehicles))
          return;
        ImmutableArray<Vehicle>.Enumerator enumerator = vehicleRecoveryData.AssignedVehicles.GetEnumerator();
        while (enumerator.MoveNext())
        {
          Vehicle current = enumerator.Current;
          if (!current.IsDestroyed && current.IsSpawned && current.AssignedTo.IsNone)
            assignedWithVehicles.AssignVehicle(current);
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Exception thrown while restoring assignments for " + vehicle.GetTitle());
      }
    }

    private void removeVehicleRegistration(Vehicle vehicle)
    {
      this.m_recoveryData.RemoveAndAssert(vehicle);
      vehicle.RemoveObserver((IEntityObserver) this);
    }

    public void AbortRecovery(Vehicle vehicle) => this.removeVehicleRegistration(vehicle);

    public void OnEntityDestroy(IEntity entity)
    {
      if (!(entity is Vehicle vehicle))
        return;
      this.removeVehicleRegistration(vehicle);
    }

    public VehicleRecoveryManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_recoveryData = new Dict<Vehicle, VehicleRecoveryManager.VehicleRecoveryData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleRecoveryManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleRecoveryManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleRecoveryManager) obj).SerializeData(writer));
      VehicleRecoveryManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleRecoveryManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private struct VehicleRecoveryData
    {
      public Option<IEntityAssignedWithVehicles> AssignedTo;
      public ImmutableArray<Vehicle> AssignedVehicles;

      public static void Serialize(
        VehicleRecoveryManager.VehicleRecoveryData value,
        BlobWriter writer)
      {
        Option<IEntityAssignedWithVehicles>.Serialize(value.AssignedTo, writer);
        ImmutableArray<Vehicle>.Serialize(value.AssignedVehicles, writer);
      }

      public static VehicleRecoveryManager.VehicleRecoveryData Deserialize(BlobReader reader)
      {
        return new VehicleRecoveryManager.VehicleRecoveryData()
        {
          AssignedTo = Option<IEntityAssignedWithVehicles>.Deserialize(reader),
          AssignedVehicles = ImmutableArray<Vehicle>.Deserialize(reader)
        };
      }
    }
  }
}
