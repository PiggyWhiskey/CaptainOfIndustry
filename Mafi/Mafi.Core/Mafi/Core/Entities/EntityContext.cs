// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntityContext
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Environment;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Context that is accessible through any Entity in game.
  /// It should be used only for operations that are not performed
  /// often. For anything that happens on every simUpdate or every
  /// time a product is made, cache the references.
  /// 
  /// Benefit of this is that it reduces size of Entity classes that
  /// needs some of these deps rarely. It also reduces duplication of
  /// manages between individual entity helpers. Finally it is also not
  /// saved (faster save) and created from resolver on load. So deps can
  /// be added / removed anytime.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class EntityContext
  {
    public readonly IConstructionManager ConstructionManager;
    public readonly IEntitiesManager EntitiesManager;
    public readonly IAssetTransactionManager AssetTransactionManager;
    public readonly INotificationsManager NotificationsManager;
    public readonly IPropertiesDb PropertiesDb;
    public readonly IIoPortsManager IoPortsManager;
    public readonly IProductsManager ProductsManager;
    public readonly IWorkersManager WorkersManager;
    public readonly IUpointsManager UpointsManager;
    public readonly ICalendar Calendar;
    public readonly IComputingConsumerFactory ComputingConsumerFactory;
    public readonly IElectricityConsumerFactory ElectricityConsumerFactory;
    public readonly IUnityConsumerFactory UnityConsumerFactory;
    public readonly ProtosDb ProtosDb;
    public readonly UnlockedProtosDb UnlockedProtosDb;
    public readonly IoPortId.Factory PortIdFactory;
    public readonly TerrainManager TerrainManager;
    public readonly AirPollutionManager AirPollutionManager;

    public EntityContext(
      IConstructionManager constructionManager,
      IEntitiesManager entitiesManager,
      IAssetTransactionManager assetTransactionManager,
      INotificationsManager notificationsManager,
      IPropertiesDb propertiesDb,
      IIoPortsManager ioPortsManager,
      IProductsManager productsManager,
      IWorkersManager workersManager,
      IUpointsManager upointsManager,
      ICalendar calendar,
      IComputingConsumerFactory computingConsumerFactory,
      IElectricityConsumerFactory electricityConsumerFactory,
      IUnityConsumerFactory unityConsumerFactory,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      IoPortId.Factory portIdFactory,
      TerrainManager terrainManager,
      AirPollutionManager airPollutionManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConstructionManager = constructionManager;
      this.EntitiesManager = entitiesManager;
      this.AssetTransactionManager = assetTransactionManager;
      this.NotificationsManager = notificationsManager;
      this.PropertiesDb = propertiesDb;
      this.IoPortsManager = ioPortsManager;
      this.ProductsManager = productsManager;
      this.WorkersManager = workersManager;
      this.UpointsManager = upointsManager;
      this.Calendar = calendar;
      this.ComputingConsumerFactory = computingConsumerFactory;
      this.ElectricityConsumerFactory = electricityConsumerFactory;
      this.UnityConsumerFactory = unityConsumerFactory;
      this.ProtosDb = protosDb;
      this.UnlockedProtosDb = unlockedProtosDb;
      this.PortIdFactory = portIdFactory;
      this.TerrainManager = terrainManager;
      this.AirPollutionManager = airPollutionManager;
    }
  }
}
