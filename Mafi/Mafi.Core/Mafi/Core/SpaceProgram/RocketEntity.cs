// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.RocketEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.SpaceProgram
{
  [GenerateSerializer(false, null, 0)]
  public class RocketEntity : 
    RocketEntityBase,
    IEntityWithSound,
    IEntityWithSimUpdate,
    IEntity,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly RelTile1f MAX_ALTITUDE;
    private readonly ProductBuffer m_fuelBuffer;
    private int m_altitudeCheckIndex;
    private readonly RocketLaunchManager m_rocketLaunchManager;
    private readonly IProductsManager m_productsManager;
    private readonly IEntitiesManager m_entitiesManager;

    public static void Serialize(RocketEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RocketEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RocketEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RelTile1f.Serialize(this.Acceleration, writer);
      RelTile1f.Serialize(this.GainedAltitude, writer);
      writer.WriteNullableStruct<bool>(this.IsExploded);
      Duration.Serialize(this.LaunchedFor, writer);
      writer.WriteInt(this.m_altitudeCheckIndex);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      ProductBuffer.Serialize(this.m_fuelBuffer, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      RocketLaunchManager.Serialize(this.m_rocketLaunchManager, writer);
      Tile3f.Serialize(this.Position, writer);
      writer.WriteGeneric<RocketProto>(this.Prototype);
    }

    public static RocketEntity Deserialize(BlobReader reader)
    {
      RocketEntity rocketEntity;
      if (reader.TryStartClassDeserialization<RocketEntity>(out rocketEntity))
        reader.EnqueueDataDeserialization((object) rocketEntity, RocketEntity.s_deserializeDataDelayedAction);
      return rocketEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Acceleration = RelTile1f.Deserialize(reader);
      this.GainedAltitude = RelTile1f.Deserialize(reader);
      this.IsExploded = reader.ReadNullableStruct<bool>();
      this.LaunchedFor = Duration.Deserialize(reader);
      this.m_altitudeCheckIndex = reader.ReadInt();
      reader.SetField<RocketEntity>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<RocketEntity>(this, "m_fuelBuffer", (object) ProductBuffer.Deserialize(reader));
      reader.SetField<RocketEntity>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<RocketEntity>(this, "m_rocketLaunchManager", (object) RocketLaunchManager.Deserialize(reader));
      this.Position = Tile3f.Deserialize(reader);
      this.Prototype = reader.ReadGenericAs<RocketProto>();
    }

    public RocketProto Prototype { get; private set; }

    public override bool CanBePaused => false;

    /// <summary>This is only valid when not attached.</summary>
    public Tile3f Position { get; private set; }

    public IProductBufferReadOnly FuelBuffer => (IProductBufferReadOnly) this.m_fuelBuffer;

    public Duration LaunchedFor { get; private set; }

    public RelTile1f Acceleration { get; private set; }

    public RelTile1f GainedAltitude { get; private set; }

    public bool IsLaunched => this.LaunchedFor.IsPositive;

    public bool? IsExploded { get; private set; }

    public bool IsSoundOn => this.IsLaunched;

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.Prototype.Graphics.SoundPrefabPath, SoundSignificance.High, fadeOnChange: false, doNotLimit: true));
      }
    }

    public RocketEntity(
      EntityId id,
      RocketProto prototype,
      EntityContext context,
      RocketLaunchManager rocketLaunchManager,
      IProductsManager productsManager,
      IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (EntityProto) prototype, context);
      this.m_rocketLaunchManager = rocketLaunchManager;
      this.m_productsManager = productsManager;
      this.m_entitiesManager = entitiesManager;
      this.Prototype = prototype;
      this.m_fuelBuffer = new ProductBuffer(prototype.LaunchFuel.Quantity, prototype.LaunchFuel.Product);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (!this.IsLaunched)
        return;
      this.LaunchedFor += Duration.OneTick;
      if (this.GainedAltitude < RocketEntity.MAX_ALTITUDE)
      {
        this.Acceleration += this.Prototype.AccelerationPerTick;
        this.GainedAltitude += this.Acceleration;
        this.Position = this.Position.AddZ(this.Acceleration.Value);
      }
      if (!(this.LaunchedFor >= this.Prototype.TotalFlightTime))
        return;
      this.m_productsManager.ProductDestroyed(this.m_fuelBuffer.Product, this.m_fuelBuffer.Quantity, DestroyReason.UsedAsFuel);
      this.m_fuelBuffer.Clear();
      this.IsExploded = new bool?(false);
      this.m_rocketLaunchManager.ReportLaunchDone(this);
      this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) this, EntityRemoveReason.Remove);
    }

    public bool CanLaunch() => this.m_fuelBuffer.IsFull() && !this.IsLaunched;

    public bool TryDetachAndLaunch(Tile3f launchPosition)
    {
      if (!this.CanLaunch())
        return false;
      if (this.Owner.HasValue)
      {
        Assert.That<Option<RocketEntityBase>>(this.Owner.Value.DetachRocket()).IsEqualTo<RocketEntityBase>((RocketEntityBase) this);
        Assert.That<Option<IRocketOwner>>(this.Owner).IsNone<IRocketOwner>("Owner did not detached itself from the rocket. Forgot to call `SetOwner`?");
      }
      this.Position = launchPosition;
      this.LaunchedFor = Duration.OneTick;
      this.Acceleration = RelTile1f.Zero;
      this.m_altitudeCheckIndex = 0;
      this.m_rocketLaunchManager.ReportRocketLaunched(this);
      return true;
    }

    public Quantity LoadAsMuchAs(ProductQuantity pq)
    {
      return (Proto) pq.Product == (Proto) this.m_fuelBuffer.Product ? this.m_fuelBuffer.StoreAsMuchAs(pq) : pq.Quantity;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      Assert.That<bool>(this.m_fuelBuffer.IsEmpty()).IsTrue("Destroying rocket with some products left in fuel buffer.");
    }

    static RocketEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RocketEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RocketEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      RocketEntity.MAX_ALTITUDE = RelTile1f.MaxValue / 10;
    }
  }
}
