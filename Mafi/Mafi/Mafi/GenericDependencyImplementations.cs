// Decompiled with JetBrains decompiler
// Type: Mafi.GenericDependencyImplementations
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Reflection;

#nullable disable
namespace Mafi
{
  public class GenericDependencyImplementations
  {
    public readonly Type DependencyType;
    /// <summary>
    /// Simple lookup table from the first generic parameter of DependencyType that implementation of DependencyType
    /// has. If such a parameter is a generic parameter (IsGenericParameter == true), such implementation is in
    /// m_otherImplementations.
    /// </summary>
    private readonly Dict<Type, Lyst<GenericDependencyImplementations.GenericImplementation>> m_firstParamToImplementation;
    private readonly Lyst<GenericDependencyImplementations.GenericImplementation> m_otherImplementations;

    public GenericDependencyImplementations(Type dependecyType, Lyst<Type> implementationTypes)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_firstParamToImplementation = new Dict<Type, Lyst<GenericDependencyImplementations.GenericImplementation>>();
      this.m_otherImplementations = new Lyst<GenericDependencyImplementations.GenericImplementation>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(dependecyType.IsGenericTypeDefinition).IsTrue();
      this.DependencyType = dependecyType;
      if (dependecyType.IsValueType)
        this.initGenericStruct(implementationTypes);
      else if (dependecyType.IsClass)
      {
        this.initGenericClass(implementationTypes);
      }
      else
      {
        if (!dependecyType.IsInterface)
          throw new DependencyResolverException(string.Format("Bad dependentyType argument '{0}' - not class, interface or struct.", (object) dependecyType));
        this.initGenericInterface(implementationTypes);
      }
    }

    private Option<Type> getFirstGenericParamGenricDefinition(Type type)
    {
      Assert.That<Type>(this.DependencyType).IsEqualTo<Type>(type.GetGenericTypeDefinition());
      Type genericArgument = type.GetGenericArguments()[0];
      if (genericArgument.IsGenericType)
        return (Option<Type>) genericArgument.GetGenericTypeDefinition();
      if (genericArgument.IsArray)
        return (Option<Type>) typeof (Array);
      return genericArgument.IsGenericParameter ? (Option<Type>) Option.None : (Option<Type>) genericArgument;
    }

    private void addImplementation(Type implementationType, Type inheritedType)
    {
      Option<Type> genricDefinition = this.getFirstGenericParamGenricDefinition(inheritedType);
      GenericDependencyImplementations.GenericImplementation genericImplementation = new GenericDependencyImplementations.GenericImplementation(implementationType, inheritedType);
      if (genricDefinition.HasValue)
      {
        Lyst<GenericDependencyImplementations.GenericImplementation> lyst;
        if (!this.m_firstParamToImplementation.TryGetValue(genricDefinition.Value, out lyst))
        {
          lyst = new Lyst<GenericDependencyImplementations.GenericImplementation>();
          this.m_firstParamToImplementation.Add(genricDefinition.Value, lyst);
        }
        lyst.Add(genericImplementation);
      }
      else
        this.m_otherImplementations.Add(genericImplementation);
    }

    private void initGenericStruct(Lyst<Type> implementationTypes)
    {
      Assert.That<Lyst<Type>>(implementationTypes).HasLength<Type>(1);
      Assert.That<Type>(this.DependencyType).IsEqualTo<Type>(implementationTypes[0]);
      this.addImplementation(implementationTypes[0], this.DependencyType);
    }

    private void initGenericClass(Lyst<Type> implementationTypes)
    {
      Lyst<Type>.Enumerator enumerator = implementationTypes.GetEnumerator();
label_6:
      while (enumerator.MoveNext())
      {
        Type current = enumerator.Current;
        Type inheritedType = current;
        while (true)
        {
          if (inheritedType.IsGenericType && inheritedType.ContainsGenericParameters)
          {
            if (inheritedType.GetGenericTypeDefinition() == this.DependencyType)
              this.addImplementation(current, inheritedType);
            inheritedType = inheritedType.BaseType;
          }
          else
            goto label_6;
        }
      }
    }

