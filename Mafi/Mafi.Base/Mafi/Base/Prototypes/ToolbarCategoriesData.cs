// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.ToolbarCategoriesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes
{
  internal class ToolbarCategoriesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      string translationComment = "toolbar category name";
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Transports, Proto.CreateStr(Ids.ToolbarCategories.Transports, "Transports", translationComment: translationComment), 110f, "Assets/Unity/UserInterface/Toolbar/Transports.svg", true, true, "TRANSPORT"));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Machines, Proto.CreateStr(Ids.ToolbarCategories.Machines, "General machines", translationComment: translationComment), 10f, "Assets/Unity/UserInterface/Toolbar/Machines.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.MachinesWater, Proto.CreateStr(Ids.ToolbarCategories.MachinesWater, "Water extraction & processing", translationComment: translationComment), 12f, "Assets/Unity/UserInterface/Toolbar/WaterMachines.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.MachinesFood, Proto.CreateStr(Ids.ToolbarCategories.MachinesFood, "Food production", translationComment: translationComment), 15f, "Assets/Unity/UserInterface/Toolbar/Farms.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.MachinesMetallurgy, Proto.CreateStr(Ids.ToolbarCategories.MachinesMetallurgy, "Metallurgy & smelting", translationComment: translationComment), 20f, "Assets/Unity/UserInterface/Toolbar/Metallurgy.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.MachinesElectricity, Proto.CreateStr(Ids.ToolbarCategories.MachinesElectricity, "Power production", translationComment: translationComment), 30f, "Assets/Unity/UserInterface/Toolbar/Power.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.MachinesOil, Proto.CreateStr(Ids.ToolbarCategories.MachinesOil, "Crude oil refining", translationComment: translationComment), 40f, "Assets/Unity/UserInterface/Toolbar/Oil.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Waste, Proto.CreateStr(Ids.ToolbarCategories.Waste, "Waste management", translationComment: translationComment), 50f, "Assets/Unity/UserInterface/Toolbar/Waste.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Storages, Proto.CreateStr(Ids.ToolbarCategories.Storages, "Storage", translationComment: translationComment), 210f, "Assets/Unity/UserInterface/Toolbar/Storages.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Buildings, Proto.CreateStr(Ids.ToolbarCategories.Buildings, "Buildings", translationComment: translationComment), 220f, "Assets/Unity/UserInterface/Toolbar/Buildings.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.BuildingsForVehicles, Proto.CreateStr(Ids.ToolbarCategories.BuildingsForVehicles, "Buildings (for vehicles)", translationComment: translationComment), 230f, "Assets/Unity/UserInterface/Toolbar/VehicleStructures.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Housing, Proto.CreateStr(Ids.ToolbarCategories.Housing, "Housing & services", translationComment: translationComment), 240f, "Assets/Unity/UserInterface/Toolbar/Settlement.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Docks, Proto.CreateStr(Ids.ToolbarCategories.Docks, "Cargo docks", translationComment: translationComment), 250f, "Assets/Unity/UserInterface/Toolbar/CargoShip.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(Ids.ToolbarCategories.Landmarks, Proto.CreateStr(Ids.ToolbarCategories.Landmarks, "Landmarks & decorations", translationComment: translationComment), 260f, "Assets/Unity/UserInterface/Toolbar/Landmarks.svg", true));
      prototypesDb.Add<ToolbarCategoryProto>(new ToolbarCategoryProto(IdsCore.ToolbarCategories.Surfaces, Proto.CreateStr(IdsCore.ToolbarCategories.Surfaces, "Surfaces", translationComment: translationComment), 340f, "Assets/Unity/UserInterface/Toolbar/Concrete128.png", true, shortcutId: "SURFACE"));
    }

    public ToolbarCategoriesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
