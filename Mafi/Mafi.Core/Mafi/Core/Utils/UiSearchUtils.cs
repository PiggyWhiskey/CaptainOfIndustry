// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.UiSearchUtils
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  public static class UiSearchUtils
  {
    private static readonly char[] SEARCH_QUERY_SEPARATOR;

    /// <summary>
    /// Can match in the following ways:
    /// 1) e.g. query: 'B' or 'BF' (matches: Blast Furnace)
    /// 2) otherwise just searches for each query word as a substring as long as
    /// the query is longer than 1 character.
    /// </summary>
    public static void MatchProtos<T>(string query, IIndexable<T> protos, Set<T> result) where T : Proto
    {
      UiSearchUtils.MatchItems<T>(query, protos, (Func<T, string>) (p => p.Strings.Name.TranslatedString), result);
    }

    public static void MatchItems<T>(
      string query,
      IIndexable<T> items,
      Func<T, string> itemToString,
      Set<T> result)
    {
      result.Clear();
      if (query == null || query.IsEmpty())
      {
        result.AddRange(items.AsEnumerable());
      }
      else
      {
        string[] qWords = query.Trim().ToLower(LocalizationManager.CurrentCultureInfo).Split(UiSearchUtils.SEARCH_QUERY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
        foreach (T obj in items)
        {
          if (matches(itemToString(obj)))
            result.Add(obj);
        }

        bool matches(string itemName)
        {
          if (qWords.Length == 0)
            return false;
          string[] protoWords = itemName.ToLower(LocalizationManager.CurrentCultureInfo).Split(UiSearchUtils.SEARCH_QUERY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
          if (isFirstLettersMatch(protoWords))
            return true;
          if (qWords.Length == 1 && qWords[0].Length <= 1)
            return false;
          foreach (string qWord in qWords)
          {
            bool flag = false;
            foreach (string str in protoWords)
            {
              if (str.Contains(qWord))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              return false;
          }
          return true;
        }

        bool isFirstLettersMatch(string[] protoWords)
        {
          if (qWords.Length != 1 || qWords[0].IsEmpty())
            return false;
          string str = qWords.First<string>();
          for (int index = 0; index < str.Length; ++index)
          {
            if (index >= protoWords.Length || (int) str[index] != (int) protoWords[index][0])
              return false;
          }
          return true;
        }
      }
    }

    public static bool Matches(string textToSearchIn, string[] query)
    {
      foreach (string other in query)
      {
        if (!textToSearchIn.Contains(other, StringComparison.CurrentCultureIgnoreCase))
          return false;
      }
      return true;
    }

    static UiSearchUtils()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UiSearchUtils.SEARCH_QUERY_SEPARATOR = new char[1]
      {
        ' '
      };
    }
  }
}
