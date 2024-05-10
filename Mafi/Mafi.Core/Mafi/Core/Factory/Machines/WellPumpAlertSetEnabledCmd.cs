// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.WellPumpAlertSetEnabledCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  [GenerateSerializer(false, null, 0)]
  public class WellPumpAlertSetEnabledCmd : InputCommand
  {
    public readonly EntityId WellPumpId;
    public readonly bool IsEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public WellPumpAlertSetEnabledCmd(EntityId wellPumpId, bool isEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.WellPumpId = wellPumpId;
      this.IsEnabled = isEnabled;
    }

    public static void Serialize(WellPumpAlertSetEnabledCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WellPumpAlertSetEnabledCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WellPumpAlertSetEnabledCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsEnabled);
      EntityId.Serialize(this.WellPumpId, writer);
    }

    public static WellPumpAlertSetEnabledCmd Deserialize(BlobReader reader)
    {
      WellPumpAlertSetEnabledCmd alertSetEnabledCmd;
      if (reader.TryStartClassDeserialization<WellPumpAlertSetEnabledCmd>(out alertSetEnabledCmd))
        reader.EnqueueDataDeserialization((object) alertSetEnabledCmd, WellPumpAlertSetEnabledCmd.s_deserializeDataDelayedAction);
      return alertSetEnabledCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WellPumpAlertSetEnabledCmd>(this, "IsEnabled", (object) reader.ReadBool());
      reader.SetField<WellPumpAlertSetEnabledCmd>(this, "WellPumpId", (object) EntityId.Deserialize(reader));
    }

    static WellPumpAlertSetEnabledCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WellPumpAlertSetEnabledCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      WellPumpAlertSetEnabledCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
