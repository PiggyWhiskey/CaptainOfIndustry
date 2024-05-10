// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapLocation
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Fleet;
using Mafi.Core.World.Entities;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  public class WorldMapLocation
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(WorldMapLocation value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapLocation>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapLocation.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Option<BattleFleet>.Serialize(this.Enemy, writer);
      Option<IWorldMapEntity>.Serialize(this.Entity, writer);
      Option<WorldMapEntityProto>.Serialize(this.EntityProto, writer);
      Option<WorldMapLocationGfxProto>.Serialize(this.Graphics, writer);
      WorldMapLocId.Serialize(this.Id, writer);
      writer.WriteBool(this.IsEnemyKnown);
      writer.WriteBool(this.IsScannedByRadar);
      Option<WorldMapLoot>.Serialize(this.Loot, writer);
      writer.WriteString(this.Name);
      Vector2i.Serialize(this.Position, writer);
      writer.WriteInt((int) this.State);
    }

    public static WorldMapLocation Deserialize(BlobReader reader)
    {
      WorldMapLocation worldMapLocation;
      if (reader.TryStartClassDeserialization<WorldMapLocation>(out worldMapLocation))
        reader.EnqueueDataDeserialization((object) worldMapLocation, WorldMapLocation.s_deserializeDataDelayedAction);
      return worldMapLocation;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Enemy = Option<BattleFleet>.Deserialize(reader);
      this.Entity = Option<IWorldMapEntity>.Deserialize(reader);
      this.EntityProto = Option<WorldMapEntityProto>.Deserialize(reader);
      this.Graphics = Option<WorldMapLocationGfxProto>.Deserialize(reader);
      this.Id = WorldMapLocId.Deserialize(reader);
      this.IsEnemyKnown = reader.ReadBool();
      this.IsScannedByRadar = reader.ReadBool();
      this.Loot = Option<WorldMapLoot>.Deserialize(reader);
      this.Name = reader.ReadString();
      this.Position = Vector2i.Deserialize(reader);
      this.State = (WorldMapLocationState) reader.ReadInt();
    }

    public WorldMapLocId Id { get; private set; }

    public string Name { get; private set; }

    public Vector2i Position { get; private set; }

    public WorldMapLocationState State { get; private set; }

    public Option<WorldMapLoot> Loot { get; set; }

    public Option<WorldMapEntityProto> EntityProto { get; private set; }

    public Option<IWorldMapEntity> Entity { get; private set; }

    public Option<BattleFleet> Enemy { get; private set; }

    public Option<WorldMapLocationGfxProto> Graphics { get; private set; }

    public bool IsEnemyKnown { get; set; }

    public bool IsScannedByRadar { get; set; }

    public WorldMapLocation(string name, Vector2i position)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CId\u003Ek__BackingField = new WorldMapLocId(-1);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Position = position;
      this.Name = name;
      this.State = WorldMapLocationState.Hidden;
    }

    internal void SetId(WorldMapLocId id) => this.Id = id;

    internal void ResetId() => this.Id = new WorldMapLocId(-1);

    internal void SetPosition(Vector2i newPosition) => this.Position = newPosition;

    internal void SetState(WorldMapLocationState newState) => this.State = newState;

    internal void SetEnemy(BattleFleet battleFleet)
    {
      Assert.That<Option<BattleFleet>>(this.Enemy).IsNone<BattleFleet>();
      this.Enemy = Option.Some<BattleFleet>(battleFleet);
    }

    internal void SetGraphics(WorldMapLocationGfxProto gfx)
    {
      this.Graphics = (Option<WorldMapLocationGfxProto>) gfx;
    }

    internal void MarkEnemyAsDefeated()
    {
      Assert.That<Option<BattleFleet>>(this.Enemy).HasValue<BattleFleet>();
      this.Enemy = Option<BattleFleet>.None;
    }

    internal void ClearLoot() => this.Loot = Option<WorldMapLoot>.None;

    internal void ClearProto() => this.EntityProto = (Option<WorldMapEntityProto>) Option.None;

    internal void SetEntityProto(WorldMapEntityProto entityProto)
    {
      Assert.That<Option<WorldMapEntityProto>>(this.EntityProto).IsNone<WorldMapEntityProto>();
      this.EntityProto = Option.Some<WorldMapEntityProto>(entityProto);
    }

    internal void SetEntity(IWorldMapEntity entity)
    {
      Assert.That<Option<IWorldMapEntity>>(this.Entity).IsNone<IWorldMapEntity>();
      this.Entity = Option.Some<IWorldMapEntity>(entity);
    }

    internal void RemoveCurrentEntity() => this.Entity = Option<IWorldMapEntity>.None;

    static WorldMapLocation()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapLocation.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldMapLocation) obj).SerializeData(writer));
      WorldMapLocation.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldMapLocation) obj).DeserializeData(reader));
    }
  }
}
