// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsMaintenanceTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Maintenance;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsMaintenanceTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly MaintenanceManager m_maintenanceManager;
    private ChartWithRangeSelectors m_chart;

    public StatsMaintenanceTab(ICalendar calendar, MaintenanceManager maintenanceManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("MaintenanceStats");
      this.m_calendar = calendar;
      this.m_maintenanceManager = maintenanceManager;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      foreach (IMaintenanceBufferReadonly maintenanceBuffer in this.m_maintenanceManager.MaintenanceBuffers)
      {
        LocStr name = maintenanceBuffer.Product.Strings.Name;
        this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) name), maintenanceBuffer.Product.Graphics.IconPath, (ItemStats) maintenanceBuffer.ProducedTotalStats));
        this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) name), maintenanceBuffer.Product.Graphics.IconPath, (ItemStats) maintenanceBuffer.ConsumedTotalStats));
      }
      this.SetSize<StatsMaintenanceTab>(this.m_chart.GetSize());
      this.m_chart.PutTo<ChartWithRangeSelectors>((IUiElement) this);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_chart.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_chart.SyncUpdate(gameTime);
    }
  }
}
