// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Game.PercentSettingInfo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Game
{
  public class PercentSettingInfo : DiffSettingInfo<Percent>
  {
    private readonly bool m_negativeIsBetter;

    public PercentSettingInfo(
      string valueMemberName,
      LocStrFormatted title,
      LocStrFormatted tooltip,
      Percent[] options,
      bool negativeIsBetter)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(valueMemberName, title, tooltip, options);
      this.m_negativeIsBetter = negativeIsBetter;
    }

    public override string CreateLabel(Percent value)
    {
      if (value == Percent.MaxValue)
        return TrCore.Option_Unlimited.TranslatedString ?? "";
      if (value.IsZero)
        return TrCore.OptionValStandard.TranslatedString ?? "";
      return value.IsNegative ? TrCore.OptionValReduced.Format(string.Format("{0}%", (object) value.Abs().ToIntPercentRounded())).Value : TrCore.OptionValIncreased.Format(string.Format("{0}%", (object) value.Abs().ToIntPercentRounded())).Value;
    }

    public override PropDifficultyRating GetDifficultyRating(Percent value)
    {
      if (value.IsNegative && this.m_negativeIsBetter || value.IsPositive && !this.m_negativeIsBetter)
        return PropDifficultyRating.Easy;
      return value.IsNegative && !this.m_negativeIsBetter || value.IsPositive && this.m_negativeIsBetter ? PropDifficultyRating.Hard : PropDifficultyRating.Standard;
    }

    protected override string ValToStringInternal(Percent value) => value.ToString();

    protected override DiffSettingInfo<Percent>.DiffResult CompareValuesInternal(
      Percent oldValue,
      Percent newValue)
    {
      if (oldValue == newValue)
        return DiffSettingInfo<Percent>.DiffResult.Same;
      return oldValue < newValue ? (!this.m_negativeIsBetter ? DiffSettingInfo<Percent>.DiffResult.NewIsEasier : DiffSettingInfo<Percent>.DiffResult.NewIsHarder) : (!this.m_negativeIsBetter ? DiffSettingInfo<Percent>.DiffResult.NewIsHarder : DiffSettingInfo<Percent>.DiffResult.NewIsEasier);
    }
  }
}
