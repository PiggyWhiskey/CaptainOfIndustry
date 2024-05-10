// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileSurfaceSlimId
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
  public readonly struct TileSurfaceSlimId : IEquatable<TileSurfaceSlimId>
  {
    public readonly byte Value;

    public static void Serialize(TileSurfaceSlimId value, BlobWriter writer)
    {
      writer.WriteByte(value.Value);
    }

    public static TileSurfaceSlimId Deserialize(BlobReader reader)
    {
      return new TileSurfaceSlimId(reader.ReadByte());
    }

    public static TileSurfaceSlimId PhantomId => new TileSurfaceSlimId();

    public bool IsPhantom => this.Value == (byte) 0;

    public bool IsNotPhantom => this.Value > (byte) 0;

    public TileSurfaceSlimId(byte value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public TerrainTileSurfaceProto AsProtoOrPhantom(TileSurfacesSlimIdManager manager)
    {
      return manager.ResolveOrPhantom(this);
    }

    public TerrainTileSurfaceProto AsProtoOrPhantom(TerrainManager manager)
    {
      return manager.TileSurfacesSlimIdManager.ResolveOrPhantom(this);
    }

    public override string ToString() => this.Value.ToString();

    public static bool operator ==(TileSurfaceSlimId left, TileSurfaceSlimId right)
    {
      return (int) left.Value == (int) right.Value;
    }

    public static bool operator !=(TileSurfaceSlimId left, TileSurfaceSlimId right)
    {
      return (int) left.Value != (int) right.Value;
    }

    public bool Equals(TileSurfaceSlimId other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj) => obj is TileSurfaceSlimId other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public TerrainTileSurfaceProto ResolveToProto(TerrainManager terrainManager)
    {
      return terrainManager.ResolveSlimSurface(this);
    }
  }
}
