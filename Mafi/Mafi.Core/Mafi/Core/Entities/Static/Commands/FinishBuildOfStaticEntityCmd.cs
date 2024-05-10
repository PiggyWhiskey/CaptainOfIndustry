// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.FinishBuildOfStaticEntityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Commands
{
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public class FinishBuildOfStaticEntityCmd : InputCommand<EntityId>
  {
    public readonly EntityId EntityId;
    /// <summary>
    /// Whether this should be paid for with unity. Otherwise it serves as a cheat basically.
    /// </summary>
    public readonly bool PayWithUnity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public FinishBuildOfStaticEntityCmd(EntityId id, bool payWithUnity = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = id;
      this.PayWithUnity = payWithUnity;
    }

    public static void Serialize(FinishBuildOfStaticEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FinishBuildOfStaticEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FinishBuildOfStaticEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteBool(this.PayWithUnity);
    }

    public static FinishBuildOfStaticEntityCmd Deserialize(BlobReader reader)
    {
      FinishBuildOfStaticEntityCmd ofStaticEntityCmd;
      if (reader.TryStartClassDeserialization<FinishBuildOfStaticEntityCmd>(out ofStaticEntityCmd))
        reader.EnqueueDataDeserialization((object) ofStaticEntityCmd, FinishBuildOfStaticEntityCmd.s_deserializeDataDelayedAction);
      return ofStaticEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FinishBuildOfStaticEntityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<FinishBuildOfStaticEntityCmd>(this, "PayWithUnity", (object) reader.ReadBool());
    }

    static FinishBuildOfStaticEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FinishBuildOfStaticEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<EntityId>) obj).SerializeData(writer));
      FinishBuildOfStaticEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<EntityId>) obj).DeserializeData(reader));
    }
  }
}
