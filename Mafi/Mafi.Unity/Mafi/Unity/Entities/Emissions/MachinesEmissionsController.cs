// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Emissions.MachinesEmissionsController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Emissions
{
  public class MachinesEmissionsController
  {
    private readonly Machine m_machine;
    private readonly EmissionParams m_emissionParams;
    private readonly ImmutableArray<Material> m_materials;
    private float m_renderIntensity;
    private float m_syncIntensity;
    private static readonly int EMISSION_COLOR_SHADER_ID;
    private readonly int m_delayInTicks;
    private readonly int m_endTimeInTicks;

    public EmissionParams EmissionParams => this.m_emissionParams;

    public float RenderIntensity => this.m_renderIntensity;

    public MachinesEmissionsController(
      Machine machine,
      ImmutableArray<Material> materialsToEmit,
      EmissionParams emissionParams)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_renderIntensity = -1f;
      this.m_syncIntensity = -1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_machine = machine;
      this.m_emissionParams = emissionParams;
      this.m_materials = materialsToEmit;
      this.m_delayInTicks = emissionParams.Delay.Ticks;
      int ticks = emissionParams.Duration.Ticks;
      this.m_endTimeInTicks = ticks > 0 ? this.m_delayInTicks + ticks : int.MaxValue;
    }

    public void RenderUpdate(GameTime time)
    {
      this.m_renderIntensity = this.m_renderIntensity.Lerp(this.m_syncIntensity, time.RelativeT);
      if (this.m_renderIntensity.IsNear(this.m_syncIntensity))
        return;
      foreach (Material material in this.m_materials)
        material.SetColor(MachinesEmissionsController.EMISSION_COLOR_SHADER_ID, this.m_emissionParams.Color.ToColor() * this.m_renderIntensity);
    }

    public void SyncUpdate(GameTime time)
    {
      int ticks = this.m_machine.RecipeProductionTicks.Ticks;
      bool flag = this.m_machine.WorkedThisTick && ticks >= this.m_delayInTicks && ticks < this.m_endTimeInTicks;
      if (time.IsGamePaused)
        return;
      if (flag)
        this.m_syncIntensity = (this.m_syncIntensity + this.m_emissionParams.DiffToOn * (float) time.GameSpeedMult).Min(this.m_emissionParams.Intensity);
      else
        this.m_syncIntensity = (this.m_syncIntensity - this.m_emissionParams.DiffToOff * (float) time.GameSpeedMult).Max(0.0f);
    }

    static MachinesEmissionsController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MachinesEmissionsController.EMISSION_COLOR_SHADER_ID = Shader.PropertyToID("_EmissionColor");
    }
  }
}
