// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Datacenters.DataCenterToggleRackCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Datacenters
{
  [GenerateSerializer(false, null, 0)]
  public class DataCenterToggleRackCmd : InputCommand
  {
    public readonly EntityId DataCenterId;
    public readonly Proto.ID ServerRackId;
    /// <summary>Negative if removal, positive if addition.</summary>
    public readonly int Difference;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public DataCenterToggleRackCmd(
      DataCenter dataCenter,
      ServerRackProto serverRack,
      int difference)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(dataCenter.Id, serverRack.Id, difference);
    }

    public DataCenterToggleRackCmd(EntityId dataCenterId, Proto.ID serverRackId, int difference)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DataCenterId = dataCenterId;
      this.ServerRackId = serverRackId;
      this.Difference = difference;
    }

    public static void Serialize(DataCenterToggleRackCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DataCenterToggleRackCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DataCenterToggleRackCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.DataCenterId, writer);
      writer.WriteInt(this.Difference);
      Proto.ID.Serialize(this.ServerRackId, writer);
    }

    public static DataCenterToggleRackCmd Deserialize(BlobReader reader)
    {
      DataCenterToggleRackCmd centerToggleRackCmd;
      if (reader.TryStartClassDeserialization<DataCenterToggleRackCmd>(out centerToggleRackCmd))
        reader.EnqueueDataDeserialization((object) centerToggleRackCmd, DataCenterToggleRackCmd.s_deserializeDataDelayedAction);
      return centerToggleRackCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<DataCenterToggleRackCmd>(this, "DataCenterId", (object) EntityId.Deserialize(reader));
      reader.SetField<DataCenterToggleRackCmd>(this, "Difference", (object) reader.ReadInt());
      reader.SetField<DataCenterToggleRackCmd>(this, "ServerRackId", (object) Proto.ID.Deserialize(reader));
    }

    static DataCenterToggleRackCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DataCenterToggleRackCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      DataCenterToggleRackCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
