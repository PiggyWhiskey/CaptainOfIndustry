// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Logistics.LogisticsOverviewWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Logistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class LogisticsOverviewWindow : 
    WindowView,
    IWindowWithInnerWindowsSupport,
    IWindow,
    IDynamicSizeElement,
    IUiElement
  {
    public int WIDTH;
    private TabsContainer m_tabsContainer;
    private readonly LogisticsGeneralTab m_logisticsGeneralTab;
    private readonly LogisticsStatsTab m_logisticsStatsTab;

    internal LogisticsOverviewWindow(
      CameraController cameraController,
      EntitiesManager entitiesManager,
      ITerrainDumpingManager dumpingManager,
      VehicleBuffersRegistry vehicleBuffersRegistry,
      VehicleJobStatsManager jobStatsManager,
      IInputScheduler inputScheduler)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.WIDTH = 600;
      // ISSUE: explicit constructor call
      base.\u002Ector("LogisticsOverview");
      this.ShowAfterSync = true;
      this.m_logisticsGeneralTab = new LogisticsGeneralTab(cameraController, inputScheduler, entitiesManager, vehicleBuffersRegistry, dumpingManager, (IWindowWithInnerWindowsSupport) this);
      this.m_logisticsStatsTab = new LogisticsStatsTab(jobStatsManager);
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.VehiclesManagement);
      this.PositionSelfToCenter();
      this.MakeMovable();
      float maxSeenHeight = 200f;
      Vector2 size = new Vector2((float) this.WIDTH, maxSeenHeight);
      this.SetContentSize(size);
      this.m_tabsContainer = this.Builder.NewTabsContainer(size.x.RoundToInt(), size.y.RoundToInt(), (IUiElement) this.GetContentPanel()).AddTab((LocStrFormatted) Tr.General, new IconStyle?(new IconStyle("Assets/Unity/UserInterface/Toolbar/Vehicles.svg", new ColorRgba?(this.Builder.Style.Global.DefaultPanelTextColor))), (Tab) this.m_logisticsGeneralTab).AddTab((LocStrFormatted) TrCore.TrucksAnalytics__Title, new IconStyle?(new IconStyle("Assets/Unity/UserInterface/Toolbar/Stats.svg", new ColorRgba?(this.Builder.Style.Global.DefaultPanelTextColor))), (Tab) this.m_logisticsStatsTab).PutTo<TabsContainer>((IUiElement) this.GetContentPanel());
      this.m_logisticsGeneralTab.SizeChanged += new Action<IUiElement>(sizeChanged);
      this.m_logisticsStatsTab.SizeChanged += new Action<IUiElement>(sizeChanged);

      void sizeChanged(IUiElement element)
      {
        maxSeenHeight = maxSeenHeight.Max(element.GetHeight());
        this.SetContentSize((float) this.WIDTH, maxSeenHeight);
      }
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      this.m_tabsContainer.RenderUpdate(gameTime);
      base.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      this.m_tabsContainer.SyncUpdate(gameTime);
      base.SyncUpdate(gameTime);
    }

    public void SetupInnerWindowWithButton(
      WindowView innerWindow,
      IUiElement btnHolder,
      IUiElement btn,
      Action returnBtnHolder,
      Action onExitAction)
    {
      innerWindow.BuildUi(this.Builder);
      innerWindow.MakeMovable(elementToMove: (IUiElement) this);
      innerWindow.PutToLeftTopOf<WindowView>(btnHolder, innerWindow.GetSize(), Offset.Left(btn.GetWidth() - 1f));
      Panel overlay = this.AddOverlay(onExitAction);
      innerWindow.OnShowStart += (Action) (() =>
      {
        overlay.Show<Panel>();
        btnHolder.SetParent<IUiElement>((IUiElement) this);
      });
      innerWindow.OnHide += (Action) (() =>
      {
        overlay.Hide<Panel>();
        returnBtnHolder();
      });
    }
  }
}
