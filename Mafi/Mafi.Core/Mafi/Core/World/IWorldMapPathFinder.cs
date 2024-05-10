// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.IWorldMapPathFinder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;

#nullable disable
namespace Mafi.Core.World
{
  public interface IWorldMapPathFinder
  {
    /// <summary>Finds path between two map cells.</summary>
    /// <remarks>
    /// For API simplicity, this is not time-sliced. If this ever becomes problem, use time-slicing similarly as
    /// vehicles do.
    /// </remarks>
    bool FindPath(
      WorldMapLocId start,
      WorldMapLocId goal,
      bool allowOnlyExplored,
      Lyst<WorldMapLocId> foundPath);

    bool FindPath(
      Lyst<WorldMapLocId> starts,
      WorldMapLocId goal,
      bool allowOnlyExplored,
      Lyst<WorldMapLocId> foundPath);
  }
}
