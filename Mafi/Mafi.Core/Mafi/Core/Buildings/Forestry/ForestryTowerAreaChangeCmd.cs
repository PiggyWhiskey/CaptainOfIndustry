// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Forestry.ForestryTowerAreaChangeCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Forestry
{
  /// <summary>
  /// Command to change the range (area) of <see cref="T:Mafi.Core.Buildings.Forestry.ForestryTower" />.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class ForestryTowerAreaChangeCmd : InputCommand
  {
    public readonly EntityId ForestryTowerId;
    public readonly RectangleTerrainArea2i Area;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ForestryTowerAreaChangeCmd(EntityId forestryTowerId, RectangleTerrainArea2i area)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ForestryTowerId = forestryTowerId;
      this.Area = area;
    }

    public static void Serialize(ForestryTowerAreaChangeCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ForestryTowerAreaChangeCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ForestryTowerAreaChangeCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
      EntityId.Serialize(this.ForestryTowerId, writer);
    }

    public static ForestryTowerAreaChangeCmd Deserialize(BlobReader reader)
    {
      ForestryTowerAreaChangeCmd towerAreaChangeCmd;
      if (reader.TryStartClassDeserialization<ForestryTowerAreaChangeCmd>(out towerAreaChangeCmd))
        reader.EnqueueDataDeserialization((object) towerAreaChangeCmd, ForestryTowerAreaChangeCmd.s_deserializeDataDelayedAction);
      return towerAreaChangeCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ForestryTowerAreaChangeCmd>(this, "Area", (object) RectangleTerrainArea2i.Deserialize(reader));
      reader.SetField<ForestryTowerAreaChangeCmd>(this, "ForestryTowerId", (object) EntityId.Deserialize(reader));
    }

    static ForestryTowerAreaChangeCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ForestryTowerAreaChangeCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ForestryTowerAreaChangeCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
