// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.FlyWheelEntity
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.MechanicalPower;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class FlyWheelEntity : 
    LayoutEntity,
    IAnimatedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithSimUpdate,
    IEntityWithSound,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private FlyWheelEntityProto m_proto;
    [DoNotSave(0, null)]
    private Percent m_lastInertia;
    private readonly IShaftManager m_shaftManager;

    public static void Serialize(FlyWheelEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FlyWheelEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FlyWheelEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteGeneric<FlyWheelEntityProto>(this.m_proto);
      writer.WriteGeneric<IShaftManager>(this.m_shaftManager);
    }

    public static FlyWheelEntity Deserialize(BlobReader reader)
    {
      FlyWheelEntity flyWheelEntity;
      if (reader.TryStartClassDeserialization<FlyWheelEntity>(out flyWheelEntity))
        reader.EnqueueDataDeserialization((object) flyWheelEntity, FlyWheelEntity.s_deserializeDataDelayedAction);
      return flyWheelEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.m_proto = reader.ReadGenericAs<FlyWheelEntityProto>();
      reader.SetField<FlyWheelEntity>(this, "m_shaftManager", (object) reader.ReadGenericAs<IShaftManager>());
    }

    [DoNotSave(0, null)]
    public FlyWheelEntityProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => false;

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return !this.m_proto.Graphics.SoundPrefabPath.HasValue ? new Mafi.Core.Entities.SoundParams?() : new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.m_proto.Graphics.SoundPrefabPath.Value, SoundSignificance.Normal));
      }
    }

    public bool IsSoundOn => this.IsEnabled && this.m_lastInertia.IsPositive;

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.m_proto.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public FlyWheelEntity(
      EntityId id,
      FlyWheelEntityProto proto,
      TileTransform transform,
      EntityContext context,
      IAnimationStateFactory animationStateFactory,
      IShaftManager shaftManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_shaftManager = shaftManager;
      this.Prototype = proto;
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.IsNotEnabled)
      {
        this.AnimationStatesProvider.Step(Percent.Zero, Percent.Zero);
      }
      else
      {
        this.m_lastInertia = this.m_shaftManager.GetCurrentShaftFor((IStaticEntity) this).CurrentInertia;
        this.AnimationStatesProvider.Step(this.m_lastInertia, Percent.Zero);
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      Log.Warning("Flywheel should not receive any products.");
      return pq.Quantity;
    }

    static FlyWheelEntity()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      FlyWheelEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      FlyWheelEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
