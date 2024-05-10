// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.ToggleMechGeneratorAutoBalanceCmd
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleMechGeneratorAutoBalanceCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId EntityId;

    public static void Serialize(ToggleMechGeneratorAutoBalanceCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleMechGeneratorAutoBalanceCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleMechGeneratorAutoBalanceCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static ToggleMechGeneratorAutoBalanceCmd Deserialize(BlobReader reader)
    {
      ToggleMechGeneratorAutoBalanceCmd generatorAutoBalanceCmd;
      if (reader.TryStartClassDeserialization<ToggleMechGeneratorAutoBalanceCmd>(out generatorAutoBalanceCmd))
        reader.EnqueueDataDeserialization((object) generatorAutoBalanceCmd, ToggleMechGeneratorAutoBalanceCmd.s_deserializeDataDelayedAction);
      return generatorAutoBalanceCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleMechGeneratorAutoBalanceCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    public ToggleMechGeneratorAutoBalanceCmd(MechPowerGeneratorFromProduct entity)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entity.Id;
    }

    public ToggleMechGeneratorAutoBalanceCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
    }

    static ToggleMechGeneratorAutoBalanceCmd()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      ToggleMechGeneratorAutoBalanceCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleMechGeneratorAutoBalanceCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
