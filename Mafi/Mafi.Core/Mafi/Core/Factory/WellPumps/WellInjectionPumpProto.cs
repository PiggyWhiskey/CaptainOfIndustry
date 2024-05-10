// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellInjectionPumpProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  public class WellInjectionPumpProto : MachineProto
  {
    public readonly LooseProductProto TerrainProductRequired;

    public override Type EntityType => typeof (WellInjectionPump);

    public WellInjectionPumpProto(
      MachineProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity consumedPowerPerTick,
      LooseProductProto terrainProductRequired,
      int? buffersMultiplier,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      MachineProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MachineProto.ID id1 = id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      EntityCosts costs1 = costs;
      Electricity consumedPowerPerTick1 = consumedPowerPerTick;
      Computing zero = Computing.Zero;
      int? buffersMultiplier1 = buffersMultiplier;
      Option<MachineProto> none = (Option<MachineProto>) Option.None;
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams1 = animationParams;
      Upoints? nullable = new Upoints?(MachineProto.BOOST_COST);
      MachineProto.Gfx graphics1 = graphics;
      IEnumerable<Tag> tags1 = tags;
      int? emissionWhenRunning = new int?();
      Upoints? boostCost = nullable;
      IEnumerable<Tag> tags2 = tags1;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, costs1, consumedPowerPerTick1, zero, buffersMultiplier1, none, true, animationParams1, graphics1, emissionWhenRunning, boostCost: boostCost, tags: tags2);
      this.TerrainProductRequired = terrainProductRequired;
    }
  }
}
