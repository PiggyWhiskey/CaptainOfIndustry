// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.IFuelStatsCollector
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Stats
{
  public interface IFuelStatsCollector
  {
    /// <summary>
    /// Reports the product to statistics and destroys it in product manager as used fuel.
    /// </summary>
    void ReportFuelUseAndDestroy(ProductProto product, Quantity quantity, FuelUsedBy reason);
  }
}
