// Decompiled with JetBrains decompiler
// Type: Mafi.Event
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Saveable event with no arguments.
  /// IMPORTANT: Using Event can have significant perf overhead. Read more in EventBase.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class Event : EventBase<Action>, IEvent, IEventNonSaveable
  {
    [DoNotSave(0, null)]
    private Option<EventNonSaveable> m_nonSaveable;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Event(ThreadType primaryThread = ThreadType.Sim)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector(primaryThread);
    }

    protected override bool TryLoadCallback(
      EventBase<Action>.CallbackSaveData data,
      out Action callback)
    {
      MethodInfo mi;
      if (!data.TryResolveMethodInfo(out mi))
      {
        callback = (Action) null;
        return false;
      }
      Assert.That<int>(mi.GetParameters().Length).IsZero();
      Expression<Action> expression = (Expression<Action>) (() => Expression.Call(data.Owner, mi));
      callback = expression.Compile();
      return true;
    }

    public void Invoke()
    {
      foreach (EventBase<Action>.CallbackData callback in this.Callbacks)
      {
        try
        {
          callback.Callback();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(callback.Callback);
        }
      }
      this.m_nonSaveable.ValueOrNull?.Invoke();
    }

    public void InvokeTraced(string traceCategory)
    {
      foreach (EventBase<Action>.CallbackData callback in this.Callbacks)
      {
        try
        {
          callback.Callback();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(callback.Callback);
        }
      }
      this.m_nonSaveable.ValueOrNull?.InvokeTraced(traceCategory);
    }

    public void AddNonSaveable<TOwner>(TOwner owner, Action action) where TOwner : class
    {
      if (this.m_nonSaveable == (EventNonSaveable) null)
        this.m_nonSaveable = (Option<EventNonSaveable>) new EventNonSaveable(this.PrimaryThread);
      this.m_nonSaveable.Value.AddNonSaveable<TOwner>(owner, action);
    }

    public void RemoveNonSaveable<TOwner>(TOwner owner, Action action) where TOwner : class
    {
      this.m_nonSaveable.ValueOrNull?.RemoveNonSaveable<TOwner>(owner, action);
    }

    public override void ClearAll()
    {
      this.m_nonSaveable.ValueOrNull?.ClearAll();
      base.ClearAll();
    }

    public static void Serialize(Event value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Event>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Event.s_serializeDataDelayedAction);
    }

    public static Event Deserialize(BlobReader reader)
    {
      Event @event;
      if (reader.TryStartClassDeserialization<Event>(out @event))
        reader.EnqueueDataDeserialization((object) @event, Event.s_deserializeDataDelayedAction);
      return @event;
    }

    static Event()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Event.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EventBase<Action>) obj).SerializeData(writer));
      Event.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EventBase<Action>) obj).DeserializeData(reader));
    }
  }
}
