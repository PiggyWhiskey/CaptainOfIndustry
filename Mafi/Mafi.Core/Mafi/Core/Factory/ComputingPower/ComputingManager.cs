// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ComputingPower.ComputingManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Factory.ComputingPower
{
  [GenerateSerializer(false, null, 0)]
  [DebuggerDisplay("Computing: prod={ProducedLastTick}, demand={DemandedThisTick}")]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class ComputingManager : IComputingManager
  {
    private Computing m_producedThisTick;
    public readonly ComputingAvgStats ProductionStats;
    public readonly ComputingAvgStats DemandStats;
    public readonly ComputingAvgStats GenerationCapacityStats;
    private readonly Lyst<IComputingConsumerInternal> m_sortedConsumers;
    private readonly Lyst<IComputingGenerator> m_generators;
    private Computing m_freeComputingPerTick;
    private readonly IProductsManager m_productsManager;
    private LystStruct<ComputingManager.ConsumptionPerProto> m_consumptionStatsPerProto;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<IEntityProto, int> m_consumerProtoIdsMap;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<ComputingManager.ConsumptionLastTick> m_consumptionStatsCache;
    private LystStruct<ComputingManager.ProductionPerProto> m_productionStatsPerProto;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<IEntityProto, int> m_producerProtoIdsMap;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private LystStruct<ComputingManager.ProductionLastTick> m_productionStatsCache;
    private readonly StatsManager m_statsManager;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public VirtualProductProto ComputingProductProto { get; private set; }

    public ImmutableArray<ProductProto> ProvidedProducts { get; private set; }

    public Computing ProducedLastTick { get; private set; }

    public Computing DemandedThisTick { get; private set; }

    public Computing GenerationCapacityThisTick { get; private set; }

    public ComputingManager(
      ISimLoopEvents simLoopEvents,
      IProductsManager productsManager,
      IConstructionManager constructionManager,
      StatsManager statsManager,
      [ProtoDep("Product_Virtual_Computing")] VirtualProductProto computingProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_sortedConsumers = new Lyst<IComputingConsumerInternal>();
      this.m_generators = new Lyst<IComputingGenerator>();
      this.m_freeComputingPerTick = Computing.Zero;
      this.m_consumerProtoIdsMap = new Dict<IEntityProto, int>();
      this.m_producerProtoIdsMap = new Dict<IEntityProto, int>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ComputingProductProto = computingProto;
      this.m_productsManager = productsManager;
      this.m_statsManager = statsManager;
      this.ProvidedProducts = ImmutableArray.Create<ProductProto>((ProductProto) computingProto);
      this.ProductionStats = new ComputingAvgStats((Option<StatsManager>) statsManager);
      this.DemandStats = new ComputingAvgStats((Option<StatsManager>) statsManager);
      this.GenerationCapacityStats = new ComputingAvgStats((Option<StatsManager>) statsManager);
      constructionManager.EntityConstructed.Add<ComputingManager>(this, new Action<IStaticEntity>(this.entityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<ComputingManager>(this, new Action<IStaticEntity>(this.entityDeconstructionStarted));
      simLoopEvents.UpdateStart.Add<ComputingManager>(this, new Action(this.updateStart));
    }

    [InitAfterLoad(InitPriority.High)]
    private void initSelf()
    {
      for (int index = 0; index < this.m_consumptionStatsPerProto.Count; ++index)
      {
        this.m_consumerProtoIdsMap.Add(this.m_consumptionStatsPerProto[index].ConsumerProto, index);
        this.m_consumptionStatsCache.Add(ComputingManager.ConsumptionLastTick.Empty);
      }
      for (int index = 0; index < this.m_productionStatsPerProto.Count; ++index)
      {
        this.m_producerProtoIdsMap.Add(this.m_productionStatsPerProto[index].ProducerProto, index);
        this.m_productionStatsCache.Add(ComputingManager.ProductionLastTick.Empty);
      }
      foreach (IComputingConsumerInternal sortedConsumer in this.m_sortedConsumers)
        this.assignProtoTokenTo(sortedConsumer);
    }

    public void Cheat_AddFreeComputingPerTick(Computing computing)
    {
      this.m_freeComputingPerTick = computing;
    }

    private void updateStart()
    {
      if (this.m_freeComputingPerTick.IsPositive)
        this.m_producedThisTick += this.m_freeComputingPerTick;
      this.GenerationCapacityThisTick = Computing.Zero;
      foreach (IComputingGenerator generator in this.m_generators)
      {
        if (generator.IsEnabled)
        {
          Computing computing = generator.GenerateComputing();
          if (computing.IsNegative)
          {
            Mafi.Log.Error("Negative computing?");
          }
          else
          {
            this.m_producedThisTick += computing;
            this.GenerationCapacityThisTick += generator.MaxComputingGenerationCapacity;
            int index;
            if (this.m_producerProtoIdsMap.TryGetValue((IEntityProto) generator.Prototype, out index) && index < this.m_productionStatsCache.Count)
            {
              ref ComputingManager.ProductionLastTick local = ref this.m_productionStatsCache.GetRefAt(index);
              local.Produced += computing;
              local.MaxGenerationCapacity += generator.MaxComputingGenerationCapacity;
            }
          }
        }
      }
      this.ProductionStats.Set(this.m_producedThisTick);
      this.DemandStats.Set(this.DemandedThisTick);
      this.GenerationCapacityStats.Set(this.GenerationCapacityThisTick);
      this.DemandedThisTick = Computing.Zero;
      this.ProducedLastTick = this.m_producedThisTick;
      Computing producedThisTick = this.m_producedThisTick;
      this.m_producedThisTick = Computing.Zero;
      for (int index = 0; index < this.m_productionStatsCache.Count; ++index)
      {
        this.m_productionStatsPerProto.GetRefAt(index).Add(this.m_productionStatsCache[index]);
        this.m_productionStatsCache[index] = ComputingManager.ProductionLastTick.Empty;
      }
      foreach (IComputingConsumerInternal sortedConsumer in this.m_sortedConsumers)
      {
        if (!sortedConsumer.IsEnabled)
        {
          sortedConsumer.RechargeSkipped();
        }
        else
        {
          ref ComputingManager.ConsumptionLastTick local = ref this.m_consumptionStatsCache.GetRefAt(sortedConsumer.ProtoToken < this.m_consumptionStatsCache.Count ? sortedConsumer.ProtoToken : 0);
          local.MaxPossibleConsumption += sortedConsumer.ComputingRequired;
          Computing computingToAdd = sortedConsumer.ComputingRequired - sortedConsumer.ComputingCharged;
          if (computingToAdd.IsPositive)
          {
            local.Demand += computingToAdd;
            this.DemandedThisTick += computingToAdd;
            if (producedThisTick >= computingToAdd)
            {
              producedThisTick -= computingToAdd;
              sortedConsumer.Recharge(computingToAdd);
              local.Consumed += computingToAdd;
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
        this.m_consumptionStatsPerProto.GetRefAt(index).Add(this.m_consumptionStatsCache[index]);
        this.m_consumptionStatsCache[index] = ComputingManager.ConsumptionLastTick.Empty;
      }
    }

    private void entityConstructed(IStaticEntity entity)
    {
      if (!(entity is IComputingGenerator generator))
        return;
      this.m_generators.AddAssertNew(generator);
      this.assignProtoTokenTo(generator);
    }

    private void entityDeconstructionStarted(IStaticEntity entity)
    {
      if (!(entity is IComputingGenerator computingGenerator))
        return;
      this.m_generators.RemoveAndAssert(computingGenerator);
    }

    private void assignProtoTokenTo(IComputingGenerator generator)
    {
      if (this.m_producerProtoIdsMap.TryGetValue((IEntityProto) generator.Prototype, out int _))
        return;
      ComputingManager.ProductionPerProto productionPerProto = new ComputingManager.ProductionPerProto((IEntityProto) generator.Prototype, new ComputingAvgStats((Option<StatsManager>) this.m_statsManager));
      int count = this.m_productionStatsPerProto.Count;
      this.m_productionStatsPerProto.Add(productionPerProto);
      this.m_productionStatsCache.Add(ComputingManager.ProductionLastTick.Empty);
      this.m_producerProtoIdsMap.Add((IEntityProto) generator.Prototype, count);
    }

    public void AddConsumer(IComputingConsumerInternal consumer)
    {
      Mafi.Assert.That<bool>(consumer.Entity.IsDestroyed).IsFalse();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_sortedConsumers.PriorityListInsertSorted<IComputingConsumerInternal>(consumer, ComputingManager.\u003C\u003EO.\u003C0\u003E__priorityProvider ?? (ComputingManager.\u003C\u003EO.\u003C0\u003E__priorityProvider = new Func<IComputingConsumerInternal, int>(priorityProvider)));
      this.assignProtoTokenTo(consumer);

      static int priorityProvider(IComputingConsumer c) => c.Priority;
    }

    public void RemoveConsumer(IComputingConsumerInternal consumer)
    {
      this.m_sortedConsumers.RemoveAndAssert(consumer);
    }

    private void assignProtoTokenTo(IComputingConsumerInternal consumer)
    {
      int count;
      if (!this.m_consumerProtoIdsMap.TryGetValue((IEntityProto) consumer.Entity.Prototype, out count))
      {
        ComputingManager.ConsumptionPerProto consumptionPerProto = new ComputingManager.ConsumptionPerProto((IEntityProto) consumer.Entity.Prototype, new ComputingAvgStats((Option<StatsManager>) this.m_statsManager));
        count = this.m_consumptionStatsPerProto.Count;
        this.m_consumptionStatsPerProto.Add(consumptionPerProto);
        this.m_consumptionStatsCache.Add(ComputingManager.ConsumptionLastTick.Empty);
        this.m_consumerProtoIdsMap.Add((IEntityProto) consumer.Entity.Prototype, count);
      }
      consumer.ProtoToken = count;
    }

    public IEnumerable<ComputingManager.ConsumptionPerProto> GetConsumptionStatsPerProto()
    {
      for (int index = 0; index < this.m_consumptionStatsPerProto.Count; ++index)
        this.m_consumptionStatsPerProto.GetRefAt(index).EntitiesTotal = 0;
      foreach (IComputingConsumerInternal sortedConsumer in this.m_sortedConsumers)
      {
        if (sortedConsumer.IsEnabled)
          ++this.m_consumptionStatsPerProto.GetRefAt(sortedConsumer.ProtoToken).EntitiesTotal;
      }
      return this.m_consumptionStatsPerProto.AsEnumerable();
    }

    public IEnumerable<ComputingManager.ProductionPerProto> GetProductionStatsPerProto()
    {
      for (int index = 0; index < this.m_productionStatsPerProto.Count; ++index)
        this.m_productionStatsPerProto.GetRefAt(index).EntitiesTotal = 0;
      foreach (IComputingGenerator generator in this.m_generators)
      {
        int index;
        if (this.m_producerProtoIdsMap.TryGetValue((IEntityProto) generator.Prototype, out index))
          this.m_productionStatsPerProto.GetRefAt(index).EntitiesTotal += generator.IsEnabled ? 1 : 0;
      }
      return this.m_productionStatsPerProto.AsEnumerable();
    }

    public static void Serialize(ComputingManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ComputingManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ComputingManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<VirtualProductProto>(this.ComputingProductProto);
      Computing.Serialize(this.DemandedThisTick, writer);
      ComputingAvgStats.Serialize(this.DemandStats, writer);
      ComputingAvgStats.Serialize(this.GenerationCapacityStats, writer);
      Computing.Serialize(this.GenerationCapacityThisTick, writer);
      LystStruct<ComputingManager.ConsumptionPerProto>.Serialize(this.m_consumptionStatsPerProto, writer);
      Computing.Serialize(this.m_freeComputingPerTick, writer);
      Lyst<IComputingGenerator>.Serialize(this.m_generators, writer);
      Computing.Serialize(this.m_producedThisTick, writer);
      LystStruct<ComputingManager.ProductionPerProto>.Serialize(this.m_productionStatsPerProto, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Lyst<IComputingConsumerInternal>.Serialize(this.m_sortedConsumers, writer);
      StatsManager.Serialize(this.m_statsManager, writer);
      Computing.Serialize(this.ProducedLastTick, writer);
      ComputingAvgStats.Serialize(this.ProductionStats, writer);
      ImmutableArray<ProductProto>.Serialize(this.ProvidedProducts, writer);
    }

    public static ComputingManager Deserialize(BlobReader reader)
    {
      ComputingManager computingManager;
      if (reader.TryStartClassDeserialization<ComputingManager>(out computingManager))
        reader.EnqueueDataDeserialization((object) computingManager, ComputingManager.s_deserializeDataDelayedAction);
      return computingManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ComputingProductProto = reader.ReadGenericAs<VirtualProductProto>();
      this.DemandedThisTick = Computing.Deserialize(reader);
      reader.SetField<ComputingManager>(this, "DemandStats", (object) ComputingAvgStats.Deserialize(reader));
      reader.SetField<ComputingManager>(this, "GenerationCapacityStats", (object) ComputingAvgStats.Deserialize(reader));
      this.GenerationCapacityThisTick = Computing.Deserialize(reader);
      reader.SetField<ComputingManager>(this, "m_consumerProtoIdsMap", (object) new Dict<IEntityProto, int>());
      this.m_consumptionStatsCache = new LystStruct<ComputingManager.ConsumptionLastTick>();
      this.m_consumptionStatsPerProto = LystStruct<ComputingManager.ConsumptionPerProto>.Deserialize(reader);
      this.m_freeComputingPerTick = Computing.Deserialize(reader);
      reader.SetField<ComputingManager>(this, "m_generators", (object) Lyst<IComputingGenerator>.Deserialize(reader));
      this.m_producedThisTick = Computing.Deserialize(reader);
      reader.SetField<ComputingManager>(this, "m_producerProtoIdsMap", (object) new Dict<IEntityProto, int>());
      this.m_productionStatsCache = new LystStruct<ComputingManager.ProductionLastTick>();
      this.m_productionStatsPerProto = LystStruct<ComputingManager.ProductionPerProto>.Deserialize(reader);
      reader.SetField<ComputingManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<ComputingManager>(this, "m_sortedConsumers", (object) Lyst<IComputingConsumerInternal>.Deserialize(reader));
      reader.SetField<ComputingManager>(this, "m_statsManager", (object) StatsManager.Deserialize(reader));
      this.ProducedLastTick = Computing.Deserialize(reader);
      reader.SetField<ComputingManager>(this, "ProductionStats", (object) ComputingAvgStats.Deserialize(reader));
      this.ProvidedProducts = ImmutableArray<ProductProto>.Deserialize(reader);
      reader.RegisterInitAfterLoad<ComputingManager>(this, "initSelf", InitPriority.High);
    }

    static ComputingManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ComputingManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ComputingManager) obj).SerializeData(writer));
      ComputingManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ComputingManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public struct ConsumptionPerProto
    {
      public readonly IEntityProto ConsumerProto;
      public readonly ComputingAvgStats ConsumptionStats;
      public ComputingManager.ConsumptionLastTick LastTick;
      [DoNotSave(0, null)]
      public int EntitiesTotal;

      public ConsumptionPerProto(IEntityProto consumerProto, ComputingAvgStats consumptionStats)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ConsumerProto = consumerProto;
        this.ConsumptionStats = consumptionStats;
        this.EntitiesTotal = 0;
        this.LastTick = new ComputingManager.ConsumptionLastTick();
      }

      public void Add(ComputingManager.ConsumptionLastTick lastTickData)
      {
        this.ConsumptionStats.Set(lastTickData.Consumed);
        this.LastTick = lastTickData;
      }

      public static void Serialize(ComputingManager.ConsumptionPerProto value, BlobWriter writer)
      {
        writer.WriteGeneric<IEntityProto>(value.ConsumerProto);
        ComputingAvgStats.Serialize(value.ConsumptionStats, writer);
        ComputingManager.ConsumptionLastTick.Serialize(value.LastTick, writer);
      }

      public static ComputingManager.ConsumptionPerProto Deserialize(BlobReader reader)
      {
        return new ComputingManager.ConsumptionPerProto(reader.ReadGenericAs<IEntityProto>(), ComputingAvgStats.Deserialize(reader))
        {
          LastTick = ComputingManager.ConsumptionLastTick.Deserialize(reader)
        };
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ProductionPerProto
    {
      public readonly IEntityProto ProducerProto;
      public readonly ComputingAvgStats ProductionStats;
      public ComputingManager.ProductionLastTick LastTick;
      [DoNotSave(0, null)]
      public int EntitiesTotal;

      public ProductionPerProto(IEntityProto producerProto, ComputingAvgStats productionStats)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ProducerProto = producerProto;
        this.ProductionStats = productionStats;
        this.EntitiesTotal = 0;
        this.LastTick = new ComputingManager.ProductionLastTick();
      }

      public void Add(ComputingManager.ProductionLastTick lastTickData)
      {
        this.ProductionStats.Set(lastTickData.Produced);
        this.LastTick = lastTickData;
      }

      public static void Serialize(ComputingManager.ProductionPerProto value, BlobWriter writer)
      {
        writer.WriteGeneric<IEntityProto>(value.ProducerProto);
        ComputingAvgStats.Serialize(value.ProductionStats, writer);
        ComputingManager.ProductionLastTick.Serialize(value.LastTick, writer);
      }

      public static ComputingManager.ProductionPerProto Deserialize(BlobReader reader)
      {
        return new ComputingManager.ProductionPerProto(reader.ReadGenericAs<IEntityProto>(), ComputingAvgStats.Deserialize(reader))
        {
          LastTick = ComputingManager.ProductionLastTick.Deserialize(reader)
        };
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ConsumptionLastTick
    {
      public static readonly ComputingManager.ConsumptionLastTick Empty;
      public Computing Consumed;
      public Computing Demand;
      public Computing MaxPossibleConsumption;

      public ConsumptionLastTick(
        Computing consumed,
        Computing demand,
        Computing maxPossibleConsumption)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Consumed = consumed;
        this.Demand = demand;
        this.MaxPossibleConsumption = maxPossibleConsumption;
      }

      public static void Serialize(ComputingManager.ConsumptionLastTick value, BlobWriter writer)
      {
        Computing.Serialize(value.Consumed, writer);
        Computing.Serialize(value.Demand, writer);
        Computing.Serialize(value.MaxPossibleConsumption, writer);
      }

      public static ComputingManager.ConsumptionLastTick Deserialize(BlobReader reader)
      {
        return new ComputingManager.ConsumptionLastTick(Computing.Deserialize(reader), Computing.Deserialize(reader), Computing.Deserialize(reader));
      }

      static ConsumptionLastTick() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
    }

    [GenerateSerializer(false, null, 0)]
    public struct ProductionLastTick
    {
      public static readonly ComputingManager.ProductionLastTick Empty;
      public Computing Produced;
      public Computing MaxGenerationCapacity;

      public ProductionLastTick(Computing produced, Computing maxGenerationCapacity)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Produced = produced;
        this.MaxGenerationCapacity = maxGenerationCapacity;
      }

      public static void Serialize(ComputingManager.ProductionLastTick value, BlobWriter writer)
      {
        Computing.Serialize(value.Produced, writer);
        Computing.Serialize(value.MaxGenerationCapacity, writer);
      }

      public static ComputingManager.ProductionLastTick Deserialize(BlobReader reader)
      {
        return new ComputingManager.ProductionLastTick(Computing.Deserialize(reader), Computing.Deserialize(reader));
      }

      static ProductionLastTick() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
    }
  }
}
