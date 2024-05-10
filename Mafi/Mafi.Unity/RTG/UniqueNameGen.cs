// Decompiled with JetBrains decompiler
// Type: RTG.UniqueNameGen
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;

#nullable disable
namespace RTG
{
  public static class UniqueNameGen
  {
    public static string Generate(string desiredName, IEnumerable<string> existingNames)
    {
      string str = desiredName;
      int num = 0;
      bool flag;
      do
      {
        flag = false;
        foreach (string existingName in existingNames)
        {
          if (existingName == str)
          {
            str = desiredName + num.ToString();
            ++num;
            flag = true;
            break;
          }
        }
      }
      while (flag && num != int.MaxValue);
      return str;
    }
  }
}
