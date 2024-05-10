// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerraformingCommandsProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TerraformingCommandsProcessor : 
    ICommandProcessor<TerraformerDepositCmd>,
    IAction<TerraformerDepositCmd>,
    ICommandProcessor<TerraformerRemoveCmd>,
    IAction<TerraformerRemoveCmd>
  {
    private readonly TerrainManager m_terrainManager;
    private readonly ProtosDb m_protosDb;

    public TerraformingCommandsProcessor(TerrainManager terrainManager, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_protosDb = protosDb;
    }

    public void Invoke(TerraformerDepositCmd cmd)
    {
      Option<TerrainMaterialProto> option = this.m_protosDb.Get<TerrainMaterialProto>(cmd.MaterialId);
      if (option.IsNone)
      {
        Assert.Fail<Proto.ID>("Unknown or not loose product '{0}'.", cmd.MaterialId);
        cmd.SetResultError(string.Format("Unknown or not loose product '{0}'.", (object) cmd.MaterialId));
      }
      else
      {
        foreach (Tile2i enumerateVertex in cmd.Selection.EnumerateVertices())
        {
          TerrainTile terrainTile = this.m_terrainManager[enumerateVertex];
          ThicknessTilesF thickness = cmd.Height - terrainTile.Height;
          if (!thickness.IsNotPositive)
            this.m_terrainManager.DumpMaterial(terrainTile.CoordAndIndex, new TerrainMaterialThicknessSlim(option.Value, thickness));
        }
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(TerraformerRemoveCmd cmd)
    {
      TilesRectSelection tilesRectSelection = cmd.Selection;
      tilesRectSelection = tilesRectSelection.EnumerateVertices();
      foreach (Tile2i coord in tilesRectSelection)
      {
        TerrainTile terrainTile = this.m_terrainManager[coord];
label_2:
        if (terrainTile.Height > cmd.TargetHeight && !this.m_terrainManager.MineMaterial(terrainTile.CoordAndIndex, terrainTile.Height - cmd.TargetHeight).IsEmpty)
          goto label_2;
      }
      cmd.SetResultSuccess();
    }
  }
}
