// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.Crc32
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.IO;

#nullable disable
namespace Mafi.Serialization
{
  public static class Crc32
  {
    private static uint[] s_table;

    public static uint Compute(byte[] input) => Crc32.Append(0U, input, 0, input.Length);

    public static uint ComputeSlow(byte[] input) => Crc32.AppendSlow(0U, input, 0, input.Length);

    public static uint Compute(Stream input, out long readBytes)
    {
      return Crc32.Append(0U, input, out readBytes);
    }

    public static uint Append(uint initial, byte[] input)
    {
      return Crc32.Append(initial, input, 0, input.Length);
    }

    public static uint AppendSlow(uint initial, byte[] input, int offset, int length)
    {
      uint num = uint.MaxValue ^ initial;
      uint[] numArray = Crc32.s_table;
      if (numArray == null)
        Crc32.s_table = numArray = Crc32.initTable(3988292384U);
      while (--length >= 0)
        num = numArray[(int) (byte) ((int) num ^ (int) input[offset++])] ^ num >> 8;
      return num ^ uint.MaxValue;
    }

    public static uint Append(uint initial, byte[] input, int offset, int length)
    {
      uint num1 = uint.MaxValue ^ initial;
      uint[] numArray = Crc32.s_table;
      if (numArray == null)
        Crc32.s_table = numArray = Crc32.initTable(3988292384U);
      for (; length >= 16; length -= 16)
      {
        uint num2 = numArray[768 + (int) input[offset + 12]] ^ numArray[512 + (int) input[offset + 13]] ^ numArray[256 + (int) input[offset + 14]] ^ numArray[(int) input[offset + 15]];
        uint num3 = numArray[1792 + (int) input[offset + 8]] ^ numArray[1536 + (int) input[offset + 9]] ^ numArray[1280 + (int) input[offset + 10]] ^ numArray[1024 + (int) input[offset + 11]];
        uint num4 = numArray[2816 + (int) input[offset + 4]] ^ numArray[2560 + (int) input[offset + 5]] ^ numArray[2304 + (int) input[offset + 6]] ^ numArray[2048 + (int) input[offset + 7]];
        num1 = numArray[3840 + ((int) (byte) num1 ^ (int) input[offset])] ^ numArray[3584 + ((int) (byte) (num1 >> 8) ^ (int) input[offset + 1])] ^ numArray[3328 + ((int) (byte) (num1 >> 16) ^ (int) input[offset + 2])] ^ numArray[3072 + ((int) (num1 >> 24) ^ (int) input[offset + 3])] ^ num4 ^ num3 ^ num2;
        offset += 16;
      }
      while (--length >= 0)
        num1 = numArray[(int) (byte) ((int) num1 ^ (int) input[offset++])] ^ num1 >> 8;
      return num1 ^ uint.MaxValue;
    }

    public static uint Append(uint initial, Stream input, out long readBytes)
    {
      uint num1 = uint.MaxValue ^ initial;
      uint[] numArray = Crc32.s_table;
      if (numArray == null)
        Crc32.s_table = numArray = Crc32.initTable(3988292384U);
      byte[] buffer = new byte[16];
      readBytes = 0L;
      int num2;
      while (true)
      {
        num2 = input.Read(buffer, 0, 16);
        readBytes += (long) num2;
        if (num2 >= 16)
        {
          uint num3 = numArray[768 + (int) buffer[12]] ^ numArray[512 + (int) buffer[13]] ^ numArray[256 + (int) buffer[14]] ^ numArray[(int) buffer[15]];
          uint num4 = numArray[1792 + (int) buffer[8]] ^ numArray[1536 + (int) buffer[9]] ^ numArray[1280 + (int) buffer[10]] ^ numArray[1024 + (int) buffer[11]];
          uint num5 = numArray[2816 + (int) buffer[4]] ^ numArray[2560 + (int) buffer[5]] ^ numArray[2304 + (int) buffer[6]] ^ numArray[2048 + (int) buffer[7]];
          num1 = numArray[3840 + ((int) (byte) num1 ^ (int) buffer[0])] ^ numArray[3584 + ((int) (byte) (num1 >> 8) ^ (int) buffer[1])] ^ numArray[3328 + ((int) (byte) (num1 >> 16) ^ (int) buffer[2])] ^ numArray[3072 + ((int) (num1 >> 24) ^ (int) buffer[3])] ^ num5 ^ num4 ^ num3;
        }
        else
          break;
      }
      for (int index = 0; index < num2; ++index)
        num1 = numArray[(int) (byte) (num1 ^ (uint) buffer[index])] ^ num1 >> 8;
      return num1 ^ uint.MaxValue;
    }

    private static uint[] initTable(uint poly)
    {
      uint[] numArray = new uint[4096];
      for (uint index1 = 0; index1 < 256U; ++index1)
      {
        uint num = index1;
        for (int index2 = 0; index2 < 16; ++index2)
        {
          for (int index3 = 0; index3 < 8; ++index3)
            num = ((int) num & 1) == 1 ? poly ^ num >> 1 : num >> 1;
          numArray[(long) (index2 * 256) + (long) index1] = num;
        }
      }
      return numArray;
    }
  }
}
