// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UnityModConfig
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Unity.Utils;
using Mafi.Unity.Weather;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public sealed class UnityModConfig : 
    IModConfig,
    IConfig,
    IScreenshotConfig,
    IWeatherRenderingConfig
  {
    public bool DisableUi { get; set; }

    public bool DisableTerrainRendering { get; set; }

    /// <summary>
    /// Whether to include generated layout for all layout entities.
    /// </summary>
    public bool IncludeGenLayoutForEntities { get; set; }

    public Duration AutoScreenshotTakeInterval { get; set; }

    public bool DisableWeatherRendering { get; set; }

    public UnityModConfig()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
