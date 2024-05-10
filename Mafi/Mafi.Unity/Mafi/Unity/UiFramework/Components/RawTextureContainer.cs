// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.RawTextureContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class RawTextureContainer : IUiElement
  {
    private readonly RawImage m_image;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public RawTextureContainer(string name)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GameObject = new GameObject(name);
      this.RectTransform = this.GameObject.AddComponent<RectTransform>();
      this.GameObject.AddComponent<CanvasRenderer>();
      this.m_image = this.GameObject.AddComponent<RawImage>();
    }

    public RawTextureContainer SetTexture(Texture texture)
    {
      this.m_image.texture = texture;
      return this;
    }
  }
}
