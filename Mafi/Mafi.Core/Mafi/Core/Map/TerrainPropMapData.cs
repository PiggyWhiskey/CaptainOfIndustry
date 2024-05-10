// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.TerrainPropMapData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TerrainPropMapData
  {
    public readonly Proto.ID ProtoId;
    public readonly Tile2f Position;
    public readonly ThicknessTilesF HeightOffset;
    public readonly AngleSlim RotationYaw;
    public readonly AngleSlim RotationPitch;
    public readonly AngleSlim RotationRoll;
    public readonly Percent Scale;
    public readonly int VariantIndex;

    public TerrainPropMapData(
      Proto.ID protoId,
      Tile2f position,
      AngleSlim rotationYaw = default (AngleSlim),
      AngleSlim rotationPitch = default (AngleSlim),
      AngleSlim rotationRoll = default (AngleSlim),
      Percent scale = default (Percent),
      int variantIndex = 0,
      ThicknessTilesF heightOffset = default (ThicknessTilesF))
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.ProtoId = protoId;
      this.Position = position;
      this.RotationYaw = rotationYaw;
      this.RotationPitch = rotationPitch;
      this.RotationRoll = rotationRoll;
      this.Scale = scale == new Percent() ? Percent.Hundred : scale;
      this.VariantIndex = variantIndex;
      this.HeightOffset = heightOffset;
    }

    public static void Serialize(TerrainPropMapData value, BlobWriter writer)
    {
      Proto.ID.Serialize(value.ProtoId, writer);
      Tile2f.Serialize(value.Position, writer);
      AngleSlim.Serialize(value.RotationYaw, writer);
      AngleSlim.Serialize(value.RotationPitch, writer);
      AngleSlim.Serialize(value.RotationRoll, writer);
      Percent.Serialize(value.Scale, writer);
      writer.WriteInt(value.VariantIndex);
      ThicknessTilesF.Serialize(value.HeightOffset, writer);
    }

    public static TerrainPropMapData Deserialize(BlobReader reader)
    {
      return new TerrainPropMapData(Proto.ID.Deserialize(reader), Tile2f.Deserialize(reader), AngleSlim.Deserialize(reader), AngleSlim.Deserialize(reader), AngleSlim.Deserialize(reader), Percent.Deserialize(reader), reader.ReadInt(), ThicknessTilesF.Deserialize(reader));
    }
  }
}
