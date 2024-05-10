// Decompiled with JetBrains decompiler
// Type: RTG.ShaderPool
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class ShaderPool : Singleton<ShaderPool>
  {
    private Shader _linearGradientCameraBk;
    private Shader _xzGrid_Plane;
    private Shader _gizmoSolidHandle;
    private Shader _tintedTexture;
    private Shader _simpleColor;
    private Shader _circleCull;
    private Shader _torusCull;
    private Shader _cylindricalTorusCull;

    public void MafiInitialize(AssetsDb assetsDb)
    {
      this._linearGradientCameraBk = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_LinearGradientCameraBk.shader");
      this._xzGrid_Plane = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_XZGrid_Plane.shader");
      this._gizmoSolidHandle = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_GizmoSolidHandle.shader");
      this._tintedTexture = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_TintedTexture.shader");
      this._simpleColor = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_SimpleColor.shader");
      this._circleCull = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_CircleCull.shader");
      this._torusCull = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_TorusCull.shader");
      this._cylindricalTorusCull = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Unity/MapEditor/RuntimeGizmos/RTApp_CylindricalTorusCull.shader");
    }

    public Shader LinearGradientCameraBk
    {
      get
      {
        if ((Object) this._linearGradientCameraBk == (Object) null)
          this._linearGradientCameraBk = Shader.Find("RTUnityApp/LinearGradientCameraBk");
        return this._linearGradientCameraBk;
      }
    }

    public Shader XZGrid_Plane
    {
      get
      {
        if ((Object) this._xzGrid_Plane == (Object) null)
          this._xzGrid_Plane = Shader.Find("RTUnityApp/XZGrid_Plane");
        return this._xzGrid_Plane;
      }
    }

    public Shader GizmoSolidHandle
    {
      get
      {
        if ((Object) this._gizmoSolidHandle == (Object) null)
          this._gizmoSolidHandle = Shader.Find("RTUnityApp/GizmoSolidHandle");
        return this._gizmoSolidHandle;
      }
    }

    public Shader TintedTexture
    {
      get
      {
        if ((Object) this._tintedTexture == (Object) null)
          this._tintedTexture = Shader.Find("RTUnityApp/TintedTexture");
        return this._tintedTexture;
      }
    }

    public Shader SimpleColor
    {
      get
      {
        if ((Object) this._simpleColor == (Object) null)
          this._simpleColor = Shader.Find("RTUnityApp/SimpleColor");
        return this._simpleColor;
      }
    }

    public Shader CircleCull
    {
      get
      {
        if ((Object) this._circleCull == (Object) null)
          this._circleCull = Shader.Find("RTUnityApp/CircleCull");
        return this._circleCull;
      }
    }

    public Shader TorusCull
    {
      get
      {
        if ((Object) this._torusCull == (Object) null)
          this._torusCull = Shader.Find("RTUnityApp/TorusCull");
        return this._torusCull;
      }
    }

    public Shader CylindricalTorusCull
    {
      get
      {
        if ((Object) this._cylindricalTorusCull == (Object) null)
          this._cylindricalTorusCull = Shader.Find("RTUnityApp/CylindricalTorusCull");
        return this._cylindricalTorusCull;
      }
    }

    public ShaderPool()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
