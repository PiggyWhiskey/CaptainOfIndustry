// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.DataCenterMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Datacenters;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class DataCenterMbFactory : IEntityMbFactory<DataCenter>, IFactory<DataCenter, EntityMb>
  {
    private readonly LayoutEntityModelFactory m_modelFactory;
    private readonly AssetsDb m_assetsDb;
    private readonly IRandom m_random;

    public DataCenterMbFactory(
      LayoutEntityModelFactory modelFactory,
      RandomProvider randomProvider,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_assetsDb = assetsDb;
      this.m_random = randomProvider.GetNonSimRandomFor((object) this);
    }

    public EntityMb Create(DataCenter dataCenter)
    {
      DataCenterMb dataCenterMb = this.m_modelFactory.Create((ILayoutEntityProto) dataCenter.Prototype).AddComponent<DataCenterMb>();
      dataCenterMb.Initialize(dataCenter, this.m_assetsDb, this.m_random);
      return (EntityMb) dataCenterMb;
    }
  }
}
