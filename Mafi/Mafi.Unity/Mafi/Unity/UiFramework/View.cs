// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.View
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public abstract class View : IUiElement
  {
    protected UiBuilder Builder;
    protected UiStyle Style;
    private Panel m_mainPanel;
    private Option<AudioSource> m_onShowSound;
    private Option<AudioSource> m_onHideSound;
    private readonly Vector3[] m_corners;
    private readonly string m_id;
    private readonly IUiUpdater m_updater;
    public Option<IUiElement> Parent;
    private bool m_clippingPreventionEnabled;

    public GameObject GameObject
    {
      get
      {
        Assert.That<Panel>(this.m_mainPanel).IsNotNull<Panel>("You have to build your view before accessing its GameOject.");
        return this.m_mainPanel.GameObject;
      }
    }

    public RectTransform RectTransform
    {
      get
      {
        Assert.That<Panel>(this.m_mainPanel).IsNotNull<Panel>("You have to build your view before accessing its GameOject.");
        return this.m_mainPanel.RectTransform;
      }
    }

    /// <summary>
    /// Whether the view should wait for sync and render pass before being shown.
    /// </summary>
    protected bool ShowAfterSync { get; set; }

    public bool IsVisible { get; private set; }

    /// <summary>Invoked when this view is about to be showed up.</summary>
    public event Action OnShowStart;

    /// <summary>Invoked when this view was successfully showed up.</summary>
    public event Action OnShowDone;

    /// <summary>Invoked when this view gets hidden.</summary>
    public event Action OnHide;

    protected View(string id, SyncFrequency syncFrequency = SyncFrequency.Critical)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_corners = new Vector3[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_id = id;
      this.m_updater = UpdaterBuilder.Start().Build(syncFrequency);
    }

    public View BuildIfNeeded(UiBuilder builder, IUiElement parent = null)
    {
      if (this.Builder == null)
        this.BuildUi(builder, parent);
      return this;
    }

    public View BuildAndShow(UiBuilder builder, IUiElement parent = null)
    {
      if (this.Builder == null)
        this.BuildUi(builder, parent);
      this.Show();
      return this;
    }

    public void BuildUi(UiBuilder builder) => this.BuildUi(builder, (IUiElement) null);

    public void BuildUi(UiBuilder builder, IUiElement parent)
    {
      Assert.That<Panel>(this.m_mainPanel).IsNull<Panel>("View was already built! You cannot build it twice.");
      this.Builder = builder.CheckNotNull<UiBuilder>();
      this.Style = builder.Style;
      this.Parent = parent == null ? this.GetParent(builder) : parent.SomeOption<IUiElement>();
      this.m_mainPanel = !this.Parent.HasValue ? this.Builder.NewPanel(this.m_id) : this.Builder.NewPanel(this.m_id, this.Parent.Value);
      Panel mainPanel = this.m_mainPanel;
      if (mainPanel != null)
        mainPanel.Hide<Panel>();
      this.BuildUi();
    }

    protected virtual Option<IUiElement> GetParent(UiBuilder builder) => Option<IUiElement>.None;

    protected void SetBackground(ColorRgba color) => this.m_mainPanel.SetBackground(color);

    public void SetOnShowSound(Option<AudioSource> openSound) => this.m_onShowSound = openSound;

    public void SetOnHideSound(Option<AudioSource> closeSound) => this.m_onHideSound = closeSound;

    public void OnMouseEnter(Action onMouseEnter) => this.m_mainPanel.OnMouseEnter(onMouseEnter);

    public void OnMouseLeave(Action onMouseLeave) => this.m_mainPanel.OnMouseLeave(onMouseLeave);

    protected void EnableClippingPrevention() => this.m_clippingPreventionEnabled = true;

    protected void DisableClippingPrevention() => this.m_clippingPreventionEnabled = false;

    public virtual void Show()
    {
      Assert.That<Panel>(this.m_mainPanel).IsNotNull<Panel>("You have to build your view before calling Show().");
      if (this.IsVisible)
        return;
      this.IsVisible = true;
      Action onShowStart = this.OnShowStart;
      if (onShowStart != null)
        onShowStart();
      this.m_updater.Invalidate();
      if (this.ShowAfterSync)
        this.m_updater.SetOneTimeAfterSyncCallback(new Action(this.showInternal));
      else
        this.showInternal();
    }

    private void showInternal()
    {
      if (!this.IsVisible)
        return;
      if (this.m_onShowSound.HasValue)
        this.m_onShowSound.Value.Play();
      Panel mainPanel = this.m_mainPanel;
      if (mainPanel != null)
        mainPanel.Show<Panel>();
      Action onShowDone = this.OnShowDone;
      if (onShowDone != null)
        onShowDone();
      if (!this.m_clippingPreventionEnabled)
        return;
      this.RectTransform.GetWorldCorners(this.m_corners);
      Vector3 vector3 = this.m_corners[1] / this.Builder.MainCanvas.ScaleFactor;
      int num = 90;
      if ((double) vector3.y > (double) this.Builder.MainCanvas.GetHeight() - (double) num)
        this.RectTransform.anchoredPosition = new Vector2(this.RectTransform.anchoredPosition.x, this.RectTransform.anchoredPosition.y - (vector3.y - this.Builder.MainCanvas.GetHeight() + (float) num));
      if ((double) vector3.x >= 0.0)
        return;
      this.RectTransform.anchoredPosition = new Vector2(this.RectTransform.anchoredPosition.x - vector3.x, this.RectTransform.anchoredPosition.y);
    }

    public virtual void Hide()
    {
      if (this.IsVisible && this.m_onHideSound.HasValue)
        this.m_onHideSound.Value.Play();
      Action onHide = this.OnHide;
      if (onHide != null)
        onHide();
      this.IsVisible = false;
      Panel mainPanel = this.m_mainPanel;
      if (mainPanel == null)
        return;
      mainPanel.Hide<Panel>();
    }

    public void Destroy()
    {
      Panel mainPanel = this.m_mainPanel;
      if (mainPanel == null)
        return;
      mainPanel.Destroy();
    }

    public void AddUpdater(IUiUpdater updater) => this.m_updater.AddChildUpdater(updater);

    public virtual void RenderUpdate(GameTime gameTime) => this.m_updater.RenderUpdate();

    public virtual void SyncUpdate(GameTime gameTime) => this.m_updater.SyncUpdate();

    protected abstract void BuildUi();
  }
}
