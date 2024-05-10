// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.KeyCodeExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace Mafi.Unity
{
  internal static class KeyCodeExtensions
  {
    public const KeyCode Scroll = (KeyCode) 10077;

    public static string ToNiceString(this KeyCode code)
    {
      switch (code)
      {
        case KeyCode.Delete:
          return "DEL";
        case KeyCode.KeypadMinus:
          return "Minus (keypad)";
        case KeyCode.KeypadPlus:
          return "Plus (keypad)";
        case KeyCode.UpArrow:
          return "Up arrow";
        case KeyCode.DownArrow:
          return "Down arrow";
        case KeyCode.RightArrow:
          return "Right arrow";
        case KeyCode.LeftArrow:
          return "Left arrow";
        case KeyCode.RightShift:
          return "RSHIFT";
        case KeyCode.LeftShift:
          return "SHIFT";
        case KeyCode.RightControl:
          return "RCTRL";
        case KeyCode.LeftControl:
          return "CTRL";
        case KeyCode.Mouse0:
          return "Left click";
        case KeyCode.Mouse1:
          return "Right click";
        case KeyCode.Mouse2:
          return "Middle click";
        default:
          return code.ToString();
      }
    }

    public static bool IsModifier(this KeyCode code)
    {
      switch (code)
      {
        case KeyCode.RightShift:
        case KeyCode.LeftShift:
        case KeyCode.RightControl:
        case KeyCode.LeftControl:
        case KeyCode.RightAlt:
        case KeyCode.LeftAlt:
        case KeyCode.RightMeta:
        case KeyCode.LeftMeta:
        case KeyCode.LeftWindows:
        case KeyCode.RightWindows:
          return true;
        default:
          return false;
      }
    }
  }
}
