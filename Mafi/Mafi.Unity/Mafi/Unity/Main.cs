// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Main
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.SaveGame;
using Mafi.Core.Terrain.Generation;
using Mafi.Localization;
using Mafi.Serialization;
using Mafi.Unity.Audio;
using Mafi.Unity.MainMenu;
using Mafi.Unity.MapEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// Main class that handles flow of the game and loading of scenes.
  /// </summary>
  public class Main : IMain
  {
    public const int SCENE_INIT_STEP_MIN_DURATION_MS = 20;
    public const int SCENE_LOAD_MAX_STEPS_APPROX = 130;
    private readonly Event<DependencyResolver> m_newGameStarted;
    private readonly Event<DependencyResolver> m_gameTerminated;
    private readonly Mafi.Event m_onQuitRequested;
    private bool m_quitRequested;
    private readonly ImmutableArray<GameObject> m_persistentGos;
    private readonly AssetsDb m_assetsDb;
    private readonly AudioDb m_audioDb;
    private readonly BackgroundMusicManager m_musicManager;
    private readonly Action<LocStrFormatted> m_showFatalErrorAndTerminate;
    private readonly Stopwatch m_initStopwatch;
    private readonly Stopwatch m_startupStopwatch;
    private Option<LoadingScreen> m_loadingScreen;
    private Option<IEnumerator<string>> m_sceneInitialization;
    private Option<IEnumerator> m_sceneCleanupTask;
    private int m_sceneInitializationSteps;
    private bool m_didModsPreInit;
    private readonly string m_coreAssetsBundlePath;
    internal static string ThrowInModsInit;
    internal static string ThrowInSceneStart;
    internal static string ThrowInSceneInit;
    internal static string ThrowInMainMenuInit;

    public AssetsDb AssetsDb => this.m_assetsDb;

    public AudioDb AudioDb => this.m_audioDb;

    public BackgroundMusicManager BackgroundMusicManager => this.m_musicManager;

    public Option<IGameScene> CurrentScene { get; private set; }

    public IFileSystemHelper FileSystemHelper { get; private set; }

    public IEventNonSaveable<DependencyResolver> NewGameStarted
    {
      get => (IEventNonSaveable<DependencyResolver>) this.m_newGameStarted;
    }

    public IEventNonSaveable<DependencyResolver> GameTerminated
    {
      get => (IEventNonSaveable<DependencyResolver>) this.m_gameTerminated;
    }

    /// <summary>
    /// Called immediately after resolver is created in the new scene. The scene will not be initialized yet.
    /// </summary>
    public event Action<DependencyResolver> ResolverCreated;

    public IEventNonSaveable QuitRequested => (IEventNonSaveable) this.m_onQuitRequested;

    /// <summary>
    /// Note: If you need to know whether a mod fully succeeded to load, PreInitializeModsAndProtos
    /// must be called first!
    /// </summary>
    public ImmutableArray<ModData> Mods { get; }

    public ImmutableArray<ModInfoRaw> FailedMods { get; }

    public bool IsInitializing => this.m_sceneInitialization.HasValue;

    public Main(
      IFileSystemHelper fsHelper,
      AssetsDb assetsDb,
      ImmutableArray<ModData> allMods,
      ImmutableArray<ModInfoRaw> failedMods,
      ImmutableArray<GameObject> persistentGos,
      Action<LocStrFormatted> showFatalErrorAndTerminate)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_newGameStarted = new Event<DependencyResolver>(ThreadType.Any);
      this.m_gameTerminated = new Event<DependencyResolver>(ThreadType.Any);
      this.m_onQuitRequested = new Mafi.Event(ThreadType.Any);
      this.m_initStopwatch = new Stopwatch();
      this.m_startupStopwatch = new Stopwatch();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_showFatalErrorAndTerminate = showFatalErrorAndTerminate;
      this.FileSystemHelper = fsHelper.CheckNotNull<IFileSystemHelper>();
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
      this.Mods = allMods.CheckNotDefaultStruct<ImmutableArray<ModData>>();
      this.FailedMods = failedMods.CheckNotDefaultStruct<ImmutableArray<ModInfoRaw>>();
      this.m_persistentGos = persistentGos.CheckNotDefaultStruct<ImmutableArray<GameObject>>();
      this.m_coreAssetsBundlePath = allMods.FirstOrDefault((Func<ModData, bool>) (x => x.ModType == typeof (CoreMod))).AssetsPath.ValueOrNull;
      if (string.IsNullOrEmpty(this.m_coreAssetsBundlePath))
        throw new Exception("CoreMod must have valid asset bundles path.");
      string dirPath = fsHelper.GetDirPath(FileType.AssetsOverrides, false);
      if (Directory.Exists(dirPath))
        assetsDb.SetAssetOverrideRootPath(dirPath);
      foreach (ModData filterCoreAndDlcMod in allMods.FilterCoreAndDlcMods())
      {
        if (filterCoreAndDlcMod.AssetsPath.HasValue)
          this.registerAssetsDir(filterCoreAndDlcMod.AssetsPath.Value, this.m_coreAssetsBundlePath);
      }
      this.m_audioDb = new AudioDb(this.m_assetsDb);
      this.m_musicManager = new BackgroundMusicManager(this.m_audioDb);
    }

    private void registerAssetsDir(string assetsDirPath, string coreBundlesDirPath)
    {
      this.m_assetsDb.RegisterAssetBundlesIn(assetsDirPath, coreBundlesDirPath);
    }

    public void InitMods(ImmutableArray<IMod> mods)
    {
      foreach (IMod mod in mods)
      {
        System.Type modType = mod.GetType();
        ModData modData = this.Mods.FirstOrDefault((Func<ModData, bool>) (x => x.ModType == modType));
        if (modData.AssetsPath.HasValue)
          this.registerAssetsDir(modData.AssetsPath.Value, this.m_coreAssetsBundlePath);
      }
      if (((IEnumerable<string>) Environment.GetCommandLineArgs()).Contains<string>("--skip-assets-preload"))
        return;
      this.m_assetsDb.LoadAllRegisteredAssets();
    }

    public PreInitModsAndProtos PreInitializeModsAndProtos()
    {
      this.m_didModsPreInit = true;
      ProtosDb protosDb = new ProtosDb();
      return new PreInitModsAndProtos(GameBuilder.TryInstantiateModsAndProtos(this.Mods, new ProtoRegistrator(protosDb, ImmutableArray<IConfig>.Empty)), protosDb);
    }

    public void EnsureModsStatusInitialized()
    {
      if (this.m_didModsPreInit)
        return;
      this.PreInitializeModsAndProtos();
    }

    /// <summary>
    /// Terminates current scene and starts <see cref="T:Mafi.Unity.MainMenu.MainMenuScene" />.
    /// </summary>
    public void GoToMainMenu(MainMenuArgs args)
    {
      this.startScene((IGameScene) new MainMenuScene((IMain) this, args), true);
    }

    /// <summary>
    /// Terminates current scene and starts <see cref="T:Mafi.Unity.GamePlayScene" /> with given <see cref="T:Mafi.Core.Game.StartNewGameArgs" />.
    /// </summary>
    public void StartNewGame(ImmutableArray<IConfig> configs, ImmutableArray<ModData> mods)
    {
      this.startScene((IGameScene) new GamePlayScene((IMain) this, new StartNewGameArgs(mods, configs, this.FileSystemHelper)), true);
    }

    /// <summary>
    /// Terminates current scene and starts <see cref="T:Mafi.Unity.GamePlayScene" /> with given <see cref="T:Mafi.Core.Game.LoadGameArgs" />.
    /// </summary>
    public void LoadGame(SaveFileInfo save, ImmutableArray<ModData>? thirdPartyModsToUse = null)
    {
      this.startScene((IGameScene) new GamePlayScene((IMain) this, new LoadGameArgsFromFile(save, this.FileSystemHelper, thirdPartyModsToUse ?? ImmutableArray<ModData>.Empty)), true);
    }

    /// <summary>
    /// Terminates current scene and starts <see cref="T:Mafi.Unity.MapEditor.MapEditorScene" /> with given <see cref="T:Mafi.Core.Game.StartNewGameArgs" />.
    /// </summary>
    public void StartMapEditor(ImmutableArray<IConfig> configs, ImmutableArray<ModData> modTypes)
    {
      this.startScene((IGameScene) new MapEditorScene((IMain) this, new StartNewGameArgs(modTypes, configs, this.FileSystemHelper)), true);
    }

    public void LoadMapEditor(string saveNameOrPath, ImmutableArray<ModData> extraModsToUse)
    {
      this.startScene((IGameScene) new MapEditorScene((IMain) this, new LoadGameArgsFromMapFile(saveNameOrPath, this.FileSystemHelper, extraModsToUse)), true);
    }

    public void LoadMapEditor(
      MapEditorConfig config,
      IWorldRegionMap map,
      IWorldRegionMapPreviewData previewData,
      IWorldRegionMapAdditionalData additionalData,
      ImmutableArray<ModData> extraModsToUse)
    {
      this.startScene((IGameScene) new MapEditorScene((IMain) this, new LoadMapInstanceArgs(config, map, previewData, additionalData, this.FileSystemHelper, extraModsToUse)), true);
    }

    /// <summary>
    /// Sets <see cref="P:Mafi.Unity.Main.QuitRequested" /> which will cause termination by the owning code.
    /// </summary>
    public void QuitGame()
    {
      if (this.m_quitRequested)
        return;
      AudioListener.pause = true;
      this.m_quitRequested = true;
      Mafi.Log.Info("Requesting game quit.");
      this.m_onQuitRequested.Invoke();
    }

    /// <summary>
    /// Primary method for running of the game. Should be called every frame.
    /// </summary>
    public void Update()
    {
      if (this.m_sceneCleanupTask.HasValue)
      {
        if (this.m_sceneCleanupTask.Value.MoveNext())
        {
          Mafi.Log.Info("Scene clean step");
          return;
        }
        Mafi.Log.Info("Scene clean done");
        this.m_sceneCleanupTask = Option<IEnumerator>.None;
        Stopwatch stopwatch = Stopwatch.StartNew();
        long totalMemory1 = GC.GetTotalMemory(false);
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        long totalMemory2 = GC.GetTotalMemory(false);
        Mafi.Log.Info(string.Format("Total memory: {0} MB, GC collected ", (object) (totalMemory2 / 1024L / 1024L)) + string.Format("{0} MB in {1} ms.", (object) ((totalMemory1 - totalMemory2) / 1024L / 1024L), (object) stopwatch.Elapsed.TotalMilliseconds));
      }
      if (this.m_sceneInitialization.HasValue)
      {
        this.stepSceneInitialization();
      }
      else
      {
        Fix32 deltaMs = Fix32.FromFloat(Time.deltaTime * 1000f);
        if (deltaMs <= 0)
        {
          Mafi.Assert.That<Fix32>(deltaMs).IsNotNegative("Negative delta time.");
        }
        else
        {
          if (!this.CurrentScene.HasValue)
            return;
          this.CurrentScene.Value.Update(deltaMs);
        }
      }
    }

    private static LocStrFormatted getGameInitErrorMessageForException(
      ref Exception ex,
      out bool doNotOfferBugReport)
    {
      LocStrFormatted locStrFormatted = (LocStrFormatted) TrCore.GameInitFail;
      doNotOfferBugReport = false;
      Exception exception = ex;
      while (true)
      {
        switch (exception)
        {
          case null:
            goto label_7;
          case FatalGameException _:
            goto label_1;
          case OutOfMemoryException _:
            goto label_2;
          case MissingMethodException _:
            goto label_3;
          case CorruptedSaveException corruptedSaveException:
            goto label_4;
          default:
            exception = exception.InnerException;
            continue;
        }
      }
label_1:
      ex = (Exception) new FatalGameException(exception.Message);
      return (LocStrFormatted) TrCore.GameInitFail;
label_2:
      doNotOfferBugReport = true;
      return (LocStrFormatted) TrCore.GameInitFail__OutOrMemory;
label_3:
      locStrFormatted = (LocStrFormatted) TrCore.GameInitFail__Mod;
      goto label_7;
label_4:
      doNotOfferBugReport = corruptedSaveException.DoNotOfferBugReport;
      locStrFormatted = corruptedSaveException.MessageForPlayer ?? (LocStrFormatted) TrCore.GameInitFail;
label_7:
      return new LocStrFormatted(locStrFormatted.Value + "\n\n" + Mafi.Log.CleanStackTrace(ex.ToString()));
    }

    private void stepSceneInitialization()
    {
      try
      {
        float num = 20f;
        bool flag = true;
        string str = (string) null;
        while (this.m_sceneInitialization.Value.MoveNext())
        {
          string current = this.m_sceneInitialization.Value.Current;
          if (!string.IsNullOrEmpty(current) && current != str)
          {
            str = current;
            ++this.m_sceneInitializationSteps;
            float totalMilliseconds = (float) this.m_initStopwatch.Elapsed.TotalMilliseconds;
            this.m_initStopwatch.Restart();
            num -= totalMilliseconds;
            Mafi.Log.Info("[" + totalMilliseconds.RoundToSigDigits(3, false, false, false) + " ms] " + current);
            if (this.m_loadingScreen.HasValue)
              this.m_loadingScreen.Value.SetProgress(Percent.FromRatio(this.m_sceneInitializationSteps, 130));
          }
          if ((double) num - (double) this.m_initStopwatch.ElapsedMilliseconds < 0.0)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
          return;
        this.m_sceneInitialization = Option<IEnumerator<string>>.None;
        if (this.CurrentScene.HasValue)
          this.CurrentScene.Value.ResolverCreated -= new Action<DependencyResolver>(this.onResolverCreated);
        this.hideLoadingScreen();
        this.m_startupStopwatch.Stop();
        float totalSeconds = (float) this.m_startupStopwatch.Elapsed.TotalSeconds;
        Mafi.Log.Info(string.Format("Game was initialized in {0} steps and took ", (object) this.m_sceneInitializationSteps) + totalSeconds.RoundToSigDigits(3, false, false, false) + " seconds to complete.");
        if (!(this.CurrentScene.Value is GamePlayScene gamePlayScene))
          return;
        this.m_newGameStarted.Invoke(gamePlayScene.GetResolver());
        LocalizationManager.IgnoreDuplicates();
      }
      catch (Exception ex)
      {
        this.m_sceneInitialization = Option<IEnumerator<string>>.None;
        Option<IGameScene> currentScene = this.CurrentScene;
        if (currentScene.HasValue)
          this.CurrentScene.Value.ResolverCreated -= new Action<DependencyResolver>(this.onResolverCreated);
        LocalizationManager.IgnoreDuplicates();
        bool doNotOfferBugReport;
        LocStrFormatted messageForException = Main.getGameInitErrorMessageForException(ref ex, out doNotOfferBugReport);
        Mafi.Log.Exception(ex, ex.GetType().Name + " thrown during game initialization.");
        currentScene = this.CurrentScene;
        if (currentScene.ValueOrNull is MainMenuScene)
          this.m_showFatalErrorAndTerminate(messageForException);
        else
          this.GoToMainMenu(new MainMenuArgs(new LocStrFormatted?(messageForException), doNotOfferBugReport));
      }
    }

    private void onResolverCreated(DependencyResolver resolver)
    {
      Action<DependencyResolver> resolverCreated = this.ResolverCreated;
      if (resolverCreated == null)
        return;
      resolverCreated(resolver);
    }

    private IEnumerator<string> unloadAndStartNewScene(IGameScene scene, bool displayLoadingScreen)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new Main.\u003CunloadAndStartNewScene\u003Ed__70(0)
      {
        \u003C\u003E4__this = this,
        scene = scene,
        displayLoadingScreen = displayLoadingScreen
      };
    }

    /// <summary>
    /// Terminates current scene and starts new scene. Optionally displays loading screen.
    /// </summary>
    private void startScene(IGameScene scene, bool displayLoadingScreen)
    {
      try
      {
        Mafi.Assert.That<Option<IEnumerator<string>>>(this.m_sceneInitialization).IsNone<IEnumerator<string>>();
        this.m_sceneInitialization = this.unloadAndStartNewScene(scene, displayLoadingScreen).SomeOption<IEnumerator<string>>();
      }
      catch (Exception ex)
      {
        bool doNotOfferBugReport;
        LocStrFormatted messageForException = Main.getGameInitErrorMessageForException(ref ex, out doNotOfferBugReport);
        Mafi.Log.Exception(ex, ex.GetType().Name + " thrown during scene start.");
        this.CurrentScene = Option<IGameScene>.None;
        this.m_sceneInitialization = Option<IEnumerator<string>>.None;
        scene.ResolverCreated -= new Action<DependencyResolver>(this.onResolverCreated);
        if (scene is MainMenuScene)
          this.m_showFatalErrorAndTerminate(messageForException);
        else
          this.GoToMainMenu(new MainMenuArgs(new LocStrFormatted?(messageForException), doNotOfferBugReport));
      }
    }

    private void terminateCurrentScene()
    {
      if (this.CurrentScene.IsNone)
        return;
      if (this.CurrentScene.Value is GamePlayScene gamePlayScene && gamePlayScene.IsInitialized)
        this.m_gameTerminated.Invoke(gamePlayScene.GetResolver());
      this.CurrentScene.ValueOrNull?.Terminate();
      this.CurrentScene = Option<IGameScene>.None;
    }

    private static IEnumerator cleanUnusedAssets()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Main.\u003CcleanUnusedAssets\u003Ed__73(0);
    }

    /// <summary>
    /// Removes all non-persistent game objects from current scene.
    /// </summary>
    private void cleanupRootScene()
    {
      int num = 0;
      foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
      {
        if (!this.m_persistentGos.Contains(rootGameObject) && !((UnityEngine.Object) this.m_loadingScreen.ValueOrNull?.RootGo == (UnityEngine.Object) rootGameObject) && !rootGameObject.HasTag(UnityTag.Persistent))
        {
          if (rootGameObject.IsNullOrDestroyed())
          {
            Mafi.Log.Warning("Already destroyed object in the scene: " + rootGameObject.name);
          }
          else
          {
            rootGameObject.Destroy();
            ++num;
          }
        }
      }
      this.m_assetsDb.ClearCachedAssets();
    }

    private void showLoadingScreen()
    {
      if (this.m_loadingScreen.HasValue)
        this.hideLoadingScreen();
      this.m_loadingScreen = (Option<LoadingScreen>) new LoadingScreen(this.m_assetsDb);
    }

    private void hideLoadingScreen()
    {
      if (this.m_loadingScreen.IsNone)
        return;
      this.m_loadingScreen.Value.Destroy();
      this.m_loadingScreen = Option<LoadingScreen>.None;
    }

    public void OnProjectChanged()
    {
      if (!this.CurrentScene.HasValue)
        return;
      this.CurrentScene.Value.OnProjectChanged();
    }
  }
}
