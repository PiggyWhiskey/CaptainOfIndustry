// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.DestroyReason
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Products
{
  public enum DestroyReason
  {
    /// <summary>From clearing of entity.</summary>
    Cleared,
    DumpedOnTerrain,
    General,
    UsedAsFuel,
    /// <summary>
    /// For instance electricity that couldn't be consumed. No money given.
    /// </summary>
    Wasted,
    /// <summary>Products sold via QuickTrade.</summary>
    QuickTrade,
    /// <summary>Products exported from the island.</summary>
    Export,
    Construction,
    /// <summary>Used as input in maintenance.</summary>
    Maintenance,
    Settlement,
    Research,
    Farms,
    Cheated,
    LoanPayment,
  }
}
