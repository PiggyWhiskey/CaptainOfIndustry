// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.RectangleTerrainArea2iRelative
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct RectangleTerrainArea2iRelative : IEquatable<RectangleTerrainArea2iRelative>
  {
    /// <summary>The tile with lowest X and Y coordinates in the area.</summary>
    public readonly RelTile2i Origin;
    /// <summary>Size of the area. Size of zero represents empty area</summary>
    public readonly RelTile2i Size;

    public static void Serialize(RectangleTerrainArea2iRelative value, BlobWriter writer)
    {
      RelTile2i.Serialize(value.Origin, writer);
      RelTile2i.Serialize(value.Size, writer);
    }

    public static RectangleTerrainArea2iRelative Deserialize(BlobReader reader)
    {
      return new RectangleTerrainArea2iRelative(RelTile2i.Deserialize(reader), RelTile2i.Deserialize(reader));
    }

    public int AreaTiles => this.Size.X * this.Size.Y;

    public RectangleTerrainArea2iRelative(RelTile2i origin, RelTile2i size)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Assert.That<int>(size.X).IsNotNegative();
      Assert.That<int>(size.Y).IsNotNegative();
      this.Origin = origin;
      this.Size = size;
    }

    public static bool operator ==(
      RectangleTerrainArea2iRelative lhs,
      RectangleTerrainArea2iRelative rhs)
    {
      return lhs.Equals(rhs);
    }

    public static bool operator !=(
      RectangleTerrainArea2iRelative lhs,
      RectangleTerrainArea2iRelative rhs)
    {
      return !lhs.Equals(rhs);
    }

    public bool Equals(RectangleTerrainArea2iRelative other)
    {
      return this.Origin == other.Origin && this.Size == other.Size;
    }

    public override bool Equals(object obj)
    {
      return obj is RectangleTerrainArea2iRelative other && this.Equals(other);
    }

    public override int GetHashCode() => Hash.Combine<RelTile2i, RelTile2i>(this.Origin, this.Size);

    public override string ToString()
    {
      return string.Format("Rel area {0}+{1}", (object) this.Origin, (object) this.Size);
    }
  }
}
