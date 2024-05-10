// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.VehicleDepots.VehicleDepotBaseProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Buildings.VehicleDepots
{
  [DebuggerDisplay("VehicleDepotProto: {Id}")]
  public abstract class VehicleDepotBaseProto : 
    LayoutEntityProto,
    IProtoWithPowerConsumption,
    IProtoWithUpgrade<VehicleDepotBaseProto>,
    IProtoWithUpgrade,
    IProto
  {
    public readonly Electricity ConsumedPowerPerTick;
    public readonly Computing ConsumedComputingPerTick;
    /// <summary>Minimal interval between spawned vehicles.</summary>
    public readonly Duration SpawnInterval;
    /// <summary>
    /// Tile relative to the depot position where vehicles will appear. This is usually somewhere inside of the
    /// depot.
    /// </summary>
    public readonly RelTile2f SpawnPosition;
    /// <summary>
    /// Tile relative to the depot position where entities are sent to when spawning. This is usually in front of the
    /// main door.
    /// </summary>
    public readonly RelTile2f SpawnDriveTargetPosition;
    /// <summary>Direction of a vehicle when spawned.</summary>
    public readonly AngleDegrees1f SpawnDirection;
    /// <summary>
    /// Tile relative to the depot position where vehicles will disappear. This is usually somewhere inside of the
    /// depot.
    /// </summary>
    public readonly RelTile2f DespawnPosition;
    /// <summary>
    /// Tile relative to the depot position where entities are sent to when spawning. This is usually in front of the
    /// main door.
    /// </summary>
    public readonly RelTile2f DespawnDriveTargetPosition;
    /// <summary>Duration of door opening animation.</summary>
    public readonly Duration DoorOpenDuration;
    public readonly VehicleDepotBaseProto.Gfx Graphics;
    private readonly Lyst<DynamicGroundEntityProto> m_buildableEntities;

    public UpgradeData<VehicleDepotBaseProto> Upgrade { get; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public Electricity ElectricityConsumed => this.ConsumedPowerPerTick;

    /// <summary>Entities buildable by this depot type.</summary>
    public ImmutableArray<DynamicGroundEntityProto> BuildableEntities { get; private set; }

    public VehicleDepotBaseProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Electricity consumedPowerPerTick,
      Computing consumedComputingPerTick,
      Duration spawnInterval,
      Duration doorOpenDuration,
      RelTile2f spawnPosition,
      RelTile2f spawnDriveTargetPosition,
      RelTile2f despawnPosition,
      RelTile2f despawnDriveTargetPosition,
      Option<VehicleDepotBaseProto> nextTier,
      VehicleDepotBaseProto.Gfx graphics,
      bool cannotBeReflected = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_buildableEntities = new Lyst<DynamicGroundEntityProto>();
      StaticEntityProto.ID id1 = id;
      Proto.Str strings1 = strings;
      EntityLayout layout1 = layout;
      EntityCosts costs1 = costs;
      VehicleDepotBaseProto.Gfx graphics1 = graphics;
      bool flag = cannotBeReflected;
      Duration? constructionDurationPerProduct = new Duration?();
      Upoints? boostCost = new Upoints?();
      int num = flag ? 1 : 0;
      // ISSUE: explicit constructor call
      base.\u002Ector(id1, strings1, layout1, costs1, (LayoutEntityProto.Gfx) graphics1, constructionDurationPerProduct, boostCost, cannotBeReflected: num != 0);
      this.SpawnInterval = spawnInterval.CheckNotNegative();
      this.DoorOpenDuration = doorOpenDuration.CheckPositive();
      this.SpawnPosition = spawnPosition;
      this.SpawnDriveTargetPosition = spawnDriveTargetPosition;
      this.SpawnDirection = (spawnDriveTargetPosition - spawnPosition).Angle;
      this.DespawnPosition = despawnPosition;
      this.DespawnDriveTargetPosition = despawnDriveTargetPosition;
      this.ConsumedPowerPerTick = consumedPowerPerTick.CheckNotNegative();
      this.ConsumedComputingPerTick = consumedComputingPerTick.CheckNotNegative();
      this.Upgrade = new UpgradeData<VehicleDepotBaseProto>(this, nextTier);
      this.Graphics = graphics;
    }

    protected override void OnInitialize(ProtosDb protosDb)
    {
      base.OnInitialize(protosDb);
      this.BuildableEntities = this.m_buildableEntities.ToImmutableArray();
    }

    public void AddBuildableEntity(DynamicGroundEntityProto entity)
    {
      if (this.IsInitialized)
        Mafi.Log.Error(string.Format("Adding entity {0} to an already initialized depot", (object) entity));
      else
        this.m_buildableEntities.AddAssertNew(entity);
    }

    public new class Gfx : LayoutEntityProto.Gfx
    {
      public static readonly VehicleDepotBaseProto.Gfx Empty;
      /// <summary>The sound the generator makes while it is operating.</summary>
      public readonly Option<string> SoundPrefabPath;

      public Gfx(
        string prefabPath,
        Option<string> soundPrefabPath,
        ImmutableArray<ToolbarCategoryProto> categories,
        Option<string> customIconPath = default (Option<string>))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        string prefabPath1 = prefabPath;
        Option<string> option = customIconPath;
        ImmutableArray<ToolbarCategoryProto>? nullable = new ImmutableArray<ToolbarCategoryProto>?(categories);
        RelTile3f prefabOrigin = new RelTile3f();
        Option<string> customIconPath1 = option;
        ColorRgba color = new ColorRgba();
        LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
        ImmutableArray<ToolbarCategoryProto>? categories1 = nullable;
        ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
        // ISSUE: explicit constructor call
        base.\u002Ector(prefabPath1, prefabOrigin, customIconPath1, color, visualizedLayers: visualizedLayers, categories: categories1, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects);
        this.SoundPrefabPath = soundPrefabPath;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Option<string> customIconPath = (Option<string>) "EMPTY";
        VehicleDepotBaseProto.Gfx.Empty = new VehicleDepotBaseProto.Gfx("EMPTY", (Option<string>) Option.None, ImmutableArray<ToolbarCategoryProto>.Empty, customIconPath);
      }
    }
  }
}
