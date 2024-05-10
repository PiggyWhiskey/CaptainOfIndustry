// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.RocketLaunchPadData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class RocketLaunchPadData : IModData
  {
    public static readonly Proto.Str LAUNCH_PAD_STR;

    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<RocketLaunchPadProto>(new RocketLaunchPadProto(Ids.Buildings.RocketLaunchPad, RocketLaunchPadData.LAUNCH_PAD_STR, Costs.Buildings.RocketLaunchPad.MapToEntityCosts(registrator), new RelTile1i(-10), new RelTile1i(-3), new RelTile1i(3), 4.Seconds(), 5.Seconds(), 10.Seconds(), ImmutableArray.Create<char>('E', 'F'), 160.Of(prototypesDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.CleanWater)), 8.Seconds(), registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.HasVehicleSurface), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[3]
      {
        new CustomLayoutToken("-0]", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-h, 4, LayoutTileConstraint.Ground, new int?(-h)))),
        new CustomLayoutToken("+0=", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? terrainSurfaceHeight = new int?(0);
          Fix32? nullable = new Fix32?((Fix32) 0);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = nullable;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("-0=", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightFrom = -h;
          int? terrainSurfaceHeight = new int?(-h);
          Fix32? nullable = new Fix32?((Fix32) 0);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = nullable;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightFrom, 9, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }, hardenedFloorSurfaceId: new Proto.ID?(Ids.TerrainTileSurfaces.ConcreteReinforced), enforceEmptySurface: true), "                        [3][3][6][6][6][6][6][6][3][3]                                             ", "                        [3][3][4][4][4][4][4][4][3][3]                                             ", "                        [3][3][4][4][4][4][4][4][3][3]                                             ", "                        [3][3][4][4][4][4][4][4][3][3]                                             ", "                        [3][3]-1]-1]-1]-1]-1]-1][3][3]                                             ", "                        [3][3]-2]-2]-2]-2]-2]-2][3][3]                                             ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4]                           ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4]<@A                        ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4]<@B                        ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4]                           ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4]                           ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4]                           ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4]                           ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4]                           ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_+9=+9=+9=+9=-3=-3=-3=-3=-3=-3=-0=12]12]12]12]12]12][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_+9=+9=+9=+9=-3=-3=-3=-3=-3=-3=-0=12]12]12]12]12]12][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_+9=+9=+9=+9=-3=-3= *  * -3=-3=-0=12]12]12]12]12]12][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_+9=+9=+9=+9=-3=-3= *  * -3=-3=-0=12]12]12]12]12]12][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_+9=+9=+9=+9=-3=-3=-3=-3=-3=-3=-0=12]12]12]12]12]12][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_+9=+9=+9=+9=-3=-3=-3=-3=-3=-3=-0=12]12]12]12]12]12][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][6][6][6][6][6][6][6][6]   ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][4][4][4][4][4][4]         ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][4][4][4][4][4][4]         ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][4][4][4][4][4][4]         ", "_1__1__1__1__1__1_[4][4][4][4]-3]-3]-3]-3]-3]-3][4][4][4][4][4][4][4][4][4][4][4][4][4][4]         ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4][4][4][4][4][4][4]         ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4][4][4][4][4][4][4]         ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4][4][4][4][4][4][4]<@E      ", "                        [3][3]-3]-3]-3]-3]-3]-3][3][3][4][4][4][4][4][4][4][4][4][4][4][4]<@F      ", "                        [3][3]-2]-2]-2]-2]-2]-2][3][3][4][4][4][4][4][4][4][4][4][4][4][4]         ", "                        [3][3]-1]-1]-1]-1]-1]-1][3][3][4][4][4][4][4][4][4][4][4][4][4][4]         ", "                        [3][3][4][4][4][4][4][4][3][3]                                             ", "                        [3][3][4][4][4][4][4][4][3][3]                                             ", "                        [3][3][4][4][4][4][4][4][3][3]                                             ", "                        [3][3][6][6][6][6][6][6][3][3]                                             "), new RocketLaunchPadProto.Gfx("Assets/Base/Buildings/RocketLaunchPad.prefab", registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings))));
    }

    public RocketLaunchPadData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static RocketLaunchPadData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      RocketLaunchPadData.LAUNCH_PAD_STR = Proto.CreateStr((Proto.ID) Ids.Buildings.RocketLaunchPad, "Rocket launch pad", "Enables launching rockets to space! After a rocket is delivered and securely attached to the tower, it will be filled with fuel based on the rocket type. This has to match with connected fuel to this tower. Fuel is never stored at the launch pad for safety reasons. Water also needs to be connected since the launch process requires a large amount of it for dampening of rocket exhaust shockwaves.");
    }
  }
}
