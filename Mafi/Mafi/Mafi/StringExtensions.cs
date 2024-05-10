// Decompiled with JetBrains decompiler
// Type: Mafi.StringExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Mafi
{
  public static class StringExtensions
  {
    private static readonly Regex s_cleanId;
    private static readonly Regex s_splitCamelCase;

    public static string FormatInvariant(this string format, params object[] args)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args);
    }

    /// <summary>
    /// Whether string is empty (more efficient than <see cref="!:LinqExtensions.IsEmpty&lt;T&gt;" />
    /// </summary>
    public static bool IsEmpty(this string value) => value.Length == 0;

    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

    public static Option<string> NoneIfNullOrWhiteSpace(this string value)
    {
      return !string.IsNullOrWhiteSpace(value) ? (Option<string>) value : Option<string>.None;
    }

    public static bool IsNotEmpty(this string value) => value.Length > 0;

    /// <summary>
    /// Whether this string contains only printable ASCII characters (chars from ' ' to '~').
    /// </summary>
    public static bool IsPrintableAscii(this string value)
    {
      return value.All<char>((Func<char, bool>) (c => c >= ' ' && c <= '~'));
    }

    public static string WithoutPrefix(this string str, int length) => str.Substring(length);

    public static string WithoutSuffix(this string str, int length)
    {
      return str.Substring(0, str.Length - length);
    }

    public static string JoinStrings(this IEnumerable<string> strings, string separator = "")
    {
      return string.Join(separator, strings);
    }

    /// <summary>
    /// Replaces chars that are not a-A, 0-9, `_` or `-` with `_`.
    /// </summary>
    public static string ToCleanId(this string str) => StringExtensions.s_cleanId.Replace(str, "_");

    /// <summary>
    /// Splits "Some123CamelCASEString" to ["Some", "123", "Camel", "CASE", "String"].
    /// </summary>
    public static IEnumerable<string> SplitCamelCase(this string str)
    {
      return ((IEnumerable<string>) StringExtensions.s_splitCamelCase.Split(str)).Where<string>((Func<string, bool>) (x => x.Length > 0));
    }

    /// <summary>
    /// Converts "Some123CamelCASEString" to "Some 123 camel cASE string".
    /// </summary>
    public static string CamelCaseToSpacedSentenceCase(this string str)
    {
      Lyst<string> lyst = str.SplitCamelCase().ToLyst<string>();
      StringBuilder stringBuilder = new StringBuilder(str.Length + lyst.Count - 1);
      foreach (string str1 in lyst)
      {
        Assert.That<int>(str1.Length).IsPositive();
        if (stringBuilder.Length == 0)
        {
          stringBuilder.Append(str1);
        }
        else
        {
          stringBuilder.Append(" ");
          stringBuilder.Append(char.ToLowerInvariant(str1[0]));
          if (str1.Length > 1)
            stringBuilder.Append(str1.Substring(1));
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Converts "Some123CamelCASEString" to "some 123 camel cASE string".
    /// </summary>
    public static string CamelCaseToSpacedWords(this string str)
    {
      Lyst<string> lyst = str.SplitCamelCase().ToLyst<string>();
      StringBuilder stringBuilder = new StringBuilder(str.Length + lyst.Count - 1);
      foreach (string str1 in lyst)
      {
        Assert.That<int>(str1.Length).IsPositive();
        if (stringBuilder.Length > 0)
          stringBuilder.Append(" ");
        stringBuilder.Append(char.ToLowerInvariant(str1[0]));
        if (str1.Length > 1)
          stringBuilder.Append(str1.Substring(1));
      }
      return stringBuilder.ToString();
    }

    public static string CapitalizeFirstChar(this string str)
    {
      return string.IsNullOrEmpty(str) ? str : char.ToUpperInvariant(str[0]).ToString() + str.Substring(1);
    }

    public static string LowerFirstChar(this string str)
    {
      return string.IsNullOrEmpty(str) ? str : char.ToLowerInvariant(str[0]).ToString() + str.Substring(1);
    }

    public static bool Contains(this string str, string other, StringComparison comparison)
    {
      return str.IndexOf(other, comparison) >= 0;
    }

    public static bool SubstringEqualOrdinal(
      this string str,
      string substring,
      int positionOfSubstring)
    {
      return string.Compare(str, positionOfSubstring, substring, 0, substring.Length, StringComparison.Ordinal) == 0;
    }

    public static string RepeatString(this string str, int count)
    {
      if (count <= 0)
      {
        Assert.That<int>(count).IsNotNegative();
        return "";
      }
      StringBuilder stringBuilder = new StringBuilder(str.Length * count);
      for (int index = 0; index < count; ++index)
        stringBuilder.Append(str);
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns the first line. If the given string is null, returns an empty string (returned value is never null).
    /// </summary>
    public static string FirstLine(this string str)
    {
      if (str == null)
        return "";
      for (int index = 0; index < str.Length; ++index)
      {
        if (str[index] == '\r' || str[index] == '\n')
          return str.Substring(0, index);
      }
      return str;
    }

    public static string FirstLines(this string str, int linesCount)
    {
      if (str == null || linesCount <= 0)
        return "";
      for (int index = 0; index < str.Length; ++index)
      {
        if (str[index] == '\n')
        {
          --linesCount;
          if (linesCount == 0)
          {
            if (index > 0 && str[index - 1] == '\r')
              --index;
            return str.Substring(0, index);
          }
        }
      }
      return str;
    }

    /// <summary>
    /// A safe version of substring that is smart by automatically adjusting limits so that it throws.
    /// </summary>
    public static string SubstringSafe(this string str, int startIndex, int? length = null)
    {
      if (string.IsNullOrEmpty(str) || startIndex >= str.Length)
        return "";
      int? nullable = length;
      int num = 0;
      if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
        return "";
      if (startIndex < 0)
        startIndex = 0;
      int length1 = !length.HasValue || startIndex + length.Value > str.Length ? str.Length - startIndex : length.Value;
      return str.Substring(startIndex, length1);
    }

    public static string RemoveSuffixIfExists(this string str, string suffix)
    {
      return !str.EndsWith(suffix, StringComparison.Ordinal) ? str : str.Substring(0, str.Length - suffix.Length);
    }

    /// <summary>Splits string to lines, handles any line endings.</summary>
    public static Lyst<string> SplitToLines(this string str)
    {
      Lyst<string> outLines = new Lyst<string>();
      str.SplitToLines(outLines);
      return outLines;
    }

    /// <summary>Splits string to lines, handles any line endings.</summary>
    public static int SplitToLines(this string str, Lyst<string> outLines)
    {
      int count = outLines.Count;
      int startIndex = 0;
      for (int index = 0; index < str.Length; ++index)
      {
        switch (str[index])
        {
          case '\n':
            string str1 = str.Substring(startIndex, index - startIndex);
            startIndex = index + 1;
            outLines.Add(str1);
            break;
          case '\r':
            string str2 = str.Substring(startIndex, index - startIndex);
            if (index + 1 < str.Length && str[index + 1] == '\n')
              ++index;
            startIndex = index + 1;
            outLines.Add(str2);
            break;
        }
      }
      if (startIndex != str.Length)
        outLines.Add(str.Substring(startIndex));
      else
        outLines.Add("");
      return outLines.Count - count;
    }

    static StringExtensions()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      StringExtensions.s_cleanId = new Regex("[^a-zA-Z0-9-_]", RegexOptions.Compiled);
      StringExtensions.s_splitCamelCase = new Regex("([A-Z]+(?=$|[A-Z][a-z]|[0-9])|[A-Z]?[a-z]+|[0-9]+)", RegexOptions.Compiled);
    }
  }
}
