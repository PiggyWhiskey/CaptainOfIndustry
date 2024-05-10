// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.UpgradeHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public static class UpgradeHelper
  {
    public static bool AreProtosInSameUpgradeChain(Proto first, Proto second)
    {
      if (first == second)
        return true;
      if (!(first is IProtoWithUpgrade protoWithUpgrade1) || !(second is IProtoWithUpgrade protoWithUpgrade2))
        return false;
      return protoWithUpgrade1.CanUpgradeTo(protoWithUpgrade2) || protoWithUpgrade2.CanUpgradeTo(protoWithUpgrade1);
    }

    public static bool CanUpgradeTo(this IProtoWithUpgrade current, IProtoWithUpgrade protoToCheck)
    {
      for (IProtoWithUpgrade valueOrNull = current.UpgradeNonGeneric.NextTierNonGeneric.ValueOrNull; valueOrNull != null; valueOrNull = valueOrNull.UpgradeNonGeneric.NextTierNonGeneric.ValueOrNull)
      {
        if (valueOrNull == protoToCheck)
          return true;
      }
      return false;
    }
  }
}
