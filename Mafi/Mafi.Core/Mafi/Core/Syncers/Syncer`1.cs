// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.Syncer`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Syncers
{
  public sealed class Syncer<T> : ISyncer<T>, ISyncer
  {
    private static IEqualityComparer<T> s_comparer;
    private T m_value;
    private readonly Func<T> m_provider;

    public bool HasChanged { get; private set; }

    public Syncer(Func<T> provider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_provider = provider.CheckNotNull<Func<T>>();
      if (Syncer<T>.s_comparer != null)
        return;
      Syncer<T>.s_comparer = (IEqualityComparer<T>) EqualityComparer<T>.Default;
    }

    void ISyncer.SyncUpdate(bool invalidate)
    {
      T x = this.m_provider();
      this.HasChanged = ((this.HasChanged ? 1 : 0) | (invalidate ? 1 : (!Syncer<T>.s_comparer.Equals(x, this.m_value) ? 1 : 0))) != 0;
      this.m_value = x;
    }

    public T GetValueAndReset()
    {
      this.HasChanged = false;
      return this.m_value;
    }
  }
}
