// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.GoToLocationCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  public class GoToLocationCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly WorldMapLocId LocationId;
    public readonly LocationVisitReason Reason;

    public static void Serialize(GoToLocationCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoToLocationCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoToLocationCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      WorldMapLocId.Serialize(this.LocationId, writer);
      writer.WriteInt((int) this.Reason);
    }

    public static GoToLocationCmd Deserialize(BlobReader reader)
    {
      GoToLocationCmd goToLocationCmd;
      if (reader.TryStartClassDeserialization<GoToLocationCmd>(out goToLocationCmd))
        reader.EnqueueDataDeserialization((object) goToLocationCmd, GoToLocationCmd.s_deserializeDataDelayedAction);
      return goToLocationCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoToLocationCmd>(this, "LocationId", (object) WorldMapLocId.Deserialize(reader));
      reader.SetField<GoToLocationCmd>(this, "Reason", (object) (LocationVisitReason) reader.ReadInt());
    }

    public GoToLocationCmd(WorldMapLocId id, LocationVisitReason reason)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LocationId = id;
      this.Reason = reason;
    }

    static GoToLocationCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoToLocationCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      GoToLocationCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
