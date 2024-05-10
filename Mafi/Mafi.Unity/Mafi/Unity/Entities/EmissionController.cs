// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EmissionController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  public class EmissionController
  {
    private readonly IEntityWithEmission m_entity;
    private readonly ImmutableArray<EmissionController.MaterialData> m_materials;
    private float m_renderIntensity;
    private float m_syncIntensity;
    private static readonly int EMISSION_COLOR_SHADER_ID;

    public EmissionController(GameObject go, IEntityWithEmission entity)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_renderIntensity = -1f;
      this.m_syncIntensity = -1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entity = entity;
      Lyst<Material> outInstantiatedMaterials = new Lyst<Material>();
      go.InstantiateMaterials((Predicate<MeshRenderer>) (x => x.sharedMaterial.IsKeywordEnabled("_EMISSION")), outInstantiatedMaterials);
      if (outInstantiatedMaterials.IsEmpty)
        Log.Error(string.Format("No emissions found for {0}", (object) entity.Prototype.Id));
      this.m_materials = ((ICollection<EmissionController.MaterialData>) outInstantiatedMaterials.Select<EmissionController.MaterialData>((Func<Material, EmissionController.MaterialData>) (x => new EmissionController.MaterialData()
      {
        Material = x,
        Color = x.GetColor(EmissionController.EMISSION_COLOR_SHADER_ID)
      }))).ToImmutableArray<EmissionController.MaterialData>();
    }

    public void RenderUpdate()
    {
      if (this.m_renderIntensity.IsNear(this.m_syncIntensity))
        return;
      this.m_renderIntensity = this.m_syncIntensity;
      foreach (EmissionController.MaterialData material in this.m_materials)
        material.Material.SetColor(EmissionController.EMISSION_COLOR_SHADER_ID, material.Color * this.m_renderIntensity);
    }

    public void SyncUpdate()
    {
      this.m_syncIntensity = this.m_entity.EmissionIntensity.GetValueOrDefault();
    }

    static EmissionController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      EmissionController.EMISSION_COLOR_SHADER_ID = Shader.PropertyToID("_EmissionColor");
    }

    private struct MaterialData
    {
      public Material Material;
      public Color Color;
    }
  }
}
