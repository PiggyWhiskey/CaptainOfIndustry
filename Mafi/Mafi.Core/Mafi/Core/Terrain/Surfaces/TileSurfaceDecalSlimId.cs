// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Surfaces.TileSurfaceDecalSlimId
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Surfaces
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TileSurfaceDecalSlimId : IEquatable<TileSurfaceDecalSlimId>
  {
    public readonly byte Value;

    public static void Serialize(TileSurfaceDecalSlimId value, BlobWriter writer)
    {
      writer.WriteByte(value.Value);
    }

    public static TileSurfaceDecalSlimId Deserialize(BlobReader reader)
    {
      return new TileSurfaceDecalSlimId(reader.ReadByte());
    }

    public static TileSurfaceDecalSlimId PhantomId => new TileSurfaceDecalSlimId();

    public bool IsPhantom => this.Value == (byte) 0;

    public bool IsNotPhantom => this.Value > (byte) 0;

    public TileSurfaceDecalSlimId(byte value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    public TerrainTileSurfaceDecalProto AsProtoOrPhantom(TileSurfaceDecalsSlimIdManager manager)
    {
      return manager.ResolveOrPhantom(this);
    }

    public TerrainTileSurfaceDecalProto AsProtoOrPhantom(TerrainManager manager)
    {
      return manager.TileSurfaceDecalsSlimIdManager.ResolveOrPhantom(this);
    }

    public override string ToString() => this.Value.ToString();

    public static bool operator ==(TileSurfaceDecalSlimId left, TileSurfaceDecalSlimId right)
    {
      return (int) left.Value == (int) right.Value;
    }

    public static bool operator !=(TileSurfaceDecalSlimId left, TileSurfaceDecalSlimId right)
    {
      return (int) left.Value != (int) right.Value;
    }

    public bool Equals(TileSurfaceDecalSlimId other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj)
    {
      return obj is TileSurfaceDecalSlimId other && this.Equals(other);
    }

    public override int GetHashCode() => this.Value.GetHashCode();
  }
}
