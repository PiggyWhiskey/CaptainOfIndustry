// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.IMain
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.SaveGame;
using Mafi.Core.Terrain.Generation;
using Mafi.Unity.Audio;
using Mafi.Unity.MainMenu;
using Mafi.Unity.MapEditor;

#nullable disable
namespace Mafi.Unity
{
  public interface IMain
  {
    ImmutableArray<ModData> Mods { get; }

    /// <summary>
    /// Mods that failed hard on type resolution (not included in Mods).
    /// </summary>
    ImmutableArray<ModInfoRaw> FailedMods { get; }

    bool IsInitializing { get; }

    AssetsDb AssetsDb { get; }

    AudioDb AudioDb { get; }

    BackgroundMusicManager BackgroundMusicManager { get; }

    Option<IGameScene> CurrentScene { get; }

    IFileSystemHelper FileSystemHelper { get; }

    void InitMods(ImmutableArray<IMod> mods);

    PreInitModsAndProtos PreInitializeModsAndProtos();

    void EnsureModsStatusInitialized();

    void GoToMainMenu(MainMenuArgs args);

    void StartNewGame(ImmutableArray<IConfig> configs, ImmutableArray<ModData> mods);

    void LoadGame(SaveFileInfo save, ImmutableArray<ModData>? thirdPartyModsToUse);

    void StartMapEditor(ImmutableArray<IConfig> configs, ImmutableArray<ModData> modTypes);

    void LoadMapEditor(string saveNameOrPath, ImmutableArray<ModData> extraModsToUse);

    void LoadMapEditor(
      MapEditorConfig config,
      IWorldRegionMap map,
      IWorldRegionMapPreviewData previewData,
      IWorldRegionMapAdditionalData additionalData,
      ImmutableArray<ModData> extraModsToUse);

    void QuitGame();
  }
}
