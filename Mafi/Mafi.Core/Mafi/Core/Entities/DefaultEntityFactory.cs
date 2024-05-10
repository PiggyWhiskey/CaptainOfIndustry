// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.DefaultEntityFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class DefaultEntityFactory : IFactory<EntityProto, Entity>
  {
    private readonly EntityId.Factory m_idFactory;
    private readonly DependencyResolver m_resolver;

    public DefaultEntityFactory(EntityId.Factory idFactory, DependencyResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_idFactory = idFactory;
      this.m_resolver = resolver;
    }

    public Entity Create(EntityProto proto)
    {
      return (Entity) this.m_resolver.Instantiate(proto.EntityType, (object) this.m_idFactory.GetNextId(), (object) proto);
    }
  }
}
