// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.ForestProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [OnlyForSaveCompatibility(null)]
  public class ForestProto : Proto
  {
    public readonly TerrainMaterialProto ForestFloorMaterial;
    public readonly ImmutableArray<TreeProto> Trees;

    public ForestProto(
      Proto.ID id,
      Proto.Str strings,
      TerrainMaterialProto forestFloorMaterial,
      ImmutableArray<TreeProto> trees)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings);
      this.Trees = trees;
      this.ForestFloorMaterial = forestFloorMaterial;
      foreach (TreeProto tree in trees)
        tree.SetForestProto(this);
    }
  }
}
