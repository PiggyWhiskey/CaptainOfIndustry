// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.TreeData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TreeData
  {
    public readonly TreeId Id;
    public readonly TreeProto Proto;
    [OnlyForSaveCompatibility(null)]
    public readonly RelTile2f PositionWithinTile;
    public readonly AngleSlim Rotation;
    public readonly Percent BaseScale;
    public readonly bool CreatedByTerrainGenerator;
    public readonly HeightTilesF PlantedAtHeight;
    public readonly int PlantedAtTick;

    public static void Serialize(TreeData value, BlobWriter writer)
    {
      TreeId.Serialize(value.Id, writer);
      writer.WriteGeneric<TreeProto>(value.Proto);
      RelTile2f.Serialize(value.PositionWithinTile, writer);
      Percent.Serialize(value.BaseScale, writer);
      HeightTilesF.Serialize(value.PlantedAtHeight, writer);
      writer.WriteInt(value.PlantedAtTick);
      AngleSlim.Serialize(value.Rotation, writer);
      writer.WriteBool(value.CreatedByTerrainGenerator);
    }

    public static TreeData Deserialize(BlobReader reader)
    {
      return new TreeData(TreeId.Deserialize(reader), reader.ReadGenericAs<TreeProto>(), RelTile2f.Deserialize(reader), Percent.Deserialize(reader), HeightTilesF.Deserialize(reader), reader.ReadInt(), AngleSlim.Deserialize(reader), reader.ReadBool());
    }

    public bool IsValid => (Mafi.Core.Prototypes.Proto) this.Proto != (Mafi.Core.Prototypes.Proto) null;

    public ProductProto.ID HarvestedProductId => this.Proto.ProductWhenHarvested.Product.Id;

    public Tile2f Position => this.Id.Position.CornerTile2f + this.PositionWithinTile;

    [LoadCtor]
    private TreeData(
      TreeId id,
      TreeProto proto,
      RelTile2f positionWithinTile,
      Percent baseScale,
      HeightTilesF plantedAtHeight,
      int plantedAtTick,
      AngleSlim rotation = default (AngleSlim),
      bool createdByTerrainGenerator = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Id = id;
      this.Proto = proto.CheckNotNull<TreeProto>();
      this.PlantedAtHeight = plantedAtHeight;
      this.PositionWithinTile = positionWithinTile;
      this.BaseScale = baseScale;
      this.PlantedAtTick = plantedAtTick;
      this.Rotation = rotation;
      this.CreatedByTerrainGenerator = createdByTerrainGenerator;
    }

    public TreeData(
      TreeProto proto,
      Tile2f position,
      HeightTilesF plantedAtHeight,
      Percent baseScale,
      int plantedAtTick,
      AngleSlim rotation = default (AngleSlim),
      bool createdByTerrainGenerator = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Id = new TreeId(position.Tile2i.AsSlim);
      this.Proto = proto.CheckNotNull<TreeProto>();
      this.PositionWithinTile = position - this.Id.Position.CornerTile2f;
      this.BaseScale = baseScale;
      this.PlantedAtHeight = plantedAtHeight;
      this.PlantedAtTick = plantedAtTick;
      this.Rotation = rotation;
      this.CreatedByTerrainGenerator = createdByTerrainGenerator;
    }

    public TreeData(
      TreeDataBase baseData,
      HeightTilesF plantedAtHeight,
      int plantedAtTick,
      bool createdByTerrainGenerator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Id = new TreeId(baseData.Position.Tile2i.AsSlim);
      this.Proto = baseData.Proto;
      this.PositionWithinTile = baseData.Position.FractionalPartNonNegative();
      this.BaseScale = baseData.Scale;
      this.PlantedAtHeight = plantedAtHeight;
      this.PlantedAtTick = plantedAtTick;
      this.Rotation = baseData.Rotation;
      this.CreatedByTerrainGenerator = createdByTerrainGenerator;
    }

    public Percent GetScale(ICalendar calendar)
    {
      return this.BaseScale * this.GetScaleIgnoringBase(calendar);
    }

    public Percent GetScaleIgnoringBase(ICalendar calendar)
    {
      return this.Proto.GetScale(calendar.RealTime.Ticks - this.PlantedAtTick);
    }
  }
}
