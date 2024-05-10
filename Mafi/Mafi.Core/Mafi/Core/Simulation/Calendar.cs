// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.Calendar
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.GameLoop;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Simulation
{
  /// <summary>
  /// Keeps track of in-game time. Provides calendar events to be subscribed to and conversions from real time to
  /// in-game time. These are not real calendar events but in-game calendar events where everything takes less time.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class Calendar : ICalendar
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// One day is 2 seconds. This makes 1 month exactly 1 minute long and 1 year is exactly 12 minutes.
    /// </summary>
    public const int SIM_STEPS_PER_DAY = 20;
    private readonly Event m_newYear;
    private readonly Event m_newYearEnd;
    private readonly Event m_newMonthStart;
    private readonly Event m_newMonth;
    private readonly Event m_newMonthEnd;
    private readonly Event m_newDay;
    private readonly Event m_newDayEnd;
    private int m_ticksTillNextDay;

    public static void Serialize(Calendar value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Calendar>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Calendar.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      GameDate.Serialize(this.CurrentDate, writer);
      Event.Serialize(this.m_newDay, writer);
      Event.Serialize(this.m_newDayEnd, writer);
      Event.Serialize(this.m_newMonth, writer);
      Event.Serialize(this.m_newMonthEnd, writer);
      Event.Serialize(this.m_newMonthStart, writer);
      Event.Serialize(this.m_newYear, writer);
      Event.Serialize(this.m_newYearEnd, writer);
      writer.WriteInt(this.m_ticksTillNextDay);
      Duration.Serialize(this.RealTime, writer);
    }

    public static Calendar Deserialize(BlobReader reader)
    {
      Calendar calendar;
      if (reader.TryStartClassDeserialization<Calendar>(out calendar))
        reader.EnqueueDataDeserialization((object) calendar, Calendar.s_deserializeDataDelayedAction);
      return calendar;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.CurrentDate = GameDate.Deserialize(reader);
      reader.SetField<Calendar>(this, "m_newDay", (object) Event.Deserialize(reader));
      reader.SetField<Calendar>(this, "m_newDayEnd", (object) Event.Deserialize(reader));
      reader.SetField<Calendar>(this, "m_newMonth", (object) Event.Deserialize(reader));
      reader.SetField<Calendar>(this, "m_newMonthEnd", (object) Event.Deserialize(reader));
      reader.SetField<Calendar>(this, "m_newMonthStart", (object) Event.Deserialize(reader));
      reader.SetField<Calendar>(this, "m_newYear", (object) Event.Deserialize(reader));
      reader.SetField<Calendar>(this, "m_newYearEnd", (object) Event.Deserialize(reader));
      this.m_ticksTillNextDay = reader.ReadInt();
      this.RealTime = Duration.Deserialize(reader);
    }

    public IEvent NewYear => (IEvent) this.m_newYear;

    public IEvent NewYearEnd => (IEvent) this.m_newYearEnd;

    public IEvent NewMonthStart => (IEvent) this.m_newMonthStart;

    public IEvent NewMonth => (IEvent) this.m_newMonth;

    public IEvent NewMonthEnd => (IEvent) this.m_newMonthEnd;

    public IEvent NewDay => (IEvent) this.m_newDay;

    public IEvent NewDayEnd => (IEvent) this.m_newDayEnd;

    public GameDate CurrentDate { get; private set; }

    public Duration RealTime { get; private set; }

    public Calendar(ISimLoopEvents simLoopEvents, IGameLoopEvents gameLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_newYear = new Event();
      this.m_newYearEnd = new Event();
      this.m_newMonthStart = new Event();
      this.m_newMonth = new Event();
      this.m_newMonthEnd = new Event();
      this.m_newDay = new Event();
      this.m_newDayEnd = new Event();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Calendar owner = this;
      gameLoopEvents.RegisterNewGameInitialized((object) this, new Action(init));

      void init()
      {
        simLoopEvents.UpdateEnd.Add<Calendar>(owner, new Action(owner.updateEnd));
        owner.m_ticksTillNextDay = 20;
      }
    }

    public RelGameDate DurationToRelTime(Duration duration)
    {
      return RelGameDate.FromDays(duration.Ticks / 20);
    }

    public Duration RelTimeToDuration(RelGameDate relGameDate)
    {
      return new Duration(20 * relGameDate.TotalDays);
    }

    private void updateEnd()
    {
      this.RealTime += Duration.OneTick;
      --this.m_ticksTillNextDay;
      if (this.m_ticksTillNextDay > 0)
        return;
      this.m_ticksTillNextDay = 20;
      GameDate currentDate1 = this.CurrentDate;
      this.CurrentDate += RelGameDate.OneDay;
      this.m_newDay.InvokeTraced("NewDay");
      this.m_newDayEnd.InvokeTraced("NewDayEnd");
      GameDate currentDate2 = this.CurrentDate;
      if (currentDate2.Month == currentDate1.Month)
        return;
      this.m_newMonthStart.InvokeTraced("NewMonthStart");
      this.m_newMonth.InvokeTraced("NewMonth");
      Event @event = this.m_newMonthEnd;
      if (@event == null)
      {
        Log.Error("Calendar's `m_newMonthEnd` is null, WTF?");
        @event = new Event();
        ReflectionUtils.SetField<Calendar>(this, "m_newMonthEnd", (object) @event);
      }
      @event.InvokeTraced("NewMonthEnd");
      currentDate2 = this.CurrentDate;
      if (currentDate2.Year == currentDate1.Year)
        return;
      this.m_newYear.InvokeTraced("NewYear");
      this.m_newYearEnd.InvokeTraced("NewYearEnd");
    }

    static Calendar()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Calendar.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Calendar) obj).SerializeData(writer));
      Calendar.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Calendar) obj).DeserializeData(reader));
    }
  }
}
