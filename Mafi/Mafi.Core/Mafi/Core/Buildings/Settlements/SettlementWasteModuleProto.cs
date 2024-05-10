// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementWasteModuleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  public class SettlementWasteModuleProto : 
    StorageBaseProto,
    ISettlementModuleProto,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto,
    IProto,
    IProtoWithUiRecipe
  {
    public readonly ProductProto ProductAccepted;

    public override Type EntityType => typeof (SettlementWasteModule);

    public IRecipeForUi Recipe { get; }

    public SettlementWasteModuleProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ProductProto productAccepted,
      Quantity capacity,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, capacity, costs, graphics);
      this.ProductAccepted = productAccepted;
      this.Recipe = (IRecipeForUi) new RecipeForUiData(60.Seconds(), (ImmutableArray<RecipeInput>) ImmutableArray.Empty, ImmutableArray.Create<RecipeOutput>(new RecipeOutput(productAccepted, Quantity.Zero)));
    }
  }
}
