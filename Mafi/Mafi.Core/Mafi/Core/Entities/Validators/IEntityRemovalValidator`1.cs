// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Validators.IEntityRemovalValidator`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities.Validators
{
  /// <summary>
  /// Any class that implements this interface states that it wants to validate whether every entity assignable to <see cref="!:TEntity" /> can be removed from the world. Classes can implement multiple of these interfaces.
  /// </summary>
  /// <typeparam name="TEntity">Type of the entity supported by the validator.</typeparam>
  public interface IEntityRemovalValidator<TEntity> : IEntityRemovalValidator where TEntity : IEntity
  {
    EntityValidationResult CanRemove(TEntity entity, EntityRemoveReason reason);
  }
}
