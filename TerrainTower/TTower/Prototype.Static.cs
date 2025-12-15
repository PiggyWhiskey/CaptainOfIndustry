using Mafi;
using Mafi.Base;
using Mafi.Core.Prototypes;

namespace TerrainTower.TTower
{
    public partial class TerrainTowerProto
    {
        public static readonly Upoints BOOST_COST;
        public static readonly Quantity BUFFER_CAPACITY;
        public static readonly Percent CONVERSION_LOSS;
        public static readonly string CUSTOM_ICON_PATH;
        public static readonly MineArea DEFAULT_AREA;
        public static readonly Electricity ELECTRICITY_COST;
        public static readonly EntityCostsTpl ENTITY_COSTS;
        public static readonly string[] LAYOUT;
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
        public static readonly string PREFAB_PATH;
        public static readonly int RESEARCH_DIFFICULTY;
        public static readonly Duration SORT_DURATION;
        public static readonly Quantity SORT_QUANTITY_PER_DURATION;
        public static readonly Duration TERRAIN_DURATION;

        static TerrainTowerProto()
        {
#if DEBUG
            BOOST_COST = 1.0.Upoints();
            ELECTRICITY_COST = 0.Kw();
            ENTITY_COSTS = new EntityCostsTpl.Builder().CP(1);
            RESEARCH_DIFFICULTY = 1;

            SORT_DURATION = 2.Seconds();
            TERRAIN_DURATION = 2.Seconds();

            MAX_TERRAIN_MOD_THICKNESS = 2.0.TilesThick();
            MAX_TOP_LAYER_FOR_MINING_BELOW = 0.5.TilesThick();
#else
            BOOST_COST = 1.0.Upoints();
            ELECTRICITY_COST = 200.Kw();
            ENTITY_COSTS = new EntityCostsTpl.Builder().CP(10).MaintenanceT1(20).Workers(10);
            RESEARCH_DIFFICULTY = 20;

            SORT_DURATION = 5.Seconds();
            TERRAIN_DURATION = 5.Seconds();

            MAX_TERRAIN_MOD_THICKNESS = 1.0.TilesThick();
            MAX_TOP_LAYER_FOR_MINING_BELOW = 0.5.TilesThick();
#endif

            CONVERSION_LOSS = Percent.FromRatio(25, 100);

            DEFAULT_AREA = new MineArea(new RelTile2i(17, 4), new RelTile2i(60, 60), new RelTile1i(int.MaxValue / 2));

            BUFFER_CAPACITY = 2500.Quantity();
            MAX_DUMP_QUANTITY = 500.Quantity();
            MAX_MINE_QUANTITY = 500.Quantity();
            SORT_QUANTITY_PER_DURATION = 100.Quantity();

            MAX_OUTPUTS = 8;
            MAX_PRODUCTS = 10;

            CUSTOM_ICON_PATH = "Assets/MiningDumpingMod/Building256.png";

#if false
            PREFAB_PATH = "Assets/MiningDumpingMod/MDControlBuilding2.prefab";
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
#else
            PREFAB_PATH = "Assets/TerrainTower/TerrainTower.prefab";
            LAYOUT = new string[16]
            {
            "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
            "<~O[4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]U~>",
            "   [4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]   ",
            "<~P[4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]V~>",
            "   [4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]   ",
            "   [4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]   ",
            "<~Q[4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]W~>",
            "   [4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]   ",
            "<~R[4][7][9][9][9][9][6][6][6][9][9][9][9][7][4]X~>",
            "   [4][7][9][9][9][9][9][9][9][9][9][9][9][7][4]   ",
            "   [4][7][9][9][9][9][9][9][9][9][9][9][9][7][4]   ",
            "<~S[4][7][9][9][9][9][9][9][9][9][9][9][9][7][4]Y~>",
            "   [4][7][9][9][9][9][9][9][9][9][9][9][9][7][4]   ",
            "<~T[4][7][7][7][7][7][7][7][7][7][7][7][7][7][4]Z~>",
            "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ",
            "      ^~A   ^~B      ^~C   ^~D      ^~E   ^~F      "
            };
#endif
        }
    }
}