// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.UpgradeEntityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class UpgradeEntityCmd : InputCommand
  {
    public readonly EntityId EntityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public UpgradeEntityCmd(IUpgradableEntity entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id);
    }

    public UpgradeEntityCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
    }

    public static void Serialize(UpgradeEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UpgradeEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UpgradeEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static UpgradeEntityCmd Deserialize(BlobReader reader)
    {
      UpgradeEntityCmd upgradeEntityCmd;
      if (reader.TryStartClassDeserialization<UpgradeEntityCmd>(out upgradeEntityCmd))
        reader.EnqueueDataDeserialization((object) upgradeEntityCmd, UpgradeEntityCmd.s_deserializeDataDelayedAction);
      return upgradeEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<UpgradeEntityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    static UpgradeEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UpgradeEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      UpgradeEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
