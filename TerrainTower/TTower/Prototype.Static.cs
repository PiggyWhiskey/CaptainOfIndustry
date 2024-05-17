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
#else
            BOOST_COST = 5.0.Upoints();
            ELECTRICITY_COST = 400.Kw();
            ENTITY_COSTS = new EntityCostsTpl.Builder().CP(10).MaintenanceT1(20).Workers(10);
            RESEARCH_DIFFICULTY = 20;
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
    }
}