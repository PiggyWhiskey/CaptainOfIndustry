// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.StatusBar
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar
{
  /// <summary>
  /// UI stripe at top of the game window. Mostly displays summary of player's resources.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class StatusBar
  {
    public const float BAR_HEIGHT = 30f;
    private readonly DependencyResolver m_resolver;
    private UiBuilder m_builder;
    private PanelWithShadow m_barPanel;
    private PanelWithShadow m_barPanelSecond;
    private readonly Lyst<StatusBar.BarItem> m_middleItems;
    private readonly Lyst<StatusBar.BarItem> m_rightItems;
    private ViewsCacheHomogeneous<Panel> m_delimitersCache;
    private PanelWithShadow m_largeTilesPanel;
    private StackContainer m_largeTilesStack;
    private float m_lastSeenWidth;
    private readonly Lyst<IUiElement> m_topItemsTemp;
    private readonly Lyst<IUiElement> m_bottomItemsTemp;

    public event Action<float> OnHeightChanged;

    private bool UiRegistered => this.m_barPanel != null;

    public StatusBar(IGameLoopEvents gameLoopEvents, DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_middleItems = new Lyst<StatusBar.BarItem>();
      this.m_rightItems = new Lyst<StatusBar.BarItem>();
      this.m_topItemsTemp = new Lyst<IUiElement>();
      this.m_bottomItemsTemp = new Lyst<IUiElement>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
      gameLoopEvents.RenderUpdate.AddNonSaveable<StatusBar>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void renderUpdate(GameTime time)
    {
      if (this.m_lastSeenWidth.IsNear(this.m_barPanel.GetWidth()))
        return;
      this.updateElements();
    }

    public void Build(UiBuilder builder)
    {
      builder.StatusBar = (Option<StatusBar>) this;
      this.m_builder = builder;
      this.m_delimitersCache = new ViewsCacheHomogeneous<Panel>((Func<Panel>) (() => this.createDelimiter()));
      this.m_largeTilesPanel = this.m_builder.NewPanelWithShadow("StatusBarLarge").SetBackground(this.m_builder.Style.Global.PanelsBg).AddShadowRightBottom().PutToLeftTopOf<PanelWithShadow>((IUiElement) builder.MainCanvas, new Vector2(0.0f, 90f));
      this.m_largeTilesStack = this.m_builder.NewStackContainer("TilesContainer").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).PutToLeftOf<StackContainer>((IUiElement) this.m_largeTilesPanel, 0.0f);
      this.m_largeTilesStack.SizeChanged += (Action<IUiElement>) (element =>
      {
        this.m_largeTilesPanel.SetWidth<PanelWithShadow>(element.GetWidth());
        this.m_barPanel.PutToTopOf<PanelWithShadow>((IUiElement) builder.MainCanvas, 30f, Offset.Left(element.GetWidth()));
        this.m_barPanelSecond.PutToTopOf<PanelWithShadow>((IUiElement) builder.MainCanvas, 30f, Offset.Left(element.GetWidth()) + Offset.Top(30f));
      });
      this.m_barPanel = builder.NewPanelWithShadow(nameof (StatusBar)).SetBackground(this.m_builder.Style.Global.ControlsBgColor).AddShadowBottom(true).PutToTopOf<PanelWithShadow>((IUiElement) builder.MainCanvas, 30f);
      this.createDelimiter().PutToLeftOf<Panel>((IUiElement) this.m_barPanel, 1f);
      this.m_barPanelSecond = builder.NewPanelWithShadow(nameof (StatusBar)).SetBackground(this.m_builder.Style.Global.ControlsBgColor).AddShadowBottom(true).PutToTopOf<PanelWithShadow>((IUiElement) builder.MainCanvas, 30f, Offset.Top(30f));
      this.createDelimiter().PutToLeftOf<Panel>((IUiElement) this.m_barPanelSecond, 1f);
      this.updateElements();
      foreach (IStatusBarItem implementation in this.m_resolver.ResolveAll<IStatusBarItem>().Implementations)
        implementation.RegisterIntoStatusBar(this);
    }

    public void SendToFront()
    {
      this.m_largeTilesPanel.SendToFront<PanelWithShadow>();
      this.m_barPanel.SendToFront<PanelWithShadow>();
      this.m_barPanelSecond.SendToFront<PanelWithShadow>();
    }

    private Panel createDelimiter()
    {
      return this.m_builder.NewPanel("Delimiter").SetBackground(ColorRgba.Black);
    }

    private void updateElements()
    {
      float num1 = (float) ((double) this.calculateRightWidthNeeded() + (double) this.calculateMiddleWidthNeeded(false, true) + 50.0);
      bool flag = (double) this.m_barPanel.GetWidth() < (double) num1;
      this.m_barPanelSecond.SetVisibility<PanelWithShadow>(flag);
      float num2 = this.placeRightItems(flag);
      this.m_lastSeenWidth = this.m_barPanel.GetWidth();
      this.placeMiddleItems((this.m_lastSeenWidth - 1f - num2).Max(0.0f), flag);
      Action<float> onHeightChanged = this.OnHeightChanged;
      if (onHeightChanged == null)
        return;
      onHeightChanged(this.GetHeight());
    }

    public float GetHeight()
    {
      return this.m_barPanel.GetHeight() + (this.m_barPanelSecond.IsVisible() ? this.m_barPanelSecond.GetHeight() : 0.0f);
    }

    private float calculateRightWidthNeeded()
    {
      float num1 = 10f;
      int num2 = 0;
      for (int index = 0; index < this.m_rightItems.Count; ++index)
      {
        StatusBar.BarItem rightItem = this.m_rightItems[index];
        if (rightItem.Element.IsVisible())
        {
          num1 += rightItem.Element.GetWidth();
          ++num2;
        }
      }
      return num1 + (float) (num2 - 1) * 9f;
    }

    /// <summary>Places items on the right side of the bar.</summary>
    /// <returns>Width used by items on the right and their delimiters (middle items cannot use it).</returns>
    private float placeRightItems(bool hasTwoBars)
    {
      float num = 10f;
      float rightOffset1 = 10f;
      this.m_delimitersCache.ReturnAll();
      for (int index = this.m_rightItems.Count - 1; index >= 0; --index)
      {
        StatusBar.BarItem rightItem = this.m_rightItems[index];
        IUiElement element = rightItem.Element;
        bool flag = !hasTwoBars || rightItem.TopBarPreferred;
        if (element.IsVisible())
        {
          Panel view = this.m_delimitersCache.GetView();
          view.Show<Panel>();
          float width = element.GetWidth();
          if (flag)
          {
            element.PutToRightOf<IUiElement>((IUiElement) this.m_barPanel, width, Offset.Right(num));
            float rightOffset2 = num + (width + 4f);
            view.PutToRightOf<Panel>((IUiElement) this.m_barPanel, 1f, Offset.Right(rightOffset2));
            num = rightOffset2 + 5f;
          }
          else
          {
            element.PutToRightOf<IUiElement>((IUiElement) this.m_barPanelSecond, width, Offset.Right(rightOffset1));
            float rightOffset3 = rightOffset1 + (width + 4f);
            view.PutToRightOf<Panel>((IUiElement) this.m_barPanelSecond, 1f, Offset.Right(rightOffset3));
            rightOffset1 = rightOffset3 + 5f;
          }
        }
      }
      return num.Max(rightOffset1);
    }

    /// <summary>Places items in the middle of the bar.</summary>
    private void placeMiddleItems(float availableSpace, bool hasTwoBars)
    {
      float leftOffset;
      if (!hasTwoBars)
      {
        float middleWidthNeeded = this.calculateMiddleWidthNeeded(false, true);
        leftOffset = (float) (1.0 + ((double) availableSpace - (double) middleWidthNeeded) / 2.0);
      }
      else
      {
        float middleWidthNeeded1 = this.calculateMiddleWidthNeeded(true, true);
        float middleWidthNeeded2 = this.calculateMiddleWidthNeeded(true, false);
        leftOffset = (float) (1.0 + ((double) availableSpace - (double) middleWidthNeeded1.Max(middleWidthNeeded2)) / 2.0);
      }
      this.m_topItemsTemp.Clear();
      this.m_bottomItemsTemp.Clear();
      foreach (StatusBar.BarItem middleItem in this.m_middleItems)
      {
        if (middleItem.Element.IsVisible())
        {
          if (!hasTwoBars || middleItem.TopBarPreferred)
            this.m_topItemsTemp.Add(middleItem.Element);
          else
            this.m_bottomItemsTemp.Add(middleItem.Element);
        }
      }
      for (int index = 0; index < this.m_topItemsTemp.Count.Max(this.m_bottomItemsTemp.Count); ++index)
      {
        float self = 0.0f;
        if (index < this.m_topItemsTemp.Count)
        {
          IUiElement uiElement = this.m_topItemsTemp[index];
          uiElement.PutToLeftOf<IUiElement>((IUiElement) this.m_barPanel, uiElement.GetWidth(), Offset.Left(leftOffset));
          self = uiElement.GetWidth();
        }
        if (index < this.m_bottomItemsTemp.Count)
        {
          IUiElement uiElement = this.m_bottomItemsTemp[index];
          uiElement.PutToLeftOf<IUiElement>((IUiElement) this.m_barPanelSecond, uiElement.GetWidth(), Offset.Left(leftOffset));
          self = self.Max(uiElement.GetWidth());
        }
        leftOffset += self + 25f;
      }
    }

    private float calculateMiddleWidthNeeded(bool hasTwoBars, bool calculateForTopBar)
    {
      int num1 = 0;
      float num2 = 0.0f;
      foreach (StatusBar.BarItem middleItem in this.m_middleItems)
      {
        if (middleItem.Element.IsVisible())
        {
          if (hasTwoBars)
          {
            if (calculateForTopBar == middleItem.TopBarPreferred)
            {
              ++num1;
              num2 += middleItem.Element.GetWidth();
            }
          }
          else
          {
            ++num1;
            num2 += middleItem.Element.GetWidth();
          }
        }
      }
      return num2 + (float) ((num1 - 1) * 25);
    }

    public void AddLargeTileToLeft(IUiElement element, float size, float order)
    {
      element.AddTo<IUiElement>(this.m_largeTilesStack, order, size);
    }

    /// <summary>Adds element to the right section of the status bar.</summary>
    public void AddElementToMiddle(IUiElement element, float order, bool isTopBarPreferred)
    {
      this.addElement(element, order, isTopBarPreferred, this.m_middleItems);
      this.UpdateElements();
    }

    /// <summary>Adds element to the right section of the status bar.</summary>
    public void AddElementToRight(IUiElement element, float order, bool isTopBarPreferred)
    {
      this.addElement(element, order, isTopBarPreferred, this.m_rightItems);
      this.UpdateElements();
    }

    /// <summary>
    /// Call when item has been resized of its visibility has been changed
    /// </summary>
    public void OnItemChanged() => this.UpdateElements();

    /// <summary>
    /// Call after element is shown/hidden/changes width (ff it doesn't implement <see cref="T:Mafi.Unity.UiFramework.IDynamicSizeElement" />).
    /// </summary>
    public void UpdateElements()
    {
      if (!this.UiRegistered)
        return;
      this.updateElements();
    }

    private void addElement(
      IUiElement element,
      float order,
      bool topBarPreferred,
      Lyst<StatusBar.BarItem> list)
    {
      if (element is IDynamicSizeElement dynamicSizeElement)
        dynamicSizeElement.SizeChanged += new Action<IUiElement>(this.elementSizeChanged);
      element.SetParent<IUiElement>((IUiElement) this.m_barPanel, false);
      for (int index = 0; index < list.Count; ++index)
      {
        if ((double) order < (double) list[index].Order)
        {
          list.Insert(index, new StatusBar.BarItem(element, order, topBarPreferred));
          return;
        }
      }
      list.Add(new StatusBar.BarItem(element, order, topBarPreferred));
    }

    private void elementSizeChanged(IUiElement element) => this.UpdateElements();

    private struct BarItem
    {
      public readonly IUiElement Element;
      public readonly float Order;
      public readonly bool TopBarPreferred;

      public BarItem(IUiElement element, float order, bool isTopBarPreferred)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Element = element;
        this.Order = order;
        this.TopBarPreferred = isTopBarPreferred;
      }
    }
  }
}
