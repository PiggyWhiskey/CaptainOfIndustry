// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.ChartWithRangeSelectors
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  internal class ChartWithRangeSelectors : IUiElement
  {
    private readonly StackContainer m_stackContainer;
    private readonly StatsChartView m_chart;
    private readonly Txt m_title;

    public GameObject GameObject => this.m_stackContainer.GameObject;

    public RectTransform RectTransform => this.m_stackContainer.RectTransform;

    public StatsChartView Chart => this.m_chart;

    public ChartWithRangeSelectors(
      IUiElement parent,
      UiBuilder builder,
      ICalendar calendar,
      string messageWhenNoData,
      int width,
      int maxHeight)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ChartWithRangeSelectors withRangeSelectors = this;
      this.m_stackContainer = builder.NewStackContainer("Tab's container", parent).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic);
      int num = 40;
      StackContainer buttonsContainer = builder.NewStackContainer("Buttons container", (IUiElement) this.m_stackContainer).SetStackingDirection(StackContainer.Direction.RightToLeft).SetItemSpacing(4f).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).AppendTo<StackContainer>(this.m_stackContainer, new float?((float) num), Offset.TopBottom(10f) + Offset.LeftRight(20f));
      Btn max = builder.NewBtn("Max", (IUiElement) buttonsContainer).EnableDynamicSize().SetText((LocStrFormatted) Tr.StatsRange__Max).SetButtonStyle(builder.Style.Global.GeneralBtn).AppendTo<Btn>(buttonsContainer);
      Btn lastNYears = builder.NewBtn("Yearly", (IUiElement) buttonsContainer).EnableDynamicSize().SetText(Tr.StatsRange__Years.Format("100", 100)).SetButtonStyle(builder.Style.Global.GeneralBtn).AppendTo<Btn>(buttonsContainer);
      Btn lastNMonths = builder.NewBtn("Monthly", (IUiElement) buttonsContainer).EnableDynamicSize().SetText(Tr.StatsRange__Months.Format(120.ToStringCached(), 120)).SetButtonStyle(builder.Style.Global.GeneralBtn).AppendTo<Btn>(buttonsContainer);
      Btn lastNDays = builder.NewBtn("Daily", (IUiElement) buttonsContainer).EnableDynamicSize().SetText(Tr.StatsRange__Days.Format(120.ToStringCached(), 120)).SetButtonStyle(builder.Style.Global.GeneralBtn).AppendTo<Btn>(buttonsContainer);
      buttonsContainer.HideItem((IUiElement) lastNDays);
      this.m_title = builder.NewTitle("Title", (IUiElement) buttonsContainer).SetText("").SetAlignment(TextAnchor.MiddleLeft).PutToLeftOf<Txt>((IUiElement) buttonsContainer, 250f);
      this.m_chart = new StatsChartView((IUiElement) this.m_stackContainer, builder, calendar, messageWhenNoData, width, maxHeight - num, new Action(onDailySeriesAdded));
      max.OnClick((Action) (() => setRange(StatsDataRange.QuarterCenturies)));
      lastNYears.OnClick((Action) (() => setRange(StatsDataRange.Last100Years)));
      lastNMonths.OnClick((Action) (() => setRange(StatsDataRange.Last120Months)));
      lastNDays.OnClick((Action) (() => setRange(StatsDataRange.Last120Days)));
      this.m_stackContainer.SetWidth<StackContainer>(this.m_chart.GetWidth());
      this.m_stackContainer.Append((IUiElement) this.m_chart, new float?());
      setRange(StatsDataRange.Last120Months);

      void onDailySeriesAdded() => buttonsContainer.ShowItem((IUiElement) lastNDays);

      void setRange(StatsDataRange range)
      {
        max.SetEnabled(range != StatsDataRange.QuarterCenturies);
        lastNYears.SetEnabled(range != StatsDataRange.Last100Years);
        lastNMonths.SetEnabled(range != StatsDataRange.Last120Months);
        lastNDays.SetEnabled(range != 0);
        withRangeSelectors.m_chart.SetRange(range);
      }
    }

    public void SetTitle(string title) => this.m_title.SetText(title);

    public void RenderUpdate(GameTime gameTime) => this.m_chart.RenderUpdate(gameTime);

    public void SyncUpdate(GameTime gameTime) => this.m_chart.SyncUpdate(gameTime);
  }
}
