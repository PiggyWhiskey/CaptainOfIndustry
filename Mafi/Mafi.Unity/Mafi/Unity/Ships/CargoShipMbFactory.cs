// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ships.CargoShipMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Environment;
using Mafi.Core.GameLoop;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.Ships.Modules;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ships
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CargoShipMbFactory : IEntityMbFactory<CargoShip>, IFactory<CargoShip, EntityMb>
  {
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly EntityAudioManager m_audioManager;
    private readonly AssetsDb m_assetsDb;
    private readonly CargoShipModuleMbFactory m_moduleMbFactory;
    private readonly WeatherManager m_weatherManager;
    private readonly IRandom m_rng;

    public CargoShipMbFactory(
      IGameLoopEvents gameLoopEvents,
      AssetsDb assetsDb,
      CargoShipModuleMbFactory moduleMbFactory,
      EntityAudioManager audioManager,
      WeatherManager weatherManager,
      RandomProvider randomProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_audioManager = audioManager;
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
      this.m_moduleMbFactory = moduleMbFactory.CheckNotNull<CargoShipModuleMbFactory>();
      this.m_weatherManager = weatherManager.CheckNotNull<WeatherManager>();
      this.m_rng = randomProvider.GetNonSimRandomFor((object) this, "CargoShip");
    }

    public EntityMb Create(CargoShip entity)
    {
      CargoShipMb cargoShipMb = new GameObject().AddComponent<CargoShipMb>();
      cargoShipMb.Initialize(entity, this.m_gameLoopEvents, this.m_assetsDb, this.m_moduleMbFactory, this.m_audioManager, this.m_weatherManager, this.m_rng.NextAngle().ToUnityAngleDegrees());
      return (EntityMb) cargoShipMb;
    }
  }
}
