// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.StaticEntityVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("m_terrainManager", 140, typeof (TerrainManager), 0, false)]
  public sealed class StaticEntityVehicleGoal : VehicleGoalBase
  {
    private Tile3i m_position;
    [NewInSaveVersion(140, null, null, typeof (StaticEntityVehicleGoal.Factory), null)]
    private readonly StaticEntityVehicleGoal.Factory m_factory;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<IStaticEntity> GoalStaticEntity { get; private set; }

    public bool UseCustomTarget { get; private set; }

    public override LocStrFormatted GoalName
    {
      get
      {
        IStaticEntity valueOrNull = this.GoalStaticEntity.ValueOrNull;
        return valueOrNull == null ? LocStrFormatted.Empty : valueOrNull.DefaultTitle;
      }
    }

    public StaticEntityVehicleGoal(
      IVehicleSurfaceProvider vehicleSurfaceProvider,
      StaticEntityVehicleGoal.Factory factory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(vehicleSurfaceProvider);
      this.m_factory = factory;
    }

    public void Initialize(IStaticEntity staticEntity, bool useCustomTarget = false)
    {
      Assert.That<bool>(staticEntity.IsDestroyed).IsFalse("Creating nav goal to a destroyed entity.");
      this.Initialize();
      this.GoalStaticEntity = staticEntity.SomeOption<IStaticEntity>();
      this.UseCustomTarget = useCustomTarget;
      this.m_position = staticEntity.CenterTile;
    }

    public override Tile3f GetGoalPosition()
    {
      return !this.GoalStaticEntity.HasValue ? new Tile3f() : this.GoalStaticEntity.Value.Position3f;
    }

    protected override bool ShouldCheckGoalHeights(
      int retryNumber,
      out HeightTilesF goalHeightLow,
      out HeightTilesF goalHeightHigh)
    {
      if (this.GoalStaticEntity.IsNone)
      {
        Log.Error("Entity is None.");
        goalHeightLow = HeightTilesF.Zero;
        goalHeightHigh = HeightTilesF.Zero;
        return false;
      }
      IStaticEntity staticEntity = this.GoalStaticEntity.Value;
      if (!staticEntity.Prototype.VehicleGoalHeightAllowedRange.HasValue)
      {
        goalHeightLow = HeightTilesF.Zero;
        goalHeightHigh = HeightTilesF.Zero;
        return false;
      }
      HeightTilesF heightTilesF = staticEntity.CenterTile.Height.HeightTilesF;
      goalHeightLow = heightTilesF - staticEntity.Prototype.VehicleGoalHeightAllowedRange.Value.To.ThicknessTilesF;
      goalHeightHigh = heightTilesF - staticEntity.Prototype.VehicleGoalHeightAllowedRange.Value.From.ThicknessTilesF;
      return true;
    }

    protected override bool GetGoalTilesInternal(
      Tile2i startTile,
      VehiclePathFindingParams pfParams,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile,
      int retryNumber,
      RelTile1i extraTolerancePerRetry)
    {
      Assert.That<Option<IStaticEntity>>(this.GoalStaticEntity).HasValue<IStaticEntity>();
      Assert.That<Lyst<Tile2i>>(goalTiles).IsEmpty<Tile2i>();
      IStaticEntity staticEntity = this.GoalStaticEntity.Value;
      if (staticEntity.IsDestroyed)
      {
        distanceEstimationGoalTile = startTile;
        return false;
      }
      distanceEstimationGoalTile = staticEntity.Position2f.Tile2i;
      if (this.UseCustomTarget)
      {
        bool customPfTargetTiles = staticEntity.GetCustomPfTargetTiles(retryNumber, goalTiles);
        Assert.That<Lyst<Tile2i>>(goalTiles).IsNotEmpty<Tile2i>("Requested custom PF target tiles but no tiles were returned.");
        return customPfTargetTiles;
      }
      foreach (Tile2i tile2i in staticEntity.PfTargetTiles.GetPfTargetsAtDistance(pfParams.RequiredClearance + extraTolerancePerRetry * retryNumber, this.m_factory.m_terrainManager))
      {
        if (tile2i == startTile)
        {
          goalTiles.Clear();
          goalTiles.Add(tile2i);
          return true;
        }
        goalTiles.Add(tile2i);
      }
      return false;
    }

    public override bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf)
    {
      retryPf = false;
      if (this.GoalStaticEntity.IsNone || this.GoalStaticEntity.Value.IsDestroyed)
        return false;
      if (!(this.m_position != this.GoalStaticEntity.Value.CenterTile))
        return true;
      retryPf = true;
      return false;
    }

    public override void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle)
    {
      if (isSuccess)
        this.m_factory.UnreachablesManager.MarkReachableFor((IEntity) this.GoalStaticEntity.Value, vehicle);
      else
        this.m_factory.UnreachablesManager.MarkUnreachableFor((IEntity) this.GoalStaticEntity.Value, vehicle);
    }

    public override string ToString()
    {
      return string.Format("{0} (use custom: {1})", (object) this.GoalStaticEntity.Value, (object) this.UseCustomTarget);
    }

    public static void Serialize(StaticEntityVehicleGoal value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StaticEntityVehicleGoal>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StaticEntityVehicleGoal.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<IStaticEntity>.Serialize(this.GoalStaticEntity, writer);
      StaticEntityVehicleGoal.Factory.Serialize(this.m_factory, writer);
      Tile3i.Serialize(this.m_position, writer);
      writer.WriteBool(this.UseCustomTarget);
    }

    public static StaticEntityVehicleGoal Deserialize(BlobReader reader)
    {
      StaticEntityVehicleGoal entityVehicleGoal;
      if (reader.TryStartClassDeserialization<StaticEntityVehicleGoal>(out entityVehicleGoal))
        reader.EnqueueDataDeserialization((object) entityVehicleGoal, StaticEntityVehicleGoal.s_deserializeDataDelayedAction);
      return entityVehicleGoal;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.GoalStaticEntity = Option<IStaticEntity>.Deserialize(reader);
      reader.SetField<StaticEntityVehicleGoal>(this, "m_factory", reader.LoadedSaveVersion >= 140 ? (object) StaticEntityVehicleGoal.Factory.Deserialize(reader) : (object) (StaticEntityVehicleGoal.Factory) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<StaticEntityVehicleGoal>(this, "m_factory", typeof (StaticEntityVehicleGoal.Factory), true);
      this.m_position = Tile3i.Deserialize(reader);
      if (reader.LoadedSaveVersion < 140)
        TerrainManager.Deserialize(reader);
      this.UseCustomTarget = reader.ReadBool();
    }

    static StaticEntityVehicleGoal()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntityVehicleGoal.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleGoalBase) obj).SerializeData(writer));
      StaticEntityVehicleGoal.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleGoalBase) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    [OnlyForSaveCompatibility("This does not need a serializer!")]
    [GenerateSerializer(false, null, 0)]
    public sealed class Factory
    {
      internal readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
      internal readonly TerrainManager m_terrainManager;
      [NewInSaveVersion(140, null, null, typeof (IUnreachablesManager), null)]
      internal readonly IUnreachablesManager UnreachablesManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Factory(
        IVehicleSurfaceProvider vehicleSurfaceProvider,
        TerrainManager terrainManager,
        IUnreachablesManager unreachablesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
        this.m_terrainManager = terrainManager;
        this.UnreachablesManager = unreachablesManager;
      }

      public StaticEntityVehicleGoal Create(IStaticEntity staticEntity, bool useCustomTarget = false)
      {
        StaticEntityVehicleGoal entityVehicleGoal = new StaticEntityVehicleGoal(this.m_vehicleSurfaceProvider, this);
        entityVehicleGoal.Initialize(staticEntity, useCustomTarget);
        return entityVehicleGoal;
      }

      public static void Serialize(StaticEntityVehicleGoal.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<StaticEntityVehicleGoal.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, StaticEntityVehicleGoal.Factory.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        TerrainManager.Serialize(this.m_terrainManager, writer);
        writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
        writer.WriteGeneric<IUnreachablesManager>(this.UnreachablesManager);
      }

      public static StaticEntityVehicleGoal.Factory Deserialize(BlobReader reader)
      {
        StaticEntityVehicleGoal.Factory factory;
        if (reader.TryStartClassDeserialization<StaticEntityVehicleGoal.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, StaticEntityVehicleGoal.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<StaticEntityVehicleGoal.Factory>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
        reader.SetField<StaticEntityVehicleGoal.Factory>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
        reader.SetField<StaticEntityVehicleGoal.Factory>(this, "UnreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IUnreachablesManager>() : (object) (IUnreachablesManager) null);
        if (reader.LoadedSaveVersion >= 140)
          return;
        reader.RegisterResolvedMember<StaticEntityVehicleGoal.Factory>(this, "UnreachablesManager", typeof (IUnreachablesManager), true);
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        StaticEntityVehicleGoal.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StaticEntityVehicleGoal.Factory) obj).SerializeData(writer));
        StaticEntityVehicleGoal.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StaticEntityVehicleGoal.Factory) obj).DeserializeData(reader));
      }
    }
  }
}
