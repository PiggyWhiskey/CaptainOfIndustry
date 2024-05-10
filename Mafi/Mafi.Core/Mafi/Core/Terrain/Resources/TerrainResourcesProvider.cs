// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Resources.TerrainResourcesProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Resources
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TerrainResourcesProvider
  {
    /// <summary>Terrain products that should be visualized.</summary>
    public readonly HybridSet<LooseProductProto> LooseTerrainProducts;
    /// <summary>
    /// Products in terrain represented as VirtualResources (Oil for example).
    /// </summary>
    public readonly HybridSet<VirtualResourceProductProto> VirtualResourceProducts;

    public TerrainResourcesProvider(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LooseTerrainProducts = HybridSet<LooseProductProto>.From(protosDb.Filter<TerrainMaterialProto>((Func<TerrainMaterialProto, bool>) (x => x.MinedProduct.Graphics.ResourcesVizColor.IsNotEmpty)).Select<TerrainMaterialProto, LooseProductProto>((Func<TerrainMaterialProto, LooseProductProto>) (x => x.MinedProduct)));
      this.VirtualResourceProducts = HybridSet<VirtualResourceProductProto>.From(protosDb.All<VirtualResourceProductProto>());
    }
  }
}
