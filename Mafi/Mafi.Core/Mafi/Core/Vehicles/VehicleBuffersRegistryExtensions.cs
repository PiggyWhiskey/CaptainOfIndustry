// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.VehicleBuffersRegistryExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public static class VehicleBuffersRegistryExtensions
  {
    public static void RegisterInputBufferAndAssert(
      this IVehicleBuffersRegistry registry,
      IStaticEntity entity,
      IProductBuffer buffer,
      IInputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled = false,
      bool isFallbackOnly = false,
      bool allowDeliveryAtDistanceWhenBlocked = false)
    {
      if (!buffer.Product.CanBeLoadedOnTruck)
        return;
      Assert.That<bool>(registry.TryRegisterInputBuffer(entity, buffer, priorityProvider, alwaysEnabled, isFallbackOnly, allowDeliveryAtDistanceWhenBlocked)).IsTrue();
    }

    public static void RegisterOutputBufferAndAssert(
      this IVehicleBuffersRegistry registry,
      IStaticEntity entity,
      IProductBuffer buffer,
      IOutputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled = false,
      bool useFallbackIfNeeded = false,
      bool allowPickupAtDistanceWhenBlocked = false)
    {
      if (!buffer.Product.CanBeLoadedOnTruck)
        return;
      Assert.That<bool>(registry.TryRegisterOutputBuffer(entity, buffer, priorityProvider, alwaysEnabled, useFallbackIfNeeded, allowPickupAtDistanceWhenBlocked)).IsTrue();
    }

    public static void UnregisterInputBufferAndAssert(
      this IVehicleBuffersRegistry registry,
      IProductBuffer buffer,
      bool keepCurrentReservations = false)
    {
      if (!buffer.Product.CanBeLoadedOnTruck)
        return;
      Assert.That<bool>(registry.TryUnregisterInputBuffer(buffer, keepCurrentReservations)).IsTrue<ProductProto>("Failed to unregister input buffer for {0}", buffer.Product);
    }

    public static void UnregisterOutputBufferAndAssert(
      this IVehicleBuffersRegistry registry,
      IProductBuffer buffer)
    {
      if (!buffer.Product.CanBeLoadedOnTruck)
        return;
      Assert.That<bool>(registry.TryUnregisterOutputBuffer(buffer)).IsTrue<ProductProto>("Failed to unregister output buffer for {0}", buffer.Product);
    }

    public static RegisteredInputBuffer GetInputBuffer(
      this IVehicleBuffersRegistry registry,
      IStaticEntity entity,
      ProductProto product)
    {
      Option<RegisteredInputBuffer> inputBuffer = registry.TryGetInputBuffer(entity, product);
      return inputBuffer.HasValue ? inputBuffer.Value : throw new ArgumentException("Input buffer not found!");
    }

    public static RegisteredOutputBuffer GetOutputBuffer(
      this IVehicleBuffersRegistry registry,
      IStaticEntity entity,
      ProductProto product)
    {
      Option<RegisteredOutputBuffer> outputBuffer = registry.TryGetOutputBuffer(entity, product);
      return outputBuffer.HasValue ? outputBuffer.Value : throw new ArgumentException("Output buffer not found!");
    }
  }
}
