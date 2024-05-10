// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IoPortKey
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct IoPortKey : IEquatable<IoPortKey>
  {
    public readonly Tile3i Position;
    public readonly Direction903d Direction;

    public IoPortKey(Tile3i position, Direction903d direction)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Position = position;
      this.Direction = direction;
    }

    public bool Equals(IoPortKey other)
    {
      return this.Position.Equals(other.Position) && this.Direction.Equals(other.Direction);
    }

    public override bool Equals(object obj) => obj is IoPortKey other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<Tile3i, Direction903d>(this.Position, this.Direction);
    }

    public override string ToString()
    {
      return string.Format("{0}{1}", (object) this.Position, (object) this.Direction);
    }

    public static void Serialize(IoPortKey value, BlobWriter writer)
    {
      Tile3i.Serialize(value.Position, writer);
      Direction903d.Serialize(value.Direction, writer);
    }

    public static IoPortKey Deserialize(BlobReader reader)
    {
      return new IoPortKey(Tile3i.Deserialize(reader), Direction903d.Deserialize(reader));
    }
  }
}
