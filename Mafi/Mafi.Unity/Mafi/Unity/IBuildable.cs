// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.IBuildable
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// Marks class, usually <see cref="T:UnityEngine.MonoBehaviour" />, as buildable. If you plan on having buildable MB consider
  /// deriving from <see cref="T:Mafi.Unity.BuildableMb" />.
  /// </summary>
  public interface IBuildable
  {
    void SetSharedMesh(
      Vector3[] vertices,
      int[] indices,
      Vector3[] normals = null,
      Vector4[] tangents = null,
      Color32[] colors = null,
      Vector2[] uvs = null,
      Vector2[] uvs2 = null,
      bool recalculateNormals = false);
  }
}
