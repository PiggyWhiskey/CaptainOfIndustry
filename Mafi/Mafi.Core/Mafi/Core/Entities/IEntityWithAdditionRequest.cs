// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IEntityWithAdditionRequest
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Validators;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Entity which has an addition request that should be verified before the entity is added.
  /// TODO: Kill this and use only proto-based validation.
  /// </summary>
  public interface IEntityWithAdditionRequest : IEntity, IIsSafeAsHashKey
  {
    /// <summary>
    /// Gets request under which the current entity was added to the world.
    /// </summary>
    IEntityAddRequest GetAddRequest(EntityAddReason reasonToAdd);
  }
}
