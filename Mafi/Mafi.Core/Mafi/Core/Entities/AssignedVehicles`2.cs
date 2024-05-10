// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.AssignedVehicles`2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>Helper class that handles assigned vehicles.</summary>
  [GenerateSerializer(false, null, 0)]
  public class AssignedVehicles<TVehicle, TProto>
    where TVehicle : Vehicle
    where TProto : DynamicEntityProto
  {
    private readonly Lyst<TVehicle> m_vehicles;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<Vehicle> m_vehiclesUntyped;
    private readonly IEntityAssignedWithVehicles m_entity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>All assigned vehicles.</summary>
    public IIndexable<TVehicle> All => (IIndexable<TVehicle>) this.m_vehicles;

    public IIndexable<Vehicle> AllUntyped => (IIndexable<Vehicle>) this.m_vehiclesUntyped;

    public bool IsEmpty => this.m_vehicles.IsEmpty;

    public bool IsNotEmpty => this.m_vehicles.IsNotEmpty;

    public int Count => this.All.Count;

    public AssignedVehicles(IEntityAssignedWithVehicles entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_vehicles = new Lyst<TVehicle>();
      this.m_vehiclesUntyped = new Lyst<Vehicle>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entity = entity.CheckNotNull<IEntityAssignedWithVehicles>();
    }

    [InitAfterLoad(InitPriority.Highest)]
    private void initSelf()
    {
      this.m_vehiclesUntyped.AddRange((IEnumerable<Vehicle>) this.m_vehicles);
    }

    public bool AssignVehicle(TVehicle vehicle, bool doNotCancelJobs = false)
    {
      Assert.That<bool>(vehicle.CanBeAssigned).IsTrue<TVehicle>("Assigning vehicle '{0}' that cannot be assigned.", vehicle);
      if (!this.m_vehicles.AddIfNotPresent(vehicle))
      {
        Assert.Fail(string.Format("Vehicle {0} is already assigned!", (object) vehicle.Id));
        return false;
      }
      this.m_vehiclesUntyped.Add((Vehicle) vehicle);
      vehicle.AssignTo(this.m_entity, doNotCancelJobs);
      return true;
    }

    public bool UnassignVehicle(TVehicle vehicle, bool cancelJobs)
    {
      if (!this.m_vehicles.TryRemove(vehicle))
        return false;
      this.m_vehiclesUntyped.Remove((Vehicle) vehicle);
      vehicle.UnassignFrom(this.m_entity, cancelJobs);
      return true;
    }

    public static void Serialize(AssignedVehicles<TVehicle, TProto> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AssignedVehicles<TVehicle, TProto>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AssignedVehicles<TVehicle, TProto>.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IEntityAssignedWithVehicles>(this.m_entity);
      Lyst<TVehicle>.Serialize(this.m_vehicles, writer);
    }

    public static AssignedVehicles<TVehicle, TProto> Deserialize(BlobReader reader)
    {
      AssignedVehicles<TVehicle, TProto> assignedVehicles;
      if (reader.TryStartClassDeserialization<AssignedVehicles<TVehicle, TProto>>(out assignedVehicles))
        reader.EnqueueDataDeserialization((object) assignedVehicles, AssignedVehicles<TVehicle, TProto>.s_deserializeDataDelayedAction);
      return assignedVehicles;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<AssignedVehicles<TVehicle, TProto>>(this, "m_entity", (object) reader.ReadGenericAs<IEntityAssignedWithVehicles>());
      reader.SetField<AssignedVehicles<TVehicle, TProto>>(this, "m_vehicles", (object) Lyst<TVehicle>.Deserialize(reader));
      reader.SetField<AssignedVehicles<TVehicle, TProto>>(this, "m_vehiclesUntyped", (object) new Lyst<Vehicle>());
      reader.RegisterInitAfterLoad<AssignedVehicles<TVehicle, TProto>>(this, "initSelf", InitPriority.Highest);
    }

    static AssignedVehicles()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssignedVehicles<TVehicle, TProto>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AssignedVehicles<TVehicle, TProto>) obj).SerializeData(writer));
      AssignedVehicles<TVehicle, TProto>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AssignedVehicles<TVehicle, TProto>) obj).DeserializeData(reader));
    }
  }
}
