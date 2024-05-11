using Mafi;
using Mafi.Base;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

using TerrainTower.TTower;

namespace TerrainTower
{
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
            m_protoString = Proto.CreateStr(Extras.CustomIds.Prototypes.TerrainTowerProtoId, "Terrain Tower", "Tower to control Mining and Dumping");

            TerrainTowerProto protoObject = new TerrainTowerProto(
                id: Extras.CustomIds.Prototypes.TerrainTowerProtoId,
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
                .Start("Massive Storage Containers", Extras.CustomIds.Research.TerrainTowerResearchId)
                .Description("Terrain Tower Research")
                .AddLayoutEntityToUnlock(Extras.CustomIds.Prototypes.TerrainTowerProtoId)
                .SetGridPosition(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.IronSmeltingScrap).GridPosition + new Vector2i(0, -4))
                .SetCosts(new ResearchCostsTpl(TerrainTowerProto.RESEARCH_DIFFICULTY))
                .BuildAndAdd();
        }
    }

    internal class NotificationRegistrator : IModData
    {
        public void RegisterData(ProtoRegistrator registrator)
        {
            NotificationProtoBuilder notificationProtoBuilder = registrator.NotificationProtoBuilder;

            //Using vanilla AddIcon assets because I don't know how to design custom Entity Icons
            //AddEntityIcon are shown over affected entities.
            //Should have AddIcon - TerrainTowerIcon (or something like that) and AddEntityIcon showing the specific issue with the specific entity (Blocked/Designation etc)
            //If using {entity} in the message, EntityIcon is required.
            notificationProtoBuilder
                .Start("{entity}: configured for mining with no remaining mining designations", Extras.CustomIds.Notifications.TerrainTowerMissingMineDesignation)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/EntityIcons/Designation.png")
                .BuildAndAdd();

            notificationProtoBuilder
                .Start("{entity}: configured for dumping with no remaining dumping designations", Extras.CustomIds.Notifications.TerrainTowerMissingDumpDesignation)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/EntityIcons/Designation.png")
                .BuildAndAdd();

            notificationProtoBuilder
                .Start("{entity}: No available products to dump", Extras.CustomIds.Notifications.TerrainTowerMissingDumpItem)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .BuildAndAdd();

            notificationProtoBuilder
                .Start("{entity}: blocked due to full output", Extras.CustomIds.Notifications.TerrainTowerBlockedOuput)
                .SetType(NotificationType.Continuous)
                .SetStyle(NotificationStyle.Warning)
                .AddEntityIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .AddIcon("Assets/Unity/UserInterface/General/Blocked.svg")
                .BuildAndAdd();
        }
    }
}