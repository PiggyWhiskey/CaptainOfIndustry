// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapLoot
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Research;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  public class WorldMapLoot
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public int People;
    public AssetValue Products;
    public ImmutableArray<TechnologyProto> ProtosToUnlock;
    /// <summary>
    /// Whether these rewards are so significant that they deserved to be marked on the map.
    /// </summary>
    public bool IsTreasure;

    public static void Serialize(WorldMapLoot value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapLoot>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapLoot.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsTreasure);
      writer.WriteInt(this.People);
      AssetValue.Serialize(this.Products, writer);
      ImmutableArray<TechnologyProto>.Serialize(this.ProtosToUnlock, writer);
    }

    public static WorldMapLoot Deserialize(BlobReader reader)
    {
      WorldMapLoot worldMapLoot;
      if (reader.TryStartClassDeserialization<WorldMapLoot>(out worldMapLoot))
        reader.EnqueueDataDeserialization((object) worldMapLoot, WorldMapLoot.s_deserializeDataDelayedAction);
      return worldMapLoot;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsTreasure = reader.ReadBool();
      this.People = reader.ReadInt();
      this.Products = AssetValue.Deserialize(reader);
      this.ProtosToUnlock = ImmutableArray<TechnologyProto>.Deserialize(reader);
    }

    public bool IsEmpty => this.People <= 0 && this.Products.IsEmpty && this.ProtosToUnlock.IsEmpty;

    public WorldMapLoot()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Products = AssetValue.Empty;
      this.ProtosToUnlock = ImmutableArray<TechnologyProto>.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static WorldMapLoot()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapLoot.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldMapLoot) obj).SerializeData(writer));
      WorldMapLoot.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldMapLoot) obj).DeserializeData(reader));
    }
  }
}
