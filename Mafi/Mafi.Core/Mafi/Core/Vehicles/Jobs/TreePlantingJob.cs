// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.TreePlantingJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>TreePlanter job for cutting down a single tree.</summary>
  [GenerateSerializer(false, null, 0)]
  public class TreePlantingJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly TickTimer m_timer;
    private readonly TreePlanter m_planter;
    private readonly Tile2i m_plantingPosition;
    private readonly TreeProto m_treeProto;
    private readonly TreePlantingJob.Factory m_factory;
    private readonly ITreePlantingManager m_treePlantingManager;
    private readonly RobustNavHelper m_navHelper;
    private TreePlantingJob.State m_state;
    private readonly Option<ForestryTower> m_tmpTower;

    public static void Serialize(TreePlantingJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreePlantingJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreePlantingJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      TreePlanter.Serialize(this.m_planter, writer);
      Tile2i.Serialize(this.m_plantingPosition, writer);
      writer.WriteInt((int) this.m_state);
      TickTimer.Serialize(this.m_timer, writer);
      Option<ForestryTower>.Serialize(this.m_tmpTower, writer);
      writer.WriteGeneric<ITreePlantingManager>(this.m_treePlantingManager);
      writer.WriteGeneric<TreeProto>(this.m_treeProto);
    }

    public static TreePlantingJob Deserialize(BlobReader reader)
    {
      TreePlantingJob treePlantingJob;
      if (reader.TryStartClassDeserialization<TreePlantingJob>(out treePlantingJob))
        reader.EnqueueDataDeserialization((object) treePlantingJob, TreePlantingJob.s_deserializeDataDelayedAction);
      return treePlantingJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.RegisterResolvedMember<TreePlantingJob>(this, "m_factory", typeof (TreePlantingJob.Factory), true);
      reader.SetField<TreePlantingJob>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      reader.SetField<TreePlantingJob>(this, "m_planter", (object) TreePlanter.Deserialize(reader));
      reader.SetField<TreePlantingJob>(this, "m_plantingPosition", (object) Tile2i.Deserialize(reader));
      this.m_state = (TreePlantingJob.State) reader.ReadInt();
      reader.SetField<TreePlantingJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<TreePlantingJob>(this, "m_tmpTower", (object) Option<ForestryTower>.Deserialize(reader));
      reader.SetField<TreePlantingJob>(this, "m_treePlantingManager", (object) reader.ReadGenericAs<ITreePlantingManager>());
      reader.SetField<TreePlantingJob>(this, "m_treeProto", (object) reader.ReadGenericAs<TreeProto>());
    }

    public override LocStrFormatted JobInfo => TreePlantingJob.s_jobInfo;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public Tile2i PlantingPosition => this.m_plantingPosition;

    public TreeProto PlantingProto => this.m_treeProto;

    public TreePlantingJob(
      VehicleJobId id,
      ITreePlantingManager treePlantingManager,
      TreePlantingJob.Factory factory,
      TreePlanter planter,
      TreeProto treeProto,
      Tile2i position,
      Option<ForestryTower> tmpTower)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_treePlantingManager = treePlantingManager;
      this.m_navHelper = new RobustNavHelper(factory.PathFindingManager);
      this.m_planter = planter.CheckNotNull<TreePlanter>();
      this.m_plantingPosition = position;
      this.m_treeProto = treeProto;
      this.m_tmpTower = tmpTower;
      this.m_state = TreePlantingJob.State.StartNav;
      if (this.m_plantingPosition.CenterTile2f.DistanceSqrTo(planter.Position2f) < (2 * this.m_planter.Prototype.TreePlantDistance).Squared)
        this.m_planter.SetCabinTarget(this.m_plantingPosition.CenterTile2f);
      planter.EnqueueJob((VehicleJob) this, false);
    }

    protected override bool DoJobInternal()
    {
      TreePlantingJob.State state;
      switch (this.m_state)
      {
        case TreePlantingJob.State.StartNav:
          state = this.handleStartNav();
          break;
        case TreePlantingJob.State.Navigating:
          state = this.handleNavigating();
          break;
        case TreePlantingJob.State.StartPlanting:
          state = this.handleStartPlanting();
          break;
        case TreePlantingJob.State.ContinuePlanting:
          state = this.handleContinuePlanting();
          break;
        case TreePlantingJob.State.Done:
          return false;
        default:
          Log.Error(string.Format("Unknown state '{0}'.", (object) this.m_state));
          this.m_state = TreePlantingJob.State.Done;
          goto case TreePlantingJob.State.Done;
      }
      this.m_state = state;
      return this.m_state != TreePlantingJob.State.Done;
    }

    private bool isTargetValid()
    {
      Option<ForestryTower> option = this.m_planter.ForestryTower.HasValue ? this.m_planter.ForestryTower : this.m_tmpTower;
      if (!option.HasValue)
        return this.m_treePlantingManager.HasReservedManualTree(this.m_plantingPosition);
      return option.Value.IsValidTileForPlanting(this.m_plantingPosition, this.m_treeProto.SpacingToOtherTree, (Option<TreePlanter>) this.m_planter) && option.Value.CanPlantTreeType(this.m_treeProto);
    }

    private TreePlantingJob.State handleStartNav()
    {
      if (!this.isTargetValid())
      {
        this.RequestCancel();
        return TreePlantingJob.State.Done;
      }
      this.m_navHelper.StartNavigationTo((Vehicle) this.m_planter, (IVehicleGoalFull) this.m_factory.TreeGoalFactory.Create(this.m_plantingPosition, this.m_planter.Prototype.TreePlantDistance));
      return TreePlantingJob.State.Navigating;
    }

    private TreePlantingJob.State handleNavigating()
    {
      if (!this.isTargetValid())
      {
        this.RequestCancel();
        return TreePlantingJob.State.Done;
      }
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return TreePlantingJob.State.Navigating;
        case RobustNavResult.GoalReachedSuccessfully:
          return TreePlantingJob.State.StartPlanting;
        case RobustNavResult.FailGoalUnreachable:
          this.RequestCancel();
          return TreePlantingJob.State.Done;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }
    }

    private TreePlantingJob.State handleStartPlanting()
    {
      if (!this.isTargetValid())
        return TreePlantingJob.State.Done;
      if (!this.m_planter.TryStartNearbyTreePlant(this.m_plantingPosition, this.m_treeProto))
      {
        Assert.Fail(string.Format("Failed to start tree planting at {0}.", (object) this.m_plantingPosition));
        this.RequestCancel();
        return TreePlantingJob.State.Done;
      }
      ref Pair<Tile2f, TreeProto>? local1 = ref this.m_planter.PlantingData;
      Assert.That<Tile2i?>(local1.HasValue ? new Tile2i?(local1.GetValueOrDefault().First.Tile2i) : new Tile2i?()).IsEqualTo<Tile2i?>(new Tile2i?(this.m_plantingPosition));
      ref Pair<Tile2f, TreeProto>? local2 = ref this.m_planter.PlantingData;
      Assert.That<TreeProto>(local2.HasValue ? local2.GetValueOrDefault().Second : (TreeProto) null).IsEqualTo<TreeProto>(this.m_treeProto);
      return TreePlantingJob.State.ContinuePlanting;
    }

    private TreePlantingJob.State handleContinuePlanting()
    {
      return !this.m_planter.PlantingData.HasValue ? TreePlantingJob.State.Done : TreePlantingJob.State.ContinuePlanting;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_treePlantingManager.TryCancelReserveManualTree(this.m_plantingPosition);
      ((IVehicleFriend) this.m_planter).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.m_state = TreePlantingJob.State.Done;
      this.m_timer.Reset();
    }

    static TreePlantingJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreePlantingJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      TreePlantingJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      TreePlantingJob.s_jobInfo = new LocStrFormatted("Tree planting.");
    }

    private enum State
    {
      StartNav,
      Navigating,
      StartPlanting,
      ContinuePlanting,
      Done,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      public readonly PlantingVehicleGoal.Factory TreeGoalFactory;
      public readonly IVehiclePathFindingManager PathFindingManager;
      private readonly ITreePlantingManager m_treePlantingManager;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly UnreachableTerrainDesignationsManager UnreachablesManager;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        PlantingVehicleGoal.Factory treeGoalFactory,
        IVehiclePathFindingManager pathFindingManager,
        ITreePlantingManager treePlantingManager,
        UnreachableTerrainDesignationsManager unreachablesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.TreeGoalFactory = treeGoalFactory;
        this.PathFindingManager = pathFindingManager;
        this.m_treePlantingManager = treePlantingManager;
        this.UnreachablesManager = unreachablesManager;
      }

      public void EnqueueJob(
        TreePlanter planter,
        TreeProto treeProto,
        Tile2i position,
        Option<ForestryTower> tmpTower)
      {
        TreePlantingJob treePlantingJob = new TreePlantingJob(this.m_vehicleJobIdFactory.GetNextId(), this.m_treePlantingManager, this, planter, treeProto, position, tmpTower);
      }
    }
  }
}
