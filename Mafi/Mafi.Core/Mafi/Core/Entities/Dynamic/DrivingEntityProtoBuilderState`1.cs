// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DrivingEntityProtoBuilderState`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.PathFinding;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public class DrivingEntityProtoBuilderState<TState> : ProtoBuilderState<TState> where TState : ProtoBuilderState<TState>
  {
    protected readonly Lyst<DynamicEntityDustParticlesSpec> DustParticlesSpecs;
    protected Option<VehicleExhaustParticlesSpec> ExhaustParticlesSpec;
    protected EntityCosts Costs;
    private readonly FuelTankProtoBuilder m_tankBuilder;

    protected Option<string> PrefabPath { get; private set; }

    protected Option<string> CustomIconPath { get; private set; }

    protected RelTile2f? FrontContactPtsOffset { get; private set; }

    protected RelTile2f? RearContactPtsOffset { get; private set; }

    protected RelTile3f? Size { get; private set; }

    protected Option<Mafi.Core.Entities.Dynamic.DrivingData> DrivingData { get; private set; }

    protected ImmutableArray<ThicknessTilesF> DisruptionByDistance { get; private set; }

    protected Option<FuelTankProto> FuelTank { get; private set; }

    protected Option<VehiclePathFindingParams> PathFindingParams { get; private set; }

    protected Duration? DurationToBuild { get; private set; }

    public DrivingEntityProtoBuilderState(
      IProtoBuilder builder,
      Proto.ID id,
      string name,
      FuelTankProtoBuilder tankBuilder,
      string translationComment = "vehicle")
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.DustParticlesSpecs = new Lyst<DynamicEntityDustParticlesSpec>();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder, id, name, translationComment);
      this.m_tankBuilder = tankBuilder;
    }

    [MustUseReturnValue]
    public TState SetPrefabPath(string prefabPath)
    {
      this.PrefabPath = (Option<string>) prefabPath.CheckNotNullOrEmpty();
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetCustomIconPath(string customIconPath)
    {
      this.CustomIconPath = (Option<string>) customIconPath.CheckNotNullOrEmpty();
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetTerrainContactPointsOffsets(RelTile2f front, RelTile2f rear)
    {
      this.FrontContactPtsOffset = new RelTile2f?(front);
      this.RearContactPtsOffset = new RelTile2f?(rear);
      return (this as TState).CheckNotNull<TState>();
    }

    /// <summary>
    /// Bounding box size that is used to compute distances and threshold for navigation.
    /// </summary>
    [MustUseReturnValue]
    public TState SetSizeInMeters(double l, double w, double h)
    {
      this.Size = new RelTile3f?(RelTile3f.FromDimensionsInMeters(l, w, h));
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetDrivingData(Mafi.Core.Entities.Dynamic.DrivingData drivingData)
    {
      this.DrivingData = (Option<Mafi.Core.Entities.Dynamic.DrivingData>) drivingData;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetCosts(EntityCostsTpl costs)
    {
      this.Costs = costs.MapToEntityCosts(this.Builder.Registrator);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetDisruptionByDistance(params byte[] disruption)
    {
      this.DisruptionByDistance = ((IReadOnlyCollection<byte>) disruption).ToImmutableArray<byte, ThicknessTilesF>((Func<byte, ThicknessTilesF>) (x => new ThicknessTilesF((int) x / 256.ToFix32())));
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetDurationToBuild(Duration durationToBuild)
    {
      this.DurationToBuild = new Duration?(this.Builder.Registrator.DisableAllProtoCosts ? Duration.OneTick : durationToBuild);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetFuelTank(
      Func<FuelTankProtoBuilder, FuelTankProto> tankFactory)
    {
      if (!this.Builder.Registrator.DisableVehicleFuelConsumption)
        this.FuelTank = (Option<FuelTankProto>) tankFactory(this.m_tankBuilder);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState SetPathFindingParams(VehiclePathFindingParams pfParams)
    {
      this.PathFindingParams = (Option<VehiclePathFindingParams>) pfParams;
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState AddDustSource(
      DynamicEntityDustParticlesSpec dynamicEntityDustParticlesSpec)
    {
      this.DustParticlesSpecs.Add(dynamicEntityDustParticlesSpec);
      return (this as TState).CheckNotNull<TState>();
    }

    [MustUseReturnValue]
    public TState AddExhaustSources(Option<VehicleExhaustParticlesSpec> particlesSpec)
    {
      this.ExhaustParticlesSpec = particlesSpec;
      return (this as TState).CheckNotNull<TState>();
    }
  }
}
