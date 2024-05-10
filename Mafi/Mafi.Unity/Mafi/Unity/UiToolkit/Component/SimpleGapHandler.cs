// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.SimpleGapHandler
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
  /// Does gap properly but fails for components that wrap content.
  /// </summary>
  public class SimpleGapHandler : IGapHandler
  {
    private readonly UiComponent m_component;
    private Px m_rowGap;
    private Px m_columnGap;
    private bool m_isReversed;

    public SimpleGapHandler(UiComponent component, FlexDirection direction)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_component = component;
      this.m_isReversed = direction == FlexDirection.ColumnReverse || direction == FlexDirection.RowReverse;
    }

    public void OnChildInserted(int index, UiComponent child)
    {
      if (this.m_component.GetChildrenContainer().childCount < 2)
        return;
      if (this.m_isReversed)
      {
        int num = this.m_component.Count - 1;
        this.updateGapFor(index == num ? this.m_component[num - 1] : child, this.m_rowGap, this.m_columnGap);
      }
      else
        this.updateGapFor(index == 0 ? this.m_component[1] : child, this.m_rowGap, this.m_columnGap);
    }

    public void OnChildRemovedAt(int index, UiComponent child)
    {
      if (!this.m_isReversed && index != 0 || this.m_isReversed && index != this.m_component.Count - 1)
        this.updateGapFor(child, -this.m_rowGap, -this.m_columnGap);
      if (this.m_component.Count <= 0)
        return;
      if (!this.m_isReversed && index == 0)
      {
        this.updateGapFor(this.m_component[0], -this.m_rowGap, -this.m_columnGap);
      }
      else
      {
        if (!this.m_isReversed || index != this.m_component.Count - 1)
          return;
        this.updateGapFor(this.m_component[this.m_component.Count - 1], -this.m_rowGap, -this.m_columnGap);
      }
    }

    public void OnDirectionChanged(FlexDirection direction)
    {
      this.updateGaps(-this.m_rowGap, -this.m_columnGap);
      this.m_isReversed = direction == FlexDirection.ColumnReverse || direction == FlexDirection.RowReverse;
      this.updateGaps(this.m_rowGap, this.m_columnGap);
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

    private void updateGaps(Px rowGapDiff, Px columnGapDiff)
    {
      if ((double) (float) rowGapDiff == 0.0 && (double) (float) columnGapDiff == 0.0)
        return;
      if (this.m_isReversed)
      {
        for (int index = 0; index < this.m_component.Count - 1; ++index)
          this.updateGapFor(this.m_component[index], rowGapDiff, columnGapDiff);
      }
      else
      {
        for (int index = 1; index < this.m_component.Count; ++index)
          this.updateGapFor(this.m_component[index], rowGapDiff, columnGapDiff);
      }
    }

    private void updateGapFor(UiComponent child, Px rowGapDiff, Px columnGapDiff)
    {
      if (child.IsPositionAbsolute())
        return;
      ILayoutDecorator layoutDecorator = (ILayoutDecorator) null;
      if ((double) (float) rowGapDiff != 0.0)
      {
        layoutDecorator = child.GetLayoutDecorator();
        layoutDecorator.AddMarginLeft((float) rowGapDiff);
      }
      if ((double) (float) columnGapDiff == 0.0)
        return;
      if (layoutDecorator == null)
        layoutDecorator = child.GetLayoutDecorator();
      layoutDecorator.AddMarginTop((float) columnGapDiff);
    }
  }
}
