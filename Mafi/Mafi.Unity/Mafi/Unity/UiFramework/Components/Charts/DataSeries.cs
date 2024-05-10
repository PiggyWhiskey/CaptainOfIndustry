// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.DataSeries
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Stats;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public sealed class DataSeries
  {
    public readonly LocStrFormatted Name;
    /// <summary>
    /// We store values as longs since this can both integer and fix-point numbers.
    /// </summary>
    public readonly IIndexable<long> Values;
    public readonly ILabelsProvider LabelsProvider;
    public readonly IDataValuesFormatter DataFormatter;

    public DataSeries(
      LocStrFormatted name,
      IIndexable<long> values,
      ILabelsProvider labelsProvider,
      IDataValuesFormatter dataFormatter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name.CheckNotDefaultStruct<LocStrFormatted>();
      this.Values = values.CheckNotNull<IIndexable<long>>();
      this.LabelsProvider = labelsProvider.CheckNotNull<ILabelsProvider>();
      this.DataFormatter = dataFormatter;
    }
  }
}
