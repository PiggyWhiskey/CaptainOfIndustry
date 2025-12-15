using Mafi;
using Mafi.Base;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

using TerrainTower.Extras;
using TerrainTower.TTower;

namespace TerrainTower
{
    internal class NotificationRegistrator : IModData
    {
        public void RegisterData(ProtoRegistrator registrator)
        {
            NotificationProtoBuilder notificationProtoBuilder = registrator.NotificationProtoBuilder;

            //Using vanilla AddIcon assets because I don't know how to design custom Entity Icons
            //AddEntityIcon are shown over affected entities.
            //Should have AddIcon - TerrainTowerIcon (or something like that) and AddEntityIcon showing the specific issue with the specific entity (Blocked/Designation etc)
            //If using {entity} in the message, EntityIcon is required.
            _ = notificationProtoBuilder
                .Start("{entity}: has no remaining mining designations", CustomIds.Notifications.TerrainTowerMissingDesignation)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/EntityIcons/Designation.png")
                .BuildAndAdd();

            _ = notificationProtoBuilder
                .Start("{entity}: No available products to dump", CustomIds.Notifications.TerrainTowerMissingDumpItem)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .BuildAndAdd();

            _ = notificationProtoBuilder
                .Start("{entity}: blocked due to full output", CustomIds.Notifications.TerrainTowerBlockedOuput)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .BuildAndAdd();

            _ = notificationProtoBuilder
                .Start("{entity}: blocked due to full mixed buffer", CustomIds.Notifications.TerrainTowerFullMixedBuffer)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .BuildAndAdd();
        }
    }

    internal class PrototypeRegistrator : IModData
    {
        private LayoutEntityProto.Gfx m_entityGraphics;
        private Proto.Str m_protoString;

        public void RegisterData(ProtoRegistrator registrator)
        {
            m_entityGraphics = new LayoutEntityProto.Gfx(
                prefabPath: TerrainTowerProto.PREFAB_PATH,
                customIconPath: TerrainTowerProto.CUSTOM_ICON_PATH,
                categories: registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings));
            m_protoString = Proto.CreateStr(CustomIds.Prototypes.TerrainTowerProtoId, "Terrain Tower", "Tower to control Mining and Dumping");

            TerrainTowerProto protoObject = new TerrainTowerProto(
                id: CustomIds.Prototypes.TerrainTowerProtoId,
                strings: m_protoString,
                layout: registrator.LayoutParser.ParseLayoutOrThrow(TerrainTowerProto.LAYOUT),
                costs: TerrainTowerProto.ENTITY_COSTS.MapToEntityCosts(registrator),
                bufferCapacity: TerrainTowerProto.BUFFER_CAPACITY,
                conversionLoss: TerrainTowerProto.CONVERSION_LOSS,
                sortDuration: TerrainTowerProto.SORT_DURATION,
                sortQuantityPerDuration: TerrainTowerProto.SORT_QUANTITY_PER_DURATION,
                terrainDuration: TerrainTowerProto.TERRAIN_DURATION,
                electricityConsumed: TerrainTowerProto.ELECTRICITY_COST,
                area: TerrainTowerProto.DEFAULT_AREA,
                boostCost: TerrainTowerProto.BOOST_COST,
                graphics: m_entityGraphics
                );

            _ = registrator.PrototypesDb.Add(protoObject);
        }
    }

    internal class ResearchRegistrator : IModData
    {
        public void RegisterData(ProtoRegistrator registrator)
        {
            _ = registrator.ResearchNodeProtoBuilder
                .Start(name: "Terrain Tower",
                       nodeId: CustomIds.Research.TerrainTowerResearchId,
                       costMonths: TerrainTowerProto.RESEARCH_DIFFICULTY)
                .Description("Terrain Tower Research")
                .AddLayoutEntityToUnlock(CustomIds.Prototypes.TerrainTowerProtoId)
                .SetGridPosition(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.IronSmeltingScrap).GridPosition + new Vector2i(0, -4))
                .BuildAndAdd();
        }
    }
}