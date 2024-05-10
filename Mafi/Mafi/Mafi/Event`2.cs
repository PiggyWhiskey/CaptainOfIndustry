// Decompiled with JetBrains decompiler
// Type: Mafi.Event`2
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
  /// Saveable event with two arguments.
  /// IMPORTANT: Using Event can have significant perf overhead. Read more in EventBase.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class Event<T1, T2> : 
    EventBase<Action<T1, T2>>,
    IEvent<T1, T2>,
    IEventNonSaveable<T1, T2>
  {
    [DoNotSave(0, null)]
    private Option<EventNonSaveable<T1, T2>> m_nonSaveable;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Event(ThreadType primaryThread = ThreadType.Sim)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector(primaryThread);
    }

    protected override bool TryLoadCallback(
      EventBase<Action<T1, T2>>.CallbackSaveData data,
      out Action<T1, T2> callback)
    {
      MethodInfo mi;
      if (!data.TryResolveMethodInfo(out mi))
      {
        callback = (Action<T1, T2>) null;
        return false;
      }
      Assert.That<int>(mi.GetParameters().Length).IsEqualTo(2);
      ConstantExpression instance = Expression.Constant(data.Owner, data.DeclaringType);
      Expression<Action<T1, T2>> expression = (Expression<Action<T1, T2>>) ((obj1, obj2) => Expression.Call((Expression) instance, mi, obj1, obj2));
      callback = expression.Compile();
      return true;
    }

    public void Invoke(T1 arg1, T2 arg2)
    {
      foreach (EventBase<Action<T1, T2>>.CallbackData callback in this.Callbacks)
      {
        try
        {
          callback.Callback(arg1, arg2);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(callback.Callback);
        }
      }
      this.m_nonSaveable.ValueOrNull?.Invoke(arg1, arg2);
    }

    public void InvokeTraced(T1 arg1, T2 arg2, string traceCategory)
    {
      foreach (EventBase<Action<T1, T2>>.CallbackData callback in this.Callbacks)
      {
        try
        {
          callback.Callback(arg1, arg2);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(callback.Callback);
        }
      }
      this.m_nonSaveable.ValueOrNull?.InvokeTraced(arg1, arg2, traceCategory);
    }

    public void AddNonSaveable<TOwner>(TOwner owner, Action<T1, T2> action) where TOwner : class
    {
      if (this.m_nonSaveable == (EventNonSaveable<T1, T2>) null)
        this.m_nonSaveable = (Option<EventNonSaveable<T1, T2>>) new EventNonSaveable<T1, T2>(this.PrimaryThread);
      this.m_nonSaveable.Value.AddNonSaveable<TOwner>(owner, action);
    }

    public void RemoveNonSaveable<TOwner>(TOwner owner, Action<T1, T2> action) where TOwner : class
    {
      this.m_nonSaveable.ValueOrNull?.RemoveNonSaveable<TOwner>(owner, action);
    }

    public override void ClearAll()
    {
      this.m_nonSaveable.ValueOrNull?.ClearAll();
      base.ClearAll();
    }

    public static void Serialize(Event<T1, T2> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Event<T1, T2>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Event<T1, T2>.s_serializeDataDelayedAction);
    }

    public static Event<T1, T2> Deserialize(BlobReader reader)
    {
      Event<T1, T2> @event;
      if (reader.TryStartClassDeserialization<Event<T1, T2>>(out @event))
        reader.EnqueueDataDeserialization((object) @event, Event<T1, T2>.s_deserializeDataDelayedAction);
      return @event;
    }

    static Event()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Event<T1, T2>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EventBase<Action<T1, T2>>) obj).SerializeData(writer));
      Event<T1, T2>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EventBase<Action<T1, T2>>) obj).DeserializeData(reader));
    }
  }
}
