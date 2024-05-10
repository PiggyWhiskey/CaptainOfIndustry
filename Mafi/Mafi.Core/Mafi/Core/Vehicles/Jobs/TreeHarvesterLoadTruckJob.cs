// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.TreeHarvesterLoadTruckJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// TreeHarvester job for moving harvester's cargo to a truck.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class TreeHarvesterLoadTruckJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly TreeHarvester m_harvester;
    private Option<Truck> m_selectedTruck;
    private ProductQuantity m_cargoBeforeUnload;
    private readonly TreeHarvesterLoadTruckJob.Factory m_factory;

    public static void Serialize(TreeHarvesterLoadTruckJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreeHarvesterLoadTruckJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreeHarvesterLoadTruckJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductQuantity.Serialize(this.m_cargoBeforeUnload, writer);
      TreeHarvester.Serialize(this.m_harvester, writer);
      Option<Truck>.Serialize(this.m_selectedTruck, writer);
    }

    public static TreeHarvesterLoadTruckJob Deserialize(BlobReader reader)
    {
      TreeHarvesterLoadTruckJob harvesterLoadTruckJob;
      if (reader.TryStartClassDeserialization<TreeHarvesterLoadTruckJob>(out harvesterLoadTruckJob))
        reader.EnqueueDataDeserialization((object) harvesterLoadTruckJob, TreeHarvesterLoadTruckJob.s_deserializeDataDelayedAction);
      return harvesterLoadTruckJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_cargoBeforeUnload = ProductQuantity.Deserialize(reader);
      reader.RegisterResolvedMember<TreeHarvesterLoadTruckJob>(this, "m_factory", typeof (TreeHarvesterLoadTruckJob.Factory), true);
      reader.SetField<TreeHarvesterLoadTruckJob>(this, "m_harvester", (object) TreeHarvester.Deserialize(reader));
      this.m_selectedTruck = Option<Truck>.Deserialize(reader);
    }

    public override LocStrFormatted JobInfo => TreeHarvesterLoadTruckJob.s_jobInfo;

    public override bool SkipNoMovementMonitoring => this.m_selectedTruck.IsNone;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption
    {
      get
      {
        return !this.m_selectedTruck.IsNone ? VehicleFuelConsumption.Full : VehicleFuelConsumption.Idle;
      }
    }

    public TreeHarvesterLoadTruckJob(
      VehicleJobId id,
      TreeHarvesterLoadTruckJob.Factory factory,
      TreeHarvester harvester)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_cargoBeforeUnload = ProductQuantity.None;
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_harvester = harvester.CheckNotNull<TreeHarvester>();
      Assert.That<ProductQuantity>(harvester.Cargo).IsNotEmpty();
      harvester.EnqueueJob((VehicleJob) this, false);
    }

    protected override bool DoJobInternal()
    {
      if (this.m_selectedTruck.HasValue && this.m_selectedTruck.Value.AssignedTo.ValueOrNull != this.m_harvester || !this.m_harvester.TruckQueue.IsEnabled || !this.m_harvester.TruckQueue.IsFirstVehicle(this.m_selectedTruck.ValueOrNull))
      {
        this.m_selectedTruck = Option<Truck>.None;
        this.m_cargoBeforeUnload = ProductQuantity.None;
        if (this.m_harvester.Cargo.IsEmpty)
          return false;
      }
      if (this.m_selectedTruck.IsNone)
      {
        Assert.That<ProductQuantity>(this.m_harvester.Cargo).IsNotEmpty("Empty cargo but waiting for a truck.");
        this.m_harvester.TruckQueue.Enable();
        Option<Truck> firstTruckFor = this.m_harvester.TruckQueue.TryGetFirstTruckFor(this.m_harvester.Cargo.Product);
        if (firstTruckFor.IsNone)
          return true;
        Assert.That<bool>(firstTruckFor.Value.IsFull).IsFalse("Full truck received from queue.");
        if (this.m_harvester.TruckQueue.FirstVehicleReadyAtQueueTip())
        {
          this.m_cargoBeforeUnload = this.m_harvester.Cargo;
          this.m_harvester.StartCargoUnloadTo(firstTruckFor.Value);
          this.m_harvester.SetCabinTarget(firstTruckFor.Value.Position2f);
          this.m_selectedTruck = (Option<Truck>) firstTruckFor.Value;
        }
        return true;
      }
      if (this.m_selectedTruck.Value.IsFull)
        this.m_harvester.TruckQueue.ReleaseFirstVehicle();
      else if (this.m_cargoBeforeUnload == this.m_harvester.Cargo)
      {
        Log.Error(string.Format("Failed to load {0} on a truck with {1}. ", (object) this.m_harvester.Cargo, (object) this.m_selectedTruck.Value.Cargo) + "Releasing from queue.");
        this.m_harvester.TruckQueue.ReleaseFirstVehicle();
      }
      return false;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_selectedTruck = Option<Truck>.None;
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.m_selectedTruck = (Option<Truck>) Option.None;
      this.m_cargoBeforeUnload = ProductQuantity.None;
    }

    static TreeHarvesterLoadTruckJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreeHarvesterLoadTruckJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      TreeHarvesterLoadTruckJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      TreeHarvesterLoadTruckJob.s_jobInfo = new LocStrFormatted("Loading wood to truck.");
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;

      public Factory(VehicleJobId.Factory vehicleJobIdFactory)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
      }

      public void EnqueueJob(TreeHarvester harvester)
      {
        TreeHarvesterLoadTruckJob harvesterLoadTruckJob = new TreeHarvesterLoadTruckJob(this.m_vehicleJobIdFactory.GetNextId(), this, harvester);
      }
    }
  }
}
