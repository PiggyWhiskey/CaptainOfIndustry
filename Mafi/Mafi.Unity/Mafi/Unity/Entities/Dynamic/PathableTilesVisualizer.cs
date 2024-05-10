// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.PathableTilesVisualizer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Unity.InputControl;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  [GlobalDependency(RegistrationMode.AsSelf, true, false)]
  internal class PathableTilesVisualizer
  {
    public PathableTilesVisualizer(
      ISimLoopEvents simLoopEvents,
      TerrainManager terrain,
      TerrainCursor terrainCursor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      new GameObject("DEBUG: Pathable tiles visualizer").AddComponent<PathableTilesVisualizerMb>().Initialize(simLoopEvents, terrain, terrainCursor);
    }
  }
}
