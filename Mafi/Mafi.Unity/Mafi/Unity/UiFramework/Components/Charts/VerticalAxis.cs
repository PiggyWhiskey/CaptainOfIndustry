// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.VerticalAxis
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class VerticalAxis
  {
    private readonly UiBuilder m_builder;
    private readonly ChartStyle m_chartStyle;
    public readonly AxisStyle Style;
    private readonly RectTransform m_transform;
    private readonly Lyst<Txt> m_labels;
    private readonly Lyst<Txt> m_labelsPool;

    public AxisParams<long> Parameters { get; set; }

    /// <summary>
    /// Generated after call to <see cref="M:Mafi.Unity.UiFramework.Components.Charts.VerticalAxis.Generate(Mafi.Collections.ReadonlyCollections.IIndexable{Mafi.Unity.UiFramework.Components.Charts.DataSeriesContainer})" />
    /// </summary>
    public AxisMeasure Measure { get; private set; }

    public VerticalAxis(
      UiBuilder builder,
      RectTransform parent,
      ChartStyle chartStyle,
      AxisStyle style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_labels = new Lyst<Txt>();
      this.m_labelsPool = new Lyst<Txt>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_chartStyle = chartStyle;
      this.Style = style;
      this.m_transform = new GameObject("Vertical axis").AddComponent<RectTransform>();
      this.m_transform.SetParent((Transform) parent, false);
      this.m_transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, chartStyle.VerticalAxisWidth);
      this.m_transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, chartStyle.DataViewHeight);
      this.m_transform.localPosition = (Vector3) new Vector2((float) (-((double) chartStyle.Width - (double) chartStyle.VerticalAxisWidth) / 2.0), chartStyle.HorizontalAxisHeight / 2f);
      this.Parameters = AxisParams<long>.EMPTY;
      this.Measure = AxisMeasure.EMPTY;
    }

    public void Generate(IIndexable<DataSeriesContainer> data)
    {
      this.destroy();
      long defaultValue = 1;
      long tickSize = 1;
      MinMaxPair<long> valuesRange;
      IDataValuesFormatter dataValuesFormatter;
      if (data.IsEmpty<DataSeriesContainer>())
      {
        valuesRange = new MinMaxPair<long>(0L, this.Parameters.RecommendedMax.GetValueOrDefault(1L));
        dataValuesFormatter = (IDataValuesFormatter) null;
      }
      else
      {
        foreach (DataSeriesContainer dataSeriesContainer in data)
        {
          foreach (long num in dataSeriesContainer.Data.Values)
          {
            if (num > defaultValue)
              defaultValue = num;
          }
        }
        long rangeMax = this.Parameters.RecommendedMax.GetValueOrDefault(defaultValue).Max(defaultValue);
        dataValuesFormatter = data[0].Data.DataFormatter;
        dataValuesFormatter.GetRangeAndTicksMax(0L, rangeMax, this.Parameters.RecommendedLabelsCount, out rangeMax, out tickSize);
        valuesRange = new MinMaxPair<long>(0L, rangeMax);
      }
      this.Measure = new AxisMeasure((float) (-(double) this.m_chartStyle.DataViewHeight / 2.0), this.m_chartStyle.DataViewHeight / 2f, this.m_chartStyle.LineWidth, this.m_chartStyle.PointSize, valuesRange);
      if (dataValuesFormatter == null)
      {
        this.addLabel(new LocStrFormatted("0"), this.Measure.ValueToPosition(0L));
        this.addLabel(new LocStrFormatted("1"), this.Measure.ValueToPosition(1L));
      }
      else
      {
        for (long min = valuesRange.Min; min <= valuesRange.Max; min += tickSize)
          this.addLabel(new LocStrFormatted(dataValuesFormatter.FormatValue(min).Value.Replace(' ', '\n')), this.Measure.ValueToPosition(min));
      }
    }

    public void Hide() => this.destroy();

    private void destroy()
    {
      foreach (Txt label in this.m_labels)
        label.GameObject.SetActive(false);
      this.m_labelsPool.AddRange(this.m_labels);
      this.m_labels.Clear();
    }

    private void addLabel(LocStrFormatted text, float position)
    {
      Txt element = this.m_labelsPool.IsNotEmpty ? this.m_labelsPool.PopLast() : new Txt(this.m_builder, "label");
      element.GameObject.SetActive(true);
      element.RectTransform.SetParent((Transform) this.m_transform, false);
      element.SetTextStyle(new TextStyle(ColorRgba.White, this.Style.FontSize));
      element.SetText(text);
      element.SetAlignment(TextAnchor.MiddleCenter);
      element.SetSize<Txt>(element.GetPreferedSize());
      element.SetLocalPosition<Txt>((Vector3) new Vector2(0.0f, position));
      this.m_labels.Add(element);
    }
  }
}
