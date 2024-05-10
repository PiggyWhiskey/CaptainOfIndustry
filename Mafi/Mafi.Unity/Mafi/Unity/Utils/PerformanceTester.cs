// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.PerformanceTester
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Simulation;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.Factory.Transports;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Particles;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.Terrain;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class PerformanceTester
  {
    private readonly IGameConsole m_console;
    private readonly SimLoopEvents m_simLoopEvents;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly GameRunner m_gameRunner;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly TerrainPropsRenderer m_terrainPropsRenderer;
    private readonly WaterRendererManager m_waterRenderer;
    private readonly TreeRenderer m_treeRenderer;
    private readonly IoPortsRenderer m_portsRenderer;
    private readonly EntitiesRenderingManager m_entitiesRenderer;
    private readonly ProductsRenderer m_transportedProductsRenderer;
    private readonly UiBuilder m_uiBuilder;
    private readonly IInputScheduler m_inputScheduler;
    private readonly InstancedChunkBasedLayoutEntitiesRenderer m_layoutEntitiesRenderer;
    private readonly InstancedChunkBasedTransportsRenderer m_transportsRenderer;
    private readonly TransportPillarsRenderer m_transportPillarsRenderer;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly ConstructionCubesRenderer m_constructionCubesRenderer;
    private readonly IActivator m_terrainDesignationsActivator;
    private readonly ChunkBasedRenderingManager m_chunkRenderingManager;
    private readonly SkyboxController m_skyboxController;
    private readonly FogController m_fogController;
    private readonly CameraController m_cameraController;
    private readonly DustParticlesController m_dustParticlesController;
    private readonly Lyst<float> m_syncTimes;
    private readonly Lyst<float> m_simTimes;
    private readonly Lyst<float> m_inputTimes;
    private readonly Lyst<float> m_renderTimes;
    private readonly Lyst<float> m_stateFrameTimes;
    private readonly StringBuilder m_resultsBuilder;
    private readonly Lyst<PerformanceTester.State> m_remainingStates;
    private readonly Lyst<KeyValuePair<LayoutEntityProto, int>> m_remainingEntityTypes;
    private readonly Lyst<KeyValuePair<RenderingSetting, int>> m_remainingRenderingSettings;
    private readonly Lyst<KeyValuePair<RenderingSetting, int>> m_renderingSettingsDefaults;
    private PerformanceTester.State m_state;
    private string m_stateName;
    private int m_durationPerStage;
    private int m_durationLeft;
    private bool m_wasPaused;
    private int m_warmupRemaining;
    private bool m_uiWasVisible;
    private HighlightId m_portHighlightId;
    private float? m_baselineAvgFrameTime;
    private float? m_bottomBaselineMedianFrameTime;
    private int m_entitiesCount;
    private Option<LayoutEntityProto> m_hiddenEntitiesProto;
    private int m_hiddenEntitiesCount;
    private int m_oldTargetFrameRate;
    private int m_oldVsyncCount;
    private int m_lastMsaa;
    private bool m_oldPostProcessEnabled;
    private int m_lastQualityLevel;

    public PerformanceTester(
      IGameConsole console,
      SimLoopEvents simLoopEvents,
      IGameLoopEvents gameLoopEvents,
      GameRunner gameRunner,
      TerrainRenderer terrainRenderer,
      TerrainPropsRenderer terrainPropsRenderer,
      WaterRendererManager waterRenderer,
      TreeRenderer treeRenderer,
      IoPortsRenderer portsRenderer,
      EntitiesRenderingManager entitiesRenderer,
      ProductsRenderer transportedProductsRenderer,
      UiBuilder uiBuilder,
      IInputScheduler inputScheduler,
      InstancedChunkBasedLayoutEntitiesRenderer layoutEntitiesRenderer,
      InstancedChunkBasedTransportsRenderer transportsRenderer,
      TransportPillarsRenderer transportPillarsRenderer,
      TerrainDesignationsRenderer terrainDesignationsRenderer,
      IEntitiesManager entitiesManager,
      ConstructionCubesRenderer constructionCubesRenderer,
      ChunkBasedRenderingManager chunkRenderingManager,
      SkyboxController skyboxController,
      FogController fogController,
      CameraController cameraController,
      DustParticlesController dustParticlesController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_syncTimes = new Lyst<float>();
      this.m_simTimes = new Lyst<float>();
      this.m_inputTimes = new Lyst<float>();
      this.m_renderTimes = new Lyst<float>();
      this.m_stateFrameTimes = new Lyst<float>();
      this.m_resultsBuilder = new StringBuilder(1024);
      this.m_remainingStates = new Lyst<PerformanceTester.State>();
      this.m_remainingEntityTypes = new Lyst<KeyValuePair<LayoutEntityProto, int>>();
      this.m_remainingRenderingSettings = new Lyst<KeyValuePair<RenderingSetting, int>>();
      this.m_renderingSettingsDefaults = new Lyst<KeyValuePair<RenderingSetting, int>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_console = console;
      this.m_simLoopEvents = simLoopEvents;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_gameRunner = gameRunner;
      this.m_terrainRenderer = terrainRenderer;
      this.m_terrainPropsRenderer = terrainPropsRenderer;
      this.m_waterRenderer = waterRenderer;
      this.m_treeRenderer = treeRenderer;
      this.m_portsRenderer = portsRenderer;
      this.m_entitiesRenderer = entitiesRenderer;
      this.m_transportsRenderer = transportsRenderer;
      this.m_transportedProductsRenderer = transportedProductsRenderer;
      this.m_uiBuilder = uiBuilder;
      this.m_inputScheduler = inputScheduler;
      this.m_layoutEntitiesRenderer = layoutEntitiesRenderer;
      this.m_transportPillarsRenderer = transportPillarsRenderer;
      this.m_entitiesManager = entitiesManager;
      this.m_constructionCubesRenderer = constructionCubesRenderer;
      this.m_chunkRenderingManager = chunkRenderingManager;
      this.m_skyboxController = skyboxController;
      this.m_fogController = fogController;
      this.m_cameraController = cameraController;
      this.m_dustParticlesController = dustParticlesController;
      this.m_terrainDesignationsActivator = terrainDesignationsRenderer.CreateActivator();
    }

    [ConsoleCommand(true, false, null, null)]
    private void testPerformance(string category = null, int measurements = 900, int entitiesCount = 20)
    {
      this.m_console.WriteLine("Perf testing started, DO NOT do anything until it is done", ColorRgba.Yellow);
      this.startMeasuring(category, measurements, entitiesCount);
    }

    [ConsoleCommand(true, false, null, null)]
    private void printMemoryUsage()
    {
      this.writeResults(string.Format("Current memory usage: {0} MB", (object) (GC.GetTotalMemory(true) / 1024L / 1024L)));
    }

    private void startMeasuring(string category, int measurements, int entitiesCount)
    {
      if (this.m_durationLeft > 0)
      {
        Log.Warning("Some other measurement is ongoing, skipping.");
      }
      else
      {
        this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<PerformanceTester>(this, new Action<GameTime>(this.syncUpdate));
        this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<PerformanceTester>(this, new Action<GameTime>(this.renderUpdate));
        this.m_syncTimes.Clear();
        this.m_simTimes.Clear();
        this.m_inputTimes.Clear();
        this.m_renderTimes.Clear();
        this.m_stateFrameTimes.Clear();
        this.m_baselineAvgFrameTime = new float?();
        this.m_bottomBaselineMedianFrameTime = new float?();
        this.m_resultsBuilder.Length = 0;
        this.m_resultsBuilder.AppendLine("Perf measurements:");
        this.m_entitiesCount = entitiesCount;
        this.m_oldTargetFrameRate = Application.targetFrameRate;
        this.m_oldVsyncCount = QualitySettings.vSyncCount;
        Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 0;
        this.m_wasPaused = this.m_simLoopEvents.IsSimPaused;
        this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(true));
        this.m_uiWasVisible = this.m_uiBuilder.IsUiVisible;
        this.m_uiBuilder.SetUiVisibility(false);
        this.m_durationPerStage = measurements;
        this.m_remainingStates.Clear();
        this.m_remainingStates.Add(PerformanceTester.State.Simulation);
        this.m_remainingStates.Add(PerformanceTester.State.BaselineRender);
        this.m_remainingStates.Reverse();
        if (this.m_remainingStates.IsEmpty)
        {
          this.m_console.WriteLine("No testing category was selected", ColorRgba.Red);
        }
        else
        {
          this.writeResults(string.Format("{0,16}: {1}", (object) "Version", (object) "0.6.3a"));
          this.writeResults(string.Format("{0,16}: {1}", (object) "Configuration", (object) "RELEASE") ?? "");
          this.writeResults(string.Format("{0,16}: {1} @ {2}", (object) "CPU", (object) SystemInfo.processorType, (object) SystemInfo.processorFrequency));
          this.writeResults(string.Format("{0,16}: {1}; {2}; ", (object) "GPU", (object) SystemInfo.graphicsDeviceType, (object) SystemInfo.graphicsDeviceName) + SystemInfo.graphicsDeviceVendor);
          this.writeResults(string.Format("{0,16}: {1} MB", (object) "Managed memory", (object) (GC.GetTotalMemory(true) / 1024L / 1024L)));
          this.writeResults("");
          appendComponentCounts<Transform>("All GOs count");
          appendComponentCounts<MeshRenderer>("Renderers count");
          appendComponentCounts<Collider>("Colliders count");
          appendComponentCounts<Light>("Lights count");
          this.writeResults("");
          this.m_state = this.m_remainingStates.PopLast();
          this.startStage(this.m_state);
        }
      }

      void appendComponentCounts<T>(string name) where T : Component
      {
        int count;
        int activeCount;
        PerformanceTester.countAllSceneObjectsWithComponent<T>(out count, out activeCount);
        this.writeResults(string.Format("{0,16}: {1} ({2} active)", (object) name, (object) count, (object) activeCount));
      }
    }

    private void stopMeasuring()
    {
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<PerformanceTester>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoopEvents.RenderUpdate.RemoveNonSaveable<PerformanceTester>(this, new Action<GameTime>(this.renderUpdate));
      this.m_durationLeft = 0;
      Application.targetFrameRate = this.m_oldTargetFrameRate;
      QualitySettings.vSyncCount = this.m_oldVsyncCount;
      this.m_uiBuilder.SetUiVisibility(this.m_uiWasVisible);
      this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(this.m_wasPaused));
      this.m_renderingSettingsDefaults.Clear();
    }

    private void syncUpdate(GameTime time)
    {
      Assert.That<int>(this.m_durationLeft).IsPositive();
      if (this.m_warmupRemaining > 0)
      {
        if (this.m_state == PerformanceTester.State.WithPortHlght && this.m_portsRenderer.IsHighlightPending())
          this.m_warmupRemaining = 90;
        --this.m_warmupRemaining;
      }
      else
      {
        if (this.m_state == PerformanceTester.State.Simulation)
        {
          this.m_syncTimes.Add((float) this.m_gameRunner.LatestSyncDuration.TotalMilliseconds);
          Lyst<float> inputTimes = this.m_inputTimes;
          TimeSpan timeSpan = this.m_gameRunner.LatestInputUpdateDuration;
          double totalMilliseconds1 = timeSpan.TotalMilliseconds;
          inputTimes.Add((float) totalMilliseconds1);
          Lyst<float> simTimes = this.m_simTimes;
          timeSpan = this.m_gameRunner.LatestSimUpdateDuration;
          double totalMilliseconds2 = timeSpan.TotalMilliseconds;
          simTimes.Add((float) totalMilliseconds2);
          Lyst<float> renderTimes = this.m_renderTimes;
          timeSpan = this.m_gameRunner.LatestRenderUpdateDuration;
          double totalMilliseconds3 = timeSpan.TotalMilliseconds;
          renderTimes.Add((float) totalMilliseconds3);
        }
        --this.m_durationLeft;
        if (this.m_durationLeft > 0)
          return;
        this.endStage(this.m_state);
        if (this.m_remainingStates.IsEmpty)
        {
          this.stopMeasuring();
          this.m_console.WriteLine(this.m_resultsBuilder.ToString());
        }
        else
        {
          this.m_state = this.m_remainingStates.PopLast();
          this.startStage(this.m_state);
        }
      }
    }

    private void renderUpdate(GameTime time)
    {
      if (this.m_warmupRemaining > 0)
        return;
      this.m_stateFrameTimes.Add(Time.deltaTime);
    }

    private void startStage(PerformanceTester.State state)
    {
      this.m_durationLeft = this.m_durationPerStage;
      this.m_warmupRemaining = 90;
      this.m_stateName = state.ToString();
      this.m_stateFrameTimes.Clear();
      this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(true));
    }

    private void endStage(PerformanceTester.State state)
    {
      MeanAndStdDev meanAndStdDev = this.m_stateFrameTimes.AverageAndStdDev();
      float num1 = this.m_stateFrameTimes.Sum();
      float number = (float) this.m_stateFrameTimes.Count / num1;
      float valueOrDefault1 = this.m_bottomBaselineMedianFrameTime.GetValueOrDefault();
      double num2 = (double) meanAndStdDev.Mean - (double) valueOrDefault1;
      float? baselineAvgFrameTime = this.m_baselineAvgFrameTime;
      double num3 = ((double) baselineAvgFrameTime ?? (double) meanAndStdDev.Mean) - (double) valueOrDefault1;
      float num4 = (float) (1.0 - num2 / num3);
      float mean = meanAndStdDev.Mean;
      baselineAvgFrameTime = this.m_baselineAvgFrameTime;
      float valueOrDefault2 = (baselineAvgFrameTime.HasValue ? new float?(mean - baselineAvgFrameTime.GetValueOrDefault()) : new float?()).GetValueOrDefault();
      string str1 = (this.m_baselineAvgFrameTime.HasValue ? string.Format("{0}{1:00.0}%", (double) num4 > 0.0 ? (object) "+" : (object) "", (object) (float) ((double) num4 * 100.0)) : "--").PadLeft(6);
      string str2 = (this.m_baselineAvgFrameTime.HasValue ? string.Format("{0}{1:00.0} ms", (double) valueOrDefault2 > 0.0 ? (object) "+" : (object) "", (object) (float) ((double) valueOrDefault2 * 1000.0)) : "--").PadLeft(8);
      this.writeResults(string.Format("{0,30}: {1,4} FPS, {2}, {3}, ", (object) this.m_stateName, (object) number.RoundToSigDigits(3, false, false, false), (object) str1, (object) str2) + string.Format("avg {0,4} ", (object) (meanAndStdDev.Mean * 1000f).RoundToSigDigits(3, false, false, false)) + string.Format("+- {0,4} ms, ", (object) (meanAndStdDev.StdDev * 1000f).RoundToSigDigits(3, false, false, false)) + string.Format("median {0,4} ms, ", (object) (this.m_stateFrameTimes.Median() * 1000f).RoundToSigDigits(3, false, false, false)) + string.Format("min {0,4} ms, ", (object) (this.m_stateFrameTimes.Min() * 1000f).RoundToSigDigits(3, false, false, false)) + string.Format("max {0,4} ms, ", (object) (this.m_stateFrameTimes.Max() * 1000f).RoundToSigDigits(3, false, false, false)) + string.Format("{0,4} frames over {1,4} ms", (object) this.m_stateFrameTimes.Count, (object) (num1 * 1000f).RoundToInt()));
    }

    private void writeResults(string results)
    {
      this.m_resultsBuilder.AppendLine(results);
      this.m_console.WriteLine(results);
    }

    private static void countAllSceneObjectsWithComponent<T>(out int count, out int activeCount) where T : Component
    {
      Queueue<Transform> queueue = new Queueue<Transform>();
      GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
      count = 0;
      activeCount = 0;
      foreach (GameObject gameObject in rootGameObjects)
      {
        queueue.Enqueue(gameObject.transform);
        if ((UnityEngine.Object) gameObject.GetComponent<T>() != (UnityEngine.Object) null)
        {
          ++count;
          if (gameObject.activeInHierarchy)
            ++activeCount;
        }
      }
      while (queueue.Count > 0)
      {
        foreach (Transform transform in queueue.Dequeue())
        {
          queueue.Enqueue(transform);
          if ((UnityEngine.Object) transform.GetComponent<T>() != (UnityEngine.Object) null)
          {
            ++count;
            if (transform.gameObject.activeInHierarchy)
              ++activeCount;
          }
        }
      }
    }

    private enum State
    {
      Simulation,
      BaselineRender,
      NoTerrain,
      NoTerrDetails,
      NoTerrClsDetails,
      NoTerrFarDetails,
      NoTerProps,
      NoTerSurface,
      WithTerDsgn,
      NoOceanWater,
      NoTrees,
      NoLayoutEntities,
      NoConstrCubes,
      NoSettlement,
      NoPorts,
      WithPortHlght,
      NoTransports,
      NoTrIndicators,
      NoTrProducts,
      NoStProducts,
      NoPillars,
      NoCookie,
      NoFog,
      NoTerrainDust,
      Quality0,
      Quality1,
      Quality2,
      OnlyLod0,
      OnlyLod0And1,
      OnlyLod0Layout,
      MsaaDisabled,
      Msaa2x,
      Msaa4x,
      Msaa8x,
      NoPostProcess,
      Done,
      EachLayoutEntity,
      RenderingSetting,
    }
  }
}
