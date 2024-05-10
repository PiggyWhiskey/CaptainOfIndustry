// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.MineBuildingsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  internal class MineBuildingsData : IModData
  {
    private bool m_costsDisabled;

    public void DisableCosts() => this.m_costsDisabled = true;

    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.MineTowerProtoBuilder.Start("Mine control tower", Ids.Buildings.MineTower).Description("Enables assignment of excavators and trucks to designated mine areas. Only designated mining areas within the influence of the tower can be mined.").SetCost(Costs.Buildings.MineTower, this.m_costsDisabled).ShowTerrainDesignatorsOnCreation().SetLayout("(3)(3)(8)(8)", "(3)(8)(9)(9)", "(3)(8)(9)(9)", "(3)(3)(8)(8)").SetMineArea(new MineTowerProto.MineArea(new RelTile2i(5, 2), new RelTile2i(60, 60), new RelTile1i(192))).SetCategories(Ids.ToolbarCategories.BuildingsForVehicles).SetPrefabPath("Assets/Base/Buildings/MineTower.prefab").EnableInstancedRendering().BuildAndAdd().AddParam((IProtoParam) new DrawArrowWileBuildingProtoParam(4f));
    }

    public MineBuildingsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
