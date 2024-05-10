// Decompiled with JetBrains decompiler
// Type: Mafi.Localization.LocAsserts
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Localization
{
  internal static class LocAsserts
  {
    private static readonly object[][] DUMMY_DATA;

    internal static bool VerifyParams(string id, string sourceString, int paramsCount)
    {
      for (int index = 0; index < paramsCount; ++index)
      {
        if (!sourceString.Contains(string.Format("{{{0}}}", (object) index)))
        {
          LocalizationManager.RecordTranslationError(id, string.Format("Translated string does not contain '{{{0}}}' argument: {1}", (object) index, (object) sourceString));
          return false;
        }
      }
      try
      {
        string.Format(sourceString, LocAsserts.DUMMY_DATA[paramsCount]);
      }
      catch (FormatException ex)
      {
        LocalizationManager.RecordTranslationError(id, string.Format("Formatting of string with {0} parameters failed: {1}, ", (object) paramsCount, (object) ex.Message) + "string: " + sourceString);
        return false;
      }
      return true;
    }

    static LocAsserts()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      LocAsserts.DUMMY_DATA = new object[6][]
      {
        new object[0],
        new object[1]{ (object) "1" },
        new object[2]{ (object) "1", (object) "2" },
        new object[3]{ (object) "1", (object) "2", (object) "3" },
        new object[4]
        {
          (object) "1",
          (object) "2",
          (object) "3",
          (object) "4"
        },
        new object[5]
        {
          (object) "1",
          (object) "2",
          (object) "3",
          (object) "4",
          (object) "5"
        }
      };
    }
  }
}
