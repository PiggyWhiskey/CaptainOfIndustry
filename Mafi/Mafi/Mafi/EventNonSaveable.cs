// Decompiled with JetBrains decompiler
// Type: Mafi.EventNonSaveable
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  public sealed class EventNonSaveable : EventNonSaveableBase<Action>, IEventNonSaveable
  {
    public EventNonSaveable(ThreadType primaryThread)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector(primaryThread);
    }

    public void Invoke()
    {
      foreach (EventNonSaveableBase<Action>.RegisteredCallback action in this.Actions)
      {
        try
        {
          action.Action();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(action);
        }
      }
    }

    public void InvokeTraced(string traceCategory) => this.Invoke();
  }
}
