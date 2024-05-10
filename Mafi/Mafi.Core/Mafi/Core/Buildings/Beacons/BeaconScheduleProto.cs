// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Beacons.BeaconScheduleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Beacons
{
  [DebuggerDisplay("BeaconScheduleProto: {Id}")]
  public class BeaconScheduleProto : Proto
  {
    public readonly Func<int, Option<RefugeesReward>> OffersGenerator;

    public BeaconScheduleProto(Proto.ID id, Func<int, Option<RefugeesReward>> offersGenerator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.OffersGenerator = offersGenerator;
    }
  }
}
