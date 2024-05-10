// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.WeatherManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Environment
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class WeatherManager : IWeatherManager
  {
    public static readonly Duration HALF_WEATHER_GFX_TRANSITION_DAYS;
    private readonly ICalendar m_calendar;
    private readonly IWeatherProvider m_weatherProvider;
    private readonly ImmutableArray<WeatherProto> m_allWeatherProtos;
    private ImmutableArray<WeatherProto> m_thisYearWeather;
    private ImmutableArray<WeatherProto> m_nextYearWeather;
    private int m_weatherOverrideDuration;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public WeatherProto CurrentWeather { get; private set; }

    public WeatherProto NextWeather { get; private set; }

    public Percent RainIntensity => this.CurrentWeather.RainIntensity;

    public Percent SimSunIntensity => this.CurrentWeather.SunIntensity;

    [NewInSaveVersion(140, null, null, null, null)]
    public Percent WorldWetness { get; private set; }

    public WeatherManager(
      ICalendar calendar,
      ISimLoopEvents simLoopEvents,
      ProtosDb protosDb,
      IWeatherProvider weatherProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_calendar = calendar;
      this.m_weatherProvider = weatherProvider;
      this.m_allWeatherProtos = protosDb.All<WeatherProto>().ToImmutableArray<WeatherProto>();
      this.CurrentWeather = this.m_allWeatherProtos.First;
      this.NextWeather = this.m_allWeatherProtos.First;
      this.m_thisYearWeather = this.m_weatherProvider.GetWeatherForYear(1, (Option<WeatherProto>) Option.None);
      this.m_nextYearWeather = this.m_weatherProvider.GetWeatherForYear(2, (Option<WeatherProto>) this.m_thisYearWeather.Last);
      calendar.NewDay.Add<WeatherManager>(this, new Action(this.onNewDay));
      simLoopEvents.Update.Add<WeatherManager>(this, new Action(this.onSimStep));
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      resolver.Resolve<ISimLoopEvents>().Update.Add<WeatherManager>(this, new Action(this.onSimStep));
    }

    public bool Cheat_TrySetWeatherFixed(Proto.ID id)
    {
      if (!this.setWeatherByStr(id.Value))
        return false;
      this.m_weatherOverrideDuration = int.MaxValue;
      return true;
    }

    public bool Cheat_TrySetWeatherFixed(string weather)
    {
      if (!this.setWeatherByStr(weather))
        return false;
      this.m_weatherOverrideDuration = int.MaxValue;
      return true;
    }

    public bool TrySetWeather(Proto.ID id, int durationDays = 2147483647)
    {
      if (!this.setWeatherByStr(id.Value))
        return false;
      this.m_weatherOverrideDuration = durationDays;
      return true;
    }

    private bool setWeatherByStr(string weather)
    {
      WeatherProto weatherProto = this.m_allWeatherProtos.FirstOrDefault((Func<WeatherProto, bool>) (x => x.Id.Value.Contains(weather, StringComparison.OrdinalIgnoreCase)));
      if ((Proto) weatherProto == (Proto) null)
        return false;
      this.CurrentWeather = weatherProto;
      this.NextWeather = weatherProto;
      return true;
    }

    private void onSimStep()
    {
      this.WorldWetness = this.WorldWetness.Lerp(this.CurrentWeather.RainIntensity, Percent.FromFloat(1f / 500f));
    }

    private void onNewDay()
    {
      if (this.m_weatherOverrideDuration > 0)
      {
        --this.m_weatherOverrideDuration;
      }
      else
      {
        GameDate currentDate = this.m_calendar.CurrentDate;
        if (currentDate.Month == 1)
        {
          currentDate = this.m_calendar.CurrentDate;
          if (currentDate.Day == 1)
          {
            this.m_thisYearWeather = this.m_nextYearWeather;
            IWeatherProvider weatherProvider = this.m_weatherProvider;
            currentDate = this.m_calendar.CurrentDate;
            int year = currentDate.Year + 1;
            Option<WeatherProto> last = (Option<WeatherProto>) this.m_thisYearWeather.Last;
            this.m_nextYearWeather = weatherProvider.GetWeatherForYear(year, last);
          }
        }
        currentDate = this.m_calendar.CurrentDate;
        int num1 = (currentDate.Month - 1) * 30;
        currentDate = this.m_calendar.CurrentDate;
        int day = currentDate.Day;
        int num2 = num1 + day;
        this.CurrentWeather = this.m_thisYearWeather[(num2 - 1) / 15];
        int index = (num2 - 1 + WeatherManager.HALF_WEATHER_GFX_TRANSITION_DAYS.Days.IntegerPart) / 15;
        if (index >= 24)
          this.NextWeather = this.m_nextYearWeather[index - 24];
        else
          this.NextWeather = this.m_thisYearWeather[index];
      }
    }

    public Percent GetSumOfRainyFortnights()
    {
      Percent zero = Percent.Zero;
      foreach (WeatherProto weatherProto in this.m_thisYearWeather)
        zero += weatherProto.RainIntensity;
      return zero;
    }

    public static void Serialize(WeatherManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WeatherManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WeatherManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<WeatherProto>(this.CurrentWeather);
      ImmutableArray<WeatherProto>.Serialize(this.m_allWeatherProtos, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      ImmutableArray<WeatherProto>.Serialize(this.m_nextYearWeather, writer);
      ImmutableArray<WeatherProto>.Serialize(this.m_thisYearWeather, writer);
      writer.WriteInt(this.m_weatherOverrideDuration);
      writer.WriteGeneric<IWeatherProvider>(this.m_weatherProvider);
      writer.WriteGeneric<WeatherProto>(this.NextWeather);
      Percent.Serialize(this.WorldWetness, writer);
    }

    public static WeatherManager Deserialize(BlobReader reader)
    {
      WeatherManager weatherManager;
      if (reader.TryStartClassDeserialization<WeatherManager>(out weatherManager))
        reader.EnqueueDataDeserialization((object) weatherManager, WeatherManager.s_deserializeDataDelayedAction);
      return weatherManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.CurrentWeather = reader.ReadGenericAs<WeatherProto>();
      reader.SetField<WeatherManager>(this, "m_allWeatherProtos", (object) ImmutableArray<WeatherProto>.Deserialize(reader));
      reader.SetField<WeatherManager>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      this.m_nextYearWeather = ImmutableArray<WeatherProto>.Deserialize(reader);
      this.m_thisYearWeather = ImmutableArray<WeatherProto>.Deserialize(reader);
      this.m_weatherOverrideDuration = reader.ReadInt();
      reader.SetField<WeatherManager>(this, "m_weatherProvider", (object) reader.ReadGenericAs<IWeatherProvider>());
      this.NextWeather = reader.ReadGenericAs<WeatherProto>();
      this.WorldWetness = reader.LoadedSaveVersion >= 140 ? Percent.Deserialize(reader) : new Percent();
      reader.RegisterInitAfterLoad<WeatherManager>(this, "initSelf", InitPriority.Normal);
    }

    static WeatherManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WeatherManager.HALF_WEATHER_GFX_TRANSITION_DAYS = 3.Days();
      WeatherManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WeatherManager) obj).SerializeData(writer));
      WeatherManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WeatherManager) obj).DeserializeData(reader));
    }
  }
}
