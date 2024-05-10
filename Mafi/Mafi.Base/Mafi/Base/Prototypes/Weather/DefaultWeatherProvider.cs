// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Weather.DefaultWeatherProvider
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Environment;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Weather
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class DefaultWeatherProvider : IWeatherProvider
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Percent DEFAULT;
    private readonly IRandom m_random;
    private readonly WeatherProto m_heavyRainProto;
    private readonly WeatherProto m_mediumRainProto;
    private readonly WeatherProto m_cloudyProto;
    private readonly WeatherProto m_sunnyProto;
    private readonly Dict<int, DefaultWeatherProvider.WeatherData> m_dataPerYear;
    private int m_dataPerYearLastYear;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<WeatherProto> m_tempGenResult;

    public static void Serialize(DefaultWeatherProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DefaultWeatherProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DefaultWeatherProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<WeatherProto>(this.m_cloudyProto);
      Dict<int, DefaultWeatherProvider.WeatherData>.Serialize(this.m_dataPerYear, writer);
      writer.WriteInt(this.m_dataPerYearLastYear);
      writer.WriteGeneric<WeatherProto>(this.m_heavyRainProto);
      writer.WriteGeneric<WeatherProto>(this.m_mediumRainProto);
      writer.WriteGeneric<IRandom>(this.m_random);
      writer.WriteGeneric<WeatherProto>(this.m_sunnyProto);
    }

    public static DefaultWeatherProvider Deserialize(BlobReader reader)
    {
      DefaultWeatherProvider defaultWeatherProvider;
      if (reader.TryStartClassDeserialization<DefaultWeatherProvider>(out defaultWeatherProvider))
        reader.EnqueueDataDeserialization((object) defaultWeatherProvider, DefaultWeatherProvider.s_deserializeDataDelayedAction);
      return defaultWeatherProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<DefaultWeatherProvider>(this, "m_cloudyProto", (object) reader.ReadGenericAs<WeatherProto>());
      reader.SetField<DefaultWeatherProvider>(this, "m_dataPerYear", (object) Dict<int, DefaultWeatherProvider.WeatherData>.Deserialize(reader));
      this.m_dataPerYearLastYear = reader.ReadInt();
      reader.SetField<DefaultWeatherProvider>(this, "m_heavyRainProto", (object) reader.ReadGenericAs<WeatherProto>());
      reader.SetField<DefaultWeatherProvider>(this, "m_mediumRainProto", (object) reader.ReadGenericAs<WeatherProto>());
      reader.SetField<DefaultWeatherProvider>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      reader.SetField<DefaultWeatherProvider>(this, "m_sunnyProto", (object) reader.ReadGenericAs<WeatherProto>());
      reader.SetField<DefaultWeatherProvider>(this, "m_tempGenResult", (object) new Lyst<WeatherProto>());
    }

    private static ImmutableArray<DefaultWeatherProvider.WeatherData> EasyDifficulty
    {
      get
      {
        return ((ICollection<DefaultWeatherProvider.WeatherData>) new DefaultWeatherProvider.WeatherData[2]
        {
          new DefaultWeatherProvider.WeatherData(1, 450.Percent()),
          new DefaultWeatherProvider.WeatherData(15, 350.Percent())
        }).ToImmutableArray<DefaultWeatherProvider.WeatherData>();
      }
    }

    private static ImmutableArray<DefaultWeatherProvider.WeatherData> NormalDifficulty
    {
      get
      {
        return ((ICollection<DefaultWeatherProvider.WeatherData>) new DefaultWeatherProvider.WeatherData[2]
        {
          new DefaultWeatherProvider.WeatherData(1, 400.Percent()),
          new DefaultWeatherProvider.WeatherData(10, 300.Percent())
        }).ToImmutableArray<DefaultWeatherProvider.WeatherData>();
      }
    }

    private static ImmutableArray<DefaultWeatherProvider.WeatherData> HardDifficulty
    {
      get
      {
        return ((ICollection<DefaultWeatherProvider.WeatherData>) new DefaultWeatherProvider.WeatherData[3]
        {
          new DefaultWeatherProvider.WeatherData(1, 350.Percent()),
          new DefaultWeatherProvider.WeatherData(10, 300.Percent()),
          new DefaultWeatherProvider.WeatherData(20, 250.Percent())
        }).ToImmutableArray<DefaultWeatherProvider.WeatherData>();
      }
    }

    public DefaultWeatherProvider(
      ProtosDb protosDb,
      RandomProvider randomProvider,
      GameDifficultyConfig difficultyConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_dataPerYear = new Dict<int, DefaultWeatherProvider.WeatherData>();
      this.m_tempGenResult = new Lyst<WeatherProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_heavyRainProto = protosDb.GetOrThrow<WeatherProto>(Ids.Weather.HeavyRain);
      this.m_mediumRainProto = protosDb.GetOrThrow<WeatherProto>(Ids.Weather.Rainy);
      this.m_cloudyProto = protosDb.GetOrThrow<WeatherProto>(Ids.Weather.Cloudy);
      this.m_sunnyProto = protosDb.GetOrThrow<WeatherProto>(Ids.Weather.Sunny);
      this.m_random = randomProvider.GetSimRandomFor((object) this, "Weather");
      switch (difficultyConfig.WeatherDifficulty)
      {
        case GameDifficultyConfig.WeatherDifficultySetting.Easy:
          generateDataPerYear(DefaultWeatherProvider.EasyDifficulty);
          break;
        case GameDifficultyConfig.WeatherDifficultySetting.Dry:
          generateDataPerYear(DefaultWeatherProvider.HardDifficulty);
          break;
        default:
          generateDataPerYear(DefaultWeatherProvider.NormalDifficulty);
          break;
      }

      void generateDataPerYear(
        ImmutableArray<DefaultWeatherProvider.WeatherData> source)
      {
        Assert.That<Dict<int, DefaultWeatherProvider.WeatherData>>(this.m_dataPerYear).IsEmpty<int, DefaultWeatherProvider.WeatherData>();
        Assert.That<int>(source.First.Year).IsEqualTo(1);
        for (int index = 0; index < source.Length; ++index)
        {
          DefaultWeatherProvider.WeatherData weatherData1 = source[index];
          if (index == source.Length - 1)
          {
            this.m_dataPerYear.AddAndAssertNew(weatherData1.Year, weatherData1);
            this.m_dataPerYearLastYear = weatherData1.Year;
          }
          else
          {
            DefaultWeatherProvider.WeatherData weatherData2 = source[index + 1];
            for (int year = weatherData1.Year; year < weatherData2.Year; ++year)
              this.m_dataPerYear.AddAndAssertNew(year, weatherData1);
          }
        }
      }
    }

    private ImmutableArray<WeatherProto> generateAnnualScheduleFor(
      DefaultWeatherProvider.WeatherData data,
      Option<WeatherProto> previousMonthWeather)
    {
      int num = 0;
      this.generateScheduleToTemp(data, previousMonthWeather);
      for (; containsTripleRain(this.m_tempGenResult) && num < 6; ++num)
      {
        this.m_tempGenResult.Clear();
        this.generateScheduleToTemp(data, previousMonthWeather);
      }
      Assert.That<int>(this.m_tempGenResult.Count).IsEqualTo(24);
      return this.m_tempGenResult.ToImmutableArrayAndClear();

      static bool containsTripleRain(Lyst<WeatherProto> schedule)
      {
        for (int index = 0; index < schedule.Count - 2; ++index)
        {
          if (schedule[index].RainIntensity.IsPositive && schedule[index + 1].RainIntensity.IsPositive && schedule[index + 2].RainIntensity.IsPositive)
            return true;
        }
        return false;
      }
    }

    private void generateScheduleToTemp(
      DefaultWeatherProvider.WeatherData data,
      Option<WeatherProto> previousYearLastWeather)
    {
      int intPercentRounded = data.RainPerYear.ToIntPercentRounded();
      int heavyRains = 0;
      if (intPercentRounded >= 450)
        heavyRains = this.m_random.TestProbability(80.Percent()) ? 2 : 1;
      else if (intPercentRounded >= 400)
        heavyRains = this.m_random.TestProbability(50.Percent()) ? 1 : 0;
      else if (intPercentRounded >= 250)
        heavyRains = this.m_random.TestProbability(80.Percent()) ? 1 : 0;
      Percent probabilityOfRainAndHeavyRain;
      Percent probabilityOfDoubleRain;
      if (intPercentRounded >= 400)
      {
        probabilityOfDoubleRain = 90.Percent();
        probabilityOfRainAndHeavyRain = 80.Percent();
      }
      else if (intPercentRounded >= 300)
      {
        probabilityOfDoubleRain = 70.Percent();
        probabilityOfRainAndHeavyRain = 80.Percent();
      }
      else
      {
        probabilityOfDoubleRain = 10.Percent();
        probabilityOfRainAndHeavyRain = 10.Percent();
      }
      int mediumRains = (intPercentRounded - heavyRains * this.m_heavyRainProto.RainIntensity.ToIntPercentRounded()) / this.m_mediumRainProto.RainIntensity.ToIntPercentRounded();
      Lyst<WeatherProto> result = this.m_tempGenResult;
      for (int index = 5; index > 0; --index)
      {
        generateRainAndClouds();
        if (!(data.RainPerYear < 250.Percent()))
        {
          int num = 0;
          int self = 0;
          foreach (Proto proto in result)
          {
            if (proto == (Proto) this.m_sunnyProto)
            {
              ++num;
            }
            else
            {
              self = self.Max(num);
              num = 0;
            }
          }
          if (self < 12)
            break;
        }
        else
          break;
      }
      Assert.That<int>(result.Count).IsEqualTo(24);

      void generateRainAndClouds()
      {
        result.Clear();
        for (int index = 0; index < 24; ++index)
          result.Add(this.m_sunnyProto);
        int num1 = 0;
        WeatherProto valueOrNull = previousYearLastWeather.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.RainIntensity.IsPositive ? 1 : 0) : 0) != 0)
          num1 = 1;
        Lyst<int> lyst = new Lyst<int>();
        lyst.Clear();
        for (int index = num1; index < 12; ++index)
          lyst.Add(index);
        int mediumRains = mediumRains;
        for (int index = 0; index < heavyRains; ++index)
        {
          int num2 = lyst[this.m_random];
          lyst.Remove(num2);
          if (num2 == 0 && previousYearLastWeather == this.m_sunnyProto)
          {
            result[2 * num2] = this.m_cloudyProto;
            result[2 * num2 + 1] = this.m_heavyRainProto;
          }
          else
          {
            bool flag = this.m_random.TestProbability(50.Percent());
            if (flag)
              result[2 * num2] = this.m_heavyRainProto;
            else
              result[2 * num2 + 1] = this.m_heavyRainProto;
            if (this.m_random.TestProbability(probabilityOfRainAndHeavyRain))
            {
              if (flag)
                result[2 * num2 + 1] = this.m_mediumRainProto;
              else
                result[2 * num2] = this.m_mediumRainProto;
              --mediumRains;
            }
          }
        }
        while (mediumRains > 0)
        {
          int num3 = lyst[this.m_random];
          lyst.Remove(num3);
          if (num3 == 0 && previousYearLastWeather == this.m_sunnyProto)
          {
            result[2 * num3] = this.m_cloudyProto;
            result[2 * num3 + 1] = this.m_mediumRainProto;
            --mediumRains;
          }
          else
          {
            result[2 * num3] = this.m_mediumRainProto;
            --mediumRains;
            if (mediumRains > 0 && this.m_random.TestProbability(probabilityOfDoubleRain))
            {
              result[2 * num3 + 1] = this.m_mediumRainProto;
              --mediumRains;
            }
          }
        }
        for (int index = 0; index < 24; ++index)
        {
          if (!((Proto) result[index] != (Proto) this.m_sunnyProto))
          {
            if (index < 23 && ((Proto) result[index + 1] == (Proto) this.m_mediumRainProto || (Proto) result[index + 1] == (Proto) this.m_heavyRainProto))
              result[index] = this.m_cloudyProto;
            else if (index > 0 && ((Proto) result[index - 1] == (Proto) this.m_mediumRainProto || (Proto) result[index - 1] == (Proto) this.m_heavyRainProto))
              result[index] = this.m_cloudyProto;
          }
        }
      }
    }

    public ImmutableArray<WeatherProto> GetWeatherForYear(
      int year,
      Option<WeatherProto> previousMonthWeather)
    {
      DefaultWeatherProvider.WeatherData data;
      if (this.m_dataPerYearLastYear < year)
        data = this.m_dataPerYear[this.m_dataPerYearLastYear];
      else if (!this.m_dataPerYear.TryGetValue(year, out data))
      {
        Log.Error(string.Format("Failed to find weather data for {0}!", (object) year));
        data = new DefaultWeatherProvider.WeatherData(year, DefaultWeatherProvider.DEFAULT);
      }
      return this.generateAnnualScheduleFor(data, previousMonthWeather);
    }

    static DefaultWeatherProvider()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      DefaultWeatherProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((DefaultWeatherProvider) obj).SerializeData(writer));
      DefaultWeatherProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((DefaultWeatherProvider) obj).DeserializeData(reader));
      DefaultWeatherProvider.DEFAULT = 300.Percent();
    }

    [GenerateSerializer(false, null, 0)]
    private readonly struct WeatherData
    {
      public readonly int Year;
      public readonly Percent RainPerYear;

      public static void Serialize(DefaultWeatherProvider.WeatherData value, BlobWriter writer)
      {
        writer.WriteInt(value.Year);
        Percent.Serialize(value.RainPerYear, writer);
      }

      public static DefaultWeatherProvider.WeatherData Deserialize(BlobReader reader)
      {
        return new DefaultWeatherProvider.WeatherData(reader.ReadInt(), Percent.Deserialize(reader));
      }

      [LoadCtor]
      public WeatherData(int year, Percent rainPerYear)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Year = year;
        this.RainPerYear = rainPerYear;
      }
    }
  }
}
