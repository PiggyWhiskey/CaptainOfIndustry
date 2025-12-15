using System;
using System.Collections.Generic;
using System.Linq;

using Mafi;
using Mafi.Core;
using Mafi.Core.Terrain.Designation;
using Mafi.Unity;

namespace TerrainTower.Extras
{
    internal static class Functions
    {
        private const int m_weight_distance = 10;
        private const int m_weight_height = 10;
        private const int m_weight_random = 1;

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

        public static ThicknessTilesF MinThickness(params ThicknessTilesF[] thicks)
        {
            ThicknessTilesF result = ThicknessTilesF.MaxValue;

            foreach (ThicknessTilesF thick in thicks)
            {
                result = result.Min(thick);
            }
            return result;
        }

        public static bool TryFindUnfulfilledDesignation(
            IEnumerable<TerrainDesignation> designations,
            Tile2i origin,
            Random random,
            out TerrainDesignation bestDesignation)
        {
            //Best value (Default MaxValue to ensure that any valid designation will be better)
            Fix32 bestScore = Fix32.MaxValue;
            //Best designation (Null to ensure that any valid designation will be better)
            bestDesignation = null;

            foreach (TerrainDesignation desig in designations.Where(d => !d.IsMiningFulfilled || !d.IsDumpingFulfilled))
            {
                Assert.AssertTrue(desig != null, $"{nameof(TryFindUnfulfilledDesignation)} was given a null designation, skipping");
                Fix32 tmpScore = scoringFunction(desig, origin, random);

                if (tmpScore < bestScore)
                {
                    //Update Best value/designation
                    bestScore = tmpScore;
                    bestDesignation = desig;
                }
            }
#if DEBUG
            Logger.Info($"{nameof(TryFindUnfulfilledDesignation)}: BestScore {bestScore}");
            Logger.Info($"{nameof(TryFindUnfulfilledDesignation)}: MiningFulfilled {bestDesignation.IsMiningFulfilled}: MiningReady {bestDesignation.IsMiningReadyToBeFulfilled} ");
            Logger.Info($"{nameof(TryFindUnfulfilledDesignation)}: DumpingFulfilled {bestDesignation.IsDumpingFulfilled}: DumpingReady {bestDesignation.IsDumpingReadyToBeFulfilled} ");
            Logger.Info($"{nameof(TryFindUnfulfilledDesignation)}: Fulfilled {bestDesignation.IsFulfilled}: Ready {bestDesignation.IsMiningReadyToBeFulfilled} ");
#endif

            return bestDesignation != null;

            /// Scoring function uses CONST weights to calculate a score for the designation.
            Fix32 scoringFunction(TerrainDesignation p_desig, Tile2i p_origin, Random rng)
            {
                return (m_weight_height > 0 ? Height() * m_weight_height : Fix32.Zero)
                + (m_weight_random > 0 ? Random() * m_weight_random : Fix32.Zero)
                + (m_weight_distance > 0 ? Distance() * m_weight_distance : Fix32.Zero);
                Fix32 Height()
                {
                    return (p_desig.Manager.Value.TerrainManager[p_desig.OriginTileCoord].Height.ToUnityUnits().CeilToInt() - p_desig.MaxTargetHeight.ToUnityUnits()).Abs();
                }
                Fix32 Random()
                {
                    return rng.Next(1, 101);
                }
                Fix32 Distance()
                {
                    return p_desig.OriginTileCoord.DistanceTo(p_origin);
                }
            }
        }
    }
}