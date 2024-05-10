// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.ToggleEnabledGroupCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleEnabledGroupCmd : InputCommand
  {
    public readonly ImmutableArray<EntityId> EntityIds;
    public readonly bool PauseOnly;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ToggleEnabledGroupCmd(ImmutableArray<EntityId> entityIds, bool pauseOnly)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityIds = entityIds;
      this.PauseOnly = pauseOnly;
    }

    public static void Serialize(ToggleEnabledGroupCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleEnabledGroupCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleEnabledGroupCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<EntityId>.Serialize(this.EntityIds, writer);
      writer.WriteBool(this.PauseOnly);
    }

    public static ToggleEnabledGroupCmd Deserialize(BlobReader reader)
    {
      ToggleEnabledGroupCmd toggleEnabledGroupCmd;
      if (reader.TryStartClassDeserialization<ToggleEnabledGroupCmd>(out toggleEnabledGroupCmd))
        reader.EnqueueDataDeserialization((object) toggleEnabledGroupCmd, ToggleEnabledGroupCmd.s_deserializeDataDelayedAction);
      return toggleEnabledGroupCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleEnabledGroupCmd>(this, "EntityIds", (object) ImmutableArray<EntityId>.Deserialize(reader));
      reader.SetField<ToggleEnabledGroupCmd>(this, "PauseOnly", (object) reader.ReadBool());
    }

    static ToggleEnabledGroupCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleEnabledGroupCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleEnabledGroupCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
