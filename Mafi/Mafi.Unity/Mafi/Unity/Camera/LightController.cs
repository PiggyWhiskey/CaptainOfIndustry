// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Camera.LightController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Console;
using Mafi.Unity.Terrain;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Camera
{
  /// <summary>
  /// Controls lighting in the game such as sun and shadows.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class LightController
  {
    private static readonly Vector3 DEFAULT_LIGHT_ANGLES;
    private readonly Light m_light;
    private readonly WaterRendererManager m_waterRenderer;
    private LightController.State m_state;

    public Vector3 LightAngles => this.m_light.transform.eulerAngles;

    public float LightIntensity => this.m_state.LightIntensity;

    public LightController(WaterRendererManager waterRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_waterRenderer = waterRenderer;
      this.m_light = Object.FindObjectOfType<Light>().CheckNotNull<Light>();
      this.m_light.cookie = (Texture) null;
    }

    [ConsoleCommand(true, false, null, null)]
    private void setLightDirection(int angleDegrees)
    {
      Vector3 eulerAngles = this.m_light.transform.eulerAngles;
      this.m_light.transform.eulerAngles = new Vector3(eulerAngles.x, (float) (angleDegrees % 360), eulerAngles.z);
    }

    [ConsoleCommand(true, false, null, null)]
    private void setLightElevation(int angleDegrees)
    {
      Vector3 eulerAngles = this.m_light.transform.eulerAngles;
      this.m_light.transform.eulerAngles = new Vector3((float) angleDegrees.Clamp(20, 90), eulerAngles.y, eulerAngles.z);
    }

    [ConsoleCommand(true, false, null, null)]
    private void resetLightAngles()
    {
      this.m_light.transform.eulerAngles = LightController.DEFAULT_LIGHT_ANGLES;
    }

    public void AddLightCommandBuffer(LightEvent e, CommandBuffer cb)
    {
      this.m_light.AddCommandBuffer(e, cb);
    }

    public void RemoveLightCommandBuffer(LightEvent e, CommandBuffer cb)
    {
      this.m_light.RemoveCommandBuffer(e, cb);
    }

    public void SetLightIntensity(float percent, float shadowsPercent)
    {
      this.m_state.LightIntensity = percent;
      this.m_state.ShadowsStrength = shadowsPercent;
      this.m_light.intensity = this.m_state.LightIntensity + this.m_state.LightExtraIntensity;
      this.m_light.shadowStrength = 0.7f * this.m_state.ShadowsStrength;
      this.m_waterRenderer.SetSpecularIntensity(this.m_state.LightIntensity * this.m_state.LightIntensity);
    }

    public void SetExtraLightIntensity(float intensity)
    {
      this.m_state.LightExtraIntensity = intensity;
      this.m_light.intensity = this.m_state.LightIntensity + this.m_state.LightExtraIntensity;
      this.m_waterRenderer.SetSpecularIntensity(this.m_state.LightIntensity * this.m_state.LightIntensity);
    }

    public void SetLightColor(Color color) => this.m_light.color = this.m_state.LightColor = color;

    public void SetPosition(float x, float z)
    {
      this.m_light.transform.localPosition = new Vector3(x, this.m_light.transform.localPosition.y, z);
    }

    public LightController.State GetState() => this.m_state;

    public void SetState(LightController.State state)
    {
      this.m_state = state;
      this.m_light.intensity = state.LightIntensity + state.LightExtraIntensity;
      this.m_light.shadowStrength = 0.7f * state.ShadowsStrength;
      this.m_light.color = state.LightColor;
      this.m_waterRenderer.SetSpecularIntensity(state.LightIntensity * state.LightIntensity);
    }

    static LightController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LightController.DEFAULT_LIGHT_ANGLES = new Vector3(45f, 35.26439f, 0.0f);
    }

    public struct State
    {
      public float LightIntensity;
      public float LightExtraIntensity;
      public Color LightColor;
      public float ShadowsStrength;
    }
  }
}
