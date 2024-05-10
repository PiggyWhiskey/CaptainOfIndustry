// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntityValidators
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Validators;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Helper class that collects all entity validators as lazy dependencies and allows easy access to them.
  /// </summary>
  /// <remarks>
  /// Having all validators as lazy dependencies breaks common dependency loop when a validator also wants to subscribe
  /// to Entity Manager's entity added/removed events.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntityValidators
  {
    private readonly LazyResolve<AllImplementationsOf<IEntityAdditionValidator>> m_entityAddValidatorsImpls;
    private readonly LazyResolve<AllImplementationsOf<IEntityRemovalValidator>> m_entityRemoveValidatorsImpls;
    private readonly Mafi.Lazy<ImmutableArray<EntityValidators.IAddValidatorWrapper>> m_addValidators;
    private readonly Mafi.Lazy<ImmutableArray<EntityValidators.IRemoveValidatorWrapper>> m_removeValidators;
    private readonly Dict<Type, ImmutableArray<EntityValidators.IAddValidatorWrapper>> m_addValidatorsMap;
    private readonly Dict<Type, ImmutableArray<EntityValidators.IRemoveValidatorWrapper>> m_removeValidatorsMap;

    public EntityValidators(
      LazyResolve<AllImplementationsOf<IEntityAdditionValidator>> entityAddValidators,
      LazyResolve<AllImplementationsOf<IEntityRemovalValidator>> entityRemoveValidators)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_addValidatorsMap = new Dict<Type, ImmutableArray<EntityValidators.IAddValidatorWrapper>>();
      this.m_removeValidatorsMap = new Dict<Type, ImmutableArray<EntityValidators.IRemoveValidatorWrapper>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entityAddValidatorsImpls = entityAddValidators;
      this.m_entityRemoveValidatorsImpls = entityRemoveValidators;
      this.m_addValidators = new Mafi.Lazy<ImmutableArray<EntityValidators.IAddValidatorWrapper>>(new Func<ImmutableArray<EntityValidators.IAddValidatorWrapper>>(this.computeAddValidators));
      this.m_removeValidators = new Mafi.Lazy<ImmutableArray<EntityValidators.IRemoveValidatorWrapper>>(new Func<ImmutableArray<EntityValidators.IRemoveValidatorWrapper>>(this.computeRemoveValidators));
    }

    internal static EntityValidators CreateEmpty()
    {
      return new EntityValidators((LazyResolve<AllImplementationsOf<IEntityAdditionValidator>>) AllImplementationsOf<IEntityAdditionValidator>.Empty, (LazyResolve<AllImplementationsOf<IEntityRemovalValidator>>) AllImplementationsOf<IEntityRemovalValidator>.Empty);
    }

    private ImmutableArray<EntityValidators.IAddValidatorWrapper> computeAddValidators()
    {
      Lyst<EntityValidators.IAddValidatorWrapper> lyst = new Lyst<EntityValidators.IAddValidatorWrapper>();
      foreach (IEntityAdditionValidator additionValidator in this.m_entityAddValidatorsImpls.Value.Implementations.OrderBy<EntityValidatorPriority>((Func<IEntityAdditionValidator, EntityValidatorPriority>) (x => x.Priority)))
      {
        foreach (Type type1 in additionValidator.GetType().GetInterfaces())
        {
          if (type1.IsGenericType && !(type1.GetGenericTypeDefinition() != typeof (IEntityAdditionValidator<>)))
          {
            Type type2 = typeof (EntityValidators.AddValidatorWrapper<>).MakeGenericType(type1.GetGenericArguments()[0]);
            lyst.Add((EntityValidators.IAddValidatorWrapper) Activator.CreateInstance(type2, (object) additionValidator));
          }
        }
      }
      return lyst.ToImmutableArray();
    }

    private ImmutableArray<EntityValidators.IRemoveValidatorWrapper> computeRemoveValidators()
    {
      Lyst<EntityValidators.IRemoveValidatorWrapper> lyst = new Lyst<EntityValidators.IRemoveValidatorWrapper>();
      foreach (IEntityRemovalValidator removalValidator in this.m_entityRemoveValidatorsImpls.Value.Implementations.OrderBy<EntityValidatorPriority>((Func<IEntityRemovalValidator, EntityValidatorPriority>) (x => x.Priority)))
      {
        foreach (Type type1 in removalValidator.GetType().GetInterfaces())
        {
          if (type1.IsGenericType && !(type1.GetGenericTypeDefinition() != typeof (IEntityRemovalValidator<>)))
          {
            Type type2 = typeof (EntityValidators.RemoveValidatorWrapper<>).MakeGenericType(type1.GetGenericArguments()[0]);
            lyst.Add((EntityValidators.IRemoveValidatorWrapper) Activator.CreateInstance(type2, (object) removalValidator));
          }
        }
      }
      return lyst.ToImmutableArray();
    }

    public EntityValidationResult CallAllAddValidatorsFor(
      IEntityAddRequest entityAddRequest,
      bool forceRunAllValidators = false,
      Lyst<IEntityPreAddValidator> preAddValidators = null)
    {
      EntityValidationResult validationResult1 = EntityValidationResult.Success;
      foreach (EntityValidators.IAddValidatorWrapper allAddValidator in this.getAllAddValidators(entityAddRequest.GetType()))
      {
        EntityValidationResult validationResult2 = allAddValidator.CanAdd((object) entityAddRequest);
        if (validationResult2.IsSuccess)
        {
          IEntityPreAddValidator result;
          if (preAddValidators != null && allAddValidator.TryGetValidatorAs<IEntityPreAddValidator>(out result))
            preAddValidators.Add(result);
        }
        else
        {
          if (!forceRunAllValidators || validationResult2.ValidationStatus == EntityValidationResultStatus.FatalError)
            return validationResult2;
          if (validationResult1.IsSuccess)
            validationResult1 = validationResult2;
        }
      }
      return validationResult1;
    }

    private ImmutableArray<EntityValidators.IAddValidatorWrapper> getAllAddValidators(
      Type addRequestType)
    {
      ImmutableArray<EntityValidators.IAddValidatorWrapper> immutableArray;
      if (!this.m_addValidatorsMap.TryGetValue(addRequestType, out immutableArray))
      {
        immutableArray = this.m_addValidators.Value.Where((Func<EntityValidators.IAddValidatorWrapper, bool>) (x => x.Supports(addRequestType))).ToImmutableArray<EntityValidators.IAddValidatorWrapper>();
        this.m_addValidatorsMap[addRequestType] = immutableArray;
      }
      return immutableArray;
    }

    public EntityValidationResult CallRemoveValidatorsFor(IEntity entity, EntityRemoveReason reason)
    {
      foreach (EntityValidators.IRemoveValidatorWrapper allRemoveValidator in this.getAllRemoveValidators(entity.GetType()))
      {
        EntityValidationResult validationResult = allRemoveValidator.CanRemove((object) entity, reason);
        if (validationResult.IsError)
          return validationResult;
      }
      return EntityValidationResult.Success;
    }

    private ImmutableArray<EntityValidators.IRemoveValidatorWrapper> getAllRemoveValidators(
      Type entityType)
    {
      ImmutableArray<EntityValidators.IRemoveValidatorWrapper> immutableArray;
      if (!this.m_removeValidatorsMap.TryGetValue(entityType, out immutableArray))
      {
        immutableArray = this.m_removeValidators.Value.Where((Func<EntityValidators.IRemoveValidatorWrapper, bool>) (x => x.Supports(entityType))).ToImmutableArray<EntityValidators.IRemoveValidatorWrapper>();
        this.m_removeValidatorsMap[entityType] = immutableArray;
      }
      return immutableArray;
    }

    private interface IAddValidatorWrapper
    {
      EntityValidationResult CanAdd(object arg);

      bool Supports(Type type);

      bool TryGetValidatorAs<T>(out T result);
    }

    private class AddValidatorWrapper<T> : EntityValidators.IAddValidatorWrapper where T : IEntityAddRequest
    {
      private readonly IEntityAdditionValidator<T> m_validator;

      public AddValidatorWrapper(IEntityAdditionValidator<T> validator)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_validator = validator;
      }

      public EntityValidationResult CanAdd(object arg) => this.m_validator.CanAdd((T) arg);

      public bool Supports(Type type) => type.IsAssignableTo<T>();

      public bool TryGetValidatorAs<T1>(out T1 result)
      {
        if (this.m_validator is T1 validator)
        {
          result = validator;
          return true;
        }
        result = default (T1);
        return false;
      }
    }

    private interface IRemoveValidatorWrapper
    {
      EntityValidationResult CanRemove(object arg, EntityRemoveReason reason);

      bool Supports(Type type);
    }

    private class RemoveValidatorWrapper<T> : EntityValidators.IRemoveValidatorWrapper where T : IEntity
    {
      private readonly IEntityRemovalValidator<T> m_validator;

      public RemoveValidatorWrapper(IEntityRemovalValidator<T> validator)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_validator = validator;
      }

      public EntityValidationResult CanRemove(object arg, EntityRemoveReason reason)
      {
        return this.m_validator.CanRemove((T) arg, reason);
      }

      public bool Supports(Type type) => type.IsAssignableTo<T>();
    }
  }
}
