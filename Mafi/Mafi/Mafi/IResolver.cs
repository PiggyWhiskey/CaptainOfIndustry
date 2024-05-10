// Decompiled with JetBrains decompiler
// Type: Mafi.IResolver
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi
{
  public interface IResolver
  {
    T Resolve<T>();

    bool TryResolve<T>(out T dep) where T : class;

    T Instantiate<T>();

    T Instantiate<T>(params object[] args);

    T InstantiateAs<T>(Type t, params object[] args);

    Option<T> TryInvokeFactoryHierarchy<T>(object arg1) where T : class;

    Option<T> TryInvokeFactoryHierarchy<T>(object arg1, object arg2) where T : class;
  }
}
