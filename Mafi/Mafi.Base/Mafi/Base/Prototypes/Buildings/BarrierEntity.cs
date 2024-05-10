// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.BarrierEntity
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  [GenerateSerializer(false, null, 0)]
  public sealed class BarrierEntity : LayoutEntityBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly BarrierProto Prototype;

    public static void Serialize(BarrierEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<BarrierEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, BarrierEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<BarrierProto>(this.Prototype);
    }

    public static BarrierEntity Deserialize(BlobReader reader)
    {
      BarrierEntity barrierEntity;
      if (reader.TryStartClassDeserialization<BarrierEntity>(out barrierEntity))
        reader.EnqueueDataDeserialization((object) barrierEntity, BarrierEntity.s_deserializeDataDelayedAction);
      return barrierEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<BarrierEntity>(this, "Prototype", (object) reader.ReadGenericAs<BarrierProto>());
    }

    public override bool CanBePaused => false;

    public BarrierEntity(
      EntityId id,
      BarrierProto proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
    }

    static BarrierEntity()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      BarrierEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      BarrierEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
