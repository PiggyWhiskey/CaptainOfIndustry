// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.ProtoRegistrator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Beacons;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.RainwaterHarvesters;
using Mafi.Core.Buildings.ResearchLab;
using Mafi.Core.Buildings.RuinedBuildings;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Datacenters;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.WellPumps;
using Mafi.Core.Fleet;
using Mafi.Core.Game;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Mods
{
  /// <summary>
  /// Helper designed for mods to register their prototypes.Prototypes registration should be organized into data
  /// classes implementing the interface <see cref="T:Mafi.Core.Mods.IModData" />. Each data class should be provided with this
  /// class from which it can get all necessary dependencies.
  /// </summary>
  public class ProtoRegistrator
  {
    public readonly ProtosDb PrototypesDb;
    public readonly EntityLayoutParser LayoutParser;
    public readonly BeaconProtoBuilder BeaconProtoBuilder;
    public readonly SettlementHousingProtoBuilder HouseProtoBuilder;
    public readonly DataCenterProtoBuilder DataCenterProtoBuilder;
    public readonly FluidProductProtoBuilder FluidProductProtoBuilder;
    public readonly MachineProtoBuilder MachineProtoBuilder;
    public readonly MineTowerProtoBuilder MineTowerProtoBuilder;
    public readonly MoltenProductProtoBuilder MoltenProductProtoBuilder;
    public readonly NotificationProtoBuilder NotificationProtoBuilder;
    public readonly RecipeProtoBuilder RecipeProtoBuilder;
    public readonly ResearchLabProtoBuilder ResearchLabProtoBuilder;
    public readonly ResearchNodeProtoBuilder ResearchNodeProtoBuilder;
    public readonly FleetEntityHullProtoBuilder FleetEntityHullProtoBuilder;
    public readonly SettlementModuleProtoBuilder SettlementModuleProtoBuilder;
    public readonly RuinsProtoBuilder RuinsProtoBuilder;
    public readonly StorageProtoBuilder StorageProtoBuilder;
    public readonly CargoDepotModuleProtoBuilder CargoDepotModuleProtoBuilder;
    public readonly CargoDepotProtoBuilder CargoDepotProtoBuilder;
    public readonly RainwaterHarvesterProtoBuilder RainwaterHarvesterProtoBuilder;
    public readonly VehicleDepotProtoBuilder VehicleDepotProtoBuilder;
    public readonly FuelTankProtoBuilder FuelTankProtoBuilder;
    public readonly FuelStationProtoBuilder FuelStationProtoBuilder;
    public readonly CargoShipProtoBuilder CargoShipProtoBuilder;
    public readonly WellPumpProtoBuilder WellPumpProtoBuilder;
    public readonly TruckProtoBuilder TruckProtoBuilder;
    public readonly ExcavatorProtoBuilder ExcavatorProtoBuilder;
    private readonly Set<Type> m_registeredDataClasses;

    /// <summary>
    /// List of types of modules that were registered with <see cref="M:Mafi.Core.Mods.ProtoRegistrator.RegisterData``1(System.Action{``0})" />.
    /// </summary>
    public IReadOnlySet<Type> RegisteredDataClasses
    {
      get => (IReadOnlySet<Type>) this.m_registeredDataClasses;
    }

    public bool DisableAllProtoCosts => false;

    public bool DisableVehicleFuelConsumption => false;

    internal ProtoRegistrator(ProtosDb protosDb, ImmutableArray<IConfig> configs)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_registeredDataClasses = new Set<Type>(64);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.PrototypesDb = protosDb.CheckNotNull<ProtosDb>();
      this.LayoutParser = new EntityLayoutParser(this.PrototypesDb);
      this.BeaconProtoBuilder = new BeaconProtoBuilder(this);
      this.HouseProtoBuilder = new SettlementHousingProtoBuilder(this);
      this.DataCenterProtoBuilder = new DataCenterProtoBuilder(this);
      this.FluidProductProtoBuilder = new FluidProductProtoBuilder(this);
      this.MachineProtoBuilder = new MachineProtoBuilder(this);
      this.MineTowerProtoBuilder = new MineTowerProtoBuilder(this);
      this.MoltenProductProtoBuilder = new MoltenProductProtoBuilder(this);
      this.NotificationProtoBuilder = new NotificationProtoBuilder(this);
      this.RecipeProtoBuilder = new RecipeProtoBuilder(this);
      this.ResearchLabProtoBuilder = new ResearchLabProtoBuilder(this);
      this.ResearchNodeProtoBuilder = new ResearchNodeProtoBuilder(this);
      this.FleetEntityHullProtoBuilder = new FleetEntityHullProtoBuilder(this);
      this.SettlementModuleProtoBuilder = new SettlementModuleProtoBuilder(this);
      this.RuinsProtoBuilder = new RuinsProtoBuilder(this);
      this.StorageProtoBuilder = new StorageProtoBuilder(this);
      this.CargoDepotModuleProtoBuilder = new CargoDepotModuleProtoBuilder(this);
      this.CargoDepotProtoBuilder = new CargoDepotProtoBuilder(this);
      this.RainwaterHarvesterProtoBuilder = new RainwaterHarvesterProtoBuilder(this);
      this.VehicleDepotProtoBuilder = new VehicleDepotProtoBuilder(this);
      this.FuelTankProtoBuilder = new FuelTankProtoBuilder(this.PrototypesDb);
      this.FuelStationProtoBuilder = new FuelStationProtoBuilder(this);
      this.CargoShipProtoBuilder = new CargoShipProtoBuilder(this);
      this.WellPumpProtoBuilder = new WellPumpProtoBuilder(this);
      this.TruckProtoBuilder = new TruckProtoBuilder(this, this.FuelTankProtoBuilder);
      this.ExcavatorProtoBuilder = new ExcavatorProtoBuilder(this, this.FuelTankProtoBuilder);
    }

    public void RegisterDataWithInterface<TData>() where TData : class, IModData
    {
      foreach (TData dataClass in ((IEnumerable<Type>) Assembly.GetCallingAssembly().GetTypes()).Where<Type>((Func<Type, bool>) (x => ((IEnumerable<Type>) x.GetInterfaces()).Contains<Type>(typeof (TData)))).Select<Type, TData>((Func<Type, TData>) (x => (TData) Activator.CreateInstance(x))))
        this.RegisterData((IModData) dataClass);
    }

    /// <summary>
    /// Creates instance of given type <typeparamref name="TData" /> and calls <see cref="M:Mafi.Core.Mods.IModData.RegisterData(Mafi.Core.Mods.ProtoRegistrator)" /> on it. Use this when registering your prototypes organized into data classes.
    /// </summary>
    /// <exception cref="T:Mafi.Core.Mods.DataRegistrationException">When module registration throws.</exception>
    public void RegisterData<TData>(Action<TData> config = null) where TData : IModData, new()
    {
      TData dataClass = new TData();
      if (config != null)
        config(dataClass);
      this.RegisterData((IModData) dataClass);
    }

    /// <summary>
    /// Calls <see cref="M:Mafi.Core.Mods.IModData.RegisterData(Mafi.Core.Mods.ProtoRegistrator)" /> on the given instance. Use this when registering your
    /// prototypes organized into data classes.
    /// </summary>
    /// <exception cref="T:Mafi.Core.Mods.DataRegistrationException">When module registration throws.</exception>
    public void RegisterData(IModData dataClass)
    {
      Type type = dataClass.GetType();
      if (!this.m_registeredDataClasses.Add(type))
      {
        Log.Error("Data class '" + type.Name + "' was already registered, skipping.");
      }
      else
      {
        try
        {
          dataClass.RegisterData(this);
        }
        catch (Exception ex)
        {
          throw new DataRegistrationException(ex.GetType().Name + " thrown on registration of data '" + type.Name + "'.", ex);
        }
      }
    }

    /// <summary>
    /// Automatically registers all products from calling assembly.
    /// </summary>
    public void RegisterAllProducts(Assembly assembly = null)
    {
      if (assembly == (Assembly) null)
        assembly = Assembly.GetCallingAssembly();
      Lyst<FieldInfo> lyst1 = ((IEnumerable<Type>) assembly.GetExportedTypes()).Where<Type>((Func<Type, bool>) (x => x.IsPublic && x.IsAbstract && x.IsSealed)).SelectMany<Type, Type>((Func<Type, IEnumerable<Type>>) (t => Enumerable.Repeat<Type>(t, 1).Concat<Type>(selectAllNestedStaticTypes(t)))).SelectMany<Type, FieldInfo>((Func<Type, IEnumerable<FieldInfo>>) (t => (IEnumerable<FieldInfo>) t.GetFields(BindingFlags.Static | BindingFlags.Public))).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.CustomAttributes.Any<CustomAttributeData>())).ToLyst<FieldInfo>();
      Dict<string, Lyst<IProtoParam>> dict = new Dict<string, Lyst<IProtoParam>>();
      foreach (FieldInfo element in lyst1)
      {
        ProtoParamForAttribute customAttribute = element.GetCustomAttribute<ProtoParamForAttribute>();
        if (customAttribute != null)
        {
          if (element.FieldType.IsAssignableTo<IProtoParam>())
          {
            Lyst<IProtoParam> lyst2;
            if (!dict.TryGetValue(customAttribute.IdFieldName, out lyst2))
            {
              lyst2 = new Lyst<IProtoParam>();
              dict.Add(customAttribute.IdFieldName, lyst2);
            }
            lyst2.Add((IProtoParam) element.GetValue((object) null));
          }
          else
            Log.Error("Proto param field '" + element.Name + "' is not assignable to 'IProtoParam', skipping.");
        }
      }
      int num = 0;
      foreach (FieldInfo fieldInfo in lyst1)
      {
        ProductAttribute customAttribute1 = fieldInfo.GetCustomAttribute<ProductAttribute>();
        if (customAttribute1 != null)
        {
          Proto proto = customAttribute1.Register(this, fieldInfo);
          ++num;
          Lyst<IProtoParam> lyst3;
          if (dict.TryGetValue(fieldInfo.Name, out lyst3))
          {
            foreach (IProtoParam protoParam in lyst3)
              proto.AddParam(protoParam);
          }
        }
        ResearchCostsAttribute customAttribute2 = fieldInfo.GetCustomAttribute<ResearchCostsAttribute>();
        if (customAttribute2 != null)
        {
          if (fieldInfo.FieldType.IsAssignableTo<ResearchNodeProto.ID>())
            this.ResearchNodeProtoBuilder.SetResearchCost((ResearchNodeProto.ID) fieldInfo.GetValue((object) null), customAttribute2.GetResearchCostsTpl());
          else
            Log.Error("Research cost field '" + fieldInfo.Name + "' is not assignable to 'ResearchNodeProto.ID', skipping.");
        }
      }
      Log.Info(string.Format("Registered {0} products from assembly '{1}'.", (object) num, (object) assembly.GetName().Name));

      static IEnumerable<Type> selectAllNestedStaticTypes(Type t)
      {
        Type[] nestedTypes = t.GetNestedTypes(BindingFlags.Static | BindingFlags.Public);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return ((IEnumerable<Type>) nestedTypes).Concat<Type>(((IEnumerable<Type>) nestedTypes).SelectMany<Type, Type>(ProtoRegistrator.\u003C\u003EO.\u003C0\u003E__selectAllNestedStaticTypes ?? (ProtoRegistrator.\u003C\u003EO.\u003C0\u003E__selectAllNestedStaticTypes = new Func<Type, IEnumerable<Type>>(selectAllNestedStaticTypes))));
      }
    }

    public ImmutableArray<ToolbarCategoryProto> GetCategoriesProtos(params Proto.ID[] ids)
    {
      return ((IReadOnlyCollection<Proto.ID>) ids).ToImmutableArray<Proto.ID, ToolbarCategoryProto>(new Func<Proto.ID, ToolbarCategoryProto>(this.PrototypesDb.GetOrThrow<ToolbarCategoryProto>));
    }
  }
}
