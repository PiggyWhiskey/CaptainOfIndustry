// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Dynamic.ShipRollHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Environment;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Dynamic
{
  public class ShipRollHelper
  {
    private WeatherManager m_weatherManager;
    private readonly float m_startingAngle;
    private float m_targetChoppiness;
    private float m_oldChoppiness;
    private float m_chopTransitionDurationRemainingMs;
    private float m_targetRollScale;
    private float m_oldRollScale;
    private float m_rollScaleTransitionDurationRemainingMs;

    public ShipRollHelper(WeatherManager weatherManager, float startingAngle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_weatherManager = weatherManager;
      this.m_startingAngle = startingAngle;
    }

    public void SyncUpdate(GameTime time, float targetRollScale = 1f)
    {
      if ((double) this.m_targetChoppiness != (double) this.m_weatherManager.CurrentWeather.Graphics.OceanChoppiness)
      {
        this.m_oldChoppiness = this.m_targetChoppiness;
        this.m_targetChoppiness = this.m_weatherManager.CurrentWeather.Graphics.OceanChoppiness;
        this.m_chopTransitionDurationRemainingMs = 8000f;
      }
      if ((double) this.m_targetRollScale == (double) targetRollScale)
        return;
      this.m_oldRollScale = this.m_targetRollScale;
      this.m_targetRollScale = targetRollScale;
      this.m_rollScaleTransitionDurationRemainingMs = 4000f;
    }

    public Quaternion RenderUpdateGetRotation(GameTime time)
    {
      float self;
      if ((double) this.m_chopTransitionDurationRemainingMs > 0.0)
      {
        this.m_chopTransitionDurationRemainingMs -= (float) time.GameSpeedMult * time.DeltaTimeMs;
        self = this.m_targetChoppiness.Lerp(this.m_oldChoppiness, (this.m_chopTransitionDurationRemainingMs / 8000f).Max(0.0f));
      }
      else
        self = this.m_targetChoppiness;
      float num1;
      if ((double) this.m_rollScaleTransitionDurationRemainingMs > 0.0)
      {
        this.m_rollScaleTransitionDurationRemainingMs -= (float) time.GameSpeedMult * time.DeltaTimeMs;
        num1 = this.m_targetRollScale.Lerp(this.m_oldRollScale, (this.m_rollScaleTransitionDurationRemainingMs / 4000f).Max(0.0f));
      }
      else
        num1 = this.m_targetRollScale;
      float num2 = self.Max(0.1f);
      double num3 = time.TimeSinceLoadMs.ToDouble() * 2.0 * Math.PI;
      float num4 = num2 * (float) (1.0 + 0.20000000298023224 * (double) Mathf.Sin((float) num3 / 17011f));
      return Quaternion.AngleAxis((float) ((double) Mathf.Sin((float) (num3 / 14011.0) + this.m_startingAngle) * (double) num4 * 2.0), Vector3.forward) * Quaternion.AngleAxis((float) ((double) Mathf.Sin((float) (num3 / 10007.0) + this.m_startingAngle) * (double) num4 * 8.0) * num1, Vector3.right);
    }
  }
}
