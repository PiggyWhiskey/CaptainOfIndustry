// Decompiled with JetBrains decompiler
// Type: Mafi.IEvent
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi
{
  public interface IEvent : IEventNonSaveable
  {
    /// <summary>
    /// Adds the given action to be invoked when the event is triggered.
    /// 
    /// The given action must be a reference to a regular method that is defined on the type TOwner. Meaning that you
    /// can't register any lambda or method that is defined on the base class. However base class itself can register
    /// its own methods.
    /// 
    /// Having TOwner to be declaring type of the method has 2 reasons:
    /// 1) Checking correct input is fast
    /// 2) When base class registers its callback there is no clash with super class in case the callbacks have the
    /// same name, e.g. onEnabled() { }.
    /// </summary>
    void Add<TOwner>(TOwner owner, Action action) where TOwner : class;

    void Remove<TOwner>(TOwner owner, Action action) where TOwner : class;

    bool IsAdded<TOwner>(TOwner owner, Action action) where TOwner : class;
  }
}
