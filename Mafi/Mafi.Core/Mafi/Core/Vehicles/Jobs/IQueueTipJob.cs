// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.IQueueTipJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job to be started after a vehicle gets to the front of the queue. The vehicle stays at the front of
  /// the queue until tho job is done. After the job is done, the vehicle is released from the queue.
  /// </summary>
  public interface IQueueTipJob : IVehicleJob, IVehicleJobReadOnly, IIsSafeAsHashKey
  {
    /// <summary>
    /// Whether the next vehicle in queue should wait behind owner of this job or behind owner of the whole queue.
    /// </summary>
    bool WaitBehindQueueTipVehicle { get; }
  }
}
