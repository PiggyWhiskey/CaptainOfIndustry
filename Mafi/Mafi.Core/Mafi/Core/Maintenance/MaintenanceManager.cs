// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.MaintenanceManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Game;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class MaintenanceManager : 
    IVirtualBufferProvider,
    ICommandProcessor<QuickRepairEntityCmd>,
    IAction<QuickRepairEntityCmd>
  {
    private readonly Event<VirtualProductProto> m_notEnoughMaintenanceThisMonth;
    public readonly IMaintenanceConfig Config;
    private readonly Dict<ProductProto, MaintenanceManager.Buffer> m_buffers;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly IUpointsManager m_upointsManager;
    [NewInSaveVersion(140, null, null, typeof (GameDifficultyConfig), null)]
    private readonly GameDifficultyConfig m_gameDifficultyConfig;
    private readonly IRandom m_random;
    private readonly IProperty<Percent> m_maintenanceConsumptionMultiplier;
    private bool m_maintenanceDisabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Returns duration of a new entity when it has guaranteed 100% reliability.
    /// </summary>
    public Duration GetNewEntityWontBreakPeriod(EntityProto entity)
    {
      return this.Config.BufferMaxCapacity.ScaledBy(entity.Costs.Maintenance.InitialMaintenanceBoost) + this.Config.BufferMaxCapacity.ScaledBy(Percent.Hundred - this.Config.ReliabilityIssuesStartAt);
    }

    public IEvent<VirtualProductProto> NotEnoughMaintenanceThisMonth
    {
      get => (IEvent<VirtualProductProto>) this.m_notEnoughMaintenanceThisMonth;
    }

    public ImmutableArray<ProductProto> ProvidedProducts { get; private set; }

    [NewInSaveVersion(140, null, null, null, null)]
    public bool CanSlowDownIfBroken { get; set; }

    public Percent ConsumptionMultiplier => this.m_maintenanceConsumptionMultiplier.Value;

    public IEnumerable<IMaintenanceBufferReadonly> MaintenanceBuffers
    {
      get => (IEnumerable<IMaintenanceBufferReadonly>) this.m_buffers.Values;
    }

    public MaintenanceManager(
      IMaintenanceConfig config,
      ICalendar calendar,
      IPropertiesDb propertiesDb,
      ProtosDb protos,
      ProductsManager productsManager,
      UnlockedProtosDb unlockedProtosDb,
      IEntitiesManager entitiesManager,
      IUpointsManager upointsManager,
      INotificationsManager notificationsManager,
      IConstructionManager constructionManager,
      RandomProvider randomProvider,
      StatsManager statsManager,
      GameDifficultyConfig gameDifficultyConfig)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_notEnoughMaintenanceThisMonth = new Event<VirtualProductProto>();
      this.m_buffers = new Dict<ProductProto, MaintenanceManager.Buffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Config = config;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_entitiesManager = entitiesManager;
      this.m_upointsManager = upointsManager;
      this.m_gameDifficultyConfig = gameDifficultyConfig;
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      ImmutableArray<VirtualProductProto> immutableArray = protos.All<VirtualProductProto>().Where<VirtualProductProto>((Func<VirtualProductProto, bool>) (x => x.TryGetParam<MaintenanceProtoParam>(out MaintenanceProtoParam _))).ToImmutableArray<VirtualProductProto>();
      this.ProvidedProducts = immutableArray.CastArray<ProductProto>();
      foreach (VirtualProductProto virtualProductProto in immutableArray)
        this.m_buffers.Add((ProductProto) virtualProductProto, new MaintenanceManager.Buffer(virtualProductProto, this, productsManager, notificationsManager, calendar, statsManager));
      this.m_maintenanceConsumptionMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.MaintenanceConsumptionMultiplier);
      this.m_maintenanceConsumptionMultiplier.OnChange.Add<MaintenanceManager>(this, new Action<Percent>(this.onCostModifierChanged));
      IProperty<bool> property = propertiesDb.GetProperty<bool>(IdsCore.PropertyIds.SlowDownIfBroken);
      property.OnChange.Add<MaintenanceManager>(this, new Action<bool>(this.onSlowerIfBrokenChanged));
      this.CanSlowDownIfBroken = property.Value;
      constructionManager.EntityConstructed.Add<MaintenanceManager>(this, new Action<IStaticEntity>(this.entityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<MaintenanceManager>(this, new Action<IStaticEntity>(this.entityDeconstructionStarted));
      calendar.NewDay.Add<MaintenanceManager>(this, new Action(this.onNewDay));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      IProperty<bool> property = resolver.Resolve<IPropertiesDb>().GetProperty<bool>(IdsCore.PropertyIds.SlowDownIfBroken);
      property.OnChange.Add<MaintenanceManager>(this, new Action<bool>(this.onSlowerIfBrokenChanged));
      this.CanSlowDownIfBroken = property.Value;
    }

    private void entityConstructed(IStaticEntity entity)
    {
      if (!(entity is MaintenanceDepot depot))
        return;
      this.addDepot(depot);
    }

    private void entityDeconstructionStarted(IStaticEntity entity)
    {
      if (!(entity is MaintenanceDepot depot))
        return;
      this.removeDepot(depot);
    }

    private void onCostModifierChanged(Percent modifier)
    {
      foreach (MaintenanceManager.Buffer buffer in this.m_buffers.Values)
      {
        foreach (EntityMaintenanceProvider sortedProvider in buffer.SortedProviders)
          sortedProvider.OnCostModifierChanged();
      }
    }

    private void onSlowerIfBrokenChanged(bool shouldBrokenEntityWork)
    {
      this.CanSlowDownIfBroken = shouldBrokenEntityWork;
    }

    public Upoints GetQuickRepairCost(
      VirtualProductProto product,
      PartialQuantity missingPointsToFull)
    {
      if (!this.m_gameDifficultyConfig.CanQuickRepair || missingPointsToFull.IsNotPositive)
        return Upoints.Zero;
      MaintenanceProtoParam paramValue;
      if (product.TryGetParam<MaintenanceProtoParam>(out paramValue))
      {
        Upoints upoints = paramValue.QuickRepairCost * missingPointsToFull.Value;
        upoints = upoints.Max(0.1.Upoints());
        return upoints.ScaledBy(this.m_upointsManager.QuickActionCostMultiplier);
      }
      Log.Error("Maintenance product without `MaintenanceProtoParam`?");
      return Upoints.Zero;
    }

    public Option<IProductBuffer> GetBuffer(ProductProto product, Option<IEntity> entity)
    {
      MaintenanceManager.Buffer buffer;
      return !this.m_buffers.TryGetValue(product, out buffer) ? Option<IProductBuffer>.None : (Option<IProductBuffer>) (IProductBuffer) buffer;
    }

    private void onNewDay()
    {
      foreach (MaintenanceManager.Buffer buffer in this.m_buffers.Values)
      {
        buffer.MonthlyNeededMaintenanceMax = PartialQuantity.Zero;
        buffer.MonthlyNeededMaintenance = PartialQuantity.Zero;
      }
      if (this.m_maintenanceDisabled)
        return;
      foreach (MaintenanceManager.Buffer buffer in this.m_buffers.Values)
      {
        buffer.MonthlyNeededMaintenanceMax = PartialQuantity.Zero;
        buffer.MonthlyNeededMaintenance = PartialQuantity.Zero;
        foreach (EntityMaintenanceProvider sortedProvider in buffer.SortedProviders)
        {
          if (sortedProvider.Status.IsBroken || sortedProvider.Entity.IsEnabled)
          {
            if (sortedProvider.Costs.MaxMaintenancePerMonth.IsNotPositive)
            {
              sortedProvider.SetMaintenanceStatus(new MaintenanceStatus(false, PartialQuantity.Zero, PartialQuantity.Zero, Percent.Zero, Fix32.Zero));
              return;
            }
            int protoToken = sortedProvider.ProtoToken < buffer.ConsumptionStatsCache.Count ? sortedProvider.ProtoToken : 0;
            ref MaintenanceManager.ConsumptionLastTick local = ref buffer.ConsumptionStatsCache.GetRefAt(protoToken);
            PartialQuantity monthlyCost = sortedProvider.Entity.IsIdleForMaintenance ? new PartialQuantity(this.Config.IdleMaintenanceMultiplier.Apply(sortedProvider.Costs.MaintenancePerMonth.Value)) : sortedProvider.Costs.MaintenancePerMonth;
            local.MaxPossibleConsumption += sortedProvider.Costs.MaintenancePerMonth;
            local.Demand += monthlyCost;
            buffer.MonthlyNeededMaintenanceMax += sortedProvider.Costs.MaintenancePerMonth;
            buffer.MonthlyNeededMaintenance += monthlyCost;
            this.applyDailyMaintenance(sortedProvider, monthlyCost, buffer);
          }
        }
        if (buffer.ConsumptionStatsCache.Validate() && buffer.ConsumptionStatsPerProto.Validate())
        {
          if (buffer.ConsumptionStatsPerProto.Count != buffer.ConsumptionStatsCache.Count)
          {
            Log.Error(string.Format("Count mismatch between stats `{0}` ", (object) buffer.ConsumptionStatsPerProto.Count) + string.Format("and cache `{0}`.", (object) buffer.ConsumptionStatsCache.Count));
            buffer.ConsumptionStatsCache.Resize(buffer.ConsumptionStatsPerProto.Count);
          }
          for (int index = 0; index < buffer.ConsumptionStatsPerProto.Count; ++index)
          {
            buffer.ConsumptionStatsPerProto.GetRefAt(index).Add(buffer.ConsumptionStatsCache[index]);
            buffer.ConsumptionStatsCache[index] = MaintenanceManager.ConsumptionLastTick.Empty;
          }
          buffer.ReportConsumption();
        }
      }
    }

    private void applyDailyMaintenance(
      EntityMaintenanceProvider provider,
      PartialQuantity monthlyCost,
      MaintenanceManager.Buffer buffer)
    {
      PartialQuantity partialQuantity = (monthlyCost / 30).ScaledBy(Percent.Hundred - provider.Status.CurrentBreakdownChance.Squared());
      PartialQuantity newMaintenance = provider.Status.IsBroken ? provider.Status.MaintenancePointsCurrent : (provider.Status.MaintenancePointsCurrent - partialQuantity).Max(PartialQuantity.Zero);
      this.setMaintenanceCost(provider, newMaintenance, buffer);
    }

    /// <summary>Applies extra maintenance cost to given provider.</summary>
    public void ApplyExtraMaintenanceCost(EntityMaintenanceProvider provider, PartialQuantity cost)
    {
      this.setMaintenanceCost(provider, (provider.Status.MaintenancePointsCurrent - cost).Max(PartialQuantity.Zero));
    }

    /// <summary>
    /// Applies multiplier to the maintenance buffer of given provider. Value is clamped to range
    /// [0, max maintenance].
    /// </summary>
    public void ApplyExtraMaintenanceCost(EntityMaintenanceProvider provider, Percent mult)
    {
      this.setMaintenanceCost(provider, provider.Status.MaintenancePointsCurrent.ScaledBy(mult).Clamp(PartialQuantity.Zero, provider.Status.MaintenancePointsMax));
    }

    private void setMaintenanceCost(
      EntityMaintenanceProvider provider,
      PartialQuantity newMaintenance,
      MaintenanceManager.Buffer buffer = null)
    {
      PartialQuantity maxMaintenance = this.getMaxMaintenance(provider.Costs);
      PartialQuantity maxQuantity = maxMaintenance - newMaintenance;
      if (maxQuantity.IsPositive)
      {
        Percent scale = this.hasReliabilityIssues(newMaintenance, maxMaintenance) ? this.Config.MaxReplenishSpeed : (this.Config.MaxReplenishSpeed / 2).Max(Percent.Hundred);
        PartialQuantity partialQuantity = (this.Config.BaseReplenishPerMonth + provider.Costs.MaintenancePerMonth.ScaledBy(scale)) / 30;
        if (maxQuantity > partialQuantity)
          maxQuantity = partialQuantity;
        if (buffer != null)
        {
          newMaintenance += buffer.RemoveAsMuchAs(maxQuantity);
        }
        else
        {
          MaintenanceManager.Buffer buffer1;
          if (this.m_buffers.TryGetValue((ProductProto) provider.Costs.Product, out buffer1))
            newMaintenance += buffer1.RemoveAsMuchAs(maxQuantity);
          else
            Log.Error(string.Format("Entity '{0}' has unknown maintenance product '{1}' ", (object) provider.Entity, (object) provider.Costs.Product) + "Missing a `CoreTags.MaintenanceProduct` tag on the maintenance product proto?.");
        }
      }
      Percent breakdownChance = this.computeBreakdownChance(newMaintenance, maxMaintenance);
      Fix32 brokenDurationDays = provider.Status.BrokenDurationDays + breakdownChance.ToFix32();
      bool isBroken;
      if (provider.Status.IsBroken)
      {
        brokenDurationDays -= (Fix32) 1;
        isBroken = brokenDurationDays.IsPositive;
      }
      else
        isBroken = brokenDurationDays > this.getBrokenThreshold(breakdownChance).Days && this.m_random.TestProbability(this.Config.DailyBreakdownChanceWhenShouldBeBroken);
      provider.SetMaintenanceStatus(new MaintenanceStatus(isBroken, newMaintenance, maxMaintenance, breakdownChance, brokenDurationDays));
    }

    private PartialQuantity getMaxMaintenance(MaintenanceCosts costs)
    {
      return costs.MaxMaintenancePerMonth * this.Config.BufferMaxCapacity.Months.ToIntRounded();
    }

    private Percent computeBreakdownChance(PartialQuantity current, PartialQuantity max)
    {
      Percent percent = Percent.FromRatio(current.Value, max.Value);
      if (percent >= this.Config.ReliabilityIssuesStartAt)
        return Percent.Zero;
      Percent t = percent / this.Config.ReliabilityIssuesStartAt;
      Assert.That<Percent>(t).IsWithin0To100PercIncl();
      return this.Config.MaxBreakdownChance.Lerp(Percent.Zero, t);
    }

    private bool hasReliabilityIssues(PartialQuantity current, PartialQuantity max)
    {
      return !(Percent.FromRatio(current.Value, max.Value) >= this.Config.ReliabilityIssuesStartAt);
    }

    private Duration getBrokenThreshold(Percent breakdownChance)
    {
      return this.Config.BrokenDurationMin.Lerp(this.Config.BrokenDurationMax, breakdownChance / this.Config.MaxBreakdownChance);
    }

    public MaintenanceStatus AddNewEntityHandler(
      EntityMaintenanceProvider handler,
      Percent? percentMaintained = null)
    {
      Assert.That<PartialQuantity>(handler.Costs.MaintenancePerMonth).IsPositive();
      Assert.That<bool>(handler.IsDestroyed).IsFalse();
      MaintenanceManager.Buffer buffer;
      if (!this.m_buffers.TryGetValue((ProductProto) handler.Costs.Product, out buffer))
        Assert.Fail(string.Format("Maintenance buffer for {0} not found!", (object) handler.Costs.Product));
      else
        buffer.AddConsumer(handler);
      PartialQuantity maxMaintenance = this.getMaxMaintenance(handler.Costs);
      PartialQuantity partialQuantity = maxMaintenance.ScaledBy(handler.Costs.InitialMaintenanceBoost);
      PartialQuantity maintenancePointsCurrent = maxMaintenance + partialQuantity;
      if (percentMaintained.HasValue)
        maintenancePointsCurrent = maintenancePointsCurrent.ScaledBy(percentMaintained.Value);
      return new MaintenanceStatus(false, maintenancePointsCurrent, maxMaintenance, Percent.Zero, Fix32.Zero);
    }

    public void RemoveEntityHandler(EntityMaintenanceProvider handler)
    {
      MaintenanceManager.Buffer buffer;
      if (!this.m_buffers.TryGetValue((ProductProto) handler.Costs.Product, out buffer))
        Log.Error(string.Format("Maintenance buffer for {0} not found!", (object) handler.Costs.Product));
      else
        buffer.RemoveConsumer(handler);
    }

    public void UpdatePriorityFor(EntityMaintenanceProvider handler)
    {
      this.RemoveEntityHandler(handler);
      this.AddNewEntityHandler(handler);
    }

    public void CHEAT_SetMaintenanceEnabled(bool isEnabled)
    {
      this.m_maintenanceDisabled = !isEnabled;
    }

    public void Cheat_FillAllMaintenanceBuffers()
    {
      foreach (MaintenanceManager.Buffer buffer in this.m_buffers.Values)
      {
        if (buffer.UsableCapacity.IsPositive)
          buffer.StoreExactly(buffer.UsableCapacity);
      }
    }

    public void Invoke(QuickRepairEntityCmd cmd)
    {
      IMaintainedEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<IMaintainedEntity>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("Failed to find maintained entity '{0}'.", (object) cmd.EntityId));
      }
      else
      {
        Upoints repairCost = entity.Maintenance.RepairCost;
        if (!this.m_upointsManager.TryConsume(IdsCore.UpointsCategories.QuickRepair, repairCost))
        {
          cmd.SetResultError("Not enough Unity for quick-repair.");
        }
        else
        {
          entity.Maintenance.SetCurrentMaintenanceTo(Percent.Hundred);
          cmd.SetResultSuccess();
        }
      }
    }

    private void addDepot(MaintenanceDepot depot)
    {
      foreach (MaintenanceManager.Buffer buffer1 in this.m_buffers.Values)
      {
        MaintenanceManager.Buffer buffer = buffer1;
        if (depot.Prototype.Recipes.Any<RecipeProto>((Predicate<RecipeProto>) (r => r.AllOutputs.Any((Func<RecipeOutput, bool>) (x => (Proto) x.Product == (Proto) buffer.Product)))) && buffer.AddDepot(depot))
          buffer.SetCapacity(buffer.Capacity + depot.Prototype.MaintenanceBufferExtraCapacity);
      }
    }

    private void removeDepot(MaintenanceDepot depot)
    {
      foreach (MaintenanceManager.Buffer buffer1 in this.m_buffers.Values)
      {
        MaintenanceManager.Buffer buffer = buffer1;
        if (depot.Prototype.Recipes.Any<RecipeProto>((Predicate<RecipeProto>) (r => r.AllOutputs.Any((Func<RecipeOutput, bool>) (x => (Proto) x.Product == (Proto) buffer.Product)))) && buffer.RemoveDepot(depot))
          buffer.SetCapacity(buffer.Capacity - depot.Prototype.MaintenanceBufferExtraCapacity);
      }
    }

    public void ReportUpgradeDone(MaintenanceDepot depot, Quantity capacityDiff)
    {
      foreach (MaintenanceManager.Buffer buffer in this.m_buffers.Values)
      {
        MaintenanceManager.Buffer buff = buffer;
        if (depot.RecipesAssigned.Any<RecipeProto>((Predicate<RecipeProto>) (r => r.AllOutputs.Any((Func<RecipeOutput, bool>) (p => (Proto) p.Product == (Proto) buff.Product)))))
          buff.SetCapacity(buff.Capacity + capacityDiff);
      }
    }

    public IEnumerable<MaintenanceManager.ConsumptionPerProto> GetConsumptionStatsPerProto(
      ProductProto product)
    {
      MaintenanceManager.Buffer buffer;
      return this.m_buffers.TryGetValue(product, out buffer) ? buffer.GetConsumptionStatsPerProto() : Enumerable.Empty<MaintenanceManager.ConsumptionPerProto>();
    }

    public static void Serialize(MaintenanceManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MaintenanceManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MaintenanceManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.CanSlowDownIfBroken);
      writer.WriteGeneric<IMaintenanceConfig>(this.Config);
      Dict<ProductProto, MaintenanceManager.Buffer>.Serialize(this.m_buffers, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      GameDifficultyConfig.Serialize(this.m_gameDifficultyConfig, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_maintenanceConsumptionMultiplier);
      writer.WriteBool(this.m_maintenanceDisabled);
      Event<VirtualProductProto>.Serialize(this.m_notEnoughMaintenanceThisMonth, writer);
      writer.WriteGeneric<IRandom>(this.m_random);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
      ImmutableArray<ProductProto>.Serialize(this.ProvidedProducts, writer);
    }

    public static MaintenanceManager Deserialize(BlobReader reader)
    {
      MaintenanceManager maintenanceManager;
      if (reader.TryStartClassDeserialization<MaintenanceManager>(out maintenanceManager))
        reader.EnqueueDataDeserialization((object) maintenanceManager, MaintenanceManager.s_deserializeDataDelayedAction);
      return maintenanceManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CanSlowDownIfBroken = reader.LoadedSaveVersion >= 140 && reader.ReadBool();
      reader.SetField<MaintenanceManager>(this, "Config", (object) reader.ReadGenericAs<IMaintenanceConfig>());
      reader.SetField<MaintenanceManager>(this, "m_buffers", (object) Dict<ProductProto, MaintenanceManager.Buffer>.Deserialize(reader));
      reader.SetField<MaintenanceManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<MaintenanceManager>(this, "m_gameDifficultyConfig", reader.LoadedSaveVersion >= 140 ? (object) GameDifficultyConfig.Deserialize(reader) : (object) (GameDifficultyConfig) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<MaintenanceManager>(this, "m_gameDifficultyConfig", typeof (GameDifficultyConfig), true);
      reader.SetField<MaintenanceManager>(this, "m_maintenanceConsumptionMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.m_maintenanceDisabled = reader.ReadBool();
      reader.SetField<MaintenanceManager>(this, "m_notEnoughMaintenanceThisMonth", (object) Event<VirtualProductProto>.Deserialize(reader));
      reader.SetField<MaintenanceManager>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      reader.SetField<MaintenanceManager>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.SetField<MaintenanceManager>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      this.ProvidedProducts = ImmutableArray<ProductProto>.Deserialize(reader);
      reader.RegisterInitAfterLoad<MaintenanceManager>(this, "initSelf", InitPriority.Normal);
    }

    static MaintenanceManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MaintenanceManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MaintenanceManager) obj).SerializeData(writer));
      MaintenanceManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MaintenanceManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class Buffer : IProductBuffer, IProductBufferReadOnly, IMaintenanceBufferReadonly
    {
      private VirtualProductProto m_product;
      private PartialQuantity m_consumedThisMonth;
      private PartialQuantity m_consumedUnreportedPartial;
      private PartialQuantity m_producedThisMonth;
      private PartialQuantity m_quantity;
      private bool m_notEnoughMaintenanceThisMonth;
      private readonly MaintenanceManager m_maintenanceManager;
      private readonly ProductsManager m_productsManager;
      private NotificatorWithProtoParam m_notEnoughMaintenanceNotif;
      private readonly Lyst<MaintenanceDepot> m_depots;
      private readonly Lyst<EntityMaintenanceProvider> m_sortedProviders;
      internal LystStruct<MaintenanceManager.ConsumptionPerProto> ConsumptionStatsPerProto;
      [DoNotSaveCreateNewOnLoad(null, 0)]
      private readonly Dict<IEntityProto, int> m_consumerProtoIdsMap;
      [DoNotSaveCreateNewOnLoad(null, 0)]
      internal LystStruct<MaintenanceManager.ConsumptionLastTick> ConsumptionStatsCache;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public ProductProto Product => (ProductProto) this.m_product;

      public Quantity UsableCapacity => (this.Capacity - this.Quantity).Max(Quantity.Zero);

      public Quantity Capacity { get; private set; }

      public Quantity Quantity => this.m_quantity.IntegerPart;

      public PartialQuantity DeltaLastMonth { get; private set; }

      public bool ShouldBeLastDeltaReported { get; private set; }

      public QuantitySumStats ProducedTotalStats { get; private set; }

      public QuantitySumStats ConsumedTotalStats { get; private set; }

      public PartialQuantity MonthlyNeededMaintenance { get; set; }

      /// <summary>Does not take idleness into account.</summary>
      public PartialQuantity MonthlyNeededMaintenanceMax { get; set; }

      public bool ShouldShowInUi
      {
        get
        {
          return this.m_depots.IsNotEmpty || this.Quantity.IsPositive || this.MonthlyNeededMaintenance.IsPositive || this.m_consumedThisMonth.IsPositive;
        }
      }

      public IIndexable<EntityMaintenanceProvider> SortedProviders
      {
        get => (IIndexable<EntityMaintenanceProvider>) this.m_sortedProviders;
      }

      public Buffer(
        VirtualProductProto product,
        MaintenanceManager maintenanceManager,
        ProductsManager productsManager,
        INotificationsManager notificationsManager,
        ICalendar calendar,
        StatsManager statsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_depots = new Lyst<MaintenanceDepot>();
        this.m_sortedProviders = new Lyst<EntityMaintenanceProvider>();
        this.m_consumerProtoIdsMap = new Dict<IEntityProto, int>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_product = product;
        this.m_maintenanceManager = maintenanceManager;
        this.m_productsManager = productsManager;
        calendar.NewMonth.Add<MaintenanceManager.Buffer>(this, new Action(this.onNewMonth));
        this.ProducedTotalStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
        this.ConsumedTotalStats = new QuantitySumStats((Option<StatsManager>) statsManager, true);
        this.m_notEnoughMaintenanceNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.NotEnoughMaintenance);
      }

      [InitAfterLoad(InitPriority.High)]
      private void initSelf(int saveVersion)
      {
        if (this.ConsumptionStatsCache.IsNotEmpty)
          Log.Error(string.Format("ConsumptionStatsCache is not empty, has {0} elements!", (object) this.ConsumptionStatsCache.Count));
        for (int index = 0; index < this.ConsumptionStatsPerProto.Count; ++index)
        {
          this.m_consumerProtoIdsMap.Add(this.ConsumptionStatsPerProto[index].Proto, index);
          this.ConsumptionStatsCache.Add(MaintenanceManager.ConsumptionLastTick.Empty);
        }
        foreach (EntityMaintenanceProvider sortedProvider in this.m_sortedProviders)
          this.assignProtoTokenTo(sortedProvider);
        if (saveVersion >= 108 || this.verifyAssignersOrder())
          return;
        this.m_sortedProviders.Sort((Comparison<EntityMaintenanceProvider>) ((x, y) => x.Priority.CompareTo(y.Priority)));
      }

      private bool verifyAssignersOrder()
      {
        for (int index = 1; index < this.m_sortedProviders.Count; ++index)
        {
          if (this.m_sortedProviders[index - 1].Priority > this.m_sortedProviders[index].Priority)
            return false;
        }
        return true;
      }

      public void AddConsumer(EntityMaintenanceProvider provider)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_sortedProviders.PriorityListInsertSorted<EntityMaintenanceProvider>(provider, MaintenanceManager.Buffer.\u003C\u003EO.\u003C0\u003E__priorityProvider ?? (MaintenanceManager.Buffer.\u003C\u003EO.\u003C0\u003E__priorityProvider = new Func<EntityMaintenanceProvider, int>(priorityProvider)));
        this.assignProtoTokenTo(provider);

        static int priorityProvider(EntityMaintenanceProvider h) => h.Priority;
      }

      public void RemoveConsumer(EntityMaintenanceProvider provider)
      {
        this.m_sortedProviders.RemoveAndAssert(provider);
      }

      Quantity IProductBuffer.StoreAsMuchAs(Quantity quantity)
      {
        Assert.That<Quantity>(quantity).IsNotNegative();
        Quantity quantity1 = quantity < this.UsableCapacity ? quantity : this.UsableCapacity;
        if (quantity1.IsPositive)
        {
          this.m_quantity += quantity1.AsPartial;
          this.ProducedTotalStats.Add((QuantityLarge) quantity1);
          this.m_producedThisMonth += quantity1.AsPartial;
        }
        return quantity - quantity1;
      }

      Quantity IProductBuffer.RemoveAsMuchAs(Quantity maxQuantity)
      {
        Log.Error("Maintenance cannot be consumed from global buffer.");
        return Quantity.Zero;
      }

      public PartialQuantity RemoveAsMuchAs(PartialQuantity maxQuantity)
      {
        Assert.That<PartialQuantity>(maxQuantity).IsNotNegative();
        PartialQuantity partialQuantity = this.Quantity.AsPartial <= maxQuantity ? this.Quantity.AsPartial : maxQuantity;
        this.m_quantity -= partialQuantity;
        this.m_consumedThisMonth += partialQuantity;
        this.m_consumedUnreportedPartial += partialQuantity;
        this.m_notEnoughMaintenanceThisMonth |= partialQuantity < maxQuantity;
        return partialQuantity;
      }

      public bool AddDepot(MaintenanceDepot depot)
      {
        if (this.m_depots.Contains(depot))
          return false;
        this.m_depots.Add(depot);
        return true;
      }

      public bool RemoveDepot(MaintenanceDepot depot) => this.m_depots.Remove(depot);

      public void SetCapacity(Quantity newCapacity)
      {
        if (newCapacity.IsNegative)
          Log.Error(string.Format("Cannot set negative capacity {0}", (object) newCapacity));
        else
          this.Capacity = newCapacity;
      }

      public void ReportConsumption()
      {
        Quantity integerPart = this.m_consumedUnreportedPartial.IntegerPart;
        if (!integerPart.IsPositive)
          return;
        this.m_productsManager.ProductDestroyed((ProductProto) this.m_product, integerPart, DestroyReason.Maintenance);
        this.ConsumedTotalStats.Add((QuantityLarge) integerPart);
        this.m_consumedUnreportedPartial = this.m_consumedUnreportedPartial.FractionalPart;
      }

      private void onNewMonth()
      {
        bool flag = false;
        foreach (Machine depot in this.m_depots)
        {
          if (depot.CurrentState == Machine.State.OutputFull)
          {
            flag = true;
            break;
          }
        }
        this.DeltaLastMonth = this.m_producedThisMonth - this.m_consumedThisMonth;
        this.ShouldBeLastDeltaReported = !flag || this.DeltaLastMonth.IsPositive;
        this.m_producedThisMonth = PartialQuantity.Zero;
        this.m_consumedThisMonth = PartialQuantity.Zero;
        this.m_notEnoughMaintenanceNotif.NotifyIff((Proto) this.Product, this.m_notEnoughMaintenanceThisMonth || this.DeltaLastMonth.IsNegative && (this.Capacity.IsZero || Percent.FromRatio(this.Quantity.Value, this.Capacity.Value) < 50.Percent()));
        if (this.m_notEnoughMaintenanceThisMonth)
          this.m_maintenanceManager.m_notEnoughMaintenanceThisMonth.Invoke(this.m_product);
        this.m_notEnoughMaintenanceThisMonth = false;
      }

      public IEnumerable<MaintenanceManager.ConsumptionPerProto> GetConsumptionStatsPerProto()
      {
        for (int index = 0; index < this.ConsumptionStatsPerProto.Count; ++index)
          this.ConsumptionStatsPerProto.GetRefAt(index).EntitiesTotal = 0;
        foreach (EntityMaintenanceProvider sortedProvider in this.SortedProviders)
        {
          if (sortedProvider.Entity.IsEnabled)
            ++this.ConsumptionStatsPerProto.GetRefAt(sortedProvider.ProtoToken).EntitiesTotal;
        }
        return this.ConsumptionStatsPerProto.AsEnumerable();
      }

      private void assignProtoTokenTo(EntityMaintenanceProvider consumer)
      {
        int count;
        if (!this.m_consumerProtoIdsMap.TryGetValue((IEntityProto) consumer.Entity.Prototype, out count))
        {
          MaintenanceManager.ConsumptionPerProto consumptionPerProto = new MaintenanceManager.ConsumptionPerProto((IEntityProto) consumer.Entity.Prototype);
          count = this.ConsumptionStatsPerProto.Count;
          this.ConsumptionStatsPerProto.Add(consumptionPerProto);
          this.ConsumptionStatsCache.Add(MaintenanceManager.ConsumptionLastTick.Empty);
          this.m_consumerProtoIdsMap.Add((IEntityProto) consumer.Entity.Prototype, count);
        }
        consumer.ProtoToken = count;
      }

      public static void Serialize(MaintenanceManager.Buffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<MaintenanceManager.Buffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, MaintenanceManager.Buffer.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Quantity.Serialize(this.Capacity, writer);
        QuantitySumStats.Serialize(this.ConsumedTotalStats, writer);
        LystStruct<MaintenanceManager.ConsumptionPerProto>.Serialize(this.ConsumptionStatsPerProto, writer);
        PartialQuantity.Serialize(this.DeltaLastMonth, writer);
        PartialQuantity.Serialize(this.m_consumedThisMonth, writer);
        PartialQuantity.Serialize(this.m_consumedUnreportedPartial, writer);
        Lyst<MaintenanceDepot>.Serialize(this.m_depots, writer);
        MaintenanceManager.Serialize(this.m_maintenanceManager, writer);
        NotificatorWithProtoParam.Serialize(this.m_notEnoughMaintenanceNotif, writer);
        writer.WriteBool(this.m_notEnoughMaintenanceThisMonth);
        PartialQuantity.Serialize(this.m_producedThisMonth, writer);
        writer.WriteGeneric<VirtualProductProto>(this.m_product);
        ProductsManager.Serialize(this.m_productsManager, writer);
        PartialQuantity.Serialize(this.m_quantity, writer);
        Lyst<EntityMaintenanceProvider>.Serialize(this.m_sortedProviders, writer);
        PartialQuantity.Serialize(this.MonthlyNeededMaintenance, writer);
        PartialQuantity.Serialize(this.MonthlyNeededMaintenanceMax, writer);
        QuantitySumStats.Serialize(this.ProducedTotalStats, writer);
        writer.WriteBool(this.ShouldBeLastDeltaReported);
      }

      public static MaintenanceManager.Buffer Deserialize(BlobReader reader)
      {
        MaintenanceManager.Buffer buffer;
        if (reader.TryStartClassDeserialization<MaintenanceManager.Buffer>(out buffer))
          reader.EnqueueDataDeserialization((object) buffer, MaintenanceManager.Buffer.s_deserializeDataDelayedAction);
        return buffer;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.Capacity = Quantity.Deserialize(reader);
        this.ConsumedTotalStats = QuantitySumStats.Deserialize(reader);
        this.ConsumptionStatsCache = new LystStruct<MaintenanceManager.ConsumptionLastTick>();
        this.ConsumptionStatsPerProto = LystStruct<MaintenanceManager.ConsumptionPerProto>.Deserialize(reader);
        this.DeltaLastMonth = PartialQuantity.Deserialize(reader);
        this.m_consumedThisMonth = PartialQuantity.Deserialize(reader);
        this.m_consumedUnreportedPartial = PartialQuantity.Deserialize(reader);
        reader.SetField<MaintenanceManager.Buffer>(this, "m_consumerProtoIdsMap", (object) new Dict<IEntityProto, int>());
        reader.SetField<MaintenanceManager.Buffer>(this, "m_depots", (object) Lyst<MaintenanceDepot>.Deserialize(reader));
        reader.SetField<MaintenanceManager.Buffer>(this, "m_maintenanceManager", (object) MaintenanceManager.Deserialize(reader));
        this.m_notEnoughMaintenanceNotif = NotificatorWithProtoParam.Deserialize(reader);
        this.m_notEnoughMaintenanceThisMonth = reader.ReadBool();
        this.m_producedThisMonth = PartialQuantity.Deserialize(reader);
        this.m_product = reader.ReadGenericAs<VirtualProductProto>();
        reader.SetField<MaintenanceManager.Buffer>(this, "m_productsManager", (object) ProductsManager.Deserialize(reader));
        this.m_quantity = PartialQuantity.Deserialize(reader);
        reader.SetField<MaintenanceManager.Buffer>(this, "m_sortedProviders", (object) Lyst<EntityMaintenanceProvider>.Deserialize(reader));
        this.MonthlyNeededMaintenance = PartialQuantity.Deserialize(reader);
        this.MonthlyNeededMaintenanceMax = PartialQuantity.Deserialize(reader);
        this.ProducedTotalStats = QuantitySumStats.Deserialize(reader);
        this.ShouldBeLastDeltaReported = reader.ReadBool();
        reader.RegisterInitAfterLoad<MaintenanceManager.Buffer>(this, "initSelf", InitPriority.High);
      }

      static Buffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        MaintenanceManager.Buffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MaintenanceManager.Buffer) obj).SerializeData(writer));
        MaintenanceManager.Buffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MaintenanceManager.Buffer) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ConsumptionPerProto
    {
      public readonly IEntityProto Proto;
      public MaintenanceManager.ConsumptionLastTick LastTick;
      [DoNotSave(0, null)]
      public int EntitiesTotal;

      public ConsumptionPerProto(IEntityProto proto)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Proto = proto;
        this.EntitiesTotal = 0;
        this.LastTick = new MaintenanceManager.ConsumptionLastTick();
      }

      public void Add(
        MaintenanceManager.ConsumptionLastTick lastTickData)
      {
        this.LastTick = lastTickData;
      }

      public static void Serialize(MaintenanceManager.ConsumptionPerProto value, BlobWriter writer)
      {
        writer.WriteGeneric<IEntityProto>(value.Proto);
        MaintenanceManager.ConsumptionLastTick.Serialize(value.LastTick, writer);
      }

      public static MaintenanceManager.ConsumptionPerProto Deserialize(BlobReader reader)
      {
        return new MaintenanceManager.ConsumptionPerProto(reader.ReadGenericAs<IEntityProto>())
        {
          LastTick = MaintenanceManager.ConsumptionLastTick.Deserialize(reader)
        };
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ConsumptionLastTick
    {
      public static readonly MaintenanceManager.ConsumptionLastTick Empty;
      public PartialQuantity Demand;
      public PartialQuantity MaxPossibleConsumption;

      public ConsumptionLastTick(PartialQuantity demand, PartialQuantity maxPossibleConsumption)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Demand = demand;
        this.MaxPossibleConsumption = maxPossibleConsumption;
      }

      public static void Serialize(MaintenanceManager.ConsumptionLastTick value, BlobWriter writer)
      {
        PartialQuantity.Serialize(value.Demand, writer);
        PartialQuantity.Serialize(value.MaxPossibleConsumption, writer);
      }

      public static MaintenanceManager.ConsumptionLastTick Deserialize(BlobReader reader)
      {
        return new MaintenanceManager.ConsumptionLastTick(PartialQuantity.Deserialize(reader), PartialQuantity.Deserialize(reader));
      }

      static ConsumptionLastTick() => MBiHIp97M4MqqbtZOh.rMWAw2OR8();
    }
  }
}
