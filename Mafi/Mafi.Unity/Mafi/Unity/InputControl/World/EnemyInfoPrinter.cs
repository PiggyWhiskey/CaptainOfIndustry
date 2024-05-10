// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.EnemyInfoPrinter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Fleet;
using System;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  public static class EnemyInfoPrinter
  {
    public static string GetEnemyInfo(BattleFleet fleet)
    {
      int num = fleet.Entities.Sum<FleetEntity>((Func<FleetEntity, int>) (x => x.GetBattleScore()));
      return Tr.Location_EnemyScore.Format(fleet.Entities.Count).Value + string.Format(" {0}", (object) num);
    }
  }
}
