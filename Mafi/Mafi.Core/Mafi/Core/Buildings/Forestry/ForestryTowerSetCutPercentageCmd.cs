// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Forestry.ForestryTowerSetCutPercentageCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Forestry
{
  [GenerateSerializer(false, null, 0)]
  public class ForestryTowerSetCutPercentageCmd : InputCommand
  {
    public readonly EntityId ForestryTowerId;
    public readonly Percent CutPercent;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ForestryTowerSetCutPercentageCmd(ForestryTower tower, Percent cutPercent)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(tower.Id, cutPercent);
    }

    public ForestryTowerSetCutPercentageCmd(EntityId towerId, Percent cutPercent)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ForestryTowerId = towerId;
      this.CutPercent = cutPercent;
    }

    public static void Serialize(ForestryTowerSetCutPercentageCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ForestryTowerSetCutPercentageCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ForestryTowerSetCutPercentageCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.CutPercent, writer);
      EntityId.Serialize(this.ForestryTowerId, writer);
    }

    public static ForestryTowerSetCutPercentageCmd Deserialize(BlobReader reader)
    {
      ForestryTowerSetCutPercentageCmd cutPercentageCmd;
      if (reader.TryStartClassDeserialization<ForestryTowerSetCutPercentageCmd>(out cutPercentageCmd))
        reader.EnqueueDataDeserialization((object) cutPercentageCmd, ForestryTowerSetCutPercentageCmd.s_deserializeDataDelayedAction);
      return cutPercentageCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ForestryTowerSetCutPercentageCmd>(this, "CutPercent", (object) Percent.Deserialize(reader));
      reader.SetField<ForestryTowerSetCutPercentageCmd>(this, "ForestryTowerId", (object) EntityId.Deserialize(reader));
    }

    static ForestryTowerSetCutPercentageCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ForestryTowerSetCutPercentageCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ForestryTowerSetCutPercentageCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
