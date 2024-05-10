// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleJobStatsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class VehicleJobStatsManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Queueue<Dict<ProductProto, JobStatistics>> m_generalJobsStats;
    private readonly Queueue<Dict<ProductProto, JobStatistics>> m_miningJobsStats;
    private readonly Queueue<Dict<ProductProto, JobStatistics>> m_refuelingJobsStats;

    public static void Serialize(VehicleJobStatsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleJobStatsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleJobStatsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Queueue<Dict<ProductProto, JobStatistics>>.Serialize(this.m_generalJobsStats, writer);
      Queueue<Dict<ProductProto, JobStatistics>>.Serialize(this.m_miningJobsStats, writer);
      Queueue<Dict<ProductProto, JobStatistics>>.Serialize(this.m_refuelingJobsStats, writer);
    }

    public static VehicleJobStatsManager Deserialize(BlobReader reader)
    {
      VehicleJobStatsManager vehicleJobStatsManager;
      if (reader.TryStartClassDeserialization<VehicleJobStatsManager>(out vehicleJobStatsManager))
        reader.EnqueueDataDeserialization((object) vehicleJobStatsManager, VehicleJobStatsManager.s_deserializeDataDelayedAction);
      return vehicleJobStatsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehicleJobStatsManager>(this, "m_generalJobsStats", (object) Queueue<Dict<ProductProto, JobStatistics>>.Deserialize(reader));
      reader.SetField<VehicleJobStatsManager>(this, "m_miningJobsStats", (object) Queueue<Dict<ProductProto, JobStatistics>>.Deserialize(reader));
      reader.SetField<VehicleJobStatsManager>(this, "m_refuelingJobsStats", (object) Queueue<Dict<ProductProto, JobStatistics>>.Deserialize(reader));
    }

    public IIndexable<Dict<ProductProto, JobStatistics>> GeneralJobsStats
    {
      get => (IIndexable<Dict<ProductProto, JobStatistics>>) this.m_generalJobsStats;
    }

    public IIndexable<Dict<ProductProto, JobStatistics>> MiningJobsStats
    {
      get => (IIndexable<Dict<ProductProto, JobStatistics>>) this.m_miningJobsStats;
    }

    public IIndexable<Dict<ProductProto, JobStatistics>> RefuelingJobsStats
    {
      get => (IIndexable<Dict<ProductProto, JobStatistics>>) this.m_refuelingJobsStats;
    }

    public VehicleJobStatsManager(ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_generalJobsStats = new Queueue<Dict<ProductProto, JobStatistics>>();
      this.m_miningJobsStats = new Queueue<Dict<ProductProto, JobStatistics>>();
      this.m_refuelingJobsStats = new Queueue<Dict<ProductProto, JobStatistics>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < 12; ++index)
      {
        this.m_generalJobsStats.Enqueue(new Dict<ProductProto, JobStatistics>());
        this.m_miningJobsStats.Enqueue(new Dict<ProductProto, JobStatistics>());
        this.m_refuelingJobsStats.Enqueue(new Dict<ProductProto, JobStatistics>());
      }
      calendar.NewMonth.Add<VehicleJobStatsManager>(this, new Action(this.onNewMonth));
    }

    private void onNewMonth()
    {
      Dict<ProductProto, JobStatistics> dict1 = this.m_generalJobsStats.Dequeue();
      dict1.Clear();
      this.m_generalJobsStats.Enqueue(dict1);
      Dict<ProductProto, JobStatistics> dict2 = this.m_miningJobsStats.Dequeue();
      dict2.Clear();
      this.m_miningJobsStats.Enqueue(dict2);
      Dict<ProductProto, JobStatistics> dict3 = this.m_refuelingJobsStats.Dequeue();
      dict3.Clear();
      this.m_refuelingJobsStats.Enqueue(dict3);
    }

    public void RecordGeneralJobStats(ProductQuantity pq)
    {
      this.addTo(this.m_generalJobsStats, pq);
    }

    public void RecordMiningJobStats(ProductQuantity pq) => this.addTo(this.m_miningJobsStats, pq);

    public void RecordJobStatsFor(Truck truck, ProductQuantity pq)
    {
      if (pq.IsEmpty)
        return;
      if (pq.Product.IsPhantom)
      {
        Log.Error("Phantom product received");
      }
      else
      {
        Option<IEntityAssignedWithVehicles> assignedTo;
        if (pq.Product.Type == LooseProductProto.ProductType)
        {
          assignedTo = truck.AssignedTo;
          if (assignedTo.ValueOrNull is MineTower)
            goto label_6;
        }
        assignedTo = truck.AssignedTo;
        if (!(assignedTo.ValueOrNull is TreeHarvester))
        {
          this.RecordGeneralJobStats(pq);
          return;
        }
label_6:
        this.RecordMiningJobStats(pq);
      }
    }

    public void RecordRefuelingJobStats(ProductQuantity pq)
    {
      this.addTo(this.m_refuelingJobsStats, pq);
    }

    private void addTo(
      Queueue<Dict<ProductProto, JobStatistics>> statsToUse,
      ProductQuantity pq)
    {
      if (pq.IsEmpty)
        return;
      ref JobStatistics local = ref statsToUse.Last.GetRefValue(pq.Product, out bool _);
      local.Product = pq.Product;
      local.Quantity += pq.Quantity;
      ++local.JobsCount;
    }

    static VehicleJobStatsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleJobStatsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJobStatsManager) obj).SerializeData(writer));
      VehicleJobStatsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJobStatsManager) obj).DeserializeData(reader));
    }
  }
}
