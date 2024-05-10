// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ConstructionProgress
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GenerateSerializer(false, null, 0)]
  public class ConstructionProgress : IConstructionProgress
  {
    public readonly IEntity Owner;
    private Lyst<ConstructionProgress.BufferAndCost> m_buffersAndCosts;
    /// <summary>
    /// If this is set and all buffers are full, cancelling deconstruction should result in free construction.
    /// Also, if this is set during construction, deconstruction will result in normal refund as if this entity was
    /// constructed.
    /// </summary>
    public readonly bool AllowFreeRebuild;
    public readonly Duration DurationPerProduct;
    private readonly Quantity m_totalNeeded;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Sometimes we might need to consume products during the construction (e.g. per each ship's hp repair).
    /// Updating it here will make sure this class still works and UI shows meaningful information.
    /// </summary>
    public AssetValue AlreadyRemovedCost { get; set; }

    public ImmutableArray<IProductBufferReadOnly> Buffers { get; private set; }

    public ImmutableArray<ProductBuffer> ConstructionBuffers { get; private set; }

    public AssetValue TotalCost { get; private set; }

    public Percent Progress => Percent.FromRatio(this.CurrentSteps, this.MaxSteps);

    public bool IsNearlyFinished => this.CurrentSteps > this.MaxSteps - (this.ExtraSteps >> 1);

    /// <summary>
    /// Number of currently completed construction steps. This increases for construction and decreases for
    /// destruction.
    /// </summary>
    public int CurrentSteps { get; protected set; }

    public int MaxSteps { get; private set; }

    public int ExtraSteps { get; private set; }

    /// <summary>
    /// Value that is set when this object is initialized but it is up to users to reset it whenever they process it.
    /// </summary>
    public int AlreadyProcessedSteps { get; set; }

    /// <summary>
    /// A total amount of steps that CurrentSteps can reach to (either incrementing for construction
    /// or decrementing for deconstruction).
    /// 
    /// The value is updated during <see cref="M:Mafi.Core.Entities.Static.ConstructionProgress.TryMakeStep" /> call or by calling <see cref="M:Mafi.Core.Entities.Static.ConstructionProgress.UpdateAllowedSteps" />.
    /// </summary>
    public int AllowedSteps { get; private set; }

    public bool IsInExtraStepPhase
    {
      get
      {
        return !this.IsDeconstruction ? this.CurrentSteps > this.MaxSteps - this.ExtraSteps : this.CurrentSteps < this.ExtraSteps;
      }
    }

    public bool IsDone
    {
      get => !this.IsDeconstruction ? this.CurrentSteps >= this.MaxSteps : this.CurrentSteps <= 0;
    }

    public bool WasBlockedOnProductsLastSim { get; private set; }

    public bool IsDeconstruction { get; private set; }

    public bool IsUpgrade { get; private set; }

    public bool IsPaused { get; set; }

    public bool TerrainDisruptionDisabled { get; private set; }

    public ConstructionProgress(
      IEntity owner,
      ImmutableArray<ProductBuffer> buffers,
      AssetValue totalCost,
      Duration durationPerProduct,
      Duration extraDuration,
      bool isDeconstruction = false,
      bool isUpgrade = false,
      Percent? currentStepsPerc = null,
      bool allowFreeRebuild = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_buffersAndCosts = new Lyst<ConstructionProgress.BufferAndCost>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AlreadyRemovedCost = AssetValue.Empty;
      this.IsDeconstruction = isDeconstruction;
      this.IsUpgrade = isUpgrade;
      this.TotalCost = totalCost;
      this.m_totalNeeded = this.TotalCost.GetQuantitySum();
      this.ConstructionBuffers = buffers;
      this.Buffers = this.ConstructionBuffers.As<IProductBufferReadOnly>();
      this.Owner = owner;
      this.DurationPerProduct = durationPerProduct;
      this.AllowFreeRebuild = allowFreeRebuild;
      if (buffers.IsNotEmpty)
      {
        foreach (ProductQuantity product in this.TotalCost.Products)
        {
          ProductQuantity cost = product;
          ProductBuffer productBuffer = buffers.FirstOrDefault((Func<ProductBuffer, bool>) (x => (Proto) x.Product == (Proto) cost.Product));
          if (productBuffer == null)
            Log.Error(string.Format("There is no buffer for '{0}' in costs!", (object) cost.Product));
          else
            this.m_buffersAndCosts.Add(new ConstructionProgress.BufferAndCost()
            {
              Buffer = productBuffer,
              Cost = cost
            });
        }
      }
      this.ExtraSteps = extraDuration.Ticks;
      this.MaxSteps = this.ExtraSteps + this.m_totalNeeded.Value * durationPerProduct.Ticks;
      if (this.MaxSteps <= 0)
      {
        Log.Warning("Zero max steps on construction, setting to one.");
        this.MaxSteps = 1;
      }
      if (currentStepsPerc.HasValue)
      {
        this.CurrentSteps = this.MaxSteps.ScaledByRounded(currentStepsPerc.Value).CheckWithinIncl(0, this.MaxSteps);
        this.AlreadyProcessedSteps = this.IsDeconstruction ? this.MaxSteps : 0;
      }
      else if (this.IsDeconstruction)
      {
        this.CurrentSteps = this.MaxSteps;
        this.AlreadyProcessedSteps = this.MaxSteps;
      }
      else
      {
        this.CurrentSteps = 0;
        this.AlreadyProcessedSteps = 0;
      }
    }

    public virtual bool TryMakeStep()
    {
      if (this.IsPaused)
        return false;
      return !this.IsDeconstruction ? this.makeConstructionStep() : this.makeDeconstructionStep();
    }

    private bool makeConstructionStep()
    {
      this.AllowedSteps = this.computeAllowedStepsInConstruction();
      if (this.CurrentSteps < this.AllowedSteps)
      {
        this.WasBlockedOnProductsLastSim = false;
        ++this.CurrentSteps;
        Assert.That<int>(this.CurrentSteps).IsLessOrEqual(this.MaxSteps);
        return true;
      }
      if (this.CurrentSteps > this.AllowedSteps)
      {
        this.WasBlockedOnProductsLastSim = false;
        --this.CurrentSteps;
        Assert.That<int>(this.CurrentSteps).IsNotNegative();
        return true;
      }
      this.WasBlockedOnProductsLastSim = this.CurrentSteps < this.MaxSteps;
      return !this.WasBlockedOnProductsLastSim;
    }

    private bool makeDeconstructionStep()
    {
      this.AllowedSteps = this.computeAllowedStepsInDeconstruction();
      if (this.CurrentSteps > this.AllowedSteps)
      {
        this.WasBlockedOnProductsLastSim = false;
        --this.CurrentSteps;
        Assert.That<int>(this.CurrentSteps).IsNotNegative();
        return true;
      }
      if (this.CurrentSteps < this.AllowedSteps)
      {
        this.WasBlockedOnProductsLastSim = false;
        ++this.CurrentSteps;
        Assert.That<int>(this.CurrentSteps).IsLessOrEqual(this.MaxSteps);
        return true;
      }
      this.WasBlockedOnProductsLastSim = this.CurrentSteps > 0;
      return !this.WasBlockedOnProductsLastSim;
    }

    private int computeAllowedStepsInConstruction()
    {
      Quantity quantity = this.getSumOfProductsInBuffers() + this.AlreadyRemovedCost.GetQuantitySum();
      return quantity >= this.m_totalNeeded ? this.MaxSteps : quantity.Value * this.DurationPerProduct.Ticks;
    }

    private int computeAllowedStepsInDeconstruction()
    {
      return this.getSumOfProductsInBuffers().Value * this.DurationPerProduct.Ticks;
    }

    private Quantity getSumOfProductsInBuffers()
    {
      Quantity zero = Quantity.Zero;
      foreach (ConstructionProgress.BufferAndCost buffersAndCost in this.m_buffersAndCosts)
        zero += buffersAndCost.Buffer.Quantity.Min(buffersAndCost.Cost.Quantity);
      return zero;
    }

    public Quantity GetMissingQuantityFor(ProductProto product)
    {
      ConstructionProgress.BufferAndCost bufferAndCost = this.m_buffersAndCosts.FirstOrDefault<ConstructionProgress.BufferAndCost>((Predicate<ConstructionProgress.BufferAndCost>) (x => (Proto) x.Buffer.Product == (Proto) product));
      return bufferAndCost.Cost.IsEmpty ? Quantity.Zero : bufferAndCost.Cost.Quantity - bufferAndCost.Buffer.Quantity;
    }

    /// <summary>
    /// Sets progress without adjustments for construction/deconstruction.
    /// </summary>
    public void SetProgressRaw(Percent targetProgress)
    {
      Assert.That<Percent>(targetProgress).IsWithin0To100PercIncl();
      this.CurrentSteps = targetProgress.Apply(this.MaxSteps);
    }

    public void SetProgressRawClamped(Percent targetProgress)
    {
      this.UpdateAllowedSteps();
      int self = targetProgress.Apply(this.MaxSteps);
      this.CurrentSteps = this.IsDeconstruction ? self.Max(this.AllowedSteps) : self.Min(this.AllowedSteps);
    }

    /// <summary>
    /// Pushes progress to the target value. Value 100% means that the task is finished. During deconstruction
    /// this means that the actual <see cref="P:Mafi.Core.Entities.Static.ConstructionProgress.Progress" /> will set to 0% because that's what make it 100% done.
    /// This also updates <see cref="P:Mafi.Core.Entities.Static.ConstructionProgress.AllowedSteps" /> and clamps the actual target by its value.
    /// </summary>
    public void SetAdjustedProgressTo(Percent targetProgress)
    {
      Assert.That<Percent>(targetProgress).IsWithin0To100PercIncl();
      this.UpdateAllowedSteps();
      int self = targetProgress.Apply(this.MaxSteps);
      int num = this.IsDeconstruction ? self.Max(this.AllowedSteps) : self.Min(this.AllowedSteps);
      this.CurrentSteps = this.IsDeconstruction ? this.MaxSteps - num : num;
    }

    /// <summary>
    /// Updates value of <see cref="P:Mafi.Core.Entities.Static.ConstructionProgress.AllowedSteps" /> based on contents of construction buffers.
    /// </summary>
    public void UpdateAllowedSteps()
    {
      this.AllowedSteps = this.IsDeconstruction ? this.computeAllowedStepsInDeconstruction() : this.computeAllowedStepsInConstruction();
    }

    public bool IsAllowedToFinish()
    {
      this.UpdateAllowedSteps();
      return !this.IsDeconstruction ? this.AllowedSteps >= this.MaxSteps : this.AllowedSteps <= 0;
    }

    /// <summary>
    /// Make this construction finished (sets adjusted progress to 100%). This ignores <see cref="P:Mafi.Core.Entities.Static.ConstructionProgress.AllowedSteps" />.
    /// </summary>
    public void MakeFinished() => this.CurrentSteps = this.IsDeconstruction ? 0 : this.MaxSteps;

    public Upoints CostForQuickBuild(IAssetTransactionManager assetManager, out bool hasProducts)
    {
      if (this.IsDeconstruction || this.Owner.Prototype.Costs.IsQuickBuildDisabled)
      {
        hasProducts = false;
        return Upoints.Zero;
      }
      if (!this.IsDeconstruction && this.Owner.Prototype.HasParam<DisableQuickBuildParam>() && this is EntityConstructionProgress)
      {
        hasProducts = false;
        return Upoints.Zero;
      }
      int quantity1 = 0;
      hasProducts = true;
      foreach (ConstructionProgress.BufferAndCost buffersAndCost in this.m_buffersAndCosts)
      {
        Quantity quantity2 = buffersAndCost.Cost.Quantity - buffersAndCost.Buffer.Quantity;
        if (!quantity2.IsNotPositive)
        {
          if (quantity2 > assetManager.GetAvailableQuantityForRemoval(buffersAndCost.Buffer.Product))
            hasProducts = false;
          quantity1 += quantity2.Value;
        }
      }
      return QuickDeliverCostHelper.QuantityToUnityCost(quantity1, this.Owner.Context.UpointsManager.QuickActionCostMultiplier) ?? Upoints.Zero;
    }

    public Upoints? CostForQuickRemove(IAssetTransactionManager assetManager)
    {
      if (!this.IsDeconstruction)
        return new Upoints?();
      int quantity = 0;
      foreach (IProductBufferReadOnly buffer in this.Buffers)
        quantity += buffer.Quantity.Value;
      return QuickDeliverCostHelper.QuantityToUnityCost(quantity, this.Owner.Context.UpointsManager.QuickActionCostMultiplier);
    }

    public bool TryPerformQuickBuild(
      IAssetTransactionManager assetManager,
      IUpointsManager upointsManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry)
    {
      bool hasProducts;
      Upoints unity = this.CostForQuickBuild(assetManager, out hasProducts);
      if (unity.IsZero || !hasProducts || !upointsManager.TryConsume(IdsCore.UpointsCategories.QuickBuild, unity))
        return false;
      if (this.Owner is IStaticEntity owner)
      {
        foreach (ConstructionProgress.BufferAndCost buffersAndCost in this.m_buffersAndCosts)
        {
          Quantity maxToRemove = buffersAndCost.Cost.Quantity - buffersAndCost.Buffer.Quantity;
          if (!maxToRemove.IsNotPositive)
          {
            Option<RegisteredInputBuffer> inputBuffer = vehicleBuffersRegistry.TryGetInputBuffer(owner, buffersAndCost.Buffer.Product);
            if (inputBuffer.HasValue)
            {
              foreach (IVehicleJob immutable in inputBuffer.Value.AllReservedJobs.ToImmutableArray<IVehicleJob>())
              {
                if (immutable is CargoDeliveryJob cargoDeliveryJob)
                {
                  bool shouldJobBeCanceled;
                  Quantity quantity = cargoDeliveryJob.TryRemoveQuantityForQuickDeliver(maxToRemove, inputBuffer.Value, out shouldJobBeCanceled);
                  buffersAndCost.Buffer.StoreExactly(quantity);
                  if (shouldJobBeCanceled)
                    immutable.RequestCancel();
                }
              }
            }
          }
        }
      }
      foreach (ConstructionProgress.BufferAndCost buffersAndCost in this.m_buffersAndCosts)
      {
        Quantity quantity1 = buffersAndCost.Cost.Quantity - buffersAndCost.Buffer.Quantity;
        if (!quantity1.IsNotPositive)
        {
          Quantity quantity2 = assetManager.RemoveAsMuchAs(buffersAndCost.Buffer.Product.WithQuantity(quantity1), new DestroyReason?());
          buffersAndCost.Buffer.StoreExactly(quantity2);
        }
      }
      return true;
    }

    public bool PerformQuickRemove(
      IAssetTransactionManager assetManager,
      IUpointsManager upointsManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      bool noErrorOnMissingCost = false)
    {
      ImmutableArray<ProductBuffer> constructionBuffers = this.ConstructionBuffers;
      if (constructionBuffers.All((Func<ProductBuffer, bool>) (x => x.IsEmpty)))
        return false;
      Upoints? nullable = this.CostForQuickRemove(assetManager);
      if (!nullable.HasValue)
      {
        if (!noErrorOnMissingCost)
          Log.Error(string.Format("Cannot quick remove if there is no unity cost! Owner: '{0}'.", (object) this.Owner));
        return false;
      }
      if (!upointsManager.TryConsume(IdsCore.UpointsCategories.QuickRemove, nullable.Value))
        return false;
      constructionBuffers = this.ConstructionBuffers;
      foreach (ProductBuffer buffer in constructionBuffers)
        assetManager.ClearBuffer((IProductBuffer) buffer);
      if (this.Owner is IStaticEntity owner)
      {
        constructionBuffers = this.ConstructionBuffers;
        foreach (ProductBuffer productBuffer in constructionBuffers)
        {
          if (productBuffer.IsEmpty)
            vehicleBuffersRegistry.TryGetOutputBuffer(owner, productBuffer.Product).ValueOrNull?.ClearAndCancelAllJobs();
        }
      }
      this.CurrentSteps = this.CurrentSteps.Min(this.ExtraSteps);
      return true;
    }

    public void CheatFull(IProductsManager productsManager)
    {
      foreach (ConstructionProgress.BufferAndCost buffersAndCost in this.m_buffersAndCosts)
      {
        ProductBuffer buffer = buffersAndCost.Buffer;
        Quantity quantity = buffer.UsableCapacity - buffer.StoreAsMuchAs(buffer.Product.WithQuantity(buffer.UsableCapacity));
        productsManager.ProductCreated(buffer.Product, quantity, CreateReason.Cheated);
      }
    }

    public void CheatEmpty(IProductsManager productsManager)
    {
      foreach (ConstructionProgress.BufferAndCost buffersAndCost in this.m_buffersAndCosts)
      {
        ProductBuffer buffer = buffersAndCost.Buffer;
        Quantity quantity = buffer.RemoveAll();
        productsManager.ProductDestroyed(buffer.Product, quantity, DestroyReason.Cheated);
      }
    }

    public void SetTerrainDisruptionDuringConstruction(bool terrainDisruptionDisabled)
    {
      this.TerrainDisruptionDisabled = terrainDisruptionDisabled;
    }

    public static void Serialize(ConstructionProgress value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ConstructionProgress>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ConstructionProgress.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.AllowedSteps);
      writer.WriteBool(this.AllowFreeRebuild);
      writer.WriteInt(this.AlreadyProcessedSteps);
      AssetValue.Serialize(this.AlreadyRemovedCost, writer);
      ImmutableArray<IProductBufferReadOnly>.Serialize(this.Buffers, writer);
      ImmutableArray<ProductBuffer>.Serialize(this.ConstructionBuffers, writer);
      writer.WriteInt(this.CurrentSteps);
      Duration.Serialize(this.DurationPerProduct, writer);
      writer.WriteInt(this.ExtraSteps);
      writer.WriteBool(this.IsDeconstruction);
      writer.WriteBool(this.IsPaused);
      writer.WriteBool(this.IsUpgrade);
      Lyst<ConstructionProgress.BufferAndCost>.Serialize(this.m_buffersAndCosts, writer);
      Quantity.Serialize(this.m_totalNeeded, writer);
      writer.WriteInt(this.MaxSteps);
      writer.WriteGeneric<IEntity>(this.Owner);
      writer.WriteBool(this.TerrainDisruptionDisabled);
      AssetValue.Serialize(this.TotalCost, writer);
      writer.WriteBool(this.WasBlockedOnProductsLastSim);
    }

    public static ConstructionProgress Deserialize(BlobReader reader)
    {
      ConstructionProgress constructionProgress;
      if (reader.TryStartClassDeserialization<ConstructionProgress>(out constructionProgress))
        reader.EnqueueDataDeserialization((object) constructionProgress, ConstructionProgress.s_deserializeDataDelayedAction);
      return constructionProgress;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllowedSteps = reader.ReadInt();
      reader.SetField<ConstructionProgress>(this, "AllowFreeRebuild", (object) reader.ReadBool());
      this.AlreadyProcessedSteps = reader.ReadInt();
      this.AlreadyRemovedCost = AssetValue.Deserialize(reader);
      this.Buffers = ImmutableArray<IProductBufferReadOnly>.Deserialize(reader);
      this.ConstructionBuffers = ImmutableArray<ProductBuffer>.Deserialize(reader);
      this.CurrentSteps = reader.ReadInt();
      reader.SetField<ConstructionProgress>(this, "DurationPerProduct", (object) Duration.Deserialize(reader));
      this.ExtraSteps = reader.ReadInt();
      this.IsDeconstruction = reader.ReadBool();
      this.IsPaused = reader.ReadBool();
      this.IsUpgrade = reader.ReadBool();
      this.m_buffersAndCosts = Lyst<ConstructionProgress.BufferAndCost>.Deserialize(reader);
      reader.SetField<ConstructionProgress>(this, "m_totalNeeded", (object) Quantity.Deserialize(reader));
      this.MaxSteps = reader.ReadInt();
      reader.SetField<ConstructionProgress>(this, "Owner", (object) reader.ReadGenericAs<IEntity>());
      this.TerrainDisruptionDisabled = reader.ReadBool();
      this.TotalCost = AssetValue.Deserialize(reader);
      this.WasBlockedOnProductsLastSim = reader.ReadBool();
    }

    static ConstructionProgress()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ConstructionProgress.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ConstructionProgress) obj).SerializeData(writer));
      ConstructionProgress.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ConstructionProgress) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private struct BufferAndCost
    {
      public ProductQuantity Cost;
      public ProductBuffer Buffer;

      public static void Serialize(ConstructionProgress.BufferAndCost value, BlobWriter writer)
      {
        ProductBuffer.Serialize(value.Buffer, writer);
        ProductQuantity.Serialize(value.Cost, writer);
      }

      public static ConstructionProgress.BufferAndCost Deserialize(BlobReader reader)
      {
        return new ConstructionProgress.BufferAndCost()
        {
          Buffer = ProductBuffer.Deserialize(reader),
          Cost = ProductQuantity.Deserialize(reader)
        };
      }
    }
  }
}
