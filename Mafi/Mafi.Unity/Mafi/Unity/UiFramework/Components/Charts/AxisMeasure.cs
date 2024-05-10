// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.AxisMeasure
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public sealed class AxisMeasure
  {
    public static readonly AxisMeasure EMPTY;
    private readonly float m_valueToPositionMult;
    public readonly float ViewStartPosition;
    /// <summary>Length of the visible part of the axis in GUI.</summary>
    public readonly float ViewLength;
    public readonly MinMaxPair<long> ValuesRange;

    public float ViewEndPosition => this.ViewStartPosition + this.ViewLength;

    public AxisMeasure(
      float startViewPos,
      float endViewPos,
      float dataLineWidth,
      float dataPointSize,
      MinMaxPair<long> valuesRange)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ValuesRange = valuesRange;
      float num1 = dataLineWidth.Max(dataPointSize) / 2f;
      this.ViewStartPosition = startViewPos + num1;
      Assert.That<float>(startViewPos).IsLessOrEqual(endViewPos);
      this.ViewLength = (float) ((double) endViewPos - (double) startViewPos - 2.0 * (double) num1);
      Assert.That<float>(this.ViewLength).IsPositive();
      long num2 = this.ValuesRange.Max - this.ValuesRange.Min;
      if (num2 == 0L)
        num2 = 1L;
      this.m_valueToPositionMult = this.ViewLength / (float) num2;
    }

    public float ValueToPosition(long value)
    {
      return this.ViewStartPosition + (float) (value - this.ValuesRange.Min) * this.m_valueToPositionMult;
    }

    public long PositionToValue(float position)
    {
      return this.m_valueToPositionMult.IsNearZero() ? -1L : ((position - this.ViewStartPosition) / this.m_valueToPositionMult).RoundToLong() + this.ValuesRange.Min;
    }

    static AxisMeasure()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AxisMeasure.EMPTY = new AxisMeasure(0.0f, 1f, 0.0f, 0.0f, new MinMaxPair<long>(0L, 1L));
    }
  }
}
