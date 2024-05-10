// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.RenderingSettingOption
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public readonly struct RenderingSettingOption
  {
    public readonly LocStrFormatted Name;
    public readonly LocStrFormatted Tooltip;
    public readonly int Value;
    public readonly RenderingSettingPreset Preset;
    public readonly bool IsSupported;

    public RenderingSettingOption(
      LocStrFormatted name,
      int value,
      RenderingSettingPreset preset = RenderingSettingPreset.None,
      bool isSupported = true,
      LocStrFormatted tooltip = default (LocStrFormatted))
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Name = name;
      this.Value = value;
      this.Preset = preset;
      this.IsSupported = isSupported;
      this.Tooltip = tooltip;
    }
  }
}
