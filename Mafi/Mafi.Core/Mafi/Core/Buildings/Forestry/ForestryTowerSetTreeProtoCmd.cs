// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Forestry.ForestryTowerSetTreeProtoCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Forestry
{
  [GenerateSerializer(false, null, 0)]
  public class ForestryTowerSetTreeProtoCmd : InputCommand
  {
    public readonly EntityId ForestryTowerId;
    public readonly Proto.ID GroupProtoId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ForestryTowerSetTreeProtoCmd(ForestryTower tower, TreePlantingGroupProto groupProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(tower.Id, groupProto.Id);
    }

    public ForestryTowerSetTreeProtoCmd(EntityId towerId, Proto.ID groupProtoId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ForestryTowerId = towerId;
      this.GroupProtoId = groupProtoId;
    }

    public static void Serialize(ForestryTowerSetTreeProtoCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ForestryTowerSetTreeProtoCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ForestryTowerSetTreeProtoCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ForestryTowerId, writer);
      Proto.ID.Serialize(this.GroupProtoId, writer);
    }

    public static ForestryTowerSetTreeProtoCmd Deserialize(BlobReader reader)
    {
      ForestryTowerSetTreeProtoCmd towerSetTreeProtoCmd;
      if (reader.TryStartClassDeserialization<ForestryTowerSetTreeProtoCmd>(out towerSetTreeProtoCmd))
        reader.EnqueueDataDeserialization((object) towerSetTreeProtoCmd, ForestryTowerSetTreeProtoCmd.s_deserializeDataDelayedAction);
      return towerSetTreeProtoCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ForestryTowerSetTreeProtoCmd>(this, "ForestryTowerId", (object) EntityId.Deserialize(reader));
      reader.SetField<ForestryTowerSetTreeProtoCmd>(this, "GroupProtoId", (object) Proto.ID.Deserialize(reader));
    }

    static ForestryTowerSetTreeProtoCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ForestryTowerSetTreeProtoCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ForestryTowerSetTreeProtoCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
