// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Validators.IEntityAddRequest
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities.Validators
{
  /// <summary>
  /// Base interface for all requests under which an entity can be added to the world. These requests are validated
  /// using <see cref="T:Mafi.Core.Entities.Validators.IEntityAdditionValidator" />.
  /// </summary>
  public interface IEntityAddRequest
  {
    EntityAddReason ReasonToAdd { get; }
  }
}
