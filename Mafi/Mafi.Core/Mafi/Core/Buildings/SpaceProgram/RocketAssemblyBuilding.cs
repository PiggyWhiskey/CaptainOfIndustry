// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.SpaceProgram.RocketAssemblyBuilding
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.SpaceProgram
{
  public class RocketAssemblyBuilding : 
    VehicleDepotBase,
    IRocketTransporterOwner,
    IRocketOwner,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey
  {
    public static readonly LocStr1 RocketAssemblyDepot__CannotDestroy;
    public readonly RocketAssemblyBuildingProto Prototype;
    private Option<RocketTransporter> m_releasedRocketTransporter;
    private readonly RocketAssemblyAttachJob.Factory m_rocketAttachJobFactory;
    private int m_roofOpenTicks;
    private int m_roofOpenTicksTarget;
    private int m_rocketRaiseTicks;
    private int m_rocketRaiseTicksTarget;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public Percent RoofOpenPerc
    {
      get => Percent.FromRatio(this.m_roofOpenTicks, this.Prototype.RoofOpenDuration.Ticks);
    }

    public Percent RocketRaisePerc
    {
      get => Percent.FromRatio(this.m_rocketRaiseTicks, this.Prototype.RocketRaiseDuration.Ticks);
    }

    protected override bool ProvideProductsForVehicleScrap => false;

    public RocketAssemblyBuilding(
      EntityId id,
      RocketAssemblyBuildingProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      IVehiclesManager vehiclesManager,
      TerrainManager terrainManager,
      SpawnJob.Factory spawnJobFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IProductsManager productsManager,
      IInstaBuildManager instaBuildManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      UpointsManager upointsManager,
      EntitiesCreator entitiesCreator,
      RocketAssemblyAttachJob.Factory rocketAttachJobFactory,
      EntitiesCloneConfigHelper cloneConfigHelper)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (VehicleDepotBaseProto) proto, transform, context, simLoopEvents, vehiclesManager, terrainManager, spawnJobFactory, vehicleBuffersRegistry, productsManager, instaBuildManager, upgraderFactory, upointsManager, entitiesCreator, cloneConfigHelper);
      this.Prototype = proto;
      this.m_rocketAttachJobFactory = rocketAttachJobFactory;
    }

    public bool WaitForRoofOpen()
    {
      this.m_roofOpenTicksTarget = this.Prototype.RoofOpenDuration.Ticks;
      return this.m_roofOpenTicks >= this.m_roofOpenTicksTarget;
    }

    public bool WaitForRocketRaise()
    {
      this.m_rocketRaiseTicksTarget = this.Prototype.RocketRaiseDuration.Ticks;
      return this.m_rocketRaiseTicks >= this.m_rocketRaiseTicksTarget;
    }

    public override void SimUpdate()
    {
      base.SimUpdate();
      if (this.m_roofOpenTicks != this.m_roofOpenTicksTarget)
      {
        this.ConsumedPower = true;
        this.ConsumedComputing = true;
        if (this.m_roofOpenTicksTarget > this.m_roofOpenTicks)
        {
          if (this.CanWork)
            ++this.m_roofOpenTicks;
        }
        else
          --this.m_roofOpenTicks;
      }
      this.m_roofOpenTicksTarget = 0;
      if (this.m_rocketRaiseTicks != this.m_rocketRaiseTicksTarget)
      {
        this.ConsumedPower = true;
        this.ConsumedComputing = true;
        if (this.m_rocketRaiseTicksTarget > this.m_rocketRaiseTicks)
        {
          if (this.CanWork)
            ++this.m_rocketRaiseTicks;
        }
        else
          --this.m_rocketRaiseTicks;
      }
      this.m_rocketRaiseTicksTarget = 0;
    }

    public override bool CanFinalizeVehicleBuildAndAddToWorld()
    {
      return this.m_releasedRocketTransporter.IsNone;
    }

    protected override bool TryBuildVehicle(out Vehicle vehicle)
    {
      Assert.That<Option<RocketTransporter>>(this.m_releasedRocketTransporter).IsNone<RocketTransporter>();
      if (!base.TryBuildVehicle(out vehicle))
        return false;
      if (!(vehicle is RocketTransporter rocketTransporter))
      {
        Log.Error(string.Format("Rocket assembly created non-rocket vehicle: {0}", (object) vehicle));
        ((IEntityFriend) vehicle).Destroy();
        return false;
      }
      RocketEntityBase entity;
      if (!this.EntitiesCreator.TryCreateRocket<RocketEntityBase>((EntityProto) rocketTransporter.Prototype.RocketProto, out entity))
      {
        Log.Error("Failed to create a rocket via factory.");
        ((IEntityFriend) vehicle).Destroy();
        return false;
      }
      if (!rocketTransporter.CanAttachRocket(entity))
      {
        Log.Error("Failed to attach a rocket.");
        ((IEntityFriend) entity).Destroy();
        ((IEntityFriend) vehicle).Destroy();
        return false;
      }
      this.AttachRocket(entity);
      this.m_releasedRocketTransporter = (Option<RocketTransporter>) rocketTransporter;
      rocketTransporter.SetOwningDepot((IRocketTransporterOwner) this);
      ((EntitiesManager) this.Context.EntitiesManager).AddEntityNoChecks((IEntity) entity);
      return true;
    }

    protected override void SpawnVehicle(Vehicle vehicle)
    {
      if (!(vehicle is RocketTransporter rocketTransporter))
      {
        Log.Error(string.Format("Spawning non-rocket vehicle: {0}", (object) vehicle));
        vehicle.Spawn(this.SpawnDrivePosition, this.SpawnDirection);
      }
      else if (this.InstaBuildManager.IsInstaBuildEnabled)
      {
        vehicle.Spawn(this.SpawnDrivePosition, this.SpawnDirection);
        this.TryTransferRocketTo((IRocketOwner) rocketTransporter);
      }
      else
      {
        vehicle.Spawn(this.SpawnPosition, this.SpawnDirection.Rotated180Deg);
        this.m_rocketAttachJobFactory.EnqueueJob(rocketTransporter, this);
      }
    }

    public override bool CanAccept(DynamicGroundEntity entity)
    {
      return this.m_releasedRocketTransporter.ValueOrNull == entity;
    }

    public override bool CanAcceptForUpgrade(
      DrivingEntityProto currentProto,
      DrivingEntityProto newProto)
    {
      return false;
    }

    public void NotifyTransporterReturned(RocketTransporter rocketTransporter)
    {
      Assert.That<IRocketTransporterOwner>(rocketTransporter.OwningDepot).IsEqualTo<IRocketTransporterOwner>((IRocketTransporterOwner) this, "Transporter notifying return to the wrong depot.");
      Assert.That<RocketTransporter>(rocketTransporter).IsEqualTo<RocketTransporter>(this.m_releasedRocketTransporter.ValueOrNull, "Transporter destroyed in the wrong depot.");
      this.m_releasedRocketTransporter = Option<RocketTransporter>.None;
    }

    public Option<RocketEntityBase> AttachedRocketBase { get; private set; }

    public bool CanAttachRocket(RocketEntityBase rocketBase) => this.AttachedRocketBase.IsNone;

    public void AttachRocket(RocketEntityBase rocketBase)
    {
      Assert.That<Option<IRocketOwner>>(rocketBase.Owner).IsNone<IRocketOwner>("Rocket already owned by someone. Forgot to call `SetOwner`?");
      if (!this.CanAttachRocket(rocketBase))
      {
        Log.Error("Cannot attach rocket '" + rocketBase.GetType().Name + "'.");
      }
      else
      {
        this.AttachedRocketBase = (Option<RocketEntityBase>) rocketBase;
        rocketBase.SetOwner((Option<IRocketOwner>) (IRocketOwner) this);
      }
    }

    public Option<RocketEntityBase> DetachRocket()
    {
      if (this.AttachedRocketBase.IsNone)
      {
        Log.Warning("No rocket to detach.");
        return Option<RocketEntityBase>.None;
      }
      RocketEntityBase rocketEntityBase = this.AttachedRocketBase.Value;
      rocketEntityBase.SetOwner(Option<IRocketOwner>.None);
      this.AttachedRocketBase = Option<RocketEntityBase>.None;
      return (Option<RocketEntityBase>) rocketEntityBase;
    }

    public override EntityValidationResult CanStartDeconstruction()
    {
      return this.m_releasedRocketTransporter.HasValue ? EntityValidationResult.CreateError(RocketAssemblyBuilding.RocketAssemblyDepot__CannotDestroy.Format(this.m_releasedRocketTransporter.Value.Prototype.Strings.Name)) : base.CanStartDeconstruction();
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      if (!this.AttachedRocketBase.HasValue)
        return;
      ((IEntityFriend) this.AttachedRocketBase.Value).Destroy();
    }

    public static void Serialize(RocketAssemblyBuilding value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RocketAssemblyBuilding>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RocketAssemblyBuilding.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<RocketEntityBase>.Serialize(this.AttachedRocketBase, writer);
      Option<RocketTransporter>.Serialize(this.m_releasedRocketTransporter, writer);
      writer.WriteInt(this.m_rocketRaiseTicks);
      writer.WriteInt(this.m_rocketRaiseTicksTarget);
      writer.WriteInt(this.m_roofOpenTicks);
      writer.WriteInt(this.m_roofOpenTicksTarget);
      writer.WriteGeneric<RocketAssemblyBuildingProto>(this.Prototype);
    }

    public static RocketAssemblyBuilding Deserialize(BlobReader reader)
    {
      RocketAssemblyBuilding assemblyBuilding;
      if (reader.TryStartClassDeserialization<RocketAssemblyBuilding>(out assemblyBuilding))
        reader.EnqueueDataDeserialization((object) assemblyBuilding, RocketAssemblyBuilding.s_deserializeDataDelayedAction);
      return assemblyBuilding;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AttachedRocketBase = Option<RocketEntityBase>.Deserialize(reader);
      this.m_releasedRocketTransporter = Option<RocketTransporter>.Deserialize(reader);
      reader.RegisterResolvedMember<RocketAssemblyBuilding>(this, "m_rocketAttachJobFactory", typeof (RocketAssemblyAttachJob.Factory), true);
      this.m_rocketRaiseTicks = reader.ReadInt();
      this.m_rocketRaiseTicksTarget = reader.ReadInt();
      this.m_roofOpenTicks = reader.ReadInt();
      this.m_roofOpenTicksTarget = reader.ReadInt();
      reader.SetField<RocketAssemblyBuilding>(this, "Prototype", (object) reader.ReadGenericAs<RocketAssemblyBuildingProto>());
    }

    static RocketAssemblyBuilding()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RocketAssemblyBuilding.RocketAssemblyDepot__CannotDestroy = Loc.Str1(nameof (RocketAssemblyDepot__CannotDestroy), "The {0} is not parked inside.", "error popup when player tried to destroy rocket assembly depot but the rocket transporter is not parked inside, {0} is 'rocket transporter'");
      RocketAssemblyBuilding.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RocketAssemblyBuilding.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
