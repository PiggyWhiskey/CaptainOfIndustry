// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapLocId
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.World
{
  [ManuallyWrittenSerialization]
  [DebuggerStepThrough]
  [DebuggerDisplay("{Value,nq}")]
  public readonly struct WorldMapLocId : IEquatable<WorldMapLocId>, IComparable<WorldMapLocId>
  {
    /// <summary>Underlying int value of this Id.</summary>
    public readonly int Value;

    public WorldMapLocId(int value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public static bool operator ==(WorldMapLocId lhs, WorldMapLocId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(WorldMapLocId lhs, WorldMapLocId rhs) => lhs.Value != rhs.Value;

    public override bool Equals(object other)
    {
      return other is WorldMapLocId other1 && this.Equals(other1);
    }

    public bool Equals(WorldMapLocId other) => this.Value.Equals(other.Value);

    public int CompareTo(WorldMapLocId other) => this.Value.CompareTo(other.Value);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value;

    public static void Serialize(WorldMapLocId value, BlobWriter writer)
    {
      writer.WriteInt(value.Value);
    }

    public static WorldMapLocId Deserialize(BlobReader reader)
    {
      return new WorldMapLocId(reader.ReadInt());
    }
  }
}
