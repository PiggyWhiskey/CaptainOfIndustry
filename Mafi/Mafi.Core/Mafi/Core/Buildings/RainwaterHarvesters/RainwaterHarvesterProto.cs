// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RainwaterHarvesters.RainwaterHarvesterProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Buildings.RainwaterHarvesters
{
  public class RainwaterHarvesterProto : StorageBaseProto
  {
    /// <summary>Proto to be collected from rain by this entity.</summary>
    public readonly FluidProductProto WaterProto;
    /// <summary>
    /// Amount of water collected every day when it is raining.
    /// </summary>
    public readonly PartialQuantity WaterCollectedPerDay;

    public override Type EntityType => typeof (RainwaterHarvester);

    public RainwaterHarvesterProto(
      StaticEntityProto.ID id,
      FluidProductProto waterProto,
      Quantity capacity,
      PartialQuantity waterCollectedPerDay,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, capacity, costs, graphics);
      this.WaterProto = waterProto.CheckNotNull<FluidProductProto>();
      this.WaterCollectedPerDay = waterCollectedPerDay;
    }
  }
}
