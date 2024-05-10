// Decompiled with JetBrains decompiler
// Type: Mafi.EventNonSaveableBase`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public abstract class EventNonSaveableBase<TAction> where TAction : Delegate
  {
    protected readonly ThreadType PrimaryThread;
    protected readonly LystMutableDuringIter<EventNonSaveableBase<TAction>.RegisteredCallback> Actions;
    private Dict<EventNonSaveableBase<TAction>.RegisteredCallback, int> m_exceptionCounters;

    public int RegisteredEventsCount => this.Actions.Count;

    public EventNonSaveableBase(ThreadType primaryThread)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Actions = new LystMutableDuringIter<EventNonSaveableBase<TAction>.RegisteredCallback>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.PrimaryThread = primaryThread;
    }

    public void AddNonSaveable<TOwner>(TOwner owner, TAction action) where TOwner : class
    {
      this.Actions.Add(new EventNonSaveableBase<TAction>.RegisteredCallback((object) owner, action));
    }

    public void RemoveNonSaveable<TOwner>(TOwner owner, TAction action) where TOwner : class
    {
      this.Actions.RemoveAndAssert(new EventNonSaveableBase<TAction>.RegisteredCallback((object) owner, action));
    }

    public void ClearAll() => this.Actions.Clear();

    protected void RecordExceptionForCallbackAt(
      EventNonSaveableBase<TAction>.RegisteredCallback callback)
    {
      if (this.m_exceptionCounters == null)
        this.m_exceptionCounters = new Dict<EventNonSaveableBase<TAction>.RegisteredCallback, int>();
      if (this.m_exceptionCounters.IncOrInsert1<EventNonSaveableBase<TAction>.RegisteredCallback>(callback) <= 100)
        return;
      this.Actions.Remove(callback);
      Log.Error(string.Format("Too many exceptions by non-savable Action on '{0}'.", callback.Owner));
      this.m_exceptionCounters.Remove(callback);
    }

    protected readonly struct RegisteredCallback : 
      IEquatable<EventNonSaveableBase<TAction>.RegisteredCallback>
    {
      public readonly object Owner;
      public readonly TAction Action;

      public RegisteredCallback(object owner, TAction action)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Owner = owner;
        this.Action = action;
      }

      public bool Equals(
        EventNonSaveableBase<TAction>.RegisteredCallback other)
      {
        return this.Owner == other.Owner && EqualityComparer<TAction>.Default.Equals(this.Action, other.Action);
      }

      public override bool Equals(object obj)
      {
        return obj is EventNonSaveableBase<TAction>.RegisteredCallback other && this.Equals(other);
      }

      public override int GetHashCode() => Hash.Combine<object, TAction>(this.Owner, this.Action);
    }
  }
}
