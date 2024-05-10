﻿// Decompiled with JetBrains decompiler
// Type: Mafi.SetExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Collections;
using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class SetExtensions
  {
    public static bool Overlaps<T>(this IReadOnlySet<T> set, IEnumerable<T> other)
    {
      if (set.Count == 0)
        return false;
      foreach (T obj in other)
      {
        if (set.Contains(obj))
          return true;
      }
      return false;
    }
  }
}
