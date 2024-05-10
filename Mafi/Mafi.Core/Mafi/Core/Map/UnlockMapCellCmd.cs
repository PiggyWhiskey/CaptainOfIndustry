// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.UnlockMapCellCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public class UnlockMapCellCmd : InputCommand
  {
    public readonly MapCellId MapCellId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public UnlockMapCellCmd(MapCellId mapCellId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MapCellId = mapCellId;
    }

    public static void Serialize(UnlockMapCellCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnlockMapCellCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnlockMapCellCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      MapCellId.Serialize(this.MapCellId, writer);
    }

    public static UnlockMapCellCmd Deserialize(BlobReader reader)
    {
      UnlockMapCellCmd unlockMapCellCmd;
      if (reader.TryStartClassDeserialization<UnlockMapCellCmd>(out unlockMapCellCmd))
        reader.EnqueueDataDeserialization((object) unlockMapCellCmd, UnlockMapCellCmd.s_deserializeDataDelayedAction);
      return unlockMapCellCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<UnlockMapCellCmd>(this, "MapCellId", (object) MapCellId.Deserialize(reader));
    }

    static UnlockMapCellCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnlockMapCellCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      UnlockMapCellCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
