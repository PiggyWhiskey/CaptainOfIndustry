// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.ToggleEdictEnabledCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleEdictEnabledCmd : InputCommand
  {
    public readonly Proto.ID EdictProtoId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ToggleEdictEnabledCmd(EdictProto edict)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EdictProtoId = edict.Id;
    }

    public static void Serialize(ToggleEdictEnabledCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleEdictEnabledCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleEdictEnabledCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Proto.ID.Serialize(this.EdictProtoId, writer);
    }

    public static ToggleEdictEnabledCmd Deserialize(BlobReader reader)
    {
      ToggleEdictEnabledCmd toggleEdictEnabledCmd;
      if (reader.TryStartClassDeserialization<ToggleEdictEnabledCmd>(out toggleEdictEnabledCmd))
        reader.EnqueueDataDeserialization((object) toggleEdictEnabledCmd, ToggleEdictEnabledCmd.s_deserializeDataDelayedAction);
      return toggleEdictEnabledCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleEdictEnabledCmd>(this, "EdictProtoId", (object) Proto.ID.Deserialize(reader));
    }

    static ToggleEdictEnabledCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleEdictEnabledCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleEdictEnabledCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
