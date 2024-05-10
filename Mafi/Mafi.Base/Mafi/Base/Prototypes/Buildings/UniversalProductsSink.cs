// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.UniversalProductsSink
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
  public class UniversalProductsSink : 
    LayoutEntity,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly ProductsManager m_productsManager;

    public static void Serialize(UniversalProductsSink value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UniversalProductsSink>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UniversalProductsSink.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductsManager.Serialize(this.m_productsManager, writer);
    }

    public static UniversalProductsSink Deserialize(BlobReader reader)
    {
      UniversalProductsSink universalProductsSink;
      if (reader.TryStartClassDeserialization<UniversalProductsSink>(out universalProductsSink))
        reader.EnqueueDataDeserialization((object) universalProductsSink, UniversalProductsSink.s_deserializeDataDelayedAction);
      return universalProductsSink;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<UniversalProductsSink>(this, "m_productsManager", (object) ProductsManager.Deserialize(reader));
    }

    public override bool CanBePaused => true;

    public UniversalProductsSink(
      EntityId id,
      UniversalProductsSinkProto proto,
      TileTransform transform,
      EntityContext context,
      ProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_productsManager = productsManager;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled)
        return pq.Quantity;
      this.m_productsManager.ProductDestroyed(pq, DestroyReason.Cheated);
      return Quantity.Zero;
    }

    static UniversalProductsSink()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      UniversalProductsSink.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      UniversalProductsSink.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
