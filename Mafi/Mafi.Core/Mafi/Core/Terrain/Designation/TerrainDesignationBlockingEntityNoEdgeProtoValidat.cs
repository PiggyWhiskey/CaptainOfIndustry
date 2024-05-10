// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.TerrainDesignationBlockingEntityNoEdgeProtoValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class TerrainDesignationBlockingEntityNoEdgeProtoValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    private readonly ITerrainDesignationsManager m_designationsManager;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public TerrainDesignationBlockingEntityNoEdgeProtoValidator(
      ITerrainDesignationsManager designationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_designationsManager = designationsManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (this.m_designationsManager.AllowDesignationsOverEntities || !(addRequest.Proto is ITerrainDesignationBlockingEntityNoEdgeProto))
        return EntityValidationResult.Success;
      bool flag = false;
      int index = 0;
      while (true)
      {
        int num = index;
        ReadOnlyArray<OccupiedVertexRelative> occupiedVertices = addRequest.OccupiedVertices;
        int length = occupiedVertices.Length;
        if (num < length)
        {
          occupiedVertices = addRequest.OccupiedVertices;
          OccupiedVertexRelative occupiedVertexRelative = occupiedVertices[index];
          Tile2i tile2i = addRequest.Origin.Xy + occupiedVertexRelative.RelCoord;
          Tile2i origin = TerrainDesignation.GetOrigin(tile2i);
          if (tile2i.X != origin.X && tile2i.Y != origin.Y && this.m_designationsManager.GetDesignationAt(tile2i).HasValue)
          {
            if (addRequest.HasAdditionalErrorTiles)
            {
              addRequest.SetTileError((int) occupiedVertexRelative.LowestTileIndex);
              flag = true;
            }
            else
              break;
          }
          ++index;
        }
        else
          goto label_9;
      }
      return EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__DesignationOverlap);
label_9:
      return !flag ? EntityValidationResult.Success : EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__DesignationOverlap);
    }
  }
}
