// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.TitleTooltipPositionPolicy
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
  public class TitleTooltipPositionPolicy : IFloatingPositionPolicy
  {
    private float m_maxOverflow;

    public void InitConstraints(UiComponent floatingPanel, UiComponent target)
    {
      floatingPanel.MaxWidth<UiComponent>((target.ResolvedWidth + this.m_maxOverflow).px());
    }

    public Vector2 GetPosition(Rect boundsOfTarget, Vector2 floatingPanelSize, Vector2 screenSize)
    {
      float x1 = boundsOfTarget.x;
      float y1 = boundsOfTarget.y;
      float x2 = x1 + (float) (((double) boundsOfTarget.width - (double) floatingPanelSize.x) / 2.0);
      if ((double) x2 < 0.0)
        x2 = 0.0f;
      float num = x2 + floatingPanelSize.x - screenSize.x;
      if ((double) num > 0.0)
        x2 -= num;
      float y2 = (float) ((double) y1 - (double) floatingPanelSize.y - 4.0);
      return new Vector2(x2, y2);
    }

    public TitleTooltipPositionPolicy()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_maxOverflow = 20f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
