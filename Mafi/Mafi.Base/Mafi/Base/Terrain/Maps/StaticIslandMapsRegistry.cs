// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.Maps.StaticIslandMapsRegistry
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Map;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.Maps
{
  public class StaticIslandMapsRegistry
  {
    private static Lyst<KeyValuePair<StaticIslandMap, StaticIslandMapPreviewData>> s_islandMaps;

    public static Lyst<KeyValuePair<StaticIslandMap, StaticIslandMapPreviewData>> IslandMaps
    {
      get
      {
        return StaticIslandMapsRegistry.s_islandMaps ?? (StaticIslandMapsRegistry.s_islandMaps = StaticIslandMapsRegistry.getIslandMaps());
      }
    }

    private static Lyst<KeyValuePair<StaticIslandMap, StaticIslandMapPreviewData>> getIslandMaps()
    {
      Lyst<KeyValuePair<StaticIslandMap, StaticIslandMapPreviewData>> list = new Lyst<KeyValuePair<StaticIslandMap, StaticIslandMapPreviewData>>();
      list.Add<StaticIslandMap, StaticIslandMapPreviewData>(StaticIslandMap.AlphaMap, AlphaStaticIslandMap.GetPreviewData());
      list.Add<StaticIslandMap, StaticIslandMapPreviewData>(StaticIslandMap.Beach, BeachStaticIslandMap.GetPreviewData());
      list.Add<StaticIslandMap, StaticIslandMapPreviewData>(StaticIslandMap.Curland, CurlandMap.GetPreviewData());
      list.Add<StaticIslandMap, StaticIslandMapPreviewData>(StaticIslandMap.GoldenPeak, GoldenPeakStaticIslandMap.GetPreviewData());
      list.Add<StaticIslandMap, StaticIslandMapPreviewData>(StaticIslandMap.YouShallNotPass, YouShallNotPassStaticIslandMap.GetPreviewData());
      list.Add<StaticIslandMap, StaticIslandMapPreviewData>(StaticIslandMap.InsulaMortis, InsulaMortis.GetPreviewData());
      return list;
    }

    public StaticIslandMapsRegistry()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
