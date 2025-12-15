using Mafi;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;

using System;

namespace TerrainTower.TTower
{
    public partial class TerrainTowerProto : LayoutEntityProto, IProto, IProtoWithPowerConsumption, IProtoWithUnityConsumption
    {
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
                Assert.That(initialSize.X).IsPositive();
                Assert.That(initialSize.Y).IsPositive();
                Origin = origin;
                InitialSize = initialSize;
                MaxAreaEdgeSize = maxAreaEdgeSize;
            }
        }
    }
}