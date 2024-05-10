// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Contracts.ToggleContractCmd
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
namespace Mafi.Core.World.Contracts
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleContractCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Proto.ID ContractId;

    public static void Serialize(ToggleContractCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleContractCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleContractCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Proto.ID.Serialize(this.ContractId, writer);
    }

    public static ToggleContractCmd Deserialize(BlobReader reader)
    {
      ToggleContractCmd toggleContractCmd;
      if (reader.TryStartClassDeserialization<ToggleContractCmd>(out toggleContractCmd))
        reader.EnqueueDataDeserialization((object) toggleContractCmd, ToggleContractCmd.s_deserializeDataDelayedAction);
      return toggleContractCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleContractCmd>(this, "ContractId", (object) Proto.ID.Deserialize(reader));
    }

    public ToggleContractCmd(Proto.ID contractId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ContractId = contractId;
    }

    static ToggleContractCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleContractCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleContractCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
