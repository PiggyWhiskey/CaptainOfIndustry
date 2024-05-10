// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.GraphChartDataView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class GraphChartDataView
  {
    private readonly ChartStyle m_style;
    private readonly HorizontalAxis m_horizontalAxis;
    private readonly VerticalAxis m_verticalAxis;
    private readonly Panel m_viewPanel;
    private readonly RectTransform m_dataContainerTransform;
    private readonly Lyst<GraphChartDataView.LineContainer> m_lines;
    private readonly PointHighlighter m_pointHighlighter;

    public RectTransform RectTransform => this.m_viewPanel.RectTransform;

    public GraphChartDataView(
      UiBuilder builder,
      RectTransform parent,
      ChartStyle style,
      HorizontalAxis horizontalAxis,
      VerticalAxis verticalAxis)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lines = new Lyst<GraphChartDataView.LineContainer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_style = style;
      this.m_horizontalAxis = horizontalAxis;
      this.m_verticalAxis = verticalAxis;
      this.m_viewPanel = new Panel(builder, "Chart view");
      this.m_viewPanel.RectTransform.SetParent((Transform) parent, false);
      this.m_viewPanel.SetSize<Panel>(new Vector2(style.DataViewWidth, style.DataViewHeight));
      this.m_viewPanel.SetLocalPosition<Panel>((Vector3) new Vector2(style.VerticalAxisWidth / 2f, style.HorizontalAxisHeight / 2f));
      this.m_viewPanel.SetBackground(style.BackgroundColor);
      GameObject objectToPosition = new GameObject("Data container", new System.Type[1]
      {
        typeof (RectTransform)
      });
      this.m_dataContainerTransform = (RectTransform) objectToPosition.transform;
      LayoutHelper.Fill(this.m_viewPanel.GameObject, objectToPosition);
      this.m_pointHighlighter = new PointHighlighter(builder, this.m_viewPanel.RectTransform, this.m_style, (IIndexable<GraphChartDataView.LineContainer>) this.m_lines, horizontalAxis, verticalAxis);
    }

    public void Generate()
    {
      foreach (GraphChartDataView.LineContainer line in this.m_lines)
      {
        line.Line.Generate(this.m_horizontalAxis.Measure, this.m_verticalAxis.Measure);
        line.Points.Generate(this.m_horizontalAxis.Measure, this.m_verticalAxis.Measure);
        if (line.SeriesContainer.IsActive)
          line.Show();
        else
          line.Hide();
      }
      this.m_pointHighlighter.UpdateAxes();
    }

    public void Hide()
    {
      foreach (GraphChartDataView.LineContainer line in this.m_lines)
        line.Hide();
    }

    public void HideSeries(int index) => this.m_lines[index].Hide();

    public void ShowSeries(int index) => this.m_lines[index].Show();

    public void AddSeries(DataSeriesContainer seriesContainer)
    {
      DataSeriesStyle style = seriesContainer.Style;
      GraphChartLine line = new GraphChartLine(this.m_dataContainerTransform, seriesContainer.Data, this.m_style.LineWidth, style.LineMaterial, style.LineColor);
      this.m_lines.Add(new GraphChartDataView.LineContainer(new GraphChartPoints(this.m_dataContainerTransform, seriesContainer.Data, this.m_style.PointSize, style.PointsMaterial, style.PointsColor), line, seriesContainer));
    }

    public void OnMouseMove(Vector2 position) => this.m_pointHighlighter.OnMouseMove(position);

    public readonly struct LineContainer
    {
      public readonly GraphChartPoints Points;
      public readonly GraphChartLine Line;
      public readonly DataSeriesContainer SeriesContainer;

      public DataSeries Data => this.SeriesContainer.Data;

      public LineContainer(
        GraphChartPoints points,
        GraphChartLine line,
        DataSeriesContainer seriesContainer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Points = points;
        this.Line = line;
        this.SeriesContainer = seriesContainer;
      }

      public void Hide()
      {
        this.Points.Hide();
        this.Line.Hide();
      }

      public void Show()
      {
        this.Points.Show();
        this.Line.Show();
      }
    }
  }
}
