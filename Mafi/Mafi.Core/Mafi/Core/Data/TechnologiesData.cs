// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Data.TechnologiesData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Data
{
  internal class TechnologiesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.CustomSurfaces, Proto.CreateStr(IdsCore.Technology.CustomSurfaces, "Custom surfaces", "Allows laying down custom surfaces, such as concrete, on the ground. Surfaces reduce the maintenance needs of vehicles that drive over them."), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/Concrete128.png")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.Recycling, Proto.CreateStr(IdsCore.Technology.Recycling, "Recycling", "Enables to recycle products in places like research labs."), new TechnologyProto.Gfx("Assets/Base/Icons/Technologies/Recycling.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.CropRotation, Proto.CreateStr(IdsCore.Technology.CropRotation, "Crop rotation", "Enables to set up crop rotation for farms. Can be used to increase farm's yield."), new TechnologyProto.Gfx("Assets/Base/Icons/Technologies/CropRotation.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.Blueprints, Proto.CreateStr(IdsCore.Technology.Blueprints, "Blueprints", "Enables to create, import and use blueprints."), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/Blueprints.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.CopyTool, Proto.CreateStrFromLocalized(IdsCore.Technology.CopyTool, (LocStrFormatted) TrCore.CopyTool, LocStrFormatted.Empty), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/Copy.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.CutTool, Proto.CreateStrFromLocalized(IdsCore.Technology.CutTool, (LocStrFormatted) TrCore.CutTool, LocStrFormatted.Empty), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/Cut.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.CloneTool, Proto.CreateStrFromLocalized(IdsCore.Technology.CloneTool, (LocStrFormatted) TrCore.CloneTool, LocStrFormatted.Empty), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/Clone128.png")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.UnityTool, Proto.CreateStrFromLocalized(IdsCore.Technology.UnityTool, (LocStrFormatted) TrCore.UnityTool, LocStrFormatted.Empty), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/UpointsTool.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.PauseTool, Proto.CreateStrFromLocalized(IdsCore.Technology.PauseTool, (LocStrFormatted) TrCore.PauseTool, LocStrFormatted.Empty), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/PauseTool.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.UpgradeTool, Proto.CreateStrFromLocalized(IdsCore.Technology.UpgradeTool, (LocStrFormatted) TrCore.UpgradeTool, LocStrFormatted.Empty), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/Upgrade.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.PlanningTool, Proto.CreateStrFromLocalized(IdsCore.Technology.PlanningTool, (LocStrFormatted) TrCore.PlanningMode, LocStrFormatted.Empty), new TechnologyProto.Gfx("Assets/Unity/UserInterface/Toolbar/Planning.svg")));
      registrator.PrototypesDb.Add<TechnologyProto>(new TechnologyProto(IdsCore.Technology.TerrainLeveling, Proto.CreateStr(IdsCore.Technology.TerrainLeveling, "Terrain Leveling", "Combines mining & dumping designations into one. Useful for terrain flattening or ramps setup. If part of terrain is below the designation, trucks will dump there. If part of terrain is above designation, it will be mined by excavators. For mining to work, the designation has to be inside of a mine tower area."), new TechnologyProto.Gfx("Assets/Unity/UserInterface/General/PlatformFlat128.png")));
    }

    public TechnologiesData()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
