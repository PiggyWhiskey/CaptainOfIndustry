// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.EdictTogglePanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Core.Population.Edicts;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  internal class EdictTogglePanel : IUiElement
  {
    public readonly IUiUpdater Updater;
    public readonly Edict FirstEdict;
    private readonly Panel m_container;
    private readonly Lyst<EdictCard> m_cards;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public EdictTogglePanel(
      IUiElement parent,
      UiBuilder builder,
      IInputScheduler inputScheduler,
      UnlockedProtosDbForUi unlockedProtosDb,
      Edict firstEdict,
      ImmutableArray<Edict> allEdicts)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_cards = new Lyst<EdictCard>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      EdictTogglePanel edictTogglePanel = this;
      this.FirstEdict = firstEdict;
      this.Updater = UpdaterBuilder.Start().Build();
      this.m_container = builder.NewPanel("EdictGroup", parent);
      int order = 0;
      for (Edict edictToAdd = firstEdict; edictToAdd != null; edictToAdd = allEdicts.FirstOrDefault((Func<Edict, bool>) (x => x.Prototype.PreviousTier == edictToAdd.Prototype)))
      {
        EdictCard edictCard = new EdictCard((IUiElement) this.m_container, builder, inputScheduler, unlockedProtosDb, edictToAdd, order, new Action<EdictCard>(this.onEdictCardHeaderClick));
        edictCard.PutToLeftTopOf<EdictCard>((IUiElement) this.m_container, edictCard.GetSize());
        this.Updater.AddChildUpdater(edictCard.Updater);
        this.m_cards.Add(edictCard);
        if (order == 0)
          this.m_container.SetSize<Panel>(edictCard.GetSize());
        ++order;
      }
      foreach (EdictCard card in this.m_cards)
      {
        card.TopCard.SetParent<IUiElement>((IUiElement) this);
        card.TopCard.SendToBack<IUiElement>();
      }
      this.onEdictCardHeaderClick(this.m_cards.First);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Panel unityBalanceHolder = builder.NewPanel("UnityBalanceHolder").SetBackground(builder.Style.Panel.ItemOverlay).PutToRightTopOf<Panel>((IUiElement) this.m_container, new Vector2(0.0f, 30f));
      TextWithIcon unityBalance = new TextWithIcon(builder, 18).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg");
      unityBalance.PutToRightOf<TextWithIcon>((IUiElement) unityBalanceHolder, 0.0f, Offset.Right(5f));
      updaterBuilder.Observe<KeyValuePair<Upoints, Upoints>>((Func<KeyValuePair<Upoints, Upoints>>) (() =>
      {
        Upoints zero1 = Upoints.Zero;
        Upoints zero2 = Upoints.Zero;
        foreach (EdictCard card in edictTogglePanel.m_cards)
        {
          Edict edict = card.Edict;
          if (edict.IsEnabled)
            zero2 += edict.Prototype.MonthlyUpointsCost;
          if (edict.IsActive)
            zero1 += edict.Prototype.MonthlyUpointsCost;
        }
        return Make.Kvp<Upoints, Upoints>(zero1, zero2);
      })).Do((Action<KeyValuePair<Upoints, Upoints>>) (upointsData =>
      {
        Upoints key = upointsData.Key;
        Upoints upoints = upointsData.Value;
        if (upoints.IsNotZero)
        {
          if (key != upoints)
          {
            unityBalance.SetPrefixText(key.Format1Dec() + " / " + upoints.Format1Dec());
            unityBalance.SetColor(builder.Style.Global.OrangeText);
          }
          else if (upoints.IsPositive)
          {
            unityBalance.SetPrefixText(upoints.Format1Dec() ?? "");
            unityBalance.SetColor(builder.Style.Global.DangerClr);
          }
          else
          {
            unityBalance.SetPrefixText((-upoints).Format1Dec() ?? "");
            unityBalance.SetColor(builder.Style.Global.GreenForDark);
          }
          unityBalanceHolder.SetWidth<Panel>(unityBalance.GetWidth() + 10f);
        }
        unityBalanceHolder.SetVisibility<Panel>(upoints.IsNotZero);
      }));
      IconContainer warningIcon = builder.NewIconContainer("Warning", (IUiElement) this.m_container).SetIcon("Assets/Unity/UserInterface/General/Warning128.png", builder.Style.Global.OrangeText).PutToLeftTopOf<IconContainer>((IUiElement) this.m_container, new Vector2(24f, 30f), Offset.Left((float) (order * 50 + 10)));
      Tooltip warningTxt = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) warningIcon);
      updaterBuilder.Observe<string>((Func<string>) (() =>
      {
        string str = string.Empty;
        foreach (EdictCard card in edictTogglePanel.m_cards)
        {
          Edict edict = card.Edict;
          Edict.EdictEnableCheckResult enableCheckResult = edict.CanBeEnabled();
          if (!edict.IsEnabled && card.IsSelected && !enableCheckResult.CanBeEnabled)
            str = enableCheckResult.Explanation;
        }
        return str;
      })).Do((Action<string>) (errorMaybe =>
      {
        warningIcon.SetVisibility<IconContainer>(errorMaybe.IsNotEmpty());
        if (!errorMaybe.IsNotEmpty())
          return;
        warningTxt.SetText(errorMaybe);
      }));
      IconContainer errorIcon = builder.NewIconContainer("Warning").SetIcon("Assets/Unity/UserInterface/General/Warning128.png", builder.Style.Global.DangerClr).PutToLeftTopOf<IconContainer>((IUiElement) this.m_container, new Vector2(24f, 30f), Offset.Left((float) (order * 50 + 10)));
      Tooltip errorTxt = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) errorIcon);
      updaterBuilder.Observe<string>((Func<string>) (() =>
      {
        string str = string.Empty;
        foreach (EdictCard card in edictTogglePanel.m_cards)
        {
          Edict edict = card.Edict;
          if (edict.IsEnabled && !edict.IsActive)
            str = edict.LastReasonForNotBeingActive;
        }
        return str;
      })).Do((Action<string>) (errorMaybe =>
      {
        errorIcon.SetVisibility<IconContainer>(errorMaybe.IsNotEmpty());
        if (!errorMaybe.IsNotEmpty())
          return;
        errorTxt.SetText(errorMaybe);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    private void onEdictCardHeaderClick(EdictCard edictCard)
    {
      foreach (EdictCard card in this.m_cards)
      {
        card.SetSelected(card == edictCard);
        card.SetBottomPartVisibility(card == edictCard);
      }
    }
  }
}
