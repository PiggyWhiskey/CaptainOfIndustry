// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.HorizontalAxis
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class HorizontalAxis
  {
    private readonly UiBuilder m_builder;
    private readonly ChartStyle m_chartStyle;
    public readonly AxisStyle Style;
    private readonly RectTransform m_transform;
    private readonly Lyst<Txt> m_labels;
    private readonly Lyst<Txt> m_labelsPool;

    public AxisParams<int> Parameters { get; set; }

    public AxisMeasure Measure { get; private set; }

    public HorizontalAxis(
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
      this.m_transform = new GameObject("Horizontal axis").AddComponent<RectTransform>();
      this.m_transform.SetParent((Transform) parent, false);
      this.m_transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, chartStyle.DataViewWidth);
      this.m_transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, chartStyle.HorizontalAxisHeight);
      this.m_transform.localPosition = (Vector3) new Vector2(chartStyle.VerticalAxisWidth / 2f, (float) (-((double) chartStyle.Height - (double) chartStyle.HorizontalAxisHeight) / 2.0));
      this.Parameters = AxisParams<int>.EMPTY;
      this.Measure = AxisMeasure.EMPTY;
    }

    public void Generate(ILabelsProvider labelsProvider, int count)
    {
      this.destroy();
      if (count == 0)
        return;
      int min = (count - 1).Max(0);
      this.Measure = new AxisMeasure((float) (-(double) this.m_chartStyle.DataViewWidth / 2.0), this.m_chartStyle.DataViewWidth / 2f, this.m_chartStyle.LineWidth, this.m_chartStyle.PointSize, new MinMaxPair<long>((long) min, 0L));
      int num1 = (this.Parameters.RecommendedLabelsCount - 1).Max(1);
      int num2 = (min / num1).Max(1);
      int num3 = num2 > 2 ? (num2 > 6 ? (num2 > 12 ? (num2 > 24 ? num2.RoundToMultipleOf(10) : 20) : 10) : 5) : 1;
      this.addLabel(labelsProvider.GetLabelAt(0), this.Measure.ValueToPosition(0L));
      for (int i = num3 == 1 ? 1 : num3 - 1; i <= min; i += num3)
        this.addLabel(labelsProvider.GetLabelAt(i), this.Measure.ValueToPosition((long) i));
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
      element.SetSize<Txt>(element.GetPreferedSize());
      element.SetLocalPosition<Txt>((Vector3) new Vector2(position, 0.0f));
      this.m_labels.Add(element);
    }
  }
}
