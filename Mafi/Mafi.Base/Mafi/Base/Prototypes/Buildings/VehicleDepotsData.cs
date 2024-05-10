// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.VehicleDepotsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class VehicleDepotsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      VehicleDepotProtoBuilder depotProtoBuilder = registrator.VehicleDepotProtoBuilder;
      LocStr desc = Loc.Str(Ids.Buildings.VehiclesDepot.ToString() + "__desc", "Allows building vehicles such as trucks and excavators.", "description of a machine");
      depotProtoBuilder.Start("Vehicles depot III", Ids.Buildings.VehiclesDepotT3).Description(desc).SetCost(Costs.Buildings.VehiclesDepot3).SetConsumedPower(640.Kw()).SetSpawnInterval(6.3.Seconds()).SetDoorOpenCloseDuration(Duration.FromKeyframes(120)).SetSpawnTiles(new RelTile2f((Fix32) 2, 0.5.ToFix32()), new RelTile2f((Fix32) 15, 0.5.ToFix32())).SetDespawnTiles(new RelTile2f((Fix32) 2, -0.5.ToFix32()), new RelTile2f((Fix32) 15, -0.5.ToFix32())).SetLayout(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.HasVehicleSurface), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("!0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h + 1;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]                     ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]                     ", "   [9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9]                     ", "A#>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "B#>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "C#>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "D#>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "E#>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "F#>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "G#>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9] *  * !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9] *  * !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "P@>!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]_1__1__1__1__1__1__1_", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "   !9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]!9]                     ", "   [9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9][9]                     ").SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetPrefabPath("Assets/Base/Buildings/VehicleDepots/VehicleDepotT3.prefab").SetSoundPath("Assets/Base/Buildings/VehicleDepots/T1/DepotT1Sound.prefab").BuildAndAdd().AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(5f)).AddParam((IProtoParam) DisableQuickBuildParam.Instance);
      VehicleDepotProto nextTier = depotProtoBuilder.Start("Vehicles depot II", Ids.Buildings.VehiclesDepotT2).Description(desc).SetCost(Costs.Buildings.VehiclesDepot2).SetConsumedPower(320.Kw()).SetSpawnInterval(6.3.Seconds()).SetDoorOpenCloseDuration(Duration.FromKeyframes(100)).SetSpawnTiles(new RelTile2f((Fix32) 0, 1.5.ToFix32()), new RelTile2f((Fix32) 9, 1.5.ToFix32())).SetDespawnTiles(new RelTile2f((Fix32) 0, -1.5.ToFix32()), new RelTile2f((Fix32) 9, -1.5.ToFix32())).SetLayout(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.HasVehicleSurface)), "   [2][2][2][2][2][2][2][2][2][2][2]               ", "P@>[6][6][6][6][6][6][6][6][6][6][6]               ", "A#>[7][7][7][7][7][7][7][7][7][7][7]_1__1__1__1__1_", "B#>[7][7][7][7][7][7][7][7][7][7][7]_1__1__1__1__1_", "C#>[7][7][7][7][7][7][7][7][7][7][7]_1__1__1__1__1_", "D#>[7][7][7][7] *  * [7][7][7][7][7]_1__1__1__1__1_", "E#>[7][7][7][7] *  * [7][7][7][7][7]_1__1__1__1__1_", "F#>[7][7][7][7][7][7][7][7][7][7][7]_1__1__1__1__1_", "G#>[7][7][7][7][7][7][7][7][7][7][7]_1__1__1__1__1_", "H#>[7][7][7][7][7][7][7][7][7][7][7]_1__1__1__1__1_", "   [6][6][6][6][6][6][6][6][6][6][6]               ", "   [2][2][2][2][2][2][2][2][2][2][2]               ").SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetPrefabPath("Assets/Base/Buildings/VehicleDepots/VehicleDepotT2.prefab").SetSoundPath("Assets/Base/Buildings/VehicleDepots/T1/DepotT1Sound.prefab").BuildAndAdd();
      nextTier.AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(5f)).AddParam((IProtoParam) DisableQuickBuildParam.Instance);
      depotProtoBuilder.Start("Vehicles depot", Ids.Buildings.VehiclesDepot).Description(desc).SetCost(Costs.Buildings.VehiclesDepot).SetSpawnInterval(6.3.Seconds()).SetDoorOpenCloseDuration(Duration.FromKeyframes(120)).SetSpawnTiles(new RelTile2f((Fix32) 0, 1.5.ToFix32()), new RelTile2f((Fix32) 9, 1.5.ToFix32())).SetDespawnTiles(new RelTile2f((Fix32) 0, -1.5.ToFix32()), new RelTile2f((Fix32) 9, -1.5.ToFix32())).SetLayout(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.HasVehicleSurface)), "[2][2][2][2][2][2][2][2][2][2][2]               ", "[2][5][5][5][5][5][5][5][5][5][5]               ", "[2][6][6][6][6][6][6][6][6][6][6]_1__1__1__1__1_", "[2][6][6][6][6][6][6][6][6][6][6]_1__1__1__1__1_", "[2][6][6][6][6][6][6][6][6][6][6]_1__1__1__1__1_", "[2][6][6][6] *  * [6][6][6][6][6]_1__1__1__1__1_", "[2][6][6][6] *  * [6][6][6][6][6]_1__1__1__1__1_", "[2][6][6][6][6][6][6][6][6][6][6]_1__1__1__1__1_", "[2][6][6][6][6][6][6][6][6][6][6]_1__1__1__1__1_", "[2][6][6][6][6][6][6][6][6][6][6]_1__1__1__1__1_", "[2][5][5][5][5][5][5][5][5][5][5]               ", "[2][2][2][2][2][2][2][2][2][2][2]               ").SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetPrefabPath("Assets/Base/Buildings/VehicleDepots/VehicleDepotT1.prefab").SetSoundPath("Assets/Base/Buildings/VehicleDepots/T1/DepotT1Sound.prefab").SetNextTier(nextTier).BuildAndAdd().AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(5f)).AddParam((IProtoParam) DisableQuickBuildParam.Instance);
    }

    public VehicleDepotsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
