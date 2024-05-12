using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Utils;
using Mafi.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;

using TerrainTower.Extras;

using static Mafi.Base.Prototypes.Buildings.ThermalStorages.ThermalStorageProto;

namespace TerrainTower.TTower
{
    /// <summary>
    /// <para><see cref="m_productsData"/> All mining processing, handled as <see cref="TerrainTowerProductData"/></para>
    /// SortProducts = Move from Unsorted to BeingSorted
    /// PushResultsToBuffer = Move from BeingSorted to Sorted
    /// Functions separated to allow paced sorted, and to prevent abuse of extremely fast mining.
    /// </summary>
    [GenerateSerializer]
    public sealed partial class TerrainTowerEntity : LayoutEntity, IMaintainedEntity, IElectricityConsumingEntity, IEntityWithWorkers, IEntityWithPorts, IAreaManagingTower, IEntityWithBoost, IUnityConsumingEntity, IEntityWithSimUpdate
    {
        #region _STATIC CLASS INFO

        public static readonly Quantity MAX_DUMP_QUANTITY;
        public static readonly Quantity MAX_MINE_QUANTITY;

        /// <summary>
        /// Output Ports count, for the purpose of assigning outputs.
        /// </summary>
        public static readonly int MAX_OUTPUTS;

        /// <summary>
        /// Max items selectable
        /// </summary>
        public static readonly int MAX_PRODUCTS;

        public static readonly ThicknessTilesF MAX_TERRAIN_MOD_THICKNESS;
        public static readonly ThicknessTilesF MAX_TOP_LAYER_FOR_MINING_BELOW;
        private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

        //private static readonly hasMined<TerrainTowerEntity, BlobWriter> s_serializeDataDelayedAction;
        //private static readonly hasMined<TerrainTowerEntity, BlobReader> s_deserializeDataDelayedAction;
        private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

        static TerrainTowerEntity()
        {
            MAX_OUTPUTS = 8;
            MAX_PRODUCTS = 10;
            MAX_DUMP_QUANTITY = 100.Quantity();
            MAX_MINE_QUANTITY = 100.Quantity();
            MAX_TERRAIN_MOD_THICKNESS = 1.0.TilesThick();
            MAX_TOP_LAYER_FOR_MINING_BELOW = 0.5.TilesThick();

            s_serializeDataDelayedAction = ((obj, writer) => ((TerrainTowerEntity)obj).SerializeData(writer));
            s_deserializeDataDelayedAction = ((obj, reader) => ((TerrainTowerEntity)obj).DeserializeData(reader));
        }

        #endregion _STATIC CLASS INFO

        #region _SERIALISATION

        public static TerrainTowerEntity Deserialize(BlobReader reader)
        {
            if (reader.TryStartClassDeserialization(out TerrainTowerEntity terrainTower))
            {
                reader.EnqueueDataDeserialization(terrainTower, s_deserializeDataDelayedAction);
            }
            return terrainTower;
        }

        public static void Serialize(TerrainTowerEntity value, BlobWriter writer)
        {
            if (writer.TryStartClassSerialization(value))
            {
                writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }
        }

        protected override void DeserializeData(BlobReader reader)
        {
            base.DeserializeData(reader);
            CurrentState = (State)reader.ReadInt();
            TerrainConfigState = (TerrainTowerConfigState)reader.ReadInt();
            IsBoosted = reader.ReadBool();
            IsBoostRequested = reader.ReadBool();
            m_sortedBufferIsPositive = reader.ReadBool();

            m_prototype = reader.ReadGenericAs<TerrainTowerProto>();

            Area = RectangleTerrainArea2i.Deserialize(reader);

            reader.SetField(this, nameof(m_productsData), Dict<ProductProto, TerrainTowerProductData>.Deserialize(reader));
            reader.SetField(this, nameof(m_dumpBuffers), Dict<ProductProto, GlobalOutputBuffer>.Deserialize(reader));

            reader.SetField(this, nameof(m_productsManager), reader.ReadGenericAs<IProductsManager>());

            m_terrainTowerManager = Option<TerrainTowersManager>.Deserialize(reader);

            reader.SetField(this, nameof(m_designationManager), TerrainDesignationsManager.Deserialize(reader));
            reader.SetField(this, nameof(m_managedDesignations), Set<TerrainDesignation>.Deserialize(reader));
            reader.SetField(this, nameof(m_unfulfilledDumpingDesignations), Set<TerrainDesignation>.Deserialize(reader));
            reader.SetField(this, nameof(m_unfulfilledMiningDesignations), Set<TerrainDesignation>.Deserialize(reader));

            AllSupportedProducts = ImmutableArray<ProductProto>.Deserialize(reader);

            reader.SetField(this, nameof(m_calendar), reader.ReadGenericAs<ICalendar>());
            reader.SetField(this, nameof(m_electricityConsumer), reader.ReadGenericAs<IElectricityConsumer>());
            UnityConsumer = reader.ReadGenericAs<Option<UnityConsumer>>();
            Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();

            reader.SetField(this, nameof(m_sortingTimer), TickTimer.Deserialize(reader));
            reader.SetField(this, nameof(m_terrainTimer), TickTimer.Deserialize(reader));

            m_entityBoostedNotif = EntityNotificator.Deserialize(reader);
            m_missingDumpItemNotif = EntityNotificator.Deserialize(reader);
            m_missingDumpingDesignationNotif = EntityNotificator.Deserialize(reader);
            m_missingMiningDesignationNotif = EntityNotificator.Deserialize(reader);
            m_outputBlockedNotif = EntityNotificator.Deserialize(reader);
            m_inputBlockedNotif = EntityNotificator.Deserialize(reader);

            reader.RegisterInitAfterLoad(this, nameof(initSelf), InitPriority.Normal);
        }

        protected override void SerializeData(BlobWriter writer)
        {
            //TODO: FINISH SERIALISATION
            base.SerializeData(writer);

            writer.WriteInt((int)CurrentState);
            writer.WriteInt((int)TerrainConfigState);
            writer.WriteBool(IsBoosted);
            writer.WriteBool(IsBoostRequested);
            writer.WriteBool(m_sortedBufferIsPositive);

            writer.WriteGeneric(m_prototype);

            RectangleTerrainArea2i.Serialize(Area, writer);

            Dict<ProductProto, TerrainTowerProductData>.Serialize(m_productsData, writer);
            Dict<ProductProto, GlobalOutputBuffer>.Serialize(m_dumpBuffers, writer);

            writer.WriteGeneric(m_productsManager);

            Option<TerrainTowersManager>.Serialize(m_terrainTowerManager, writer);

            TerrainDesignationsManager.Serialize(m_designationManager, writer);
            Set<TerrainDesignation>.Serialize(m_managedDesignations, writer);
            Set<TerrainDesignation>.Serialize(m_unfulfilledDumpingDesignations, writer);
            Set<TerrainDesignation>.Serialize(m_unfulfilledMiningDesignations, writer);

            ImmutableArray<ProductProto>.Serialize(AllSupportedProducts, writer);

            writer.WriteGeneric(m_calendar);
            writer.WriteGeneric(m_electricityConsumer);
            writer.WriteGeneric(UnityConsumer);
            writer.WriteGeneric(Maintenance);

            TickTimer.Serialize(m_sortingTimer, writer);
            TickTimer.Serialize(m_terrainTimer, writer);

            EntityNotificator.Serialize(m_entityBoostedNotif, writer);
            EntityNotificator.Serialize(m_missingDumpItemNotif, writer);
            EntityNotificator.Serialize(m_missingDumpingDesignationNotif, writer);
            EntityNotificator.Serialize(m_missingMiningDesignationNotif, writer);
            EntityNotificator.Serialize(m_outputBlockedNotif, writer);
            EntityNotificator.Serialize(m_inputBlockedNotif, writer);
        }

        /// <summary>
        /// Called from Constructor and <see cref="initSelf"/>. Sets initial data of calculated, but not user modified values (i.e. SupportedProducts, OutputNames)
        /// </summary>
        private void initData()
        {
            Logger.Info("TerrainTowerEntity.initData");
            //Set Capacity
            Capacity = Prototype.BufferCapacity;

            DumpTotal = m_dumpBuffers.Sum(buff => buff.Value.Quantity.Value).Quantity();

            //Set all minable products
            AllSupportedProducts = Context.ProtosDb.All<TerrainMaterialProto>().Select(x => (ProductProto)x.MinedProduct).Distinct().ToImmutableArray();

            //Set available Output Ports
            m_outputsNames = Ports.Where(x => x.Type == IoPortType.Output).Select(x => x.Name).OrderBy(x => x).Prepend('-').ToImmutableArray();
        }

