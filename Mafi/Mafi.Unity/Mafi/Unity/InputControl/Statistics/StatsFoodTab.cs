// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsFoodTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsFoodTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly ProtosDb m_protosDb;
    private readonly ProductsManager m_productsManager;
    private ChartWithRangeSelectors m_chart;

    public StatsFoodTab(ICalendar calendar, ProtosDb protosDb, ProductsManager productsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("FoodStats");
      this.m_calendar = calendar;
      this.m_protosDb = protosDb;
      this.m_productsManager = productsManager;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      this.m_chart.SetTitle(Tr.Consumption.TranslatedString);
      int index = 0;
      foreach (FoodProto foodProto in this.m_protosDb.All<FoodProto>())
      {
        ProductStats statsFor = this.m_productsManager.GetStatsFor(foodProto.Product);
        this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) foodProto.Product.Strings.Name), foodProto.Product.Graphics.IconPath, (ItemStats) statsFor.UsedInSettlementStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[index])));
        ++index;
      }
      this.SetSize<StatsFoodTab>(this.m_chart.GetSize());
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
