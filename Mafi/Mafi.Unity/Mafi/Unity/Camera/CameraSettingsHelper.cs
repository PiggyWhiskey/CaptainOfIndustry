// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Camera.CameraSettingsHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Camera
{
  public static class CameraSettingsHelper
  {
    public const string PREF_KEY_CAMERA_FOV = "Camera FOV";
    public const float DEFAULT_FOV = 60f;
    public static readonly ImmutableArray<float> FOV_VALUES;

    public static event Action<float> OnFovChanged;

    public static float GetCurrentFov() => PlayerPrefs.GetFloat("Camera FOV", 60f);

    public static int GetCurrentFovIndex()
    {
      float currentFov = CameraSettingsHelper.GetCurrentFov();
      int num = CameraSettingsHelper.FOV_VALUES.IndexOf(currentFov);
      return num < 0 ? CameraSettingsHelper.FOV_VALUES.IndexOf(60f) : num;
    }

    public static void SetAndSaveFov(float fov)
    {
      PlayerPrefs.SetFloat("Camera FOV", fov);
      PlayerPrefs.Save();
      Action<float> onFovChanged = CameraSettingsHelper.OnFovChanged;
      if (onFovChanged == null)
        return;
      onFovChanged(fov);
    }

    static CameraSettingsHelper()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CameraSettingsHelper.FOV_VALUES = ImmutableArray.Create<float>(50f, 55f, 60f, 65f, 70f);
    }
  }
}
