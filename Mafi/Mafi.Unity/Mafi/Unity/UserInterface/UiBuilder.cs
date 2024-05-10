// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.UiBuilder
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.FloatingPanel;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class UiBuilder
  {
    public readonly AssetsDb AssetsDb;
    public readonly AudioDb AudioDb;
    public readonly UiStyle Style;
    public readonly UiAudio Audio;
    private readonly LazyResolve<ShortcutsManager> m_shortcutsMap;
    private Option<IRootEscapeManager> m_inputManager;
    public readonly Canvass MainCanvas;
    public readonly Mafi.Unity.UiFramework.Components.Panel GameOverlay;
    public readonly Canvass GameOverlayCanvas;
    public readonly Mafi.Unity.UiFramework.Components.Panel GameOverlaySuper;
    internal readonly Canvass GameOverlaySuperCanvas;
    public readonly UIDocument UiDocument;
    private readonly UiComponent MainContainer;
    public readonly UiComponent FloatingItemsContainer;
    public readonly Option<FontAsset> Font;
    public readonly Option<FontAsset> FontMonoSpace;
    public readonly UiPlayerPrefs UiPreferences;
    public readonly RecipeDurationNormalizer DurationNormalizer;
    /// <summary>
    /// Avoids UI using static cache for singletons. Users of the cache are responsible to have unique key
    /// and cast values to proper types.
    /// </summary>
    internal Dict<string, object> ElementsCache;
    internal Dict<System.Type, object> DepsCache;
    internal readonly AudioSource ClickSound;
    internal readonly AudioSource InvalidOpSound;
    /// <summary>
    /// Dummy go that is created to be instantiated instead of us calling new GameObject(). This allows
    /// to set parent before new go is created and avoid expensive re-parenting.
    /// </summary>
    private readonly GameObject m_goToClone;
    private NotificationToast m_notification;

    public ShortcutsManager ShortcutsManager => this.m_shortcutsMap.Value;

    public Option<Mafi.Unity.InputControl.TopStatusBar.StatusBar> StatusBar { get; set; }

    public bool IsUiVisible => this.MainCanvas.GameObject.activeSelf;

    public event Action<GameTime> RenderUpdate;

    public UiBuilder(
      AssetsDb assetsDb,
      AudioDb audioDb,
      UiStyle style,
      UiAudio audio,
      LazyResolve<ShortcutsManager> shortcutsMap)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.ElementsCache = new Dict<string, object>();
      this.DepsCache = new Dict<System.Type, object>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_goToClone = new GameObject("UiGo");
      this.m_goToClone.AddComponent<RectTransform>();
      this.m_goToClone.AddComponent<CanvasRenderer>();
      this.AssetsDb = assetsDb;
      this.AudioDb = audioDb;
      this.Style = style;
      this.Audio = audio;
      this.m_shortcutsMap = shortcutsMap;
      this.Font = this.AssetsDb.GetSharedFontAsset("Assets/Unity/TextMeshPro/Fonts/Main-Regular/Roboto-Dynamic.asset");
      this.FontMonoSpace = this.AssetsDb.GetSharedFontAsset("Assets/Unity/TextMeshPro/Fonts/Console/RobotoMono-Dynamic.asset");
      this.ClickSound = this.AudioDb.GetSharedAudio("Assets/Unity/UserInterface/Audio/ButtonClick.prefab", AudioChannel.UserInterface);
      this.InvalidOpSound = this.AudioDb.GetSharedAudio("Assets/Unity/UserInterface/Audio/InvalidOp.prefab", AudioChannel.UserInterface);
      this.UiPreferences = new UiPlayerPrefs();
      this.DurationNormalizer = new RecipeDurationNormalizer(this);
      this.MainCanvas = this.NewCanvas("Main canvas").SetRenderMode(RenderMode.ScreenSpaceOverlay).SetConstantPixelSize().SetSortOrder(0).MakeInteractive();
      this.GameOverlayCanvas = this.NewCanvas("Game overlay canvas").SetRenderMode(RenderMode.ScreenSpaceOverlay).SetConstantPixelSize().SetSortOrder(10).MakeInteractive();
      this.GameOverlay = this.NewPanel(nameof (GameOverlay)).SetBackground(this.Style.Toolbar.GameMenuOverlayBgColor).PutTo<Mafi.Unity.UiFramework.Components.Panel>((IUiElement) this.GameOverlayCanvas, Mafi.Unity.UiFramework.Offset.Zero).Hide<Mafi.Unity.UiFramework.Components.Panel>();
      this.GameOverlaySuperCanvas = this.NewCanvas("Game overlay super canvas").SetRenderMode(RenderMode.ScreenSpaceOverlay).SetConstantPixelSize().SetSortOrder(20).MakeInteractive();
      this.GameOverlaySuper = this.NewPanel(nameof (GameOverlaySuper)).SetBackground(this.Style.Toolbar.GameMenuOverlayBgColor).PutTo<Mafi.Unity.UiFramework.Components.Panel>((IUiElement) this.GameOverlaySuperCanvas, Mafi.Unity.UiFramework.Offset.Zero).Hide<Mafi.Unity.UiFramework.Components.Panel>();
      this.UiDocument = new GameObject("UIDocument").AddComponent<UIDocument>();
      this.UiDocument.panelSettings = this.AssetsDb.GetSharedAssetOrThrow<PanelSettings>("Assets/Unity/UI Toolkit/PanelSettings.asset");
      this.UiDocument.panelSettings.scaleMode = PanelScaleMode.ConstantPixelSize;
      this.UiDocument.panelSettings.scale = UiScaleHelper.GetCurrentScaleFloat();
      VisualElement rootVisualElement = this.UiDocument.rootVisualElement;
      rootVisualElement.style.position = (StyleEnum<Position>) Position.Absolute;
      rootVisualElement.style.top = (StyleLength) 0.0f;
      rootVisualElement.style.right = (StyleLength) 0.0f;
      rootVisualElement.style.bottom = (StyleLength) 0.0f;
      rootVisualElement.style.left = (StyleLength) 0.0f;
      this.MainContainer = new UiComponent(new VisualElement());
      this.MainContainer.Name<UiComponent>(nameof (MainContainer)).AbsolutePositionFillParent<UiComponent>().IgnoreInputPicking<UiComponent>();
      rootVisualElement.Add(this.MainContainer.Build(this));
      this.FloatingItemsContainer = new UiComponent(new VisualElement());
      this.FloatingItemsContainer.Name<UiComponent>(nameof (FloatingItemsContainer)).AbsolutePositionFillParent<UiComponent>().IgnoreInputPicking<UiComponent>();
      rootVisualElement.Add(this.FloatingItemsContainer.Build(this));
      this.FloatingItemsContainer.Add((UiComponent) (this.m_notification = new NotificationToast()));
    }

    public void AddComponent(UiComponent component, UiBuilder.UiMode mode = UiBuilder.UiMode.Game)
    {
      this.MainContainer.Add(component);
    }

    public void AddFloatingComponent(UiComponent component)
    {
      this.FloatingItemsContainer.Add(component);
    }

    public void ShowSuccessNotification(LocStrFormatted message, LocStrFormatted tooltip = default (LocStrFormatted))
    {
      this.m_notification.ShowSuccess(message, tooltip);
    }

    public void ShowGeneralNotification(LocStrFormatted message, bool showForever = false)
    {
      this.m_notification.ShowGeneral(message, showForever);
    }

    public void ShowFailureNotification(LocStrFormatted message, LocStrFormatted details = default (LocStrFormatted))
    {
      this.m_notification.ShowError(message, details);
    }

    public void SetRootEscManager(IRootEscapeManager manager)
    {
      this.m_inputManager = manager.CreateOption<IRootEscapeManager>();
    }

    public void SetOneTimeEscBlockingCallback(IRootEscapeHandler listener)
    {
      this.m_inputManager.ValueOrNull?.SetRootEscapeHandler(listener);
    }

    public void ClearEscBlockingCallback(IRootEscapeHandler listener)
    {
      this.m_inputManager.ValueOrNull?.ClearRootEscapeHandler(listener);
    }

    public GameObject GetClonedGo(string name, GameObject parent)
    {
      if ((UnityEngine.Object) parent == (UnityEngine.Object) null)
      {
        GameObject clonedGo = new GameObject(name);
        clonedGo.AddComponent<RectTransform>();
        clonedGo.AddComponent<CanvasRenderer>();
        return clonedGo;
      }
      GameObject clonedGo1 = UnityEngine.Object.Instantiate<GameObject>(this.m_goToClone, parent.transform, false);
      clonedGo1.name = name;
      return clonedGo1;
    }

    public void SetUiVisibility(bool isVisible) => this.MainCanvas.GameObject.SetActive(isVisible);

    internal Option<T> GetDependency<T>() where T : class
    {
      object obj;
      return this.DepsCache.TryGetValue(typeof (T), out obj) ? (Option<T>) (obj as T) : Option<T>.None;
    }

    internal void AddDependency<T>(T impl) where T : class
    {
      this.DepsCache[typeof (T)] = (object) impl;
    }

    private void renderUpdate(GameTime time)
    {
      Action<GameTime> renderUpdate = this.RenderUpdate;
      if (renderUpdate == null)
        return;
      renderUpdate(time);
    }

    public void Destroy()
    {
    }

    internal void RunRenderUpdate(GameTime gameTime) => this.renderUpdate(gameTime);

    public Queueue<T> GetOrCreateCache<T>(string cacheId)
    {
      object cache;
      if (!this.ElementsCache.TryGetValue(cacheId, out cache))
      {
        cache = (object) new Queueue<T>();
        this.ElementsCache.Add(cacheId, cache);
      }
      return (Queueue<T>) cache;
    }

    public enum UiMode
    {
      Game,
      MapEditor,
    }

    [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
    internal class BuilderUpdater : IUnityUi
    {
      private readonly UiBuilder m_builder;

      public BuilderUpdater(UiBuilder builder, IGameLoopEvents gameLoopEvents)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_builder = builder;
        gameLoopEvents.RenderUpdate.AddNonSaveable<UiBuilder.BuilderUpdater>(this, new Action<GameTime>(this.renderUpdate));
      }

      private void renderUpdate(GameTime time) => this.m_builder.renderUpdate(time);

      public void RegisterUi(UiBuilder builder)
      {
      }
    }
  }
}
