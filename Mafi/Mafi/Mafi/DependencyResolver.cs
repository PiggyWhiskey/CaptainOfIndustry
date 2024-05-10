// Decompiled with JetBrains decompiler
// Type: Mafi.DependencyResolver
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Logging;
using Mafi.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

#nullable disable
namespace Mafi
{
  /// <summary>All public methods are thread safe.</summary>
  [ManuallyWrittenSerialization]
  public sealed class DependencyResolver : IResolver
  {
    private readonly ObjectPool2<Set<Type>> m_setOfTypesPool;
    /// <summary>
    /// Dependency registrations. Key is dependency type and value is instance type. Multiple types can be registered
    /// under one dependency type.
    /// </summary>
    private readonly IReadOnlyDictionary<Type, Type[]> m_dependencyRegistrations;
    /// <summary>
    /// Dependency registrations for generic type definitions. Key is dependency type and value is instance type.
    /// Multiple types can be registered under one dependency type. Both key and items in the value array are generic
    /// type definitions.
    /// </summary>
    private readonly IReadOnlyDictionary<Type, GenericDependencyImplementations> m_genericDependencyRegistrations;
    private readonly Lyst<object> m_resolvedObjects;
    private readonly IReadOnlyDictionary<Type, Type> m_dependencyResolvePreferences;
    /// <summary>
    /// Already instantiated dependencies ready for resolving. Key is a type of dependency, usually some interface.
    /// </summary>
    private readonly Dict<Type, object> m_resolvedInstancesByRegisteredType;
    /// <summary>
    /// Dictionary for lookup of duplicates to avoid instantiating a dependency implementation more than once. This
    /// can happen if one object is registered under two different interfaces. A key is a real type of the dependency
    /// instance (not its interface).
    /// </summary>
    private readonly Dict<Type, object> m_resolvedInstancesByRealType;
    private readonly IReadOnlyDictionary<Type, int> m_registrationOrder;
    /// <summary>
    /// Extra resolver functions that can be used for injection of dependency resolving process.
    /// </summary>
    private readonly ImmutableArray<Func<ParameterInfo, Option<object>>> m_customResolveFuncs;
    private readonly Predicate<Type> m_shouldSerialize;
    /// <summary>Cached factory actions based on the argument type.</summary>
    private readonly ConcurrentDictionary<Type, Action<object[]>> m_factoryActionCache;
    /// <summary>Cached factory functions based on the argument type.</summary>
    private readonly ConcurrentDictionary<DependencyResolver.FactoryFuncKey, Func<object[], object>> m_factoryFuncCache;
    /// <summary>Cached factory functions based on the argument types.</summary>
    private readonly ConcurrentDictionary<DependencyResolver.FactoryFunc2Key, Func<object[], object>> m_factoryFunc2Cache;
    private readonly object m_resolveLock;
    private int? m_calledExternallyThreadId;
    private int m_externalRecursiveCalls;
    private bool m_isFrozen;
    private bool m_gameWasLoaded;
    private bool m_isTerminated;
    private readonly Dict<Type, bool> m_shouldSerializeCache;
    private readonly Lyst<object> m_serializationTmp;
    private readonly Lyst<KeyValuePair<Type, object>> m_serializationTmpPair;

    /// <summary>
    /// Returns empty dependency resolver. This is mainly for testing.
    /// </summary>
    public static DependencyResolver CreateEmpty()
    {
      return new DependencyResolver((IReadOnlyDictionary<Type, Lyst<Type>>) new Dict<Type, Lyst<Type>>(), (IReadOnlyDictionary<Type, Type>) new Dict<Type, Type>(), (IReadOnlyDictionary<Type, Lyst<Type>>) new Dict<Type, Lyst<Type>>(), new Dict<Type, object>(), new Dict<Type, object>(), new Dict<Type, int>(), ImmutableArray<Func<ParameterInfo, Option<object>>>.Empty, (Predicate<Type>) (_ => true));
    }

    /// <summary>
    /// Called when a new object is instantiated by the resolver. This can be due to dependency resolving or explicit
    /// instantiation.
    /// </summary>
    public event Action<object> ObjectInstantiated;

    public IIndexable<object> ResolvedObjects => (IIndexable<object>) this.m_resolvedObjects;

    /// <summary>Returns all resolved objects so far.</summary>
    internal IEnumerable<object> AllResolvedInstances
    {
      get => (IEnumerable<object>) this.m_resolvedInstancesByRealType.Values;
    }

    /// <summary>
    /// Creates dependency resolver from given data. This should be done by <see cref="T:Mafi.DependencyResolverBuilder" />
    /// class. This constructor takes given instances so make sure the caller is surrendering them!
    /// </summary>
    internal DependencyResolver(
      IReadOnlyDictionary<Type, Lyst<Type>> dependencyRegistrations,
      IReadOnlyDictionary<Type, Type> dependencyResolvePreferences,
      IReadOnlyDictionary<Type, Lyst<Type>> genericDependencyRegistrations,
      Dict<Type, object> instancesByRegisteredType,
      Dict<Type, object> instancesByRealType,
      Dict<Type, int> registrationOrder,
      ImmutableArray<Func<ParameterInfo, Option<object>>> customResolvers,
      Predicate<Type> shouldSerialize)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_resolvedObjects = new Lyst<object>(256);
      this.m_factoryActionCache = new ConcurrentDictionary<Type, Action<object[]>>();
      this.m_factoryFuncCache = new ConcurrentDictionary<DependencyResolver.FactoryFuncKey, Func<object[], object>>();
      this.m_factoryFunc2Cache = new ConcurrentDictionary<DependencyResolver.FactoryFunc2Key, Func<object[], object>>();
      this.m_resolveLock = new object();
      this.m_shouldSerializeCache = new Dict<Type, bool>();
      this.m_serializationTmp = new Lyst<object>(true);
      this.m_serializationTmpPair = new Lyst<KeyValuePair<Type, object>>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_dependencyRegistrations = (IReadOnlyDictionary<Type, Type[]>) dependencyRegistrations.Where<KeyValuePair<Type, Lyst<Type>>>((Func<KeyValuePair<Type, Lyst<Type>>, bool>) (x => x.Value.IsNotEmpty)).ToDictionary<KeyValuePair<Type, Lyst<Type>>, Type, Type[]>((Func<KeyValuePair<Type, Lyst<Type>>, Type>) (kvp => kvp.Key), (Func<KeyValuePair<Type, Lyst<Type>>, Type[]>) (kvp => kvp.Value.ToArray()));
      this.m_dependencyResolvePreferences = (IReadOnlyDictionary<Type, Type>) dependencyResolvePreferences.Where<KeyValuePair<Type, Type>>((Func<KeyValuePair<Type, Type>, bool>) (x => this.m_dependencyRegistrations.ContainsKey(x.Key))).ToDict<Type, Type>();
      this.m_genericDependencyRegistrations = (IReadOnlyDictionary<Type, GenericDependencyImplementations>) genericDependencyRegistrations.Where<KeyValuePair<Type, Lyst<Type>>>((Func<KeyValuePair<Type, Lyst<Type>>, bool>) (x => x.Value.IsNotEmpty)).ToDictionary<KeyValuePair<Type, Lyst<Type>>, Type, GenericDependencyImplementations>((Func<KeyValuePair<Type, Lyst<Type>>, Type>) (kvp => kvp.Key), (Func<KeyValuePair<Type, Lyst<Type>>, GenericDependencyImplementations>) (kvp => new GenericDependencyImplementations(kvp.Key, kvp.Value)));
      this.m_resolvedInstancesByRegisteredType = instancesByRegisteredType;
      this.m_resolvedInstancesByRealType = instancesByRealType;
      this.m_registrationOrder = (IReadOnlyDictionary<Type, int>) registrationOrder;
      this.m_customResolveFuncs = customResolvers;
      this.m_shouldSerialize = shouldSerialize;
      this.m_resolvedInstancesByRealType[typeof (DependencyResolver)] = (object) this;
      this.m_resolvedInstancesByRegisteredType[typeof (DependencyResolver)] = (object) this;
      this.m_resolvedInstancesByRegisteredType[typeof (IResolver)] = (object) this;
      this.m_setOfTypesPool = new ObjectPool2<Set<Type>>(8, (Func<ObjectPool2<Set<Type>>, Set<Type>>) (pool => new Set<Type>()), (Action<Set<Type>>) (x =>
      {
        Assert.That<bool>(Monitor.IsEntered(this.m_resolveLock)).IsTrue("Lock must be held use this pool.");
        x.Clear();
      }));
    }

