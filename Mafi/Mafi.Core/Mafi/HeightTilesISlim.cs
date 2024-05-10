// Decompiled with JetBrains decompiler
// Type: Mafi.HeightTilesISlim
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
  [DebuggerStepThrough]
  [ManuallyWrittenSerialization]
  public readonly struct HeightTilesISlim : 
    IEquatable<HeightTilesISlim>,
    IComparable<HeightTilesISlim>
  {
    public readonly short Value;

    public HeightTilesISlim(short value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public override string ToString() => this.Value.ToString();

    public bool Equals(HeightTilesISlim other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj) => obj is HeightTilesISlim other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(HeightTilesISlim other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(HeightTilesISlim lhs, HeightTilesISlim rhs)
    {
      return (int) lhs.Value == (int) rhs.Value;
    }

    public static bool operator !=(HeightTilesISlim lhs, HeightTilesISlim rhs)
    {
      return (int) lhs.Value != (int) rhs.Value;
    }

    public static HeightTilesISlim operator -(HeightTilesISlim rhs)
    {
      return new HeightTilesISlim(-rhs.Value);
    }

    public static void Serialize(HeightTilesISlim value, BlobWriter writer)
    {
      writer.WriteShort(value.Value);
    }

    public static HeightTilesISlim Deserialize(BlobReader reader)
    {
      return new HeightTilesISlim(reader.ReadShort());
    }

    public HeightTilesI AsHeightTilesI => new HeightTilesI((int) this.Value);

    public static bool operator <(HeightTilesISlim lhs, HeightTilesISlim rhs)
    {
      return (int) lhs.Value < (int) rhs.Value;
    }

    public static bool operator <=(HeightTilesISlim lhs, HeightTilesISlim rhs)
    {
      return (int) lhs.Value <= (int) rhs.Value;
    }

    public static bool operator >(HeightTilesISlim lhs, HeightTilesISlim rhs)
    {
      return (int) lhs.Value > (int) rhs.Value;
    }

    public static bool operator >=(HeightTilesISlim lhs, HeightTilesISlim rhs)
    {
      return (int) lhs.Value >= (int) rhs.Value;
    }

    public static bool operator ==(HeightTilesISlim lhs, HeightTilesI rhs)
    {
      return (int) lhs.Value == rhs.Value;
    }

    public static bool operator !=(HeightTilesISlim lhs, HeightTilesI rhs)
    {
      return (int) lhs.Value != rhs.Value;
    }

    public static bool operator <(HeightTilesISlim lhs, HeightTilesI rhs)
    {
      return (int) lhs.Value < rhs.Value;
    }

    public static bool operator <=(HeightTilesISlim lhs, HeightTilesI rhs)
    {
      return (int) lhs.Value <= rhs.Value;
    }

    public static bool operator >(HeightTilesISlim lhs, HeightTilesI rhs)
    {
      return (int) lhs.Value > rhs.Value;
    }

    public static bool operator >=(HeightTilesISlim lhs, HeightTilesI rhs)
    {
      return (int) lhs.Value >= rhs.Value;
    }

    public static bool operator ==(HeightTilesI lhs, HeightTilesISlim rhs)
    {
      return lhs.Value == (int) rhs.Value;
    }

    public static bool operator !=(HeightTilesI lhs, HeightTilesISlim rhs)
    {
      return lhs.Value != (int) rhs.Value;
    }

    public static bool operator <(HeightTilesI lhs, HeightTilesISlim rhs)
    {
      return lhs.Value < (int) rhs.Value;
    }

    public static bool operator <=(HeightTilesI lhs, HeightTilesISlim rhs)
    {
      return lhs.Value <= (int) rhs.Value;
    }

    public static bool operator >(HeightTilesI lhs, HeightTilesISlim rhs)
    {
      return lhs.Value > (int) rhs.Value;
    }

    public static bool operator >=(HeightTilesI lhs, HeightTilesISlim rhs)
    {
      return lhs.Value >= (int) rhs.Value;
    }
  }
}
