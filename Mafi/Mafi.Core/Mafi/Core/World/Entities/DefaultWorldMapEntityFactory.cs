// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.DefaultWorldMapEntityFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.World.Entities
{
  /// <summary>
  /// Default factory for static entities that take `WorldMapEntityProto` (or derived) and WorldMapLocation as its first two
  /// arguments and all other arguments are resolved from the resolver.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class DefaultWorldMapEntityFactory : 
    IFactory<WorldMapEntityProto, WorldMapLocation, WorldMapEntity>
  {
    private readonly EntityId.Factory m_idFactory;
    private readonly DependencyResolver m_resolver;

    public DefaultWorldMapEntityFactory(EntityId.Factory idFactory, DependencyResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_idFactory = idFactory;
      this.m_resolver = resolver;
    }

    public WorldMapEntity Create(WorldMapEntityProto proto, WorldMapLocation location)
    {
      return (WorldMapEntity) this.m_resolver.Instantiate(proto.EntityType, (object) this.m_idFactory.GetNextId(), (object) proto, (object) location);
    }
  }
}
