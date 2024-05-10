// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.BatchCreateStaticEntitiesCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class BatchCreateStaticEntitiesCmd : InputCommand
  {
    public readonly ImmutableArray<EntityConfigData> ConfigData;
    /// <summary>
    /// Whether we should automatically connect to transports with minizippers.
    /// </summary>
    [NewInSaveVersion(140, "BuildMiniZippersMode.DeferToProto", null, null, null)]
    public readonly BuildMiniZippersMode BuildMiniZippers;
    /// <summary>
    /// Whether this entity was created as part of the initial game setup.
    /// </summary>
    public readonly bool IsFree;
    public readonly bool ApplyConfiguration;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public BatchCreateStaticEntitiesCmd(
      ImmutableArray<EntityConfigData> configData,
      BuildMiniZippersMode buildMiniZippers = BuildMiniZippersMode.DeferToProto,
      bool isFree = false,
      bool applyConfiguration = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigData = configData;
      this.BuildMiniZippers = buildMiniZippers;
      this.IsFree = isFree;
      this.ApplyConfiguration = applyConfiguration;
    }

    public static void Serialize(BatchCreateStaticEntitiesCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BatchCreateStaticEntitiesCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BatchCreateStaticEntitiesCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.ApplyConfiguration);
      writer.WriteInt((int) this.BuildMiniZippers);
      ImmutableArray<EntityConfigData>.Serialize(this.ConfigData, writer);
      writer.WriteBool(this.IsFree);
    }

    public static BatchCreateStaticEntitiesCmd Deserialize(BlobReader reader)
    {
      BatchCreateStaticEntitiesCmd staticEntitiesCmd;
      if (reader.TryStartClassDeserialization<BatchCreateStaticEntitiesCmd>(out staticEntitiesCmd))
        reader.EnqueueDataDeserialization((object) staticEntitiesCmd, BatchCreateStaticEntitiesCmd.s_deserializeDataDelayedAction);
      return staticEntitiesCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<BatchCreateStaticEntitiesCmd>(this, "ApplyConfiguration", (object) reader.ReadBool());
      reader.SetField<BatchCreateStaticEntitiesCmd>(this, "BuildMiniZippers", (object) (BuildMiniZippersMode) (reader.LoadedSaveVersion >= 140 ? reader.ReadInt() : 0));
      reader.SetField<BatchCreateStaticEntitiesCmd>(this, "ConfigData", (object) ImmutableArray<EntityConfigData>.Deserialize(reader));
      reader.SetField<BatchCreateStaticEntitiesCmd>(this, "IsFree", (object) reader.ReadBool());
    }

    static BatchCreateStaticEntitiesCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BatchCreateStaticEntitiesCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      BatchCreateStaticEntitiesCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
