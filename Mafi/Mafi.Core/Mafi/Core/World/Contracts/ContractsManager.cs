// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Contracts.ContractsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.World.Contracts
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class ContractsManager : ICommandProcessor<ToggleContractCmd>, IAction<ToggleContractCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Lyst<ContractProto> m_activeContracts;
    private readonly Lyst<ContractProto> m_allUnlockedContracts;
    private readonly Dict<ContractProto, WorldMapVillage> m_contractToVillage;
    private readonly Set<ContractProto> m_activePaidContracts;
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly IUpointsManager m_upointsManager;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_profitMultiplier;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ProductType, Quantity> m_modulesCapacities;
    [DoNotSave(0, null)]
    private bool m_isAnyContractUnlocked;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<ContractProto, Lyst<ContractsManager.ContractEstimateData>> m_contractsEstimatesCache;

    public static void Serialize(ContractsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ContractsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ContractsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<ContractProto>.Serialize(this.m_activeContracts, writer);
      Set<ContractProto>.Serialize(this.m_activePaidContracts, writer);
      Lyst<ContractProto>.Serialize(this.m_allUnlockedContracts, writer);
      Dict<ContractProto, WorldMapVillage>.Serialize(this.m_contractToVillage, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      writer.WriteGeneric<IProperty<Percent>>(this.m_profitMultiplier);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      writer.WriteGeneric<IUpointsManager>(this.m_upointsManager);
    }

    public static ContractsManager Deserialize(BlobReader reader)
    {
      ContractsManager contractsManager;
      if (reader.TryStartClassDeserialization<ContractsManager>(out contractsManager))
        reader.EnqueueDataDeserialization((object) contractsManager, ContractsManager.s_deserializeDataDelayedAction);
      return contractsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ContractsManager>(this, "m_activeContracts", (object) Lyst<ContractProto>.Deserialize(reader));
      reader.SetField<ContractsManager>(this, "m_activePaidContracts", (object) Set<ContractProto>.Deserialize(reader));
      reader.SetField<ContractsManager>(this, "m_allUnlockedContracts", (object) Lyst<ContractProto>.Deserialize(reader));
      reader.SetField<ContractsManager>(this, "m_contractsEstimatesCache", (object) new Dict<ContractProto, Lyst<ContractsManager.ContractEstimateData>>());
      reader.SetField<ContractsManager>(this, "m_contractToVillage", (object) Dict<ContractProto, WorldMapVillage>.Deserialize(reader));
      reader.SetField<ContractsManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<ContractsManager>(this, "m_modulesCapacities", (object) new Dict<ProductType, Quantity>());
      reader.SetField<ContractsManager>(this, "m_profitMultiplier", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      reader.RegisterResolvedMember<ContractsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<ContractsManager>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.SetField<ContractsManager>(this, "m_upointsManager", (object) reader.ReadGenericAs<IUpointsManager>());
      reader.RegisterInitAfterLoad<ContractsManager>(this, "initSelf", InitPriority.Normal);
      reader.RegisterInitAfterLoad<ContractsManager>(this, "initSelfForCompatibility", InitPriority.High);
    }

    public IIndexable<ContractProto> ActiveContracts
    {
      get => (IIndexable<ContractProto>) this.m_activeContracts;
    }

    public Percent ProfitMultiplier => this.m_profitMultiplier.Value;

    public ContractsManager(
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      IEntitiesManager entitiesManager,
      IPropertiesDb propsDb,
      IUpointsManager upointsManager,
      ICalendar calendar,
      IWorldMapManager worldMapManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_activeContracts = new Lyst<ContractProto>();
      this.m_allUnlockedContracts = new Lyst<ContractProto>();
      this.m_contractToVillage = new Dict<ContractProto, WorldMapVillage>();
      this.m_activePaidContracts = new Set<ContractProto>();
      this.m_modulesCapacities = new Dict<ProductType, Quantity>();
      this.m_contractsEstimatesCache = new Dict<ContractProto, Lyst<ContractsManager.ContractEstimateData>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_entitiesManager = entitiesManager;
      this.m_upointsManager = upointsManager;
      this.m_profitMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.ContractsProfitMultiplier);
      this.m_profitMultiplier.OnChange.AddNonSaveable<ContractsManager>(this, new Action<Percent>(this.multChanged));
      this.initShipCapacities();
      worldMapManager.OnWorldEntityCreated.Add<ContractsManager>(this, new Action<IWorldMapEntity>(this.onWorldMapEntityCreated));
      calendar.NewMonth.Add<ContractsManager>(this, new Action(this.onNewMonth));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.initShipCapacities();
      foreach (WorldMapVillage worldMapVillage in this.m_contractToVillage.Values.Distinct<WorldMapVillage, WorldMapVillage>((Func<WorldMapVillage, WorldMapVillage>) (x => x)).ToArray<WorldMapVillage>())
      {
        foreach (ContractProto contract in worldMapVillage.Prototype.Contracts)
        {
          if (this.m_contractToVillage.TryAdd(contract, worldMapVillage))
            this.m_allUnlockedContracts.Add(contract);
        }
      }
      this.m_profitMultiplier.OnChange.AddNonSaveable<ContractsManager>(this, new Action<Percent>(this.multChanged));
    }

    [InitAfterLoad(InitPriority.High)]
    [OnlyForSaveCompatibility(null)]
    private void initSelfForCompatibility(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<ContractsManager>(this, "m_profitMultiplier", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.ContractsProfitMultiplier));
    }

    private void multChanged(Percent newVal) => this.m_contractsEstimatesCache?.Clear();

    private void initShipCapacities()
    {
      Option<CargoShipProto> option = this.m_protosDb.First<CargoShipProto>();
      if (!option.HasValue)
        return;
      foreach (CargoShipModuleProto availableModule in option.Value.AvailableModules)
        this.m_modulesCapacities.Add(availableModule.ProductType, availableModule.Capacity);
    }

    private void onNewMonth()
    {
      this.m_activePaidContracts.Clear();
      foreach (ContractProto activeContract in this.m_activeContracts)
      {
        if (this.m_upointsManager.CanConsume(activeContract.UpointsPerMonth))
        {
          IUpointsManager upointsManager = this.m_upointsManager;
          Proto.ID contract = IdsCore.UpointsCategories.Contract;
          Upoints upointsPerMonth = activeContract.UpointsPerMonth;
          LocStr? nullable = new LocStr?(TrCore.Contract__MonthlyCost);
          Option<IEntity> consumer = new Option<IEntity>();
          LocStr? extraTitle = nullable;
          upointsManager.ConsumeExactly(contract, upointsPerMonth, consumer, extraTitle);
          this.m_activePaidContracts.Add(activeContract);
        }
      }
    }

    private void onWorldMapEntityCreated(IWorldMapEntity entity)
    {
      if (!(entity is WorldMapVillage worldMapVillage))
        return;
      foreach (ContractProto contract in worldMapVillage.Prototype.Contracts)
      {
        if (!this.m_contractToVillage.TryAdd(contract, worldMapVillage))
          Log.Error("Adding a village twice?");
        else
          this.m_allUnlockedContracts.Add(contract);
      }
    }

    public Quantity GetToBuy(ContractProto contract)
    {
      return contract.GetQuantityToBuy(this.m_profitMultiplier.Value);
    }

    public void GetUnlockedContractsButNotActive(Lyst<ContractProto> result)
    {
      result.Clear();
      foreach (ContractProto unlockedContract in this.m_allUnlockedContracts)
      {
        if (!this.m_activeContracts.Contains(unlockedContract))
          result.Add(unlockedContract);
      }
    }

    public bool IsAnyContractUnlocked()
    {
      if (this.m_isAnyContractUnlocked)
        return true;
      foreach (ContractProto unlockedContract in this.m_allUnlockedContracts)
      {
        if (this.CanEstablish(unlockedContract) == ContractsManager.EstablishCheckResult.Ok || this.CanEstablish(unlockedContract) == ContractsManager.EstablishCheckResult.AlreadyActive)
        {
          this.m_isAnyContractUnlocked = true;
          return true;
        }
      }
      return false;
    }

    public ContractsManager.EstablishCheckResult CanEstablish(ContractProto contract)
    {
      if (this.IsEstablished(contract))
        return ContractsManager.EstablishCheckResult.AlreadyActive;
      if (this.m_unlockedProtosDb.IsLocked((IProto) contract.ProductToPayWith) || this.m_unlockedProtosDb.IsLocked((IProto) contract.ProductToBuy))
        return ContractsManager.EstablishCheckResult.ProductLocked;
      WorldMapVillage worldMapVillage;
      if (!this.m_contractToVillage.TryGetValue(contract, out worldMapVillage) || worldMapVillage.Reputation < contract.MinReputationRequired)
        return ContractsManager.EstablishCheckResult.VillageLevelLow;
      return !this.m_upointsManager.CanConsume(contract.UpointsToEstablish) ? ContractsManager.EstablishCheckResult.LacksUpoints : ContractsManager.EstablishCheckResult.Ok;
    }

    public ContractsManager.CancelCheckResult CanCancel(ContractProto contract)
    {
      if (!this.IsEstablished(contract))
        return ContractsManager.CancelCheckResult.NotActive;
      foreach (CargoDepot cargoDepot in this.m_entitiesManager.GetAllEntitiesOfType<CargoDepot>())
      {
        if (cargoDepot.ContractAssigned == contract)
          return ContractsManager.CancelCheckResult.HasShipsAssigned;
      }
      return ContractsManager.CancelCheckResult.Ok;
    }

    public bool IsEstablished(ContractProto contract) => this.m_activeContracts.Contains(contract);

    public ContractsManager.ShipDepartureCheckResult CanShipDepartForContract(
      CargoShip ship,
      ContractProto contract,
      bool payUnityCost,
      bool wasDepartureRequested)
    {
      Quantity exportQuantity;
      Quantity exportCapacity;
      Quantity importedQuantity;
      Quantity usableImportCapacity;
      this.getShipCargoStats(ship, contract, out exportQuantity, out exportCapacity, out importedQuantity, out usableImportCapacity);
      if (!this.m_activePaidContracts.Contains(contract))
        return ContractsManager.ShipDepartureCheckResult.NotEnoughUpoints;
      if (exportQuantity.IsZero || usableImportCapacity.IsZero)
        return ContractsManager.ShipDepartureCheckResult.WaitingForCargo;
      Quantity toBuy1 = this.GetToBuy(contract);
      Quantity toBuy2 = (exportQuantity.Value * toBuy1 / contract.QuantityToPayWith.Value).Min(usableImportCapacity);
      return toBuy2.IsNotPositive || !wasDepartureRequested && (!importedQuantity.IsZero || !(exportCapacity == exportQuantity) && !(toBuy2 == usableImportCapacity)) ? ContractsManager.ShipDepartureCheckResult.WaitingForCargo : hasUpointsToBuy(toBuy2);

      ContractsManager.ShipDepartureCheckResult hasUpointsToBuy(Quantity toBuy)
      {
        Upoints forQuantityBought = contract.CalculateUpointsForQuantityBought(toBuy);
        if (!this.m_upointsManager.CanConsume(forQuantityBought))
          return ContractsManager.ShipDepartureCheckResult.NotEnoughUpoints;
        if (payUnityCost)
        {
          IUpointsManager upointsManager = this.m_upointsManager;
          Proto.ID contract = IdsCore.UpointsCategories.Contract;
          Upoints unity = forQuantityBought;
          LocStr? nullable = new LocStr?(TrCore.Contract__ExchangeCost);
          Option<IEntity> consumer = new Option<IEntity>();
          LocStr? extraTitle = nullable;
          upointsManager.ConsumeExactly(contract, unity, consumer, extraTitle);
        }
        return ContractsManager.ShipDepartureCheckResult.Ok;
      }
    }

    public void ExchangeContractProducts(CargoShip ship, ContractProto contract)
    {
      Quantity exportQuantity;
      Quantity usableImportCapacity;
      this.getShipCargoStats(ship, contract, out exportQuantity, out Quantity _, out Quantity _, out usableImportCapacity);
      if (exportQuantity.IsZero || usableImportCapacity.IsZero)
        return;
      ProductProto productToBuy = contract.ProductToBuy;
      ProductProto productToPayWith = contract.ProductToPayWith;
      Quantity toBuy = this.GetToBuy(contract);
      Quantity quantity1 = (exportQuantity.Value * toBuy / contract.QuantityToPayWith.Value).Min(usableImportCapacity);
      Quantity quantity2 = quantity1.Value * contract.QuantityToPayWith / toBuy.Value;
      Quantity quantity3 = quantity1;
      Quantity quantity4 = quantity2;
      foreach (Option<CargoShipModule> module in ship.Modules)
      {
        Option<ProductProto>? storedProduct = module.ValueOrNull?.StoredProduct;
        Option<ProductProto> option1 = (Option<ProductProto>) productToBuy;
        if ((storedProduct.HasValue ? (storedProduct.GetValueOrDefault() == option1 ? 1 : 0) : 0) != 0 && quantity3.IsPositive)
        {
          quantity3 = module.Value.StoreAsMuchAs(productToBuy.WithQuantity(quantity3));
        }
        else
        {
          storedProduct = module.ValueOrNull?.StoredProduct;
          Option<ProductProto> option2 = (Option<ProductProto>) productToPayWith;
          if ((storedProduct.HasValue ? (storedProduct.GetValueOrDefault() == option2 ? 1 : 0) : 0) != 0 && quantity4.IsPositive)
          {
            Quantity quantity5 = quantity4.Min(module.Value.Quantity);
            ((ICargoShipModuleFriend) module.Value).RemoveExactly(quantity5);
            quantity4 -= quantity5;
          }
        }
      }
    }

    public void GetTradeEstimateForShipSize(
      ContractProto contract,
      int modulesCount,
      out Quantity maxToImport,
      out Quantity maxToExport)
    {
      Lyst<ContractsManager.ContractEstimateData> indexable;
      if (this.m_contractsEstimatesCache.TryGetValue(contract, out indexable))
      {
        ContractsManager.ContractEstimateData contractEstimateData = indexable.FirstOrDefault<ContractsManager.ContractEstimateData>((Predicate<ContractsManager.ContractEstimateData>) (x => x.ModulesCount == modulesCount));
        if (contractEstimateData.ModulesCount == modulesCount)
        {
          maxToImport = contractEstimateData.MaxImportQuantity;
          maxToExport = contractEstimateData.MaxExportQuantity;
          return;
        }
      }
      int num1 = modulesCount;
      Quantity quantity1;
      if (!this.m_modulesCapacities.TryGetValue(contract.ProductToBuy.Type, out quantity1))
        quantity1 = 360.Quantity();
      Quantity quantity2;
      if (!this.m_modulesCapacities.TryGetValue(contract.ProductToPayWith.Type, out quantity2))
        quantity2 = 360.Quantity();
      Quantity zero = Quantity.Zero;
      int num2 = 0;
label_13:
      while (num1 > 0)
      {
        --num1;
        Quantity toBuy = this.GetToBuy(contract);
        Quantity rhs = quantity2.Value * toBuy / contract.QuantityToPayWith.Value;
        while (true)
        {
          if (rhs.IsPositive && (zero.IsPositive || num1 > 0))
          {
            if (zero.IsPositive)
            {
              Quantity quantity3 = rhs.Min(zero);
              zero -= quantity3;
              rhs -= quantity3;
            }
            else
            {
              ++num2;
              --num1;
              Quantity quantity4 = quantity1.Min(rhs);
              rhs -= quantity4;
              zero += quantity1 - quantity4;
            }
          }
          else
            goto label_13;
        }
      }
      Quantity totalImportCapacity = num2 * quantity1;
      Quantity totalExportCapacity = (modulesCount - num2) * quantity2;
      Quantity maxExportQuantity1;
      Quantity maxImportQuantity1;
      this.GetContractStatsForCapacities(contract, totalImportCapacity, totalExportCapacity, out maxExportQuantity1, out maxImportQuantity1);
      maxToExport = maxExportQuantity1;
      maxToImport = maxImportQuantity1;
      if (indexable == null)
      {
        indexable = new Lyst<ContractsManager.ContractEstimateData>();
        this.m_contractsEstimatesCache.Add(contract, indexable);
      }
      Lyst<ContractsManager.ContractEstimateData> lyst = indexable;
      int modulesCount1 = modulesCount;
      Quantity quantity5 = maxExportQuantity1;
      Quantity maxImportQuantity2 = maxImportQuantity1;
      Quantity maxExportQuantity2 = quantity5;
      ContractsManager.ContractEstimateData contractEstimateData1 = new ContractsManager.ContractEstimateData(modulesCount1, maxImportQuantity2, maxExportQuantity2);
      lyst.Add(contractEstimateData1);
    }

    public void GetContractStatsForDepot(
      CargoDepot depot,
      ContractProto contract,
      out Quantity maxExportQuantity,
      out Quantity maxImportQuantity)
    {
      CargoShipProto cargoShipProto = depot.Prototype.CargoShipProto;
      Quantity zero1 = Quantity.Zero;
      Quantity zero2 = Quantity.Zero;
      foreach (Option<CargoDepotModule> module1 in depot.Modules)
      {
        if (!module1.IsNone)
        {
          CargoDepotModule module = module1.Value;
          if (module.StoredProduct == contract.ProductToBuy)
          {
            CargoShipModuleProto cargoShipModuleProto = cargoShipProto.AvailableModules.FirstOrDefault((Func<CargoShipModuleProto, bool>) (x => x.ProductType == module.Prototype.ProductType));
            zero2 += cargoShipModuleProto.Capacity;
          }
          else if (module.StoredProduct == contract.ProductToPayWith)
          {
            CargoShipModuleProto cargoShipModuleProto = cargoShipProto.AvailableModules.FirstOrDefault((Func<CargoShipModuleProto, bool>) (x => x.ProductType == module.Prototype.ProductType));
            zero1 += cargoShipModuleProto.Capacity;
          }
        }
      }
      Quantity maxExportQuantity1;
      Quantity maxImportQuantity1;
      this.GetContractStatsForCapacities(contract, zero2, zero1, out maxExportQuantity1, out maxImportQuantity1);
      maxExportQuantity = maxExportQuantity1;
      maxImportQuantity = maxImportQuantity1;
    }

    public void GetContractStatsForCapacities(
      ContractProto contract,
      Quantity totalImportCapacity,
      Quantity totalExportCapacity,
      out Quantity maxExportQuantity,
      out Quantity maxImportQuantity)
    {
      Quantity toBuy = this.GetToBuy(contract);
      Quantity quantity = totalExportCapacity.Value * toBuy / contract.QuantityToPayWith.Value;
      maxImportQuantity = quantity.Min(totalImportCapacity);
      maxExportQuantity = maxImportQuantity.Value * contract.QuantityToPayWith / toBuy.Value;
    }

    private void getShipCargoStats(
      CargoShip ship,
      ContractProto contract,
      out Quantity exportQuantity,
      out Quantity exportCapacity,
      out Quantity importedQuantity,
      out Quantity usableImportCapacity)
    {
      exportQuantity = Quantity.Zero;
      exportCapacity = Quantity.Zero;
      importedQuantity = Quantity.Zero;
      usableImportCapacity = Quantity.Zero;
      foreach (Option<CargoShipModule> module in ship.Modules)
      {
        Option<ProductProto>? storedProduct = module.ValueOrNull?.StoredProduct;
        Option<ProductProto> productToBuy = (Option<ProductProto>) contract.ProductToBuy;
        if ((storedProduct.HasValue ? (storedProduct.GetValueOrDefault() == productToBuy ? 1 : 0) : 0) != 0)
        {
          importedQuantity += module.Value.Quantity;
          usableImportCapacity += module.Value.UsableCapacity;
        }
        else
        {
          storedProduct = module.ValueOrNull?.StoredProduct;
          Option<ProductProto> productToPayWith = (Option<ProductProto>) contract.ProductToPayWith;
          if ((storedProduct.HasValue ? (storedProduct.GetValueOrDefault() == productToPayWith ? 1 : 0) : 0) != 0)
          {
            exportQuantity += module.Value.Quantity;
            exportCapacity += module.Value.Capacity;
          }
        }
      }
    }

    public void Invoke(ToggleContractCmd cmd)
    {
      ContractProto proto;
      if (!this.m_protosDb.TryGetProto<ContractProto>(cmd.ContractId, out proto))
        cmd.SetResultError(string.Format("Contract with id {0} not found!", (object) cmd.ContractId));
      else if (this.IsEstablished(proto))
      {
        if (this.CanCancel(proto) != ContractsManager.CancelCheckResult.Ok)
        {
          cmd.SetResultError(string.Format("Contract with id {0} cannot be canceled!", (object) cmd.ContractId));
        }
        else
        {
          this.m_activeContracts.RemoveAndAssert(proto);
          cmd.SetResultSuccess();
        }
      }
      else if (this.CanEstablish(proto) != ContractsManager.EstablishCheckResult.Ok)
      {
        cmd.SetResultError(string.Format("Contract with id {0} cannot be activated!", (object) cmd.ContractId));
      }
      else
      {
        this.m_upointsManager.ConsumeAsMuchAs(IdsCore.UpointsCategories.ContractEstablish, proto.UpointsToEstablish);
        this.m_activeContracts.AddAssertNew(proto);
        this.m_activePaidContracts.Add(proto);
        cmd.SetResultSuccess();
      }
    }

    static ContractsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ContractsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ContractsManager) obj).SerializeData(writer));
      ContractsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ContractsManager) obj).DeserializeData(reader));
    }

    public enum ShipDepartureCheckResult
    {
      Ok,
      NotEnoughUpoints,
      WaitingForCargo,
    }

    public enum EstablishCheckResult
    {
      Ok,
      AlreadyActive,
      VillageLevelLow,
      ProductLocked,
      LacksUpoints,
    }

    public enum CancelCheckResult
    {
      Ok,
      NotActive,
      HasShipsAssigned,
    }

    private struct ContractEstimateData
    {
      public readonly int ModulesCount;
      public readonly Quantity MaxImportQuantity;
      public readonly Quantity MaxExportQuantity;

      public ContractEstimateData(
        int modulesCount,
        Quantity maxImportQuantity,
        Quantity maxExportQuantity)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ModulesCount = modulesCount;
        this.MaxImportQuantity = maxImportQuantity;
        this.MaxExportQuantity = maxExportQuantity;
      }
    }
  }
}
