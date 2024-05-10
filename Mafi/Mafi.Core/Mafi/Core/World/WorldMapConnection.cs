// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapConnection
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World
{
  /// <summary>Canonical connection. Ordering is given by hash.</summary>
  [GenerateSerializer(false, null, 0)]
  public struct WorldMapConnection : IEquatable<WorldMapConnection>
  {
    public readonly WorldMapLocation Location1;
    public readonly WorldMapLocation Location2;

    public static void Serialize(WorldMapConnection value, BlobWriter writer)
    {
      WorldMapLocation.Serialize(value.Location1, writer);
      WorldMapLocation.Serialize(value.Location2, writer);
    }

    public static WorldMapConnection Deserialize(BlobReader reader)
    {
      return new WorldMapConnection(WorldMapLocation.Deserialize(reader), WorldMapLocation.Deserialize(reader));
    }

    public WorldMapConnection(WorldMapLocation location1, WorldMapLocation location2)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Location1 = location1;
      this.Location2 = location2;
    }

    public WorldMapConnection CreateOrdered(WorldMapLocation location1, WorldMapLocation location2)
    {
      Assert.That<Vector2i>(location1.Position).IsNotEqualTo<Vector2i>(location2.Position);
      return location1.Position.CompareTo(location2.Position) > 0 ? new WorldMapConnection(location2, location1) : new WorldMapConnection(location1, location2);
    }

    public WorldMapLocation GetOtherLocation(WorldMapLocation location)
    {
      if (this.Location1 == location)
        return this.Location2;
      Assert.That<WorldMapLocation>(this.Location2).IsEqualTo<WorldMapLocation>(location);
      return this.Location1;
    }

    public bool Equals(WorldMapConnection other)
    {
      return object.Equals((object) this.Location1, (object) other.Location1) && object.Equals((object) this.Location2, (object) other.Location2);
    }

    public override bool Equals(object obj)
    {
      return obj is WorldMapConnection other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return Hash.Combine<WorldMapLocation, WorldMapLocation>(this.Location1, this.Location2);
    }
  }
}
