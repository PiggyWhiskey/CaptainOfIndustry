// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.LoadSave.DateTimeLabel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.LoadSave
{
  public class DateTimeLabel : Label
  {
    private static readonly TimeSpan SECONDS_THRESHOLD;
    private static readonly TimeSpan MINUTES_THRESHOLD;
    private static readonly TimeSpan HOURS_THRESHOLD;
    private static readonly TimeSpan DAYS_THRESHOLD;

    public DateTimeLabel(DateTime dateTime)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(LocStrFormatted.Empty);
      this.Text<DateTimeLabel>(this.BuildLabel(dateTime));
    }

    private LocStrFormatted BuildLabel(DateTime dateTime)
    {
      TimeSpan timeSpan = (DateTime.Now - dateTime).Duration();
      if (timeSpan < DateTimeLabel.SECONDS_THRESHOLD)
        return Tr.RelativeTime_Seconds.Format(timeSpan.TotalSeconds.RoundToInt());
      if (timeSpan < DateTimeLabel.MINUTES_THRESHOLD)
        return Tr.RelativeTime_Minutes.Format(timeSpan.TotalMinutes.RoundToInt());
      if (timeSpan < DateTimeLabel.HOURS_THRESHOLD)
        return string.Format("{0} - {1:T}", (object) Tr.RelativeTime_Hours.Format(timeSpan.TotalHours.RoundToInt()).Value, (object) dateTime).AsLoc();
      return timeSpan < DateTimeLabel.DAYS_THRESHOLD ? string.Format("{0} - {1:T}", (object) Tr.RelativeTime_Days.Format(timeSpan.TotalDays.RoundToInt()).Value, (object) dateTime).AsLoc() : dateTime.ToString().AsLoc();
    }

    static DateTimeLabel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DateTimeLabel.SECONDS_THRESHOLD = TimeSpan.FromMinutes(1.0);
      DateTimeLabel.MINUTES_THRESHOLD = TimeSpan.FromMinutes(120.0);
      DateTimeLabel.HOURS_THRESHOLD = TimeSpan.FromHours(48.0);
      DateTimeLabel.DAYS_THRESHOLD = TimeSpan.FromDays(100.0);
    }
  }
}