        /// <summary>
        /// Intiation on deserialisation, also calls <see cref="initData"/>
        /// </summary>
        [InitAfterLoad(InitPriority.Low)]
        private void initSelf(int saveVersion)
        {
            Logger.Info("TerrainTowerEntity.initSelf");
            if (m_productsData != null)
            {
                //Create tmp Reference Buffers
                m_outputBuffers = m_productsData.Values.Select<TerrainTowerProductData, IProductBuffer>(x => x.Buffer).ToLyst();
                //Mimic SortSortingPlant Entity
                //Update Mixed Quantity

                updateMixedQuantity();
            }
            else
            {
                Logger.Warning("TerrainTowerEntity.initSelf: m_productsData is null");
                MixedTotal = Quantity.Zero;
            }

            //Call Universal Init (Whether Construtor or Deserialisation)
            initData();

            //Mimic Machine Entity
            if (IsBoostRequested)
            {
                UnityConsumer valueOrNull = UnityConsumer.ValueOrNull;
                if (valueOrNull != null && valueOrNull.MonthlyUnity.IsNotPositive)
                {
                    UnityConsumer.Value?.RefreshUnityConsumed();
                }
            }
        }

        #endregion _SERIALISATION

        #region _QUICK REMOVE

        //TODO: Add quick remove functionality to view
        public bool QuickRemoveProduct(ProductQuantity productQuantity)
        {
            if (!GetQuickRemoveCost(out Upoints unity, productQuantity.Quantity) || unity.IsNotPositive) return false;

            Context.UpointsManager.ConsumeExactly(IdsCore.UpointsCategories.QuickRemove, unity);
            Context.AssetTransactionManager.StoreClearedProduct(productQuantity);
            return true;
        }

        internal bool GetQuickRemoveCost(out Upoints unity, Quantity quantity)
        {
            unity = Upoints.Zero;
            if (quantity.IsNotPositive) return false;
            unity = QuantityToUnityCost(quantity.Value, Context.UpointsManager.QuickActionCostMultiplier) ?? Upoints.Zero;
            return Context.UpointsManager.CanConsume(unity);
            //QuickDeliverCostHelper
            Upoints? QuantityToUnityCost(int Quantity, Percent Multiplier)
            {
                if (Quantity <= 0)
                {
                    return new Upoints?();
                }
                if (Quantity > Fix32.MaxValue)
                {
                    Quantity = Fix32.MaxIntValue.IntegerPart;
                }

                return new Upoints?((((0.47.ToFix32() * Quantity.Sqrt() - 0.8.ToFix32()) * 10.ToFix32()).ToIntRounded() / 10.ToFix32())
                    .Upoints()
                    .ScaledBy(Multiplier)
                    .Max(0.1.Upoints()));
            }
        }

        #endregion _QUICK REMOVE

        #region _PRODUCT VARIABLES

        /// <summary>
        /// Dict of GlobalOutputBuffers to handle Inbound Products for dumping
        /// </summary>
        private readonly Dict<ProductProto, GlobalOutputBuffer> m_dumpBuffers;

        /// <summary>
        /// All mined products and their buffers/sorting/output control and data <see cref="TerrainTowerProductData"/>
        /// </summary>
        private readonly Dict<ProductProto, TerrainTowerProductData> m_productsData;

        /// <summary>
        /// Interface used within <see cref="TerrainTowerProductData"/> to control product output to other entities
        /// </summary>
        private readonly IProductsManager m_productsManager;

        /// <summary>
        /// Chars from A to Z, 1 for each Output Port, plus a '-' for do not assign
        /// </summary>
        private ImmutableArray<char> m_outputsNames;

        /// <summary>
        /// Products we've mined or selected to be Sorted/Outputted
        /// </summary>
        public IEnumerable<ProductProto> AllowedProducts => m_productsData.Keys;

        /// <summary>
        /// All mineable products
        /// </summary>
        public ImmutableArray<ProductProto> AllSupportedProducts { get; private set; }

        public IReadOnlyCollection<IProductBuffer> DumpBuffers => m_dumpBuffers.Values;

        /// <summary>
        /// Local Variable - Sum of m_dumpBuffers quantity - Cached to allow fast access
        /// </summary>
        [DoNotSave(0, null)]
        public Quantity DumpTotal { get; private set; }

        /// <summary>
        /// Total of all Unsorted Quantities
        /// - Cached and updated via <see cref="updateMixedQuantity"/>
        /// - Updated during <see cref="tryMineDesignation_MineLayer"/> too
        /// </summary>
        [DoNotSave(0, null)]
        public Quantity MixedTotal { get; private set; }

        #endregion _PRODUCT VARIABLES

        private readonly ICalendar m_calendar;
        private readonly TerrainDesignationsManager m_designationManager;

        private readonly IElectricityConsumer m_electricityConsumer;
        private readonly Set<TerrainDesignation> m_managedDesignations;
        private readonly TickTimer m_sortingTimer;

        private readonly TickTimer m_terrainTimer;

        /// <summary>
        /// Used to speed up the process of finding the nearest Dumping Designation by storing only unfulfilled designations.
        /// </summary>
        private readonly Set<TerrainDesignation> m_unfulfilledDumpingDesignations;

        /// <summary>
        /// Used to speed up the process of finding the nearest Mining Designation by storing only unfulfilled designations.
        /// </summary>
        private readonly Set<TerrainDesignation> m_unfulfilledMiningDesignations;

        private EntityNotificator m_entityBoostedNotif;
        private EntityNotificator m_inputBlockedNotif;
        private EntityNotificator m_missingDumpingDesignationNotif;
        private EntityNotificator m_missingDumpItemNotif;
        private EntityNotificator m_missingMiningDesignationNotif;
        private EntityNotificator m_outputBlockedNotif;

        /// <summary>
        /// Private variable for all <see cref="m_productsData"/> buffers.
        /// </summary>
        [DoNotSave(0, null)]
        private Lyst<IProductBuffer> m_outputBuffers;

        private TerrainTowerProto m_prototype;
        private bool m_sortedBufferIsPositive;
        private Option<TerrainTowersManager> m_terrainTowerManager;

        [DoNotSave(0, null)]
        private TerrainDesignation m_tmpDumpDesignation;

        [DoNotSave(0, null)]
        private TerrainDesignation m_tmpMineDesignation;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="id"></param>
        /// <param name="proto"></param>
        /// <param name="transform"></param>
        /// <param name="context"></param>
        /// <param name="designationManager"></param>
        /// <param name="maintenanceProvidersFactory"></param>
        /// <param name="productsManager"></param>
        /// <param name="towersManager"></param>
        internal TerrainTowerEntity(
            EntityId id,
            TerrainTowerProto proto,
            TileTransform transform,
            EntityContext context,
            TerrainDesignationsManager designationManager,
            ICalendar calendar,
            IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
            IProductsManager productsManager,
            TerrainTowersManager towersManager) : base(id, proto, transform, context)
        {
            Prototype = proto;

            //Designation Management
            m_designationManager = designationManager;
            m_designationManager.DesignationAdded.Add(this, new Action<TerrainDesignation>(onDesignationAdded));
            m_designationManager.DesignationRemoved.Add(this, new Action<TerrainDesignation>(onDesignationRemoved));
            m_designationManager.DesignationFulfilledChanged.Add(this, new Action<TerrainDesignation>(onDesignationFulfilledChanged));

            //All Mined Products (Mixed, Unsorted, Sorted)
            m_productsData = new Dict<ProductProto, TerrainTowerProductData>();

            //Buffers from m_productsData.Values.Buffer
            m_outputBuffers = new Lyst<IProductBuffer>();

            //All Dump products
            m_dumpBuffers = new Dict<ProductProto, GlobalOutputBuffer>();

            //Product Manager used to Add/Remove products from the world production statistics
            m_productsManager = productsManager;

            m_managedDesignations = new Set<TerrainDesignation>();
            m_unfulfilledMiningDesignations = new Set<TerrainDesignation>();
            m_unfulfilledDumpingDesignations = new Set<TerrainDesignation>();

            //Timers for Sorting and Terrain manipulation
            m_sortingTimer = new TickTimer();
            m_terrainTimer = new TickTimer();

            //Tower Manager is used to show the tower area along with Mining Tower/Forestry Tower
            m_terrainTowerManager = (Option<TerrainTowersManager>)towersManager;

            //Electricity and Maintenance Consumers during production
            m_electricityConsumer = Context.ElectricityConsumerFactory.CreateConsumer(this);
            Maintenance = maintenanceProvidersFactory.CreateFor(this);

            //Unity Consumer is created when actually boosting
            UnityConsumer = Option.None;

            //Notifications for Missing Designations, Items, and Blocked Outputs
            m_missingDumpItemNotif = context.NotificationsManager.CreateNotificatorFor(Extras.CustomIds.Notifications.TerrainTowerMissingDumpItem);
            m_entityBoostedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.EntityIsBoosted);
            m_outputBlockedNotif = context.NotificationsManager.CreateNotificatorFor(Extras.CustomIds.Notifications.TerrainTowerBlockedOuput);
            m_missingMiningDesignationNotif = context.NotificationsManager.CreateNotificatorFor(Extras.CustomIds.Notifications.TerrainTowerMissingMineDesignation);
            m_missingDumpingDesignationNotif = context.NotificationsManager.CreateNotificatorFor(Extras.CustomIds.Notifications.TerrainTowerMissingDumpDesignation);
            m_inputBlockedNotif = context.NotificationsManager.CreateNotificatorFor(Extras.CustomIds.Notifications.TerrainTowerFullMixedBuffer);

