// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.ExhaustExtensions
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Core.Factory.Recipes;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal static class ExhaustExtensions
  {
    public const int EXHAUST_FULL_UTIL_PER_10_SEC = 8;
    public const int EXHAUST_HALF_UTIL_PER_10_SEC = 4;
    public const int EXHAUST_QUARTER_UTIL_PER_10_SEC = 2;

    [MustUseReturnValue]
    public static T AddQuarterExhaust<T>(
      this IRecipeProtoBuilderState<T> builder,
      string portSelector = "*")
    {
      return builder.AddOutput<T>(portSelector, Ids.Products.Exhaust, (builder.RecipeDuration.Value.Seconds / 10 * 2).ToIntRounded().Quantity(), true);
    }

    [MustUseReturnValue]
    public static T AddHalfExhaust<T>(this IRecipeProtoBuilderState<T> builder, string portSelector = "*")
    {
      return builder.AddOutput<T>(portSelector, Ids.Products.Exhaust, (builder.RecipeDuration.Value.Seconds / 10 * 4).ToIntRounded().Quantity(), true);
    }

    [MustUseReturnValue]
    public static T AddFullExhaust<T>(this IRecipeProtoBuilderState<T> builder, string portSelector = "*")
    {
      return builder.AddOutput<T>(portSelector, Ids.Products.Exhaust, (builder.RecipeDuration.Value.Seconds / 10 * 8).ToIntRounded().Quantity(), true);
    }

    [MustUseReturnValue]
    public static T AddDoubleExhaust<T>(
      this IRecipeProtoBuilderState<T> builder,
      string portSelector = "*")
    {
      return builder.AddOutput<T>(portSelector, Ids.Products.Exhaust, (builder.RecipeDuration.Value.Seconds / 10 * 2 * 8).ToIntRounded().Quantity(), true);
    }

    [MustUseReturnValue]
    public static T AddAirPollutionFromBurning<T>(
      this IRecipeProtoBuilderState<T> builder,
      Quantity quantity)
    {
      return builder.AddAirPollution<T>(2 * quantity);
    }

    [MustUseReturnValue]
    public static T AddAirPollutionFromExhaust<T>(
      this IRecipeProtoBuilderState<T> builder,
      Quantity quantity)
    {
      return builder.AddAirPollution<T>(quantity);
    }

    [MustUseReturnValue]
    public static T AddAirPollutionFromCarbonDioxide<T>(
      this IRecipeProtoBuilderState<T> builder,
      Quantity quantity)
    {
      return builder.AddAirPollution<T>(quantity / 2);
    }

    [MustUseReturnValue]
    public static T AddAirPollution<T>(this IRecipeProtoBuilderState<T> builder, Quantity quantity)
    {
      return builder.AddOutput<T>("VIRTUAL", Ids.Products.PollutedAir, quantity);
    }
  }
}
