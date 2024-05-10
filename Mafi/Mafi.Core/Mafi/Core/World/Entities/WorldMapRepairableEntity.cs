// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapRepairableEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public abstract class WorldMapRepairableEntity : 
    WorldMapEntity,
    IWorldMapRepairableEntity,
    IWorldMapEntity,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithSimUpdate
  {
    private static readonly Duration DURATION_TO_REPAIR;
    protected readonly Event<IWorldMapRepairableEntity> m_onConstructed;
    private readonly Event<IWorldMapRepairableEntity> m_onAllRepairProductsAvailable;
    private Option<Mafi.Core.Entities.Static.ConstructionProgress> m_repairProgress;
    private bool m_isRepaired;
    private Option<Mafi.Core.Entities.Static.ConstructionProgress> m_upgradeProgress;
    private readonly WorldMapManager m_worldMapManager;
    private readonly IInstaBuildManager m_instaBuildManager;
    private readonly IProductsManager m_productsManager;

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<IInstaBuildManager>(this.m_instaBuildManager);
      writer.WriteBool(this.m_isRepaired);
      Event<IWorldMapRepairableEntity>.Serialize(this.m_onAllRepairProductsAvailable, writer);
      Event<IWorldMapRepairableEntity>.Serialize(this.m_onConstructed, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Option<Mafi.Core.Entities.Static.ConstructionProgress>.Serialize(this.m_repairProgress, writer);
      Option<Mafi.Core.Entities.Static.ConstructionProgress>.Serialize(this.m_upgradeProgress, writer);
      WorldMapManager.Serialize(this.m_worldMapManager, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WorldMapRepairableEntity>(this, "m_instaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      this.m_isRepaired = reader.ReadBool();
      reader.SetField<WorldMapRepairableEntity>(this, "m_onAllRepairProductsAvailable", (object) Event<IWorldMapRepairableEntity>.Deserialize(reader));
      reader.SetField<WorldMapRepairableEntity>(this, "m_onConstructed", (object) Event<IWorldMapRepairableEntity>.Deserialize(reader));
      reader.SetField<WorldMapRepairableEntity>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_repairProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.Deserialize(reader);
      this.m_upgradeProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.Deserialize(reader);
      reader.SetField<WorldMapRepairableEntity>(this, "m_worldMapManager", (object) WorldMapManager.Deserialize(reader));
    }

    public override bool IsOwnedByPlayer => this.IsRepaired;

    public abstract AssetValue CostToRepair { get; }

    public IEvent<IWorldMapRepairableEntity> OnConstructionDone
    {
      get => (IEvent<IWorldMapRepairableEntity>) this.m_onConstructed;
    }

    public IEvent<IWorldMapRepairableEntity> OnAllConstructionProductsAvailable
    {
      get => (IEvent<IWorldMapRepairableEntity>) this.m_onAllRepairProductsAvailable;
    }

    public bool IsBeingRepaired => this.m_repairProgress.HasValue;

    [DoNotSave(0, null)]
    public bool IsRepaired
    {
      get => this.m_isRepaired;
      private set
      {
        if (this.m_isRepaired == value)
          return;
        this.m_isRepaired = value;
        this.UpdateIsEnabled();
      }
    }

    public bool IsBeingUpgraded => this.m_upgradeProgress.HasValue;

    public bool IsUnderConstruction => this.ConstructionProgressInternal.HasValue;

    public Option<IConstructionProgress> ConstructionProgress
    {
      get => this.ConstructionProgressInternal.As<IConstructionProgress>();
    }

    private Option<Mafi.Core.Entities.Static.ConstructionProgress> ConstructionProgressInternal
    {
      get
      {
        return (Option<Mafi.Core.Entities.Static.ConstructionProgress>) (this.m_repairProgress.ValueOrNull ?? this.m_upgradeProgress.ValueOrNull);
      }
    }

    public bool NeedsProductsForConstruction
    {
      get
      {
        return this.ConstructionProgressInternal.HasValue && this.ConstructionProgressInternal.Value.ConstructionBuffers.Any((Func<ProductBuffer, bool>) (x => x.IsNotFull()));
      }
    }

    public WorldMapRepairableEntity(
      EntityId entityId,
      WorldMapEntityProto proto,
      WorldMapLocation location,
      EntityContext context,
      WorldMapManager worldMapManager,
      IInstaBuildManager instaBuildManager,
      IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onConstructed = new Event<IWorldMapRepairableEntity>();
      this.m_onAllRepairProductsAvailable = new Event<IWorldMapRepairableEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector(entityId, (EntityProto) proto, location, context);
      this.m_worldMapManager = worldMapManager;
      this.m_instaBuildManager = instaBuildManager;
      this.m_productsManager = productsManager;
    }

    void IEntityWithSimUpdate.SimUpdate() => this.SimUpdateInternal();

    protected virtual void SimUpdateInternal()
    {
      if (this.doRepairStepIfNeeded())
        return;
      this.doUpgradeStepIfNeeded();
    }

    protected void MarkRepaired()
    {
      if (this.IsBeingRepaired)
        return;
      this.IsRepaired = true;
    }

    protected void SetUpgradeProgress(Mafi.Core.Entities.Static.ConstructionProgress progress)
    {
      if (this.m_upgradeProgress.HasValue)
      {
        Log.Error("Progress already set!");
      }
      else
      {
        this.m_upgradeProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) progress;
        this.m_worldMapManager.ReportEntityConstructionStarted((IWorldMapRepairableEntity) this);
      }
    }

    private bool doRepairStepIfNeeded()
    {
      if (this.m_repairProgress.IsNone)
        return false;
      Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_repairProgress.Value;
      constructionProgress.TryMakeStep();
      if (constructionProgress.IsDone)
      {
        this.m_productsManager.ClearBuffersAndReportProducts(constructionProgress.ConstructionBuffers, DestroyReason.Construction);
        this.m_repairProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.None;
        this.IsRepaired = true;
        this.m_onConstructed.Invoke((IWorldMapRepairableEntity) this);
        if (this.CanBePaused)
          this.SetPaused(true);
      }
      return true;
    }

    private void doUpgradeStepIfNeeded()
    {
      if (this.m_upgradeProgress.IsNone)
        return;
      Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_upgradeProgress.Value;
      constructionProgress.TryMakeStep();
      if (!constructionProgress.IsDone)
        return;
      this.m_productsManager.ClearBuffersAndReportProducts(constructionProgress.ConstructionBuffers, DestroyReason.Construction);
      this.m_upgradeProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.None;
      this.m_onConstructed.Invoke((IWorldMapRepairableEntity) this);
      this.OnUpgradeProgressDone();
    }

    protected virtual void OnUpgradeProgressDone()
    {
    }

    internal void StartRepair()
    {
      if (this.IsRepaired)
        return;
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        this.IsRepaired = true;
        this.m_onConstructed.Invoke((IWorldMapRepairableEntity) this);
      }
      else
      {
        AssetValue costToRepair = this.CostToRepair;
        ImmutableArray<ProductBuffer> immutableArray = costToRepair.Products.Map<ProductBuffer>((Func<ProductQuantity, ProductBuffer>) (x => new ProductBuffer(x.Quantity, x.Product))).ToImmutableArray();
        costToRepair = this.CostToRepair;
        int num = costToRepair.GetQuantitySum().Value;
        Duration durationPerProduct = num <= 0 ? Duration.OneTick : Duration.OneTick.Max(WorldMapRepairableEntity.DURATION_TO_REPAIR / num);
        this.m_repairProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, immutableArray, this.CostToRepair, durationPerProduct, Duration.Zero);
        this.m_worldMapManager.ReportEntityConstructionStarted((IWorldMapRepairableEntity) this);
      }
    }

    internal void CancelRepairOrUpgrade()
    {
      if (this.m_repairProgress.HasValue)
      {
        if (this.m_repairProgress.Value.AlreadyProcessedSteps > 0)
          return;
        this.m_repairProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.None;
        this.m_worldMapManager.ReportEntityConstructionCanceled((IWorldMapRepairableEntity) this);
      }
      else
      {
        if (!this.m_upgradeProgress.HasValue || this.m_upgradeProgress.Value.AlreadyProcessedSteps > 0)
          return;
        this.m_upgradeProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.None;
        this.m_worldMapManager.ReportEntityConstructionCanceled((IWorldMapRepairableEntity) this);
      }
    }

    /// <summary>Returns how much was not stored.</summary>
    public Quantity StoreAsMuchAs(ProductQuantity toStore)
    {
      Mafi.Core.Entities.Static.ConstructionProgress constructionProgress;
      if (this.m_repairProgress.HasValue)
      {
        constructionProgress = this.m_repairProgress.Value;
      }
      else
      {
        if (!this.m_upgradeProgress.HasValue)
          return toStore.Quantity;
        constructionProgress = this.m_upgradeProgress.Value;
      }
      ProductBuffer buffer = constructionProgress.ConstructionBuffers.FirstOrDefault((Func<ProductBuffer, bool>) (x => (Proto) x.Product == (Proto) toStore.Product));
      if (buffer == null || buffer.IsFull())
        return toStore.Quantity;
      Quantity quantity = buffer.StoreAsMuchAs(toStore);
      if (constructionProgress.ConstructionBuffers.All((Func<ProductBuffer, bool>) (x => x.IsFull())))
        this.m_onAllRepairProductsAvailable.Invoke((IWorldMapRepairableEntity) this);
      return quantity;
    }

    static WorldMapRepairableEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapRepairableEntity.DURATION_TO_REPAIR = 60.Seconds();
    }
  }
}
