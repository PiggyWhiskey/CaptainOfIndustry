// Decompiled with JetBrains decompiler
// Type: Mafi.ThicknessTilesISemiSlimOption
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct ThicknessTilesISemiSlimOption : IEquatable<ThicknessTilesISemiSlimOption>
  {
    public readonly short RawValue;

    public static ThicknessTilesISemiSlimOption None
    {
      get => new ThicknessTilesISemiSlimOption(short.MinValue);
    }

    public ThicknessTilesISemiSlim Value => new ThicknessTilesISemiSlim(this.RawValue);

    public bool HasValue => this.RawValue != short.MinValue;

    public bool IsNone => this.RawValue != short.MinValue;

    [LoadCtor]
    public ThicknessTilesISemiSlimOption(short rawValue)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawValue = rawValue;
    }

    public ThicknessTilesISemiSlimOption(ThicknessTilesISemiSlim value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawValue = value.Value;
    }

    public override string ToString() => !this.HasValue ? "None" : this.Value.ToString();

    public static implicit operator ThicknessTilesISemiSlimOption(ThicknessTilesISemiSlim value)
    {
      return new ThicknessTilesISemiSlimOption(value);
    }

    public static bool operator ==(
      ThicknessTilesISemiSlimOption left,
      ThicknessTilesISemiSlimOption right)
    {
      return (int) left.RawValue == (int) right.RawValue;
    }

    public static bool operator !=(
      ThicknessTilesISemiSlimOption left,
      ThicknessTilesISemiSlimOption right)
    {
      return (int) left.RawValue != (int) right.RawValue;
    }

    public bool Equals(ThicknessTilesISemiSlimOption other)
    {
      return (int) this.RawValue == (int) other.RawValue;
    }

    public override bool Equals(object obj)
    {
      return obj is ThicknessTilesISemiSlimOption other && this.Equals(other);
    }

    public override int GetHashCode() => this.RawValue.GetHashCode();

    public static void Serialize(ThicknessTilesISemiSlimOption value, BlobWriter writer)
    {
      writer.WriteShort(value.RawValue);
    }

    public static ThicknessTilesISemiSlimOption Deserialize(BlobReader reader)
    {
      return new ThicknessTilesISemiSlimOption(reader.ReadShort());
    }
  }
}
