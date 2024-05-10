// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.BalancingJobSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Trucks;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  public struct BalancingJobSpec
  {
    public readonly Truck Truck;
    public ProductQuantity ProductQuantity;
    public readonly Option<RegisteredInputBuffer> InputBuffer;
    public readonly Option<TerrainDesignation> DumpDesignation;
    public readonly Option<Lyst<TerrainDesignation>> ExtraDumpDesignations;
    public readonly Option<Lyst<SurfaceDesignation>> SurfacePlaceDesignations;
    public readonly Option<Lyst<SurfaceDesignation>> SurfaceClearDesignations;
    public readonly Option<RegisteredOutputBuffer> OutputBuffer;
    public readonly Option<Lyst<SecondaryInputBufferSpec>> SecondaryInputBuffers;
    public readonly Option<Lyst<SecondaryOutputBufferSpec>> SecondaryOutputBuffers;

    public BalancingJobSpec(
      Truck truck,
      RegisteredInputBuffer inputBuffer,
      RegisteredOutputBuffer outputBuffer)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.InputBuffer = (Option<RegisteredInputBuffer>) Option.None;
      this.DumpDesignation = (Option<TerrainDesignation>) Option.None;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) Option.None;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) Option.None;
      this.SecondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
      this.SecondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      this.Truck = truck;
      this.InputBuffer = (Option<RegisteredInputBuffer>) inputBuffer;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) outputBuffer;
      this.ProductQuantity = ProductQuantity.None;
    }

    public BalancingJobSpec(
      Truck truck,
      TerrainDesignation designation,
      Lyst<TerrainDesignation> extraDesignations,
      RegisteredOutputBuffer outputBuffer,
      ProductQuantity productQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.InputBuffer = (Option<RegisteredInputBuffer>) Option.None;
      this.DumpDesignation = (Option<TerrainDesignation>) Option.None;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) Option.None;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) Option.None;
      this.SecondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
      this.SecondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      this.Truck = truck;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) outputBuffer;
      this.DumpDesignation = (Option<TerrainDesignation>) designation;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) extraDesignations;
      this.ProductQuantity = productQuantity;
    }

    public BalancingJobSpec(
      Truck truck,
      Lyst<SurfaceDesignation> clearDesignations,
      ProductQuantity productQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.InputBuffer = (Option<RegisteredInputBuffer>) Option.None;
      this.DumpDesignation = (Option<TerrainDesignation>) Option.None;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) Option.None;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) Option.None;
      this.SecondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
      this.SecondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      this.Truck = truck;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) clearDesignations;
      this.ProductQuantity = productQuantity;
    }

    public BalancingJobSpec(
      Truck truck,
      Lyst<SurfaceDesignation> designations,
      RegisteredInputBuffer buffer,
      ProductQuantity productQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.InputBuffer = (Option<RegisteredInputBuffer>) Option.None;
      this.DumpDesignation = (Option<TerrainDesignation>) Option.None;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) Option.None;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) Option.None;
      this.SecondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
      this.SecondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      this.Truck = truck;
      this.InputBuffer = (Option<RegisteredInputBuffer>) buffer;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) designations;
      this.ProductQuantity = productQuantity;
    }

    public BalancingJobSpec(
      Truck truck,
      Lyst<SurfaceDesignation> designations,
      RegisteredOutputBuffer buffer,
      ProductQuantity productQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.InputBuffer = (Option<RegisteredInputBuffer>) Option.None;
      this.DumpDesignation = (Option<TerrainDesignation>) Option.None;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) Option.None;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) Option.None;
      this.SecondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
      this.SecondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      this.Truck = truck;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) buffer;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) designations;
      this.ProductQuantity = productQuantity;
    }

    public BalancingJobSpec(
      Truck truck,
      Lyst<SurfaceDesignation> clearDesignations,
      Lyst<SurfaceDesignation> placeDesignations,
      ProductQuantity productQuantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.InputBuffer = (Option<RegisteredInputBuffer>) Option.None;
      this.DumpDesignation = (Option<TerrainDesignation>) Option.None;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) Option.None;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) Option.None;
      this.SecondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
      this.SecondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      this.Truck = truck;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) clearDesignations;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) placeDesignations;
      this.ProductQuantity = productQuantity;
    }

    public BalancingJobSpec(
      Truck truck,
      RegisteredInputBuffer inputBuffer,
      RegisteredOutputBuffer outputBuffer,
      ProductQuantity productQuantity,
      Option<Lyst<SecondaryInputBufferSpec>> secondaryInputBuffers,
      Option<Lyst<SecondaryOutputBufferSpec>> secondaryOutputBuffers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.InputBuffer = (Option<RegisteredInputBuffer>) Option.None;
      this.DumpDesignation = (Option<TerrainDesignation>) Option.None;
      this.ExtraDumpDesignations = (Option<Lyst<TerrainDesignation>>) Option.None;
      this.SurfacePlaceDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.SurfaceClearDesignations = (Option<Lyst<SurfaceDesignation>>) Option.None;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) Option.None;
      this.SecondaryInputBuffers = (Option<Lyst<SecondaryInputBufferSpec>>) Option.None;
      this.SecondaryOutputBuffers = (Option<Lyst<SecondaryOutputBufferSpec>>) Option.None;
      this.Truck = truck;
      this.InputBuffer = (Option<RegisteredInputBuffer>) inputBuffer;
      this.OutputBuffer = (Option<RegisteredOutputBuffer>) outputBuffer;
      this.ProductQuantity = productQuantity;
      this.SecondaryInputBuffers = secondaryInputBuffers;
      this.SecondaryOutputBuffers = secondaryOutputBuffers;
      Assert.That<Quantity>(productQuantity.Quantity).IsPositive();
      Assert.That<ProductProto>(inputBuffer.Product).IsEqualTo<ProductProto>(outputBuffer.Product);
    }

    public Quantity GetSecondaryInputsQuantitySum()
    {
      Lyst<SecondaryInputBufferSpec> valueOrNull = this.SecondaryInputBuffers.ValueOrNull;
      return valueOrNull == null ? Quantity.Zero : valueOrNull.Sum<SecondaryInputBufferSpec>((Func<SecondaryInputBufferSpec, int>) (x => x.Quantity.Value)).Quantity();
    }

    public Quantity GetSecondaryOutputsQuantitySum()
    {
      Lyst<SecondaryOutputBufferSpec> valueOrNull = this.SecondaryOutputBuffers.ValueOrNull;
      return valueOrNull == null ? Quantity.Zero : valueOrNull.Sum<SecondaryOutputBufferSpec>((Func<SecondaryOutputBufferSpec, int>) (x => x.Quantity.Value)).Quantity();
    }
  }
}