    public void InstantiateAllAndLock()
    {
      Assert.That<bool>(this.m_isFrozen).IsFalse("Resolver was already initialized and frozen.");
      Assert.That<bool>(this.m_isTerminated).IsFalse("Resolver was already cleared and terminated.");
      StringBuilder stringBuilder = (StringBuilder) null;
      if (Log.IsLogged(LogType.Debug))
      {
        stringBuilder = new StringBuilder(2048);
        stringBuilder.AppendLine("Dependencies resolved:");
      }
      Lyst<KeyValuePair<int, KeyValuePair<Type, Type>>> lyst = new Lyst<KeyValuePair<int, KeyValuePair<Type, Type>>>();
      foreach (KeyValuePair<Type, Type[]> dependencyRegistration in (IEnumerable<KeyValuePair<Type, Type[]>>) this.m_dependencyRegistrations)
      {
        Type key = dependencyRegistration.Key;
        string str = ((IEnumerable<Type>) dependencyRegistration.Value).Select<Type, string>((Func<Type, string>) (x => x.Name)).JoinStrings(", ");
        if (key.GetCustomAttribute<MultiDependencyAttribute>() != null)
        {
          Type type1;
          if (this.m_dependencyResolvePreferences.TryGetValue(key, out type1))
            Log.Error(string.Format("Dependency `{0}` has resolve preference set to `{1}` ", (object) key, (object) type1) + "but the dependency is marked with `MultiDependencyAttribute` and all " + string.Format("registered types ({0}) are being instantiated anyways.", (object) dependencyRegistration.Value.Length));
          stringBuilder?.AppendLine("  " + key.Name + ": instantiating all " + str);
          foreach (Type type2 in dependencyRegistration.Value)
            lyst.Add(Make.Kvp<int, KeyValuePair<Type, Type>>(this.getRegistrationOrder(type2), Make.Kvp<Type, Type>(type2, key)));
        }
        else
        {
          Type type3;
          Type type4;
          if (this.m_dependencyResolvePreferences.TryGetValue(key, out type3))
          {
            if (!((IEnumerable<Type>) dependencyRegistration.Value).Contains<Type>(type3))
              throw new DependencyResolverException(string.Format("Dependency `{0}` has resolve preference set to `{1}` ", (object) key, (object) type3) + "but this type is not in the list of available implementations: " + str + ".");
            type4 = type3;
          }
          else
            type4 = dependencyRegistration.Value.Last<Type>();
          stringBuilder?.AppendLine("  " + key.Name + ": instantiating as " + type4.Name + " out of " + str);
          lyst.Add(Make.Kvp<int, KeyValuePair<Type, Type>>(this.getRegistrationOrder(type4), Make.Kvp<Type, Type>(type4, key)));
        }
      }
      if (stringBuilder != null)
      {
        stringBuilder.AppendLine("Dependencies resolve order:");
        stringBuilder.Append("  ");
      }
      lyst.Sort((Comparison<KeyValuePair<int, KeyValuePair<Type, Type>>>) ((a, b) => a.Key.CompareTo(b.Key)));
      foreach (KeyValuePair<int, KeyValuePair<Type, Type>> keyValuePair in lyst)
      {
        Type key = keyValuePair.Value.Key;
        Type asType = keyValuePair.Value.Value;
        if (stringBuilder != null)
        {
          stringBuilder.Append(key.Name);
          stringBuilder.Append(" (#");
          stringBuilder.Append(keyValuePair.Key);
          stringBuilder.Append("), ");
        }
        this.EnsureResolved(key, asType);
      }
      this.m_isFrozen = true;
    }

    /// <summary>
    /// Returns all implementations types registered under requested dependency type. Note that some dependencies may
    /// have been added as instances and won't be listed here.
    /// </summary>
    public Type[] GetRegistrationsFor<T>() => this.GetRegistrationsFor(typeof (T));

    /// <summary>
    /// Returns all implementation types registered under requested dependency type. Note that some dependencies may
    /// have been added as instances and won't be listed here.
    /// </summary>
    public Type[] GetRegistrationsFor(Type dependencyType)
    {
      Type[] source;
      return this.m_dependencyRegistrations.TryGetValue(dependencyType, out source) ? ((IEnumerable<Type>) source).ToArray<Type>() : Array.Empty<Type>();
    }

    private int getRegistrationOrder(Type t)
    {
      int num;
      return !this.m_registrationOrder.TryGetValue(t, out num) ? int.MaxValue : num;
    }

    /// <summary>
    /// To properly implement locking, any code that does instantiation or resolving must follow this pattern:
    /// 
    /// <code>
    /// checkDeadlock();
    /// lock (m_resolveLock) {
    /// 	enterExternalCall();
    /// 	try {
    /// 		// Your code here.
    /// 	} catch (DependencyResolverException ex) {
    /// 		throw new DependencyResolverException("Helpful message.", ex);
    /// 	} finally {
    /// 		exitExternalCall();
    /// 	}
    /// }
    /// </code>
    /// </summary>
    private void enterExternalCall()
    {
      Assert.That<bool>(Monitor.IsEntered(this.m_resolveLock)).IsTrue("Lock must be held when entering external call.");
      Assert.That<bool>(this.m_isTerminated).IsFalse("Resolver was already cleared and terminated.");
      this.m_calledExternallyThreadId = !this.m_calledExternallyThreadId.HasValue ? new int?(Thread.CurrentThread.ManagedThreadId) : throw new DependencyResolverException("Dependency that is being resolved called resolve. This is not allowed to prevent hard-to-detect infinite recursions. Please take all deps via ctor and have them resolve automatically.");
    }

    /// <summary>
    /// See <see cref="M:Mafi.DependencyResolver.enterExternalCall" /> for docs on usage.
    /// </summary>
    private void exitExternalCall()
    {
      Assert.That<bool>(Monitor.IsEntered(this.m_resolveLock)).IsTrue("Lock must be held when exiting external call");
      this.m_calledExternallyThreadId = this.m_calledExternallyThreadId.HasValue ? new int?() : throw new DependencyResolverException("Inconsistent external call state.");
    }

