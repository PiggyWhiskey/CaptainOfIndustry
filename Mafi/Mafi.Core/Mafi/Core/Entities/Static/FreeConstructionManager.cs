// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.FreeConstructionManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Test-only construction manager that makes all construction free and instantaneous.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class FreeConstructionManager : IConstructionManager
  {
    private readonly Event<IStaticEntity, ConstructionState> m_entityConstructionStateChanged;
    private readonly Event<IStaticEntity> m_entityConstructed;
    private readonly Event<IStaticEntity> m_entityStartedDeconstruction;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IEvent<IStaticEntity, ConstructionState> EntityConstructionStateChanged
    {
      get => (IEvent<IStaticEntity, ConstructionState>) this.m_entityConstructionStateChanged;
    }

    public IEvent<IStaticEntity> EntityConstructed
    {
      get => (IEvent<IStaticEntity>) this.m_entityConstructed;
    }

    public IEvent<IStaticEntity> EntityStartedDeconstruction
    {
      get => (IEvent<IStaticEntity>) this.m_entityStartedDeconstruction;
    }

    public FreeConstructionManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityConstructed = new Event<IStaticEntity>();
      this.m_entityStartedDeconstruction = new Event<IStaticEntity>();
      this.m_entityConstructionStateChanged = new Event<IStaticEntity, ConstructionState>();
    }

    public void Initialize(IStaticEntity staticEntity)
    {
      staticEntity.SetConstructionState(ConstructionState.Constructed);
      this.m_entityConstructed.Invoke(staticEntity);
    }

    public void ResetConstructionAnimationState(IStaticEntity entity)
    {
    }

    public void StartConstruction(IStaticEntity staticEntity)
    {
      staticEntity.SetConstructionState(ConstructionState.Constructed);
      this.m_entityConstructed.Invoke(staticEntity);
      staticEntity.SetConstructionState(ConstructionState.Constructed);
    }

    public void StartDeconstruction(
      IStaticEntity staticEntity,
      bool doNotCreateProducts = false,
      EntityRemoveReason entityRemoveReason = EntityRemoveReason.Remove,
      bool createEmptyBuffersWithCapacity = false)
    {
      staticEntity.SetConstructionState(ConstructionState.InDeconstruction);
      this.m_entityStartedDeconstruction.Invoke(staticEntity);
      staticEntity.SetConstructionState(ConstructionState.Deconstructed);
      ((Entity) staticEntity).Context.EntitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) staticEntity, EntityRemoveReason.Remove);
    }

    public void MarkConstructed(
      IStaticEntity staticEntity,
      bool disableTerrainDisruption = false,
      bool doNotAdjustTerrainHeight = false)
    {
    }

    public void MarkDeconstructed(
      IStaticEntity staticEntity,
      bool disableTerrainDisruption = false,
      bool doNotRecoverTerrainHeight = false,
      EntityRemoveReason entityRemoveReason = EntityRemoveReason.Remove)
    {
    }

    public AssetValue FillConstructionBuffersWith(
      IStaticEntity staticEntity,
      AssetValue value,
      Percent? targetProgress)
    {
      return value;
    }

    public void MarkForPendingDeconstruction(IStaticEntity staticEntity)
    {
    }

    public bool TrySetConstructionPause(IStaticEntity entity, bool pause) => false;

    public Option<IEntityConstructionProgress> GetConstructionProgress(IStaticEntity staticEntity)
    {
      return (Option<IEntityConstructionProgress>) Option.None;
    }

    public AssetValue CancelConstructionAndReturnBuffers(IStaticEntity staticEntity)
    {
      staticEntity.SetConstructionState(ConstructionState.Constructed);
      return AssetValue.Empty;
    }

    public AssetValue GetDeconstructionValueFor(IStaticEntity staticEntity) => AssetValue.Empty;

    public AssetValue GetSellValue(AssetValue originalValue) => AssetValue.Empty;

    public ImmutableArray<IProductBufferReadOnly> GetConstructionBuffers(IStaticEntity staticEntity)
    {
      return ImmutableArray<IProductBufferReadOnly>.Empty;
    }

    public static void Serialize(FreeConstructionManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FreeConstructionManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FreeConstructionManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Event<IStaticEntity>.Serialize(this.m_entityConstructed, writer);
      Event<IStaticEntity, ConstructionState>.Serialize(this.m_entityConstructionStateChanged, writer);
      Event<IStaticEntity>.Serialize(this.m_entityStartedDeconstruction, writer);
    }

    public static FreeConstructionManager Deserialize(BlobReader reader)
    {
      FreeConstructionManager constructionManager;
      if (reader.TryStartClassDeserialization<FreeConstructionManager>(out constructionManager))
        reader.EnqueueDataDeserialization((object) constructionManager, FreeConstructionManager.s_deserializeDataDelayedAction);
      return constructionManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<FreeConstructionManager>(this, "m_entityConstructed", (object) Event<IStaticEntity>.Deserialize(reader));
      reader.SetField<FreeConstructionManager>(this, "m_entityConstructionStateChanged", (object) Event<IStaticEntity, ConstructionState>.Deserialize(reader));
      reader.SetField<FreeConstructionManager>(this, "m_entityStartedDeconstruction", (object) Event<IStaticEntity>.Deserialize(reader));
    }

    static FreeConstructionManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FreeConstructionManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FreeConstructionManager) obj).SerializeData(writer));
      FreeConstructionManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FreeConstructionManager) obj).DeserializeData(reader));
    }
  }
}
