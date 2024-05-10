// Decompiled with JetBrains decompiler
// Type: RTG.SceneRaycastPrecision
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace RTG
{
  public enum SceneRaycastPrecision
  {
    /// <summary>
    /// If the object has a mesh, the raycast will be performed against the object
    /// mesh surface. If the object doesn't have a mesh, but it has a terrain with
    /// a terrain collider, it will be performed against the terrain collider. If
    /// none of these are available, the raycast will be performed against the object's
    /// volume/box.
    /// </summary>
    BestFit,
    /// <summary>
    /// The raycast will always be performed against the object's volume/box.
    /// </summary>
    Box,
  }
}
