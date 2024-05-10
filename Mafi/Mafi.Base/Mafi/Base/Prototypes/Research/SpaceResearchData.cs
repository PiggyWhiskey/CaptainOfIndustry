// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Research.SpaceResearchData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Research;

#nullable disable
namespace Mafi.Base.Prototypes.Research
{
  internal class SpaceResearchData : IResearchNodesData, IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.ResearchNodeProtoBuilder.Start("Rocket assembly & launch", Ids.Research.RocketAssemblyAndLaunch).AddLayoutEntityToUnlock(Ids.Buildings.RocketAssemblyDepot).AddLayoutEntityToUnlock(Ids.Buildings.RocketLaunchPad).AddVehicleToUnlock(Ids.Rockets.GetRocketTransporterId(Ids.Rockets.TestingRocketT0)).AddSurfaceToUnlock(Ids.TerrainTileSurfaces.ConcreteReinforced).BuildAndAdd();
    }

    public SpaceResearchData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
