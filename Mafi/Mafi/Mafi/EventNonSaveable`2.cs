// Decompiled with JetBrains decompiler
// Type: Mafi.EventNonSaveable`2
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  public sealed class EventNonSaveable<T1, T2> : 
    EventNonSaveableBase<Action<T1, T2>>,
    IEventNonSaveable<T1, T2>
  {
    public EventNonSaveable(ThreadType primaryThread)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector(primaryThread);
    }

    public void Invoke(T1 arg1, T2 arg2)
    {
      foreach (EventNonSaveableBase<Action<T1, T2>>.RegisteredCallback action in this.Actions)
      {
        try
        {
          action.Action(arg1, arg2);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(action);
        }
      }
    }

    public void InvokeTraced(T1 arg1, T2 arg2, string traceCategory) => this.Invoke(arg1, arg2);
  }
}
