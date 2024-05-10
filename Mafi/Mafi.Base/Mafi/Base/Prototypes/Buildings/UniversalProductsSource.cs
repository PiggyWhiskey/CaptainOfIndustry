// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.UniversalProductsSource
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  [GenerateSerializer(false, null, 0)]
  public class UniversalProductsSource : 
    LayoutEntity,
    IEntityWithSimUpdate,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IProductsManager m_productsManager;

    public static void Serialize(UniversalProductsSource value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UniversalProductsSource>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UniversalProductsSource.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Quantity.Serialize(this.MaxProvidedPerTick, writer);
      Quantity.Serialize(this.ProvidedLastTick, writer);
      Option<ProductProto>.Serialize(this.ProvidedProduct, writer);
    }

    public static UniversalProductsSource Deserialize(BlobReader reader)
    {
      UniversalProductsSource universalProductsSource;
      if (reader.TryStartClassDeserialization<UniversalProductsSource>(out universalProductsSource))
        reader.EnqueueDataDeserialization((object) universalProductsSource, UniversalProductsSource.s_deserializeDataDelayedAction);
      return universalProductsSource;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<UniversalProductsSource>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.MaxProvidedPerTick = Quantity.Deserialize(reader);
      this.ProvidedLastTick = Quantity.Deserialize(reader);
      this.ProvidedProduct = Option<ProductProto>.Deserialize(reader);
    }

    public override bool CanBePaused => true;

    public Option<ProductProto> ProvidedProduct { get; private set; }

    public Quantity MaxProvidedPerTick { get; private set; }

    public Quantity ProvidedLastTick { get; private set; }

    public UniversalProductsSource(
      EntityId id,
      UniversalProductsSourceProto proto,
      TileTransform transform,
      EntityContext context,
      ProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxProvidedPerTick\u003Ek__BackingField = 10.Quantity();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_productsManager = (IProductsManager) productsManager;
    }

    public void SetProvidedProduct(Option<ProductProto> product) => this.ProvidedProduct = product;

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.ProvidedLastTick = Quantity.Zero;
      if (this.IsNotEnabled || this.ProvidedProduct.IsNone)
        return;
      foreach (IoPortData connectedOutputPort in this.ConnectedOutputPorts)
      {
        if (connectedOutputPort.AllowedProductType == this.ProvidedProduct.Value.Type)
          this.ProvidedLastTick += this.MaxProvidedPerTick - connectedOutputPort.SendAsMuchAs(this.MaxProvidedPerTick.Of(this.ProvidedProduct.Value));
      }
      this.m_productsManager.ProductCreated(this.ProvidedProduct.Value, this.ProvidedLastTick, CreateReason.Cheated);
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      Assert.Fail("Invalid operation.");
      return pq.Quantity;
    }

    static UniversalProductsSource()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      UniversalProductsSource.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      UniversalProductsSource.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
