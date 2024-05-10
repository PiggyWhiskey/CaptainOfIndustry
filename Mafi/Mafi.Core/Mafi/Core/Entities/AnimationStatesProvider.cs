// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.AnimationStatesProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Animations;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class AnimationStatesProvider : IEntityObserverForUpgrade, IEntityObserver
  {
    private readonly IAnimationStateFactory m_factory;
    [DoNotSave(0, null)]
    private ImmutableArray<IAnimationStateImpl> m_animationStatesImpl;
    private readonly IAnimatedEntity m_entity;
    private Duration m_currentProcessDuration;
    private Percent? m_utilization;
    private Percent? m_progress;
    private bool m_isPaused;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ImmutableArray<IAnimationState> AnimationStates
    {
      get => this.m_animationStatesImpl.CastArray<IAnimationState>();
    }

    public AnimationStatesProvider(IAnimatedEntity entity, IAnimationStateFactory factory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_isPaused = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entity = entity;
      this.m_factory = factory;
      this.m_animationStatesImpl = this.createAnimationStatesImpl(entity);
      if (!(entity is IUpgradableEntity))
        return;
      entity.AddObserver((IEntityObserver) this);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_animationStatesImpl = this.createAnimationStatesImpl(this.m_entity);
      bool isPaused = this.m_isPaused;
      bool flag = false;
      if (this.m_currentProcessDuration.IsPositive)
      {
        this.Start(this.m_currentProcessDuration);
        flag = true;
      }
      if (this.m_utilization.HasValue && this.m_progress.HasValue)
      {
        this.Step(this.m_utilization.Value, this.m_progress.Value);
        flag = true;
      }
      if (!(isPaused & flag))
        return;
      this.Pause();
    }

    private ImmutableArray<IAnimationStateImpl> createAnimationStatesImpl(IAnimatedEntity entity)
    {
      ImmutableArray<IAnimationStateImpl> animationStatesImpl = this.m_factory.Create(entity.AnimationParams);
      foreach (IAnimationStateImpl animationStateImpl in animationStatesImpl)
        animationStateImpl.Initialize((IEntity) entity);
      return animationStatesImpl;
    }

    void IEntityObserverForUpgrade.OnEntityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      if (entity is IAnimatedEntity entity1)
        this.m_animationStatesImpl = this.createAnimationStatesImpl(entity1);
      else
        Log.Warning("Entity upgraded but is not IAnimatedEntity (AnimationStatesProvider).");
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
    }

    public void Pause()
    {
      foreach (IAnimationStateImpl animationStateImpl in this.m_animationStatesImpl)
        animationStateImpl.Pause();
      this.m_isPaused = true;
    }

    public void Start(Duration currentProcessDuration)
    {
      for (int index = 0; index < this.m_animationStatesImpl.Length; ++index)
        this.m_animationStatesImpl[index].Start((IEntity) this.m_entity, currentProcessDuration, index);
      this.m_isPaused = false;
      this.m_currentProcessDuration = currentProcessDuration;
    }

    public void Step(Percent utilization, Percent progress)
    {
      foreach (IAnimationStateImpl animationStateImpl in this.m_animationStatesImpl)
        animationStateImpl.Step(utilization, progress);
      this.m_isPaused = false;
      this.m_utilization = new Percent?(utilization);
      this.m_progress = new Percent?(progress);
    }

    public static void Serialize(AnimationStatesProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AnimationStatesProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AnimationStatesProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Duration.Serialize(this.m_currentProcessDuration, writer);
      writer.WriteGeneric<IAnimatedEntity>(this.m_entity);
      writer.WriteBool(this.m_isPaused);
      writer.WriteNullableStruct<Percent>(this.m_progress);
      writer.WriteNullableStruct<Percent>(this.m_utilization);
    }

    public static AnimationStatesProvider Deserialize(BlobReader reader)
    {
      AnimationStatesProvider animationStatesProvider;
      if (reader.TryStartClassDeserialization<AnimationStatesProvider>(out animationStatesProvider))
        reader.EnqueueDataDeserialization((object) animationStatesProvider, AnimationStatesProvider.s_deserializeDataDelayedAction);
      return animationStatesProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_currentProcessDuration = Duration.Deserialize(reader);
      reader.SetField<AnimationStatesProvider>(this, "m_entity", (object) reader.ReadGenericAs<IAnimatedEntity>());
      reader.RegisterResolvedMember<AnimationStatesProvider>(this, "m_factory", typeof (IAnimationStateFactory), true);
      this.m_isPaused = reader.ReadBool();
      this.m_progress = reader.ReadNullableStruct<Percent>();
      this.m_utilization = reader.ReadNullableStruct<Percent>();
      reader.RegisterInitAfterLoad<AnimationStatesProvider>(this, "initSelf", InitPriority.Normal);
    }

    static AnimationStatesProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AnimationStatesProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((AnimationStatesProvider) obj).SerializeData(writer));
      AnimationStatesProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((AnimationStatesProvider) obj).DeserializeData(reader));
    }
  }
}
