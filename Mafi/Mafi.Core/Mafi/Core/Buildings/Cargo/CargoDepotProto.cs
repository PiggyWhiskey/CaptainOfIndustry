// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.CargoDepotProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [DebuggerDisplay("CargoDepotProto: {Id}")]
  public class CargoDepotProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<CargoDepotProto>,
    IProtoWithUpgrade,
    IProto,
    IProtoWithReservedOcean,
    ILayoutEntityProto,
    IStaticEntityProto,
    IEntityProto
  {
    public static readonly HeightTilesI MIN_GROUND_HEIGHT;
    public static readonly HeightTilesI MAX_GROUND_HEIGHT;
    public readonly ImmutableArray<CargoDepotProto.ModuleSlotPosition> ModuleSlots;
    /// <summary>
    /// Id of ship prototype used for the depot. This is specified as id and not directly as <see cref="P:Mafi.Core.Buildings.Cargo.CargoDepotProto.CargoShipProto" />, because the ship proto may not yet exist at the time when <see cref="T:Mafi.Core.Buildings.Cargo.CargoDepotProto" /> is being created.
    /// </summary>
    private readonly EntityProto.ID m_cargoShipProtoId;
    public readonly ImmutableArray<string> DockingAnimationsPrefabPaths;

    public override Type EntityType => typeof (CargoDepot);

    public UpgradeData<CargoDepotProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    /// <summary>
    /// Prototype of cargo ship used to service the cargo depot.
    /// </summary>
    public CargoShipProto CargoShipProto { get; private set; }

    public ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> ReservedOceanAreasSets { get; }

    public HeightTilesI MinGroundHeight => CargoDepotProto.MIN_GROUND_HEIGHT;

    public HeightTilesI MaxGroundHeight => CargoDepotProto.MAX_GROUND_HEIGHT;

    public CargoDepotProto(
      CargoDepotProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      ImmutableArray<CargoDepotProto.ModuleSlotPosition> moduleSlots,
      EntityProto.ID cargoShipProtoId,
      Option<CargoDepotProto> nextTier,
      ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets,
      LayoutEntityProto.Gfx graphics,
      ImmutableArray<string> dockingAnimationsPrefabPaths)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((StaticEntityProto.ID) id, strings, layout, costs, graphics, cannotBeReflected: true);
      this.ModuleSlots = moduleSlots;
      this.m_cargoShipProtoId = cargoShipProtoId;
      this.Upgrade = new UpgradeData<CargoDepotProto>(this, nextTier);
      this.ReservedOceanAreasSets = reservedOceanAreasSets;
      this.DockingAnimationsPrefabPaths = dockingAnimationsPrefabPaths;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      this.CargoShipProto = protosDb.GetOrThrow<CargoShipProto>((Proto.ID) this.m_cargoShipProtoId);
    }

    public CargoDepotProto.ID Id => new CargoDepotProto.ID(base.Id.Value);

    static CargoDepotProto()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotProto.MIN_GROUND_HEIGHT = new HeightTilesI(2);
      CargoDepotProto.MAX_GROUND_HEIGHT = new HeightTilesI(5);
    }

    /// <summary>
    /// Represents position of a cargo depot module slot relative to the depot.
    /// </summary>
    public readonly struct ModuleSlotPosition
    {
      /// <summary>
      /// Position of the origin, the [0, 0] relative layout coordinate.
      /// Under identity transformation thi is the lowest x and y coordinates relative to the depot.
      /// </summary>
      public readonly RelTile2i Origin;
      public readonly RelTile2i SlotSize;

      public ModuleSlotPosition(RelTile2i origin, RelTile2i slotSize)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Origin = origin;
        this.SlotSize = slotSize;
      }

      public Tile3i GetModulePosition(CargoDepot depot, CargoDepotModuleProto moduleProto)
      {
        return this.GetModuleOrigin(depot) - moduleProto.Layout.TransformRelative(RelTile2i.Zero, new TileTransform(Tile3i.Zero, depot.Transform.Rotation, depot.Transform.IsReflected)).ExtendZ(0);
      }

      public Tile3i GetModuleOrigin(CargoDepot depot)
      {
        return this.GetModuleOrigin(depot.Prototype, depot.Transform);
      }

      public Tile3i GetModuleOrigin(CargoDepotProto depotProto, TileTransform depotTransform)
      {
        return depotProto.Layout.Transform(this.Origin, depotTransform).ExtendZ(depotTransform.Position.Z);
      }

      public TileTransform GetModuleTransform(CargoDepot depot, CargoDepotModuleProto moduleProto)
      {
        return new TileTransform(this.GetModulePosition(depot, moduleProto), depot.Transform.Rotation, depot.Transform.IsReflected);
      }
    }

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : IEquatable<CargoDepotProto.ID>, IComparable<CargoDepotProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Entities.Static.StaticEntityProto.ID" />.
      /// </summary>
      public static implicit operator StaticEntityProto.ID(CargoDepotProto.ID id)
      {
        return new StaticEntityProto.ID(id.Value);
      }

      public static bool operator ==(StaticEntityProto.ID lhs, CargoDepotProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(StaticEntityProto.ID lhs, CargoDepotProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotProto.ID lhs, StaticEntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Entities.EntityProto.ID" />.
      /// </summary>
      public static implicit operator EntityProto.ID(CargoDepotProto.ID id)
      {
        return new EntityProto.ID(id.Value);
      }

      public static bool operator ==(EntityProto.ID lhs, CargoDepotProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotProto.ID lhs, EntityProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(EntityProto.ID lhs, CargoDepotProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotProto.ID lhs, EntityProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(CargoDepotProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, CargoDepotProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, CargoDepotProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(CargoDepotProto.ID lhs, CargoDepotProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(CargoDepotProto.ID lhs, CargoDepotProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is CargoDepotProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(CargoDepotProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(CargoDepotProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(CargoDepotProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static CargoDepotProto.ID Deserialize(BlobReader reader)
      {
        return new CargoDepotProto.ID(reader.ReadString());
      }
    }
  }
}
