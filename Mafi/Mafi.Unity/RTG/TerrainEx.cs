// Decompiled with JetBrains decompiler
// Type: RTG.TerrainEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class TerrainEx
  {
    public static Vector2 ToNormCoords(this Terrain terrain, Vector3 worldPos)
    {
      Vector3 vector3 = Vector3.Scale(worldPos - terrain.transform.position, new Vector3(1f / terrain.terrainData.size.x, 1f, 1f / terrain.terrainData.size.z));
      return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 GetInterpolatedNormal(this Terrain terrain, Vector3 worldPos)
    {
      Vector2 normCoords = terrain.ToNormCoords(worldPos);
      return terrain.terrainData.GetInterpolatedNormal(normCoords.x, normCoords.y);
    }
  }
}
