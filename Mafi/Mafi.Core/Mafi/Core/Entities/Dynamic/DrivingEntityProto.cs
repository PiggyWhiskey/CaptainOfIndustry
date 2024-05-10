// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DrivingEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Core.Vehicles.Trucks;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  public abstract class DrivingEntityProto : DynamicGroundEntityProto
  {
    public readonly DrivingData DrivingData;
    public readonly Option<Mafi.Core.Entities.Dynamic.FuelTankProto> FuelTankProto;
    public readonly VehiclePathFindingParams PathFindingParams;
    public readonly Option<DrivingEntityProto> NextTier;
    public readonly float UIOrder;

    public AssetValue CostToBuild { get; private set; }

    public DrivingEntityProto(
      DynamicEntityProto.ID id,
      Proto.Str strings,
      RelTile3f entitySize,
      EntityCosts costs,
      int vehicleQuotaCost,
      DrivingData drivingData,
      Option<Mafi.Core.Entities.Dynamic.FuelTankProto> fuelTankProto,
      VehiclePathFindingParams pathFindingParams,
      ImmutableArray<ThicknessTilesF> disruptionByDistance,
      Duration durationToBuild,
      Option<DrivingEntityProto> nextTier,
      DynamicGroundEntityProto.Gfx graphics,
      float uiOrder = 0.0f)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, entitySize, costs, vehicleQuotaCost, disruptionByDistance, durationToBuild, graphics);
      Assert.That<RelTile1f>(drivingData.AxlesDistance).IsLess<DynamicEntityProto.ID>(this.EntitySize.X, "Entity axle distance is greater or equal to entity size for entity '{0}'.", id);
      this.DrivingData = drivingData.CheckNotNull<DrivingData>();
      this.FuelTankProto = fuelTankProto;
      this.NextTier = nextTier;
      this.PathFindingParams = pathFindingParams.CheckNotNull<VehiclePathFindingParams>();
      this.CostToBuild = this.Costs.Price;
      if (this.FuelTankProto.HasValue)
        this.CostToBuild += new AssetValue(this.FuelTankProto.Value.Product, this.FuelTankProto.Value.Capacity);
      if ((double) uiOrder <= 0.0)
      {
        float num = (float) this.Costs.Price.GetQuantitySum().Value * 0.0001f;
        Assert.That<float>(num).IsLess(1f);
        ProductProto.ID? id1 = this.FuelTankProto.ValueOrNull?.Product.Id;
        ProductProto.ID diesel = IdsCore.Products.Diesel;
        if ((id1.HasValue ? (id1.GetValueOrDefault() != diesel ? 1 : 0) : 1) != 0)
          num += 10f;
        switch (this)
        {
          case TruckProto _:
            uiOrder = num + 0.0f;
            break;
          case ExcavatorProto _:
            uiOrder = num + 1f;
            break;
          case TreeHarvesterProto _:
            uiOrder = num + 2f;
            break;
          case TreePlanterProto _:
            uiOrder = num + 3f;
            break;
          case RocketTransporterProto _:
            uiOrder = num + 4f;
            break;
          default:
            Log.WarningOnce(string.Format("Unknown proto for scoring '{0}'", (object) this));
            uiOrder = num + 5f;
            break;
        }
      }
      this.UIOrder = uiOrder;
    }

    public override void OnPropertyUpdated(IProperty property)
    {
      base.OnPropertyUpdated(property);
      if (!property.TryGetValueAs<Percent>(IdsCore.PropertyIds.ConstructionCostsMultiplier, out Percent _))
        return;
      if (this.FuelTankProto.HasValue)
        this.CostToBuild = this.Costs.Price + new AssetValue(this.FuelTankProto.Value.Product, this.FuelTankProto.Value.Capacity);
      else
        this.CostToBuild = this.Costs.Price;
    }

    public PartialProductQuantity GetFuelConsumedPer60()
    {
      if (this.FuelTankProto.IsNone)
        return PartialProductQuantity.None;
      Mafi.Core.Entities.Dynamic.FuelTankProto fuelTankProto = this.FuelTankProto.Value;
      Fix32 fix32 = 60.Seconds().Ticks * fuelTankProto.Capacity.Value / fuelTankProto.Duration.Ticks.ToFix32();
      return new PartialProductQuantity(fuelTankProto.Product, new PartialQuantity(fix32));
    }
  }
}
