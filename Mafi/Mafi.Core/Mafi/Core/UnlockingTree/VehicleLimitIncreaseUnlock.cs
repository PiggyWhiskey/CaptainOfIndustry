// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.VehicleLimitIncreaseUnlock
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Utils;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  /// <summary>Increase global vehicle cap.</summary>
  public class VehicleLimitIncreaseUnlock : IUnlockUnitWithTitleAndIcon, IUnlockNodeUnit
  {
    private static readonly LocStr1Plural TITLE_STR;
    public readonly int LimitIncrease;

    public LocStrFormatted Title { get; }

    public LocStrFormatted Description => LocStrFormatted.Empty;

    public Option<string> IconPath { get; }

    public bool HideInUI => false;

    public VehicleLimitIncreaseUnlock(int limitIncrease, string iconPath)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LimitIncrease = limitIncrease;
      this.Title = VehicleLimitIncreaseUnlock.TITLE_STR.Format(limitIncrease.ToString(), limitIncrease);
      this.IconPath = (Option<string>) iconPath;
    }

    public bool MatchesSearchQuery(string[] query)
    {
      return UiSearchUtils.Matches(this.Title.Value, query);
    }

    static VehicleLimitIncreaseUnlock()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleLimitIncreaseUnlock.TITLE_STR = Loc.Str1Plural("VehicleLimitIncrease", "+{0} VEHICLE CAP", "+{0} VEHICLES CAP", "vehicles cap increase increase, all caps");
    }
  }
}
