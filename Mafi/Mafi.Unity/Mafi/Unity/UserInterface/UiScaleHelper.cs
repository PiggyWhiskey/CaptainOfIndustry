// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.UiScaleHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface
{
  public static class UiScaleHelper
  {
    public static ImmutableArray<int> Options;
    public static ImmutableArray<string> StrOptions;

    public static float GetCurrentScaleFloat() => (float) UiScaleHelper.getCurrentScale() / 100f;

    public static Vector2 MousePosition
    {
      get => new Vector2(Input.mousePosition.x, Input.mousePosition.y).ApplyScale();
    }

    public static Vector2 ScreenSize
    {
      get => new Vector2((float) Screen.width, (float) Screen.height).ApplyScale();
    }

    public static Vector2 ApplyScale(this Vector2 pos)
    {
      return pos / UiScaleHelper.GetCurrentScaleFloat();
    }

    private static int getCurrentScale()
    {
      return PlayerPrefs.GetInt("UiScale", UiScaleHelper.resolveDefaultScale());
    }

    public static int GetCurrentScaleIndex()
    {
      int currentScale = UiScaleHelper.getCurrentScale();
      int currentScaleIndex = UiScaleHelper.Options.IndexOf(currentScale);
      if (currentScaleIndex != -1)
        return currentScaleIndex;
      Log.Error(string.Format("Invalid current scale {0}. Using a fallback one.", (object) currentScale));
      return 2;
    }

    private static int resolveDefaultScale()
    {
      Resolution currentResolution = Screen.currentResolution;
      if (currentResolution.width >= 3840 && currentResolution.height >= 2160)
        return 140;
      if (currentResolution.height >= 1400)
        return 120;
      return currentResolution.width < 1920 || currentResolution.height < 1080 ? 90 : 100;
    }

    public static void SetNewScaleIndex(int index)
    {
      if (index < 0 || index >= UiScaleHelper.Options.Length)
      {
        Log.Error(string.Format("Invalid UI scale index {0}", (object) index));
      }
      else
      {
        PlayerPrefs.SetInt("UiScale", UiScaleHelper.Options[index]);
        PlayerPrefs.Save();
      }
    }

    static UiScaleHelper()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      UiScaleHelper.Options = ImmutableArray.Create<int>(80, 90, 100, 110, 120, 130, 140, 150, 160, 180, 200);
      UiScaleHelper.StrOptions = UiScaleHelper.Options.Select<string>((Func<int, string>) (x => string.Format("{0}%", (object) x))).ToImmutableArray<string>();
    }
  }
}
