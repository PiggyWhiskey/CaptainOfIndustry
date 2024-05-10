// Decompiled with JetBrains decompiler
// Type: RTG.GizmoSolidMaterial
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoSolidMaterial : Singleton<GizmoSolidMaterial>
  {
    private Material _material;

    public Material Material
    {
      get
      {
        if ((Object) this._material == (Object) null)
          this._material = Singleton<MaterialPool>.Get.GizmoSolidHandle;
        return this._material;
      }
    }

    public bool IsLit => this.Material.GetInt("_IsLit") == 1;

    public float LightIntensity => this.Material.GetFloat("_LightIntensity");

    public GizmoSolidMaterial()
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
  }
}
