// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.WorkersManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Notifications;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.World;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Population
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class WorkersManager : IWorkersManager
  {
    private readonly Event<int> m_workersAmountChanged;
    private readonly SettlementsManager m_settlementsManager;
    public readonly IntAvgStats TotalWorkersNeededStats;
    private readonly Lyst<WorkersManager.EntityWorkersAssigner> m_sortedAssigners;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<IEntityWithWorkers, WorkersManager.EntityWorkersAssigner> m_assignersMap;
    private int m_amountOfFreeWorkers;
    private int m_amountOfWorkersMissing;
    private int m_amountOfWorkersMissingLastSim;
    private bool m_freeWorkersAmountChanged;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<IEntityProto, WorkersManager.WorkersStatsPerProto> m_protoToStatMapCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int AmountOfFreeWorkers => this.m_amountOfFreeWorkers;

    public int AmountOfFreeWorkersOrMissing
    {
      get => this.m_amountOfFreeWorkers - this.m_amountOfWorkersMissingLastSim;
    }

    public int NumberOfWorkersWithheld { get; set; }

    public IEvent<int> WorkersAmountChanged => (IEvent<int>) this.m_workersAmountChanged;

    public WorkersManager(
      IEntitiesManager entitiesManager,
      ISimLoopEvents simLoopEvents,
      SettlementsManager settlementsManager,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_workersAmountChanged = new Event<int>();
      this.m_sortedAssigners = new Lyst<WorkersManager.EntityWorkersAssigner>();
      this.m_assignersMap = new Dict<IEntityWithWorkers, WorkersManager.EntityWorkersAssigner>();
      this.m_protoToStatMapCache = new Dict<IEntityProto, WorkersManager.WorkersStatsPerProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_settlementsManager = settlementsManager;
      this.m_amountOfFreeWorkers = settlementsManager.GetTotalPopulationWithoutHomeless();
      this.TotalWorkersNeededStats = new IntAvgStats((Option<StatsManager>) statsManager);
      simLoopEvents.UpdateEnd.Add<WorkersManager>(this, new Action(this.simUpdateEnd));
      settlementsManager.OnWorkersAdded.Add<WorkersManager>(this, new Action<int>(this.onPopsAdded));
      settlementsManager.OnWorkersRemoved.Add<WorkersManager>(this, new Action<int>(this.updateWorkersAfterRemovedPops));
      entitiesManager.EntityAdded.Add<WorkersManager>(this, new Action<IEntity>(this.entityAdded));
    }

    [InitAfterLoad(InitPriority.High)]
    private void initSelf(int saveVersion)
    {
      foreach (WorkersManager.EntityWorkersAssigner sortedAssigner in this.m_sortedAssigners)
      {
        this.m_assignersMap.Add(sortedAssigner.Entity, sortedAssigner);
        sortedAssigner.Entity.HasWorkersCached = sortedAssigner.NumberOfWorkerAssigned == sortedAssigner.Entity.WorkersNeeded;
      }
      if (saveVersion >= 108 || this.VerifyAssignersOrder())
        return;
      this.m_sortedAssigners.Sort((Comparison<WorkersManager.EntityWorkersAssigner>) ((x, y) => x.Priority.CompareTo(y.Priority)));
    }

    internal bool VerifyAssignersOrder()
    {
      for (int index = 1; index < this.m_sortedAssigners.Count; ++index)
      {
        if (this.m_sortedAssigners[index - 1].Priority > this.m_sortedAssigners[index].Priority)
          return false;
      }
      return true;
    }

    private void onPopsAdded(int amount)
    {
      this.m_amountOfFreeWorkers += amount;
      this.m_freeWorkersAmountChanged = true;
    }

    private void addAssigner(WorkersManager.EntityWorkersAssigner assigner)
    {
      Assert.That<bool>(assigner.IsDestroyed).IsFalse();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_sortedAssigners.PriorityListInsertSorted<WorkersManager.EntityWorkersAssigner>(assigner, WorkersManager.\u003C\u003EO.\u003C0\u003E__priorityProvider ?? (WorkersManager.\u003C\u003EO.\u003C0\u003E__priorityProvider = new Func<WorkersManager.EntityWorkersAssigner, int>(priorityProvider)));

      static int priorityProvider(WorkersManager.EntityWorkersAssigner a) => a.Priority;
    }

    private void removeAssigner(WorkersManager.EntityWorkersAssigner assigner)
    {
      Assert.That<int>(assigner.NumberOfWorkerAssigned).IsEqualTo(0);
      this.m_sortedAssigners.RemoveAndAssert(assigner);
    }

    private void requestWorkers(WorkersManager.EntityWorkersAssigner assigner)
    {
      Assert.That<bool>(assigner.IsDestroyed).IsFalse();
      if (assigner.NumberOfWorkersMissing == 0)
        return;
      if (assigner.NumberOfWorkersMissing < 0)
        this.returnWorkers(assigner);
      if (tryAssignFreeWorkers())
        return;
      int num = assigner.NumberOfWorkersMissing - this.m_amountOfFreeWorkers;
      if (this.revokeWorkers(assigner.Priority + 1, num, true) >= num)
      {
        Assert.That<int>(this.revokeWorkers(assigner.Priority + 1, num, false)).IsGreaterOrEqual(num);
        if (tryAssignFreeWorkers())
          return;
        Assert.Fail(string.Format("Failed to assign {0} workers!", (object) num));
      }
      this.m_amountOfWorkersMissing += assigner.NumberOfWorkersMissing;

      bool tryAssignFreeWorkers()
      {
        if (assigner.NumberOfWorkersMissing > this.m_amountOfFreeWorkers)
          return false;
        this.m_amountOfFreeWorkers -= assigner.NumberOfWorkersMissing;
        assigner.SetNumberOfWorkersAssigned(assigner.NumberOfWorkersNeeded);
        this.m_freeWorkersAmountChanged = true;
        return true;
      }
    }

    public void WithholdWorkers(int numberOfWorkerToWithhold)
    {
      int populationWithoutHomeless = this.m_settlementsManager.GetTotalPopulationWithoutHomeless();
      if (this.NumberOfWorkersWithheld + numberOfWorkerToWithhold > populationWithoutHomeless)
      {
        Log.Error("Can't withhold pops than total pops.");
      }
      else
      {
        this.NumberOfWorkersWithheld += numberOfWorkerToWithhold;
        this.updateWorkersAfterRemovedPops(numberOfWorkerToWithhold);
      }
    }

    public void ReturnWithheldWorkers(int numberOfWorkerToReturn)
    {
      int amount = numberOfWorkerToReturn.Min(this.NumberOfWorkersWithheld).Min(this.m_settlementsManager.GetTotalPopulationWithoutHomeless());
      this.NumberOfWorkersWithheld -= amount;
      this.onPopsAdded(amount);
    }

    private int revokeWorkers(int minPriorityIncl, int amountNeed, bool simulateOnly)
    {
      int num = 0;
      for (int index = this.m_sortedAssigners.Count - 1; index >= 0; --index)
      {
        WorkersManager.EntityWorkersAssigner sortedAssigner = this.m_sortedAssigners[index];
        if (sortedAssigner.Priority < minPriorityIncl)
          return num;
        if (sortedAssigner.NumberOfWorkerAssigned > 0)
        {
          num += sortedAssigner.NumberOfWorkerAssigned;
          if (!simulateOnly)
          {
            this.m_amountOfFreeWorkers += sortedAssigner.NumberOfWorkerAssigned;
            sortedAssigner.SetNumberOfWorkersAssigned(0);
            this.m_freeWorkersAmountChanged = true;
          }
        }
        if (amountNeed <= num)
          return num;
      }
      return num;
    }

    private void returnWorkers(WorkersManager.EntityWorkersAssigner assigner)
    {
      if (assigner.NumberOfWorkerAssigned <= 0)
        return;
      this.m_amountOfFreeWorkers += assigner.NumberOfWorkerAssigned;
      assigner.SetNumberOfWorkersAssigned(0);
      this.m_freeWorkersAmountChanged = true;
    }

    private void simUpdateEnd()
    {
      this.TotalWorkersNeededStats.Set((long) (this.m_settlementsManager.GetTotalPopulationWithoutHomeless() - this.NumberOfWorkersWithheld - this.m_amountOfFreeWorkers + this.m_amountOfWorkersMissing));
      if (this.m_freeWorkersAmountChanged || this.m_amountOfWorkersMissing != this.m_amountOfWorkersMissingLastSim)
      {
        this.m_freeWorkersAmountChanged = false;
        this.m_workersAmountChanged.Invoke(this.m_amountOfFreeWorkers - this.m_amountOfWorkersMissing);
      }
      this.m_amountOfWorkersMissingLastSim = this.m_amountOfWorkersMissing;
      this.m_amountOfWorkersMissing = 0;
    }

    private void updateWorkersAfterRemovedPops(int amountToRemove)
    {
      if (this.m_amountOfFreeWorkers >= amountToRemove)
      {
        this.m_amountOfFreeWorkers -= amountToRemove;
        this.m_freeWorkersAmountChanged = true;
      }
      else
      {
        int amountNeed = amountToRemove - this.m_amountOfFreeWorkers;
        if (this.revokeWorkers(0, amountNeed, false) < amountNeed)
          Assert.Fail("Did not manage to remove workers properly.");
        this.m_amountOfFreeWorkers = (this.m_amountOfFreeWorkers - amountToRemove).Max(0);
      }
    }

    public void Cheat_addWorkers(int workers) => this.m_amountOfFreeWorkers += workers;

    private void entityAdded(IEntity entity)
    {
      if (!(entity is IEntityWithWorkers entity1))
        return;
      this.registerEntity(entity1);
    }

    private void registerEntity(IEntityWithWorkers entity)
    {
      if (this.m_assignersMap.ContainsKey(entity))
      {
        Log.Error(string.Format("Entity {0} already registered!", (object) entity));
      }
      else
      {
        WorkersManager.EntityWorkersAssigner entityWorkersAssigner = new WorkersManager.EntityWorkersAssigner(entity, this);
        this.m_assignersMap.Add(entity, entityWorkersAssigner);
      }
    }

    public bool CanWork(IEntityWithWorkers entity, bool doNotNotify = false)
    {
      WorkersManager.EntityWorkersAssigner entityWorkersAssigner;
      if (this.m_assignersMap.TryGetValue(entity, out entityWorkersAssigner))
        return entityWorkersAssigner.CanWork(doNotNotify);
      Log.Error(string.Format("Entity {0} not registered!", (object) entity));
      return false;
    }

    public void ReturnWorkersVoluntarily(IEntityWithWorkers entity)
    {
      WorkersManager.EntityWorkersAssigner entityWorkersAssigner;
      if (!this.m_assignersMap.TryGetValue(entity, out entityWorkersAssigner))
        Log.Error(string.Format("Entity {0} not registered!", (object) entity));
      entityWorkersAssigner?.ReturnWorkersVoluntarily();
    }

    public IEnumerable<WorkersManager.WorkersStatsPerProto> GetWorkersStatsPerProto()
    {
      this.m_protoToStatMapCache.Clear();
      foreach (WorkersManager.EntityWorkersAssigner sortedAssigner in this.m_sortedAssigners)
      {
        if (sortedAssigner.NumberOfWorkersNeeded != 0 && !(sortedAssigner.Entity.Prototype is TravelingFleetProto))
        {
          ref WorkersManager.WorkersStatsPerProto local = ref this.m_protoToStatMapCache.GetRefValue((IEntityProto) sortedAssigner.Entity.Prototype, out bool _);
          local.Proto = (IEntityProto) sortedAssigner.Entity.Prototype;
          local.EntitiesTotal += sortedAssigner.Entity.IsEnabled ? 1 : 0;
          local.WorkersAssigned += sortedAssigner.NumberOfWorkerAssigned;
          local.WorkersNeeded += sortedAssigner.HasWorkersDemand ? sortedAssigner.NumberOfWorkersNeeded : 0;
        }
      }
      return (IEnumerable<WorkersManager.WorkersStatsPerProto>) this.m_protoToStatMapCache.Values;
    }

    public static void Serialize(WorkersManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorkersManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorkersManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.m_amountOfFreeWorkers);
      writer.WriteInt(this.m_amountOfWorkersMissing);
      writer.WriteInt(this.m_amountOfWorkersMissingLastSim);
      writer.WriteBool(this.m_freeWorkersAmountChanged);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      Lyst<WorkersManager.EntityWorkersAssigner>.Serialize(this.m_sortedAssigners, writer);
      Event<int>.Serialize(this.m_workersAmountChanged, writer);
      writer.WriteInt(this.NumberOfWorkersWithheld);
      IntAvgStats.Serialize(this.TotalWorkersNeededStats, writer);
    }

    public static WorkersManager Deserialize(BlobReader reader)
    {
      WorkersManager workersManager;
      if (reader.TryStartClassDeserialization<WorkersManager>(out workersManager))
        reader.EnqueueDataDeserialization((object) workersManager, WorkersManager.s_deserializeDataDelayedAction);
      return workersManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_amountOfFreeWorkers = reader.ReadInt();
      this.m_amountOfWorkersMissing = reader.ReadInt();
      this.m_amountOfWorkersMissingLastSim = reader.ReadInt();
      reader.SetField<WorkersManager>(this, "m_assignersMap", (object) new Dict<IEntityWithWorkers, WorkersManager.EntityWorkersAssigner>());
      this.m_freeWorkersAmountChanged = reader.ReadBool();
      reader.SetField<WorkersManager>(this, "m_protoToStatMapCache", (object) new Dict<IEntityProto, WorkersManager.WorkersStatsPerProto>());
      reader.SetField<WorkersManager>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<WorkersManager>(this, "m_sortedAssigners", (object) Lyst<WorkersManager.EntityWorkersAssigner>.Deserialize(reader));
      reader.SetField<WorkersManager>(this, "m_workersAmountChanged", (object) Event<int>.Deserialize(reader));
      this.NumberOfWorkersWithheld = reader.ReadInt();
      reader.SetField<WorkersManager>(this, "TotalWorkersNeededStats", (object) IntAvgStats.Deserialize(reader));
      reader.RegisterInitAfterLoad<WorkersManager>(this, "initSelf", InitPriority.High);
    }

    static WorkersManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorkersManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorkersManager) obj).SerializeData(writer));
      WorkersManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorkersManager) obj).DeserializeData(reader));
    }

    public struct WorkersStatsPerProto
    {
      public IEntityProto Proto;
      public int EntitiesTotal;
      public int WorkersAssigned;
      public int WorkersNeeded;
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class EntityWorkersAssigner : 
      IComparable<WorkersManager.EntityWorkersAssigner>,
      IEventOwner,
      IEntityObserverForPriority,
      IEntityObserver,
      IEntityObserverForEnabled,
      IEntityObserverForUpgrade
    {
      public bool HasWorkersDemand;
      public readonly IEntityWithWorkers Entity;
      private readonly WorkersManager m_workersManager;
      private EntityNotificator m_notEnoughWorkersNotif;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public bool IsDestroyed => this.Entity.IsDestroyed;

      public int Priority => this.Entity.GeneralPriority;

      public bool HasAllRequiredWorkers => this.NumberOfWorkersMissing <= 0;

      public int NumberOfWorkersMissing => this.NumberOfWorkersNeeded - this.NumberOfWorkerAssigned;

      public int NumberOfWorkerAssigned { get; private set; }

      public int NumberOfWorkersNeeded => this.Entity.WorkersNeeded;

      public EntityWorkersAssigner(IEntityWithWorkers entity, WorkersManager workersManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Entity = entity;
        this.m_workersManager = workersManager;
        this.NumberOfWorkerAssigned = 0;
        this.m_notEnoughWorkersNotif = entity.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NotEnoughWorkers);
        entity.AddObserver((IEntityObserver) this);
        workersManager.addAssigner(this);
      }

      void IEntityObserver.OnEntityDestroy(IEntity entity)
      {
        entity.RemoveObserver((IEntityObserver) this);
        this.ReturnWorkersVoluntarily();
        this.m_workersManager.removeAssigner(this);
      }

      void IEntityObserverForPriority.OnGeneralPriorityChange(IEntity entity)
      {
        bool allRequiredWorkers = this.HasAllRequiredWorkers;
        this.ReturnWorkersVoluntarily();
        this.m_workersManager.removeAssigner(this);
        this.m_workersManager.addAssigner(this);
        if (!allRequiredWorkers || this.HasAllRequiredWorkers)
          return;
        this.m_workersManager.requestWorkers(this);
        this.HasWorkersDemand = true;
      }

      void IEntityObserverForEnabled.OnEnabledChange(IEntity entity, bool isEnabled)
      {
        if (isEnabled)
          return;
        this.m_notEnoughWorkersNotif.Deactivate((IEntity) this.Entity);
        this.ReturnWorkersVoluntarily();
      }

      void IEntityObserverForUpgrade.OnEntityUpgraded(
        IEntity upgradedEntity,
        IEntityProto previousProto)
      {
        bool allRequiredWorkers = this.HasAllRequiredWorkers;
        this.ReturnWorkersVoluntarily();
        if (!allRequiredWorkers || this.HasAllRequiredWorkers)
          return;
        this.m_workersManager.requestWorkers(this);
        this.HasWorkersDemand = true;
      }

      public bool CanWork(bool doNotNotify = false)
      {
        if (this.NumberOfWorkersMissing < 0)
          this.m_workersManager.requestWorkers(this);
        this.HasWorkersDemand = true;
        if (this.HasAllRequiredWorkers)
        {
          this.m_notEnoughWorkersNotif.Deactivate((IEntity) this.Entity);
          this.Entity.HasWorkersCached = true;
          return true;
        }
        this.m_workersManager.requestWorkers(this);
        if (!doNotNotify)
          this.m_notEnoughWorkersNotif.NotifyIff(!this.HasAllRequiredWorkers, (IEntity) this.Entity);
        this.Entity.HasWorkersCached = this.HasAllRequiredWorkers;
        return this.HasAllRequiredWorkers;
      }

      public void ReturnWorkersVoluntarily()
      {
        this.Entity.HasWorkersCached = false;
        this.HasWorkersDemand = false;
        if (this.NumberOfWorkerAssigned <= 0)
          return;
        this.m_workersManager.returnWorkers(this);
      }

      public void SetNumberOfWorkersAssigned(int numberOfWorkersAssigned)
      {
        this.NumberOfWorkerAssigned = numberOfWorkersAssigned;
        Assert.That<int>(this.NumberOfWorkerAssigned).IsNotNegative();
        this.Entity.HasWorkersCached = false;
      }

      public int CompareTo(WorkersManager.EntityWorkersAssigner other)
      {
        return this.Priority.CompareTo(other.Priority);
      }

      public static void Serialize(WorkersManager.EntityWorkersAssigner value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<WorkersManager.EntityWorkersAssigner>(value))
          return;
        writer.EnqueueDataSerialization((object) value, WorkersManager.EntityWorkersAssigner.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<IEntityWithWorkers>(this.Entity);
        writer.WriteBool(this.HasWorkersDemand);
        EntityNotificator.Serialize(this.m_notEnoughWorkersNotif, writer);
        WorkersManager.Serialize(this.m_workersManager, writer);
        writer.WriteInt(this.NumberOfWorkerAssigned);
      }

      public static WorkersManager.EntityWorkersAssigner Deserialize(BlobReader reader)
      {
        WorkersManager.EntityWorkersAssigner entityWorkersAssigner;
        if (reader.TryStartClassDeserialization<WorkersManager.EntityWorkersAssigner>(out entityWorkersAssigner))
          reader.EnqueueDataDeserialization((object) entityWorkersAssigner, WorkersManager.EntityWorkersAssigner.s_deserializeDataDelayedAction);
        return entityWorkersAssigner;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<WorkersManager.EntityWorkersAssigner>(this, "Entity", (object) reader.ReadGenericAs<IEntityWithWorkers>());
        this.HasWorkersDemand = reader.ReadBool();
        this.m_notEnoughWorkersNotif = EntityNotificator.Deserialize(reader);
        reader.SetField<WorkersManager.EntityWorkersAssigner>(this, "m_workersManager", (object) WorkersManager.Deserialize(reader));
        this.NumberOfWorkerAssigned = reader.ReadInt();
      }

      static EntityWorkersAssigner()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WorkersManager.EntityWorkersAssigner.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorkersManager.EntityWorkersAssigner) obj).SerializeData(writer));
        WorkersManager.EntityWorkersAssigner.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorkersManager.EntityWorkersAssigner) obj).DeserializeData(reader));
      }
    }
  }
}
