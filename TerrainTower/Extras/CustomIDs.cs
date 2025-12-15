using Mafi.Base;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Research;

namespace TerrainTower.Extras
{
    internal static class CustomIds
    {
        internal static class Notifications
        {
            public static readonly EntityNotificationProto.ID TerrainTowerBlockedOuput = new EntityNotificationProto.ID(nameof(TerrainTowerBlockedOuput));
            public static readonly EntityNotificationProto.ID TerrainTowerFullMixedBuffer = new EntityNotificationProto.ID(nameof(TerrainTowerFullMixedBuffer));
            public static readonly EntityNotificationProto.ID TerrainTowerMissingDesignation = new EntityNotificationProto.ID(nameof(TerrainTowerMissingDesignation));
            public static readonly EntityNotificationProto.ID TerrainTowerMissingDumpItem = new EntityNotificationProto.ID(nameof(TerrainTowerMissingDumpItem));
        }

        internal static class Prototypes
        {
            public static readonly StaticEntityProto.ID TerrainTowerProtoId = new StaticEntityProto.ID(nameof(TerrainTowerProtoId));
        }

        internal static class Research
        {
            public static readonly ResearchNodeProto.ID TerrainTowerResearchId = Ids.Research.CreateId(nameof(TerrainTowerResearchId));
        }
    }
}