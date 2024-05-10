// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Surfaces.SurfaceDecalValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Terrain.Surfaces
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class SurfaceDecalValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly TerrainManager m_terrainManager;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public SurfaceDecalValidator(TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (!(addRequest.Proto is TerrainTileSurfaceDecalProto))
        return EntityValidationResult.Success;
      Tile2i xy = addRequest.Origin.Xy;
      foreach (OccupiedTileRelative occupiedTile in addRequest.OccupiedTiles)
      {
        if (this.m_terrainManager[xy + occupiedTile.RelCoord].GetTileSurface().IsValid)
          return EntityValidationResult.Success;
      }
      if (addRequest.RecordTileErrorsAndMetadata)
      {
        for (int occupiedTileIndex = 0; occupiedTileIndex < addRequest.OccupiedTiles.Length; ++occupiedTileIndex)
          addRequest.SetTileError(occupiedTileIndex);
      }
      return EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__NotASurface);
    }
  }
}
