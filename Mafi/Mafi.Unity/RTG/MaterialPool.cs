// Decompiled with JetBrains decompiler
// Type: RTG.MaterialPool
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class MaterialPool : Singleton<MaterialPool>
  {
    private Material _linearGradientCameraBk;
    private Material _xzGrid_Plane;
    private Material _gizmoSolidHandle;
    private Material _tintedTexture;
    private Material _simpleColor;
    private Material _circleCull;
    private Material _torusCull;
    private Material _cylindricalTorusCull;

    public Material LinearGradientCameraBk
    {
      get
      {
        if ((Object) this._linearGradientCameraBk == (Object) null)
          this._linearGradientCameraBk = new Material(Singleton<ShaderPool>.Get.LinearGradientCameraBk);
        return this._linearGradientCameraBk;
      }
    }

    public Material XZGrid_Plane
    {
      get
      {
        if ((Object) this._xzGrid_Plane == (Object) null)
          this._xzGrid_Plane = new Material(Singleton<ShaderPool>.Get.XZGrid_Plane);
        return this._xzGrid_Plane;
      }
    }

    public Material GizmoSolidHandle
    {
      get
      {
        if ((Object) this._gizmoSolidHandle == (Object) null)
          this._gizmoSolidHandle = new Material(Singleton<ShaderPool>.Get.GizmoSolidHandle);
        return this._gizmoSolidHandle;
      }
    }

    public Material TintedTexture
    {
      get
      {
        if ((Object) this._tintedTexture == (Object) null)
          this._tintedTexture = new Material(Singleton<ShaderPool>.Get.TintedTexture);
        return this._tintedTexture;
      }
    }

    public Material SimpleColor
    {
      get
      {
        if ((Object) this._simpleColor == (Object) null)
          this._simpleColor = new Material(Singleton<ShaderPool>.Get.SimpleColor);
        return this._simpleColor;
      }
    }

    public Material CircleCull
    {
      get
      {
        if ((Object) this._circleCull == (Object) null)
          this._circleCull = new Material(Singleton<ShaderPool>.Get.CircleCull);
        return this._circleCull;
      }
    }

    public Material TorusCull
    {
      get
      {
        if ((Object) this._torusCull == (Object) null)
          this._torusCull = new Material(Singleton<ShaderPool>.Get.TorusCull);
        return this._torusCull;
      }
    }

    public Material CylindricalTorusCull
    {
      get
      {
        if ((Object) this._cylindricalTorusCull == (Object) null)
          this._cylindricalTorusCull = new Material(Singleton<ShaderPool>.Get.CylindricalTorusCull);
        return this._cylindricalTorusCull;
      }
    }

    public MaterialPool()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
