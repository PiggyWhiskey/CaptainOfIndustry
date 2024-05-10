// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.TombOfCaptainsMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Buildings;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class TombOfCaptainsMbFactory : 
    IEntityMbFactory<TombOfCaptains>,
    IFactory<TombOfCaptains, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;

    public TombOfCaptainsMbFactory(ProtoModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
    }

    public EntityMb Create(TombOfCaptains pad)
    {
      TombOfCaptainsMb tombOfCaptainsMb = this.m_modelFactory.CreateModelFor<TombOfCaptainsProto>(pad.Prototype).AddComponent<TombOfCaptainsMb>();
      tombOfCaptainsMb.Initialize(pad);
      return (EntityMb) tombOfCaptainsMb;
    }
  }
}
