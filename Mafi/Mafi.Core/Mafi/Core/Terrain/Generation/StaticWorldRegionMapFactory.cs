// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.StaticWorldRegionMapFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public sealed class StaticWorldRegionMapFactory : IWorldRegionMapFactory
  {
    private (IWorldRegionMap, IWorldRegionMapPreviewData)? m_mapAndPreview;

    public IEnumerator<Percent> GenerateIslandMapTimeSliced(Option<object> mapConfig)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new StaticWorldRegionMapFactory.\u003CGenerateIslandMapTimeSliced\u003Ed__2(0)
      {
        \u003C\u003E4__this = this,
        mapConfig = mapConfig
      };
    }

    public IWorldRegionMap GetMapAndClear(out IWorldRegionMapPreviewData previewData)
    {
      IWorldRegionMap mapAndClear;
      (mapAndClear, previewData) = this.m_mapAndPreview.HasValue ? this.m_mapAndPreview.Value : throw new InvalidOperationException("No map was generated.");
      this.m_mapAndPreview = new (IWorldRegionMap, IWorldRegionMapPreviewData)?();
      return mapAndClear;
    }

    public StaticWorldRegionMapFactory()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public class Config
    {
      public IWorldRegionMap Map { get; private set; }

      public IWorldRegionMapPreviewData PreviewData { get; private set; }

      public Config(IWorldRegionMap map, IWorldRegionMapPreviewData previewData)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Map = map;
        this.PreviewData = previewData;
      }

      public void SetMap(IWorldRegionMap map, IWorldRegionMapPreviewData previewData)
      {
        this.Map = map;
        this.PreviewData = previewData;
      }
    }
  }
}
