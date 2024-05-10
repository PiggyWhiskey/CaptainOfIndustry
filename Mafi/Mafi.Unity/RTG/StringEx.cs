// Decompiled with JetBrains decompiler
// Type: RTG.StringEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace RTG
{
  public static class StringEx
  {
    public static string RemoveTrailingSlashes(this string str)
    {
      string str1 = str;
      while (str1.LastChar() == '\\' || str1.LastChar() == '/')
        str1 = str1.Substring(0, str1.LastCharIndex());
      return str1;
    }

    public static char LastChar(this string str) => str[str.LastCharIndex()];

    public static int LastCharIndex(this string str) => str.Length - 1;

    public static bool ContainsOnlyWhiteSpace(this string str)
    {
      for (int index = 0; index < str.Length; ++index)
      {
        if (!char.IsWhiteSpace(str[index]))
          return false;
      }
      return true;
    }

    public static bool IsSingleDigit(this string str) => str.Length == 1 && char.IsDigit(str[0]);

    public static bool IsSingleLetter(this string str) => str.Length == 1 && char.IsLetter(str[0]);

    public static bool IsSingleChar(this string str, char character)
    {
      return str.Length == 1 && (int) str[0] == (int) character;
    }
  }
}
