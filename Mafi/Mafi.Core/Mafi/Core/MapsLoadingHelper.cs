// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MapsLoadingHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.SaveGame;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Mafi.Core
{
  public class MapsLoadingHelper
  {
    private LystStruct<Pair<string, IWorldRegionMapPreviewData>> m_loadedMapPreviews;
    private Option<MapSerializer> m_mapSerializer;

    public void Initialize(ProtosDb protosDb)
    {
      this.m_mapSerializer = (Option<MapSerializer>) new MapSerializer(ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new ProtosSerializerFactory(protosDb)));
      this.m_loadedMapPreviews.Clear();
    }

    public void LoadMapPreviews(string dirName, IFileSystemHelper fsHelper, bool includeWip)
    {
      if (!Directory.Exists(dirName))
        return;
      this.LoadMapPreviews(Directory.EnumerateFiles(dirName, "*.map"), fsHelper, includeWip);
    }

    public void LoadMapPreviews(
      IEnumerable<string> mapPaths,
      IFileSystemHelper fsHelper,
      bool includeWip)
    {
      if (this.m_mapSerializer.IsNone)
      {
        Log.Error("No map serializer.");
      }
      else
      {
        foreach (string mapPath in mapPaths)
        {
          if (includeWip || !fsHelper.IsWipMap(mapPath))
          {
            IWorldRegionMapPreviewData previewData;
            Option<Exception> exception;
            if (this.m_mapSerializer.Value.TryLoadPreviewMinimalFromFile(mapPath, out previewData, out exception))
            {
              if (includeWip || previewData.IsPublished)
                this.m_loadedMapPreviews.Add(Pair.Create<string, IWorldRegionMapPreviewData>(mapPath, previewData));
            }
            else
              Log.Exception(exception.Value, "Failed to load core map from file '" + mapPath + "'.");
          }
        }
      }
    }

    public IEnumerable<IWorldRegionMapPreviewData> GetLoadedMapPreviews()
    {
      return (IEnumerable<IWorldRegionMapPreviewData>) this.m_loadedMapPreviews.ToArray<IWorldRegionMapPreviewData>((Func<Pair<string, IWorldRegionMapPreviewData>, IWorldRegionMapPreviewData>) (x => x.Second));
    }

    public bool TryGetMapData(
      IWorldRegionMapPreviewData preview,
      out IWorldRegionMapAdditionalData fullData,
      out WorldRegionMapFactoryConfig factoryConfig)
    {
      fullData = (IWorldRegionMapAdditionalData) null;
      factoryConfig = (WorldRegionMapFactoryConfig) null;
      if (this.m_mapSerializer.IsNone)
      {
        Log.Error("No map serializer.");
        return false;
      }
      int index = this.m_loadedMapPreviews.IndexOf<IWorldRegionMapPreviewData>(preview, (Func<Pair<string, IWorldRegionMapPreviewData>, IWorldRegionMapPreviewData>) (x => x.Second));
      if (index < 0)
      {
        Log.Error("File path not found for given map: " + preview.Name);
        return false;
      }
      string first = this.m_loadedMapPreviews[index].First;
      try
      {
        using (FileStream fileStream = File.OpenRead(first))
        {
          fullData = this.m_mapSerializer.Value.LoadPreviewFull((Stream) fileStream, false, out IWorldRegionMapPreviewData _);
          factoryConfig = new WorldRegionMapFactoryConfig(typeof (FileWorldRegionMapFactory), (Option<object>) (object) new FileWorldRegionMapFactory.Config(first));
          return true;
        }
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to load map full data from file '" + first + "'.");
        return false;
      }
    }

    public void ClearMapData()
    {
      this.m_loadedMapPreviews = new LystStruct<Pair<string, IWorldRegionMapPreviewData>>();
      this.m_mapSerializer = Option<MapSerializer>.None;
    }

    public MapsLoadingHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
