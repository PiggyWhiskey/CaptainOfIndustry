// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.OptionalParticlesWrapper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// Convenient wrapper that hides the fact that particles might be missing. Definitely better
  /// than crashing the game or break rendering.
  /// </summary>
  public readonly struct OptionalParticlesWrapper
  {
    private readonly Option<ParticleSystem> m_particlesMaybe;

    public OptionalParticlesWrapper(GameObject parent, string particlesName, bool okIfMissing = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Transform transform = parent.transform.Find(particlesName);
      if ((Object) transform != (Object) null)
      {
        this.m_particlesMaybe = (Option<ParticleSystem>) transform.GetComponent<ParticleSystem>();
      }
      else
      {
        if (!okIfMissing)
          Log.Error("Particles " + particlesName + " not found!");
        this.m_particlesMaybe = Option<ParticleSystem>.None;
      }
    }

    public void Play(GameTime time)
    {
      if (this.m_particlesMaybe.IsNone)
        return;
      if (!this.m_particlesMaybe.Value.isEmitting)
        this.m_particlesMaybe.Value.Play(true);
      if ((double) this.m_particlesMaybe.Value.main.simulationSpeed == (double) time.GameSpeedMult)
        return;
      OptionalParticlesWrapper.setSpeedRecursively(this.m_particlesMaybe.Value, (float) time.GameSpeedMult);
    }

    public void Stop() => this.m_particlesMaybe.ValueOrNull?.Stop(true);

    public void UpdateSpeedIfPlaying(GameTime time)
    {
      if (!this.m_particlesMaybe.HasValue || !this.m_particlesMaybe.Value.isEmitting || (double) this.m_particlesMaybe.Value.main.simulationSpeed == (double) time.GameSpeedMult)
        return;
      OptionalParticlesWrapper.setSpeedRecursively(this.m_particlesMaybe.Value, (float) time.GameSpeedMult);
    }

    private static void setSpeedRecursively(ParticleSystem ps, float speed)
    {
      ps.main.simulationSpeed = speed;
      ParticleSystem.InheritVelocityModule inheritVelocity = ps.inheritVelocity;
      if (inheritVelocity.enabled)
        inheritVelocity.curveMultiplier = (double) speed == 0.0 ? 1f : 1f / speed;
      Transform transform = ps.transform;
      for (int index = 0; index < transform.childCount; ++index)
      {
        ParticleSystem component;
        if (transform.GetChild(index).TryGetComponent<ParticleSystem>(out component))
          OptionalParticlesWrapper.setSpeedRecursively(component, speed);
      }
    }
  }
}
