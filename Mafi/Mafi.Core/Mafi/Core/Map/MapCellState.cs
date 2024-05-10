// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapCellState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Map
{
  public enum MapCellState
  {
    /// <summary>
    /// Cell is not available to unlock. This may be when the cell has no neighbors that are unlocked.
    /// </summary>
    NotAvailable,
    /// <summary>
    /// Terrain under cell is being generated and it will be available to unlock one it finishes.
    /// </summary>
    PendingAvailableToUnlock,
    /// <summary>Cell can be unlocked by the player.</summary>
    AvailableToUnlock,
    /// <summary>
    /// Cell is being unlocked. This involves setting all tiles to unlocked state.
    /// </summary>
    PendingUnlocked,
    /// <summary>Cell is unlocked. The player can build/mine on it.</summary>
    Unlocked,
  }
}
