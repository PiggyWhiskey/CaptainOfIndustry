// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.SetRocketAutoLaunchCmd
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
  public class SetRocketAutoLaunchCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId LaunchPadId;
    public readonly bool AutoLaunch;

    public static void Serialize(SetRocketAutoLaunchCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetRocketAutoLaunchCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetRocketAutoLaunchCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.AutoLaunch);
      EntityId.Serialize(this.LaunchPadId, writer);
    }

    public static SetRocketAutoLaunchCmd Deserialize(BlobReader reader)
    {
      SetRocketAutoLaunchCmd rocketAutoLaunchCmd;
      if (reader.TryStartClassDeserialization<SetRocketAutoLaunchCmd>(out rocketAutoLaunchCmd))
        reader.EnqueueDataDeserialization((object) rocketAutoLaunchCmd, SetRocketAutoLaunchCmd.s_deserializeDataDelayedAction);
      return rocketAutoLaunchCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetRocketAutoLaunchCmd>(this, "AutoLaunch", (object) reader.ReadBool());
      reader.SetField<SetRocketAutoLaunchCmd>(this, "LaunchPadId", (object) EntityId.Deserialize(reader));
    }

    public SetRocketAutoLaunchCmd(EntityId launchPadId, bool autoLaunch)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LaunchPadId = launchPadId;
      this.AutoLaunch = autoLaunch;
    }

    public SetRocketAutoLaunchCmd(RocketLaunchPad launchPad, bool autoLaunch)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LaunchPadId = launchPad.Id;
      this.AutoLaunch = autoLaunch;
    }

    static SetRocketAutoLaunchCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetRocketAutoLaunchCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetRocketAutoLaunchCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
