// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.RuinsMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.RuinedBuildings;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class RuinsMbFactory : IEntityMbFactory<Ruins>, IFactory<Ruins, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;

    public RuinsMbFactory(ProtoModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
    }

    public EntityMb Create(Ruins ruins)
    {
      RuinsMb ruinsMb = this.m_modelFactory.CreateModelFor<RuinsProto>(ruins.Prototype).AddComponent<RuinsMb>();
      ruinsMb.Initialize(ruins);
      return (EntityMb) ruinsMb;
    }
  }
}