    /// <summary>
    /// Checks whether current thread is holding a lock.
    /// 
    /// See <see cref="M:Mafi.DependencyResolver.enterExternalCall" /> for docs on usage.
    /// </summary>
    private void checkDeadlock()
    {
      int? externallyThreadId = this.m_calledExternallyThreadId;
      int managedThreadId = Thread.CurrentThread.ManagedThreadId;
      if (externallyThreadId.GetValueOrDefault() == managedThreadId & externallyThreadId.HasValue)
        throw new DependencyResolverException("Dependency that is being resolved called resolve recursively. This will cause a deadlock!");
    }

    /// <summary>
    /// Returns instance of requested type if it was already resolved. Otherwise returns None. This does no
    /// resolving.
    /// </summary>
    public Option<object> GetResolvedInstance(Type type)
    {
      this.checkDeadlock();
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          object resolvedInstance;
          this.m_resolvedInstancesByRealType.TryGetValue(type, out resolvedInstance);
          return (Option<object>) resolvedInstance;
        }
        finally
        {
          this.exitExternalCall();
        }
      }
    }

    public Option<T> GetResolvedInstance<T>() where T : class
    {
      this.checkDeadlock();
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          object obj;
          this.m_resolvedInstancesByRealType.TryGetValue(typeof (T), out obj);
          return Option<T>.Create(obj as T);
        }
        finally
        {
          this.exitExternalCall();
        }
      }
    }

    public Option<T> GetResolvedDependency<T>() where T : class
    {
      this.checkDeadlock();
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          object obj;
          this.m_resolvedInstancesByRegisteredType.TryGetValue(typeof (T), out obj);
          return Option<T>.Create(obj as T);
        }
        finally
        {
          this.exitExternalCall();
        }
      }
    }

    public bool TryGetResolvedDependency<T>(out T dep) where T : class
    {
      this.checkDeadlock();
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          object obj;
          this.m_resolvedInstancesByRegisteredType.TryGetValue(typeof (T), out obj);
          dep = obj as T;
          return (object) dep != null;
        }
        finally
        {
          this.exitExternalCall();
        }
      }
    }

    /// <summary>
    /// Only use when absolutely necessary, like when interface was added to a dependency to keep save compatibility.
    /// </summary>
    internal bool TryRegisterAdditionalInterface<T>(object obj)
    {
      return this.m_resolvedInstancesByRegisteredType.TryAdd(typeof (T), obj);
    }

    /// <summary>
    /// Instantiates given type and provides all necessary dependencies to its constructor. Always returns new
    /// instance.
    /// </summary>
    public T Instantiate<T>() => (T) this.Instantiate(typeof (T), Array.Empty<object>());

    public T Instantiate<T>(params object[] args) => (T) this.Instantiate(typeof (T), args);

    public T InstantiateAs<T>(Type t, params object[] args)
    {
      object obj1 = this.Instantiate(t, args);
      return obj1 is T obj2 ? obj2 : throw new DependencyResolverException("Failed to resolve '" + t.Name + "' as '" + typeof (T).Name + "', got instance of '" + obj1.GetType().Name + "'.");
    }

    /// <summary>
    /// Instantiates given type and provides all necessary dependencies to its constructor. Always returns new
    /// instance.
    /// </summary>
    public object Instantiate(Type type) => this.Instantiate(type, Array.Empty<object>());

    public object Instantiate(Type type, params object[] args)
    {
      this.checkDeadlock();
      object obj;
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          Set<Type> instance = this.m_setOfTypesPool.GetInstance();
          obj = this.instantiate(type, instance, 0, args);
          this.m_setOfTypesPool.ReturnInstance(ref instance);
        }
        catch (DependencyResolverException ex)
        {
          throw new DependencyResolverException(string.Format("Failed to instantiate '{0}'.", (object) type), (Exception) ex);
        }
        finally
        {
          this.exitExternalCall();
        }
      }
      return obj;
    }

    public T Instantiate<T>(Type type) => this.Instantiate<T>(type, Array.Empty<object>());

    public T Instantiate<T>(Type type, params object[] args)
    {
      this.checkDeadlock();
      object obj1;
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          Set<Type> instance = this.m_setOfTypesPool.GetInstance();
          obj1 = this.instantiate(type, instance, 0, args);
          this.m_setOfTypesPool.ReturnInstance(ref instance);
        }
        catch (DependencyResolverException ex)
        {
          throw new DependencyResolverException(string.Format("Failed to instantiate '{0}'.", (object) type), (Exception) ex);
        }
        finally
        {
          this.exitExternalCall();
        }
      }
      return obj1 is T obj2 ? obj2 : throw new DependencyResolverException(string.Format("Failed to instantiate '{0}' as '{1}'. ", (object) type, (object) typeof (T).Name) + "Instantiated type has incompatible type '" + obj1.GetType().Name + "'.");
    }

    private object instantiate(
      Type type,
      Set<Type> pendingResolves,
      int recursionDepth,
      object[] args)
    {
      Assert.That<bool>(Monitor.IsEntered(this.m_resolveLock)).IsTrue("Lock must be held to enter this method.");
      Assert.That<Type>(type).IsNotNull<Type>();
      Assert.That<Set<Type>>(pendingResolves).IsNotNull<Set<Type>>();
      Assert.That<object[]>(args).IsNotNull<object[]>();
      Assert.That<Type>(type).IsNotEqualTo<Type>(typeof (DependencyResolver));
      if (type.IsInterface)
        this.resolveDepType(type, true, out type);
      ConstructorInfo[] constructors = type.GetConstructors();
      if (constructors.Length == 0)
        constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
      ConstructorInfo constructorInfo = constructors.Length == 1 ? constructors[0] : throw new DependencyResolverException(string.Format("Ambiguous constructors of '{0}'. One public or one private constructor expected but ", (object) type) + string.Format("{0} found.", (object) constructors.Length));
      ParameterInfo[] parameters = constructorInfo.GetParameters();
      if (args.Length > parameters.Length)
        throw new DependencyResolverException(string.Format("Failed to instantiate '{0}'. Found constructor has {1} parameters but caller ", (object) type, (object) parameters.Length) + string.Format("supplied {0} of custom parameters found.", (object) args.Length));
      object[] objArray = ArrayPool<object>.Get(parameters.Length);
      for (int index = 0; index < objArray.Length; ++index)
      {
        ParameterInfo parameterInfo = parameters[index];
        if (index < args.Length && args[index] != null)
        {
          Assert.That<bool>(args[index].GetType().IsAssignableTo(parameterInfo.ParameterType)).IsTrue(string.Format("Given parameter of type {0} does not match ctor argument type ", (object) args[index].GetType()) + string.Format("{0} in type {1} at index {2}.", (object) parameterInfo.ParameterType, (object) type, (object) index));
          objArray[index] = args[index];
        }
        else
        {
          foreach (Func<ParameterInfo, Option<object>> customResolveFunc in this.m_customResolveFuncs)
          {
            objArray[index] = customResolveFunc(parameterInfo).ValueOrNull;
            if (objArray[index] != null)
            {
              if (!objArray[index].GetType().IsAssignableTo(parameterInfo.ParameterType))
                throw new DependencyResolverException(string.Format("Custom resolver returned invalid value of type '{0}' ", (object) objArray[index].GetType()) + string.Format("for parameter of type '{0}'.", (object) parameterInfo.ParameterType));
              break;
            }
          }
          if (objArray[index] == null)
          {
            try
            {
              objArray[index] = this.resolveDependency(parameterInfo.ParameterType, true, pendingResolves, recursionDepth + 1);
            }
            catch (DependencyResolverException ex)
            {
              throw new DependencyResolverException(string.Format("Failed to instantiate '{0}'.", (object) type), (Exception) ex);
            }
            Assert.That<object>(objArray[index]).IsNotNull<object>();
          }
        }
      }
      object obj;
      try
      {
        obj = constructorInfo.Invoke(objArray);
      }
      catch (Exception ex)
      {
        throw new DependencyResolverException(string.Format("{0} thrown by constructor of '{1}'.", (object) ex.GetType().Name, (object) type), ex);
      }
      objArray.ReturnToPool<object>();
      Action<object> objectInstantiated = this.ObjectInstantiated;
      if (objectInstantiated != null)
        objectInstantiated(obj);
      return obj;
    }

    private object getOrInstantiateDepImpl(
      Type implType,
      Set<Type> pendingResolves,
      int recursionDepth)
    {
      Assert.That<bool>(Monitor.IsEntered(this.m_resolveLock)).IsTrue("Lock must be held to enter this method.");
      object instantiateDepImpl1;
      if (this.m_resolvedInstancesByRealType.TryGetValue(implType, out instantiateDepImpl1))
        return instantiateDepImpl1;
      object instantiateDepImpl2 = this.instantiate(implType, pendingResolves, recursionDepth + 1, Array.Empty<object>());
      this.m_resolvedInstancesByRealType.Add(instantiateDepImpl2.GetType(), instantiateDepImpl2);
      this.m_resolvedObjects.AddAssertNew(instantiateDepImpl2);
      return instantiateDepImpl2;
    }

    /// <summary>
    /// Resolves given dependency type and returns its instance. It can resolve only previously registered types.
    /// Returned instance is always the same for the requested type - only one instance per dependency is allowed. If
    /// one type implements two dependencies, only one instance will be created.
    /// </summary>
    public T Resolve<T>() => (T) this.Resolve(typeof (T));

    /// <summary>
    /// Resolves given dependency type and returns its instance. It can resolve only previously registered types.
    /// Returned instance is always the same for the requested type - only one instance per dependency is allowed. If
    /// one type implements two dependencies, only one instance will be created.
    /// </summary>
    public object Resolve(Type dependencyType)
    {
      int? externallyThreadId = this.m_calledExternallyThreadId;
      int managedThreadId = Thread.CurrentThread.ManagedThreadId;
      if (externallyThreadId.GetValueOrDefault() == managedThreadId & externallyThreadId.HasValue)
      {
        if (this.m_externalRecursiveCalls >= 10)
          throw new DependencyResolverException("Dependency that is being resolved called resolve recursively too many times. ");
        ++this.m_externalRecursiveCalls;
        object obj;
        try
        {
          Set<Type> instance = this.m_setOfTypesPool.GetInstance();
          obj = this.resolveDependency(dependencyType, true, instance, 0);
          this.m_setOfTypesPool.ReturnInstance(ref instance);
        }
        catch (DependencyResolverException ex)
        {
          throw new DependencyResolverException(string.Format("Failed to resolve '{0}'.", (object) dependencyType), (Exception) ex);
        }
        finally
        {
          --this.m_externalRecursiveCalls;
          Assert.That<int>(this.m_externalRecursiveCalls).IsNotNegative();
        }
        return obj;
      }
      object obj1;
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          Set<Type> instance = this.m_setOfTypesPool.GetInstance();
          obj1 = this.resolveDependency(dependencyType, true, instance, 0);
          this.m_setOfTypesPool.ReturnInstance(ref instance);
        }
        catch (DependencyResolverException ex)
        {
          throw new DependencyResolverException(string.Format("Failed to resolve '{0}'.", (object) dependencyType), (Exception) ex);
        }
        finally
        {
          this.exitExternalCall();
        }
      }
      return obj1;
    }

    /// <summary>
    /// Ensures that given type is resolved and cached. This will also work for types that are not registered as
    /// self. This does not return any value to avoid leaking classes that are not registered as self. Note that this
    /// works only for class types, not interfaces or special types like <see cref="T:Mafi.AllImplementationsOf`1" />.
    /// </summary>
    public void EnsureResolved(Type implType, Type asType)
    {
      Assert.That<bool>(implType.IsClass).IsTrue();
      Assert.That<bool>(implType.IsAbstract).IsFalse();
      Assert.That<bool>(implType.IsAssignableTo(asType)).IsTrue();
      this.checkDeadlock();
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        object obj1;
        try
        {
          object obj2;
          if (this.m_resolvedInstancesByRealType.TryGetValue(implType, out obj2))
          {
            this.m_resolvedInstancesByRegisteredType[asType] = obj2;
            return;
          }
          obj1 = this.instantiate(implType, new Set<Type>(), 0, Array.Empty<object>());
        }
        catch (DependencyResolverException ex)
        {
          throw new DependencyResolverException(string.Format("Failed to instantiate {0}.", (object) implType), (Exception) ex);
        }
        finally
        {
          this.exitExternalCall();
        }
        Assert.That<Type>(obj1.GetType()).IsEqualTo<Type>(implType);
        this.m_resolvedInstancesByRealType.AddAndAssertNew(obj1.GetType(), obj1);
        this.m_resolvedInstancesByRegisteredType[asType] = obj1;
        this.m_resolvedObjects.AddAssertNew(obj1);
      }
    }

    public Option<T> TryResolve<T>() where T : class
    {
      return (Option<T>) (T) this.TryResolve(typeof (T)).ValueOrNull;
    }

    public bool TryResolve<T>(out T dep) where T : class
    {
      dep = (T) this.TryResolve(typeof (T)).ValueOrNull;
      return (object) dep != null;
    }

    /// <summary>
    /// Same as <see cref="M:Mafi.DependencyResolver.Resolve(System.Type)" /> but returns Option.None instead of throwing if there is no corresponding
    /// dependency registered. Still may throw if dependencies of the requested type are missing.
    /// </summary>
    public Option<object> TryResolve(Type dependencyType)
    {
      this.checkDeadlock();
      object obj;
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          Set<Type> instance = this.m_setOfTypesPool.GetInstance();
          obj = this.resolveDependency(dependencyType, false, instance, 0);
          this.m_setOfTypesPool.ReturnInstance(ref instance);
        }
        finally
        {
          this.exitExternalCall();
        }
      }
      return (Option<object>) obj;
    }

    /// <summary>
    /// Resolves all implementations of given dependency type <typeparamref name="T" />. It can resolve only
    /// previously registered types.
    /// </summary>
    public AllImplementationsOf<T> ResolveAll<T>()
    {
      this.checkDeadlock();
      AllImplementationsOf<T> implementationsOf;
      lock (this.m_resolveLock)
      {
        this.enterExternalCall();
        try
        {
          Set<Type> instance = this.m_setOfTypesPool.GetInstance();
          implementationsOf = (AllImplementationsOf<T>) this.resolveDependency(typeof (AllImplementationsOf<T>), true, instance, 0);
          this.m_setOfTypesPool.ReturnInstance(ref instance);
        }
        catch (DependencyResolverException ex)
        {
          throw new DependencyResolverException(string.Format("Failed to resolve all dependencies of '{0}'.", (object) typeof (T)), (Exception) ex);
        }
        finally
        {
          this.exitExternalCall();
        }
      }
      return implementationsOf;
    }

    /// <summary>
    /// Resolves given dependency types and all its dependencies recursively. Returns singleton instance of the
    /// dependency within this resolver (resolving of type T will always return the same instance). If the resolving
    /// fails, it either returns null ( <paramref name="throwOnError" /> set to false) or throws <see cref="T:Mafi.DependencyResolverException" />. This can happen when dependency is not registered or cyclic dependency
    /// is detected. If <paramref name="throwOnError" /> is set to true it always returns non-null.
    /// </summary>
    private object resolveDependency(
      Type dependencyType,
      bool throwOnError,
      Set<Type> pendingResolves,
      int recursionDepth)
    {
      Assert.That<Type>(dependencyType).IsNotNull<Type>();
      Assert.That<Set<Type>>(pendingResolves).IsNotNull<Set<Type>>();
      if (recursionDepth >= 32)
        throw new DependencyResolverException(string.Format("Max recursion depth of {0} was reached while resolving '{1}'.", (object) recursionDepth, (object) dependencyType));
      object obj1;
      if (this.m_resolvedInstancesByRegisteredType.TryGetValue(dependencyType, out obj1))
        return obj1;
      object obj2 = this.tryResolveSpecial(dependencyType, pendingResolves, recursionDepth);
      if (obj2 != null)
        return obj2;
      Type implType;
      if (this.resolveDepType(dependencyType, throwOnError, out implType))
        return (object) null;
      if (this.m_resolvedInstancesByRealType.TryGetValue(implType, out obj1))
      {
        this.m_resolvedInstancesByRegisteredType.Add(dependencyType, obj1);
        return obj1;
      }
      if (!pendingResolves.Add(implType))
      {
        if (throwOnError)
          throw new DependencyResolverException(string.Format("Cyclic dependency of '{0}' detected.", (object) implType));
        return (object) null;
      }
      if (this.m_isFrozen && !implType.IsGenericType)
        throw new DependencyResolverException(string.Format("Failed to instantiate '{0}' while resolving '{1}', resolver is locked.", (object) implType, (object) dependencyType));
      obj1 = this.instantiate(implType, pendingResolves, recursionDepth + 1, Array.Empty<object>());
      Assert.That<bool>(obj1 is DependencyResolver).IsFalse("Duplicate resolver was created.");
      this.m_resolvedInstancesByRegisteredType.AddAndAssertNew(dependencyType, obj1);
      this.m_resolvedInstancesByRealType.AddAndAssertNew(obj1.GetType(), obj1);
      this.m_resolvedObjects.AddAssertNew(obj1);
      return obj1;
    }

    private bool resolveDepType(Type dependencyType, bool throwOnError, out Type implType)
    {
      Type[] typeArray;
      if (this.m_dependencyRegistrations.TryGetValue(dependencyType, out typeArray) && typeArray.IsNotEmpty<Type>())
      {
        Type type;
        if (this.m_dependencyResolvePreferences.TryGetValue(dependencyType, out type))
          implType = ((IEnumerable<Type>) typeArray).Contains<Type>(type) ? type : throw new DependencyResolverException(string.Format("Dependency `{0}` has resolve preference set to `{1}` ", (object) dependencyType, (object) type) + "but this type is not in the list of available implementations: " + ((IEnumerable<Type>) typeArray).Select<Type, string>((Func<Type, string>) (x => x.Name)).JoinStrings(", ") + ".");
        else
          implType = typeArray.Last<Type>();
      }
      else
      {
        GenericDependencyImplementations dependencyImplementations;
        if (dependencyType.IsGenericType && this.m_genericDependencyRegistrations.TryGetValue(dependencyType.GetGenericTypeDefinition(), out dependencyImplementations))
        {
          if (!dependencyImplementations.TryMakeGenericType(dependencyType, out implType))
          {
            if (throwOnError)
              throw new DependencyResolverException(string.Format("Failed to resolve '{0}', failed to create generic type.", (object) dependencyType));
            return true;
          }
        }
        else
        {
          if (throwOnError)
            throw new DependencyResolverException(string.Format("Failed to resolve '{0}', no dependencies registered under that type.", (object) dependencyType));
          implType = (Type) null;
          return true;
        }
      }
      return false;
    }

    private object tryResolveSpecial(
      Type dependencyType,
      Set<Type> pendingResolves,
      int recursionDepth)
    {
      if (!dependencyType.IsGenericType)
        return (object) null;
      Type genericTypeDefinition = dependencyType.GetGenericTypeDefinition();
      if (genericTypeDefinition == typeof (AllImplementationsOf<>))
      {
        object[] andInit = ArrayPool<object>.GetAndInit((object) this.resolveAllDependencies(dependencyType.GetGenericArguments()[0], pendingResolves, recursionDepth + 1));
        object instance = Activator.CreateInstance(dependencyType, andInit);
        andInit.ReturnToPool<object>();
        this.m_resolvedInstancesByRegisteredType.Add(dependencyType, instance);
        return instance;
      }
      if (genericTypeDefinition == typeof (NewInstanceOf<>))
      {
        object[] andInit = ArrayPool<object>.GetAndInit(this.instantiate(dependencyType.GetGenericArguments()[0], pendingResolves, recursionDepth + 1, Array.Empty<object>()));
        object instance = Activator.CreateInstance(dependencyType, andInit);
        andInit.ReturnToPool<object>();
        return instance;
      }
      if (genericTypeDefinition == typeof (LazyResolve<>))
      {
        object[] andInit = ArrayPool<object>.GetAndInit((object) this);
        object instance = Activator.CreateInstance(dependencyType, andInit);
        andInit.ReturnToPool<object>();
        this.m_resolvedInstancesByRegisteredType.Add(dependencyType, instance);
        return instance;
      }
      if (!(genericTypeDefinition == typeof (Option<>)))
        return (object) null;
      object[] andInit1 = ArrayPool<object>.GetAndInit(this.resolveDependency(dependencyType.GetGenericArguments()[0], false, pendingResolves, recursionDepth + 1));
      object instance1 = Activator.CreateInstance(dependencyType, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance, (Binder) null, andInit1, (CultureInfo) null, (object[]) null);
      andInit1.ReturnToPool<object>();
      this.m_resolvedInstancesByRegisteredType.Add(dependencyType, instance1);
      return instance1;
    }

    private object[] resolveAllDependencies(
      Type dependencyType,
      Set<Type> pendingResolves,
      int recursionDepth)
    {
      if (dependencyType.GetCustomAttribute<MultiDependencyAttribute>() == null)
        throw new DependencyResolverException("Failed to resolve all deps of '" + dependencyType.Name + "'. This type is not marked with the 'MultiDependencyAttribute' attribute.");
      Type[] typeArray;
      if (!this.m_dependencyRegistrations.TryGetValue(dependencyType, out typeArray))
        return (object[]) Array.CreateInstance(dependencyType, 0);
      object[] instance1 = (object[]) Array.CreateInstance(dependencyType, typeArray.Length);
      for (int index = 0; index < typeArray.Length; ++index)
      {
        Type type = typeArray[index];
        object instantiateDepImpl;
        if (!this.m_resolvedInstancesByRealType.TryGetValue(type, out instantiateDepImpl))
        {
          Set<Type> instance2 = this.m_setOfTypesPool.GetInstance();
          instance2.AddRange((IEnumerable<Type>) pendingResolves);
          try
          {
            instantiateDepImpl = this.getOrInstantiateDepImpl(type, instance2, recursionDepth + 1);
          }
          catch (DependencyResolverException ex)
          {
            throw new DependencyResolverException(string.Format("Failed to instantiate instance '{0}' of dependency '{1}'.", (object) type, (object) dependencyType), (Exception) ex);
          }
          this.m_setOfTypesPool.ReturnInstance(ref instance2);
        }
        instance1[index] = instantiateDepImpl;
      }
      return instance1;
    }

    /// <summary>
    /// Resolves dependency of type <see cref="T:Mafi.IAction`1" /> and invokes its <see cref="M:Mafi.IAction`1.Invoke(`0)" /> method with given argument that produces desired result. If resolving
    /// fails it also tries to hierarchically resolve all actions with <c>TArg</c> as all base classes of given
    /// <paramref name="arg" />. The first found action will be called.
    /// </summary>
    public void InvokeActionHierarchy(object arg)
    {
      Assert.That<object>(arg).IsNotNull<object>();
      Type type = arg.GetType();
      Action<object[]> action;
      if (!this.m_factoryActionCache.TryGetValue(type, out action))
      {
        this.checkDeadlock();
        MethodInfo methodInfo;
        object actionObject;
        lock (this.m_resolveLock)
        {
          this.enterExternalCall();
          try
          {
            actionObject = this.resolveActionHierarchically(type, out methodInfo);
          }
          finally
          {
            this.exitExternalCall();
          }
        }
        if (actionObject == null)
          throw new DependencyResolverException(string.Format("Failed to hierarchically resolve action for '{0}'.", (object) arg.GetType()));
        action = (Action<object[]>) (args => methodInfo.Invoke(actionObject, args));
        this.m_factoryActionCache.TryAdd(type, action);
      }
      object[] andInit = ArrayPool<object>.GetAndInit(arg);
      action(andInit);
      andInit.ReturnToPool<object>();
    }

    private object resolveActionHierarchically(Type argType, out MethodInfo action)
    {
      for (Type type = argType; type != typeof (object); type = type.BaseType)
      {
        Type dependencyType = typeof (IAction<>).MakeGenericType(type);
        Set<Type> instance = this.m_setOfTypesPool.GetInstance();
        object obj = this.resolveDependency(dependencyType, false, instance, 0);
        this.m_setOfTypesPool.ReturnInstance(ref instance);
        if (obj != null)
        {
          Type[] andInit = ArrayPool<Type>.GetAndInit(type);
          action = dependencyType.GetMethod("Invoke", andInit);
          andInit.ReturnToPool<Type>();
          Assert.That<MethodInfo>(action).IsNotNull<MethodInfo>();
          return obj;
        }
      }
      action = (MethodInfo) null;
      return (object) null;
    }

    /// <summary>
    /// Resolves dependency of type <see cref="T:Mafi.IFactory`2" /> and invokes its <see cref="M:Mafi.IFactory`2.Create(`0)" /> method with given argument that produces desired result. The <c>TArg</c> is type of
    /// given argument. If resolving fails it also tries to hierarchically resolve all factories with <c>TArg</c> as
    /// all base classes of given <paramref name="arg" />. The first found factory will be used for creation of the
    /// result.
    /// </summary>
    public TResult InvokeFactoryHierarchy<TResult>(object arg) where TResult : class
    {
      Option<TResult> option = this.TryInvokeFactoryHierarchy<TResult>(arg);
      return !option.IsNone ? option.Value : throw new DependencyResolverException(string.Format("Failed to hierarchically resolve '{0}' factory for '{1}'.", (object) typeof (TResult), (object) arg.GetType()));
    }

    /// <summary>
    /// Resolves dependency of type <see cref="T:Mafi.IFactory`2" /> and invokes its <see cref="M:Mafi.IFactory`2.Create(`0)" /> method with given argument that produces desired result. The <c>TArg</c> is type of
    /// given argument. If resolving fails it also tries to hierarchically resolve all factories with <c>TArg</c> as
    /// all base classes of given <paramref name="arg" />. The first found factory will be used for creation of the
    /// result.
    /// </summary>
    public Option<TResult> TryInvokeFactoryHierarchy<TResult>(object arg) where TResult : class
    {
      Assert.That<object>(arg).IsNotNull<object>();
      DependencyResolver.FactoryFuncKey key = new DependencyResolver.FactoryFuncKey(arg.GetType(), typeof (TResult));
      Func<object[], object> func;
      if (!this.m_factoryFuncCache.TryGetValue(key, out func))
      {
        this.checkDeadlock();
        MethodInfo factoryMethod;
        object factory;
        lock (this.m_resolveLock)
        {
          this.enterExternalCall();
          try
          {
            factory = this.resolveFactoryHierarchically<TResult>(key.ArgType, out factoryMethod);
          }
          finally
          {
            this.exitExternalCall();
          }
        }
        if (factory == null)
          return Option<TResult>.None;
        func = (Func<object[], object>) (args => factoryMethod.Invoke(factory, args));
        this.m_factoryFuncCache.TryAdd(key, func);
      }
      object[] andInit = ArrayPool<object>.GetAndInit(arg);
      TResult result = (TResult) func(andInit);
      if ((object) result == null)
        Log.Error(string.Format("Factory for '{0}' returned null.", (object) arg.GetType()));
      andInit.ReturnToPool<object>();
      return (Option<TResult>) result;
    }

    private object resolveFactoryHierarchically<TResult>(Type argType, out MethodInfo factoryMethod)
    {
      Assert.That<Type>(argType).IsNotEqualTo<Type>(typeof (object));
      for (Type type1 = argType; type1 != typeof (object); type1 = type1.BaseType)
      {
        Type dependencyType = typeof (IFactory<,>).MakeGenericType(type1, typeof (TResult));
        Set<Type> instance = this.m_setOfTypesPool.GetInstance();
        object obj = this.resolveDependency(dependencyType, false, instance, 0);
        this.m_setOfTypesPool.ReturnInstance(ref instance);
        if (obj == null)
        {
          foreach (Type type2 in type1.GetInterfaces())
          {
            dependencyType = typeof (IFactory<,>).MakeGenericType(type2, typeof (TResult));
            instance = this.m_setOfTypesPool.GetInstance();
            obj = this.resolveDependency(dependencyType, false, instance, 0);
            this.m_setOfTypesPool.ReturnInstance(ref instance);
            if (obj != null)
              break;
          }
        }
        if (obj != null)
        {
          Type[] andInit = ArrayPool<Type>.GetAndInit(type1);
          factoryMethod = dependencyType.GetMethod("Create", andInit);
          andInit.ReturnToPool<Type>();
          Assert.That<MethodInfo>(factoryMethod).IsNotNull<MethodInfo>();
          return obj;
        }
      }
      factoryMethod = (MethodInfo) null;
      return (object) null;
    }

    /// <summary>
    /// Resolves dependency of type <see cref="T:Mafi.IFactory`3" /> and invokes its <see cref="M:Mafi.IFactory`3.Create(`0,`1)" /> method with given arguments that produces
    /// desired result. The <c>TArg1</c> and <c>TArg2</c> are types of given arguments. If resolving fails it also
    /// tries to hierarchically resolve all factories of all combinations of base classes of types <paramref name="arg1" /> and <paramref name="arg2" />. The first found factory will be used for creation of the result.
    /// Note that first all base classes of <paramref name="arg2" /> are tested before next base class of <paramref name="arg1" />.
    /// </summary>
    public TResult InvokeFactoryHierarchy<TResult>(object arg1, object arg2)
    {
      Assert.That<object>(arg1).IsNotNull<object>();
      Assert.That<object>(arg2).IsNotNull<object>();
      DependencyResolver.FactoryFunc2Key key = new DependencyResolver.FactoryFunc2Key(arg1.GetType(), arg2.GetType(), typeof (TResult));
      Func<object[], object> func;
      if (!this.m_factoryFunc2Cache.TryGetValue(key, out func))
      {
        this.checkDeadlock();
        MethodInfo factoryMethod;
        object factory;
        lock (this.m_resolveLock)
        {
          this.enterExternalCall();
          try
          {
            factory = this.resolveFactoryHierarchically<TResult>(key.Arg1Type, key.Arg2Type, out factoryMethod);
          }
          finally
          {
            this.exitExternalCall();
          }
        }
        if (factory == null)
          throw new DependencyResolverException(string.Format("Failed to hierarchically resolve '{0}' factory for '{1}' and '{2}'.", (object) typeof (TResult), (object) key.Arg1Type, (object) key.Arg2Type));
        func = (Func<object[], object>) (args => factoryMethod.Invoke(factory, args));
        this.m_factoryFunc2Cache.TryAdd(key, func);
      }
      object[] andInit = ArrayPool<object>.GetAndInit(arg1, arg2);
      TResult result = (TResult) func(andInit);
      andInit.ReturnToPool<object>();
      return result;
    }

    /// <summary>
    /// Resolves dependency of type <see cref="T:Mafi.IFactory`3" /> and invokes its <see cref="M:Mafi.IFactory`3.Create(`0,`1)" /> method with given arguments that produces
    /// desired result. The <c>TArg1</c> and <c>TArg2</c> are types of given arguments. If resolving fails it also
    /// tries to hierarchically resolve all factories of all combinations of base classes of types <paramref name="arg1" /> and <paramref name="arg2" />. The first found factory will be used for creation of the result.
    /// Note that first all base classes of <paramref name="arg2" /> are tested before next base class of <paramref name="arg1" />.
    /// </summary>
    public Option<TResult> TryInvokeFactoryHierarchy<TResult>(object arg1, object arg2) where TResult : class
    {
      return (Option<TResult>) this.TryInvokeFactoryHierarchyDefault<TResult>(arg1, arg2, (Func<TResult>) (() => default (TResult)));
    }

    /// <summary>
    /// Resolves dependency of type <see cref="T:Mafi.IFactory`3" /> and invokes its <see cref="M:Mafi.IFactory`3.Create(`0,`1)" /> method with given arguments that produces
    /// desired result. The <c>TArg1</c> and <c>TArg2</c> are types of given arguments. If resolving fails it also
    /// tries to hierarchically resolve all factories of all combinations of base classes of types <paramref name="arg1" /> and <paramref name="arg2" />. The first found factory will be used for creation of the result.
    /// Note that first all base classes of <paramref name="arg2" /> are tested before next base class of <paramref name="arg1" />. If resolve fails completely than <paramref name="defaultProvider" /> is used to return a
    /// default value.
    /// </summary>
    public TResult TryInvokeFactoryHierarchyDefault<TResult>(
      object arg1,
      object arg2,
      Func<TResult> defaultProvider)
    {
      DependencyResolver.FactoryFunc2Key key = new DependencyResolver.FactoryFunc2Key(arg1.GetType(), arg2.GetType(), typeof (TResult));
      Func<object[], object> func;
      if (!this.m_factoryFunc2Cache.TryGetValue(key, out func))
      {
        this.checkDeadlock();
        MethodInfo factoryMethod;
        object factory;
        lock (this.m_resolveLock)
        {
          this.enterExternalCall();
          try
          {
            factory = this.resolveFactoryHierarchically<TResult>(key.Arg1Type, key.Arg2Type, out factoryMethod);
          }
          finally
          {
            this.exitExternalCall();
          }
        }
        if (factory == null)
          return defaultProvider();
        func = (Func<object[], object>) (args => factoryMethod.Invoke(factory, args));
        this.m_factoryFunc2Cache.TryAdd(key, func);
      }
      object[] andInit = ArrayPool<object>.GetAndInit(arg1, arg2);
      TResult result = (TResult) func(andInit);
      andInit.ReturnToPool<object>();
      return result;
    }

    private object resolveFactoryHierarchically<TResult>(
      Type arg1Type,
      Type arg2Type,
      out MethodInfo factoryMethod)
    {
      Type[] argTypesPooled = (Type[]) null;
      for (Type t1_1 = arg1Type; t1_1 != typeof (object); t1_1 = t1_1.BaseType)
      {
        object factory;
        for (Type t2_1 = arg2Type; t2_1 != typeof (object); t2_1 = t2_1.BaseType)
        {
          if (factoryForPair(t1_1, t2_1, out factoryMethod))
          {
            Assert.That<MethodInfo>(factoryMethod).IsNotNull<MethodInfo>();
            return factory;
          }
          foreach (Type t2_2 in t2_1.GetInterfaces())
          {
            if (factoryForPair(t1_1, t2_2, out factoryMethod))
            {
              Assert.That<MethodInfo>(factoryMethod).IsNotNull<MethodInfo>();
              return factory;
            }
          }
        }
        foreach (Type t1_2 in t1_1.GetInterfaces())
        {
          for (Type t2_3 = arg2Type; t2_3 != typeof (object); t2_3 = t2_3.BaseType)
          {
            if (factoryForPair(t1_2, t2_3, out factoryMethod))
            {
              Assert.That<MethodInfo>(factoryMethod).IsNotNull<MethodInfo>();
              return factory;
            }
            foreach (Type t2_4 in t2_3.GetInterfaces())
            {
              if (factoryForPair(t1_2, t2_4, out factoryMethod))
              {
                Assert.That<MethodInfo>(factoryMethod).IsNotNull<MethodInfo>();
                return factory;
              }
            }
          }
        }
        Type factoryType;
        Set<Type> pendingResolvesPooled;

        bool factoryForPair<TResult>(Type t1, Type t2, out MethodInfo factoryMethod)
        {
          factoryType = typeof (IFactory<,,>).MakeGenericType(t1, t2, typeof (TResult));
          pendingResolvesPooled = this.m_setOfTypesPool.GetInstance();
          factory = this.resolveDependency(factoryType, false, pendingResolvesPooled, 0);
          this.m_setOfTypesPool.ReturnInstance(ref pendingResolvesPooled);
          if (factory != null)
          {
            argTypesPooled = ArrayPool<Type>.GetAndInit(t1, t2);
            factoryMethod = factoryType.GetMethod("Create", argTypesPooled);
            argTypesPooled.ReturnToPool<Type>();
          }
          else
            factoryMethod = (MethodInfo) null;
          return factory != null;
        }
      }
      factoryMethod = (MethodInfo) null;
      return (object) null;
    }

    /// <summary>
    /// Clears the resolver and marks it as terminated. This also calls <see cref="M:System.IDisposable.Dispose" />
    /// on all resolved objects that implement <see cref="T:System.IDisposable" />.
    /// </summary>
    public void TerminateAndClear()
    {
      Assert.That<bool>(this.m_isTerminated).IsFalse("Resolver was already cleared and terminated.");
      for (int index = this.m_resolvedObjects.Count - 1; index >= 0; --index)
      {
        object resolvedObject = this.m_resolvedObjects[index];
        if (resolvedObject is IDisposable disposable)
        {
          try
          {
            disposable.Dispose();
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Exception thrown while disposing '" + resolvedObject.GetType().Name + "'.");
          }
        }
      }
      this.ObjectInstantiated = (Action<object>) null;
      this.m_resolvedObjects.Clear();
      this.m_resolvedInstancesByRegisteredType.Clear();
      this.m_resolvedInstancesByRealType.Clear();
      this.m_factoryActionCache.Clear();
      this.m_factoryFuncCache.Clear();
      this.m_factoryFunc2Cache.Clear();
      this.m_serializationTmp.Clear();
      this.m_serializationTmpPair.Clear();
      this.m_isTerminated = true;
    }

    public static void Serialize(DependencyResolver value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DependencyResolver>(value))
        return;
      value.serializeData(writer);
    }

    private void serializeData(BlobWriter writer)
    {
      Assert.That<bool>(this.m_isTerminated).IsFalse("Resolver was already cleared and terminated.");
      this.m_serializationTmp.Clear();
      this.m_resolvedObjects.ToLyst<object>((Predicate<object>) (x => shouldSerialize(x.GetType())), this.m_serializationTmp);
      writer.WriteIntNotNegative(this.m_serializationTmp.Count);
      foreach (object obj in this.m_serializationTmp)
        writer.WriteGeneric<object>(obj);
      this.m_serializationTmp.Clear();
      this.m_resolvedInstancesByRealType.Values.ToLyst<object>((Predicate<object>) (x => shouldSerialize(x.GetType())), this.m_serializationTmp);
      writer.WriteIntNotNegative(this.m_serializationTmp.Count);
      foreach (object obj in this.m_serializationTmp)
        writer.WriteGeneric<object>(obj);
      this.m_serializationTmp.Clear();
      this.m_serializationTmpPair.Clear();
      this.m_resolvedInstancesByRegisteredType.ToLyst<KeyValuePair<Type, object>>((Predicate<KeyValuePair<Type, object>>) (x => !isLazy(x.Key) && shouldSerialize(x.Value.GetType())), this.m_serializationTmpPair);
      writer.WriteIntNotNegative(this.m_serializationTmpPair.Count);
      foreach (KeyValuePair<Type, object> keyValuePair in this.m_serializationTmpPair)
      {
        writer.WriteType(keyValuePair.Key);
        writer.WriteGeneric<object>(keyValuePair.Value);
      }
      this.m_serializationTmpPair.Clear();

      static bool isLazy(Type t)
      {
        return t.IsGenericType && t.GetGenericTypeDefinition() == typeof (LazyResolve<>);
      }

      bool shouldSerialize(Type type)
      {
        bool flag;
        if (!this.m_shouldSerializeCache.TryGetValue(type, out flag))
        {
          Type genericTypeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : (Type) null;
          flag = type != typeof (DependencyResolver) && this.m_shouldSerialize(type) && genericTypeDefinition != typeof (AllImplementationsOf<>) && genericTypeDefinition != typeof (LazyResolve<>) && genericTypeDefinition != typeof (Option<>) && writer.CanSerialize(type);
          this.m_shouldSerializeCache.Add(type, flag);
        }
        return flag;
      }
    }

    public static DependencyResolver Deserialize(BlobReader reader)
    {
      DependencyResolver dependencyResolver;
      if (reader.TryStartClassDeserialization<DependencyResolver>(out dependencyResolver))
      {
        Log.Error("Resolver was loaded from scratch, this should not happen.");
        dependencyResolver.deserializeData(reader);
      }
      return dependencyResolver;
    }

    public static void DeserializeInto(DependencyResolver resolver, BlobReader reader)
    {
      DependencyResolver dependencyResolver;
      if (reader.TryStartClassDeserialization<DependencyResolver>(out dependencyResolver, (Func<BlobReader, Type, DependencyResolver>) ((r, t) => resolver)))
        dependencyResolver.deserializeData(reader);
      else
        Log.Error("Failed to deserialize resolver into given instance, other instance already resolved?");
    }

    private void deserializeData(BlobReader reader)
    {
      Assert.That<bool>(this.m_gameWasLoaded).IsFalse("Repeated loading of resolver?");
      this.m_gameWasLoaded = true;
      Assert.That<Lyst<object>>(this.m_resolvedObjects).IsEmpty<object>("Resolver is not empty before loading started.");
      int num1 = reader.ReadIntNotNegative();
      this.m_resolvedObjects.EnsureCapacity(this.m_resolvedObjects.Count + num1);
      for (int index = 0; index < num1; ++index)
        this.m_resolvedObjects.Add(reader.ReadGenericAs<object>());
      int num2 = reader.ReadIntNotNegative();
      this.m_resolvedInstancesByRealType.EnsureCapacity(this.m_resolvedInstancesByRealType.Count + num2);
      for (int index = 0; index < num2; ++index)
      {
        object obj = reader.ReadGenericAs<object>();
        this.m_resolvedInstancesByRealType.AddAndAssertNew(obj.GetType(), obj);
      }
      int num3 = reader.ReadIntNotNegative();
      this.m_resolvedInstancesByRegisteredType.EnsureCapacity(this.m_resolvedInstancesByRegisteredType.Count + num3);
      for (int index = 0; index < num3; ++index)
      {
        Type type = reader.ReadType();
        object obj = reader.ReadGenericAs<object>();
        if (obj.GetType().IsAssignableTo(type))
          this.m_resolvedInstancesByRegisteredType.AddAndAssertNew(type, obj);
        else
          Log.Info(string.Format("Dropping registered dependency {0} registered as {1} since it is ", (object) obj.GetType(), (object) type.Name) + "no longer derived from that type");
      }
      if (reader.LoadedSaveVersion >= 153)
        return;
      foreach (KeyValuePair<Type, object> keyValuePair in this.m_resolvedInstancesByRegisteredType)
      {
        Type type = keyValuePair.Value.GetType();
        if (type.IsGenericType)
        {
          int num4 = type.GetGenericTypeDefinition() == typeof (Option<>) ? 1 : 0;
        }
      }
    }

    /// <summary>
    /// Key for hierarchy function factories with one argument.
    /// </summary>
    private readonly struct FactoryFuncKey : IEquatable<DependencyResolver.FactoryFuncKey>
    {
      public readonly Type ArgType;
      public readonly Type ResultType;
      private readonly int m_hash;

      public FactoryFuncKey(Type argType, Type resultType)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.ArgType = argType;
        this.ResultType = resultType;
        this.m_hash = Hash.Combine<Type, Type>(argType, resultType);
      }

      public bool Equals(DependencyResolver.FactoryFuncKey other)
      {
        return this.ArgType == other.ArgType && this.ResultType == other.ResultType;
      }

      public override int GetHashCode() => this.m_hash;
    }

    /// <summary>
    /// Key for hierarchy function factories with two arguments.
    /// </summary>
    private readonly struct FactoryFunc2Key : IEquatable<DependencyResolver.FactoryFunc2Key>
    {
      public readonly Type Arg1Type;
      public readonly Type Arg2Type;
      public readonly Type ResultType;
      private readonly int m_hash;

      public FactoryFunc2Key(Type arg1Type, Type arg2Type, Type resultType)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Arg1Type = arg1Type;
        this.Arg2Type = arg2Type;
        this.ResultType = resultType;
        this.m_hash = arg1Type.GetHashCode() ^ arg2Type.GetHashCode() ^ resultType.GetHashCode();
      }

      public bool Equals(DependencyResolver.FactoryFunc2Key other)
      {
        return this.Arg1Type == other.Arg1Type && this.Arg2Type == other.Arg2Type && this.ResultType == other.ResultType;
      }

      public override int GetHashCode() => this.m_hash;
    }
  }
}
