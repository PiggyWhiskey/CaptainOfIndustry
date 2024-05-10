// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.AssignStaticEntityCmd
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
  public class AssignStaticEntityCmd : InputCommand
  {
    public readonly EntityId FirstEntityId;
    public readonly EntityId SecondEntityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public AssignStaticEntityCmd(IEntityAssignedAsOutput output, IEntityAssignedAsInput input)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FirstEntityId = output.Id;
      this.SecondEntityId = input.Id;
    }

    public static void Serialize(AssignStaticEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AssignStaticEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AssignStaticEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.FirstEntityId, writer);
      EntityId.Serialize(this.SecondEntityId, writer);
    }

    public static AssignStaticEntityCmd Deserialize(BlobReader reader)
    {
      AssignStaticEntityCmd assignStaticEntityCmd;
      if (reader.TryStartClassDeserialization<AssignStaticEntityCmd>(out assignStaticEntityCmd))
        reader.EnqueueDataDeserialization((object) assignStaticEntityCmd, AssignStaticEntityCmd.s_deserializeDataDelayedAction);
      return assignStaticEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AssignStaticEntityCmd>(this, "FirstEntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<AssignStaticEntityCmd>(this, "SecondEntityId", (object) EntityId.Deserialize(reader));
    }

    static AssignStaticEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssignStaticEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AssignStaticEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
