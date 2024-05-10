// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UnityTagExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  public static class UnityTagExtensions
  {
    /// <summary>
    /// Whether this <see cref="T:UnityEngine.GameObject" /> is tagged by given tag.
    /// </summary>
    public static bool HasTag(this GameObject go, UnityTag tag) => go.tag.Contains(tag.ToString());
  }
}
