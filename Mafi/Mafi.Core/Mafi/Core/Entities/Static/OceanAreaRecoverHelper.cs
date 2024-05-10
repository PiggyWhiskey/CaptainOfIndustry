// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.OceanAreaRecoverHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Core.Population;
using Mafi.Core.Terrain;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class OceanAreaRecoverHelper : 
    ICommandProcessor<RecoverOceanAccessCmd>,
    IAction<RecoverOceanAccessCmd>
  {
    public static readonly Upoints RECOVERY_COST;
    public static readonly int DEFAULT_TILES_RECOVERED;
    private readonly StaticEntityOceanReservationManager m_areaReservationManager;
    private readonly TerrainManager m_terrainManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly UpointsManager m_upointsManager;
    private readonly Lyst<IOceanAreaRecord> m_areasTmp;

    public OceanAreaRecoverHelper(
      StaticEntityOceanReservationManager areaReservationManager,
      TerrainManager terrainManager,
      IEntitiesManager entitiesManager,
      UpointsManager upointsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_areasTmp = new Lyst<IOceanAreaRecord>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_areaReservationManager = areaReservationManager;
      this.m_terrainManager = terrainManager;
      this.m_entitiesManager = entitiesManager;
      this.m_upointsManager = upointsManager;
    }

    public RecoverOceanResult TryRecoverOceanAccess(
      IStaticEntityWithReservedOcean entity,
      int maxTilesToFix)
    {
      this.m_areasTmp.Clear();
      this.m_areaReservationManager.GetOceanAreasFor(entity, this.m_areasTmp);
      if (this.m_areasTmp.IsEmpty)
        Log.Error("Failed to recover ocean access, no areas.");
      if (!this.m_upointsManager.CanConsume(OceanAreaRecoverHelper.RECOVERY_COST))
        return new RecoverOceanResult();
      int index1 = 0;
      int num1 = int.MaxValue;
      ImmutableArray<ImmutableArray<RectangleTerrainArea2i>> areasSets = entity.ReservedOceanAreaState.AreasSets;
      for (int index2 = 0; index2 < areasSets.Length; ++index2)
      {
        int num2 = 0;
        foreach (IOceanAreaRecord oceanAreaRecord in this.m_areasTmp)
        {
          if (oceanAreaRecord.SetIndex == index2)
          {
            int totalBlocked;
            int countBlockedByTerrainHeight;
            this.m_areaReservationManager.ExplainAreaValidity(oceanAreaRecord.Area, out totalBlocked, out int _, out countBlockedByTerrainHeight, out int _);
            Assert.That<int>(totalBlocked).IsEqualTo(oceanAreaRecord.NonOceanTiles);
            num2 += countBlockedByTerrainHeight;
          }
        }
        if (num2 > 0 && num2 < num1)
        {
          num1 = num2;
          index1 = index2;
        }
      }
      int num3 = maxTilesToFix;
      HeightTilesF height = StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT - ThicknessTilesF.One;
      int blockedByTerrainCount = 0;
      foreach (RectangleTerrainArea2i rectangleTerrainArea2i in areasSets[index1])
      {
        foreach (Tile2iAndIndex enumerateTilesAndIndex in rectangleTerrainArea2i.ClampToTerrainBounds(this.m_terrainManager).EnumerateTilesAndIndices(this.m_terrainManager))
        {
          if (this.m_terrainManager.GetHeight(enumerateTilesAndIndex.Index) > StaticEntityOceanReservationManager.MAX_OCEAN_FLOOR_HEIGHT)
          {
            if (maxTilesToFix > 0)
            {
              this.m_terrainManager.SetHeight(enumerateTilesAndIndex, height);
              --maxTilesToFix;
            }
            else
              ++blockedByTerrainCount;
          }
        }
      }
      if (maxTilesToFix < num3)
        this.m_upointsManager.ConsumeExactly(IdsCore.UpointsCategories.QuickRemove, OceanAreaRecoverHelper.RECOVERY_COST, new Option<IEntity>(), new LocStr?());
      return new RecoverOceanResult(blockedByTerrainCount, EntityId.Invalid);
    }

    public void Invoke(RecoverOceanAccessCmd cmd)
    {
      IStaticEntityWithReservedOcean entity;
      if (!this.m_entitiesManager.TryGetEntity<IStaticEntityWithReservedOcean>(cmd.EntityWithOceanAreas, out entity))
        cmd.SetResultError(new RecoverOceanResult(), string.Format("Failed to get entity {0} ", (object) cmd.EntityWithOceanAreas) + "as IStaticEntityWithReservedOcean.");
      RecoverOceanResult result = this.TryRecoverOceanAccess(entity, cmd.MaxTilesToRecover);
      cmd.SetResultSuccess(result);
    }

    static OceanAreaRecoverHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      OceanAreaRecoverHelper.RECOVERY_COST = 10.Upoints();
      OceanAreaRecoverHelper.DEFAULT_TILES_RECOVERED = 50;
    }
  }
}
