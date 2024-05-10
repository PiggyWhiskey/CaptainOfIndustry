// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.ITransportFriend
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  internal interface ITransportFriend
  {
    Queueue<TransportedProductMutable> TransportedProductsMutable { get; }

    /// <summary>
    /// Inserts products on the transport. Ignores any spacing or quantity constraints.
    /// </summary>
    void InsertNewProduct(ProductSlimId product, Quantity quantity, int waypointIndex);
  }
}
