// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.NuclearReactors.NuclearReactorToggleAutomaticRegulationCmd
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
  public class NuclearReactorToggleAutomaticRegulationCmd : InputCommand
  {
    public readonly EntityId ReactorId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public NuclearReactorToggleAutomaticRegulationCmd(NuclearReactor reactor)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(reactor.Id);
    }

    public NuclearReactorToggleAutomaticRegulationCmd(EntityId reactorId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ReactorId = reactorId;
    }

    public static void Serialize(
      NuclearReactorToggleAutomaticRegulationCmd value,
      BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NuclearReactorToggleAutomaticRegulationCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NuclearReactorToggleAutomaticRegulationCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.ReactorId, writer);
    }

    public static NuclearReactorToggleAutomaticRegulationCmd Deserialize(BlobReader reader)
    {
      NuclearReactorToggleAutomaticRegulationCmd automaticRegulationCmd;
      if (reader.TryStartClassDeserialization<NuclearReactorToggleAutomaticRegulationCmd>(out automaticRegulationCmd))
        reader.EnqueueDataDeserialization((object) automaticRegulationCmd, NuclearReactorToggleAutomaticRegulationCmd.s_deserializeDataDelayedAction);
      return automaticRegulationCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<NuclearReactorToggleAutomaticRegulationCmd>(this, "ReactorId", (object) EntityId.Deserialize(reader));
    }

    static NuclearReactorToggleAutomaticRegulationCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NuclearReactorToggleAutomaticRegulationCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      NuclearReactorToggleAutomaticRegulationCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
