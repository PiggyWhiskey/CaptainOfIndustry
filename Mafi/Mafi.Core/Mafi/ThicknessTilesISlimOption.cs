// Decompiled with JetBrains decompiler
// Type: Mafi.ThicknessTilesISlimOption
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
  public readonly struct ThicknessTilesISlimOption : IEquatable<ThicknessTilesISlimOption>
  {
    public readonly sbyte RawValue;

    public static ThicknessTilesISlimOption None => new ThicknessTilesISlimOption(sbyte.MinValue);

    public ThicknessTilesISlim Value => new ThicknessTilesISlim(this.RawValue);

    public bool HasValue => this.RawValue != sbyte.MinValue;

    public bool IsNone => this.RawValue != sbyte.MinValue;

    [LoadCtor]
    public ThicknessTilesISlimOption(sbyte rawValue)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawValue = rawValue;
    }

    public ThicknessTilesISlimOption(ThicknessTilesISlim value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.RawValue = value.Value;
    }

    public override string ToString() => !this.HasValue ? "None" : this.Value.ToString();

    public static implicit operator ThicknessTilesISlimOption(ThicknessTilesISlim value)
    {
      return new ThicknessTilesISlimOption(value);
    }

    public static bool operator ==(ThicknessTilesISlimOption left, ThicknessTilesISlimOption right)
    {
      return (int) left.RawValue == (int) right.RawValue;
    }

    public static bool operator !=(ThicknessTilesISlimOption left, ThicknessTilesISlimOption right)
    {
      return (int) left.RawValue != (int) right.RawValue;
    }

    public bool Equals(ThicknessTilesISlimOption other)
    {
      return (int) this.RawValue == (int) other.RawValue;
    }

    public override bool Equals(object obj)
    {
      return obj is ThicknessTilesISlimOption other && this.Equals(other);
    }

    public override int GetHashCode() => this.RawValue.GetHashCode();

    public static void Serialize(ThicknessTilesISlimOption value, BlobWriter writer)
    {
      writer.WriteSByte(value.RawValue);
    }

    public static ThicknessTilesISlimOption Deserialize(BlobReader reader)
    {
      return new ThicknessTilesISlimOption(reader.ReadSByte());
    }
  }
}
