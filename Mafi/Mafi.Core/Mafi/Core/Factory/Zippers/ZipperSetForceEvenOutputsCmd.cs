// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.ZipperSetForceEvenOutputsCmd
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
  public class ZipperSetForceEvenOutputsCmd : InputCommand
  {
    public readonly EntityId ZipperId;
    public readonly bool ForceEvenOutputs;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ZipperSetForceEvenOutputsCmd(EntityId zipperId, bool forceEvenOutputs)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ZipperId = zipperId;
      this.ForceEvenOutputs = forceEvenOutputs;
    }

    public static void Serialize(ZipperSetForceEvenOutputsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ZipperSetForceEvenOutputsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ZipperSetForceEvenOutputsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.ForceEvenOutputs);
      EntityId.Serialize(this.ZipperId, writer);
    }

    public static ZipperSetForceEvenOutputsCmd Deserialize(BlobReader reader)
    {
      ZipperSetForceEvenOutputsCmd forceEvenOutputsCmd;
      if (reader.TryStartClassDeserialization<ZipperSetForceEvenOutputsCmd>(out forceEvenOutputsCmd))
        reader.EnqueueDataDeserialization((object) forceEvenOutputsCmd, ZipperSetForceEvenOutputsCmd.s_deserializeDataDelayedAction);
      return forceEvenOutputsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ZipperSetForceEvenOutputsCmd>(this, "ForceEvenOutputs", (object) reader.ReadBool());
      reader.SetField<ZipperSetForceEvenOutputsCmd>(this, "ZipperId", (object) EntityId.Deserialize(reader));
    }

    static ZipperSetForceEvenOutputsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ZipperSetForceEvenOutputsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ZipperSetForceEvenOutputsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
