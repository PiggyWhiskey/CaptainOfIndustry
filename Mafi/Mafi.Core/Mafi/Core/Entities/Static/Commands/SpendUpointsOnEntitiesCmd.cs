// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.SpendUpointsOnEntitiesCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Commands
{
  /// <summary>
  /// Will try to "smartly" apply upoints change on given entities.
  /// Toggles boost, or quick deliver / remove products.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class SpendUpointsOnEntitiesCmd : InputCommand
  {
    public readonly ImmutableArray<EntityId> EntityIds;
    [NewInSaveVersion(156, null, null, null, null)]
    public readonly RectangleTerrainArea2i? Area;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SpendUpointsOnEntitiesCmd(
      ImmutableArray<EntityId> entityIds,
      RectangleTerrainArea2i? area)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityIds = entityIds;
      this.Area = area;
    }

    public static void Serialize(SpendUpointsOnEntitiesCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SpendUpointsOnEntitiesCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SpendUpointsOnEntitiesCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteNullableStruct<RectangleTerrainArea2i>(this.Area);
      ImmutableArray<EntityId>.Serialize(this.EntityIds, writer);
    }

    public static SpendUpointsOnEntitiesCmd Deserialize(BlobReader reader)
    {
      SpendUpointsOnEntitiesCmd upointsOnEntitiesCmd;
      if (reader.TryStartClassDeserialization<SpendUpointsOnEntitiesCmd>(out upointsOnEntitiesCmd))
        reader.EnqueueDataDeserialization((object) upointsOnEntitiesCmd, SpendUpointsOnEntitiesCmd.s_deserializeDataDelayedAction);
      return upointsOnEntitiesCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SpendUpointsOnEntitiesCmd>(this, "Area", (object) (reader.LoadedSaveVersion >= 156 ? reader.ReadNullableStruct<RectangleTerrainArea2i>() : new RectangleTerrainArea2i?()));
      reader.SetField<SpendUpointsOnEntitiesCmd>(this, "EntityIds", (object) ImmutableArray<EntityId>.Deserialize(reader));
    }

    static SpendUpointsOnEntitiesCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SpendUpointsOnEntitiesCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SpendUpointsOnEntitiesCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
