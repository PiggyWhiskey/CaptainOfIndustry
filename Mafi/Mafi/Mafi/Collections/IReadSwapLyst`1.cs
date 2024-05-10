// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.IReadSwapLyst`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi.Collections
{
  public interface IReadSwapLyst<T> : ICollectionWithCount
  {
    Lyst<T>.Enumerator GetEnumerator();

    /// <summary>Performs given action on every item in the read list.</summary>
    void ForEach(Action<T> action);
  }
}
