// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.ThermalStorageMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings.ThermalStorages;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class ThermalStorageMbFactory : 
    IEntityMbFactory<ThermalStorage>,
    IFactory<ThermalStorage, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;

    public ThermalStorageMbFactory(ProtoModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
    }

    public EntityMb Create(ThermalStorage storage)
    {
      ThermalStorageMb thermalStorageMb = this.m_modelFactory.CreateModelFor<ThermalStorageProto>(storage.Prototype).AddComponent<ThermalStorageMb>();
      thermalStorageMb.Initialize(storage);
      return (EntityMb) thermalStorageMb;
    }
  }
}
