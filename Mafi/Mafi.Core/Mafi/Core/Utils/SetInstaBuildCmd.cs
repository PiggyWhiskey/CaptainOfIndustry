// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.SetInstaBuildCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  [GenerateSerializer(false, null, 0)]
  public class SetInstaBuildCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly bool SetEnabled;

    public static void Serialize(SetInstaBuildCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetInstaBuildCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetInstaBuildCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.SetEnabled);
    }

    public static SetInstaBuildCmd Deserialize(BlobReader reader)
    {
      SetInstaBuildCmd setInstaBuildCmd;
      if (reader.TryStartClassDeserialization<SetInstaBuildCmd>(out setInstaBuildCmd))
        reader.EnqueueDataDeserialization((object) setInstaBuildCmd, SetInstaBuildCmd.s_deserializeDataDelayedAction);
      return setInstaBuildCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetInstaBuildCmd>(this, "SetEnabled", (object) reader.ReadBool());
    }

    public SetInstaBuildCmd(bool setEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetEnabled = setEnabled;
    }

    static SetInstaBuildCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetInstaBuildCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetInstaBuildCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
