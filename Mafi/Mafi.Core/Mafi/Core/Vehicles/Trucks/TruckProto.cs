// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.TruckProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks
{
  public class TruckProto : DrivingEntityProto
  {
    /// <summary>
    /// Type of product that this truck supports.
    /// If set, this truck has just one attachment. If null, this truck has multiple switchable attachments.
    /// </summary>
    public Mafi.Core.Products.ProductType? ProductType;
    /// <summary>
    /// Capacity of the truck. Do not use directly as the final value is affected by edicts.
    /// </summary>
    public readonly Quantity CapacityBase;
    /// <summary>
    /// Attachments that can be used on the truck - e.g. container, tank.
    /// </summary>
    public readonly AttachmentProto[] Attachments;
    /// <summary>Attachment used when truck is empty.</summary>
    public readonly Option<AttachmentProto> AttachmentWhenEmpty;
    public readonly RelTile1i MaxDumpingDistance;
    public readonly RelTile1i MinDumpingDistance;
    public readonly RelTile1i MaxSurfaceProcessingDistance;
    public readonly RelTile1i MinSurfaceProcessingDistance;
    /// <summary>
    /// Filter for <see cref="P:Mafi.Core.Vehicles.Trucks.TruckProto.AllowedProducts" />.
    /// </summary>
    private readonly Func<ProductProto, bool> m_productsFilter;
    /// <summary>
    /// Specifies how much of material can be dumped at particular radius around target tile. Length of this array
    /// naturally specifies dumping radius.
    /// </summary>
    public readonly ImmutableArray<ThicknessTilesF> DumpedThicknessByDistance;
    /// <summary>
    /// Minimum amount of dump iterations while no truck quantity changed before declaring it done.
    /// This helps with waiting on terrain physics simulation.
    /// </summary>
    public readonly int MinDumpIterationsWithoutQuantityChanged;
    public Duration DumpIterationDuration;
    /// <summary>
    /// Duration of cargo pickup from (or delivery to) storage.
    /// </summary>
    public readonly Duration CargoPickupDuration;
    public readonly TruckProto.Gfx Graphics;

    public override Type EntityType => typeof (Truck);

    /// <summary>Products that can be transported by the truck.</summary>
    public IReadOnlySet<ProductProto> AllowedProducts { get; private set; }

    public TruckProto(
      DynamicEntityProto.ID id,
      Proto.Str strings,
      RelTile3f entitySize,
      EntityCosts costs,
      int vehicleQuotaCost,
      DrivingData drivingData,
      Quantity capacity,
      Func<ProductProto, bool> productsFilter,
      ImmutableArray<ThicknessTilesF> dumpedThicknessByDistance,
      AttachmentProto[] attachments,
      Option<AttachmentProto> attachmentWhenEmpty,
      Duration cargoPickupDuration,
      RelTile1i minDumpingDistance,
      RelTile1i maxDumpingDistance,
      Option<Mafi.Core.Entities.Dynamic.FuelTankProto> fuelTankProto,
      VehiclePathFindingParams pathFindingParams,
      ImmutableArray<ThicknessTilesF> disruptionByDistance,
      Duration durationToBuild,
      Option<DrivingEntityProto> nextTier,
      TruckProto.Gfx graphics,
      Mafi.Core.Products.ProductType? productType = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.MinDumpIterationsWithoutQuantityChanged = 3;
      this.DumpIterationDuration = new Duration(5);
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, entitySize, costs, vehicleQuotaCost, drivingData, fuelTankProto, pathFindingParams, disruptionByDistance, durationToBuild, nextTier, (DynamicGroundEntityProto.Gfx) graphics);
      this.m_productsFilter = productsFilter.CheckNotNull<Func<ProductProto, bool>>();
      this.CapacityBase = capacity.CheckPositive();
      this.DumpedThicknessByDistance = dumpedThicknessByDistance.CheckNotEmpty<ThicknessTilesF>();
      this.Attachments = attachments.CheckNotNull<AttachmentProto[]>();
      this.AttachmentWhenEmpty = attachmentWhenEmpty;
      this.CargoPickupDuration = cargoPickupDuration;
      this.MinDumpingDistance = minDumpingDistance;
      this.MaxDumpingDistance = maxDumpingDistance;
      this.MinSurfaceProcessingDistance = minDumpingDistance;
      this.MaxSurfaceProcessingDistance = (2.5f * (float) maxDumpingDistance.Value).RoundToInt().Tiles();
      this.ProductType = productType;
      this.Graphics = graphics;
      if (!this.ProductType.HasValue)
        return;
      if (this.AttachmentWhenEmpty.IsNone)
        throw new InvalidProtoException(string.Format("Truck {0} has product type enforced but has no empty attachment!", (object) id));
      if (this.Attachments.Length != 1)
        throw new InvalidProtoException(string.Format("Truck {0} has product type enforced but not exactly 1 attachment!", (object) id));
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      this.AllowedProducts = (IReadOnlySet<ProductProto>) new Set<ProductProto>(protosDb.Filter<ProductProto>(this.m_productsFilter));
    }

    public new class Gfx : DynamicGroundEntityProto.Gfx
    {
      public static readonly TruckProto.Gfx Empty;
      /// <summary>
      /// Paths to the submodels of steering wheels in the game object hierarchy of the truck.
      /// </summary>
      public readonly ImmutableArray<string> SteeringWheelsSubmodelPaths;
      /// <summary>
      /// Paths to the submodels of non-steering wheels in the game object hierarchy of the truck.
      /// </summary>
      public readonly ImmutableArray<string> StaticWheelsSubmodelPaths;
      public readonly RelTile1f WheelDiameter;

      public Gfx(
        string prefabPath,
        Option<string> customIconPath,
        RelTile2f frontContactPtsOffset,
        RelTile2f rearContactPtsOffset,
        ImmutableArray<DynamicEntityDustParticlesSpec> dustParticles,
        Option<VehicleExhaustParticlesSpec> exhaustParticlesSpec,
        string engineSoundPath,
        string movementSoundPath,
        ImmutableArray<string> steeringWheelsSubmodelPaths,
        RelTile1f wheelDiameter,
        ImmutableArray<string> staticWheelsSubmodelPaths)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath, customIconPath, frontContactPtsOffset, rearContactPtsOffset, dustParticles, exhaustParticlesSpec, engineSoundPath, movementSoundPath);
        this.WheelDiameter = wheelDiameter;
        this.SteeringWheelsSubmodelPaths = steeringWheelsSubmodelPaths;
        this.StaticWheelsSubmodelPaths = staticWheelsSubmodelPaths;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        RelTile2f frontContactPtsOffset = new RelTile2f((Fix32) 1, (Fix32) 1);
        RelTile2f rearContactPtsOffset = new RelTile2f((Fix32) 1, (Fix32) 1);
        ImmutableArray<DynamicEntityDustParticlesSpec> empty1 = ImmutableArray<DynamicEntityDustParticlesSpec>.Empty;
        Option<VehicleExhaustParticlesSpec> none = Option<VehicleExhaustParticlesSpec>.None;
        RelTile1f relTile1f = 1.0.Tiles();
        ImmutableArray<string> empty2 = ImmutableArray<string>.Empty;
        RelTile1f wheelDiameter = relTile1f;
        ImmutableArray<string> empty3 = ImmutableArray<string>.Empty;
        TruckProto.Gfx.Empty = new TruckProto.Gfx("EMPTY", customIconPath, frontContactPtsOffset, rearContactPtsOffset, empty1, none, "EMPTY", "EMPTY", empty2, wheelDiameter, empty3);
      }
    }
  }
}
