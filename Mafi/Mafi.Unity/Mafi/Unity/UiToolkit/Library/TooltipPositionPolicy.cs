// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TooltipPositionPolicy
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.FloatingPanel;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class TooltipPositionPolicy : IFloatingPositionPolicy
  {
    private static readonly Px MAX_WIDTH;

    public void InitConstraints(UiComponent floatingPanel, UiComponent target)
    {
      floatingPanel.MaxWidth<UiComponent>(TooltipPositionPolicy.MAX_WIDTH);
    }

    public Vector2 GetPosition(Rect boundsOfTarget, Vector2 floatingPanelSize, Vector2 screenSize)
    {
      float x1 = boundsOfTarget.x;
      float y1 = boundsOfTarget.y;
      float x2 = x1 + 4f;
      float x3 = screenSize.x;
      float num1 = (float) ((double) x2 + (double) floatingPanelSize.x + 4.0) - x3;
      if ((double) num1 > 0.0)
        x2 -= num1 + 4f;
      if ((double) x2 < 0.0)
        x2 = 4f;
      float y2 = (float) ((double) y1 - (double) floatingPanelSize.y - 8.0);
      float x4 = screenSize.x;
      float num2 = (float) ((double) y2 + (double) floatingPanelSize.y + 4.0) - x4;
      if ((double) num2 > 0.0)
        y2 -= num2;
      if ((double) y2 < 4.0)
        y2 = (float) ((double) y1 + (double) boundsOfTarget.height + 8.0);
      return new Vector2(x2, y2);
    }

    public TooltipPositionPolicy()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TooltipPositionPolicy()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TooltipPositionPolicy.MAX_WIDTH = 300.px();
    }
  }
}
