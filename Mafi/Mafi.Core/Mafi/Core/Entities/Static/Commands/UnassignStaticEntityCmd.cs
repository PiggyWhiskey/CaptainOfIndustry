// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.UnassignStaticEntityCmd
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
  public class UnassignStaticEntityCmd : InputCommand
  {
    public readonly EntityId FirstEntityId;
    public readonly EntityId SecondEntityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public UnassignStaticEntityCmd(IEntityAssignedAsOutput first, IEntityAssignedAsInput second)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FirstEntityId = first.Id;
      this.SecondEntityId = second.Id;
    }

    public static void Serialize(UnassignStaticEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnassignStaticEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnassignStaticEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.FirstEntityId, writer);
      EntityId.Serialize(this.SecondEntityId, writer);
    }

    public static UnassignStaticEntityCmd Deserialize(BlobReader reader)
    {
      UnassignStaticEntityCmd unassignStaticEntityCmd;
      if (reader.TryStartClassDeserialization<UnassignStaticEntityCmd>(out unassignStaticEntityCmd))
        reader.EnqueueDataDeserialization((object) unassignStaticEntityCmd, UnassignStaticEntityCmd.s_deserializeDataDelayedAction);
      return unassignStaticEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<UnassignStaticEntityCmd>(this, "FirstEntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<UnassignStaticEntityCmd>(this, "SecondEntityId", (object) EntityId.Deserialize(reader));
    }

    static UnassignStaticEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnassignStaticEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      UnassignStaticEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
