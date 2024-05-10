// Decompiled with JetBrains decompiler
// Type: Mafi.ThicknessTilesISemiSlim
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
  [GenerateSerializer(false, null, 0)]
  [ManuallyWrittenSerialization]
  public readonly struct ThicknessTilesISemiSlim : 
    IEquatable<ThicknessTilesISemiSlim>,
    IComparable<ThicknessTilesISemiSlim>
  {
    public readonly short Value;

    public ThicknessTilesISemiSlim(short value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public override string ToString() => this.Value.ToString();

    public bool Equals(ThicknessTilesISemiSlim other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj)
    {
      return obj is ThicknessTilesISemiSlim other && this.Equals(other);
    }

    public override int GetHashCode() => this.Value.GetHashCode();

    public int CompareTo(ThicknessTilesISemiSlim other) => this.Value.CompareTo(other.Value);

    public static bool operator ==(ThicknessTilesISemiSlim lhs, ThicknessTilesISemiSlim rhs)
    {
      return (int) lhs.Value == (int) rhs.Value;
    }

    public static bool operator !=(ThicknessTilesISemiSlim lhs, ThicknessTilesISemiSlim rhs)
    {
      return (int) lhs.Value != (int) rhs.Value;
    }

    public static ThicknessTilesISemiSlim operator -(ThicknessTilesISemiSlim rhs)
    {
      return new ThicknessTilesISemiSlim(-rhs.Value);
    }

    public static void Serialize(ThicknessTilesISemiSlim value, BlobWriter writer)
    {
      writer.WriteShort(value.Value);
    }

    public static ThicknessTilesISemiSlim Deserialize(BlobReader reader)
    {
      return new ThicknessTilesISemiSlim(reader.ReadShort());
    }

    public ThicknessTilesI AsThicknessTilesI => new ThicknessTilesI((int) this.Value);

    public ThicknessTilesF AsThicknessTilesF => new ThicknessTilesF((int) this.Value);
  }
}
