// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.VehiclePathFindingManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Console;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Roads;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.PathFinding;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#nullable disable
namespace Mafi.Core.PathFinding
{
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("m_fulfilledDesignations", 132, typeof (Queueue<TerrainDesignation>), 0, false)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class VehiclePathFindingManager : IVehiclePathFindingManager
  {
    private static readonly Duration MIN_PF_DURATION_TICKS;
    private static readonly RelTile1i EXTRA_PF_DURATION_TICK_PER_LENGTH;
    public const int DEFAULT_STEPS_PER_UPDATE = 2000;
    public const int EXTRA_STEPS_PER_QUEUED_VEHICLE = 20;
    [DoNotSave(0, null)]
    private readonly IVehiclePathFinder m_vehiclePathFinder;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly IEntitiesManager m_entitiesManager;
    [OnlyForSaveCompatibility(null)]
    private readonly LazyResolve<ReturnHomeJob.Factory> m_returnHomeJobFactory;
    private readonly ITreesManager m_treeManager;
    private readonly StaticEntityVehicleGoal.Factory m_staticEntityGoalFactory;
    private readonly IRandom m_random;
    [NewInSaveVersion(168, null, null, typeof (IVehicleSurfaceProvider), null)]
    private readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
    private Option<IStaticEntity> m_defaultHomeEntity;
    /// <summary>Maximum total steps for the entire PF task.</summary>
    private int m_currentPfStepsRemaining;
    [DoNotSave(106, null)]
    private readonly Queueue<IManagedVehiclePathFindingTask> m_waitingTasks;
    [NewInSaveVersion(109, null, "new()", null, null)]
    private readonly Set<EntityId> m_failedNavsVehicles;
    /// <summary>Tasks waiting to be processed.</summary>
    [NewInSaveVersion(106, null, "new()", null, null)]
    private readonly Heap<IManagedVehiclePathFindingTask> m_waitingTasksQueue;
    /// <summary>
    /// Task of currently ongoing path-finding. This is to avoid passing it around. May be null if no path-finding is
    /// currently ongoing.
    /// </summary>
    private Option<IManagedVehiclePathFindingTask> m_currentTask;
    [DoNotSave(0, null)]
    private Option<Lyst<VehiclePathFindingManager.PerfData>> m_perfData;
    [DoNotSave(0, null)]
    private Stopwatch m_stopwatch;
    [DoNotSaveCreateNewOnLoad("20", 0)]
    private int m_crazyBusyReported;
    [DoNotSave(0, null)]
    private int m_crazyBusyDuration;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int MaxStepsPerUpdate { get; private set; }

    public int QueueLength => this.m_waitingTasksQueue.Count;

    public IPathabilityProvider PathabilityProvider => this.m_vehiclePathFinder.PathabilityProvider;

    internal IIndexable<KeyValuePair<Fix32, IManagedVehiclePathFindingTask>> TestOnly_TasksQueue
    {
      get
      {
        return (IIndexable<KeyValuePair<Fix32, IManagedVehiclePathFindingTask>>) this.m_waitingTasksQueue;
      }
    }

    internal Option<IManagedVehiclePathFindingTask> TestOnly_CurrentTask => this.m_currentTask;

    public bool HasMoreTasksToProcess
    {
      get => this.m_currentTask.HasValue || this.m_waitingTasksQueue.IsNotEmpty;
    }

    /// <summary>This gets incremented every time PF task completes.</summary>
    public int CompletedPfTasks { get; private set; }

    public int CompletedUnreachableGoalTasks { get; private set; }

    public VehiclePathFindingManager(
      IVehiclePathFinder vehiclePathFinder,
      ISimLoopEvents simLoopEvents,
      IEntitiesManager entitiesManager,
      LazyResolve<ReturnHomeJob.Factory> returnHomeJobFactory,
      RandomProvider randomProvider,
      ITreesManager treeManager,
      IVehicleSurfaceProvider vehicleSurfaceProvider,
      StaticEntityVehicleGoal.Factory staticEntityGoalFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxStepsPerUpdate\u003Ek__BackingField = 2000;
      this.m_waitingTasks = new Queueue<IManagedVehiclePathFindingTask>();
      this.m_failedNavsVehicles = new Set<EntityId>();
      this.m_waitingTasksQueue = new Heap<IManagedVehiclePathFindingTask>();
      this.m_crazyBusyReported = 20;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_vehiclePathFinder = vehiclePathFinder;
      this.m_simLoopEvents = simLoopEvents;
      this.m_entitiesManager = entitiesManager;
      this.m_returnHomeJobFactory = returnHomeJobFactory;
      this.m_treeManager = treeManager;
      this.m_staticEntityGoalFactory = staticEntityGoalFactory;
      this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      simLoopEvents.Update.Add<VehiclePathFindingManager>(this, new Action(this.update));
      simLoopEvents.BeforeSave.Add<VehiclePathFindingManager>(this, new Action(this.beforeSave));
    }

    [InitAfterLoad(InitPriority.High)]
    private void initAfterLoad(int saveVersion, DependencyResolver resolver)
    {
      ReflectionUtils.SetField<VehiclePathFindingManager>(this, "m_vehiclePathFinder", (object) resolver.Resolve<IVehiclePathFinder>());
      if (saveVersion < 106)
      {
        foreach (IManagedVehiclePathFindingTask waitingTask in this.m_waitingTasks)
          this.m_waitingTasksQueue.Push((Fix32) 0, waitingTask);
        this.m_waitingTasks.Clear();
      }
      if (saveVersion < 109)
        this.MaxStepsPerUpdate = 2000;
      if (saveVersion >= 132)
        return;
      resolver.Resolve<ITerrainDesignationsManager>().DesignationFulfilledChanged.Remove<VehiclePathFindingManager>(this, new Action<TerrainDesignation>(this.terrainDesignationFulfilledChanged));
    }

    private void beforeSave()
    {
      if (this.m_currentTask.HasValue)
      {
        this.m_vehiclePathFinder.ResetState();
        this.m_currentTask.Value.SetIsWaitingForProcessing((IVehiclePathFindingManager) this);
        this.m_waitingTasksQueue.Push(Fix32.MinValue, this.m_currentTask.Value);
        this.m_currentTask.Value.ResetToStart();
        this.m_currentTask = Option<IManagedVehiclePathFindingTask>.None;
      }
      this.m_failedNavsVehicles.Clear();
    }

    [ConsoleCommand(false, false, null, null)]
    private string printVehiclePathFinderQueue()
    {
      StringBuilder stringBuilder = new StringBuilder(1024);
      foreach (KeyValuePair<Fix32, IManagedVehiclePathFindingTask> keyValuePair in (IEnumerable<KeyValuePair<Fix32, IManagedVehiclePathFindingTask>>) this.m_waitingTasksQueue.OrderBy<KeyValuePair<Fix32, IManagedVehiclePathFindingTask>, Fix32>((Func<KeyValuePair<Fix32, IManagedVehiclePathFindingTask>, Fix32>) (x => x.Key)))
        stringBuilder.AppendLine(string.Format("{0}", (object) keyValuePair.Key) + (this.m_failedNavsVehicles.Contains(keyValuePair.Value.Vehicle.Id) ? " (failed before)" : "") + string.Format(" {0}", (object) keyValuePair.Value.Vehicle));
      return string.Format("PF queue length: {0}\n{1}", (object) this.QueueLength, (object) stringBuilder);
    }

    [ConsoleCommand(false, false, null, null)]
    private string setMaxStepsPerTickForVehiclePathFinder(int stepsPerUpdate)
    {
      int maxStepsPerUpdate = this.MaxStepsPerUpdate;
      this.MaxStepsPerUpdate = stepsPerUpdate.Max(100);
      return string.Format("Steps per tick changed from {0} to {1} (default value is {2}).", (object) maxStepsPerUpdate, (object) this.MaxStepsPerUpdate, (object) 2000);
    }

    public void EnqueueTask(IManagedVehiclePathFindingTask task, int priority)
    {
      Mafi.Assert.That<bool>(task.HasResult).IsFalse();
      Mafi.Assert.That<IIndexable<Tile2i>>(task.GoalTiles).IsEmpty<Tile2i>("Forgot to clear?");
      if (this.m_waitingTasksQueue.Count > this.m_crazyBusyReported)
      {
        ++this.m_crazyBusyDuration;
        if (this.m_crazyBusyDuration > 300)
        {
          this.m_crazyBusyReported += 30;
          this.m_crazyBusyDuration = 0;
          Mafi.Log.Warning(string.Format("Path finder crazy busy: {0} tasks waiting in the queue ", (object) this.m_waitingTasksQueue.Count) + "for more than 30 seconds.");
        }
      }
      else
        this.m_crazyBusyDuration = 0;
      task.SetIsWaitingForProcessing((IVehiclePathFindingManager) this);
      int num1 = task.GetGoalOrthogonalDistance().Value;
      int num2 = this.m_simLoopEvents.CurrentStep.Value + priority + num1 / 20;
      if (this.m_failedNavsVehicles.Contains(task.Vehicle.Id))
        num2 += 100 + num1 / 20;
      else if (num1 < 32 && this.m_waitingTasksQueue.IsNotEmpty)
      {
        int rawValue = this.m_waitingTasksQueue.PeekMin().Key.RawValue;
        num2 = num2.Min(rawValue + 10 + priority);
      }
      this.m_waitingTasksQueue.Push(Fix32.FromRaw(num2), task);
    }

    public void AbortTask(IManagedVehiclePathFindingTask task)
    {
      Mafi.Assert.That<bool>(task.HasResult).IsFalse();
      if (this.m_currentTask == task)
      {
        this.m_currentTask = Option<IManagedVehiclePathFindingTask>.None;
        this.m_vehiclePathFinder.ResetState();
      }
      else
      {
        Mafi.Assert.That<bool>(this.m_waitingTasksQueue.TryRemove(task)).IsTrue("Removing PF task that was not active.");
        task.SetIsBeingProcessed();
      }
      task.SetResult(this.m_vehiclePathFinder, VehiclePfResultStatus.Aborted);
    }

    public Tile2i? FindClosestValidPfNode(
      Tile2i coord,
      VehiclePathFindingParams pfParams,
      Predicate<PfNode> predicate = null)
    {
      return this.m_vehiclePathFinder.FindClosestValidPosition(coord, pfParams, predicate);
    }

    public Option<IStaticEntity> GetHomeEntityFor(Vehicle vehicle)
    {
      if (vehicle.AssignedTo.HasValue)
      {
        if (vehicle.AssignedTo.Value is IStaticEntity staticEntity)
          return Option.Some<IStaticEntity>(staticEntity);
        if (vehicle.AssignedTo.Value is Vehicle vehicle1)
          return this.GetHomeEntityFor(vehicle1);
        Mafi.Log.Error("Unsupported assignee type for home entity " + vehicle.AssignedTo.Value.GetType().Name);
      }
      return this.getDefaultHomeEntity();
    }

    private Option<IStaticEntity> getDefaultHomeEntity()
    {
      if (this.m_defaultHomeEntity.IsNone || this.m_defaultHomeEntity.Value.IsDestroyed)
        this.m_defaultHomeEntity = (Option<IStaticEntity>) (IStaticEntity) this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Buildings.Shipyard.Shipyard>().FirstOrDefault<Mafi.Core.Buildings.Shipyard.Shipyard>();
      return this.m_defaultHomeEntity;
    }

    internal void TestOnly_SetDefaultHomeEntity(IStaticEntity homeEntity)
    {
      Mafi.Assert.That<ConstructionState>(homeEntity.ConstructionState).IsEqualTo<ConstructionState>(ConstructionState.Constructed);
      Mafi.Assert.That<bool>(homeEntity.IsDestroyed).IsFalse();
      this.m_defaultHomeEntity = Option.Some<IStaticEntity>(homeEntity);
    }

    internal void TestOnly_SetMaxStepsPerUpdate(int newMaxStepsPerUpdate)
    {
      this.MaxStepsPerUpdate = newMaxStepsPerUpdate.CheckPositive();
    }

    public Lyst<VehiclePathFindingManager.PerfData> EnablePerformanceTracking()
    {
      this.m_perfData = (Option<Lyst<VehiclePathFindingManager.PerfData>>) new Lyst<VehiclePathFindingManager.PerfData>();
      this.m_stopwatch = new Stopwatch();
      return this.m_perfData.Value;
    }

    private void update()
    {
      int stepsLeft = this.MaxStepsPerUpdate + this.m_waitingTasksQueue.Count * 20;
      this.updatePathFindingTasks(ref stepsLeft);
      int num = 0;
      while (stepsLeft >= 500)
      {
        this.updatePathFindingTasks(ref stepsLeft);
        if (++num > 20)
        {
          Mafi.Log.Warning("Too many PF tasks per tick.");
          break;
        }
      }
    }

    [OnlyForSaveCompatibility(null)]
    private void terrainDesignationFulfilledChanged(TerrainDesignation d)
    {
    }

    /// <summary>
    /// Processes current or next task. At most one task will be finished per call.
    /// </summary>
    private void updatePathFindingTasks(ref int stepsLeft)
    {
      Mafi.Assert.That<bool>(this.m_perfData.IsNone).IsEqualTo<bool>(this.m_stopwatch == null);
      bool newTaskStartedThisSimStep = false;
      if (this.m_currentTask.IsNone)
      {
        if (this.m_waitingTasksQueue.IsEmpty)
        {
          stepsLeft = 0;
          return;
        }
        this.m_stopwatch?.Restart();
        IManagedVehiclePathFindingTask task = this.m_waitingTasksQueue.PopMin().Value;
        this.m_currentTask = this.startTask(task, ref stepsLeft);
        newTaskStartedThisSimStep = true;
        this.m_stopwatch?.Stop();
        if (this.m_perfData.HasValue)
          this.m_perfData.Value.Add(new VehiclePathFindingManager.PerfData()
          {
            PfId = this.m_vehiclePathFinder.CurrentPfId,
            InitTimeMs = (float) this.m_stopwatch.Elapsed.TotalMilliseconds,
            Clearance = task.PathFindingParams.RequiredClearance,
            PathLengthEuclidean = task.DistanceEstimationStartTile.DistanceTo(task.DistanceEstimationGoalTile)
          });
        if (this.m_currentTask.IsNone)
        {
          if (!this.m_perfData.HasValue)
            return;
          this.m_perfData.Value.Last = this.m_perfData.Value.Last with
          {
            Result = VehiclePfResultStatus.PathFound
          };
          return;
        }
      }
      this.m_stopwatch?.Restart();
      PathFinderResult pathFinderResult = stepsLeft <= 0 ? PathFinderResult.StillSearching : this.m_vehiclePathFinder.ContinueVehiclePathFinding(ref stepsLeft, newTaskStartedThisSimStep, this.m_vehicleSurfaceProvider, this.m_currentPfStepsRemaining <= this.MaxStepsPerUpdate);
      int num1 = stepsLeft > 0 ? this.MaxStepsPerUpdate - stepsLeft : this.MaxStepsPerUpdate;
      VehiclePfResultStatus? nullable1;
      switch (pathFinderResult)
      {
        case PathFinderResult.StillSearching:
          this.m_currentPfStepsRemaining -= num1;
          if (this.m_currentPfStepsRemaining > 0)
          {
            nullable1 = new VehiclePfResultStatus?();
            break;
          }
          bool? nullable2 = this.m_vehiclePathFinder.TryExtendGoals();
          if (!nullable2.HasValue)
          {
            nullable1 = new VehiclePfResultStatus?(VehiclePfResultStatus.StepLimitExceeded);
            break;
          }
          if (nullable2.Value)
          {
            nullable1 = new VehiclePfResultStatus?(VehiclePfResultStatus.PathFound);
            break;
          }
          this.m_currentPfStepsRemaining = this.MaxStepsPerUpdate;
          nullable1 = new VehiclePfResultStatus?();
          break;
        case PathFinderResult.PathFound:
          nullable1 = new VehiclePfResultStatus?(VehiclePfResultStatus.PathFound);
          break;
        case PathFinderResult.PathDoesNotExist:
          nullable1 = new VehiclePfResultStatus?(VehiclePfResultStatus.PathDoesNotExist);
          break;
        default:
          Mafi.Log.Error(string.Format("Unhandled PF result case: {0}", (object) pathFinderResult));
          nullable1 = new VehiclePfResultStatus?(VehiclePfResultStatus.Unknown);
          break;
      }
      this.m_stopwatch?.Stop();
      if (this.m_perfData.HasValue && this.m_perfData.Value.Count > 0)
      {
        VehiclePathFindingManager.PerfData last = this.m_perfData.Value.Last;
        float totalMilliseconds = (float) this.m_stopwatch.Elapsed.TotalMilliseconds;
        last.SearchTimeMs += totalMilliseconds;
        last.SearchTimePerTickMax = last.SearchTimePerTickMax.Max(totalMilliseconds);
        last.Result = nullable1.GetValueOrDefault();
        last.PfSteps += num1;
        ++last.SimSteps;
        VehiclePfResultStatus? nullable3 = nullable1;
        VehiclePfResultStatus vehiclePfResultStatus = VehiclePfResultStatus.PathFound;
        int num2 = nullable3.GetValueOrDefault() == vehiclePfResultStatus & nullable3.HasValue ? 1 : 0;
        Lyst<ExploredPfNode> exploredTiles = new Lyst<ExploredPfNode>(64);
        this.m_vehiclePathFinder.GetExploredTiles(exploredTiles);
        last.ExploredNodesCount = exploredTiles.Count;
        if (this.m_vehiclePathFinder is VehiclePathFinder vehiclePathFinder)
        {
          Lyst<PfNode> foundPath = new Lyst<PfNode>(16);
          Lyst<Option<RoadGraphPath>> connectingRoads = new Lyst<Option<RoadGraphPath>>();
          vehiclePathFinder.TryReconstructFoundPath(foundPath, connectingRoads);
          last.PathNodesCount = foundPath.Count;
        }
        this.m_perfData.Value.Last = last;
      }
      if (!nullable1.HasValue)
      {
        Mafi.Assert.That<int>(this.m_currentPfStepsRemaining).IsPositive();
        Mafi.Assert.That<int>(stepsLeft).IsNotPositive();
      }
      else
      {
        if (nullable1.Value == VehiclePfResultStatus.PathFound)
          this.m_failedNavsVehicles.Remove(this.m_currentTask.Value.Vehicle.Id);
        else
          this.m_failedNavsVehicles.Add(this.m_currentTask.Value.Vehicle.Id);
        this.m_currentTask.Value.SetResult(this.m_vehiclePathFinder, nullable1.Value);
        this.m_currentTask = Option<IManagedVehiclePathFindingTask>.None;
        this.m_vehiclePathFinder.ResetState();
        ++this.CompletedPfTasks;
      }
    }

    /// <summary>
    /// Starts new path finding task. Returns true when new task was started and is ready for stepping. False is
    /// returned when new task either succeeded or failed right away and should not be stepped.
    /// </summary>
    private Option<IManagedVehiclePathFindingTask> startTask(
      IManagedVehiclePathFindingTask task,
      ref int stepsLeft)
    {
      Mafi.Assert.That<Option<IManagedVehiclePathFindingTask>>(this.m_currentTask).IsNone<IManagedVehiclePathFindingTask>();
      Mafi.Assert.That<bool>(task.IsWaitingForProcessing).IsTrue();
      Mafi.Assert.That<bool>(task.HasResult).IsFalse();
      Mafi.Assert.That<IIndexable<Tile2i>>(task.GoalTiles).IsEmpty<Tile2i>();
      task.SetIsBeingProcessed();
      VehiclePathFinderInitResult finderInitResult = this.m_vehiclePathFinder.InitVehiclePathFinding((IVehiclePathFindingTask) task, ref stepsLeft);
      Option<IManagedVehiclePathFindingTask> option = Option<IManagedVehiclePathFindingTask>.None;
      switch (finderInitResult)
      {
        case VehiclePathFinderInitResult.GoalAlreadyReached:
        case VehiclePathFinderInitResult.PathFound:
          task.SetResult(this.m_vehiclePathFinder, VehiclePfResultStatus.PathFound);
          break;
        case VehiclePathFinderInitResult.ReadyForPf:
          this.m_currentPfStepsRemaining = (VehiclePathFindingManager.MIN_PF_DURATION_TICKS.Ticks + task.GetGoalOrthogonalDistance().Value / VehiclePathFindingManager.EXTRA_PF_DURATION_TICK_PER_LENGTH.Value) * this.MaxStepsPerUpdate;
          option = Option.Some<IManagedVehiclePathFindingTask>(task);
          break;
        case VehiclePathFinderInitResult.NoStarts:
        case VehiclePathFinderInitResult.AllStartsInvalid:
          task.SetResult(this.m_vehiclePathFinder, VehiclePfResultStatus.StartInvalid);
          break;
        case VehiclePathFinderInitResult.NoGoals:
          task.SetResult(this.m_vehiclePathFinder, VehiclePfResultStatus.NoValidGoals);
          break;
        case VehiclePathFinderInitResult.AllGoalsInvalid:
          task.SetResult(this.m_vehiclePathFinder, VehiclePfResultStatus.AllGoalsInvalid);
          break;
        default:
          Mafi.Log.Error(string.Format("Unhandled PF init result type: {0}", (object) finderInitResult));
          task.SetResult(this.m_vehiclePathFinder, VehiclePfResultStatus.Unknown);
          break;
      }
      if (option.IsNone)
        this.m_vehiclePathFinder.ResetState();
      return option;
    }

    public static void Serialize(VehiclePathFindingManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehiclePathFindingManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehiclePathFindingManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.CompletedPfTasks);
      writer.WriteInt(this.CompletedUnreachableGoalTasks);
      writer.WriteInt(this.m_currentPfStepsRemaining);
      Option<IManagedVehiclePathFindingTask>.Serialize(this.m_currentTask, writer);
      Option<IStaticEntity>.Serialize(this.m_defaultHomeEntity, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      Set<EntityId>.Serialize(this.m_failedNavsVehicles, writer);
      writer.WriteGeneric<IRandom>(this.m_random);
      LazyResolve<ReturnHomeJob.Factory>.Serialize(this.m_returnHomeJobFactory, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      StaticEntityVehicleGoal.Factory.Serialize(this.m_staticEntityGoalFactory, writer);
      writer.WriteGeneric<ITreesManager>(this.m_treeManager);
      writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
      Heap<IManagedVehiclePathFindingTask>.Serialize(this.m_waitingTasksQueue, writer);
      writer.WriteInt(this.MaxStepsPerUpdate);
    }

    public static VehiclePathFindingManager Deserialize(BlobReader reader)
    {
      VehiclePathFindingManager pathFindingManager;
      if (reader.TryStartClassDeserialization<VehiclePathFindingManager>(out pathFindingManager))
        reader.EnqueueDataDeserialization((object) pathFindingManager, VehiclePathFindingManager.s_deserializeDataDelayedAction);
      return pathFindingManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CompletedPfTasks = reader.ReadInt();
      this.CompletedUnreachableGoalTasks = reader.ReadInt();
      this.m_crazyBusyReported = 20;
      this.m_currentPfStepsRemaining = reader.ReadInt();
      this.m_currentTask = Option<IManagedVehiclePathFindingTask>.Deserialize(reader);
      this.m_defaultHomeEntity = Option<IStaticEntity>.Deserialize(reader);
      reader.SetField<VehiclePathFindingManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<VehiclePathFindingManager>(this, "m_failedNavsVehicles", reader.LoadedSaveVersion >= 109 ? (object) Set<EntityId>.Deserialize(reader) : (object) new Set<EntityId>());
      if (reader.LoadedSaveVersion < 132)
        Queueue<TerrainDesignation>.Deserialize(reader);
      reader.SetField<VehiclePathFindingManager>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      reader.SetField<VehiclePathFindingManager>(this, "m_returnHomeJobFactory", (object) LazyResolve<ReturnHomeJob.Factory>.Deserialize(reader));
      reader.SetField<VehiclePathFindingManager>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<VehiclePathFindingManager>(this, "m_staticEntityGoalFactory", (object) StaticEntityVehicleGoal.Factory.Deserialize(reader));
      reader.SetField<VehiclePathFindingManager>(this, "m_treeManager", (object) reader.ReadGenericAs<ITreesManager>());
      reader.SetField<VehiclePathFindingManager>(this, "m_vehicleSurfaceProvider", reader.LoadedSaveVersion >= 168 ? (object) reader.ReadGenericAs<IVehicleSurfaceProvider>() : (object) (IVehicleSurfaceProvider) null);
      if (reader.LoadedSaveVersion < 168)
        reader.RegisterResolvedMember<VehiclePathFindingManager>(this, "m_vehicleSurfaceProvider", typeof (IVehicleSurfaceProvider), true);
      if (reader.LoadedSaveVersion < 106)
        reader.SetField<VehiclePathFindingManager>(this, "m_waitingTasks", (object) Queueue<IManagedVehiclePathFindingTask>.Deserialize(reader));
      reader.SetField<VehiclePathFindingManager>(this, "m_waitingTasksQueue", reader.LoadedSaveVersion >= 106 ? (object) Heap<IManagedVehiclePathFindingTask>.Deserialize(reader) : (object) new Heap<IManagedVehiclePathFindingTask>());
      this.MaxStepsPerUpdate = reader.ReadInt();
      reader.RegisterInitAfterLoad<VehiclePathFindingManager>(this, "initAfterLoad", InitPriority.High);
    }

    static VehiclePathFindingManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehiclePathFindingManager.MIN_PF_DURATION_TICKS = 2.Seconds();
      VehiclePathFindingManager.EXTRA_PF_DURATION_TICK_PER_LENGTH = new RelTile1i(20);
      VehiclePathFindingManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehiclePathFindingManager) obj).SerializeData(writer));
      VehiclePathFindingManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehiclePathFindingManager) obj).DeserializeData(reader));
    }

    public struct PerfData
    {
      public int PfId;
      public float InitTimeMs;
      public float SearchTimeMs;
      public float SearchTimePerTickMax;
      public VehiclePfResultStatus Result;
      public Fix32 PathLength;
      public int PathTilesCount;
      public int PathNodesCount;
      public int ExploredNodesCount;
      public Fix32 PathLengthEuclidean;
      public int PfSteps;
      public int SimSteps;
      public RelTile1i Clearance;

      public readonly float TotalTimeMs => this.InitTimeMs + this.SearchTimeMs;

      public static string GetHeader()
      {
        return "ID  \tInit [ms]\tSearch [ms]\tTotal [ms]\tMax/step [ms]\tPF Result  \tPF steps\tSim steps\tPath length\tTo goal euclid\tPath tiles\tPath nodes\tExplored nodes\tClearance";
      }

      public override string ToString()
      {
        return string.Format("{0,4}\t{1,9}\t{2,11}\t", (object) this.PfId, (object) this.InitTimeMs.RoundToSigDigits(4, false, false, false), (object) this.SearchTimeMs.RoundToSigDigits(4, false, false, false)) + string.Format("{0,10}\t{1,13}\t", (object) this.TotalTimeMs.RoundToSigDigits(4, false, false, false), (object) this.SearchTimePerTickMax.RoundToSigDigits(4, false, false, false)) + string.Format("{0,11}\t{1,8}\t{2,9}\t", (object) this.Result.ToString().SubstringSafe(0, new int?(11)), (object) this.PfSteps, (object) this.SimSteps) + string.Format("{0,11}\t{1,14}\t", (object) this.PathLength.ToStringRounded(4), (object) this.PathLengthEuclidean.ToStringRounded(4)) + string.Format("{0,10}\t{1,10}\t{2,14}\t{3,9}", (object) this.PathTilesCount, (object) this.PathNodesCount, (object) this.ExploredNodesCount, (object) this.Clearance);
      }
    }
  }
}
