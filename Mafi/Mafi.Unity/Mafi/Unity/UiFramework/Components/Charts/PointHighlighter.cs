// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.PointHighlighter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class PointHighlighter
  {
    private readonly RectTransform m_transform;
    private readonly HorizontalAxis m_horizontalAxis;
    private readonly VerticalAxis m_verticalAxis;
    private readonly UiBuilder m_builder;
    private readonly ChartStyle m_style;
    private readonly IIndexable<GraphChartDataView.LineContainer> m_lines;
    private readonly MbPool<PointHighlightMb> m_highlightPool;
    private readonly Lyst<PointHighlightMb> m_highlights;

    public PointHighlighter(
      UiBuilder builder,
      RectTransform parent,
      ChartStyle style,
      IIndexable<GraphChartDataView.LineContainer> lines,
      HorizontalAxis horizontalAxis,
      VerticalAxis verticalAxis)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_highlights = new Lyst<PointHighlightMb>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.m_style = style;
      this.m_lines = lines.CheckNotNull<IIndexable<GraphChartDataView.LineContainer>>();
      this.m_horizontalAxis = horizontalAxis.CheckNotNull<HorizontalAxis>();
      this.m_verticalAxis = verticalAxis.CheckNotNull<VerticalAxis>();
      GameObject objectToPosition = new GameObject("Points highlight");
      this.m_transform = objectToPosition.AddComponent<RectTransform>();
      this.m_transform.SetParent((Transform) parent, false);
      LayoutHelper.Fill(parent.gameObject, objectToPosition);
      this.m_highlightPool = new MbPool<PointHighlightMb>("Point highlights pool", 1, new Func<PointHighlightMb>(this.createPointHighlightMb), (Action<PointHighlightMb>) (x => { }));
    }

    private PointHighlightMb createPointHighlightMb()
    {
      PointHighlightMb pointHighlightMb = new GameObject("Point highlight", new System.Type[1]
      {
        typeof (RectTransform)
      }).AddComponent<PointHighlightMb>();
      pointHighlightMb.Initialize(this.m_builder, this.m_style.PointHighlightSize, this.m_verticalAxis.Style.FontSize);
      return pointHighlightMb;
    }

    public void UpdateAxes() => this.HideHighlights();

    public void OnMouseMove(Vector2 mousePosition)
    {
      this.HideHighlights();
      int index = (int) this.m_horizontalAxis.Measure.PositionToValue(mousePosition.x);
      if (index < 0)
        return;
      foreach (GraphChartDataView.LineContainer line in this.m_lines)
      {
        if (line.SeriesContainer.IsActive && line.Data.Values.Count > index)
        {
          long num = line.Data.Values[index];
          Vector2 position = new Vector2(this.m_horizontalAxis.Measure.ValueToPosition((long) index), this.m_verticalAxis.Measure.ValueToPosition(num));
          PointHighlightMb instance = this.m_highlightPool.GetInstance();
          this.m_highlights.Add(instance);
          instance.transform.SetParent((Transform) this.m_transform, false);
          instance.Show(position, line.SeriesContainer.Style.PointsHighlightMaterial, line.SeriesContainer.Data.DataFormatter.FormatValue(num));
        }
      }
    }

    public void HideHighlights()
    {
      for (int index = 0; index < this.m_highlights.Count; ++index)
      {
        PointHighlightMb highlight = this.m_highlights[index];
        highlight.Hide();
        highlight.transform.SetParent((Transform) null, false);
        this.m_highlightPool.ReturnInstance(ref highlight);
      }
      this.m_highlights.Clear();
    }
  }
}
