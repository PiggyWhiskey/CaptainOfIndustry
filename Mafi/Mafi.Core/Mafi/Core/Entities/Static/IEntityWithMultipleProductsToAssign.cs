// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IEntityWithMultipleProductsToAssign
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public interface IEntityWithMultipleProductsToAssign : 
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    void SetProduct(Option<ProductProto> product, int bufferSlot, bool skipIfClearing);

    IIndexable<Option<ProductBuffer>> BuffersPerSlot { get; }

    Option<IProductBuffer> GetBuffer(int bufferSlot);

    Quantity GetCapacity(int bufferSlot);

    ImmutableArray<ProductProto> SupportedProducts { get; }
  }
}
