// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.VehiclePathFindingTask
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Roads;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.PathFinding
{
  /// <summary>
  /// Re-usable path finder task. This class is intended to be instantiated by owning vehicle and it should be reused
  /// for all of its path-finding tasks.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class VehiclePathFindingTask : IManagedVehiclePathFindingTask, IVehiclePathFindingTask
  {
    /// <summary>
    /// <see cref="P:Mafi.Core.PathFinding.VehiclePathFindingTask.MaxNavigateClosebyDistance" />
    /// </summary>
    internal static readonly RelTile1f DEFAULT_CLOSEST_NODE_MAX_DISTANCE;
    /// <summary>
    /// <see cref="P:Mafi.Core.PathFinding.VehiclePathFindingTask.MaxNavigateClosebyHeightDifference" />
    /// </summary>
    internal static readonly ThicknessTilesF DEFAULT_CLOSEST_NODE_MAX_HEIGHT_DIFFERENCE;
    private VehiclePathFindingTask.PathFindingResult m_result;
    private readonly Lyst<Tile2i> m_startTiles;
    private readonly Lyst<Tile2i> m_goalTiles;
    private Tile2i m_distanceEstimationGoalTile;
    [DoNotSaveCreateNewOnLoad("new Lyst<Tile2i>(canOmitClearing: true)", 0)]
    private readonly Lyst<Tile2i> m_debugOldGoalTiles;
    private Option<IVehiclePathFindingManager> m_processingOwner;
    [NewInSaveVersion(140, null, null, null, null)]
    private Tile2f? m_customStartTile;
    [NewInSaveVersion(140, null, null, null, null)]
    private Option<IVehicleGoalFull> m_vehicleGoal;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>Vehicle that owns this task.</summary>
    public IPathFindingVehicle Vehicle { get; private set; }

    public VehiclePathFindingParams PathFindingParams => this.Vehicle.PathFindingParams;

    public int MaxRetries { get; private set; }

    public RelTile1i ExtraTolerancePerRetry { get; set; }

    public bool AllowSimplePathOnly { get; private set; }

    [OnlyForSaveCompatibility(null)]
    public bool NavigateClosebyIsSufficient { get; private set; }

    /// <summary>
    /// This is distance at which path-finding tasks will succeed when require only to get close in order to
    /// complete, such as construction. Smaller values will result that such PF tasks fail more easily, larger
    /// values may cause that tasks are completed at too far distance, possibly creating ways for players to cheat.
    /// 
    /// Only works if <see cref="P:Mafi.Core.PathFinding.VehiclePathFindingTask.NavigateClosebyIsSufficient" /> is true.
    /// </summary>
    [NewInSaveVersion(140, null, "80.0.Tiles()", null, null)]
    public RelTile1f MaxNavigateClosebyDistance { get; private set; }

    /// <summary>
    /// Similar to <see cref="P:Mafi.Core.PathFinding.VehiclePathFindingTask.MaxNavigateClosebyDistance" />, but limits the max height difference between the
    /// closest point and the target.
    /// 
    /// Only works if <see cref="P:Mafi.Core.PathFinding.VehiclePathFindingTask.NavigateClosebyIsSufficient" /> is true.
    /// </summary>
    [NewInSaveVersion(168, null, "10.0.TilesThick()", null, null)]
    public ThicknessTilesF MaxNavigateClosebyHeightDifference { get; private set; }

    public bool HasResult => this.m_result.ResultStatus != 0;

    public bool IsFinished { get; private set; }

    public IPathFindingResultForVehicle Result => (IPathFindingResultForVehicle) this.m_result;

    public IIndexable<Tile2i> StartTiles => (IIndexable<Tile2i>) this.m_startTiles;

    public Tile2i DistanceEstimationStartTile { get; private set; }

    public IIndexable<Tile2i> GoalTiles => (IIndexable<Tile2i>) this.m_goalTiles;

    public Tile2i DistanceEstimationGoalTile => this.m_distanceEstimationGoalTile;

    public bool IsWaitingForProcessing => this.m_processingOwner.HasValue;

    public bool IsBeingProcessed { get; private set; }

    public Option<IVehicleGoal> Goal => this.m_vehicleGoal.As<IVehicleGoal>();

    public VehiclePathFindingTask(IPathFindingVehicle vehicle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_startTiles = new Lyst<Tile2i>();
      this.m_goalTiles = new Lyst<Tile2i>(true);
      this.m_debugOldGoalTiles = new Lyst<Tile2i>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Vehicle = vehicle.CheckNotNull<IPathFindingVehicle>();
      this.m_result = new VehiclePathFindingTask.PathFindingResult(this);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      if (saveVersion >= 140)
        return;
      this.m_vehicleGoal = this.Vehicle.NavigationGoal.As<IVehicleGoalFull>();
    }

    public void Initialize(
      IVehicleGoalFull goal,
      int maxRetries,
      RelTile1i extraTolerancePerRetry,
      bool allowSimplePathOnly,
      bool navigateClosebyIsSufficient,
      RelTile1f? maxNavigateClosebyDistance = null,
      ThicknessTilesF? maxNavigateClosebyHeightDifference = null,
      Tile2f? customStartTile = null)
    {
      Assert.That<int>(maxRetries).IsNotNegative();
      Assert.That<bool>(this.IsFinished).IsFalse("Not cleared?");
      Assert.That<Lyst<Tile2i>>(this.m_goalTiles).IsEmpty<Tile2i>("Not cleared?");
      this.MaxRetries = maxRetries;
      this.ExtraTolerancePerRetry = extraTolerancePerRetry;
      this.AllowSimplePathOnly = allowSimplePathOnly;
      this.NavigateClosebyIsSufficient = navigateClosebyIsSufficient;
      this.MaxNavigateClosebyDistance = maxNavigateClosebyDistance ?? VehiclePathFindingTask.DEFAULT_CLOSEST_NODE_MAX_DISTANCE;
      this.MaxNavigateClosebyHeightDifference = maxNavigateClosebyHeightDifference ?? VehiclePathFindingTask.DEFAULT_CLOSEST_NODE_MAX_HEIGHT_DIFFERENCE;
      this.m_customStartTile = customStartTile;
      this.m_vehicleGoal = goal.CreateOption<IVehicleGoalFull>();
    }

    public bool InitializeStartAndGoals(int retryNumber)
    {
      Assert.That<bool>(this.IsFinished).IsFalse();
      Assert.That<VehiclePfResultStatus>(this.m_result.ResultStatus).IsEqualTo<VehiclePfResultStatus>(VehiclePfResultStatus.Unknown);
      IVehicleGoalFull valueOrNull = this.m_vehicleGoal.ValueOrNull;
      if (valueOrNull == null)
      {
        Log.Error(string.Format("PF task of '{0}' is not initialized.", (object) this.Vehicle));
        return true;
      }
      if (this.m_result.NextPathSegment.HasValue)
        this.m_result.Clear();
      this.m_startTiles.Clear();
      bool goalTiles;
      if (retryNumber <= 0)
      {
        Assert.That<Lyst<Tile2i>>(this.m_goalTiles).IsEmpty<Tile2i>();
        this.m_debugOldGoalTiles.Clear();
        this.m_goalTiles.Clear();
        Tile2f startTile;
        goalTiles = this.Vehicle.GetGoalTiles(valueOrNull, 0, this.ExtraTolerancePerRetry, this.m_customStartTile, out startTile, this.m_startTiles, this.m_goalTiles, out this.m_distanceEstimationGoalTile);
        this.DistanceEstimationStartTile = this.Vehicle.PathFindingParams.RoundCenterSpace(startTile);
        this.m_startTiles.Add(this.DistanceEstimationStartTile);
      }
      else
      {
        this.m_debugOldGoalTiles.AddRange(this.m_goalTiles);
        this.m_goalTiles.Clear();
        Tile2f startTile;
        goalTiles = this.Vehicle.GetGoalTiles(valueOrNull, retryNumber, this.ExtraTolerancePerRetry, this.m_customStartTile, out startTile, this.m_startTiles, this.m_goalTiles, out this.m_distanceEstimationGoalTile);
        this.DistanceEstimationStartTile = this.Vehicle.PathFindingParams.RoundCenterSpace(startTile);
        this.m_startTiles.Add(this.DistanceEstimationStartTile);
        MafiMath.IterateCirclePoints(this.Vehicle.PathFindingParams.RequiredClearance.Value, (Action<int, int>) ((dx, dy) => this.m_startTiles.Add(this.DistanceEstimationStartTile + new RelTile2i(dx, dy))));
      }
      Assert.That<bool>(this.DistanceEstimationStartTile.IsZero).IsFalse<IVehicleGoalFull>("Zero start tile when initializing goal: {0}", valueOrNull);
      Assert.That<bool>(this.m_distanceEstimationGoalTile.IsZero).IsFalse<IVehicleGoalFull>("Zero goal tile when initializing goal: {0}", valueOrNull);
      Assert.That<bool>(!goalTiles || this.m_goalTiles.IsNotEmpty).IsTrue("Already completed but no goals.");
      return goalTiles;
    }

    public void SetResult(IVehiclePathFinder vehiclePathFinder, VehiclePfResultStatus status)
    {
      Assert.That<Option<IVehiclePathFindingManager>>(this.m_processingOwner).HasValue<IVehiclePathFindingManager>();
      Assert.That<bool>(this.IsBeingProcessed).IsTrue();
      Assert.That<bool>(this.IsFinished).IsFalse();
      this.m_result.SetResult(vehiclePathFinder, status);
      this.m_processingOwner = Option<IVehiclePathFindingManager>.None;
      this.IsBeingProcessed = false;
      this.IsFinished = true;
      switch (status)
      {
        case VehiclePfResultStatus.PathFound:
          if (DebugGameRendererConfig.SaveVehiclePathFindingSuccesses)
          {
            this.DEBUG_DrawResultMap().SaveMapAsTga("PF_Success");
            goto case VehiclePfResultStatus.Aborted;
          }
          else
            goto case VehiclePfResultStatus.Aborted;
        case VehiclePfResultStatus.Aborted:
          if (status == VehiclePfResultStatus.PathFound)
          {
            this.m_vehicleGoal.Value.OnNavigationResult(true, this.Vehicle);
            this.Vehicle.ClearUnreachableGoal();
            break;
          }
          if (status == VehiclePfResultStatus.Aborted || status == VehiclePfResultStatus.Unknown)
            break;
          this.m_vehicleGoal.Value.OnNavigationResult(false, this.Vehicle);
          this.Vehicle.SetUnreachableGoal((IVehicleGoal) this.m_vehicleGoal.Value);
          break;
        default:
          if (DebugGameRendererConfig.SaveVehiclePathFindingFailures)
          {
            this.DEBUG_DrawResultMap().SaveMapAsTga("PF_Fail");
            goto case VehiclePfResultStatus.Aborted;
          }
          else
            goto case VehiclePfResultStatus.Aborted;
      }
    }

    public void SetIsWaitingForProcessing(IVehiclePathFindingManager manager)
    {
      this.m_processingOwner = Option.Some<IVehiclePathFindingManager>(manager);
    }

    public void SetIsBeingProcessed()
    {
      Assert.That<Option<IVehiclePathFindingManager>>(this.m_processingOwner).HasValue<IVehiclePathFindingManager>();
      this.IsBeingProcessed = true;
    }

    public RelTile1i GetGoalOrthogonalDistance()
    {
      if (!this.m_vehicleGoal.IsNone)
        return this.m_vehicleGoal.Value.GetGoalPosition().Tile2i.DistanceToOrtho((this.m_customStartTile ?? this.Vehicle.Position2f).Tile2i);
      Log.Warning("GetQueuePriorityTicks: No goal?");
      return RelTile1i.Zero;
    }

    public void ResetToStart()
    {
      Assert.That<bool>(this.IsFinished).IsFalse();
      Assert.That<VehiclePfResultStatus>(this.m_result.ResultStatus).IsEqualTo<VehiclePfResultStatus>(VehiclePfResultStatus.Unknown);
      this.m_goalTiles.Clear();
      this.m_debugOldGoalTiles.Clear();
    }

    public void Clear(bool keepPath)
    {
      if (this.m_processingOwner.HasValue)
        this.m_processingOwner.Value.AbortTask((IManagedVehiclePathFindingTask) this);
      this.m_vehicleGoal = Option<IVehicleGoalFull>.None;
      this.m_customStartTile = new Tile2f?();
      this.IsFinished = false;
      this.m_goalTiles.Clear();
      this.m_debugOldGoalTiles.Clear();
      this.m_result.Clear(keepPath);
    }

    internal DebugGameMapDrawing DEBUG_DrawResultMap(bool forceEnable = false, int pixelsPerTile = 9)
    {
      Assert.That<bool>(this.IsBeingProcessed).IsFalse();
      if (DebugGameRenderer.IsDisabled && !forceEnable)
        return DebugGameRenderer.EMPTY_DEBUG_GAME_MAP_DRAWING;
      Tile2f tile2f = this.Vehicle.Position2f;
      Tile2i tile2i1 = tile2f.Tile2i;
      Tile2i tile2i2 = tile2i1;
      if (this.DistanceEstimationStartTile.IsNotZero)
      {
        tile2i1 = tile2i1.Min(this.DistanceEstimationStartTile);
        tile2i2 = tile2i2.Min(this.DistanceEstimationStartTile);
      }
      if (this.DistanceEstimationGoalTile.IsNotZero)
      {
        tile2i1 = tile2i1.Min(this.DistanceEstimationGoalTile);
        tile2i2 = tile2i2.Min(this.DistanceEstimationGoalTile);
      }
      foreach (Tile2i startTile in this.m_startTiles)
      {
        tile2i1 = tile2i1.Min(startTile);
        tile2i2 = tile2i2.Max(startTile);
      }
      foreach (Tile2i goalTile in this.m_goalTiles)
      {
        tile2i1 = tile2i1.Min(goalTile);
        tile2i2 = tile2i2.Max(goalTile);
      }
      for (Option<IVehiclePathSegment> option = this.Result.NextPathSegment; option.HasValue; option = option.Value.NextSegment)
      {
        RectangleTerrainArea2i boundingBox = option.Value.ComputeBoundingBox();
        if (boundingBox.AreaTiles > 0)
        {
          tile2i1 = tile2i1.Min(boundingBox.Origin);
          tile2i2 = tile2i2.Max(boundingBox.PlusXyTileIncl);
        }
      }
      Tile2i from = tile2i1.AddXy(-10);
      tile2i2 = tile2i2.AddXy(10);
      RelTile2i size = tile2i2 - from;
      Fix32 fix32 = this.PathFindingParams.RequiredClearance.Value.Over(2).Modulo((Fix32) 1);
      RelTile2f fixOffset = new RelTile2f(fix32, fix32);
      DebugGameMapDrawing drawing = DebugGameRenderer.DrawGameImage(from, size, pixelsPerTile, forceEnable).DrawAllTileHeights();
      PathFindingEntity pfe = this.Vehicle as PathFindingEntity;
      if (pfe != null)
      {
        drawing = drawing.DrawPathabilityOverlayFor(pfe).DrawDynamicEntity((DynamicGroundEntity) pfe, ColorRgba.White).DrawLine(pfe.Position2f, pfe.Target ?? pfe.Position2f, ColorRgba.Yellow).DrawCross((IEnumerable<KeyValuePair<Tile2f, ColorRgba>>) this.m_debugOldGoalTiles.Select<KeyValuePair<Tile2f, ColorRgba>>((Func<Tile2i, KeyValuePair<Tile2f, ColorRgba>>) (x => Make.Kvp<Tile2f, ColorRgba>(x.CornerTile2f + fixOffset, pfe.IsPathable(x) ? new ColorRgba(0, 128, (int) byte.MaxValue, 128) : new ColorRgba((int) byte.MaxValue, 128, 0, 128))))).DrawCross((IEnumerable<KeyValuePair<Tile2f, ColorRgba>>) this.m_goalTiles.Select<KeyValuePair<Tile2f, ColorRgba>>((Func<Tile2i, KeyValuePair<Tile2f, ColorRgba>>) (x => Make.Kvp<Tile2f, ColorRgba>(x.CornerTile2f + fixOffset, pfe.IsPathable(x) ? new ColorRgba(0, 128, (int) byte.MaxValue, (int) byte.MaxValue) : new ColorRgba((int) byte.MaxValue, 128, 0, (int) byte.MaxValue)))));
        if (this.Vehicle is Mafi.Core.Entities.Dynamic.Vehicle vehicle)
          drawing.DrawUnreachableDesignationsFor(vehicle);
      }
      DebugGameMapDrawing debugGameMapDrawing1 = drawing.DrawString(new RelTile2f((Fix32) 0, (Fix32) (size.Y + 3)), string.Format("Vehicle: {0}", (object) this.Vehicle), ColorRgba.White);
      tile2f = this.DistanceEstimationStartTile.CornerTile2f + fixOffset;
      Tile2f decrementY1 = tile2f.DecrementY;
      string str1 = string.Format("start ({0})", (object) this.m_startTiles.Count);
      ColorRgba color1 = new ColorRgba(0, 128, 192, (int) byte.MaxValue);
      DebugGameMapDrawing debugGameMapDrawing2 = debugGameMapDrawing1.DrawString(decrementY1, str1, color1);
      tile2f = this.DistanceEstimationGoalTile.CornerTile2f + fixOffset;
      Tile2f decrementY2 = tile2f.DecrementY;
      string str2 = string.Format("goal ({0})", (object) this.m_goalTiles.Count);
      ColorRgba color2 = new ColorRgba(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      DebugGameMapDrawing debugGameMapDrawing3 = debugGameMapDrawing2.DrawString(decrementY2, str2, color2).DrawCross(this.DistanceEstimationGoalTile.CornerTile2f + fixOffset, new ColorRgba(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)).DrawCross((IEnumerable<Tile2f>) this.m_startTiles.Select<Tile2f>((Func<Tile2i, Tile2f>) (x => x.CornerTile2f + fixOffset)), new ColorRgba(0, 128, 192, (int) byte.MaxValue)).DrawString(new RelTile2f((Fix32) 0, (Fix32) (size.Y + 2)), string.Format("status: {0}", (object) this.Vehicle.PathFindingResult.ResultStatus), ColorRgba.White).DrawString(new RelTile2f((Fix32) 0, (Fix32) (size.Y + 1)), string.Format("goal: {0}", (object) this.Vehicle.NavigationGoal.ValueOrNull), ColorRgba.White).DrawString(new RelTile2f((Fix32) 0, (Fix32) size.Y), string.Format("close is sufficient: {0}", (object) this.NavigateClosebyIsSufficient), ColorRgba.White).DrawVehiclePath(this.Result.NextPathSegment.ValueOrNull, this.PathFindingParams, new ColorRgba(0, (int) byte.MaxValue, 128, (int) byte.MaxValue));
      if (this.Result.ExploredTiles.IsNotEmpty<ExploredPfNode>())
      {
        foreach (KeyValuePair<Tile2f, Tile2f> keyValuePair in this.Result.ExploredTiles.Where<ExploredPfNode>((Func<ExploredPfNode, bool>) (x => x.IsVisitedFromStart)).Select<ExploredPfNode, KeyValuePair<Tile2f, Tile2f>>((Func<ExploredPfNode, KeyValuePair<Tile2f, Tile2f>>) (x => Make.Kvp<Tile2f, Tile2f>(this.PathFindingParams.ConvertToCenterTileSpace(x.Node), this.PathFindingParams.ConvertToCenterTileSpace(x.ParentNode)))))
          debugGameMapDrawing3.DrawLine(keyValuePair.Key, keyValuePair.Value, new ColorRgba((int) byte.MaxValue, (int) byte.MaxValue, 0, 64));
        foreach (KeyValuePair<Tile2f, Tile2f> keyValuePair in this.Result.ExploredTiles.Where<ExploredPfNode>((Func<ExploredPfNode, bool>) (x => !x.IsVisitedFromStart)).Select<ExploredPfNode, KeyValuePair<Tile2f, Tile2f>>((Func<ExploredPfNode, KeyValuePair<Tile2f, Tile2f>>) (x => Make.Kvp<Tile2f, Tile2f>(this.PathFindingParams.ConvertToCenterTileSpace(x.Node), this.PathFindingParams.ConvertToCenterTileSpace(x.ParentNode)))))
          debugGameMapDrawing3.DrawLine(keyValuePair.Key, keyValuePair.Value, new ColorRgba(0, 0, (int) byte.MaxValue, 64));
      }
      return debugGameMapDrawing3;
    }

    public static void Serialize(VehiclePathFindingTask value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehiclePathFindingTask>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehiclePathFindingTask.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AllowSimplePathOnly);
      Tile2i.Serialize(this.DistanceEstimationStartTile, writer);
      RelTile1i.Serialize(this.ExtraTolerancePerRetry, writer);
      writer.WriteBool(this.IsBeingProcessed);
      writer.WriteBool(this.IsFinished);
      writer.WriteNullableStruct<Tile2f>(this.m_customStartTile);
      Tile2i.Serialize(this.m_distanceEstimationGoalTile, writer);
      Lyst<Tile2i>.Serialize(this.m_goalTiles, writer);
      Option<IVehiclePathFindingManager>.Serialize(this.m_processingOwner, writer);
      VehiclePathFindingTask.PathFindingResult.Serialize(this.m_result, writer);
      Lyst<Tile2i>.Serialize(this.m_startTiles, writer);
      Option<IVehicleGoalFull>.Serialize(this.m_vehicleGoal, writer);
      RelTile1f.Serialize(this.MaxNavigateClosebyDistance, writer);
      ThicknessTilesF.Serialize(this.MaxNavigateClosebyHeightDifference, writer);
      writer.WriteInt(this.MaxRetries);
      writer.WriteBool(this.NavigateClosebyIsSufficient);
      writer.WriteGeneric<IPathFindingVehicle>(this.Vehicle);
    }

    public static VehiclePathFindingTask Deserialize(BlobReader reader)
    {
      VehiclePathFindingTask vehiclePathFindingTask;
      if (reader.TryStartClassDeserialization<VehiclePathFindingTask>(out vehiclePathFindingTask))
        reader.EnqueueDataDeserialization((object) vehiclePathFindingTask, VehiclePathFindingTask.s_deserializeDataDelayedAction);
      return vehiclePathFindingTask;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllowSimplePathOnly = reader.ReadBool();
      this.DistanceEstimationStartTile = Tile2i.Deserialize(reader);
      this.ExtraTolerancePerRetry = RelTile1i.Deserialize(reader);
      this.IsBeingProcessed = reader.ReadBool();
      this.IsFinished = reader.ReadBool();
      this.m_customStartTile = reader.LoadedSaveVersion >= 140 ? reader.ReadNullableStruct<Tile2f>() : new Tile2f?();
      reader.SetField<VehiclePathFindingTask>(this, "m_debugOldGoalTiles", (object) new Lyst<Tile2i>(true));
      this.m_distanceEstimationGoalTile = Tile2i.Deserialize(reader);
      reader.SetField<VehiclePathFindingTask>(this, "m_goalTiles", (object) Lyst<Tile2i>.Deserialize(reader));
      this.m_processingOwner = Option<IVehiclePathFindingManager>.Deserialize(reader);
      this.m_result = VehiclePathFindingTask.PathFindingResult.Deserialize(reader);
      reader.SetField<VehiclePathFindingTask>(this, "m_startTiles", (object) Lyst<Tile2i>.Deserialize(reader));
      this.m_vehicleGoal = reader.LoadedSaveVersion >= 140 ? Option<IVehicleGoalFull>.Deserialize(reader) : new Option<IVehicleGoalFull>();
      this.MaxNavigateClosebyDistance = reader.LoadedSaveVersion >= 140 ? RelTile1f.Deserialize(reader) : 80.0.Tiles();
      this.MaxNavigateClosebyHeightDifference = reader.LoadedSaveVersion >= 168 ? ThicknessTilesF.Deserialize(reader) : 10.0.TilesThick();
      this.MaxRetries = reader.ReadInt();
      this.NavigateClosebyIsSufficient = reader.ReadBool();
      this.Vehicle = reader.ReadGenericAs<IPathFindingVehicle>();
      reader.RegisterInitAfterLoad<VehiclePathFindingTask>(this, "initSelf", InitPriority.Normal);
    }

    static VehiclePathFindingTask()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehiclePathFindingTask.DEFAULT_CLOSEST_NODE_MAX_DISTANCE = 80.0.Tiles();
      VehiclePathFindingTask.DEFAULT_CLOSEST_NODE_MAX_HEIGHT_DIFFERENCE = 10.0.TilesThick();
      VehiclePathFindingTask.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehiclePathFindingTask) obj).SerializeData(writer));
      VehiclePathFindingTask.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehiclePathFindingTask) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    [MemberRemovedInSaveVersion("m_pathTilesRaw", 140, typeof (Stak<Tile2i>), 0, false)]
    private sealed class PathFindingResult : IPathFindingResultForVehicle, IPathFindingResult
    {
      private readonly VehiclePathFindingTask m_parentTask;
      [DoNotSaveCreateNewOnLoad("new Lyst<ExploredPfNode>(canOmitClearing: true)", 0)]
      private readonly Lyst<ExploredPfNode> m_exploredTiles;
      private Tile2i m_goalTileRaw;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public IVehiclePathFindingTask Task => (IVehiclePathFindingTask) this.m_parentTask;

      public bool HasNextPathSegment => this.NextPathSegment.HasValue;

      public VehiclePfResultStatus ResultStatus { get; private set; }

      [NewInSaveVersion(140, null, "new()", null, null)]
      public Option<IVehiclePathSegment> NextPathSegment { get; private set; }

      public IIndexable<ExploredPfNode> ExploredTiles
      {
        get => (IIndexable<ExploredPfNode>) this.m_exploredTiles;
      }

      public Tile2i GoalRawTile => this.m_goalTileRaw;

      internal int PathFindingId { get; private set; }

      public PathFindingResult(VehiclePathFindingTask parentTask)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_exploredTiles = new Lyst<ExploredPfNode>(true);
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_parentTask = parentTask;
      }

      public void SetResult(IVehiclePathFinder vehiclePathFinder, VehiclePfResultStatus status)
      {
        Assert.That<VehiclePfResultStatus>(this.ResultStatus).IsEqualTo<VehiclePfResultStatus>(VehiclePfResultStatus.Unknown, "Result was not reset.");
        this.ResultStatus = status;
        this.PathFindingId = vehiclePathFinder.CurrentPfId;
        this.m_exploredTiles.Clear();
        IVehiclePathSegment firstPathSegment;
        if (status == VehiclePfResultStatus.PathFound && vehiclePathFinder.TryReconstructFoundPath(out firstPathSegment, out this.m_goalTileRaw))
          this.NextPathSegment = firstPathSegment.SomeOption<IVehiclePathSegment>();
        if (!this.m_parentTask.Vehicle.TrackExploredTiles && !DebugGameRenderer.IsEnabled)
          return;
        vehiclePathFinder.GetExploredTiles(this.m_exploredTiles);
      }

      public IVehiclePathSegment PopNextPathSegment()
      {
        IVehiclePathSegment vehiclePathSegment = this.NextPathSegment.Value;
        this.NextPathSegment = vehiclePathSegment.NextSegment;
        return vehiclePathSegment;
      }

      public void Clear(bool keepPath = false)
      {
        this.ResultStatus = VehiclePfResultStatus.Unknown;
        if (!keepPath)
          this.NextPathSegment = Option<IVehiclePathSegment>.None;
        this.m_exploredTiles.Clear();
      }

      public static void Serialize(
        VehiclePathFindingTask.PathFindingResult value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<VehiclePathFindingTask.PathFindingResult>(value))
          return;
        writer.EnqueueDataSerialization((object) value, VehiclePathFindingTask.PathFindingResult.s_serializeDataDelayedAction);
      }

      private void SerializeData(BlobWriter writer)
      {
        Tile2i.Serialize(this.m_goalTileRaw, writer);
        VehiclePathFindingTask.Serialize(this.m_parentTask, writer);
        Option<IVehiclePathSegment>.Serialize(this.NextPathSegment, writer);
        writer.WriteInt(this.PathFindingId);
        writer.WriteInt((int) this.ResultStatus);
      }

      public static VehiclePathFindingTask.PathFindingResult Deserialize(BlobReader reader)
      {
        VehiclePathFindingTask.PathFindingResult pathFindingResult;
        if (reader.TryStartClassDeserialization<VehiclePathFindingTask.PathFindingResult>(out pathFindingResult))
          reader.EnqueueDataDeserialization((object) pathFindingResult, VehiclePathFindingTask.PathFindingResult.s_deserializeDataDelayedAction);
        return pathFindingResult;
      }

      private void DeserializeData(BlobReader reader)
      {
        reader.SetField<VehiclePathFindingTask.PathFindingResult>(this, "m_exploredTiles", (object) new Lyst<ExploredPfNode>(true));
        this.m_goalTileRaw = Tile2i.Deserialize(reader);
        reader.SetField<VehiclePathFindingTask.PathFindingResult>(this, "m_parentTask", (object) VehiclePathFindingTask.Deserialize(reader));
        if (reader.LoadedSaveVersion < 140)
          Stak<Tile2i>.Deserialize(reader);
        this.NextPathSegment = reader.LoadedSaveVersion >= 140 ? Option<IVehiclePathSegment>.Deserialize(reader) : new Option<IVehiclePathSegment>();
        this.PathFindingId = reader.ReadInt();
        this.ResultStatus = (VehiclePfResultStatus) reader.ReadInt();
      }

      static PathFindingResult()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        VehiclePathFindingTask.PathFindingResult.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehiclePathFindingTask.PathFindingResult) obj).SerializeData(writer));
        VehiclePathFindingTask.PathFindingResult.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehiclePathFindingTask.PathFindingResult) obj).DeserializeData(reader));
      }
    }
  }
}
