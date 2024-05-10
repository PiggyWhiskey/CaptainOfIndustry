// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.Stacker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GenerateSerializer(false, null, 0)]
  public class Stacker : 
    LayoutEntity,
    IEntityWithOutputToTerrain,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IEntityWithCloneableConfig,
    IElectricityConsumingEntity,
    IEntityWithGeneralPriority,
    IEntityWithSimUpdate,
    IEntityWithParticles,
    IEntityWithPorts
  {
    private static readonly Duration STACKER_ACTIVE_TIMEOUT;
    private readonly IProductsManager m_productsManager;
    private readonly TerrainManager m_terrainManager;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly IElectricityConsumer m_electricityConsumer;
    private StackerProto m_proto;
    private ProductQuantity m_waitingForEnqueue;
    private Queueue<Pair<LooseProductProto, SimStep>> m_productsToDump;
    private SimStep m_operatedSteps;
    private SimStep m_lastDumpStep;
    [DoNotSave(0, null)]
    private Tile2iAndIndex m_dumpTileAndIndex;
    private Option<TerrainMaterialProto> m_lastDumpedMaterial;
    private bool m_isDumpingActive;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ThicknessTilesI DumpHeightOffset { get; private set; }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    [DoNotSave(0, null)]
    public StackerProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => true;

    public Electricity PowerRequired => this.m_proto.ElectricityConsumed;

    public bool AreParticlesEnabled => true;

    /// <summary>Location from which dumped material emerges.</summary>
    public Tile2i DumpPositionXy => this.m_dumpTileAndIndex.TileCoord;

    /// <summary>The last material that was dumped.</summary>
    public Option<TerrainMaterialProto> LastDumpedMaterial => this.m_lastDumpedMaterial;

    /// <summary>Whether the stacker is actively dumping.</summary>
    public bool IsDumpingActive => this.m_isDumpingActive;

    public Stacker(
      EntityId id,
      StackerProto proto,
      TileTransform transform,
      EntityContext context,
      IProductsManager productsManager,
      TerrainManager terrainManager,
      ISimLoopEvents simLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_waitingForEnqueue = ProductQuantity.None;
      this.m_productsToDump = new Queueue<Pair<LooseProductProto, SimStep>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto.CheckNotNull<StackerProto>();
      this.m_productsManager = productsManager;
      this.m_terrainManager = terrainManager;
      this.m_simLoopEvents = simLoopEvents;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.DumpHeightOffset = this.Prototype.DefaultDumpOffset;
      this.initSelf();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_dumpTileAndIndex = this.m_terrainManager.ExtendTileIndex(this.CenterTile.Xy + this.Transform.TransformMatrix.Transform(this.m_proto.DumpHeadRelPos.Xy));
    }

    public void SimUpdate()
    {
      if (!this.IsEnabled || !this.m_electricityConsumer.CanConsume())
      {
        this.m_isDumpingActive = false;
      }
      else
      {
        if (this.m_waitingForEnqueue.Quantity.IsPositive && (this.m_productsToDump.IsEmpty || this.m_operatedSteps - this.m_productsToDump.Last.Second >= this.m_proto.DumpPeriod))
        {
          this.m_productsToDump.Enqueue(Pair.Create<LooseProductProto, SimStep>(this.m_waitingForEnqueue.Product.DumpableProduct.Value, this.m_operatedSteps));
          this.m_waitingForEnqueue = new ProductQuantity(this.m_waitingForEnqueue.Product, this.m_waitingForEnqueue.Quantity - Quantity.One);
          Assert.That<int>(this.m_productsToDump.Count).IsLessOrEqual(this.m_proto.MaxProductsInQueue, "Stacker has unexpectedly high products in queue");
        }
        else if (this.m_productsToDump.IsEmpty)
        {
          this.m_isDumpingActive = this.m_simLoopEvents.CurrentStep - this.m_lastDumpStep <= Stacker.STACKER_ACTIVE_TIMEOUT;
          return;
        }
        Pair<LooseProductProto, SimStep> first = this.m_productsToDump.First;
        if (this.m_operatedSteps - first.Second < this.m_proto.DumpDelay)
        {
          if (this.m_electricityConsumer.TryConsume())
          {
            this.m_operatedSteps += SimStep.One;
            this.m_isDumpingActive = true;
            this.m_lastDumpedMaterial = first.First.TerrainMaterial;
          }
          else
            this.m_isDumpingActive = false;
        }
        else
        {
          HeightTilesF heightTilesF = (new HeightTilesI(this.CenterTile.Z + this.m_proto.DumpHeadRelPos.Z) - this.DumpHeightOffset).HeightTilesF;
          Tile2iAndIndex tileAndIndex = new Tile2iAndIndex();
          foreach (Tile2iAndIndexRel tileCornersDelta in this.m_terrainManager.FourTileCornersDeltas)
          {
            Tile2iAndIndex tile2iAndIndex = this.m_dumpTileAndIndex + tileCornersDelta;
            HeightTilesF height = this.m_terrainManager.GetHeight(tile2iAndIndex.Index);
            if (height < heightTilesF)
            {
              heightTilesF = height;
              tileAndIndex = tile2iAndIndex;
            }
          }
          if (tileAndIndex == new Tile2iAndIndex())
            this.m_isDumpingActive = this.m_simLoopEvents.CurrentStep - this.m_lastDumpStep <= Stacker.STACKER_ACTIVE_TIMEOUT;
          else if (!this.m_electricityConsumer.TryConsume())
          {
            this.m_isDumpingActive = false;
          }
          else
          {
            this.m_operatedSteps += SimStep.One;
            this.m_isDumpingActive = true;
            this.m_lastDumpStep = this.m_simLoopEvents.CurrentStep;
            this.m_productsManager.ProductDestroyed(new ProductQuantity((ProductProto) first.First, Quantity.One), DestroyReason.DumpedOnTerrain);
            this.m_productsToDump.Dequeue();
            LooseProductProto valueOrNull = first.First.DumpableProduct.ValueOrNull;
            if ((Proto) valueOrNull == (Proto) null)
            {
              Log.Warning(string.Format("Trying to dump non-dumpable material '{0}'.", (object) first.First));
            }
            else
            {
              ThicknessTilesF thickness = valueOrNull.TerrainMaterial.Value.QuantityToThickness(Quantity.One);
              this.m_terrainManager.DumpMaterial(tileAndIndex, new TerrainMaterialThicknessSlim(valueOrNull.TerrainMaterial.Value.SlimId, thickness));
              this.m_lastDumpedMaterial = valueOrNull.TerrainMaterial;
            }
          }
        }
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || pq.Quantity.IsNotPositive || pq.Product.DumpableProduct.IsNone || this.m_waitingForEnqueue.IsNotEmpty)
        return pq.Quantity;
      this.m_waitingForEnqueue = pq;
      return Quantity.Zero;
    }

    public void SetDumpHeightOffset(ThicknessTilesI offset)
    {
      if (offset < this.Prototype.MinDumpOffset)
      {
        Log.Warning(string.Format("Dumping offset {0} is too low, minimum is {1}.", (object) offset, (object) this.Prototype.MinDumpOffset));
        offset = this.Prototype.MinDumpOffset;
      }
      this.DumpHeightOffset = offset;
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetDumpHeightOffset(this.DumpHeightOffset);
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      this.SetDumpHeightOffset(data.GetDumpHeightOffset() ?? this.DumpHeightOffset);
    }

    protected override void OnDestroy()
    {
      foreach (Pair<LooseProductProto, SimStep> pair in this.m_productsToDump)
        this.Context.AssetTransactionManager.StoreClearedProduct(new ProductQuantity((ProductProto) pair.First, Quantity.One));
      base.OnDestroy();
    }

    public static void Serialize(Stacker value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Stacker>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Stacker.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ThicknessTilesI.Serialize(this.DumpHeightOffset, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      writer.WriteBool(this.m_isDumpingActive);
      Option<TerrainMaterialProto>.Serialize(this.m_lastDumpedMaterial, writer);
      SimStep.Serialize(this.m_lastDumpStep, writer);
      SimStep.Serialize(this.m_operatedSteps, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Queueue<Pair<LooseProductProto, SimStep>>.Serialize(this.m_productsToDump, writer);
      writer.WriteGeneric<StackerProto>(this.m_proto);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      ProductQuantity.Serialize(this.m_waitingForEnqueue, writer);
    }

    public static Stacker Deserialize(BlobReader reader)
    {
      Stacker stacker;
      if (reader.TryStartClassDeserialization<Stacker>(out stacker))
        reader.EnqueueDataDeserialization((object) stacker, Stacker.s_deserializeDataDelayedAction);
      return stacker;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.DumpHeightOffset = ThicknessTilesI.Deserialize(reader);
      reader.SetField<Stacker>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_isDumpingActive = reader.ReadBool();
      this.m_lastDumpedMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
      this.m_lastDumpStep = SimStep.Deserialize(reader);
      this.m_operatedSteps = SimStep.Deserialize(reader);
      reader.SetField<Stacker>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_productsToDump = Queueue<Pair<LooseProductProto, SimStep>>.Deserialize(reader);
      this.m_proto = reader.ReadGenericAs<StackerProto>();
      reader.SetField<Stacker>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<Stacker>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      this.m_waitingForEnqueue = ProductQuantity.Deserialize(reader);
      reader.RegisterInitAfterLoad<Stacker>(this, "initSelf", InitPriority.Normal);
    }

    static Stacker()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Stacker.STACKER_ACTIVE_TIMEOUT = 2.Seconds();
      Stacker.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Stacker.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
