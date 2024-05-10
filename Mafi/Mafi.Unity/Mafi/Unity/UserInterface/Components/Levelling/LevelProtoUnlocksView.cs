// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.Levelling.LevelProtoUnlocksView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.Levelling
{
  /// <summary>
  /// A single row showing unlocks the player gets after attaining the next level.
  /// </summary>
  public class LevelProtoUnlocksView : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly LevelProtoUnlocksView.Style m_style;
    private readonly StackContainer m_container;
    private readonly Lyst<IconContainer> m_icons;
    private readonly ImmutableArray<LevelProtoUnlocksView.Group> m_groups;
    private readonly Panel m_mainContainer;

    public GameObject GameObject => this.m_mainContainer.GameObject;

    public RectTransform RectTransform => this.m_mainContainer.RectTransform;

    public LevelProtoUnlocksView(UiBuilder builder, LevelProtoUnlocksView.Style style)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_icons = new Lyst<IconContainer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_style = style;
      this.m_mainContainer = builder.NewPanel("bla");
      this.m_container = builder.NewStackContainer(nameof (LevelProtoUnlocksView)).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(this.m_style.SpaceBetweenGroups).PutTo<StackContainer>((IUiElement) this.m_mainContainer);
      this.m_groups = ImmutableArray.Create<LevelProtoUnlocksView.Group>(new LevelProtoUnlocksView.Group(this, Tr.ResearchUnlocked__NewBuildings.TranslatedString, this.m_style.BuildingsIconSize, (Func<Proto, LevelProtoUnlocksView.ProtoGfx?>) (proto => proto is LayoutEntityProto layoutEntityProto ? new LevelProtoUnlocksView.ProtoGfx?(new LevelProtoUnlocksView.ProtoGfx((LocStrFormatted) layoutEntityProto.Strings.Name, layoutEntityProto.Graphics.IconPath)) : new LevelProtoUnlocksView.ProtoGfx?())).AppendTo<LevelProtoUnlocksView.Group>(this.m_container, new Vector2?(new Vector2(0.0f, (float) this.m_style.BuildingsIconSize)), ContainerPosition.MiddleOrCenter), new LevelProtoUnlocksView.Group(this, Tr.ResearchUnlocked__NewProducts.TranslatedString, this.m_style.ProductsIconSize, (Func<Proto, LevelProtoUnlocksView.ProtoGfx?>) (proto => proto is ProductProto productProto ? new LevelProtoUnlocksView.ProtoGfx?(new LevelProtoUnlocksView.ProtoGfx((LocStrFormatted) productProto.Strings.Name, productProto.Graphics.IconPath)) : new LevelProtoUnlocksView.ProtoGfx?())).AppendTo<LevelProtoUnlocksView.Group>(this.m_container, new Vector2?(new Vector2(0.0f, (float) this.m_style.ProductsIconSize)), ContainerPosition.MiddleOrCenter));
    }

    public void SetProtos(IEnumerable<Proto> protos)
    {
      foreach (LevelProtoUnlocksView.Group group in this.m_groups)
        group.ClearProtos();
      foreach (Proto proto in protos)
      {
        ImmutableArray<LevelProtoUnlocksView.Group>.Enumerator enumerator = this.m_groups.GetEnumerator();
label_6:
        if (enumerator.MoveNext() && !enumerator.Current.TryAddProto(proto))
          goto label_6;
      }
      this.m_container.UpdateSizesFromItems();
      float num = 0.0f;
      foreach (LevelProtoUnlocksView.Group group in this.m_groups)
      {
        this.m_container.SetItemVisibility((IUiElement) group, group.Items.Count > 0);
        num = num.Max(group.GetWidth());
      }
      this.m_mainContainer.SetHeight<Panel>(this.m_container.GetDynamicHeight());
      this.m_mainContainer.SetWidth<Panel>(num);
    }

    private class Group : IUiElement
    {
      private readonly LevelProtoUnlocksView m_view;
      private readonly string m_labelText;
      private readonly Func<Proto, LevelProtoUnlocksView.ProtoGfx?> m_tryGetGfx;
      private readonly int m_iconSize;
      private readonly UiBuilder m_builder;
      public readonly Lyst<LevelProtoUnlocksView.ProtoGfx> Items;
      private readonly StackContainer m_container;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public Group(
        LevelProtoUnlocksView view,
        string labelText,
        int iconSize,
        Func<Proto, LevelProtoUnlocksView.ProtoGfx?> tryGetGfx)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Items = new Lyst<LevelProtoUnlocksView.ProtoGfx>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_view = view;
        this.m_labelText = labelText;
        this.m_tryGetGfx = tryGetGfx;
        this.m_iconSize = iconSize;
        this.m_builder = this.m_view.m_builder;
        this.m_container = this.m_builder.NewStackContainer(nameof (LevelProtoUnlocksView)).SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(this.m_view.m_style.SpaceBetweenIcons);
      }

      public void ClearProtos()
      {
        this.m_container.ClearAndDestroyAll();
        this.Items.Clear();
        Txt txt = this.m_builder.NewTxt("GroupLabel").SetTextStyle(this.m_view.m_style.TextStyle).AllowVerticalOverflow().SetText(this.m_labelText).SetPreferredSize();
        txt.AppendTo<Txt>(this.m_container, new Vector2?(txt.GetSize()), ContainerPosition.MiddleOrCenter);
      }

      public bool TryAddProto(Proto proto)
      {
        LevelProtoUnlocksView.ProtoGfx? nullable = this.m_tryGetGfx(proto);
        if (!nullable.HasValue)
          return false;
        this.Items.Add(nullable.Value);
        IconContainer objectToPlace = this.m_builder.NewIconContainer("ProtoIcon");
        objectToPlace.SetIcon(nullable.Value.IconPath);
        objectToPlace.AppendTo<IconContainer>(this.m_container, new float?((float) this.m_iconSize));
        return true;
      }
    }

    private struct ProtoGfx
    {
      public readonly LocStrFormatted Name;
      public readonly string IconPath;

      public ProtoGfx(LocStrFormatted name, string iconPath)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Name = name;
        this.IconPath = iconPath;
      }
    }

    public class Style
    {
      public readonly int BuildingsIconSize;
      public readonly int ProductsIconSize;
      public readonly float SpaceBetweenIcons;
      public readonly float SpaceBetweenLabelAndIcons;
      public readonly float SpaceBetweenGroups;
      public readonly TextStyle TextStyle;

      public Style(
        int buildingsIconSize,
        int productsIconSize,
        float spaceBetweenIcons,
        float spaceBetweenLabelAndIcons,
        float spaceBetweenGroups,
        TextStyle textStyle)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.BuildingsIconSize = buildingsIconSize;
        this.ProductsIconSize = productsIconSize;
        this.SpaceBetweenIcons = spaceBetweenIcons;
        this.SpaceBetweenLabelAndIcons = spaceBetweenLabelAndIcons;
        this.SpaceBetweenGroups = spaceBetweenGroups;
        this.TextStyle = textStyle;
      }
    }
  }
}
