// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.IBattleSimulator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;

#nullable disable
namespace Mafi.Core.Fleet
{
  public interface IBattleSimulator
  {
    /// <summary>Ongoing battle.</summary>
    Option<BattleState> OngoingBattle { get; }

    /// <summary>
    /// Starts a battle. This can be done only when no other battles are in progress.
    /// </summary>
    Option<IBattleState> StartBattle(BattleFleet attacker, BattleFleet defender);

    /// <summary>History of all battle results in the current game.</summary>
    IIndexable<PlayersBattleResult> AllBattleResults { get; }
  }
}
