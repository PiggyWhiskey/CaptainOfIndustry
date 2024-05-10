// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.UpgradeToolEntityRow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  public class UpgradeToolEntityRow : IUiElementWithUpdater, IUiElement
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly Action m_onSelectionChanged;
    private readonly Panel m_container;
    private readonly UpgradeToolEntityRow.CurrentEntityIcon m_currentEntityIcon;
    private readonly IconContainer m_arrow;
    private readonly UiStyle m_style;
    private readonly Txt m_count;
    private readonly StackContainer m_itemsContainer;
    private readonly Lyst<UpgradeToolEntityRow.UpgradeEntityButton> m_buttons;
    private readonly Lyst<IUpgradableEntity> m_entities;
    private int m_selectedIdx;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public UpgradeToolEntityRow(
      IUiElement parent,
      UiBuilder builder,
      IAssetTransactionManager assetTransactionManager,
      UnlockedProtosDbForUi unlockedProtosDb,
      Action onSelectionChanged)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_selectedIdx = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_style = builder.Style;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_onSelectionChanged = onSelectionChanged;
      this.m_container = builder.NewPanel("EntityView", parent).SetBackground(builder.Style.Panel.ItemOverlay).SetHeight<Panel>(110f);
      this.m_entities = new Lyst<IUpgradableEntity>();
      this.m_buttons = new Lyst<UpgradeToolEntityRow.UpgradeEntityButton>(1);
      this.m_itemsContainer = builder.NewStackContainer("Stack").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f).SetInnerPadding(Offset.LeftRight(10f)).PutToLeftOf<StackContainer>((IUiElement) this.m_container, 0.0f, Offset.TopBottom(5f));
      this.m_currentEntityIcon = new UpgradeToolEntityRow.CurrentEntityIcon(builder, this.m_container);
      this.m_currentEntityIcon.AppendTo<UpgradeToolEntityRow.CurrentEntityIcon>(this.m_itemsContainer, new float?(90f));
      this.m_arrow = builder.NewIconContainer("Arrow").SetIcon("Assets/Unity/UserInterface/General/UpgradeHorizontal.svg").SetColor(builder.Style.Global.DefaultPanelTextColor).AppendTo<IconContainer>(this.m_itemsContainer, new Vector2?(26.Vector2()), ContainerPosition.MiddleOrCenter, Offset.Left(20f));
      Txt txt = builder.NewTxt("Count").SetAlignment(TextAnchor.MiddleRight);
      TextStyle textBigBold = builder.Style.Global.TextBigBold;
      ref TextStyle local = ref textBigBold;
      int? nullable = new int?(18);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_count = txt.SetTextStyle(textStyle).AppendTo<Txt>(this.m_itemsContainer, new float?(50f));
      this.Updater = UpdaterBuilder.Start().Build();
      for (int parentIdx = 0; parentIdx < 1; ++parentIdx)
      {
        UpgradeToolEntityRow.UpgradeEntityButton objectToPlace = new UpgradeToolEntityRow.UpgradeEntityButton(this.m_container, builder, assetTransactionManager, parentIdx, (Action<int>) (id => this.setSelected(id)));
        objectToPlace.AppendTo<UpgradeToolEntityRow.UpgradeEntityButton>(this.m_itemsContainer, new float?(90f));
        this.m_buttons.Add(objectToPlace);
        this.Updater.AddChildUpdater(objectToPlace.Updater);
      }
    }

    public void SetEntity(IEnumerable<IUpgradableEntity> entities)
    {
      this.m_entities.Clear();
      this.m_entities.AddRange(entities);
      if (this.m_entities.IsEmpty)
        return;
      IUpgradableEntity upgradableEntity = this.m_entities.First<IUpgradableEntity>();
      Assert.That<Option<Proto>>(upgradableEntity.Upgrader.NextTier).HasValue<Proto>();
      IProtoWithUpgrade entityProto = upgradableEntity.Prototype as IProtoWithUpgrade;
      this.m_currentEntityIcon.SetEntity(entityProto);
      this.m_selectedIdx = -1;
      for (int index1 = 0; index1 < 2; ++index1)
      {
        if (index1 != 1 && entityProto != null)
        {
          Option<IProtoWithUpgrade> nextTierNonGeneric = entityProto.UpgradeNonGeneric.NextTierNonGeneric;
          if (!nextTierNonGeneric.IsNone && this.m_unlockedProtosDb.IsUnlocked((IProto) (entityProto.UpgradeNonGeneric.NextTierNonGeneric.Value as Proto)))
          {
            this.m_buttons[index1].SetEntity(entityProto.UpgradeNonGeneric.NextTierNonGeneric.Value, (IIndexable<IUpgradableEntity>) this.m_entities);
            this.m_buttons[index1].Show<UpgradeToolEntityRow.UpgradeEntityButton>();
            nextTierNonGeneric = entityProto.UpgradeNonGeneric.NextTierNonGeneric;
            entityProto = nextTierNonGeneric.ValueOrNull;
            continue;
          }
        }
        this.setSelected(index1 - 1, true);
        for (int index2 = index1; index2 < 1; ++index2)
          this.m_buttons[index2].Hide<UpgradeToolEntityRow.UpgradeEntityButton>();
        break;
      }
      this.m_count.SetText(this.m_entities.Count.ToStringCached() + " x");
    }

    private void setSelected(int idx, bool skipUpdatingCosts = false)
    {
      this.m_selectedIdx = idx == this.m_selectedIdx ? -1 : idx;
      for (int index = 0; index < 1; ++index)
        this.m_buttons[index].SetSelected(index == this.m_selectedIdx);
      ColorRgba color = this.m_selectedIdx == -1 ? this.m_style.Global.DefaultPanelTextColor : UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_ON;
      this.m_arrow.SetColor(color);
      this.m_count.SetColor(color);
      if (skipUpdatingCosts)
        return;
      Action selectionChanged = this.m_onSelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged();
    }

    public float GetDynamicWidth() => this.m_itemsContainer.GetDynamicWidth();

    public AssetValue GetCost()
    {
      return this.m_selectedIdx < 0 ? AssetValue.Empty : this.m_buttons[this.m_selectedIdx].Cost;
    }

    public bool IsUpgradeSelected() => this.m_selectedIdx >= 0;

    public void OnUpgrade(IInputScheduler inputScheduler)
    {
      if (this.m_selectedIdx == -1)
        return;
      foreach (IUpgradableEntity entity in this.m_entities)
        inputScheduler.ScheduleInputCmd<UpgradeEntityCmd>(new UpgradeEntityCmd(entity));
    }

    private class CurrentEntityIcon : IUiElement
    {
      private readonly UiStyle m_style;
      private readonly Panel m_container;
      private readonly IconContainer m_icon;
      private readonly Txt m_title;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public CurrentEntityIcon(UiBuilder builder, Panel parent)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_style = builder.Style;
        this.m_container = builder.NewPanel("IconPanel", (IUiElement) parent).SetBackground(UpgradeToolEntityRow.UpgradeEntityButton.BTN_BG_COLOR).SetBorderStyle(new BorderStyle(UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_OFF));
        this.m_icon = builder.NewIconContainer("Icon", (IUiElement) this.m_container).PutTo<IconContainer>((IUiElement) this.m_container, Offset.LeftRight(10f) + Offset.Top(5f) + Offset.Bottom(30f));
        this.m_title = builder.NewTxt("Title", (IUiElement) this.m_container).SetAlignment(TextAnchor.UpperCenter).SetTextStyle(this.m_style.EntitiesMenu.ItemTitleStyle).AllowVerticalOverflow().PutToBottomOf<Txt>((IUiElement) this.m_container, 30f);
      }

      public void SetEntity(IProtoWithUpgrade entityProto)
      {
        switch (entityProto)
        {
          case LayoutEntityProto layoutEntityProto:
            this.m_icon.SetIcon(layoutEntityProto.Graphics.IconPath, this.m_style.EntitiesMenu.ItemIconClr);
            this.m_title.SetText(layoutEntityProto.Strings.Name.TranslatedString);
            break;
          case TransportProto transportProto:
            this.m_icon.SetIcon(transportProto.Graphics.IconPath, this.m_style.EntitiesMenu.ItemIconClr);
            this.m_title.SetText(transportProto.Strings.Name.TranslatedString);
            break;
          default:
            Assert.Fail(string.Format("{0} is unknown type", (object) entityProto));
            break;
        }
      }
    }

    private class UpgradeEntityButton : IUiElementWithUpdater, IUiElement
    {
      public static readonly ColorRgba BTN_BG_COLOR;
      public static readonly ColorRgba BORDER_COLOR_OFF;
      public static readonly ColorRgba BORDER_COLOR_ON;
      private readonly UiStyle m_style;
      private readonly Btn m_button;
      private readonly IconContainer m_icon;
      private readonly Txt m_title;
      private readonly Panel m_tickBox;
      private readonly CostTooltip m_costTooltip;
      private bool m_isSelected;

      public GameObject GameObject => this.m_button.GameObject;

      public RectTransform RectTransform => this.m_button.RectTransform;

      public IUiUpdater Updater { get; }

      public AssetValue Cost { get; private set; }

      public UpgradeEntityButton(
        Panel parent,
        UiBuilder builder,
        IAssetTransactionManager assetsManager,
        int parentIdx,
        Action<int> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: reference to a compiler-generated field
        this.\u003CCost\u003Ek__BackingField = AssetValue.Empty;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_isSelected = false;
        this.m_style = builder.Style;
        this.m_button = builder.NewBtn("Button", (IUiElement) parent).OnClick((Action) (() => onClick(parentIdx))).SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave)).SetButtonStyle(new BtnStyle(border: new BorderStyle?(new BorderStyle(UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_OFF)))).SetBackgroundColor(UpgradeToolEntityRow.UpgradeEntityButton.BTN_BG_COLOR);
        this.m_costTooltip = new CostTooltip(builder, assetsManager);
        this.m_costTooltip.AttachTo<Btn>((IUiElementWithHover<Btn>) this.m_button);
        this.Updater = this.m_costTooltip.CreateUpdater();
        this.m_icon = builder.NewIconContainer("Icon", (IUiElement) this.m_button).PutTo<IconContainer>((IUiElement) this.m_button, Offset.All(10f) + Offset.Bottom(20f));
        this.m_title = builder.NewTxt("Title", (IUiElement) this.m_button).SetAlignment(TextAnchor.UpperCenter).SetTextStyle(this.m_style.EntitiesMenu.ItemTitleStyle).AllowVerticalOverflow().PutToBottomOf<Txt>((IUiElement) this.m_button, 30f, Offset.LeftRight(5f));
        this.m_tickBox = builder.NewPanel("TickBox", (IUiElement) this.m_button).SetBackground(UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_ON).PutToLeftTopOf<Panel>((IUiElement) this.m_button, 18.Vector2()).Hide<Panel>();
        builder.NewIconContainer("Icon", (IUiElement) this.m_tickBox).SetIcon("Assets/Unity/UserInterface/General/Tick128.png", UpgradeToolEntityRow.UpgradeEntityButton.BTN_BG_COLOR).PutTo<IconContainer>((IUiElement) this.m_tickBox);
      }

      public void SetEntity(
        IProtoWithUpgrade entityProto,
        IIndexable<IUpgradableEntity> sourceEntities)
      {
        switch (entityProto)
        {
          case LayoutEntityProto layoutEntityProto:
            this.m_icon.SetIcon(layoutEntityProto.Graphics.IconPath, this.m_style.EntitiesMenu.ItemIconClr);
            this.m_title.SetText(layoutEntityProto.Strings.Name.TranslatedString);
            break;
          case TransportProto transportProto:
            this.m_icon.SetIcon(transportProto.Graphics.IconPath, this.m_style.EntitiesMenu.ItemIconClr);
            this.m_title.SetText(transportProto.Strings.Name.TranslatedString);
            break;
          default:
            Assert.Fail(string.Format("{0} is unknown type", (object) entityProto));
            break;
        }
        this.Cost = AssetValue.Empty;
        foreach (IUpgradableEntity sourceEntity in sourceEntities)
          this.Cost += sourceEntity.Upgrader.PriceToUpgrade;
        this.m_costTooltip.SetCost(this.Cost);
      }

      private void onMouseEnter()
      {
        this.m_icon.SetColor(this.m_style.EntitiesMenu.ItemIconHoveredClr);
      }

      private void onMouseLeave() => this.m_icon.SetColor(this.m_style.EntitiesMenu.ItemIconClr);

      public void SetSelected(bool selected)
      {
        this.m_isSelected = selected;
        this.m_tickBox.SetVisibility<Panel>(this.m_isSelected);
        this.m_button.SetBorderColor(this.m_isSelected ? UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_ON : UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_OFF);
      }

      static UpgradeEntityButton()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        UpgradeToolEntityRow.UpgradeEntityButton.BTN_BG_COLOR = (ColorRgba) 1052688;
        UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_OFF = (ColorRgba) 4737096;
        UpgradeToolEntityRow.UpgradeEntityButton.BORDER_COLOR_ON = (ColorRgba) 16762368;
      }
    }
  }
}
