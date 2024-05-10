// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.TruckMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Dynamic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TruckMbFactory : IEntityMbFactory<Truck>, IFactory<Truck, EntityMb>
  {
    private readonly DynamicGroundEntityModelFactory m_modelFactory;
    private readonly TruckAttachmentFactory m_attachmentFactory;
    private readonly AssetsDb m_assetsDb;
    private readonly IWeatherManager m_weatherManager;
    private readonly EntityAudioManager m_audioManager;
    private readonly DynamicGroundEntityDeps m_dependencies;
    private readonly RandomProvider m_randomProvider;

    public TruckMbFactory(
      DynamicGroundEntityModelFactory modelFactory,
      TruckAttachmentFactory attachmentFactory,
      AssetsDb assetsDb,
      IWeatherManager weatherManager,
      RandomProvider randomProvider,
      EntityAudioManager audioManager,
      DynamicGroundEntityDeps dependencies)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_attachmentFactory = attachmentFactory;
      this.m_assetsDb = assetsDb;
      this.m_weatherManager = weatherManager;
      this.m_audioManager = audioManager;
      this.m_dependencies = dependencies;
      this.m_randomProvider = randomProvider;
    }

    public EntityMb Create(Truck entity)
    {
      TruckMb truckMb = this.m_modelFactory.Create((DynamicGroundEntityProto) entity.Prototype).AddComponent<TruckMb>();
      truckMb.Initialize(entity, this.m_attachmentFactory, this.m_assetsDb, this.m_weatherManager, this.m_randomProvider, this.m_audioManager, this.m_dependencies);
      return (EntityMb) truckMb;
    }
  }
}
