using Mafi;
using Mafi.Core;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Unity;

using System;
using System.Collections.Generic;

using TerrainTower.TTower;

namespace TerrainTower.Extras
{
    internal class Functions
    {
        public static readonly Predicate<TerrainDesignation> isDumpingAvailable = d =>
        {
            return (d.ProtoId == IdsCore.TerrainDesignators.DumpingDesignator || d.ProtoId == IdsCore.TerrainDesignators.LevelDesignator) && d.IsDumpingNotFulfilled;
        };

        public static readonly Predicate<TerrainDesignation> isLevellingAvailable = d =>
        {
            return (d.ProtoId == IdsCore.TerrainDesignators.LevelDesignator || d.ProtoId == IdsCore.TerrainDesignators.MiningDesignator || d.ProtoId == IdsCore.TerrainDesignators.LevelDesignator) && (d.IsNotFulfilled || d.IsMiningNotFulfilled || d.IsDumpingNotFulfilled);
        };

        public static readonly Predicate<TerrainDesignation> isMiningAvailable = d =>
        {
            return (d.ProtoId == IdsCore.TerrainDesignators.MiningDesignator || d.ProtoId == IdsCore.TerrainDesignators.LevelDesignator) && d.IsMiningNotFulfilled;
        };

        private enum ScoringFn
        {
            Close,
            Random,
            Distance
        };

        internal static ThicknessTilesF MinThickness(params ThicknessTilesF[] thicks)
        {
            ThicknessTilesF result = ThicknessTilesF.MaxValue;

            foreach (ThicknessTilesF thick in thicks)
            {
                result = result.Min(thick);
            }
            return result;
        }

        internal static bool TryFindUnfulfilledDesignation_Dumping(IEnumerable<TerrainDesignation> designations, Tile2i origin, out TerrainDesignation bestDesignation)
        {
            return tryFindUnfulfilledDesignation(
                designations,
                origin,
                out bestDesignation,
                TerrainTowerEntity.TerrainTowerConfigState.Dumping);
        }

        internal static bool TryFindUnfulfilledDesignation_Mining(IEnumerable<TerrainDesignation> designations, Tile2i origin, out TerrainDesignation bestDesignation)
        {
            return tryFindUnfulfilledDesignation(
                designations,
                origin,
                out bestDesignation,
                TerrainTowerEntity.TerrainTowerConfigState.Mining);
        }

        private static bool tryFindUnfulfilledDesignation(
            IEnumerable<TerrainDesignation> designations,
            Tile2i origin,
            out TerrainDesignation bestDesignation,
            TerrainTowerEntity.TerrainTowerConfigState config)
        {
            //FUTURE: Possible make a class handling the functions, and the ScoringFunction variable - Unsure if this would save any performance
            //Best value
            Fix32 bestScore = Fix32.MaxValue;
            //Best designation
            bestDesignation = null;

            //Distance Function (Close or RNG are alternatives)
            ScoringFn scoringFunctionMode = ScoringFn.Distance;

            Random random = null;
            TerrainManager terrainManager = null;
            Func<TerrainDesignation, Fix32> scoringFunction = null;

            switch (scoringFunctionMode)
            {
                case ScoringFn.Close:
                    scoringFunction = new Func<TerrainDesignation, Fix32>(scoringFunction_Close);
                    break;

                case ScoringFn.Random:
                    random = new Random();
                    scoringFunction = new Func<TerrainDesignation, Fix32>(scoringFunction_Random);
                    break;

                case ScoringFn.Distance:
                default:
                    scoringFunction = new Func<TerrainDesignation, Fix32>(scoringFunction_Distance);
                    break;
            }
            if (!config.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Dumping) && !config.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Mining))
            {
                return false;
            }

            foreach (TerrainDesignation desig in designations)
            {
                if (desig == null)
                {
                    Log.Warning("tryFindBestDesignation was given a null designation, skipping");
                    continue;
                }

                //Used with scoringFunction_Close
                if (scoringFunctionMode == ScoringFn.Close && terrainManager == null)
                {
                    terrainManager = desig.Manager.Value.TerrainManager;
                }

                Fix32 tmpScore;
                tmpScore = (config.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Dumping) && isDumpingAvailable(desig))
                    || (config.HasFlag(TerrainTowerEntity.TerrainTowerConfigState.Mining) && isMiningAvailable(desig))
                    ? scoringFunction(desig)
                    : Fix32.MaxValue;

                if ((tmpScore != Fix32.MaxValue) && (tmpScore < bestScore))
                {
                    //Update Best value/designation
                    bestScore = tmpScore;
                    bestDesignation = desig;
                }
            }
#if DEBUG2
            Logger.InfoDebug("FindDesignation: State {0}, bestScore {1}", config.ToStringSafe(), bestScore);
#endif
            return bestDesignation != null;
            Fix32 scoringFunction_Close(TerrainDesignation desig)
            {
                return (terrainManager[desig.OriginTileCoord].Height.ToUnityUnits().CeilToInt() - desig.MaxTargetHeight.ToUnityUnits()).Abs();
            }
            Fix32 scoringFunction_Random(TerrainDesignation desig)
            {
                return random.Next(1, 101);
            }
            Fix32 scoringFunction_Distance(TerrainDesignation desig)
            {
                return desig.OriginTileCoord.DistanceTo(origin);
            }
        }
    }
}