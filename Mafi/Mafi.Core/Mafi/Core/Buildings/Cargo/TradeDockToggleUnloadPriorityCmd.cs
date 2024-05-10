// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.TradeDockToggleUnloadPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [GenerateSerializer(false, null, 0)]
  public class TradeDockToggleUnloadPriorityCmd : InputCommand
  {
    public readonly EntityId TradeDockId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public TradeDockToggleUnloadPriorityCmd(EntityId tradeDockId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TradeDockId = tradeDockId;
    }

    public static void Serialize(TradeDockToggleUnloadPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TradeDockToggleUnloadPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TradeDockToggleUnloadPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.TradeDockId, writer);
    }

    public static TradeDockToggleUnloadPriorityCmd Deserialize(BlobReader reader)
    {
      TradeDockToggleUnloadPriorityCmd unloadPriorityCmd;
      if (reader.TryStartClassDeserialization<TradeDockToggleUnloadPriorityCmd>(out unloadPriorityCmd))
        reader.EnqueueDataDeserialization((object) unloadPriorityCmd, TradeDockToggleUnloadPriorityCmd.s_deserializeDataDelayedAction);
      return unloadPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<TradeDockToggleUnloadPriorityCmd>(this, "TradeDockId", (object) EntityId.Deserialize(reader));
    }

    static TradeDockToggleUnloadPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TradeDockToggleUnloadPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      TradeDockToggleUnloadPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
