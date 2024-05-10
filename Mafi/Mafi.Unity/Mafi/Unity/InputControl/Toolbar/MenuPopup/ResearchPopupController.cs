// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.ResearchPopupController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
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
  public class ResearchPopupController : IUnityUi
  {
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly LayoutEntityPopupProvider m_layoutEntityPopupProvider;
    private readonly DefaultProtoPopupProvider m_defaultProvider;
    private readonly ImmutableArray<IPopupProvider> m_providers;
    private readonly MenuPopupView m_view;
    private UiBuilder m_builder;
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
    private Option<IProto> m_selectedItem;
    private bool m_wasBuilt;
    private RectTransform m_parent;

    public ResearchPopupController(
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
      this.m_layoutEntityPopupProvider = new LayoutEntityPopupProvider(unlockedProtosDb);
      this.m_defaultProvider = new DefaultProtoPopupProvider();
    }

    public void SetParentToUse(RectTransform transform) => this.m_parent = transform;

    public void RegisterUi(UiBuilder builder)
    {
      this.m_builder = builder;
      if ((UnityEngine.Object) this.m_parent == (UnityEngine.Object) null)
        this.m_parent = builder.MainCanvas.RectTransform;
      this.m_view.BuildUi(builder);
      this.m_view.Hide();
      this.m_wasBuilt = true;
    }

    public void Activate()
    {
      this.m_gameLoop.RenderUpdate.AddNonSaveable<ResearchPopupController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void Deactivate()
    {
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<ResearchPopupController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void ItemHovered(Option<IProto> item, IUiElement element)
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
          IPopupProvider popupProvider = this.m_providers.FirstOrDefault((Func<IPopupProvider, bool>) (p => p.SupportedType.IsInstanceOfType((object) item.Value))) ?? (!(item.Value is LayoutEntityProto) ? (IPopupProvider) this.m_defaultProvider : (IPopupProvider) this.m_layoutEntityPopupProvider);
          this.m_view.Reset();
          popupProvider.PopulateView(this.m_view, (object) item.Value, true);
          this.m_view.SetupFinished();
          this.positionAndShow(element);
        }
      }
    }

    private void positionAndShow(IUiElement hoveredElement)
    {
      float x = hoveredElement.RectTransform.position.x / this.m_builder.MainCanvas.ScaleFactor - (float) ((double) this.m_view.GetWidth() / 2.0 + 20.0);
      float num = hoveredElement.RectTransform.position.y / this.m_builder.MainCanvas.ScaleFactor + hoveredElement.GetHeight() / 2f;
      this.m_view.RectTransform.anchorMin = Vector2.zero;
      this.m_view.RectTransform.anchorMax = Vector2.zero;
      this.m_view.RectTransform.anchoredPosition = new Vector2(x, num - this.m_view.GetHeight() / 2f);
      this.m_view.RectTransform.SetParent((Transform) this.m_parent, false);
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
        this.m_selectedItem = (Option<IProto>) Option.None;
      }
    }
  }
}
