// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Fleet.UpgradablePercentProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Fleet
{
  /// <summary>Defines percent upgrade.</summary>
  /// <remarks>Note that this is not real proto as it does not derive from Proto.</remarks>
  public class UpgradablePercentProto
  {
    public readonly Percent BonusValue;
    public readonly Percent BonusMultiplier;

    public UpgradablePercentProto(Percent bonusValue = default (Percent), Percent bonusMultiplier = default (Percent))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.BonusValue = bonusValue;
      this.BonusMultiplier = bonusMultiplier;
    }
  }
}
