// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Beacons.BeaconProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Beacons
{
  [DebuggerDisplay("BeaconProto: {Id}")]
  public class BeaconProto : 
    LayoutEntityProto,
    IProtoWithPowerConsumption,
    IProtoWithUnityConsumption
  {
    public readonly int Tier;

    public override Type EntityType => typeof (Beacon);

    public Upoints UnityMonthlyCost { get; }

    public UpointsCategoryProto UpointsCategory { get; }

    public Electricity ElectricityConsumed { get; }

    public BeaconProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      Electricity electricityConsumed,
      EntityCosts costs,
      Upoints unityMonthlyCost,
      UpointsCategoryProto upointsCategory,
      int tier,
      LayoutEntityProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityProto.ID id1 = id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      EntityCosts costs1 = costs;
      LayoutEntityProto.Gfx graphics1 = graphics;
      IEnumerable<Tag> tags1 = tags;
      Duration? constructionDurationPerProduct = new Duration?();
      Upoints? boostCost = new Upoints?();
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, costs1, graphics1, constructionDurationPerProduct, boostCost, isUnique: true, tags: tags2);
      this.ElectricityConsumed = electricityConsumed;
      this.UnityMonthlyCost = unityMonthlyCost;
      this.UpointsCategory = upointsCategory;
      this.Tier = tier;
    }
  }
}
