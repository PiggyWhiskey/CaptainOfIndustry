// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Panel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Decorators;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class Panel : IUiElement, IUiElementWithHover<Panel>
  {
    private Option<Image> m_image;
    private Option<UiElementListener> m_listener;
    private Option<DragDropHandlerMb> m_dragListener;
    private readonly UiBuilder m_builder;

    public GameObject GameObject { get; }

    public RectTransform RectTransform { get; }

    public Option<GameObject> Border { get; private set; }

    public Panel(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.GameObject = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = this.GameObject.GetComponent<RectTransform>();
    }

    public Panel SetBorderStyle(BorderStyle borderStyle)
    {
      this.Border = BorderDecorator.DecorateWithBorders(this.m_builder, this.Border, this.GameObject, borderStyle);
      return this;
    }

    public Panel SetBackground(Color color)
    {
      if (this.m_image.IsNone)
        this.m_image = (Option<Image>) this.GameObject.AddComponent<Image>();
      this.m_image.Value.color = color;
      return this;
    }

    public Panel SetBackground(ColorRgba color) => this.SetBackground(color.ToColor());

    public Panel SetBackground(Sprite sprite, ColorRgba? color = null, bool isTiled = false)
    {
      BackgroundDecorator.DecorateWithBgImage(this.GameObject, sprite, this.m_builder, color, isTiled);
      return this;
    }

    public Panel SetBackground(string path, ColorRgba? color = null, bool isTiled = false)
    {
      BackgroundDecorator.DecorateWithBgImage(this.GameObject, this.m_builder.AssetsDb.GetSharedSprite(path), this.m_builder, color, isTiled);
      return this;
    }

    public Panel SetBackground(SlicedSpriteStyle spriteStyle, ColorRgba? color = null, bool isTiled = false)
    {
      BackgroundDecorator.DecorateWithBgImage(this.GameObject, this.m_builder.AssetsDb.GetSharedSprite(spriteStyle), this.m_builder, color, isTiled);
      return this;
    }

    public Panel AddBottomBorder(ColorRgba color)
    {
      this.m_builder.NewPanel("bottomBorder").SetBackground(color).PutToBottomOf<Panel>((IUiElement) this, 1f);
      return this;
    }

    public Panel SetOnMouseEnterLeaveActions(Action enterAction, Action leaveAction)
    {
      UiElementListener listener = this.getOrCreateListener();
      listener.MouseEnterAction = (Option<Action>) enterAction.CheckNotNull<Action>();
      listener.MouseLeaveAction = (Option<Action>) leaveAction.CheckNotNull<Action>();
      return this;
    }

    public Panel OnClick(Action onClick)
    {
      this.getOrCreateListener().LeftClickAction = (Option<Action>) onClick.CheckNotNull<Action>();
      return this;
    }

    public Panel OnRightClick(Action onClick)
    {
      this.getOrCreateListener().RightClickAction = (Option<Action>) onClick.CheckNotNull<Action>();
      return this;
    }

    public Panel OnMouseEnter(Action onMouseEnter)
    {
      this.getOrCreateListener().MouseEnterAction = (Option<Action>) onMouseEnter.CheckNotNull<Action>();
      return this;
    }

    public Panel OnMouseLeave(Action onMouseLeave)
    {
      this.getOrCreateListener().MouseLeaveAction = (Option<Action>) onMouseLeave.CheckNotNull<Action>();
      return this;
    }

    private UiElementListener getOrCreateListener()
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<UiElementListener>) this.GameObject.AddComponent<UiElementListener>();
      return this.m_listener.Value;
    }

    public Panel AddToolTip(string tooltip)
    {
      this.m_builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this).SetText(tooltip);
      return this;
    }

    public Tooltip AddToolTipAndReturn()
    {
      return this.m_builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this);
    }

    public Panel SetupDragDrop(Action onBeginDrag, Action onDrag, Action onEndDrag)
    {
      if (this.m_dragListener.IsNone)
      {
        this.m_dragListener = (Option<DragDropHandlerMb>) this.GameObject.AddComponent<DragDropHandlerMb>();
        this.m_dragListener.Value.Initialize((IUiElement) this, this.m_builder.MainCanvas);
      }
      this.m_dragListener.Value.OnBeginDragAction = (Option<Action>) onBeginDrag.CheckNotNull<Action>();
      this.m_dragListener.Value.OnDragAction = (Option<Action>) onDrag.CheckNotNull<Action>();
      this.m_dragListener.Value.OnEndDragAction = (Option<Action>) onEndDrag.CheckNotNull<Action>();
      return this;
    }
  }
}
