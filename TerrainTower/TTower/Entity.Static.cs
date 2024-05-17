using Mafi;
using Mafi.Serialization;

using System;

namespace TerrainTower.TTower
{
    public sealed partial class TerrainTowerEntity
    {
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
    }
}