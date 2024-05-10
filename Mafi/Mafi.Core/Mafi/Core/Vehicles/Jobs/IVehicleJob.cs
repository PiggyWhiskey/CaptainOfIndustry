// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.IVehicleJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents an abstract task of a vehicle. This task can be queried for info of current operation or canceled.
  /// </summary>
  /// <remarks>
  /// It is responsibility of individual jobs to call <see cref="!:VehicleJobsSequence.CancelAll(bool)" /> when the job gets
  /// canceled to cancel whole sequence. There might be some jobs that may not require canceling the whole sequence.
  /// </remarks>
  public interface IVehicleJob : IVehicleJobReadOnly, IIsSafeAsHashKey
  {
    bool IsDestroyed { get; }

    /// <summary>
    /// Requests job cancellation. Returns <c>true</c> if the job was cancelled immediately and can be removed
    /// from the queue. This will notify all involved entities to drop it.
    /// </summary>
    bool RequestCancel();

    /// <summary>
    /// Whether this job is purposefully not moving with the vehicle, such as waiting.
    /// Vehicles are monitored for movement to detect when they are stuck.
    /// </summary>
    bool SkipNoMovementMonitoring { get; }

    /// <summary>How much of fuel is the job currently consuming.</summary>
    VehicleFuelConsumption CurrentFuelConsumption { get; }

    void AddObserver(IVehicleJobObserver observer);

    void RemoveObserver(IVehicleJobObserver observer);
  }
}
