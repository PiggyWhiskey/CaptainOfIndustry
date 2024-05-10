// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.WaterRendererManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Localization;
using Mafi.Unity.Utils;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [HasRenderingSettings]
  public class WaterRendererManager : IDisposable
  {
    public static readonly LocStr OceanRenderingQuality;
    public static readonly RenderingSetting WaterRenderingSettings;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly DependencyResolver m_resolver;
    private IWaterRenderer m_waterRenderer;
    private float m_specularIntensity;
    private bool m_isOceanRenderingEnabled;

    public bool IsLightCookieEnabled { get; private set; }

    public WaterRendererManager(
      IGameLoopEvents gameLoopEvents,
      DependencyResolver resolver,
      TerrainRenderer terrainRenderer,
      ReloadAfterAssetUpdateManager reloadManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_specularIntensity = 1f;
      this.m_isOceanRenderingEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_resolver = resolver;
      WaterRendererManager.WaterRenderingSettings.OnSettingChange += new Action<RenderingSetting>(this.onOceanRenderingSettingsChange);
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.createWaterRenderer));
    }

    public void Dispose()
    {
      WaterRendererManager.WaterRenderingSettings.OnSettingChange -= new Action<RenderingSetting>(this.onOceanRenderingSettingsChange);
      this.m_waterRenderer?.UnregisterAndDispose();
    }

    private void createWaterRenderer()
    {
      if (this.m_gameLoopEvents.IsTerminated)
      {
        Log.Error("Hanging event handler on terminated game.");
        WaterRendererManager.WaterRenderingSettings.OnSettingChange -= new Action<RenderingSetting>(this.onOceanRenderingSettingsChange);
      }
      else
      {
        this.m_waterRenderer?.UnregisterAndDispose();
        this.m_waterRenderer = WaterRendererManager.WaterRenderingSettings.CurrentOption.Value != 1 || !WaterRendererFft.IsSupported ? (IWaterRenderer) this.m_resolver.Instantiate<WaterRenderer>() : (IWaterRenderer) this.m_resolver.Instantiate<WaterRendererFft>();
        if (!this.m_waterRenderer.TryInitializeAndRegister(this))
        {
          Log.Info("Failed to initialize simulated ocean, falling back to non-simulated one.");
          this.m_waterRenderer.UnregisterAndDispose();
          this.m_waterRenderer = (IWaterRenderer) this.m_resolver.Instantiate<WaterRenderer>();
          this.m_waterRenderer.TryInitializeAndRegister(this);
        }
        this.m_waterRenderer.SetOceanRenderingState(this.m_isOceanRenderingEnabled);
      }
    }

    private void onOceanRenderingSettingsChange(RenderingSetting setting)
    {
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<WaterRendererManager>(this, new Action<GameTime>(this.changeRendererInSync));
    }

    private void changeRendererInSync(GameTime obj)
    {
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<WaterRendererManager>(this, new Action<GameTime>(this.changeRendererInSync));
      this.createWaterRenderer();
    }

    public void SetSpecularIntensity(float percent)
    {
      this.m_specularIntensity = percent;
      this.m_waterRenderer.SetSpecularIntensity(percent);
    }

    public void SetLightCookieEnabled(bool enabled)
    {
      this.IsLightCookieEnabled = enabled;
      this.m_waterRenderer.SetLightCookieEnabled(enabled);
    }

    public void NotifyChunkUpdated(Chunk2i chunk) => this.m_waterRenderer.NotifyChunkUpdated(chunk);

    public void SetOceanOpacity(float opacity) => this.m_waterRenderer.SetOpacity(opacity);

    public void SetOceanRenderingState(bool isEnabled)
    {
      this.m_isOceanRenderingEnabled = isEnabled;
      this.m_waterRenderer.SetOceanRenderingState(isEnabled);
    }

    static WaterRendererManager()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WaterRendererManager.OceanRenderingQuality = Loc.Str(nameof (OceanRenderingQuality), "Ocean quality", "title for ocean quality setting");
      WaterRendererManager.WaterRenderingSettings = new RenderingSetting("WaterRenderingQuality", WaterRendererManager.OceanRenderingQuality, 160, ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__High, 1, RenderingSettingPreset.MediumQuality | RenderingSettingPreset.HighQuality | RenderingSettingPreset.UltraQuality, WaterRendererFft.IsSupported), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Low, 0, RenderingSettingPreset.LowQuality)));
    }
  }
}
