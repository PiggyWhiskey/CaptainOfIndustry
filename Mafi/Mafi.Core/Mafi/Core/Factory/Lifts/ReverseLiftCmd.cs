// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Lifts.ReverseLiftCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Lifts
{
  [GenerateSerializer(false, null, 0)]
  public class ReverseLiftCmd : InputCommand
  {
    public readonly EntityId LiftId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ReverseLiftCmd(Lift lift)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LiftId = lift.Id;
    }

    public ReverseLiftCmd(EntityId liftId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LiftId = liftId;
    }

    public static void Serialize(ReverseLiftCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ReverseLiftCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ReverseLiftCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.LiftId, writer);
    }

    public static ReverseLiftCmd Deserialize(BlobReader reader)
    {
      ReverseLiftCmd reverseLiftCmd;
      if (reader.TryStartClassDeserialization<ReverseLiftCmd>(out reverseLiftCmd))
        reader.EnqueueDataDeserialization((object) reverseLiftCmd, ReverseLiftCmd.s_deserializeDataDelayedAction);
      return reverseLiftCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ReverseLiftCmd>(this, "LiftId", (object) EntityId.Deserialize(reader));
    }

    static ReverseLiftCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ReverseLiftCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ReverseLiftCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
