// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.ToolbarStripInfoView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class ToolbarStripInfoView : View, IToolbarItemRegistrar
  {
    private readonly TransportBuildController m_transportBuildController;
    private readonly ToolbarController m_toolbarController;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly UiBuilder m_builder;

    public ToolbarStripInfoView(
      IGameLoopEvents gameLoopEvents,
      TransportBuildController transportBuildController,
      ToolbarController toolbarController,
      ShortcutsManager shortcutsManager,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (ToolbarStripInfoView));
      this.m_transportBuildController = transportBuildController;
      this.m_toolbarController = toolbarController;
      this.m_shortcutsManager = shortcutsManager;
      this.m_builder = builder;
      gameLoopEvents.RenderUpdate.AddNonSaveable<ToolbarStripInfoView>(this, new Action<GameTime>(((View) this).RenderUpdate));
      gameLoopEvents.SyncUpdate.AddNonSaveable<ToolbarStripInfoView>(this, new Action<GameTime>(((View) this).SyncUpdate));
    }

    protected override void BuildUi()
    {
      TransportSnappingDisabledView snappingView = new TransportSnappingDisabledView(this.m_shortcutsManager);
      snappingView.BuildUi(this.Builder);
      snappingView.PutToBottomOf<TransportSnappingDisabledView>((IUiElement) this, snappingView.GetHeight());
      snappingView.Hide<TransportSnappingDisabledView>();
      this.SetHeight<ToolbarStripInfoView>(snappingView.GetHeight());
      this.m_toolbarController.SetNotificationStripForCentralButtons((IUiElement) snappingView);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_transportBuildController.ShouldShowDisableSnappingInfo && !this.m_toolbarController.HasBottomMenuOpen)).Do((Action<bool>) (showSnappingOff => snappingView.SetVisibility<TransportSnappingDisabledView>(showSnappingOff)));
      this.AddUpdater(updaterBuilder.Build());
    }

    public void RegisterIntoToolbar(ToolbarController controller) => this.BuildUi(this.m_builder);
  }
}
