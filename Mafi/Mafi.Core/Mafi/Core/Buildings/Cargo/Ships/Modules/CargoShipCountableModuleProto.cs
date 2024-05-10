// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.Modules.CargoShipCountableModuleProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships.Modules
{
  public class CargoShipCountableModuleProto : CargoShipModuleProto
  {
    public readonly CargoShipCountableModuleProto.Gfx Graphics;

    public CargoShipCountableModuleProto(
      Proto.ID id,
      Proto.Str strings,
      Quantity capacity,
      CargoShipCountableModuleProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, CountableProductProto.ProductType, capacity, (CargoShipModuleProto.Gfx) graphics);
      this.Graphics = graphics;
    }

    public new class Gfx : CargoShipModuleProto.Gfx
    {
      public readonly string ContainerPrefabPath;
      /// <summary>
      /// Path to the sub-object of the module's game object which will be the parent of the container game objects.
      /// </summary>
      public readonly string ContainersParentGoPath;
      /// <summary>
      /// Local positions of containers when added to the module.
      /// </summary>
      public readonly ImmutableArray<Vector3f> ContainerPositions;

      public Gfx(
        string prefabPath,
        string containerPrefabPath,
        string containersParentGoPath,
        ImmutableArray<Vector3f> containerPositions)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath);
        this.ContainerPrefabPath = containerPrefabPath;
        this.ContainersParentGoPath = containersParentGoPath;
        this.ContainerPositions = containerPositions;
      }
    }
  }
}
