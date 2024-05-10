// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.IVehicleBuffersRegistry
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Trucks;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public interface IVehicleBuffersRegistry
  {
    Option<RegisteredOutputBuffer> TryGetProductOutputForVehicle(
      Vehicle vehicle,
      ProductProto product,
      IStaticEntity entityToIgnore = null,
      IReadOnlySet<IEntityAssignedAsOutput> preferredEntities = null,
      bool isForRefueling = false,
      bool forcePreferredEntity = false);

    Option<RegisteredInputBuffer> TryGetProductInputForVehicle(
      Vehicle vehicle,
      ProductQuantity productQuantity,
      Option<IReadOnlySet<IEntityAssignedAsInput>> preferredEntities,
      out bool hadEligibleAssignedEntity,
      bool allowOnlyPreferredEntities = false,
      IEntityAssignedAsOutput customAssignedEntity = null);

    Option<RegisteredInputBuffer> TryGetFallbackInputForVehicle(ProductProto product);

    void RegisterTruckForBalancingJob(Truck truck);

    bool TryRegisterInputBuffer(
      IStaticEntity entity,
      IProductBuffer buffer,
      IInputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled = false,
      bool isFallbackOnly = false,
      bool allowDeliveryAtDistanceWhenBlocked = false);

    bool TryUnregisterInputBuffer(IProductBuffer buffer, bool keepCurrentReservations = false);

    bool TryRegisterOutputBuffer(
      IStaticEntity entity,
      IProductBuffer buffer,
      IOutputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled = false,
      bool useFallbackIfNeeded = false,
      bool allowPickupAtDistanceWhenBlocked = false);

    bool TryUnregisterOutputBuffer(IProductBuffer buffer);

    void UnregisterAllBuffers(IStaticEntity entity);

    void ClearAndCancelAllJobs(IStaticEntity entity);

    Option<RegisteredInputBuffer> TryGetInputBuffer(IStaticEntity entity, ProductProto product);

    Option<RegisteredOutputBuffer> TryGetOutputBuffer(IStaticEntity entity, ProductProto product);
  }
}
