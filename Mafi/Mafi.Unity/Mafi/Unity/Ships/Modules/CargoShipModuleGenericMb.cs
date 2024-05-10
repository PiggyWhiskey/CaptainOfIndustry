// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ships.Modules.CargoShipModuleGenericMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Cargo.Ships.Modules;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ships.Modules
{
  internal class CargoShipModuleGenericMb : CargoShipModuleBaseMb
  {
    public CargoShipModuleGenericMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
    public class Factory : IFactory<CargoShipModule, CargoShipModuleProto, CargoShipModuleBaseMb>
    {
      private readonly AssetsDb m_assetsDb;

      public Factory(AssetsDb assetsDb)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_assetsDb = assetsDb;
      }

      public CargoShipModuleBaseMb Create(CargoShipModule module, CargoShipModuleProto proto)
      {
        return (CargoShipModuleBaseMb) this.m_assetsDb.GetClonedPrefabOrEmptyGo(module.Prototype.Graphics.PrefabPath).AddComponent<CargoShipModuleGenericMb>();
      }
    }
  }
}
