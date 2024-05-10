using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using System;

namespace COI_Questing
{
    public sealed class COI_QuestingMod : DataOnlyMod
    {
        public static Version ModVersion = new Version(1, 0, 0);
        public override string Name => "Massive Storage Mod";

        public override int Version => 1;

        public COI_QuestingMod(CoreMod coreMod, BaseMod baseMod) { }

        public override void RegisterPrototypes(ProtoRegistrator registrator)
        {
            //Build Prototypes
            registrator.RegisterData<COI_Questing.COI_QuestingRegistrator>();

        }
    }
}
