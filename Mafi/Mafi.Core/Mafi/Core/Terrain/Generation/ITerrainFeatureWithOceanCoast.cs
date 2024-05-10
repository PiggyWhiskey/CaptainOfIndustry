// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainFeatureWithOceanCoast
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Numerics;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainFeatureWithOceanCoast : ITerrainFeatureGenerator, ITerrainFeatureBase
  {
    /// <summary>
    /// Returns ocean floor height data. This is called on non-initialized generator.
    /// </summary>
    void GetOceanFloorHeightData(Lyst<Line2f> data);
  }
}
