// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.SaveHeaders
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Mafi.Core.SaveGame
{
  /// <summary>
  /// Constants used in save file to determine its chunks and guard correctness.
  /// Note: Due to perf, fields are mutable arrays, make sure these are NEVER mutated!
  /// </summary>
  internal static class SaveHeaders
  {
    /// <summary>Marks the total beginning of the save file.</summary>
    public static readonly byte[] HEADER_MAIN;
    public static readonly ulong HEADER_MAIN_LE;
    /// <summary>Marks a chunk that contains mod types.</summary>
    public static readonly byte[] HEADER_MOD_TYPES;
    public static readonly ulong HEADER_MOD_TYPES_LE;
    public static readonly byte[] HEADER_SAVE_INFO;
    public static readonly ulong HEADER_SAVE_INFO_LE;
    /// <summary>Marks a chunk that contains configs.</summary>
    public static readonly byte[] HEADER_CONFIGS;
    public static readonly ulong HEADER_CONFIGS_LE;
    /// <summary>
    /// Marks a chunk that contains data of the game as serialized resolver.
    /// </summary>
    public static readonly byte[] HEADER_RESOLVER;
    public static readonly ulong HEADER_RESOLVER_LE;
    /// <summary>Marks the total end of the save file.</summary>
    public static readonly byte[] HEADER_SAVE_END;
    public static readonly ulong HEADER_SAVE_END_LE;
    /// <summary>Marks beginning of custom save file format.</summary>
    public static readonly byte[] HEADER_CUSTOM_SAVE;
    public static readonly ulong HEADER_CUSTOM_SAVE_LE;
    /// <summary>Marks beginning of uber blueprint.</summary>
    public static readonly byte[] HEADER_UBER_BLUEPRINT;
    public static readonly ulong HEADER_UBER_BLUEPRINT_LE;

    [OnlyForSaveCompatibility(null)]
    private static byte[] createHeader(string str)
    {
      byte[] header = str.Length == 8 ? Encoding.ASCII.GetBytes(str) : throw new Exception("Header must be 8 chars long.");
      if (header.Length != 8)
        throw new Exception("Header must be 8 ASCII chars.");
      Array.Reverse((Array) header);
      return header;
    }

    public static byte[] CreateHeaderV2(string str)
    {
      byte[] numArray = str.Length == 8 ? Encoding.ASCII.GetBytes(str) : throw new Exception("Header must be 8 chars long.");
      return numArray.Length == 8 ? numArray : throw new Exception("Header must be 8 ASCII chars.");
    }

    public static ulong HeaderToUlong(byte[] header) => BitConverter.ToUInt64(header, 0);

    public static bool AreHeadersMatching(byte[] actual, byte[] expected)
    {
      if (actual.Length != expected.Length)
        return false;
      for (int index = 0; index < actual.Length; ++index)
      {
        if ((int) actual[index] != (int) expected[index])
          return false;
      }
      return true;
    }

    public static string HeaderToAscii(byte[] header)
    {
      return header != null ? Encoding.ASCII.GetString(header) : "--null--";
    }

    public static string HeaderToAscii(ulong value)
    {
      return Encoding.ASCII.GetString(BitConverter.GetBytes(value));
    }

    public static string HeaderToStringInclHex(ulong value)
    {
      return string.Format("{0} (0x{1:X16})", (object) Encoding.ASCII.GetString(BitConverter.GetBytes(value)), (object) value);
    }

    public static bool TryReadHeader(Stream stream, out ulong header)
    {
      byte[] numArray = ArrayPool<byte>.Get(8);
      int num = stream.Read(numArray, 0, 8);
      header = BitConverter.ToUInt64(numArray, 0);
      numArray.ReturnToPool<byte>();
      return num == 8;
    }

    static SaveHeaders()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SaveHeaders.HEADER_MAIN = SaveHeaders.createHeader("MaFiSave");
      SaveHeaders.HEADER_MAIN_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_MAIN);
      SaveHeaders.HEADER_MOD_TYPES = SaveHeaders.createHeader("ModTypes");
      SaveHeaders.HEADER_MOD_TYPES_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_MOD_TYPES);
      SaveHeaders.HEADER_SAVE_INFO = SaveHeaders.createHeader("SaveInfo");
      SaveHeaders.HEADER_SAVE_INFO_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_SAVE_INFO);
      SaveHeaders.HEADER_CONFIGS = SaveHeaders.createHeader("GlobConf");
      SaveHeaders.HEADER_CONFIGS_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_CONFIGS);
      SaveHeaders.HEADER_RESOLVER = SaveHeaders.createHeader("Resolver");
      SaveHeaders.HEADER_RESOLVER_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_RESOLVER);
      SaveHeaders.HEADER_SAVE_END = SaveHeaders.createHeader("SaveStop");
      SaveHeaders.HEADER_SAVE_END_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_SAVE_END);
      SaveHeaders.HEADER_CUSTOM_SAVE = SaveHeaders.createHeader("MaFiTxt\n");
      SaveHeaders.HEADER_CUSTOM_SAVE_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_CUSTOM_SAVE);
      SaveHeaders.HEADER_UBER_BLUEPRINT = SaveHeaders.createHeader("MaFiUber");
      SaveHeaders.HEADER_UBER_BLUEPRINT_LE = SaveHeaders.HeaderToUlong(SaveHeaders.HEADER_UBER_BLUEPRINT);
    }
  }
}
