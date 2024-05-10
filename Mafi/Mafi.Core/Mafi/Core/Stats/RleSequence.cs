// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Stats.RleSequence
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Stats
{
  /// <summary>
  /// Sequence of numbers compressed using RLE (run-length encoding).
  /// This compresses sequence like [0, 0, 0, 1, 1, 2, ...] into [3 x 0, 2 x 1, 1 x 2, ...]
  /// Max stored value is +-2^54 ~ 18,000,000,000,000,000.
  /// </summary>
  /// <remarks>
  /// Values are stored as longs, the high bits are used as (number of repetitions - 1).
  /// WARNING: Mutable struct, do not store in readonly or nullable fields.
  /// </remarks>
  [ManuallyWrittenSerialization]
  public struct RleSequence
  {
    public const int DEFAULT_RLE_CAPACITY = 8;
    public const int MAX_REPS_PER_ENTRY = 512;
    /// <summary>
    /// RLE values. Length of this array must be a power of two to be able to use bit operations instead of modulo.
    /// </summary>
    private Option<ulong[]> m_rleValues;
    /// <summary>Number of stored RLE Values.</summary>
    private int m_rleValuesCount;
    private int m_rleNewestValueIndex;
    private int m_rleOldestValueIndex;

    /// <summary>Number of stored values (uncompressed).</summary>
    public int Count { get; private set; }

    public readonly int CompressedCount => this.m_rleValuesCount;

    public readonly int CompressedCapacity
    {
      get
      {
        ulong[] valueOrNull = this.m_rleValues.ValueOrNull;
        return valueOrNull == null ? 0 : valueOrNull.Length;
      }
    }

    public readonly bool HasAnyNonZeroData
    {
      get
      {
        if (this.m_rleValuesCount > 1)
          return true;
        return this.m_rleValuesCount == 1 && (this.m_rleValues.ValueOrNull[0] & 36028797018963967UL) > 0UL;
      }
    }

    public readonly bool IsAllocated => this.m_rleValues.HasValue;

    public readonly bool IsNotEmpty => this.Count > 0;

    public readonly long NewestValueOrDefault
    {
      get
      {
        return this.m_rleValuesCount <= 0 ? 0L : RleSequence.reconstructValue(this.m_rleValues.ValueOrNull[this.m_rleNewestValueIndex]);
      }
    }

    public void AddValue(long value)
    {
      ulong maskedValue = (ulong) (value & 36028797018963967L);
      ulong[] valueOrNull = this.m_rleValues.ValueOrNull;
      if (this.m_rleValuesCount <= 0)
      {
        if (valueOrNull == null)
        {
          ulong[] numArray;
          this.m_rleValues = (Option<ulong[]>) (numArray = new ulong[8]);
          this.m_rleNewestValueIndex = numArray.Length - 1;
          this.m_rleOldestValueIndex = 0;
        }
        this.appendNew(maskedValue);
      }
      else
      {
        ulong num1 = valueOrNull[this.m_rleNewestValueIndex];
        if (((long) num1 & 36028797018963967L) == (long) maskedValue)
        {
          ulong num2 = num1 >> 55;
          if (num2 < 511UL)
          {
            valueOrNull[this.m_rleNewestValueIndex] = (ulong) ((long) num2 + 1L << 55) | maskedValue;
            ++this.Count;
          }
          else
            this.appendNew(maskedValue);
        }
        else
          this.appendNew(maskedValue);
      }
    }

    public void RemoveOldest()
    {
      if (this.m_rleValuesCount == 0)
      {
        Log.Error("Called `RemoveOldest` on an empty RleSequence.");
      }
      else
      {
        ulong[] valueOrNull = this.m_rleValues.ValueOrNull;
        ulong num1 = valueOrNull[this.m_rleOldestValueIndex];
        ulong num2 = num1 >> 55;
        if (num2 == 0UL)
        {
          this.removeOldest_noChecks();
        }
        else
        {
          valueOrNull[this.m_rleOldestValueIndex] = (ulong) ((long) num2 - 1L << 55 | (long) num1 & 36028797018963967L);
          --this.Count;
        }
      }
    }

    /// <summary>
    /// Returns sum of last n elements. Works directly on compressed data.
    /// </summary>
    public readonly long GetSumOfLastNValues(int count)
    {
      if (this.m_rleValuesCount <= 0)
        return 0;
      ulong[] valueOrNull = this.m_rleValues.ValueOrNull;
      int index = this.m_rleNewestValueIndex;
      int num1 = valueOrNull.Length - 1;
      long sumOfLastNvalues = 0;
      int num2 = 0;
      while (num2 < this.m_rleValuesCount)
      {
        if ((uint) index >= (uint) valueOrNull.Length)
        {
          Log.Error(string.Format("Invalid RLE index {0} for values array of length {1} in GetSumOfLastNValues", (object) index, (object) valueOrNull.Length));
          break;
        }
        ulong rleValue = valueOrNull[index];
        int num3 = (int) (rleValue >> 55) + 1;
        long num4 = RleSequence.reconstructValue(rleValue);
        if (num3 > count)
        {
          sumOfLastNvalues += num4 * (long) count;
          break;
        }
        sumOfLastNvalues += num4 * (long) num3;
        count -= num3;
        if (count > 0)
        {
          ++num2;
          index = index + num1 & num1;
        }
        else
          break;
      }
      return sumOfLastNvalues;
    }

    /// <summary>
    /// Returns max of last n elements. Works directly on compressed data.
    /// </summary>
    public readonly long GetMaxOfLastNValues(int count)
    {
      if (this.m_rleValuesCount <= 0)
        return 0;
      ulong[] valueOrNull = this.m_rleValues.ValueOrNull;
      int index = this.m_rleNewestValueIndex;
      int num1 = valueOrNull.Length - 1;
      long self = long.MinValue;
      int num2 = 0;
      while (num2 < this.m_rleValuesCount)
      {
        if ((uint) index >= (uint) valueOrNull.Length)
        {
          Log.Error(string.Format("Invalid RLE index {0} for values array of length {1} in GetMaxOfLastNValues", (object) index, (object) valueOrNull.Length));
          if (self == long.MinValue)
            return 0;
          break;
        }
        ulong rleValue = valueOrNull[index];
        int num3 = (int) (rleValue >> 55) + 1;
        self = self.Max(RleSequence.reconstructValue(rleValue));
        count -= num3;
        if (count > 0)
        {
          ++num2;
          index = index + num1 & num1;
        }
        else
          break;
      }
      return self;
    }

    /// <summary>
    /// Returns average of last n elements by working directly on compressed data.
    /// </summary>
    public readonly long GetAvgOfLastNValues(int count)
    {
      return this.Count <= 0 ? 0L : this.GetSumOfLastNValues(count) / (long) count.Min(this.Count);
    }

    /// <summary>
    /// Removes oldest without any checks, it's callers responsibility to ensure that this is value.
    /// </summary>
    private void removeOldest_noChecks()
    {
      this.m_rleOldestValueIndex = this.wrapIndex(this.m_rleOldestValueIndex + 1);
      --this.m_rleValuesCount;
      --this.Count;
    }

    private void appendNew(ulong maskedValue)
    {
      ulong[] valueOrNull = this.m_rleValues.ValueOrNull;
      if (this.m_rleValuesCount >= valueOrNull.Length)
      {
        this.doubleCapacity();
        valueOrNull = this.m_rleValues.ValueOrNull;
      }
      this.m_rleNewestValueIndex = this.wrapIndex(this.m_rleNewestValueIndex + 1);
      valueOrNull[this.m_rleNewestValueIndex] = maskedValue;
      ++this.m_rleValuesCount;
      ++this.Count;
    }

    private void doubleCapacity()
    {
      ulong[] valueOrNull = this.m_rleValues.ValueOrNull;
      ulong[] destinationArray = new ulong[valueOrNull.Length << 1];
      if (this.m_rleNewestValueIndex > this.m_rleOldestValueIndex)
      {
        Array.Copy((Array) valueOrNull, this.m_rleOldestValueIndex, (Array) destinationArray, 0, this.m_rleValuesCount);
      }
      else
      {
        int num = valueOrNull.Length - this.m_rleOldestValueIndex;
        Array.Copy((Array) valueOrNull, this.m_rleOldestValueIndex, (Array) destinationArray, 0, num);
        Array.Copy((Array) valueOrNull, 0, (Array) destinationArray, num, this.m_rleNewestValueIndex + 1);
      }
      this.m_rleValues = (Option<ulong[]>) destinationArray;
      this.m_rleOldestValueIndex = 0;
      this.m_rleNewestValueIndex = this.m_rleValuesCount - 1;
    }

    private static long reconstructValue(ulong rleValue)
    {
      long num = (long) rleValue & 36028797018963967L;
      if (((long) rleValue & 18014398509481984L) != 0L)
        num |= -36028797018963968L;
      return num;
    }

    private readonly int wrapIndex(int index)
    {
      int length = this.m_rleValues.ValueOrNull.Length;
      return index + length & length - 1;
    }

    public void CopyFrom(RleSequence source)
    {
      if (source.m_rleValues.HasValue)
        this.m_rleValues = (Option<ulong[]>) source.m_rleValues.Value.CloneArray<ulong>();
      this.Count = source.Count;
      this.m_rleValuesCount = source.m_rleValuesCount;
      this.m_rleNewestValueIndex = source.m_rleNewestValueIndex;
      this.m_rleOldestValueIndex = source.m_rleOldestValueIndex;
    }

    public readonly RleSequence.Enumerator GetEnumerator() => new RleSequence.Enumerator(this);

    [OnlyForSaveCompatibility(null)]
    public static void Serialize(RleSequence value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.m_rleValuesCount);
      if (value.m_rleValuesCount <= 0 || value.m_rleValues.IsNone)
        return;
      ulong[] numArray = value.m_rleValues.Value;
      int num1 = numArray.Length - 1;
      int index = value.m_rleOldestValueIndex;
      int num2 = 0;
      for (int rleValuesCount = value.m_rleValuesCount; num2 < rleValuesCount; ++num2)
      {
        if ((uint) index < (uint) numArray.Length)
        {
          writer.WriteULong(numArray[index]);
        }
        else
        {
          Log.Error(string.Format("Invalid RLE index {0} for values array of length {1} in Serialize", (object) index, (object) numArray.Length));
          writer.WriteULong(0UL);
        }
        index = index + 1 & num1;
      }
    }

    public static RleSequence Deserialize(BlobReader reader)
    {
      int number = reader.ReadIntNotNegative();
      if (number <= 0)
        return new RleSequence();
      RleSequence rleSequence = new RleSequence();
      ulong[] numArray = new ulong[number <= 8 ? 8 : number.CeilToPowerOfTwoOrZero()];
      rleSequence.m_rleValuesCount = number;
      rleSequence.m_rleValues = (Option<ulong[]>) numArray;
      rleSequence.m_rleNewestValueIndex = number - 1;
      for (int index = 0; index < number; ++index)
      {
        ulong num = reader.ReadULong();
        numArray[index] = num;
        rleSequence.Count += (int) (num >> 55) + 1;
      }
      return rleSequence;
    }

    /// <summary>Enumerates from the newest to the oldest.</summary>
    public struct Enumerator
    {
      private readonly RleSequence m_sequence;
      private int m_index;
      private int m_repsRemaining;

      internal Enumerator(RleSequence sequence)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_sequence = sequence;
        this.m_index = sequence.m_rleNewestValueIndex + 1;
        this.m_repsRemaining = -1;
        this.Current = 0L;
      }

      public long Current { get; private set; }

      public bool MoveNext()
      {
        if (this.m_repsRemaining > 0)
        {
          --this.m_repsRemaining;
          return true;
        }
        ulong[] valueOrNull = this.m_sequence.m_rleValues.ValueOrNull;
        if (valueOrNull == null || this.m_index == this.m_sequence.m_rleOldestValueIndex && this.m_repsRemaining == 0)
          return false;
        this.m_index = this.m_index - 1 + valueOrNull.Length & valueOrNull.Length - 1;
        ulong rleValue = valueOrNull[this.m_index];
        this.m_repsRemaining = (int) (rleValue >> 55);
        this.Current = RleSequence.reconstructValue(rleValue);
        return true;
      }
    }
  }
}
