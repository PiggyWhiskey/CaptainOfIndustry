// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.FileWorldRegionMapFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.SaveGame;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public sealed class FileWorldRegionMapFactory : IWorldRegionMapFactory
  {
    private readonly MapSerializer m_mapSerializer;
    private (IWorldRegionMap, IWorldRegionMapPreviewData)? m_mapAndPreview;

    public FileWorldRegionMapFactory(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_mapSerializer = new MapSerializer(ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new ProtosSerializerFactory(protosDb)));
    }

    public IEnumerator<Percent> GenerateIslandMapTimeSliced(Option<object> mapConfig)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new FileWorldRegionMapFactory.\u003CGenerateIslandMapTimeSliced\u003Ed__4(0)
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

    public class Config
    {
      public readonly string MapFilePath;

      public Config(string mapFilePath)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.MapFilePath = mapFilePath;
      }
    }
  }
}
