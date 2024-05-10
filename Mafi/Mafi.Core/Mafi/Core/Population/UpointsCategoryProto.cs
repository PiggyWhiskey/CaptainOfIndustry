// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.UpointsCategoryProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Population
{
  /// <summary>
  /// Used for statistics and overview table in UI.
  /// 
  /// It works the following way:
  /// Each stats entry has to have UpointsCategoryProto assigned, this give it place in overview.
  /// However each has to have UpointsStatsCategoryProto assigned as well (but it can be this).
  /// - This one is used for further grouping in statistics (history chart data).
  /// 
  /// Example:
  /// A) Boost has UpointsCategoryProto called Boost, and UpointsStatsCategoryProto this (Boost again).
  /// So for Furnace and Boiler boost we get this:
  /// Table:
  /// - Boost =&gt; tooltip showing Furnace and Boiler boost
  /// Stats:
  /// - Boost
  /// B) Settlement service has each own special UpointsCategoryProto but they all share UpointsStatsCategoryProto
  /// So for Water and Food we get this:
  /// Table:
  /// - Water
  /// - Food
  /// Stats:
  /// - Services (containing sum of Water and Food).
  /// </summary>
  public class UpointsCategoryProto : UpointsStatsCategoryProto
  {
    private Proto.ID? m_otherProtoString;
    /// <summary>
    /// If true, UI will sum up all the values under same UpointsCategoryProto but it won't prefix
    /// the count. Used for services for instance as count does not make much sense there.
    /// </summary>
    public readonly bool HideCount;
    public readonly UpointsStatsCategoryProto StatsCategory;

    /// <summary>
    /// One time actions like quick deliver, vehicle recover etc.
    /// </summary>
    public new bool IsOneTimeAction
    {
      get => this.StatsCategory.Id == IdsCore.UpointsStatsCategories.OneTimeAction;
    }

    public new bool IgnoreInStats => this.StatsCategory.Id == IdsCore.UpointsStatsCategories.Ignore;

    public new bool HideInUI => this.IsOneTimeAction || this.IgnoreInStats;

    public UpointsCategoryProto(
      Proto.ID id,
      Proto.Str strings,
      UpointsStatsCategoryProto.Gfx graphics,
      Option<UpointsStatsCategoryProto> statsCategory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, graphics);
      this.Title = this.Strings.Name;
      this.StatsCategory = statsCategory.ValueOrNull ?? (UpointsStatsCategoryProto) this;
    }

    public UpointsCategoryProto(
      Proto.ID otherProtoStrings,
      string iconPath,
      Option<UpointsStatsCategoryProto> statsCategory,
      bool hideCount = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Proto.ID("UpointsCat_" + otherProtoStrings.Value), Proto.Str.Empty, new UpointsStatsCategoryProto.Gfx(iconPath));
      this.m_otherProtoString = new Proto.ID?(otherProtoStrings);
      this.HideCount = hideCount;
      this.StatsCategory = statsCategory.ValueOrNull ?? (UpointsStatsCategoryProto) this;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      if (!this.m_otherProtoString.HasValue)
        return;
      this.Title = protosDb.GetOrThrow<Proto>(this.m_otherProtoString.Value).Strings.Name;
    }
  }
}
