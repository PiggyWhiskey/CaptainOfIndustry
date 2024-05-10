// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.RocketAssemblyBuildingMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Entities.Static.Layout;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class RocketAssemblyBuildingMbFactory : 
    IEntityMbFactory<RocketAssemblyBuilding>,
    IFactory<RocketAssemblyBuilding, EntityMb>
  {
    private readonly LayoutEntityModelFactory m_modelFactory;

    public RocketAssemblyBuildingMbFactory(LayoutEntityModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
    }

    public EntityMb Create(RocketAssemblyBuilding building)
    {
      RocketAssemblyBuildingMb assemblyBuildingMb = this.m_modelFactory.Create((ILayoutEntityProto) building.Prototype).AddComponent<RocketAssemblyBuildingMb>();
      assemblyBuildingMb.Initialize(building);
      return (EntityMb) assemblyBuildingMb;
    }
  }
}
