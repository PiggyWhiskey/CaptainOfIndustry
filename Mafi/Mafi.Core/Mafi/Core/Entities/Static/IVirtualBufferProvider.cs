// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IVirtualBufferProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [MultiDependency]
  public interface IVirtualBufferProvider
  {
    ImmutableArray<ProductProto> ProvidedProducts { get; }

    /// <summary>Returns a buffer for requested product.</summary>
    /// <param name="product">Requested product, should be one of <see cref="P:Mafi.Core.Entities.Static.IVirtualBufferProvider.ProvidedProducts" />.</param>
    /// <param name="entity">Entity that requested the buffer.</param>
    Option<IProductBuffer> GetBuffer(ProductProto product, Option<IEntity> entity);
  }
}
