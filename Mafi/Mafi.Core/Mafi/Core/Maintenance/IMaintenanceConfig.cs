// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.IMaintenanceConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Maintenance
{
  public interface IMaintenanceConfig
  {
    /// <summary>
    /// Maintenance buffer max capacity in terms on in-game duration.
    /// </summary>
    Duration BufferMaxCapacity { get; }

    /// <summary>
    /// A threshold of maintenance buffer fill when reliability starts decreasing, to not punish for
    /// short-term reliability outages.
    /// </summary>
    Percent ReliabilityIssuesStartAt { get; }

    /// <summary>
    /// Maximum breakdown chance when maintenance buffer reaches zero.
    /// </summary>
    Percent MaxBreakdownChance { get; }

    /// <summary>
    /// Speed of maintenance replenishment compared to regular drain. This is to limit abrupt maintenance
    /// consumption of a few machines that get are processed first during shortages. Should be larger than 100%.
    /// </summary>
    Percent MaxReplenishSpeed { get; }

    /// <summary>Maintenance reduction for entities that are idle.</summary>
    Percent IdleMaintenanceMultiplier { get; }

    /// <summary>
    /// Maintenance replenish amount in addition to <see cref="P:Mafi.Core.Maintenance.IMaintenanceConfig.MaxReplenishSpeed" />. This will make
    /// entities with smaller buffer replenish faster.
    /// </summary>
    PartialQuantity BaseReplenishPerMonth { get; }

    /// <summary>
    /// Minimal broken duration (interpolated with <see cref="P:Mafi.Core.Maintenance.IMaintenanceConfig.BrokenDurationMax" /> based on maintenance buffer fill).
    /// </summary>
    Duration BrokenDurationMin { get; }

    /// <summary>
    /// Maximal broken duration (interpolated with <see cref="P:Mafi.Core.Maintenance.IMaintenanceConfig.BrokenDurationMin" /> based on maintenance buffer fill).
    /// </summary>
    Duration BrokenDurationMax { get; }

    /// <summary>
    /// When entity should be broken, we test this probability whether it will actually get broken, otherwise we
    /// postpone the breakage event. This is to not break down all entities at once when maintenance is running out.
    /// 
    /// This percentage is basically the amount of actually broken entities out of pool of all entities that
    /// should be broken. Values below 10% are advised.
    /// </summary>
    Percent DailyBreakdownChanceWhenShouldBeBroken { get; }
  }
}
