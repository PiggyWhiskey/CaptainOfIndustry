// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapCellId
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Map
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  [DebuggerDisplay("{Value,nq}")]
  public readonly struct MapCellId : IEquatable<MapCellId>, IComparable<MapCellId>
  {
    /// <summary>Underlying int value of this Id.</summary>
    public readonly int Value;

    public MapCellId(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Mafi.Assert.That<int>(value).IsNotNegative("Invalid MapCellId value");
      this.Value = value;
    }

    public static bool operator ==(MapCellId lhs, MapCellId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(MapCellId lhs, MapCellId rhs) => lhs.Value != rhs.Value;

    public override bool Equals(object other) => other is MapCellId other1 && this.Equals(other1);

    public bool Equals(MapCellId other) => this.Value.Equals(other.Value);

    public int CompareTo(MapCellId other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value;

    public static void Serialize(MapCellId value, BlobWriter writer)
    {
      writer.WriteIntNotNegative(value.Value);
    }

    public static MapCellId Deserialize(BlobReader reader)
    {
      return new MapCellId(reader.ReadIntNotNegative());
    }
  }
}
