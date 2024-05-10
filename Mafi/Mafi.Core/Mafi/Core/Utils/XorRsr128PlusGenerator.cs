// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.XorRsr128PlusGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Random;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

#nullable disable
namespace Mafi.Core.Utils
{
  /// <summary>
  /// Xor Rotate-shift-rotate pseudo-random generator has 128 bits of state and period of 2^128 - 1. Every 64-bit value
  /// expect zero is guaranteed to appear exactly 2^64 times in this period.The state must be seeded so that it is not
  /// everywhere zero.
  /// </summary>
  /// <remarks>
  /// Written in 2016 by David Blackman and Sebastiano Vigna (vigna@acm.org)
  /// * License: http://creativecommons.org/publicdomain/zero/1.0/
  /// * Original source: http://xoroshiro.di.unimi.it/xoroshiro128plus.c
  /// 
  /// This is the successor to xorshift128+. It is the fastest full-period generator passing BigCrush without
  /// systematic failures, but due to the relatively short period it is acceptable only for applications with a mild
  /// amount of parallelism; otherwise, use a xorshift1024* generator.
  /// 
  /// Beside passing BigCrush, this generator passes the PractRand test suite up to (and included) 16TB, with the
  /// exception of binary rank tests, which fail due to the lowest bit being an LFSR; all other bits pass all tests. We
  /// suggest to use a sign test to extract a random Boolean value.
  /// 
  /// TODO: Improve https://www.pcg-random.org/
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public sealed class XorRsr128PlusGenerator : ICoreRandom, IRandom
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    [ThreadStatic]
    private static MD5 s_md5;
    /// <summary>Whether this generator is affecting sim state.</summary>
    private readonly RandomGeneratorType m_generatorType;
    private ulong m_state0;
    private ulong m_state1;

    public static void Serialize(XorRsr128PlusGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<XorRsr128PlusGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, XorRsr128PlusGenerator.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteInt((int) this.m_generatorType);
      writer.WriteULong(this.m_state0);
      writer.WriteULong(this.m_state1);
    }

    public static XorRsr128PlusGenerator Deserialize(BlobReader reader)
    {
      XorRsr128PlusGenerator rsr128PlusGenerator;
      if (reader.TryStartClassDeserialization<XorRsr128PlusGenerator>(out rsr128PlusGenerator))
        reader.EnqueueDataDeserialization((object) rsr128PlusGenerator, XorRsr128PlusGenerator.s_deserializeDataDelayedAction);
      return rsr128PlusGenerator;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<XorRsr128PlusGenerator>(this, "m_generatorType", (object) (RandomGeneratorType) reader.ReadInt());
      this.m_state0 = reader.ReadULong();
      this.m_state1 = reader.ReadULong();
    }

