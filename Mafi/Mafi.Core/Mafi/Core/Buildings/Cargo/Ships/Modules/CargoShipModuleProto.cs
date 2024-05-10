// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.Modules.CargoShipModuleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships.Modules
{
  public class CargoShipModuleProto : Proto
  {
    public readonly CargoShipModuleProto.Gfx Graphics;
    public ProductType ProductType;
    public Quantity Capacity;

    public CargoShipModuleProto(
      Proto.ID id,
      Proto.Str strings,
      ProductType productType,
      Quantity capacity,
      CargoShipModuleProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.ProductType = productType;
      this.Capacity = capacity;
      this.Graphics = graphics.CheckNotNull<CargoShipModuleProto.Gfx>();
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly CargoShipModuleProto.Gfx EMPTY;
      public readonly string PrefabPath;

      public Gfx(string prefabPath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.PrefabPath = prefabPath.CheckNotNull<string>();
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        CargoShipModuleProto.Gfx.EMPTY = new CargoShipModuleProto.Gfx(nameof (EMPTY));
      }
    }
  }
}