    private void initGenericInterface(Lyst<Type> implementationTypes)
    {
      foreach (Type implementationType in implementationTypes)
      {
        foreach (Type inheritedType in implementationType.GetInterfaces())
        {
          if (inheritedType.IsGenericType && !(inheritedType.GetGenericTypeDefinition() != this.DependencyType))
            this.addImplementation(implementationType, inheritedType);
        }
      }
    }

    public bool TryMakeGenericType(Type targetType, out Type genericType)
    {
      Assert.That<Type>(this.DependencyType).IsEqualTo<Type>(targetType.GetGenericTypeDefinition());
      Option<Type> genricDefinition = this.getFirstGenericParamGenricDefinition(targetType);
      Lyst<GenericDependencyImplementations.GenericImplementation> lyst;
      if (genricDefinition.HasValue && this.m_firstParamToImplementation.TryGetValue(genricDefinition.Value, out lyst))
      {
        foreach (GenericDependencyImplementations.GenericImplementation genericImplementation in lyst)
        {
          if (this.tryMakeGenericType(targetType, genericImplementation, out genericType))
            return true;
        }
      }
      foreach (GenericDependencyImplementations.GenericImplementation otherImplementation in this.m_otherImplementations)
      {
        if (this.tryMakeGenericType(targetType, otherImplementation, out genericType))
          return true;
      }
      genericType = (Type) null;
      return false;
    }

    private bool tryMakeGenericType(
      Type targetType,
      GenericDependencyImplementations.GenericImplementation genericImplementation,
      out Type genericType)
    {
      Assert.That<Type>(this.DependencyType).IsEqualTo<Type>(targetType.GetGenericTypeDefinition());
      Type[] typeArray = ArrayPool<Type>.Get(genericImplementation.ImplementationType.GetGenericArguments().Length);
      genericType = (Type) null;
      try
      {
        if (!this.matchGenericArgumentsOf(targetType, genericImplementation.InheritedType, typeArray))
          return false;
        if (Array.IndexOf<Type>(typeArray, (Type) null) >= 0)
        {
          Log.Error(string.Format("Cannot instantiate {0} using generic {1}", (object) targetType, (object) genericImplementation.ImplementationType) + string.Format("{0} has too many unspecified generic arguments.", (object) genericImplementation.ImplementationType));
          return false;
        }
        if (!this.argumentsMatchConstraints(genericImplementation.ImplementationType, typeArray))
          return false;
        genericType = genericImplementation.ImplementationType.MakeGenericType(typeArray).CheckNotNull<Type>();
        return true;
      }
      catch (ArgumentException ex)
      {
        Log.Warning("Consider extending type parameter checks in argumentsMatchConstraints." + string.Format("Not enough checks caused exception in MakeGenericType, exceptions are slow: {0}", (object) ex));
        return false;
      }
      finally
      {
        ArrayPool<Type>.ReturnToPool(typeArray);
      }
    }

    private bool matchGenericArgumentsOf(
      Type targetType,
      Type matchingType,
      Type[] instanceTypeArguments)
    {
      Type[] genericArguments1 = targetType.GetGenericArguments();
      Type[] genericArguments2 = matchingType.GetGenericArguments();
      Assert.That<Type[]>(genericArguments1).HasLength<Type>(genericArguments2.Length);
      for (int index = 0; index < genericArguments1.Length; ++index)
      {
        Type targetArgument = genericArguments1[index];
        Type parameterArgument = genericArguments2[index];
        if (!parameterArgument.ContainsGenericParameters)
        {
          if (parameterArgument != targetArgument)
            return false;
        }
        else if (!this.matchGenericArgument(targetArgument, parameterArgument, instanceTypeArguments))
          return false;
      }
      return true;
    }

