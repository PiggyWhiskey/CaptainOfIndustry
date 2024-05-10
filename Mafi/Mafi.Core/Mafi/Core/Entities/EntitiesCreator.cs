// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntitiesCreator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.SpaceProgram;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Helper class that creates entities using resolver so that the user does not need to remember what
  /// parameters are passed to <see cref="M:Mafi.DependencyResolver.TryInvokeFactoryHierarchy``1(System.Object)" />.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntitiesCreator
  {
    private readonly DependencyResolver m_resolver;

    public EntitiesCreator(DependencyResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
    }

    public bool TryCreateStaticEntity(
      StaticEntityProto proto,
      TileTransform transform,
      out StaticEntity entity)
    {
      entity = this.m_resolver.TryInvokeFactoryHierarchy<StaticEntity>((object) proto, (object) transform).ValueOrNull;
      return entity != null;
    }

    public bool TryCreateVehicle(DynamicGroundEntityProto proto, out Vehicle vehicle)
    {
      vehicle = this.m_resolver.TryInvokeFactoryHierarchy<Vehicle>((object) proto).ValueOrNull;
      return vehicle != null;
    }

    public bool TryCreateRocket<TEntity>(EntityProto proto, out TEntity entity) where TEntity : RocketEntityBase
    {
      Option<Entity> option = this.m_resolver.TryInvokeFactoryHierarchy<Entity>((object) proto);
      if (option.IsNone)
      {
        Log.Error("Failed to create a rocket via factory.");
        entity = default (TEntity);
        return false;
      }
      entity = option.Value as TEntity;
      if ((object) entity != null)
        return true;
      Log.Error("Created entity '" + option.Value.GetType().Name + "' is not derived from 'TEntity'.");
      return false;
    }
  }
}
