// Decompiled with JetBrains decompiler
// Type: Mafi.Base.StartingFactoryConfig
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.Products;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class StartingFactoryConfig : IStartingFactoryConfig, IConfig
  {
    public int InitialTrucks { get; private set; }

    public int InitialExcavators { get; private set; }

    public int InitialTreeHarvesters { get; private set; }

    public ProductProto.ID StartingFoodProto { get; private set; }

    public ImmutableArray<KeyValuePair<ProductProto.ID, Quantity>> InitialProducts { get; private set; }

    /// <summary>
    /// Extra products to compensate the player because they skipped tutorials and will miss on rewards.
    /// </summary>
    public ImmutableArray<KeyValuePair<ProductProto.ID, Quantity>> ExtraInitialProductsIfGoalsSkipped { get; private set; }

    public StartingFactoryConfig(GameDifficultyConfig gameDifficulty)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.InitialTrucks = 8;
      this.InitialExcavators = 0;
      this.InitialTreeHarvesters = 1;
      this.StartingFoodProto = Ids.Products.Potato;
      Percent scale = 100.Percent() + gameDifficulty.ExtraStartingMaterial;
      this.InitialProducts = ImmutableArray.Create<KeyValuePair<ProductProto.ID, Quantity>>(Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Rubber, new Quantity(160).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Copper, new Quantity(320).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Electronics, new Quantity(150).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.MechanicalParts, new Quantity(80).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.ConstructionParts, new Quantity(480).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.VehicleParts, new Quantity(120).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Diesel, new Quantity(440).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Coal, new Quantity(60).ScaledBy(scale)));
      this.ExtraInitialProductsIfGoalsSkipped = ImmutableArray.Create<KeyValuePair<ProductProto.ID, Quantity>>(Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Copper, new Quantity(50).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Electronics, new Quantity(100).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.ConstructionParts, new Quantity(200).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Diesel, new Quantity(80).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.VehicleParts, new Quantity(100).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Bricks, new Quantity(200).ScaledBy(scale)), Make.Kvp<ProductProto.ID, Quantity>(Ids.Products.Cement, new Quantity(170).ScaledBy(scale)));
    }
  }
}
