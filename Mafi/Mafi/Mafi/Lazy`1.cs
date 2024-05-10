// Decompiled with JetBrains decompiler
// Type: Mafi.Lazy`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Simple implementation of a value that will be fetched on the first access and cached. This class is thread safe.
  /// </summary>
  public class Lazy<T>
  {
    private Func<T> m_factoryFunc;
    private T m_value;

    /// <summary>Creates a lazy instance directly from a value.</summary>
    private Lazy(T value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_value = value;
    }

    /// <summary>
    /// Creates a lazy instance from a function that when called returns value. This function is called at most once.
    /// </summary>
    public Lazy(Func<T> valueFactory)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_factoryFunc = valueFactory.CheckNotNull<Func<T>>();
    }

    public bool IsValueCreated => this.m_factoryFunc == null;

    public T Value
    {
      get
      {
        if (this.m_factoryFunc != null)
        {
          Func<T> factoryFunc = this.m_factoryFunc;
          if (factoryFunc != null)
          {
            this.m_value = factoryFunc();
            this.m_factoryFunc = (Func<T>) null;
          }
        }
        return this.m_value;
      }
    }

    /// <summary>
    /// Wraps given value in Lazy wrapper. This is mostly convenient in tests when we usually already have the values
    /// ready and do not need to use the lazy semantics.
    /// </summary>
    public static implicit operator Lazy<T>(T value) => new Lazy<T>(value);

    public void EnsureValueCreated() => this.m_value = this.Value;
  }
}
