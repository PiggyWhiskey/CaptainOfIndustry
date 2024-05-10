// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.SetPlanningModeEnabledCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class SetPlanningModeEnabledCmd : InputCommand
  {
    public readonly bool IsEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetPlanningModeEnabledCmd(bool isEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.IsEnabled = isEnabled;
    }

    public static void Serialize(SetPlanningModeEnabledCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetPlanningModeEnabledCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetPlanningModeEnabledCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsEnabled);
    }

    public static SetPlanningModeEnabledCmd Deserialize(BlobReader reader)
    {
      SetPlanningModeEnabledCmd planningModeEnabledCmd;
      if (reader.TryStartClassDeserialization<SetPlanningModeEnabledCmd>(out planningModeEnabledCmd))
        reader.EnqueueDataDeserialization((object) planningModeEnabledCmd, SetPlanningModeEnabledCmd.s_deserializeDataDelayedAction);
      return planningModeEnabledCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetPlanningModeEnabledCmd>(this, "IsEnabled", (object) reader.ReadBool());
    }

    static SetPlanningModeEnabledCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetPlanningModeEnabledCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetPlanningModeEnabledCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
