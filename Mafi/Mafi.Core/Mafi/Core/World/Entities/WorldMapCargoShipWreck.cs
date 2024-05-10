// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Entities.WorldMapCargoShipWreck
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class WorldMapCargoShipWreck : WorldMapRepairableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly WorldMapCargoShipWreckProto Prototype;
    private readonly WorldMapManager m_worldMapManager;
    private readonly CargoDepotManager m_cargoDepotManager;

    public static void Serialize(WorldMapCargoShipWreck value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapCargoShipWreck>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapCargoShipWreck.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      CargoDepotManager.Serialize(this.m_cargoDepotManager, writer);
      WorldMapManager.Serialize(this.m_worldMapManager, writer);
      writer.WriteGeneric<WorldMapCargoShipWreckProto>(this.Prototype);
    }

    public static WorldMapCargoShipWreck Deserialize(BlobReader reader)
    {
      WorldMapCargoShipWreck mapCargoShipWreck;
      if (reader.TryStartClassDeserialization<WorldMapCargoShipWreck>(out mapCargoShipWreck))
        reader.EnqueueDataDeserialization((object) mapCargoShipWreck, WorldMapCargoShipWreck.s_deserializeDataDelayedAction);
      return mapCargoShipWreck;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<WorldMapCargoShipWreck>(this, "m_cargoDepotManager", (object) CargoDepotManager.Deserialize(reader));
      reader.SetField<WorldMapCargoShipWreck>(this, "m_worldMapManager", (object) WorldMapManager.Deserialize(reader));
      reader.SetField<WorldMapCargoShipWreck>(this, "Prototype", (object) reader.ReadGenericAs<WorldMapCargoShipWreckProto>());
    }

    public override bool IsOwnedByPlayer => false;

    public override bool CanBePaused => false;

    public override AssetValue CostToRepair => this.Prototype.CostToRepair;

    public WorldMapCargoShipWreck(
      EntityId entityId,
      WorldMapCargoShipWreckProto prototype,
      WorldMapLocation location,
      EntityContext context,
      WorldMapManager worldMapManager,
      CargoDepotManager cargoDepotManager,
      IInstaBuildManager instaBuildManager,
      IProductsManager productsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(entityId, (WorldMapEntityProto) prototype, location, context, worldMapManager, instaBuildManager, productsManager);
      this.Prototype = prototype;
      this.m_worldMapManager = worldMapManager;
      this.m_cargoDepotManager = cargoDepotManager;
      this.m_cargoDepotManager.ReportNewCargoShipFound();
    }

    protected override void SimUpdateInternal()
    {
      base.SimUpdateInternal();
      if (!this.IsRepaired)
        return;
      this.m_cargoDepotManager.ReportNewCargoShipRepaired();
      this.m_worldMapManager.RequestEntityRemoval((IWorldMapEntity) this);
      this.Location.ClearProto();
    }

    static WorldMapCargoShipWreck()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapCargoShipWreck.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      WorldMapCargoShipWreck.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
