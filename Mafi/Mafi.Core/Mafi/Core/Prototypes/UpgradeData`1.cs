// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.UpgradeData`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public class UpgradeData<T> : IUpgradeData where T : Proto, IProtoWithUpgrade
  {
    public Option<T> NextTier { get; }

    public Option<IProtoWithUpgrade> NextTierNonGeneric => this.NextTier.As<IProtoWithUpgrade>();

    public Option<T> PreviousTier { get; private set; }

    public Option<IProtoWithUpgrade> PreviousTierNonGeneric
    {
      get => this.PreviousTier.As<IProtoWithUpgrade>();
    }

    internal int PreviousTiersCount { get; private set; }

    public UpgradeData(T owner, Option<T> nextTier)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.NextTier = nextTier;
      if (!nextTier.HasValue)
        return;
      ++((IProtoWithUpgrade<T>) (object) nextTier.Value).Upgrade.PreviousTiersCount;
      if (!nextTier.Value.UpgradeNonGeneric.PreviousTierNonGeneric.IsNone)
        return;
      ((IProtoWithUpgrade<T>) (object) nextTier.Value).Upgrade.PreviousTier = (Option<T>) owner;
    }
  }
}
