// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.ElectricityManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class ElectricityManager : 
    IElectricityManager,
    IElectricityGeneratorRegistratorFactory,
    IEntityObserver,
    ICommandProcessor<SetElectricityGenerationPriorityCmd>,
    IAction<SetElectricityGenerationPriorityCmd>,
    ICommandProcessor<SetIsElectricitySurplusGeneratorCmd>,
    IAction<SetIsElectricitySurplusGeneratorCmd>,
    ICommandProcessor<SetIsElectricitySurplusConsumerCmd>,
    IAction<SetIsElectricitySurplusConsumerCmd>
  {
    public const int MAX_PRIORITY_STEPS = 14;
    public readonly ElectricityAvgStats ProductionStats;
    public readonly ElectricityAvgStats ConsumptionStats;
    public readonly ElectricityAvgStats GenerationCapacityStats;
    private Electricity m_currentFreeElectricity;
    private readonly Lyst<ElectricityManager.ElectricityGeneratorBuffer> m_sortedGenerators;
    private readonly Lyst<IElectricityConsumerInternal> m_sortedConsumers;
    private LystStruct<ElectricityManager.ConsumptionPerProto> m_consumptionStatsPerProto;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<IEntityProto, int> m_consumerProtoIdsMap;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<ElectricityManager.ConsumptionLastTick> m_consumptionStatsCache;
    private LystStruct<ElectricityManager.ProductionPerProto> m_productionStatsPerProto;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<IEntityProto, int> m_producerProtoIdsMap;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<ElectricityManager.ProductionLastTick> m_productionStatsCache;
    private readonly StatsManager m_statsManager;
    private int m_currentGeneratorIndex;
    private Electricity m_freeElectricityPerTick;
    [DoNotSave(0, null)]
    private bool m_discardAllElectricity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public VirtualProductProto ElectricityProto { get; private set; }

    /// <summary>
    /// Maximum generation capacity if all generators were working on 100% of their max capacity.
    /// Re-computed during <see cref="P:Mafi.Core.Simulation.SimLoopEvents.UpdateStart" />.
    /// </summary>
    public Electricity MaxGenerationCapacity { get; private set; }

    /// <summary>
    /// Current generation capacity with respect to current conditions. This takes into account paused generators.
    /// Re-computed during <see cref="P:Mafi.Core.Simulation.SimLoopEvents.UpdateStart" />.
    /// </summary>
    public Electricity GenerationCapacityThisTick { get; private set; }

    /// <summary>
    /// Amount of generated electricity this sim update. Note that generation and consumption are not synchronous
    /// and consumption is one tick behind consumption. Introduce `GeneratedLastTick` if needed.
    /// Re-computed during <see cref="P:Mafi.Core.Simulation.SimLoopEvents.UpdateStart" />.
    /// </summary>
    public Electricity GeneratedThisTick { get; private set; }

    /// <summary>
    /// Amount of consumed electricity during this sim update (so far). Total value will be accurate only after all
    /// entities request electricity (after <see cref="P:Mafi.Core.Simulation.SimLoopEvents.Update" /> finishes).
    /// Re-computed during <see cref="P:Mafi.Core.Simulation.SimLoopEvents.Update" />.
    /// </summary>
    public Electricity ConsumedThisTick { get; private set; }

    /// <summary>
    /// Amount of requested electricity during this sim update (so far). Total value will be accurate only after all
    /// entities request electricity (after <see cref="P:Mafi.Core.Simulation.SimLoopEvents.Update" /> finishes). Does not contain surplus demands.
    /// Re-computed during <see cref="P:Mafi.Core.Simulation.SimLoopEvents.Update" />.
    /// </summary>
    public Electricity DemandedThisTick { get; private set; }

    /// <summary>
    /// Amount of requested surplus electricity during this sim update (so far).
    /// </summary>
    public Electricity SurplusDemandedThisTick { get; private set; }

    /// <summary>Amount of wasted electricity.</summary>
    public Electricity WastedElectricityThisTick { get; private set; }

    public int GeneratorsCount => this.m_sortedGenerators.Count;

    public int ConsumersCount => this.m_sortedConsumers.Count;

    public ElectricityManager(
      IElectricityConfig electricityConfig,
      ISimLoopEvents simLoopEvents,
      [ProtoDep("Product_Virtual_Electricity")] VirtualProductProto electricityProto,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_sortedGenerators = new Lyst<ElectricityManager.ElectricityGeneratorBuffer>();
      this.m_sortedConsumers = new Lyst<IElectricityConsumerInternal>();
      this.m_consumerProtoIdsMap = new Dict<IEntityProto, int>();
      this.m_producerProtoIdsMap = new Dict<IEntityProto, int>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_statsManager = statsManager;
      this.ElectricityProto = electricityProto;
      this.ProductionStats = new ElectricityAvgStats((Option<StatsManager>) statsManager);
      this.ConsumptionStats = new ElectricityAvgStats((Option<StatsManager>) statsManager);
      this.GenerationCapacityStats = new ElectricityAvgStats((Option<StatsManager>) statsManager);
      simLoopEvents.UpdateStart.Add<ElectricityManager>(this, new Action(this.updateStart));
    }

    [InitAfterLoad(InitPriority.High)]
    private void initSelf()
    {
      for (int index = 0; index < this.m_consumptionStatsPerProto.Count; ++index)
      {
        this.m_consumerProtoIdsMap.Add(this.m_consumptionStatsPerProto[index].ConsumerProto, index);
        this.m_consumptionStatsCache.Add(ElectricityManager.ConsumptionLastTick.Empty);
      }
      for (int index = 0; index < this.m_productionStatsPerProto.Count; ++index)
      {
        this.m_producerProtoIdsMap.Add(this.m_productionStatsPerProto[index].ProducerProto, index);
        this.m_productionStatsCache.Add(ElectricityManager.ProductionLastTick.Empty);
      }
      foreach (IElectricityConsumerInternal sortedConsumer in this.m_sortedConsumers)
        this.assignProtoTokenTo(sortedConsumer);
      foreach (ElectricityManager.ElectricityGeneratorBuffer sortedGenerator in this.m_sortedGenerators)
        this.assignProtoTokenTo(sortedGenerator);
    }

    internal void SetDiscardAllElectricityEveryTick(bool discardingEnabled)
    {
      this.m_discardAllElectricity = discardingEnabled;
    }

    public void Cheat_AddFreeElectricityPerTick(Electricity e)
    {
      this.m_freeElectricityPerTick += e;
    }

    public void Cheat_ClearFreeElectricityPerTick()
    {
      this.m_freeElectricityPerTick = Electricity.Zero;
    }

    private void updateStart()
    {
      this.ProductionStats.Set(this.GeneratedThisTick);
      this.ConsumptionStats.Set(this.ConsumedThisTick);
      this.GenerationCapacityStats.Set(this.GenerationCapacityThisTick);
      this.m_currentFreeElectricity = this.m_freeElectricityPerTick;
      this.MaxGenerationCapacity = this.m_freeElectricityPerTick;
      this.GenerationCapacityThisTick = this.m_freeElectricityPerTick;
      this.GeneratedThisTick = this.m_freeElectricityPerTick;
      this.ConsumedThisTick = Electricity.Zero;
      this.DemandedThisTick = Electricity.Zero;
      this.SurplusDemandedThisTick = Electricity.Zero;
      this.WastedElectricityThisTick = Electricity.Zero;
      foreach (ElectricityManager.ElectricityGeneratorBuffer sortedGenerator in this.m_sortedGenerators)
      {
        Electricity intoBuffer = sortedGenerator.GenerateIntoBuffer();
        Electricity generationCapacity = sortedGenerator.MaxGenerationCapacity;
        this.WastedElectricityThisTick += intoBuffer;
        this.MaxGenerationCapacity += generationCapacity;
        this.GenerationCapacityThisTick += sortedGenerator.GenerationCapacityThisTick;
        this.GeneratedThisTick += sortedGenerator.GeneratedThisTick;
        ref ElectricityManager.ProductionLastTick local = ref this.m_productionStatsCache.GetRefAt(sortedGenerator.ProtoToken < this.m_productionStatsCache.Count ? sortedGenerator.ProtoToken : 0);
        local.Produced += sortedGenerator.GeneratedThisTick;
        local.Wasted += intoBuffer;
        local.MaxGenerationCapacity += generationCapacity;
      }
      this.m_currentGeneratorIndex = 0;
      for (int index = 0; index < this.m_productionStatsCache.Count; ++index)
      {
        this.m_productionStatsPerProto.GetRefAt(index).Set(this.m_productionStatsCache[index]);
        this.m_productionStatsCache[index] = ElectricityManager.ProductionLastTick.Empty;
      }
      foreach (IElectricityConsumerInternal sortedConsumer in this.m_sortedConsumers)
      {
        if (!sortedConsumer.IsEnabled)
        {
          sortedConsumer.RechargeSkipped();
        }
        else
        {
          ref ElectricityManager.ConsumptionLastTick local = ref this.m_consumptionStatsCache.GetRefAt(sortedConsumer.ProtoToken < this.m_consumptionStatsCache.Count ? sortedConsumer.ProtoToken : 0);
          local.MaxPossibleConsumption += sortedConsumer.PowerRequired;
          Electricity electricity = sortedConsumer.PowerRequired - sortedConsumer.PowerCharged;
          if (electricity.IsPositive)
          {
            local.Demand += electricity;
            if (!sortedConsumer.IsSurplusConsumer)
              this.DemandedThisTick += electricity;
            else
              this.SurplusDemandedThisTick += electricity;
            if (this.tryConsume(electricity, sortedConsumer.IsSurplusConsumer))
            {
              sortedConsumer.Recharge(electricity);
              local.Consumed += electricity;
            }
            else
              sortedConsumer.RechargeSkipped();
          }
          else
            sortedConsumer.RechargeSkipped();
        }
      }
      for (int index = 0; index < this.m_consumptionStatsCache.Count; ++index)
      {
        this.m_consumptionStatsPerProto.GetRefAt(index).Set(this.m_consumptionStatsCache[index]);
        this.m_consumptionStatsCache[index] = ElectricityManager.ConsumptionLastTick.Empty;
      }
      this.WastedElectricityThisTick += this.m_currentFreeElectricity;
      this.m_currentFreeElectricity = Electricity.Zero;
    }

    public void AddConsumer(IElectricityConsumerInternal consumer)
    {
      Assert.That<bool>(consumer.Entity.IsDestroyed).IsFalse();
      this.m_sortedConsumers.PriorityListInsertSorted<IElectricityConsumerInternal>(consumer);
      this.assignProtoTokenTo(consumer);
    }

    private void assignProtoTokenTo(IElectricityConsumerInternal consumer)
    {
      int count;
      if (!this.m_consumerProtoIdsMap.TryGetValue((IEntityProto) consumer.Entity.Prototype, out count))
      {
        ElectricityManager.ConsumptionPerProto consumptionPerProto = new ElectricityManager.ConsumptionPerProto((IEntityProto) consumer.Entity.Prototype, new ElectricityAvgStats((Option<StatsManager>) this.m_statsManager));
        count = this.m_consumptionStatsPerProto.Count;
        this.m_consumptionStatsPerProto.Add(consumptionPerProto);
        this.m_consumptionStatsCache.Add(ElectricityManager.ConsumptionLastTick.Empty);
        this.m_consumerProtoIdsMap.Add((IEntityProto) consumer.Entity.Prototype, count);
      }
      consumer.ProtoToken = count;
    }

    public void RemoveConsumer(IElectricityConsumerInternal consumer)
    {
      this.m_sortedConsumers.RemoveAndAssert(consumer);
    }

    private bool tryConsume(Electricity requestedElectricity, bool isSurplusConsumer)
    {
      Electricity requested = requestedElectricity;
      for (int currentGeneratorIndex = this.m_currentGeneratorIndex; currentGeneratorIndex < this.m_sortedGenerators.Count && requested.IsPositive; ++currentGeneratorIndex)
      {
        ElectricityManager.ElectricityGeneratorBuffer sortedGenerator = this.m_sortedGenerators[currentGeneratorIndex];
        if (!isSurplusConsumer || sortedGenerator.IsSurplusGenerator)
        {
          if (sortedGenerator.Quantity.IsNotPositive)
          {
            ++this.m_currentGeneratorIndex;
          }
          else
          {
            Electricity electricity = sortedGenerator.RemoveAsMuchAs(requested);
            requested -= electricity;
          }
        }
        else
          break;
      }
      if (requested.IsNotPositive)
      {
        Assert.That<Electricity>(requested).IsZero();
        this.ConsumedThisTick += requestedElectricity;
        return true;
      }
      if (this.m_currentFreeElectricity.IsPositive)
      {
        Electricity electricity = requested.Min(this.m_currentFreeElectricity);
        requested -= electricity;
        this.m_currentFreeElectricity -= electricity;
        Assert.That<Electricity>(this.m_currentFreeElectricity).IsNotNegative();
        if (requested.IsNotPositive)
        {
          Assert.That<Electricity>(requested).IsZero();
          this.ConsumedThisTick += requestedElectricity;
          return true;
        }
      }
      this.WastedElectricityThisTick += requestedElectricity - requested;
      return false;
    }

    /// <summary>
    /// Adds a new generator while preserving descending order.
    /// </summary>
    private void addGenerator(
      ElectricityManager.ElectricityGeneratorBuffer generator)
    {
      this.m_sortedGenerators.PriorityListInsertSorted<ElectricityManager.ElectricityGeneratorBuffer>(generator);
      this.assignProtoTokenTo(generator);
    }

    private void assignProtoTokenTo(
      ElectricityManager.ElectricityGeneratorBuffer generator)
    {
      IEntityProto prototype;
      if (generator.Generator is IElectricityGeneratingEntity generator2)
        prototype = (IEntityProto) generator2.Prototype;
      else if (generator.Generator is IElectricityGeneratingEntityGrouped generator1)
      {
        prototype = generator1.Prototype;
      }
      else
      {
        Log.Error(string.Format("Unknown generator type {0}", (object) generator));
        return;
      }
      int count;
      if (!this.m_producerProtoIdsMap.TryGetValue(prototype, out count))
      {
        ElectricityManager.ProductionPerProto productionPerProto = new ElectricityManager.ProductionPerProto(prototype, new ElectricityAvgStats((Option<StatsManager>) this.m_statsManager));
        count = this.m_productionStatsPerProto.Count;
        this.m_productionStatsPerProto.Add(productionPerProto);
        this.m_productionStatsCache.Add(ElectricityManager.ProductionLastTick.Empty);
        this.m_producerProtoIdsMap.Add(prototype, count);
      }
      generator.ProtoToken = count;
    }

    public void DiscardAllElectricity()
    {
      Electricity zero = Electricity.Zero;
      foreach (ElectricityManager.ElectricityGeneratorBuffer sortedGenerator in this.m_sortedGenerators)
      {
        zero += sortedGenerator.Quantity;
        this.WastedElectricityThisTick += sortedGenerator.SetCapacity(Electricity.Zero);
      }
    }

    public IElectricityGeneratorRegistrator CreateAndRegisterFor(
      IElectricityGeneratingEntity entity,
      int generationPriority)
    {
      if (generationPriority < 0 || generationPriority > 14)
      {
        Log.Error(string.Format("Generation priority {0} is out of range 0 - {1}", (object) generationPriority, (object) 14));
        generationPriority = generationPriority.Clamp(0, 14);
      }
      ElectricityManager.ElectricityGeneratorBuffer generator = new ElectricityManager.ElectricityGeneratorBuffer((IElectricityGenerator) entity, generationPriority, false);
      this.addGenerator(generator);
      entity.AddObserver((IEntityObserver) this);
      return (IElectricityGeneratorRegistrator) generator;
    }

    public void CreateAndRegisterFor(
      IElectricityGeneratingEntityGrouped generator,
      int generationPriority,
      bool isSurplusGenerator)
    {
      if (generationPriority < 0 || generationPriority > 14)
      {
        Log.Error(string.Format("Generation priority {0} is out of range 0 - {1}", (object) generationPriority, (object) 14));
        generationPriority = generationPriority.Clamp(0, 14);
      }
      this.addGenerator(new ElectricityManager.ElectricityGeneratorBuffer((IElectricityGenerator) generator, generationPriority, isSurplusGenerator));
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
      ElectricityManager.ElectricityGeneratorBuffer electricityGeneratorBuffer = this.m_sortedGenerators.FirstOrDefault<ElectricityManager.ElectricityGeneratorBuffer>((Predicate<ElectricityManager.ElectricityGeneratorBuffer>) (x => x.Generator == entity));
      if (electricityGeneratorBuffer == null)
        Log.Error(string.Format("Failed to remove generator: {0} it was never added", (object) entity));
      else
        this.m_sortedGenerators.Remove(electricityGeneratorBuffer);
    }

    public void Invoke(SetElectricityGenerationPriorityCmd cmd)
    {
      if (cmd.Priority < 0 || cmd.Priority > 14)
      {
        cmd.SetResultError(string.Format("Priority {0} is out of range.", (object) cmd.Priority));
      }
      else
      {
        ElectricityManager.ElectricityGeneratorBuffer generator = this.m_sortedGenerators.FirstOrDefault<ElectricityManager.ElectricityGeneratorBuffer>((Predicate<ElectricityManager.ElectricityGeneratorBuffer>) (x => x.EntityId == cmd.EntityId));
        if (generator == null)
          cmd.SetResultError(string.Format("Failed to find generator: {0} it was never added", (object) cmd.EntityId));
        else if (generator.GenerationPriority == cmd.Priority)
        {
          cmd.SetResultSuccess();
        }
        else
        {
          this.m_sortedGenerators.Remove(generator);
          generator.GenerationPriority = cmd.Priority;
          this.addGenerator(generator);
          cmd.SetResultSuccess();
        }
      }
    }

    public void SetGenerationPriorityFor(IEntity entity, int priority)
    {
      if (priority < 0 || priority > 14)
        return;
      ElectricityManager.ElectricityGeneratorBuffer generator = this.m_sortedGenerators.FirstOrDefault<ElectricityManager.ElectricityGeneratorBuffer>((Predicate<ElectricityManager.ElectricityGeneratorBuffer>) (x => x.Generator == entity));
      if (generator == null || generator.GenerationPriority == priority)
        return;
      this.m_sortedGenerators.Remove(generator);
      generator.GenerationPriority = priority;
      this.addGenerator(generator);
    }

    public void Invoke(SetIsElectricitySurplusGeneratorCmd cmd)
    {
      ElectricityManager.ElectricityGeneratorBuffer generator = this.m_sortedGenerators.FirstOrDefault<ElectricityManager.ElectricityGeneratorBuffer>((Predicate<ElectricityManager.ElectricityGeneratorBuffer>) (x => x.EntityId == cmd.EntityId));
      if (generator == null)
        cmd.SetResultError(string.Format("Failed to find generator: {0} it was never added", (object) cmd.EntityId));
      else if (generator.IsSurplusGenerator == cmd.IsSurplusGenerator)
      {
        cmd.SetResultSuccess();
      }
      else
      {
        this.m_sortedGenerators.Remove(generator);
        generator.IsSurplusGenerator = cmd.IsSurplusGenerator;
        this.addGenerator(generator);
        cmd.SetResultSuccess();
      }
    }

    public void SetIsSurplusGenerator(IElectricityGeneratingEntity entity, bool isSurplusGenerator)
    {
      if (entity.ElectricityGenerator.IsSurplusGenerator == isSurplusGenerator || !(entity.ElectricityGenerator is ElectricityManager.ElectricityGeneratorBuffer electricityGenerator))
        return;
      this.m_sortedGenerators.Remove(electricityGenerator);
      electricityGenerator.IsSurplusGenerator = isSurplusGenerator;
      this.addGenerator(electricityGenerator);
    }

    public void Invoke(SetIsElectricitySurplusConsumerCmd cmd)
    {
      IElectricityConsumerInternal consumer = this.m_sortedConsumers.FirstOrDefault<IElectricityConsumerInternal>((Predicate<IElectricityConsumerInternal>) (x => x.Entity.Id == cmd.EntityId));
      if (consumer == null)
        cmd.SetResultError(string.Format("Failed to find consumer: {0} it was never added", (object) cmd.EntityId));
      else if (consumer.IsSurplusConsumer == cmd.IsSurplusConsumer)
      {
        cmd.SetResultSuccess();
      }
      else
      {
        this.m_sortedConsumers.Remove(consumer);
        consumer.SetIsSurplusConsumer(cmd.IsSurplusConsumer);
        this.AddConsumer(consumer);
        cmd.SetResultSuccess();
      }
    }

    public void SetIsSurplusConsumer(IElectricityConsumingEntity consumer, bool isSurplusConsumer)
    {
      if (consumer.ElectricityConsumer.IsNone)
      {
        Log.Error("Entity has no consumer set!");
      }
      else
      {
        if (consumer.ElectricityConsumer.Value.IsSurplusConsumer == isSurplusConsumer || !(consumer.ElectricityConsumer.Value is ElectricityConsumer consumer1))
          return;
        this.m_sortedConsumers.Remove((IElectricityConsumerInternal) consumer1);
        consumer1.SetIsSurplusConsumer(isSurplusConsumer);
        this.AddConsumer((IElectricityConsumerInternal) consumer1);
      }
    }

    public IEnumerable<ElectricityManager.ConsumptionPerProto> GetConsumptionStatsPerProto()
    {
      for (int index = 0; index < this.m_consumptionStatsPerProto.Count; ++index)
        this.m_consumptionStatsPerProto.GetRefAt(index).EntitiesTotal = 0;
      foreach (IElectricityConsumerInternal sortedConsumer in this.m_sortedConsumers)
      {
        if (sortedConsumer.IsEnabled)
          ++this.m_consumptionStatsPerProto.GetRefAt(sortedConsumer.ProtoToken).EntitiesTotal;
      }
      return this.m_consumptionStatsPerProto.AsEnumerable();
    }

    public IEnumerable<ElectricityManager.ProductionPerProto> GetProductionStatsPerProto()
    {
      for (int index = 0; index < this.m_productionStatsPerProto.Count; ++index)
        this.m_productionStatsPerProto.GetRefAt(index).EntitiesTotal = 0;
      foreach (ElectricityManager.ElectricityGeneratorBuffer sortedGenerator in this.m_sortedGenerators)
      {
        if (sortedGenerator.Generator is IElectricityGeneratingEntityGrouped generator2)
          this.m_productionStatsPerProto.GetRefAt(sortedGenerator.ProtoToken).EntitiesTotal += generator2.GeneratorsTotal;
        else if (sortedGenerator.Generator is IElectricityGeneratingEntity generator1)
          this.m_productionStatsPerProto.GetRefAt(sortedGenerator.ProtoToken).EntitiesTotal += generator1.IsEnabled ? 1 : 0;
      }
      return this.m_productionStatsPerProto.AsEnumerable();
    }

    public static void Serialize(ElectricityManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ElectricityManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ElectricityManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Electricity.Serialize(this.ConsumedThisTick, writer);
      ElectricityAvgStats.Serialize(this.ConsumptionStats, writer);
      Electricity.Serialize(this.DemandedThisTick, writer);
      writer.WriteGeneric<VirtualProductProto>(this.ElectricityProto);
      Electricity.Serialize(this.GeneratedThisTick, writer);
      ElectricityAvgStats.Serialize(this.GenerationCapacityStats, writer);
      Electricity.Serialize(this.GenerationCapacityThisTick, writer);
      LystStruct<ElectricityManager.ConsumptionPerProto>.Serialize(this.m_consumptionStatsPerProto, writer);
      Electricity.Serialize(this.m_currentFreeElectricity, writer);
      writer.WriteInt(this.m_currentGeneratorIndex);
      Electricity.Serialize(this.m_freeElectricityPerTick, writer);
      LystStruct<ElectricityManager.ProductionPerProto>.Serialize(this.m_productionStatsPerProto, writer);
      Lyst<IElectricityConsumerInternal>.Serialize(this.m_sortedConsumers, writer);
      Lyst<ElectricityManager.ElectricityGeneratorBuffer>.Serialize(this.m_sortedGenerators, writer);
      StatsManager.Serialize(this.m_statsManager, writer);
      Electricity.Serialize(this.MaxGenerationCapacity, writer);
      ElectricityAvgStats.Serialize(this.ProductionStats, writer);
      Electricity.Serialize(this.SurplusDemandedThisTick, writer);
      Electricity.Serialize(this.WastedElectricityThisTick, writer);
    }

    public static ElectricityManager Deserialize(BlobReader reader)
    {
      ElectricityManager electricityManager;
      if (reader.TryStartClassDeserialization<ElectricityManager>(out electricityManager))
        reader.EnqueueDataDeserialization((object) electricityManager, ElectricityManager.s_deserializeDataDelayedAction);
      return electricityManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.ConsumedThisTick = Electricity.Deserialize(reader);
      reader.SetField<ElectricityManager>(this, "ConsumptionStats", (object) ElectricityAvgStats.Deserialize(reader));
      this.DemandedThisTick = Electricity.Deserialize(reader);
      this.ElectricityProto = reader.ReadGenericAs<VirtualProductProto>();
      this.GeneratedThisTick = Electricity.Deserialize(reader);
      reader.SetField<ElectricityManager>(this, "GenerationCapacityStats", (object) ElectricityAvgStats.Deserialize(reader));
      this.GenerationCapacityThisTick = Electricity.Deserialize(reader);
      reader.SetField<ElectricityManager>(this, "m_consumerProtoIdsMap", (object) new Dict<IEntityProto, int>());
      this.m_consumptionStatsCache = new LystStruct<ElectricityManager.ConsumptionLastTick>();
      this.m_consumptionStatsPerProto = LystStruct<ElectricityManager.ConsumptionPerProto>.Deserialize(reader);
      this.m_currentFreeElectricity = Electricity.Deserialize(reader);
      this.m_currentGeneratorIndex = reader.ReadInt();
      this.m_freeElectricityPerTick = Electricity.Deserialize(reader);
      reader.SetField<ElectricityManager>(this, "m_producerProtoIdsMap", (object) new Dict<IEntityProto, int>());
      this.m_productionStatsCache = new LystStruct<ElectricityManager.ProductionLastTick>();
      this.m_productionStatsPerProto = LystStruct<ElectricityManager.ProductionPerProto>.Deserialize(reader);
      reader.SetField<ElectricityManager>(this, "m_sortedConsumers", (object) Lyst<IElectricityConsumerInternal>.Deserialize(reader));
      reader.SetField<ElectricityManager>(this, "m_sortedGenerators", (object) Lyst<ElectricityManager.ElectricityGeneratorBuffer>.Deserialize(reader));
      reader.SetField<ElectricityManager>(this, "m_statsManager", (object) StatsManager.Deserialize(reader));
      this.MaxGenerationCapacity = Electricity.Deserialize(reader);
      reader.SetField<ElectricityManager>(this, "ProductionStats", (object) ElectricityAvgStats.Deserialize(reader));
      this.SurplusDemandedThisTick = Electricity.Deserialize(reader);
      this.WastedElectricityThisTick = Electricity.Deserialize(reader);
      reader.RegisterInitAfterLoad<ElectricityManager>(this, "initSelf", InitPriority.High);
    }

    static ElectricityManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ElectricityManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ElectricityManager) obj).SerializeData(writer));
      ElectricityManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ElectricityManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class ElectricityGeneratorBuffer : 
      IElectricityGeneratorRegistrator,
      IComparable<ElectricityManager.ElectricityGeneratorBuffer>
    {
      /// <summary>
      /// Can be invalid if this generator is not backed by an entity.
      /// </summary>
      public readonly EntityId EntityId;
      public readonly IElectricityGenerator Generator;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      /// <summary>
      /// Unique id of generator Proto for fast collection of electricity stats.
      /// Set by ElectricityManager. Do not change, do not save.
      /// </summary>
      [DoNotSave(0, null)]
      public int ProtoToken { get; set; }

      /// <summary>Don't change this without ElectricityManager</summary>
      public int GenerationPriority { get; set; }

      /// <summary>Don't change this without ElectricityManager</summary>
      public bool IsSurplusGenerator { get; set; }

      public Electricity MaxGenerationCapacity => this.Generator.MaxGenerationCapacity;

      public Electricity GenerationCapacityThisTick { get; private set; }

      public Electricity GeneratedThisTick { get; private set; }

      public Electricity Quantity { get; private set; }

      public Electricity FreeCapacity => this.GenerationCapacityThisTick - this.Quantity;

      public ElectricityGeneratorBuffer(
        IElectricityGenerator generator,
        int generationPriority,
        bool isSurplusGenerator)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Generator = generator;
        if (generator is IElectricityGeneratingEntity generatingEntity)
          this.EntityId = generatingEntity.Id;
        this.GenerationPriority = generationPriority;
        this.IsSurplusGenerator = isSurplusGenerator;
      }

      public Electricity GenerateIntoBuffer()
      {
        bool canGenerate;
        Electricity currentMaxGeneration = this.Generator.GetCurrentMaxGeneration(out canGenerate);
        Assert.That<Electricity>(currentMaxGeneration).IsLessOrEqual(this.MaxGenerationCapacity);
        Electricity intoBuffer;
        if (canGenerate)
        {
          Electricity electricity = this.SetCapacity(currentMaxGeneration);
          this.GeneratedThisTick = this.Generator.GenerateAsMuchAs(this.FreeCapacity, currentMaxGeneration);
          Assert.That<Electricity>(this.GeneratedThisTick).IsLessOrEqual(this.MaxGenerationCapacity);
          intoBuffer = electricity + this.storeAsMuchAs(this.GeneratedThisTick);
        }
        else
        {
          intoBuffer = this.SetCapacity(Electricity.Zero);
          this.GeneratedThisTick = Electricity.Zero;
        }
        return intoBuffer;
      }

      private Electricity storeAsMuchAs(Electricity generated)
      {
        Assert.That<Electricity>(generated).IsNotNegative();
        Electricity electricity = generated < this.FreeCapacity ? generated : this.FreeCapacity;
        this.Quantity += electricity;
        return generated - electricity;
      }

      public Electricity RemoveAsMuchAs(Electricity requested)
      {
        Assert.That<Electricity>(requested).IsNotNegative();
        Electricity electricity = this.Quantity <= requested ? this.Quantity : requested;
        this.Quantity -= electricity;
        return electricity;
      }

      public Electricity SetCapacity(Electricity newCapacity)
      {
        Electricity electricity;
        if (this.Quantity > newCapacity)
        {
          electricity = this.Quantity - newCapacity;
          this.Quantity -= electricity;
        }
        else
          electricity = Electricity.Zero;
        this.GenerationCapacityThisTick = newCapacity;
        return electricity;
      }

      public int CompareTo(
        ElectricityManager.ElectricityGeneratorBuffer other)
      {
        if (this.IsSurplusGenerator == other.IsSurplusGenerator)
          return this.GenerationPriority.CompareTo(other.GenerationPriority);
        return !this.IsSurplusGenerator ? 1 : -1;
      }

      public static void Serialize(
        ElectricityManager.ElectricityGeneratorBuffer value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<ElectricityManager.ElectricityGeneratorBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, ElectricityManager.ElectricityGeneratorBuffer.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        EntityId.Serialize(this.EntityId, writer);
        Electricity.Serialize(this.GeneratedThisTick, writer);
        Electricity.Serialize(this.GenerationCapacityThisTick, writer);
        writer.WriteInt(this.GenerationPriority);
        writer.WriteGeneric<IElectricityGenerator>(this.Generator);
        writer.WriteBool(this.IsSurplusGenerator);
        Electricity.Serialize(this.Quantity, writer);
      }

      public static ElectricityManager.ElectricityGeneratorBuffer Deserialize(BlobReader reader)
      {
        ElectricityManager.ElectricityGeneratorBuffer electricityGeneratorBuffer;
        if (reader.TryStartClassDeserialization<ElectricityManager.ElectricityGeneratorBuffer>(out electricityGeneratorBuffer))
          reader.EnqueueDataDeserialization((object) electricityGeneratorBuffer, ElectricityManager.ElectricityGeneratorBuffer.s_deserializeDataDelayedAction);
        return electricityGeneratorBuffer;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<ElectricityManager.ElectricityGeneratorBuffer>(this, "EntityId", (object) EntityId.Deserialize(reader));
        this.GeneratedThisTick = Electricity.Deserialize(reader);
        this.GenerationCapacityThisTick = Electricity.Deserialize(reader);
        this.GenerationPriority = reader.ReadInt();
        reader.SetField<ElectricityManager.ElectricityGeneratorBuffer>(this, "Generator", (object) reader.ReadGenericAs<IElectricityGenerator>());
        this.IsSurplusGenerator = reader.ReadBool();
        this.Quantity = Electricity.Deserialize(reader);
      }

      static ElectricityGeneratorBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        ElectricityManager.ElectricityGeneratorBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ElectricityManager.ElectricityGeneratorBuffer) obj).SerializeData(writer));
        ElectricityManager.ElectricityGeneratorBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ElectricityManager.ElectricityGeneratorBuffer) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ConsumptionPerProto
    {
      public readonly IEntityProto ConsumerProto;
      public readonly ElectricityAvgStats ConsumptionStats;
      public ElectricityManager.ConsumptionLastTick LastTick;
      [DoNotSave(0, null)]
      public int EntitiesTotal;

      public ConsumptionPerProto(IEntityProto consumerProto, ElectricityAvgStats consumptionStats)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ConsumerProto = consumerProto;
        this.ConsumptionStats = consumptionStats;
        this.EntitiesTotal = 0;
        this.LastTick = new ElectricityManager.ConsumptionLastTick();
      }

      public void Set(
        ElectricityManager.ConsumptionLastTick lastTickData)
      {
        this.ConsumptionStats.Set(lastTickData.Consumed);
        this.LastTick = lastTickData;
      }

      public static void Serialize(ElectricityManager.ConsumptionPerProto value, BlobWriter writer)
      {
        writer.WriteGeneric<IEntityProto>(value.ConsumerProto);
        ElectricityAvgStats.Serialize(value.ConsumptionStats, writer);
        ElectricityManager.ConsumptionLastTick.Serialize(value.LastTick, writer);
      }

      public static ElectricityManager.ConsumptionPerProto Deserialize(BlobReader reader)
      {
        return new ElectricityManager.ConsumptionPerProto(reader.ReadGenericAs<IEntityProto>(), ElectricityAvgStats.Deserialize(reader))
        {
          LastTick = ElectricityManager.ConsumptionLastTick.Deserialize(reader)
        };
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ProductionPerProto
    {
      public readonly IEntityProto ProducerProto;
      public readonly ElectricityAvgStats ProductionStats;
      public ElectricityManager.ProductionLastTick LastTick;
      [DoNotSave(0, null)]
      public int EntitiesTotal;

      public ProductionPerProto(IEntityProto producerProto, ElectricityAvgStats productionStats)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ProducerProto = producerProto;
        this.ProductionStats = productionStats;
        this.EntitiesTotal = 0;
        this.LastTick = new ElectricityManager.ProductionLastTick();
      }

      public void Set(ElectricityManager.ProductionLastTick lastTickData)
      {
        this.ProductionStats.Set(lastTickData.ProduceAndWasted);
        this.LastTick = lastTickData;
      }

      public static void Serialize(ElectricityManager.ProductionPerProto value, BlobWriter writer)
      {
        writer.WriteGeneric<IEntityProto>(value.ProducerProto);
        ElectricityAvgStats.Serialize(value.ProductionStats, writer);
        ElectricityManager.ProductionLastTick.Serialize(value.LastTick, writer);
      }

      public static ElectricityManager.ProductionPerProto Deserialize(BlobReader reader)
      {
        return new ElectricityManager.ProductionPerProto(reader.ReadGenericAs<IEntityProto>(), ElectricityAvgStats.Deserialize(reader))
        {
          LastTick = ElectricityManager.ProductionLastTick.Deserialize(reader)
        };
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ConsumptionLastTick
    {
      public static readonly ElectricityManager.ConsumptionLastTick Empty;
      public Electricity Consumed;
      public Electricity Demand;
      public Electricity MaxPossibleConsumption;

      public ConsumptionLastTick(
        Electricity consumed,
        Electricity demand,
        Electricity maxPossibleConsumption)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Consumed = consumed;
        this.Demand = demand;
        this.MaxPossibleConsumption = maxPossibleConsumption;
      }

      public static void Serialize(ElectricityManager.ConsumptionLastTick value, BlobWriter writer)
      {
        Electricity.Serialize(value.Consumed, writer);
        Electricity.Serialize(value.Demand, writer);
        Electricity.Serialize(value.MaxPossibleConsumption, writer);
      }

      public static ElectricityManager.ConsumptionLastTick Deserialize(BlobReader reader)
      {
        return new ElectricityManager.ConsumptionLastTick(Electricity.Deserialize(reader), Electricity.Deserialize(reader), Electricity.Deserialize(reader));
      }

      static ConsumptionLastTick() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
    }

    [GenerateSerializer(false, null, 0)]
    public struct ProductionLastTick
    {
      public static readonly ElectricityManager.ProductionLastTick Empty;
      public Electricity Produced;
      public Electricity Wasted;
      public Electricity MaxGenerationCapacity;

      public Electricity ProduceAndWasted => this.Produced + this.Wasted;

      public ProductionLastTick(
        Electricity produced,
        Electricity wasted,
        Electricity maxGenerationCapacity)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Produced = produced;
        this.Wasted = wasted;
        this.MaxGenerationCapacity = maxGenerationCapacity;
      }

      public static void Serialize(ElectricityManager.ProductionLastTick value, BlobWriter writer)
      {
        Electricity.Serialize(value.Produced, writer);
        Electricity.Serialize(value.Wasted, writer);
        Electricity.Serialize(value.MaxGenerationCapacity, writer);
      }

      public static ElectricityManager.ProductionLastTick Deserialize(BlobReader reader)
      {
        return new ElectricityManager.ProductionLastTick(Electricity.Deserialize(reader), Electricity.Deserialize(reader), Electricity.Deserialize(reader));
      }

      static ProductionLastTick() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
    }
  }
}
