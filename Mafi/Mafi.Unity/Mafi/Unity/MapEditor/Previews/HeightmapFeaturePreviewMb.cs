// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Previews.HeightmapFeaturePreviewMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.Previews;
using Unity.Collections;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Previews
{
  public class HeightmapFeaturePreviewMb : MonoBehaviour
  {
    private Mesh m_terrainChunkMesh;
    private Texture2D m_heightTexture;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;

    public void Initialize(AssetsDb assetsDb)
    {
      this.m_terrainChunkMesh = MeshBuilder.CreateChunkMesh(64, 2f, true);
      this.m_terrainChunkMesh.name = "Preview terrain chunk mesh";
      Texture2D texture2D = new Texture2D(65, 65, TextureFormat.RFloat, false, true);
      texture2D.name = "TerrainHeight";
      texture2D.anisoLevel = 0;
      texture2D.filterMode = FilterMode.Bilinear;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      this.m_heightTexture = texture2D;
      Material clonedMaterial = assetsDb.GetClonedMaterial("Assets/Unity/MapEditor/TerrainResViz.mat");
      clonedMaterial.SetTexture("_HeightTex", (Texture) this.m_heightTexture);
      this.m_meshFilter = this.gameObject.AddComponent<MeshFilter>();
      this.m_meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
      this.m_meshFilter.mesh = this.m_terrainChunkMesh;
      this.m_meshRenderer.sharedMaterial = clonedMaterial;
    }

    internal NativeArray<float> GetRawTextureData()
    {
      return this.m_heightTexture.GetRawTextureData<float>();
    }

    internal void NotifyDataUpdated(Vector3 position, Bounds bounds)
    {
      this.gameObject.transform.localPosition = position;
      this.m_heightTexture.Apply(false, false);
      this.m_meshRenderer.bounds = bounds;
    }

    public void UpdateData(HeightmapFeaturePreviewChunkData data)
    {
      NativeArray<float> rawTextureData = this.m_heightTexture.GetRawTextureData<float>();
      if (data.Heights.Length != rawTextureData.Length)
      {
        Log.Error("Height array size mismatch");
      }
      else
      {
        Vector3 vector3 = data.Chunk.ToVector3();
        this.gameObject.transform.localPosition = vector3;
        bool flag = false;
        float self1 = float.MinValue;
        float self2 = float.MaxValue;
        int length = data.Heights.Length;
        for (int index = 0; index < length; ++index)
        {
          HeightTilesF height = data.Heights[index];
          if (height == HeightTilesF.MinValue)
          {
            flag |= (double) rawTextureData[index] > -1000000000.0;
            rawTextureData[index] = -2E+09f;
          }
          else
          {
            float unityUnits = height.ToUnityUnits();
            flag |= (double) rawTextureData[index] != (double) unityUnits;
            rawTextureData[index] = unityUnits;
            self1 = self1.Max(unityUnits);
            self2 = self2.Min(unityUnits);
          }
        }
        if (!flag)
          return;
        this.m_heightTexture.Apply(false, false);
        if ((double) self1 == -3.4028234663852886E+38)
          return;
        Vector3 size = new Vector3(128f, self1 - self2, 128f);
        this.m_meshRenderer.bounds = new Bounds(vector3 + new Vector3(64f, self2 + size.y / 2f, 64f), size);
      }
    }

    public HeightmapFeaturePreviewMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
