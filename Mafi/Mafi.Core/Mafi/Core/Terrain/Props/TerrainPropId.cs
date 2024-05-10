// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Props.TerrainPropId
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Terrain.Props
{
  [ManuallyWrittenSerialization]
  [DebuggerDisplay("{Position,nq}")]
  [DebuggerStepThrough]
  public readonly struct TerrainPropId : IEquatable<TerrainPropId>, IComparable<TerrainPropId>
  {
    /// <summary>Underlying Tile2iSlim value of this Id.</summary>
    public readonly Tile2iSlim Position;
    public static readonly TerrainPropId Invalid;

    public TerrainPropId(Tile2iSlim position)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
    }

    public static bool operator ==(TerrainPropId lhs, TerrainPropId rhs)
    {
      return lhs.Position == rhs.Position;
    }

    public static bool operator !=(TerrainPropId lhs, TerrainPropId rhs)
    {
      return lhs.Position != rhs.Position;
    }

    public override bool Equals(object other)
    {
      return other is TerrainPropId other1 && this.Equals(other1);
    }

    public bool Equals(TerrainPropId other) => this.Position.Equals(other.Position);

    public int CompareTo(TerrainPropId other) => this.Position.CompareTo(other.Position);

    public override string ToString() => this.Position.ToString();

    public override int GetHashCode() => this.Position.GetHashCode();

    public static void Serialize(TerrainPropId value, BlobWriter writer)
    {
      Tile2iSlim.Serialize(value.Position, writer);
    }

    public static TerrainPropId Deserialize(BlobReader reader)
    {
      return new TerrainPropId(Tile2iSlim.Deserialize(reader));
    }

    public bool IsValid => this != TerrainPropId.Invalid;

    public TerrainPropId(int x, int y)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = new Tile2iSlim((ushort) x, (ushort) y);
    }

    static TerrainPropId()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainPropId.Invalid = new TerrainPropId(0, 0);
    }
  }
}
