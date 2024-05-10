// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.CoastLinesData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Numerics;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public class CoastLinesData : ITerrainGenerationExtraData
  {
    public readonly ImmutableArray<Line2f> CoastLines;

    public CoastLinesData(ImmutableArray<Line2f> coastLines)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CoastLines = coastLines;
    }

    void ITerrainGenerationExtraData.Initialize(Chunk64Area area)
    {
    }

    void ITerrainGenerationExtraData.ApplyInArea(Chunk64Area area, bool isInMapEditor)
    {
    }
  }
}
