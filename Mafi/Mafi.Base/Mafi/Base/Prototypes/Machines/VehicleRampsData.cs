// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.VehicleRampsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Ramps;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  public class VehicleRampsData : IModData
  {
    private static readonly LocStr RAMPS_DESC;

    public void RegisterData(ProtoRegistrator registrator)
    {
      EntityLayoutParams layoutParams = new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[5]
      {
        new CustomLayoutToken("=0=", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightFrom = h - 1;
          int heightToExcl = h;
          int? nullable1 = new int?(h - 1);
          Fix32? nullable2 = new Fix32?(EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT.Value);
          Proto.ID? nullable3 = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = nullable1;
          Fix32? vehicleHeight = nullable2;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable3;
          return new LayoutTokenSpec(heightFrom, heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("_0=", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightFrom = h - 1;
          int heightToExcl = h;
          int? nullable4 = new int?(h - 1);
          Fix32? nullable5 = new Fix32?((Fix32) (h - 1));
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = nullable4;
          Fix32? vehicleHeight = nullable5;
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightFrom, heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        })),
        new CustomLayoutToken("<R1", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: 1, surfaceId: new Proto.ID?(p.HardenedFloorSurfaceId), isRamp: true))),
        new CustomLayoutToken("<R2", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(1, 2, maxTerrainHeight: new int?(1), isRamp: true))),
        new CustomLayoutToken("=23", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(1, 3, maxTerrainHeight: new int?(1), vehicleHeight: new Fix32?(EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT.Value))))
      });
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      StaticEntityProto.ID vehicleRamp = Ids.Buildings.VehicleRamp;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.VehicleRamp, "Vehicle ramp (small)", VehicleRampsData.RAMPS_DESC);
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow(layoutParams, "[1][1][2][2]=2==2==2=[2][2][1][1]", "_1_<R1<R1<R1_2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=<R1<R1<R1_1_", "[1][1][2][2]=2==2==2=[2][2][1][1]");
      EntityCosts entityCosts1 = Costs.Buildings.VehicleRamp.MapToEntityCosts(registrator);
      ImmutableArray<ToolbarCategoryProto>? categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.BuildingsForVehicles));
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/VehicleRamp.prefab", categories: categories, useInstancedRendering: true);
      VehicleRampProto proto1 = new VehicleRampProto(vehicleRamp, str1, layoutOrThrow1, entityCosts1, graphics1);
      prototypesDb1.Add<VehicleRampProto>(proto1);
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      StaticEntityProto.ID vehicleRamp2 = Ids.Buildings.VehicleRamp2;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.VehicleRamp2, "Vehicle ramp (medium)", VehicleRampsData.RAMPS_DESC);
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow(layoutParams, "[1][1][2][2]=2==2==2=[2]=2==2==2=[2][2][1][1]", "_1_<R1<R1<R1_2=_2=_2=_2__2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=_2__2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=_2__2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=_2__2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=_2__2=_2=_2=<R1<R1<R1_1_", "_1_<R1<R1<R1_2=_2=_2=_2__2=_2=_2=<R1<R1<R1_1_", "[1][1][2][2]=2==2==2=[2]=2==2==2=[2][2][1][1]");
      EntityCosts entityCosts2 = Costs.Buildings.VehicleRamp2.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.BuildingsForVehicles));
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/VehicleRamp2.prefab", categories: categories, useInstancedRendering: true);
      VehicleRampProto proto2 = new VehicleRampProto(vehicleRamp2, str2, layoutOrThrow2, entityCosts2, graphics2);
      prototypesDb2.Add<VehicleRampProto>(proto2);
      ProtosDb prototypesDb3 = registrator.PrototypesDb;
      StaticEntityProto.ID vehicleRamp3 = Ids.Buildings.VehicleRamp3;
      Proto.Str str3 = Proto.CreateStr((Proto.ID) Ids.Buildings.VehicleRamp3, "Vehicle ramp (large)", VehicleRampsData.RAMPS_DESC);
      EntityLayout layoutOrThrow3 = registrator.LayoutParser.ParseLayoutOrThrow(layoutParams, "[1][1][2][2]=23=23=23[3]=3==3==3==3=[3]=23=23=23[2][2][1][1]", "_1_<R1<R1<R1<R2<R2<R2_3__3=_3=_3=_3=_3_<R2<R2<R2<R1<R1<R1_1_", "_1_<R1<R1<R1<R2<R2<R2_3__3=_3=_3=_3=_3_<R2<R2<R2<R1<R1<R1_1_", "_1_<R1<R1<R1<R2<R2<R2_3__3=_3=_3=_3=_3_<R2<R2<R2<R1<R1<R1_1_", "_1_<R1<R1<R1<R2<R2<R2_3__3=_3=_3=_3=_3_<R2<R2<R2<R1<R1<R1_1_", "_1_<R1<R1<R1<R2<R2<R2_3__3=_3=_3=_3=_3_<R2<R2<R2<R1<R1<R1_1_", "_1_<R1<R1<R1<R2<R2<R2_3__3=_3=_3=_3=_3_<R2<R2<R2<R1<R1<R1_1_", "[1][1][2][2]=23=23=23[3]=3==3==3==3=[3]=23=23=23[2][2][1][1]");
      EntityCosts entityCosts3 = Costs.Buildings.VehicleRamp3.MapToEntityCosts(registrator);
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.BuildingsForVehicles));
      LayoutEntityProto.Gfx graphics3 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/VehicleRamp3.prefab", categories: categories, useInstancedRendering: true);
      VehicleRampProto proto3 = new VehicleRampProto(vehicleRamp3, str3, layoutOrThrow3, entityCosts3, graphics3);
      prototypesDb3.Add<VehicleRampProto>(proto3);
    }

    public VehicleRampsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleRampsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      VehicleRampsData.RAMPS_DESC = Loc.Str("VehicleRamp__desc", "Allows vehicles to cross obstacles such as transports.", "description for vehicle ramps");
    }
  }
}