            //Add Calendar to track monthly sorting values
            m_calendar = calendar;
            calendar.NewMonth.Add(this, new Action(onNewMonth));

            //Set default to Flatten
            TerrainConfigState = TerrainTowerConfigState.Flatten;

            calcIntialArea();
            initData();
        }

        public RectangleTerrainArea2i Area { get; private set; }

        /// <summary>
        /// Unused by Terrain Tower, but required for Interface
        /// </summary>
        public IReadOnlySet<FuelStation> AssignedFuelStations => null;

        public Upoints? BoostCost => Prototype.BoostCost;

        public override bool CanBePaused => true;

        /// <summary>
        /// General Capacity Value, set from Prototype. Used for <see cref="MixedTotal"/>, <see cref="m_productsData"/>, and <see cref="m_dumpBuffers"/>
        /// </summary>
        [DoNotSave(0, null)]
        public Quantity Capacity { get; internal set; }

        public State CurrentState { get; private set; }

        public Option<IElectricityConsumerReadonly> ElectricityConsumer => m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();

        [DoNotSave(0, null)]
        bool IEntityWithWorkers.HasWorkersCached { get; set; }

        public bool IsBoosted { get; private set; }
        public bool IsBoostRequested { get; private set; }

        public bool IsIdleForMaintenance => CurrentState != State.Working;

        public IEntityMaintenanceProvider Maintenance { get; private set; }

        MaintenanceCosts IMaintainedEntity.MaintenanceCosts => Prototype.Costs.Maintenance;

        public IReadOnlySet<TerrainDesignation> ManagedDesignations => m_managedDesignations;

        public Upoints MaxMonthlyUnityConsumed => MonthlyUnityConsumed;

        /// <summary>
        /// Quantity left in <see cref="MixedTotal"/>
        /// </summary>
        public Quantity MixedCapacityLeft => (Capacity - MixedTotal).Max(Quantity.Zero);

        /// <summary>
        /// Percent: <see cref="MixedTotal"/> / <see cref="Capacity"/>
        /// </summary>
        public Percent MixedPercentFull => Capacity.IsNotPositive || MixedTotal.IsNotPositive ? Percent.Zero : !(MixedTotal > Capacity) ? Percent.FromRatio(MixedTotal.Value, Capacity.Value) : Percent.Hundred;

        /// <summary>
        /// If Boost is Requested, and a Boost Cost is set, return the cost, otherwise return 0
        /// </summary>
        public Upoints MonthlyUnityConsumed => IsBoostRequested && BoostCost.HasValue ? BoostCost.Value : Upoints.Zero;

        /// <summary>
        /// Buffers from private variable <see cref="m_outputBuffers"/>
        /// </summary>
        public IIndexable<IProductBuffer> OutputBuffers => m_outputBuffers;

        Electricity IElectricityConsumingEntity.PowerRequired => Prototype.ElectricityConsumed;

        [DoNotSave(0, null)]
        public new TerrainTowerProto Prototype
        {
            get => m_prototype;
            set
            {
                m_prototype = value;
                base.Prototype = value;
            }
        }

        [DoNotSave(0, null)]
        public Duration SortDuration => Prototype.SortDuration;

        /// <summary>
        /// Quantity that is allowed to be sorted per SortDuration
        /// </summary>
        [DoNotSave(0, null)]
        public Quantity SortQuantityPerDuration => Prototype.SortQuantityPerDuration;

        public TerrainTowerConfigState TerrainConfigState { get; internal set; }

        public Option<UnityConsumer> UnityConsumer { get; private set; }

        Proto.ID IUnityConsumingEntity.UpointsCategoryId => IdsCore.UpointsCategories.Boost;

        int IEntityWithWorkers.WorkersNeeded => Prototype.Costs.Workers;

        protected override bool IsEnabledNow => base.IsEnabledNow && Maintenance.CanWork();

        private TerrainManager ContextTerrainManager => Context.TerrainManager;

        private bool HasUnfulfilledDumping { get; set; }
        private bool HasUnfulfilledMining { get; set; }

        /// <summary>
        /// If the Tower is missing a configured Terrain Designation, TRUE
        /// HasUnfulfilled is set from updateTerrainNotifications
        /// </summary>
        private bool IsMissingDesignation
        {
            get
            {
                return (TerrainConfigState == TerrainTowerConfigState.Mining && !HasUnfulfilledMining)
                    || (TerrainConfigState == TerrainTowerConfigState.Dumping && !HasUnfulfilledDumping)
                    || (TerrainConfigState == TerrainTowerConfigState.Flatten && !HasUnfulfilledDumping && !HasUnfulfilledMining);
            }
        }

        /// <summary>
        /// Gets all Products from <see cref="m_productsData"/> for a particular port
        /// - Used within <see cref="TerrainTowerPortProductResolver"/>
        /// </summary>
        /// <param name="port"></param>
        /// <returns>List of Products for an Output Port</returns>
        public ImmutableArray<ProductProto> GetPortProducts(IoPort port)
        {
            return m_productsData.Values.Where(x => x.OutputPort == port.Name).Select(x => x.Buffer.Product).ToImmutableArray();
        }

        /// <summary>
        /// Receive products to <see cref="m_dumpBuffer"/> creating new buffer entry if not already present
        /// </summary>
        /// <param name="pq">Product/Quantity to receive</param>
        /// <param name="sourcePort">Receive from port token</param>
        /// <returns>Quantity that could not fit</returns>
        Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
        {
            //Receive only if enabled
            if (IsNotEnabled) return pq.Quantity;

            //Buffer doesn't exist, create and add to m_dumpBuffers
            if (!m_dumpBuffers.TryGetValue(pq.Product, out GlobalOutputBuffer dumpBuffer))
            {
                dumpBuffer = new GlobalOutputBuffer(
                    Capacity,
                    pq.Product,
                    m_productsManager,
                    15,
                    this);
                m_dumpBuffers.TryAdd(pq.Product, dumpBuffer);
            }
            else if (dumpBuffer.IsFull)
            {
                return pq.Quantity;
            }

            Quantity quant = dumpBuffer.StoreAsMuchAs(pq.Quantity);
            DumpTotal += pq.Quantity - quant;
            return quant;
        }

        /// <summary>
        /// Set if Boost is Requested, and create a UnityConsumer if not already present
        /// </summary>
        /// <param name="isBoosted">TRUE/FALSE for if a Boost is to be Enabled or Disabeld</param>
        void IEntityWithBoost.SetBoosted(bool isBoosted)
        {
            if (isBoosted && !BoostCost.HasValue)
            {
                Assert.Fail(string.Format("Cannot boost '{0}', not allowed!", Prototype.Id));
            }
            m_entityBoostedNotif.NotifyIff(isBoosted, this);
            IsBoostRequested = isBoosted;

            if (IsBoostRequested && UnityConsumer.IsNone)
            {
                UnityConsumer = Context.UnityConsumerFactory.CreateConsumer(this);
            }
            else if (!IsBoostRequested && UnityConsumer.HasValue)
            {
                UnityConsumer.Value.Destroy();
                UnityConsumer = Option.None;
            }

            if (UnityConsumer.HasValue && UnityConsumer.Value.MonthlyUnity.IsNotPositive)
            {
                UnityConsumer.Value.RefreshUnityConsumed();
                Assert.That(UnityConsumer.Value.MonthlyUnity).IsPositive();
            }
        }

        /// <summary>
        /// Process SimUpdate including UpdateState, NotifyIfFullOutput, Mine/Dump, and SendOutputs
        /// </summary>
        void IEntityWithSimUpdate.SimUpdate()
        {
            CurrentState = simStepUpdateState();
            /*
            Paused              - No Work
            Broken              - No Work
            MissingWorkers      - No Work
            MissingDumpResource - Sorting
            NotEnoughPower      - Sorting
            MissingDesignation  - Sorting
            Working             - Terrain Work & Sorting
            */

            bool NoWork = CurrentState == State.Paused || CurrentState == State.Broken || CurrentState == State.MissingWorkers;
            bool CanTerrain = CurrentState == State.Working;
            bool CanSort = CurrentState == State.Working || CurrentState == State.MissingDumpResource || CurrentState == State.MissingDesignation;
            bool CanOutput = !NoWork;

            //Decrement Timers i.e. Actually count down Time (Not automatic)
            if (m_sortingTimer.IsNotFinished) { m_sortingTimer.DecrementOnly(); }
            if (m_terrainTimer.IsNotFinished) { m_terrainTimer.DecrementOnly(); }

            //Absolutly no work can be done if the Tower is Paused, Broken, or Missing Workers
            if (NoWork)
            {
                return;
            }

            if (CanTerrain)
            {
                //Boosted and Can Work = Boosted
                IsBoosted = IsBoostRequested && (UnityConsumer.ValueOrNull?.CanWork() ?? true);

                if (m_electricityConsumer.TryConsume())
                {
                    //If everything is working, and we have power, continue
                    if (simStepTerrainProcessing())
                    {
                        //Terrain Actions occured
                    }
                }
            }

            //Sorting after Terrain Processing to allow in-line Mining->Sorting->Output
            //Sorting does not need Power
            if (CanSort)
            {
                simStepSorting();
            }

            if (CanOutput)
            {
                sendOutputs();
            }
        }

