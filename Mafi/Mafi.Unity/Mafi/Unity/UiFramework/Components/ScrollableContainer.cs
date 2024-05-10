// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.ScrollableContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.InputControl;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  /// <summary>
  /// Provides scrolling of the given content. You have to grow your content dynamically and the ScrollableContainer
  /// will take care of scrolling. You can position this view as any other view.
  /// </summary>
  public class ScrollableContainer : IUiElement
  {
    public const int VERT_SCROLLBAR_WIDTH = 16;
    private Vector3[] m_cachedCorners;
    private readonly GameObject m_scrollView;
    private readonly Panel m_viewport;
    private readonly UiBuilder m_builder;
    private readonly ScrollRect m_scrollRect;
    private Option<ScrollableContainer.ScrollListener> m_listener;
    private Option<Scrollbar> m_verticalScrollbar;
    private Vector2 m_currentPan;
    private static readonly float PAN_DAMPING_SHARPNESS;

    public GameObject GameObject => this.m_scrollView;

    public RectTransform RectTransform { get; }

    public float ScrollbarWidth => this.m_verticalScrollbar.HasValue ? 16f : 0.0f;

    public Panel Viewport => this.m_viewport;

    public Vector2 NormalizedPosition => this.m_scrollRect.normalizedPosition;

    public ScrollableContainer(UiBuilder builder, string name, GameObject parent = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      this.m_scrollView = this.m_builder.GetClonedGo(name, parent);
      this.RectTransform = this.m_scrollView.GetComponent<RectTransform>();
      this.m_scrollRect = (ScrollRect) this.m_scrollView.AddComponent<LeftRightScrollRect>();
      this.m_viewport = builder.NewPanel(nameof (Viewport), (IUiElement) this).PutTo<Panel>((IUiElement) this);
      this.m_viewport.GameObject.AddComponent<Mask>().showMaskGraphic = false;
      this.m_viewport.GameObject.AddComponent<Image>().color = Color.white;
      this.m_scrollRect.viewport = this.m_viewport.RectTransform;
      this.m_scrollRect.movementType = ScrollRect.MovementType.Clamped;
      this.m_scrollRect.scrollSensitivity = 60f;
      this.m_scrollRect.decelerationRate = 0.08f;
    }

    /// <summary>
    /// Funny story: Unity masks have no way to select their ordering. What this means is
    /// that if scroller A is created before scroller B, B will leak its scrolled content into A.
    /// However, if we re-enable mask on A, it will order its mask back and B will no longer leak into A.
    /// Why do we need this? Because otherwise scrolling recipe book or anything else leak its content
    /// into the list of goals. Lovely.
    /// 
    /// There is also option to order masks by having them in separate canvas with different order
    /// value. But that means that A draws over B (due to canvas order) but we don't want that as we
    /// want goals to be in the background.
    /// </summary>
    internal void HackMakeLastMask()
    {
      Mask component = this.m_viewport.GameObject.GetComponent<Mask>();
      component.enabled = false;
      component.enabled = true;
    }

    public ScrollableContainer SetOnScrollAction(Action action)
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<ScrollableContainer.ScrollListener>) this.m_scrollView.AddComponent<ScrollableContainer.ScrollListener>();
      this.m_listener.Value.OnScrollAction = (Option<Action>) action;
      if (this.m_verticalScrollbar.HasValue)
        this.m_verticalScrollbar.Value.onValueChanged.AddListener((UnityAction<float>) (_ => action()));
      return this;
    }

    public ScrollableContainer SetOnDragAction(Action action)
    {
      if (this.m_listener.IsNone)
        this.m_listener = (Option<ScrollableContainer.ScrollListener>) this.m_scrollView.AddComponent<ScrollableContainer.ScrollListener>();
      this.m_listener.Value.OnDragAction = (Option<Action>) action;
      return this;
    }

    public ScrollableContainer DisableVerticalScroll()
    {
      this.m_scrollRect.vertical = false;
      return this;
    }

    public ScrollableContainer DisableHorizontalScroll()
    {
      this.m_scrollRect.horizontal = false;
      return this;
    }

    public ScrollableContainer SetScrollSensitivity(float sensitivity)
    {
      this.m_scrollRect.scrollSensitivity = sensitivity;
      return this;
    }

    public ScrollableContainer DisableScrollByMouseWheel()
    {
      this.m_scrollRect.scrollSensitivity = 0.0f;
      return this;
    }

    public ScrollableContainer SetDecelerationRate(float rate)
    {
      this.m_scrollRect.decelerationRate = rate;
      return this;
    }

    public ScrollableContainer OnClick(Action onClick)
    {
      this.m_viewport.OnClick(onClick);
      return this;
    }

    public ScrollableContainer AddVerticalScrollbar()
    {
      if (this.m_verticalScrollbar.HasValue)
      {
        Assert.Fail("Tried to add scrollbar multiple times!");
        return this;
      }
      Panel rightOf = this.m_builder.NewPanel("Scrollbar Vertical").SetBackground(this.m_builder.Style.Global.ControlsBgColor).PutToRightOf<Panel>((IUiElement) this, 16f);
      Panel panel = this.m_builder.NewPanel("Handle").SetBackground(this.m_builder.AssetsDb.GetSharedSprite(this.m_builder.Style.Icons.WhiteBgGrayBorder), new ColorRgba?((ColorRgba) 5723991)).PutTo<Panel>((IUiElement) rightOf);
      Scrollbar scrollbar1 = rightOf.GameObject.AddComponent<Scrollbar>();
      scrollbar1.direction = Scrollbar.Direction.BottomToTop;
      scrollbar1.handleRect = panel.RectTransform;
      scrollbar1.targetGraphic = (Graphic) panel.GameObject.GetComponent<Image>();
      Scrollbar scrollbar2 = scrollbar1;
      ColorBlock colorBlock1 = new ColorBlock();
      ref ColorBlock local1 = ref colorBlock1;
      ColorBlock colors = scrollbar1.colors;
      double colorMultiplier = (double) colors.colorMultiplier;
      local1.colorMultiplier = (float) colorMultiplier;
      ref ColorBlock local2 = ref colorBlock1;
      colors = scrollbar1.colors;
      Color disabledColor = colors.disabledColor;
      local2.disabledColor = disabledColor;
      ref ColorBlock local3 = ref colorBlock1;
      colors = scrollbar1.colors;
      double fadeDuration = (double) colors.fadeDuration;
      local3.fadeDuration = (float) fadeDuration;
      colorBlock1.normalColor = 13948116.ToColor();
      colorBlock1.highlightedColor = Color.white;
      colorBlock1.pressedColor = Color.white;
      colorBlock1.selectedColor = Color.white;
      ColorBlock colorBlock2 = colorBlock1;
      scrollbar2.colors = colorBlock2;
      this.m_verticalScrollbar = (Option<Scrollbar>) scrollbar1;
      this.m_scrollRect.verticalScrollbar = scrollbar1;
      this.m_scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
      return this;
    }

    internal ScrollableContainer AddVerticalScrollbarLeftMinimal()
    {
      if (this.m_verticalScrollbar.HasValue)
      {
        Assert.Fail("Tried to add scrollbar multiple times!");
        return this;
      }
      Panel leftOf = this.m_builder.NewPanel("Scrollbar Vertical").PutToLeftOf<Panel>((IUiElement) this, 16f, Offset.Left(-16f));
      Panel panel = this.m_builder.NewPanel("Handle").SetBackground(this.m_builder.AssetsDb.GetSharedSprite(this.m_builder.Style.Icons.WhiteBgGrayBorder), new ColorRgba?((ColorRgba) 2565927)).PutTo<Panel>((IUiElement) leftOf, Offset.LeftRight(2f));
      Scrollbar scrollbar1 = leftOf.GameObject.AddComponent<Scrollbar>();
      scrollbar1.direction = Scrollbar.Direction.BottomToTop;
      scrollbar1.handleRect = panel.RectTransform;
      scrollbar1.targetGraphic = (Graphic) panel.GameObject.GetComponent<Image>();
      Scrollbar scrollbar2 = scrollbar1;
      ColorBlock colorBlock1 = new ColorBlock();
      ref ColorBlock local1 = ref colorBlock1;
      ColorBlock colors = scrollbar1.colors;
      double colorMultiplier = (double) colors.colorMultiplier;
      local1.colorMultiplier = (float) colorMultiplier;
      ref ColorBlock local2 = ref colorBlock1;
      colors = scrollbar1.colors;
      Color disabledColor = colors.disabledColor;
      local2.disabledColor = disabledColor;
      ref ColorBlock local3 = ref colorBlock1;
      colors = scrollbar1.colors;
      double fadeDuration = (double) colors.fadeDuration;
      local3.fadeDuration = (float) fadeDuration;
      colorBlock1.normalColor = 0.ToColor();
      colorBlock1.highlightedColor = Color.white;
      colorBlock1.pressedColor = Color.white;
      colorBlock1.selectedColor = Color.white;
      ColorBlock colorBlock2 = colorBlock1;
      scrollbar2.colors = colorBlock2;
      this.m_verticalScrollbar = (Option<Scrollbar>) scrollbar1;
      this.m_scrollRect.verticalScrollbar = scrollbar1;
      this.m_scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
      return this;
    }

    /// <summary>
    /// This does NOT add the object to the viewport it will only set it for scrolling. Before calling this call <see cref="!:AddChild" />. This is useful when you have more children and you want to choose which one is active.
    /// </summary>
    public void SetContentToScroll(IUiElement content)
    {
      this.m_scrollRect.content = (RectTransform) content.GameObject.transform;
    }

    public void ScrollBy(float value) => this.m_scrollRect.verticalNormalizedPosition += value;

    public void ScrollByPixels(Vector2 diffPx)
    {
      Vector2 vector2 = this.m_scrollRect.content.rect.size - this.m_scrollRect.viewport.rect.size;
      if ((double) vector2.x > 0.0)
        this.m_scrollRect.horizontalNormalizedPosition += diffPx.x / vector2.x;
      if ((double) vector2.y <= 0.0)
        return;
      this.m_scrollRect.verticalNormalizedPosition += diffPx.y / vector2.y;
    }

    public void ScrollToElement(RectTransform item)
    {
      this.m_scrollRect.normalizedPosition = this.CalculateFocusedScrollPosition(item);
    }

    private Vector2 CalculateFocusedScrollPosition(Vector2 focusPoint)
    {
      Vector2 size1 = this.m_scrollRect.content.rect.size;
      Vector2 size2 = ((RectTransform) this.m_scrollRect.content.parent).rect.size;
      Vector2 localScale = (Vector2) this.m_scrollRect.content.localScale;
      size1.Scale(localScale);
      focusPoint.Scale(localScale);
      Vector2 normalizedPosition = this.m_scrollRect.normalizedPosition;
      if (this.m_scrollRect.horizontal && (double) size1.x > (double) size2.x)
        normalizedPosition.x = Mathf.Clamp01((float) (((double) focusPoint.x - (double) size2.x * 0.5) / ((double) size1.x - (double) size2.x)));
      if (this.m_scrollRect.vertical && (double) size1.y > (double) size2.y)
        normalizedPosition.y = Mathf.Clamp01((float) (((double) focusPoint.y - (double) size2.y * 0.5) / ((double) size1.y - (double) size2.y)));
      return normalizedPosition;
    }

    private Vector2 CalculateFocusedScrollPosition(RectTransform item)
    {
      Vector2 vector2 = (Vector2) this.m_scrollRect.content.InverseTransformPoint(item.transform.TransformPoint((Vector3) item.rect.center));
      Vector2 size = this.m_scrollRect.content.rect.size;
      size.Scale(this.m_scrollRect.content.pivot);
      return this.CalculateFocusedScrollPosition(vector2 + size);
    }

    /// <summary>
    /// Adds the given object to the viewport and set it active for scrolling if requested.
    /// </summary>
    public void AddItemTop(IUiElement child, bool setActive = true)
    {
      child.PutToTopOf<IUiElement>((IUiElement) this.m_viewport, child.GetHeight());
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    public void AddItemTopLeft(IUiElement child, bool setActive = true)
    {
      child.PutToLeftTopOf<IUiElement>((IUiElement) this.m_viewport, child.GetSize());
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    /// <summary>
    /// Adds the given object to the viewport and set it active for scrolling if requested.
    /// </summary>
    public void AddItemTopCenter(IUiElement child, bool setActive = true)
    {
      child.PutToCenterTopOf<IUiElement>((IUiElement) this.m_viewport, child.GetSize());
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    /// <summary>
    /// Adds the given object to the viewport and set it active for scrolling if requested.
    /// </summary>
    public void AddItemBottom(IUiElement child, bool setActive = true)
    {
      child.PutToBottomOf<IUiElement>((IUiElement) this.m_viewport, 0.0f);
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    /// <summary>
    /// dds the given object to the viewport and set it active for scrolling if requested.
    /// </summary>
    public void AddItemLeft(IUiElement child, bool setActive = true)
    {
      child.PutToLeftOf<IUiElement>((IUiElement) this.m_viewport, 0.0f);
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    /// <summary>
    /// dds the given object to the viewport and set it active for scrolling if requested.
    /// </summary>
    public void AddItemCenter(IUiElement child, bool setActive = true)
    {
      child.PutToCenterOf<IUiElement>((IUiElement) this.m_viewport, 0.0f);
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    /// <summary>
    /// Adds the given object to the viewport and set it active for scrolling if requested.
    /// </summary>
    public void AddItemRight(IUiElement child, bool setActive = true)
    {
      child.PutToRightOf<IUiElement>((IUiElement) this.m_viewport, 0.0f);
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    /// <summary>
    /// Adds the object to the viewport and setup scrolling for it. Same as calling <see cref="!:AddChild" /> + <see cref="M:Mafi.Unity.UiFramework.Components.ScrollableContainer.SetContentToScroll(Mafi.Unity.UiFramework.IUiElement)" />.
    /// </summary>
    public void AddItem(
      IUiElement child,
      VerticalPosition verticalPos = VerticalPosition.Top,
      HorizontalPosition horizontalPos = HorizontalPosition.Left,
      bool setActive = true)
    {
      IUiElement objectToPlace = child;
      Panel viewport = this.m_viewport;
      Vector2 zero = Vector2.zero;
      VerticalPosition verticalPosition = verticalPos;
      int num1 = (int) horizontalPos;
      int num2 = (int) verticalPosition;
      Offset offset = new Offset();
      objectToPlace.PutRelativeTo<IUiElement>((IUiElement) viewport, zero, (HorizontalPosition) num1, (VerticalPosition) num2, offset);
      if (!setActive)
        return;
      this.SetContentToScroll(child);
    }

    public bool CanScrollLeft() => (double) this.m_scrollRect.horizontalNormalizedPosition > 0.01;

    public bool CanScrollRight()
    {
      this.m_cachedCorners = new Vector3[4];
      this.m_scrollView.GetComponent<RectTransform>().GetWorldCorners(this.m_cachedCorners);
      return (double) this.m_scrollRect.content.rect.width > (double) Mathf.Abs(this.m_cachedCorners[0].x - this.m_cachedCorners[2].x) && (double) this.m_scrollRect.horizontalNormalizedPosition < 0.99;
    }

    /// <summary>Resets the scroll position to left of the container.</summary>
    public void ResetToLeft() => this.m_scrollRect.normalizedPosition = new Vector2(0.0f, 0.0f);

    /// <summary>
    /// Resets the scroll position to the top of the container.
    /// </summary>
    public void ResetToTop() => this.m_scrollRect.normalizedPosition = new Vector2(0.0f, 1f);

    /// <summary>
    /// Resets the scroll position to the bottom of the container.
    /// </summary>
    public void ResetToBottom() => this.m_scrollRect.normalizedPosition = new Vector2(0.0f, 0.0f);

    /// <summary>
    /// Clamps normalized scroll position to its normal limits [0, 1] in both axes.
    /// </summary>
    public void FixScroll()
    {
      Vector2 normalizedPosition = this.m_scrollRect.normalizedPosition;
      this.m_scrollRect.normalizedPosition = new Vector2(normalizedPosition.x.Clamp01(), normalizedPosition.y.Clamp01());
    }

    public bool UpdateKeyboardPan(ShortcutsManager shortcutsManager)
    {
      float smoothDeltaTime = Time.smoothDeltaTime;
      float x = 0.0f;
      float y = 0.0f;
      bool flag = false;
      if (shortcutsManager.IsOn(shortcutsManager.MoveLeft))
      {
        --x;
        if ((double) this.m_currentPan.x > 0.0)
          this.m_currentPan = new Vector2(0.0f, this.m_currentPan.y);
        flag = true;
      }
      else if (shortcutsManager.IsOn(shortcutsManager.MoveRight))
      {
        ++x;
        if ((double) this.m_currentPan.x < 0.0)
          this.m_currentPan = new Vector2(0.0f, this.m_currentPan.y);
        flag = true;
      }
      if (shortcutsManager.IsOn(shortcutsManager.MoveUp))
      {
        ++y;
        if ((double) this.m_currentPan.y < 0.0)
          this.m_currentPan = new Vector2(this.m_currentPan.x, 0.0f);
        flag = true;
      }
      else if (shortcutsManager.IsOn(shortcutsManager.MoveDown))
      {
        --y;
        if ((double) this.m_currentPan.y > 0.0)
          this.m_currentPan = new Vector2(this.m_currentPan.x, 0.0f);
        flag = true;
      }
      this.m_currentPan = this.m_currentPan.SmoothDampToUnscaledTime(new Vector2(x, y).normalized, ScrollableContainer.PAN_DAMPING_SHARPNESS);
      if (this.m_currentPan.sqrMagnitude.IsNearZero())
        return flag;
      float num = smoothDeltaTime * 1000f;
      if (shortcutsManager.IsOn(shortcutsManager.PanSpeedBoost))
        num *= 2f;
      this.ScrollByPixels(this.m_currentPan * (num / this.m_scrollRect.content.localScale.x));
      return true;
    }

    static ScrollableContainer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ScrollableContainer.PAN_DAMPING_SHARPNESS = 0.3f;
    }

    public class ScrollListener : MonoBehaviour, IScrollHandler, IEventSystemHandler, IDragHandler
    {
      private ScrollRect m_scrollRect;

      public Option<Action> OnScrollAction { get; set; }

      public Option<Action> OnDragAction { get; set; }

      [PublicAPI("Unity MB API")]
      private void Awake() => this.m_scrollRect = this.gameObject.GetComponent<ScrollRect>();

      public void OnScroll(PointerEventData eventData)
      {
        if (!this.OnScrollAction.HasValue)
          return;
        this.OnScrollAction.Value();
      }

      public void OnDrag(PointerEventData eventData)
      {
        if (!this.OnDragAction.HasValue)
          return;
        this.OnDragAction.Value();
      }

      public ScrollListener()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
