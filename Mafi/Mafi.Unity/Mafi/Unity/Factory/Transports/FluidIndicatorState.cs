// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Factory.Transports.FluidIndicatorState
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Factory.Transports
{
  public class FluidIndicatorState
  {
    private static readonly int s_shaderColorId;
    private static readonly int s_offsetAndScaleId;
    private readonly MeshRenderer m_renderer;
    private readonly MaterialPropertyBlock m_materialBlock;
    private readonly float m_flowRateMult;
    private float m_targetFlowRate;
    private Color m_targetColor;
    private float m_transitionRemaining;
    private float m_prevFlowRate;
    private Color m_prevColor;
    private bool m_needsUpdate;
    private float m_texOffset;
    private Vector4 m_offsetAndScale;

    public FluidIndicatorState(MeshRenderer renderer, FluidIndicatorGfxParams indicatorParams)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_renderer = renderer;
      this.m_offsetAndScale = new Vector4(0.0f, 0.0f, indicatorParams.DetailsScale, indicatorParams.StillMovementScale);
      this.m_materialBlock = new MaterialPropertyBlock();
      this.m_materialBlock.SetColor(FluidIndicatorState.s_shaderColorId, Color.black);
      this.m_materialBlock.SetVector(FluidIndicatorState.s_offsetAndScaleId, this.m_offsetAndScale);
      this.m_flowRateMult = (float) (1.0 / (double) indicatorParams.SizePerTextureWidthMeters / 1000.0);
      renderer.SetPropertyBlock(this.m_materialBlock);
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

    public void SetColor(Color color)
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

    public void RenderUpdate(GameTime time)
    {
      if (this.m_needsUpdate)
      {
        float rate;
        Color color;
        if ((double) this.m_transitionRemaining > 0.0)
        {
          this.m_transitionRemaining = (this.m_transitionRemaining - time.DeltaT).Max(0.0f);
          float t = this.m_transitionRemaining / 2f;
          rate = this.m_targetFlowRate.Lerp(this.m_prevFlowRate, t);
          color = this.m_targetColor.LerpNoAlpha(this.m_prevColor, t);
        }
        else
        {
          rate = this.m_targetFlowRate.IsNearZero() ? 0.0f : this.m_targetFlowRate;
          color = this.m_targetColor;
          this.m_needsUpdate = false;
        }
        applyFlowRate(rate);
        this.m_materialBlock.SetColor(FluidIndicatorState.s_shaderColorId, color);
        this.m_materialBlock.SetVector(FluidIndicatorState.s_offsetAndScaleId, this.m_offsetAndScale);
        this.m_renderer.SetPropertyBlock(this.m_materialBlock);
      }
      else
      {
        if ((double) this.m_targetFlowRate == 0.0)
          return;
        applyFlowRate(this.m_targetFlowRate);
        this.m_materialBlock.SetVector(FluidIndicatorState.s_offsetAndScaleId, this.m_offsetAndScale);
        this.m_renderer.SetPropertyBlock(this.m_materialBlock);
      }

      void applyFlowRate(float rate)
      {
        this.m_texOffset += rate * time.DeltaTimeMs * (float) time.GameSpeedMult;
        if ((double) this.m_texOffset > 100.0)
          this.m_texOffset -= 64f;
        else if ((double) this.m_texOffset < -100.0)
          this.m_texOffset += 64f;
        this.m_offsetAndScale.x = this.m_texOffset;
      }
    }

    static FluidIndicatorState()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FluidIndicatorState.s_shaderColorId = Shader.PropertyToID("_Color");
      FluidIndicatorState.s_offsetAndScaleId = Shader.PropertyToID("_OffsetAndScale");
    }
  }
}
