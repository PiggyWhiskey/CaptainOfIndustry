using Mafi;
using Mafi.Base;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using System.Linq;

namespace MassiveStorage.BuildingDefaults
{
    internal class DefaultsBase
    {
        public readonly Tiered<int> StorageCapacity = new Tiered<int>(5000, 500000, 1000000, int.MaxValue);
        public readonly Tiered<EntityCostsTpl> BuildCost = new Tiered<EntityCostsTpl>(Costs.Build.CP(5), Costs.Build.CP(10), Costs.Build.CP2(5), Costs.Build.CP2(10));
        public readonly IProtoParam DrawArrow = new DrawArrowWileBuildingProtoParam(4f);
        public virtual string IDDescription => "Massive Storage - ";
        public virtual string Description => "Massive Storage Mod: ";
        public virtual string PrefabPath => "Assets/Base/Buildings/";
        public virtual string CustomIconPath => "Assets/Unity/Generated/Icons/LayoutEntity/";
    }
    internal class DefaultsStorageBase : DefaultsBase
    {
        public override string PrefabPath => string.Concat(base.PrefabPath, "Storages/");
        public readonly Proto.ID Category = Ids.ToolbarCategories.Storages;
        public virtual string[] Layout => new string[5]
        {
            "A{0}>[4][4][4][4][4]>{0}V",
            "B{0}>[4][4][4][4][4]>{0}W",
            "C{0}>[4][4][4][4][4]>{0}X",
            "D{0}>[4][4][4][4][4]>{0}Y",
            "E{0}>[4][4][4][4][4]>{0}Z"
        };

    }
    internal sealed class DefaultsStorageFluid : DefaultsStorageBase
    {
        public override string IDDescription => string.Concat(base.IDDescription, "Fluid ");
        public override string Description => string.Concat(base.Description, "Fluid Storage ");
        public override string PrefabPath => string.Concat(base.PrefabPath, "GasT1.prefab");
        public override string CustomIconPath => string.Concat(base.CustomIconPath, "StorageFluid.png");
        public override string[] Layout => base.Layout.Select(x => string.Format(x, "@")).ToArray();
    }
    internal sealed class DefaultsStorageUnit : DefaultsStorageBase
    {
        public override string IDDescription => string.Concat(base.IDDescription, "Unit ");
        public override string Description => string.Concat(base.Description, "Unit Storage ");
        public override string PrefabPath => string.Concat(base.PrefabPath, "UnitT1.prefab");
        public override string CustomIconPath => string.Concat(base.CustomIconPath, "StorageUnit.png");
        public override string[] Layout => base.Layout.Select(x => string.Format(x, "#")).ToArray();
    }
    internal sealed class DefaultsStorageLoose : DefaultsStorageBase
    {
        public override string IDDescription => string.Concat(base.IDDescription, "Loose ");
        public override string Description => string.Concat(base.Description, "Loose Storage ");
        public override string PrefabPath => string.Concat(base.PrefabPath, "LooseT1.prefab");
        public override string CustomIconPath => string.Concat(base.CustomIconPath, "StorageLoose.png");
        public override string[] Layout => base.Layout.Select(x => string.Format(x, "~")).ToArray();
    }
    internal sealed class DefaultsOreSortingBase : DefaultsBase
    {
        public readonly Proto.ID Category = Ids.ToolbarCategories.BuildingsForVehicles;
        public override string IDDescription => string.Concat(base.IDDescription, "Ore Sorting Plant ");
        public override string Description => string.Concat(base.Description, "Ore Sorting Plant ");
        public override string PrefabPath => string.Concat(base.PrefabPath, "OreSorterT1.prefab");

        public override string CustomIconPath => string.Concat(base.CustomIconPath, "OreSortingPlantT1.png");

        public readonly string[] Layout = new string[11]
        {
            "   (2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)               ",
            "<~V(2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)",
            "   (2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)",
            "<~W(2)(2)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)",
            "   (2)(2)(5)(5)(6)(6)(6)(6)(6)(6)(6)(6)(6)(6)(4)(4)(4)(4)",
            "<~X(2)(2)(5)(5)(6)(6)(6)(6)(6)(6)(6)(6)(6)(6)(4)(4)(4)(4)",
            "   (2)(2)(5)(5)(6)(6)(6)(6)(6)(6)(6)(6)(6)(6)(4)(4)(4)(4)",
            "<~Y(2)(2)(4)(4)(4)(4)(4)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)",
            "   (2)(2)(4)(4)(4)(4)(4)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)",
            "   (2)(2)(4)(4)(4)(4)(4)(5)(5)(5)(4)(4)(4)(4)(4)(4)(4)(4)",
            "   (2)(2)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)(4)"
        };

        public readonly Tiered<int> WorkDuration = new Tiered<int>(5, 5, 5, 5);
        public readonly Tiered<int> ProcessQuantity = new Tiered<int>(500, 50000, 100000, int.MaxValue / 10);
        public readonly Tiered<int> ConversionLoss = new Tiered<int>(0, 0, 0, 0);
        public readonly Tiered<int> ElectricityConsumed = new Tiered<int>(100, 200, 500, 1000);
    }
    internal sealed class Tiered<T>
    {
        public T T1 { get; set; }
        public T T2 { get; set; }
        public T T3 { get; set; }
        public T T4 { get; set; }
        public Tiered(T t1, T t2, T t3, T t4)
        {
            T1 = t1;
            T2 = t2;
            T3 = t3;
            T4 = t4;
        }
    }
}
