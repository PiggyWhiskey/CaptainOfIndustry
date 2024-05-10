// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.TreeStumpData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Simulation;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TreeStumpData
  {
    public readonly TreeId Id;
    public readonly TreeProto TreeProto;
    public readonly RelTile2f PositionWithinTile;
    public readonly AngleSlim Rotation;
    public readonly Percent Scale;
    public readonly HeightTilesF PlantedAtHeight;
    public readonly SimStep CreatedAtTick;

    public static void Serialize(TreeStumpData value, BlobWriter writer)
    {
      TreeId.Serialize(value.Id, writer);
      writer.WriteGeneric<TreeProto>(value.TreeProto);
      RelTile2f.Serialize(value.PositionWithinTile, writer);
      AngleSlim.Serialize(value.Rotation, writer);
      Percent.Serialize(value.Scale, writer);
      HeightTilesF.Serialize(value.PlantedAtHeight, writer);
      SimStep.Serialize(value.CreatedAtTick, writer);
    }

    public static TreeStumpData Deserialize(BlobReader reader)
    {
      return new TreeStumpData(TreeId.Deserialize(reader), reader.ReadGenericAs<TreeProto>(), RelTile2f.Deserialize(reader), AngleSlim.Deserialize(reader), Percent.Deserialize(reader), HeightTilesF.Deserialize(reader), SimStep.Deserialize(reader));
    }

    public Tile2f Position => this.Id.Position.CornerTile2f + this.PositionWithinTile;

    [LoadCtor]
    private TreeStumpData(
      TreeId id,
      TreeProto treeProto,
      RelTile2f positionWithinTile,
      AngleSlim rotation,
      Percent scale,
      HeightTilesF plantedAtHeight,
      SimStep createdAtTick)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Id = id;
      this.TreeProto = treeProto;
      this.PositionWithinTile = positionWithinTile;
      this.Rotation = rotation;
      this.Scale = scale;
      this.PlantedAtHeight = plantedAtHeight;
      this.CreatedAtTick = createdAtTick;
    }

    public TreeStumpData(TreeData treeData, ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Id = treeData.Id;
      this.TreeProto = treeData.Proto;
      this.PositionWithinTile = treeData.PositionWithinTile;
      this.Rotation = treeData.Rotation;
      this.Scale = treeData.GetScale(calendar);
      this.PlantedAtHeight = treeData.PlantedAtHeight;
      this.CreatedAtTick = new SimStep(calendar.RealTime.Ticks);
    }

    public ThicknessTilesF GetDeltaHeight(ICalendar calendar)
    {
      return new ThicknessTilesF(-(calendar.RealTime.Ticks - this.CreatedAtTick.Value) * TreesManager.STUMP_SINK_RATE_PER_MONTH / 600);
    }
  }
}
