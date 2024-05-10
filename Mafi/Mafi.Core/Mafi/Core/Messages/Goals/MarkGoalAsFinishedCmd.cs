// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.MarkGoalAsFinishedCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class MarkGoalAsFinishedCmd : InputCommand
  {
    public Mafi.Core.Prototypes.Proto.ID ProtoId;
    public readonly bool GoalWasSkipped;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MarkGoalAsFinishedCmd(Mafi.Core.Prototypes.Proto.ID protoId, bool goalWasSkipped = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
      this.GoalWasSkipped = goalWasSkipped;
    }

    public static void Serialize(MarkGoalAsFinishedCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MarkGoalAsFinishedCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MarkGoalAsFinishedCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.GoalWasSkipped);
      Mafi.Core.Prototypes.Proto.ID.Serialize(this.ProtoId, writer);
    }

    public static MarkGoalAsFinishedCmd Deserialize(BlobReader reader)
    {
      MarkGoalAsFinishedCmd goalAsFinishedCmd;
      if (reader.TryStartClassDeserialization<MarkGoalAsFinishedCmd>(out goalAsFinishedCmd))
        reader.EnqueueDataDeserialization((object) goalAsFinishedCmd, MarkGoalAsFinishedCmd.s_deserializeDataDelayedAction);
      return goalAsFinishedCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MarkGoalAsFinishedCmd>(this, "GoalWasSkipped", (object) reader.ReadBool());
      this.ProtoId = Mafi.Core.Prototypes.Proto.ID.Deserialize(reader);
    }

    static MarkGoalAsFinishedCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MarkGoalAsFinishedCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MarkGoalAsFinishedCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
