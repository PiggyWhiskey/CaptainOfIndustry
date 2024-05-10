// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.AvailableCargoShipData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Products;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct AvailableCargoShipData
  {
    public readonly Option<ProductProto> FuelProto;
    public readonly Quantity? FuelQuantity;

    [LoadCtor]
    public AvailableCargoShipData(Option<ProductProto> fuelProto, Quantity? fuelQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.FuelProto = fuelProto;
      this.FuelQuantity = fuelQuantity;
    }

    public AvailableCargoShipData(CargoShip existingShip)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.FuelProto = (Option<ProductProto>) existingShip.FuelProto;
      this.FuelQuantity = new Quantity?(existingShip.FuelBuffer.Quantity);
    }

    public static void Serialize(AvailableCargoShipData value, BlobWriter writer)
    {
      Option<ProductProto>.Serialize(value.FuelProto, writer);
      writer.WriteNullableStruct<Quantity>(value.FuelQuantity);
    }

    public static AvailableCargoShipData Deserialize(BlobReader reader)
    {
      return new AvailableCargoShipData(Option<ProductProto>.Deserialize(reader), reader.ReadNullableStruct<Quantity>());
    }
  }
}
