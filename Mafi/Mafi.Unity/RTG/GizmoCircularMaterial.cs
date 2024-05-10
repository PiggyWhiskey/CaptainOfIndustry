// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCircularMaterial
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCircularMaterial : Singleton<GizmoCircularMaterial>
  {
    private GizmoCircularMaterial.Type _circularType;
    private Material _circleMaterial;
    private Material _torusMaterial;
    private Material _cylindricalTorusMaterial;

    public Material CircleMaterial
    {
      get
      {
        if ((Object) this._circleMaterial == (Object) null)
          this._circleMaterial = Singleton<MaterialPool>.Get.CircleCull;
        return this._circleMaterial;
      }
    }

    public Material TorusMaterial
    {
      get
      {
        if ((Object) this._torusMaterial == (Object) null)
          this._torusMaterial = Singleton<MaterialPool>.Get.TorusCull;
        return this._torusMaterial;
      }
    }

    public Material CylindricalTorusMaterial
    {
      get
      {
        if ((Object) this._cylindricalTorusMaterial == (Object) null)
          this._cylindricalTorusMaterial = Singleton<MaterialPool>.Get.CylindricalTorusCull;
        return this._cylindricalTorusMaterial;
      }
    }

    public Material Material
    {
      get
      {
        if (this._circularType == GizmoCircularMaterial.Type.Circle)
          return this.CircleMaterial;
        return this._circularType == GizmoCircularMaterial.Type.Torus ? this.TorusMaterial : this.CylindricalTorusMaterial;
      }
    }

    public GizmoCircularMaterial.Type CircularType
    {
      get => this._circularType;
      set => this._circularType = value;
    }

    public bool IsLit => this.Material.GetInt("_IsLit") == 1;

    public float LightIntensity => this.Material.GetFloat("_LightIntensity");

    public GizmoCircularMaterial()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ResetValuesToSensibleDefaults();
    }

    public void ResetValuesToSensibleDefaults()
    {
      this.SetZWriteEnabled(false);
      this.SetZTestAlways();
      this.SetCullModeBack();
      this.SetLit(true);
      this.SetLightIntensity(1.23f);
    }

    public void SetCullAlphaScale(float scale) => this.Material.SetFloat("_CullAlphaScale", scale);

    public void SetShapeCenter(Vector3 center)
    {
      if (this._circularType == GizmoCircularMaterial.Type.Circle)
        this.Material.SetVector("_CircleCenter", (Vector4) center);
      else
        this.Material.SetVector("_TorusCenter", (Vector4) center);
    }

    public void SetTorusCoreRadius(float radius)
    {
      if (this._circularType != GizmoCircularMaterial.Type.Torus && this._circularType != GizmoCircularMaterial.Type.CylindricalTorus)
        return;
      this.Material.SetFloat("_TorusCoreRadius", radius);
    }

    public void SetTorusTubeRadius(float radius)
    {
      if (this._circularType != GizmoCircularMaterial.Type.Torus)
        return;
      this.Material.SetFloat("_TorusTubeRadius", radius);
    }

    public void SetCylindricalTorusRadii(float hrzRadius, float vertRadius)
    {
      if (this._circularType != GizmoCircularMaterial.Type.CylindricalTorus)
        return;
      this.Material.SetFloat("_TorusHrzRadius", hrzRadius);
      this.Material.SetFloat("_TorusVertRadius", vertRadius);
    }

    public void SetCamera(Camera camera)
    {
      this.Material.SetVector("_CamLook", (Vector4) camera.transform.forward);
      this.Material.SetInt("_OrthoCam", camera.orthographic ? 1 : 0);
    }

    public void SetLit(bool isLit) => this.Material.SetInt("_IsLit", isLit ? 1 : 0);

    public void SetLightDirection(Vector3 lightDir)
    {
      this.Material.SetVector("_LightDir", (Vector4) lightDir);
    }

    public void SetLightIntensity(float intensity)
    {
      this.Material.SetFloat("_LightIntensity", intensity);
    }

    public void SetColor(Color color) => this.Material.SetColor("_Color", color);

    public void SetZWriteEnabled(bool isEnabled)
    {
      this.Material.SetInt("_ZWrite", isEnabled ? 1 : 0);
    }

    public void SetZTestEnabled(bool isEnabled)
    {
      this.Material.SetInt("_ZTest", isEnabled ? 4 : 8);
    }

    public void SetZTestAlways() => this.Material.SetInt("_ZTest", 8);

    public void SetZTestLess() => this.Material.SetInt("_ZTest", 2);

    public void SetCullModeBack() => this.Material.SetInt("_CullMode", 2);

    public void SetCullModeFront() => this.Material.SetInt("_CullMode", 1);

    public void SetCullModeOff() => this.Material.SetInt("_CullMode", 0);

    public void SetPass(int passIndex) => this.Material.SetPass(0);

    public enum Type
    {
      Circle,
      Torus,
      CylindricalTorus,
    }
  }
}
