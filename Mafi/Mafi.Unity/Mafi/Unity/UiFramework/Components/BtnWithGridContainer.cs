// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.BtnWithGridContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  /// <summary>
  /// Component that wraps a button (to add elements) and grid view of added elements. Used for instance for assigned
  /// buildings.
  /// </summary>
  public class BtnWithGridContainer : IUiElement, IDynamicSizeElement
  {
    private readonly UiBuilder m_builder;
    public readonly GridContainer ItemsContainer;
    private readonly Panel m_container;
    private readonly StackContainer m_btnHolder;
    private float m_itemsLeftOffset;
    private readonly Txt m_noItemsText;
    private float m_topOffset;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public BtnWithGridContainer(UiBuilder builder, Vector2 itemSize)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      BtnWithGridContainer withGridContainer = this;
      this.m_builder = builder;
      UiStyle style = builder.Style;
      this.m_container = builder.NewPanel("GridContainerWithBtn").SetBackground(style.Panel.ItemOverlay);
      int y = 38;
      this.m_topOffset = (float) (((double) itemSize.y - (double) y) / 2.0 + 3.0);
      this.m_btnHolder = builder.NewStackContainer("PlusBtnContainer").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(3f).PutToLeftTopOf<StackContainer>((IUiElement) this.m_container, new Vector2(0.0f, (float) y), Offset.Left(style.Panel.Indent) + Offset.Top(this.m_topOffset));
      this.m_btnHolder.SizeChanged += (Action<IUiElement>) (_ =>
      {
        withGridContainer.m_itemsLeftOffset = withGridContainer.m_btnHolder.GetDynamicWidth() + style.Panel.Indent + style.Panel.Padding;
        withGridContainer.ItemsContainer.PutToTopOf<GridContainer>((IUiElement) withGridContainer.m_container, 0.0f, Offset.Left(withGridContainer.m_itemsLeftOffset));
      });
      this.ItemsContainer = builder.NewGridContainer("Items").SetDynamicHeightMode().SetCellSpacing(3f).SetCellSize(itemSize).SetInnerPadding(Offset.TopBottom(3f) + Offset.Right(20f)).PutToTopOf<GridContainer>((IUiElement) this.m_container, 0.0f);
      this.m_noItemsText = builder.NewTxt("NoItemsInfo").SetTextStyle(style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) this.m_container).Hide<Txt>();
    }

    public void AddBtn(Btn btn) => this.m_btnHolder.Append((IUiElement) btn, new float?(38f));

    public void SetBtnVisibility(Btn btn, bool isVisible)
    {
      this.m_btnHolder.SetItemVisibility((IUiElement) btn, isVisible);
    }

    public BtnWithGridContainer SetNumberOfColumns(int columnsCount)
    {
      this.ItemsContainer.SetDynamicHeightMode(columnsCount);
      return this;
    }

    public void StartBatchOperation() => this.ItemsContainer.StartBatchOperation();

    public void FinishBatchOperation()
    {
      this.ItemsContainer.FinishBatchOperation();
      this.UpdateLayout();
    }

    public void UpdateLayout()
    {
      this.m_noItemsText.SetVisibility<Txt>(this.ItemsContainer.VisibleItemsCount == 0);
      this.m_noItemsText.PutTo<Txt>((IUiElement) this.m_container, Offset.Left(2f * this.m_builder.Style.Panel.Indent + this.m_btnHolder.GetDynamicWidth()));
      this.SetHeight<BtnWithGridContainer>(Mathf.Max(this.ItemsContainer.ComputeHeightFor(1), this.ItemsContainer.GetHeight()));
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    public BtnWithGridContainer SetTextToShowWhenEmpty(string text)
    {
      this.m_noItemsText.SetText(text);
      return this;
    }

    public float GetRequiredWidth()
    {
      return this.m_itemsLeftOffset + this.ItemsContainer.GetRequiredWidth();
    }

    public void SetupInnerWindowWithButton(
      IWindowWithInnerWindowsSupport owner,
      WindowView innerView,
      Action onExitAction,
      Btn btn)
    {
      // ISSUE: method pointer
      owner.SetupInnerWindowWithButton(innerView, (IUiElement) this.m_btnHolder, (IUiElement) btn, new Action((object) this, __methodptr(\u003CSetupInnerWindowWithButton\u003Eg__returnBtnHolder\u007C23_0)), onExitAction);
    }
  }
}
