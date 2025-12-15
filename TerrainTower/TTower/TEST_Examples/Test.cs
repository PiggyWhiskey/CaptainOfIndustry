using Mafi;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using Mafi.Unity.InputControl;
using Mafi.Unity.Mine;
using Mafi.Unity.Ui;
using Mafi.Unity.Ui.Controllers;
using Mafi.Unity.Ui.Library;
using Mafi.Unity.Ui.Library.Inspectors;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.Utils;

using System;
using System.Linq;

#pragma warning disable

namespace TerrainTower.TTower
{
    namespace Mafi.Core.Buildings.Mine
    {
        [GlobalDependency(RegistrationMode.AsSelf, false, false)]
        [MemberRemovedInSaveVersion("m_onAreaChange", 180, typeof(Event<MineTower, RectangleTerrainArea2i>), 0, false, double.NaN)]
        [GenerateSerializer(false, null, 0)]
        public class MineTowersManager
        {
            private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
            private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
            private readonly EntitiesManager m_entitiesManager;
            private readonly Lyst<MineTower> m_mineTowers;

            [NewInSaveVersion(180, null, "new()", null, null)]
            private readonly Event<MineTower, PolygonTerrainArea2i> m_onAreaChange;

            private readonly Event<MineTower, EntityAddReason> m_onTowerAdded;
            private readonly Event<MineTower, EntityRemoveReason> m_onTowerRemoved;

            static MineTowersManager()
            {
                MineTowersManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>)((obj, writer) => ((MineTowersManager)obj).SerializeData(writer));
                MineTowersManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>)((obj, reader) => ((MineTowersManager)obj).DeserializeData(reader));
            }

            public MineTowersManager(EntitiesManager entitiesManager) : base()
            {
                this.m_onTowerAdded = new Event<MineTower, EntityAddReason>();
                this.m_onTowerRemoved = new Event<MineTower, EntityRemoveReason>();
                this.m_onAreaChange = new Event<MineTower, PolygonTerrainArea2i>();
                this.m_mineTowers = new Lyst<MineTower>();
                this.m_entitiesManager = entitiesManager;
                entitiesManager.EntityAddedFull.Add<MineTowersManager>(this, new Action<IEntity, EntityAddReason>(this.entityAdded));
                entitiesManager.EntityRemovedFull.Add<MineTowersManager>(this, new Action<IEntity, EntityRemoveReason>(this.entityRemoved));
            }

            /// <summary>
            /// Invoked when mine tower is changed. Parameters are current mine tower and old area.
            /// </summary>
            public IEvent<MineTower, PolygonTerrainArea2i> OnAreaChange
            {
                get => (IEvent<MineTower, PolygonTerrainArea2i>)this.m_onAreaChange;
            }

            public IEvent<MineTower, EntityAddReason> OnTowerAdded
            {
                get => (IEvent<MineTower, EntityAddReason>)this.m_onTowerAdded;
            }

            public IEvent<MineTower, EntityRemoveReason> OnTowerRemoved
            {
                get => (IEvent<MineTower, EntityRemoveReason>)this.m_onTowerRemoved;
            }

            public IIndexable<MineTower> Towers => (IIndexable<MineTower>)this.m_mineTowers;

            public static MineTowersManager Deserialize(BlobReader reader)
            {
                MineTowersManager mineTowersManager;
                if (reader.TryStartClassDeserialization<MineTowersManager>(out mineTowersManager))
                    reader.EnqueueDataDeserialization((object)mineTowersManager, MineTowersManager.s_deserializeDataDelayedAction);
                return mineTowersManager;
            }

            public static void Serialize(MineTowersManager value, BlobWriter writer)
            {
                if (!writer.TryStartClassSerialization<MineTowersManager>(value))
                    return;
                writer.EnqueueDataSerialization((object)value, MineTowersManager.s_serializeDataDelayedAction);
            }

            internal void InvokeOnAreaChanged(MineTower tower, PolygonTerrainArea2i oldArea)
            {
                this.m_onAreaChange.Invoke(tower, oldArea);
            }

