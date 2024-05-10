// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.TerrainDesignationVehicleGoal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public sealed class TerrainDesignationVehicleGoal : VehicleGoalBase
  {
    [DoNotSave(140, null)]
    [RenamedInVersion(140, "ActualGoalDesignation")]
    private Option<TerrainDesignation> m_actualGoalDesignationOld;
    [DoNotSave(140, null)]
    [RenamedInVersion(140, "PrimaryGoalDesignation")]
    private TerrainDesignation m_primaryGoalDesignationOld;
    [RenamedInVersion(140, "m_allDesignations")]
    [DoNotSave(140, null)]
    private Lyst<TerrainDesignation> m_allDesignationsOld;
    [NewInSaveVersion(140, null, "new()", null, null)]
    private readonly Lyst<IDesignation> m_allDesignations;
    [DoNotSaveCreateNewOnLoad("new Lyst<Tile2i>(canOmitClearing: true)", 0)]
    private readonly Lyst<Tile2i> m_tilesTmp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<Tile2i> m_tilesUniqueTmp;
    [NewInSaveVersion(140, null, null, typeof (TerrainDesignationVehicleGoal.Factory), null)]
    private readonly TerrainDesignationVehicleGoal.Factory m_factory;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Actual goal designation to which the path was found. Sen when pathfinding is done.
    /// </summary>
    [NewInSaveVersion(140, null, "Option<IDesignation>.Create(m_actualGoalDesignationOld.ValueOrNull)", null, null)]
    public Option<IDesignation> ActualGoalDesignation { get; private set; }

    public Tile2i? FoundGoalPosition { get; private set; }

    [NewInSaveVersion(140, null, "m_primaryGoalDesignationOld", null, null)]
    public IDesignation PrimaryGoalDesignation { get; private set; }

    public RelTile1i ToleranceRadius { get; private set; }

    public override LocStrFormatted GoalName
    {
      get
      {
        IDesignation valueOrNull = this.ActualGoalDesignation.ValueOrNull;
        return valueOrNull == null ? LocStrFormatted.Empty : valueOrNull.Name;
      }
    }

    public TerrainDesignationVehicleGoal(TerrainDesignationVehicleGoal.Factory factory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_allDesignations = new Lyst<IDesignation>();
      this.m_tilesTmp = new Lyst<Tile2i>(true);
      this.m_tilesUniqueTmp = new Set<Tile2i>();
      // ISSUE: explicit constructor call
      base.\u002Ector(factory.m_vehicleSurfaceProvider);
      this.m_factory = factory;
    }

    public void Initialize(
      IDesignation designation,
      RelTile1i tolerance,
      IEnumerable<IDesignation> extraDesignations = null)
    {
      this.Initialize();
      this.PrimaryGoalDesignation = designation;
      this.m_allDesignations.Clear();
      this.m_allDesignations.Add(designation);
      if (extraDesignations != null)
        this.m_allDesignations.AddRange(extraDesignations);
      this.ActualGoalDesignation = Option<IDesignation>.None;
      this.FoundGoalPosition = new Tile2i?();
      this.ToleranceRadius = tolerance.CheckPositive();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      if (saveVersion >= 140)
        return;
      foreach (IDesignation designation in this.m_allDesignationsOld)
        this.m_allDesignations.Add(designation);
    }

    public override Tile3f GetGoalPosition()
    {
      return this.PrimaryGoalDesignation.GetTargetCoordAt(TerrainDesignation.Size / 2);
    }

    protected override bool ShouldCheckGoalHeights(
      int retryNumber,
      out HeightTilesF goalHeightLow,
      out HeightTilesF goalHeightHigh)
    {
      if (this.PrimaryGoalDesignation is SurfaceDesignation)
      {
        goalHeightLow = HeightTilesF.Zero;
        goalHeightHigh = HeightTilesF.Zero;
        return false;
      }
      goalHeightLow = this.m_allDesignations.Min<IDesignation, HeightTilesI>((Func<IDesignation, HeightTilesI>) (x => !(x is TerrainDesignation terrainDesignation1) ? HeightTilesI.Zero : terrainDesignation1.MinTargetHeight)).HeightTilesF - ThicknessTilesF.One;
      goalHeightHigh = this.m_allDesignations.Max<IDesignation, HeightTilesI>((Func<IDesignation, HeightTilesI>) (x => !(x is TerrainDesignation terrainDesignation2) ? HeightTilesI.Zero : terrainDesignation2.MaxTargetHeight)).HeightTilesF + ThicknessTilesF.One;
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
      Assert.That<Lyst<Tile2i>>(goalTiles).IsEmpty<Tile2i>();
      this.ActualGoalDesignation = Option<IDesignation>.None;
      if (this.PrimaryGoalDesignation == null)
      {
        Log.Error("Designation goal is not initialized.");
        distanceEstimationGoalTile = new Tile2i();
        return false;
      }
      distanceEstimationGoalTile = this.PrimaryGoalDesignation.CenterTileCoord;
      this.m_tilesTmp.Clear();
      foreach (IDesignation allDesignation in this.m_allDesignations)
      {
        if (allDesignation.IsFulfilled)
        {
          goalTiles.Add(allDesignation.OriginTileCoord);
          goalTiles.Add(allDesignation.OriginTileCoord.AddX(4));
          goalTiles.Add(allDesignation.OriginTileCoord.AddY(4));
          goalTiles.Add(allDesignation.OriginTileCoord.AddXy(4));
          goalTiles.Add(allDesignation.CenterTileCoord);
        }
        else
        {
          int num = allDesignation is TerrainDesignation ? 5 : 4;
          for (int y = 0; y < num; ++y)
          {
            for (int x = 0; x < num; ++x)
            {
              RelTile2i coord = new RelTile2i(x, y);
              if (!allDesignation.IsFulfilledAt(coord))
                this.m_tilesTmp.Add(allDesignation.OriginTileCoord + coord);
            }
          }
        }
      }
      RelTile1i tolerance = this.ToleranceRadius + extraTolerancePerRetry * retryNumber;
      this.m_tilesUniqueTmp.Clear();
      bool goalTilesInternal = TerrainDesignationVehicleGoal.expandGoalTilesByRadius(startTile, this.m_tilesTmp, tolerance, this.m_tilesUniqueTmp);
      goalTiles.AddRange((IEnumerable<Tile2i>) this.m_tilesUniqueTmp);
      return goalTilesInternal;
    }

    public override bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf)
    {
      retryPf = false;
      IDesignation designation = this.ActualGoalDesignation.HasValue ? this.ActualGoalDesignation.Value : this.PrimaryGoalDesignation;
      return designation != null && !designation.IsDestroyed && designation.IsReadyToBeFulfilled;
    }

    public override void NotifyGoalFound(Tile2i foundGoal)
    {
      Assert.That<Option<IDesignation>>(this.ActualGoalDesignation).IsNone<IDesignation>();
      this.FoundGoalPosition = new Tile2i?(foundGoal);
      IDesignation allDesignation = this.m_allDesignations[0];
      long num1 = allDesignation.CenterTileCoord.DistanceSqrTo(foundGoal);
      for (int index = 1; index < this.m_allDesignations.Count; ++index)
      {
        long num2 = this.m_allDesignations[index].CenterTileCoord.DistanceSqrTo(foundGoal);
        if (num2 < num1)
        {
          num1 = num2;
          allDesignation = this.m_allDesignations[index];
        }
      }
      this.ActualGoalDesignation = Option<IDesignation>.Create(allDesignation);
    }

    private static bool expandGoalTilesByRadius(
      Tile2i start,
      Lyst<Tile2i> candidates,
      RelTile1i tolerance,
      Set<Tile2i> goalTiles)
    {
      if (candidates.IsEmpty)
        return false;
      Assert.That<Set<Tile2i>>(goalTiles).IsEmpty<Tile2i>();
      if (tolerance <= RelTile1i.Zero)
      {
        foreach (Tile2i candidate in candidates)
        {
          if (start == candidate)
          {
            goalTiles.Clear();
            goalTiles.Add(candidate);
            return true;
          }
          goalTiles.Add(candidate);
        }
        return false;
      }
      int roundedNonNegative1 = (tolerance.Value * Fix32.OneOverSqrt2).ToIntRoundedNonNegative();
      long squared = tolerance.Squared;
      foreach (Tile2i candidate in candidates)
      {
        if ((start - candidate).LengthSqr <= squared)
        {
          goalTiles.Clear();
          goalTiles.Add(candidate);
          return true;
        }
        goalTiles.Add(candidate.AddX(tolerance.Value));
        goalTiles.Add(candidate.AddX(-tolerance.Value));
        goalTiles.Add(candidate.AddY(tolerance.Value));
        goalTiles.Add(candidate.AddY(-tolerance.Value));
        goalTiles.Add(candidate.AddXy(roundedNonNegative1));
        goalTiles.Add(candidate.AddXy(-roundedNonNegative1));
        goalTiles.Add(candidate + new RelTile2i(roundedNonNegative1, -roundedNonNegative1));
        goalTiles.Add(candidate + new RelTile2i(-roundedNonNegative1, roundedNonNegative1));
      }
      if (tolerance.Value >= 4)
      {
        int roundedNonNegative2 = (tolerance.Value * Fix32.OneOverSqrt5).ToIntRoundedNonNegative();
        int roundedNonNegative3 = (2 * tolerance.Value * Fix32.OneOverSqrt5).ToIntRoundedNonNegative();
        foreach (Tile2i candidate in candidates)
        {
          goalTiles.Add(candidate + new RelTile2i(roundedNonNegative2, -roundedNonNegative3));
          goalTiles.Add(candidate + new RelTile2i(roundedNonNegative2, roundedNonNegative3));
          goalTiles.Add(candidate + new RelTile2i(-roundedNonNegative2, roundedNonNegative3));
          goalTiles.Add(candidate + new RelTile2i(-roundedNonNegative2, -roundedNonNegative3));
          goalTiles.Add(candidate + new RelTile2i(roundedNonNegative3, -roundedNonNegative2));
          goalTiles.Add(candidate + new RelTile2i(roundedNonNegative3, roundedNonNegative2));
          goalTiles.Add(candidate + new RelTile2i(-roundedNonNegative3, roundedNonNegative2));
          goalTiles.Add(candidate + new RelTile2i(-roundedNonNegative3, -roundedNonNegative2));
        }
      }
      return false;
    }

    public override void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle)
    {
      if (isSuccess)
        this.m_factory.UnreachablesManager.MarkReachableFor((IEnumerable<IDesignation>) this.m_allDesignations, vehicle);
      else
        this.m_factory.UnreachablesManager.MarkUnreachableFor((IEnumerable<IDesignation>) this.m_allDesignations, vehicle);
    }

    public override string ToString()
    {
      return string.Format("{0} +- {1}", (object) (this.ActualGoalDesignation.ValueOrNull ?? this.PrimaryGoalDesignation), (object) this.ToleranceRadius);
    }

    public static void Serialize(TerrainDesignationVehicleGoal value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainDesignationVehicleGoal>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainDesignationVehicleGoal.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<IDesignation>.Serialize(this.ActualGoalDesignation, writer);
      writer.WriteNullableStruct<Tile2i>(this.FoundGoalPosition);
      Lyst<IDesignation>.Serialize(this.m_allDesignations, writer);
      TerrainDesignationVehicleGoal.Factory.Serialize(this.m_factory, writer);
      writer.WriteGeneric<IDesignation>(this.PrimaryGoalDesignation);
      RelTile1i.Serialize(this.ToleranceRadius, writer);
    }

    public static TerrainDesignationVehicleGoal Deserialize(BlobReader reader)
    {
      TerrainDesignationVehicleGoal designationVehicleGoal;
      if (reader.TryStartClassDeserialization<TerrainDesignationVehicleGoal>(out designationVehicleGoal))
        reader.EnqueueDataDeserialization((object) designationVehicleGoal, TerrainDesignationVehicleGoal.s_deserializeDataDelayedAction);
      return designationVehicleGoal;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      if (reader.LoadedSaveVersion < 140)
        this.m_actualGoalDesignationOld = Option<TerrainDesignation>.Deserialize(reader);
      this.ActualGoalDesignation = reader.LoadedSaveVersion >= 140 ? Option<IDesignation>.Deserialize(reader) : Option<IDesignation>.Create((IDesignation) this.m_actualGoalDesignationOld.ValueOrNull);
      this.FoundGoalPosition = reader.ReadNullableStruct<Tile2i>();
      if (reader.LoadedSaveVersion < 140)
        this.m_allDesignationsOld = Lyst<TerrainDesignation>.Deserialize(reader);
      reader.SetField<TerrainDesignationVehicleGoal>(this, "m_allDesignations", reader.LoadedSaveVersion >= 140 ? (object) Lyst<IDesignation>.Deserialize(reader) : (object) new Lyst<IDesignation>());
      reader.SetField<TerrainDesignationVehicleGoal>(this, "m_factory", reader.LoadedSaveVersion >= 140 ? (object) TerrainDesignationVehicleGoal.Factory.Deserialize(reader) : (object) (TerrainDesignationVehicleGoal.Factory) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<TerrainDesignationVehicleGoal>(this, "m_factory", typeof (TerrainDesignationVehicleGoal.Factory), true);
      reader.SetField<TerrainDesignationVehicleGoal>(this, "m_tilesTmp", (object) new Lyst<Tile2i>(true));
      reader.SetField<TerrainDesignationVehicleGoal>(this, "m_tilesUniqueTmp", (object) new Set<Tile2i>());
      if (reader.LoadedSaveVersion < 140)
        this.m_primaryGoalDesignationOld = TerrainDesignation.Deserialize(reader);
      this.PrimaryGoalDesignation = reader.LoadedSaveVersion >= 140 ? reader.ReadGenericAs<IDesignation>() : (IDesignation) this.m_primaryGoalDesignationOld;
      this.ToleranceRadius = RelTile1i.Deserialize(reader);
      reader.RegisterInitAfterLoad<TerrainDesignationVehicleGoal>(this, "initSelf", InitPriority.Normal);
    }

    static TerrainDesignationVehicleGoal()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainDesignationVehicleGoal.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleGoalBase) obj).SerializeData(writer));
      TerrainDesignationVehicleGoal.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleGoalBase) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    [OnlyForSaveCompatibility("This does not need a serializer!")]
    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public sealed class Factory
    {
      internal readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
      [NewInSaveVersion(140, null, null, typeof (UnreachableTerrainDesignationsManager), null)]
      internal readonly UnreachableTerrainDesignationsManager UnreachablesManager;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Factory(
        IVehicleSurfaceProvider vehicleSurfaceProvider,
        UnreachableTerrainDesignationsManager unreachablesManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
        this.UnreachablesManager = unreachablesManager;
      }

      public TerrainDesignationVehicleGoal Create(
        IDesignation designation,
        RelTile1i tolerance,
        IEnumerable<IDesignation> extraDesignations = null)
      {
        TerrainDesignationVehicleGoal designationVehicleGoal = new TerrainDesignationVehicleGoal(this);
        designationVehicleGoal.Initialize(designation, tolerance, extraDesignations);
        return designationVehicleGoal;
      }

      public static void Serialize(TerrainDesignationVehicleGoal.Factory value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TerrainDesignationVehicleGoal.Factory>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TerrainDesignationVehicleGoal.Factory.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
        UnreachableTerrainDesignationsManager.Serialize(this.UnreachablesManager, writer);
      }

      public static TerrainDesignationVehicleGoal.Factory Deserialize(BlobReader reader)
      {
        TerrainDesignationVehicleGoal.Factory factory;
        if (reader.TryStartClassDeserialization<TerrainDesignationVehicleGoal.Factory>(out factory))
          reader.EnqueueDataDeserialization((object) factory, TerrainDesignationVehicleGoal.Factory.s_deserializeDataDelayedAction);
        return factory;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<TerrainDesignationVehicleGoal.Factory>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
        reader.SetField<TerrainDesignationVehicleGoal.Factory>(this, "UnreachablesManager", reader.LoadedSaveVersion >= 140 ? (object) UnreachableTerrainDesignationsManager.Deserialize(reader) : (object) (UnreachableTerrainDesignationsManager) null);
        if (reader.LoadedSaveVersion >= 140)
          return;
        reader.RegisterResolvedMember<TerrainDesignationVehicleGoal.Factory>(this, "UnreachablesManager", typeof (UnreachableTerrainDesignationsManager), true);
      }

      static Factory()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TerrainDesignationVehicleGoal.Factory.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainDesignationVehicleGoal.Factory) obj).SerializeData(writer));
        TerrainDesignationVehicleGoal.Factory.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainDesignationVehicleGoal.Factory) obj).DeserializeData(reader));
      }
    }
  }
}
