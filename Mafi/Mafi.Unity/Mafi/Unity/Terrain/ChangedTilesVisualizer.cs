// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.ChangedTilesVisualizer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsSelf, true, false)]
  internal class ChangedTilesVisualizer
  {
    public ChangedTilesVisualizer(ISimLoopEvents simLoopEvents, TerrainManager terrain)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      new GameObject("DEBUG: Changed tiles visualizer").AddComponent<ChangedTilesVisualizerMb>().Initialize(simLoopEvents, terrain);
    }
  }
}
