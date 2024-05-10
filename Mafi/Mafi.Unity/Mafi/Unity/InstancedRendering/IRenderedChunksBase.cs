// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.IRenderedChunksBase
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  public interface IRenderedChunksBase
  {
    /// <summary>Name of this chunk (for debug purposes only).</summary>
    string Name { get; }

    /// <summary>
    /// Reports rendered instances by adding one or more records to the list. This is only for debug purposes but it is
    /// very helpful to report accurate data.
    /// </summary>
    void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info);
  }
}
