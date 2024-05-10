// Decompiled with JetBrains decompiler
// Type: Mafi.Event`1
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
  /// Saveable event with one argument.
  /// IMPORTANT: Using Event can have significant perf overhead. Read more in EventBase.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class Event<T> : EventBase<Action<T>>, IEvent<T>, IEventNonSaveable<T>
  {
    [DoNotSave(0, null)]
    private Option<EventNonSaveable<T>> m_nonSaveable;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    internal int RegisteredEventsNonSaveableCount
    {
      get
      {
        EventNonSaveable<T> valueOrNull = this.m_nonSaveable.ValueOrNull;
        return valueOrNull == null ? 0 : valueOrNull.RegisteredEventsCount;
      }
    }

    public Event(ThreadType primaryThread = ThreadType.Sim)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector(primaryThread);
    }

    protected override bool TryLoadCallback(
      EventBase<Action<T>>.CallbackSaveData data,
      out Action<T> callback)
    {
      MethodInfo mi;
      if (!data.TryResolveMethodInfo(out mi))
      {
        callback = (Action<T>) null;
        return false;
      }
      Assert.That<int>(mi.GetParameters().Length).IsEqualTo(1);
      ParameterExpression parameterExpression;
      Expression<Action<T>> expression = Expression.Lambda<Action<T>>((Expression) Expression.Call((Expression) Expression.Constant(data.Owner, data.DeclaringType), mi, (Expression) parameterExpression), parameterExpression);
      callback = expression.Compile();
      return true;
    }

    public void Invoke(T arg)
    {
      foreach (EventBase<Action<T>>.CallbackData callback in this.Callbacks)
      {
        try
        {
          callback.Callback(arg);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(callback.Callback);
        }
      }
      this.m_nonSaveable.ValueOrNull?.Invoke(arg);
    }

    public void InvokeTraced(T arg, string traceCategory)
    {
      foreach (EventBase<Action<T>>.CallbackData callback in this.Callbacks)
      {
        try
        {
          callback.Callback(arg);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception thrown in event callback.");
          this.RecordExceptionForCallbackAt(callback.Callback);
        }
      }
      this.m_nonSaveable.ValueOrNull?.Invoke(arg);
    }

    public void AddNonSaveable<TOwner>(TOwner owner, Action<T> action) where TOwner : class
    {
      if (this.m_nonSaveable == (EventNonSaveable<T>) null)
        this.m_nonSaveable = (Option<EventNonSaveable<T>>) new EventNonSaveable<T>(this.PrimaryThread);
      this.m_nonSaveable.Value.AddNonSaveable<TOwner>(owner, action);
    }

    public void RemoveNonSaveable<TOwner>(TOwner owner, Action<T> action) where TOwner : class
    {
      this.m_nonSaveable.ValueOrNull?.RemoveNonSaveable<TOwner>(owner, action);
    }

    public override void ClearAll()
    {
      this.m_nonSaveable.ValueOrNull?.ClearAll();
      base.ClearAll();
    }

    public static void Serialize(Event<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Event<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Event<T>.s_serializeDataDelayedAction);
    }

    public static Event<T> Deserialize(BlobReader reader)
    {
      Event<T> @event;
      if (reader.TryStartClassDeserialization<Event<T>>(out @event))
        reader.EnqueueDataDeserialization((object) @event, Event<T>.s_deserializeDataDelayedAction);
      return @event;
    }

    static Event()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Event<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EventBase<Action<T>>) obj).SerializeData(writer));
      Event<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EventBase<Action<T>>) obj).DeserializeData(reader));
    }
  }
}
