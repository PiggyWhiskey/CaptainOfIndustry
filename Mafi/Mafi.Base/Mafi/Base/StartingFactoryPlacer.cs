// Decompiled with JetBrains decompiler
// Type: Mafi.Base.StartingFactoryPlacer
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Base
{
  [DependencyRegisteredManually("")]
  public class StartingFactoryPlacer
  {
    private readonly ProtosDb m_protosDb;
    private readonly StartingFactoryPlan m_factoryPlan;

    public StartingFactoryPlacer(
      TerrainManager terrainManager,
      EntitiesBuilder entitiesBuilder,
      ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_factoryPlan = new StartingFactoryPlan(terrainManager, entitiesBuilder);
    }

    public bool TryCreatePlan(
      Tile2i planPosition,
      Rotation90 rotation,
      out StartingFactoryPlan planShared,
      out string error,
      Option<Lyst<Pair<Tile3i, Option<string>>>> results)
    {
      planShared = this.m_factoryPlan;
      planShared.Initialize(rotation, planPosition);
      if (!planShared.TryAddShipyard(results))
      {
        error = "Failed to build shipyard.";
        return false;
      }
      if (!planShared.TryPlaceRadioTower(results))
      {
        error = "Failed to place radio tower.";
        return false;
      }
      Option<SettlementHousingModuleProto> option = this.m_protosDb.Get<SettlementHousingModuleProto>((Proto.ID) Ids.Buildings.Housing);
      if (option.IsNone)
      {
        error = "Failed to get housing proto.";
        return false;
      }
      if (this.m_protosDb.Get<SettlementFoodModuleProto>((Proto.ID) Ids.Buildings.SettlementFoodModule).IsNone)
      {
        error = "Failed to get food proto.";
        return false;
      }
      if (!planShared.TryPlaceSettlement(option.Value, results))
      {
        error = "Failed to place settlement.";
        return false;
      }
      error = "";
      return true;
    }
  }
}
