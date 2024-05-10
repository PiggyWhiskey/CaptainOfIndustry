// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Camera.VictoryCameraController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Simulation;
using Mafi.Core.SpaceProgram;
using Mafi.Core.UiState;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl;
using Mafi.Unity.MainMenu;
using Mafi.Unity.UserInterface;
using System;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Camera
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class VictoryCameraController : IUnityInputController
  {
    private static readonly RelTile1f CAM_DIST_FROM;
    private static readonly RelTile1f CAM_DIST_TO;
    private static readonly RelTile1f CAM_DIST_RATE_PER_SEC;
    private static readonly AngleDegrees1f CAM_PITCH;
    private static readonly AngleDegrees1f CAM_YAW_RATE_PER_SEC;
    private static readonly RelTile1f GAINED_ALTITUDE_BEFORE_CREDITS;
    private readonly EntitiesManager m_entitiesManager;
    private readonly MbBasedEntitiesRenderer m_entitiesRenderer;
    private readonly CameraController m_cameraController;
    private readonly IUnityInputMgr m_unityInputMgr;
    private readonly IInputScheduler m_inputScheduler;
    private readonly UiBuilder m_uiBuilder;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly UiBuilder m_builder;
    private Option<RocketEntity> m_launchedRocket;
    private Option<CreditsScreen> m_creditsScreen;
    private bool m_oldUiVisibility;
    private bool m_gameWasPaused;

    public ControllerConfig Config => ControllerConfig.WindowFullscreen;

    public VictoryCameraController(
      GameVictoryManager gameVictoryManager,
      EntitiesManager entitiesManager,
      MbBasedEntitiesRenderer entitiesRenderer,
      CameraController cameraController,
      IUnityInputMgr unityInputMgr,
      IInputScheduler inputScheduler,
      UiBuilder uiBuilder,
      IGameLoopEvents gameLoopEvents,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_entitiesRenderer = entitiesRenderer;
      this.m_cameraController = cameraController;
      this.m_unityInputMgr = unityInputMgr;
      this.m_inputScheduler = inputScheduler;
      this.m_uiBuilder = uiBuilder;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_builder = builder;
      gameVictoryManager.OnGameVictory.AddNonSaveable<VictoryCameraController>(this, new Action(this.onVictory));
    }

    private void onVictory()
    {
      this.m_launchedRocket = (Option<RocketEntity>) this.m_entitiesManager.GetAllEntitiesOfType<RocketEntity>().FirstOrDefault<RocketEntity>((Func<RocketEntity, bool>) (x => x.IsLaunched));
      if (this.m_launchedRocket == (RocketEntity) null)
      {
        Log.Warning("VictoryCameraManager: Failed to find launched rocket.");
      }
      else
      {
        this.m_inputScheduler.ScheduleInputCmd<GameSpeedChangeCmd>(new GameSpeedChangeCmd(1));
        this.m_gameLoopEvents.InvokeInSyncNotSaved(new Action(this.startRocketTracking));
      }
    }

    private void startRocketTracking()
    {
      if (this.m_launchedRocket.IsNone)
      {
        Log.Warning("VictoryCameraManager: No launched rocket?");
      }
      else
      {
        EntityMb entityMb;
        if (!this.m_entitiesRenderer.TryGetMbFor((IRenderedEntity) this.m_launchedRocket.Value, out entityMb))
        {
          Log.Warning("VictoryCameraManager: Failed to find renderer for launched.");
          this.m_launchedRocket = Option<RocketEntity>.None;
        }
        else
        {
          this.m_unityInputMgr.DeactivateAllControllers();
          this.m_unityInputMgr.ActivateNewController((IUnityInputController) this);
          this.m_cameraController.CameraModel.SetTargetTracking(entityMb.gameObject.transform);
          this.m_cameraController.CameraModel.ResetPose(entityMb.transform.position.ToTile3f().Xy, new RelTile1f?(VictoryCameraController.CAM_DIST_FROM), new AngleDegrees1f?(VictoryCameraController.CAM_PITCH));
        }
      }
    }

    public void Activate()
    {
      this.m_oldUiVisibility = this.m_uiBuilder.IsUiVisible;
      this.m_uiBuilder.SetUiVisibility(false);
    }

    public void Deactivate()
    {
      this.m_launchedRocket = Option<RocketEntity>.None;
      this.m_cameraController.CameraModel.CancelTargetTracking();
      this.m_creditsScreen.ValueOrNull?.Destroy();
      this.m_uiBuilder.SetUiVisibility(this.m_oldUiVisibility);
      if (!this.m_gameWasPaused)
        return;
      this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(false));
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_launchedRocket.IsNone)
        return false;
      UiCameraState state = this.m_cameraController.CameraModel.State;
      Percent scale = Percent.FromFloat(Time.smoothDeltaTime);
      if (state.OrbitRadius < VictoryCameraController.CAM_DIST_TO)
        this.m_cameraController.CameraModel.State.OrbitRadius += VictoryCameraController.CAM_DIST_RATE_PER_SEC.ScaledBy(scale);
      state.YawAngle += VictoryCameraController.CAM_YAW_RATE_PER_SEC.ScaledBy(scale);
      if (this.m_creditsScreen.IsNone && this.m_launchedRocket.Value.GainedAltitude > VictoryCameraController.GAINED_ALTITUDE_BEFORE_CREDITS)
      {
        this.m_gameWasPaused = true;
        this.m_inputScheduler.ScheduleInputCmd<SetSimPauseStateCmd>(new SetSimPauseStateCmd(true));
        this.m_creditsScreen = (Option<CreditsScreen>) new CreditsScreen(this.m_builder, 0.2f);
      }
      else if (this.m_launchedRocket.Value.GainedAltitude > 2 * VictoryCameraController.GAINED_ALTITUDE_BEFORE_CREDITS)
      {
        this.m_cameraController.CameraModel.CancelTargetTracking();
        this.m_launchedRocket = Option<RocketEntity>.None;
      }
      return false;
    }

    static VictoryCameraController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      VictoryCameraController.CAM_DIST_FROM = 50.0.Tiles();
      VictoryCameraController.CAM_DIST_TO = 200.0.Tiles();
      VictoryCameraController.CAM_DIST_RATE_PER_SEC = 20.0.Tiles();
      VictoryCameraController.CAM_PITCH = 40.Degrees();
      VictoryCameraController.CAM_YAW_RATE_PER_SEC = 8.Degrees();
      VictoryCameraController.GAINED_ALTITUDE_BEFORE_CREDITS = 100.0.Tiles();
    }
  }
}
