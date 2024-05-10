// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.NuclearReactors.NuclearReactorSetPowerLevelCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.NuclearReactors
{
  [GenerateSerializer(false, null, 0)]
  public class NuclearReactorSetPowerLevelCmd : InputCommand
  {
    public readonly EntityId ReactorId;
    public readonly int PowerLevel;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public NuclearReactorSetPowerLevelCmd(NuclearReactor reactor, int powerLevel)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(reactor.Id, powerLevel);
    }

    public NuclearReactorSetPowerLevelCmd(EntityId reactorId, int powerLevel)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ReactorId = reactorId;
      this.PowerLevel = powerLevel;
    }

    public static void Serialize(NuclearReactorSetPowerLevelCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NuclearReactorSetPowerLevelCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NuclearReactorSetPowerLevelCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.PowerLevel);
      EntityId.Serialize(this.ReactorId, writer);
    }

    public static NuclearReactorSetPowerLevelCmd Deserialize(BlobReader reader)
    {
      NuclearReactorSetPowerLevelCmd setPowerLevelCmd;
      if (reader.TryStartClassDeserialization<NuclearReactorSetPowerLevelCmd>(out setPowerLevelCmd))
        reader.EnqueueDataDeserialization((object) setPowerLevelCmd, NuclearReactorSetPowerLevelCmd.s_deserializeDataDelayedAction);
      return setPowerLevelCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<NuclearReactorSetPowerLevelCmd>(this, "PowerLevel", (object) reader.ReadInt());
      reader.SetField<NuclearReactorSetPowerLevelCmd>(this, "ReactorId", (object) EntityId.Deserialize(reader));
    }

    static NuclearReactorSetPowerLevelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NuclearReactorSetPowerLevelCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      NuclearReactorSetPowerLevelCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
