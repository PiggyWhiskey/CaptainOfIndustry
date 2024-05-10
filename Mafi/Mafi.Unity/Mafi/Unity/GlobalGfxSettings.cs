// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.GlobalGfxSettings
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  [HasRenderingSettings]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class GlobalGfxSettings
  {
    public static readonly LocStr AntiAliasingRenderingSetting__Name;
    public static readonly LocStr1 AntiAliasingRenderingSetting__Msaa;
    public static readonly LocStr1 AntiAliasingRenderingSetting__MsaaShort;
    public static readonly LocStr AntiAliasingRenderingSetting__Smaa;
    public static readonly LocStr AntiAliasingRenderingSetting__SmaaShort;
    public static readonly RenderingSetting AntiAliasingRenderingSetting;
    private static float s_camDistance;
    private static float s_shadowDistanceBase;
    private static float s_shadowDistanceMult;
    public static readonly LocStr ShadowsQualityRenderingSetting__Name;
    public static readonly RenderingSetting ShadowsQualityRenderingSetting;
    public static readonly LocStr FpsLimitOption__NoLimit;
    public static readonly LocStr FpsLimitOption__VSync1;
    public static readonly LocStr FpsLimitOption__VSync1Tooltip;
    public static readonly LocStr FpsLimitOption__VSync2;
    public static readonly LocStr FpsLimitOption__VSync2Tooltip;
    public static readonly LocStr GameFpsLimitRenderingSetting__Name;
    public static readonly RenderingSetting GameFpsLimitRenderingSetting;
    public static readonly LocStr MenusFpsLimitRenderingSetting__Name;
    public static readonly RenderingSetting MenusFpsLimitRenderingSetting;
    public static readonly LocStr BackgroundFpsLimitRenderingSetting__Name;
    public static readonly RenderingSetting BackgroundFpsLimitRenderingSetting;
    private static int s_gameVsync;
    private static int s_gameTargetFps;
    private static int s_menusVsync;
    private static int s_menusTargetFps;
    private static int s_backgroundVsync;
    private static int s_backgroundTargetFps;
    private static bool s_isInBackground;
    private static int s_isInMenus;
    public static readonly LocStr LodBiasRenderingSetting__Name;
    public static readonly RenderingSetting LodBiasRenderingSetting;

    public static void NotifyCameraDistanceChanged(float camDistance)
    {
      GlobalGfxSettings.s_camDistance = camDistance;
      GlobalGfxSettings.updateShadowsDistance(camDistance);
    }

    private static void updateShadowsDistance(float camDistance)
    {
      QualitySettings.shadowDistance = GlobalGfxSettings.s_shadowDistanceBase + camDistance * GlobalGfxSettings.s_shadowDistanceMult;
    }

    public static void NotifyIsInBackground(bool isInBackground)
    {
      GlobalGfxSettings.s_isInBackground = isInBackground;
      GlobalGfxSettings.updateFpsLimit();
    }

    public static void NotifyIsInMenus(bool isInMenus)
    {
      GlobalGfxSettings.s_isInMenus += isInMenus ? 1 : -1;
      Assert.That<int>(GlobalGfxSettings.s_isInMenus).IsNotNegative();
      GlobalGfxSettings.updateFpsLimit();
    }

    private static void updateFpsLimit()
    {
      Application.targetFrameRate = -1;
      QualitySettings.vSyncCount = 0;
      if (GlobalGfxSettings.s_isInBackground)
      {
        Application.targetFrameRate = GlobalGfxSettings.s_backgroundTargetFps;
        QualitySettings.vSyncCount = GlobalGfxSettings.s_backgroundVsync;
      }
      else if (GlobalGfxSettings.s_isInMenus > 0)
      {
        Application.targetFrameRate = GlobalGfxSettings.s_menusTargetFps;
        QualitySettings.vSyncCount = GlobalGfxSettings.s_menusVsync;
      }
      else
      {
        Application.targetFrameRate = GlobalGfxSettings.s_gameTargetFps;
        QualitySettings.vSyncCount = GlobalGfxSettings.s_gameVsync;
      }
    }

    private static void onAntiAliasingChanged(RenderingSetting setting)
    {
      int self = setting.CurrentOption.Value;
      bool flag = self < 0;
      int num = self.Abs();
      if (num == 1)
        num = 0;
      Camera main = Camera.main;
      if ((bool) (UnityEngine.Object) main)
      {
        PostProcessLayer component;
        if (main.gameObject.TryGetComponent<PostProcessLayer>(out component))
        {
          component.antialiasingMode = flag ? PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing : PostProcessLayer.Antialiasing.None;
          main.enabled = false;
          main.enabled = true;
        }
        else
          Log.Error("Failed to set anti-aliasing, post-process layer was not found.");
      }
      else
        Log.Error("Failed to set anti-aliasing, camera was not found.");
      QualitySettings.antiAliasing = num;
    }

    private static void onShadowsChanged(RenderingSetting setting)
    {
      int num1;
      float num2;
      float num3;
      ShadowResolution shadowResolution;
      switch (setting.CurrentOption.Value)
      {
        case 1:
          num1 = 1;
          num2 = 400f;
          num3 = 1.5f;
          shadowResolution = ShadowResolution.High;
          break;
        case 2:
          num1 = 4;
          num2 = 800f;
          num3 = 1.25f;
          shadowResolution = ShadowResolution.High;
          QualitySettings.shadowCascade4Split = new Vector3(0.12f, 0.28f, 0.5f);
          break;
        case 3:
          num1 = 4;
          num2 = 1200f;
          num3 = 1f;
          shadowResolution = ShadowResolution.VeryHigh;
          QualitySettings.shadowCascade4Split = new Vector3(0.12f, 0.28f, 0.5f);
          break;
        default:
          num1 = 1;
          num2 = 200f;
          num3 = 1.5f;
          shadowResolution = ShadowResolution.Medium;
          break;
      }
      GlobalGfxSettings.s_shadowDistanceBase = num2;
      GlobalGfxSettings.s_shadowDistanceMult = num3;
      QualitySettings.shadowCascades = num1;
      QualitySettings.shadowResolution = shadowResolution;
      QualitySettings.shadows = ShadowQuality.All;
      GlobalGfxSettings.updateShadowsDistance(GlobalGfxSettings.s_camDistance);
    }

    private static void onFpsLimitChanged(
      RenderingSetting setting,
      out int vsync,
      out int targetFps)
    {
      switch (setting.CurrentOption.Value)
      {
        case -2:
          vsync = 2;
          targetFps = -1;
          break;
        case -1:
          vsync = 1;
          targetFps = -1;
          break;
        case 0:
          vsync = 0;
          targetFps = -1;
          break;
        default:
          vsync = 0;
          targetFps = setting.CurrentOption.Value.CheckWithinIncl(10, 200);
          break;
      }
      GlobalGfxSettings.updateFpsLimit();
    }

    private static void onLodChanged(RenderingSetting setting)
    {
      QualitySettings.lodBias = (float) setting.CurrentOption.Value / 10f;
    }

    public GlobalGfxSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static GlobalGfxSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GlobalGfxSettings.AntiAliasingRenderingSetting__Name = Loc.Str(nameof (AntiAliasingRenderingSetting__Name), "Anti-aliasing", "rendering setting name");
      GlobalGfxSettings.AntiAliasingRenderingSetting__Msaa = Loc.Str1(nameof (AntiAliasingRenderingSetting__Msaa), "Multisample anti-aliasing (MSAA) {0}", "anti-aliasing mode, {0} is number of samples, for example 'x8'");
      GlobalGfxSettings.AntiAliasingRenderingSetting__MsaaShort = Loc.Str1(nameof (AntiAliasingRenderingSetting__MsaaShort), "MSAA {0}", "anti-aliasing mode, {0} is number of samples, for example 'x8'");
      GlobalGfxSettings.AntiAliasingRenderingSetting__Smaa = Loc.Str(nameof (AntiAliasingRenderingSetting__Smaa), "Subpixel morphological anti-aliasing (SMAA)", "anti-aliasing mode");
      GlobalGfxSettings.AntiAliasingRenderingSetting__SmaaShort = Loc.Str(nameof (AntiAliasingRenderingSetting__SmaaShort), "SMAA", "anti-aliasing mode");
      LocStr renderingSettingName1 = GlobalGfxSettings.AntiAliasingRenderingSetting__Name;
      ImmutableArray<RenderingSettingOption> options1 = ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption(GlobalGfxSettings.AntiAliasingRenderingSetting__MsaaShort.Format("x8"), 8, RenderingSettingPreset.UltraQuality, tooltip: GlobalGfxSettings.AntiAliasingRenderingSetting__Msaa.Format("x8")), new RenderingSettingOption(GlobalGfxSettings.AntiAliasingRenderingSetting__MsaaShort.Format("x4"), 4, RenderingSettingPreset.HighQuality, tooltip: GlobalGfxSettings.AntiAliasingRenderingSetting__Msaa.Format("x4")), new RenderingSettingOption(GlobalGfxSettings.AntiAliasingRenderingSetting__MsaaShort.Format("x2"), 2, RenderingSettingPreset.MediumQuality, tooltip: GlobalGfxSettings.AntiAliasingRenderingSetting__Msaa.Format("x2")), new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.AntiAliasingRenderingSetting__SmaaShort, -1, RenderingSettingPreset.LowQuality, tooltip: (LocStrFormatted) GlobalGfxSettings.AntiAliasingRenderingSetting__Smaa), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Off, 0));
      Action<RenderingSetting> action1 = new Action<RenderingSetting>(GlobalGfxSettings.onAntiAliasingChanged);
      int? defaultSettingIndex1 = new int?();
      Action<RenderingSetting> defaultCallback1 = action1;
      GlobalGfxSettings.AntiAliasingRenderingSetting = new RenderingSetting(nameof (AntiAliasingRenderingSetting), renderingSettingName1, 100, options1, defaultSettingIndex1, defaultCallback1);
      GlobalGfxSettings.s_camDistance = 100f;
      GlobalGfxSettings.ShadowsQualityRenderingSetting__Name = Loc.Str(nameof (ShadowsQualityRenderingSetting__Name), "Shadows quality", "rendering setting name");
      LocStr renderingSettingName2 = GlobalGfxSettings.ShadowsQualityRenderingSetting__Name;
      ImmutableArray<RenderingSettingOption> options2 = ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__VeryHigh, 3, RenderingSettingPreset.UltraQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__High, 2, RenderingSettingPreset.HighQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Medium, 1, RenderingSettingPreset.MediumQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Low, 0, RenderingSettingPreset.LowQuality));
      Action<RenderingSetting> action2 = new Action<RenderingSetting>(GlobalGfxSettings.onShadowsChanged);
      int? nullable = new int?();
      int? defaultSettingIndex2 = nullable;
      Action<RenderingSetting> defaultCallback2 = action2;
      GlobalGfxSettings.ShadowsQualityRenderingSetting = new RenderingSetting(nameof (ShadowsQualityRenderingSetting), renderingSettingName2, 120, options2, defaultSettingIndex2, defaultCallback2);
      GlobalGfxSettings.FpsLimitOption__NoLimit = Loc.Str(nameof (FpsLimitOption__NoLimit), "No limit", "option for FpsLimitOption__NoLimit");
      GlobalGfxSettings.FpsLimitOption__VSync1 = Loc.Str(nameof (FpsLimitOption__VSync1), "VSync", "option for FpsLimitOption__VSync1");
      GlobalGfxSettings.FpsLimitOption__VSync1Tooltip = Loc.Str(nameof (FpsLimitOption__VSync1Tooltip), "Synchronize with screen refresh rate", "tooltip for FpsLimitOption__VSync1Tooltip");
      GlobalGfxSettings.FpsLimitOption__VSync2 = Loc.Str(nameof (FpsLimitOption__VSync2), "VSync x2", "option for FpsLimitOption__VSync2");
      GlobalGfxSettings.FpsLimitOption__VSync2Tooltip = Loc.Str(nameof (FpsLimitOption__VSync2Tooltip), "Synchronize with screen, every other frame", "tooltip for FpsLimitOption__VSync2Tooltip");
      GlobalGfxSettings.GameFpsLimitRenderingSetting__Name = Loc.Str(nameof (GameFpsLimitRenderingSetting__Name), "FPS limit in game", "rendering setting name");
      LocStr renderingSettingName3 = GlobalGfxSettings.GameFpsLimitRenderingSetting__Name;
      nullable = new int?(1);
      ImmutableArray<RenderingSettingOption> options3 = ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__NoLimit, 0), new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync1, -1, tooltip: (LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync1Tooltip), new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync2, -2, tooltip: (LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync2Tooltip), new RenderingSettingOption("30".AsLoc(), 30), new RenderingSettingOption("45".AsLoc(), 45), new RenderingSettingOption("60".AsLoc(), 60), new RenderingSettingOption("80".AsLoc(), 80), new RenderingSettingOption("100".AsLoc(), 100));
      int? defaultSettingIndex3 = nullable;
      Action<RenderingSetting> defaultCallback3 = (Action<RenderingSetting>) (s => GlobalGfxSettings.onFpsLimitChanged(s, out GlobalGfxSettings.s_gameVsync, out GlobalGfxSettings.s_gameTargetFps));
      GlobalGfxSettings.GameFpsLimitRenderingSetting = new RenderingSetting(nameof (GameFpsLimitRenderingSetting), renderingSettingName3, 10, options3, defaultSettingIndex3, defaultCallback3);
      GlobalGfxSettings.MenusFpsLimitRenderingSetting__Name = Loc.Str(nameof (MenusFpsLimitRenderingSetting__Name), "FPS limit in menus", "rendering setting name");
      LocStr renderingSettingName4 = GlobalGfxSettings.MenusFpsLimitRenderingSetting__Name;
      nullable = new int?(1);
      ImmutableArray<RenderingSettingOption> options4 = ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__NoLimit, 0), new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync1, -1, tooltip: (LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync1Tooltip), new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync2, -2, tooltip: (LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync2Tooltip), new RenderingSettingOption("30".AsLoc(), 30), new RenderingSettingOption("45".AsLoc(), 45), new RenderingSettingOption("60".AsLoc(), 60), new RenderingSettingOption("80".AsLoc(), 80));
      int? defaultSettingIndex4 = nullable;
      Action<RenderingSetting> defaultCallback4 = (Action<RenderingSetting>) (s => GlobalGfxSettings.onFpsLimitChanged(s, out GlobalGfxSettings.s_menusVsync, out GlobalGfxSettings.s_menusTargetFps));
      GlobalGfxSettings.MenusFpsLimitRenderingSetting = new RenderingSetting(nameof (MenusFpsLimitRenderingSetting), renderingSettingName4, 20, options4, defaultSettingIndex4, defaultCallback4);
      GlobalGfxSettings.BackgroundFpsLimitRenderingSetting__Name = Loc.Str(nameof (BackgroundFpsLimitRenderingSetting__Name), "FPS limit in background", "rendering setting name");
      LocStr renderingSettingName5 = GlobalGfxSettings.BackgroundFpsLimitRenderingSetting__Name;
      nullable = new int?(3);
      ImmutableArray<RenderingSettingOption> options5 = ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__NoLimit, 0), new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync1, -1, tooltip: (LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync1Tooltip), new RenderingSettingOption((LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync2, -2, tooltip: (LocStrFormatted) GlobalGfxSettings.FpsLimitOption__VSync2Tooltip), new RenderingSettingOption("30".AsLoc(), 30), new RenderingSettingOption("45".AsLoc(), 45), new RenderingSettingOption("60".AsLoc(), 60), new RenderingSettingOption("80".AsLoc(), 80));
      int? defaultSettingIndex5 = nullable;
      Action<RenderingSetting> defaultCallback5 = (Action<RenderingSetting>) (s => GlobalGfxSettings.onFpsLimitChanged(s, out GlobalGfxSettings.s_backgroundVsync, out GlobalGfxSettings.s_backgroundTargetFps));
      GlobalGfxSettings.BackgroundFpsLimitRenderingSetting = new RenderingSetting(nameof (BackgroundFpsLimitRenderingSetting), renderingSettingName5, 30, options5, defaultSettingIndex5, defaultCallback5);
      GlobalGfxSettings.LodBiasRenderingSetting__Name = Loc.Str(nameof (LodBiasRenderingSetting__Name), "Level of detail (LOD)", "rendering setting name");
      LocStr renderingSettingName6 = GlobalGfxSettings.LodBiasRenderingSetting__Name;
      ImmutableArray<RenderingSettingOption> options6 = ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__VeryHigh, 15, RenderingSettingPreset.UltraQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__High, 10, RenderingSettingPreset.HighQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Medium, 8, RenderingSettingPreset.MediumQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Low, 6, RenderingSettingPreset.LowQuality));
      Action<RenderingSetting> action3 = new Action<RenderingSetting>(GlobalGfxSettings.onLodChanged);
      int? defaultSettingIndex6 = new int?();
      Action<RenderingSetting> defaultCallback6 = action3;
      GlobalGfxSettings.LodBiasRenderingSetting = new RenderingSetting(nameof (LodBiasRenderingSetting), renderingSettingName6, 110, options6, defaultSettingIndex6, defaultCallback6);
    }
  }
}
