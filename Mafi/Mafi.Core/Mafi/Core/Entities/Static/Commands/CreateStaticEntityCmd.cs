// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.CreateStaticEntityCmd
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
  public class CreateStaticEntityCmd : InputCommand<EntityId>
  {
    public readonly StaticEntityProto.ID ProtoId;
    public readonly TileTransform Transform;
    /// <summary>
    /// Whether this entity was created as part of the initial game setup.
    /// </summary>
    public readonly bool IsFree;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CreateStaticEntityCmd(
      StaticEntityProto.ID protoId,
      TileTransform transform,
      bool isFree = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
      this.Transform = transform;
      this.IsFree = isFree;
    }

    public static void Serialize(CreateStaticEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CreateStaticEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CreateStaticEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsFree);
      StaticEntityProto.ID.Serialize(this.ProtoId, writer);
      TileTransform.Serialize(this.Transform, writer);
    }

    public static CreateStaticEntityCmd Deserialize(BlobReader reader)
    {
      CreateStaticEntityCmd createStaticEntityCmd;
      if (reader.TryStartClassDeserialization<CreateStaticEntityCmd>(out createStaticEntityCmd))
        reader.EnqueueDataDeserialization((object) createStaticEntityCmd, CreateStaticEntityCmd.s_deserializeDataDelayedAction);
      return createStaticEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CreateStaticEntityCmd>(this, "IsFree", (object) reader.ReadBool());
      reader.SetField<CreateStaticEntityCmd>(this, "ProtoId", (object) StaticEntityProto.ID.Deserialize(reader));
      reader.SetField<CreateStaticEntityCmd>(this, "Transform", (object) TileTransform.Deserialize(reader));
    }

    static CreateStaticEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CreateStaticEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<EntityId>) obj).SerializeData(writer));
      CreateStaticEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<EntityId>) obj).DeserializeData(reader));
    }
  }
}
