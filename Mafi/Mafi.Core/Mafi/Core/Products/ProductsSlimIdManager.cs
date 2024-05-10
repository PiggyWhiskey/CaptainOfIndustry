// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.ProductsSlimIdManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class ProductsSlimIdManager : SlimIdManagerBase<ProductProto, ProductSlimId>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ProductsSlimIdManager(ProtosDb db)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(db);
    }

    public override ProductProto PhantomProto => ProductProto.Phantom;

    public override int MaxIdValue => (int) byte.MaxValue;

    protected override ProductSlimId CreateSlimId(int index) => new ProductSlimId((byte) index);

    protected override int GetIndex(ProductSlimId slimId) => (int) slimId.Value;

    public static void Serialize(ProductsSlimIdManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ProductsSlimIdManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ProductsSlimIdManager.s_serializeDataDelayedAction);
    }

    public static ProductsSlimIdManager Deserialize(BlobReader reader)
    {
      ProductsSlimIdManager productsSlimIdManager;
      if (reader.TryStartClassDeserialization<ProductsSlimIdManager>(out productsSlimIdManager))
        reader.EnqueueDataDeserialization((object) productsSlimIdManager, ProductsSlimIdManager.s_deserializeDataDelayedAction);
      return productsSlimIdManager;
    }

    static ProductsSlimIdManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProductsSlimIdManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SlimIdManagerBase<ProductProto, ProductSlimId>) obj).SerializeData(writer));
      ProductsSlimIdManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SlimIdManagerBase<ProductProto, ProductSlimId>) obj).DeserializeData(reader));
    }
  }
}
