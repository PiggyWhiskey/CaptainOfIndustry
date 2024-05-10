// Decompiled with JetBrains decompiler
// Type: Mafi.Base.BaseMod
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Prototypes;
using Mafi.Base.Prototypes.Buildings;
using Mafi.Base.Prototypes.Cargo;
using Mafi.Base.Prototypes.Fleet;
using Mafi.Base.Prototypes.Machines;
using Mafi.Base.Prototypes.Messages;
using Mafi.Base.Prototypes.Research;
using Mafi.Base.Prototypes.Rockets;
using Mafi.Base.Prototypes.Settlements;
using Mafi.Base.Prototypes.Storages;
using Mafi.Base.Prototypes.Transport;
using Mafi.Base.Prototypes.Vehicles;
using Mafi.Base.Prototypes.Weather;
using Mafi.Base.Prototypes.World;
using Mafi.Base.Terrain;
using Mafi.Base.Terrain.Maps;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Map;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base
{
  public sealed class BaseMod : IMod
  {
    public string Name => "Mafi-Base";

    public int Version => 0;

    public bool IsUiOnly => false;

    public Option<IConfig> ModConfig => this.Config.SomeOption<IConfig>();

    public BaseModConfig Config { get; }

    /// <summary>
    /// Base mod depends on <see cref="T:Mafi.Core.CoreMod" />.
    /// </summary>
    public BaseMod(CoreMod coreMod, BaseModConfig config)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Config = config;
    }

    void IMod.RegisterPrototypes(ProtoRegistrator registrator)
    {
      registrator.RegisterAllProducts();
      registrator.RegisterData<PropertiesData>();
      registrator.RegisterData<ToolbarCategoriesData>();
      registrator.RegisterData<WeatherData>();
      registrator.RegisterData<TerrainPropsData>();
      registrator.RegisterData<TerrainTileSurfacesData>();
      registrator.RegisterData<TerrainTileSurfaceDecalsData>();
      registrator.RegisterData<TerrainDetailsData>();
      registrator.RegisterData<TerrainMaterialsData>();
      registrator.RegisterData<TreesData>();
      registrator.RegisterData<CellSurfacesData>();
      registrator.RegisterData<PortShapesData>();
      registrator.RegisterData<TransportsData>();
      AllMachinesRegistrator.RegisterModules(registrator);
      registrator.RegisterData<StoragesData>();
      registrator.RegisterData<ThermalStoragesData>();
      registrator.RegisterData<ResearchLabsData>();
      registrator.RegisterData<BeaconsData>();
      registrator.RegisterData<CargoDepotsData>();
      registrator.RegisterData<VehicleDepotsData>();
      registrator.RegisterData<MineBuildingsData>();
      registrator.RegisterData<ForestryBuildingsData>();
      registrator.RegisterData<FuelStationsData>();
      registrator.RegisterData<CaptainOfficesData>();
      registrator.RegisterData<VehicleRampsData>();
      registrator.RegisterData<RuinsData>();
      registrator.RegisterData<MaintenanceDepotsData>();
      registrator.RegisterData<RetainingWallsData>();
      registrator.RegisterData<BarriersData>();
      registrator.RegisterData<StatuesData>();
      registrator.RegisterData<TombOfCaptainsData>();
      registrator.RegisterData<StackerData>();
      registrator.RegisterData<TrucksData>();
      registrator.RegisterData<ExcavatorsData>();
      registrator.RegisterData<TreeHarvestersData>();
      registrator.RegisterData<TreePlanetersData>();
      registrator.RegisterData<RocketAssemblyDepotData>();
      registrator.RegisterData<RocketLaunchPadData>();
      registrator.RegisterData<RocketsData>();
      registrator.RegisterData<CropsData>();
      registrator.RegisterData<FoodData>();
      registrator.RegisterData<FarmsData>();
      registrator.RegisterData<AnimalFarmsData>();
      registrator.RegisterData<FoodMillData>();
      registrator.RegisterData<FoodProcessorData>();
      registrator.RegisterData<BakingUnitData>();
      registrator.RegisterData<TechnologyData>();
      registrator.RegisterData<CargoShipsData>();
      registrator.RegisterData<TradeDockData>();
      registrator.RegisterData<FleetData>();
      registrator.RegisterData<ShipyardData>();
      registrator.RegisterData<SettlementsData>();
      registrator.RegisterData<SettlementDecorationsData>();
      registrator.RegisterData<WorldMapEntitiesData>();
      registrator.RegisterData<WorldMapIslands>();
      registrator.RegisterData<DiseasesData>();
      registrator.RegisterData<EdictsData>();
      registrator.RegisterDataWithInterface<IResearchNodesData>();
      new ResearchNodesPositionSetup(registrator.PrototypesDb).SetupPositionsAndParents();
      registrator.RegisterData<MessageGroupsData>();
      registrator.RegisterData<ChatMessagesData>();
      registrator.RegisterData<TutorialMessagesData>();
      registrator.RegisterData<WarningMessagesData>();
      registrator.RegisterData<GoalsData>();
    }

    void IMod.RegisterDependencies(
      DependencyResolverBuilder depBuilder,
      ProtosDb protosDb,
      bool gameWasLoaded)
    {
      if (gameWasLoaded || !this.Config.BuildStartingFactory)
        return;
      depBuilder.RegisterDependency<StartingFactoryPlacer>().AsSelf();
      depBuilder.RegisterDependency<StartingFactoryBuilder>().AsSelf();
    }

    void IMod.EarlyInit(DependencyResolver resolver)
    {
    }

    void IMod.Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
    }

    public static ImmutableArray<StaticIslandMapPreviewData> GetMaps()
    {
      return ImmutableArray.Create<StaticIslandMapPreviewData>(AlphaStaticIslandMap.GetPreviewData(), BeachStaticIslandMap.GetPreviewData(), CurlandMap.GetPreviewData(), GoldenPeakStaticIslandMap.GetPreviewData(), InsulaMortis.GetPreviewData(), YouShallNotPassStaticIslandMap.GetPreviewData());
    }
  }
}
