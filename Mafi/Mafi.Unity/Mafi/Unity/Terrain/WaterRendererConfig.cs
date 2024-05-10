// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.WaterRendererConfig
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Game;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [Serializable]
  public class WaterRendererConfig : IConfig
  {
    public float G;
    public float Depth;
    public float LengthScale0;
    public float LengthScale1;
    public float BoundaryLow;
    public float BoundaryHigh;
    public WaterRendererConfig.DisplaySpectrumSettings DefaultSpectrum;
    public WaterRendererConfig.WaterWeatherParams CalmWaterParams;
    public WaterRendererConfig.WaterWeatherParams RoughWaterParams;

    public WaterRendererConfig()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.G = 4.905f;
      this.Depth = 500f;
      this.LengthScale0 = 777f;
      this.LengthScale1 = 40f;
      this.BoundaryLow = 0.01f;
      this.BoundaryHigh = 20f;
      this.DefaultSpectrum = new WaterRendererConfig.DisplaySpectrumSettings()
      {
        Scale = 1f,
        WindSpeed = 1f,
        Fetch = 1000000f,
        PeakEnhancement = 0.5f
      };
      this.CalmWaterParams = new WaterRendererConfig.WaterWeatherParams()
      {
        Smoothness = 0.8f,
        Wind = 0.25f,
        DepthColorAttenMult = 0.025f,
        TransparencyAttenMult = 0.01f,
        SssIntensity = 80f,
        ShoreFoamDepth = 7f
      };
      this.RoughWaterParams = new WaterRendererConfig.WaterWeatherParams()
      {
        Smoothness = 0.4f,
        Wind = 1.5f,
        DepthColorAttenMult = 0.05f,
        TransparencyAttenMult = 0.025f,
        SssIntensity = 0.0f,
        ShoreFoamDepth = 10f
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    [Serializable]
    public struct DisplaySpectrumSettings
    {
      [Range(0.0f, 1f)]
      public float Scale;
      public float WindSpeed;
      public float Fetch;
      public float PeakEnhancement;
    }

    [Serializable]
    public struct WaterWeatherParams
    {
      public float Smoothness;
      public float Wind;
      public float DepthColorAttenMult;
      public float TransparencyAttenMult;
      public float SssIntensity;
      public float ShoreFoamDepth;
    }
  }
}
