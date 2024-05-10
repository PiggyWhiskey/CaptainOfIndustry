// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Previews.HeightmapTopBottomFeaturePreviewMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.Previews;
using Mafi.Collections;
using Unity.Collections;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Previews
{
  public class HeightmapTopBottomFeaturePreviewMb : MonoBehaviour
  {
    private HeightmapFeaturePreviewMb m_topPreviewMb;
    private HeightmapFeaturePreviewMb m_bottomPreviewMb;

    public void Initialize(AssetsDb assetsDb)
    {
      GameObject gameObject1 = new GameObject("top");
      gameObject1.transform.SetParent(this.gameObject.transform, false);
      this.m_topPreviewMb = gameObject1.AddComponent<HeightmapFeaturePreviewMb>();
      this.m_topPreviewMb.Initialize(assetsDb);
      GameObject gameObject2 = new GameObject("bottom");
      gameObject2.transform.SetParent(this.gameObject.transform, false);
      this.m_bottomPreviewMb = gameObject2.AddComponent<HeightmapFeaturePreviewMb>();
      this.m_bottomPreviewMb.Initialize(assetsDb);
      this.gameObject.transform.localPosition = Vector3.zero;
    }

    public void UpdateData(HeightmapTopBottomPreviewChunkData data)
    {
      NativeArray<float> rawTextureData1 = this.m_topPreviewMb.GetRawTextureData();
      NativeArray<float> rawTextureData2 = this.m_bottomPreviewMb.GetRawTextureData();
      if (data.Heights.Length != rawTextureData1.Length || data.Heights.Length != rawTextureData2.Length)
      {
        Log.Error("Height array size mismatch");
      }
      else
      {
        bool flag1 = false;
        bool flag2 = false;
        float self1 = float.MinValue;
        float self2 = float.MaxValue;
        float self3 = float.MinValue;
        float self4 = float.MaxValue;
        int length = data.Heights.Length;
        for (int index = 0; index < length; ++index)
        {
          Pair<HeightTilesF, HeightTilesF> height = data.Heights[index];
          if (height.First == height.Second)
          {
            flag1 |= (double) rawTextureData1[index] > -1000000000.0;
            rawTextureData1[index] = -2E+09f;
            flag2 |= (double) rawTextureData2[index] > -1000000000.0;
            rawTextureData2[index] = -2E+09f;
          }
          else
          {
            float unityUnits1 = height.First.ToUnityUnits();
            flag1 |= (double) rawTextureData1[index] != (double) unityUnits1;
            rawTextureData1[index] = unityUnits1;
            self1 = self1.Max(unityUnits1);
            self2 = self2.Min(unityUnits1);
            float unityUnits2 = height.Second.ToUnityUnits();
            flag2 |= (double) rawTextureData2[index] != (double) unityUnits2;
            rawTextureData2[index] = unityUnits2;
            self3 = self3.Max(unityUnits2);
            self4 = self4.Min(unityUnits2);
          }
        }
        if (flag1)
        {
          Bounds bounds = new Bounds();
          Vector3 vector3 = data.Chunk.ToVector3();
          if ((double) self1 != -3.4028234663852886E+38)
          {
            Vector3 size = new Vector3(128f, self1 - self2, 128f);
            bounds = new Bounds(vector3 + new Vector3(64f, self2 + size.y / 2f, 64f), size);
          }
          this.m_topPreviewMb.NotifyDataUpdated(vector3, bounds);
        }
        if (!flag2)
          return;
        Bounds bounds1 = new Bounds();
        Vector3 vector3_1 = data.Chunk.ToVector3();
        if ((double) self3 != -3.4028234663852886E+38)
        {
          Vector3 size = new Vector3(128f, self3 - self4, 128f);
          bounds1 = new Bounds(vector3_1 + new Vector3(64f, self4 + size.y / 2f, 64f), size);
        }
        this.m_bottomPreviewMb.NotifyDataUpdated(vector3_1, bounds1);
      }
    }

    public HeightmapTopBottomFeaturePreviewMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