            protected virtual void DeserializeData(BlobReader reader)
            {
                reader.SetField<MineTowersManager>(this, "m_entitiesManager", (object)EntitiesManager.Deserialize(reader));
                reader.SetField<MineTowersManager>(this, "m_mineTowers", (object)Lyst<MineTower>.Deserialize(reader));
                reader.SetField<MineTowersManager>(this, "m_onAreaChange", reader.LoadedSaveVersion >= 180 ? (object)Event<MineTower, PolygonTerrainArea2i>.Deserialize(reader) : (object)new Event<MineTower, PolygonTerrainArea2i>());
                if (reader.LoadedSaveVersion < 180)
                    Event<MineTower, RectangleTerrainArea2i>.Deserialize(reader);
                reader.SetField<MineTowersManager>(this, "m_onTowerAdded", (object)Event<MineTower, EntityAddReason>.Deserialize(reader));
                reader.SetField<MineTowersManager>(this, "m_onTowerRemoved", (object)Event<MineTower, EntityRemoveReason>.Deserialize(reader));
            }

            protected virtual void SerializeData(BlobWriter writer)
            {
                EntitiesManager.Serialize(this.m_entitiesManager, writer);
                Lyst<MineTower>.Serialize(this.m_mineTowers, writer);
                Event<MineTower, PolygonTerrainArea2i>.Serialize(this.m_onAreaChange, writer);
                Event<MineTower, EntityAddReason>.Serialize(this.m_onTowerAdded, writer);
                Event<MineTower, EntityRemoveReason>.Serialize(this.m_onTowerRemoved, writer);
            }

            private void entityAdded(IEntity entity, EntityAddReason addReason)
            {
                if (!(entity is MineTower mineTower))
                    return;
                this.m_mineTowers.Add(mineTower);
                this.m_onTowerAdded.Invoke(mineTower, addReason);
            }

