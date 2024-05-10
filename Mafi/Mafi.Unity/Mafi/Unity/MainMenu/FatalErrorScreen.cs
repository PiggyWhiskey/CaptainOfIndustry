// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.FatalErrorScreen
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using System;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  public class FatalErrorScreen
  {
    public FatalErrorScreen(string errorMessage, Action exitAction)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      GameObject parent = new GameObject("Error screen");
      parent.AddComponent<FatalErrorScreen.TerminateOnInputMb>().Initialize(exitAction, (Action) (() => GUIUtility.systemCopyBuffer = errorMessage));
      Canvas canvas = parent.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      canvas.sortingOrder = 10000;
      parent.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      GameObject objectToPlace1 = new GameObject("Black background");
      objectToPlace1.AddComponent<Image>().color = Color.black;
      objectToPlace1.PutTo(parent);
      Font builtinResource = UnityEngine.Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
      GameObject objectToPlace2 = new GameObject("Text");
      Text text = objectToPlace2.AddComponent<Text>();
      text.alignment = TextAnchor.MiddleCenter;
      text.verticalOverflow = VerticalWrapMode.Overflow;
      text.text = errorMessage + "\n\nPress [C] to copy the error message\n\nPress [Esc] or [Space] to exit";
      text.font = builtinResource;
      text.fontSize = 12;
      text.color = Color.white;
      text.supportRichText = false;
      objectToPlace2.PutTo(parent, Offset.All(150f));
    }

    private class TerminateOnInputMb : MonoBehaviour
    {
      private Action m_action;
      private Action m_copyErrorAction;

      public void Initialize(Action action, Action copyErrorAction)
      {
        this.m_action = action;
        this.m_copyErrorAction = copyErrorAction;
      }

      private void Update()
      {
        if (Input.GetKeyDown(KeyCode.C))
        {
          this.m_copyErrorAction();
        }
        else
        {
          if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Escape))
            return;
          this.m_action();
        }
      }

      public TerminateOnInputMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
