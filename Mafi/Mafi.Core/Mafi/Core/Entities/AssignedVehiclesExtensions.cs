// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.AssignedVehiclesExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using System;
using System.Linq;
using System.Threading;

#nullable disable
namespace Mafi.Core.Entities
{
  public static class AssignedVehiclesExtensions
  {
    private static readonly ThreadLocal<Lyst<Vehicle>> s_vehiclesCache;

    private static Lyst<Vehicle> VehiclesCache => AssignedVehiclesExtensions.s_vehiclesCache.Value;

    public static IIndexable<Vehicle> AllSpawnedVehicles(this IEntityAssignedWithVehicles entity)
    {
      AssignedVehiclesExtensions.VehiclesCache.Clear();
      foreach (Vehicle allVehicle in entity.AllVehicles)
      {
        if (allVehicle.IsSpawned)
          AssignedVehiclesExtensions.VehiclesCache.Add(allVehicle);
      }
      return (IIndexable<Vehicle>) AssignedVehiclesExtensions.VehiclesCache;
    }

    public static bool HasUsefulIdleVehicle(this IEntityAssignedWithVehicles entity)
    {
      foreach (Vehicle allVehicle in entity.AllVehicles)
      {
        if (allVehicle.IsEnabled && !allVehicle.IsDestroyed && !allVehicle.HasTrueJob)
          return true;
      }
      return false;
    }

    /// <summary>
    /// Returns all vehicles that have the same proto as the given one.
    /// WARNING: The collection returned is a cache, make a copy if you need to persist it.
    /// </summary>
    public static IIndexable<Vehicle> AllVehiclesWithProto(
      this IEntityAssignedWithVehicles entity,
      DynamicEntityProto vehicleProto)
    {
      AssignedVehiclesExtensions.VehiclesCache.Clear();
      foreach (Vehicle allVehicle in entity.AllVehicles)
      {
        if ((Proto) allVehicle.Prototype == (Proto) vehicleProto)
          AssignedVehiclesExtensions.VehiclesCache.Add(allVehicle);
      }
      return (IIndexable<Vehicle>) AssignedVehiclesExtensions.VehiclesCache;
    }

    public static int VehiclesTotal(this IEntityAssignedWithVehicles entity)
    {
      return entity.AllVehicles.Count;
    }

    public static bool HasAssignedVehicle(
      this IEntityAssignedWithVehicles assignedVehicles,
      Vehicle vehicle)
    {
      return assignedVehicles.AllVehicles.Contains<Vehicle>(vehicle);
    }

    public static void AssignVehicle(
      this IEntityAssignedWithVehicles entity,
      IVehiclesManager vehiclesManager,
      DynamicEntityProto proto)
    {
      Option<Vehicle> freeVehicle = vehiclesManager.GetFreeVehicle<Vehicle>(proto, entity.Position2f);
      if (freeVehicle.IsNone)
        return;
      entity.AssignVehicle(freeVehicle.Value);
    }

    public static void UnassignVehicle(
      this IEntityAssignedWithVehicles entity,
      DynamicEntityProto proto)
    {
      Vehicle vehicle = entity.AllVehicles.Where<Vehicle>((Func<Vehicle, bool>) (x => (Proto) x.Prototype == (Proto) proto)).LastOrDefault<Vehicle>();
      if (vehicle == null)
        return;
      entity.UnassignVehicle(vehicle);
    }

    static AssignedVehiclesExtensions()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssignedVehiclesExtensions.s_vehiclesCache = new ThreadLocal<Lyst<Vehicle>>((Func<Lyst<Vehicle>>) (() => new Lyst<Vehicle>(32)));
    }
  }
}
