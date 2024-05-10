// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.ICargoShipFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships
{
  /// <summary>
  /// Interface serving for <see cref="T:Mafi.Core.Buildings.Cargo.Ships.CargoShip" /> creation by <see cref="T:Mafi.Core.Buildings.Cargo.CargoDepot" />.
  /// </summary>
  public interface ICargoShipFactory
  {
    /// <summary>
    /// Creates a ship of a specified type heading to the specified cargo depot.
    /// </summary>
    CargoShip AddCargoShip(
      CargoDepot cargoDepot,
      CargoShipProto proto,
      Option<ProductProto> fuelProto);
  }
}
