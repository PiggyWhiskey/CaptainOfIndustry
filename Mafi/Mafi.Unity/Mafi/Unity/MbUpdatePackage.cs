// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MbUpdatePackage
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// This class can be used to store and apply update for <see cref="T:UnityEngine.MonoBehaviour" />. This is useful for caching
  /// updates of for building mesh update on the simulation thread and updating it in the main thread.
  /// </summary>
  public readonly struct MbUpdatePackage
  {
    public readonly Vector3[] Vertices;
    public readonly int[] Indices;
    public readonly Option<Vector3[]> Normals;
    public readonly Option<Vector4[]> Tangents;
    public readonly Option<Color32[]> Colors;
    public readonly Option<Vector2[]> TexCoords;
    public readonly Option<Vector2[]> TexCoords2;

    public MbUpdatePackage(
      Vector3[] vertices,
      int[] indices,
      Option<Vector3[]> normals,
      Option<Vector4[]> tangents,
      Option<Color32[]> colors,
      Option<Vector2[]> texCoords,
      Option<Vector2[]> texCoords2)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Vertices = vertices.CheckNotNull<Vector3[]>();
      this.Indices = indices.CheckNotNull<int[]>();
      this.Normals = normals;
      this.Tangents = tangents;
      this.Colors = colors;
      this.TexCoords = texCoords;
      this.TexCoords2 = texCoords2;
    }

    public void Apply(IBuildable buildableMb)
    {
      buildableMb.SetSharedMesh(this.Vertices, this.Indices, this.Normals.ValueOrNull, this.Tangents.ValueOrNull, this.Colors.ValueOrNull, this.TexCoords.ValueOrNull, this.TexCoords2.ValueOrNull);
    }
  }
}
