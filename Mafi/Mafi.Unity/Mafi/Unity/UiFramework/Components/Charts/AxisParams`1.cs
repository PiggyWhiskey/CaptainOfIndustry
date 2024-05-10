// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.AxisParams`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public struct AxisParams<TValue> where TValue : struct, IComparable<TValue>
  {
    public static readonly AxisParams<TValue> EMPTY;
    public readonly int RecommendedLabelsCount;
    public readonly TValue? RecommendedMin;
    public readonly TValue? RecommendedMax;

    public AxisParams(int recommendedLabelsCount, TValue? recommendedMin = null, TValue? recommendedMax = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.RecommendedLabelsCount = recommendedLabelsCount.CheckPositive();
      Assert.That<TValue?>(recommendedMin).IsLessOrOneIsNull<TValue>(recommendedMax);
      this.RecommendedMin = recommendedMin;
      this.RecommendedMax = recommendedMax;
    }

    static AxisParams()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AxisParams<TValue>.EMPTY = new AxisParams<TValue>(2);
    }
  }
}
