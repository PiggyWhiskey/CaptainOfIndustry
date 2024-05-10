// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Weather.FogRendererMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.Camera;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Weather
{
  internal class FogRendererMb : MonoBehaviour
  {
    private CameraController m_cameraController;
    private readonly Vector3[] m_frustumCorners;

    public void Initialize(CameraController cameraController, Shader fogShader)
    {
      this.m_cameraController = cameraController;
      MeshRenderer component = this.gameObject.GetComponent<MeshRenderer>();
      Assert.That<MeshRenderer>(component).IsValidUnityObject<MeshRenderer>();
      component.material = new Material(fogShader);
      this.transform.localRotation = Quaternion.identity;
    }

    public void LateUpdate()
    {
      UnityEngine.Camera camera = this.m_cameraController.Camera;
      float z = camera.nearClipPlane * 2f;
      camera.CalculateFrustumCorners(new Rect(0.0f, 0.0f, 1f, 1f), z, camera.stereoActiveEye, this.m_frustumCorners);
      float x = this.m_frustumCorners[3].x - this.m_frustumCorners[0].x;
      float y = this.m_frustumCorners[1].y - this.m_frustumCorners[0].y;
      this.transform.localPosition = new Vector3(0.0f, 0.0f, z);
      this.transform.localScale = new Vector3(x, y, 1f);
    }

    public FogRendererMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_frustumCorners = new Vector3[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
