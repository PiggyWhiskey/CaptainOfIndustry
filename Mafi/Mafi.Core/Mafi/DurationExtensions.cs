// Decompiled with JetBrains decompiler
// Type: Mafi.DurationExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class DurationExtensions
  {
    [Pure]
    public static Duration Ticks(this int value) => Duration.FromTicks(value);

    [Pure]
    public static Duration Seconds(this int value) => Duration.FromSec(value);

    [Pure]
    public static Duration Seconds(this double value) => Duration.FromSec(value);

    [Pure]
    public static Duration Minutes(this int value) => Duration.FromMin((double) value);

    [Pure]
    public static Duration Minutes(this double value) => Duration.FromMin(value);

    [Pure]
    public static Duration Days(this int value) => Duration.FromDays(value);

    [Pure]
    public static Duration Months(this int value) => Duration.FromMonths(value);

    [Pure]
    public static Duration Years(this int value) => Duration.FromYears(value);

    [Pure]
    public static Duration ScaleByAnimationSpeed(this Duration duration, Percent speed)
    {
      return (duration.Ticks / speed.ToFix32()).ToIntRounded().Ticks();
    }
  }
}
