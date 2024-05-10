// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.IVehicleFriend
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Vehicles.Jobs;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public interface IVehicleFriend
  {
    /// <summary>
    /// Cancels all jobs in the vehicle queue. Caller's `Cancel` method is not called but all jobs including the
    /// caller are returned to pool and their data is reset. This should only be called as a last line in Cancel.
    /// </summary>
    void AlsoCancelAllOtherJobs(VehicleJob caller);

    /// <summary>Cancels all jobs except given job.</summary>
    void CancelAllJobsExcept(VehicleJob job);
  }
}
