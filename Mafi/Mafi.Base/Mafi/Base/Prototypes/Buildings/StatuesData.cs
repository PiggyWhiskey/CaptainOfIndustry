// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.StatuesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class StatuesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto orThrow = prototypesDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.FuelGas);
      LocStr2 locStr2 = Loc.Str2(Ids.Buildings.StatueOfMaintenance.ToString() + "__desc", "Statue that not just demonstrates the wealth of your island but also serves as a celebration of your industrial success. It has such a positive effect on your population that it reduces your island's maintenance requirements by {0}. In order to get the bonus, the statue needs to be provided with '{1}' on a continuous basis. However, if you don't maintain your statue you will get a negative effect. Because nothing demotivates your workers from performing maintenance more than seeing a broken statue of maintenance. You can build this statue multiple times should your wealth allow it, but each additional statue's effect is reduced by half.", "e.g. {0} - '5%', {1} - 'Fuel gas'");
      LocStr alreadyLocalizedStr1 = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.StatueOfMaintenance.ToString() + "_formatted", locStr2.Format(StatueOfMaintenanceManager.MAINTENANCE_BONUS_BASE.ToString(), orThrow.Strings.Name.TranslatedString).Value);
      string enUs = "Text on the plaque:\r\n\r\nLife can be rough,\r\nif you don't maintain your stuff.\r\nAnd when things fail, just look -\r\na missing entry in the service book!";
      LocStr alreadyLocalizedStr2 = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.StatueOfMaintenance.ToString() + "_extraText", enUs);
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("[0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 3 * h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "   [7][7][7][7][7][7][7][7][7][7]", "   [7][7][7][7][7][7][7][7][7][7]", "   [7][7][9][8![8![8![8![9][7][7]", "   [7][7][9][8![8![8![8![7![7][7]", "   [7][7][9][8![8![8![8![7![7][7]", "A@>[7][7][9][8![8![8![8![9][7][7]", "   [7][7][9][8![8![8![8![9][7][7]", "   [7][7][9][9![9![9![9![9][7][7]", "   [7][7][9][9![9![9![9![9][7][7]", "   [7][7][7][9![9![9![9![7][7][7]", "   [7][7][7][8![8![8![8![7][7][7]");
      ProtosDb protosDb1 = prototypesDb;
      StaticEntityProto.ID statueOfMaintenance = Ids.Buildings.StatueOfMaintenance;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.StatueOfMaintenance, "The statue of maintenance", alreadyLocalizedStr1);
      EntityLayout layout1 = layoutOrThrow;
      EntityCosts entityCosts1 = Costs.Buildings.StatueOfMaintenance.MapToEntityCosts(registrator);
      Option<ProductProto> option1 = (Option<ProductProto>) orThrow;
      Duration duration1 = 30.Seconds();
      Option<StatueProto> none1 = Option<StatueProto>.None;
      Option<ProductProto> inputProduct1 = option1;
      Duration durationPerOneQuantity1 = duration1;
      LocStr extraText1 = alreadyLocalizedStr2;
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/Landmarks/StatueOfMaintenance.prefab", categories: new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Landmarks)));
      StatueProto proto1 = new StatueProto(statueOfMaintenance, str1, layout1, entityCosts1, none1, inputProduct1, durationPerOneQuantity1, extraText1, graphics1);
      StatueProto statueProto = protosDb1.Add<StatueProto>(proto1);
      ProtosDb protosDb2 = prototypesDb;
      StaticEntityProto.ID maintenanceGolden = Ids.Buildings.StatueOfMaintenanceGolden;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.StatueOfMaintenanceGolden, "The statue of maintenance (golden)", alreadyLocalizedStr1);
      EntityLayout layout2 = layoutOrThrow;
      EntityCosts entityCosts2 = Costs.Buildings.StatueOfMaintenanceGolden.MapToEntityCosts(registrator);
      Option<ProductProto> option2 = (Option<ProductProto>) orThrow;
      Duration duration2 = 30.Seconds();
      Option<StatueProto> none2 = Option<StatueProto>.None;
      Option<ProductProto> inputProduct2 = option2;
      Duration durationPerOneQuantity2 = duration2;
      LocStr extraText2 = alreadyLocalizedStr2;
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx(statueProto.Graphics.PrefabPath);
      StatueProto proto2 = new StatueProto(maintenanceGolden, str2, layout2, entityCosts2, none2, inputProduct2, durationPerOneQuantity2, extraText2, graphics2);
      protosDb2.Add<StatueProto>(proto2).SetAvailability(false);
    }

    public StatuesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
