// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.MapEditor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain;
using Mafi.Base.Terrain.FeatureGenerators;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Game;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.SaveGame;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Logging;
using Mafi.Numerics;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.ResVis;
using Mafi.Unity.MapEditor.Previews;
using Mafi.Unity.MapEditor.Templates;
using Mafi.Unity.Prototypes;
using Mafi.Unity.Terrain;
using Mafi.Unity.UiToolkit.Library.ObjectEditor;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using Mafi.Unity.Vehicles;
using Mafi.Unity.Weather;
using RTG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class MapEditor : IRegisterInMapEditor, IUnityInputController, IMapEditorInfo, IDisposable
  {
    public static readonly Vector2i MAP_THUMBNAIL_SIZE;
    public static readonly Vector2i MAP_PREVIEW_SIZE;
    private WorldRegionMap m_map;
    private WorldRegionMapPreviewData m_previewData;
    private WorldRegionMapAdditionalData m_additionalData;
    private readonly IUnityInputMgr m_inputManager;
    public readonly TerrainManager TerrainManager;
    private readonly VirtualResourceManager m_virtualResourceManager;
    private readonly StaticEntityOceanReservationManager m_staticEntityOceanReservationManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly DependencyResolver m_resolver;
    private readonly TreesManager m_treeManager;
    public readonly ManualDetailPreviewController DetailPreviewController;
    public readonly EntityTesterPreviewController EntityTesterPreviewController;
    private readonly UiBuilder m_uiBuilder;
    private readonly CursorPickingManager m_cursorPickingManager;
    public readonly AssetsDb AssetsDb;
    private readonly ProtosDb m_protosDb;
    private readonly ITerrainGeneratorV2 m_terrainGenerator;
    private readonly LinesFactory m_linesFactory;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly IFileSystemHelper m_fsHelper;
    private readonly TerrainCursor m_terrainCursor;
    private readonly ResVisBarsRenderer.Activator m_resVizRendererActivator;
    private readonly Dict<GameObject, TerrainFeatureEditor> m_featureEditorHandles;
    private readonly Dict<IEditableTerrainFeature, GameObject> m_featureToHandle;
    private readonly Dict<IEditableTerrainFeature, TerrainFeatureEditor> m_featureEditors;
    private readonly Lyst<ITerrainFeatureWithSimUpdate> m_updatableFeatures;
    private readonly Lyst<ITerrainFeatureWithSimUpdate> m_updatableFeaturesOnSim;
    private bool m_updatableFeaturesOnSimNeedUpdate;
    private Option<TerrainFeatureEditor> m_selectedFeatureEditor;
    private bool m_suppressPreviewsAndHandles;
    public bool m_suppressPreviews;
    private Option<IEnumerator<RegenProgress>> m_regenerationTaskEnumerator;
    private Option<IEnumerator<RegenProgress>> m_mapPublishingEnumerator;
    private readonly Lyst<Chunk2i> m_chunksToUpdateRenderer;
    private readonly Area2iRenderer m_regeneratedAreaRenderer;
    private readonly Stopwatch m_stopwatch;
    private volatile bool m_regenerationCompletedOnSim;
    private volatile bool m_mapPublishCompletedOnSim;
    private int m_nextFeatureId;
    private readonly UndoRedoManager<Mafi.Unity.MapEditor.MapEditor.UndoState> m_undoManager;
    private readonly AreaSelectionToolV2 m_areaSelection;
    private ObjEditor m_editorSettingsEditor;
    private ObjEditor m_mapLoadConfigEditor;
    private ObjEditor m_mapSizeConfigEditor;
    private ObjEditor m_terrainFeaturesConfigEditor;
    private ObjEditor m_postProcessorsConfigEditor;
    private ObjEditor m_virtualResourcesConfigEditor;
    private ObjEditor m_startingLocationsConfigEditor;
    private ObjEditor m_mapPublishConfigEditor;
    private ObjEditor m_selectedFeatureObjectEditor;
    private ObjEditor m_historyObjectEditor;
    private ObjEditor m_noiseParserHelpViewEditor;
    private ObjEditor m_terrainLayersViewEditor;
    private MapEditorToolbar m_toolbar;
    private readonly Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig m_toolbarConfig;
    private MapEditorScreen m_editorsScreen;
    private ObjEditorsRegistry m_editorsRegistry;
    private readonly Mafi.Unity.MapEditor.MapEditor.MapEditorSettings m_editorSettings;
    private readonly Mafi.Unity.MapEditor.MapEditor.MapLoadConfig m_mapLoadConfig;
    private readonly Mafi.Unity.MapEditor.MapEditor.MapSizeConfig m_mapSizeConfig;
    private readonly Mafi.Unity.MapEditor.MapEditor.TerrainFeaturesConfig m_terrainFeaturesConfig;
    private readonly Mafi.Unity.MapEditor.MapEditor.PostProcessorsConfig m_postProcessorsConfig;
    private readonly Mafi.Unity.MapEditor.MapEditor.VirtualResourcesConfig m_virtualResourcesConfig;
    private readonly Mafi.Unity.MapEditor.MapEditor.StartingLocationsConfig m_startingLocationsConfig;
    private readonly Mafi.Unity.MapEditor.MapEditor.MapPublishConfig m_mapPublishConfig;
    private readonly Mafi.Unity.MapEditor.MapEditor.HistoryView m_historyView;
    private readonly Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView m_noiseParserHelpView;
    private readonly Mafi.Unity.MapEditor.MapEditor.TerrainLayersView m_terrainLayersView;
    private readonly IActivator m_gridLinesActivator;
    private readonly MapSerializer m_mapSerializer;
    private readonly ASyncSaver m_asyncSaver;
    private readonly IMain m_main;
    public readonly CameraController CameraController;
    private readonly WaterRendererManager m_waterRenderer;
    private readonly ClearancePathabilityProvider m_clearancePathabilityProvider;
    private readonly FogController m_fogController;
    private readonly VehiclesPathabilityOverlayRenderer m_navOverlayRenderer;
    private readonly SerializationUtils m_serializationUtils;
    private readonly ScreenshotTaker m_screenshotTaker;
    private bool m_applyChanges;
    private Option<string> m_nextUndoStateName;
    private RectangleTerrainArea2i? m_nextUndoAffectedArea;
    private LystStruct<ITerrainFeatureWithSimUpdate> m_notifyFeatureUpdatedOnSim;
    private LystStruct<Chunk2iSlim> m_navOverlayChunksToProcess;
    private Tile2i m_navOverlaySortOrigin;
    private Mafi.Unity.MapEditor.MapEditor.OverlayMode m_currentOverlayMode;
    private RegenProgress m_regenTaskProgressOnSim;
    private bool m_resetNavOverlayOnSim;
    private readonly Stopwatch m_autosaveStopwatch;
    private readonly ImmutableArray<ITerrainFeatureTemplate> m_allFeatureTemplates;
    private readonly IRandom m_featureTemplatesRng;
    private bool m_cancelRegeneration;
    private LystStruct<string> m_mapPublishErrorsOnSim;
    private LystStruct<LogEntry> m_logsDuringPublish;

    public ControllerConfig Config { get; }

    public bool IsEditorBusy
    {
      get => this.m_regenerationTaskEnumerator.HasValue || this.m_mapPublishingEnumerator.HasValue;
    }

    public IResolver Resolver => (IResolver) this.m_resolver;

    public bool SuppressPreviewsAndHandles
    {
      get => this.m_suppressPreviewsAndHandles;
      set
      {
        this.m_suppressPreviewsAndHandles = value;
        this.applySuppressions();
      }
    }

    public bool SuppressPreviews
    {
      get => this.m_suppressPreviews;
      set
      {
        this.m_suppressPreviews = value;
        this.applySuppressions();
      }
    }

    public bool SuppressHandles { get; set; }

    public bool ArePreviewsSuppressed
    {
      get => this.m_suppressPreviews || this.m_suppressPreviewsAndHandles;
    }

    public bool AreHandlesSuppressed => this.SuppressHandles || this.m_suppressPreviewsAndHandles;

    public bool HasUnsavedChanges => this.m_toolbarConfig.HasUnsavedChanges;

    private int getNextFeatureId() => this.m_nextFeatureId++;

    internal UndoRedoManager<Mafi.Unity.MapEditor.MapEditor.UndoState> UndoManager
    {
      get => this.m_undoManager;
    }

    public Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig ToolbarConfig => this.m_toolbarConfig;

    public MapEditorScreen EditorScreen => this.m_editorsScreen;

    public MapEditor(
      MapEditorConfig editorConfig,
      IUnityInputMgr inputManager,
      IWorldRegionMap worldRegionMap,
      TerrainManager terrainManager,
      VirtualResourceManager virtualResourceManager,
      StaticEntityOceanReservationManager staticEntityOceanReservationManager,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      DependencyResolver resolver,
      ResVisBarsRenderer resVizRenderer,
      TreesManager treeManager,
      ManualDetailPreviewController detailPreviewController,
      EntityTesterPreviewController entityTesterPreviewController,
      UiBuilder uiBuilder,
      CursorPickingManager cursorPickingManager,
      AssetsDb assetsDb,
      ProtosDb protosDb,
      ITerrainGeneratorV2 terrainGenerator,
      LinesFactory linesFactory,
      NewInstanceOf<AreaSelectionToolV2> areaSelectionTool,
      ShortcutsManager shortcutsManager,
      TerrainRenderer terrainRenderer,
      NewInstanceOf<TerrainCursor> terrainCursor,
      SpecialSerializerFactories specialSerializers,
      IFileSystemHelper fsHelper,
      IWorldRegionMapPreviewData previewData,
      IWorldRegionMapAdditionalData additionalData,
      IMain main,
      CameraController cameraController,
      WaterRendererManager waterRenderer,
      ClearancePathabilityProvider clearancePathabilityProvider,
      FogController fogController,
      VehiclesPathabilityOverlayRenderer navOverlayRenderer,
      SerializationUtils serializationUtils,
      ScreenshotTaker screenshotTaker,
      AllImplementationsOf<ITerrainFeatureTemplateFactory> templateFactories,
      ConfigurableNoise2dParser parser,
      RandomProvider randomProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Config = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        DeactivateOnOtherControllerActive = false,
        IgnoreEscapeKey = true,
        PreventSpeedControl = true,
        Group = ControllerGroup.AlwaysActive
      };
      this.m_featureEditorHandles = new Dict<GameObject, TerrainFeatureEditor>();
      this.m_featureToHandle = new Dict<IEditableTerrainFeature, GameObject>();
      this.m_featureEditors = new Dict<IEditableTerrainFeature, TerrainFeatureEditor>();
      this.m_updatableFeatures = new Lyst<ITerrainFeatureWithSimUpdate>();
      this.m_updatableFeaturesOnSim = new Lyst<ITerrainFeatureWithSimUpdate>();
      this.m_chunksToUpdateRenderer = new Lyst<Chunk2i>();
      this.m_stopwatch = new Stopwatch();
      this.m_nextFeatureId = 1;
      this.m_autosaveStopwatch = Stopwatch.StartNew();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      RTGApp.MafiInitialize(assetsDb, cameraController);
      this.m_inputManager = inputManager;
      this.TerrainManager = terrainManager;
      this.m_virtualResourceManager = virtualResourceManager;
      this.m_staticEntityOceanReservationManager = staticEntityOceanReservationManager;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_resolver = resolver;
      this.m_resVizRendererActivator = resVizRenderer.CreateActivator();
      this.m_treeManager = treeManager;
      this.DetailPreviewController = detailPreviewController;
      this.EntityTesterPreviewController = entityTesterPreviewController;
      this.m_uiBuilder = uiBuilder;
      this.m_cursorPickingManager = cursorPickingManager;
      this.AssetsDb = assetsDb;
      this.m_protosDb = protosDb;
      this.m_terrainGenerator = terrainGenerator;
      this.m_linesFactory = linesFactory;
      this.m_terrainRenderer = terrainRenderer;
      this.m_fsHelper = fsHelper;
      this.m_main = main;
      this.CameraController = cameraController;
      this.m_waterRenderer = waterRenderer;
      this.m_clearancePathabilityProvider = clearancePathabilityProvider;
      this.m_fogController = fogController;
      this.m_navOverlayRenderer = navOverlayRenderer;
      this.m_serializationUtils = serializationUtils;
      this.m_screenshotTaker = screenshotTaker;
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_featureTemplatesRng = randomProvider.GetNonSimRandomFor((object) this, "Feature templates seed");
      shortcutsManager.SetShortcutsMode(ShortcutMode.MapEditor);
      terrainGenerator.SetPerfDataCollection(true);
      this.m_areaSelection = areaSelectionTool.Instance;
      this.m_areaSelection.SetWidth(0.5f);
      this.m_areaSelection.SetPrimaryBinding(shortcutsManager.PrimaryAction, shortcutsManager.SecondaryAction, Color.yellow);
      this.m_areaSelection.SelectionCompleted += new AreaSelectionToolV2.AreaSelectionDelegate(this.onAreaSelectionDone);
      if (worldRegionMap is WorldRegionMap worldRegionMap1)
      {
        this.m_map = worldRegionMap1;
      }
      else
      {
        this.m_map = new WorldRegionMap(worldRegionMap.Size, worldRegionMap.BedrockMaterial);
        Mafi.Log.Error("TODO: Convert map to WorldRegionMap");
      }
      if (previewData is WorldRegionMapPreviewData regionMapPreviewData)
      {
        this.m_previewData = regionMapPreviewData;
      }
      else
      {
        this.m_previewData = new WorldRegionMapPreviewData();
        Mafi.Log.Error("TODO: Convert preview data to WorldRegionMapPreviewData");
      }
      if (additionalData is WorldRegionMapAdditionalData mapAdditionalData)
      {
        this.m_additionalData = mapAdditionalData;
      }
      else
      {
        this.m_additionalData = new WorldRegionMapAdditionalData();
        Mafi.Log.Error("TODO: Convert additional data to WorldRegionMapAdditionalData");
      }
      if (this.m_previewData.CreatedInSaveVersion < 157)
      {
        int? nullable = this.m_map.TerrainPostProcessors.FirstIndexOf<ITerrainPostProcessorV2>((Predicate<ITerrainPostProcessorV2>) (x => x is TerrainPropsPostProcessor));
        if (nullable.HasValue)
          this.m_map.TerrainPostProcessorsList[nullable.Value] = (ITerrainPostProcessorV2) new PropsPostProcessorTemplate(protosDb).CreateNewFeature(this.m_featureTemplatesRng);
        Mafi.Log.Info("Old props replaced (save version " + string.Format("{0} => {1}).", (object) this.m_previewData.CreatedInSaveVersion, (object) 157));
        this.m_previewData.CreatedInSaveVersion = 157;
      }
      if (this.m_previewData.CreatedInSaveVersion < 158)
      {
        int? nullable = this.m_map.TerrainPostProcessors.FirstIndexOf<ITerrainPostProcessorV2>((Predicate<ITerrainPostProcessorV2>) (x => x is FlowersPostProcessor));
        if (nullable.HasValue && ((FlowersPostProcessor) this.m_map.TerrainPostProcessorsList[nullable.Value]).ConfigMutable.FlowersConfigs.Count == 4)
          this.m_map.TerrainPostProcessorsList[nullable.Value] = (ITerrainPostProcessorV2) new FlowersPostProcessorTemplate(protosDb).CreateNewFeature(this.m_featureTemplatesRng);
        Mafi.Log.Info("Old flowers replaced (save version " + string.Format("{0} => {1}).", (object) this.m_previewData.CreatedInSaveVersion, (object) 158));
        this.m_previewData.CreatedInSaveVersion = 158;
      }
      this.m_regeneratedAreaRenderer = new Area2iRenderer(linesFactory);
      this.m_regeneratedAreaRenderer.SetColor(Color.yellow);
      this.m_regeneratedAreaRenderer.SetWidth(3f);
      this.m_regeneratedAreaRenderer.Hide();
      this.m_mapSerializer = new MapSerializer(specialSerializers.SpecialSerializersForGame);
      this.m_asyncSaver = new ASyncSaver();
      foreach (ITerrainFeatureBase enumerateAllFeature in this.m_map.EnumerateAllFeatures())
        enumerateAllFeature.Id = this.getNextFeatureId();
      this.m_allFeatureTemplates = templateFactories.Implementations.AsEnumerable().SelectMany<ITerrainFeatureTemplateFactory, ITerrainFeatureTemplate>((Func<ITerrainFeatureTemplateFactory, IEnumerable<ITerrainFeatureTemplate>>) (x => x.GetTemplates())).ToImmutableArray<ITerrainFeatureTemplate>();
      this.m_undoManager = new UndoRedoManager<Mafi.Unity.MapEditor.MapEditor.UndoState>(100, new Mafi.Unity.MapEditor.MapEditor.UndoState("Initial state", new RectangleTerrainArea2i?(), serializationUtils.Serialize<WorldRegionMap>(this.m_map)));
      this.m_undoManager.OnHistoryChanged += new Action(this.onHistoryChanged);
      this.m_editorSettings = new Mafi.Unity.MapEditor.MapEditor.MapEditorSettings();
      this.m_mapLoadConfig = new Mafi.Unity.MapEditor.MapEditor.MapLoadConfig(this);
      this.m_mapLoadConfig.SelectMap(editorConfig.MapFileName);
      this.m_mapSizeConfig = new Mafi.Unity.MapEditor.MapEditor.MapSizeConfig(this, (IWorldRegionMap) this.m_map);
      this.m_terrainFeaturesConfig = new Mafi.Unity.MapEditor.MapEditor.TerrainFeaturesConfig(this, this.m_allFeatureTemplates.Where((Func<ITerrainFeatureTemplate, bool>) (x => x.FeatureType.IsAssignableTo<ITerrainFeatureGenerator>())).ToImmutableArray<ITerrainFeatureTemplate>());
      this.m_postProcessorsConfig = new Mafi.Unity.MapEditor.MapEditor.PostProcessorsConfig(this, this.m_allFeatureTemplates.Where((Func<ITerrainFeatureTemplate, bool>) (x => x.FeatureType.IsAssignableTo<ITerrainPostProcessorV2>())).ToImmutableArray<ITerrainFeatureTemplate>());
      this.m_virtualResourcesConfig = new Mafi.Unity.MapEditor.MapEditor.VirtualResourcesConfig(this, this.m_allFeatureTemplates.Where((Func<ITerrainFeatureTemplate, bool>) (x => x.FeatureType.IsAssignableTo<IVirtualTerrainResourceGenerator>())).ToImmutableArray<ITerrainFeatureTemplate>());
      this.m_startingLocationsConfig = new Mafi.Unity.MapEditor.MapEditor.StartingLocationsConfig(this);
      this.m_mapPublishConfig = new Mafi.Unity.MapEditor.MapEditor.MapPublishConfig(this, this.getUniqueMapName(this.m_previewData.FilePath));
      this.m_toolbarConfig = new Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig(this);
      this.m_toolbarConfig.HasUnsavedChanges = editorConfig.IsLoadedMapUnsaved;
      this.m_toolbarConfig.MapNameWip = this.m_mapPublishConfig.MapFileName + ".wip.map";
      this.m_historyView = new Mafi.Unity.MapEditor.MapEditor.HistoryView(new Action(this.Undo), new Action(this.Redo));
      this.m_historyView.UpdateValues(this.m_undoManager);
      this.m_noiseParserHelpView = new Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView(parser);
      this.m_terrainLayersView = new Mafi.Unity.MapEditor.MapEditor.TerrainLayersView(this);
      this.m_gridLinesActivator = terrainRenderer.CreateGridLinesActivator();
      gameLoopEvents.RegisterRendererInitState((object) this, this.init());
      gameLoopEvents.SyncUpdate.AddNonSaveable<Mafi.Unity.MapEditor.MapEditor>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<Mafi.Unity.MapEditor.MapEditor>(this, new Action<GameTime>(this.renderUpdate));
      gameLoopEvents.RenderUpdateEnd.AddNonSaveable<Mafi.Unity.MapEditor.MapEditor>(this, new Action<GameTime>(this.renderUpdateEnd));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<Mafi.Unity.MapEditor.MapEditor>(this, new Action(this.simUpdate));
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void Dispose()
    {
      Mafi.Assert.AssertFired -= new Action<LogEntry>(this.logReceived);
      Mafi.Log.LogReceived -= new Action<LogEntry>(this.logReceived);
      RTGApp.MafiTerminate();
    }

    private void logReceived(LogEntry entry)
    {
      if ((entry.Type & (Mafi.Logging.LogType.Exception | Mafi.Logging.LogType.Error | Mafi.Logging.LogType.Assert)) == (Mafi.Logging.LogType) 0)
        return;
      this.m_toolbar.LogErrorSilentlyAndThreadSafe(entry.Message, entry.ToString(-1, omitMessage: true));
    }

    private IEnumerator<string> init()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new Mafi.Unity.MapEditor.MapEditor.\u003Cinit\u003Ed__150(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private void openMapsDirectoryInExplorer()
    {
      Application.OpenURL("file://" + this.m_fsHelper.GetDirPath(FileType.Map, true));
    }

    private string getUniqueMapName(Option<string> mapPath)
    {
      string fileName = (string) null;
      if (mapPath.HasValue)
        fileName = Path.GetFileName(mapPath.Value).RemoveSuffixIfExists(".map").RemoveSuffixIfExists(".autosave").RemoveSuffixIfExists(".wip");
      if (fileName == null)
      {
        string str = DateTime.Now.ToString("yyyy-MM-dd");
        fileName = "New map " + str;
        for (int index = 1; index < 1000; ++index)
        {
          string filePath1 = this.m_fsHelper.GetFilePath(fileName, FileType.Map);
          string filePath2 = this.m_fsHelper.GetFilePath(fileName + ".wip", FileType.Map);
          string filePath3 = this.m_fsHelper.GetFilePath(fileName + ".autosave", FileType.Map);
          if (File.Exists(filePath1) || File.Exists(filePath2) || File.Exists(filePath3))
            fileName = string.Format("New map {0} ({1})", (object) str, (object) index);
          else
            break;
        }
      }
      return fileName;
    }

    private void saveMapAsWip()
    {
      string mapFileName = this.m_mapPublishConfig.MapFileName;
      if (string.IsNullOrEmpty(mapFileName))
      {
        this.m_toolbarConfig.SetSaveStatus("Error: Invalid map name. Set it in the Publishing tab.");
      }
      else
      {
        string fileName = mapFileName + ".wip";
        string error;
        this.m_toolbarConfig.SetSaveStatus(this.trySaveMapAsWip(this.m_fsHelper.GetFilePath(fileName, FileType.Map), out error) ? "Saved as '" + fileName + ".map'" : "Error: " + error);
      }
    }

    private void autoSaveMap()
    {
      string mapFileName = this.m_mapPublishConfig.MapFileName;
      if (string.IsNullOrEmpty(mapFileName))
      {
        this.m_mapLoadConfig.SetAutosaveStatus("Error: Invalid map name. Set it in the Publishing tab.");
      }
      else
      {
        string fileName = mapFileName + ".autosave";
        string error;
        this.m_mapLoadConfig.SetAutosaveStatus(this.trySaveMapAsWip(this.m_fsHelper.GetFilePath(fileName, FileType.Map), out error) ? string.Format("Saved as '{0}{1}' at {2:T}", (object) fileName, (object) ".map", (object) DateTime.Now) : "Auto-save error: " + error);
      }
    }

    private bool trySaveMapAsWip(string mapFilePath, out string error)
    {
      this.updateMapPreviewData();
      this.m_previewData.IsPublished = false;
      try
      {
        this.m_mapSerializer.StartSave((IWorldRegionMap) this.m_map, (IWorldRegionMapPreviewData) this.m_previewData, (IWorldRegionMapAdditionalData) this.m_additionalData);
        this.m_asyncSaver.RunInSyncAndReset((IAsyncSavable) this.m_mapSerializer, mapFilePath);
        ref string local1 = ref error;
        ref LocStrFormatted? local2 = ref this.m_asyncSaver.LastSaveError;
        string str = local2.HasValue ? local2.GetValueOrDefault().Value : (string) null;
        local1 = str;
      }
      catch (Exception ex)
      {
        error = ex.ToString();
      }
      if (!error.IsNullOrEmpty())
      {
        this.m_toolbar.ShowErrorNotification("Save failed", error.ToString());
        return false;
      }
      this.m_toolbar.ShowNotification("Saved!");
      this.m_toolbarConfig.HasUnsavedChanges = false;
      error = "";
      return true;
    }

    private void updateMapPreviewData()
    {
      this.m_previewData.CreatedInGameVersion = "0.6.3a";
      this.m_previewData.CreatedInSaveVersion = 168;
      this.m_previewData.LastEditedDateTimeUtc = DateTime.UtcNow;
      this.m_previewData.MapSize = this.TerrainManager.TerrainSize;
      WorldRegionMapPreviewData previewData = this.m_previewData;
      StartingLocationV2 startingLocationV2 = (StartingLocationV2) this.m_map.StartingLocations.FirstOrDefault<IStartingLocationV2>();
      int num = startingLocationV2 != null ? (int) startingLocationV2.ConfigMutable.Difficulty : 1;
      previewData.Difficulty = (StartingLocationDifficulty) num;
      IStartingLocationV2[] array = this.m_map.StartingLocationsList.OrderBy<IStartingLocationV2, int>((Func<IStartingLocationV2, int>) (x => x.Order)).ToArray<IStartingLocationV2>();
      this.m_map.StartingLocationsList.Clear();
      this.m_map.StartingLocationsList.AddRange(array);
      this.m_additionalData.StartingLocations = this.m_map.StartingLocationsList.Where<IStartingLocationV2>((Func<IStartingLocationV2, bool>) (x => !x.IsDisabled)).Select<IStartingLocationV2, StartingLocationPreview>((Func<IStartingLocationV2, StartingLocationPreview>) (s => s.ToPreview())).ToImmutableArray<StartingLocationPreview>();
    }

    private void setNextUndoStateName(string name, RectangleTerrainArea2i? affectedArea)
    {
      this.m_nextUndoStateName = (Option<string>) name;
      this.m_nextUndoAffectedArea = affectedArea;
    }

    private void pushAlreadyNamedUndoSnapshot()
    {
      this.pushNewUndoSnapshot("EXPECT_ALREADY_SET", new RectangleTerrainArea2i?());
    }

    private void pushNewUndoSnapshot(string name, RectangleTerrainArea2i? affectedArea)
    {
      if (this.m_nextUndoStateName != "DO_NOT_SAVE")
      {
        RectangleTerrainArea2i valueOrDefault;
        if (this.m_nextUndoAffectedArea.HasValue)
        {
          valueOrDefault = this.m_nextUndoAffectedArea.Value;
          if (!valueOrDefault.IsEmpty)
          {
            ref RectangleTerrainArea2i? local = ref affectedArea;
            RectangleTerrainArea2i rectangleTerrainArea2i;
            if (!affectedArea.HasValue)
            {
              rectangleTerrainArea2i = this.m_nextUndoAffectedArea.Value;
            }
            else
            {
              valueOrDefault = affectedArea.GetValueOrDefault();
              rectangleTerrainArea2i = valueOrDefault.Union(this.m_nextUndoAffectedArea.Value);
            }
            local = new RectangleTerrainArea2i?(rectangleTerrainArea2i);
          }
        }
        if (this.m_nextUndoStateName.HasValue)
          name = this.m_nextUndoStateName.Value;
        else if (name == "EXPECT_ALREADY_SET")
          Mafi.Log.Error("Expected already set undo name but it was not.");
        UndoRedoManager<Mafi.Unity.MapEditor.MapEditor.UndoState> undoManager = this.m_undoManager;
        string name1 = name;
        RectangleTerrainArea2i? affectedArea1;
        if (affectedArea.HasValue)
        {
          valueOrDefault = affectedArea.Value;
          if (!valueOrDefault.IsEmpty)
          {
            affectedArea1 = affectedArea;
            goto label_15;
          }
        }
        affectedArea1 = new RectangleTerrainArea2i?();
label_15:
        byte[] mapData = this.m_serializationUtils.Serialize<WorldRegionMap>(this.m_map);
        Mafi.Unity.MapEditor.MapEditor.UndoState state = new Mafi.Unity.MapEditor.MapEditor.UndoState(name1, affectedArea1, mapData);
        undoManager.PushUndoState(state);
        if (this.m_autosaveStopwatch.Elapsed > new TimeSpan(0, this.m_mapLoadConfig.AutosaveIntervalMinutes, 0))
        {
          this.m_autosaveStopwatch.Restart();
          this.autoSaveMap();
        }
      }
      this.includeInAreaForRegeneration(affectedArea);
      this.m_nextUndoStateName = Option<string>.None;
      this.m_nextUndoAffectedArea = new RectangleTerrainArea2i?();
    }

    public void Undo()
    {
      Mafi.Unity.MapEditor.MapEditor.UndoState currentState = this.m_undoManager.CurrentState;
      this.m_undoManager.Undo(1);
      this.updateState(currentState, this.m_undoManager.CurrentState);
      this.includeInAreaForRegeneration(currentState.AffectedArea);
    }

    public void Redo()
    {
      Mafi.Unity.MapEditor.MapEditor.UndoState currentState = this.m_undoManager.CurrentState;
      this.m_undoManager.Redo(1);
      this.updateState(currentState, this.m_undoManager.CurrentState);
      this.includeInAreaForRegeneration(this.m_undoManager.CurrentState.AffectedArea);
    }

    public void ApplyChangesNow() => this.m_applyChanges = true;

    public void RegenerateEntireMap()
    {
      this.m_toolbarConfig.AreaToRegenerate = new RectangleTerrainArea2i?(this.TerrainManager.TerrainArea);
      this.m_applyChanges = true;
    }

    public void StopRegeneration() => this.m_cancelRegeneration = true;

    public void AddTemplate(ITerrainFeatureTemplate template)
    {
      if (!this.tryAddAndSelectNewFeatureFromTemplate(template.SomeOption<ITerrainFeatureTemplate>()))
        return;
      this.pushAlreadyNamedUndoSnapshot();
    }

    private void updateState(Mafi.Unity.MapEditor.MapEditor.UndoState oldState, Mafi.Unity.MapEditor.MapEditor.UndoState newState)
    {
      if (oldState.MapData == newState.MapData)
        return;
      this.setNewMap(this.m_serializationUtils.Deserialize<WorldRegionMap>(newState.MapData));
    }

    private void setNewMap(WorldRegionMap map)
    {
      int selectedFeatureId = 0;
      if (this.m_selectedFeatureEditor.HasValue)
      {
        IEditableTerrainFeature terrainFeature = this.m_selectedFeatureEditor.Value.TerrainFeature;
        selectedFeatureId = terrainFeature.Id;
        Mafi.Assert.That<int>(selectedFeatureId).IsPositive<IEditableTerrainFeature>("Selected feature '{0}' has zero ID", terrainFeature);
        this.selectFeatureEditor(Option<TerrainFeatureEditor>.None);
      }
      this.m_map = map;
      this.m_updatableFeatures.Clear();
      this.m_updatableFeaturesOnSimNeedUpdate = true;
      foreach (TerrainFeatureEditor terrainFeatureEditor in this.m_featureEditors.Values)
        terrainFeatureEditor.Destroy();
      this.m_featureEditors.Clear();
      this.m_featureEditorHandles.Clear();
      this.m_featureToHandle.Clear();
      this.m_terrainFeaturesConfig.TerrainFeatureGenerators.Clear();
      this.m_postProcessorsConfig.TerrainPostProcessors.Clear();
      this.m_virtualResourcesConfig.VirtualTerrainResourceGenerators.Clear();
      this.m_startingLocationsConfig.StartingLocations.Clear();
      foreach (ITerrainFeatureBase enumerateAllFeature in this.m_map.EnumerateAllFeatures())
        this.registerFeature(enumerateAllFeature);
      this.m_terrainFeaturesConfigEditor.UpdateValues();
      this.m_postProcessorsConfigEditor.UpdateValues();
      this.m_virtualResourcesConfigEditor.UpdateValues();
      this.m_startingLocationsConfigEditor.UpdateValues();
      if (selectedFeatureId <= 0)
        return;
      ITerrainFeatureBase feature = map.EnumerateAllFeatures().FirstOrDefault<ITerrainFeatureBase>((Func<ITerrainFeatureBase, bool>) (x => x.Id == selectedFeatureId));
      if (feature == null)
        return;
      this.selectFeatureEditor(feature);
    }

    private void includeInAreaForRegeneration(RectangleTerrainArea2i? area)
    {
      if (!area.HasValue || area.Value.IsEmpty)
        return;
      Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig toolbarConfig = this.m_toolbarConfig;
      RectangleTerrainArea2i? areaToRegenerate = this.m_toolbarConfig.AreaToRegenerate;
      ref RectangleTerrainArea2i? local = ref areaToRegenerate;
      RectangleTerrainArea2i? nullable = local.HasValue ? new RectangleTerrainArea2i?(local.GetValueOrDefault().Union(area.Value)) : area;
      toolbarConfig.AreaToRegenerate = nullable;
      this.updateAreaToRegeneratePreview();
    }

    private void onAreaSelectionDone(RectangleTerrainArea2i area)
    {
      if (!this.m_regenerationTaskEnumerator.IsNone || !area.IsNotEmpty)
        return;
      this.m_applyChanges = true;
      this.m_toolbarConfig.AreaToRegenerate = new RectangleTerrainArea2i?(area);
      this.updateAreaToRegeneratePreview();
    }

    public void OnToolbarConfigChanged(bool isFinal)
    {
      if (this.m_toolbarConfig.ShowResourcesBars)
        this.m_resVizRendererActivator.ShowAll();
      else
        this.m_resVizRendererActivator.HideAll();
      this.m_gridLinesActivator.SetActive(this.m_toolbarConfig.ShowTerrainGrid);
      this.m_waterRenderer.SetOceanRenderingState(!this.m_toolbarConfig.TransparentOcean);
      this.m_fogController.SetFogRenderingState(!this.m_toolbarConfig.DisableFog);
      this.m_terrainRenderer.SetTerrainDetailsGeneration(!this.m_toolbarConfig.DisableTerrainDetails);
      VehiclePathFindingParams selectedPfParams = this.m_toolbarConfig.GetSelectedPfParams();
      if (this.m_currentOverlayMode == this.m_toolbarConfig.Overlay && (!this.m_navOverlayRenderer.IsOverlayShown || !(this.m_navOverlayRenderer.ShownPfParams != selectedPfParams)))
        return;
      this.m_currentOverlayMode = this.m_toolbarConfig.Overlay;
      this.m_navOverlayRenderer.HideOverlay();
      switch (this.m_toolbarConfig.Overlay)
      {
        case Mafi.Unity.MapEditor.MapEditor.OverlayMode.Pathability:
          this.m_navOverlayRenderer.ShowOverlayFor(selectedPfParams);
          break;
      }
      this.resetNavOverlay();
    }

    private void terrainFeaturesConfigChanged(bool isFinal)
    {
      if (!isFinal)
        return;
      this.ensureFeaturesOrder();
      this.m_map.TerrainFeatureGeneratorsList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper featureGenerator in this.m_terrainFeaturesConfig.TerrainFeatureGenerators)
        this.m_map.TerrainFeatureGeneratorsList.Add(featureGenerator.FeatureGenerator);
      this.clearAllFeaturesCaches();
      this.pushNewUndoSnapshot("Features list updated", new RectangleTerrainArea2i?());
    }

    private void postProcessorsConfigChanged(bool isFinal)
    {
      if (!isFinal)
        return;
      this.ensurePostProcessorsOrder();
      this.m_map.TerrainPostProcessorsList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper terrainPostProcessor in this.m_postProcessorsConfig.TerrainPostProcessors)
        this.m_map.TerrainPostProcessorsList.Add(terrainPostProcessor.PostProcessor);
      this.clearAllFeaturesCaches();
      this.pushNewUndoSnapshot("Post-processors list updated", new RectangleTerrainArea2i?());
    }

    private void virtualResourcesConfigChanged(bool isFinal)
    {
      if (!isFinal)
        return;
      this.m_map.VirtualTerrainResourceGeneratorList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper resourceGenerator in this.m_virtualResourcesConfig.VirtualTerrainResourceGenerators)
      {
        this.m_map.VirtualTerrainResourceGeneratorList.Add(resourceGenerator.VirtualResourceGenerator);
        resourceGenerator.VirtualResourceGenerator.ClearCaches();
      }
      this.pushNewUndoSnapshot("Virtual resources list updated", new RectangleTerrainArea2i?());
    }

    private void startingLocationsConfigChanged(bool isFinal)
    {
      if (!isFinal)
        return;
      this.m_map.StartingLocationsList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper startingLocation in this.m_startingLocationsConfig.StartingLocations)
      {
        this.m_map.StartingLocationsList.Add(startingLocation.StartingLocation);
        startingLocation.StartingLocation.ClearCaches();
      }
      this.pushNewUndoSnapshot("Updated: Starting location", new RectangleTerrainArea2i?());
    }

    private void m_mapPublishConfigChanged(bool isFinal)
    {
      if (!isFinal)
        return;
      this.m_previewData.Name = this.m_mapPublishConfig.MapName;
      this.m_previewData.Description = this.m_mapPublishConfig.Description;
      this.m_previewData.MapVersion = this.m_mapPublishConfig.Version;
      this.m_previewData.AuthorName = this.m_mapPublishConfig.AuthorName;
      this.m_previewData.NameTranslationId = Option<string>.None;
      this.m_previewData.DescriptionTranslationId = Option<string>.None;
      this.m_previewData.ThumbnailImageData = this.m_mapPublishConfig.GetDataForTexture(this.m_mapPublishConfig.Thumbnail) ?? new EncodedImageAndMatrix(Array.Empty<byte>(), new UnityCameraPose?(), new UnityMatrix4?(), 0.0f);
      this.m_additionalData.PreviewImagesData = ((IEnumerable<EncodedImageAndMatrix?>) this.m_mapPublishConfig.Images.Select<EncodedImageAndMatrix?>(new Func<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix, EncodedImageAndMatrix?>(this.m_mapPublishConfig.GetDataForTexture))).Where<EncodedImageAndMatrix?>((Func<EncodedImageAndMatrix?, bool>) (x => x.HasValue)).Select<EncodedImageAndMatrix?, EncodedImageAndMatrix>((Func<EncodedImageAndMatrix?, EncodedImageAndMatrix>) (x => x.Value)).ToImmutableArray<EncodedImageAndMatrix>();
      if (string.IsNullOrEmpty(this.m_mapPublishConfig.MapFileName))
        return;
      this.m_toolbarConfig.MapNameWip = this.m_mapPublishConfig.MapFileName + ".wip.map";
    }

    private void selectedFeatureConfigChanged(bool isFinal)
    {
      TerrainFeatureEditor valueOrNull = this.m_selectedFeatureEditor.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.TerrainFeature.Reset();
      if (!isFinal)
        return;
      if (valueOrNull.TerrainFeature is ITerrainFeatureGenerator)
        this.ensureFeaturesOrder();
      else if (valueOrNull.TerrainFeature is ITerrainPostProcessorV2)
        this.ensurePostProcessorsOrder();
      RectangleTerrainArea2i? affectedArea = valueOrNull.NotifyFeatureUpdated();
      this.pushNewUndoSnapshot("Updated: " + valueOrNull.TerrainFeature.Name, affectedArea);
    }

    public void Save() => this.saveMapAsWip();

    public void ShowLoadTab() => this.m_editorsScreen.ShowEditor(this.m_mapLoadConfigEditor);

    public void ShowPublishTab() => this.m_editorsScreen.ShowEditor(this.m_mapPublishConfigEditor);

    public void ShowLayersTab() => this.m_editorsScreen.ShowEditor(this.m_terrainLayersViewEditor);

    public void NotifySelectedFeatureWasUpdated(
      Option<string> undoSnapshotName,
      RectangleTerrainArea2i? affectedArea)
    {
      this.m_selectedFeatureObjectEditor.UpdateValues();
      if (!undoSnapshotName.HasValue)
        return;
      this.m_terrainFeaturesConfigEditor.UpdateValues();
      this.pushNewUndoSnapshot(undoSnapshotName.Value, affectedArea);
    }

    private void updateMapFeatures()
    {
      this.m_map.TerrainFeatureGeneratorsList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper featureGenerator in this.m_terrainFeaturesConfig.TerrainFeatureGenerators)
        this.m_map.TerrainFeatureGeneratorsList.Add(featureGenerator.FeatureGenerator);
      this.m_map.TerrainPostProcessorsList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper terrainPostProcessor in this.m_postProcessorsConfig.TerrainPostProcessors)
        this.m_map.TerrainPostProcessorsList.Add(terrainPostProcessor.PostProcessor);
      this.m_map.VirtualTerrainResourceGeneratorList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper resourceGenerator in this.m_virtualResourcesConfig.VirtualTerrainResourceGenerators)
        this.m_map.VirtualTerrainResourceGeneratorList.Add(resourceGenerator.VirtualResourceGenerator);
      this.m_map.StartingLocationsList.Clear();
      foreach (Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper startingLocation in this.m_startingLocationsConfig.StartingLocations)
        this.m_map.StartingLocationsList.Add(startingLocation.StartingLocation);
    }

    private void onHistoryChanged()
    {
      this.m_historyView.UpdateValues(this.m_undoManager);
      this.m_historyObjectEditor.UpdateValues();
      this.m_toolbarConfig.HasUnsavedChanges = true;
    }

    private void ensureFeaturesOrder()
    {
      Lyst<Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper> featureGenerators = this.m_terrainFeaturesConfig.TerrainFeatureGenerators;
      for (int index = 1; index < featureGenerators.Count; ++index)
      {
        if (featureGenerators[index - 1].FeatureGenerator.SortingPriority > featureGenerators[index].FeatureGenerator.SortingPriority)
        {
          this.m_terrainFeaturesConfig.TerrainFeatureGenerators = featureGenerators.OrderBy<Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper, int>((Func<Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper, int>) (x => x.FeatureGenerator.SortingPriority)).ToLyst<Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper>();
          this.m_map.TerrainFeatureGeneratorsList.Clear();
          foreach (Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper featureGenerator in this.m_terrainFeaturesConfig.TerrainFeatureGenerators)
            this.m_map.TerrainFeatureGeneratorsList.Add(featureGenerator.FeatureGenerator);
          this.clearAllFeaturesCaches();
          break;
        }
      }
      this.m_terrainFeaturesConfigEditor.UpdateValues();
    }

    private void ensurePostProcessorsOrder()
    {
      Lyst<Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper> terrainPostProcessors = this.m_postProcessorsConfig.TerrainPostProcessors;
      for (int index = 1; index < terrainPostProcessors.Count; ++index)
      {
        if (terrainPostProcessors[index - 1].PostProcessor.SortingPriority > terrainPostProcessors[index].PostProcessor.SortingPriority)
        {
          this.m_postProcessorsConfig.TerrainPostProcessors = terrainPostProcessors.OrderBy<Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper, int>((Func<Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper, int>) (x => x.PostProcessor.SortingPriority)).ToLyst<Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper>();
          this.m_map.TerrainPostProcessorsList.Clear();
          foreach (Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper terrainPostProcessor in this.m_postProcessorsConfig.TerrainPostProcessors)
            this.m_map.TerrainPostProcessorsList.Add(terrainPostProcessor.PostProcessor);
          this.clearAllFeaturesCaches();
          break;
        }
      }
      this.m_postProcessorsConfigEditor.UpdateValues();
    }

    private bool tryAddAndSelectNewFeatureFromTemplate(Option<ITerrainFeatureTemplate> template)
    {
      if (template.IsNone)
        return false;
      Tile3f pivotPosition3f = this.CameraController.CameraModel.State.PivotPosition3f;
      string error;
      if (this.tryAddAndSelectNewFeature(template.Value.CreateNewFeatureAt(pivotPosition3f, this.m_featureTemplatesRng), out error))
        return true;
      Mafi.Log.Error("Failed to add a new feature: " + error);
      return false;
    }

    private bool tryAddNewFeature(
      ITerrainFeatureBase feature,
      out string error,
      bool noUndoStateName = false)
    {
      if (feature.IsUnique && !this.m_mapPublishConfig.IgnoreUniqueFeaturesConstraint)
      {
        System.Type type = feature.GetType();
        foreach (ITerrainFeatureBase enumerateAllFeature in this.m_map.EnumerateAllFeatures())
        {
          if (enumerateAllFeature.GetType() == type)
          {
            error = "Unique feature '" + enumerateAllFeature.Name + "' is already in the map.";
            return false;
          }
        }
      }
      feature.Id = this.getNextFeatureId();
      this.registerFeature(feature);
      this.updateMapFeatures();
      switch (feature)
      {
        case ITerrainFeatureGenerator _:
          this.ensureFeaturesOrder();
          break;
        case ITerrainPostProcessorV2 _:
          this.ensurePostProcessorsOrder();
          break;
      }
      RectangleTerrainArea2i? affectedArea = feature.GetBoundingBox();
      TerrainFeatureEditor terrainFeatureEditor;
      if (feature is IEditableTerrainFeature key && this.m_featureEditors.TryGetValue(key, out terrainFeatureEditor))
        affectedArea = terrainFeatureEditor.NotifyFeatureUpdated();
      if (!noUndoStateName)
        this.setNextUndoStateName("Added new: " + feature.Name, affectedArea);
      error = "";
      return true;
    }

    private bool tryAddAndSelectNewFeature(
      ITerrainFeatureBase feature,
      out string error,
      bool noUndoStateName = false)
    {
      if (!this.tryAddNewFeature(feature, out error, noUndoStateName))
        return false;
      this.selectFeatureEditor(feature);
      return true;
    }

    private void registerFeature(ITerrainFeatureBase feature)
    {
      if (feature is ITerrainFeatureGenerator featureGenerator)
        this.m_terrainFeaturesConfig.TerrainFeatureGenerators.Add(new Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper(featureGenerator, this));
      if (feature is ITerrainPostProcessorV2 postProcessor)
        this.m_postProcessorsConfig.TerrainPostProcessors.Add(new Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper(postProcessor, this));
      if (feature is IVirtualTerrainResourceGenerator virtualResource)
        this.m_virtualResourcesConfig.VirtualTerrainResourceGenerators.Add(new Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper(virtualResource, this));
      if (feature is IStartingLocationV2 startingLocation)
        this.m_startingLocationsConfig.StartingLocations.Add(new Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper(startingLocation, this));
      if (feature is IEditableTerrainFeature key)
      {
        TerrainFeatureEditor terrainFeatureEditor = this.m_resolver.Instantiate<TerrainFeatureEditor>(new object[1]
        {
          (object) key
        });
        this.m_featureEditors.Add(key, terrainFeatureEditor);
        GameObject handleGo;
        if (terrainFeatureEditor.TryGetHandleObject(this, out handleGo))
        {
          handleGo.layer = Layer.Custom13Icons.ToId();
          handleGo.SetActive(true);
          this.m_featureEditorHandles.Add(handleGo, terrainFeatureEditor);
          this.m_featureToHandle.Add(key, handleGo);
        }
      }
      if (!(feature is ITerrainFeatureWithSimUpdate featureWithSimUpdate))
        return;
      this.m_updatableFeatures.Add(featureWithSimUpdate);
      this.m_updatableFeaturesOnSimNeedUpdate = true;
    }

    private void unregisterFeature(ITerrainFeatureBase feature)
    {
      ITerrainFeatureGenerator tfg = feature as ITerrainFeatureGenerator;
      if (tfg != null)
        this.m_terrainFeaturesConfig.TerrainFeatureGenerators.RemoveFirst((Predicate<Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper>) (x => x.FeatureGenerator == tfg));
      ITerrainPostProcessorV2 tpp = feature as ITerrainPostProcessorV2;
      if (tpp != null)
        this.m_postProcessorsConfig.TerrainPostProcessors.RemoveFirst((Predicate<Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper>) (x => x.PostProcessor == tpp));
      IVirtualTerrainResourceGenerator vtr = feature as IVirtualTerrainResourceGenerator;
      if (vtr != null)
        this.m_virtualResourcesConfig.VirtualTerrainResourceGenerators.RemoveFirst((Predicate<Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper>) (x => x.VirtualResourceGenerator == vtr));
      IStartingLocationV2 sl = feature as IStartingLocationV2;
      if (sl != null)
        this.m_startingLocationsConfig.StartingLocations.RemoveFirst((Predicate<Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper>) (x => x.StartingLocation == sl));
      RectangleTerrainArea2i? affectedArea = new RectangleTerrainArea2i?(feature.GetBoundingBox() ?? this.TerrainManager.TerrainArea);
      if (feature is IEditableTerrainFeature key)
      {
        TerrainFeatureEditor terrainFeatureEditor;
        if (this.m_featureEditors.TryRemove(key, out terrainFeatureEditor))
        {
          affectedArea = terrainFeatureEditor.NotifyFeatureUpdated();
          terrainFeatureEditor.Deactivate();
          if (this.m_selectedFeatureEditor == terrainFeatureEditor)
            this.selectFeatureEditor(Option<TerrainFeatureEditor>.None);
        }
        GameObject gameObject;
        if (this.m_featureToHandle.TryGetValue(key, out gameObject))
        {
          this.m_featureEditorHandles.Remove(gameObject);
          this.m_featureToHandle.Remove(key);
          gameObject.Destroy();
        }
      }
      if (feature is ITerrainFeatureWithSimUpdate featureWithSimUpdate)
      {
        this.m_updatableFeatures.Remove(featureWithSimUpdate);
        this.m_updatableFeaturesOnSimNeedUpdate = true;
      }
      this.updateMapFeatures();
      this.setNextUndoStateName("Removed: " + feature.Name, affectedArea);
    }

    private void selectFeatureEditor(ITerrainFeatureBase feature)
    {
      TerrainFeatureEditor featureEditor;
      if (!(feature is IEditableTerrainFeature key) || !this.m_featureEditors.TryGetValue(key, out featureEditor))
        return;
      this.selectFeatureEditor((Option<TerrainFeatureEditor>) featureEditor);
    }

    private void selectFeatureEditor(Option<TerrainFeatureEditor> featureEditor)
    {
      bool hasValue = this.m_selectedFeatureEditor.HasValue;
      if (hasValue)
        this.m_selectedFeatureEditor.Value.Deactivate();
      this.m_selectedFeatureEditor = featureEditor;
      if (this.m_selectedFeatureEditor.HasValue)
      {
        this.m_selectedFeatureEditor.Value.Activate();
        this.m_selectedFeatureObjectEditor.SetObjectToEdit((Option<object>) (object) new Mafi.Unity.MapEditor.MapEditor.EditableFeatureWrapperForEditView(featureEditor.Value.TerrainFeature, this));
        this.m_editorsScreen.ShowEditor(this.m_selectedFeatureObjectEditor);
        featureEditor.Value.TerrainFeature.Reset();
        this.DetailPreviewController.DeactivateIfCan();
      }
      else
      {
        if (!hasValue)
          return;
        this.m_selectedFeatureObjectEditor.HideEditor();
      }
    }

    private void renderUpdate(GameTime time)
    {
      this.m_editorsScreen.RenderUpdate(time);
      this.m_toolbar.RenderUpdate();
    }

    private void renderUpdateEnd(GameTime time)
    {
      foreach (TerrainFeatureEditor terrainFeatureEditor in this.m_featureEditors.Values)
        terrainFeatureEditor.ScaleHandle();
    }

    private void syncUpdate(GameTime time)
    {
      this.m_editorsScreen.SyncUpdate(time);
      this.m_toolbar.SyncUpdate(time);
      this.CameraController.CameraModel.SetAutoHeightAdjustEnabled(!this.IsEditorBusy);
      if (this.m_cancelRegeneration)
      {
        this.m_cancelRegeneration = false;
        this.m_terrainGenerator.Cancel();
        this.m_toolbar.ShowNotification("Cancelled");
        this.m_regenerationTaskEnumerator = Option<IEnumerator<RegenProgress>>.None;
        if (this.m_mapPublishingEnumerator.HasValue)
        {
          this.m_mapPublishErrorsOnSim.Add("Cancelled");
          this.m_mapPublishCompletedOnSim = true;
          this.m_mapPublishingEnumerator = Option<IEnumerator<RegenProgress>>.None;
        }
        this.m_chunksToUpdateRenderer.Clear();
      }
      if (this.m_regenerationCompletedOnSim)
      {
        this.m_regenerationCompletedOnSim = false;
        this.m_regenerationTaskEnumerator = Option<IEnumerator<RegenProgress>>.None;
        this.m_toolbarConfig.AreaToRegenerate = new RectangleTerrainArea2i?();
        this.updateAreaToRegeneratePreview();
        this.m_toolbar.UpdateProgress(new RegenProgress(this.m_regenTaskProgressOnSim.Message, Percent.Hundred));
        this.m_virtualResourceManager.InitializeResources();
        this.m_resVizRendererActivator.Renderer.InvalidateAllResourceBars();
        foreach (TerrainFeatureEditor terrainFeatureEditor in this.m_featureEditors.Values)
          terrainFeatureEditor.UpdateHandle();
      }
      else if (this.m_regenerationTaskEnumerator.HasValue)
      {
        this.m_toolbar.UpdateProgress(this.m_regenTaskProgressOnSim);
        return;
      }
      if (this.m_mapPublishCompletedOnSim)
      {
        this.m_mapPublishCompletedOnSim = false;
        this.m_mapPublishingEnumerator = Option<IEnumerator<RegenProgress>>.None;
        this.mapPublishCompleted();
        this.m_toolbar.UpdateProgress(new RegenProgress(this.m_regenTaskProgressOnSim.Message, Percent.Hundred));
      }
      else if (this.m_mapPublishingEnumerator.HasValue && !this.m_applyChanges)
      {
        this.m_toolbar.UpdateProgress(this.m_regenTaskProgressOnSim);
        return;
      }
      if (this.m_updatableFeaturesOnSimNeedUpdate)
      {
        this.m_updatableFeaturesOnSimNeedUpdate = false;
        this.m_updatableFeaturesOnSim.Clear();
        this.m_updatableFeaturesOnSim.AddRange(this.m_updatableFeatures);
      }
      if (this.m_selectedFeatureEditor.HasValue && !this.ArePreviewsSuppressed)
        this.m_selectedFeatureEditor.Value.UpdatePreview(this.TerrainManager, this.m_map.TerrainPostProcessors, this.m_editorSettings.PreviewProcessingTimeBudgetMs);
      if (this.m_notifyFeatureUpdatedOnSim.IsNotEmpty)
      {
        foreach (IEditableTerrainFeature key in this.m_notifyFeatureUpdatedOnSim)
        {
          TerrainFeatureEditor terrainFeatureEditor;
          if (this.m_featureEditors.TryGetValue(key, out terrainFeatureEditor))
            terrainFeatureEditor.NotifyFeatureUpdated(true);
        }
        this.m_notifyFeatureUpdatedOnSim.Clear();
      }
      if (this.m_chunksToUpdateRenderer.IsNotEmpty)
      {
        foreach (Chunk2i chunk in this.m_chunksToUpdateRenderer)
        {
          this.m_terrainRenderer.NotifyChunkUpdated(chunk);
          this.m_waterRenderer.NotifyChunkUpdated(chunk);
        }
        this.m_chunksToUpdateRenderer.Clear();
      }
      if (!this.m_applyChanges)
        return;
      RectangleTerrainArea2i? areaToRegenerate = this.m_toolbarConfig.AreaToRegenerate;
      if (!areaToRegenerate.HasValue || !this.m_regenerationTaskEnumerator.IsNone)
        return;
      this.m_applyChanges = false;
      areaToRegenerate = this.m_toolbarConfig.AreaToRegenerate;
      this.startTerrainRegeneration(areaToRegenerate.Value);
      this.m_toolbar.ShowNotification("Applying changes");
      this.updateAreaToRegeneratePreview();
      this.m_chunksToUpdateRenderer.Clear();
    }

    private void simUpdate()
    {
      IEnumerator<RegenProgress> valueOrNull1 = this.m_regenerationTaskEnumerator.ValueOrNull;
      if (valueOrNull1 != null)
      {
        if (this.m_regenerationCompletedOnSim)
          return;
        Option<Exception> exception;
        if (this.tryContinueProcessing<RegenProgress>(valueOrNull1, this.m_editorSettings.MapGenerationTimeBudgetMs, out this.m_regenTaskProgressOnSim, out exception))
        {
          this.m_navOverlayRenderer.SetRuntimeBudgetMultiplierForNextTick(-1);
          return;
        }
        this.m_regenerationCompletedOnSim = true;
        if (exception.HasValue)
        {
          Mafi.Log.Exception(exception.Value, "Error during map generation");
          string message = "Error occurred: " + exception.Value.GetType().Name;
          Option<string> option = (Option<string>) exception.Value.ToString();
          Percent progress = new Percent();
          Option<string> errorTip = option;
          this.m_regenTaskProgressOnSim = new RegenProgress(message, progress, errorTip);
        }
      }
      IEnumerator<RegenProgress> valueOrNull2 = this.m_mapPublishingEnumerator.ValueOrNull;
      if (valueOrNull2 != null && !this.m_mapPublishCompletedOnSim && !this.m_regenerationCompletedOnSim && !this.m_applyChanges)
      {
        Mafi.Assert.That<IEnumerator<RegenProgress>>(valueOrNull1).IsNull<IEnumerator<RegenProgress>>("Generation not finished");
        Option<Exception> exception;
        if (this.tryContinueProcessing<RegenProgress>(valueOrNull2, this.m_editorSettings.MapGenerationTimeBudgetMs, out this.m_regenTaskProgressOnSim, out exception))
          return;
        this.m_mapPublishCompletedOnSim = true;
        if (exception.HasValue)
        {
          Mafi.Log.Exception(exception.Value, "Error during map publishing");
          this.m_mapPublishErrorsOnSim.Add("Error occurred: " + exception.Value.Message);
          string message = "Error occurred: " + exception.Value.GetType().Name;
          Option<string> option = (Option<string>) exception.Value.ToString();
          Percent progress = new Percent();
          Option<string> errorTip = option;
          this.m_regenTaskProgressOnSim = new RegenProgress(message, progress, errorTip);
        }
      }
      foreach (ITerrainFeatureWithSimUpdate featureWithSimUpdate in this.m_updatableFeaturesOnSim)
      {
        if (featureWithSimUpdate.SimUpdate(this.Resolver))
          this.m_notifyFeatureUpdatedOnSim.Add(featureWithSimUpdate);
      }
      if (this.m_resetNavOverlayOnSim)
      {
        this.m_resetNavOverlayOnSim = false;
        this.m_clearancePathabilityProvider.ClearAllData();
        this.m_navOverlayChunksToProcess.Clear();
        this.m_navOverlaySortOrigin = Tile2i.MaxValue;
        if (this.m_navOverlayRenderer.IsOverlayShown)
          this.m_navOverlayChunksToProcess.AddRange(this.TerrainManager.TerrainArea.EnumerateChunksSlim());
      }
      this.m_navOverlayRenderer.SetRuntimeBudgetMultiplierForNextTick(this.m_editorSettings.NavOverlayRuntimeBudgetMult);
      if (!this.m_navOverlayChunksToProcess.IsNotEmpty)
        return;
      int num = 5;
      Tile2i camPos = this.CameraController.CameraModel.State.PivotPosition.Tile2i;
      if (this.m_navOverlaySortOrigin.DistanceSqrTo(camPos) > 2500L)
      {
        this.m_navOverlaySortOrigin = camPos;
        num = 1;
        this.m_navOverlayChunksToProcess.Sort((Comparison<Chunk2iSlim>) ((x, y) => y.AsFull.CenterTile2i.DistanceSqrTo(camPos).CompareTo(x.AsFull.CenterTile2i.DistanceSqrTo(camPos))));
      }
      if (this.m_navOverlayChunksToProcess.Last.AsFull.CenterTile2i.DistanceSqrTo(camPos) >= 250000L)
        return;
      for (int index = 0; index < num && this.m_navOverlayChunksToProcess.IsNotEmpty; ++index)
        this.m_clearancePathabilityProvider.RecomputeAllDataAt(this.m_navOverlayChunksToProcess.PopLast().AsFull);
    }

    private void resetNavOverlay() => this.m_resetNavOverlayOnSim = true;

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_editorsScreen.InputUpdate(inputScheduler);
      if (this.m_nextUndoStateName.HasValue)
      {
        Mafi.Log.Error("Failed to add undo state: " + this.m_nextUndoStateName.Value);
        this.m_nextUndoStateName = Option<string>.None;
      }
      MonoSingleton<RTGApp>.Get.enabled = !this.IsEditorBusy;
      if (!EventSystem.current.IsPointerOverGameObject())
      {
        GameObject valueOrNull = this.m_cursorPickingManager.PickedGameObject.ValueOrNull;
        TerrainFeatureEditor featureEditor;
        if ((UnityEngine.Object) valueOrNull != (UnityEngine.Object) null && !this.IsEditorBusy && UnityEngine.Input.GetMouseButtonDown(0) && this.m_featureEditorHandles.TryGetValue(valueOrNull, out featureEditor))
        {
          this.selectFeatureEditor((Option<TerrainFeatureEditor>) featureEditor);
          return true;
        }
      }
      if (this.m_selectedFeatureEditor.HasValue && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
      {
        this.selectFeatureEditor(Option<TerrainFeatureEditor>.None);
        return true;
      }
      if (this.IsEditorBusy)
      {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
          this.StopRegeneration();
        return true;
      }
      if (this.m_areaSelection.InputUpdate())
        return true;
      if (UnityEngine.Input.GetKeyUp(KeyCode.Tab))
        this.SuppressPreviewsAndHandles = false;
      else if (UnityEngine.Input.GetKeyDown(KeyCode.Tab))
        this.SuppressPreviewsAndHandles = true;
      if (UnityEngine.Input.GetKeyDown(KeyCode.Delete) && this.m_selectedFeatureEditor.HasValue)
      {
        this.unregisterFeature((ITerrainFeatureBase) this.m_selectedFeatureEditor.Value.TerrainFeature);
        this.pushAlreadyNamedUndoSnapshot();
        return true;
      }
      bool flag = !this.DetailPreviewController.IsEnabled && !this.AreHandlesSuppressed && !this.EntityTesterPreviewController.IsEnabled;
      foreach (GameObject gameObject in this.m_featureToHandle.Values)
        gameObject.SetActive(flag);
      if (EventSystem.current.IsPointerOverGameObject() || MonoSingleton<RTGizmosEngine>.Get.HasHoveredSceneEntity || !this.m_regenerationTaskEnumerator.IsNone)
        return false;
      if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKey(KeyCode.LeftShift))
        this.m_areaSelection.Activate();
      else if (this.m_areaSelection.IsActive)
        this.m_areaSelection.Deactivate();
      if (!UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(KeyCode.LeftShift) || !UnityEngine.Input.GetMouseButton(0))
        return false;
      this.m_terrainLayersView.TilePosition = Tile2f.Zero;
      this.m_terrainLayersView.Layers.Clear();
      Tile3f position;
      if (this.m_terrainCursor.TryComputeCurrentPosition(out position))
        this.m_terrainLayersView.ShowTileInfo(position);
      this.m_terrainLayersViewEditor.UpdateValues();
      this.m_editorsScreen.ShowEditor(this.m_terrainLayersViewEditor);
      return true;
    }

    private void applySuppressions()
    {
      bool previewsSuppressed = this.ArePreviewsSuppressed;
      if (previewsSuppressed && this.m_selectedFeatureEditor.HasValue)
        this.selectFeatureEditor(this.m_selectedFeatureEditor);
      else if (!previewsSuppressed)
        this.m_selectedFeatureEditor.ValueOrNull?.ClearPreview();
      this.m_mapSizeConfig.SetAreasVisibility(!previewsSuppressed);
    }

    private void applyTranslation(RelTile3f translation)
    {
      foreach (ITerrainFeatureBase enumerateAllFeature in this.m_map.EnumerateAllFeatures())
        enumerateAllFeature.TranslateBy(translation);
      foreach (TerrainFeatureEditor terrainFeatureEditor in this.m_featureEditors.Values)
        terrainFeatureEditor.UpdateHandle();
    }

    private void clearAllFeaturesCaches()
    {
      foreach (ITerrainFeatureBase enumerateAllFeature in this.m_map.EnumerateAllFeatures())
        enumerateAllFeature.ClearCaches();
    }

    private void startTerrainRegeneration(RectangleTerrainArea2i affectedArea)
    {
      if (this.m_regenerationTaskEnumerator.HasValue)
        Mafi.Log.Error("Cannot regenerate two areas at the same time.");
      else
        this.m_regenerationTaskEnumerator = this.regenerateTerrainTimeSliced(affectedArea).SomeOption<IEnumerator<RegenProgress>>();
    }

    private IEnumerator<RegenProgress> regenerateTerrainTimeSliced(
      RectangleTerrainArea2i affectedArea)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<RegenProgress>) new Mafi.Unity.MapEditor.MapEditor.\u003CregenerateTerrainTimeSliced\u003Ed__203(0)
      {
        \u003C\u003E4__this = this,
        affectedArea = affectedArea
      };
    }

    private bool tryContinueProcessing<T>(
      IEnumerator<T> enumerator,
      int timeBudgetMs,
      out T progress,
      out Option<Exception> exception)
    {
      this.m_stopwatch.Restart();
      exception = Option<Exception>.None;
      progress = default (T);
      try
      {
        do
        {
          bool flag = enumerator.MoveNext();
          progress = enumerator.Current;
          if (!flag)
            return false;
        }
        while (this.m_stopwatch.ElapsedMilliseconds < (long) timeBudgetMs);
      }
      catch (OperationCanceledException ex)
      {
        return false;
      }
      catch (Exception ex)
      {
        exception = (Option<Exception>) ex;
        return false;
      }
      return true;
    }

    private void updateAreaToRegeneratePreview()
    {
      if (this.m_toolbarConfig.AreaToRegenerate.HasValue)
      {
        this.m_regeneratedAreaRenderer.SetArea(this.m_toolbarConfig.AreaToRegenerate.Value, this.TerrainManager);
        this.m_regeneratedAreaRenderer.Show();
      }
      else
        this.m_regeneratedAreaRenderer.Hide();
    }

    public void Activate()
    {
    }

    public void Deactivate() => Mafi.Log.Error("Cannot deactivate!");

    private void publishMap()
    {
      try
      {
        this.initiateMapPublish();
      }
      catch (Exception ex)
      {
        Mafi.Log.Exception(ex, "Failed to initiate map publish.");
        this.m_mapPublishConfig.SetPublishResults((Option<string>) "Resolve error in order to publish a map", (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[1]
        {
          "Error: " + ex.Message
        }));
        this.m_mapPublishConfigEditor.UpdateValues();
        this.m_mapPublishingEnumerator = Option<IEnumerator<RegenProgress>>.None;
        this.StopRegeneration();
      }
    }

    private void initiateMapPublish()
    {
      this.m_mapPublishConfig.ClearPublishResults();
      this.updateMapPreviewData();
      this.ensureFeaturesOrder();
      this.ensurePostProcessorsOrder();
      LystStruct<string> lyst1 = new LystStruct<string>();
      if (this.m_regenerationTaskEnumerator.HasValue)
        lyst1.Add("Map is being regenerated.");
      if (this.m_mapPublishingEnumerator.HasValue)
        lyst1.Add("Other publish operation is in progress.");
      if (string.IsNullOrEmpty(this.m_mapPublishConfig.MapFileName) || this.m_mapPublishConfig.MapFileName.Length <= 2)
        lyst1.Add("Invalid map file name");
      if (FileSystemHelper.ContainsInvalidFileNameCharacters(this.m_mapPublishConfig.MapFileName))
      {
        lyst1.Add("Map file name contains invalid characters.");
      }
      else
      {
        string filePath = this.m_fsHelper.GetFilePath(this.m_mapPublishConfig.MapFileName, FileType.Map);
        if (!this.m_mapPublishConfig.AllowFileOverwrite && File.Exists(filePath))
          lyst1.Add("File '" + Path.GetFileName(filePath) + "' already exists. Check 'Allow file overwrite' to skip this check.");
      }
      if (string.IsNullOrEmpty(this.m_previewData.Name) || this.m_previewData.Name.Length <= 2)
        lyst1.Add("Invalid map name: Too short");
      if (string.IsNullOrEmpty(this.m_previewData.Description) || this.m_previewData.Description.Length <= 10)
        lyst1.Add("Invalid map description: Too short");
      if (string.IsNullOrEmpty(this.m_previewData.AuthorName) || this.m_previewData.AuthorName.Length <= 2)
        lyst1.Add("Invalid author name: Too short");
      if (this.m_previewData.MapVersion <= 0)
        lyst1.Add("Invalid map version: Must be positive");
      if (this.m_previewData.ThumbnailImageData.ImageData.IsNullOrEmpty<byte>())
        lyst1.Add("Thumbnail image is missing.");
      if (this.m_additionalData.StartingLocations.IsEmpty)
        lyst1.Add("No starting locations");
      if (this.m_additionalData.PreviewImagesData.IsEmpty)
        lyst1.Add("No preview image was captured.");
      if (!Enum.IsDefined(typeof (StartingLocationDifficulty), (object) this.m_previewData.Difficulty))
        lyst1.Add("Invalid map difficulty");
      if (!this.m_mapPublishConfig.IgnoreFeaturesOutOfOrder)
      {
        Lyst<ITerrainFeatureGenerator> featureGeneratorsList = this.m_map.TerrainFeatureGeneratorsList;
        for (int index = 1; index < featureGeneratorsList.Count; ++index)
        {
          ITerrainFeatureGenerator featureGenerator1 = featureGeneratorsList[index - 1];
          ITerrainFeatureGenerator featureGenerator2 = featureGeneratorsList[index];
          if (featureGenerator1.SortingPriority > featureGenerator2.SortingPriority)
            lyst1.Add(string.Format("Features '{0}' ({1}) with priority {2} and ", (object) featureGenerator1.Name, (object) featureGenerator1.GetType().Name, (object) featureGenerator1.SortingPriority) + string.Format("'{0}' ({1}) with priority {2} are out of order.", (object) featureGenerator2.Name, (object) featureGenerator2.GetType().Name, (object) featureGenerator2.SortingPriority));
        }
      }
      if (!this.m_mapPublishConfig.IgnoreUniqueFeaturesConstraint)
      {
        Set<System.Type> set = new Set<System.Type>();
        foreach (ITerrainFeatureBase terrainFeatureBase in this.m_map.EnumerateAllFeatures().Where<ITerrainFeatureBase>((Func<ITerrainFeatureBase, bool>) (x => x.IsUnique)))
        {
          System.Type type = terrainFeatureBase.GetType();
          if (!set.Add(type))
            lyst1.Add("Duplicated unique feature: " + terrainFeatureBase.Name + " (" + type.Name + ")");
        }
      }
      Lyst<string> lyst2 = new Lyst<string>();
      foreach (IEditableTerrainFeature editableTerrainFeature in this.m_map.EnumerateAllFeatures().OfType<IEditableTerrainFeature>())
      {
        if (!editableTerrainFeature.ValidateConfig(this.Resolver, lyst2))
          lyst1.Add("Feature '" + editableTerrainFeature.Name + "' (" + editableTerrainFeature.GetType().Name + ") has invalid config:\n" + lyst2.JoinStrings("\n"));
      }
      for (int index = 0; index < this.m_map.StartingLocationsList.Count; ++index)
      {
        Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper startingLocation = this.m_startingLocationsConfig.StartingLocations[index];
        if (this.m_map.StartingLocationsList[index] != startingLocation.StartingLocation)
        {
          Mafi.Log.Error(string.Format("Invalid starting location order at index {0} (map vs. wrapper).", (object) index));
          lyst1.Add("Invalid starting location order (map vs. wrapper).");
        }
        if (this.m_additionalData.StartingLocations[index].Position != startingLocation.StartingLocation.ToPreview().Position)
        {
          Mafi.Log.Error(string.Format("Invalid starting location order at index {0} (additional data vs wrapper).", (object) index));
          lyst1.Add("Invalid starting location order (additional data vs. wrapper).");
        }
      }
      if (lyst1.IsNotEmpty)
      {
        this.m_toolbar.ShowErrorNotification("Publishing failed", lyst1.JoinStrings("\n"));
        this.m_mapPublishConfig.SetPublishResults((Option<string>) "Publish failed, fix errors and try again.", lyst1.AsEnumerable());
      }
      else
      {
        this.m_logsDuringPublish.Clear();
        this.m_mapPublishErrorsOnSim.Clear();
        Mafi.Log.LogReceived += new Action<LogEntry>(this.logDuringPublish);
        this.m_mapPublishingEnumerator = this.finalizeMapPublishTimeSliced().SomeOption<IEnumerator<RegenProgress>>();
        this.clearAllFeaturesCaches();
        this.RegenerateEntireMap();
      }
    }

    private void logDuringPublish(LogEntry log)
    {
      if (log.Type > Mafi.Logging.LogType.Error)
        return;
      this.m_logsDuringPublish.Add(log);
    }

    private IEnumerator<RegenProgress> computeTotalResources()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<RegenProgress>) new Mafi.Unity.MapEditor.MapEditor.\u003CcomputeTotalResources\u003Ed__215(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private IEnumerator<RegenProgress> finalizeMapPublishTimeSliced()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<RegenProgress>) new Mafi.Unity.MapEditor.MapEditor.\u003CfinalizeMapPublishTimeSliced\u003Ed__216(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private void mapPublishCompleted()
    {
      foreach (TerrainFeatureEditor terrainFeatureEditor in this.m_featureEditors.Values)
        terrainFeatureEditor.UpdateHandle();
      Mafi.Log.LogReceived -= new Action<LogEntry>(this.logDuringPublish);
      if (this.m_mapPublishErrorsOnSim.IsNotEmpty)
      {
        this.m_mapPublishConfig.SetPublishResults((Option<string>) "Publish failed, fix errors and try again.", this.m_mapPublishErrorsOnSim.AsEnumerable());
        this.m_toolbar.ShowErrorNotification("Publish failed", this.m_mapPublishErrorsOnSim.AsEnumerable().JoinStrings("\n"));
        this.m_mapPublishErrorsOnSim.Clear();
      }
      else if (this.m_logsDuringPublish.IsNotEmpty)
      {
        this.m_mapPublishConfig.SetPublishResults((Option<string>) "Publish failed due to errors.", this.m_logsDuringPublish.AsEnumerable().Select<LogEntry, string>((Func<LogEntry, string>) (x => x.ToString(3, true))));
        this.m_toolbar.ShowErrorNotification("Publish failed", this.m_logsDuringPublish.First.Message);
        this.m_logsDuringPublish.Clear();
      }
      else
      {
        this.m_previewData.IsPublished = true;
        this.m_mapSerializer.StartSave((IWorldRegionMap) this.m_map, (IWorldRegionMapPreviewData) this.m_previewData, (IWorldRegionMapAdditionalData) this.m_additionalData);
        string filePath = this.m_fsHelper.GetFilePath(this.m_mapPublishConfig.MapFileName, FileType.Map);
        this.m_asyncSaver.RunInSyncAndReset((IAsyncSavable) this.m_mapSerializer, filePath);
        if (this.m_asyncSaver.LastSaveError.HasValue)
        {
          this.m_mapPublishConfig.SetPublishResults((Option<string>) "Failed to save published map.", (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[1]
          {
            string.Format("Failed: {0}", (object) this.m_asyncSaver.LastSaveError.Value)
          }));
          this.m_toolbar.ShowErrorNotification("Publish failed", this.m_asyncSaver.LastSaveError.Value.Value);
        }
        else
        {
          this.m_mapPublishConfig.SetPublishResults(Option<string>.None, (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[1]
          {
            "Published successfully as " + Path.GetFileName(filePath) + "!"
          }));
          this.m_toolbar.ShowNotification("Publish complete!");
        }
        this.m_toolbarConfig.HasUnsavedChanges = false;
      }
    }

    private string saveCapturedImages()
    {
      string str = this.m_mapPublishConfig.MapFileName ?? "map";
      if (!this.m_previewData.ThumbnailImageData.ImageData.IsNullOrEmpty<byte>())
        File.WriteAllBytes(this.m_fsHelper.GetFilePath("map_" + str + "_thn.jpg", FileType.Debug, true), this.m_previewData.ThumbnailImageData.ImageData);
      int index = 0;
      while (true)
      {
        int num = index;
        ImmutableArray<EncodedImageAndMatrix> previewImagesData = this.m_additionalData.PreviewImagesData;
        int length = previewImagesData.Length;
        if (num < length)
        {
          previewImagesData = this.m_additionalData.PreviewImagesData;
          EncodedImageAndMatrix encodedImageAndMatrix = previewImagesData[index];
          if (encodedImageAndMatrix.ViewProjectionMatrix.HasValue && !encodedImageAndMatrix.ImageData.IsNullOrEmpty<byte>())
            File.WriteAllBytes(this.m_fsHelper.GetFilePath(string.Format("map_{0}_preview_{1}.jpg", (object) str, (object) index), FileType.Debug, true), encodedImageAndMatrix.ImageData);
          ++index;
        }
        else
          break;
      }
      return this.m_fsHelper.GetDirPath(FileType.Debug, true);
    }

    private static string getFeatureName(ITerrainFeatureBase feature)
    {
      return feature.Name + (feature.IsUnique ? " (unique)" : "");
    }

    private static string getFeatureTypeName(ITerrainFeatureBase feature)
    {
      return feature.GetType().Name.CamelCaseToSpacedSentenceCase();
    }

    private void duplicateFeature(ITerrainFeatureBase feature)
    {
      ITerrainFeatureBase feature1 = this.m_serializationUtils.DeepCopyViaSerialization<ITerrainFeatureBase>(feature);
      RectangleTerrainArea2i? boundingBox = feature.GetBoundingBox();
      ref RectangleTerrainArea2i? local = ref boundingBox;
      Tile2f? nullable = local.HasValue ? new Tile2f?(local.GetValueOrDefault().CenterCoordF) : new Tile2f?();
      RelTile2f relTile2f;
      if (nullable.HasValue)
      {
        Tile2f pivotPosition = this.CameraController.CameraModel.State.PivotPosition;
        relTile2f = !(pivotPosition.DistanceSqrTo(nullable.Value) < 16) ? pivotPosition - nullable.Value : new RelTile2f((Fix32) 10, (Fix32) 10);
      }
      else
        relTile2f = new RelTile2f((Fix32) 10, (Fix32) 10);
      feature1.TranslateBy(relTile2f.ExtendZ((Fix32) 0));
      string error;
      if (!this.tryAddAndSelectNewFeature(feature1, out error, true))
      {
        Mafi.Log.Error("Failed to add a new feature: " + error);
      }
      else
      {
        string name = "Cloned: " + feature.Name;
        boundingBox = feature1.GetBoundingBox();
        RectangleTerrainArea2i? affectedArea = new RectangleTerrainArea2i?(boundingBox ?? this.TerrainManager.TerrainArea);
        this.setNextUndoStateName(name, affectedArea);
      }
    }

    private void bringToView(ITerrainFeatureBase featureEditor)
    {
      if (featureEditor is IEditableTerrainFeature key)
      {
        GameObject gameObject;
        if (this.m_featureToHandle.TryGetValue(key, out gameObject))
        {
          this.CameraController.PanTo(gameObject.transform.localPosition.ToTile2f());
          return;
        }
        HandleData? handleData = key.GetHandleData();
        if (handleData.HasValue)
        {
          this.CameraController.PanTo(handleData.Value.Position);
          return;
        }
      }
      RectangleTerrainArea2i? boundingBox = featureEditor.GetBoundingBox();
      if (!boundingBox.HasValue)
        return;
      this.CameraController.PanTo(boundingBox.Value.CenterCoordF);
    }

    static MapEditor()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Mafi.Unity.MapEditor.MapEditor.MAP_THUMBNAIL_SIZE = new Vector2i(640, 320);
      Mafi.Unity.MapEditor.MapEditor.MAP_PREVIEW_SIZE = new Vector2i(2048, 1024);
    }

    public class EditorToolbarConfig
    {
      [EditorIgnore]
      public readonly ImmutableArray<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption> PathabilityClasses;
      /// <summary>Whether the map has unsaved changes.</summary>
      [EditorIgnore]
      public bool HasUnsavedChanges;
      [EditorIgnore]
      public ImmutableArray<(TerrainEditorMenuCategoryProto, IOrderedEnumerable<ITerrainFeatureTemplate>)> BottomTools;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;

      [EditorEnforceOrder(30)]
      public bool ShowResourcesBars { get; set; }

      [EditorEnforceOrder(33)]
      public bool ShowTerrainGrid { get; set; }

      [EditorEnforceOrder(36)]
      public bool DisableFog { get; set; }

      [EditorEnforceOrder(41)]
      [EditorLabel(null, "Disables terrain details such as grass and flowers which results in faster terrain regeneration", false, false)]
      public bool DisableTerrainDetails { get; set; }

      [EditorEnforceOrder(44)]
      public bool TransparentOcean { get; set; }

      [EditorEnforceOrder(49)]
      [EditorLabel(null, "Whether to keep terrain generators caches during regeneration. Keeping caches will make generation faster but may result in inaccurately generated chunks.", false, false)]
      public bool KeepGeneratorsCachesDuringRegeneration { get; set; }

      [EditorEnforceOrder(57)]
      public Mafi.Unity.MapEditor.MapEditor.OverlayMode Overlay { get; set; }

      [EditorEnforceOrder(60)]
      public int PathabilityClass { get; set; }

      [EditorEnforceOrder(66)]
      public RectangleTerrainArea2i? AreaToRegenerate { get; set; }

      [EditorIgnore]
      public string MapNameWip { get; set; }

      [EditorEnforceOrder(72)]
      public string SaveStatus { get; private set; }

      public EditorToolbarConfig(Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.BottomTools = ImmutableArray<(TerrainEditorMenuCategoryProto, IOrderedEnumerable<ITerrainFeatureTemplate>)>.Empty;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_editor = editor;
        this.PathabilityClasses = editor.m_protosDb.All<DrivingEntityProto>().GroupBy<DrivingEntityProto, int>((Func<DrivingEntityProto, int>) (x => editor.m_clearancePathabilityProvider.GetPathabilityClassIndex(x.PathFindingParams.PathabilityQueryMask))).Select<IGrouping<int, DrivingEntityProto>, Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>((Func<IGrouping<int, DrivingEntityProto>, Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>) (group => new Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption(group.Key, group.ToImmutableArray<DrivingEntityProto>()))).ToImmutableArray<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>();
        this.PathabilityClass = this.PathabilityClasses.Length - 1;
      }

      public string GetRegeneratedAreaSummary()
      {
        if (!this.AreaToRegenerate.HasValue)
          return "";
        int productInt = this.AreaToRegenerate.Value.Size.ProductInt;
        Percent percent = Percent.FromRatio(productInt, this.m_editor.TerrainManager.TerrainTilesCount);
        return "Regenerate a partial area of:\n" + productInt.Over(1000000).ToStringRoundedAdaptive() + "M tiles (" + percent.ToStringRounded() + " of the map)";
      }

      public VehiclePathFindingParams GetSelectedPfParams()
      {
        return this.PathabilityClasses[this.PathabilityClass.Clamp(0, this.PathabilityClasses.Length - 1)].Protos[0].PathFindingParams;
      }

      public void SetSaveStatus(string status) => this.SaveStatus = status;

      public readonly struct PathabilityClassOption
      {
        public readonly int Class;
        public readonly ImmutableArray<DrivingEntityProto> Protos;
        public readonly LocStrFormatted Name;
        public readonly LocStrFormatted Tooltip;

        public bool IsEmpty => this.Protos.IsEmpty;

        public PathabilityClassOption(int cls, ImmutableArray<DrivingEntityProto> protos)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Tooltip = LocStrFormatted.Empty;
          this.Class = cls;
          this.Protos = protos;
          this.Name = (LocStrFormatted) this.Protos[0].Strings.Name;
          if (this.Protos.Length <= 1)
            return;
          this.Tooltip = ("Includes:\n" + this.Protos.Select<string>((Func<DrivingEntityProto, string>) (p => string.Format(" - {0}", (object) p.Strings.Name))).Distinct<string>().JoinStrings("\n")).AsLoc();
        }
      }
    }

    public class MapEditorSettings
    {
      [EditorRange(5.0, 50.0)]
      [EditorEnforceOrder(150)]
      public int MapGenerationTimeBudgetMs { get; set; }

      [EditorEnforceOrder(154)]
      [EditorRange(5.0, 50.0)]
      public int PreviewProcessingTimeBudgetMs { get; set; }

      [EditorRange(1.0, 20.0)]
      [EditorEnforceOrder(158)]
      public int NavOverlayRuntimeBudgetMult { get; set; }

      public MapEditorSettings()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMapGenerationTimeBudgetMs\u003Ek__BackingField = 20;
        // ISSUE: reference to a compiler-generated field
        this.\u003CPreviewProcessingTimeBudgetMs\u003Ek__BackingField = 20;
        // ISSUE: reference to a compiler-generated field
        this.\u003CNavOverlayRuntimeBudgetMult\u003Ek__BackingField = 10;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    public class MapLoadConfig
    {
      [EditorEnforceOrder(173)]
      public readonly Action LoadSelectedMap;
      [EditorEnforceOrder(176)]
      public readonly Action RefreshMapsList;
      [EditorEnforceOrder(179)]
      public readonly Action OpenMapsDirectory;
      [EditorEnforceOrder(195)]
      public readonly Action ImportSelectedMap;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;
      private readonly Lyst<string> m_mapsToLoad;
      private readonly Lyst<string> m_mapsToLoadPaths;

      [EditorDropdown("m_mapsToLoad")]
      [EditorSection("Load map", "Loading a new map discards the current one.", true, false)]
      [EditorEnforceOrder(167)]
      public string MapToLoad { get; set; }

      [EditorEnforceOrder(170)]
      public bool IgnoreUnsavedChanges { get; set; }

      [EditorEnforceOrder(182)]
      public string LoadStatus { get; private set; }

      [EditorDropdown("m_mapsToLoad")]
      [EditorEnforceOrder(192)]
      [EditorSection("Import", "Imports all features of the selected map to the middle of the current view. This includes land masses, resources, trees, manually placed props, and starting locations. Unique/global features such as particle erosion are not imported. This operation can be reverted using undo.", true, false)]
      public string MapToImport { get; set; }

      [EditorEnforceOrder(198)]
      public string ImportStatus { get; private set; }

      [EditorSection("Autosave", null, true, false)]
      [EditorLabel(null, "Auto-save interval in minutes, set to zero to disable auto-save.", false, false)]
      [EditorEnforceOrder(204)]
      public int AutosaveIntervalMinutes { get; set; }

      [EditorEnforceOrder(207)]
      public string LastAutosaveStatus { get; private set; }

      public MapLoadConfig(Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: reference to a compiler-generated field
        this.\u003CAutosaveIntervalMinutes\u003Ek__BackingField = 10;
        this.m_mapsToLoad = new Lyst<string>();
        this.m_mapsToLoadPaths = new Lyst<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_editor = editor;
        this.LoadSelectedMap = new Action(this.loadMap);
        this.RefreshMapsList = new Action(this.refreshMapsList);
        this.OpenMapsDirectory = new Action(editor.openMapsDirectoryInExplorer);
        this.ImportSelectedMap = new Action(this.importMap);
        this.refreshMapsList();
      }

      public void SetAutosaveStatus(string status) => this.LastAutosaveStatus = status;

      private bool tryGetSelectedMapPath(string selectedMap, out string mapPath)
      {
        int index = this.m_mapsToLoad.IndexOf(selectedMap);
        if (index < 0)
        {
          mapPath = "";
          return false;
        }
        if (index < this.m_mapsToLoadPaths.Count)
        {
          mapPath = this.m_mapsToLoadPaths[index];
          return true;
        }
        Mafi.Log.Error("Invalid lengths of `MapsToLoadPaths` array");
        mapPath = "";
        return false;
      }

      private void loadMap()
      {
        if (this.m_editor.m_toolbarConfig.HasUnsavedChanges && !this.IgnoreUnsavedChanges)
        {
          this.LoadStatus = "Current map has unsaved changed.";
          this.m_editor.m_mapLoadConfigEditor.UpdateValues();
        }
        else
        {
          string mapPath;
          if (!this.tryGetSelectedMapPath(this.MapToLoad, out mapPath) || string.IsNullOrEmpty(mapPath))
          {
            this.LoadStatus = "Invalid selected map.";
          }
          else
          {
            Option<Exception> exception;
            if (!this.m_editor.m_mapSerializer.TryLoadMapFromFile(mapPath, out IWorldRegionMapPreviewData _, out IWorldRegionMapAdditionalData _, out IWorldRegionMap _, out exception))
            {
              this.LoadStatus = string.Format("Failed to load the selected map: {0}", (object) exception.ValueOrNull);
            }
            else
            {
              this.m_editor.m_main.LoadMapEditor(mapPath, ImmutableArray<ModData>.Empty);
              return;
            }
          }
          this.refreshMapsList();
          this.m_editor.m_mapLoadConfigEditor.UpdateValues();
        }
      }

      private void refreshMapsList()
      {
        this.m_mapsToLoad.Clear();
        this.m_mapsToLoadPaths.Clear();
        MapSerializer mapSerializer = this.m_editor.m_mapSerializer;
        LystStruct<(IWorldRegionMapPreviewData, string, bool)> lystStruct = new LystStruct<(IWorldRegionMapPreviewData, string, bool)>();
        foreach (string enumerateFile in Directory.EnumerateFiles("Maps", "*.map"))
        {
          IWorldRegionMapPreviewData previewData;
          if (mapSerializer.TryLoadPreviewMinimalFromFile(enumerateFile, out previewData, out Option<Exception> _) && !previewData.IsProtected)
            lystStruct.Add((previewData, Path.GetFullPath(enumerateFile), true));
        }
        foreach (FileInfo allFile in this.m_editor.m_fsHelper.GetAllFiles(FileType.Map))
        {
          IWorldRegionMapPreviewData previewData;
          if (mapSerializer.TryLoadPreviewMinimalFromFile(allFile.FullName, out previewData, out Option<Exception> _) && !previewData.IsProtected)
            lystStruct.Add((previewData, allFile.FullName, false));
        }
        DateTime failDate = new DateTime(2000, 1, 1);
        foreach ((IWorldRegionMapPreviewData regionMapPreviewData, string path, bool flag) in (IEnumerable<(IWorldRegionMapPreviewData, string, bool)>) lystStruct.AsEnumerable().OrderByDescending<(IWorldRegionMapPreviewData, string, bool), DateTime>((Func<(IWorldRegionMapPreviewData, string, bool), DateTime>) (x =>
        {
          IWorldRegionMapPreviewData regionMapPreviewData = x.Item1;
          return regionMapPreviewData == null ? failDate : regionMapPreviewData.LastEditedDateTimeUtc;
        })))
        {
          string fileName = Path.GetFileName(path);
          if (regionMapPreviewData == null)
          {
            this.m_mapsToLoad.Add("!! Failed to load\n" + fileName);
            this.m_mapsToLoadPaths.Add("");
          }
          else
          {
            float totalDays = (float) (DateTime.UtcNow - regionMapPreviewData.LastEditedDateTimeUtc).TotalDays;
            string str = "";
            if (!flag)
            {
              if ((double) totalDays > 1.0)
              {
                str = ", " + totalDays.ToStringRoundedAdaptive() + " days ago";
              }
              else
              {
                float number = totalDays * 24f;
                str = (double) number > 1.0 ? ", " + number.ToStringRoundedAdaptive() + " hours ago" : string.Format(", {0} minutes ago", (object) (number * 60f).RoundToInt());
              }
            }
            this.m_mapsToLoad.Add(regionMapPreviewData.Name + " \n" + regionMapPreviewData.MapSize.ToStringWithTimes() + str + "\n" + (regionMapPreviewData.IsPublished ? "" : "[WIP] ") + (flag ? "[built-in]" : fileName));
            this.m_mapsToLoadPaths.Add(path);
          }
        }
        if (this.m_mapsToLoad.IsEmpty)
        {
          this.m_mapsToLoad.Add("No maps to load");
          this.m_mapsToLoadPaths.Add("");
        }
        if (!this.m_mapsToLoad.Contains(this.MapToLoad))
          this.MapToLoad = this.m_mapsToLoad.First;
        if (this.m_mapsToLoad.Contains(this.MapToImport))
          return;
        this.MapToImport = this.m_mapsToLoad.First;
      }

      public void SelectMap(string filename)
      {
        int index = this.m_mapsToLoadPaths.IndexOf(filename);
        if (index >= 0)
          this.MapToLoad = this.m_mapsToLoad[index];
        else
          this.MapToLoad = this.m_mapsToLoad.First;
      }

      private void importMap()
      {
        string mapPath;
        if (!this.tryGetSelectedMapPath(this.MapToImport, out mapPath) || string.IsNullOrEmpty(mapPath))
        {
          this.ImportStatus = "Invalid selected map.";
        }
        else
        {
          IWorldRegionMap map;
          Option<Exception> exception;
          if (!this.m_editor.m_mapSerializer.TryLoadMapFromFile(mapPath, out IWorldRegionMapPreviewData _, out IWorldRegionMapAdditionalData _, out map, out exception))
          {
            this.ImportStatus = string.Format("Failed to import the selected map: {0}", (object) exception.ValueOrNull);
          }
          else
          {
            RectangleTerrainArea2i? affectedArea = this.importMapFeatures(map);
            this.m_editor.pushNewUndoSnapshot("Imported map from " + Path.GetFileName(mapPath), affectedArea);
            return;
          }
        }
        this.refreshMapsList();
        this.m_editor.m_mapLoadConfigEditor.UpdateValues();
      }

      private RectangleTerrainArea2i? importMapFeatures(IWorldRegionMap map)
      {
        RelTile2f relTile2f = new RelTile2f(this.m_editor.CameraController.CameraModel.State.PivotPosition) - map.Size.RelTile2f.HalfFast;
        RectangleTerrainArea2i? nullable = new RectangleTerrainArea2i?();
        this.ImportStatus = "";
        foreach (ITerrainFeatureBase feature in map.EnumerateAllFeatures().Where<ITerrainFeatureBase>((Func<ITerrainFeatureBase, bool>) (x => x.IsImportable)))
        {
          feature.TranslateBy(relTile2f.ExtendZ(Fix32.Zero));
          if (this.m_editor.tryAddNewFeature(feature, out string _, true))
          {
            RectangleTerrainArea2i otherArea = feature.GetBoundingBox() ?? this.m_editor.TerrainManager.TerrainArea;
            nullable = new RectangleTerrainArea2i?(nullable.HasValue ? nullable.GetValueOrDefault().Union(otherArea) : otherArea);
          }
          else
            Mafi.Log.Warning("Failed to import feature: " + feature.Name + " (" + feature.GetType().Name + ")");
        }
        return nullable;
      }
    }

    private readonly struct TextureWithMatrix
    {
      [EditorEnforceOrder(395)]
      public readonly Texture2D Texture;
      [EditorLabel(null, "Density of fog when this image was captured. You can edit this value and re-take all images automatically for this to take effect.", false, false)]
      [EditorEnforceOrder(400)]
      public readonly float FogDensity;
      [EditorLabel(null, "Size in KB of the image in the map file. Note that only thumbnail is stored in game save files, preview images only stored in the map file.", false, false)]
      [EditorEnforceOrder(405)]
      [EditorReadonly]
      public readonly int SizeKb;
      [EditorIgnore]
      public readonly UnityCameraPose? CameraPose;
      [EditorIgnore]
      public readonly UnityMatrix4? Matrix;

      public TextureWithMatrix(
        Texture2D texture,
        UnityCameraPose? cameraPose,
        UnityMatrix4? matrix,
        float fogDensity,
        int sizeKb)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Texture = texture;
        this.CameraPose = cameraPose;
        this.Matrix = matrix;
        this.FogDensity = fogDensity;
        this.SizeKb = sizeKb;
      }
    }

    public class MapSizeConfig : IEditableObject
    {
      private static readonly ImmutableArray<(string, RelTile2i)> STANDARD_MAP_SIZES;
      private string m_standardMapSizeValue;
      private readonly ImmutableArray<string> m_standardMapSizes;
      private RelTile2i m_oldSize;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;
      private readonly Action m_resizeMapAndRestart;
      private readonly Action m_translateFeatures;
      private readonly Area2iRenderer m_mapAreaRenderer;
      private readonly Area2iRenderer m_mapOffLimitsAreaRenderer;

      [EditorDropdown("m_standardMapSizes")]
      [EditorLabel(null, "Pick from a list of predefined map sizes or enter your own size below.", false, false)]
      [EditorSection("Size", null, true, false)]
      [EditorEnforceOrder(458)]
      public string StandardMapSizes { get; set; }

      [EditorEnforceOrder(468)]
      public RelTile2i MapSize { get; set; }

      [EditorEnforceOrder(472)]
      public string TotalMapArea { get; private set; }

      [EditorEnforceOrder(475)]
      public string Limits { get; }

      [EditorEnforceOrder(478)]
      public RelTile2i TranslateAllMapFeaturesBy { get; set; }

      [EditorEnforceOrder(481)]
      public string ResizeInfo { get; private set; }

      [EditorLabel("", null, false, false)]
      [EditorEnforceOrder(486)]
      [EditorReadonly]
      public string Warning { get; private set; }

      [EditorEnforceOrder(489)]
      public Action ResizeMapAndRestartEditor { get; private set; }

      [EditorEnforceOrder(492)]
      public Action TranslateAllFeatures { get; private set; }

      [EditorEnforceOrder(495)]
      public Action Reset { get; private set; }

      public MapSizeConfig(Mafi.Unity.MapEditor.MapEditor editor, IWorldRegionMap map)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_standardMapSizes = Mafi.Unity.MapEditor.MapEditor.MapSizeConfig.STANDARD_MAP_SIZES.Map<string>((Func<(string, RelTile2i), string>) (x =>
        {
          if (x.Item2 == RelTile2i.Zero)
            return "Custom";
          return x.Item1 + " " + x.Item2.ToStringWithTimes() + "\n" + Mafi.Unity.MapEditor.MapEditor.MapSizeConfig.getAreaTilesM(x.Item2) + "M tiles\u00B2";
        }));
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.MapSizeConfig mapSizeConfig = this;
        this.m_editor = editor;
        this.m_resizeMapAndRestart = new Action(this.resizeMapAndRestart);
        this.m_translateFeatures = new Action(this.translateFeatures);
        this.m_mapAreaRenderer = new Area2iRenderer(this.m_editor.m_linesFactory);
        this.m_mapAreaRenderer.SetColor(Color.red);
        this.m_mapAreaRenderer.SetWidth(3f);
        this.m_mapAreaRenderer.Show();
        this.m_mapOffLimitsAreaRenderer = new Area2iRenderer(this.m_editor.m_linesFactory);
        this.m_mapOffLimitsAreaRenderer.SetColor(new Color(1f, 0.5f, 0.0f));
        this.m_mapOffLimitsAreaRenderer.SetWidth(3f);
        this.m_mapOffLimitsAreaRenderer.Show();
        this.StandardMapSizes = this.m_standardMapSizeValue = this.m_standardMapSizes.First;
        this.Limits = "Max map area: " + 67.1088638f.RoundToSigDigits(3, false, false, false) + " km\u00B2\n" + string.Format("Max map dimension: {0} tiles\n", (object) 16384) + "Min map dimension: 256\nNote: Map dimensions must be a multiple of 256";
        this.Reset = (Action) (() =>
        {
          mapSizeConfig.MapSize = mapSizeConfig.m_oldSize = editor.m_map.Size;
          mapSizeConfig.TranslateAllMapFeaturesBy = RelTile2i.Zero;
        });
        this.UpdateValues(map);
      }

      public void SetAreasVisibility(bool visible)
      {
        if (visible)
        {
          this.m_mapAreaRenderer.Show();
          this.m_mapOffLimitsAreaRenderer.Show();
        }
        else
        {
          this.m_mapAreaRenderer.Hide();
          this.m_mapOffLimitsAreaRenderer.Hide();
        }
      }

      private void resizeMapAndRestart()
      {
        if (this.MapSize.IsZero || this.MapSize == this.m_editor.m_map.Size)
          return;
        if (this.TranslateAllMapFeaturesBy != RelTile2i.Zero)
          this.m_editor.applyTranslation(this.TranslateAllMapFeaturesBy.ExtendZ(0).CornerRelTile3f);
        this.m_editor.m_map.SetMapSize(this.MapSize);
        IMain main = this.m_editor.m_main;
        MapEditorConfig config = new MapEditorConfig();
        config.MapFileName = this.m_editor.m_mapPublishConfig.MapFileName;
        config.IsLoadedMapUnsaved = true;
        WorldRegionMap map = this.m_editor.m_map;
        WorldRegionMapPreviewData previewData = this.m_editor.m_previewData;
        WorldRegionMapAdditionalData additionalData = this.m_editor.m_additionalData;
        ImmutableArray<ModData> empty = ImmutableArray<ModData>.Empty;
        main.LoadMapEditor(config, (IWorldRegionMap) map, (IWorldRegionMapPreviewData) previewData, (IWorldRegionMapAdditionalData) additionalData, empty);
      }

      private void translateFeatures()
      {
        if (!(this.TranslateAllMapFeaturesBy != RelTile2i.Zero))
          return;
        this.m_editor.m_toolbarConfig.AreaToRegenerate = new RectangleTerrainArea2i?(this.m_editor.TerrainManager.TerrainArea);
        this.m_editor.clearAllFeaturesCaches();
        this.m_editor.applyTranslation(this.TranslateAllMapFeaturesBy.ExtendZ(0).CornerRelTile3f);
        this.UpdateValues((IWorldRegionMap) this.m_editor.m_map);
        this.m_editor.pushNewUndoSnapshot("Features translation", new RectangleTerrainArea2i?(this.m_editor.TerrainManager.TerrainArea));
      }

      private static string getAreaTilesM(RelTile2i size)
      {
        return size.ProductInt.Over(1000000).ToStringRoundedAdaptive();
      }

      public void UpdateValues(IWorldRegionMap map)
      {
        this.MapSize = this.m_oldSize = map.Size;
        this.TranslateAllMapFeaturesBy = RelTile2i.Zero;
        this.ObjectWasEdited();
      }

      private RelTile2i getSelectedMapSize()
      {
        int index = this.m_standardMapSizes.IndexOf(this.StandardMapSizes);
        return index < 0 ? RelTile2i.Zero : Mafi.Unity.MapEditor.MapEditor.MapSizeConfig.STANDARD_MAP_SIZES[index].Item2;
      }

      private int getMapSizeIndex(RelTile2i size)
      {
        return Mafi.Unity.MapEditor.MapEditor.MapSizeConfig.STANDARD_MAP_SIZES.IndexOf<RelTile2i>(size, (Func<(string, RelTile2i), RelTile2i, bool>) ((x, s) => x.Item2 == s)).Max(0);
      }

      public bool ObjectWasEdited()
      {
        if (this.StandardMapSizes != this.m_standardMapSizeValue)
        {
          this.m_standardMapSizeValue = this.StandardMapSizes;
          this.MapSize = this.getSelectedMapSize();
        }
        int x = this.MapSize.X.CeilToMultipleOf(256).Clamp(256, 16384);
        int y = this.MapSize.Y.CeilToMultipleOf(256).Clamp(256, 16384);
        if (x != this.m_oldSize.X)
        {
          if (x * y > 16777216)
            x = 16777216 / y;
        }
        else if (y != this.m_oldSize.Y && x * y > 16777216)
          y = 16777216 / x;
        this.MapSize = this.m_oldSize = new RelTile2i(x, y);
        this.m_standardMapSizeValue = this.StandardMapSizes = this.m_standardMapSizes[this.getMapSizeIndex(this.MapSize)];
        string stringRoundedAdaptive = ((float) (this.MapSize.ProductInt * 4) / 1000000f).ToStringRoundedAdaptive();
        this.TotalMapArea = Mafi.Unity.MapEditor.MapEditor.MapSizeConfig.getAreaTilesM(this.MapSize) + "M tiles\u00B2\n" + stringRoundedAdaptive + " km\u00B2";
        Tile2i origin = Tile2i.Zero - this.TranslateAllMapFeaturesBy;
        this.m_mapAreaRenderer.SetArea(new RectangleTerrainArea2i(origin, this.MapSize), new HeightTilesF(2));
        MapOffLimitsSize offLimitsSize = this.m_editor.m_map.OffLimitsSize;
        this.m_mapOffLimitsAreaRenderer.SetArea(new RectangleTerrainArea2i(origin + new RelTile2i(offLimitsSize.PlusX, offLimitsSize.PlusY), this.MapSize - new RelTile2i(offLimitsSize.MinusX + offLimitsSize.PlusX, offLimitsSize.MinusY + offLimitsSize.PlusY)), new HeightTilesF(2));
        RelTile2i terrainSize = this.m_editor.TerrainManager.TerrainSize;
        if (this.MapSize != terrainSize)
        {
          this.ResizeInfo = "Resize from: " + terrainSize.ToStringWithTimes() + "\nResize to: " + this.MapSize.ToStringWithTimes() + "\n" + string.Format("Delta: {0}\n", (object) (this.MapSize - terrainSize)) + (this.TranslateAllMapFeaturesBy != RelTile2i.Zero ? string.Format("Features translation: {0}", (object) this.TranslateAllMapFeaturesBy) : "");
          this.ResizeMapAndRestartEditor = this.m_resizeMapAndRestart;
          this.Warning = this.MapSize.Product > 9437184L ? "WARNING: Selected map size is very large, game performance may suffer." : "";
        }
        else if (this.TranslateAllMapFeaturesBy != RelTile2i.Zero)
        {
          this.ResizeInfo = string.Format("Features translation: {0}", (object) this.TranslateAllMapFeaturesBy);
          this.TranslateAllFeatures = this.m_translateFeatures;
        }
        else
        {
          this.ResizeInfo = "";
          this.ResizeMapAndRestartEditor = (Action) null;
          this.TranslateAllFeatures = (Action) null;
        }
        return true;
      }

      static MapSizeConfig()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        Mafi.Unity.MapEditor.MapEditor.MapSizeConfig.STANDARD_MAP_SIZES = ((ICollection<(string, RelTile2i)>) new (string, RelTile2i)[8]
        {
          ("Custom", RelTile2i.Zero),
          ("Tiny", new RelTile2i(1024, 1024)),
          ("Small", new RelTile2i(1536, 1536)),
          ("Standard", new RelTile2i(2048, 2048)),
          ("Large", new RelTile2i(3072, 3072)),
          ("Huge", new RelTile2i(4096, 4096)),
          ("Spaghetti", new RelTile2i(1024, 8192)),
          ("Ribbon", new RelTile2i(512, 16384))
        }).ToImmutableArray<(string, RelTile2i)>();
      }
    }

    private class TerrainFeaturesConfig
    {
      [EditorLabel("Template To Add", null, false, false)]
      [EditorSection("Add a new feature", null, true, false)]
      [EditorDropdown("m_templateNames")]
      [EditorEnforceOrder(670)]
      public string FeatureToAdd;
      private readonly ImmutableArray<string> m_templateNames;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;
      private readonly ImmutableArray<ITerrainFeatureTemplate> m_availableTemplates;
      [EditorEnforceOrder(677)]
      public readonly Action AddFeature;
      [EditorEnforceOrder(681)]
      [EditorSection("Terrain features", null, true, false)]
      public readonly Action EnableAll;
      [EditorEnforceOrder(684)]
      public readonly Action DisableAll;
      [EditorEnforceOrder(688)]
      public Lyst<Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper> TerrainFeatureGenerators;

      public TerrainFeaturesConfig(
        Mafi.Unity.MapEditor.MapEditor editor,
        ImmutableArray<ITerrainFeatureTemplate> availableTemplates)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.TerrainFeatureGenerators = new Lyst<Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.TerrainFeaturesConfig terrainFeaturesConfig = this;
        this.m_editor = editor;
        this.m_availableTemplates = availableTemplates;
        this.m_templateNames = availableTemplates.Map<string>((Func<ITerrainFeatureTemplate, int, string>) ((x, i) => string.Format("{0}: {1}{2}", (object) (i + 1), (object) x.Name, x.IsGlobal ? (object) " (global)" : (object) "")));
        this.FeatureToAdd = this.m_templateNames.FirstOrDefault() ?? "";
        this.AddFeature = (Action) (() => editor.tryAddAndSelectNewFeatureFromTemplate(terrainFeaturesConfig.GetSelectedFeatureToAdd()));
        this.EnableAll = (Action) (() => terrainFeaturesConfig.setAllEnabledState(true));
        this.DisableAll = (Action) (() => terrainFeaturesConfig.setAllEnabledState(false));
      }

      public Option<ITerrainFeatureTemplate> GetSelectedFeatureToAdd()
      {
        int index = this.m_templateNames.IndexOf(this.FeatureToAdd);
        return index < 0 ? Option<ITerrainFeatureTemplate>.None : this.m_availableTemplates[index].SomeOption<ITerrainFeatureTemplate>();
      }

      private void setAllEnabledState(bool enabled)
      {
        bool flag = false;
        foreach (Mafi.Unity.MapEditor.MapEditor.FeatureConfigWrapper featureGenerator in this.TerrainFeatureGenerators)
        {
          if (featureGenerator.IsEnabled != enabled)
          {
            featureGenerator.IsEnabled = enabled;
            flag = true;
          }
        }
        if (!flag)
          return;
        this.m_editor.m_terrainFeaturesConfigEditor.UpdateValues();
        this.m_editor.m_selectedFeatureObjectEditor.UpdateValues();
        this.m_editor.setNextUndoStateName("All features " + (enabled ? nameof (enabled) : "disabled"), new RectangleTerrainArea2i?(this.m_editor.TerrainManager.TerrainArea));
      }
    }

    private class PostProcessorsConfig
    {
      [EditorLabel("Post Processor To Add", null, false, false)]
      [EditorEnforceOrder(739)]
      [EditorDropdown("m_templateNames")]
      [EditorSection("Add a new post-processor", null, true, false)]
      public string PostProcessorToAdd;
      private readonly ImmutableArray<string> m_templateNames;
      private readonly ImmutableArray<ITerrainFeatureTemplate> m_availableTemplates;
      [EditorEnforceOrder(745)]
      public readonly Action AddPostProcessor;
      [EditorEnforceOrder(749)]
      [EditorSection("Terrain post-processors", null, true, false)]
      public readonly Action EnableAll;
      [EditorEnforceOrder(752)]
      public readonly Action DisableAll;
      [EditorEnforceOrder(756)]
      public Lyst<Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper> TerrainPostProcessors;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;

      public PostProcessorsConfig(
        Mafi.Unity.MapEditor.MapEditor editor,
        ImmutableArray<ITerrainFeatureTemplate> availableTemplates)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.TerrainPostProcessors = new Lyst<Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.PostProcessorsConfig processorsConfig = this;
        this.m_editor = editor;
        this.m_availableTemplates = availableTemplates;
        this.m_templateNames = availableTemplates.Map<string>((Func<ITerrainFeatureTemplate, int, string>) ((x, i) => string.Format("{0}: {1}{2}", (object) (i + 1), (object) x.Name, x.IsGlobal ? (object) " (global)" : (object) "")));
        this.PostProcessorToAdd = this.m_templateNames.FirstOrDefault() ?? "";
        this.AddPostProcessor = (Action) (() => editor.tryAddAndSelectNewFeatureFromTemplate(processorsConfig.GetSelectedFeatureToAdd()));
        this.EnableAll = (Action) (() => processorsConfig.setAllEnabledState(true));
        this.DisableAll = (Action) (() => processorsConfig.setAllEnabledState(false));
      }

      public Option<ITerrainFeatureTemplate> GetSelectedFeatureToAdd()
      {
        int index = this.m_templateNames.IndexOf(this.PostProcessorToAdd);
        return index < 0 ? Option<ITerrainFeatureTemplate>.None : this.m_availableTemplates[index].SomeOption<ITerrainFeatureTemplate>();
      }

      private void setAllEnabledState(bool enabled)
      {
        bool flag = false;
        foreach (Mafi.Unity.MapEditor.MapEditor.PostProcessorConfigWrapper terrainPostProcessor in this.TerrainPostProcessors)
        {
          if (terrainPostProcessor.IsEnabled != enabled)
          {
            terrainPostProcessor.IsEnabled = enabled;
            flag = true;
          }
        }
        if (!flag)
          return;
        this.m_editor.m_postProcessorsConfigEditor.UpdateValues();
        this.m_editor.m_selectedFeatureObjectEditor.UpdateValues();
        this.m_editor.setNextUndoStateName("All post-processors " + (enabled ? nameof (enabled) : "disabled"), new RectangleTerrainArea2i?(this.m_editor.TerrainManager.TerrainArea));
      }
    }

    private class VirtualResourcesConfig
    {
      [EditorDropdown("m_templateNames")]
      [EditorSection("Add a new virtual resource", null, true, false)]
      [EditorEnforceOrder(808)]
      [EditorLabel("Virtual Resource To Add", null, false, false)]
      public string VirtualResourceToAdd;
      private readonly ImmutableArray<string> m_templateNames;
      private readonly ImmutableArray<ITerrainFeatureTemplate> m_availableTemplates;
      [EditorEnforceOrder(814)]
      public readonly Action AddVirtualResource;
      [EditorEnforceOrder(818)]
      [EditorSection("Virtual resources", null, true, false)]
      public readonly Action EnableAll;
      [EditorEnforceOrder(821)]
      public readonly Action DisableAll;
      [EditorEnforceOrder(824)]
      public readonly Lyst<Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper> VirtualTerrainResourceGenerators;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;

      public VirtualResourcesConfig(
        Mafi.Unity.MapEditor.MapEditor editor,
        ImmutableArray<ITerrainFeatureTemplate> availableTemplates)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.VirtualTerrainResourceGenerators = new Lyst<Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.VirtualResourcesConfig virtualResourcesConfig = this;
        this.m_editor = editor;
        this.m_availableTemplates = availableTemplates;
        this.m_templateNames = availableTemplates.Map<string>((Func<ITerrainFeatureTemplate, int, string>) ((x, i) => string.Format("{0}: {1}{2}", (object) (i + 1), (object) x.Name, x.IsGlobal ? (object) " (global)" : (object) "")));
        this.VirtualResourceToAdd = this.m_templateNames.FirstOrDefault() ?? "";
        this.AddVirtualResource = (Action) (() => editor.tryAddAndSelectNewFeatureFromTemplate(virtualResourcesConfig.GetSelectedFeatureToAdd()));
        this.EnableAll = (Action) (() => virtualResourcesConfig.setAllEnabledState(true));
        this.DisableAll = (Action) (() => virtualResourcesConfig.setAllEnabledState(false));
      }

      public Option<ITerrainFeatureTemplate> GetSelectedFeatureToAdd()
      {
        int index = this.m_templateNames.IndexOf(this.VirtualResourceToAdd);
        return index < 0 ? Option<ITerrainFeatureTemplate>.None : this.m_availableTemplates[index].SomeOption<ITerrainFeatureTemplate>();
      }

      private void setAllEnabledState(bool enabled)
      {
        bool flag = false;
        foreach (Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper resourceGenerator in this.VirtualTerrainResourceGenerators)
        {
          flag |= resourceGenerator.IsEnabled != enabled;
          resourceGenerator.IsEnabled = enabled;
        }
        if (!flag)
          return;
        this.m_editor.m_virtualResourcesConfigEditor.UpdateValues();
        this.m_editor.m_selectedFeatureObjectEditor.UpdateValues();
        this.m_editor.setNextUndoStateName("All virtual resources " + (enabled ? nameof (enabled) : "disabled"), new RectangleTerrainArea2i?());
      }
    }

    private class StartingLocationsConfig
    {
      [EditorEnforceOrder(871)]
      public readonly Action AddStartingLocation;
      [EditorEnforceOrder(875)]
      [EditorSection("Starting locations", null, true, false)]
      public readonly Lyst<Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper> StartingLocations;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;

      public StartingLocationsConfig(Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.StartingLocations = new Lyst<Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_editor = editor;
        this.AddStartingLocation = new Action(this.addStartingLocation);
      }

      private void addStartingLocation()
      {
        string error;
        this.m_editor.tryAddAndSelectNewFeature((ITerrainFeatureBase) new StartingLocationV2(new StartingLocationV2.Configuration()
        {
          Position = this.m_editor.CameraController.CameraModel.State.PivotPosition3f.Tile3i
        }), out error).AssertTrue(error);
      }
    }

    private class HistoryView
    {
      public Action Undo;
      public Action Redo;
      public readonly Lyst<string> History;

      public HistoryView(Action undo, Action redo)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.History = new Lyst<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Undo = undo;
        this.Redo = redo;
      }

      public void UpdateValues(UndoRedoManager<Mafi.Unity.MapEditor.MapEditor.UndoState> undoManager)
      {
        this.History.Clear();
        for (int index = 0; index < undoManager.UndoStates.Count; ++index)
        {
          Mafi.Unity.MapEditor.MapEditor.UndoState undoState = undoManager.UndoStates[index];
          int length = undoState.MapData.Length;
          this.History.Add(string.Format("{0}{1} ({2} kB)", index < undoManager.CurrentStateIndex ? (object) "(x) " : (index == undoManager.CurrentStateIndex ? (object) "=> " : (object) ""), (object) undoState.Name, (object) (length / 1024)));
        }
      }
    }

    public class NoiseParserHelpView
    {
      [EditorSection("Initial statements", "Statements that can only start a new chain of functions.", true, false)]
      public Lyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementHelp> InitialStatements { get; set; }

      [EditorSection("Transform statements", "Statements that can only continue a chain of functions.", true, false)]
      public Lyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementHelp> TransformStatements { get; set; }

      public NoiseParserHelpView(ConfigurableNoise2dParser parser)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: reference to a compiler-generated field
        this.\u003CInitialStatements\u003Ek__BackingField = new Lyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementHelp>();
        // ISSUE: reference to a compiler-generated field
        this.\u003CTransformStatements\u003Ek__BackingField = new Lyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementHelp>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        foreach (ConfigurableNoise2dParser.InitialStatementData initialStatementData in parser.InitialStatements.Values)
          this.InitialStatements.Add(new Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementHelp(initialStatementData.Name, initialStatementData.Description.ValueOr(""), initialStatementData.Parameters.Select<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp>((Func<ConfigurableNoise2dParamSpec, Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp>) (x => new Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp(x.Name, x.Type.Name))).ToLyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp>()));
        foreach (ConfigurableNoise2dParser.TransformStatementData transformStatementData in parser.TransformStatements.Values)
          this.TransformStatements.Add(new Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementHelp(transformStatementData.Name, transformStatementData.Description.ValueOr(""), transformStatementData.Parameters.Select<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp>((Func<ConfigurableNoise2dParamSpec, Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp>) (x => new Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp(x.Name, x.Type.Name))).ToLyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp>()));
      }

      public readonly struct StatementHelp
      {
        public readonly string Statement;
        [EditorTextArea(2, true)]
        public readonly string Description;
        public readonly Lyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp> Parameters;

        public StatementHelp(
          string statement,
          string description,
          Lyst<Mafi.Unity.MapEditor.MapEditor.NoiseParserHelpView.StatementParamHelp> parameters)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Statement = statement;
          this.Description = description;
          this.Parameters = parameters;
        }
      }

      public readonly struct StatementParamHelp
      {
        public readonly string Name;
        public readonly string Type;

        public StatementParamHelp(string name, string type)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          this.Name = name;
          this.Type = type;
        }
      }
    }

    public class TerrainLayersView
    {
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;

      [EditorLabel("", null, false, false)]
      public string Help
      {
        get => "Left-click on the terrain while holding Ctr to list the layers of the tile.";
      }

      public Tile2f TilePosition { get; set; }

      public HeightTilesF Height { get; set; }

      public RelTile2f Slope { get; set; }

      public Lyst<string> Layers { get; set; }

      public TerrainLayersView(Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: reference to a compiler-generated field
        this.\u003CLayers\u003Ek__BackingField = new Lyst<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_editor = editor;
      }

      public void ShowTileInfo(Tile3f pos)
      {
        TerrainManager terrainManager = this.m_editor.TerrainManager;
        Tile2iIndex tileIndex = terrainManager.GetTileIndex(pos.Tile2i);
        this.TilePosition = pos.Xy;
        this.Height = pos.Height;
        this.Layers.Clear();
        foreach (TerrainMaterialThicknessSlim enumerateLayer in terrainManager.EnumerateLayers(tileIndex))
        {
          TerrainMaterialProto full = enumerateLayer.SlimId.ToFull(terrainManager);
          this.Layers.Add(string.Format("{0} of {1}", (object) enumerateLayer.Thickness, (object) full.Strings.Name));
        }
        if (terrainManager.IsOnMapBoundary(tileIndex))
          return;
        int terrainWidth = terrainManager.TerrainWidth;
        ThicknessTilesF abs1 = (terrainManager.GetHeight(tileIndex.MinusXNeighborUnchecked) - this.Height).Abs;
        ref ThicknessTilesF local1 = ref abs1;
        ThicknessTilesF thicknessTilesF1 = terrainManager.GetHeight(tileIndex.PlusXNeighborUnchecked) - this.Height;
        ThicknessTilesF abs2 = thicknessTilesF1.Abs;
        ThicknessTilesF thicknessTilesF2 = local1.Max(abs2);
        ThicknessTilesF abs3 = (terrainManager.GetHeight(tileIndex.MinusYNeighborUnchecked(terrainWidth)) - this.Height).Abs;
        ref ThicknessTilesF local2 = ref abs3;
        thicknessTilesF1 = terrainManager.GetHeight(tileIndex.PlusYNeighborUnchecked(terrainWidth)) - this.Height;
        ThicknessTilesF abs4 = thicknessTilesF1.Abs;
        ThicknessTilesF thicknessTilesF3 = local2.Max(abs4);
        this.Slope = new RelTile2f(thicknessTilesF2.Value, thicknessTilesF3.Value);
      }
    }

    public enum OverlayMode
    {
      None,
      Pathability,
      TerrainStability,
    }

    internal readonly struct UndoState
    {
      public readonly string Name;
      public readonly RectangleTerrainArea2i? AffectedArea;
      public readonly byte[] MapData;

      public UndoState(string name, RectangleTerrainArea2i? affectedArea, byte[] mapData)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Name = name;
        this.AffectedArea = affectedArea;
        this.MapData = mapData;
      }
    }

    private class MapPublishConfig : IEditableObject
    {
      [EditorEnforceOrder(58)]
      [EditorButton(null, "Thumbnail is a small image that is shown in the maps list when selecting a map.", false, ObjEditorIcon.None)]
      [EditorSection("Images", null, true, false)]
      public readonly Action TakeNewThumbnail;
      [EditorButton(null, "Larger image that is shown in the maps detail view.", false, ObjEditorIcon.None)]
      [EditorEnforceOrder(62)]
      public readonly Action TakeNewScreenshot;
      [EditorButton(null, "Re-takes all images from the same camera positions.", false, ObjEditorIcon.None)]
      [EditorEnforceOrder(66)]
      public readonly Action RetakeAllImages;
      [EditorButton(null, "Specifies fog intensity in captured screenshots. Low values might cause rendering artifact to sho in the distance.", false, ObjEditorIcon.None)]
      [EditorEnforceOrder(71)]
      public Percent FogIntensityInScreenshots;
      [EditorEnforceOrder(78)]
      [EditorCollection(true, true)]
      public readonly Lyst<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix> Images;
      private string m_fileValidationMessage;
      [EditorEnforceOrder(96)]
      [EditorButton(null, "Computes metadata and saves a finalized version of map ready to be played.", true, ObjEditorIcon.None)]
      public readonly Action PublishMap;
      [EditorEnforceOrder(99)]
      public readonly Action OpenMapsDirectory;
      [EditorEnforceOrder(102)]
      public readonly Action ComputeTotalResources;
      private string m_resultsSummaryError;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;
      private LystStruct<Pair<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix, EncodedImageAndMatrix>> m_imagesData;

      [EditorLabel(null, "The in-game name of the map.", false, false)]
      [EditorEnforceOrder(37)]
      [EditorMaxLength(32)]
      [EditorSection("Map preview data", null, true, false)]
      public string MapName { get; set; }

      [EditorMaxLength(1000)]
      [EditorTextArea(3, true)]
      [EditorLabel(null, "Map description that will be visible in the map detail view in game.", false, false)]
      [EditorEnforceOrder(44)]
      public string Description { get; set; }

      [EditorEnforceOrder(48)]
      [EditorMaxLength(32)]
      public string AuthorName { get; set; }

      [EditorEnforceOrder(52)]
      [EditorRange(0.0, 99999.0)]
      public int Version { get; set; }

      [EditorEnforceOrder(74)]
      public Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix Thumbnail { get; set; }

      [EditorSection("Publishing", null, true, false)]
      [EditorEnforceOrder(86)]
      [EditorMaxLength(30)]
      [EditorLabel(null, "File name of the map, used for saves, auto-saves, and publishing. Keep this similar to the in-game map name to avoid confusion when sharing.", false, false)]
      [EditorValidationSource("m_fileValidationMessage")]
      public string MapFileName { get; set; }

      [EditorEnforceOrder(91)]
      [EditorLabel(null, "Whether to allow overwriting existing files during publishing.", false, false)]
      public bool AllowFileOverwrite { get; set; }

      [EditorEnforceOrder(105)]
      public bool IgnoreFeaturesOutOfOrder { get; set; }

      [EditorEnforceOrder(108)]
      public bool IgnoreUniqueFeaturesConstraint { get; set; }

      [EditorValidationSource("m_resultsSummaryError")]
      [EditorTextArea(4, true)]
      [EditorEnforceOrder(114)]
      [EditorSection("Publish results", null, true, false)]
      public string ResultsSummary { get; set; }

      [EditorEnforceOrder(119)]
      public Option<Mafi.Unity.MapEditor.MapEditor.MapPublishConfig.PublishResultsData> PublishResultDetails { get; set; }

      public MapPublishConfig(Mafi.Unity.MapEditor.MapEditor editor, string mapFileName)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.FogIntensityInScreenshots = 10.Percent();
        this.Images = new Lyst<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.MapPublishConfig mapPublishConfig = this;
        this.m_editor = editor;
        this.MapFileName = mapFileName;
        this.PublishMap = new Action(editor.publishMap);
        this.OpenMapsDirectory = new Action(editor.openMapsDirectoryInExplorer);
        this.ComputeTotalResources = (Action) (() =>
        {
          editor.computeTotalResources().EnumerateToTheEnd<RegenProgress>();
          mapPublishConfig.SetPublishResults(Option<string>.None, (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[1]
          {
            "Resourced recomputed"
          }));
        });
        this.TakeNewThumbnail = new Action(this.takeThumbnailScreenshot);
        this.TakeNewScreenshot = new Action(this.takePreviewScreenshot);
        this.RetakeAllImages = new Action(this.retakeAllScreenshots);
        this.UpdateValues();
        this.ObjectWasEdited();
      }

      public bool ObjectWasEdited()
      {
        string mapFileName = this.MapFileName;
        string validationMessage = this.m_fileValidationMessage;
        this.MapFileName = this.MapFileName.Trim();
        this.m_fileValidationMessage = (string) null;
        if (FileSystemHelper.ContainsInvalidFileNameCharacters(this.MapFileName))
          this.m_fileValidationMessage = "Contains invalid characters.";
        else if (!this.AllowFileOverwrite && File.Exists(this.m_editor.m_fsHelper.GetFilePath(this.MapFileName, FileType.Map)))
          this.m_fileValidationMessage = "File already exists";
        return mapFileName != this.MapName || validationMessage != this.m_fileValidationMessage;
      }

      public void UpdateValues()
      {
        WorldRegionMapPreviewData previewData = this.m_editor.m_previewData;
        WorldRegionMapAdditionalData additionalData = this.m_editor.m_additionalData;
        this.MapName = previewData.Name;
        this.Description = previewData.Description;
        this.AuthorName = previewData.AuthorName;
        this.Version = previewData.MapVersion;
        this.Thumbnail = this.GetTextureForData(previewData.ThumbnailImageData).GetValueOrDefault();
        this.Images.Clear();
        foreach (EncodedImageAndMatrix data in additionalData.PreviewImagesData)
        {
          Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix? textureForData = this.GetTextureForData(data);
          if (textureForData.HasValue)
            this.Images.Add(textureForData.Value);
          else
            Mafi.Log.Error("Failed to read map preview image");
        }
      }

      public void ClearPublishResults()
      {
        this.PublishResultDetails = Option<Mafi.Unity.MapEditor.MapEditor.MapPublishConfig.PublishResultsData>.None;
        this.m_editor.m_mapPublishConfigEditor.UpdateValues();
      }

      public void SetPublishResults(Option<string> errorMessage, IEnumerable<string> results)
      {
        this.ResultsSummary = results.AsEnumerable<string>().Select<string, string>((Func<string, int, string>) ((x, i) => string.Format("{0}: {1}\n", (object) (i + 1), (object) x))).JoinStrings();
        if (errorMessage.HasValue && this.ResultsSummary.Length == 0)
        {
          Mafi.Log.Warning("Unknown publish error occurred");
          this.ResultsSummary = "Unknown error occurred";
        }
        this.m_resultsSummaryError = errorMessage.ValueOrNull;
        if (errorMessage.HasValue)
        {
          this.ClearPublishResults();
        }
        else
        {
          WorldRegionMapPreviewData previewData = this.m_editor.m_previewData;
          WorldRegionMapAdditionalData additionalData = this.m_editor.m_additionalData;
          Mafi.Unity.MapEditor.MapEditor.MapPublishConfig.PublishResultsData publishResultsData = new Mafi.Unity.MapEditor.MapEditor.MapPublishConfig.PublishResultsData();
          this.PublishResultDetails = (Option<Mafi.Unity.MapEditor.MapEditor.MapPublishConfig.PublishResultsData>) publishResultsData;
          publishResultsData.NonOceanTilesCount = additionalData.NonOceanTilesCount;
          publishResultsData.FlatNonOceanTilesCount = additionalData.FlatNonOceanTilesCount;
          // ISSUE: method pointer
          publishResultsData.EasyToReachTerrainResourcesStats = additionalData.EasyToReachTerrainResourcesStats.Select<string>(new Func<MapTerrainResourceStats, string>((object) this, __methodptr(\u003CSetPublishResults\u003Eg__getMaterialSummary\u007C61_3))).JoinStrings("\n");
          // ISSUE: method pointer
          publishResultsData.TotalTerrainResourcesStats = additionalData.TotalTerrainResourcesStats.Select<string>(new Func<MapTerrainResourceStats, string>((object) this, __methodptr(\u003CSetPublishResults\u003Eg__getMaterialSummary\u007C61_3))).JoinStrings("\n");
          // ISSUE: method pointer
          publishResultsData.EasyToReachOtherResourcesStats = additionalData.EasyToReachOtherResourcesStats.Select<string>(new Func<MapOtherResourceStats, string>((object) this, __methodptr(\u003CSetPublishResults\u003Eg__getResourcesSummary\u007C61_4))).JoinStrings("\n");
          // ISSUE: method pointer
          publishResultsData.TotalOtherResourcesStats = additionalData.TotalOtherResourcesStats.Select<string>(new Func<MapOtherResourceStats, string>((object) this, __methodptr(\u003CSetPublishResults\u003Eg__getResourcesSummary\u007C61_4))).JoinStrings("\n");
          // ISSUE: method pointer
          publishResultsData.EasyToReachProductStats = additionalData.EasyToReachProductStats.Select<string>(new Func<MapProductStats, string>((object) this, __methodptr(\u003CSetPublishResults\u003Eg__getProductSummary\u007C61_5))).JoinStrings("\n");
          // ISSUE: method pointer
          publishResultsData.TotalProductStats = additionalData.TotalProductStats.Select<string>(new Func<MapProductStats, string>((object) this, __methodptr(\u003CSetPublishResults\u003Eg__getProductSummary\u007C61_5))).JoinStrings("\n");
          publishResultsData.ResourceLocations = additionalData.ResourceLocations.Select<string>((Func<MapResourceLocation, string>) (x => string.Format("{0} at {1}", (object) getProductOrNull(x.ProductProtoId)?.Strings.Name, (object) x.Position))).JoinStrings("\n");
          this.m_editor.m_mapPublishConfigEditor.UpdateValues();
        }

        ProductProto getProductOrNull(ProductProto.ID id)
        {
          return this.m_editor.m_protosDb.Get<ProductProto>((Proto.ID) id).ValueOrNull;
        }
      }

      public Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix? GetTextureForData(
        EncodedImageAndMatrix data)
      {
        if (data.ImageData == null || data.ImageData.Length == 0)
          return new Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix?();
        int index = this.m_imagesData.IndexOf<EncodedImageAndMatrix>(data, (Func<Pair<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix, EncodedImageAndMatrix>, EncodedImageAndMatrix>) (x => x.Second));
        if (index > 0)
          return new Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix?(this.m_imagesData[index].First);
        Texture2D texture2D = new Texture2D(2, 2);
        if (!texture2D.LoadImage(data.ImageData))
          return new Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix?();
        Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix first = new Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix(texture2D, data.CameraPose, data.ViewProjectionMatrix, data.FogDensity, data.ImageData.Length / 1024);
        this.m_imagesData.Add(Pair.Create<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix, EncodedImageAndMatrix>(first, data));
        return new Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix?(first);
      }

      public EncodedImageAndMatrix? GetDataForTexture(Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix tex)
      {
        if (!(bool) (UnityEngine.Object) tex.Texture)
          return new EncodedImageAndMatrix?();
        int index = this.m_imagesData.IndexOf<Texture2D>(tex.Texture, (Func<Pair<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix, EncodedImageAndMatrix>, Texture2D>) (x => x.First.Texture));
        if (index > 0)
          return new EncodedImageAndMatrix?(this.m_imagesData[index].Second);
        EncodedImageAndMatrix second = new EncodedImageAndMatrix(tex.Texture.EncodeToJPG(80), tex.CameraPose, tex.Matrix, tex.FogDensity);
        this.m_imagesData.Add(Pair.Create<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix, EncodedImageAndMatrix>(tex, second));
        return new EncodedImageAndMatrix?(second);
      }

      private ScreenshotConfigFlags resetEditorConfigForScreenshotTaking()
      {
        this.m_editor.selectFeatureEditor(Option<TerrainFeatureEditor>.None);
        Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig toolbarConfig = this.m_editor.m_toolbarConfig;
        toolbarConfig.ShowResourcesBars = false;
        toolbarConfig.TransparentOcean = false;
        toolbarConfig.DisableTerrainDetails = false;
        this.m_editor.OnToolbarConfigChanged(true);
        return (ScreenshotConfigFlags) 0;
      }

      private void takeThumbnailScreenshot()
      {
        ScreenshotConfigFlags extraFlags = this.resetEditorConfigForScreenshotTaking();
        this.m_editor.m_gameLoopEvents.InvokeInSyncNotSaved((Action) (() => this.takeThumbnailScreenshotInternal(extraFlags)));
      }

      private void takeThumbnailScreenshotInternal(
        ScreenshotConfigFlags extraFlags,
        Pose? cameraPose = null,
        float? fov = null,
        float? fogDensity = null)
      {
        float fog = (float) ((double) fogDensity ?? (double) this.m_editor.m_mapPublishConfig.FogIntensityInScreenshots.Apply(0.0012f));
        ScreenshotTaker screenshotTaker = this.m_editor.m_screenshotTaker;
        int x = Mafi.Unity.MapEditor.MapEditor.MAP_THUMBNAIL_SIZE.X;
        int y = Mafi.Unity.MapEditor.MapEditor.MAP_THUMBNAIL_SIZE.Y;
        Vector3? position = cameraPose?.position;
        Quaternion? rotation = cameraPose?.rotation;
        float? nullable1 = fov;
        ScreenshotConfigFlags screenshotConfigFlags = ScreenshotConfigFlags.DisableIcons | ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars | ScreenshotConfigFlags.Force8xMsaa | ScreenshotConfigFlags.ForceLod0 | ScreenshotConfigFlags.ForceNoChunkCulling | extraFlags;
        float? nullable2 = new float?(fog);
        Option<string> filePathWithoutExt = new Option<string>();
        Action<Option<Texture2D>, UnityEngine.Camera, string> encodedDataCallback = (Action<Option<Texture2D>, UnityEngine.Camera, string>) ((texture, camera, error) =>
        {
          if (texture.IsNone || !string.IsNullOrEmpty(error))
            return;
          Pose p = new Pose(camera.transform.position, camera.transform.rotation);
          Matrix4x4 imageTransformMatrix = CameraUtils.GetWorldToImageTransformMatrix(texture.Value.width, texture.Value.height, camera);
          this.Thumbnail = this.GetTextureForData(new EncodedImageAndMatrix(texture.Value.EncodeToJPG(90), new UnityCameraPose?(p.ToMafiCameraPose(camera.fieldOfView)), new UnityMatrix4?(imageTransformMatrix.ToMafiMatrix()), fog)).GetValueOrDefault();
          this.m_editor.m_mapPublishConfigChanged(true);
          this.m_editor.m_mapPublishConfigEditor.UpdateValues();
        });
        int customWidth = x;
        int customHeight = y;
        Vector3? cameraPosition = position;
        Quaternion? cameraRotation = rotation;
        float? verticalFieldOfView = nullable1;
        Vector2? nearFarClipPlane = new Vector2?();
        int configFlags = (int) screenshotConfigFlags;
        float? customFogDensity = nullable2;
        screenshotTaker.ScheduleScreenshotCapture(filePathWithoutExt, encodedDataCallback, customWidth, customHeight, cameraPosition, cameraRotation, verticalFieldOfView, nearFarClipPlane, true, configFlags: (ScreenshotConfigFlags) configFlags, customFogDensity: customFogDensity);
      }

      private void takePreviewScreenshot()
      {
        ScreenshotConfigFlags extraFlags = this.resetEditorConfigForScreenshotTaking();
        this.m_editor.m_gameLoopEvents.InvokeInSyncNotSaved((Action) (() => this.takePreviewScreenshotInternal(extraFlags)));
      }

      private void takePreviewScreenshotInternal(
        ScreenshotConfigFlags extraFlags,
        Pose? cameraPose = null,
        float? fov = null,
        float? fogDensity = null)
      {
        float fog = (float) ((double) fogDensity ?? (double) this.m_editor.m_mapPublishConfig.FogIntensityInScreenshots.Apply(0.0012f));
        ScreenshotTaker screenshotTaker = this.m_editor.m_screenshotTaker;
        int x = Mafi.Unity.MapEditor.MapEditor.MAP_PREVIEW_SIZE.X;
        int y = Mafi.Unity.MapEditor.MapEditor.MAP_PREVIEW_SIZE.Y;
        Vector3? position = cameraPose?.position;
        Quaternion? rotation = cameraPose?.rotation;
        float? nullable1 = fov;
        ScreenshotConfigFlags screenshotConfigFlags = ScreenshotConfigFlags.DisableIcons | ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars | ScreenshotConfigFlags.Force8xMsaa | ScreenshotConfigFlags.ForceLod0 | ScreenshotConfigFlags.ForceNoChunkCulling | extraFlags;
        float? nullable2 = new float?(fog);
        Option<string> filePathWithoutExt = new Option<string>();
        Action<Option<Texture2D>, UnityEngine.Camera, string> encodedDataCallback = (Action<Option<Texture2D>, UnityEngine.Camera, string>) ((texture, camera, error) =>
        {
          if (texture.IsNone || !string.IsNullOrEmpty(error))
            return;
          Pose p = new Pose(camera.transform.position, camera.transform.rotation);
          Matrix4x4 imageTransformMatrix = CameraUtils.GetWorldToImageTransformMatrix(texture.Value.width, texture.Value.height, camera);
          Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix? textureForData = this.GetTextureForData(new EncodedImageAndMatrix(texture.Value.EncodeToJPG(80), new UnityCameraPose?(p.ToMafiCameraPose(camera.fieldOfView)), new UnityMatrix4?(imageTransformMatrix.ToMafiMatrix()), fog));
          if (!textureForData.HasValue)
            return;
          this.Images.Add(textureForData.Value);
          this.m_editor.m_mapPublishConfigChanged(true);
          this.m_editor.m_mapPublishConfigEditor.UpdateValues();
        });
        int customWidth = x;
        int customHeight = y;
        Vector3? cameraPosition = position;
        Quaternion? cameraRotation = rotation;
        float? verticalFieldOfView = nullable1;
        Vector2? nearFarClipPlane = new Vector2?();
        int configFlags = (int) screenshotConfigFlags;
        float? customFogDensity = nullable2;
        screenshotTaker.ScheduleScreenshotCapture(filePathWithoutExt, encodedDataCallback, customWidth, customHeight, cameraPosition, cameraRotation, verticalFieldOfView, nearFarClipPlane, true, configFlags: (ScreenshotConfigFlags) configFlags, customFogDensity: customFogDensity);
      }

      private void retakeAllScreenshots()
      {
        ScreenshotConfigFlags extraFlags = this.resetEditorConfigForScreenshotTaking();
        this.m_editor.m_gameLoopEvents.InvokeInSyncNotSaved((Action) (() =>
        {
          if (this.Thumbnail.CameraPose.HasValue)
          {
            float verticalFieldOfView;
            this.takeThumbnailScreenshotInternal(extraFlags, new Pose?(this.Thumbnail.CameraPose.Value.ToUnityCameraPose(out verticalFieldOfView)), new float?(verticalFieldOfView), new float?(this.Thumbnail.FogDensity));
          }
          foreach (Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix image in this.Images)
          {
            if (image.CameraPose.HasValue)
            {
              float verticalFieldOfView;
              this.takePreviewScreenshotInternal(extraFlags, new Pose?(image.CameraPose.Value.ToUnityCameraPose(out verticalFieldOfView)), new float?(verticalFieldOfView), new float?(image.FogDensity));
            }
          }
          this.Images.RemoveWhere((Predicate<Mafi.Unity.MapEditor.MapEditor.TextureWithMatrix>) (x => x.CameraPose.HasValue));
        }));
      }

      private void testImageTransformMatrices()
      {
        for (int index1 = 0; index1 < this.m_editor.m_additionalData.PreviewImagesData.Length; ++index1)
        {
          EncodedImageAndMatrix encodedImageAndMatrix = this.m_editor.m_additionalData.PreviewImagesData[index1];
          if (encodedImageAndMatrix.ViewProjectionMatrix.HasValue && !encodedImageAndMatrix.ImageData.IsNullOrEmpty<byte>())
          {
            string fullPath = Path.GetFullPath(string.Format("Preview{0}.jpg", (object) index1));
            StringBuilder stringBuilder = new StringBuilder(1024);
            stringBuilder.AppendLine(string.Format("Preview #{0} saved to: {1}", (object) index1, (object) fullPath));
            Matrix4x4 unityMatrix = encodedImageAndMatrix.ViewProjectionMatrix.Value.ToUnityMatrix();
            for (int index2 = 0; index2 < this.m_editor.m_startingLocationsConfig.StartingLocations.Count; ++index2)
            {
              Tile3f centerTile3f = this.m_editor.m_startingLocationsConfig.StartingLocations[index2].StartingLocation.ToPreview().Position.CenterTile3f;
              stringBuilder.AppendLine(string.Format("Start loc #{0}: {1} => {2}", (object) index2, (object) centerTile3f, (object) transform(centerTile3f, unityMatrix)));
            }
            for (int index3 = 0; index3 < this.m_editor.m_terrainFeaturesConfig.TerrainFeatureGenerators.Count; ++index3)
            {
              if (this.m_editor.m_terrainFeaturesConfig.TerrainFeatureGenerators[index3].FeatureGenerator is PolygonTerrainFeatureGenerator featureGenerator)
              {
                Tile3f position3f = featureGenerator.GetPosition3f();
                stringBuilder.AppendLine(string.Format("Res loc #{0}: {1} => {2} ({3})", (object) index3, (object) position3f, (object) transform(position3f, unityMatrix), (object) featureGenerator.ConfigMutable.TerrainMaterial));
              }
            }
            File.WriteAllBytes(fullPath, encodedImageAndMatrix.ImageData);
            Mafi.Log.Error(stringBuilder.ToString());
          }
        }

        static Vector3 transform(Tile3f t, Matrix4x4 m) => m.MultiplyPoint(t.ToVector3());
      }

      public class PublishResultsData
      {
        [EditorEnforceOrder(125)]
        [EditorReadonly]
        public int NonOceanTilesCount { get; set; }

        [EditorEnforceOrder(129)]
        [EditorReadonly]
        public int FlatNonOceanTilesCount { get; set; }

        [EditorTextArea(4, true)]
        [EditorEnforceOrder(133)]
        public string EasyToReachProductStats { get; set; }

        [EditorEnforceOrder(137)]
        [EditorTextArea(4, true)]
        public string TotalProductStats { get; set; }

        [EditorEnforceOrder(141)]
        [EditorTextArea(4, true)]
        public string EasyToReachTerrainResourcesStats { get; set; }

        [EditorTextArea(4, true)]
        [EditorEnforceOrder(145)]
        public string TotalTerrainResourcesStats { get; set; }

        [EditorEnforceOrder(149)]
        [EditorTextArea(4, true)]
        public string EasyToReachOtherResourcesStats { get; set; }

        [EditorEnforceOrder(153)]
        [EditorTextArea(4, true)]
        public string TotalOtherResourcesStats { get; set; }

        [EditorTextArea(4, true)]
        [EditorEnforceOrder(157)]
        public string ResourceLocations { get; set; }

        public PublishResultsData()
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          // ISSUE: explicit constructor call
          base.\u002Ector();
        }
      }
    }

    public class FeatureConfigWrapper
    {
      [EditorButton(null, null, false, ObjEditorIcon.Edit)]
      [EditorEnforceOrder(47)]
      public readonly Action Edit;
      [EditorEnforceOrder(50)]
      [EditorButton(null, null, false, ObjEditorIcon.View)]
      public readonly Action View;
      [EditorButton(null, null, false, ObjEditorIcon.Clone)]
      [EditorEnforceOrder(53)]
      public readonly Action Clone;
      [EditorButton(null, null, false, ObjEditorIcon.Delete)]
      [EditorEnforceOrder(56)]
      public readonly Action Remove;
      [EditorIgnore]
      public readonly ITerrainFeatureGenerator FeatureGenerator;

      [EditorEnforceOrder(24)]
      public string Name => Mafi.Unity.MapEditor.MapEditor.getFeatureName((ITerrainFeatureBase) this.FeatureGenerator);

      [EditorEnforceOrder(27)]
      public string Type { get; private set; }

      [EditorEnforceOrder(32)]
      [EditorReadonly]
      [EditorLabel(null, "To change sorting priority, edit the 'Sorting priority adjustment' in the feature editor.", false, false)]
      public int SortingPriority => this.FeatureGenerator.SortingPriority;

      [EditorReadonly]
      [EditorLabel(null, "Total time taken to generate this feature (across all threads). Actual wall-time is likely shorter based on the amount of available processors on this machine.", false, false)]
      [EditorEnforceOrder(38)]
      public string LastGenerationTimeSec
      {
        get => this.FeatureGenerator.LastGenerationTime.TotalSeconds.RoundToSigDigits(3);
      }

      [EditorEnforceOrder(41)]
      public bool IsEnabled
      {
        get => !this.FeatureGenerator.IsDisabled;
        set => this.FeatureGenerator.IsDisabled = !value;
      }

      public FeatureConfigWrapper(ITerrainFeatureGenerator featureGenerator, Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.FeatureGenerator = featureGenerator;
        this.Type = Mafi.Unity.MapEditor.MapEditor.getFeatureTypeName((ITerrainFeatureBase) featureGenerator);
        this.Edit = (Action) (() => editor.selectFeatureEditor((ITerrainFeatureBase) featureGenerator));
        this.View = featureGenerator.GetBoundingBox().HasValue ? (Action) (() => editor.bringToView((ITerrainFeatureBase) featureGenerator)) : (Action) null;
        this.Clone = featureGenerator.IsUnique ? (Action) null : (Action) (() => editor.duplicateFeature((ITerrainFeatureBase) featureGenerator));
        this.Remove = (Action) (() => editor.unregisterFeature((ITerrainFeatureBase) featureGenerator));
      }
    }

    public class PostProcessorConfigWrapper
    {
      [EditorEnforceOrder(103)]
      [EditorButton(null, null, false, ObjEditorIcon.Edit)]
      public readonly Action Edit;
      [EditorButton(null, null, false, ObjEditorIcon.View)]
      [EditorEnforceOrder(106)]
      public readonly Action View;
      [EditorEnforceOrder(109)]
      [EditorButton(null, null, false, ObjEditorIcon.Clone)]
      public readonly Action Clone;
      [EditorEnforceOrder(112)]
      [EditorButton(null, null, false, ObjEditorIcon.Delete)]
      public readonly Action Remove;
      [EditorIgnore]
      public readonly ITerrainPostProcessorV2 PostProcessor;

      [EditorEnforceOrder(81)]
      public string Name => Mafi.Unity.MapEditor.MapEditor.getFeatureName((ITerrainFeatureBase) this.PostProcessor);

      [EditorEnforceOrder(84)]
      public string Type { get; private set; }

      [EditorLabel(null, "To change sorting priority, edit the 'Sorting priority adjustment' in the feature editor.", false, false)]
      [EditorReadonly]
      [EditorEnforceOrder(89)]
      public int SortingPriority => this.PostProcessor.SortingPriority;

      [EditorReadonly]
      [EditorLabel(null, "Time taken to run this post-processor", false, false)]
      [EditorEnforceOrder(94)]
      public string LastGenerationTimeSec
      {
        get => this.PostProcessor.LastGenerationTime.TotalSeconds.RoundToSigDigits(3);
      }

      [EditorEnforceOrder(97)]
      public bool IsEnabled
      {
        get => !this.PostProcessor.IsDisabled;
        set => this.PostProcessor.IsDisabled = !value;
      }

      public PostProcessorConfigWrapper(ITerrainPostProcessorV2 postProcessor, Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PostProcessor = postProcessor;
        this.Type = postProcessor.GetType().Name.CamelCaseToSpacedSentenceCase();
        this.Edit = (Action) (() => editor.selectFeatureEditor((ITerrainFeatureBase) postProcessor));
        this.View = postProcessor.GetBoundingBox().HasValue ? (Action) (() => editor.bringToView((ITerrainFeatureBase) postProcessor)) : (Action) null;
        this.Clone = postProcessor.IsUnique ? (Action) null : (Action) (() => editor.duplicateFeature((ITerrainFeatureBase) postProcessor));
        this.Remove = (Action) (() => editor.unregisterFeature((ITerrainFeatureBase) postProcessor));
      }
    }

    public class VirtualResourceConfigWrapper
    {
      [EditorButton(null, null, false, ObjEditorIcon.Edit)]
      [EditorEnforceOrder(147)]
      public readonly Action Edit;
      [EditorButton(null, null, false, ObjEditorIcon.View)]
      [EditorEnforceOrder(150)]
      public readonly Action View;
      [EditorButton(null, null, false, ObjEditorIcon.Clone)]
      [EditorEnforceOrder(153)]
      public readonly Action Clone;
      [EditorEnforceOrder(156)]
      [EditorButton(null, null, false, ObjEditorIcon.Delete)]
      public readonly Action Remove;
      [EditorIgnore]
      public readonly IVirtualTerrainResourceGenerator VirtualResourceGenerator;

      [EditorEnforceOrder(137)]
      public string Name
      {
        get => Mafi.Unity.MapEditor.MapEditor.getFeatureName((ITerrainFeatureBase) this.VirtualResourceGenerator);
      }

      [EditorEnforceOrder(140)]
      public bool IsEnabled
      {
        get => !this.VirtualResourceGenerator.IsDisabled;
        set => this.VirtualResourceGenerator.IsDisabled = !value;
      }

      public VirtualResourceConfigWrapper(
        IVirtualTerrainResourceGenerator virtualResource,
        Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.VirtualResourceConfigWrapper resourceConfigWrapper = this;
        this.VirtualResourceGenerator = virtualResource;
        this.Edit = (Action) (() => editor.selectFeatureEditor((ITerrainFeatureBase) virtualResource));
        this.View = virtualResource.GetBoundingBox().HasValue ? (Action) (() => editor.bringToView((ITerrainFeatureBase) virtualResource)) : (Action) null;
        this.Clone = virtualResource.IsUnique ? (Action) null : (Action) (() => editor.duplicateFeature((ITerrainFeatureBase) resourceConfigWrapper.VirtualResourceGenerator));
        this.Remove = (Action) (() => editor.unregisterFeature((ITerrainFeatureBase) resourceConfigWrapper.VirtualResourceGenerator));
      }
    }

    public class StartingLocationWrapper
    {
      [EditorButton(null, null, false, ObjEditorIcon.Edit)]
      [EditorEnforceOrder(182)]
      public readonly Action Edit;
      [EditorEnforceOrder(186)]
      [EditorButton(null, null, false, ObjEditorIcon.View)]
      public readonly Action View;
      [EditorEnforceOrder(190)]
      [EditorButton(null, null, false, ObjEditorIcon.Clone)]
      public readonly Action Clone;
      [EditorEnforceOrder(194)]
      [EditorButton(null, null, false, ObjEditorIcon.Delete)]
      public readonly Action Remove;
      [EditorIgnore]
      public readonly IStartingLocationV2 StartingLocation;

      [EditorEnforceOrder(178)]
      public bool IsEnabled { get; set; }

      public StartingLocationWrapper(IStartingLocationV2 startingLocation, Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: reference to a compiler-generated field
        this.\u003CIsEnabled\u003Ek__BackingField = true;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.StartingLocationWrapper startingLocationWrapper = this;
        this.StartingLocation = startingLocation;
        this.Edit = (Action) (() => editor.selectFeatureEditor((ITerrainFeatureBase) startingLocationWrapper.StartingLocation));
        this.View = (Action) (() => editor.bringToView((ITerrainFeatureBase) startingLocationWrapper.StartingLocation));
        this.Clone = startingLocation.IsUnique ? (Action) null : (Action) (() => editor.duplicateFeature((ITerrainFeatureBase) startingLocationWrapper.StartingLocation));
        this.Remove = (Action) (() => editor.unregisterFeature((ITerrainFeatureBase) startingLocationWrapper.StartingLocation));
      }
    }

    private class EditableFeatureWrapperForEditView : IEditableObject
    {
      [EditorEnforceOrder(216)]
      [EditorButton(null, null, false, ObjEditorIcon.View)]
      public readonly Action View;
      [EditorButton(null, null, false, ObjEditorIcon.Clone)]
      [EditorEnforceOrder(220)]
      public readonly Action Clone;
      [EditorButton(null, null, false, ObjEditorIcon.Delete)]
      [EditorEnforceOrder(224)]
      public readonly Action Remove;
      [EditorEnforceOrder(246)]
      public ITerrainFeatureConfig Config;
      private readonly IEditableTerrainFeature m_feature;
      private readonly Mafi.Unity.MapEditor.MapEditor m_editor;
      private readonly Lyst<string> m_errors;

      [EditorLabel("", null, false, true)]
      [EditorEnforceOrder(229)]
      public string ValidationResult { get; private set; }

      [EditorReadonly]
      [EditorLabel("", null, false, false)]
      [EditorEnforceOrder(234)]
      public string SelectedFeature
      {
        get => Mafi.Unity.MapEditor.MapEditor.getFeatureName((ITerrainFeatureBase) this.m_feature);
      }

      [EditorReadonly]
      [EditorEnforceOrder(238)]
      public string SortingPriority
      {
        get
        {
          if (this.m_feature is ITerrainFeatureGenerator feature1)
            return string.Format("{0} (feature)", (object) feature1.SortingPriority);
          return !(this.m_feature is ITerrainPostProcessorV2 feature2) ? "n/a" : string.Format("{0} (post-processor)", (object) feature2.SortingPriority);
        }
      }

      public EditableFeatureWrapperForEditView(IEditableTerrainFeature feature, Mafi.Unity.MapEditor.MapEditor editor)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_errors = new Lyst<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Mafi.Unity.MapEditor.MapEditor.EditableFeatureWrapperForEditView wrapperForEditView = this;
        this.m_feature = feature;
        this.m_editor = editor;
        this.Config = feature.Config;
        if (this.Config is ITerrainFeatureConfigWithInit config)
          config.InitializeInMapEditor(editor.Resolver);
        this.View = feature.GetHandleData().HasValue ? (Action) (() => editor.bringToView((ITerrainFeatureBase) feature)) : (Action) null;
        this.Clone = feature.IsUnique ? (Action) null : (Action) (() => editor.duplicateFeature((ITerrainFeatureBase) feature));
        this.Remove = (Action) (() =>
        {
          editor.unregisterFeature((ITerrainFeatureBase) feature);
          editor.pushNewUndoSnapshot("Feature removed", new RectangleTerrainArea2i?(feature.GetBoundingBox() ?? wrapperForEditView.m_editor.TerrainManager.TerrainArea));
        });
      }

      public bool ObjectWasEdited()
      {
        this.m_errors.Clear();
        bool flag1 = this.m_feature.ValidateConfig(this.m_editor.Resolver, this.m_errors);
        bool flag2 = this.ValidationResult != null;
        this.ValidationResult = (string) null;
        if (!flag1)
        {
          this.ValidationResult = this.m_errors.IsNotEmpty ? this.m_errors.JoinStrings("\n") : "Feature config is not valid.";
          flag2 = true;
        }
        return flag2;
      }
    }
  }
}
