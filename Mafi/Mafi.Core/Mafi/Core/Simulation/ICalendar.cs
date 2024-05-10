// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.ICalendar
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Simulation
{
  public interface ICalendar
  {
    /// <summary>
    /// Called when new year starts. Called on Sim thread at update end.
    /// </summary>
    IEvent NewYear { get; }

    IEvent NewYearEnd { get; }

    /// <summary>
    /// Called when new month starts right after <see cref="P:Mafi.Core.Simulation.ICalendar.NewMonth" />. Always called before <see cref="P:Mafi.Core.Simulation.ICalendar.NewYear" />.
    /// Called on Sim thread at update end.
    /// </summary>
    IEvent NewMonthStart { get; }

    /// <summary>
    /// Called when new month starts. Always called before <see cref="P:Mafi.Core.Simulation.ICalendar.NewYear" />. Called on Sim thread at update end.
    /// </summary>
    IEvent NewMonth { get; }

    /// <summary>
    /// Called when new month starts right after <see cref="P:Mafi.Core.Simulation.ICalendar.NewMonth" />. Always called before <see cref="P:Mafi.Core.Simulation.ICalendar.NewYear" />. Called on Sim thread at update end.
    /// </summary>
    IEvent NewMonthEnd { get; }

    /// <summary>
    /// Called when new day starts. Always called before <see cref="P:Mafi.Core.Simulation.ICalendar.NewMonth" />. Called on Sim thread at update end.
    /// </summary>
    IEvent NewDay { get; }

    IEvent NewDayEnd { get; }

    /// <summary>Current in-game time since start of the game.</summary>
    GameDate CurrentDate { get; }

    /// <summary>Current real duration since start of the game.</summary>
    Duration RealTime { get; }

    /// <summary>
    /// Conversion of real time to in-game time, may cause loss of precision.
    /// </summary>
    RelGameDate DurationToRelTime(Duration duration);

    /// <summary>Conversion of in-game time to real time.</summary>
    Duration RelTimeToDuration(RelGameDate relGameDate);
  }
}
