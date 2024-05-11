using Mafi.Base;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Research;

namespace TerrainTower.Extras.CustomIds
{
    internal static class Notifications
    {
        public static readonly EntityNotificationProto.ID TerrainTowerBlockedOuput = new EntityNotificationProto.ID(nameof(TerrainTowerBlockedOuput));
        public static readonly EntityNotificationProto.ID TerrainTowerMissingDumpItem = new EntityNotificationProto.ID(nameof(TerrainTowerMissingDumpItem));
        public static readonly EntityNotificationProto.ID TerrainTowerMissingDumpDesignation = new EntityNotificationProto.ID(nameof(TerrainTowerMissingDumpDesignation));
        public static readonly EntityNotificationProto.ID TerrainTowerMissingMineDesignation = new EntityNotificationProto.ID(nameof(TerrainTowerMissingMineDesignation));
        public static readonly EntityNotificationProto.ID TerrainTowerFullMixedBuffer = new EntityNotificationProto.ID(nameof(TerrainTowerFullMixedBuffer));
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