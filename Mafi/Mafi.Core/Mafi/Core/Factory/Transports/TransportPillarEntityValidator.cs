// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPillarEntityValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Validators;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public sealed class TransportPillarEntityValidator : 
    IEntityAdditionValidator<TransportPillarAddRequest>,
    IEntityAdditionValidator,
    IEntityRemovalValidator<TransportPillar>,
    IEntityRemovalValidator
  {
    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public EntityValidationResult CanAdd(TransportPillarAddRequest addRequest)
    {
      return EntityValidationResult.CreateError("Pillars cannot added.");
    }

    public EntityValidationResult CanRemove(TransportPillar entity, EntityRemoveReason reason)
    {
      return reason == EntityRemoveReason.Collapse ? EntityValidationResult.Success : EntityValidationResult.CreateError("Pillars cannot be removed.");
    }

    public TransportPillarEntityValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
