// Decompiled with JetBrains decompiler
// Type: Mafi.PathFinding.PathFinderResult
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.PathFinding
{
  public enum PathFinderResult
  {
    /// <summary>Result was not set.</summary>
    Unknown,
    /// <summary>
    /// Path was not found yet but we can continue searching to potentially find one.
    /// </summary>
    StillSearching,
    /// <summary>
    /// Path between start tile and any of the goals tiles was successfully found.
    /// </summary>
    PathFound,
    /// <summary>
    /// Either start or goals are fully enclosed and separated, thus, a valid path does not exist.
    /// </summary>
    PathDoesNotExist,
  }
}
