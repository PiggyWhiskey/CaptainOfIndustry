// Decompiled with JetBrains decompiler
// Type: Mafi.Random.SplitMix64Generator
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Random
{
  [GenerateSerializer(false, null, 0)]
  public class SplitMix64Generator : IRandom
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private ulong m_state;

    public static void Serialize(SplitMix64Generator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SplitMix64Generator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SplitMix64Generator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer) => writer.WriteULong(this.m_state);

    public static SplitMix64Generator Deserialize(BlobReader reader)
    {
      SplitMix64Generator splitMix64Generator;
      if (reader.TryStartClassDeserialization<SplitMix64Generator>(out splitMix64Generator))
        reader.EnqueueDataDeserialization((object) splitMix64Generator, SplitMix64Generator.s_deserializeDataDelayedAction);
      return splitMix64Generator;
    }

    protected virtual void DeserializeData(BlobReader reader) => this.m_state = reader.ReadULong();

    public SplitMix64Generator(ulong state)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_state = state;
    }

    public static ulong NextUlongStateless(ref ulong state)
    {
      ulong num1 = state += 11400714819323198485UL;
      ulong num2 = (ulong) (((long) num1 ^ (long) (num1 >> 30)) * -4658895280553007687L);
      ulong num3 = (ulong) (((long) num2 ^ (long) (num2 >> 27)) * -7723592293110705685L);
      return num3 ^ num3 >> 31;
    }

    /// <summary>
    /// Returns random <see cref="T:Mafi.Fix32" /> value from 0 to 1. Note that returned value has only
    /// <see cref="F:Mafi.Fix32.FRACTIONAL_BITS" /> bits of precision.
    /// </summary>
    public static Fix32 NextFix32Stateless(ref ulong state)
    {
      return Fix32.FromRaw((int) SplitMix64Generator.NextUlongStateless(ref state) & 1023);
    }

    public void Seed(byte[] seed) => throw new NotImplementedException();

    public void SeedFast(ulong seed) => this.m_state = seed;

    public void SeedFast(ulong seed1, ulong seed2) => this.m_state = seed1 ^ seed2;

    public byte[] GetState() => BitConverter.GetBytes(this.m_state);

    public void Jump() => throw new NotImplementedException();

    public ulong NextUlong() => SplitMix64Generator.NextUlongStateless(ref this.m_state);

    public IRandom Clone() => (IRandom) new SplitMix64Generator(this.m_state);

    static SplitMix64Generator()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      SplitMix64Generator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SplitMix64Generator) obj).SerializeData(writer));
      SplitMix64Generator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SplitMix64Generator) obj).DeserializeData(reader));
    }
  }
}
