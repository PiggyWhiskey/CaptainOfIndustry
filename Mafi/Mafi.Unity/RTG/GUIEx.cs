// Decompiled with JetBrains decompiler
// Type: RTG.GUIEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class GUIEx
  {
    private static Stack<Color> _colorStack;

    public static void PushColor(Color color)
    {
      GUIEx._colorStack.Push(GUI.color);
      GUI.color = color;
    }

    public static void PopColor()
    {
      if (GUIEx._colorStack.Count <= 0)
        return;
      GUI.color = GUIEx._colorStack.Pop();
    }

    static GUIEx()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GUIEx._colorStack = new Stack<Color>();
    }
  }
}
