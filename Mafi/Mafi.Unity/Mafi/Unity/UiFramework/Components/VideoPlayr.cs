// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.VideoPlayr
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class VideoPlayr : IUiElement, IDynamicSizeElement
  {
    private readonly UiBuilder m_builder;
    private readonly VideoPlayer m_player;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_player.gameObject;

    public RectTransform RectTransform { get; }

    public VideoPlayr(UiBuilder builder, GameObject parent)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      GameObject clonedGo = this.m_builder.GetClonedGo("VideoPlayer", parent);
      this.RectTransform = clonedGo.GetComponent<RectTransform>();
      RenderTexture renderTexture = new RenderTexture(960, 540, 24);
      clonedGo.AddComponent<RawImage>().texture = (Texture) renderTexture;
      this.m_player = clonedGo.AddComponent<VideoPlayer>();
      this.m_player.targetTexture = renderTexture;
      this.m_player.isLooping = true;
      this.m_player.playOnAwake = true;
      this.m_player.aspectRatio = VideoAspectRatio.FitInside;
    }

    public VideoPlayr SetClipToPlay(string pathToClip)
    {
      VideoClip result;
      if (this.m_builder.AssetsDb.TryGetSharedAsset<VideoClip>(pathToClip, out result))
        this.m_player.clip = result;
      else
        Log.Error("Failed to load clip " + pathToClip);
      return this;
    }
  }
}
