// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.TreeHarvesterTruckJobProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  /// <summary>
  /// Truck job provider providing jobs to trucks assigned to a tree harvester.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class TreeHarvesterTruckJobProvider : TruckJobProviderBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly TreeHarvester m_harvester;
    private readonly VehicleQueueJobFactory m_queueJobFactory;
    private static readonly ThreadLocal<Set<IEntityAssignedAsInput>> s_storagesCache;

    public static void Serialize(TreeHarvesterTruckJobProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreeHarvesterTruckJobProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreeHarvesterTruckJobProvider.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      TreeHarvester.Serialize(this.m_harvester, writer);
    }

    public static TreeHarvesterTruckJobProvider Deserialize(BlobReader reader)
    {
      TreeHarvesterTruckJobProvider truckJobProvider;
      if (reader.TryStartClassDeserialization<TreeHarvesterTruckJobProvider>(out truckJobProvider))
        reader.EnqueueDataDeserialization((object) truckJobProvider, TreeHarvesterTruckJobProvider.s_deserializeDataDelayedAction);
      return truckJobProvider;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<TreeHarvesterTruckJobProvider>(this, "m_harvester", (object) TreeHarvester.Deserialize(reader));
      reader.RegisterResolvedMember<TreeHarvesterTruckJobProvider>(this, "m_queueJobFactory", typeof (VehicleQueueJobFactory), true);
    }

    public TreeHarvesterTruckJobProvider(
      TreeHarvester harvester,
      TruckJobProviderContext context,
      VehicleQueueJobFactory queueJobFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(context);
      this.m_harvester = harvester;
      this.m_queueJobFactory = queueJobFactory;
    }

    public override bool TryGetJobFor(Truck truck)
    {
      if (truck.HasJobs)
      {
        Assert.Fail("The truck already has a job assigned!");
        return false;
      }
      bool hasJob;
      if (this.TryGetVehicleRefuelingJob((Vehicle) truck, out hasJob))
        return hasJob;
      if (this.tryGetRidOfCargo(truck))
        return true;
      if (!truck.IsEmpty || !this.m_harvester.TruckQueue.IsEnabled || this.Context.UnreachablesManager.HasUnreachableEntity((IPathFindingVehicle) truck, (IEntity) this.m_harvester))
        return false;
      truck.EnqueueJob((VehicleJob) this.m_queueJobFactory.CreateJobForVehicleOwnedQueue<Vehicle, Truck>(truck, (VehicleQueue<Truck, Vehicle>) this.m_harvester.TruckQueue), false);
      return true;
    }

    private bool tryGetRidOfCargo(Truck truck)
    {
      if (truck.IsEmpty)
      {
        truck.DeactivateCannotDeliver();
        return false;
      }
      ForestryTower valueOrNull = this.m_harvester.AssignedTo.ValueOrNull as ForestryTower;
      ProductQuantity firstOrPhantom = truck.Cargo.FirstOrPhantom;
      ProductProto product = firstOrPhantom.Product;
      if (valueOrNull != null && product.Id == IdsCore.Products.Wood)
      {
        Set<IEntityAssignedAsInput> set = TreeHarvesterTruckJobProvider.s_storagesCache.Value.ClearAndReturn();
        foreach (IEntityAssignedAsInput assignedInputStorage in (IEnumerable<IEntityAssignedAsInput>) valueOrNull.AssignedInputStorages)
        {
          if (assignedInputStorage is Storage storage && storage.StoredProduct == product)
            set.Add((IEntityAssignedAsInput) storage);
        }
        if (set.IsNotEmpty)
        {
          Option<RegisteredInputBuffer> productInputForVehicle = this.VehicleBuffersRegistry.TryGetProductInputForVehicle((Vehicle) truck, firstOrPhantom, set.SomeOption<IReadOnlySet<IEntityAssignedAsInput>>(), out bool _, true, (IEntityAssignedAsOutput) valueOrNull);
          if (productInputForVehicle.HasValue)
          {
            this.DeliveryJobFactory.EnqueueJob(truck, firstOrPhantom, productInputForVehicle.Value);
            truck.DeactivateCannotDeliver();
            return true;
          }
          truck.NotifyIffCannotDeliver(true, truck.Cargo.FirstOrPhantom.Product);
          return false;
        }
      }
      bool deliveryIsGuaranteed;
      if (this.TryGetRidOfCargo(truck, false, out deliveryIsGuaranteed))
      {
        if (deliveryIsGuaranteed)
          truck.DeactivateCannotDeliver();
        return true;
      }
      truck.NotifyIffCannotDeliver(true, truck.Cargo.FirstOrPhantom.Product);
      return false;
    }

    static TreeHarvesterTruckJobProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreeHarvesterTruckJobProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TruckJobProviderBase) obj).SerializeData(writer));
      TreeHarvesterTruckJobProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TruckJobProviderBase) obj).DeserializeData(reader));
      TreeHarvesterTruckJobProvider.s_storagesCache = new ThreadLocal<Set<IEntityAssignedAsInput>>((Func<Set<IEntityAssignedAsInput>>) (() => new Set<IEntityAssignedAsInput>()));
    }
  }
}
