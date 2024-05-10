// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.IDictNonGeneric
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Interface that allows access to non-generic data of any dict. DO NOT use in perf critical context, all value
  /// types are boxed during enumeration.
  /// </summary>
  internal interface IDictNonGeneric
  {
    int Count { get; }

    int Capacity { get; }

    bool HasDefaultComparer { get; }

    object this[object key] { get; set; }

    IEnumerator<KeyValuePair<object, object>> GetEnumerator();

    void Add(object key, object value);
  }
}
