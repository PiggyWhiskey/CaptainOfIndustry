// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.ChartLegend
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.InputControl.ResVis;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class ChartLegend : IUiElement
  {
    private readonly StackContainer m_stackContainer;
    private readonly Txt m_noSeriesText;
    private readonly UiBuilder m_builder;
    private readonly GraphChart m_chart;

    public GameObject GameObject => this.m_stackContainer.GameObject;

    public RectTransform RectTransform => this.m_stackContainer.RectTransform;

    public ChartLegend(UiBuilder builder, GraphChart chart, string messageWhenWithoutData = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ChartLegend chartLegend = this;
      this.m_builder = builder;
      this.m_chart = chart;
      this.m_stackContainer = builder.NewStackContainer("Chart Legend").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned);
      for (int index = 0; index < chart.SeriesCount; ++index)
        this.addSeriesRow(chart[index], index);
      chart.DataSeriesAdded += (Action<DataSeriesContainer>) (series => chartLegend.addSeriesRow(series, chart.SeriesCount - 1));
      this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
      if (chart.SeriesCount != 0)
        return;
      messageWhenWithoutData = messageWhenWithoutData ?? "No series available";
      this.m_noSeriesText = builder.NewTxt("NoSeries").SetTextStyle(builder.Style.Layers.ShowLabelTextStyle).SetAlignment(TextAnchor.MiddleCenter).SetText(messageWhenWithoutData).SetWidth<Txt>(200f).AllowVerticalOverflow();
      float preferedHeight = this.m_noSeriesText.GetPreferedHeight();
      this.m_noSeriesText.SetHeight<Txt>(preferedHeight).PutToTopOf<Txt>((IUiElement) this.m_stackContainer, preferedHeight);
    }

    private void addSeriesRow(DataSeriesContainer series, int seriesIndex)
    {
      new LayersLegendItemView((IUiElement) this.m_stackContainer, this.m_builder, series.Data.Name, series.Style.IconPath.ValueOrNull ?? "TODO", new ColorRgba?(series.Style.LineColor), (Action<LayersLegendItemView>) (view =>
      {
        view.Toggle();
        series.IsActive = view.IsOn;
        if (series.IsActive)
          this.m_chart.ShowSeries(seriesIndex);
        else
          this.m_chart.HideSeries(seriesIndex);
      }), true).AppendTo<LayersLegendItemView>(this.m_stackContainer, new float?((float) this.m_builder.Style.Layers.ItemHeight));
      if (this.m_noSeriesText == null)
        return;
      this.m_noSeriesText.Hide<Txt>();
    }
  }
}
