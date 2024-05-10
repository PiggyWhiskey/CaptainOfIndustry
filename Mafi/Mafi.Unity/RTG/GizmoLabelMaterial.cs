// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLabelMaterial
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoLabelMaterial : Singleton<GizmoLabelMaterial>
  {
    private Material _material;

    public Material Material
    {
      get
      {
        if ((Object) this._material == (Object) null)
          this._material = Singleton<MaterialPool>.Get.TintedTexture;
        return this._material;
      }
    }

    public void ResetValuesToSensibleDefaults() => this.SetZWriteEnabled(true);

    public void SetTexture(Texture2D texture)
    {
      this.Material.SetTexture("_MainTex", (Texture) texture);
    }

    public void SetColor(Color color) => this.Material.SetColor("_Color", color);

    public void SetPass(int passIndex) => this.Material.SetPass(0);

    public void SetZWriteEnabled(bool isEnabled)
    {
      this.Material.SetInt("_ZWrite", isEnabled ? 1 : 0);
    }

    public void SetZTestLessEqual() => this.Material.SetInt("_ZTest", 4);

    public void SetZTestAlways() => this.Material.SetInt("_ZTest", 8);

    public void SetZTestLess() => this.Material.SetInt("_ZTest", 2);

    public GizmoLabelMaterial()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
