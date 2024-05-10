// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.IDataValuesFormatter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Stats
{
  public interface IDataValuesFormatter
  {
    LocStrFormatted FormatValue(long value);

    /// <summary>
    /// Returns range max and tick size for given range that is a "nice" round number.
    /// The range should have approximately <paramref name="recommendedCount" /> ticks.
    /// </summary>
    void GetRangeAndTicksMax(
      long minValue,
      long maxValue,
      int recommendedCount,
      out long rangeMax,
      out long tickSize);
  }
}
