using System;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Enums and Flags for Terrain Tower
    /// </summary>
    public sealed partial class TerrainTowerEntity
    {
        public enum State
        {
            Paused,
            Broken,
            MissingWorkers,
            CantProcessTerrain,
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