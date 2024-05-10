// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.EntitiesMenuStripInfoView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.Tools;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class EntitiesMenuStripInfoView : View, IUnityUi
  {
    private readonly PlanningModeInputController m_planningModeController;
    private readonly PlanningModeManager m_planningModeManager;
    private readonly TransportBuildController m_transportBuildController;
    private readonly ShortcutsManager m_shortcutsManager;

    public EntitiesMenuStripInfoView(
      PlanningModeInputController planningModeController,
      IGameLoopEvents gameLoopEvents,
      PlanningModeManager planningModeManager,
      TransportBuildController transportBuildController,
      ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("ActivePlanningModeView");
      this.m_planningModeController = planningModeController;
      this.m_planningModeManager = planningModeManager;
      this.m_transportBuildController = transportBuildController;
      this.m_shortcutsManager = shortcutsManager;
      gameLoopEvents.RenderUpdate.AddNonSaveable<EntitiesMenuStripInfoView>(this, new Action<GameTime>(((View) this).RenderUpdate));
      gameLoopEvents.SyncUpdate.AddNonSaveable<EntitiesMenuStripInfoView>(this, new Action<GameTime>(((View) this).SyncUpdate));
    }

    protected override void BuildUi()
    {
      ActivePlanningModeView planningModeView = new ActivePlanningModeView(this.m_shortcutsManager, this.m_planningModeController);
      planningModeView.BuildUi(this.Builder);
      planningModeView.PutToBottomOf<ActivePlanningModeView>((IUiElement) this, planningModeView.GetHeight());
      planningModeView.Hide<ActivePlanningModeView>();
      TransportSnappingDisabledView snappingView = new TransportSnappingDisabledView(this.m_shortcutsManager);
      snappingView.BuildUi(this.Builder);
      snappingView.PutToBottomOf<TransportSnappingDisabledView>((IUiElement) this, snappingView.GetHeight());
      snappingView.Hide<TransportSnappingDisabledView>();
      this.SetHeight<EntitiesMenuStripInfoView>(planningModeView.GetHeight());
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_planningModeManager.IsPlanningModeEnabled)).Observe<bool>((Func<bool>) (() => this.m_transportBuildController.ShouldShowDisableSnappingInfo)).Do((Action<bool, bool>) ((isPlanningEnabled, isSnappingOff) =>
      {
        planningModeView.SetVisibility<ActivePlanningModeView>(isPlanningEnabled && !isSnappingOff);
        snappingView.SetVisibility<TransportSnappingDisabledView>(isSnappingOff);
      }));
      this.AddUpdater(updaterBuilder.Build());
    }

    public void RegisterUi(UiBuilder builder) => this.BuildUi(builder);
  }
}
