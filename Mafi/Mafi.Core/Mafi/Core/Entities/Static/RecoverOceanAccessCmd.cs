// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.RecoverOceanAccessCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GenerateSerializer(false, null, 0)]
  public class RecoverOceanAccessCmd : InputCommand<RecoverOceanResult>
  {
    public readonly EntityId EntityWithOceanAreas;
    public readonly int MaxTilesToRecover;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public RecoverOceanAccessCmd(EntityId entityWithOceanAreas, int maxTilesToRecover)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityWithOceanAreas = entityWithOceanAreas;
      this.MaxTilesToRecover = maxTilesToRecover;
    }

    public static void Serialize(RecoverOceanAccessCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RecoverOceanAccessCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RecoverOceanAccessCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityWithOceanAreas, writer);
      writer.WriteInt(this.MaxTilesToRecover);
    }

    public static RecoverOceanAccessCmd Deserialize(BlobReader reader)
    {
      RecoverOceanAccessCmd recoverOceanAccessCmd;
      if (reader.TryStartClassDeserialization<RecoverOceanAccessCmd>(out recoverOceanAccessCmd))
        reader.EnqueueDataDeserialization((object) recoverOceanAccessCmd, RecoverOceanAccessCmd.s_deserializeDataDelayedAction);
      return recoverOceanAccessCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RecoverOceanAccessCmd>(this, "EntityWithOceanAreas", (object) EntityId.Deserialize(reader));
      reader.SetField<RecoverOceanAccessCmd>(this, "MaxTilesToRecover", (object) reader.ReadInt());
    }

    static RecoverOceanAccessCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RecoverOceanAccessCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<RecoverOceanResult>) obj).SerializeData(writer));
      RecoverOceanAccessCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<RecoverOceanResult>) obj).DeserializeData(reader));
    }
  }
}
