// Decompiled with JetBrains decompiler
// Type: Mafi.LazyResolve`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using Mafi.Serialization.Generators;
using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Simple implementation of a value that will be fetched on the first access and cached. This class is thread safe.
  /// </summary>
  [GenerateSerializer(true, null, 0)]
  public sealed class LazyResolve<T> where T : class
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private Option<IResolver> m_resolver;
    private Option<T> m_value;

    public static void Serialize(LazyResolve<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LazyResolve<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LazyResolve<T>.s_serializeDataDelayedAction);
    }

    public static LazyResolve<T> Deserialize(BlobReader reader)
    {
      LazyResolve<T> lazyResolve;
      if (reader.TryStartClassDeserialization<LazyResolve<T>>(out lazyResolve))
        reader.EnqueueDataDeserialization((object) lazyResolve, LazyResolve<T>.s_deserializeDataDelayedAction);
      return lazyResolve;
    }

    /// <summary>Creates a lazy instance directly from a value.</summary>
    public LazyResolve(T value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_value = (Option<T>) value.CheckNotNull<T>();
    }

    /// <summary>
    /// Creates a lazy instance from a function that when called returns value. This function is called at most once.
    /// </summary>
    public LazyResolve(IResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = Option.Some<IResolver>(resolver);
    }

    /// <summary>Use `SetValue` to set value later.</summary>
    internal LazyResolve()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool IsValueCreated => this.m_value.HasValue;

    public T Value
    {
      get
      {
        if (this.m_resolver.HasValue && this.m_resolver.HasValue)
        {
          this.m_value = (Option<T>) this.m_resolver.Value.Resolve<T>();
          this.m_resolver = Option<IResolver>.None;
        }
        return this.m_value.Value;
      }
    }

    internal void SetValue(T value)
    {
      this.m_value = value.SomeOption<T>();
      this.m_resolver = Option<IResolver>.None;
    }

    public LazyResolve<T> Clone() => new LazyResolve<T>(this.m_resolver.Value);

    /// <summary>
    /// Wraps given value in Lazy wrapper. This is mostly convenient in tests when we usually already have the values
    /// ready and do not need to use the lazy semantics.
    /// </summary>
    public static implicit operator LazyResolve<T>(T value) => new LazyResolve<T>(value);

    private void SerializeData(BlobWriter writer)
    {
      bool hasValue = this.m_value.HasValue;
      writer.WriteBool(hasValue);
      if (hasValue)
      {
        bool flag = SerializerGenerator.IsNonSerializedGlobalDep(this.m_value.Value.GetType(), out GlobalDependencyAttribute _);
        writer.WriteBool(flag);
        if (flag)
          return;
        writer.WriteGeneric<T>(this.m_value.Value);
      }
      else
        writer.WriteGeneric<IResolver>(this.m_resolver.Value);
    }

    private void DeserializeData(BlobReader reader)
    {
      if (reader.ReadBool())
      {
        if (reader.ReadBool())
          reader.RegisterResolvedMember<LazyResolve<T>>(this, "m_value", typeof (T), true, (Func<object, object>) (x => (object) ((T) x).SomeOption<T>()));
        else
          this.m_value = reader.ReadGenericAs<T>().SomeOption<T>();
      }
      else
        this.m_resolver = reader.ReadGenericAs<IResolver>().SomeOption<IResolver>();
    }

    static LazyResolve()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LazyResolve<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((LazyResolve<T>) obj).SerializeData(writer));
      LazyResolve<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((LazyResolve<T>) obj).DeserializeData(reader));
    }
  }
}
