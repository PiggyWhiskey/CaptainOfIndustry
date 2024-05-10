// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.CreateReason
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Products
{
  public enum CreateReason
  {
    InitialResource,
    /// <summary>Player imported this from the world map.</summary>
    Imported,
    MinedFromTerrain,
    /// <summary>
    /// Produced in factory or by some other means of direct production.
    /// </summary>
    Produced,
    General,
    /// <summary>Cheating!</summary>
    Cheated,
    /// <summary>Products acquired via QuickTrade.</summary>
    QuickTrade,
    /// <summary>Loot from beacon or world map.</summary>
    Loot,
    Deconstruction,
    Settlement,
    Recycled,
    Research,
    Loan,
  }
}
