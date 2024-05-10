// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.GridGapHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  /// <summary>
  /// Handles gap for containers that wrap and use gap in both axes.
  /// But as a side effect, it also introduces padding of 1/2 gap inside the container.
  /// </summary>
  public class GridGapHandler : IGapHandler
  {
    private readonly UiComponent m_component;
    private Px m_rowGap;
    private Px m_columnGap;

    public GridGapHandler(UiComponent component)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_component = component;
    }

    public void OnChildInserted(int index, UiComponent child)
    {
      this.updateGapFor(child, this.m_rowGap, this.m_columnGap);
    }

    public void OnChildRemovedAt(int index, UiComponent child)
    {
      this.updateGapFor(child, -this.m_rowGap, -this.m_columnGap);
    }

    public void OnBeforeClear() => this.updateGaps(-this.m_rowGap, -this.m_columnGap);

    public void SetRowGap(Px gap)
    {
      Px rowGapDiff = gap - this.m_rowGap;
      this.m_rowGap = gap;
      this.updateGaps(rowGapDiff, 0.pt());
    }

    public void SetColumnGap(Px gap)
    {
      Px columnGapDiff = gap - this.m_columnGap;
      this.m_columnGap = gap;
      this.updateGaps(0.pt(), columnGapDiff);
    }

    public void OnDirectionChanged(FlexDirection direction)
    {
    }

    private void updateGaps(Px rowGapDiff, Px columnGapDiff)
    {
      if ((double) (float) rowGapDiff == 0.0 && (double) (float) columnGapDiff == 0.0)
        return;
      foreach (UiComponent child in this.m_component)
        this.updateGapFor(child, rowGapDiff, columnGapDiff);
    }

    private void updateGapFor(UiComponent child, Px rowGapDiff, Px columnGapDiff)
    {
      if (child.IsPositionAbsolute())
        return;
      ILayoutDecorator layoutDecorator = (ILayoutDecorator) null;
      if ((double) (float) rowGapDiff != 0.0)
      {
        layoutDecorator = child.GetLayoutDecorator();
        float num = (float) (rowGapDiff / 2f);
        layoutDecorator.AddMarginLeftRight(num);
      }
      if ((double) (float) columnGapDiff == 0.0)
        return;
      if (layoutDecorator == null)
        layoutDecorator = child.GetLayoutDecorator();
      float num1 = (float) (columnGapDiff / 2f);
      layoutDecorator.AddMarginTopBottom(num1);
    }
  }
}
