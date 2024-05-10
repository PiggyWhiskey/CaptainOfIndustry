// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.CargoDepotMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Cargo;
using Mafi.Core.GameLoop;
using Mafi.Unity.Entities;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CargoDepotMbFactory : IEntityMbFactory<CargoDepot>, IFactory<CargoDepot, EntityMb>
  {
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ProtoModelFactory m_modelFactory;
    private readonly AssetsDb m_assetsDb;

    public CargoDepotMbFactory(
      IGameLoopEvents gameLoopEvents,
      ProtoModelFactory modelFactory,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_modelFactory = modelFactory;
      this.m_assetsDb = assetsDb;
    }

    public EntityMb Create(CargoDepot cargoDepot)
    {
      Assert.That<CargoDepot>(cargoDepot).IsNotNull<CargoDepot>();
      CargoDepotMb cargoDepotMb = this.m_modelFactory.CreateModelFor<CargoDepotProto>(cargoDepot.Prototype).AddComponent<CargoDepotMb>();
      cargoDepotMb.Initialize(cargoDepot, this.m_gameLoopEvents, this.m_assetsDb);
      return (EntityMb) cargoDepotMb;
    }
  }
}
