// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IEntityAssignedWithVehicles
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Interface for entities that can be assigned with vehicles (e.g. storage, excavator).
  /// </summary>
  public interface IEntityAssignedWithVehicles : IEntity, IIsSafeAsHashKey
  {
    bool CanVehicleBeAssigned(DynamicEntityProto vehicle);

    Tile2f Position2f { get; }

    void AssignVehicle(Vehicle vehicle, bool doNotCancelJobs = false);

    void UnassignVehicle(Vehicle vehicle, bool cancelJobs = true);

    /// <summary>
    /// All assigned vehicles.
    /// Careful: this returns internal cache!
    /// </summary>
    IIndexable<Vehicle> AllVehicles { get; }
  }
}
