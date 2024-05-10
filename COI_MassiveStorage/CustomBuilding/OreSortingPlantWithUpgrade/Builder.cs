using Mafi;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System.Xml.Linq;

namespace MassiveStorage.CustomBuilding.OreSortingPlantWithUpgrade
{
    /*
    protBuilder
        .Start(string.Format(defaults.IDDescription,"T3"),CustomIDs.Buildings.OreSortingT3)
        .Description(string.Format(defaults.Description,"T3"))
        .SetCost(defaults.BuildCost.T4)
        .SetBufferCapacity (defaults.StorageCapacity.T3)
        .SetWorkQuantityPerDuration (defaults.ProcessQuantity.T3)
        .SetElectricityConsumption (defaults.ElectricityConsumed.T3)
        .SetWorkDurationSeconds (defaults.WorkDuration.T3)
        .SetNextTier(registrator.PrototypesDb.GetOrThrow<OreSortingPlantProtoWithUpgrade>(CustomIDs.Buildings.OreSortingT4))
        .SetCategories(defaults.Category)
        .SetCustomIconPath(defaults.CustomIconPath)
        .SetPrefabPath(defaults.PrefabPath)
        .SetLayout(defaults.Layout)
        .BuildAndAdd();
    */
    public sealed class OreSortingPlantUpgradableBuilder : IProtoBuilder
    {

        public Mafi.Core.Mods.ProtoRegistrator Registrator { get; }
        public ProtosDb ProtosDb => Registrator.PrototypesDb;

        public sealed class State : LayoutEntityBuilderState<State>
        {
            private readonly StaticEntityProto.ID m_protoId;
            private Quantity? m_capacity;
            private Quantity? m_workQuantityPerDuration;
            private Duration? m_workDurationSeconds;
            private Percent? m_workConversionLoss;
            private Option<OreSortingPlantUpgradableProto> m_nextTier;
            private ImmutableArray<AnimationParams> m_animationParams;

            public State(OreSortingPlantUpgradableBuilder builder, StaticEntityProto.ID id, string name) : base(builder, id, name, "building or machine")
            {
                m_protoId = id;
            }

            [MustUseReturnValue]
            public State SetBufferCapacity(int capacity)
            {
                m_capacity = capacity.Quantity();
                return this;
            }
            [MustUseReturnValue]
            public State SetBufferCapacity(Quantity capacity)
            {
                m_capacity = capacity;
                return this;
            }
            [MustUseReturnValue]
            public State SetElectricityConsumption(int electricity)
            {
                Electricity = electricity.Kw();
                return this;
            }
            [MustUseReturnValue]
            public State SetElectricityConsumption(Electricity electricity)
            {
                Electricity = electricity;
                return this;
            }
            [MustUseReturnValue]
            public State SetWorkQuantityPerDuration(int workQuantityPerDuration)
            {
                m_workQuantityPerDuration = workQuantityPerDuration.Quantity();
                return this;
            }
            [MustUseReturnValue]
            public State SetWorkQuantityPerDuration(Quantity workQuantityPerDuration)
            {
                m_workQuantityPerDuration = workQuantityPerDuration;
                return this;
            }
            [MustUseReturnValue]
            public State SetWorkDurationSeconds(int workDurationSeconds)
            {
                m_workDurationSeconds = workDurationSeconds.Seconds();
                return this;
            }
            [MustUseReturnValue]
            public State SetWorkDurationSeconds(Duration workDurationSeconds)
            {
                m_workDurationSeconds = workDurationSeconds;
                return this;
            }
            [MustUseReturnValue]
            public State SetWorkConversionLoss(int workConversionLoss)
            {
                m_workConversionLoss = workConversionLoss.Percent();
                return this;
            }
            [MustUseReturnValue]
            public State SetWorkConversionLoss(Percent workConversionLoss)
            {
                m_workConversionLoss = workConversionLoss;
                return this;
            }
            [MustUseReturnValue]
            public State SetNextTier(OreSortingPlantUpgradableProto nextTierId)
            {
                m_nextTier = nextTierId;
                return this;
            }
            [MustUseReturnValue]
            public State SetProtoAnimationParams(ImmutableArray<AnimationParams> animationParams)
            {
                m_animationParams = animationParams;
                return this;
            }
            public OreSortingPlantProto BuildAndAdd()
            {
                //string.IsNullOrEmpty(PrefabPath) ?
                //(OreSortingPlantProto.Gfx)LayoutEntityProto.Gfx.Empty :

                OreSortingPlantProto.Gfx ProtoGraphics = new OreSortingPlantProto.Gfx(
                        prefabPath: ValueOrThrow(base.PrefabPath, "prefab"),
                        smoothPileObjectPath: "Pile_Soft",
                        loosePileTextureParams: new LoosePileTextureParams(0.9f),
                        prefabOrigin: new RelTile3f(),
                        customIconPath: ValueOrThrow(base.CustomIconPath, "customIcon"),
                        color: new ColorRgba(),
                        visualizedLayers: new LayoutEntityProto.VisualizedLayers?(),
                        categories: base.GetCategoriesOrThrow()
                        );


                return AddToDb(
                    new OreSortingPlantUpgradableProto(
                       id: m_protoId,
                       strings: base.Strings,
                       layout: base.LayoutOrThrow,
                       costs: base.Costs,
                       inputBufferCapacity: ValueOrThrow(m_capacity, "buffer input"),
                       outputBuffersCapacity: ValueOrThrow(m_capacity, "buffer output"),
                       duration: ValueOrThrow(m_workDurationSeconds, "duration"),
                       quantityPerDuration: ValueOrThrow(m_workQuantityPerDuration, "quantity duration"),
                       conversionLoss: ValueOrThrow(m_workConversionLoss, "conversion loss"),
                       electricityConsumed: base.Electricity,
                       animationParams: m_animationParams,
                       upgradeData: m_nextTier,
                       graphics: ProtoGraphics
                   )
                );
            }
        }

        public OreSortingPlantUpgradableBuilder(ProtoRegistrator registrator) : base()
        {
            Registrator = registrator;
        }

        public State Start(string name, StaticEntityProto.ID oreSorterID)
        {
            return new State(this, oreSorterID, name);
        }
    }
}