    /// <summary>
    /// WARNING: Do not seed with hashes from `.GetHashCode()`, they are not guaranteed to be stable!
    /// </summary>
    public XorRsr128PlusGenerator(RandomGeneratorType genType, ulong state0, ulong state1)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_generatorType = genType;
      this.setSeed(state0, state1);
    }

    public void Seed(byte[] seed)
    {
      if (XorRsr128PlusGenerator.s_md5 == null)
      {
        XorRsr128PlusGenerator.s_md5 = MD5.Create();
        Mafi.Assert.That<int>(XorRsr128PlusGenerator.s_md5.HashSize).IsEqualTo(128);
      }
      byte[] hash = XorRsr128PlusGenerator.s_md5.ComputeHash(seed);
      Mafi.Assert.That<int>(hash.Length).IsEqualTo(16);
      this.setSeed(BitConverter.ToUInt64(hash, 0), BitConverter.ToUInt64(hash, 8));
    }

    /// <summary>
    /// WARNING: Do not seed with hashes from `.GetHashCode()`, they are not guaranteed to be stable!
    /// </summary>
    public void SeedFast(ulong seed)
    {
      this.m_state0 = SplitMix64Generator.NextUlongStateless(ref seed);
      this.m_state1 = SplitMix64Generator.NextUlongStateless(ref seed);
    }

    /// <summary>
    /// WARNING: Do not seed with hashes from `.GetHashCode()`, they are not guaranteed to be stable!
    /// </summary>
    public void SeedFast(ulong seed1, ulong seed2)
    {
      this.m_state0 = SplitMix64Generator.NextUlongStateless(ref seed1);
      this.m_state1 = SplitMix64Generator.NextUlongStateless(ref seed2);
    }

    public void GetState(out ulong state0, out ulong state1)
    {
      state0 = this.m_state0;
      state1 = this.m_state1;
    }

    public unsafe byte[] GetState()
    {
      byte[] state = new byte[16];
      fixed (byte* numPtr = state)
      {
        *(ulong*) numPtr = this.m_state0;
        ((ulong*) numPtr)[1] = this.m_state1;
      }
      return state;
    }

    public void SetState(ulong state0, ulong state1)
    {
      this.m_state0 = state0;
      this.m_state1 = state1;
    }

    private void setSeed(ulong state0, ulong state1)
    {
      if (state0 == 0UL && state1 == 0UL)
      {
        Mafi.Assert.Fail("Invalid seed for random generator! Choosing default seed.");
        state0 = 78187493520UL;
        state1 = 866507813107081UL;
      }
      this.m_state0 = state0;
      this.m_state1 = state1;
    }

    /// <summary>
    /// Generates 64-bit random number using XorShift128+ algorithm. This method never returns 0.
    /// </summary>
    /// <remarks>WARNING: If the state is 0 this method always returns 0!</remarks>
    public ulong NextUlong()
    {
      ulong state0 = this.m_state0;
      ulong state1 = this.m_state1;
      ulong num1 = state0 + state1;
      ulong num2 = state1 ^ state0;
      this.m_state0 = (ulong) ((long) state0.RotateLeft(55) ^ (long) num2 ^ (long) num2 << 14);
      this.m_state1 = num2.RotateLeft(36);
      return num1;
    }

    /// <summary>
    /// This is the jump function for the generator. It is equivalent to 2^64 calls to next(). It can be used to
    /// generate 2^64 non-overlapping subsequences for parallel computations.
    /// </summary>
    public void Jump()
    {
      ulong state0 = 0;
      ulong state1 = 0;
      for (int index = 0; index < 64; ++index)
      {
        if ((-4707382666127344949L & 1L << index) != 0L)
        {
          state0 ^= this.m_state0;
          state1 ^= this.m_state1;
        }
        long num = (long) this.NextUlong();
      }
      for (int index = 0; index < 64; ++index)
      {
        if ((-2852180941702784734L & 1L << index) != 0L)
        {
          state0 ^= this.m_state0;
          state1 ^= this.m_state1;
        }
        long num = (long) this.NextUlong();
      }
      this.setSeed(state0, state1);
    }

    public IRandom Clone()
    {
      return (IRandom) new XorRsr128PlusGenerator(this.m_generatorType, this.m_state0, this.m_state1);
    }

    ICoreRandom ICoreRandom.Clone(RandomGeneratorType newType)
    {
      return (ICoreRandom) new XorRsr128PlusGenerator(newType, this.m_state0, this.m_state1);
    }

    [Conditional("MAFI_ASSERTIONS_DEBUG_ONLY")]
    private void checkState_debugOnly()
    {
      Mafi.Assert.That<bool>(this.m_state0 != 0UL || this.m_state1 > 0UL).IsTrue("Not initialized!");
      switch (this.m_generatorType)
      {
        case RandomGeneratorType.Unrestricted:
          break;
        case RandomGeneratorType.SimOnly:
          break;
        case RandomGeneratorType.NonSim:
          break;
        default:
          Mafi.Assert.Fail(string.Format("Unknown random generator type: {0}", (object) this.m_generatorType));
          break;
      }
    }

    static XorRsr128PlusGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      XorRsr128PlusGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((XorRsr128PlusGenerator) obj).SerializeData(writer));
      XorRsr128PlusGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((XorRsr128PlusGenerator) obj).DeserializeData(reader));
    }
  }
}
