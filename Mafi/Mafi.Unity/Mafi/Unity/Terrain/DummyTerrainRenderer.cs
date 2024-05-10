// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.DummyTerrainRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public class DummyTerrainRenderer : ITerrainRenderer
  {
    public ImmutableArray<int> UpdateDelayPerLod => ImmutableArray<int>.Empty;

    public Tile3f? Raycast(Ray ray) => new Tile3f?();

    public IActivator CreateGridLinesActivator()
    {
      return new ActivatorState((Action) (() => { }), (Action) (() => { })).CreateActivator();
    }

    public void NotifyChunkUpdated(Chunk2i chunk)
    {
    }

    public void SetUpdateDelayPerLod(ImmutableArray<int> updateDelayPerLod)
    {
    }

    public DummyTerrainRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
