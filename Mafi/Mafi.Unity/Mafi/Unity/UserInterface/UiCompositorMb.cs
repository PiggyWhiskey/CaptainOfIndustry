// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.UiCompositorMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  public class UiCompositorMb : MonoBehaviour
  {
    public RenderTexture m_uiTexture;
    private Material m_material;

    public void Initialize(RenderTexture uiTexture, Shader shader)
    {
      this.m_uiTexture = uiTexture;
      this.m_material = new Material(shader);
      this.m_material.SetTexture("_UiTex", (Texture) uiTexture);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      Graphics.Blit((Texture) source, destination, this.m_material);
    }

    public UiCompositorMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
