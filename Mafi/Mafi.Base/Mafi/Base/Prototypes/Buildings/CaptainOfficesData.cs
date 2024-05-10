// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.CaptainOfficesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class CaptainOfficesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      string[] array = new string[20]
      {
        "(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)",
        "(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)",
        "(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)",
        "(2)(2)(2)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(2)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(8)(7)(7)(2)(2)",
        "(2)(2)(2)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(7)(2)(2)(2)",
        "(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)",
        "(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)",
        "(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)(2)"
      };
      string[] strArray = array.MapArray<string, string>((Func<string, string>) (x => x.Replace('8', '5').Replace('7', '5')));
      LocStr descShort = Loc.Str("HeadquartersT1__desc", "Enables you to declare edicts that can significantly affect your island's population and industry. Some edicts can provide you with benefits but may cost you monthly Unity. Other edicts can generate monthly Unity, but these typically require you to provides something in return. Edicts can be toggled anytime.", "");
      LocStr1 locStr1 = Loc.Str1("HeadquartersT1__QuickTrade__desc", "Also adds {0} increase in quick trade volume.", "example use 'Also adds +100% increase in quick trade volume'");
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      StaticEntityProto.ID captainOfficeT2 = Ids.Buildings.CaptainOfficeT2;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Buildings.CaptainOfficeT2, "Captain's office II", LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.CaptainOfficeT2.Value + "__desc", descShort.ToString() + " " + locStr1.Format("+100%").ToString()));
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow(array);
      EntityCosts entityCosts1 = Costs.Buildings.CaptainOfficeT2.MapToEntityCosts(registrator);
      Electricity electricityConsumed1 = 250.Kw();
      Percent tradeVolumeDiff1 = 100.Percent();
      Option<CaptainOfficeProto> none1 = (Option<CaptainOfficeProto>) Option.None;
      int? emissionIntensity1 = new int?(2);
      Option<string> none2 = Option<string>.None;
      ImmutableArray<ToolbarCategoryProto>? categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings));
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/CaptainOfficeT2.prefab", customIconPath: none2, categories: categories);
      CaptainOfficeProto proto1 = new CaptainOfficeProto(captainOfficeT2, str1, layoutOrThrow1, entityCosts1, electricityConsumed1, tradeVolumeDiff1, none1, true, emissionIntensity1, graphics1);
      CaptainOfficeProto captainOfficeProto = prototypesDb1.Add<CaptainOfficeProto>(proto1);
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      StaticEntityProto.ID captainOfficeT1 = Ids.Buildings.CaptainOfficeT1;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Buildings.CaptainOfficeT1, "Captain's office I", descShort);
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow(strArray);
      EntityCosts entityCosts2 = Costs.Buildings.CaptainOfficeT1.MapToEntityCosts(registrator);
      Electricity electricityConsumed2 = 100.Kw();
      Percent tradeVolumeDiff2 = 0.Percent();
      Option<CaptainOfficeProto> nextTier = (Option<CaptainOfficeProto>) captainOfficeProto;
      int? emissionIntensity2 = new int?(2);
      Option<string> none3 = Option<string>.None;
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Buildings));
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx("Assets/Base/Buildings/CaptainOfficeT1.prefab", customIconPath: none3, categories: categories);
      CaptainOfficeProto proto2 = new CaptainOfficeProto(captainOfficeT1, str2, layoutOrThrow2, entityCosts2, electricityConsumed2, tradeVolumeDiff2, nextTier, false, emissionIntensity2, graphics2);
      prototypesDb2.Add<CaptainOfficeProto>(proto2);
    }

    public CaptainOfficesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
