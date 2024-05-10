// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.WellPumps.WellPumpProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.WellPumps
{
  public class WellPumpProto : MachineProto
  {
    public readonly string ReserveDescription;
    public readonly VirtualResourceProductProto MinedProduct;
    public Percent NotifyWhenBelow;

    public override Type EntityType => typeof (WellPump);

    public WellPumpProto(
      MachineProto.ID id,
      Proto.Str strings,
      string reserveDescription,
      EntityLayout layout,
      EntityCosts costs,
      Electricity consumedPowerPerTick,
      VirtualResourceProductProto minedProduct,
      Percent notifyWhenBelow,
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
      this.ReserveDescription = reserveDescription;
      this.MinedProduct = minedProduct.CheckNotNull<VirtualResourceProductProto>();
      this.NotifyWhenBelow = notifyWhenBelow;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      foreach (RecipeProto recipe in this.Recipes)
      {
        if (recipe.OutputsAtEnd.All((Func<RecipeOutput, bool>) (o => (Proto) o.Product != (Proto) this.MinedProduct.Product)))
          throw new ProtoBuilderException(string.Format("WellPumpProto recipe {0} does not have mined product {1} as output.", (object) recipe, (object) this.MinedProduct.Product));
      }
    }
  }
}
