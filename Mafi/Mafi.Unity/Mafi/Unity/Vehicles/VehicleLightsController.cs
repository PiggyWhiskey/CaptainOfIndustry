// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.VehicleLightsController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  public struct VehicleLightsController
  {
    private static readonly Percent LIGHTS_ON_THRESHOLD;
    private static readonly Percent LIGHTS_CHANGE_PROBABILITY;
    private readonly Material[] m_emissionMaterials;
    private readonly float[] m_originalEmissions;
    private readonly int m_emissionStrengthPropertyId;
    private readonly DynamicGroundEntity m_entity;
    private readonly IWeatherManager m_weatherManager;
    private readonly IRandom m_randomForLights;

    public bool LightsOrEmissionsOn
    {
      get => this.LightsEnabled || (double) this.CurrentEmissionStrength > 0.0;
    }

    public bool LightsEnabled { get; private set; }

    public float CurrentEmissionStrength { get; private set; }

    /// <summary>
    /// Note: This will instantiate materials that have the `emissionStrengthPropertyId`.
    /// </summary>
    public VehicleLightsController(
      IWeatherManager weatherManager,
      IRandom random,
      DynamicGroundEntity entity,
      GameObject go,
      int? emissionStrengthPropertyId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_weatherManager = weatherManager;
      this.m_entity = entity;
      this.m_randomForLights = random;
      this.LightsEnabled = false;
      this.CurrentEmissionStrength = 0.0f;
      foreach (UnityEngine.Object componentsInChild in go.GetComponentsInChildren<Light>(true))
        UnityEngine.Object.Destroy(componentsInChild);
      if (emissionStrengthPropertyId.HasValue)
      {
        this.m_emissionStrengthPropertyId = emissionStrengthPropertyId.Value;
        Lyst<Material> outInstantiatedMaterials = new Lyst<Material>();
        go.InstantiateMaterials((Predicate<MeshRenderer>) (x => x.sharedMaterial.HasProperty(emissionStrengthPropertyId.Value)), outInstantiatedMaterials);
        if (outInstantiatedMaterials.Count == 0)
        {
          Log.Error("No mesh renderers found on game object '" + go.name + "'.");
          this.m_emissionStrengthPropertyId = -1;
          this.m_emissionMaterials = Array.Empty<Material>();
          this.m_originalEmissions = Array.Empty<float>();
        }
        else
        {
          this.m_emissionMaterials = outInstantiatedMaterials.ToArray();
          this.m_originalEmissions = this.m_emissionMaterials.MapArray<Material, float>((Func<Material, float>) (x => x.GetFloat(emissionStrengthPropertyId.Value)));
        }
      }
      else
      {
        this.m_emissionStrengthPropertyId = -1;
        this.m_emissionMaterials = Array.Empty<Material>();
        this.m_originalEmissions = Array.Empty<float>();
      }
      foreach (Material emissionMaterial in this.m_emissionMaterials)
        emissionMaterial.SetFloat(this.m_emissionStrengthPropertyId, 0.0f);
      bool isEnabled = this.m_entity.IsEnabled;
      bool enabled = this.shouldHaveLights() & isEnabled;
      this.SetLightsAndEmissionEnabled(enabled, new float?(enabled ? 2f : (isEnabled ? 1f : 0.0f)));
    }

    public void SetLightsAndEmissionEnabled(bool enabled, float? emissionStrength = null)
    {
      this.SetLightsEnabled(enabled);
      this.SetEmissionStrength((float) ((double) emissionStrength ?? (enabled ? 1.0 : 0.0)));
    }

    public void SetLightsEnabled(bool enabled) => this.LightsEnabled = enabled;

    public void SetEmissionStrength(float strength)
    {
      if ((double) this.CurrentEmissionStrength == (double) strength)
        return;
      this.CurrentEmissionStrength = strength;
      for (int index = 0; index < this.m_emissionMaterials.Length; ++index)
        this.m_emissionMaterials[index].SetFloat(this.m_emissionStrengthPropertyId, this.m_originalEmissions[index] * strength);
    }

    public void UpdateVehicleLights()
    {
      bool isEnabled = this.m_entity.IsEnabled;
      bool enabled = this.shouldHaveLights() & isEnabled;
      if (isEnabled != this.LightsOrEmissionsOn)
      {
        this.SetLightsAndEmissionEnabled(false, new float?(isEnabled ? 1f : 0.0f));
      }
      else
      {
        if (this.LightsEnabled == enabled || !this.m_randomForLights.TestProbability(VehicleLightsController.LIGHTS_CHANGE_PROBABILITY))
          return;
        this.SetLightsAndEmissionEnabled(enabled, new float?(enabled ? 2f : 1f));
      }
    }

    private bool shouldHaveLights()
    {
      return this.m_weatherManager.SimSunIntensity <= VehicleLightsController.LIGHTS_ON_THRESHOLD;
    }

    static VehicleLightsController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      VehicleLightsController.LIGHTS_ON_THRESHOLD = 50.Percent();
      VehicleLightsController.LIGHTS_CHANGE_PROBABILITY = 3.Percent();
    }
  }
}
