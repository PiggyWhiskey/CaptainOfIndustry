// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.EntityUpgradeView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class EntityUpgradeView : IUiElement
  {
    private Btn m_upgradeBtn;
    private Tooltip m_upgradeToolTip;
    private Panel m_upgradeContainer;
    private UiBuilder m_builder;
    private readonly Action m_onUpgrade;
    private readonly IAssetTransactionManager m_assetsManager;
    private Txt m_upgradeTitle;
    private IconContainer m_upgradeIcon;
    private CostTooltip m_costTooltip;
    private LocStrFormatted? m_tooltipToShowIfNotAvailable;

    public GameObject GameObject => this.m_upgradeContainer.GameObject;

    public RectTransform RectTransform => this.m_upgradeContainer.RectTransform;

    public float Height => 80f;

    public EntityUpgradeView(Action onUpgrade, IAssetTransactionManager assetsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onUpgrade = onUpgrade;
      this.m_assetsManager = assetsManager;
    }

    public EntityUpgradeView Build(IUiElement parent, UiBuilder builder)
    {
      UiStyle style = builder.Style;
      this.m_builder = builder;
      this.m_upgradeContainer = builder.NewPanel("UpgradeContainer", parent).SetBackground(style.Panel.ItemOverlay);
      this.m_upgradeIcon = builder.NewIconContainer("Icon", (IUiElement) this.m_upgradeContainer).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_upgradeContainer, new Vector2(70f, 70f), Offset.Left(10f));
      builder.NewPanel("Divider", (IUiElement) this.m_upgradeContainer).SetBackground(style.Panel.Border.Color).PutToLeftOf<Panel>((IUiElement) this.m_upgradeContainer, 1f, Offset.Left(90f) + Offset.TopBottom(5f));
      this.m_upgradeTitle = builder.NewTxt("SectionTitle", (IUiElement) this.m_upgradeContainer).SetTextStyle(builder.Style.Panel.SectionTitle).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.m_upgradeContainer, builder.Style.Panel.LineHeight, Offset.Left(100f) + Offset.Top(5f));
      this.m_upgradeBtn = builder.NewBtn("UpgradeBtn", (IUiElement) this.m_upgradeContainer).SetText((LocStrFormatted) Tr.Upgrade).SetTextAlignment(TextAnchor.MiddleLeft).SetButtonStyle(style.Global.PrimaryBtn).OnClick(this.m_onUpgrade);
      this.m_upgradeToolTip = builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this.m_upgradeBtn).SetErrorTextStyle();
      this.m_upgradeBtn.PutToLeftTopOf<Btn>((IUiElement) this.m_upgradeContainer, this.m_upgradeBtn.GetOptimalSize(), Offset.Left(100f) + Offset.Top(35f));
      this.m_costTooltip = new CostTooltip(builder, this.m_assetsManager);
      this.m_costTooltip.AttachTo<Btn>((IUiElementWithHover<Btn>) this.m_upgradeBtn);
      return this;
    }

    public void SetActionTitle(LocStrFormatted title)
    {
      this.m_upgradeBtn.SetText(title);
      this.m_upgradeBtn.SetWidth<Btn>(this.m_upgradeBtn.GetOptimalWidth());
    }

    public void ShowTooltipIfNotAvailable(LocStrFormatted tooltipToShow)
    {
      this.m_tooltipToShowIfNotAvailable = new LocStrFormatted?(tooltipToShow);
    }

    public IUiUpdater CreateUpdater(
      Func<IUpgradableEntity> entityProvider,
      Action<bool> visibilityChangeRequest)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Tooltip tooltip = this.m_builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) this.m_upgradeBtn);
      LocStrFormatted errorMessage;
      updaterBuilder.Observe<KeyValuePair<bool, string>>((Func<KeyValuePair<bool, string>>) (() => Make.Kvp<bool, string>(entityProvider().Upgrader.IsUpgradeAvailable(out errorMessage), errorMessage.Value))).Observe<bool>((Func<bool>) (() => entityProvider().Upgrader.IsUpgradeVisible())).Observe<AssetValue>((Func<AssetValue>) (() => entityProvider().Upgrader.PriceToUpgrade)).Observe<Option<Proto>>((Func<Option<Proto>>) (() => entityProvider().Upgrader.NextTier)).Do((Action<KeyValuePair<bool, string>, bool, AssetValue, Option<Proto>>) ((isAvailableAndError, isVisible, priceToUpgrade, nextTier) =>
      {
        bool flag = isVisible;
        if (nextTier.HasValue && this.m_tooltipToShowIfNotAvailable.HasValue)
          flag = true;
        if (this.IsVisible() != flag)
          visibilityChangeRequest(flag);
        if (flag)
        {
          if (string.IsNullOrEmpty(isAvailableAndError.Value))
          {
            this.m_costTooltip.SetCost(priceToUpgrade);
            this.m_upgradeToolTip.SetText("");
          }
          else
          {
            this.m_costTooltip.SetCost(AssetValue.Empty);
            this.m_upgradeToolTip.SetText(isAvailableAndError.Value);
          }
        }
        this.m_upgradeBtn.SetEnabled(isAvailableAndError.Key & isVisible);
        if (!this.m_upgradeBtn.IsEnabled && this.m_tooltipToShowIfNotAvailable.HasValue)
        {
          tooltip.SetText(this.m_tooltipToShowIfNotAvailable.Value);
          this.m_costTooltip.SetCost(AssetValue.Empty);
        }
        else
          tooltip.SetText(string.Empty);
      }));
      updaterBuilder.Observe<LocStr>((Func<LocStr>) (() => entityProvider().Upgrader.UpgradeTitle)).Do((Action<LocStr>) (text => this.m_upgradeTitle.SetText((LocStrFormatted) text)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().Upgrader.UpgradeExists)).Observe<string>((Func<string>) (() => entityProvider().Upgrader.Icon)).Do((Action<bool, string>) ((upgradeExists, iconPath) =>
      {
        if (!upgradeExists)
          return;
        this.m_upgradeIcon.SetIcon(iconPath);
      }));
      IUiUpdater updater = updaterBuilder.Build();
      updater.AddChildUpdater(this.m_costTooltip.CreateUpdater());
      return updater;
    }

    public IUiUpdater CreateUpdater(
      Func<IUpgradableWorldEntity> entityProvider,
      Action<bool> visibilityChangeRequest)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      LocStrFormatted error;
      updaterBuilder.Observe<KeyValuePair<bool, string>>((Func<KeyValuePair<bool, string>>) (() => Make.Kvp<bool, string>(entityProvider().IsUpgradeAvailable(out error), error.Value))).Observe<bool>((Func<bool>) (() => entityProvider().IsUpgradeVisible())).Observe<AssetValue>((Func<AssetValue>) (() => entityProvider().PriceToUpgrade)).Do((Action<KeyValuePair<bool, string>, bool, AssetValue>) ((isAvailableAndError, isVisible, priceToUpgrade) =>
      {
        if (this.IsVisible() != isVisible)
          visibilityChangeRequest(isVisible);
        if (isVisible)
        {
          if (string.IsNullOrEmpty(isAvailableAndError.Value))
          {
            this.m_costTooltip.SetCost(priceToUpgrade);
            this.m_upgradeToolTip.SetText("");
          }
          else
          {
            this.m_costTooltip.SetCost(AssetValue.Empty);
            this.m_upgradeToolTip.SetText(isAvailableAndError.Value);
          }
        }
        this.m_upgradeBtn.SetEnabled(isAvailableAndError.Key);
      }));
      updaterBuilder.Observe<LocStrFormatted>((Func<LocStrFormatted>) (() => entityProvider().UpgradeTitle)).Do((Action<LocStrFormatted>) (text => this.m_upgradeTitle.SetText(text)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().UpgradeExists)).Observe<string>((Func<string>) (() => entityProvider().UpgradeIcon)).Do((Action<bool, string>) ((upgradeExists, iconPath) =>
      {
        if (!upgradeExists)
          return;
        this.m_upgradeIcon.SetIcon(iconPath);
      }));
      IUiUpdater updater = updaterBuilder.Build();
      updater.AddChildUpdater(this.m_costTooltip.CreateUpdater());
      return updater;
    }
  }
}
