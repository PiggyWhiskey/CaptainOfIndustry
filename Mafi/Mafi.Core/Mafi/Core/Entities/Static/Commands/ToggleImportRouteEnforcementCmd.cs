// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.ToggleImportRouteEnforcementCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleImportRouteEnforcementCmd : InputCommand
  {
    public readonly EntityId EntityToToggleId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ToggleImportRouteEnforcementCmd(IEntityAssignedAsInput entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityToToggleId = entity.Id;
    }

    public static void Serialize(ToggleImportRouteEnforcementCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleImportRouteEnforcementCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleImportRouteEnforcementCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityToToggleId, writer);
    }

    public static ToggleImportRouteEnforcementCmd Deserialize(BlobReader reader)
    {
      ToggleImportRouteEnforcementCmd routeEnforcementCmd;
      if (reader.TryStartClassDeserialization<ToggleImportRouteEnforcementCmd>(out routeEnforcementCmd))
        reader.EnqueueDataDeserialization((object) routeEnforcementCmd, ToggleImportRouteEnforcementCmd.s_deserializeDataDelayedAction);
      return routeEnforcementCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleImportRouteEnforcementCmd>(this, "EntityToToggleId", (object) EntityId.Deserialize(reader));
    }

    static ToggleImportRouteEnforcementCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleImportRouteEnforcementCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleImportRouteEnforcementCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
