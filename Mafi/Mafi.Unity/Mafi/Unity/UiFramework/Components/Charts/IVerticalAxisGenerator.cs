// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.IVerticalAxisGenerator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public interface IVerticalAxisGenerator
  {
    MinMaxPair<long> GetRange(AxisParams<long> axisParams, IIndexable<DataSeriesContainer> data);

    void GenerateLabels(
      AxisParams<long> axisParams,
      MinMaxPair<long> range,
      Lyst<AxisLabelValue<long>> labels);
  }
}
