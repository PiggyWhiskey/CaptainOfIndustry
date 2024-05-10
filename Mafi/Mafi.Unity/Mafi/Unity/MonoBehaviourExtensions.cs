// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MonoBehaviourExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  public static class MonoBehaviourExtensions
  {
    /// <summary>
    /// Checks if a MonoBehaviour (component) has been destroyed or marked as destroyed.
    /// </summary>
    public static bool IsNullOrDestroyed(this MonoBehaviour mb) => (Object) mb == (Object) null;

    public static bool IsNotDestroyedOrNull(this MonoBehaviour mb) => (Object) mb != (Object) null;
  }
}