        /// <summary>
        /// Add a Product to the Tower for Sorting/Output. Used automatically via SimUpdate - Vanilla by GUI
        /// </summary>
        /// <param name="product">ProductProto to add</param>
        /// <param name="port">optional port to assign</param>
        internal void AddProductToSort(ProductProto product, char? port = null)
        {
            if (m_productsData.ContainsKey(product)
                || m_productsData.Count >= MAX_PRODUCTS
                || !AllSupportedProducts.Contains(product))
            {
                return;
            }
            addProductDataFor(product, port);
        }

        /// <summary>
        /// Set new Area for the Tower, controlled via <see cref="TerrainTowerInspector"/> from the GUI. Snaps outwards to Terrain Designations.
        /// </summary>
        /// <param name="newArea">Area to Set</param>
        internal void EditManagedArea(RectangleTerrainArea2i newArea)
        {
            RectangleTerrainArea2i oldArea = Area;
            removeAllManagedDesignations();
            Tile2i origin = TerrainDesignation.GetOrigin(newArea.Origin);
            Tile2i tile2i = newArea.PlusXyCoordExcl;
            tile2i = tile2i.AddX(newArea.Size.X == 0 ? 0 : -1);
            Tile2i p2 = TerrainDesignation.GetOrigin(tile2i.AddY(newArea.Size.Y == 0 ? 0 : -1)) + TerrainDesignation.Size - 1;
            Area = RectangleTerrainArea2i.FromTwoPositions(origin, p2);

            foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>)m_designationManager.Designations)
            {
                onDesignationAdded(designation);
            }

#if true
            m_terrainTowerManager.ValueOrNull?.InvokeOnAreaChanged(this, oldArea);
#else
            if (m_terrainTowerManager.HasValue)
            {
                m_terrainTowerManager.Value.InvokeOnAreaChanged(this, oldArea);
            }
