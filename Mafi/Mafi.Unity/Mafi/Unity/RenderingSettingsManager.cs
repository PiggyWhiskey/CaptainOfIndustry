// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.RenderingSettingsManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Localization;
using System;
using System.Linq;
using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public static class RenderingSettingsManager
  {
    public static readonly LocStr QualityPreset__LowQuality;
    public static readonly LocStr QualityPreset__MediumQuality;
    public static readonly LocStr QualityPreset__HighQuality;
    public static readonly LocStr QualityPreset__UltraQuality;
    public static readonly ImmutableArray<Pair<RenderingSettingPreset, LocStrFormatted>> Presets;

    public static ImmutableArray<RenderingSetting> AllRenderingSettings { get; private set; }

    internal static void Initialize(params Assembly[] assemblies)
    {
      if (RenderingSettingsManager.AllRenderingSettings.IsValid)
        RenderingSettingsManager.ClearAllCallbacks();
      Lyst<RenderingSetting> lyst = new Lyst<RenderingSetting>();
      foreach (Assembly assembly in assemblies)
        RenderingSettingsManager.collectAllRenderingSettingsFromAssembly(assembly, lyst);
      RenderingSettingsManager.AllRenderingSettings = lyst.OrderBy<RenderingSetting, string>((Func<RenderingSetting, string>) (x => x.Title.Id)).ToImmutableArray<RenderingSetting>();
    }

    public static void ApplyPreset(RenderingSettingPreset preset)
    {
      foreach (RenderingSetting renderingSetting in RenderingSettingsManager.AllRenderingSettings)
      {
        int index;
        if (renderingSetting.TryGetIndexForPreset(preset, out index))
          renderingSetting.SetSettingIndex(index);
      }
    }

    public static bool IsPresetActive(RenderingSettingPreset preset)
    {
      foreach (RenderingSetting renderingSetting in RenderingSettingsManager.AllRenderingSettings)
      {
        int index;
        if (renderingSetting.TryGetIndexForPreset(preset, out index) && renderingSetting.CurrentIndex != index)
          return false;
      }
      return true;
    }

    public static void ApplyAllRenderingSettings()
    {
      foreach (RenderingSetting renderingSetting in RenderingSettingsManager.AllRenderingSettings)
        renderingSetting.ForceApply();
    }

    public static void ClearAllCallbacks()
    {
      foreach (RenderingSetting renderingSetting in RenderingSettingsManager.AllRenderingSettings)
        renderingSetting.ClearAllCallbacks();
    }

    private static void collectAllRenderingSettingsFromAssembly(
      Assembly assembly,
      Lyst<RenderingSetting> settings)
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (type.GetCustomAttribute<HasRenderingSettingsAttribute>() != null)
        {
          bool flag = false;
          foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
          {
            if (field.FieldType == typeof (RenderingSetting))
            {
              settings.Add((RenderingSetting) field.GetValue((object) null));
              flag = true;
            }
          }
          Assert.That<bool>(flag).IsTrue(string.Format("Type '{0}' is marked with '{1}' ", (object) type, (object) "HasRenderingSettingsAttribute") + "but has no static fields of type 'RenderingSetting'.");
        }
      }
    }

    static RenderingSettingsManager()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      RenderingSettingsManager.QualityPreset__LowQuality = Loc.Str(nameof (QualityPreset__LowQuality), "High performance", "rendering preset name for the worst quality but highest performance");
      RenderingSettingsManager.QualityPreset__MediumQuality = Loc.Str(nameof (QualityPreset__MediumQuality), "Balanced", "rendering preset name for the 2nd worst quality but balanced performance");
      RenderingSettingsManager.QualityPreset__HighQuality = Loc.Str(nameof (QualityPreset__HighQuality), "High quality", "rendering preset name for 2nd best option");
      RenderingSettingsManager.QualityPreset__UltraQuality = Loc.Str(nameof (QualityPreset__UltraQuality), "Ultra high quality", "rendering preset name for best rendering option");
      RenderingSettingsManager.Presets = ImmutableArray.Create<Pair<RenderingSettingPreset, LocStrFormatted>>(Pair.Create<RenderingSettingPreset, LocStrFormatted>(RenderingSettingPreset.UltraQuality, RenderingSettingsManager.QualityPreset__UltraQuality.AsFormatted), Pair.Create<RenderingSettingPreset, LocStrFormatted>(RenderingSettingPreset.HighQuality, RenderingSettingsManager.QualityPreset__HighQuality.AsFormatted), Pair.Create<RenderingSettingPreset, LocStrFormatted>(RenderingSettingPreset.MediumQuality, RenderingSettingsManager.QualityPreset__MediumQuality.AsFormatted), Pair.Create<RenderingSettingPreset, LocStrFormatted>(RenderingSettingPreset.LowQuality, RenderingSettingsManager.QualityPreset__LowQuality.AsFormatted));
    }
  }
}
