// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Localization;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities
{
  public interface IEntity : IIsSafeAsHashKey
  {
    LocStrFormatted DefaultTitle { get; }

    /// <summary>Unique ID in this entity.</summary>
    EntityId Id { get; }

    EntityProto Prototype { get; }

    EntityContext Context { get; }

    /// <summary>
    /// Whether entity is enabled. Not enabled entity should not accept any inputs and should not do any work. This
    /// may be caused by construction state or player's pause.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Call to force the entity to refresh its enabled status.
    /// </summary>
    void UpdateIsEnabled();

    /// <summary>
    /// Call to force entity to refresh its broken status.
    /// Used by maintenance.
    /// </summary>
    void UpdateIsBroken();

    /// <summary>
    /// Call to force entity to update its properties.
    /// This updates only the properties that the entity is not directly subscribed to.
    /// </summary>
    void UpdateProperties();

    /// <summary>
    /// Whether entity was paused by player. This halts any operation and construction.
    /// </summary>
    bool IsPaused { get; }

    /// <summary>
    /// Whether it makes sense to pause this entity. If false, pause option should not be offered in the UI.
    /// </summary>
    bool CanBePaused { get; }

    bool IsDestroyed { get; }

    void SetPaused(bool isPaused);

    void AddObserver(IEntityObserver observer);

    void RemoveObserver(IEntityObserver observer);
  }
}
