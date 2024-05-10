// Decompiled with JetBrains decompiler
// Type: Mafi.DependencyResolverBuilder
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using jOgQY3RGtH5fd9qQao;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace Mafi
{
  public class DependencyResolverBuilder
  {
    /// <summary>
    /// Dependency registrations. Key is dependency type and value is instance type. Multiple types can be registered
    /// under one dependency type.
    /// </summary>
    private Dict<Type, Lyst<Type>> m_dependencyRegistrations;
    /// <summary>
    /// If more than one implementation is registered under a dependency type, this may specify which one to resolve.
    /// By default the last one is resolved.
    /// </summary>
    private Dict<Type, Type> m_dependencyResolvePreferences;
    /// <summary>
    /// Dependency registrations for generic type definitions. Key is dependency type and value is instance type.
    /// Multiple types can be registered under one dependency type. Both key and items in the value array are generic
    /// type definitions.
    /// </summary>
    private Dict<Type, Lyst<Type>> m_genericDependencyRegistrations;
    /// <summary>
    /// Already instantiated dependencies. Key is a type of dependency, usually some interface.
    /// </summary>
    private Dict<Type, object> m_instancesByRegisteredType;
    /// <summary>
    /// Already instantiated dependencies. Key is actual type of the dependency instance.
    /// </summary>
    private Dict<Type, object> m_instancesByRealType;
    /// <summary>
    /// A list of assemblies that global dependencies were already processed to prevent multi-registration caused by
    /// stupid programmers that try to define two mods in one assembly.
    /// </summary>
    private readonly Set<Assembly> m_registeredGlobalDeps;
    private Dict<Type, int> m_registrationOrder;
    private readonly Lyst<Func<ParameterInfo, Option<object>>> m_resolvingFunctions;
    private Predicate<Type> m_shouldSerialize;

    /// <summary>
    /// Creates <see cref="T:Mafi.DependencyResolver" /> from registered data.
    /// </summary>
    public DependencyResolver BuildAndClear()
    {
      return (DependencyResolver) OBqe2IUAeSpOmlOQ4O.TyOaFSuuHy(0, new object[0], (object) this)[0];
    }

    public void RegisterExtraResolver(
      Func<ParameterInfo, Option<object>> resolvingFunction)
    {
      this.m_resolvingFunctions.Add(resolvingFunction);
    }

    public void SetShouldSerializePredicate(Predicate<Type> shouldSerialize)
    {
      Assert.That<Predicate<Type>>(this.m_shouldSerialize).IsNull<Predicate<Type>>("The 'should serialize' predicate can be set only once.");
      this.m_shouldSerialize = shouldSerialize;
    }

    /// <summary>
    /// Returns all dependency type registrations. Note that some dependencies may have been added as instances and
    /// won't be listed here. Does not return registrations of generic type definitions.
    /// </summary>
    public KeyValuePair<Type, Type[]>[] GetRegistrations()
    {
      return this.m_dependencyRegistrations.Select<KeyValuePair<Type, Lyst<Type>>, KeyValuePair<Type, Type[]>>((Func<KeyValuePair<Type, Lyst<Type>>, KeyValuePair<Type, Type[]>>) (kvp => new KeyValuePair<Type, Type[]>(kvp.Key, kvp.Value.ToArray()))).ToArray<KeyValuePair<Type, Type[]>>();
    }

    /// <summary>Returns all registered instances by dependency type.</summary>
    public KeyValuePair<Type, object>[] GetRegisteredInstancesByDepType()
    {
      return this.m_instancesByRegisteredType.ToArray<KeyValuePair<Type, object>>();
    }

    /// <summary>Returns all registered instances by real type.</summary>
    public KeyValuePair<Type, object>[] GetRegisteredInstancesByRealType()
    {
      return this.m_instancesByRealType.ToArray<KeyValuePair<Type, object>>();
    }

    public bool TryGetInstanceByRegisteredType<T>(out T instance)
    {
      object obj1;
      if (this.m_instancesByRealType.TryGetValue(typeof (T), out obj1) && obj1 is T obj2)
      {
        instance = obj2;
        return true;
      }
      instance = default (T);
      return false;
    }

    /// <summary>
    /// Whether any dependency is registered under <typeparamref name="T" />.
    /// </summary>
    public bool IsAnythingRegisteredAs<T>() => this.IsAnythingRegisteredAs(typeof (T));

    public bool IsAnythingRegisteredAs(Type t)
    {
      return this.m_dependencyRegistrations.ContainsKey(t) || this.m_genericDependencyRegistrations.ContainsKey(t);
    }

    /// <summary>
    /// Whether given type <typeparamref name="TInst" /> is registered as <typeparamref name="TDep" />.
    /// </summary>
    public bool IsTypeRegisteredAs<TInst, TDep>()
    {
      return this.IsTypeRegisteredAs(typeof (TInst), typeof (TDep));
    }

    public bool IsTypeRegisteredAs<TDep>(Type tInst)
    {
      return this.IsTypeRegisteredAs(tInst, typeof (TDep));
    }

    public bool IsTypeRegisteredAs(Type tInst, Type tDep)
    {
      Lyst<Type> lyst;
      return this.m_dependencyRegistrations.TryGetValue(tDep, out lyst) && lyst.Contains(tInst);
    }

    public void SetPreferredImplementationFor(Type implType, Type depType)
    {
      Assert.That<bool>(implType.IsClass).IsTrue("Global dependency implementation should be a class.");
      Assert.That<bool>(implType.IsAbstract).IsFalse("Global dependency implementation should not be abstract.");
      this.m_dependencyResolvePreferences[depType] = implType;
    }

    public void SetPreferredImplementationForAllInterfaces(Type implType)
    {
      Type[] interfaces = implType.GetInterfaces();
      if (interfaces.Length == 0)
      {
        Log.Error(string.Format("Setting preferred implementation for all interfaces of type `{0}` that does not ", (object) implType) + "implement any interfaces.");
      }
      else
      {
        foreach (Type depType in interfaces)
        {
          if (depType.GetCustomAttributes(typeof (NotGlobalDependencyAttribute), false).Length == 0)
          {
            Assert.That<bool>(depType.Name.EndsWith("Friend")).IsFalse("Registering under friend!");
            this.SetPreferredImplementationFor(implType, depType);
          }
        }
      }
    }

    /// <summary>
    /// Starts dependency registration process of specified <typeparamref name="T" />. This call does not register
    /// anything yet.
    /// </summary>
    [MustUseReturnValue]
    public DependencyResolverBuilder.DependencyRegistrar RegisterDependency<T>() where T : class
    {
      return this.RegisterDependency(typeof (T));
    }

    /// <summary>
    /// Starts dependency registration process of given <paramref name="type" />. This call does not register anything
    /// yet.
    /// </summary>
    [MustUseReturnValue]
    public DependencyResolverBuilder.DependencyRegistrar RegisterDependency(Type type)
    {
      return new DependencyResolverBuilder.DependencyRegistrar(this, type.CheckNotNull<Type>());
    }

    private void registerDependency(Type implementationType, Type dependencyType, bool ignoreCtors = false)
    {
      Assert.That<bool>(dependencyType.IsInterface && dependencyType.Name.EndsWith("Friend")).IsFalse("Registering friend interface as dependency? You might want to use [NotGlobalDependency] attribute.");
      if (dependencyType.IsGenericTypeDefinition)
      {
        if (!implementationType.IsGenericTypeDefinition)
        {
          Log.Error(string.Format("Non-generic type '{0}' cannot be registered as generic ", (object) implementationType) + string.Format("'{0}'.", (object) dependencyType));
          return;
        }
      }
      else
      {
        if (!dependencyType.IsAssignableFrom(implementationType))
        {
          Log.Error(string.Format("Dependency implementation '{0}' is not assignable to registered ", (object) implementationType) + string.Format("type '{0}'.", (object) dependencyType));
          return;
        }
        if (dependencyType.ContainsGenericParameters || implementationType.ContainsGenericParameters)
        {
          Log.Error("If registering type (and not generic type definition), the dependency type " + string.Format("'{0}' implementation type '{1}' may not contain generic ", (object) dependencyType, (object) implementationType) + "parameters.");
          return;
        }
      }
      if (implementationType.IsAbstract)
      {
        Log.Error(string.Format("Dependency implementation '{0}' can not be abstract type.", (object) implementationType));
      }
      else
      {
        if (!ignoreCtors)
        {
          ConstructorInfo[] constructors = implementationType.GetConstructors();
          if (constructors.Length == 0)
            constructors = implementationType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
          if (constructors.Length != 1)
          {
            Log.Error(string.Format("Dependency implementation '{0}' does not have one public constructor ", (object) implementationType) + "(or one non-public constructor).");
            return;
          }
        }
        Dict<Type, Lyst<Type>> dict = dependencyType.IsGenericTypeDefinition ? this.m_genericDependencyRegistrations : this.m_dependencyRegistrations;
        Lyst<Type> lyst;
        if (!dict.TryGetValue(dependencyType, out lyst))
        {
          lyst = new Lyst<Type>();
          dict.Add(dependencyType, lyst);
        }
        if (lyst.Contains(implementationType))
        {
          Log.Warning(string.Format("Dependency implementation '{0}' is already registered for type ", (object) implementationType) + string.Format("'{0}'.", (object) dependencyType));
        }
        else
        {
          lyst.Add(implementationType);
          this.m_registrationOrder.AddIfNotPresent(implementationType, this.m_registrationOrder.Count);
          this.m_registrationOrder.AddIfNotPresent(dependencyType, this.m_registrationOrder.Count);
        }
      }
    }

    private void unregisterDependency(Type implementationType, Type dependencyType)
    {
      Dict<Type, Lyst<Type>> dict = dependencyType.IsGenericTypeDefinition ? this.m_genericDependencyRegistrations : this.m_dependencyRegistrations;
      Lyst<Type> lyst;
      if (!dict.TryGetValue(dependencyType, out lyst))
        return;
      lyst.Remove(implementationType);
      if (lyst.Count != 0)
        return;
      dict.Remove(dependencyType);
    }

    /// <summary>
    /// Starts dependency registration process of given <paramref name="instance" />. This call does not register
    /// anything yet.
    /// </summary>
    [MustUseReturnValue]
    public DependencyResolverBuilder.DependencyInstanceRegistrar<T> RegisterInstance<T>(T instance) where T : class
    {
      return (object) instance != null ? new DependencyResolverBuilder.DependencyInstanceRegistrar<T>(this, instance) : throw new DependencyResolverException(string.Format("Registering null instance of '{0}'.", (object) typeof (T)));
    }

    private void registerDependencyInstance<T>(T implementation, Type dependencyType)
    {
      Type type = implementation.GetType();
      this.registerDependency(type, dependencyType, true);
      this.m_instancesByRegisteredType.ContainsKey(dependencyType);
      this.m_instancesByRegisteredType[dependencyType] = (object) implementation;
      this.m_instancesByRealType[type] = (object) implementation;
    }

    /// <summary>
    /// Removes all registrations of given type based on registrations from <see cref="T:Mafi.GlobalDependencyAttribute" />.
    /// </summary>
    public DependencyResolverBuilder UnregisterGlobalDependency<T>() where T : class
    {
      this.unregisterGlobalDependency(typeof (T));
      return this;
    }

    /// <summary>
    /// Removes all registered implementations under given <typeparamref name="T" />.
    /// </summary>
    public void ClearRegistrations<T>() where T : class => this.ClearRegistrations(typeof (T));

    /// <summary>
    /// Removes all registered types and instances under given <paramref name="dependencyType" />.
    /// </summary>
    public void ClearRegistrations(Type dependencyType)
    {
      this.m_dependencyRegistrations.Remove(dependencyType);
      foreach (Lyst<Type> lyst in this.m_dependencyRegistrations.Values)
        lyst.RemoveFirst((Predicate<Type>) (x => x == dependencyType));
      this.m_genericDependencyRegistrations.Remove(dependencyType);
      foreach (KeyValuePair<Type, Lyst<Type>> dependencyRegistration in this.m_genericDependencyRegistrations)
        dependencyRegistration.Value.RemoveFirst((Predicate<Type>) (x => x == dependencyType));
      this.m_instancesByRegisteredType.Remove(dependencyType);
      this.m_instancesByRealType.RemoveValues((Predicate<object>) (x => x.GetType() == dependencyType));
    }

    public void ClearAllDepsImplementing<T>() where T : class
    {
      this.ClearAllDepsImplementing(typeof (T));
    }

    public void ClearAllDepsImplementing(Type type)
    {
      this.ClearRegistrations(type);
      foreach (Lyst<Type> lyst in this.m_dependencyRegistrations.Values)
        lyst.RemoveWhere((Predicate<Type>) (x => x.IsAssignableTo(type)));
      foreach (KeyValuePair<Type, Lyst<Type>> dependencyRegistration in this.m_genericDependencyRegistrations)
        dependencyRegistration.Value.RemoveFirst((Predicate<Type>) (x => x.IsAssignableTo(type)));
      this.m_instancesByRegisteredType.RemoveKeys((Predicate<Type>) (x => x.IsAssignableTo(type)));
      this.m_instancesByRealType.RemoveValues((Predicate<object>) (x => x.GetType().IsAssignableTo(type)));
    }

    public void ClearRegisteredType<T>() where T : class => this.ClearRegisteredType(typeof (T));

    public void ClearRegisteredType(Type instanceType)
    {
      foreach (KeyValuePair<Type, Lyst<Type>> dependencyRegistration in this.m_dependencyRegistrations)
        dependencyRegistration.Value.RemoveAll((Predicate<Type>) (x => x == instanceType));
    }

    /// <summary>
    /// Registers all types from given <paramref name="assembly" /> that are assignable to given <typeparamref name="T" /> as <typeparamref name="T" />.
    /// </summary>
    public void RegisterAllTypesImplementing<T>(
      Assembly assembly,
      bool ignoreCtorsCheck = false,
      bool alsoRegisterAsSelf = false,
      bool alsoRegisterAsAllInterfaces = false)
    {
      foreach (Type type1 in assembly.GetTypes())
      {
        if (type1.IsClass && !type1.IsAbstract && type1.IsAssignableTo<T>() && type1.GetCustomAttribute(typeof (NotGlobalDependencyAttribute), false) == null)
        {
          this.registerDependency(type1, typeof (T), ignoreCtorsCheck);
          if (alsoRegisterAsSelf)
            this.registerDependency(type1, type1, ignoreCtorsCheck);
          if (alsoRegisterAsAllInterfaces)
          {
            foreach (Type type2 in type1.GetInterfaces())
            {
              if (!(type2 == typeof (T)) && type2.GetCustomAttribute(typeof (NotGlobalDependencyAttribute), false) == null)
              {
                Assert.That<bool>(type2.Name.EndsWith("Friend")).IsFalse("Registering under friend!");
                this.registerDependency(type1, type2);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Registers all types from given <paramref name="assembly" /> that have the given custom attribute.
    /// </summary>
    public void RegisterAllTypesWithAttribute<T>(
      Assembly assembly,
      bool registerAsSelf,
      bool registerAsAllInterfaces,
      bool ignoreCtorsCheck = false,
      bool considerInheritedAttributes = false,
      Func<Type, T, bool> shouldRegister = null)
      where T : Attribute
    {
      foreach (Type type1 in assembly.GetTypes())
      {
        if (type1.IsClass && !type1.IsAbstract && type1.GetCustomAttribute(typeof (NotGlobalDependencyAttribute), false) == null)
        {
          T customAttribute = type1.GetCustomAttribute<T>(considerInheritedAttributes);
          if ((object) customAttribute != null && (shouldRegister == null || shouldRegister(type1, customAttribute)))
          {
            if (registerAsSelf)
              this.registerDependency(type1, type1, ignoreCtorsCheck);
            if (registerAsAllInterfaces)
            {
              foreach (Type type2 in type1.GetInterfaces())
              {
                if (type2.GetCustomAttribute(typeof (NotGlobalDependencyAttribute), false) == null)
                {
                  Assert.That<bool>(type2.Name.EndsWith("Friend")).IsFalse("Registering under friend!");
                  this.registerDependency(type1, type2);
                }
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Registers all types in given <paramref name="assembly" /> that are marked with
    /// <see cref="T:Mafi.GlobalDependencyAttribute" />. <paramref name="shouldRegisterPredicate" /> can be used to filter
    /// registered types.
    /// </summary>
    public void RegisterAllGlobalDependencies(
      Assembly assembly,
      Predicate<Type> shouldRegisterPredicate = null)
    {
      if (!this.m_registeredGlobalDeps.Add(assembly))
        return;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && !type.IsAbstract && (shouldRegisterPredicate == null || shouldRegisterPredicate(type)))
          this.tryRegisterAllGlobalDependencies(type);
      }
    }

    /// <summary>
    /// WARNING: This method is called via reflection by the tests because it is really hard to test parent public
    /// method <see cref="M:Mafi.DependencyResolverBuilder.RegisterAllGlobalDependencies(System.Reflection.Assembly,System.Predicate{System.Type})" /> that takes <see cref="T:System.Reflection.Assembly" /> as an argument.
    /// </summary>
    private void tryRegisterAllGlobalDependencies(Type implType)
    {
      Assert.That<bool>(implType.IsClass).IsTrue("Global dependency should be a class.");
      Assert.That<bool>(implType.IsAbstract).IsFalse("Global dependency should not be abstract.");
      object[] customAttributes = implType.GetCustomAttributes(typeof (GlobalDependencyAttribute), false);
      if (customAttributes.Length == 0)
        return;
      GlobalDependencyAttribute dependencyAttribute = customAttributes.Length <= 1 ? (GlobalDependencyAttribute) customAttributes[0] : throw new DependencyResolverException(string.Format("Class implements more than one `{0}`: {1}", (object) "GlobalDependencyAttribute", (object) implType));
      if (dependencyAttribute.OnlyInDebug || dependencyAttribute.OnlyInDevOnly)
        return;
      RegistrationMode registrationMode = dependencyAttribute.RegistrationMode;
      if ((registrationMode & RegistrationMode.AsSelf) != (RegistrationMode) 0)
        this.registerDependency(implType, implType);
      if ((registrationMode & RegistrationMode.AsAllInterfaces) != (RegistrationMode) 0)
      {
        Type[] interfaces = implType.GetInterfaces();
        if (interfaces.Length == 0)
        {
          Log.Error(string.Format("Registering all interfaces of type {0} that does not implement any ", (object) implType) + "interfaces.");
        }
        else
        {
          foreach (Type dependencyType in interfaces)
          {
            if (dependencyType.GetCustomAttributes(typeof (NotGlobalDependencyAttribute), false).Length == 0)
            {
              Assert.That<bool>(dependencyType.Name.EndsWith("Friend")).IsFalse("Registering under friend!");
              this.registerDependency(implType, dependencyType);
            }
          }
        }
      }
      if (registrationMode != (RegistrationMode) 0)
        return;
      Log.Warning(string.Format("No registration mode specified for global dependency {0} and no explicit ", (object) implType) + "registration types were given.");
    }

    private void unregisterGlobalDependency(Type implType)
    {
      object[] customAttributes = implType.GetCustomAttributes(typeof (GlobalDependencyAttribute), false);
      if (customAttributes.Length != 1)
      {
        Log.Warning(string.Format("Trying to unregister global dependency registrations of '{0}' but it does not ", (object) implType) + "have the GlobalDependencyAttribute.");
      }
      else
      {
        GlobalDependencyAttribute dependencyAttribute = (GlobalDependencyAttribute) customAttributes[0];
        if (dependencyAttribute.OnlyInDebug || dependencyAttribute.OnlyInDevOnly)
          return;
        RegistrationMode registrationMode = dependencyAttribute.RegistrationMode;
        if ((registrationMode & RegistrationMode.AsSelf) != (RegistrationMode) 0)
          this.unregisterDependency(implType, implType);
        if ((registrationMode & RegistrationMode.AsAllInterfaces) == (RegistrationMode) 0)
          return;
        foreach (Type dependencyType in implType.GetInterfaces())
        {
          if (dependencyType.GetCustomAttributes(typeof (NotGlobalDependencyAttribute), false).Length == 0)
          {
            Assert.That<bool>(dependencyType.Name.EndsWith("Friend")).IsFalse("Registering under friend!");
            this.unregisterDependency(implType, dependencyType);
          }
        }
      }
    }

    public string PrintCurrentRegistrations()
    {
      StringBuilder stringBuilder = new StringBuilder(4096);
      stringBuilder.AppendLine("Dependency registrations");
      foreach (KeyValuePair<Type, Lyst<Type>> dependencyRegistration in this.m_dependencyRegistrations)
      {
        if (dependencyRegistration.Value.Count == 1)
        {
          stringBuilder.AppendLine("  " + dependencyRegistration.Key.Name + ": " + dependencyRegistration.Value.First.FullName);
        }
        else
        {
          stringBuilder.AppendLine("  " + dependencyRegistration.Key.Name + ":");
          foreach (Type type in dependencyRegistration.Value)
            stringBuilder.AppendLine("    " + type.FullName);
        }
      }
      stringBuilder.AppendLine("\n\n");
      stringBuilder.AppendLine("Generic dependency registrations");
      foreach (KeyValuePair<Type, Lyst<Type>> dependencyRegistration in this.m_genericDependencyRegistrations)
      {
        if (dependencyRegistration.Value.Count == 1)
        {
          stringBuilder.AppendLine("  " + dependencyRegistration.Key.Name + ": " + dependencyRegistration.Value.First.Name);
        }
        else
        {
          stringBuilder.AppendLine("  " + dependencyRegistration.Key.Name + ":");
          foreach (Type type in dependencyRegistration.Value)
            stringBuilder.AppendLine("    " + type.FullName);
        }
      }
      stringBuilder.AppendLine("\n\n");
      stringBuilder.AppendLine("Instances by registered type");
      foreach (KeyValuePair<Type, object> keyValuePair in this.m_instancesByRegisteredType)
        stringBuilder.AppendLine(string.Format("  {0}: {1}", (object) keyValuePair.Key.Name, keyValuePair.Value));
      stringBuilder.AppendLine("\n\n");
      stringBuilder.AppendLine("Instances by real type");
      foreach (KeyValuePair<Type, object> keyValuePair in this.m_instancesByRealType)
        stringBuilder.AppendLine(string.Format("  {0}: {1}", (object) keyValuePair.Key.Name, keyValuePair.Value));
      return stringBuilder.ToString();
    }

    public DependencyResolverBuilder()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_dependencyRegistrations = new Dict<Type, Lyst<Type>>();
      this.m_dependencyResolvePreferences = new Dict<Type, Type>();
      this.m_genericDependencyRegistrations = new Dict<Type, Lyst<Type>>();
      this.m_instancesByRegisteredType = new Dict<Type, object>();
      this.m_instancesByRealType = new Dict<Type, object>();
      this.m_registeredGlobalDeps = new Set<Assembly>();
      this.m_registrationOrder = new Dict<Type, int>();
      this.m_resolvingFunctions = new Lyst<Func<ParameterInfo, Option<object>>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    /// <summary>Helper struct for modular registration process.</summary>
    public struct DependencyRegistrar
    {
      private readonly DependencyResolverBuilder m_builder;
      private readonly Type m_dependencyType;

      internal DependencyRegistrar(DependencyResolverBuilder builder, Type dependencyType)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_builder = builder.CheckNotNull<DependencyResolverBuilder>();
        this.m_dependencyType = dependencyType.CheckNotNull<Type>();
      }

      public DependencyResolverBuilder.DependencyRegistrar AsSelf()
      {
        this.m_builder.registerDependency(this.m_dependencyType, this.m_dependencyType);
        return this;
      }

      public DependencyResolverBuilder.DependencyRegistrar AsAllInterfaces(bool ignoreNoInterfaces = false)
      {
        if (this.m_dependencyType.IsGenericTypeDefinition)
          throw new DependencyResolverException(string.Format("Registering all interface of a generic type definition '{0}' is dangerous.", (object) this.m_dependencyType));
        Type[] interfaces = this.m_dependencyType.GetInterfaces();
        if (interfaces.Length == 0)
        {
          if (!ignoreNoInterfaces)
            Log.Error(string.Format("Registering all interfaces of type '{0}' that does not implement any ", (object) this.m_dependencyType) + "interfaces.");
          return this;
        }
        foreach (Type dependencyType in interfaces)
        {
          if (dependencyType.GetCustomAttributes(typeof (NotGlobalDependencyAttribute), false).Length == 0)
            this.m_builder.registerDependency(this.m_dependencyType, dependencyType);
        }
        return this;
      }

      public DependencyResolverBuilder.DependencyRegistrar As<T>() where T : class
      {
        this.m_builder.registerDependency(this.m_dependencyType, typeof (T));
        return this;
      }

      public DependencyResolverBuilder.DependencyRegistrar As(Type t)
      {
        this.m_builder.registerDependency(this.m_dependencyType, t);
        return this;
      }
    }

    /// <summary>Helper struct for modular registration process.</summary>
    public struct DependencyInstanceRegistrar<T> where T : class
    {
      private readonly DependencyResolverBuilder m_builder;
      private readonly T m_impl;

      internal DependencyInstanceRegistrar(DependencyResolverBuilder builder, T implementation)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_builder = builder.CheckNotNull<DependencyResolverBuilder>();
        this.m_impl = implementation.CheckNotNull<T>();
      }

      public DependencyResolverBuilder.DependencyInstanceRegistrar<T> AsSelf()
      {
        this.m_builder.registerDependencyInstance<T>(this.m_impl, this.m_impl.GetType());
        return this;
      }

      public DependencyResolverBuilder.DependencyInstanceRegistrar<T> AsSelfAndAllBaseClasses()
      {
        Type type = this.m_impl.GetType();
        if (type.BaseType == (Type) null || type.BaseType == typeof (object))
        {
          Log.Error(string.Format("Registering type '{0}' as self and all base classes but the type has no ", (object) typeof (T)) + "base classes.");
          return this;
        }
        for (Type dependencyType = this.m_impl.GetType(); dependencyType != typeof (object); dependencyType = dependencyType.BaseType)
          this.m_builder.registerDependencyInstance<T>(this.m_impl, dependencyType);
        return this;
      }

      public DependencyResolverBuilder.DependencyInstanceRegistrar<T> AsAllInterfaces()
      {
        Type[] interfaces = this.m_impl.GetType().GetInterfaces();
        if (interfaces.Length == 0)
        {
          Log.Error(string.Format("Registering all interfaces of type '{0}' that does not implement any interfaces.", (object) typeof (T)));
          return this;
        }
        foreach (Type dependencyType in interfaces)
        {
          if (dependencyType.GetCustomAttributes(typeof (NotGlobalDependencyAttribute), false).Length == 0)
            this.m_builder.registerDependencyInstance<T>(this.m_impl, dependencyType);
        }
        return this;
      }

      public DependencyResolverBuilder.DependencyInstanceRegistrar<T> As<U>() where U : class
      {
        this.m_builder.registerDependencyInstance<T>(this.m_impl, typeof (U));
        return this;
      }
    }
  }
}
