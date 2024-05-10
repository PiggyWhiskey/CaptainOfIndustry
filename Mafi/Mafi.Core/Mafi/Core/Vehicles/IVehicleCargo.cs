// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.IVehicleCargo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Products;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public interface IVehicleCargo
  {
    bool IsEmpty { get; }

    bool IsNotEmpty { get; }

    Quantity TotalQuantity { get; }

    ProductQuantity FirstOrPhantom { get; }

    int Count { get; }

    void GetCargoProducts(Lyst<ProductQuantity> cacheToPopulate);

    Lyst<KeyValuePair<ProductProto, Quantity>>.Enumerator GetEnumerator();

    /// <summary>
    /// Gets the quantity of the given product. Returns 0 if not found.
    /// </summary>
    Quantity GetQuantityOf(ProductProto product);

    bool CanAdd(ProductProto product);

    bool HasProduct(ProductProto product);
  }
}
