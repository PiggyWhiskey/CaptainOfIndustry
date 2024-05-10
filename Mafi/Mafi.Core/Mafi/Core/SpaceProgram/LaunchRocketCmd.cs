// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.LaunchRocketCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.SpaceProgram
{
  [GenerateSerializer(false, null, 0)]
  public class LaunchRocketCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId LaunchPadId;

    public static void Serialize(LaunchRocketCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LaunchRocketCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LaunchRocketCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.LaunchPadId, writer);
    }

    public static LaunchRocketCmd Deserialize(BlobReader reader)
    {
      LaunchRocketCmd launchRocketCmd;
      if (reader.TryStartClassDeserialization<LaunchRocketCmd>(out launchRocketCmd))
        reader.EnqueueDataDeserialization((object) launchRocketCmd, LaunchRocketCmd.s_deserializeDataDelayedAction);
      return launchRocketCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<LaunchRocketCmd>(this, "LaunchPadId", (object) EntityId.Deserialize(reader));
    }

    public LaunchRocketCmd(EntityId launchPadId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LaunchPadId = launchPadId;
    }

    public LaunchRocketCmd(RocketLaunchPad launchPad)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LaunchPadId = launchPad.Id;
    }

    static LaunchRocketCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LaunchRocketCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      LaunchRocketCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
