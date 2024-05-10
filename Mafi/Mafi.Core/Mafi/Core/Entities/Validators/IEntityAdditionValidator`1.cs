// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Validators.IEntityAdditionValidator`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities.Validators
{
  /// <summary>
  /// Any class that implements this interface states that it wants to validate whether every entity with a request
  /// assignable to <see cref="!:TRequest" /> can be added to the world. Classes can implement multiple of these
  /// interfaces.
  /// </summary>
  /// <typeparam name="TRequest">Type of the request supported by the validator.</typeparam>
  public interface IEntityAdditionValidator<TRequest> : IEntityAdditionValidator where TRequest : IEntityAddRequest
  {
    EntityValidationResult CanAdd(TRequest addRequest);
  }
}
