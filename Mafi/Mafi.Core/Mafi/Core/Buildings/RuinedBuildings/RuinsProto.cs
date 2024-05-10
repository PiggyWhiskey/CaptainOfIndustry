// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RuinedBuildings.RuinsProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.RuinedBuildings
{
  public class RuinsProto : LayoutEntityProto
  {
    public readonly Duration DurationPerProduct;
    public readonly AssetValue ProductsGiven;

    public RuinsProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      Duration durationPerProduct,
      AssetValue productsGiven,
      EntityLayout layout,
      EntityCosts costs,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics, cannotBeBuiltByPlayer: true);
      this.DurationPerProduct = durationPerProduct;
      this.ProductsGiven = productsGiven;
    }

    public override Type EntityType => typeof (Ruins);
  }
}
