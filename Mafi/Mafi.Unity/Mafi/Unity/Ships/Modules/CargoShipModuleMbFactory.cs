// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ships.Modules.CargoShipModuleMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ships.Modules
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class CargoShipModuleMbFactory
  {
    private readonly AssetsDb m_assetsDb;
    private readonly DependencyResolver m_resolver;

    public CargoShipModuleMbFactory(AssetsDb assetsDb, DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_resolver = resolver;
    }

    public CargoShipModuleBaseMb CreateModuleMb(CargoShip cargoShip, Option<CargoShipModule> module)
    {
      return module.IsNone ? (CargoShipModuleBaseMb) this.m_assetsDb.GetClonedPrefabOrEmptyGo(cargoShip.Prototype.Graphics.EmptyModulePrefabPath).AddComponent<CargoShipModuleGenericMb>() : this.m_resolver.InvokeFactoryHierarchy<CargoShipModuleBaseMb>((object) module.Value, (object) module.Value.Prototype);
    }
  }
}
