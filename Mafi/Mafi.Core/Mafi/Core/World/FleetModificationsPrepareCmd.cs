// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.FleetModificationsPrepareCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Fleet;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World
{
  /// <summary>To prepare modifications in shipyard.</summary>
  [GenerateSerializer(false, null, 0)]
  public class FleetModificationsPrepareCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public FleetEntityModificationRequest ModificationRequest;

    public static void Serialize(FleetModificationsPrepareCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetModificationsPrepareCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetModificationsPrepareCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      FleetEntityModificationRequest.Serialize(this.ModificationRequest, writer);
    }

    public static FleetModificationsPrepareCmd Deserialize(BlobReader reader)
    {
      FleetModificationsPrepareCmd modificationsPrepareCmd;
      if (reader.TryStartClassDeserialization<FleetModificationsPrepareCmd>(out modificationsPrepareCmd))
        reader.EnqueueDataDeserialization((object) modificationsPrepareCmd, FleetModificationsPrepareCmd.s_deserializeDataDelayedAction);
      return modificationsPrepareCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ModificationRequest = FleetEntityModificationRequest.Deserialize(reader);
    }

    public FleetModificationsPrepareCmd(FleetEntityModificationRequest modificationRequest)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ModificationRequest = modificationRequest;
    }

    static FleetModificationsPrepareCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetModificationsPrepareCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FleetModificationsPrepareCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
