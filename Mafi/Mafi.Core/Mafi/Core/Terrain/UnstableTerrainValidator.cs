// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.UnstableTerrainValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Products;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  public class UnstableTerrainValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
  {
    /// <summary>
    /// Maximum amount of landfill allowed on tile in order to construct something there.
    /// </summary>
    private static readonly ThicknessTilesF MAX_LANDFILL_IGNORE;
    /// <summary>
    /// Minimum layer thickness that covers landfill in order to construct something on top of it.
    /// </summary>
    private static readonly ThicknessTilesF MIN_LANDFILL_COVER;
    private readonly TerrainManager m_terrainManager;
    private readonly ImmutableArray<bool> m_isUnstableLookup;
    private readonly bool m_isBedrockUnstable;

    public UnstableTerrainValidator(TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_isUnstableLookup = this.m_terrainManager.TerrainMaterials.Map<bool>((Func<TerrainMaterialProto, bool>) (x => x.HasParam<UnstableTerrainMaterialParam>()));
      this.m_isBedrockUnstable = this.m_isUnstableLookup[(int) this.m_terrainManager.Bedrock.SlimId.Value];
    }

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      Tile2i xy = addRequest.Origin.Xy;
      bool flag = false;
      foreach (OccupiedVertexRelative occupiedVertex in addRequest.OccupiedVertices)
      {
        if (isLandfill(this.m_terrainManager.GetTileIndex(xy + occupiedVertex.RelCoord)))
        {
          flag = true;
          if (addRequest.RecordTileErrorsAndMetadata)
            addRequest.SetTileError((int) occupiedVertex.LowestTileIndex);
          else
            break;
        }
      }
      return !flag ? EntityValidationResult.Success : EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__NotStable);

      bool isLandfill(Tile2iIndex tileIndex)
      {
        TileMaterialLayers layersRawData = this.m_terrainManager.GetLayersRawData(tileIndex);
        if (layersRawData.Count <= 0)
          return this.m_isBedrockUnstable;
        ThicknessTilesF thicknessTilesF1;
        ThicknessTilesF thicknessTilesF2;
        if (this.m_isUnstableLookup[layersRawData.First.SlimIdRaw])
        {
          thicknessTilesF1 = layersRawData.First.Thickness;
          if (thicknessTilesF1 > UnstableTerrainValidator.MAX_LANDFILL_IGNORE)
            return true;
          thicknessTilesF2 = ThicknessTilesF.Zero;
        }
        else
        {
          thicknessTilesF2 = layersRawData.First.Thickness;
          if (thicknessTilesF2 >= UnstableTerrainValidator.MIN_LANDFILL_COVER)
            return false;
          thicknessTilesF1 = ThicknessTilesF.Zero;
        }
        if (layersRawData.Count == 1)
          return this.m_isBedrockUnstable;
        if (this.m_isUnstableLookup[layersRawData.Second.SlimIdRaw])
        {
          thicknessTilesF1 += layersRawData.Second.Thickness;
          if (thicknessTilesF1 > UnstableTerrainValidator.MAX_LANDFILL_IGNORE)
            return true;
        }
        else
        {
          thicknessTilesF2 += layersRawData.Second.Thickness;
          if (thicknessTilesF2 >= UnstableTerrainValidator.MIN_LANDFILL_COVER)
            return false;
        }
        if (layersRawData.Count == 2)
          return this.m_isBedrockUnstable;
        if (this.m_isUnstableLookup[layersRawData.Third.SlimIdRaw])
        {
          thicknessTilesF1 += layersRawData.Third.Thickness;
          if (thicknessTilesF1 > UnstableTerrainValidator.MAX_LANDFILL_IGNORE)
            return true;
        }
        else if (thicknessTilesF2 + layersRawData.Third.Thickness >= UnstableTerrainValidator.MIN_LANDFILL_COVER)
          return false;
        if (layersRawData.Count == 3)
          return this.m_isBedrockUnstable;
        return this.m_isUnstableLookup[layersRawData.Fourth.SlimIdRaw] && thicknessTilesF1 + layersRawData.Fourth.Thickness > UnstableTerrainValidator.MAX_LANDFILL_IGNORE;
      }
    }

    static UnstableTerrainValidator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnstableTerrainValidator.MAX_LANDFILL_IGNORE = 0.25.TilesThick();
      UnstableTerrainValidator.MIN_LANDFILL_COVER = 0.75.TilesThick();
    }
  }
}
