// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.InstancedFluidIndicatorState
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  public class InstancedFluidIndicatorState
  {
    private readonly float m_flowRateMult;
    private readonly float m_flowRateInvMult;
    private float m_targetFlowRate;
    private ColorRgba m_targetColor;
    private float m_transitionRemaining;
    private float m_prevFlowRate;
    private ColorRgba m_prevColor;
    private bool m_needsUpdate;
    private float m_texOffset;
    private ColorRgba m_currentColor;

    public float TexOffset => this.m_texOffset;

    public ColorRgba CurrentColor => this.m_currentColor;

    public ColorRgba TargetColor => this.m_targetColor;

    public float TargetFlowRate => this.m_targetFlowRate;

    public float MetersPerTick => this.m_targetFlowRate * this.m_flowRateInvMult;

    public bool IsTransitioning => (double) this.m_transitionRemaining > 0.0;

    public InstancedFluidIndicatorState(FluidIndicatorGfxParams indicatorParams)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_flowRateMult = (float) (1.0 / (double) indicatorParams.SizePerTextureWidthMeters / 1000.0);
      this.m_flowRateInvMult = 1f / this.m_flowRateMult;
    }

    public void SetFlow(float metersPerTick)
    {
      float num = metersPerTick * this.m_flowRateMult;
      if ((double) num == (double) this.m_targetFlowRate)
        return;
      if (!num.IsNear(this.m_targetFlowRate, 0.05f))
      {
        this.m_transitionRemaining = 2f;
        this.m_prevFlowRate = this.m_targetFlowRate;
      }
      this.m_targetFlowRate = num;
      this.m_needsUpdate = true;
    }

    public void SetColor(ColorRgba color)
    {
      if (color == this.m_targetColor)
        return;
      this.m_prevColor = this.m_targetColor;
      this.m_targetColor = color;
      this.m_transitionRemaining = 2f;
      this.m_needsUpdate = true;
    }

    public void SkipTransition()
    {
      this.m_prevColor = this.m_targetColor;
      this.m_prevFlowRate = this.m_targetFlowRate;
      this.m_transitionRemaining = 0.0f;
      this.m_needsUpdate = true;
    }

    public void Update(GameTime time)
    {
      float rate;
      if (this.m_needsUpdate)
      {
        ColorRgba colorRgba;
        if ((double) this.m_transitionRemaining > 0.0)
        {
          this.m_transitionRemaining = (this.m_transitionRemaining - 1f).Max(0.0f);
          Percent t = Percent.FromFloat(this.m_transitionRemaining / 2f);
          rate = this.m_targetFlowRate.Lerp(this.m_prevFlowRate, t);
          colorRgba = this.m_targetColor.Lerp(this.m_prevColor, t);
        }
        else
        {
          rate = this.m_targetFlowRate.IsNearZero() ? 0.0f : this.m_targetFlowRate;
          colorRgba = this.m_targetColor;
          this.m_needsUpdate = false;
        }
        this.m_currentColor = colorRgba;
      }
      else
      {
        if ((double) this.m_targetFlowRate == 0.0)
          return;
        rate = this.m_targetFlowRate;
      }
      if (time.IsGamePaused)
        return;
      this.m_texOffset = applyFlowRate(rate, this.m_texOffset);

      float applyFlowRate(float rate, float currentOffset)
      {
        float num = currentOffset + rate * 100f * (float) time.GameSpeedMult;
        if ((double) num > 100.0)
          num -= 64f;
        else if ((double) num < -100.0)
          num += 64f;
        return num;
      }
    }
  }
}
