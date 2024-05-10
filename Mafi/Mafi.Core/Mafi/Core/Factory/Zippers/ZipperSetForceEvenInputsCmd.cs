// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.ZipperSetForceEvenInputsCmd
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
  public class ZipperSetForceEvenInputsCmd : InputCommand
  {
    public readonly EntityId ZipperId;
    public readonly bool ForceEvenInputs;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ZipperSetForceEvenInputsCmd(EntityId zipperId, bool forceEvenInputs)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ZipperId = zipperId;
      this.ForceEvenInputs = forceEvenInputs;
    }

    public static void Serialize(ZipperSetForceEvenInputsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ZipperSetForceEvenInputsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ZipperSetForceEvenInputsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.ForceEvenInputs);
      EntityId.Serialize(this.ZipperId, writer);
    }

    public static ZipperSetForceEvenInputsCmd Deserialize(BlobReader reader)
    {
      ZipperSetForceEvenInputsCmd forceEvenInputsCmd;
      if (reader.TryStartClassDeserialization<ZipperSetForceEvenInputsCmd>(out forceEvenInputsCmd))
        reader.EnqueueDataDeserialization((object) forceEvenInputsCmd, ZipperSetForceEvenInputsCmd.s_deserializeDataDelayedAction);
      return forceEvenInputsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ZipperSetForceEvenInputsCmd>(this, "ForceEvenInputs", (object) reader.ReadBool());
      reader.SetField<ZipperSetForceEvenInputsCmd>(this, "ZipperId", (object) EntityId.Deserialize(reader));
    }

    static ZipperSetForceEvenInputsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ZipperSetForceEvenInputsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ZipperSetForceEvenInputsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
