// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.BitMap
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Collections
{
  /// <summary>
  /// Efficient bit storage.
  /// 
  /// Note that this class is a struct to avoid an extra pointer hop.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct BitMap
  {
    internal const int BITS_PER_DATA = 64;
    internal const int BITS_PER_DATA_BITS = 6;
    internal const int LOCAL_MASK = 63;
    /// <summary>Number of stored bits.</summary>
    public readonly int Size;
    [SerializeNullAsEmptyArray]
    private readonly ulong[] m_data;

    public ulong[] BackingArray => this.m_data;

    [LoadCtor]
    private BitMap(int size, ulong[] data)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Size = size;
      this.m_data = data;
    }

    public BitMap(int size)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Size = size;
      this.m_data = new ulong[BitMap.getArrayLength(size)];
    }

    public bool IsSet(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex < (uint) data.Length)
        return (data[bitArrayIndex] & BitMap.getBitMask(index)) > 0UL;
      Mafi.Log.Error("IsSet: Invalid index.");
      return false;
    }

    public bool IsNotSet(int index) => !this.IsSet(index);

    public void SetBit(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex >= (uint) data.Length)
        Mafi.Log.Error("SetBit: Invalid index.");
      else
        data[bitArrayIndex] |= BitMap.getBitMask(index);
    }

    public void SetAllBitsAround(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex >= (uint) data.Length)
        Mafi.Log.Error("SetAllBitsAround: Invalid index.");
      else
        data[bitArrayIndex] = ulong.MaxValue;
    }

    /// <summary>
    /// Sets bit and returns true if the bit was previously not set.
    /// </summary>
    public bool SetBitReportChanged(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex >= (uint) data.Length)
      {
        Mafi.Log.Error("SetBitReportChanged: Invalid index.");
        return false;
      }
      ulong num = data[bitArrayIndex];
      ulong bitMask = BitMap.getBitMask(index);
      data[bitArrayIndex] = num | bitMask;
      return ((long) num & (long) bitMask) == 0L;
    }

    public void ClearBit(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex >= (uint) data.Length)
        Mafi.Log.Error("ClearBit: Invalid index.");
      else
        data[bitArrayIndex] &= ~BitMap.getBitMask(index);
    }

    public bool ClearBitReportChanged(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex >= (uint) data.Length)
      {
        Mafi.Log.Error("ClearBit: Invalid index.");
        return false;
      }
      ulong num = data[bitArrayIndex];
      ulong bitMask = BitMap.getBitMask(index);
      data[bitArrayIndex] = num & ~bitMask;
      return (num & bitMask) > 0UL;
    }

    /// <summary>
    /// Clears all bits in the group of 64 bits. This can be used to clear bits more efficiently than
    /// <see cref="M:Mafi.Collections.BitMap.ClearAllBits" /> if we know which bits are set and if the number of bits is significantly smaller
    /// than <see cref="F:Mafi.Collections.BitMap.Size" /> (at least 100x).
    /// </summary>
    public void ClearBitsAround(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex >= (uint) data.Length)
        Mafi.Log.Error("ClearBitsAround: Invalid index.");
      else
        data[bitArrayIndex] = 0UL;
    }

    /// <summary>
    /// Clears at least the given amount of bits, it could clear more or all bits!
    /// </summary>
    public void ClearBitsAtLeast(int bitsCountFromStart)
    {
      Array.Clear((Array) this.m_data, 0, (bitsCountFromStart + 63) / 64);
    }

    /// <summary>
    /// Clears all set bits. This is safe to call on the default (uninitialized) struct instance.
    /// </summary>
    public void ClearAllBits()
    {
      if (this.m_data == null)
        return;
      Array.Clear((Array) this.m_data, 0, this.m_data.Length);
    }

    /// <summary>
    /// More efficient than setter since it does not use an if.
    /// </summary>
    public void InvertBit(int index)
    {
      ulong[] data = this.m_data;
      int bitArrayIndex = BitMap.getBitArrayIndex(index);
      if ((uint) bitArrayIndex >= (uint) data.Length)
        Mafi.Log.Error("InvertBit: Invalid index.");
      else
        data[bitArrayIndex] ^= BitMap.getBitMask(index);
    }

    public void SetAllBits()
    {
      int index = 0;
      for (int length = this.m_data.Length; index < length; ++index)
        this.m_data[index] = ulong.MaxValue;
    }

    public int CountSetBits() => this.CountSetBits(this.Size);

    public int CountSetBits(int bitsCount)
    {
      int index1 = bitsCount / 64;
      int num1 = 0;
      ulong[] data = this.m_data;
      for (int index2 = 0; index2 < index1; ++index2)
        num1 += data[index2].CountSetBits();
      int num2 = bitsCount - (index1 << 6);
      if (num2 > 0)
      {
        ulong num3 = this.m_data[index1];
        num1 += (num3 & (ulong) ((1L << num2) - 1L)).CountSetBits();
      }
      return num1;
    }

    private static int getBitArrayIndex(int i) => i >> 6;

    private static ulong getBitMask(int i) => 1UL << i;

    private static int getArrayLength(int size) => size <= 0 ? 1 : (size - 1) / 64 + 1;

    public BitMap.Enumerator EnumerateSetIndices() => new BitMap.Enumerator(this);

    public static void Serialize(BitMap value, BlobWriter writer)
    {
      writer.WriteInt(value.Size);
      writer.WriteArray<ulong>(value.m_data ?? Array.Empty<ulong>());
    }

    public static BitMap Deserialize(BlobReader reader)
    {
      return new BitMap(reader.ReadInt(), reader.ReadArray<ulong>());
    }

    [DebuggerStepThrough]
    public struct Enumerator
    {
      private readonly BitMap m_bitMap;
      private int m_dataIndex;
      private ulong m_currentData;
      private int m_bitIndex;

      public readonly int Current => this.m_dataIndex << 6 | this.m_bitIndex;

      internal Enumerator(BitMap bitMap)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_bitMap = bitMap;
        this.m_dataIndex = -1;
        this.m_currentData = 0UL;
        this.m_bitIndex = 64;
      }

      public bool MoveNext()
      {
        do
        {
          ++this.m_bitIndex;
          if (this.m_bitIndex >= 64)
            goto label_3;
        }
        while (((long) this.m_currentData & 1L << this.m_bitIndex) == 0L);
        return true;
label_3:
        do
        {
          ++this.m_dataIndex;
          if (this.m_dataIndex >= this.m_bitMap.m_data.Length)
            return false;
          this.m_currentData = this.m_bitMap.m_data[this.m_dataIndex];
        }
        while (this.m_currentData == 0UL);
        this.m_bitIndex = this.m_currentData.GetFirstSetBitIndex();
        return true;
      }

      public readonly BitMap.Enumerator GetEnumerator() => new BitMap.Enumerator(this.m_bitMap);
    }
  }
}
