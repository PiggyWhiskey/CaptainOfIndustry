// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.AnimalFarmProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [DebuggerDisplay("AnimalFarmProto: {Id}")]
  public class AnimalFarmProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<AnimalFarmProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithAnimation
  {
    public readonly PartialProductQuantity FoodPerAnimalPerMonth;
    public readonly PartialProductQuantity? ProducedPerAnimalPerMonth;
    public readonly Fix32 CarcassMultiplier;
    public readonly ProductProto CarcassProto;
    public readonly char CarcassOutpotPortName;
    public readonly Fix32 AnimalsBornPer100AnimalsPerMonth;
    public readonly int AnimalsCapacity;
    public readonly PartialProductQuantity WaterPerAnimalPerMonth;
    /// <summary>
    /// Animals are represented via virtual products to have them in stats and be able
    /// to trade them.
    /// </summary>
    public readonly VirtualProductProto Animal;
    public readonly Quantity FoodBufferCapacity;
    public readonly Quantity WaterBufferCapacity;
    public readonly Quantity CarcassBufferCapacity;
    public readonly Quantity ProducedBufferCapacity;
    public readonly LayoutEntityProto.Gfx Graphics;

    public override Type EntityType => typeof (AnimalFarm);

    public UpgradeData<AnimalFarmProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams { get; }

    public AnimalFarmProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Option<AnimalFarmProto> nextTier,
      VirtualProductProto animal,
      int animalsCapacity,
      ProductProto carcassProto,
      char carcassPortName,
      Fix32 animalsBornPer100AnimalsPerMonth,
      Fix32 carcassMultiplier,
      PartialProductQuantity foodPerAnimalPerMonth,
      PartialProductQuantity waterPerAnimalPerMonth,
      PartialProductQuantity? producedPerAnimalPerMonth,
      Quantity foodBufferCapacity,
      Quantity waterBufferCapacity,
      Quantity carcassBufferCapacity,
      Quantity producedBufferCapacity,
      ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> animationParams,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      AnimalFarmProto animalFarmProto = this;
      this.Animal = animal;
      this.AnimalsCapacity = animalsCapacity;
      this.CarcassProto = carcassProto;
      this.CarcassOutpotPortName = carcassPortName;
      this.AnimalsBornPer100AnimalsPerMonth = animalsBornPer100AnimalsPerMonth;
      this.CarcassMultiplier = carcassMultiplier;
      this.FoodPerAnimalPerMonth = foodPerAnimalPerMonth;
      this.WaterPerAnimalPerMonth = waterPerAnimalPerMonth;
      this.ProducedPerAnimalPerMonth = producedPerAnimalPerMonth;
      this.Upgrade = new UpgradeData<AnimalFarmProto>(this, nextTier);
      this.FoodBufferCapacity = foodBufferCapacity;
      this.WaterBufferCapacity = waterBufferCapacity;
      this.CarcassBufferCapacity = carcassBufferCapacity;
      this.ProducedBufferCapacity = producedBufferCapacity;
      this.AnimationParams = animationParams;
      this.Graphics = graphics;
      Mafi.Assert.That<bool>(this.Ports.Any((Func<IoPortTemplate, bool>) (x => (int) x.Name == (int) carcassPortName && x.Type == IoPortType.Output && x.Shape.AllowedProductType == animalFarmProto.CarcassProto.Type))).IsTrue();
    }
  }
}
