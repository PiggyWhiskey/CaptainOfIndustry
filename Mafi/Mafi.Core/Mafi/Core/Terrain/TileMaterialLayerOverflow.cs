// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileMaterialLayerOverflow
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Utils;

#nullable disable
namespace Mafi.Core.Terrain
{
  [ExpectedStructSize(8)]
  public struct TileMaterialLayerOverflow
  {
    public TerrainMaterialThicknessSlim Material;
    public int OverflowIndex;

    public TileMaterialLayerOverflow(TerrainMaterialThicknessSlim material, int overflowIndex)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Material = material;
      this.OverflowIndex = overflowIndex;
    }

    public override readonly string ToString()
    {
      return string.Format("{0}, next: [{1}]", (object) this.Material, (object) this.OverflowIndex);
    }
  }
}
