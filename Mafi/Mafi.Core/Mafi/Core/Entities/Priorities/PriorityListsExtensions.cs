// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Priorities.PriorityListsExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Priorities
{
  public static class PriorityListsExtensions
  {
    public static void PriorityListInsertSorted<T>(
      this Lyst<T> list,
      T item,
      Func<T, int> priorityProvider = null)
      where T : IComparable<T>
    {
      if (list.IsEmpty)
      {
        list.Add(item);
      }
      else
      {
        int index = list.BinarySearch(item);
        if (index < 0)
        {
          index = ~index;
        }
        else
        {
          Comparer<T> comparer = Comparer<T>.Default;
          while (index < list.Count && comparer.Compare(list[index], item) == 0)
            ++index;
        }
        list.Insert(index, item);
      }
    }
  }
}
