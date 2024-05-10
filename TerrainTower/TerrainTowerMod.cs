using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi;
using System;

namespace TerrainTower
{
    public sealed class TerrainTowerMod : DataOnlyMod
    {
        public override string Name => "Terrain Tower";
        public override int Version => 1;

        public static Version ModVersion = new Version(1, 0, 0);
        public new bool IsUiOnly => false;
        public override Option<IConfig> ModConfig { get; set; }


        // Mod constructor that lists mod dependencies as parameters.
        // This guarantee that all listed mods will be loaded before this mod.
        // It is a good idea to depend on `Mafi.Core.CoreMod` and `Mafi.Base.BaseMod`.
#pragma warning disable IDE0060 // Remove unused parameter
        public TerrainTowerMod(CoreMod coreMod, BaseMod baseMod)
        {
        }
#pragma warning restore IDE0060 // Remove unused parameter

        public new void Initialize(DependencyResolver resolver, bool gameWasLoaded)
        {
            Extras.Logger.Info("Initializing ");
            base.Initialize(resolver, gameWasLoaded);
        }

        public new void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded)
        {
            base.RegisterDependencies(depBuilder, protosDb, gameWasLoaded);
            Extras.Logger.Info("Register Dependencies ");
        }

        public override void RegisterPrototypes(ProtoRegistrator registrator)
        {
            //Build Prototypes
            registrator.RegisterData<PrototypeRegistrator>();
            registrator.RegisterData<NotificationRegistrator>();
            //TODO: Build RegisterData functions for Research and Notifications

        }
    }
}
