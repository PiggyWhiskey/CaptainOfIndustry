// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.TruckProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Trucks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public class TruckProtoBuilder : IProtoBuilder
  {
    private readonly FuelTankProtoBuilder m_tankBuilder;

    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public TruckProtoBuilder(ProtoRegistrator registrator, FuelTankProtoBuilder tankBuilder)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
      this.m_tankBuilder = tankBuilder;
    }

    [MustUseReturnValue]
    public TruckProtoBuilder.TruckProtoBuilderState Start(string name, DynamicEntityProto.ID id)
    {
      return new TruckProtoBuilder.TruckProtoBuilderState((IProtoBuilder) this, id, name, this.m_tankBuilder);
    }

    public class TruckProtoBuilderState : 
      DrivingEntityProtoBuilderState<TruckProtoBuilder.TruckProtoBuilderState>
    {
      protected readonly DynamicEntityProto.ID ProtoId;
      private Quantity? m_capacity;
      private ImmutableArray<ThicknessTilesF> m_dumpedThickByDist;
      private readonly Lyst<AttachmentProto> m_attachments;
      private Option<AttachmentProto> m_defaultAttachment;
      private ImmutableArray<string> m_steeringWheelsSubmodelPaths;
      private ImmutableArray<string> m_staticWheelsSubmodelPaths;
      private Duration m_cargoPickupDuration;
      private RelTile1f? m_wheelDiameter;
      private ProductType? m_productType;
      private string m_engineSoundPath;
      private RelTile1i? m_minDumpingDistance;
      private RelTile1i? m_maxDumpingDistance;
      private Option<DrivingEntityProto> m_nextTier;

      public TruckProtoBuilderState(
        IProtoBuilder builder,
        DynamicEntityProto.ID id,
        string name,
        FuelTankProtoBuilder tankBuilder)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_attachments = new Lyst<AttachmentProto>();
        this.m_cargoPickupDuration = 1.Seconds();
        this.m_nextTier = Option<DrivingEntityProto>.None;
        // ISSUE: explicit constructor call
        base.\u002Ector(builder, (Proto.ID) id, name, tankBuilder);
        this.ProtoId = id;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetCapacity(int quantity)
      {
        this.m_capacity = new Quantity?(new Quantity(quantity).CheckPositive());
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetDumpingDistance(
        RelTile1i min,
        RelTile1i max)
      {
        this.m_minDumpingDistance = new RelTile1i?(min);
        this.m_maxDumpingDistance = new RelTile1i?(max);
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetDumpedThicknessByDistanceMeters(
        params float[] thicknessMeters)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_dumpedThickByDist = ((IEnumerable<float>) thicknessMeters).Select<float, ThicknessTilesF>(TruckProtoBuilder.TruckProtoBuilderState.\u003C\u003EO.\u003C0\u003E__FromMeters ?? (TruckProtoBuilder.TruckProtoBuilderState.\u003C\u003EO.\u003C0\u003E__FromMeters = new Func<float, ThicknessTilesF>(ThicknessTilesF.FromMeters))).ToImmutableArray<ThicknessTilesF>();
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState AddAttachment(AttachmentProto attachmentProto)
      {
        this.m_attachments.Add(attachmentProto);
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetDefaultAttachment(
        AttachmentProto attachmentProto)
      {
        this.m_defaultAttachment = (Option<AttachmentProto>) attachmentProto;
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetSteeringWheelsSubmodelPaths(
        params string[] paths)
      {
        this.m_steeringWheelsSubmodelPaths = ((ICollection<string>) paths).ToImmutableArray<string>();
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetStaticWheelsSubmodelPaths(
        params string[] paths)
      {
        this.m_staticWheelsSubmodelPaths = ((ICollection<string>) paths).ToImmutableArray<string>();
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetCargoPickupDuration(
        Duration cargoPickupDuration)
      {
        this.m_cargoPickupDuration = cargoPickupDuration;
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetWheelDiameter(RelTile1f wheelDiameter)
      {
        this.m_wheelDiameter = new RelTile1f?(wheelDiameter);
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetFixedProductType(ProductType productType)
      {
        this.m_productType = new ProductType?(productType);
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetEngineSound(string engineSoundPath)
      {
        this.m_engineSoundPath = engineSoundPath;
        return this;
      }

      [MustUseReturnValue]
      public TruckProtoBuilder.TruckProtoBuilderState SetNextTier(DrivingEntityProto nextTier)
      {
        this.m_nextTier = (Option<DrivingEntityProto>) nextTier;
        return this;
      }

      private TruckProto.Gfx createGfx()
      {
        if (this.PrefabPath.IsNone)
          return TruckProto.Gfx.Empty;
        string prefabPath = this.PrefabPath.Value;
        Option<string> customIconPath = this.CustomIconPath;
        RelTile2f frontContactPtsOffset = this.ValueOrThrow<RelTile2f>(this.FrontContactPtsOffset, "FrontContactPtsOffset");
        RelTile2f rearContactPtsOffset = this.ValueOrThrow<RelTile2f>(this.RearContactPtsOffset, "RearContactPtsOffset");
        ImmutableArray<DynamicEntityDustParticlesSpec> immutableArray = this.DustParticlesSpecs.ToImmutableArray();
        Option<VehicleExhaustParticlesSpec> exhaustParticlesSpec = this.ExhaustParticlesSpec;
        string engineSoundPath = this.m_engineSoundPath ?? "EMPTY";
        RelTile1f relTile1f = this.ValueOrThrow<RelTile1f>(this.m_wheelDiameter, "WheelDiameter");
        ImmutableArray<string> steeringWheelsSubmodelPaths = this.NotEmptyOrThrow<string>(this.m_steeringWheelsSubmodelPaths, "SteeringWheelsSubmodelPaths");
        RelTile1f wheelDiameter = relTile1f;
        ImmutableArray<string> staticWheelsSubmodelPaths = this.NotEmptyOrThrow<string>(this.m_staticWheelsSubmodelPaths, "StaticWheelsSubmodelPaths");
        return new TruckProto.Gfx(prefabPath, customIconPath, frontContactPtsOffset, rearContactPtsOffset, immutableArray, exhaustParticlesSpec, engineSoundPath, "EMPTY", steeringWheelsSubmodelPaths, wheelDiameter, staticWheelsSubmodelPaths);
      }

      public TruckProto BuildAndAdd()
      {
        return this.AddToDb<TruckProto>(new TruckProto(this.ProtoId, this.Strings, this.ValueOrThrow<RelTile3f>(this.Size, "Entity size"), this.Costs, 1, this.ValueOrThrow<DrivingData>(this.DrivingData, "DrivingData"), this.ValueOrThrow<Quantity>(this.m_capacity, "Capacity"), (Func<ProductProto, bool>) (x => true), this.NotEmptyOrThrow<ThicknessTilesF>(this.m_dumpedThickByDist, "DumpedThicknessByDistance"), this.m_attachments.ToArray(), this.m_productType.HasValue ? (Option<AttachmentProto>) this.m_attachments.FirstOrDefault<AttachmentProto>() : this.m_defaultAttachment, this.m_cargoPickupDuration, this.ValueOrThrow<RelTile1i>(this.m_minDumpingDistance, "m_minDumpingDistance"), this.ValueOrThrow<RelTile1i>(this.m_maxDumpingDistance, "m_maxDumpingDistance"), this.FuelTank, this.ValueOrThrow<VehiclePathFindingParams>(this.PathFindingParams, "PathFindingParams"), this.NotEmptyOrThrow<ThicknessTilesF>(this.DisruptionByDistance, "DisruptionByDistance"), this.ValueOrThrow<Duration>(this.DurationToBuild, "DurationToBuild"), this.m_nextTier, this.createGfx(), this.m_productType));
      }
    }
  }
}
