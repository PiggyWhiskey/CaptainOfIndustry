// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.BeaconMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Beacons;
using Mafi.Core.Entities.Static;
using Mafi.Core.Simulation;
using Mafi.Unity.Entities;
using Mafi.Unity.Entities.Static;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  public class BeaconMb : 
    StaticEntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private static readonly int EMISSION_COLOR_SHADER_ID;
    private Beacon m_beacon;
    private ISimLoopEvents m_simLoopEvents;
    private Transform m_lampTransform;
    private bool m_wasWorking;
    private bool m_isWorking;
    private float m_angleCumulative;
    private Material m_material;
    private Color m_emissionColor;

    public void Initialize(Beacon beacon, ISimLoopEvents simLoopEvents)
    {
      this.Initialize((ILayoutEntity) beacon);
      this.m_beacon = beacon;
      this.m_simLoopEvents = simLoopEvents;
      this.m_lampTransform = this.gameObject.transform.Find("Beacon_lamp");
      if (!(bool) (Object) this.m_lampTransform)
      {
        Log.Warning("Lamp objects was not found.");
      }
      else
      {
        foreach (Object componentsInChild in this.m_lampTransform.GetComponentsInChildren<Light>())
          Object.Destroy(componentsInChild);
        this.m_material = Object.Instantiate<Material>(this.gameObject.GetComponent<MeshRenderer>().sharedMaterial);
        this.gameObject.SetSharedMaterialRecursively(this.m_material);
        this.m_emissionColor = this.m_material.GetColor(BeaconMb.EMISSION_COLOR_SHADER_ID);
        this.m_material.SetColor(BeaconMb.EMISSION_COLOR_SHADER_ID, Color.black);
      }
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      this.m_isWorking = this.m_beacon.LastWorkedOnSimStep == this.m_simLoopEvents.CurrentStep;
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      if (this.m_isWorking != this.m_wasWorking)
      {
        this.m_wasWorking = this.m_isWorking;
        this.m_material.SetColor(BeaconMb.EMISSION_COLOR_SHADER_ID, this.m_isWorking ? this.m_emissionColor : Color.black);
      }
      if (!this.m_isWorking)
        return;
      this.m_angleCumulative += time.DeltaTimeMs * 0.1f;
      this.m_lampTransform.localRotation = Quaternion.AngleAxis(this.m_angleCumulative, Vector3.up);
    }

    public BeaconMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static BeaconMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BeaconMb.EMISSION_COLOR_SHADER_ID = Shader.PropertyToID("_EmissionColor");
    }
  }
}