    private bool matchGenericArgument(
      Type targetArgument,
      Type parameterArgument,
      Type[] instanceTypeArguments)
    {
      if (parameterArgument.IsGenericParameter)
      {
        int parameterPosition = parameterArgument.GenericParameterPosition;
        if (instanceTypeArguments[parameterPosition] != (Type) null)
          return instanceTypeArguments[parameterPosition] == targetArgument;
        instanceTypeArguments[parameterPosition] = targetArgument;
        return true;
      }
      if (parameterArgument.IsArray)
        return targetArgument.IsArray && this.matchGenericArgument(targetArgument.GetElementType(), parameterArgument.GetElementType(), instanceTypeArguments);
      if (!parameterArgument.IsConstructedGenericType)
        return targetArgument == parameterArgument;
      return targetArgument.IsConstructedGenericType && !(parameterArgument.GetGenericTypeDefinition() != targetArgument.GetGenericTypeDefinition()) && this.matchGenericArgumentsOf(targetArgument, parameterArgument, instanceTypeArguments);
    }

    /// <summary>
    /// Tests whether type arguments match type argument constraints given by type Constraints checking is NOT
    /// exhausting! Using the type parameters is MakeGenericType thus may fail with an exception. For example:
    /// Variance, contravariance and default constructor constraints are not checked.
    /// </summary>
    private bool argumentsMatchConstraints(
      Type typeWithParameterConstraints,
      Type[] instanceTypeArguments)
    {
      Type[] genericArguments = typeWithParameterConstraints.GetGenericArguments();
      Assert.That<Type[]>(genericArguments).HasLength<Type>(instanceTypeArguments.Length);
      for (int index = 0; index < instanceTypeArguments.Length; ++index)
      {
        Type type = genericArguments[index];
        Type instanceTypeArgument = instanceTypeArguments[index];
        if (this.isRefenceFlagOn(type.GenericParameterAttributes) && !this.isReferneceType(instanceTypeArgument) || this.isNotNullableValueFlagOn(type.GenericParameterAttributes) && !this.isNotNullableValueType(instanceTypeArgument))
          return false;
        foreach (Type parameterConstraint in type.GetGenericParameterConstraints())
        {
          if (!this.matchesConstraint(instanceTypeArgument, parameterConstraint, instanceTypeArguments))
            return false;
        }
      }
      return true;
    }

    private bool isRefenceFlagOn(GenericParameterAttributes attributes)
    {
      return (attributes & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint;
    }

    private bool isNotNullableValueFlagOn(GenericParameterAttributes attributes)
    {
      return (attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint;
    }

    private bool isReferneceType(Type t) => t.IsClass || t.IsInterface;

    private bool isNotNullableValueType(Type t)
    {
      if (!t.IsValueType)
        return false;
      return !t.IsGenericType || t.GetGenericTypeDefinition() != typeof (Nullable<>);
    }

    private bool matchesConstraint(Type type, Type constraint, Type[] instanceTypeArguments)
    {
      if (constraint.IsGenericParameter)
        constraint = instanceTypeArguments[constraint.GenericParameterPosition];
      if (!constraint.ContainsGenericParameters)
        return constraint.IsAssignableFrom(type);
      if (constraint.IsInterface)
      {
        foreach (Type targetArgument in type.GetInterfaces())
        {
          if (targetArgument.IsGenericType && this.matchGenericArgument(targetArgument, constraint, instanceTypeArguments))
            return true;
        }
        return false;
      }
      if (!constraint.IsClass)
        return true;
      for (; type.IsGenericType; type = type.BaseType)
      {
        if (this.matchGenericArgument(type, constraint, instanceTypeArguments))
          return true;
      }
      return false;
    }

    public struct GenericImplementation
    {
      /// <summary>
      /// Type that implements the dependency. It implements it through generic type instance ImplementedType.
      /// </summary>
      public readonly Type ImplementationType;
      /// <summary>
      /// The type that assignable to the dependency type and implementation type implements it.
      /// InheritedType.GetGenericTypeDefinition() == dependencyType
      /// </summary>
      public readonly Type InheritedType;

      public GenericImplementation(Type implementationType, Type inheritedType)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        Assert.That<bool>(implementationType.IsGenericTypeDefinition).IsTrue();
        Assert.That<bool>(inheritedType.ContainsGenericParameters).IsTrue();
        this.ImplementationType = implementationType;
        this.InheritedType = inheritedType;
      }
    }
  }
}
