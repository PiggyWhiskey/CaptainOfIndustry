// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.UniqueEntityValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class UniqueEntityValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly EntitiesManager m_entitiesManager;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public UniqueEntityValidator(EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      return !addRequest.Proto.IsUnique || addRequest.ReasonToAdd == EntityAddReason.Move || !this.m_entitiesManager.Entities.Any<IEntity>((Predicate<IEntity>) (x => x.GetType() == addRequest.Proto.EntityType)) ? EntityValidationResult.Success : EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__Unique, "Building already exists, existing one can only be upgraded. " + string.Format("Reason: {0}", (object) addRequest.ReasonToAdd));
    }
  }
}
