// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.StaticEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public abstract class StaticEntity : 
    Entity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IAreaSelectableStaticEntity
  {
    private StaticEntityProto m_proto;
    private ConstructionState m_constructionState;

    /// <summary>Prototype of this entity.</summary>
    [DoNotSave(0, null)]
    public StaticEntityProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (EntityProto) value;
      }
    }

    /// <summary>
    /// Tile that is considered to be the center of the entity. It is often used for distance computation.
    /// </summary>
    public Tile3i CenterTile { get; private set; }

    public Tile2f Position2f => this.CenterTile.Tile2i.CenterTile2f;

    public Tile3f Position3f => this.CenterTile.CenterXyFloorZTile3f;

    public abstract AssetValue Value { get; }

    public abstract AssetValue ConstructionCost { get; }

    /// <summary>Tile coords occupied by the entity.</summary>
    public abstract ImmutableArray<OccupiedTileRelative> OccupiedTiles { get; }

    public abstract ImmutableArray<OccupiedVertexRelative> OccupiedVertices { get; }

    public abstract LayoutTileConstraint OccupiedVerticesCombinedConstraint { get; }

    public abstract ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>> VehicleSurfaceHeights { get; }

    public abstract StaticEntityPfTargetTiles PfTargetTiles { get; }

    /// <summary>
    /// Whether entity is enabled. Not enabled entity should not accept any inputs and should not do any work. This
    /// may be caused by construction state or player's pause.
    /// </summary>
    protected override bool IsEnabledNow
    {
      get
      {
        bool isEnabledNow = base.IsEnabledNow;
        if (isEnabledNow)
        {
          bool flag;
          switch (this.ConstructionState)
          {
            case ConstructionState.Constructed:
            case ConstructionState.PreparingUpgrade:
            case ConstructionState.PendingDeconstruction:
              flag = true;
              break;
            default:
              flag = false;
              break;
          }
          isEnabledNow = flag;
        }
        return isEnabledNow;
      }
    }

    protected bool IsEnabledNowIgnoreUpgrade
    {
      get
      {
        bool nowIgnoreUpgrade = base.IsEnabledNow;
        if (nowIgnoreUpgrade)
        {
          bool flag;
          switch (this.ConstructionState)
          {
            case ConstructionState.Constructed:
            case ConstructionState.PreparingUpgrade:
            case ConstructionState.BeingUpgraded:
            case ConstructionState.PendingDeconstruction:
              flag = true;
              break;
            default:
              flag = false;
              break;
          }
          nowIgnoreUpgrade = flag;
        }
        return nowIgnoreUpgrade;
      }
    }

    protected bool IsConstructedIgnoreUpgrade
    {
      get
      {
        bool constructedIgnoreUpgrade;
        switch (this.ConstructionState)
        {
          case ConstructionState.Constructed:
          case ConstructionState.PreparingUpgrade:
          case ConstructionState.BeingUpgraded:
          case ConstructionState.PendingDeconstruction:
            constructedIgnoreUpgrade = true;
            break;
          default:
            constructedIgnoreUpgrade = false;
            break;
        }
        return constructedIgnoreUpgrade;
      }
    }

    protected bool IsEnabledIgnoreUpgradeAndDeconstruction
    {
      get
      {
        bool andDeconstruction = base.IsEnabledNow;
        if (andDeconstruction)
        {
          bool flag;
          switch (this.ConstructionState)
          {
            case ConstructionState.Constructed:
            case ConstructionState.PreparingUpgrade:
            case ConstructionState.BeingUpgraded:
            case ConstructionState.PendingDeconstruction:
            case ConstructionState.InDeconstruction:
              flag = true;
              break;
            default:
              flag = false;
              break;
          }
          andDeconstruction = flag;
        }
        return andDeconstruction;
      }
    }

    private IConstructionManager ConstructionManager => this.Context.ConstructionManager;

    public ConstructionState ConstructionState => this.m_constructionState;

    public bool IsConstructed => this.ConstructionState == ConstructionState.Constructed;

    public bool IsNotConstructed => !this.IsConstructed;

    public bool IsBeingUpgraded
    {
      get
      {
        bool isBeingUpgraded;
        switch (this.ConstructionState)
        {
          case ConstructionState.PreparingUpgrade:
          case ConstructionState.BeingUpgraded:
            isBeingUpgraded = true;
            break;
          default:
            isBeingUpgraded = false;
            break;
        }
        return isBeingUpgraded;
      }
    }

    /// <summary>
    /// Construction state is valid only when the entity is in construction or destruction.
    /// </summary>
    public Option<IEntityConstructionProgress> ConstructionProgress
    {
      get => this.ConstructionManager.GetConstructionProgress((IStaticEntity) this);
    }

    public virtual bool DoNotAdjustTerrainDuringConstruction => false;

    public abstract void NotifyUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      int newIndex,
      bool wasAdded,
      out bool canCollapse);

    /// <summary>
    /// Default implementation collapses buildings based on the number of ground vertices that violates constraint
    /// compared to the total count.
    /// </summary>
    public abstract bool TryCollapseOnUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      EntityCollapseHelper collapseHelper);

    [DoNotSave(0, null)]
    ulong IRenderedEntity.RendererData { get; set; }

    public virtual bool AreConstructionCubesDisabled => false;

    protected StaticEntity(
      EntityId id,
      StaticEntityProto prototype,
      EntityContext context,
      Tile3i centerTile)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (EntityProto) prototype, context);
      this.m_proto = prototype.CheckNotNull<StaticEntityProto>();
      this.CenterTile = centerTile;
    }

    /// <summary>
    /// Fills up custom target tiles. This happens only when a vehicle requested custom target tiles.
    /// Returns true if the vehicle is already at the goal.
    /// </summary>
    public virtual bool GetCustomPfTargetTiles(int retryNumber, Lyst<Tile2i> outTiles)
    {
      Log.Warning(string.Format("Requesting custom files from entity '{0}' that does not support it.", (object) this));
      return false;
    }

    protected override void OnAddedToWorld(EntityAddReason reason)
    {
      if (this.Prototype.DoNotStartConstructionAutomatically)
        return;
      this.StartConstructionIfNotStarted();
    }

    public void StartConstructionIfNotStarted()
    {
      if (this.m_constructionState != ConstructionState.NotInitialized)
        return;
      this.ConstructionManager.Initialize((IStaticEntity) this);
    }

    /// <summary>
    /// WARNING: Never set this directly, always use ConstructionManager otherwise rendering
    /// won't get notified about it.
    /// </summary>
    public virtual void SetConstructionState(ConstructionState state)
    {
      this.m_constructionState = state;
      this.UpdateIsEnabled();
    }

    public void MakeFullyConstructed(bool disableTerrainDisruption = false, bool doNotAdjustTerrainHeight = false)
    {
      if (this.ConstructionState == ConstructionState.NotInitialized)
        this.ConstructionManager.Initialize((IStaticEntity) this);
      if (this.IsConstructed)
        return;
      this.ConstructionManager.MarkConstructed((IStaticEntity) this, disableTerrainDisruption, doNotAdjustTerrainHeight);
    }

    /// <summary>Starts deconstruction.</summary>
    public virtual void StartDeconstructionIfCan()
    {
      if (this.ConstructionState == ConstructionState.InDeconstruction)
        return;
      this.ConstructionManager.StartDeconstruction((IStaticEntity) this);
      bool flag;
      switch (this.ConstructionState)
      {
        case ConstructionState.InDeconstruction:
        case ConstructionState.Deconstructed:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      Assert.That<bool>(flag).IsTrue();
    }

    public virtual bool CanMoveFromPendingDeconstruction() => true;

    public void MakeFullyDeconstructed()
    {
      if (this.IsDestroyed)
        return;
      if (this.ConstructionState == ConstructionState.NotInitialized)
        this.ConstructionManager.Initialize((IStaticEntity) this);
      this.ConstructionManager.MarkDeconstructed((IStaticEntity) this);
    }

    public void AbortDeconstruction()
    {
      if (this.ConstructionState == ConstructionState.InConstruction)
      {
        Log.Warning("Construction already started");
      }
      else
      {
        this.ConstructionManager.StartConstruction((IStaticEntity) this);
        this.OnDeconstructionAborted();
      }
    }

    protected virtual void OnDeconstructionAborted()
    {
    }

    public ImmutableArray<IProductBufferReadOnly> GetConstructionBuffers()
    {
      return this.ConstructionManager.GetConstructionBuffers((IStaticEntity) this);
    }

    /// <summary>
    /// Whether deconstruction can be started on this entity. If the entity needs to do some custom clearing after
    /// deconstruction started, the <see cref="!:OnDeconstructionStarted" /> method should be overridden. If this method
    /// returns <c>false</c> the entity is not removable using standard remove tool in game.
    /// </summary>
    public virtual EntityValidationResult CanStartDeconstruction()
    {
      return EntityValidationResult.Success;
    }

    public virtual ImmutableArray<ConstrCubeSpec> GetConstructionCubesSpec(out int totalCubesVolume)
    {
      return ConstructionCubesHelper.ConvertColumnsToCubes(this.CenterTile, ConstructionCubesHelper.ComputeOptimizedConstructionCubeColumns(this.OccupiedTiles), true, out totalCubesVolume);
    }

    public virtual bool IsSelected(RectangleTerrainArea2i area)
    {
      return area.ContainsTile(this.CenterTile.Xy);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Tile3i.Serialize(this.CenterTile, writer);
      writer.WriteInt((int) this.m_constructionState);
      writer.WriteGeneric<StaticEntityProto>(this.m_proto);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CenterTile = Tile3i.Deserialize(reader);
      this.m_constructionState = (ConstructionState) reader.ReadInt();
      this.m_proto = reader.ReadGenericAs<StaticEntityProto>();
    }
  }
}
