// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.ZipperSetPriorityPortsCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  [GenerateSerializer(false, null, 0)]
  public class ZipperSetPriorityPortsCmd : InputCommand
  {
    public readonly EntityId ZipperId;
    public readonly char PortName;
    /// <summary>Acts like toggle if null.</summary>
    public readonly bool? IsPrioritized;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ZipperSetPriorityPortsCmd(EntityId zipperId, char portName, bool? isPrioritized)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ZipperId = zipperId;
      this.PortName = portName;
      this.IsPrioritized = isPrioritized;
    }

    public static void Serialize(ZipperSetPriorityPortsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ZipperSetPriorityPortsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ZipperSetPriorityPortsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteNullableStruct<bool>(this.IsPrioritized);
      writer.WriteChar(this.PortName);
      EntityId.Serialize(this.ZipperId, writer);
    }

    public static ZipperSetPriorityPortsCmd Deserialize(BlobReader reader)
    {
      ZipperSetPriorityPortsCmd priorityPortsCmd;
      if (reader.TryStartClassDeserialization<ZipperSetPriorityPortsCmd>(out priorityPortsCmd))
        reader.EnqueueDataDeserialization((object) priorityPortsCmd, ZipperSetPriorityPortsCmd.s_deserializeDataDelayedAction);
      return priorityPortsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ZipperSetPriorityPortsCmd>(this, "IsPrioritized", (object) reader.ReadNullableStruct<bool>());
      reader.SetField<ZipperSetPriorityPortsCmd>(this, "PortName", (object) reader.ReadChar());
      reader.SetField<ZipperSetPriorityPortsCmd>(this, "ZipperId", (object) EntityId.Deserialize(reader));
    }

    static ZipperSetPriorityPortsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ZipperSetPriorityPortsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ZipperSetPriorityPortsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
