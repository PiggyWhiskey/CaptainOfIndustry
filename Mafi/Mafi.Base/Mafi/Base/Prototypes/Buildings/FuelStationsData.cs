// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.FuelStationsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class FuelStationsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      LocStr desc = Loc.Str(Ids.Buildings.FuelStationT1.ToString() + "__desc", "Trucks assigned to a fuel station will automatically refuel excavators and tree harvesters at their working site so they don't waste their time going for fuel on their own.", "");
      LocStr locStr = Loc.Str(Ids.Buildings.FuelStationT2.ToString() + "__desc", "Provides increased storage and refueling rate compared to the previous tier.", "advanced fuel station description");
      LocStr alreadyLocalizedStr = LocalizationManager.CreateAlreadyLocalizedStr(Ids.Buildings.FuelStationT2.ToString() + "_concat", desc.TranslatedString + " " + locStr.TranslatedString);
      registrator.FuelStationProtoBuilder.Start("Hydrogen fuel station", Ids.Buildings.FuelStationHydrogenT1).Description(desc).SetCost(Costs.Buildings.FuelStationT3).SetFuelProto((Proto.ID) Ids.Products.Hydrogen).SetCapacity(240).SetMaxTransferQuantityPerVehicle(20).SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetLayout("   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "A@>(3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)").SetPrefabPath("Assets/Base/Buildings/FuelStations/HydrogenFuelStationT1.prefab").EnableInstancedRendering().BuildAndAdd();
      FuelStationProto nextTier1 = registrator.FuelStationProtoBuilder.Start("Fuel station III", Ids.Buildings.FuelStationT3).Description(alreadyLocalizedStr).SetCost(Costs.Buildings.FuelStationT3).SetFuelProto((Proto.ID) Ids.Products.Diesel).SetCapacity(360).SetMaxTransferQuantityPerVehicle(30).SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetLayout(new EntityLayoutParams(), "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "A@>(3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)").SetPrefabPath("Assets/Base/Buildings/FuelStations/FuelStationT3.prefab").EnableInstancedRendering().BuildAndAdd();
      FuelStationProto nextTier2 = registrator.FuelStationProtoBuilder.Start("Fuel station II", Ids.Buildings.FuelStationT2).Description(alreadyLocalizedStr).SetCost(Costs.Buildings.FuelStationT2).SetFuelProto((Proto.ID) Ids.Products.Diesel).SetCapacity(120).SetMaxTransferQuantityPerVehicle(15).SetNextTier(nextTier1).SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetLayout("   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "A@>(3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)").SetPrefabPath("Assets/Base/Buildings/FuelStations/FuelStationT2.prefab").EnableInstancedRendering().BuildAndAdd();
      registrator.FuelStationProtoBuilder.Start("Fuel station", Ids.Buildings.FuelStationT1).Description(desc).SetCost(Costs.Buildings.FuelStationT1).SetCapacity(40).SetMaxTransferQuantityPerVehicle(5).SetFuelProto((Proto.ID) Ids.Products.Diesel).SetNextTier(nextTier2).SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetLayout("   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "A@>(3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)", "   (3)(3)(3)(3)(3)(3)").SetPrefabPath("Assets/Base/Buildings/FuelStations/FuelStationT1.prefab").EnableInstancedRendering().BuildAndAdd();
    }

    public FuelStationsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