#endif
        }

        /// <summary>
        /// Gets all Mixed/Mined products from <see cref="m_productsData"/> to provide to GUI
        /// </summary>
        /// <param name="result">Lyst of ProductQuantity</param>
        internal void GetMixedInputProducts(Lyst<ProductQuantity> result)
        {
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                if (productData.UnsortedQuantity.IsPositive)
                {
                    result.Add(productData.Buffer.Product.WithQuantity(productData.UnsortedQuantity));
                }
            }
        }

        /// <summary>
        /// Retreive the portIndex assigned to a Product from <see cref="SetProductPortIndex"/>
        /// </summary>
        /// <param name="product">Product to check for</param>
        /// <returns>int Index of the Port from <see cref="m_outputsNames"/></returns>
        internal int GetPortIndexFor(ProductProto product)
        {
            return m_productsData.TryGetValue(product, out TerrainTowerProductData plantProductData)
                ? m_outputsNames.IndexOf(plantProductData.OutputPort).Clamp(0, m_outputsNames.Length - 1)
                : 0;
        }

        /// <summary>
        /// Reads <see cref="m_productsData"/> to get Quantity Sorted"/>
        /// </summary>
        /// <param name="product">Product Proto to read</param>
        /// <returns>Quantity Sorted</returns>
        internal Quantity GetSortedLastMonth(ProductProto product)
        {
            return m_productsData.TryGetValue(product, out TerrainTowerProductData productData) ? productData.SortedLastMonth : Quantity.Zero;
        }

        /// <summary>
        /// Tries to confirm if the ProductBlockAlert is set for the ProductProto
        /// </summary>
        /// <param name="product">Product Proto to check</param>
        /// <returns>TRUE if Full Output Notification set</returns>
        internal bool IsProductBlockedAlertEnabled(ProductProto product)
        {
            return m_productsData.TryGetValue(product, out TerrainTowerProductData plantProductData) && plantProductData.NotifyIfFullOutput;
        }

        /// <summary>
        /// Remove a product from the Tower and destroys any remaining product. Used automatically via SimUpdate - Vanilla by GUI
        /// </summary>
        /// <param name="product"></param>
        internal void RemoveProductToSort(ProductProto product)
        {
            if (m_productsData.TryRemove(product, out TerrainTowerProductData plantProductData))
            {
                m_outputBuffers.RemoveAndAssert(plantProductData.Buffer);
                Context.ProductsManager.ProductDestroyed(plantProductData.Buffer.Product, plantProductData.SortedQuantity + plantProductData.UnsortedQuantity, DestroyReason.Cleared);
                Context.AssetTransactionManager.ClearAndDestroyBuffer(plantProductData.Buffer);
                MixedTotal -= plantProductData.UnsortedQuantity;
                updateMixedBufferNotifications();
            }
        }

        /// <summary>
        /// Set <see cref="TerrainConfigState"/> by method
        /// </summary>
        /// <param name="configState"><see cref="TerrainTowerConfigState"/></param>
        internal void SetConfigState(TerrainTowerConfigState configState)
        {
            TerrainConfigState = configState;
            updateTerrainNotifications();
        }

        /// <summary>
        /// Set the Product to notify if the Output is full
        /// </summary>
        /// <param name="product">Product to Notify</param>
        /// <param name="isAlertEnabled">bool supplied by User via GUI</param>
        internal void SetProductBlockedAlert(ProductProto product, bool isAlertEnabled)
        {
            if (m_productsData.TryGetValue(product, out TerrainTowerProductData productData))
            {
                productData.NotifyIfFullOutput = isAlertEnabled;
            }
        }

        /// <summary>
        /// Assign a Product to an Output Port
        /// </summary>
        /// <param name="product">ProductProto to assign</param>
        /// <param name="portIndex">int Index of the Port to assign (Translated to Letters for UI)</param>
        internal void SetProductPortIndex(ProductProto product, int portIndex)
        {
            if (portIndex >= 0 && portIndex < m_outputsNames.Length)
            {
                char outputsName = m_outputsNames[portIndex];
                if (m_productsData.TryGetValue(product, out TerrainTowerProductData productData) && productData.OutputPort != outputsName)
                {
                    productData.OutputPort = outputsName;
                }
            }
        }

        /// <summary>
        /// Clean up Entity when Destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            //Remove Designation Manager Events
            m_designationManager.DesignationAdded.Remove(this, new Action<TerrainDesignation>(onDesignationAdded));
            m_designationManager.DesignationRemoved.Remove(this, new Action<TerrainDesignation>(onDesignationRemoved));
            m_designationManager.DesignationFulfilledChanged.Remove(this, new Action<TerrainDesignation>(onDesignationFulfilledChanged));
            removeAllManagedDesignations();
            m_terrainTowerManager = Option<TerrainTowersManager>.None;

            m_calendar.NewMonth.Remove(this, new Action(onNewMonth));
            //Remove all products from m_productsData
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                Context.ProductsManager.ProductDestroyed(productData.Buffer.Product, productData.UnsortedQuantity + productData.SortedQuantity, DestroyReason.Cleared);
                Context.AssetTransactionManager.ClearAndDestroyBuffer(productData.Buffer);
            }
            foreach (GlobalOutputBuffer dumpBuffer in m_dumpBuffers.Values)
            {
                Context.ProductsManager.ProductDestroyed(dumpBuffer.Product, dumpBuffer.Quantity, DestroyReason.Cleared);
                Context.AssetTransactionManager.ClearAndDestroyBuffer(dumpBuffer);
            }

            //Disable Notifications
            m_entityBoostedNotif.Deactivate(this);
            m_missingDumpItemNotif.Deactivate(this);
            m_missingMiningDesignationNotif.Deactivate(this);
            m_missingDumpingDesignationNotif.Deactivate(this);
            m_outputBlockedNotif.Deactivate(this);

            base.OnDestroy();
        }

        /// <summary>
        /// Used to Start/Stop processes and external events when Entity is Paused/Restarted
        /// </summary>
        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();
            if (IsEnabled)
            {
                //Start Processes
            }
            else
            {
                //End Processes
            }
        }

        /// <summary>
        /// Add Products to the <see cref="m_productsData"/> to control Sorting/Outputting
        /// </summary>
        /// <param name="product">ProductProto of item to add</param>
        /// <param name="port">char of the Port to control via GUI 'dash' for nothing</param>
        private void addProductDataFor(ProductProto product, char? port = null)
        {
            //Choose first unused Port ID
            //char outputPort = port ?? m_outputsNames.FirstOrDefault(designation => m_productsData.Values.All(p => p.OutputPort != designation));

            //Set port to first available (Dash, unassigned)
            char outputPort = port ?? m_outputsNames[0];

            if (outputPort == char.MinValue)
            {
                outputPort = m_outputsNames.Last;
            }
            GlobalOutputBuffer buffer = new GlobalOutputBuffer(
                Capacity,
                product,
                m_productsManager,
                15,
                this);
            TerrainTowerProductData productData = new TerrainTowerProductData(buffer, outputPort);
            m_productsData.Add(buffer.Product, productData);
            m_outputBuffers.Add(productData.Buffer);
        }

        /// <summary>
        /// Calculate initial area for the Tower based on <see cref="TerrainTowerProto"/> static properties
        /// </summary>
        private void calcIntialArea()
        {
            //Generate initial area
            RelTile2i initOrigin = Prototype.Area.Origin;
            RelTile2i relTile2i1 = new RelTile2i(0, Prototype.Area.InitialSize.X / 2);
            RelTile2i relTile2i2 = new RelTile2i(Prototype.Area.InitialSize.Y, 0);
            Tile2i absolute1 = relAreaToAbsolute(initOrigin + relTile2i1);
            Tile2i absolute2 = relAreaToAbsolute(initOrigin + relTile2i2 + relTile2i1);
            Tile2i absolute3 = relAreaToAbsolute(initOrigin + relTile2i2 - relTile2i1);
            Tile2i absolute4 = relAreaToAbsolute(initOrigin - relTile2i1);
            Tile2i tile2i = absolute1.Min(absolute2).Min(absolute3).Min(absolute4);

            //BUGFIX: Source of error on new Tower creation
#if true
            EditManagedArea(new RectangleTerrainArea2i(tile2i, Prototype.Area.InitialSize));
#else
            Area = new RectangleTerrainArea2i(tile2i, Prototype.Area.InitialSize);
#endif
            Tile2i relAreaToAbsolute(RelTile2i tile)
            {
                return Prototype.Layout.Transform(tile, Transform);
            }
        }

        /// <summary>
        /// Clear any Custom Surfaces from a tileAndIndex if it is above the targetHeight
        /// </summary>
        /// <param name="tileAndIndex">Tile to search</param>
        /// <param name="targetHeight">Height to check for</param>
        private void clearCustomSurface(Tile2iAndIndex tileAndIndex, HeightTilesF targetHeight)
        {
            if (ContextTerrainManager.TryGetTileSurface(tileAndIndex.Index, out TileSurfaceData tileSurfaceData))
            {
                if (!tileSurfaceData.IsAutoPlaced && tileSurfaceData.Height > targetHeight)
                {
                    ContextTerrainManager.ClearCustomSurface(tileAndIndex);
                }
            }
        }

        //TODO: Test if working
        /// <summary>
        /// Remove props from a tile (Bushes etc)
        /// - Taken from MiningJob.handleMining()
        /// </summary>
        /// <param name="tile">Tile2i of location</param>
        private void clearProps(Tile2i tile)
        {
#if DEBUG2
            //m_designationManager.TerrainPropsManager.ContainsPropInDesignation(designation)
            Logger.InfoDebug("clearProps: {0}", tile);
#endif
            TerrainPropsManager tpm = m_designationManager.TerrainPropsManager;
            if (tpm.TerrainTileToProp.TryGetValue(tile.AsSlim, out TerrainPropId key))
            {
                if (!tpm.TerrainProps.TryGetValue(key, out TerrainPropData propData))
                {
                    Logger.Warning("clearProps: PropData not found for {0}", key);
                }
                else if (!tpm.TryRemovePropAtTile(tile, false))
                {
                    Logger.Warning("clearProps: Failed to remove prop {0}", key);
                }
                else
                {
                    if (propData.Proto.ProductWhenHarvested.IsNotEmpty)
                    {
                        //Send Prop product to shipyard
                        ProductQuantity pq = propData.Proto.ProductWhenHarvested.ScaledBy(propData.Scale);
                        m_productsManager.ProductCreated(pq.Product, pq.Quantity, CreateReason.MinedFromTerrain);
                        Context.AssetTransactionManager.StoreClearedProduct(pq);
                    }
                }
            }
        }

        /// <summary>
        /// Validate if a Designation is within the Tower Area
        /// </summary>
        /// <param name="designation"><see cref="IDesignation"/></param>
        /// <returns>TRUE if within <see cref="Area"/></returns>
        private bool isWithinArea(TerrainDesignation designation) => Area.ContainsTile(designation.OriginTileCoord);

        /// <summary>
        /// Update <see cref="m_managedDesignations"/> when designations are added within the Tower Area
        /// </summary>
        /// <param name="designation">TerrainDesignation provided</param>
        private void onDesignationAdded(TerrainDesignation designation)
        {
            if (designation.Prototype.IsTerraforming && isWithinArea(designation))
            {
                designation.AddManagingTower(this);
                m_managedDesignations.AddAndAssertNew(designation);
                updateTerrainNotifications();
                if (designation.IsMiningNotFulfilled)
                {
                    m_unfulfilledMiningDesignations.AddAndAssertNew(designation);
                }
                if (designation.IsDumpingNotFulfilled)
                {
                    m_unfulfilledDumpingDesignations.AddAndAssertNew(designation);
                }
            }
        }

        /// <summary>
        /// If a Designation is fulfilled or unfulfilled (collapse), update the Tower Notifications, add/remove from temp variables
        /// </summary>
        /// <param name="designation">Designation that was changed</param>
        private void onDesignationFulfilledChanged(TerrainDesignation designation)
        {
            if (designation.Prototype.IsTerraforming && m_managedDesignations.Contains(designation) && designation.Prototype.ShouldUpdateTowerNotificationOnFulfilledChanged)
            {
                Assert.AssertTrue(isWithinArea(designation));
                updateTerrainNotifications();
            }

            if (designation.ProtoId == IdsCore.TerrainDesignators.MiningDesignator || designation.ProtoId == IdsCore.TerrainDesignators.LevelDesignator)
            {
                if (designation.IsMiningFulfilled)
                {
                    if (!m_unfulfilledMiningDesignations.Remove(designation))
                    {
                        Logger.Warning("{0} Designation was not in unfulfilled mining designations", nameof(onDesignationFulfilledChanged));
                    }
                }
                else
                {
                    m_unfulfilledMiningDesignations.AddAndAssertNew(designation);
                }
            }

            if (designation.ProtoId == IdsCore.TerrainDesignators.DumpingDesignator || designation.ProtoId == IdsCore.TerrainDesignators.LevelDesignator)
            {
                if (designation.IsDumpingFulfilled)
                {
                    if (!m_unfulfilledDumpingDesignations.Remove(designation))
                    {
                        Logger.Warning("{0} Designation was not in unfulfilled dumping designations", nameof(onDesignationFulfilledChanged));
                    }
                }
                else
                {
                    m_unfulfilledDumpingDesignations.AddAndAssertNew(designation);
                }
            }
        }

        /// <summary>
        /// Update <see cref="m_managedDesignations"/> when designations are removed within the Tower Area
        /// </summary>
        /// <param name="designation">TerrainDesignation provided</param>
        private void onDesignationRemoved(TerrainDesignation designation)
        {
            if (designation.Prototype.IsTerraforming && m_managedDesignations.Remove(designation))
            {
                if (designation.Equals(m_tmpMineDesignation)) { m_tmpMineDesignation = null; }
                if (designation.Equals(m_tmpDumpDesignation)) { m_tmpDumpDesignation = null; }

                m_unfulfilledMiningDesignations.Remove(designation);
                m_unfulfilledDumpingDesignations.Remove(designation);

                Assert.AssertTrue(isWithinArea(designation));
                designation.RemoveManagingTower(this);
                updateTerrainNotifications();
            }
        }

        /// <summary>
        /// Called on New Month to reset <see cref="TerrainTowerProductData"/> monthly values
        /// </summary>
        private void onNewMonth()
        {
            foreach (TerrainTowerProductData productData in m_productsData.Values) { productData.OnNewMonth(); }
        }

        /// <summary>
        /// Move residual products from <see cref="TerrainTowerProductData.SortedQuantity"/> to <see cref="TerrainTowerProductData.Buffer"/>
        /// </summary>
        private void pushSortedToBuffers()
        {
            m_sortedBufferIsPositive = false;
            bool HasStoredToBuffer = false;
            Assert.AssertTrue(m_productsData != null);
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
#if DEBUG
                Logger.InfoDebug("pushSortedToBuffers: Product {0} - UnsortedQuantity: {1} - SortedQuantity: {2} - Buffer: {3}",
                    productData.Buffer.Product,
                    productData.UnsortedQuantity,
                    productData.SortedQuantity,
                    productData.Buffer.Quantity);
#endif

                if (productData.SortedQuantity.IsNotPositive) { continue; }
                HasStoredToBuffer = true;
                Quantity quantity = productData.MoveSortedQuantityToBuffer();
                if (quantity.IsPositive) { m_productsManager.ProductCreated(productData.Buffer.Product, quantity, CreateReason.MinedFromTerrain); }
                if (productData.SortedQuantity.IsPositive) { m_sortedBufferIsPositive = true; }
            }
            //If Buffer has been added to, update Full Output Notifications
            if (HasStoredToBuffer) { updateFullOutputNotifications(); }
        }

        /// <summary>
        /// Clear all designations from <see cref="m_managedDesignations"/> controlled via <see cref="onDesignationRemoved"/>
        /// </summary>
        private void removeAllManagedDesignations()
        {
            //TODO: Test if LINQ is working
            m_managedDesignations.ForEach(designation => onDesignationRemoved(designation));
#if false
            foreach (TerrainDesignation designation in m_managedDesignations.ToArray())
            {
                onDesignationRemoved(designation);
            }
#endif
            Assert.AssertTrue(m_managedDesignations.IsEmpty);
        }

        /// <summary>
        /// Loop through all <see cref="LayoutEntity.ConnectedOutputPorts"/> and send matching products to the configured <see cref="TerrainTowerProductData.OutputPort"/>
        /// </summary>
        private void sendOutputs()
        {
            foreach (IoPortData oPort in ConnectedOutputPorts)
            {
                foreach (TerrainTowerProductData productData in m_productsData.Values)
                {
                    if (productData.OutputPort == oPort.Name)
                    {
                        GlobalOutputBuffer buffer = productData.Buffer;
                        Quantity bufferQuantity = buffer.Quantity;
                        Quantity sentQuantity = bufferQuantity - oPort.SendAsMuchAs(buffer.Product.WithQuantity(bufferQuantity));
                        if (sentQuantity.IsPositive)
                        {
                            buffer.RemoveExactly(sentQuantity);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called from <see cref="IEntityWithSimUpdate.SimUpdate"/>
        /// - Tries to Sort Products and Push to Buffer
        /// - If 'Finished' but no Sorting, don't re-start Timer to allow instant sorting next attempt.
        /// </summary>
        /// <returns>TRUE if Sorting Occured</returns>
        private bool simStepSorting()
        {
            //Sorted Products (Between Mining/Buffer) are Positive = Push to Buffer (Left over from last Timer.IsFinished)
            if (m_sortedBufferIsPositive) { pushSortedToBuffers(); }

            //We only sort if the Timer is Finished (Unsorted -> Sorted)
            if (m_sortingTimer.IsFinished)
            {
                if (sortProducts())
                {
                    //Products have been sorted, start the Timer
                    m_sortingTimer.Start(Prototype.SortDuration);
                    //Products have been sorted, push to Buffer (For output)
                    pushSortedToBuffers();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Called from <see cref="IEntityWithSimUpdate.SimUpdate"/>
        /// - Tries to Mine and (if Unable) Dump nearest Designation
        /// - Handled Differently than Sorting, only conducted per Timer, instead of holding a 'Finished' timer to start immediately
        /// </summary>
        /// <returns>TRUE if any Terrain Manipulation occured</returns>
        private bool simStepTerrainProcessing()
        {
            //FUTURE: Possible Changes - Allow 1x Mine & 1x Dump per round
            //FUTURE: Possible CHanges - Increase MAX_MINE or MAX_DUMP if Boosted
            if (!m_terrainTimer.IsFinished)
            {
                return false;
            }
#if DEBUG2
            Logger.InfoDebug("simStepTerrainProcessing: State {0} - DumpTotal {1}", TerrainConfigState.ToString(), DumpTotal);
#endif

            bool ActionThisRound = false;
            if (!ActionThisRound && TerrainConfigState.HasFlag(TerrainTowerConfigState.Mining) && ValidateMiningDesignation())
            {
                ActionThisRound = tryMineDesignation(m_tmpMineDesignation, MAX_MINE_QUANTITY);
            }

            if (!ActionThisRound && TerrainConfigState.HasFlag(TerrainTowerConfigState.Dumping) && DumpTotal.IsPositive && ValidateDumpingDesignation())
            {
                ActionThisRound = tryDumpDesignation(m_tmpDumpDesignation, MAX_DUMP_QUANTITY);
            }

            //Unity = 5x faster & No action = 2x slower.
            /*
                * Boost + Active = 1 second
                * Boost + Not Active = 2.5 seconds
                * NoBoost + Active = 5 seconds
                * NoBoost + Not Active = 10 Seconds
            */
            Duration duration = Prototype.TerrainDuration / (IsBoostRequested ? 5 : 1) * (ActionThisRound ? 1 : 2);
#if DEBUG2
            Logger.InfoDebug("TerrainProcessing: Timer Start: {0} seconds", duration.Seconds);
#endif
            m_terrainTimer.Start(duration);
            return ActionThisRound;

            //Separate designation validate functions to allow caching last used Designation - Prevents re-scanning entire list
            bool ValidateMiningDesignation()
            {
                if (m_tmpMineDesignation == null || m_tmpMineDesignation.IsMiningFulfilled)
                {
                    if (Functions.TryFindUnfulfilledDesignation_Mining(m_unfulfilledMiningDesignations, Position2f.Tile2i, out TerrainDesignation bestMineDesig))
                    {
#if DEBUG2
                        Logger.InfoDebug("ValidateMiningDesignation: New Mining Designation: {0}", bestMineDesig.ToStringSafe());
#endif
                        m_tmpMineDesignation = bestMineDesig;
                        Assert.AssertTrue(m_tmpMineDesignation.IsMiningNotFulfilled, "Mining Designation should be unfulfilled");
                    }
                    else
                    {
                        m_missingMiningDesignationNotif.Activate(this);
                        //m_missingMiningDesignationNotif.NotifyIff(m_tmpMineDesignation == null || m_tmpMineDesignation.IsMiningFulfilled, this);
                    }
                }
                return m_tmpMineDesignation != null && m_tmpMineDesignation.IsMiningNotFulfilled;
            }
            bool ValidateDumpingDesignation()
            {
                if (m_tmpDumpDesignation == null || m_tmpDumpDesignation.IsDumpingFulfilled)
                {
                    if (Functions.TryFindUnfulfilledDesignation_Dumping(m_unfulfilledDumpingDesignations, Position2f.Tile2i, out TerrainDesignation bestDumpDesig))
                    {
#if DEBUG2
                        Logger.InfoDebug("ValidateDumpingDesignation: New Mining Designation: {0}", bestDumpDesig.ToStringSafe());
#endif
                        m_tmpDumpDesignation = bestDumpDesig;
                        Assert.AssertTrue(m_tmpDumpDesignation.IsDumpingNotFulfilled, "Dumping Designation should be unfulfilled");
                    }
                    else
                    {
                        m_missingDumpingDesignationNotif.Activate(this);
                        //m_missingDumpingDesignationNotif.NotifyIff(m_tmpDumpDesignation == null || m_tmpDumpDesignation.IsDumpingFulfilled, this);
                    }
                }
                return m_tmpDumpDesignation != null && m_tmpDumpDesignation.IsDumpingNotFulfilled;
            }
        }

        /// <summary>
        /// Get the current State of the Tower based on current conditions
        /// </summary>
        /// <returns><see cref="State"/> to be assigned to <see cref="CurrentState"/></returns>
        private State simStepUpdateState()
        {
            //Not Enabled = No SimUpdate
            if (IsNotEnabled) { return !Maintenance.Status.IsBroken ? State.Paused : State.Broken; }

            //No Workers = No SimUpdate
            if (IsMissingWorkers(this)) { return State.MissingWorkers; }

            //No Electricity = No SimUpdate
            if (!m_electricityConsumer.CanConsume()) { return State.NotEnoughPower; }

            if (IsMissingDesignation) { return State.MissingDesignation; }

            //FUTURE: Could be removed and just flagged as a Notification, rather than a full status.
            if (TerrainConfigState == TerrainTowerConfigState.Dumping && DumpTotal.IsZero)
            {
                //Only set MissingDumpResource if we are in Dumping Mode (Not Flatten/Mining)
                return State.MissingDumpResource;
            }

            //We have Workers, Power, and are Enabled
            //We are not Missing Designation, or Dump Resource (if Dumping only)
            return State.Working;
        }

        /// <summary>
        /// Process Unsorted Products to Sorted Products and conduct Waste/Conversion Loss for configured products
        /// - Only sort if the 'Sorted Quantity' is Zero
        /// </summary>
        /// <returns>TRUE if any products were Sorted</returns>
        private bool sortProducts()
        {
            //Nothing to Sort
            if (MixedTotal.IsNotPositive) { return false; }

            Quantity sortedPerDuration = SortQuantityPerDuration;

            //Running subtotal of items sorted this round
            Quantity quantitySortedRunningTotal = Quantity.Zero;

            //Summary: Total products availble to be sorted
            Quantity quantityToSortTotal = Quantity.Zero;

#if DEBUG
            quantityToSortTotal = m_productsData.Values.Where(p => !p.SortedQuantity.IsPositive).Sum(p => p.UnsortedQuantity.Value).Quantity();
#else
            foreach (TerrainTowerProductData plantProductData in m_productsData.Values)
            {
                //Don't add UnsortedQuantity if SortedQuantity has product (This shows that the previous sort was not completed i.e. Buffer Full)
                if (!plantProductData.SortedQuantity.IsPositive) { quantityToSortTotal += plantProductData.UnsortedQuantity; }
            }
#endif

            foreach (TerrainTowerProductData plantProductData in m_productsData.Values)
            {
#if DEBUG
                Logger.InfoDebug("sortProducts: Product {0} - UnsortedQuantity: {1} - SortedQuantity: {2} - Buffer: {3} - RunningTotal: {4}",
                    plantProductData.Buffer.Product,
                    plantProductData.UnsortedQuantity,
                    plantProductData.SortedQuantity,
                    plantProductData.Buffer.Quantity,
                    quantitySortedRunningTotal);
#endif
                //If we have sorted all we can, break
                if ((quantitySortedRunningTotal >= sortedPerDuration))
                {
                    break;
                }

                //Quantity bufferQuantity = plantProductData.SortedQuantity; added to next IF statement
                if (plantProductData.SortedQuantity.IsNotPositive && plantProductData.UnsortedQuantity.IsPositive)
                {
                    Quantity tmpUnsortedQuantity = plantProductData.UnsortedQuantity;

                    //1. Scale UnsortedQuantity to even ratios with other products
                    tmpUnsortedQuantity = (plantProductData.UnsortedQuantity.Value / quantityToSortTotal.Value.ToFix32() * sortedPerDuration.Value).ToIntCeiled().Quantity();

                    tmpUnsortedQuantity = tmpUnsortedQuantity
                    //2. Min UnsortedQuantity to ensure it doesn't exceed actually available UnsortedQuantity
                        .Min(plantProductData.UnsortedQuantity)
                    //3. Cap to Sorted Products running total remaining value to ensure we're not sorting more than allowed - (Due to rounding, last product looped may have a 1/2 quantity skipped)
                        .Min(sortedPerDuration - quantitySortedRunningTotal);

                    //4. Move from Unsorted to Sorted buffers
                    plantProductData.SortQuantity(tmpUnsortedQuantity);

                    //5. Remove sorted value from mixed buffer - Enumerator prevention
                    MixedTotal -= tmpUnsortedQuantity;
                    updateMixedBufferNotifications();

                    //6. Rolling total of values sorted - after full loop = SortedPerDuration
                    quantitySortedRunningTotal += tmpUnsortedQuantity;

                    if (plantProductData.CanBeWasted)
                    {
                        //7. Add Conversion Loss to 'ToWaste' PartialQuantity buffer
                        plantProductData.ToWaste += tmpUnsortedQuantity.AsPartial.ScaledBy(Prototype.ConversionLoss);

                        //8. Round ToWaste PartialQuantity down to nearest whole number
                        Quantity wasteQuantity = plantProductData.ToWaste.IntegerPart.Min(plantProductData.SortedQuantity);
                        if (wasteQuantity.IsPositive)
                        {
                            //9. Destroy Waste
                            Context.ProductsManager.ProductDestroyed(plantProductData.Buffer.Product, wasteQuantity, DestroyReason.Wasted);

                            //10. Remove Waste from SortedQuantity
                            plantProductData.SortedQuantity -= wasteQuantity;

                            //11. Remove ConversionLoss from 'ToWaste' should be between 0.00 and 0.99
                            plantProductData.ToWaste -= wasteQuantity.AsPartial;
                        }
                    }
                }
            }
            return quantitySortedRunningTotal.IsPositive;
        }

        /// <summary>
        /// Tries to dump to the designation. Pulls directly from <see cref="m_dumpBuffers"/>
        /// </summary>
        /// <param name="designation">Designation to try to dump on</param>
        /// <param name="maxDumpQuantity">Max quantity to dump. Used as local variale to subtract from</param>
        /// <returns>TRUE if any dumping occured</returns>
        private bool tryDumpDesignation(TerrainDesignation designation, Quantity maxDumpQuantity)
        {
#if DEBUG2
            Logger.InfoDebug("tryDumpDesignation: Designation: {0} - maxDumpQuantity: {1} - DumpTotal: {2}", designation.ToStringSafe(), maxDumpQuantity, DumpTotal);
#endif

            if (designation == null || designation.IsDumpingFulfilled || maxDumpQuantity.IsNotPositive || DumpTotal.IsNotPositive)
            {
                //Logger.InfoDebug("tryDumpDesignation: IsDumpingFulfilled {0} - maxDumpQuantity {1} - DumpTotal {2}", designation.IsDumpingFulfilled, maxDumpQuantity, DumpTotal);
                //Invalid Designation, already fulfilled, or no quantity to dump or DumpTotal is empty
                m_missingDumpItemNotif.NotifyIff(DumpTotal.IsNotPositive, this);
                return false;
            }

            bool Action = false;
            /*
            Changed from Dumping based on Quantity to Thickness to allow smaller Partial Quantities to be dumped without rounding issues.
            1. Get Buffer Quanity to Thickness
            2. Dump based on Thickness (This allows partial/small values without rounding issues of Quantity vs PartialQuantity)
            3. Sum all dumped Thickness
            4. Convert Thickness Dumped to Quantity, and remove from Buffer
            */

            //First Buffer with value
            IProductBuffer buff = DumpBuffers.First(x => x.Quantity.IsPositive);

            // Min (Dump Buffer | maxDumpQuantity)
            LooseProductQuantity looseProductQuantity = new LooseProductQuantity(buff.Product.DumpableProduct.Value, maxDumpQuantity.Min(buff.Quantity));

#if DEBUG2
            Logger.InfoDebug("tryDumpDesignation: Dumping {0} - {1}", looseProductQuantity.Product.ToString(), looseProductQuantity.Quantity);
#endif
            //Convert to Thickness (Total thickness dumpable during this process)
            ThicknessTilesF dumpTotalThickness = looseProductQuantity.ToTerrainThickness().Thickness;

            //FUTURE: Check if we can change to ForEachNonFulfilledTile - Test with Mining
            //Loop through 5x5 area around Designation (1 Designation = 4x4 = 25 index points) - This includes the borders
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (dumpTotalThickness.IsNotPositive)
                    {
                        //No thickness left to Dump
                        break;
                    }

                    //Get Tile and Index for TerrainManager
                    Tile2i tile = designation.Data.OriginTile + new RelTile2i(i, j);
                    Tile2iAndIndex tileAndIndex = tile.ExtendIndex(ContextTerrainManager);

                    HeightTilesF targetHeight = designation.GetTargetHeightAt(tile);
                    HeightTilesF terrainHeight = ContextTerrainManager.GetHeight(tile);

                    if (designation.IsDumpingFulfilledAt(tile) || terrainHeight >= targetHeight)
                    {
                        // No Dumping, or no quantity to dump, or terrain is already at or above target terrainHeight
                        continue;
                    }

                    //Terrain Difference & Max Dumpable Thickness & Max Terrain Modification Thickness
                    ThicknessTilesF dumpTileThickness = (targetHeight - terrainHeight).Min(dumpTotalThickness).Min(MAX_TERRAIN_MOD_THICKNESS);

                    //Dump Product to Terrain
                    ContextTerrainManager.DumpMaterial(tileAndIndex, new TerrainMaterialThicknessSlim(looseProductQuantity.Product.TerrainMaterial.Value.SlimId, dumpTileThickness));

                    dumpTotalThickness -= dumpTileThickness;

                    Action = true;
                } // J Loop
            } // I Loop

#if DEBUG2
            Logger.InfoDebug("tryDumpDesignation: baseThickness {0} - leftoverThickness {1}", looseProductQuantity.ToTerrainThickness().Thickness, dumpTotalThickness);
#endif

            ProductQuantity totalDumped = new TerrainMaterialThickness(looseProductQuantity.Product.TerrainMaterial.Value, dumpTotalThickness).ToProductQuantityRounded();

            if ((buff.Quantity - totalDumped.Quantity) <= Quantity.One)
            {
                //Insignificant quantity, unable to convert to TerrainMaterialThickness for processing. Add to totalDumped and remove from buffer
                //Don't directly reference buff.ProductQuantity, it just wipes the entire DumpTotal
                totalDumped = new ProductQuantity(buff.Product, buff.Quantity);
            }

            buff.RemoveExactly(totalDumped.Quantity);
            DumpTotal -= totalDumped.Quantity;
            Context.ProductsManager.ProductDestroyed(totalDumped, DestroyReason.DumpedOnTerrain);

#if DEBUG2
            Logger.InfoDebug("trympDesignation: Total Dumped: {0}", totalDumped.Quantity);
#endif
            return Action;
        }

        /// <summary>
        /// Tries to mine a designation. Adds directly to <see cref="m_productsData"/>
        /// </summary>
        /// <param name="designation">Designation to try to mine on</param>
        /// <param name="maxDumpQuantity">Max quantity to mine. Used as local variale to subtract from to prevent over work</param>
        /// <returns>TRUE if any mmining occured</returns>
        private bool tryMineDesignation(TerrainDesignation designation, Quantity maxMineQuantity)
        {
#if DEBUG2
            Logger.InfoDebug("tryMineDesignation: Designation: {0} - maxMineQuantity: {1}", designation.ToStringSafe(), maxMineQuantity);
#endif
            //TODO:  MixedTotal >= Capacity OR during tile Loop, MixedCapacityLeft <= maxMineQuantity
            if (designation == null || designation.IsMiningFulfilled || maxMineQuantity.IsNotPositive || MixedTotal >= Capacity)
            {
                //Invalid Designation, already fulfilled, or no quantity to min
                return false;
            }

            bool hasMined = false;

            for (int i = 0; i <= designation.SizeTiles; i++)
            {
                for (int j = 0; j <= designation.SizeTiles; j++)
                {
                    Tile2i tile = designation.Data.OriginTile + new RelTile2i(j, i);

                    //Not allowed to mine, exit function
                    if (maxMineQuantity.IsNotPositive) { return hasMined; }

                    //Nothing to mine at tile, skip loop
                    if (designation.IsMiningFulfilledAt(tile)) { continue; }

                    HeightTilesF targetHeight = designation.GetTargetHeightAt(tile);
                    HeightTilesF terrainHeight = ContextTerrainManager.GetHeight(tile);
                    Tile2iAndIndex tileAndIndex = tile.ExtendIndex(ContextTerrainManager);

                    //Clear Custom Surface if it exists and is above target terrainHeight
                    //Process before terrain Height Check to allow clearing of Props
                    //Uncleared Props flag designation as Unfulfilled
                    clearCustomSurface(tileAndIndex, targetHeight);
                    clearProps(tile);

                    // Terrain is already at or below target terrainHeight, skip loop
                    if (terrainHeight <= targetHeight) { continue; }

                    hasMined |= tryMineDesignation_MineLayer(tileAndIndex, targetHeight, ref maxMineQuantity, true);
                    hasMined |= tryMineDesignation_MineLayer(tileAndIndex, targetHeight, ref maxMineQuantity, false);
                }
            }
            return hasMined;
        }

        /// <summary>
        /// Mine Layer at a specific TileAndIndex - Called from <see cref="tryMineDesignation"/>
        /// </summary>
        /// <param name="tileAndIndex">Location to Mine</param>
        /// <param name="targetHeight">Height to Mine to</param>
        /// <param name="maxMineQuantity">Ref value of Max Mined Quantity per SimUpdate</param>
        /// <param name="layer">TRUE if FIRST Layer else SECOND Layer</param>
        /// <returns>TRUE if Mining has occured</returns>
        private bool tryMineDesignation_MineLayer(Tile2iAndIndex tileAndIndex, HeightTilesF targetHeight, ref Quantity maxMineQuantity, bool firstLayer)
        {
            //Not enough layers to mine
            if (ContextTerrainManager.GetLayersCountNoBedrock(tileAndIndex.Index) < (firstLayer ? 1 : 2))
            {
#if DEBUG2
                Logger.InfoDebug("tryMineDesignation_MineLayer: Insufficient Layers - Layer {0}", firstLayer ? 1 : 2);
#endif
                return false;
            }

#if DEBUG2
            Logger.InfoDebug("tryMineDesignation_MineLayer: Mining Layer {0} at {1}", firstLayer ? 1 : 2, tileAndIndex.ToString());
#endif

            TerrainMaterialThickness terrainLayer = firstLayer
                ? ContextTerrainManager.GetFirstLayer(tileAndIndex.Index)
                : ContextTerrainManager.GetSecondLayer(tileAndIndex.Index);

            //Not allowed to mine this product
            //HACK: Allow ALL products to be mined without consideration to type/count etc
            //!(AllSupportedProducts.Contains(terrainLayer.Material.MinedProduct) || !AllowedProducts.Contains(terrainLayer.Material.MinedProduct))
            //AllowedProducts = m_productsData.Keys
            if (!(AllSupportedProducts.Contains(terrainLayer.Material.MinedProduct) || AllowedProducts.Contains(terrainLayer.Material.MinedProduct)))
            {
#if DEBUG2
                Logger.InfoDebug("tryMineDesignation_MineLayer: Not allowed to mine {0}", terrainLayer.Material.MinedProduct.ToStringSafe());
#endif
                return false;
            }

            ThicknessTilesF maxThickness = Functions.MinThickness(
                MAX_TERRAIN_MOD_THICKNESS,
                ContextTerrainManager.GetHeight(tileAndIndex.Index) - targetHeight,
                terrainLayer.Material.QuantityToThickness(maxMineQuantity));

            //Not thick enough to mine
            //TODO: Possible issue with rounding down to 0 thickness leaving tiny clumps and designation being considered 'Complete'
            if (maxThickness.IsNotPositive)
            {
#if DEBUG2
                Logger.InfoDebug("tryMineDesignation_MineLayer: Not thick enough to mine - {0}", maxThickness);
#endif

                return false;
            }

            TerrainMaterialThicknessSlim miningOutput = firstLayer
                ? ContextTerrainManager.MineMaterial(tileAndIndex, maxThickness)
                : ContextTerrainManager.MineMaterialFromSecondLayer(tileAndIndex, maxThickness);

            //FUTURE: Check if we need maxThickness or MAX_TERRAIN_MOD_THICKNESS
            ContextTerrainManager.DisruptExactly(tileAndIndex, maxThickness);

            //No Mining Output
            if (miningOutput.Thickness.IsZero)
            {
#if DEBUG2
                Logger.InfoDebug("tryMineDesignation_MineLayer: 0 Thickness Mined");
#endif
                return false;
            }

            PartialProductQuantity ppq = miningOutput.ToPartialProductQuantity(ContextTerrainManager);
            //TODO: Some rounding loss, Change TerrainTowerProductData to PartialQuantity???
            Quantity quantityMined = ppq.Quantity.IntegerPart;

#if DEBUG2
            Logger.InfoDebug("tryMineDesignation_MineLayer: Mined {0} of {1}", quantityMined, ppq.Product.ToStringSafe());
#endif
            if (!m_productsData.ContainsKey(ppq.Product))
            {
                addProductDataFor(ppq.Product);
            }
            m_productsData[ppq.Product].UnsortedQuantity += quantityMined;
            MixedTotal += quantityMined;
            updateMixedBufferNotifications();
            maxMineQuantity -= quantityMined;

            return true;
        }

        private void updateFullOutputNotifications()
        {
            //Blocked Output notification - even when not working
            bool shouldNotify = false;
            if (IsEnabled && m_sortedBufferIsPositive)
            {
#if DEBUG2
                Logger.InfoDebug("TerrainTowerEntity.SimUpdate: Temp Buffers Full");
#endif
                foreach (TerrainTowerProductData productData in m_productsData.Values)
                {
                    if (productData.NotifyIfFullOutput && productData.Buffer.IsFull)
                    {
                        shouldNotify = true;
                        break;
                    }
                }
            }
            m_outputBlockedNotif.NotifyIff(shouldNotify, this);
        }

        private void updateMixedBufferNotifications()
        {
            m_inputBlockedNotif.NotifyIff(IsEnabled && MixedTotal >= (Capacity - MAX_MINE_QUANTITY), this);
        }

        /// <summary>
        /// Update the Mixed Quantity from all <see cref="m_productsData"/>
        /// </summary>
        private void updateMixedQuantity()
        {
            Quantity val = Quantity.Zero;
            foreach (TerrainTowerProductData productData in m_productsData.Values)
            {
                val += productData.UnsortedQuantity;
            }
            MixedTotal = val;
            updateMixedBufferNotifications();
        }

        /// <summary>
        /// Activate/Deactivate Notifications. Trigged on Add/Remove/Fulfilled <see cref="ManagedDesignations"/>
        /// </summary>
        private void updateTerrainNotifications()
        {
            //Set unfulfilled flags
            HasUnfulfilledMining = m_managedDesignations.Any(desig => desig.IsMiningNotFulfilled);
            HasUnfulfilledDumping = m_managedDesignations.Any(desig => desig.IsDumpingNotFulfilled);

            //Update Notifications, if Flag is Set && has NotFulfilled Designations
            m_missingMiningDesignationNotif.NotifyIff(TerrainConfigState.HasFlag(TerrainTowerConfigState.Mining)
                && !HasUnfulfilledMining, this);
            m_missingDumpingDesignationNotif.NotifyIff(TerrainConfigState.HasFlag(TerrainTowerConfigState.Dumping)
                && !HasUnfulfilledDumping, this);

            //If Dumping, must have > 0 Quantity. Alternative option: must have at least MAX_DUMP_QUANTITY in a buffer
            //TODO: Remove? - It's set within dumpDesignation which has far more validation/checks, just in case the tower is set to Flatten, but we don't actually have any Dump Designations.
            //m_missingDumpItemNotif.NotifyIff(TerrainConfigState.HasFlag(TerrainTowerConfigState.Dumping) && DumpTotal.IsZero, this);
        }
    }

    public sealed partial class TerrainTowerEntity
    {
        public enum State
        {
            Paused,
            Broken,
            MissingWorkers,
            MissingDumpResource,
            NotEnoughPower,
            MissingDesignation,
            Working
        }

        [Flags]
        public enum TerrainTowerConfigState
        {
            None = 0,
            Mining = 1 << 0,
            Dumping = 1 << 1,
            Flatten = Mining | Dumping
        }
    }
}