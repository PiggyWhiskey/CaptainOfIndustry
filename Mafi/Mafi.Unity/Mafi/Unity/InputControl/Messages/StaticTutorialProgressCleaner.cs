// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.StaticTutorialProgressCleaner
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages
{
  /// <summary>
  /// Only use from main menu when resolver is no available.
  /// Game menu should use non-static version.
  /// </summary>
  public class StaticTutorialProgressCleaner : ITutorialProgressCleaner
  {
    /// <summary>WARNING: Can be only used from the main thread.</summary>
    public void ResetTutorialProgress()
    {
      if (!PlayerPrefs.HasKey("MessagesProgressKey"))
        return;
      PlayerPrefs.DeleteKey("MessagesProgressKey");
    }

    public StaticTutorialProgressCleaner()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
