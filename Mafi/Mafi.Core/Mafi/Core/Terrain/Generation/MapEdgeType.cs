// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.MapEdgeType
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct MapEdgeType
  {
    public readonly bool GroundTowardsMinusX;
    public readonly bool GroundTowardsMinusY;
    public readonly bool GroundTowardsPlusX;
    public readonly bool GroundTowardsPlusY;

    public static void Serialize(MapEdgeType value, BlobWriter writer)
    {
      writer.WriteBool(value.GroundTowardsMinusX);
      writer.WriteBool(value.GroundTowardsMinusY);
      writer.WriteBool(value.GroundTowardsPlusX);
      writer.WriteBool(value.GroundTowardsPlusY);
    }

    public static MapEdgeType Deserialize(BlobReader reader)
    {
      return new MapEdgeType(reader.ReadBool(), reader.ReadBool(), reader.ReadBool(), reader.ReadBool());
    }

    public MapEdgeType(
      bool groundTowardsMinusX,
      bool groundTowardsMinusY,
      bool groundTowardsPlusX,
      bool groundTowardsPlusY)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.GroundTowardsMinusX = groundTowardsMinusX;
      this.GroundTowardsMinusY = groundTowardsMinusY;
      this.GroundTowardsPlusX = groundTowardsPlusX;
      this.GroundTowardsPlusY = groundTowardsPlusY;
    }
  }
}
