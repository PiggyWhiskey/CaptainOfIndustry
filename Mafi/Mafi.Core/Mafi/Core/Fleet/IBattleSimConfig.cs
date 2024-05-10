// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.IBattleSimConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Fleet
{
  public interface IBattleSimConfig
  {
    /// <summary>
    /// All defender's entities get extra battle priority. This gives a slight advantage to the defender.
    /// </summary>
    int DefenderExtraBattlePriority { get; }

    /// <summary>
    /// Maximum number of battle rounds. This prevents infinite battles in case of some bug. Note that one battle
    /// steps is one sim step.
    /// </summary>
    int MaxBattleRounds { get; }

    int PossibleEscapeDistance { get; }

    /// <summary>
    /// Extra distance added between fleets before the battle begins.
    /// </summary>
    int StartingExtraFleetDistance { get; }

    Percent ShipEscapeHpThreshold { get; }

    int BaseRoundsToEscape { get; }

    Percent ChanceForSameEntityRepeatedFire { get; }

    Percent ExtraMissChanceWhenEscaping { get; }

    Percent MaxArmorReduction { get; }

    Percent RecoverableHpMultiplier { get; }

    Percent HullDamageMultWhenPartIsHit { get; }
  }
}
