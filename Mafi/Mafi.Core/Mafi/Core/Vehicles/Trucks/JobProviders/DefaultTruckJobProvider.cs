// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.JobProviders.DefaultTruckJobProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks.JobProviders
{
  /// <summary>
  /// Truck jobs provider able to provide jobs to trucks assigned to entities inheriting StorageBase.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class DefaultTruckJobProvider : TruckJobProviderBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private int m_dumpingJobsScheduled;
    private int m_waitFor;

    public static void Serialize(DefaultTruckJobProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DefaultTruckJobProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DefaultTruckJobProvider.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.m_dumpingJobsScheduled);
      writer.WriteInt(this.m_waitFor);
    }

    public static DefaultTruckJobProvider Deserialize(BlobReader reader)
    {
      DefaultTruckJobProvider truckJobProvider;
      if (reader.TryStartClassDeserialization<DefaultTruckJobProvider>(out truckJobProvider))
        reader.EnqueueDataDeserialization((object) truckJobProvider, DefaultTruckJobProvider.s_deserializeDataDelayedAction);
      return truckJobProvider;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_dumpingJobsScheduled = reader.ReadInt();
      this.m_waitFor = reader.ReadInt();
    }

    public DefaultTruckJobProvider(TruckJobProviderContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(context);
    }

    public override bool TryGetJobFor(Truck truck)
    {
      if (truck.IsOnWayToDepotForScrap)
      {
        Assert.Fail("Truck is going to depot for scrap");
        truck.CancelScrap();
      }
      if (truck.IsOnWayToDepotForReplacement)
      {
        Assert.Fail("Truck is going to depot for replacement");
        truck.CancelReplaceEnRoute();
      }
      if (truck.HasJobs)
      {
        Assert.Fail("The truck already has a job assigned!");
        return false;
      }
      bool hasJob;
      if (this.TryGetVehicleRefuelingJob((Vehicle) truck, out hasJob))
        return hasJob;
      if (this.TryGetRidOfCargoAndUpdateCannotDeliverNotif(truck, !truck.DumpingOfAllCargoPending))
        return true;
      if (truck.IsNotEmpty)
        return false;
      this.Context.VehicleLastOutputBufferManager.ClearOutputBufferFor((Vehicle) truck);
      Assert.That<bool>(truck.IsDriving).IsFalse();
      this.VehicleBuffersRegistry.RegisterTruckForBalancingJob(truck);
      if (truck.LayoutEntity.HasValue)
        return this.TryGetNavigateToJob((Vehicle) truck, truck.LayoutEntity.Value, false);
      if (!truck.AssignedTo.HasValue || !(truck.AssignedTo.ValueOrNull is ILayoutEntity valueOrNull))
        return false;
      ParkAndWaitJobFactory andWaitJobFactory = this.ParkAndWaitJobFactory;
      Truck truck1 = truck;
      RelTile1f? parkingDistance = new RelTile1f?();
      return andWaitJobFactory.TryEnqueueParkingJobIfNeeded((Vehicle) truck1, valueOrNull, parkingDistance);
    }

    public void AssignBalancingJob(BalancingJobSpec spec)
    {
      Truck truck = spec.Truck;
      if (!truck.IsAvailableToBalanceCargo())
      {
        Log.Error("Trying to assign job to a truck that is not available!");
      }
      else
      {
        if (truck.HasJobs)
        {
          Assert.That<bool>(truck.HasTrueJob).IsFalse();
          truck.CancelAllJobsAndResetState();
        }
        if (spec.DumpDesignation.HasValue)
        {
          CargoPickUpJob firstJob = enqueuePickupJob(spec.OutputBuffer.Value);
          Assert.That<bool>(spec.ProductQuantity.Product is LooseProductProto).IsTrue();
          DumpingJob secondJob = this.DumpJobFactory.EnqueueJob(truck, spec.ProductQuantity.Product as LooseProductProto, spec.DumpDesignation.Value, (IEnumerable<TerrainDesignation>) spec.ExtraDumpDesignations.ValueOrNull);
          this.Context.ChainedNavJobFactory.EnqueueAsFirstJob((Vehicle) truck, (IJobWithPreNavigation) firstJob, (IJobWithPreNavigation) secondJob);
        }
        else if (spec.SurfacePlaceDesignations.HasValue && spec.SurfaceClearDesignations.HasValue)
        {
          this.SurfaceJobFactory.EnqueueJob(true, spec.ProductQuantity, truck, (IEnumerable<SurfaceDesignation>) spec.SurfaceClearDesignations.Value);
          this.SurfaceJobFactory.EnqueueJob(false, spec.ProductQuantity, truck, (IEnumerable<SurfaceDesignation>) spec.SurfacePlaceDesignations.Value);
        }
        else if (spec.SurfacePlaceDesignations.HasValue && spec.OutputBuffer.HasValue)
        {
          CargoPickUpJob firstJob = enqueuePickupJob(spec.OutputBuffer.Value);
          SurfaceModificationJob secondJob = this.SurfaceJobFactory.EnqueueJob(false, spec.ProductQuantity, truck, (IEnumerable<SurfaceDesignation>) spec.SurfacePlaceDesignations.Value);
          this.Context.ChainedNavJobFactory.EnqueueAsFirstJob((Vehicle) truck, (IJobWithPreNavigation) firstJob, (IJobWithPreNavigation) secondJob);
        }
        else if (spec.SurfaceClearDesignations.HasValue && spec.InputBuffer.HasValue)
        {
          this.SurfaceJobFactory.EnqueueJob(true, spec.ProductQuantity, truck, (IEnumerable<SurfaceDesignation>) spec.SurfaceClearDesignations.Value);
          enqueueDeliveryJob(spec.InputBuffer.Value);
        }
        else if (spec.OutputBuffer.HasValue && spec.InputBuffer.HasValue)
        {
          CargoPickUpJob firstJob = enqueuePickupJob(spec.OutputBuffer.Value);
          CargoDeliveryJob secondJob = enqueueDeliveryJob(spec.InputBuffer.Value);
          this.Context.ChainedNavJobFactory.EnqueueAsFirstJob((Vehicle) truck, (IJobWithPreNavigation) firstJob, (IJobWithPreNavigation) secondJob);
        }
        else
          Log.Error("Unknown job spec state!");
      }

      CargoPickUpJob enqueuePickupJob(RegisteredOutputBuffer buffer)
      {
        ProductQuantity toPickup = spec.ProductQuantity + spec.GetSecondaryInputsQuantitySum();
        this.Context.VehicleLastOutputBufferManager.ReportOutputBufferFor((Vehicle) truck, buffer);
        return this.Context.PickUpJobFactory.EnqueueJob((IVehicleForCargoJob) truck, toPickup, buffer, spec.SecondaryOutputBuffers);
      }

      CargoDeliveryJob enqueueDeliveryJob(RegisteredInputBuffer buffer)
      {
        ProductQuantity toDeliver = spec.ProductQuantity + spec.GetSecondaryOutputsQuantitySum();
        return this.Context.DeliveryJobFactory.EnqueueJob(truck, toDeliver, buffer, spec.SecondaryInputBuffers.ValueOrNull);
      }
    }

    public void ClearCachedBuffer(Truck truck)
    {
      this.Context.VehicleLastOutputBufferManager.ClearOutputBufferFor((Vehicle) truck);
    }

    static DefaultTruckJobProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DefaultTruckJobProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TruckJobProviderBase) obj).SerializeData(writer));
      DefaultTruckJobProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TruckJobProviderBase) obj).DeserializeData(reader));
    }
  }
}
