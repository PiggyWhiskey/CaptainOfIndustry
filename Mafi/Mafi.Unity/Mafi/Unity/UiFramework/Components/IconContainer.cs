// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.IconContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class IconContainer : IUiElement, IUiElementWithHover<IconContainer>
  {
    private readonly UiBuilder m_builder;
    private readonly Image m_image;
    private Option<UiElementListener> m_listener;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public Graphic Graphic => this.GameObject.GetComponent<Graphic>();

    public IconContainer(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.GameObject = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = this.GameObject.GetComponent<RectTransform>();
      this.m_image = this.GameObject.AddComponent<Image>();
    }

    public IconContainer SetIcon(string spriteAssetPath, bool preserveAspect = true)
    {
      this.m_image.sprite = this.m_builder.AssetsDb.GetSharedSprite(spriteAssetPath);
      this.m_image.preserveAspect = preserveAspect;
      return this;
    }

    public IconContainer SetIcon(Sprite sprite, bool preserveAspect = true)
    {
      this.m_image.sprite = sprite;
      this.m_image.preserveAspect = preserveAspect;
      return this;
    }

    public IconContainer SetIcon(string spriteAssetPath, ColorRgba iconColor, bool preserveAspect = true)
    {
      this.m_image.sprite = this.m_builder.AssetsDb.GetSharedSprite(spriteAssetPath);
      this.m_image.preserveAspect = preserveAspect;
      this.m_image.color = iconColor.ToColor();
      return this;
    }

    public IconContainer SetIcon(IconStyle iconStyle)
    {
      this.SetIcon(iconStyle.AssetPath, iconStyle.Color, iconStyle.PreserveAspectRatio);
      return this;
    }

    public IconContainer SetColor(ColorRgba color)
    {
      this.m_image.color = color.ToColor();
      return this;
    }

    public IconContainer SetUnityColor(Color color)
    {
      this.m_image.color = color;
      return this;
    }

    internal IconContainer AddOutline()
    {
      this.GameObject.AddComponent<Outline>();
      return this;
    }

    internal IconContainer DisableRaycast()
    {
      this.m_image.raycastTarget = false;
      return this;
    }

    public IconContainer SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      UiElementListener listener = this.getOrCreateListener();
      listener.MouseEnterAction = (Option<Action>) enterAction.CheckNotNull<Action>();
      listener.MouseLeaveAction = (Option<Action>) leaveAction.CheckNotNull<Action>();
      return this;
    }

    private UiElementListener getOrCreateListener()
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<UiElementListener>) this.GameObject.AddComponent<UiElementListener>();
      return this.m_listener.Value;
    }

    public class Cache : ViewsCacheHomogeneous<IconContainer>
    {
      public Cache(UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Func<IconContainer>) (() => builder.NewIconContainer("Icon")));
      }
    }
  }
}
