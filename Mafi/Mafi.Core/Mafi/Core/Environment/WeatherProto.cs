// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Environment.WeatherProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Environment
{
  public class WeatherProto : Proto
  {
    public readonly Percent SunIntensity;
    public readonly Percent RainIntensity;
    public readonly WeatherProto.Gfx Graphics;

    public WeatherProto(
      Proto.ID id,
      Proto.Str strings,
      Percent sunIntensity,
      Percent rainIntensity,
      WeatherProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.SunIntensity = sunIntensity;
      this.RainIntensity = rainIntensity;
      this.Graphics = graphics;
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly WeatherProto.Gfx Empty;
      public readonly ColorUniversal SkyColor;
      public readonly ColorUniversal LightColor;
      public readonly ColorUniversal FogColor;
      public readonly float LightIntensity;
      public readonly float FogIntensity;
      public readonly float WindStrength;
      public readonly float MinCloudIntensity;
      public readonly float MaxCloudIntensity;
      public readonly float ShadowsIntensityAbs;
      public readonly float OceanChoppiness;
      public readonly float LightningProbabilityPerTick;
      public readonly string IconPath;
      public readonly Option<string> SoundPrefabPath;

      public Gfx(
        ColorRgba skyColor,
        ColorRgba lightColor,
        ColorRgba fogColor,
        Percent lightIntensity,
        Percent fogIntensity,
        Percent windStrength,
        Percent minCloudIntensity,
        Percent maxCloudIntensity,
        Percent shadowsIntensityAbs,
        Percent oceanChoppiness,
        string iconPath,
        Percent lightningProbabilityPerTick = default (Percent),
        Option<string> soundPrefabPath = default (Option<string>))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.SkyColor = (ColorUniversal) skyColor;
        this.LightColor = (ColorUniversal) lightColor;
        this.FogColor = (ColorUniversal) fogColor;
        this.LightIntensity = lightIntensity.ToFloat();
        this.FogIntensity = fogIntensity.ToFloat();
        this.WindStrength = windStrength.ToFloat();
        this.MinCloudIntensity = minCloudIntensity.ToFloat();
        this.MaxCloudIntensity = maxCloudIntensity.ToFloat();
        this.ShadowsIntensityAbs = shadowsIntensityAbs.ToFloat();
        this.LightningProbabilityPerTick = lightningProbabilityPerTick.ToFloat();
        this.IconPath = iconPath;
        this.OceanChoppiness = oceanChoppiness.ToFloat();
        this.SoundPrefabPath = soundPrefabPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        WeatherProto.Gfx.Empty = new WeatherProto.Gfx(ColorRgba.White, ColorRgba.White, ColorRgba.White, Percent.Hundred, Percent.Zero, Percent.Zero, Percent.Zero, Percent.Zero, Percent.Hundred, Percent.Fifty, "EMPTY");
      }
    }
  }
}
