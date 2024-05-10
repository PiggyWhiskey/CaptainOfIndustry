// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.World.Entities
{
  public abstract class WorldMapEntity : Entity, IWorldMapEntity, IEntity, IIsSafeAsHashKey
  {
    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      WorldMapLocation.Serialize(this.Location, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Location = WorldMapLocation.Deserialize(reader);
    }

    public abstract bool IsOwnedByPlayer { get; }

    public WorldMapLocation Location { get; private set; }

    protected WorldMapEntity(
      EntityId entityId,
      EntityProto prototype,
      WorldMapLocation location,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(entityId, prototype, context);
      this.Location = location;
    }
  }
}
