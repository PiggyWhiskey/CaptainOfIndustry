// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SolarPanelsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class SolarPanelsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb1 = registrator.PrototypesDb;
      StaticEntityProto.ID solarPanel = (StaticEntityProto.ID) Ids.Machines.SolarPanel;
      Proto.Str str1 = Proto.CreateStr((Proto.ID) Ids.Machines.SolarPanel, "Solar panel", "Converts sunlight to electricity. Surprisingly, the efficiency depends on how sunny it is.");
      EntityLayout layoutOrThrow1 = registrator.LayoutParser.ParseLayoutOrThrow("[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]");
      EntityCosts entityCosts1 = Costs.Machines.SolarPanel.MapToEntityCosts(registrator);
      Option<SolarElectricityGeneratorProto> none1 = (Option<SolarElectricityGeneratorProto>) Option.None;
      Electricity outputElectricity1 = 128.Kw();
      ImmutableArray<ToolbarCategoryProto>? categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity));
      LayoutEntityProto.Gfx graphics1 = new LayoutEntityProto.Gfx("Assets/Base/Machines/PowerPlant/SolarPanePoly2x2.prefab", categories: categories, useInstancedRendering: true);
      SolarElectricityGeneratorProto proto1 = new SolarElectricityGeneratorProto(solarPanel, str1, layoutOrThrow1, entityCosts1, none1, outputElectricity1, graphics1);
      prototypesDb1.Add<SolarElectricityGeneratorProto>(proto1);
      LocStr1 locStr1 = Loc.Str1(Ids.Machines.SolarPanelMono.ToString() + "__desc", "Solar panels that are made from a single crystal silicon. That makes them more expensive to produce but they provide {0}% more energy.", "solar panel description, example use of {0}: 'they provide 25% more energy'");
      ProtosDb prototypesDb2 = registrator.PrototypesDb;
      StaticEntityProto.ID solarPanelMono = (StaticEntityProto.ID) Ids.Machines.SolarPanelMono;
      Proto.Str str2 = Proto.CreateStr((Proto.ID) Ids.Machines.SolarPanelMono, "Solar panel (mono)", LocalizationManager.CreateAlreadyLocalizedFormatted(Ids.Machines.SolarPanelMono.Value, locStr1.Format(25.ToString())));
      EntityLayout layoutOrThrow2 = registrator.LayoutParser.ParseLayoutOrThrow("[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]", "[4][4][4][4][4][4][4][4][4][4]");
      EntityCosts entityCosts2 = Costs.Machines.SolarPanelMono.MapToEntityCosts(registrator);
      Option<SolarElectricityGeneratorProto> none2 = (Option<SolarElectricityGeneratorProto>) Option.None;
      Electricity outputElectricity2 = 160.Kw();
      categories = new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.MachinesElectricity));
      LayoutEntityProto.Gfx graphics2 = new LayoutEntityProto.Gfx("Assets/Base/Machines/PowerPlant/SolarPaneMono2x2.prefab", categories: categories, useInstancedRendering: true);
      SolarElectricityGeneratorProto proto2 = new SolarElectricityGeneratorProto(solarPanelMono, str2, layoutOrThrow2, entityCosts2, none2, outputElectricity2, graphics2);
      prototypesDb2.Add<SolarElectricityGeneratorProto>(proto2);
    }

    public SolarPanelsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
