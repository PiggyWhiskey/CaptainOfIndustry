// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RuinedBuildings.Ruins
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Game;
using Mafi.Core.Products;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.RuinedBuildings
{
  [GenerateSerializer(false, null, 0)]
  public class Ruins : 
    LayoutEntity,
    IOutputBufferPriorityProvider,
    IEntityWithSimUpdate,
    IEntity,
    IIsSafeAsHashKey
  {
    private static readonly Quantity LOGISTICS_BUFFER_SIZE;
    private static readonly Duration SCRAP_INITIAL_DURATION;
    public readonly RuinsProto Prototype;
    private readonly IProductsManager m_productsManager;
    private readonly Dict<ProductProto, ProductBuffer> m_innerBuffers;
    private readonly Dict<ProductProto, Ruins.ForLogisticsProductBuffer> m_logisticsBuffers;
    private readonly Mafi.Core.Entities.Static.ConstructionProgress m_scrapProgress;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => false;

    public bool IsScrapping => !this.m_scrapProgress.IsPaused;

    public IConstructionProgress ScrapProgress => (IConstructionProgress) this.m_scrapProgress;

    public Ruins(
      EntityId id,
      RuinsProto proto,
      TileTransform transform,
      EntityContext context,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IProductsManager productsManager,
      GameDifficultyConfig difficultyConfig)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_innerBuffers = new Dict<ProductProto, ProductBuffer>();
      this.m_logisticsBuffers = new Dict<ProductProto, Ruins.ForLogisticsProductBuffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      Assert.That<PartialQuantity>(proto.Costs.Maintenance.MaintenancePerMonth).IsZero();
      this.Prototype = proto;
      this.m_productsManager = productsManager;
      foreach (ProductQuantity product in this.Prototype.ProductsGiven.Products)
      {
        Quantity quantity = product.Quantity.ScaledBy(difficultyConfig.ExtraStartingMaterialMult);
        ProductBuffer buffer = new ProductBuffer(quantity, product.Product);
        buffer.StoreExactly(quantity);
        this.m_innerBuffers.Add(product.Product, buffer);
      }
      foreach (ProductQuantity product in this.Prototype.ProductsGiven.Products)
      {
        Ruins.ForLogisticsProductBuffer buffer = new Ruins.ForLogisticsProductBuffer(Ruins.LOGISTICS_BUFFER_SIZE, product.Product, this);
        vehicleBuffersRegistry.RegisterOutputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IOutputBufferPriorityProvider) this, true);
        this.m_logisticsBuffers.Add(product.Product, buffer);
      }
      this.m_scrapProgress = new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, this.m_innerBuffers.Values.ToImmutableArray<ProductBuffer>(), this.Prototype.ProductsGiven, this.Prototype.DurationPerProduct, Ruins.SCRAP_INITIAL_DURATION, true);
      this.m_scrapProgress.IsPaused = false;
    }

    private void onQuantityChanged(ProductBuffer productBuffer, Quantity quantity)
    {
      ProductBuffer buffer;
      if (!quantity.IsNegative || !this.m_innerBuffers.TryGetValue(productBuffer.Product, out buffer))
        return;
      buffer.RemoveExactly(-quantity);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.m_scrapProgress.IsPaused)
        return;
      if (this.m_scrapProgress.IsDone)
      {
        Assert.That<bool>(this.m_logisticsBuffers.Values.All<Ruins.ForLogisticsProductBuffer>((Func<Ruins.ForLogisticsProductBuffer, bool>) (x => x.IsEmpty))).IsTrue();
        Assert.That<bool>(this.m_innerBuffers.Values.All<ProductBuffer>((Func<ProductBuffer, bool>) (x => x.IsEmpty))).IsTrue();
        this.Context.EntitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) this, EntityRemoveReason.Remove);
      }
      else
      {
        if (this.m_scrapProgress.TryMakeStep())
          return;
        foreach (ProductBuffer productBuffer in this.m_innerBuffers.Values)
        {
          ProductBuffer logisticsBuffer = (ProductBuffer) this.m_logisticsBuffers[productBuffer.Product];
          Quantity quantity1 = (productBuffer.Quantity - logisticsBuffer.Quantity).Min(logisticsBuffer.UsableCapacity);
          Quantity quantity2 = logisticsBuffer.StoreAsMuchAsReturnStored(quantity1);
          if (quantity2.IsPositive)
            this.m_productsManager.ProductCreated(productBuffer.Product, quantity2, CreateReason.Deconstruction);
        }
      }
    }

    public void SetScrapEnabled(bool enabled) => this.m_scrapProgress.IsPaused = !enabled;

    public override EntityValidationResult CanStartDeconstruction()
    {
      return EntityValidationResult.CreateError((LocStrFormatted) TrCore.RemovalError__ScrapItFirst);
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      ProductBuffer productBuffer;
      if (this.m_innerBuffers.TryGetValue(request.Buffer.Product, out productBuffer))
      {
        if (productBuffer.Quantity < TruckCaps.SmallTruckCapacity)
          return BufferStrategy.FullFillAtAnyCost(7);
      }
      else
        Log.Error(string.Format("Buffer not found for {0}", (object) request.Buffer.Product.Id));
      return new BufferStrategy(14, new Quantity?(TruckCaps.SmallTruckCapacity));
    }

    protected override void OnDestroy()
    {
      foreach (IProductBuffer buffer in this.m_logisticsBuffers.Values)
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer(buffer);
      base.OnDestroy();
    }

    public static void Serialize(Ruins value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Ruins>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Ruins.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Dict<ProductProto, ProductBuffer>.Serialize(this.m_innerBuffers, writer);
      Dict<ProductProto, Ruins.ForLogisticsProductBuffer>.Serialize(this.m_logisticsBuffers, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Mafi.Core.Entities.Static.ConstructionProgress.Serialize(this.m_scrapProgress, writer);
      writer.WriteGeneric<RuinsProto>(this.Prototype);
    }

    public static Ruins Deserialize(BlobReader reader)
    {
      Ruins ruins;
      if (reader.TryStartClassDeserialization<Ruins>(out ruins))
        reader.EnqueueDataDeserialization((object) ruins, Ruins.s_deserializeDataDelayedAction);
      return ruins;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<Ruins>(this, "m_innerBuffers", (object) Dict<ProductProto, ProductBuffer>.Deserialize(reader));
      reader.SetField<Ruins>(this, "m_logisticsBuffers", (object) Dict<ProductProto, Ruins.ForLogisticsProductBuffer>.Deserialize(reader));
      reader.SetField<Ruins>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<Ruins>(this, "m_scrapProgress", (object) Mafi.Core.Entities.Static.ConstructionProgress.Deserialize(reader));
      reader.SetField<Ruins>(this, "Prototype", (object) reader.ReadGenericAs<RuinsProto>());
    }

    static Ruins()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Ruins.LOGISTICS_BUFFER_SIZE = 3 * TruckCaps.SmallTruckCapacity;
      Ruins.SCRAP_INITIAL_DURATION = 10.Seconds();
      Ruins.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Ruins.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class ForLogisticsProductBuffer : GlobalLogisticsOutputBuffer
    {
      private readonly Ruins m_ruins;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public ForLogisticsProductBuffer(Quantity capacity, ProductProto product, Ruins ruins)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product, ruins.Context.ProductsManager, 15, (IEntity) ruins);
        this.m_ruins = ruins;
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        base.OnQuantityChanged(diff);
        this.m_ruins.onQuantityChanged((ProductBuffer) this, diff);
      }

      public static void Serialize(Ruins.ForLogisticsProductBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Ruins.ForLogisticsProductBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Ruins.ForLogisticsProductBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        Ruins.Serialize(this.m_ruins, writer);
      }

      public static Ruins.ForLogisticsProductBuffer Deserialize(BlobReader reader)
      {
        Ruins.ForLogisticsProductBuffer logisticsProductBuffer;
        if (reader.TryStartClassDeserialization<Ruins.ForLogisticsProductBuffer>(out logisticsProductBuffer))
          reader.EnqueueDataDeserialization((object) logisticsProductBuffer, Ruins.ForLogisticsProductBuffer.s_deserializeDataDelayedAction);
        return logisticsProductBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<Ruins.ForLogisticsProductBuffer>(this, "m_ruins", (object) Ruins.Deserialize(reader));
      }

      static ForLogisticsProductBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Ruins.ForLogisticsProductBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        Ruins.ForLogisticsProductBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
