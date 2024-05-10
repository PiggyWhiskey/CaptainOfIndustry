// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Loans.SetLoanBufferPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.Loans
{
  [GenerateSerializer(false, null, 0)]
  public class SetLoanBufferPriorityCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly uint LoanId;
    public readonly int Priority;

    public static void Serialize(SetLoanBufferPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetLoanBufferPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetLoanBufferPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteUInt(this.LoanId);
      writer.WriteInt(this.Priority);
    }

    public static SetLoanBufferPriorityCmd Deserialize(BlobReader reader)
    {
      SetLoanBufferPriorityCmd bufferPriorityCmd;
      if (reader.TryStartClassDeserialization<SetLoanBufferPriorityCmd>(out bufferPriorityCmd))
        reader.EnqueueDataDeserialization((object) bufferPriorityCmd, SetLoanBufferPriorityCmd.s_deserializeDataDelayedAction);
      return bufferPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetLoanBufferPriorityCmd>(this, "LoanId", (object) reader.ReadUInt());
      reader.SetField<SetLoanBufferPriorityCmd>(this, "Priority", (object) reader.ReadInt());
    }

    public SetLoanBufferPriorityCmd(uint loanId, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LoanId = loanId;
      this.Priority = priority;
    }

    static SetLoanBufferPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetLoanBufferPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetLoanBufferPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
