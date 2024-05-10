// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.AnimalFarm
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GenerateSerializer(false, null, 0)]
  public class AnimalFarm : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityWithLogisticsControl,
    IEntityWithPorts,
    IAnimatedEntity,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig
  {
    public const int MAX_SLIDES_STEPS = 10;
    private AnimalFarmProto m_proto;
    private readonly ProductBuffer m_foodInputBuffer;
    private readonly ProductBuffer m_waterInputBuffer;
    private readonly Option<GlobalOutputBuffer> m_productProducedBuffer;
    private readonly GlobalOutputBuffer m_carcassBuffer;
    private PartialQuantity m_carcassPartialBuffer;
    public ImmutableArray<IProductBuffer> OutputBuffers;
    private readonly ICalendar m_calendar;
    private readonly IProductsManager m_productsManager;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private PartialQuantity m_partialWaterBuffer;
    private PartialQuantity m_partialFoodBuffer;
    private PartialQuantity m_partialProductProducedBuffer;
    private Fix32 m_animalsBornBuffer;
    private EntityNotificator m_animalFarmNotif;
    private EntityNotificator m_animalWaterNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public AnimalFarmProto Prototype
    {
      get => this.m_proto;
      private set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public AnimalFarm.State CurrentState { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IUpgrader Upgrader { get; private set; }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public IProductBuffer FoodInputBuffer => (IProductBuffer) this.m_foodInputBuffer;

    public IProductBuffer WaterInputBuffer => (IProductBuffer) this.m_waterInputBuffer;

    public Option<IProductBuffer> ProductProducedBuffer
    {
      get => this.m_productProducedBuffer.As<IProductBuffer>();
    }

    public Fix32 CarcassPerMonth
    {
      get
      {
        return !this.IsSlaughteringEnabled ? Fix32.Zero : this.AnimalsBornPerMonth * this.Prototype.CarcassMultiplier;
      }
    }

    public IProductBuffer CarcassBuffer => (IProductBuffer) this.m_carcassBuffer;

    public int AnimalsCount { get; private set; }

    public int CapacityLeft => (this.Prototype.AnimalsCapacity - this.AnimalsCount).Max(0);

    public Fix32 AnimalsBornPerMonth
    {
      get
      {
        return !this.IsGrowthPaused ? this.Prototype.AnimalsBornPer100AnimalsPerMonth * this.AnimalsCount / 100.ToFix32() : (Fix32) 0;
      }
    }

    public bool IsSlaughteringEnabled => this.SlaughterLimit.HasValue;

    public bool IsGrowthPaused { get; private set; }

    public bool AreAnimalsStarving { get; private set; }

    public bool AreAnimalsMissingWater { get; private set; }

    private int NonSatisfiedFeedDebtDaysAnimals { get; set; }

    private int NonSatisfiedWaterDebtDaysAnimals { get; set; }

    private Duration FeedDebtDuration { get; set; }

    private Duration WaterDebtDuration { get; set; }

    public int SlaughterStep
    {
      get
      {
        return !this.SlaughterLimit.HasValue ? 0 : this.SlaughterLimit.Value / (this.Prototype.AnimalsCapacity / 10);
      }
    }

    public int? SlaughterLimit { get; private set; }

    public AnimalFarm(
      EntityId id,
      AnimalFarmProto proto,
      TileTransform transform,
      EntityContext context,
      ICalendar calendar,
      IProductsManager productsManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_partialWaterBuffer = PartialQuantity.Zero;
      this.m_partialFoodBuffer = PartialQuantity.Zero;
      this.m_partialProductProducedBuffer = PartialQuantity.Zero;
      this.m_animalsBornBuffer = Fix32.Zero;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_calendar = calendar;
      this.m_productsManager = productsManager;
      this.Prototype = proto;
      this.Upgrader = upgraderFactory.CreateInstance<AnimalFarmProto, AnimalFarm>(this, this.Prototype);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.m_calendar.NewDay.Add<AnimalFarm>(this, new Action(this.onNewDay));
      this.m_calendar.NewMonth.Add<AnimalFarm>(this, new Action(this.updateStarvationOnNewMonth));
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), (IOutputBufferPriorityProvider) new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this), vehicleBuffersRegistry);
      this.m_foodInputBuffer = new ProductBuffer(this.Prototype.FoodBufferCapacity, this.Prototype.FoodPerAnimalPerMonth.Product);
      this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) this.m_foodInputBuffer);
      this.m_waterInputBuffer = new ProductBuffer(this.Prototype.WaterBufferCapacity, this.Prototype.WaterPerAnimalPerMonth.Product);
      this.m_autoLogisticsHelper.AddInputBuffer((IProductBuffer) this.m_waterInputBuffer);
      if (this.Prototype.ProducedPerAnimalPerMonth.HasValue)
      {
        this.m_productProducedBuffer = (Option<GlobalOutputBuffer>) new GlobalOutputBuffer(this.Prototype.ProducedBufferCapacity, this.Prototype.ProducedPerAnimalPerMonth.Value.Product, this.m_productsManager, 15, (IEntity) this);
        this.m_autoLogisticsHelper.AddOutputBuffer((IProductBuffer) this.m_productProducedBuffer.Value);
        this.OutputBuffers = ImmutableArray.Create<IProductBuffer>((IProductBuffer) this.m_productProducedBuffer.Value);
      }
      else
        this.OutputBuffers = (ImmutableArray<IProductBuffer>) ImmutableArray.Empty;
      this.m_animalFarmNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.AnimalFarmMissingFood);
      this.m_animalWaterNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.AnimalFarmMissingWater);
      this.m_carcassBuffer = new GlobalOutputBuffer(this.Prototype.CarcassBufferCapacity, this.Prototype.CarcassProto, this.m_productsManager, 15, (IEntity) this);
      this.m_autoLogisticsHelper.AddOutputBuffer((IProductBuffer) this.m_carcassBuffer);
    }

    /// <summary>Returns how much was added.</summary>
    public int AddAnimals(int count)
    {
      int num = (this.Prototype.AnimalsCapacity - this.AnimalsCount).Min(count);
      this.AnimalsCount += num;
      return num;
    }

    /// <summary>Returns how much was removed.</summary>
    public int RemoveAnimals(int count)
    {
      int num = this.AnimalsCount.Min(count);
      this.AnimalsCount -= num;
      return num;
    }

    public void SetSlaughterStep(int? slaughterStep)
    {
      if (!slaughterStep.HasValue)
      {
        if (this.SlaughterLimit.HasValue)
          this.SlaughterLimit = new int?();
        else
          this.SlaughterLimit = new int?(this.Prototype.AnimalsCapacity);
      }
      else
        this.SlaughterLimit = new int?(slaughterStep.Value * this.Prototype.AnimalsCapacity / 10);
    }

    public void ToggleGrowthPause() => this.IsGrowthPaused = !this.IsGrowthPaused;

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      this.m_animalFarmNotif.NotifyIff(this.AreAnimalsStarving, (IEntity) this);
      this.m_animalWaterNotif.NotifyIff(this.AreAnimalsMissingWater, (IEntity) this);
      if (this.CurrentState == AnimalFarm.State.Working || this.CurrentState == AnimalFarm.State.FullOutput)
        this.AnimationStatesProvider.Step(Percent.Hundred, Percent.Zero);
      else
        this.AnimationStatesProvider.Pause();
      if (!this.IsEnabled)
        return;
      this.sentOutputs();
    }

    private AnimalFarm.State updateState()
    {
      if (this.AreAnimalsStarving)
        return AnimalFarm.State.MissingFood;
      if (this.AreAnimalsMissingWater)
        return AnimalFarm.State.MissingWater;
      if (!this.IsEnabledNowIgnoreUpgrade)
        return AnimalFarm.State.Paused;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return AnimalFarm.State.MissingWorkers;
      if (this.m_productProducedBuffer.HasValue && this.m_productProducedBuffer.Value.IsFull)
        return AnimalFarm.State.FullOutput;
      return this.AnimalsCount == 0 ? AnimalFarm.State.NoAnimals : AnimalFarm.State.Working;
    }

    private void onNewDay()
    {
      if (this.AnimalsCount == 0)
      {
        this.m_partialFoodBuffer = PartialQuantity.Zero;
        this.m_partialWaterBuffer = PartialQuantity.Zero;
        this.FeedDebtDuration = Duration.Zero;
        this.WaterDebtDuration = Duration.Zero;
        this.NonSatisfiedFeedDebtDaysAnimals = 0;
        this.NonSatisfiedWaterDebtDaysAnimals = 0;
      }
      else
      {
        this.m_partialFoodBuffer += new PartialQuantity(this.AnimalsCount * this.Prototype.FoodPerAnimalPerMonth.Quantity.Value / 30);
        Quantity integerPart = this.m_partialFoodBuffer.IntegerPart;
        if (integerPart.IsPositive)
        {
          Quantity quantity = this.m_foodInputBuffer.RemoveAsMuchAs(this.m_partialFoodBuffer.IntegerPart);
          this.m_partialFoodBuffer -= quantity.AsPartial;
          if (quantity.IsPositive)
          {
            this.m_productsManager.ProductDestroyed(this.m_foodInputBuffer.Product, quantity, DestroyReason.Farms);
            this.NonSatisfiedFeedDebtDaysAnimals = 0;
            this.FeedDebtDuration = Duration.Zero;
          }
          else
          {
            this.NonSatisfiedFeedDebtDaysAnimals += this.AnimalsCount;
            this.FeedDebtDuration += Duration.OneDay;
          }
        }
        this.m_partialWaterBuffer += new PartialQuantity(this.AnimalsCount * this.Prototype.WaterPerAnimalPerMonth.Quantity.Value / 30);
        integerPart = this.m_partialWaterBuffer.IntegerPart;
        if (integerPart.IsPositive)
        {
          Quantity quantity = this.m_waterInputBuffer.RemoveAsMuchAs(this.m_partialWaterBuffer.IntegerPart);
          this.m_partialWaterBuffer -= quantity.AsPartial;
          if (quantity.IsPositive)
          {
            this.m_productsManager.ProductDestroyed(this.m_waterInputBuffer.Product, quantity, DestroyReason.Farms);
            this.NonSatisfiedWaterDebtDaysAnimals = 0;
            this.WaterDebtDuration = Duration.Zero;
          }
          else
          {
            this.NonSatisfiedWaterDebtDaysAnimals += this.AnimalsCount;
            this.WaterDebtDuration += Duration.OneDay;
          }
        }
        bool flag = this.NonSatisfiedFeedDebtDaysAnimals <= 0 && this.NonSatisfiedWaterDebtDaysAnimals <= 0 && this.CurrentState != AnimalFarm.State.Paused && this.CurrentState != AnimalFarm.State.MissingWorkers;
        int animalsCount1 = this.AnimalsCount;
        int? slaughterLimit;
        if (!flag)
        {
          this.m_animalsBornBuffer = Fix32.Zero;
        }
        else
        {
          if (this.Prototype.ProducedPerAnimalPerMonth.HasValue)
          {
            integerPart = this.m_partialProductProducedBuffer.IntegerPart;
            if (integerPart.IsPositive)
            {
              Quantity quantity = this.m_partialProductProducedBuffer.IntegerPart - this.m_productProducedBuffer.Value.StoreAsMuchAs(this.m_partialProductProducedBuffer.IntegerPart);
              this.m_partialProductProducedBuffer -= quantity.AsPartial;
              if (quantity.IsPositive)
                this.m_productsManager.ProductCreated(this.m_productProducedBuffer.Value.Product, quantity, CreateReason.Produced);
            }
            integerPart = this.m_partialProductProducedBuffer.IntegerPart;
            if (integerPart.IsNotPositive)
              this.m_partialProductProducedBuffer += this.AnimalsCount * this.Prototype.ProducedPerAnimalPerMonth.Value.Quantity / 30;
          }
          if (this.m_animalsBornBuffer.IntegerPart > 0)
          {
            this.AnimalsCount += this.m_animalsBornBuffer.IntegerPart;
            this.m_animalsBornBuffer -= (Fix32) this.m_animalsBornBuffer.IntegerPart;
            slaughterLimit = this.SlaughterLimit;
            if (!slaughterLimit.HasValue)
              this.AnimalsCount = this.AnimalsCount.Min(this.Prototype.AnimalsCapacity);
          }
          if (this.m_animalsBornBuffer.IntegerPart <= 0 && !this.IsGrowthPaused)
            this.m_animalsBornBuffer += this.AnimalsCount * this.Prototype.AnimalsBornPer100AnimalsPerMonth / 100 / 30;
        }
        slaughterLimit = this.SlaughterLimit;
        if (slaughterLimit.HasValue)
        {
          int animalsCount2 = this.AnimalsCount;
          slaughterLimit = this.SlaughterLimit;
          int valueOrDefault = slaughterLimit.GetValueOrDefault();
          if (animalsCount2 > valueOrDefault & slaughterLimit.HasValue)
          {
            int animalsCount3 = this.AnimalsCount;
            slaughterLimit = this.SlaughterLimit;
            int num1 = slaughterLimit.Value;
            int num2 = animalsCount3 - num1;
            this.AnimalsCount -= num2;
            integerPart = this.m_carcassPartialBuffer.IntegerPart;
            if (!integerPart.IsPositive)
              this.m_carcassPartialBuffer += new PartialQuantity(this.Prototype.CarcassMultiplier * num2);
          }
        }
        integerPart = this.m_carcassPartialBuffer.IntegerPart;
        if (integerPart.IsPositive)
        {
          Quantity quantity = this.m_carcassPartialBuffer.IntegerPart - this.m_carcassBuffer.StoreAsMuchAs(this.m_carcassPartialBuffer.IntegerPart);
          if (quantity.IsPositive)
          {
            this.m_carcassPartialBuffer -= quantity.AsPartial;
            this.m_productsManager.ProductCreated(this.Prototype.CarcassProto, quantity, CreateReason.Produced);
          }
        }
        int self = this.AnimalsCount - animalsCount1;
        if (self > 0)
        {
          this.m_productsManager.ProductCreated((ProductProto) this.Prototype.Animal, self.Quantity(), CreateReason.General);
        }
        else
        {
          if (self >= 0)
            return;
          this.m_productsManager.ProductDestroyed((ProductProto) this.Prototype.Animal, self.Abs().Quantity(), DestroyReason.Farms);
        }
      }
    }

    private void updateStarvationOnNewMonth()
    {
      this.AreAnimalsStarving = false;
      this.AreAnimalsMissingWater = false;
      if (this.AnimalsCount == 0)
        return;
      int STARVATION_GRACE_PERIOD_MONTHS = 1;
      Percent DEBT_MULT_ON_DEATH = 70.Percent();
      Percent DEATH_RATE = 5.Percent();
      updateStarvation();
      updateWater();

      void updateStarvation()
      {
        if (this.FeedDebtDuration.IsNotPositive)
        {
          this.AreAnimalsStarving = false;
        }
        else
        {
          this.m_partialFoodBuffer = this.m_partialFoodBuffer.Min(new PartialQuantity(this.AnimalsCount * this.Prototype.FoodPerAnimalPerMonth.Quantity.Value));
          this.AreAnimalsStarving = true;
          this.NonSatisfiedFeedDebtDaysAnimals = DEBT_MULT_ON_DEATH.Apply(this.NonSatisfiedFeedDebtDaysAnimals);
          if (this.FeedDebtDuration < Duration.FromMonths(STARVATION_GRACE_PERIOD_MONTHS))
            return;
          int val = this.NonSatisfiedFeedDebtDaysAnimals.CeilDiv(30);
          int num = DEATH_RATE.ApplyCeiled(val).Clamp(0, this.AnimalsCount);
          this.AnimalsCount -= num;
          this.m_productsManager.ProductDestroyed((ProductProto) this.Prototype.Animal, num.Quantity(), DestroyReason.Farms);
        }
      }

      void updateWater()
      {
        if (this.WaterDebtDuration.IsNotPositive)
        {
          this.AreAnimalsMissingWater = false;
        }
        else
        {
          this.m_partialWaterBuffer = this.m_partialWaterBuffer.Min(new PartialQuantity(this.AnimalsCount * this.Prototype.WaterPerAnimalPerMonth.Quantity.Value));
          this.AreAnimalsMissingWater = true;
          this.NonSatisfiedWaterDebtDaysAnimals = DEBT_MULT_ON_DEATH.Apply(this.NonSatisfiedWaterDebtDaysAnimals);
          if (this.WaterDebtDuration < Duration.FromMonths(STARVATION_GRACE_PERIOD_MONTHS))
            return;
          int val = this.NonSatisfiedWaterDebtDaysAnimals.CeilDiv(30);
          int num = DEATH_RATE.ApplyCeiled(val).Clamp(0, this.AnimalsCount);
          this.AnimalsCount -= num;
          this.m_productsManager.ProductDestroyed((ProductProto) this.Prototype.Animal, num.Quantity(), DestroyReason.Farms);
        }
      }
    }

    private void sentOutputs()
    {
      if (this.m_carcassBuffer.IsEmpty)
      {
        GlobalOutputBuffer valueOrNull = this.m_productProducedBuffer.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.IsEmpty ? 1 : 0) : 1) != 0)
          return;
      }
      foreach (IoPortData connectedOutputPort in this.ConnectedOutputPorts)
      {
        if ((int) connectedOutputPort.Name == (int) this.Prototype.CarcassOutpotPortName)
        {
          Quantity quantity1 = this.m_carcassBuffer.Quantity;
          if (quantity1.IsPositive)
          {
            Quantity quantity2 = quantity1 - connectedOutputPort.SendAsMuchAs(this.m_carcassBuffer.Product.WithQuantity(quantity1));
            if (quantity2.IsPositive)
            {
              this.m_carcassBuffer.RemoveExactly(quantity2);
              this.m_autoLogisticsHelper.OnProductSentToPort((IProductBuffer) this.m_carcassBuffer);
            }
          }
        }
        else if (this.ProductProducedBuffer.HasValue && connectedOutputPort.AllowedProductType == this.ProductProducedBuffer.Value.Product.Type)
        {
          IProductBuffer productBuffer = this.ProductProducedBuffer.Value;
          Quantity quantity3 = productBuffer.Quantity;
          if (quantity3.IsPositive)
          {
            Quantity quantity4 = quantity3 - connectedOutputPort.SendAsMuchAs(productBuffer.Product.WithQuantity(quantity3));
            if (quantity4.IsPositive)
            {
              productBuffer.RemoveExactly(quantity4);
              this.m_autoLogisticsHelper.OnProductSentToPort(productBuffer);
            }
          }
        }
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled)
        return pq.Quantity;
      if ((Proto) pq.Product == (Proto) this.m_foodInputBuffer.Product)
      {
        this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) this.m_foodInputBuffer);
        return this.m_foodInputBuffer.StoreAsMuchAs(pq);
      }
      if ((Proto) pq.Product == (Proto) this.m_waterInputBuffer.Product)
      {
        this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) this.m_waterInputBuffer);
        return this.m_waterInputBuffer.StoreAsMuchAs(pq);
      }
      Log.Warning(string.Format("Unexpected input product '{0}' on animal farm", (object) sourcePort.Name));
      return pq.Quantity;
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public override EntityValidationResult CanStartDeconstruction()
    {
      return this.AnimalsCount > 0 ? EntityValidationResult.CreateError((LocStrFormatted) TrCore.RemovalError__FarmHasAnimals) : EntityValidationResult.Success;
    }

    protected override void OnDestroy()
    {
      IAssetTransactionManager transactionManager = this.Context.AssetTransactionManager;
      transactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_foodInputBuffer);
      transactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_waterInputBuffer);
      transactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_carcassBuffer);
      if (this.m_productProducedBuffer.HasValue)
        transactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_productProducedBuffer.Value);
      this.m_calendar.NewDay.Remove<AnimalFarm>(this, new Action(this.onNewDay));
      this.m_calendar.NewMonth.Remove<AnimalFarm>(this, new Action(this.updateStarvationOnNewMonth));
      base.OnDestroy();
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    public bool CanDisableLogisticsInput => true;

    public bool CanDisableLogisticsOutput => true;

    public EntityLogisticsMode LogisticsInputMode => this.m_autoLogisticsHelper.LogisticsInputMode;

    public EntityLogisticsMode LogisticsOutputMode
    {
      get => this.m_autoLogisticsHelper.LogisticsOutputMode;
    }

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsInputMode(mode);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsOutputMode(mode);
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetSlaughterLimit(this.SlaughterLimit);
      data.SetIsGrowthPaused(new bool?(this.IsGrowthPaused));
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      if (this.GetType() == data.EntityType)
        this.SlaughterLimit = data.GetSlaughterLimit();
      bool? isGrowthPaused = data.GetIsGrowthPaused();
      if (!isGrowthPaused.HasValue)
        return;
      this.IsGrowthPaused = isGrowthPaused.Value;
    }

    public static void Serialize(AnimalFarm value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AnimalFarm>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AnimalFarm.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.AnimalsCount);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteBool(this.AreAnimalsMissingWater);
      writer.WriteBool(this.AreAnimalsStarving);
      writer.WriteInt((int) this.CurrentState);
      Duration.Serialize(this.FeedDebtDuration, writer);
      writer.WriteBool(this.IsGrowthPaused);
      EntityNotificator.Serialize(this.m_animalFarmNotif, writer);
      Fix32.Serialize(this.m_animalsBornBuffer, writer);
      EntityNotificator.Serialize(this.m_animalWaterNotif, writer);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      GlobalOutputBuffer.Serialize(this.m_carcassBuffer, writer);
      PartialQuantity.Serialize(this.m_carcassPartialBuffer, writer);
      ProductBuffer.Serialize(this.m_foodInputBuffer, writer);
      PartialQuantity.Serialize(this.m_partialFoodBuffer, writer);
      PartialQuantity.Serialize(this.m_partialProductProducedBuffer, writer);
      PartialQuantity.Serialize(this.m_partialWaterBuffer, writer);
      Option<GlobalOutputBuffer>.Serialize(this.m_productProducedBuffer, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<AnimalFarmProto>(this.m_proto);
      ProductBuffer.Serialize(this.m_waterInputBuffer, writer);
      writer.WriteInt(this.NonSatisfiedFeedDebtDaysAnimals);
      writer.WriteInt(this.NonSatisfiedWaterDebtDaysAnimals);
      ImmutableArray<IProductBuffer>.Serialize(this.OutputBuffers, writer);
      writer.WriteNullableStruct<int>(this.SlaughterLimit);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
      Duration.Serialize(this.WaterDebtDuration, writer);
    }

    public static AnimalFarm Deserialize(BlobReader reader)
    {
      AnimalFarm animalFarm;
      if (reader.TryStartClassDeserialization<AnimalFarm>(out animalFarm))
        reader.EnqueueDataDeserialization((object) animalFarm, AnimalFarm.s_deserializeDataDelayedAction);
      return animalFarm;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimalsCount = reader.ReadInt();
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.AreAnimalsMissingWater = reader.ReadBool();
      this.AreAnimalsStarving = reader.ReadBool();
      this.CurrentState = (AnimalFarm.State) reader.ReadInt();
      this.FeedDebtDuration = Duration.Deserialize(reader);
      this.IsGrowthPaused = reader.ReadBool();
      this.m_animalFarmNotif = EntityNotificator.Deserialize(reader);
      this.m_animalsBornBuffer = Fix32.Deserialize(reader);
      this.m_animalWaterNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<AnimalFarm>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      reader.SetField<AnimalFarm>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<AnimalFarm>(this, "m_carcassBuffer", (object) GlobalOutputBuffer.Deserialize(reader));
      this.m_carcassPartialBuffer = PartialQuantity.Deserialize(reader);
      reader.SetField<AnimalFarm>(this, "m_foodInputBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_partialFoodBuffer = PartialQuantity.Deserialize(reader);
      this.m_partialProductProducedBuffer = PartialQuantity.Deserialize(reader);
      this.m_partialWaterBuffer = PartialQuantity.Deserialize(reader);
      reader.SetField<AnimalFarm>(this, "m_productProducedBuffer", (object) Option<GlobalOutputBuffer>.Deserialize(reader));
      reader.SetField<AnimalFarm>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<AnimalFarmProto>();
      reader.SetField<AnimalFarm>(this, "m_waterInputBuffer", (object) ProductBuffer.Deserialize(reader));
      this.NonSatisfiedFeedDebtDaysAnimals = reader.ReadInt();
      this.NonSatisfiedWaterDebtDaysAnimals = reader.ReadInt();
      this.OutputBuffers = ImmutableArray<IProductBuffer>.Deserialize(reader);
      this.SlaughterLimit = reader.ReadNullableStruct<int>();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      this.WaterDebtDuration = Duration.Deserialize(reader);
    }

    static AnimalFarm()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AnimalFarm.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      AnimalFarm.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Working,
      MissingWorkers,
      MissingFood,
      MissingWater,
      NoAnimals,
      FullOutput,
    }
  }
}
