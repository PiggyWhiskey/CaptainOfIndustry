// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.RenderedInstancesInfo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  public readonly struct RenderedInstancesInfo
  {
    public readonly string Name;
    public readonly int InstancesCount;
    public readonly int IndicesPerInstanceForLod0;

    public RenderedInstancesInfo(string name, int instancesCount, int indicesCountForLod0)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Name = name;
      this.InstancesCount = instancesCount;
      this.IndicesPerInstanceForLod0 = indicesCountForLod0;
    }
  }
}
