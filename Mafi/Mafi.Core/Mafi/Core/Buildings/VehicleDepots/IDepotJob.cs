// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.VehicleDepots.IDepotJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Buildings.VehicleDepots
{
  /// <summary>
  /// Interface to identify depot jobs. This is just to avoid mistakes of passing invalid jobs to depot.
  /// </summary>
  public interface IDepotJob : IVehicleJob, IVehicleJobReadOnly, IIsSafeAsHashKey
  {
  }
}
