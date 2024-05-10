// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Weather.WeatherData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Environment;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Weather
{
  public class WeatherData : IModData
  {
    public static readonly Percent SUNNY_FOG_INTENSITY;
    public static readonly Percent RAINY_WEATHER_RAIN_INTENSITY;
    public static readonly Percent HEAVY_RAIN_WEATHER_RAIN_INTENSITY;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<WeatherProto>(new WeatherProto(Ids.Weather.Sunny, Proto.CreateStr(Ids.Weather.Sunny, "Sunny", "", "describes the current weather"), Percent.Hundred, 0.Percent(), new WeatherProto.Gfx((ColorRgba) 7715571, (ColorRgba) 16772300, (ColorRgba) 10070200, 100.Percent(), WeatherData.SUNNY_FOG_INTENSITY, 50.Percent(), 0.Percent(), 40.Percent(), Percent.Hundred, 0.Percent(), "Assets/Base/Icons/Weather/Sunny.svg")));
      prototypesDb.Add<WeatherProto>(new WeatherProto(Ids.Weather.Cloudy, Proto.CreateStr(Ids.Weather.Cloudy, "Cloudy", "", "describes the current weather"), 80.Percent(), 0.Percent(), new WeatherProto.Gfx((ColorRgba) 10078445, (ColorRgba) 16775662, (ColorRgba) 10070200, 105.Percent(), 30.Percent(), 75.Percent(), 50.Percent(), 70.Percent(), 80.Percent(), 30.Percent(), "Assets/Base/Icons/Weather/Cloudy.svg")));
      ProtosDb protosDb1 = prototypesDb;
      Proto.ID rainy = Ids.Weather.Rainy;
      Proto.Str str1 = Proto.CreateStr(Ids.Weather.Rainy, "Rainy", "", "describes the current weather");
      Percent sunIntensity1 = 50.Percent();
      Percent weatherRainIntensity1 = WeatherData.RAINY_WEATHER_RAIN_INTENSITY;
      ColorRgba skyColor1 = (ColorRgba) 14412008;
      ColorRgba lightColor1 = (ColorRgba) 15924223;
      ColorRgba fogColor1 = (ColorRgba) 9673894;
      Percent lightIntensity1 = 100.Percent();
      Percent fogIntensity1 = 60.Percent();
      Percent windStrength1 = 100.Percent();
      Percent minCloudIntensity1 = 100.Percent();
      Percent maxCloudIntensity1 = 110.Percent();
      Percent shadowsIntensityAbs1 = 70.Percent();
      Percent oceanChoppiness1 = 80.Percent();
      Option<string> option = (Option<string>) "Assets/Unity/Weather/RainMedium.prefab";
      Percent lightningProbabilityPerTick1 = new Percent();
      Option<string> soundPrefabPath1 = option;
      WeatherProto.Gfx graphics1 = new WeatherProto.Gfx(skyColor1, lightColor1, fogColor1, lightIntensity1, fogIntensity1, windStrength1, minCloudIntensity1, maxCloudIntensity1, shadowsIntensityAbs1, oceanChoppiness1, "Assets/Base/Icons/Weather/Rain.svg", lightningProbabilityPerTick1, soundPrefabPath1);
      WeatherProto proto1 = new WeatherProto(rainy, str1, sunIntensity1, weatherRainIntensity1, graphics1);
      protosDb1.Add<WeatherProto>(proto1);
      ProtosDb protosDb2 = prototypesDb;
      Proto.ID heavyRain = Ids.Weather.HeavyRain;
      Proto.Str str2 = Proto.CreateStr(Ids.Weather.HeavyRain, "Heavy rain", "", "describes the current weather");
      Percent sunIntensity2 = 20.Percent();
      Percent weatherRainIntensity2 = WeatherData.HEAVY_RAIN_WEATHER_RAIN_INTENSITY;
      ColorRgba skyColor2 = (ColorRgba) 14146526;
      ColorRgba lightColor2 = (ColorRgba) 15463423;
      ColorRgba fogColor2 = (ColorRgba) 9277844;
      Percent lightIntensity2 = 100.Percent();
      Percent fogIntensity2 = 90.Percent();
      Percent windStrength2 = 150.Percent();
      Percent minCloudIntensity2 = 125.Percent();
      Percent maxCloudIntensity2 = 130.Percent();
      Percent shadowsIntensityAbs2 = 60.Percent();
      Percent percent = 1.Percent();
      Percent oceanChoppiness2 = 100.Percent();
      Percent lightningProbabilityPerTick2 = percent;
      Option<string> soundPrefabPath2 = (Option<string>) "Assets/Unity/Weather/RainHeavy.prefab";
      WeatherProto.Gfx graphics2 = new WeatherProto.Gfx(skyColor2, lightColor2, fogColor2, lightIntensity2, fogIntensity2, windStrength2, minCloudIntensity2, maxCloudIntensity2, shadowsIntensityAbs2, oceanChoppiness2, "Assets/Base/Icons/Weather/HeavyRain.svg", lightningProbabilityPerTick2, soundPrefabPath2);
      WeatherProto proto2 = new WeatherProto(heavyRain, str2, sunIntensity2, weatherRainIntensity2, graphics2);
      protosDb2.Add<WeatherProto>(proto2);
    }

    public WeatherData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static WeatherData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      WeatherData.SUNNY_FOG_INTENSITY = 15.Percent();
      WeatherData.RAINY_WEATHER_RAIN_INTENSITY = 50.Percent();
      WeatherData.HEAVY_RAIN_WEATHER_RAIN_INTENSITY = 100.Percent();
    }
  }
}
