// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleQueueAssertions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles.Jobs;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public static class VehicleQueueAssertions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void NotContains<TVehicle>(
      this Assertion<IVehicleQueueFriend<TVehicle>> actual,
      VehicleQueueJob<TVehicle> job,
      string message = "")
      where TVehicle : Vehicle
    {
      if (!actual.Value.Contains(job))
        return;
      Mafi.Assert.FailAssertion(string.Format("Job {0} of Vehicle {1} is in the VehicleQueue of owner {2}", (object) job, (object) job.Vehicle, (object) actual.Value.Owner) + " even though it is not supposed to.", message);
    }
  }
}
