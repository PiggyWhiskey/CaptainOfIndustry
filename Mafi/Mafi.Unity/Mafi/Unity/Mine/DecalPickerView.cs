// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.DecalPickerView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Mine
{
  internal class DecalPickerView : IUiElement
  {
    private readonly ProtosDb m_protosDb;
    private readonly ToolbarController m_toolbarController;
    public static ImmutableArray<ColorRgba> Colors;
    private readonly Lyst<DecalPickerView.ColorView> m_colorsViews;
    private readonly Lyst<DecalPickerView.DecalsCategoryView> m_categories;
    public TerrainTileSurfaceDecalProto SelectedDecal;
    public int SelectedColorIndex;
    private Panel m_container;
    private Action m_onClose;
    private Panel m_innerContainer;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    internal DecalPickerView(ProtosDb protosDb, ToolbarController toolbarController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_colorsViews = new Lyst<DecalPickerView.ColorView>();
      this.m_categories = new Lyst<DecalPickerView.DecalsCategoryView>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_toolbarController = toolbarController;
    }

    public void SetOnClose(Action onClose) => this.m_onClose = onClose;

    public void BuildUi(UiBuilder builder)
    {
      this.m_container = builder.NewPanel("DecalsPicker");
      int y = 30;
      Panel panel = builder.NewPanel("Colors").SetBackground(builder.Style.Panel.ItemOverlay);
      StackContainer leftTopOf = builder.NewStackContainer("Stack").SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(5f).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Left(8f)).PutToLeftTopOf<StackContainer>((IUiElement) panel, new Vector2(0.0f, (float) y), Offset.Top(8f));
      this.m_innerContainer = builder.NewPanel("Inner").SetBackground(builder.Style.Panel.Background).PutTo<Panel>((IUiElement) this.m_container, Offset.Top((float) (y + 16)));
      builder.NewPanel("Stripe").SetBackground(builder.Style.Panel.ItemOverlay).PutToTopOf<Panel>((IUiElement) this.m_container, 4f, Offset.Top((float) (y + 16)));
      int index = 0;
      foreach (ColorRgba color in DecalPickerView.Colors)
      {
        DecalPickerView.ColorView objectToPlace = new DecalPickerView.ColorView(builder, color, index, new Action<int>(this.onColorSelected));
        objectToPlace.AppendTo<DecalPickerView.ColorView>(leftTopOf, new float?((float) y));
        this.m_colorsViews.Add(objectToPlace);
        ++index;
      }
      builder.NewBtn("Close").SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Close.svg").OnClick((Action) (() =>
      {
        Action onClose = this.m_onClose;
        if (onClose == null)
          return;
        onClose();
      })).PutToRightMiddleOf<Btn>((IUiElement) panel, 20.Vector2(), Offset.Right(8f));
      panel.PutToLeftTopOf<Panel>((IUiElement) this.m_container, new Vector2(leftTopOf.GetDynamicWidth() + 50f, (float) (y + 16)));
      ScrollableContainer scrollableContainer = builder.NewScrollableContainer("Scroller").AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.m_innerContainer);
      StackContainer child = builder.NewStackContainer("Cats").SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.TopToBottom).SetInnerPadding(Offset.All(6f) + Offset.Bottom(10f));
      scrollableContainer.AddItemTopLeft((IUiElement) child);
      ImmutableArray<TerrainTileSurfaceDecalProto> immutableArray1 = this.m_protosDb.All<TerrainTileSurfaceDecalProto>().ToImmutableArray<TerrainTileSurfaceDecalProto>();
      foreach (SurfaceDecalCategoryProto decalCategoryProto in (IEnumerable<SurfaceDecalCategoryProto>) this.m_protosDb.All<SurfaceDecalCategoryProto>().OrderBy<SurfaceDecalCategoryProto, float>((Func<SurfaceDecalCategoryProto, float>) (x => x.Order)))
      {
        SurfaceDecalCategoryProto category = decalCategoryProto;
        ImmutableArray<TerrainTileSurfaceDecalProto> immutableArray2 = immutableArray1.Where((Func<TerrainTileSurfaceDecalProto, bool>) (x => x.Graphics.Category == category)).ToImmutableArray<TerrainTileSurfaceDecalProto>();
        if (immutableArray2.IsNotEmpty)
        {
          DecalPickerView.DecalsCategoryView element = new DecalPickerView.DecalsCategoryView(builder, category, immutableArray2, new Action<TerrainTileSurfaceDecalProto>(this.onDecalSelected));
          child.Append((IUiElement) element, new Vector2?(element.GetSize()), new ContainerPosition?(ContainerPosition.LeftOrTop));
          this.m_categories.Add(element);
        }
      }
      this.onDecalSelected(immutableArray1.First);
      this.onColorSelected(0);
      this.PutToCenterBottomOf<DecalPickerView>((IUiElement) this.m_toolbarController.ToolsWindowAnchor, new Vector2((float) ((double) this.m_categories.First.GetWidth() + 12.0 + 15.0), 300f));
    }

    private void onColorSelected(int index)
    {
      this.SelectedColorIndex = index;
      ColorRgba color = DecalPickerView.Colors[index];
      foreach (DecalPickerView.DecalsCategoryView category in this.m_categories)
        category.SetColor(color);
      foreach (DecalPickerView.ColorView colorsView in this.m_colorsViews)
        colorsView.SetSelected(colorsView.Index == index);
    }

    private void onDecalSelected(TerrainTileSurfaceDecalProto decal)
    {
      this.SelectedDecal = decal;
      foreach (DecalPickerView.DecalsCategoryView category in this.m_categories)
        category.SetSelected(decal);
    }

    static DecalPickerView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DecalPickerView.Colors = ImmutableArray.Create<ColorRgba>(new ColorRgba(14906390), new ColorRgba(14925583), new ColorRgba(13583171), new ColorRgba(4698439), new ColorRgba(6327509), new ColorRgba(8481978), new ColorRgba(13750737), new ColorRgba(0));
    }

    private class DecalsCategoryView : IUiElement
    {
      private readonly Panel m_container;
      private readonly Lyst<DecalPickerView.DecalView> m_decalsViews;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public DecalsCategoryView(
        UiBuilder builder,
        SurfaceDecalCategoryProto category,
        ImmutableArray<TerrainTileSurfaceDecalProto> decals,
        Action<TerrainTileSurfaceDecalProto> onDecalSelected)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_decalsViews = new Lyst<DecalPickerView.DecalView>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_container = builder.NewPanel("Category");
        builder.NewTitle("Title").SetText((LocStrFormatted) category.Strings.Name).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.m_container, 25f);
        GridContainer leftTopOf = builder.NewGridContainer("Decals").SetCellSize(52.Vector2()).SetCellSpacing(8f).SetDynamicHeightMode(10).SetInnerPadding(Offset.LeftRight(5f)).PutToLeftTopOf<GridContainer>((IUiElement) this.m_container, 0.Vector2(), Offset.Top(25f));
        foreach (TerrainTileSurfaceDecalProto decal in decals)
        {
          DecalPickerView.DecalView element = new DecalPickerView.DecalView(builder, decal, onDecalSelected);
          leftTopOf.Append((IUiElement) element);
          this.m_decalsViews.Add(element);
        }
        this.SetSize<DecalPickerView.DecalsCategoryView>(new Vector2(leftTopOf.GetRequiredWidth(), leftTopOf.GetRequiredHeight() + 25f));
      }

      public void SetColor(ColorRgba color)
      {
        foreach (DecalPickerView.DecalView decalsView in this.m_decalsViews)
          decalsView.SetColor(color);
      }

      public void SetSelected(TerrainTileSurfaceDecalProto decalProto)
      {
        foreach (DecalPickerView.DecalView decalsView in this.m_decalsViews)
          decalsView.SetSelected((Proto) decalsView.DecalProto == (Proto) decalProto);
      }
    }

    private class ColorView : IUiElement
    {
      public readonly int Index;
      private readonly Panel m_panel;
      private readonly Btn m_btn;
      private bool m_isSelected;

      public GameObject GameObject => this.m_btn.GameObject;

      public RectTransform RectTransform => this.m_btn.RectTransform;

      public ColorView(UiBuilder builder, ColorRgba color, int index, Action<int> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Index = index;
        this.m_btn = builder.NewBtn("Color").SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave)).OnClick((Action) (() => onClick(index)));
        this.m_panel = builder.NewPanel("Bg").SetBackground("Assets/Unity/UserInterface/General/WhiteGrayBg32.png", new ColorRgba?(color));
        this.SetSelected(false);
      }

      private void onMouseEnter()
      {
        if (this.m_isSelected)
          return;
        this.m_panel.PutTo<Panel>((IUiElement) this.m_btn, Offset.Zero);
      }

      private void onMouseLeave()
      {
        if (this.m_isSelected)
          return;
        this.m_panel.PutTo<Panel>((IUiElement) this.m_btn, Offset.All(2f));
      }

      public void SetSelected(bool isSelected)
      {
        this.m_isSelected = isSelected;
        this.m_panel.PutTo<Panel>((IUiElement) this.m_btn, isSelected ? Offset.Zero : Offset.All(2f));
      }
    }

    private class DecalView : IUiElement
    {
      public readonly TerrainTileSurfaceDecalProto DecalProto;
      private readonly IconContainer m_icon;
      private readonly Btn m_btn;
      private bool m_isSelected;

      public GameObject GameObject => this.m_btn.GameObject;

      public RectTransform RectTransform => this.m_btn.RectTransform;

      public DecalView(
        UiBuilder builder,
        TerrainTileSurfaceDecalProto decalProto,
        Action<TerrainTileSurfaceDecalProto> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        DecalPickerView.DecalView decalView = this;
        this.DecalProto = decalProto;
        this.m_btn = builder.NewBtn("Decal").SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave)).OnClick((Action) (() => onClick(decalView.DecalProto)));
        this.m_icon = builder.NewIconContainer("Decal").SetIcon(decalProto.Graphics.IconPath).PutTo<IconContainer>((IUiElement) this.m_btn);
        this.SetSelected(false);
      }

      private void onMouseEnter()
      {
        if (this.m_isSelected)
          return;
        this.m_icon.PutTo<IconContainer>((IUiElement) this.m_btn, Offset.All(2f));
      }

      private void onMouseLeave()
      {
        if (this.m_isSelected)
          return;
        this.m_icon.PutTo<IconContainer>((IUiElement) this.m_btn, Offset.All(4f));
      }

      public void SetColor(ColorRgba color) => this.m_icon.SetColor(color);

      public void SetSelected(bool isSelected)
      {
        this.m_isSelected = isSelected;
        this.m_icon.PutTo<IconContainer>((IUiElement) this.m_btn, isSelected ? Offset.All(2f) : Offset.All(4f));
        this.m_btn.SetBackgroundColor(isSelected ? (ColorRgba) 3026478 : ColorRgba.Empty);
      }
    }
  }
}
