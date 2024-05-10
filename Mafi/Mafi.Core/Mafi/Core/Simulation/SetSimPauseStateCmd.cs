// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Simulation.SetSimPauseStateCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Simulation
{
  /// <summary>Pauses or resumes the game.</summary>
  [GenerateSerializer(false, null, 0)]
  public class SetSimPauseStateCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly bool IsPaused;

    public static void Serialize(SetSimPauseStateCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetSimPauseStateCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetSimPauseStateCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsPaused);
    }

    public static SetSimPauseStateCmd Deserialize(BlobReader reader)
    {
      SetSimPauseStateCmd simPauseStateCmd;
      if (reader.TryStartClassDeserialization<SetSimPauseStateCmd>(out simPauseStateCmd))
        reader.EnqueueDataDeserialization((object) simPauseStateCmd, SetSimPauseStateCmd.s_deserializeDataDelayedAction);
      return simPauseStateCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetSimPauseStateCmd>(this, "IsPaused", (object) reader.ReadBool());
    }

    public override bool AffectsSaveState => false;

    public SetSimPauseStateCmd(bool isPaused)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.IsPaused = isPaused;
    }

    static SetSimPauseStateCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetSimPauseStateCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetSimPauseStateCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
