// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.GameSaveInfoProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Research;
using Mafi.Core.SaveGame;
using Mafi.Core.Simulation;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Terrain.Generation;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class GameSaveInfoProvider : IGameSaveInfoProvider
  {
    public static readonly Vector2i GAME_SAVE_THUMBNAIL_SIZE;
    public const int GAME_SAVE_THUMBNAIL_JPQ_QUALITY = 90;
    public const ScreenshotConfigFlags GAME_SAVE_THUMBNAIL_CONFIG_FLAGS = ScreenshotConfigFlags.DisableIcons | ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars | ScreenshotConfigFlags.Force8xMsaa;
    private readonly ICalendar m_calendar;
    private readonly SettlementsManager m_settlementsManager;
    private readonly ResearchManager m_researchManager;
    private readonly IRocketLaunchManager m_rocketLaunchManager;
    private readonly ScreenshotTaker m_screenshotTaker;
    private Option<DependencyResolver> m_resolver;
    private Option<IWorldRegionMapPreviewData> m_mapData;
    private Action m_renderDoneCallback;
    private Option<byte[]> m_screenshotData;
    private bool m_screenshotTakingIsPending;

    public GameSaveInfoProvider(
      ICalendar calendar,
      SettlementsManager settlementsManager,
      ResearchManager researchManager,
      IRocketLaunchManager rocketLaunchManager,
      ScreenshotTaker screenshotTaker,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_calendar = calendar;
      this.m_settlementsManager = settlementsManager;
      this.m_researchManager = researchManager;
      this.m_rocketLaunchManager = rocketLaunchManager;
      this.m_screenshotTaker = screenshotTaker;
      this.m_resolver = (Option<DependencyResolver>) resolver;
    }

    /// <summary>
    /// This should be called one frame in advance of <see cref="M:Mafi.Unity.Utils.GameSaveInfoProvider.CreateGameSaveInfo" /> to get the save thumbnail.
    /// </summary>
    public void ScheduleScreenshotRendering(Action doneCallback)
    {
      if (this.m_screenshotTakingIsPending)
      {
        Assert.That<bool>(this.m_screenshotTakingIsPending).IsFalse("Screenshot still pending.");
      }
      else
      {
        this.m_screenshotTakingIsPending = true;
        this.m_renderDoneCallback = doneCallback;
        ScreenshotTaker screenshotTaker = this.m_screenshotTaker;
        int x = GameSaveInfoProvider.GAME_SAVE_THUMBNAIL_SIZE.X;
        int y = GameSaveInfoProvider.GAME_SAVE_THUMBNAIL_SIZE.Y;
        Action<Option<Texture2D>, Camera, string> action = new Action<Option<Texture2D>, Camera, string>(this.screenshotCaptured);
        Option<string> filePathWithoutExt = new Option<string>();
        Action<Option<Texture2D>, Camera, string> encodedDataCallback = action;
        int customWidth = x;
        int customHeight = y;
        Vector3? cameraPosition = new Vector3?();
        Quaternion? cameraRotation = new Quaternion?();
        float? verticalFieldOfView = new float?();
        Vector2? nearFarClipPlane = new Vector2?();
        float? customFogDensity = new float?();
        screenshotTaker.ScheduleScreenshotCapture(filePathWithoutExt, encodedDataCallback, customWidth, customHeight, cameraPosition, cameraRotation, verticalFieldOfView, nearFarClipPlane, true, configFlags: ScreenshotConfigFlags.DisableIcons | ScreenshotConfigFlags.DisableTerrainOverlays | ScreenshotConfigFlags.DisableTerrainGrid | ScreenshotConfigFlags.DisableHighlights | ScreenshotConfigFlags.DisableResourceBars | ScreenshotConfigFlags.Force8xMsaa, customFogDensity: customFogDensity);
      }
    }

    public void NotifySaveDone()
    {
      if (this.m_screenshotTakingIsPending)
      {
        Log.Warning("Save screenshot was not captured in time.");
        this.m_screenshotTakingIsPending = false;
      }
      this.m_screenshotData = Option<byte[]>.None;
      this.m_renderDoneCallback = (Action) null;
    }

    private void screenshotCaptured(Option<Texture2D> texture, Camera camera, string error)
    {
      if (!this.m_screenshotTakingIsPending)
      {
        Log.Warning("Screenshot not captured in time.");
      }
      else
      {
        this.m_screenshotTakingIsPending = false;
        Texture2D valueOrNull = texture.ValueOrNull;
        this.m_screenshotData = (Option<byte[]>) (valueOrNull != null ? valueOrNull.EncodeToJPG(90) : (byte[]) null);
        Action renderDoneCallback = this.m_renderDoneCallback;
        if (renderDoneCallback != null)
          renderDoneCallback();
        this.m_renderDoneCallback = (Action) null;
      }
    }

    /// <summary>
    /// This should be called only after save was requested in the <see cref="T:Mafi.Core.SaveGame.SaveManager" />,
    /// otherwise screenshot data won't be available.
    /// </summary>
    public GameSaveInfo CreateGameSaveInfo()
    {
      Assert.That<bool>(this.m_screenshotTakingIsPending).IsFalse("Screenshot was not captured in time.");
      if (this.m_resolver.HasValue)
      {
        IWorldRegionMapPreviewData dep;
        if (this.m_resolver.Value.TryResolve<IWorldRegionMapPreviewData>(out dep))
          this.m_mapData = dep.CreateOption<IWorldRegionMapPreviewData>();
        this.m_resolver = Option<DependencyResolver>.None;
      }
      return new GameSaveInfo(this.m_calendar.CurrentDate, this.m_mapData.ValueOrNull?.Name ?? "", this.m_settlementsManager.GetTotalPopulation(), this.m_researchManager.AllNodes.Count((Func<ResearchNode, bool>) (x => x.State == ResearchNodeState.Researched)), this.m_researchManager.AllNodes.Length, this.m_rocketLaunchManager.LaunchesCount, "", GameSaveInfoProvider.GAME_SAVE_THUMBNAIL_SIZE, this.m_screenshotData.ValueOrNull ?? Array.Empty<byte>());
    }

    static GameSaveInfoProvider()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GameSaveInfoProvider.GAME_SAVE_THUMBNAIL_SIZE = new Vector2i(640, 360);
    }
  }
}
