// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.WeatherView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Environment;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class WeatherView : IUiElement, IDynamicSizeElement, IStatusBarItem
  {
    private readonly IWeatherManager m_weatherManager;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly UiBuilder m_builder;
    private IUiUpdater m_updater;
    private IconContainer m_iconContainer;
    private Tooltip m_tooltip;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_iconContainer.GameObject;

    public RectTransform RectTransform => this.m_iconContainer.RectTransform;

    public WeatherView(IWeatherManager weatherManager, IGameLoopEvents gameLoop, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_weatherManager = weatherManager;
      this.m_gameLoop = gameLoop;
      this.m_builder = builder;
    }

    private void renderUpdate(GameTime gameTime) => this.m_updater.RenderUpdate();

    private void syncUpdate(GameTime time) => this.m_updater.SyncUpdate();

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_iconContainer = this.m_builder.NewIconContainer("WeatherLabel").SetHeight<IconContainer>(30f).SetWidth<IconContainer>(24f);
      this.m_tooltip = this.m_builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) this.m_iconContainer);
      statusBar.AddElementToRight((IUiElement) this, 100f, true);
      this.m_updater = UpdaterBuilder.Start().Observe<WeatherProto>((Func<WeatherProto>) (() => this.m_weatherManager.CurrentWeather)).Do((Action<WeatherProto>) (weather =>
      {
        this.m_iconContainer.SetIcon(weather.Graphics.IconPath, this.m_builder.Style.StatusBar.IconColor);
        this.m_tooltip.SetText((LocStrFormatted) weather.Strings.Name);
      })).Build();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<WeatherView>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<WeatherView>(this, new Action<GameTime>(this.renderUpdate));
    }
  }
}
