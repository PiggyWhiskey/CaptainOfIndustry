// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.QuickTrade.IVirtualProductQuickTradeHandler
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.World.QuickTrade
{
  [MultiDependency]
  public interface IVirtualProductQuickTradeHandler
  {
    LocStrFormatted MessageOnDelivery { get; }

    LocStrFormatted DescriptionOfTrade { get; }

    bool IsProductManaged(ProductProto product);

    bool CanBuy(ProductProto product, Quantity quantityToBuy, out LocStrFormatted error);

    void StoreBoughtProduct(ProductProto product, Quantity quantityToBuy);
  }
}
