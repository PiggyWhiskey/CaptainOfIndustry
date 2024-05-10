// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Levelling.LogisticsStatusBarView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Unity.InputControl.Logistics;
using Mafi.Unity.InputControl.TopStatusBar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Levelling
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class LogisticsStatusBarView : IStatusBarItem
  {
    public const float WIDTH = 90f;
    private Panel m_container;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly VehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly UiBuilder m_builder;
    private IUiUpdater m_updater;
    private Action m_onClick;

    public LogisticsStatusBarView(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      LogisticsOverviewController logisticsOverview,
      VehicleBuffersRegistry vehicleBuffersRegistry,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_builder = builder;
      this.m_onClick = (Action) (() => inputManager.ActivateNewController((IUnityInputController) logisticsOverview));
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_container = this.m_builder.NewPanel("PopulationStatusBarView");
      this.m_builder.NewPanel("divider").SetBackground(this.m_builder.Style.Panel.Border.Color).PutToLeftOf<Panel>((IUiElement) this.m_container, 1f);
      StackContainer leftOf = this.m_builder.NewStackContainer("PopTiles").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).PutToLeftOf<StackContainer>((IUiElement) this.m_container, 0.0f, Offset.Top(5f) + Offset.Bottom(5f));
      PopulationStatusBarView.PopInfoTile jobsPendingTile = new PopulationStatusBarView.PopInfoTile(this.m_builder, "Assets/Unity/UserInterface/Toolbar/Vehicles.svg").OnClick(this.m_onClick).AppendTo<PopulationStatusBarView.PopInfoTile>(leftOf, new float?(90f));
      jobsPendingTile.MakeAsSingleText();
      jobsPendingTile.AddTooltip().SetText((LocStrFormatted) Tr.LogisticsStatus__Tooltip);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Duration>((Func<Duration>) (() => this.m_vehicleBuffersRegistry.GetBalancingLatency())).Do((Action<Duration>) (latency =>
      {
        if (latency > Duration.FromSec(400))
        {
          jobsPendingTile.SetText((LocStrFormatted) Tr.LogisticsStatus__ExtremelyBusy);
          jobsPendingTile.SetCriticalColor();
        }
        else if (latency > Duration.FromSec(140))
        {
          jobsPendingTile.SetText((LocStrFormatted) Tr.LogisticsStatus__VeryBusy);
          jobsPendingTile.SetCriticalColor();
        }
        else if (latency > Duration.FromSec(40))
        {
          jobsPendingTile.SetText((LocStrFormatted) Tr.LogisticsStatus__Busy);
          jobsPendingTile.SetWarningColor();
        }
        else
        {
          jobsPendingTile.SetText((LocStrFormatted) Tr.LogisticsStatus__Stable);
          jobsPendingTile.SetStandardColor();
        }
      }));
      this.m_updater = updaterBuilder.Build(SyncFrequency.OncePerSec);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<LogisticsStatusBarView>(this, (Action<GameTime>) (x => this.m_updater.SyncUpdate()));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<LogisticsStatusBarView>(this, (Action<GameTime>) (x => this.m_updater.RenderUpdate()));
      statusBar.AddLargeTileToLeft((IUiElement) this.m_container, 90f, 1f);
    }
  }
}
