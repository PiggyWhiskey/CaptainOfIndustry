// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.LoadEventsCollector
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Serialization
{
  public class LoadEventsCollector
  {
    private readonly Lyst<LoadEventsCollector.LoadMethodTarget> m_initSelfCriticalMethodsToInvoke;
    private readonly Lyst<LoadEventsCollector.LoadMethodTarget> m_initSelfMethodsToInvoke;
    private readonly Lyst<LoadEventsCollector.LoadMethodTarget> m_initAllMethodsToInvoke;
    private readonly Dict<Type, LoadEventsCollector.LoadMethodsOnType> m_loadMethods;
    private readonly Lyst<Action> m_preInitActions;
    private readonly Lyst<MethodInfo> m_methodInfosCache;

    public void ScanForLoadEvent(object obj) => obj.GetType();

    public void AddPreInitAction(Action action) => this.m_preInitActions.Add(action);

    private LoadEventsCollector.CustomMethodInfo[] collectMethodsFor<T>(Type type)
    {
      this.m_methodInfosCache.Clear();
      for (; type != (Type) null; type = type.BaseType)
        this.m_methodInfosCache.AddRange(((IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.GetCustomAttributes(typeof (T), true).Length != 0)));
      if (this.m_methodInfosCache.IsEmpty)
        return Array.Empty<LoadEventsCollector.CustomMethodInfo>();
      this.m_methodInfosCache.Reverse();
      LoadEventsCollector.CustomMethodInfo[] array = this.m_methodInfosCache.ToArray<LoadEventsCollector.CustomMethodInfo>((Func<MethodInfo, LoadEventsCollector.CustomMethodInfo>) (x => new LoadEventsCollector.CustomMethodInfo(x)));
      this.m_methodInfosCache.Clear();
      return array;
    }

    public void InvokeAll(int saveVersion, DependencyResolver dependencyResolver)
    {
    }

    public LoadEventsCollector()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_initSelfCriticalMethodsToInvoke = new Lyst<LoadEventsCollector.LoadMethodTarget>(128);
      this.m_initSelfMethodsToInvoke = new Lyst<LoadEventsCollector.LoadMethodTarget>(128);
      this.m_initAllMethodsToInvoke = new Lyst<LoadEventsCollector.LoadMethodTarget>(128);
      this.m_loadMethods = new Dict<Type, LoadEventsCollector.LoadMethodsOnType>(64);
      this.m_preInitActions = new Lyst<Action>();
      this.m_methodInfosCache = new Lyst<MethodInfo>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private readonly struct LoadMethodsOnType
    {
      public readonly LoadEventsCollector.CustomMethodInfo[] InitSelfCriticalMethods;
      public readonly LoadEventsCollector.CustomMethodInfo[] InitSelfMethods;
      public readonly LoadEventsCollector.CustomMethodInfo[] InitAllMethods;

      public LoadMethodsOnType(
        LoadEventsCollector.CustomMethodInfo[] initSelfCriticalMethods,
        LoadEventsCollector.CustomMethodInfo[] initSelfMethods,
        LoadEventsCollector.CustomMethodInfo[] initAllMethods)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.InitSelfCriticalMethods = initSelfCriticalMethods;
        this.InitSelfMethods = initSelfMethods;
        this.InitAllMethods = initAllMethods;
      }
    }

    private readonly struct CustomMethodInfo
    {
      public readonly MethodInfo MethodInfo;
      public readonly ParameterInfo[] Parameters;

      public CustomMethodInfo(MethodInfo methodInfo)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.MethodInfo = methodInfo;
        this.Parameters = methodInfo.GetParameters();
      }
    }

    private readonly struct LoadMethodTarget
    {
      private readonly object m_owner;
      private readonly LoadEventsCollector.CustomMethodInfo m_method;

      public LoadMethodTarget(object owner, LoadEventsCollector.CustomMethodInfo method)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_owner = owner;
        this.m_method = method;
      }
    }
  }
}
