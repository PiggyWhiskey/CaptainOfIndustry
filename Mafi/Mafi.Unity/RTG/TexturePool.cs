// Decompiled with JetBrains decompiler
// Type: RTG.TexturePool
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
  public class TexturePool : Singleton<TexturePool>
  {
    private Texture2D _xAxisLabel;
    private Texture2D _yAxisLabel;
    private Texture2D _zAxisLabel;
    private Texture2D _camPerspMode;
    private Texture2D _camOrthoMode;

    public void MafiInitialize(AssetsDb assetsDb)
    {
      this._xAxisLabel = assetsDb.GetSharedAssetOrThrow<Texture2D>("Assets/Unity/MapEditor/RuntimeGizmos/XAxisLabel.png");
      this._yAxisLabel = assetsDb.GetSharedAssetOrThrow<Texture2D>("Assets/Unity/MapEditor/RuntimeGizmos/YAxisLabel.png");
      this._zAxisLabel = assetsDb.GetSharedAssetOrThrow<Texture2D>("Assets/Unity/MapEditor/RuntimeGizmos/ZAxisLabel.png");
      this._camPerspMode = assetsDb.GetSharedAssetOrThrow<Texture2D>("Assets/Unity/MapEditor/RuntimeGizmos/CamPerspMode.png");
      this._camOrthoMode = assetsDb.GetSharedAssetOrThrow<Texture2D>("Assets/Unity/MapEditor/RuntimeGizmos/CamOrthoMode.png");
    }

    public Texture2D XAxisLabel
    {
      get
      {
        if ((Object) this._xAxisLabel == (Object) null)
          this._xAxisLabel = UnityEngine.Resources.Load("Textures/XAxisLabel") as Texture2D;
        return this._xAxisLabel;
      }
    }

    public Texture2D YAxisLabel
    {
      get
      {
        if ((Object) this._yAxisLabel == (Object) null)
          this._yAxisLabel = UnityEngine.Resources.Load("Textures/YAxisLabel") as Texture2D;
        return this._yAxisLabel;
      }
    }

    public Texture2D ZAxisLabel
    {
      get
      {
        if ((Object) this._zAxisLabel == (Object) null)
          this._zAxisLabel = UnityEngine.Resources.Load("Textures/ZAxisLabel") as Texture2D;
        return this._zAxisLabel;
      }
    }

    public Texture2D CamPerspMode
    {
      get
      {
        if ((Object) this._camPerspMode == (Object) null)
          this._camPerspMode = UnityEngine.Resources.Load("Textures/CamPerspMode") as Texture2D;
        return this._camPerspMode;
      }
    }

    public Texture2D CamOrthoMode
    {
      get
      {
        if ((Object) this._camOrthoMode == (Object) null)
          this._camOrthoMode = UnityEngine.Resources.Load("Textures/CamOrthoMode") as Texture2D;
        return this._camOrthoMode;
      }
    }

    public TexturePool()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
