// Decompiled with JetBrains decompiler
// Type: RTG.RTInput
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class RTInput
  {
    public static Vector3 MousePosition => Input.mousePosition;

    public static bool IsMousePresent => Input.mousePresent;

    public static int TouchCount => Input.touchCount;

    public static bool WasLeftMouseButtonPressedThisFrame() => Input.GetMouseButtonDown(0);

    public static bool WasRightMouseButtonPressedThisFrame() => Input.GetMouseButtonDown(1);

    public static bool WasMiddleMouseButtonPressedThisFrame() => Input.GetMouseButtonDown(2);

    public static bool WasMouseButtonPressedThisFrame(int mouseButton)
    {
      return Input.GetMouseButtonDown(mouseButton);
    }

    public static bool WasMouseButtonReleasedThisFrame(int mouseButton)
    {
      return Input.GetMouseButtonUp(mouseButton);
    }

    public static bool IsLeftMouseButtonPressed() => Input.GetMouseButton(0);

    public static bool IsRightMouseButtonPressed() => Input.GetMouseButton(1);

    public static bool IsMiddleMouseButtonPressed() => Input.GetMouseButton(2);

    public static bool IsMouseButtonPressed(int mouseButton) => Input.GetMouseButton(mouseButton);

    public static bool WasMouseMoved()
    {
      return (double) Input.GetAxis("MouseX") != 0.0 || (double) Input.GetAxis("MouseY") != 0.0;
    }

    public static float MouseAxisX() => Input.GetAxis("MouseX");

    public static float MouseAxisY() => Input.GetAxis("MouseY");

    public static float MouseScroll() => Input.GetAxis(nameof (MouseScroll));

    public static bool WasKeyPressedThisFrame(KeyCode keyCode) => Input.GetKeyDown(keyCode);

    public static bool IsKeyPressed(KeyCode keyCode) => Input.GetKey(keyCode);

    public static Vector2 TouchDelta(int touchIndex) => Input.GetTouch(touchIndex).deltaPosition;

    public static Vector2 TouchPosition(int touchIndex) => Input.GetTouch(touchIndex).position;

    public static bool TouchBegan(int touchIndex)
    {
      return Input.GetTouch(touchIndex).phase == TouchPhase.Began;
    }

    public static bool TouchEndedOrCanceled(int touchIndex)
    {
      Touch touch = Input.GetTouch(touchIndex);
      return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
    }

    public static bool TouchMoved(int touchIndex)
    {
      return Input.GetTouch(touchIndex).phase == TouchPhase.Moved;
    }
  }
}
