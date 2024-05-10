// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.MenuPopupController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Unity.InputControl.Toolbar.EntitiesMenu;
using Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class MenuPopupController : IUnityUi
  {
    /// <summary>
    /// How many milliseconds it should take before the popup gets hidden after the player hovers out of its item.
    /// </summary>
    public const int MS_BEFORE_HIDE = 200;
    /// <summary>
    /// How many milliseconds it should take before the popup gets shown after the player hovers over its item.
    /// </summary>
    public const int MS_BEFORE_SHOW = 100;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IPopupProvider m_defaultProvider;
    private readonly ImmutableArray<IPopupProvider> m_providers;
    private readonly SurfaceItemPopupProvider m_surfaceItemProvider;
    private readonly MenuPopupView m_view;
    private UiBuilder m_builder;
    /// <summary>Offset of the popup from the bottom of the screen.</summary>
    private float m_bottomOffset;
    /// <summary>
    /// Time in ms when hiding was started. 0 if it should be started.
    /// </summary>
    private long m_hidingStart;
    private bool m_hideRequested;
    /// <summary>
    /// Time in ms when show was started. 0 if it should be started.
    /// </summary>
    private Fix64 m_showStart;
    private bool m_showRequested;
    private Option<EntitiesMenuItem> m_selectedItem;
    private bool m_wasBuilt;

    public MenuPopupController(
      ShortcutsManager shortcutsManager,
      IGameLoopEvents gameLoop,
      NewInstanceOf<MenuPopupView> popupView,
      UnlockedProtosDbForUi unlockedProtosDb,
      AllImplementationsOf<IPopupProvider> popupProviders)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_showStart = (Fix64) -1L;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.m_gameLoop = gameLoop;
      this.m_view = popupView.Instance;
      this.m_providers = popupProviders.Implementations;
      this.m_surfaceItemProvider = new SurfaceItemPopupProvider();
      this.m_defaultProvider = (IPopupProvider) new LayoutEntityPopupProvider(unlockedProtosDb);
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_builder = builder;
      this.m_view.BuildUi(builder);
      this.m_view.Hide();
      this.m_bottomOffset = (float) ((double) builder.Style.EntitiesMenu.ItemHeight + (double) builder.Style.Toolbar.MainMenuStripHeight + 5.0);
      this.m_wasBuilt = true;
    }

    public void Activate()
    {
      this.m_gameLoop.RenderUpdate.AddNonSaveable<MenuPopupController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void Deactivate()
    {
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<MenuPopupController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void ItemHovered(Option<EntitiesMenuItem> item, float x)
    {
      if (!this.m_wasBuilt)
        return;
      if (item.IsNone)
      {
        this.m_showRequested = false;
        if (!this.m_view.GameObject.activeInHierarchy)
          return;
        this.m_hideRequested = true;
        this.m_hidingStart = 0L;
      }
      else
      {
        this.m_hideRequested = false;
        if (item == this.m_selectedItem)
        {
          this.m_view.Show();
        }
        else
        {
          this.m_selectedItem = item;
          this.m_view.Hide();
          this.m_view.Reset();
          if (item.Value is PaintSurfaceDecalsMenuItem)
            return;
          if (item.Value is TerrainTileSurfaceMenuItem)
            this.m_surfaceItemProvider.PopulateView(this.m_view, (object) item.Value, false);
          else if (item.Value.Proto.HasValue)
            (this.m_providers.FirstOrDefault((Func<IPopupProvider, bool>) (p => p.SupportedType.IsInstanceOfType((object) item.Value.Proto.Value))) ?? this.m_defaultProvider).PopulateView(this.m_view, (object) item.Value.Proto.Value, false);
          this.m_view.SetupFinished();
          this.positionAndShow(x);
        }
      }
    }

    public void HideAll()
    {
      this.m_view.Hide();
      this.m_hideRequested = false;
      this.m_showRequested = false;
      this.m_selectedItem = (Option<EntitiesMenuItem>) Option.None;
    }

    private void positionAndShow(float x)
    {
      float width = this.m_builder.MainCanvas.GetWidth();
      float num = this.m_view.GetWidth() / 2f;
      x /= this.m_builder.MainCanvas.ScaleFactor;
      x = Mathf.Max(x, 10f + num);
      x = Mathf.Min(x, width - 10f - num);
      this.m_view.RectTransform.anchorMin = Vector2.zero;
      this.m_view.RectTransform.anchorMax = Vector2.zero;
      this.m_view.RectTransform.anchoredPosition = new Vector2(x, this.m_bottomOffset + this.m_view.GetHeight() / 2f);
      this.m_view.RectTransform.SetParent((Transform) this.m_builder.MainCanvas.RectTransform, false);
      this.m_showRequested = true;
      this.m_showStart = (Fix64) 0L;
    }

    private void renderUpdate(GameTime time)
    {
      this.updateViewVisibility();
      if (!this.m_view.IsVisible)
        return;
      this.m_view.RenderUpdate(time);
    }

    private void updateViewVisibility()
    {
      if (this.m_showRequested)
      {
        if (this.m_showStart == 0)
          this.m_showStart = (Fix64) (long) Environment.TickCount;
        else if ((Environment.TickCount - this.m_showStart).Abs() > 100)
        {
          this.m_showRequested = false;
          this.m_view.Show();
        }
      }
      if (!this.m_hideRequested)
        return;
      if (this.m_view.IsHovered || this.m_shortcutsManager.IsPrimaryActionOn)
        this.m_hidingStart = 0L;
      else if (this.m_hidingStart == 0L)
      {
        this.m_hidingStart = (long) Environment.TickCount;
      }
      else
      {
        if (((long) Environment.TickCount - this.m_hidingStart).Abs() <= 200L)
          return;
        this.m_hideRequested = false;
        this.m_view.Hide();
        this.m_selectedItem = (Option<EntitiesMenuItem>) Option.None;
      }
    }
  }
}
