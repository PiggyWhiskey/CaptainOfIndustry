// Decompiled with JetBrains decompiler
// Type: Mafi.Base.StartingFactoryPlan
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Terrain;

#nullable disable
namespace Mafi.Base
{
  public class StartingFactoryPlan
  {
    private readonly TerrainManager m_terrainManager;
    private readonly EntitiesBuilder m_entitiesBuilder;
    private Tile2i m_planPosition;

    public Rotation90 Rotation { get; private set; }

    public Tile3i? Housing1Pos { get; private set; }

    public Tile3i? Housing2Pos { get; private set; }

    public Tile3i? ShipyardPos { get; private set; }

    public Tile3i? RadioTowerPos { get; private set; }

    public StartingFactoryPlan(TerrainManager terrainManager, EntitiesBuilder entitiesBuilder)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_entitiesBuilder = entitiesBuilder;
    }

    public void Initialize(Rotation90 rotation, Tile2i planPosition)
    {
      this.Rotation = rotation;
      this.m_planPosition = planPosition;
      this.Housing1Pos = new Tile3i?();
      this.Housing2Pos = new Tile3i?();
      this.ShipyardPos = new Tile3i?();
      this.RadioTowerPos = new Tile3i?();
    }

    public bool IsValid()
    {
      return this.Housing1Pos.HasValue && this.Housing2Pos.HasValue && this.ShipyardPos.HasValue && this.RadioTowerPos.HasValue;
    }

    public bool TryAddShipyard(Option<Lyst<Pair<Tile3i, Option<string>>>> results)
    {
      RelTile2i relTile2i1 = new RelTile2i(20, 0).Rotate(this.Rotation);
      RelTile2i relTile2i2 = new RelTile2i(-20, 0).Rotate(this.Rotation);
      Tile2i fromLocation = this.m_planPosition + relTile2i1;
      Tile2i toLocation = this.m_planPosition + relTile2i2;
      HeightTilesI tilesHeightRounded = this.m_terrainManager.GetHeight(this.m_planPosition).TilesHeightRounded;
      Tile3i firstBuildPosition;
      if (!this.m_entitiesBuilder.CanBuildLayoutEntityApprox(Ids.Buildings.Shipyard, fromLocation, toLocation, tilesHeightRounded, out firstBuildPosition, this.Rotation, results: results))
        return false;
      this.ShipyardPos = new Tile3i?(firstBuildPosition);
      return true;
    }

    public bool TryPlaceRadioTower(Option<Lyst<Pair<Tile3i, Option<string>>>> results)
    {
      HeightTilesI tilesHeightRounded = this.m_terrainManager.GetHeight(this.m_planPosition).TilesHeightRounded;
      Tile2i tile2i1 = this.m_planPosition + new RelTile2i(15, 0).Rotate(this.Rotation);
      Tile2i tile2i2 = this.m_planPosition + new RelTile2i(60, -20).Rotate(this.Rotation);
      EntitiesBuilder entitiesBuilder = this.m_entitiesBuilder;
      StaticEntityProto.ID ruins = Ids.Buildings.Ruins;
      Tile2i fromLocation = tile2i2;
      Tile2i toLocation = tile2i1;
      HeightTilesI height = tilesHeightRounded;
      Tile3i tile3i;
      ref Tile3i local = ref tile3i;
      Option<Lyst<Pair<Tile3i, Option<string>>>> option = results;
      Rotation90 rotation = new Rotation90();
      Option<Lyst<Pair<Tile3i, Option<string>>>> results1 = option;
      if (!entitiesBuilder.CanBuildLayoutEntityApprox(ruins, fromLocation, toLocation, height, out local, rotation, results: results1))
        return false;
      this.RadioTowerPos = new Tile3i?(tile3i);
      return true;
    }

    public bool TryPlaceSettlement(
      SettlementHousingModuleProto settlementProto,
      Option<Lyst<Pair<Tile3i, Option<string>>>> results)
    {
      Tile2i tile = this.m_planPosition + new RelTile2i(78, 50).Rotate(this.Rotation);
      HeightTilesI tilesHeightRounded = this.m_terrainManager.GetHeight(tile).TilesHeightRounded;
      int[] numArray = new int[5]{ 0, 1, -1, 2, -2 };
      foreach (int num1 in numArray)
      {
        foreach (int num2 in numArray)
        {
          RelTile2i relTile2i1 = new RelTile2i(num2 * 20, num1 * 20);
          RelTile2i relTile2i2 = relTile2i1.Rotate(this.Rotation);
          Tile3i tile3i1 = (tile + relTile2i2).ExtendHeight(tilesHeightRounded);
          Tile3i tile3i2 = tile3i1;
          relTile2i1 = new RelTile2i(0, settlementProto.Layout.LayoutSize.Y);
          relTile2i1 = relTile2i1.Rotate(this.Rotation);
          RelTile3i relTile3i = relTile2i1.ExtendZ(0);
          Tile3i tile3i3 = tile3i2 + relTile3i;
          string error1;
          bool flag1 = this.m_entitiesBuilder.CanBuildLayoutEntity(settlementProto.Id, tile3i1, Rotation90.Deg0, false, out error1);
          if (results.HasValue)
            results.Value.Add(Pair.Create<Tile3i, Option<string>>(tile3i1, string.IsNullOrEmpty(error1) ? Option<string>.None : (Option<string>) error1));
          if (flag1)
          {
            string error2;
            bool flag2 = this.m_entitiesBuilder.CanBuildLayoutEntity(settlementProto.Id, tile3i3, Rotation90.Deg90, false, out error2);
            if (results.HasValue)
              results.Value.Add(Pair.Create<Tile3i, Option<string>>(tile3i3, string.IsNullOrEmpty(error2) ? Option<string>.None : (Option<string>) error2));
            if (flag2)
            {
              this.Housing1Pos = new Tile3i?(tile3i1);
              this.Housing2Pos = new Tile3i?(tile3i3);
              return true;
            }
          }
        }
      }
      return false;
    }
  }
}
