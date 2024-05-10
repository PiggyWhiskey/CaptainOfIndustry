// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.ShipEntityMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Environment;
using Mafi.Core.World;
using Mafi.Unity.Audio;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ShipEntityMbFactory : 
    IEntityMbFactory<TravelingFleet>,
    IFactory<TravelingFleet, EntityMb>
  {
    private readonly EntityAudioManager m_audioManager;
    private readonly AssetsDb m_assetsDb;
    private readonly WeatherManager m_weatherManager;
    private readonly RandomProvider m_randomProvider;

    public ShipEntityMbFactory(
      AssetsDb assetsDb,
      EntityAudioManager audioManager,
      WeatherManager weatherManager,
      RandomProvider randomProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_audioManager = audioManager;
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
      this.m_weatherManager = weatherManager.CheckNotNull<WeatherManager>();
      this.m_randomProvider = randomProvider.CheckNotNull<RandomProvider>();
    }

    public EntityMb Create(TravelingFleet fleet)
    {
      ShipEntityMb shipEntityMb = new GameObject(fleet.Prototype.Id.Value).AddComponent<ShipEntityMb>();
      shipEntityMb.Initialize(fleet, this.m_assetsDb, this.m_audioManager, this.m_weatherManager, this.m_randomProvider.GetNonSimRandomFor((object) this, fleet.Prototype.Id.ToString()));
      return (EntityMb) shipEntityMb;
    }
  }
}
