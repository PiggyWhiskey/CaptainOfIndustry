// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.RetainingWallEntity
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
  public class RetainingWallEntity : LayoutEntityBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly RetainingWallProto Prototype;

    public static void Serialize(RetainingWallEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RetainingWallEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RetainingWallEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<RetainingWallProto>(this.Prototype);
    }

    public static RetainingWallEntity Deserialize(BlobReader reader)
    {
      RetainingWallEntity retainingWallEntity;
      if (reader.TryStartClassDeserialization<RetainingWallEntity>(out retainingWallEntity))
        reader.EnqueueDataDeserialization((object) retainingWallEntity, RetainingWallEntity.s_deserializeDataDelayedAction);
      return retainingWallEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RetainingWallEntity>(this, "Prototype", (object) reader.ReadGenericAs<RetainingWallProto>());
    }

    public override bool CanBePaused => false;

    public RetainingWallEntity(
      EntityId id,
      RetainingWallProto proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
    }

    static RetainingWallEntity()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      RetainingWallEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RetainingWallEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
