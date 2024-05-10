// Decompiled with JetBrains decompiler
// Type: Mafi.EventBase`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Serialization;
using System;
using System.Reflection;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Besides memory overhead, Events are extremely costly for load / init.
  /// Use them only for high level stuff where count of listeners / registrars is very small.
  /// </summary>
  public abstract class EventBase<TAction> where TAction : Delegate
  {
    public const int MAX_EXCEPTIONS_COUNT = 100;
    public readonly ThreadType PrimaryThread;
    [DoNotSave(0, null)]
    private LystMutableDuringIter<EventBase<TAction>.CallbackData> m_callbacks;
    [DoNotSave(0, null)]
    private Dict<TAction, int> m_exceptionCounters;
    private readonly Lyst<EventBase<TAction>.CallbackSaveData> m_callbacksSaveData;

    internal int RegisteredEventsCount => this.Callbacks.Count;

    /// <summary>
    /// We load callbacks only after someone requests them because resolving callbacks and
    /// compiling their lambda expressions is extremely expensive. It takes approx 1 sec
    /// to load 1-2k callbacks. It does not matter if we have 1 event with 1k callbacks
    /// or 1k events with 1 callback each. However, this still means that game might lag
    /// when some larger set of Events is fired initially - e.g. registering render managers.
    /// So avoid using Events for many objects.
    /// </summary>
    [DoNotSave(0, null)]
    protected LystMutableDuringIter<EventBase<TAction>.CallbackData> Callbacks
    {
      get
      {
        if (this.m_callbacks != null)
          return this.m_callbacks;
        this.m_callbacks = new LystMutableDuringIter<EventBase<TAction>.CallbackData>();
        foreach (EventBase<TAction>.CallbackSaveData data in this.m_callbacksSaveData)
        {
          TAction callback;
          if (this.TryLoadCallback(data, out callback))
            this.Callbacks.Add(new EventBase<TAction>.CallbackData(callback, data.Owner, data.MethodName));
          else
            Log.Error("Failed to load event '" + data.MethodName + "' on type '" + data.DeclaringType.Name + "'.");
        }
        return this.m_callbacks;
      }
    }

    public EventBase(ThreadType primaryThread)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_callbacks = new LystMutableDuringIter<EventBase<TAction>.CallbackData>();
      this.m_callbacksSaveData = new Lyst<EventBase<TAction>.CallbackSaveData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.PrimaryThread = primaryThread;
    }

    /// <summary>
    /// Adds the given action to be invoked when the event is triggered.
    /// 
    /// The given action must be a reference to a regular method that is defined on the type TOwner. You can't
    /// register a lambda or method that is defined on the base class (unless <paramref name="allowInherited" />
    /// is set to true.
    /// 
    /// Having TOwner to be declaring type of the method has 2 reasons:
    /// 1) Checking correct input is fast
    /// 2) When base class registers its callback there is no clash with super class in case the callbacks have the
    /// same name, e.g. onEnabled() { }.
    /// </summary>
    public void Add<TOwner>(TOwner owner, TAction callback) where TOwner : class
    {
      string name = callback.Method.Name;
      EventBase<TAction>.CallbackSaveData callbackSaveData1 = new EventBase<TAction>.CallbackSaveData((object) owner, typeof (TOwner), name);
      if (!callbackSaveData1.TryResolveMethodInfo(out MethodInfo _))
      {
        Log.Error("Failed to register event as method '" + name + "' on type '" + typeof (TOwner).Name + "'. This method cannot be loaded back. Is it non-static instance method on that type?");
      }
      else
      {
        foreach (EventBase<TAction>.CallbackSaveData callbackSaveData2 in this.m_callbacksSaveData)
        {
          if (callbackSaveData2.Owner == callbackSaveData1.Owner && callbackSaveData2.MethodName == callbackSaveData1.MethodName)
          {
            Log.Error("Event method '" + callbackSaveData1.MethodName + "' on type '" + typeof (TOwner).Name + "' is already registered.");
            return;
          }
        }
        this.Callbacks.Add(new EventBase<TAction>.CallbackData(callback, (object) owner, name));
        this.m_callbacksSaveData.Add(callbackSaveData1);
      }
    }

    public void Remove<TOwner>(TOwner owner, TAction callback) where TOwner : class
    {
      this.Remove<TOwner>(owner, callback.Method.Name);
    }

    public void Remove<TOwner>(TOwner owner, string methodName) where TOwner : class
    {
      LystMutableDuringIter<EventBase<TAction>.CallbackData> callbacks = this.Callbacks;
      int index1 = -1;
      for (int index2 = 0; index2 < callbacks.Count; ++index2)
      {
        EventBase<TAction>.CallbackData callbackData = callbacks[index2];
        if (callbackData.Owner == (object) owner && callbackData.MethodName == methodName)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 < 0)
      {
        Log.Error("Failed to unregister event '" + methodName + "' on type '" + typeof (TOwner).Name + "'.");
      }
      else
      {
        Assert.That<int>(callbacks.Count).IsGreaterOrEqual(this.m_callbacksSaveData.Count);
        EventBase<TAction>.CallbackData callbackData = callbacks[index1];
        callbacks.RemoveAt(index1);
        int index3 = -1;
        for (int index4 = 0; index4 < this.m_callbacksSaveData.Count; ++index4)
        {
          EventBase<TAction>.CallbackSaveData callbackSaveData = this.m_callbacksSaveData[index4];
          if (callbackSaveData.Owner == (object) owner && callbackSaveData.MethodName == methodName)
          {
            index3 = index4;
            break;
          }
        }
        if (index3 < 0)
          Log.Error("Failed to remove save data for event '" + callbackData.MethodName + "' on type '" + typeof (TOwner).Name + "'.");
        else
          this.m_callbacksSaveData.RemoveAt(index3);
      }
    }

    public bool IsAdded<TOwner>(TOwner owner, TAction callback) where TOwner : class
    {
      return this.IsAdded<TOwner>(owner, callback.Method.Name);
    }

    public bool IsAdded<TOwner>(TOwner owner, string methodName) where TOwner : class
    {
      return this.Callbacks.Contains<EventBase<TAction>.CallbackData>((Predicate<EventBase<TAction>.CallbackData>) (x => x.Owner == (object) (TOwner) owner && x.MethodName == methodName));
    }

    protected abstract bool TryLoadCallback(
      EventBase<TAction>.CallbackSaveData data,
      out TAction callback);

    protected void RecordExceptionForCallbackAt(TAction callback)
    {
      if (this.m_exceptionCounters == null)
        this.m_exceptionCounters = new Dict<TAction, int>();
      if (this.m_exceptionCounters.IncOrInsert1<TAction>(callback) <= 100)
        return;
      int index = this.Callbacks.FindIndex((Predicate<EventBase<TAction>.CallbackData>) (x => (Delegate) x.Callback == (Delegate) callback));
      if (index < 0)
      {
        Log.Error("Failed to find callback to remove.");
      }
      else
      {
        EventBase<TAction>.CallbackData callback1 = this.Callbacks[index];
        this.Callbacks.RemoveAt(index);
        this.m_exceptionCounters.Remove(callback);
        Log.Error(string.Format("Too many exceptions by '{0}' from object '{1}'.", (object) callback1.MethodName, callback1.Owner));
      }
    }

    public virtual void ClearAll()
    {
      this.Callbacks.Clear();
      this.m_callbacksSaveData.Clear();
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<EventBase<TAction>.CallbackSaveData>.Serialize(this.m_callbacksSaveData, writer);
      writer.WriteInt((int) this.PrimaryThread);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<EventBase<TAction>>(this, "m_callbacksSaveData", (object) Lyst<EventBase<TAction>.CallbackSaveData>.Deserialize(reader));
      reader.SetField<EventBase<TAction>>(this, "PrimaryThread", (object) (ThreadType) reader.ReadInt());
    }

    private readonly struct RegisteredCallback : IEquatable<EventBase<TAction>.RegisteredCallback>
    {
      public readonly object Owner;
      public readonly MethodInfo Method;

      public bool IsNone => this.Owner == null && this.Method == (MethodInfo) null;

      public RegisteredCallback(object owner, MethodInfo method)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Owner = owner;
        this.Method = method;
      }

      public bool Equals(EventBase<TAction>.RegisteredCallback other)
      {
        return this.Owner == other.Owner && this.Method == other.Method;
      }

      public override bool Equals(object obj)
      {
        return obj is EventBase<TAction>.RegisteredCallback other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return Hash.Combine<object, MethodInfo>(this.Owner, this.Method);
      }
    }

    public readonly struct CallbackData
    {
      public readonly TAction Callback;
      public readonly object Owner;
      public readonly string MethodName;

      public CallbackData(TAction callback, object owner, string methodName)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Callback = callback;
        this.Owner = owner;
        this.MethodName = methodName;
      }
    }

    [GenerateSerializer(false, null, 0)]
    public readonly struct CallbackSaveData
    {
      public readonly object Owner;
      public readonly Type DeclaringType;
      public readonly string MethodName;

      public CallbackSaveData(object owner, Type declaringType, string methodName)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.Owner = owner;
        this.DeclaringType = declaringType;
        this.MethodName = methodName;
      }

      public bool TryResolveMethodInfo(out MethodInfo mi)
      {
        try
        {
          mi = this.DeclaringType.GetMethod(this.MethodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
          return mi != (MethodInfo) null;
        }
        catch (AmbiguousMatchException ex)
        {
          Log.Error("Overloaded event methods are not supported: " + this.MethodName);
          mi = (MethodInfo) null;
          return false;
        }
      }

      public static void Serialize(EventBase<TAction>.CallbackSaveData value, BlobWriter writer)
      {
        writer.WriteGeneric<object>(value.Owner);
        writer.WriteGeneric<Type>(value.DeclaringType);
        writer.WriteString(value.MethodName);
      }

      public static EventBase<TAction>.CallbackSaveData Deserialize(BlobReader reader)
      {
        return new EventBase<TAction>.CallbackSaveData(reader.ReadGenericAs<object>(), reader.ReadGenericAs<Type>(), reader.ReadString());
      }
    }
  }
}
