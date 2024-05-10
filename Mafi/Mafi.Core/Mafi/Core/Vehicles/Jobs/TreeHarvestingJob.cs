// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.TreeHarvestingJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>TreeHarvester job for cutting down a single tree.</summary>
  [GenerateSerializer(false, null, 0)]
  public class TreeHarvestingJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly TickTimer m_timer;
    private readonly TreeHarvester m_harvester;
    private readonly Option<ForestryTower> m_tmpForestryTower;
    private readonly TreeId m_tree;
    private readonly TreeHarvestingJob.Factory m_factory;
    private readonly RobustNavHelper m_navHelper;
    private TreeHarvestingJob.State m_state;

    public static void Serialize(TreeHarvestingJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreeHarvestingJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreeHarvestingJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      TreeHarvester.Serialize(this.m_harvester, writer);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      writer.WriteInt((int) this.m_state);
      TickTimer.Serialize(this.m_timer, writer);
      Option<ForestryTower>.Serialize(this.m_tmpForestryTower, writer);
      TreeId.Serialize(this.m_tree, writer);
    }

    public static TreeHarvestingJob Deserialize(BlobReader reader)
    {
      TreeHarvestingJob treeHarvestingJob;
      if (reader.TryStartClassDeserialization<TreeHarvestingJob>(out treeHarvestingJob))
        reader.EnqueueDataDeserialization((object) treeHarvestingJob, TreeHarvestingJob.s_deserializeDataDelayedAction);
      return treeHarvestingJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.RegisterResolvedMember<TreeHarvestingJob>(this, "m_factory", typeof (TreeHarvestingJob.Factory), true);
      reader.SetField<TreeHarvestingJob>(this, "m_harvester", (object) TreeHarvester.Deserialize(reader));
      reader.SetField<TreeHarvestingJob>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      this.m_state = (TreeHarvestingJob.State) reader.ReadInt();
      reader.SetField<TreeHarvestingJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<TreeHarvestingJob>(this, "m_tmpForestryTower", (object) Option<ForestryTower>.Deserialize(reader));
      reader.SetField<TreeHarvestingJob>(this, "m_tree", (object) TreeId.Deserialize(reader));
      reader.RegisterInitAfterLoad<TreeHarvestingJob>(this, "initSelf", InitPriority.Normal);
    }

    public override LocStrFormatted JobInfo => TreeHarvestingJob.s_jobInfo;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public TreeHarvestingJob(
      VehicleJobId id,
      TreeHarvestingJob.Factory factory,
      TreeHarvester harvester,
      TreeId tree,
      Option<ForestryTower> tmpTower)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_navHelper = new RobustNavHelper(factory.PathFindingManager);
      Assert.That<bool>(this.m_factory.HarvestingManager.HasTree(tree)).IsTrue("Invalid tree given to tree harvesting job.");
      this.m_harvester = harvester.CheckNotNull<TreeHarvester>();
      this.m_tree = tree;
      this.m_tmpForestryTower = tmpTower;
      this.m_state = TreeHarvestingJob.State.StartNav;
      this.m_factory.HarvestingManager.TryReserveTree(tree);
      if (tree.Position.CenterTile2f.DistanceSqrTo(harvester.Position2f) < (2 * this.m_harvester.Prototype.TreeHarvestDistance).Squared)
        this.m_harvester.SetCabinTarget(tree.Position.CenterTile2f);
      harvester.EnqueueJob((VehicleJob) this, false);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      if (this.m_navHelper != null)
        return;
      ReflectionUtils.SetField<TreeHarvestingJob>(this, "m_navHelper", (object) new RobustNavHelper(this.m_factory.PathFindingManager));
      this.m_state = TreeHarvestingJob.State.StartNav;
    }

    protected override bool DoJobInternal()
    {
      TreeHarvestingJob.State state;
      switch (this.m_state)
      {
        case TreeHarvestingJob.State.StartNav:
          state = this.handleStartNav();
          break;
        case TreeHarvestingJob.State.Navigating:
          state = this.handleNavigating();
          break;
        case TreeHarvestingJob.State.StartHarvesting:
          state = this.handleStartHarvesting();
          break;
        case TreeHarvestingJob.State.ContinueHarvesting:
          state = this.handleContinueHarvesting();
          break;
        case TreeHarvestingJob.State.Done:
          return false;
        default:
          Log.Error(string.Format("Unknown state '{0}'.", (object) this.m_state));
          this.m_state = TreeHarvestingJob.State.Done;
          goto case TreeHarvestingJob.State.Done;
      }
      this.m_state = state;
      return this.m_state != TreeHarvestingJob.State.Done;
    }

    private bool isTreeSelected()
    {
      if (this.m_harvester.ForestryTower.HasValue)
      {
        if (!this.m_harvester.ForestryTower.Value.IsTreeReadyForHarvest(this.m_tree))
          return false;
      }
      else if (this.m_tmpForestryTower.HasValue)
      {
        if (!this.m_tmpForestryTower.Value.IsTreeReadyForHarvest(this.m_tree))
          return false;
      }
      else if (!this.m_factory.HarvestingManager.IsTreeSelected(this.m_tree))
        return false;
      return true;
    }

    private TreeHarvestingJob.State handleStartNav()
    {
      if (!this.isTreeSelected())
      {
        this.RequestCancel();
        return TreeHarvestingJob.State.Done;
      }
      this.m_navHelper.StartNavigationTo((Vehicle) this.m_harvester, (IVehicleGoalFull) this.m_factory.TreeGoalFactory.Create(this.m_tree, this.m_harvester.Prototype.TreeHarvestDistance));
      return TreeHarvestingJob.State.Navigating;
    }

    private TreeHarvestingJob.State handleNavigating()
    {
      if (!this.isTreeSelected())
      {
        this.RequestCancel();
        return TreeHarvestingJob.State.Done;
      }
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          if (this.m_harvester.PfState != PathFindingEntityState.PathFinding)
            this.m_harvester.DidNotFindTreeToHarvest = false;
          return TreeHarvestingJob.State.Navigating;
        case RobustNavResult.GoalReachedSuccessfully:
          this.m_harvester.DidNotFindTreeToHarvest = false;
          return TreeHarvestingJob.State.StartHarvesting;
        case RobustNavResult.FailGoalUnreachable:
          this.RequestCancel();
          return TreeHarvestingJob.State.Done;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }
    }

    private TreeHarvestingJob.State handleStartHarvesting()
    {
      if (!this.m_harvester.TryStartNearbyTreeHarvest(this.m_tree))
      {
        Assert.Fail(string.Format("Failed to start tree harvesting of {0}.", (object) this.m_tree));
        this.RequestCancel();
        return TreeHarvestingJob.State.Done;
      }
      ref TreeData? local = ref this.m_harvester.TreeToBeCut;
      Assert.That<TreeId?>(local.HasValue ? new TreeId?(local.GetValueOrDefault().Id) : new TreeId?()).IsEqualTo<TreeId?>(new TreeId?(this.m_tree));
      return TreeHarvestingJob.State.ContinueHarvesting;
    }

    private TreeHarvestingJob.State handleContinueHarvesting()
    {
      return !this.m_harvester.TreeToBeCut.HasValue ? TreeHarvestingJob.State.Done : TreeHarvestingJob.State.ContinueHarvesting;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_factory.HarvestingManager.TryCancelTreeReservation(this.m_tree);
      this.m_navHelper.CancelAndClear();
      ((IVehicleFriend) this.m_harvester).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.m_navHelper.Clear();
      this.m_factory.HarvestingManager.TryCancelTreeReservation(this.m_tree);
      this.m_state = TreeHarvestingJob.State.Done;
      this.m_timer.Reset();
    }

    static TreeHarvestingJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreeHarvestingJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      TreeHarvestingJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      TreeHarvestingJob.s_jobInfo = new LocStrFormatted("Tree harvesting.");
    }

    private enum State
    {
      StartNav,
      Navigating,
      StartHarvesting,
      ContinueHarvesting,
      Done,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      public readonly TreeVehicleGoal.Factory TreeGoalFactory;
      public readonly ITreeHarvestingManager HarvestingManager;
      public readonly IVehiclePathFindingManager PathFindingManager;
      public readonly UnreachableTerrainDesignationsManager UnreachablesManager;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;

      public Factory(
        ITreeHarvestingManager harvestingManager,
        VehicleJobId.Factory vehicleJobIdFactory,
        TreeVehicleGoal.Factory treeGoalFactory,
        IVehiclePathFindingManager pathFindingManager,
        UnreachableTerrainDesignationsManager unreachablesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.HarvestingManager = harvestingManager;
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.TreeGoalFactory = treeGoalFactory;
        this.PathFindingManager = pathFindingManager;
        this.UnreachablesManager = unreachablesManager;
      }

      public void EnqueueJob(TreeHarvester harvester, TreeId tree, Option<ForestryTower> tower)
      {
        TreeHarvestingJob treeHarvestingJob = new TreeHarvestingJob(this.m_vehicleJobIdFactory.GetNextId(), this, harvester, tree, tower);
      }
    }
  }
}
