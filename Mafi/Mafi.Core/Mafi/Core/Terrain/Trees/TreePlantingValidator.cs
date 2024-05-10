// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.TreePlantingValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class TreePlantingValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly TerrainManager m_terrainManager;
    private readonly ITreePlantingManager m_treePlantingManager;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public TreePlantingValidator(
      TerrainManager terrainManager,
      ITreePlantingManager treePlantingManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_treePlantingManager = treePlantingManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (!(addRequest.Proto is TreeProto proto))
        return EntityValidationResult.Success;
      Assert.That<int>(addRequest.OccupiedTiles.Length).IsEqualTo(1, "We currently only support single tile trees.");
      TerrainTile terrainTile = this.m_terrainManager[addRequest.Origin.Xy];
      if (this.m_treePlantingManager.IsBlockedOrOccupied((Tile2i) terrainTile.TileCoordSlim))
      {
        if (addRequest.RecordTileErrorsAndMetadata)
          addRequest.SetTileError(0);
        return EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__SomethingInWay);
      }
      if (!this.m_treePlantingManager.IsGroundFertileAtPosition(terrainTile.TileCoord))
      {
        if (addRequest.RecordTileErrorsAndMetadata)
          addRequest.SetTileError(0);
        return EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__NotFertile);
      }
      if (this.m_treePlantingManager.HasEnoughSpacingToOtherTrees((Tile2i) terrainTile.TileCoordSlim, proto.SpacingToOtherTree))
        return EntityValidationResult.Success;
      if (addRequest.RecordTileErrorsAndMetadata)
        addRequest.SetTileError(0);
      return EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__TooCloseToOtherTree);
    }
  }
}
