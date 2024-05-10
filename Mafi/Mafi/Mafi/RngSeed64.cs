// Decompiled with JetBrains decompiler
// Type: Mafi.RngSeed64
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct RngSeed64 : IEquatable<RngSeed64>
  {
    [SerializeUsingNonVariableEncoding]
    public readonly ulong Value;

    public static void Serialize(RngSeed64 value, BlobWriter writer)
    {
      writer.WriteULongNonVariable(value.Value);
    }

    public static RngSeed64 Deserialize(BlobReader reader)
    {
      return new RngSeed64(reader.ReadULongNonVariable());
    }

    public RngSeed64(ulong value)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Value = value;
    }

    public SimplexNoise2dSeed64 ToNoiseSeed2d64() => this.Value.ToNoiseSeed2d64();

    public bool Equals(RngSeed64 other) => (long) this.Value == (long) other.Value;

    public override bool Equals(object obj) => obj is RngSeed64 other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();
  }
}
