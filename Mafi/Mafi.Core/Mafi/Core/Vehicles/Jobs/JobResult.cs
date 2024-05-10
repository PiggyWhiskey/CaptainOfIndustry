// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.JobResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Result of <see cref="T:Mafi.Core.Vehicles.Jobs.IVehicleJob" />
  /// </summary>
  public struct JobResult
  {
    public static readonly JobResult Empty;

    /// <summary>Next job that should the assigned vehicle execute.</summary>
    public Option<IVehicleJob> NextJob { get; }

    /// <summary>
    /// New cargo value that should be assigned to the vehicle.
    /// </summary>
    public ProductQuantity Cargo { get; }

    public JobResult(ProductQuantity cargo, Option<IVehicleJob> nextJob)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Cargo = cargo;
      this.NextJob = nextJob;
    }

    public static JobResult FromCargo(ProductQuantity newCargo)
    {
      return new JobResult(newCargo, Option<IVehicleJob>.None);
    }

    public static JobResult FromResult(JobResult result, IVehicleJob job)
    {
      return new JobResult(result.Cargo, Option<IVehicleJob>.Some(job));
    }

    static JobResult()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      JobResult.Empty = new JobResult(ProductQuantity.None, Option<IVehicleJob>.None);
    }
  }
}
