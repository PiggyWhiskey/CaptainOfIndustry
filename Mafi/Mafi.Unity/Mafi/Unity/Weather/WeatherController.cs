// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Weather.WeatherController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Environment;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Unity.Audio;
using Mafi.Unity.Camera;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Weather
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class WeatherController
  {
    private static readonly Duration TRANSITION_DURATION;
    private static readonly int MAFI_WIND_STRENGTH;
    private static readonly int MAFI_WORLD_WETNESS_SMOOTHING;
    private static readonly int MAFI_WORLD_WETNESS_BRIGHTNESS;
    private static readonly ImmutableArray<string> THUNDER_ASSETS;
    private static readonly ImmutableArray<string> THUNDER_RUMBLE_ASSETS;
    private readonly IWeatherManager m_weatherManager;
    private readonly CameraController m_cameraController;
    private readonly LightController m_lightController;
    private readonly FogController m_fogController;
    private readonly AssetsDb m_assetsDb;
    private readonly AudioDb m_audioDb;
    private readonly IRandom m_random;
    private readonly Lyst<KeyValuePair<WeatherProto, WeatherController.AudioSourceController>> m_audioControllers;
    private GameObject m_audioGo;
    private GameObject m_rainGo;
    private float m_weatherHeight;
    private ParticleSystem m_rainPs;
    private Option<WeatherController.AudioSourceController> m_currentAudio;
    private Option<WeatherController.AudioSourceController> m_oldAudio;
    private float m_maxRate;
    private WeatherProto m_currentWeather;
    private WeatherProto m_oldWeather;
    private float m_transitionLeftTicks;
    private Vector3 m_pausedRainPos;
    private float m_oldTargetCloudIntensity;
    private float m_targetCloudIntensity;
    private readonly System.Random m_lightningRng;
    private float m_timeFromLightningStrike;
    private readonly ImmutableArray<AudioSource> m_thunderAudioSources;
    private readonly ImmutableArray<AudioSource> m_thunderRumbleAudioSources;
    private Option<AudioSource> m_currentThunderSound;
    private float m_lightningIntensityMult;
    private int m_lightningCount;
    private readonly SkyboxController m_skyboxController;
    private bool m_areCloudsDisabled;

    public bool WeatherIsVisible => this.m_rainGo.activeSelf;

    public WeatherController(
      IGameLoopEvents gameLoopEvents,
      IWeatherManager weatherManager,
      CameraController cameraController,
      LightController lightController,
      FogController fogController,
      SkyboxController skyboxController,
      AssetsDb assetsDb,
      AudioDb audioDb,
      IWeatherRenderingConfig config,
      RandomProvider randomProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_audioControllers = new Lyst<KeyValuePair<WeatherProto, WeatherController.AudioSourceController>>();
      this.m_lightningRng = new System.Random();
      this.m_timeFromLightningStrike = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (config.DisableWeatherRendering)
        return;
      this.m_weatherManager = weatherManager;
      this.m_cameraController = cameraController;
      this.m_lightController = lightController;
      this.m_fogController = fogController;
      this.m_skyboxController = skyboxController;
      this.m_assetsDb = assetsDb;
      this.m_audioDb = audioDb;
      this.m_random = randomProvider.GetNonSimRandomFor((object) this, System.Environment.TickCount.ToString());
      ImmutableArray<AudioSource> immutableArray = WeatherController.THUNDER_ASSETS.Map<AudioSource>((Func<string, AudioSource>) (x => this.m_audioDb.GetClonedAudio(x, AudioChannel.Weather)));
      this.m_thunderAudioSources = immutableArray.Filter((Predicate<AudioSource>) (x => (bool) (UnityEngine.Object) x));
      immutableArray = WeatherController.THUNDER_RUMBLE_ASSETS.Map<AudioSource>((Func<string, AudioSource>) (x => this.m_audioDb.GetClonedAudio(x, AudioChannel.Weather)));
      this.m_thunderRumbleAudioSources = immutableArray.Filter((Predicate<AudioSource>) (x => (bool) (UnityEngine.Object) x));
      foreach (AudioSource thunderAudioSource in this.m_thunderAudioSources)
        thunderAudioSource.loop = false;
      foreach (AudioSource rumbleAudioSource in this.m_thunderRumbleAudioSources)
        rumbleAudioSource.loop = false;
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.SyncUpdate.AddNonSaveable<WeatherController>(this, new Action<GameTime>(this.sync));
      gameLoopEvents.RenderUpdate.AddNonSaveable<WeatherController>(this, new Action<GameTime>(this.rendererUpdate));
    }

    public void SetWeatherEffectsVisibility(bool isVisible)
    {
      this.m_rainGo.SetActive(isVisible);
      if (!isVisible)
        return;
      this.m_rainPs.Simulate(this.m_rainPs.main.startLifetime.constant, false);
    }

    public void SetCloudsEnabled(bool enabled)
    {
      this.m_targetCloudIntensity = !enabled ? 0.0f : (float) (0.5 * ((double) this.m_currentWeather.Graphics.MinCloudIntensity + (double) this.m_currentWeather.Graphics.MaxCloudIntensity));
      this.m_skyboxController.SetCloudIntensity(this.m_targetCloudIntensity);
      this.m_oldTargetCloudIntensity = this.m_targetCloudIntensity;
      this.m_areCloudsDisabled = !enabled;
    }

    private void initState()
    {
      this.m_audioGo = new GameObject("Rain audio");
      this.m_rainGo = this.m_assetsDb.GetClonedPrefabOrEmptyGo("Assets/Unity/Weather/RainParticleSystem.prefab");
      this.m_rainPs = this.m_rainGo.GetComponent<ParticleSystem>();
      this.m_rainPs.Stop();
      this.m_weatherHeight = this.m_rainGo.transform.position.y;
      this.m_rainPs.GetComponent<Renderer>().sharedMaterial.renderQueue = 3050;
      this.m_maxRate = this.m_rainPs.emission.rateOverTimeMultiplier;
      this.m_currentWeather = this.m_oldWeather = this.m_weatherManager.CurrentWeather;
      this.m_lightController.SetLightIntensity(this.m_currentWeather.Graphics.LightIntensity, this.m_currentWeather.Graphics.ShadowsIntensityAbs);
      Shader.SetGlobalFloat(WeatherController.MAFI_WIND_STRENGTH, this.m_currentWeather.Graphics.WindStrength);
      this.m_fogController.SetFogIntensity(this.m_currentWeather.Graphics.FogIntensity);
      this.m_fogController.SetFogColor(this.m_currentWeather.Graphics.FogColor.AsColor());
      this.m_targetCloudIntensity = !this.m_areCloudsDisabled ? (float) (0.5 * ((double) this.m_currentWeather.Graphics.MinCloudIntensity + (double) this.m_currentWeather.Graphics.MaxCloudIntensity)) : 0.0f;
      this.m_oldTargetCloudIntensity = this.m_targetCloudIntensity;
      this.m_skyboxController.SetCloudIntensity(this.m_targetCloudIntensity);
      this.m_skyboxController.SetSkyColor(this.m_currentWeather.Graphics.SkyColor.AsColor());
      this.m_skyboxController.SetFogColor(this.m_currentWeather.Graphics.FogColor.AsColor());
      this.m_skyboxController.UpdateSkyColor(this.m_currentWeather.Graphics.SkyColor.AsColor(), this.m_targetCloudIntensity);
      this.m_lightController.SetLightColor(this.m_currentWeather.Graphics.LightColor.AsColor());
      this.setRainIntensity(this.m_weatherManager.RainIntensity.ToFloat());
      this.m_currentAudio = this.getAudioSourceController(this.m_currentWeather);
      this.m_currentAudio.ValueOrNull?.Play(1f);
      this.m_currentAudio.ValueOrNull?.SkipTransition();
    }

    private void sync(GameTime time)
    {
      if ((Proto) this.m_currentWeather != (Proto) this.m_weatherManager.NextWeather)
      {
        if ((double) this.m_transitionLeftTicks > 0.0)
          Log.Warning("Previous weather transition was not finished yet.");
        this.m_transitionLeftTicks = (float) WeatherController.TRANSITION_DURATION.Ticks;
        this.m_oldWeather = this.m_currentWeather;
        this.m_currentWeather = this.m_weatherManager.NextWeather;
        this.m_oldTargetCloudIntensity = this.m_targetCloudIntensity;
        this.m_targetCloudIntensity = !this.m_areCloudsDisabled ? this.m_currentWeather.Graphics.MinCloudIntensity + this.m_random.NextFloat() * (this.m_currentWeather.Graphics.MaxCloudIntensity - this.m_currentWeather.Graphics.MinCloudIntensity) : 0.0f;
        if ((double) this.m_targetCloudIntensity < 0.0)
        {
          Log.Warning(string.Format("Target cloud intensity is negative? {0}", (object) this.m_targetCloudIntensity));
          this.m_targetCloudIntensity = -this.m_targetCloudIntensity;
        }
        this.m_oldAudio.ValueOrNull?.StopImmediate();
        if (this.getAudioSourceController(this.m_currentWeather) != this.m_currentAudio)
        {
          this.m_oldAudio = this.m_currentAudio;
          this.m_oldAudio.ValueOrNull?.Stop();
          this.m_currentAudio = this.getAudioSourceController(this.m_currentWeather);
          this.m_currentAudio.ValueOrNull?.Play(1f);
        }
      }
      if (this.m_lightningCount > 0)
      {
        --this.m_lightningCount;
        if (this.m_lightningRng.Next(2) == 0)
        {
          this.m_timeFromLightningStrike = 0.0f;
          this.m_lightningIntensityMult = (float) (this.m_lightningRng.NextDouble() * 2.0 + 1.0);
        }
      }
      else if (!time.IsGamePaused && (double) this.m_currentWeather.Graphics.LightningProbabilityPerTick > 0.0)
      {
        if (this.m_currentThunderSound.IsNone)
        {
          if (this.m_lightningRng.NextDouble() < (double) this.m_currentWeather.Graphics.LightningProbabilityPerTick)
            this.startThunderAndLightning(this.m_lightningRng.Next(2) == 0);
        }
        else if (!this.m_currentThunderSound.Value.isPlaying)
          this.m_currentThunderSound = Option<AudioSource>.None;
      }
      Shader.SetGlobalFloat(WeatherController.MAFI_WORLD_WETNESS_SMOOTHING, this.m_weatherManager.WorldWetness.ToFloat() * this.m_lightController.LightIntensity);
      Shader.SetGlobalFloat(WeatherController.MAFI_WORLD_WETNESS_BRIGHTNESS, this.m_weatherManager.WorldWetness.ToFloat());
    }

    private void startThunderAndLightning(bool noLightRumbleOnly)
    {
      if (this.m_currentThunderSound.HasValue)
      {
        this.m_currentThunderSound.Value.Stop();
        this.m_currentThunderSound = Option<AudioSource>.None;
      }
      ImmutableArray<AudioSource> immutableArray = noLightRumbleOnly ? this.m_thunderRumbleAudioSources : this.m_thunderAudioSources;
      if (immutableArray.IsEmpty)
      {
        Log.Warning("No thunder sounds to play.");
      }
      else
      {
        int index = this.m_lightningRng.Next(immutableArray.Length);
        AudioSource audioSource = immutableArray[index];
        this.m_currentThunderSound = (Option<AudioSource>) audioSource;
        audioSource.Play();
        if (noLightRumbleOnly || GlobalPlayerPrefs.DisableFlashes)
          return;
        this.m_timeFromLightningStrike = 0.0f;
        this.m_lightningIntensityMult = (float) this.m_lightningRng.NextDouble() + 0.5f;
        this.m_lightningCount = this.m_lightningRng.Next(3) + 1;
      }
    }

    private void rendererUpdate(GameTime time)
    {
      if ((double) this.m_timeFromLightningStrike < 1.0)
      {
        float fromLightningStrike = this.m_timeFromLightningStrike;
        this.m_timeFromLightningStrike += Time.fixedUnscaledDeltaTime;
        if ((double) this.m_timeFromLightningStrike < 1.0)
          this.m_lightController.SetExtraLightIntensity(2f * (1f / (float) Math.Pow(1.0 + (double) fromLightningStrike, 60.0)) * this.m_lightningIntensityMult);
        else
          this.m_lightController.SetExtraLightIntensity(0.0f);
      }
      if (time.IsGamePaused)
      {
        if (this.m_rainPs.isEmitting)
        {
          this.m_rainPs.Pause();
          this.m_currentAudio.ValueOrNull?.Pause();
          this.m_oldAudio.ValueOrNull?.Pause();
          this.m_pausedRainPos = this.m_rainGo.transform.position;
        }
      }
      else
      {
        if (!this.m_rainPs.isEmitting)
        {
          this.m_rainPs.Play();
          this.m_currentAudio.ValueOrNull?.Resume();
          this.m_oldAudio.ValueOrNull?.Resume();
        }
        this.m_rainPs.main.simulationSpeed = (float) ((1.0 + (double) time.GameSpeedMult) * 0.5);
        if ((double) this.m_transitionLeftTicks > 0.0)
        {
          this.m_transitionLeftTicks = (this.m_transitionLeftTicks - time.DeltaSimStepsApprox.ToFloat() * (float) time.GameSpeedMult).Max(0.0f);
          float t = (float) (1.0 - (double) this.m_transitionLeftTicks / (double) WeatherController.TRANSITION_DURATION.Ticks);
          this.m_lightController.SetLightIntensity(this.m_oldWeather.Graphics.LightIntensity.Lerp(this.m_currentWeather.Graphics.LightIntensity, t), this.m_oldWeather.Graphics.ShadowsIntensityAbs.Lerp(this.m_currentWeather.Graphics.ShadowsIntensityAbs, t));
          this.m_lightController.SetLightColor(Color.Lerp(this.m_oldWeather.Graphics.LightColor.AsColor(), this.m_currentWeather.Graphics.LightColor.AsColor(), t));
          float num1 = this.m_oldWeather.Graphics.WindStrength.Lerp(this.m_currentWeather.Graphics.WindStrength, t);
          Shader.SetGlobalFloat(WeatherController.MAFI_WIND_STRENGTH, num1);
          float percent = this.m_oldWeather.Graphics.FogIntensity.Lerp(this.m_currentWeather.Graphics.FogIntensity, t);
          Color color1 = this.m_oldWeather.Graphics.FogColor.AsColor().Lerp(this.m_currentWeather.Graphics.FogColor.AsColor(), t);
          this.m_fogController.SetFogIntensity(percent);
          this.m_fogController.SetFogColor(color1);
          float num2 = Mathf.SmoothStep(this.m_oldTargetCloudIntensity, this.m_targetCloudIntensity, t);
          Color color2 = this.m_oldWeather.Graphics.SkyColor.AsColor().Lerp(this.m_currentWeather.Graphics.SkyColor.AsColor(), t);
          this.m_skyboxController.SetCloudIntensity(num2);
          this.m_skyboxController.SetFogColor(color1);
          this.m_skyboxController.SetSkyColor(color2);
          this.m_skyboxController.UpdateSkyColor(color2, num2);
          float num3 = this.m_currentWeather.RainIntensity < this.m_oldWeather.RainIntensity ? (t * 2f).Min(1f) : ((float) (((double) t - 0.5) * 2.0)).Max(0.0f);
          this.setRainIntensity(this.m_oldWeather.RainIntensity.ToFloat().Lerp(this.m_currentWeather.RainIntensity.ToFloat(), num3));
          this.m_oldAudio.ValueOrNull?.Update(num3);
          this.m_currentAudio.ValueOrNull?.Update(num3);
        }
      }
      Vector3 eyePosition = this.m_cameraController.EyePosition;
      Vector3 targetPosition = this.m_cameraController.CameraModel.TargetPosition;
      Vector3 normalized = (targetPosition - eyePosition).normalized;
      Vector3 vector3_1 = eyePosition + new Vector3((float) ((double) normalized.x * (double) this.m_rainPs.shape.scale.x * 0.40000000596046448), this.m_rainPs.shape.scale.y, (float) ((double) normalized.z * (double) this.m_rainPs.shape.scale.z * 0.40000000596046448));
      this.m_rainGo.transform.position = vector3_1;
      this.m_rainGo.transform.eulerAngles = new Vector3(0.0f, this.m_cameraController.CameraModel.Rotation.eulerAngles.y, 0.0f);
      if (time.IsGamePaused)
      {
        Vector3 vector3_2 = this.m_pausedRainPos - vector3_1;
        if ((double) Vector3.Dot(vector3_2, vector3_2) > (double) 150f.Squared())
        {
          this.m_rainPs.Simulate(this.m_rainPs.main.startLifetime.constant, false);
          this.m_pausedRainPos = vector3_1;
        }
      }
      this.m_audioGo.transform.position = this.m_cameraController.Camera.transform.localPosition with
      {
        y = targetPosition.y
      };
    }

    private void setRainIntensity(float rainIntensity)
    {
      this.m_rainPs.emission.rateOverTimeMultiplier = this.m_maxRate * rainIntensity;
      this.m_rainPs.Play();
    }

    private Option<WeatherController.AudioSourceController> getAudioSourceController(
      WeatherProto weatherProto)
    {
      if (weatherProto.Graphics.SoundPrefabPath.IsNone)
        return Option<WeatherController.AudioSourceController>.None;
      WeatherController.AudioSourceController sourceController1;
      if (this.m_audioControllers.TryGetValue<WeatherProto, WeatherController.AudioSourceController>(weatherProto, out sourceController1))
        return (Option<WeatherController.AudioSourceController>) sourceController1;
      AudioSource clonedAudio = this.m_audioDb.GetClonedAudio(weatherProto.Graphics.SoundPrefabPath.Value, AudioChannel.Weather, (Option<GameObject>) this.m_audioGo);
      clonedAudio.loop = true;
      WeatherController.AudioSourceController sourceController2 = new WeatherController.AudioSourceController(clonedAudio);
      this.m_audioControllers.Add<WeatherProto, WeatherController.AudioSourceController>(weatherProto, sourceController2);
      return (Option<WeatherController.AudioSourceController>) sourceController2;
    }

    static WeatherController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WeatherController.TRANSITION_DURATION = WeatherManager.HALF_WEATHER_GFX_TRANSITION_DAYS * 2;
      WeatherController.MAFI_WIND_STRENGTH = Shader.PropertyToID("_MafiWindStrength");
      WeatherController.MAFI_WORLD_WETNESS_SMOOTHING = Shader.PropertyToID("_Mafi_WorldWetnessSmoothness");
      WeatherController.MAFI_WORLD_WETNESS_BRIGHTNESS = Shader.PropertyToID("_Mafi_WorldWetnessBrightness");
      WeatherController.THUNDER_ASSETS = ImmutableArray.Create<string>("Assets/Unity/Weather/Thunder01.prefab", "Assets/Unity/Weather/Thunder02.prefab", "Assets/Unity/Weather/Thunder03.prefab");
      WeatherController.THUNDER_RUMBLE_ASSETS = ImmutableArray.Create<string>("Assets/Unity/Weather/ThunderRumble01.prefab", "Assets/Unity/Weather/ThunderRumble02.prefab");
    }

    private class AudioSourceController
    {
      private float m_startVolume;
      private float m_targetVolume;
      private readonly AudioSource m_audioSource;
      private bool m_isPaused;

      public AudioSourceController(AudioSource audioSource)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_audioSource = audioSource;
        this.m_audioSource.volume = 0.0f;
        this.m_audioSource.Stop();
      }

      public void Play(float targetVolume)
      {
        if (!this.m_isPaused && !this.m_audioSource.isPlaying)
        {
          this.m_audioSource.volume = 0.0f;
          this.m_audioSource.Play();
        }
        this.m_startVolume = this.m_audioSource.volume;
        this.m_targetVolume = targetVolume * 0.6f;
      }

      public void Stop()
      {
        this.m_startVolume = this.m_audioSource.volume;
        this.m_targetVolume = 0.0f;
      }

      public void StopImmediate()
      {
        this.m_targetVolume = this.m_startVolume = 0.0f;
        this.m_audioSource.volume = 0.0f;
        this.m_audioSource.Play();
      }

      public void Pause()
      {
        this.m_isPaused = true;
        this.m_audioSource.Stop();
      }

      public void Resume()
      {
        this.m_isPaused = false;
        if ((double) this.m_audioSource.volume <= 0.0)
          return;
        this.m_audioSource.Play();
      }

      public void Update(float perc)
      {
        float num = this.m_startVolume.Lerp(this.m_targetVolume, perc);
        this.m_audioSource.volume = num;
        if (!this.m_audioSource.isPlaying || (double) num > 0.0)
          return;
        this.m_audioSource.Stop();
      }

      public void SkipTransition()
      {
        this.m_startVolume = this.m_targetVolume;
        this.m_audioSource.volume = this.m_startVolume;
        if (this.m_audioSource.isPlaying)
        {
          if ((double) this.m_startVolume > 0.0)
            return;
          this.m_audioSource.Stop();
        }
        else
        {
          if ((double) this.m_startVolume <= 0.0)
            return;
          this.m_audioSource.Play();
        }
      }
    }
  }
}
