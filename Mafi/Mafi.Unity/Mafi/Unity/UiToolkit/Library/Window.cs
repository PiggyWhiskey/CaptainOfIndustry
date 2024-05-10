// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Window
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  /// <summary>
  /// Provides a centered and decorated frame with X button, title, and click-out behavior to host content.
  /// When the screen is below the target size for either axis, the window will stretch fullscreen along
  /// that axis to get as close to the target as possible. The window will match the target size on either
  /// axis when the screen is larger.
  /// </summary>
  public class Window : Column
  {
    public static readonly Px DEFAULT_WIDTH;
    public static readonly Px DEFAULT_HEIGHT;
    private Action m_onShow;
    private Action m_onHide;
    private LystStruct<Window> m_childWindows;
    protected Option<IUiUpdater> m_updater;
    private readonly UiComponent m_frameShadow;
    protected (Px Width, Px Height) m_targetSize;
    protected (Px? Width, Px? Height) m_maxSize;
    private readonly Label m_title;
    private Option<IVisualElementScheduledItem> m_fadeInTask;
    private bool m_isVisible;

    public UiComponent Frame { get; private set; }

    public Row TitleBar { get; protected set; }

    public Column Body { get; protected set; }

    public Window(LocStrFormatted title, bool hideX = false, bool darkMask = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_targetSize = (Window.DEFAULT_WIDTH, Window.DEFAULT_HEIGHT);
      this.m_maxSize = (new Px?(), new Px?());
      this.m_isVisible = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<Window>("mask").AbsolutePositionFillParent<Window>().AlignItemsCenterMiddle<Window>().ClassIff<Window>(Cls.dark, darkMask);
      UiComponent component1 = new UiComponent();
      component1.Add<UiComponent>((Action<UiComponent>) (c => c.Class<UiComponent>(Cls.fadeIn)));
      component1.Add(new UiComponent().Class<UiComponent>(Cls.windowShadow));
      Column component2 = new Column();
      component2.Add<Column>((Action<Column>) (c => c.Class<Column>(Cls.window)));
      component2.Add((UiComponent) new WindowBackground());
      Row component3 = new Row();
      component3.Add<Row>((Action<Row>) (c => c.FlexShrink<Row>(0.0f).Padding<Row>(2.pt()).PaddingBottom<Row>(6.px()).MarginBottom<Row>(-10.px())));
      component3.Add(hideX ? (UiComponent) null : (UiComponent) new Button(new Action(this.HandleClose), Outer.WindowCloseButton).WrapClass(Cls.windowCloseButton).VisibleForRender<UiComponentDecorated<UnityEngine.UIElements.Button>>(false));
      component3.Add((UiComponent) (this.m_title = new Label(title).Class<Label>(Cls.window__title).UpperCase().FlexGrow<Label>(1f)));
      component3.Add(hideX ? (UiComponent) null : (UiComponent) new Button(new Action(this.HandleClose), Outer.WindowCloseButton).WrapClass(Cls.windowCloseButton));
      Row component4 = component3;
      this.TitleBar = component3;
      component2.Add((UiComponent) component4.Visible<Row>(!hideX || title.IsNotEmpty));
      Column component5 = new Column();
      component5.Add<Column>((Action<Column>) (c => c.Class<Column>(Cls.window__body).AlignItemsStretch<Column>().FlexGrow<Column>(1f)));
      Column child1 = component5;
      this.Body = component5;
      component2.Add((UiComponent) child1);
      UiComponent child2 = (UiComponent) component2;
      this.Frame = (UiComponent) component2;
      component1.Add(child2);
      UiComponent child3 = component1;
      this.m_frameShadow = component1;
      this.Add(child3);
      this.OnMouseDown<Window>(new Action<MouseDownEvent>(this.HandleMouseDown));
      this.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.handleGeometryChanged), TrickleDown.NoTrickleDown);
    }

    protected override void setSizeInternal(StyleLength? width, StyleLength? height)
    {
      StyleLength styleLength;
      if (width.HasValue)
      {
        if (width.Value.value.unit != LengthUnit.Pixel)
          throw new NotImplementedException("Only pixel sizing is supported on window components");
        ref (Px, Px) local = ref this.m_targetSize;
        styleLength = width.Value;
        Px px;
        if (styleLength.keyword != StyleKeyword.Auto)
        {
          styleLength = width.Value;
          px = styleLength.value.value.px();
        }
        else
          px = Px.Auto;
        local.Item1 = px;
      }
      if (height.HasValue)
      {
        styleLength = height.Value;
        if (styleLength.value.unit != LengthUnit.Pixel)
          throw new NotImplementedException("Only pixel sizing is supported on window components");
        ref (Px, Px) local = ref this.m_targetSize;
        styleLength = height.Value;
        Px px;
        if (styleLength.keyword != StyleKeyword.Auto)
        {
          styleLength = height.Value;
          px = styleLength.value.value.px();
        }
        else
          px = Px.Auto;
        local.Item2 = px;
      }
      this.updateSize();
    }

    /// <summary>Set the window size to the screen size.</summary>
    public Window Fullscreen()
    {
      this.Frame.AbsolutePositionFillParent<UiComponent>();
      this.m_frameShadow.AbsolutePositionFillParent<UiComponent>();
      return this;
    }

    /// <summary>
    /// Cause the window to grow vertically by WINDOW_GROWTH_SCALE% up to the given max.
    /// </summary>
    /// <param name="maxHeight">Maximum height the window will grow to, null disables growth</param>
    public Window GrowVertically(float? maxHeight = 1200f)
    {
      Px? width = this.m_maxSize.Width;
      return this.Grow(width.HasValue ? new float?((float) width.GetValueOrDefault()) : new float?(), maxHeight);
    }

    /// <summary>
    /// Cause the window to grow horizontally by WINDOW_GROWTH_SCALE% up to the given max.
    /// </summary>
    /// <param name="maxWidth">Maximum width the window will grow to, null disables growth</param>
    public Window GrowHorizontally(float? maxWidth = 1600f)
    {
      float? maxWidth1 = maxWidth;
      Px? height = this.m_maxSize.Height;
      float? maxHeight = height.HasValue ? new float?((float) height.GetValueOrDefault()) : new float?();
      return this.Grow(maxWidth1, maxHeight);
    }

    /// <summary>
    /// Cause the window to grow by WINDOW_GROWTH_SCALE% on both axes when above the target size and below the
    /// given maximums.
    /// </summary>
    /// <param name="maxWidth">Maximum width the window will grow to, null disables growth</param>
    /// <param name="maxHeight">Maximum height the window will grow to, null disables growth</param>
    public Window Grow(float? maxWidth = 1600f, float? maxHeight = 1200f)
    {
      this.m_maxSize = (maxWidth.HasValue ? new Px?(maxWidth.GetValueOrDefault().px()) : new Px?(), maxHeight.HasValue ? new Px?(maxHeight.GetValueOrDefault().px()) : new Px?());
      this.updateSize();
      return this;
    }

    public Window Title(LocStrFormatted title)
    {
      this.m_title.Text<Label>(title);
      return this;
    }

    public Window OnHide(Action handler)
    {
      this.m_onHide += handler;
      return this;
    }

    public Window OnShow(Action handler)
    {
      this.m_onShow += handler;
      return this;
    }

    public Window MarkAsError()
    {
      this.m_title.Color<Label>(new ColorRgba?(Theme.WarningText));
      return this;
    }

    public override bool IsVisible() => this.m_isVisible;

    public override void SetVisible(bool newVisible)
    {
      bool flag = this.IsVisible();
      this.m_isVisible = newVisible;
      if (this.m_fadeInTask.HasValue)
      {
        this.m_frameShadow.ClassIff<UiComponent>(Cls.show, newVisible);
        this.ClassIff<Window>(Cls.show, newVisible);
      }
      if (!newVisible & flag)
      {
        this.RegisterCallback<TransitionEndEvent>(new EventCallback<TransitionEndEvent>(this.handleHideEnd), TrickleDown.NoTrickleDown);
        this.RegisterCallback<TransitionCancelEvent>(new EventCallback<TransitionCancelEvent>(this.handleHideEnd), TrickleDown.NoTrickleDown);
      }
      else
      {
        if (!newVisible || flag)
          return;
        base.SetVisible(true);
        Action onShow = this.m_onShow;
        if (onShow == null)
          return;
        onShow();
      }
    }

    public virtual bool InputUpdate()
    {
      foreach (Window childWindow in this.m_childWindows)
      {
        if (childWindow.InputUpdate())
          return true;
      }
      if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape) || !this.IsVisible())
        return false;
      this.Hide<Window>();
      return true;
    }

    public virtual void SyncUpdate(GameTime gameTime)
    {
      foreach (Window childWindow in this.m_childWindows)
      {
        if (childWindow.IsVisible())
          childWindow.SyncUpdate(gameTime);
      }
      this.m_updater.ValueOrNull?.SyncUpdate();
    }

    public virtual void RenderUpdate(GameTime gameTime)
    {
      foreach (Window childWindow in this.m_childWindows)
      {
        if (childWindow.IsVisible())
          childWindow.RenderUpdate(gameTime);
      }
      this.m_updater.ValueOrNull?.RenderUpdate();
    }

    protected void AddChildWindow(Window window)
    {
      this.m_childWindows.Add(window);
      this.RunWithBuilder((Action<UiBuilder>) (b => b.AddComponent((UiComponent) window)));
    }

    protected void RemoveChildWindow(Window window)
    {
      this.m_childWindows.Remove(window);
      window.RemoveFromHierarchy();
    }

    protected virtual void HandleClose() => this.SetVisible(false);

    protected virtual void HandleMouseDown(MouseDownEvent evt)
    {
      if (evt.target != this.Element)
        return;
      this.SetVisible(false);
    }

    private void updateSize()
    {
      if (this.Frame.IsPositionAbsolute())
        return;
      Vector2 screenSize = UiScaleHelper.ScreenSize;
      Vector2 vector2 = screenSize * 0.9f;
      (Px px1, Px px2) = this.m_targetSize;
      Px? nullable1;
      float? nullable2;
      if (this.m_maxSize.Width.HasValue)
      {
        ref Px local = ref px1;
        double pixels;
        if ((double) vector2.x >= (double) (float) this.m_targetSize.Width)
        {
          nullable1 = this.m_maxSize.Width;
          nullable2 = nullable1.HasValue ? new float?((float) nullable1.GetValueOrDefault()) : new float?();
          float x = vector2.x;
          pixels = (double) nullable2.GetValueOrDefault() < (double) x & nullable2.HasValue ? (double) (float) this.m_maxSize.Width.Value : (double) vector2.x;
        }
        else
          pixels = (double) (float) this.m_targetSize.Width;
        local = new Px((float) pixels);
      }
      if (this.m_maxSize.Height.HasValue)
      {
        ref Px local = ref px2;
        double pixels;
        if ((double) vector2.y >= (double) (float) this.m_targetSize.Height)
        {
          nullable1 = this.m_maxSize.Height;
          nullable2 = nullable1.HasValue ? new float?((float) nullable1.GetValueOrDefault()) : new float?();
          float y = vector2.y;
          pixels = (double) nullable2.GetValueOrDefault() < (double) y & nullable2.HasValue ? (double) (float) this.m_maxSize.Height.Value : (double) vector2.y;
        }
        else
          pixels = (double) (float) this.m_targetSize.Height;
        local = new Px((float) pixels);
      }
      this.Frame.MaxWidth<UiComponent>(screenSize.x.px()).MaxHeight<UiComponent>(screenSize.y.px());
      this.Frame.Size<UiComponent>(px1, px2);
    }

    private void handleGeometryChanged(GeometryChangedEvent evt)
    {
      this.updateSize();
      if (!this.m_fadeInTask.IsNone)
        return;
      Action onShow = this.m_onShow;
      if (onShow != null)
        onShow();
      this.m_fadeInTask = this.Schedule.Execute((Action) (() =>
      {
        this.m_frameShadow.Class<UiComponent>(Cls.show);
        this.Class<Window>(Cls.show);
      })).SomeOption<IVisualElementScheduledItem>();
    }

    private void handleHideEnd(EventBase evt)
    {
      this.UnregisterCallback<TransitionEndEvent>(new EventCallback<TransitionEndEvent>(this.handleHideEnd));
      this.UnregisterCallback<TransitionCancelEvent>(new EventCallback<TransitionCancelEvent>(this.handleHideEnd));
      base.SetVisible(false);
      Action onHide = this.m_onHide;
      if (onHide == null)
        return;
      onHide();
    }

    static Window()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Window.DEFAULT_WIDTH = 1200.px();
      Window.DEFAULT_HEIGHT = 700.px();
    }
  }
}