            private void entityRemoved(IEntity entity, EntityRemoveReason removeReason)
            {
                if (!(entity is MineTower mineTower))
                    return;
                Assert.That<bool>(this.m_mineTowers.TryRemoveReplaceLast(mineTower)).IsTrue();
                this.m_onTowerRemoved.Invoke(mineTower, removeReason);
            }
        }
    }

    internal class MineTowerInspector : BaseInspector<MineTower>
    {
        private readonly PolygonAreaSelectionController m_areaSelectionTool;
        private readonly IActivator m_towerAreasAndDesignatorsActivator;
        private readonly TowerAreasRenderer m_towerAreasRenderer;
        private Option<MineTower> m_entityUnderEdit;

        public MineTowerInspector(
            UiContext context,
            TowerAreasRenderer towerAreasRenderer,
            AssignedBuildingsHighlighter highlighter,
            BuildingsAssigner buildingsAssigner,
            NewInstanceOf<PolygonAreaSelectionController> areaSelectionTool) : base(context)
        {
            MineTowerInspector mineTowerInspector = this;
            m_towerAreasRenderer = towerAreasRenderer;
            m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
            m_areaSelectionTool = areaSelectionTool.Instance;
            EmbedStatusToTheTop();

            TopLeftDisplays
                .Add(new ButtonIconText(Button.Primary, "Assets/Unity/UserInterface/Toolbox/SelectArea.svg", (LocStrFormatted)Tr.ManagedArea__EditAction)
                .OnClick(new Action(activateAreaEditing)), new ButtonIcon(Button.General, "Assets/Unity/UserInterface/General/Search.svg")
                .OnClick(() => mineTowerInspector.Context.CameraController.PanTo(mineTowerInspector.Entity.Area.BoundingBoxCenter.CenterTile2f))
                .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.FocusManagedAreaTooltip)));

            Lyst<ProductProto> allDumpableProducts = Context.ProtosDb
                .All<LooseProductProto>()
                .Where(x => x.CanBeOnTerrain)
                .Cast<ProductProto>().ToLyst();

            MultipleProductsPickerUi component1 = new MultipleProductsPickerUi(
                () => allDumpableProducts,
                () => (mineTowerInspector.Entity).ProductsToNotifyIfCannotGetRidOf,
                p => mineTowerInspector.ScheduleCommand(new AddProductToNotifyIfCannotDumpCmd(mineTowerInspector.Entity, p)),
                p => mineTowerInspector.ScheduleCommand(new RemoveProductToNotifyIfCannotDumpCmd(mineTowerInspector.Entity, p)),
                string.Format("({0})", Tr.MineTowerNotifyFilter__Empty).AsLoc());

            Info.AddTutorial(IdsCore.Messages.TutorialOnMineTower);

            this.AddVehicleAssigner<MineTower, ExcavatorProto>(
                (LocStrFormatted)Tr.AssignedExcavators__Title,
                new LocStrFormatted?((LocStrFormatted)Tr.AssignedExcavators__MineTower_Title)
                );

            this.AddVehicleAssigner<MineTower, TruckProto>(
                (LocStrFormatted)Tr.AssignedTrucks__Title,
                new LocStrFormatted?((LocStrFormatted)Tr.AssignedTrucks__MineTower_Tooltip)
                );

            MultipleProductsPickerUi component2 = new MultipleProductsPickerUi(
                () => context.UnlockedProtosDbForUi.FilterUnlocked(allDumpableProducts),
                () => (mineTowerInspector.Entity).DumpableProducts,
                p => mineTowerInspector.ScheduleCommand(new AddProductToDumpCmd((Option<MineTower>)mineTowerInspector.Entity, p)),
                p => mineTowerInspector.ScheduleCommand(new RemoveProductToDumpCmd((Option<MineTower>)(mineTowerInspector.Entity), p)),
                string.Format("({0})", Tr.DumpingFilter__Empty).AsLoc());

            UiComponent[] uiComponentArray = new UiComponent[1];
            Row row1 = new Row
            {
                component2.FlexGrow(1f, Percent.Fifty),
                new VerticalDivider().MarginRight(2.pt()),
                component1.FlexGrow(1f, Percent.Fifty)
            };
            uiComponentArray[0] = row1;

            Row header = AddPanelWithHeader(uiComponentArray).Header;

            Row row2 = new Row
            {
                c => c.Fill().AlignSelfStretch(),
                new Label((LocStrFormatted)Tr.DumpingFilter__Title)
                    .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.DumpingFilter__Tooltip))
                    .TextCenterMiddle().FontBold()
                    .FlexGrow(1f, Percent.Fifty),
                new VerticalDivider()
                    .MarginRight(2.pt()),
                new Label((LocStrFormatted)Tr.MineTowerNotifyFilter__Title)
                    .Tooltip(new LocStrFormatted?((LocStrFormatted)Tr.MineTowerNotifyFilter__Tooltip))
                    .TextCenterMiddle().FontBold()
                    .FlexGrow(1f, Percent.Fifty)
            };

            header.Add(row2);

            string str = Context.ProtosDb.All<MineTowerProto>().FirstOrDefault()?.Strings.Name.TranslatedString ?? "Mine tower";
            this.AddBuildingsAssignerForExportImport(
                highlighter,
                buildingsAssigner,
                Tr.AssignedForLogistics__ExportTooltipMineTower.Format(str),
                Tr.AssignedForLogistics__ImportTooltipMineTower.Format(str)
                );
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            m_towerAreasRenderer.HighlightTowerArea((Option<IAreaManagingTower>)Entity);
            m_towerAreasAndDesignatorsActivator.ActivateIfNotActive();
            m_entityUnderEdit = Option<MineTower>.None;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (m_entityUnderEdit.IsNone)
                m_towerAreasAndDesignatorsActivator.DeactivateIfActive();
            m_towerAreasRenderer.HighlightTowerArea((Option<IAreaManagingTower>)Option.None);
        }

        private void activateAreaEditing()
        {
            m_entityUnderEdit = (Option<MineTower>)Entity;
            m_towerAreasRenderer.MarkAreaUnderEdit((Option<IAreaManagingTower>)Entity);
            m_areaSelectionTool.BeginEdit(
                Entity.Area,
                (Fix32)Entity.Prototype.Area.MaxAreaEdgeSize.Value,
                new Action(deactivateEditing),
                new Action(reopen),
                new Action<PolygonTerrainArea2i>(onAreaChanged)
                );
        }

        private void deactivateEditing()
        {
            m_towerAreasAndDesignatorsActivator.DeactivateIfActive();
            m_towerAreasRenderer.MarkAreaUnderEdit(Option<IAreaManagingTower>.None);
        }

        private void onAreaChanged(PolygonTerrainArea2i newArea)
        {
            if (!m_entityUnderEdit.HasValue)
                return;
            _ = ScheduleCommand(new MineTowerAreaChangeCmd(m_entityUnderEdit.Value.Id, newArea));
        }

        private void reopen()
        {
            if (m_entityUnderEdit.HasValue)
                Context.InspectorsManager.TryActivateFor(m_entityUnderEdit.Value);
            m_entityUnderEdit = Option<MineTower>.None;
        }
    }
}