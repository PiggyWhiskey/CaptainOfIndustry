// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.ITerrainRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Unity.Utils;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public interface ITerrainRenderer
  {
    ImmutableArray<int> UpdateDelayPerLod { get; }

    Tile3f? Raycast(Ray ray);

    IActivator CreateGridLinesActivator();

    void NotifyChunkUpdated(Chunk2i chunk);

    void SetUpdateDelayPerLod(ImmutableArray<int> updateDelayPerLod);
  }
}
