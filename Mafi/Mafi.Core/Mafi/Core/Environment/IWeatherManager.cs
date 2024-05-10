// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.IWeatherManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Environment
{
  public interface IWeatherManager
  {
    WeatherProto CurrentWeather { get; }

    WeatherProto NextWeather { get; }

    Percent RainIntensity { get; }

    Percent SimSunIntensity { get; }

    Percent WorldWetness { get; }

    bool TrySetWeather(Proto.ID id, int durationDays = 2147483647);
  }
}
