// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.Modules.CargoShipLooseModuleProto
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
  public class CargoShipLooseModuleProto : CargoShipModuleProto
  {
    public readonly CargoShipLooseModuleProto.Gfx Graphics;

    public CargoShipLooseModuleProto(
      Proto.ID id,
      Proto.Str strings,
      Quantity capacity,
      CargoShipLooseModuleProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, LooseProductProto.ProductType, capacity, (CargoShipModuleProto.Gfx) graphics);
      this.Graphics = graphics;
    }

    public new class Gfx : CargoShipModuleProto.Gfx
    {
      /// <summary>
      /// Path to the sub-object which represents smooth pile model.
      /// </summary>
      public readonly string SmoothPilePath;
      /// <summary>
      /// Path to the sub-object which represents rough pile model.
      /// </summary>
      public readonly string RoughPilePath;
      /// <summary>
      /// Offset (local position) within parent when cargo is empty. Offset is linearly interpolated between
      /// empty and full states based on cargo amount. This value is in Unity coordinate space (meters).
      /// </summary>
      public readonly Vector3f OffsetEmpty;
      /// <summary>
      /// Offset (local position) within parent when cargo is full. Offset is linearly interpolated between empty
      /// and full states based on cargo amount. This value is in Unity coordinate space (meters).
      /// </summary>
      public readonly Vector3f OffsetFull;

      public Gfx(
        string prefabPath,
        string smoothPilePath,
        string roughPilePath,
        Vector3f offsetEmpty,
        Vector3f offsetFull)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath);
        this.SmoothPilePath = smoothPilePath;
        this.RoughPilePath = roughPilePath;
        this.OffsetEmpty = offsetEmpty;
        this.OffsetFull = offsetFull;
      }
    }
  }
}
