// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.RocketEntityBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.SpaceProgram
{
  public abstract class RocketEntityBase : Entity, IRenderedEntity, IEntity, IIsSafeAsHashKey
  {
    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<IRocketOwner>.Serialize(this.Owner, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Owner = Option<IRocketOwner>.Deserialize(reader);
    }

    public Option<IRocketOwner> Owner { get; private set; }

    [DoNotSave(0, null)]
    ulong IRenderedEntity.RendererData { get; set; }

    protected RocketEntityBase(EntityId id, EntityProto prototype, EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, prototype, context);
    }

    public void SetOwner(Option<IRocketOwner> newOwner) => this.Owner = newOwner;
  }
}
