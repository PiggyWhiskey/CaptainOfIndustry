using Mafi;
using Mafi.Base;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;

using System;

namespace TerrainTower.TTower
{
    public class TerrainTowerProto : LayoutEntityProto, IProto, IProtoWithPowerConsumption, IProtoWithUnityConsumption
    {
        #region STATIC CLASS INFORMATION

        public static readonly Upoints BOOST_COST;
        public static readonly Quantity BUFFER_CAPACITY;
        public static readonly Percent CONVERSION_LOSS;
        public static readonly string CUSTOM_ICON_PATH;
        public static readonly MineArea DEFAULT_AREA;
        public static readonly Electricity ELECTRICITY_COST;
        public static readonly EntityCostsTpl ENTITY_COSTS;
        public static readonly string[] LAYOUT;
        public static readonly string PREFAB_PATH;
        public static readonly Duration SORT_DURATION;
        public static readonly Quantity SORT_QUANTITY_PER_DURATION;
        public static readonly Duration TERRAIN_DURATION;

        static TerrainTowerProto()
        {
#if DEBUG
            BOOST_COST = 1.0.Upoints();
            ELECTRICITY_COST = 0.Kw();
            ENTITY_COSTS = new EntityCostsTpl.Builder().CP(1);
#else
            BOOST_COST = 5.0.Upoints();
            ELECTRICITY_COST = 400.Kw();
            ENTITY_COSTS = new EntityCostsTpl.Builder().CP(10).MaintenanceT1(20).Workers(10);
#endif

            BUFFER_CAPACITY = 1000.Quantity();
            CONVERSION_LOSS = Percent.Zero;
            DEFAULT_AREA = new MineArea(new RelTile2i(17, 4), new RelTile2i(60, 60), new RelTile1i(int.MaxValue));
            SORT_DURATION = 5.Seconds();
            SORT_QUANTITY_PER_DURATION = 500.Quantity();
            TERRAIN_DURATION = 5.Seconds();

            CUSTOM_ICON_PATH = "Assets/Prefabs/building3.png";
            PREFAB_PATH = "Assets/Prefabs/MDControlBuilding2.prefab";

            LAYOUT = new string[10]
            {
                "      ^~A            ^~B            ^~C            ",
                "   [8][8][8][8][8][8][8][8][8][8][8][8][8][8][8]   ",
                "   [8][8][9][9][9][9][9][8][9][9][9][9][9][8][8]   ",
                "Y~>[8][8][9][9][9][9][9][8][9][9][9][9][9][8][8]>~D",
                "   [8][8][9][9][9][9][9][8][9][9][9][9][9][8][8]   ",
                "   [8][8][9][9][9][9][9][8][9][9][9][9][9][8][8]   ",
                "Z~>[8][8][9][9][9][9][9][8][9][9][9][9][9][8][8]>~E",
                "   [8][8][9][9][9][9][9][8][9][9][9][9][9][8][8]   ",
                "   [8][8][8][8][8][8][8][8][8][8][8][8][8][8][8]   ",
                "      v~H            v~G            v~F            "
            };
        }

        #endregion STATIC CLASS INFORMATION

        public readonly MineArea Area;

        public TerrainTowerProto(
                ID id,
                Str strings,
                EntityLayout layout,
                EntityCosts costs,
                Quantity bufferCapacity,
                Duration sortDuration,
                Quantity sortQuantityPerDuration,
                Duration terrainDuration,
                Percent conversionLoss,
                Electricity electricityConsumed,
                MineArea area,
                Gfx graphics,
                Upoints boostCost
            ) : base(
                id: id,
                strings: strings,
                layout: layout,
                costs: costs,
                graphics: graphics,
                boostCost: boostCost
            )
        {
            BufferCapacity = bufferCapacity;
            SortDuration = sortDuration;
            SortQuantityPerDuration = sortQuantityPerDuration;
            TerrainDuration = terrainDuration;
            ConversionLoss = conversionLoss;
            ElectricityConsumed = electricityConsumed;
            Area = area;
        }

        public Quantity BufferCapacity { get; }
        public Percent ConversionLoss { get; }
        public Electricity ElectricityConsumed { get; }
        public override Type EntityType => typeof(TerrainTowerEntity);
        public Duration SortDuration { get; }
        public Quantity SortQuantityPerDuration { get; }
        public Duration TerrainDuration { get; }
        public Upoints UnityMonthlyCost { get; }

        /// <summary>
        /// Holds the configuration of the initial mining area of a <see cref="TerrainTowerProto" />.
        /// </summary>
        public readonly struct MineArea
        {
            /// <summary>
            /// Initial size of the area before player makes any changes.
            /// </summary>
            public readonly RelTile2i InitialSize;

            /// <summary>
            /// Maximal allowed area size (for both the width and length).
            /// </summary>
            public readonly RelTile1i MaxAreaEdgeSize;

            /// <summary>
            /// Denotes the point (relative to the entity layout) where the area starts. Please see the example.
            /// </summary>
            /// <example>Below is an example of such area where size is (4,6). X denotes the origin point.</example>
            /// <code>
            /// halfWidth (2)
            /// ▲
            /// ******
            /// *    *
            /// TT   X    * ► length (6)
            /// *    *
            /// ******
            /// ▼
            /// halfWidth (2)
            /// </code>
            public readonly RelTile2i Origin;

            public MineArea(RelTile2i origin, RelTile2i initialSize, RelTile1i maxAreaEdgeSize)
            {
                Origin = origin;
                InitialSize = initialSize;
                MaxAreaEdgeSize = maxAreaEdgeSize;
            }
        }
    }
}