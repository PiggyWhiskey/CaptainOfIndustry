// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.GraphChart
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  /// <summary>
  /// A 2D chart showing data as either points or lines connecting the points.
  /// </summary>
  public class GraphChart : IUiElement, IMouseHandlingMbListener
  {
    private readonly ChartStyle m_style;
    private readonly Panel m_container;
    private readonly Lyst<DataSeriesContainer> m_data;
    private readonly GraphChartDataView m_dataView;
    private readonly HorizontalAxis m_horizontalAxis;
    private readonly VerticalAxis m_verticalAxis;

    public event Action<DataSeriesContainer> DataSeriesAdded;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public AxisParams<int> HorizontalAxisParams
    {
      get => this.m_horizontalAxis.Parameters;
      set => this.m_horizontalAxis.Parameters = value;
    }

    public AxisParams<long> VerticalAxisParams
    {
      get => this.m_verticalAxis.Parameters;
      set => this.m_verticalAxis.Parameters = value;
    }

    public int SeriesCount => this.m_data.Count;

    public DataSeriesContainer this[int index] => this.m_data[index];

    public GraphChart(
      IUiElement parent,
      UiBuilder builder,
      ChartStyle style,
      AxisStyle verticalAxisStyle,
      AxisStyle horizontalAxisStyle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_data = new Lyst<DataSeriesContainer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_style = style;
      this.m_container = new Panel(builder, "Chart", parent.GameObject);
      this.m_container.SetSize<Panel>(new Vector2(style.Width, style.Height));
      this.m_horizontalAxis = new HorizontalAxis(builder, this.RectTransform, style, horizontalAxisStyle);
      this.m_verticalAxis = new VerticalAxis(builder, this.RectTransform, style, verticalAxisStyle);
      this.m_dataView = new GraphChartDataView(builder, this.RectTransform, style, this.m_horizontalAxis, this.m_verticalAxis);
      this.m_container.GameObject.AddComponent<MouseHandlingMb>().Initialize((IMouseHandlingMbListener) this, this.m_container.RectTransform, builder.MainCanvas.GameObject.GetComponent<GraphicRaycaster>());
    }

    public void AddSeries(DataSeries data, DataSeriesStyle style)
    {
      DataSeriesContainer seriesContainer = new DataSeriesContainer(data, style);
      this.m_data.Add(seriesContainer);
      this.m_dataView.AddSeries(seriesContainer);
      Action<DataSeriesContainer> dataSeriesAdded = this.DataSeriesAdded;
      if (dataSeriesAdded == null)
        return;
      dataSeriesAdded(seriesContainer);
    }

    /// <summary>
    /// (Re)Generates graphics of the chart. Invoke after change in data.
    /// </summary>
    public void Generate()
    {
      if (this.m_data.IsEmpty)
      {
        this.hideData();
      }
      else
      {
        int num = 0;
        foreach (DataSeriesContainer dataSeriesContainer in this.m_data)
          num = num.Max(dataSeriesContainer.Data.Values.Count);
        if (num == 0)
        {
          this.hideData();
        }
        else
        {
          this.m_horizontalAxis.Generate(this.m_data.First.Data.LabelsProvider, num);
          this.m_verticalAxis.Generate((IIndexable<DataSeriesContainer>) this.m_data);
          this.m_dataView.Generate();
        }
      }
    }

    private void hideData()
    {
      this.m_horizontalAxis.Hide();
      this.m_verticalAxis.Hide();
      this.m_dataView.Hide();
    }

    public void HideSeries(int index)
    {
      this.m_data[index].IsActive = false;
      this.m_dataView.HideSeries(index);
    }

    public void ShowSeries(int index)
    {
      this.m_data[index].IsActive = true;
      this.m_dataView.ShowSeries(index);
    }

    void IMouseHandlingMbListener.OnMouseMove(Vector2 position)
    {
      Vector2? positionInChildRect = this.getPositionInChildRect(this.m_dataView.RectTransform, position);
      if (!positionInChildRect.HasValue)
        return;
      this.m_dataView.OnMouseMove(positionInChildRect.Value);
    }

    private Vector2? getPositionInChildRect(RectTransform childTransform, Vector2 mousePos)
    {
      Vector2 point = mousePos - new Vector2(childTransform.localPosition.x, childTransform.localPosition.y);
      return childTransform.rect.Contains(point) ? new Vector2?(point) : new Vector2?();
    }
  }
}
