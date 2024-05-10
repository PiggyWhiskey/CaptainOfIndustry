// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IConstructionManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public interface IConstructionManager
  {
    /// <summary>
    /// Invoked when the entity is fully constructed. This can happen either after normal construction, or after
    /// cancellation of deconstruction. This is paired with <see cref="P:Mafi.Core.Entities.Static.IConstructionManager.EntityStartedDeconstruction" />.
    /// </summary>
    IEvent<IStaticEntity> EntityConstructed { get; }

    /// <summary>
    /// Invoked when the entity is started deconstruction. This can happen only after deconstruction from
    /// construction state, not after cancellation of ongoing construction. This is to keep symmetry with
    /// <see cref="P:Mafi.Core.Entities.Static.IConstructionManager.EntityConstructed" />.
    /// </summary>
    IEvent<IStaticEntity> EntityStartedDeconstruction { get; }

    /// <summary>
    /// Used in UI to track state changes. Not recommended for business logic. rather use
    /// EntityConstructed, EntityStartedDeconstruction to manage entities.
    /// </summary>
    IEvent<IStaticEntity, ConstructionState> EntityConstructionStateChanged { get; }

    void Initialize(IStaticEntity staticEntity);

    /// <summary>
    /// Resets construction animations (such as construction cubes) to the current state.
    /// </summary>
    void ResetConstructionAnimationState(IStaticEntity entity);

    /// <summary>
    /// Starts construction for given entity. This can be also used to stop deconstruction and resume construction.
    /// </summary>
    void StartConstruction(IStaticEntity staticEntity);

    /// <summary>
    /// Starts deconstruction of given entity. This can happen also when the entity is under construction.
    /// </summary>
    void StartDeconstruction(
      IStaticEntity staticEntity,
      bool doNotCreateProducts = false,
      EntityRemoveReason entityRemoveReason = EntityRemoveReason.Remove,
      bool createEmptyBuffersWithCapacity = false);

    /// <summary>
    /// This can be called by the static entity to notify this manager that it was fully constructed. This happens
    /// during "insta-build" or during r=insta-replacement of already-built entities like splitting constructed
    /// transport.
    /// </summary>
    void MarkConstructed(
      IStaticEntity staticEntity,
      bool disableTerrainDisruption = false,
      bool doNotAdjustTerrainHeight = false);

    void MarkDeconstructed(
      IStaticEntity staticEntity,
      bool disableTerrainDisruption = false,
      bool doNotRecoverTerrainHeight = false,
      EntityRemoveReason entityRemoveReason = EntityRemoveReason.Remove);

    Option<IEntityConstructionProgress> GetConstructionProgress(IStaticEntity staticEntity);

    /// <summary>
    /// Returns all products from construction/destruction buffers. This can be used during upgrades to avoid
    /// products of destroyed entity being marked as destroyed.
    /// If no construction is in progress, returns <see cref="F:Mafi.Core.Economy.AssetValue.Empty" />.
    /// </summary>
    AssetValue CancelConstructionAndReturnBuffers(IStaticEntity staticEntity);

    /// <summary>
    /// Returns deconstruction value of entity. This entity can be under construction.
    /// </summary>
    AssetValue GetDeconstructionValueFor(IStaticEntity staticEntity);

    AssetValue GetSellValue(AssetValue originalValue);

    ImmutableArray<IProductBufferReadOnly> GetConstructionBuffers(IStaticEntity staticEntity);

    /// <summary>
    /// Fills (de)construction buffers of given entity as much as possible and returns unused value.
    /// </summary>
    AssetValue FillConstructionBuffersWith(
      IStaticEntity staticEntity,
      AssetValue value,
      Percent? targetProgress);

    void MarkForPendingDeconstruction(IStaticEntity staticEntity);

    bool TrySetConstructionPause(IStaticEntity entity, bool pause);
  }
}
