// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.LayoutEntityMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  /// <summary>
  /// Default MB factory for all entities of base type <see cref="T:Mafi.Core.Entities.Static.ILayoutEntity" />. This serves as a fall-back to all
  /// entities that do not have their own custom factories.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class LayoutEntityMbFactory : 
    IEntityMbFactory<ILayoutEntity>,
    IFactory<ILayoutEntity, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;

    public LayoutEntityMbFactory(ProtoModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
    }

    public EntityMb Create(ILayoutEntity entity)
    {
      Assert.That<ILayoutEntity>(entity).IsNotNull<ILayoutEntity>();
      StaticEntityMb staticEntityMb = this.m_modelFactory.CreateModelFor<LayoutEntityProto>(entity.Prototype).AddComponent<StaticEntityMb>();
      staticEntityMb.Initialize(entity);
      return (EntityMb) staticEntityMb;
    }
  }
}
