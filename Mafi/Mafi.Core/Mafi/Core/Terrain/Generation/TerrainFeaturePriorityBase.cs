// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainFeaturePriorityBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public enum TerrainFeaturePriorityBase
  {
    First = 0,
    TerrainResources = 1000, // 0x000003E8
    TerrainSurfaces = 2000, // 0x000007D0
    PostProcessors = 5000, // 0x00001388
    Last = 9000, // 0x00002328
  }
}
