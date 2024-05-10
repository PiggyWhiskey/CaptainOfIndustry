// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.RenderStats
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Runtime.InteropServices;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public readonly struct RenderStats
  {
    public RenderStats(
      int drawCallsCount,
      int instancesCount,
      int renderedInstances,
      int renderedIndices)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
    }

    public static RenderStats operator +(RenderStats lhs, RenderStats rhs) => new RenderStats();
  }
}
