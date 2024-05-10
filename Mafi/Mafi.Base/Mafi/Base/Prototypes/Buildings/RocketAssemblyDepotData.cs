// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.RocketAssemblyDepotData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class RocketAssemblyDepotData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<RocketAssemblyBuildingProto>(new RocketAssemblyBuildingProto(Ids.Buildings.RocketAssemblyDepot, Proto.CreateStrFormatDesc1((Proto.ID) Ids.Buildings.RocketAssemblyDepot, "Rocket assembly depot", "Constructs space rockets and delivers them to the nearest {0} using a specialized transporter. The transporter is a large vehicle and cannot drive over uneven ground so make sure that the {0} is on the same height level as this building.", (LocStrFormatted) RocketLaunchPadData.LAUNCH_PAD_STR.Name, "rocket assembly building description, {0} is launch pad"), Costs.Buildings.RocketAssemblyDepot.MapToEntityCosts(registrator), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.HasVehicleSurface), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[5]
      {
        new CustomLayoutToken("|0|", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-3, h, LayoutTileConstraint.Ground, new int?(-3)))),
        new CustomLayoutToken("10|", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-3, 10 + h, LayoutTileConstraint.Ground, new int?(-3)))),
        new CustomLayoutToken("10_", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 10 + h;
          Fix32? nullable = new Fix32?((Fix32) 0);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = nullable;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightToExcl: heightToExcl, constraint: LayoutTileConstraint.Ground, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("10=", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 10 + h;
          Fix32? nullable1 = new Fix32?((Fix32) 0);
          Proto.ID? nullable2 = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = nullable1;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable2;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, constraint: LayoutTileConstraint.Ground, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("10+", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 10 + h;
          Fix32? nullable = new Fix32?((Fix32) 0);
          int? terrainSurfaceHeight = new int?(-3);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = nullable;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightToExcl: heightToExcl, constraint: LayoutTileConstraint.Ground, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "      [9][9][9][9][9][9][9][9][9][9][9]                                                                                    ", "      [9][9][9][9][9][9][9][9][9][9][9]                           A#v   B#v   C#v   D#v                                    ", "      [9][9][9][9][9][9][9][9][9][9][9]                        [2][2][2][2][2][2][2][2][2]                                 ", "      [9][9][9][9][9][9][9][9][9][9][9]            [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  ", "      [9][9][9][9][9][9][9][9][9][9][9]            [5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  ", "[5][5][9][9][9][9][9][9][9][9][9][9][9][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  ", "[6]|6||9||9||9|[9][9][9][9][9]|9||9||9||5|[5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  ", "[6]|6||6||5||5|[5][5][5][5][5]|5||5||5||5|[5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  ", "[6]|6||6||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5|[5][5][5][5][5][5]                  ", "[6]|6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6|[6][6][6][6][6][6]_1__1__1__1__1__1_", "[6]|6||6||5||5||5||5|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15+15_15_15_15_15_15=_1__1__1__1__1__1_", "[6]|6||6||5||5||5||5|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15+15+15+15_15_15_15=_1__1__1__1__1__1_", "[6]|6||6||5||5||5||5|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15+15+15+ * 15_15_15=_1__1__1__1__1__1_", "[6]|6||6||5||5||5||5|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15+15+15+ * 15_15_15=_1__1__1__1__1__1_", "[6]|6||6||5||5||5||5|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15+15+15+15_15_15_15=_1__1__1__1__1__1_", "[6]|6||6||5||5||5||5|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15|15+15_15_15_15_15_15=_1__1__1__1__1__1_", "[6]|6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6||6|[6][6][6][6][6][6]_1__1__1__1__1__1_", "[6]|6||6||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5||5|[5][5][5][5][5][5]                  ", "[6]|6||6||5||5|[5][5][5][5][5]|5||5||5||5|[5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  ", "[6]|6||6||5||5|[5][5][5][5][5]|5||5||5||5|[5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  ", "[5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5][5]                  "), 2000.Kw(), Computing.FromTFlops(24), 10.Seconds(), Duration.FromKeyframes(120), Duration.FromKeyframes(290), Duration.FromKeyframes(500), new RelTile2f((Fix32) 0, (Fix32) 0), new RelTile2f((Fix32) 7, (Fix32) 0), new RelTile2f((Fix32) 0, (Fix32) 0), new RelTile2f((Fix32) 7, (Fix32) 0), new VehicleDepotBaseProto.Gfx("Assets/Base/Buildings/RocketAssemblyDepot.prefab", Option<string>.None, registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings)), "Inside/rameno/chnapky/RocketHolder"));
    }

    public RocketAssemblyDepotData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
