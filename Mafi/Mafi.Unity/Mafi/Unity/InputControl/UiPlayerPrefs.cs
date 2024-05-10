// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.UiPlayerPrefs
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public class UiPlayerPrefs
  {
    public bool IsRecipeNormalizationOn;
    public bool AreNotificationsMuted;

    public UiPlayerPrefs()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.IsRecipeNormalizationOn = PlayerPrefs.GetInt("UI_IsRecipeNormalizationOn") == 1;
      this.AreNotificationsMuted = PlayerPrefs.GetInt("UI_AreNotificationsMuted") == 1;
    }

    public void SetRecipeNormalizationOn(bool isOn)
    {
      this.IsRecipeNormalizationOn = isOn;
      PlayerPrefs.SetInt("UI_IsRecipeNormalizationOn", this.IsRecipeNormalizationOn ? 1 : 0);
      PlayerPrefs.Save();
    }

    public void SetNotificationsMuted(bool isMuted)
    {
      this.AreNotificationsMuted = isMuted;
      PlayerPrefs.SetInt("UI_AreNotificationsMuted", this.AreNotificationsMuted ? 1 : 0);
      PlayerPrefs.Save();
    }
  }
}
