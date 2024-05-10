// Decompiled with JetBrains decompiler
// Type: Mafi.ThicknessTilesISlim
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  [GenerateSerializer(false, null, 0)]
  public readonly struct ThicknessTilesISlim : 
    IEquatable<ThicknessTilesISlim>,
    IComparable<ThicknessTilesISlim>
  {
    public readonly sbyte Value;

    public ThicknessTilesISlim(sbyte value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public override string ToString() => this.Value.ToString();

    public bool Equals(ThicknessTilesISlim other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj)
    {
      return obj is ThicknessTilesISlim other && this.Equals(other);
    }

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(ThicknessTilesISlim other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(ThicknessTilesISlim lhs, ThicknessTilesISlim rhs)
    {
      return (int) lhs.Value == (int) rhs.Value;
    }

    public static bool operator !=(ThicknessTilesISlim lhs, ThicknessTilesISlim rhs)
    {
      return (int) lhs.Value != (int) rhs.Value;
    }

    public static void Serialize(ThicknessTilesISlim value, BlobWriter writer)
    {
      writer.WriteSByte(value.Value);
    }

    public static ThicknessTilesISlim Deserialize(BlobReader reader)
    {
      return new ThicknessTilesISlim(reader.ReadSByte());
    }

    public ThicknessTilesI AsThicknessTilesI => new ThicknessTilesI((int) this.Value);

    public ThicknessTilesF AsThicknessTilesF => new ThicknessTilesF((int) this.Value);
  }
}
