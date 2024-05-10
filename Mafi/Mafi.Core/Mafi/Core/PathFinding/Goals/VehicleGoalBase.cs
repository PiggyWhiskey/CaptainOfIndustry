// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.Goals.VehicleGoalBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.PathFinding.Goals
{
  /// <summary>
  /// This class provides height-based goal checking. If derived goal class does not need this, it can implement
  /// <see cref="T:Mafi.Core.PathFinding.Goals.IVehicleGoal" /> directly.
  /// </summary>
  public abstract class VehicleGoalBase : IVehicleGoalFull, IVehicleGoal
  {
    protected readonly IVehicleSurfaceProvider VehicleSurfaceProvider;

    public bool IsInitialized { get; private set; }

    public abstract LocStrFormatted GoalName { get; }

    protected VehicleGoalBase(IVehicleSurfaceProvider vehicleSurfaceProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleSurfaceProvider = vehicleSurfaceProvider;
    }

    /// <summary>
    /// Ctor and initialization are separate to support pooling.
    /// </summary>
    protected void Initialize()
    {
      Assert.That<bool>(this.IsInitialized).IsFalse("Goal already initialized.");
      this.IsInitialized = true;
    }

    protected virtual void Reset()
    {
      Assert.That<bool>(this.IsInitialized).IsTrue("Goal was not initialized.");
      this.IsInitialized = false;
    }

    protected abstract bool ShouldCheckGoalHeights(
      int retryNumber,
      out HeightTilesF goalHeightLow,
      out HeightTilesF goalHeightHigh);

    public abstract Tile3f GetGoalPosition();

    public bool GetGoalTiles(
      Tile2i startTile,
      VehiclePathFindingParams pfParams,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile,
      int retryNumber,
      RelTile1i extraTolerancePerRetry)
    {
      Assert.That<bool>(this.IsInitialized).IsTrue("Goal was not initialized.");
      if (this.GetGoalTilesInternal(startTile, pfParams, goalTiles, out distanceEstimationGoalTile, retryNumber, extraTolerancePerRetry))
        return true;
      HeightTilesF goalHeightLow;
      HeightTilesF goalHeightHigh;
      if (this.ShouldCheckGoalHeights(retryNumber, out goalHeightLow, out goalHeightHigh))
      {
        HeightTilesF heightTilesF1 = goalHeightLow - ThicknessTilesF.One;
        HeightTilesF heightTilesF2 = goalHeightHigh + ThicknessTilesF.One;
        for (int index = 0; index < goalTiles.Count; ++index)
        {
          HeightTilesF vehicleSurfaceAt = this.VehicleSurfaceProvider.GetVehicleSurfaceAt(goalTiles[index], out bool _);
          if (vehicleSurfaceAt <= heightTilesF1 || vehicleSurfaceAt >= heightTilesF2)
          {
            goalTiles.RemoveAtReplaceWithLast(index);
            --index;
          }
        }
      }
      return false;
    }

    protected abstract bool GetGoalTilesInternal(
      Tile2i startTile,
      VehiclePathFindingParams pfParams,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile,
      int retryNumber,
      RelTile1i extraTolerancePerRetry);

    public abstract bool IsGoalValid(PathFindingEntity vehicle, out bool retryPf);

    public virtual void NotifyGoalFound(Tile2i foundGoal)
    {
    }

    public abstract void OnNavigationResult(bool isSuccess, IPathFindingVehicle vehicle);

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsInitialized);
      writer.WriteGeneric<IVehicleSurfaceProvider>(this.VehicleSurfaceProvider);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsInitialized = reader.ReadBool();
      reader.SetField<VehicleGoalBase>(this, "VehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
    }
  }
}
