// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.TreePlantingGroupProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  public class TreePlantingGroupProto : 
    Proto,
    IProtoWithIconAndName,
    IProtoWithIcon,
    IProto,
    IProtoWithPropertiesUpdate
  {
    public readonly ImmutableArray<TreeProto> Trees;
    private readonly Duration m_timeTo40PercentGrowth;
    private readonly Duration m_timeTo60PercentGrowth;
    private readonly Duration m_timeTo80PercentGrowth;
    private readonly Duration m_timeTo100PercentGrowth;
    public readonly Proto.Gfx Graphics;

    public ProductQuantity ProductWhenHarvested { get; }

    public Duration TimeTo40PercentGrowth { get; private set; }

    public Duration TimeTo60PercentGrowth { get; private set; }

    public Duration TimeTo80PercentGrowth { get; private set; }

    public Duration TimeTo100PercentGrowth { get; private set; }

    public QuantityFormatter QuantityFormatter { get; }

    public string IconPath { get; }

    public TreePlantingGroupProto(
      Proto.ID id,
      ImmutableArray<TreeProto> trees,
      ProductQuantity productWhenHarvested,
      Duration timeTo40PercentGrowth,
      Duration timeTo60PercentGrowth,
      Duration timeTo80PercentGrowth,
      Duration timeTo100PercentGrowth,
      string iconPath)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.Trees = trees;
      this.ProductWhenHarvested = productWhenHarvested.CheckNotDefaultStruct<ProductQuantity>();
      this.TimeTo40PercentGrowth = this.m_timeTo40PercentGrowth = timeTo40PercentGrowth.CheckNotNegative();
      this.TimeTo60PercentGrowth = this.m_timeTo60PercentGrowth = timeTo60PercentGrowth.CheckNotNegative();
      this.TimeTo80PercentGrowth = this.m_timeTo80PercentGrowth = timeTo80PercentGrowth.CheckNotNegative();
      this.TimeTo100PercentGrowth = this.m_timeTo100PercentGrowth = timeTo100PercentGrowth.CheckNotNegative();
      foreach (TreeProto tree in trees)
        tree.SetPlantingGroupProto(this);
      this.IconPath = iconPath.CheckNotNull<string>();
      this.QuantityFormatter = (QuantityFormatter) ProductCountQuantityFormatter.Instance;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      protosDb.TrackProperty((IProtoWithPropertiesUpdate) this, IdsCore.PropertyIds.TreesGrowthSpeed.Value);
    }

    public void OnPropertyUpdated(IProperty property)
    {
      Percent percent;
      if (!property.TryGetValueAs<Percent>(IdsCore.PropertyIds.TreesGrowthSpeed, out percent))
        return;
      Fix32 fix32 = percent.ToFix32();
      this.TimeTo40PercentGrowth = apply(fix32, this.m_timeTo40PercentGrowth);
      this.TimeTo60PercentGrowth = apply(fix32, this.m_timeTo60PercentGrowth);
      this.TimeTo80PercentGrowth = apply(fix32, this.m_timeTo80PercentGrowth);
      this.TimeTo100PercentGrowth = apply(fix32, this.m_timeTo100PercentGrowth);

      static Duration apply(Fix32 speed, Duration duration)
      {
        return (duration.Months / speed).ToIntRounded().Months();
      }
    }
  }
}
