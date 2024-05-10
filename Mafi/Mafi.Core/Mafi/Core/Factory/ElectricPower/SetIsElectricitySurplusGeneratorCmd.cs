// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.SetIsElectricitySurplusGeneratorCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [GenerateSerializer(false, null, 0)]
  public class SetIsElectricitySurplusGeneratorCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly bool IsSurplusGenerator;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetIsElectricitySurplusGeneratorCmd(
      IElectricityGeneratingEntity entity,
      bool isSurplusGenerator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, isSurplusGenerator);
    }

    public SetIsElectricitySurplusGeneratorCmd(EntityId entityId, bool isSurplusGenerator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.IsSurplusGenerator = isSurplusGenerator;
    }

    public static void Serialize(SetIsElectricitySurplusGeneratorCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetIsElectricitySurplusGeneratorCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetIsElectricitySurplusGeneratorCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteBool(this.IsSurplusGenerator);
    }

    public static SetIsElectricitySurplusGeneratorCmd Deserialize(BlobReader reader)
    {
      SetIsElectricitySurplusGeneratorCmd surplusGeneratorCmd;
      if (reader.TryStartClassDeserialization<SetIsElectricitySurplusGeneratorCmd>(out surplusGeneratorCmd))
        reader.EnqueueDataDeserialization((object) surplusGeneratorCmd, SetIsElectricitySurplusGeneratorCmd.s_deserializeDataDelayedAction);
      return surplusGeneratorCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetIsElectricitySurplusGeneratorCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetIsElectricitySurplusGeneratorCmd>(this, "IsSurplusGenerator", (object) reader.ReadBool());
    }

    static SetIsElectricitySurplusGeneratorCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetIsElectricitySurplusGeneratorCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetIsElectricitySurplusGeneratorCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
