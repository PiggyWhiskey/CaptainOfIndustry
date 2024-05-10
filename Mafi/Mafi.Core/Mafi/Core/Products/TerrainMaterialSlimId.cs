// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.TerrainMaterialSlimId
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  /// <summary>
  /// Special ID for terrain materials that is one byte. There is maximum of 255 unique terrain materials + phantom.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct TerrainMaterialSlimId : IEquatable<TerrainMaterialSlimId>
  {
    public readonly byte Value;

    public static TerrainMaterialSlimId PhantomId => new TerrainMaterialSlimId();

    public bool IsPhantom => this.Value == (byte) 0;

    public bool IsNotPhantom => this.Value > (byte) 0;

    public TerrainMaterialSlimId(byte value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Value = value;
    }

    [Pure]
    public TerrainMaterialProto ToFull(TerrainManager manager) => manager.ResolveSlimMaterial(this);

    public override string ToString() => this.Value.ToString();

    public static bool operator ==(TerrainMaterialSlimId left, TerrainMaterialSlimId right)
    {
      return (int) left.Value == (int) right.Value;
    }

    public static bool operator !=(TerrainMaterialSlimId left, TerrainMaterialSlimId right)
    {
      return (int) left.Value != (int) right.Value;
    }

    public bool Equals(TerrainMaterialSlimId other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj)
    {
      return obj != null && obj is TerrainMaterialSlimId other && this.Equals(other);
    }

    public override int GetHashCode() => (int) this.Value;

    public static void Serialize(TerrainMaterialSlimId value, BlobWriter writer)
    {
      writer.WriteByte(value.Value);
    }

    public static TerrainMaterialSlimId Deserialize(BlobReader reader)
    {
      return new TerrainMaterialSlimId(reader.ReadByte());
    }
  }
}
