// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.RocketTransporterMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Dynamic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class RocketTransporterMbFactory : 
    IEntityMbFactory<RocketTransporter>,
    IFactory<RocketTransporter, EntityMb>
  {
    private readonly DynamicGroundEntityModelFactory m_modelFactory;
    private readonly AssetsDb m_assetsDb;
    private readonly IWeatherManager m_weatherManager;
    private readonly EntityAudioManager m_audioManager;
    private readonly DynamicGroundEntityDeps m_dependencies;

    public RocketTransporterMbFactory(
      DynamicGroundEntityModelFactory modelFactory,
      AssetsDb assetsDb,
      IWeatherManager weatherManager,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_assetsDb = assetsDb;
      this.m_weatherManager = weatherManager;
      this.m_audioManager = audioManager;
      this.m_dependencies = dependencies;
    }

    public EntityMb Create(RocketTransporter entity)
    {
      RocketTransporterMb rocketTransporterMb = this.m_modelFactory.Create((DynamicGroundEntityProto) entity.Prototype).AddComponent<RocketTransporterMb>();
      rocketTransporterMb.Initialize(entity, this.m_assetsDb, this.m_weatherManager, this.m_audioManager, this.m_dependencies);
      return (EntityMb) rocketTransporterMb;
    }
  }
}
