// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.ScreenshotConfigFlags
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace Mafi.Unity.Utils
{
  [Flags]
  public enum ScreenshotConfigFlags
  {
    DisableWeather = 2,
    DisableIcons = 4,
    DisableTerrainOverlays = 8,
    DisableTerrainGrid = 16, // 0x00000010
    DisableHighlights = 32, // 0x00000020
    DisableResourceBars = 64, // 0x00000040
    DisableFog = 128, // 0x00000080
    Force8xMsaa = 256, // 0x00000100
    ForceLod0 = 512, // 0x00000200
    ForceNoChunkCulling = 1024, // 0x00000400
  }
}
