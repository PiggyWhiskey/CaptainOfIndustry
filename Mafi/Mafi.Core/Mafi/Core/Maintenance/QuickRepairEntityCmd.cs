// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Maintenance.QuickRepairEntityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Maintenance
{
  [GenerateSerializer(false, null, 0)]
  public class QuickRepairEntityCmd : InputCommand
  {
    public readonly EntityId EntityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public QuickRepairEntityCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
    }

    public static void Serialize(QuickRepairEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<QuickRepairEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, QuickRepairEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static QuickRepairEntityCmd Deserialize(BlobReader reader)
    {
      QuickRepairEntityCmd quickRepairEntityCmd;
      if (reader.TryStartClassDeserialization<QuickRepairEntityCmd>(out quickRepairEntityCmd))
        reader.EnqueueDataDeserialization((object) quickRepairEntityCmd, QuickRepairEntityCmd.s_deserializeDataDelayedAction);
      return quickRepairEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<QuickRepairEntityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    static QuickRepairEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      QuickRepairEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      QuickRepairEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
