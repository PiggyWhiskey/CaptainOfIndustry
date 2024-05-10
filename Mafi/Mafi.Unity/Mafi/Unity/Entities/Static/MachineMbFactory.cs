// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.MachineMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class MachineMbFactory : IEntityMbFactory<Machine>, IFactory<Machine, EntityMb>
  {
    private readonly AssetsDb m_assetsDb;
    private readonly ProtoModelFactory m_modelFactory;

    public MachineMbFactory(AssetsDb assetsDb, ProtoModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_modelFactory = modelFactory;
    }

    public EntityMb Create(Machine entity)
    {
      Assert.That<Machine>(entity).IsNotNull<Machine>();
      GameObject modelFor = this.m_modelFactory.CreateModelFor<MachineProto>(entity.Prototype);
      if (entity.Prototype.Graphics.HasSign)
      {
        MachineWithSignMb machineWithSignMb = modelFor.AddComponent<MachineWithSignMb>();
        machineWithSignMb.Initialize(this.m_assetsDb, entity);
        return (EntityMb) machineWithSignMb;
      }
      StaticEntityMb staticEntityMb = modelFor.AddComponent<StaticEntityMb>();
      staticEntityMb.Initialize((ILayoutEntity) entity);
      return (EntityMb) staticEntityMb;
    }
  }
}
