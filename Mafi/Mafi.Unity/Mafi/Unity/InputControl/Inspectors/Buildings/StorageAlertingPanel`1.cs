// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.StorageAlertingPanel`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  public class StorageAlertingPanel<T> : IUiElementWithUpdater, IUiElement where T : IEntityWithStorageAlert
  {
    private readonly Panel m_container;
    private readonly Panel m_innerContainer;
    private SwitchBtn m_aboveToggle;
    private SwitchBtn m_belowToggle;
    private readonly IInputScheduler m_inputScheduler;
    private readonly Func<T> m_entityProvider;
    private readonly Btn m_toggleOpenBtn;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public StorageAlertingPanel(
      IUiElement parent,
      UiBuilder builder,
      IInputScheduler inputScheduler,
      Func<T> entityProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      StorageAlertingPanel<T> storageAlertingPanel = this;
      this.m_inputScheduler = inputScheduler;
      this.m_entityProvider = entityProvider;
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_container = builder.NewPanel("AlertPanel", parent);
      this.m_toggleOpenBtn = builder.NewBtn("TogglePanelBtn").SetButtonStyle(builder.Style.Global.GeneralBtnToToggle).SetIcon("Assets/Unity/UserInterface/General/Bell128.png", 16.Vector2()).SetText((LocStrFormatted) Tr.StorageAlert__BtnTitle).OnClick(new Action(this.onToggleOpenClick));
      this.m_toggleOpenBtn.PutToLeftTopOf<Btn>((IUiElement) this.m_container, new Vector2(this.m_toggleOpenBtn.GetOptimalWidth(), 25f), Offset.Left((float) (-(double) this.m_toggleOpenBtn.GetOptimalWidth() - 10.0)));
      updaterBuilder.Observe<bool>((Func<bool>) (() =>
      {
        T obj = storageAlertingPanel.m_entityProvider();
        return obj is IEntityWithAlertAbove entityWithAlertAbove2 && entityWithAlertAbove2.AlertWhenAboveEnabled || obj is IEntityWithAlertBelow entityWithAlertBelow2 && entityWithAlertBelow2.AlertWhenBelowEnabled;
      })).Do((Action<bool>) (alertsEnabled =>
      {
        if (alertsEnabled)
          storageAlertingPanel.m_toggleOpenBtn.SetButtonStyle(builder.Style.Global.GeneralBtnActive);
        else
          storageAlertingPanel.m_toggleOpenBtn.SetButtonStyle(builder.Style.Global.GeneralBtnToToggle);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => storageAlertingPanel.m_entityProvider().AreAlertsAvailable)).Do((Action<bool>) (hasStoredProduct => storageAlertingPanel.m_toggleOpenBtn.SetVisibility<Btn>(hasStoredProduct)));
      this.m_innerContainer = builder.NewPanel("InnerContent").SetBorderStyle(new BorderStyle(ColorRgba.Black)).SetBackground((ColorRgba) 3815994).PutTo<Panel>((IUiElement) this.m_container, Offset.Top(30f));
      List<Percent> options1 = new List<Percent>()
      {
        0.Percent(),
        25.Percent(),
        50.Percent(),
        75.Percent()
      };
      List<Percent> options2 = new List<Percent>()
      {
        25.Percent(),
        50.Percent(),
        75.Percent(),
        100.Percent()
      };
      bool flag1 = typeof (T).IsAssignableTo(typeof (IEntityWithAlertAbove));
      bool flag2 = typeof (T).IsAssignableTo(typeof (IEntityWithAlertBelow));
      Assert.That<bool>(flag1 | flag2).IsTrue();
      if (flag2)
        addAlertRow(options1, false);
      if (flag1)
        addAlertRow(options2, true);
      this.SetHeight<StorageAlertingPanel<T>>(110f);
      this.Updater = updaterBuilder.Build();
      this.HidePanel();

      void addAlertRow(List<Percent> options, bool isAbove)
      {
        List<string> options1 = new List<string>();
        foreach (Percent option in options)
        {
          if (option.IsZero)
            options1.Add(Tr.StorageAlert__Empty.TranslatedString);
          else if (option.IsNearHundred)
            options1.Add(Tr.StorageAlert__Full.TranslatedString);
          else if (isAbove)
            options1.Add(string.Format("> {0}", (object) option));
          else
            options1.Add(string.Format("< {0}", (object) option));
        }
        Offset offset = isAbove ? Offset.Top(38f) : Offset.Top(2f);
        Panel topOf = builder.NewPanel("Row").PutToTopOf<Panel>((IUiElement) storageAlertingPanel.m_innerContainer, 40f, Offset.Left(10f) + offset);
        SwitchBtn switchBtn = builder.NewSwitchBtn().SetText(Tr.StorageAlert__Prefix.TranslatedString).SetOnToggleAction((Action<bool>) (isEnabled => storageAlertingPanel.onAlertToggle(isEnabled, isAbove))).PutToLeftOf<SwitchBtn>((IUiElement) topOf, 60f);
        Dropdwn dropdown = builder.NewDropdown("Dropdown", (IUiElement) topOf).AddOptions(options1).PutToLeftMiddleOf<Dropdwn>((IUiElement) topOf, new Vector2(120f, (float) Dropdwn.HEIGHT), Offset.Left(10f + switchBtn.GetWidth()));
        dropdown.OnValueChange(new Action<int>(onValueChangedInternal));
        storageAlertingPanel.m_container.SetWidth<Panel>((float) ((double) switchBtn.GetWidth() + 30.0 + 120.0));
        if (isAbove)
        {
          storageAlertingPanel.m_aboveToggle = switchBtn;
          updaterBuilder.Observe<bool>((Func<bool>) (() => ((IEntityWithAlertAbove) (object) storageAlertingPanel.m_entityProvider()).AlertWhenAboveEnabled)).Observe<Percent>((Func<Percent>) (() => ((IEntityWithAlertAbove) (object) storageAlertingPanel.m_entityProvider()).AlertWhenAbove)).Do((Action<bool, Percent>) ((enabled, value) =>
          {
            switchBtn.SetIsOn(enabled);
            int itemIndex = options.IndexOf(value);
            if (itemIndex < 0)
              return;
            dropdown.SetValueWithoutNotify(itemIndex);
          }));
        }
        else
        {
          storageAlertingPanel.m_belowToggle = switchBtn;
          updaterBuilder.Observe<bool>((Func<bool>) (() => ((IEntityWithAlertBelow) (object) storageAlertingPanel.m_entityProvider()).AlertWhenBelowEnabled)).Observe<Percent>((Func<Percent>) (() => ((IEntityWithAlertBelow) (object) storageAlertingPanel.m_entityProvider()).AlertWhenBelow)).Do((Action<bool, Percent>) ((enabled, value) =>
          {
            switchBtn.SetIsOn(enabled);
            int itemIndex = options.IndexOf(value);
            if (itemIndex < 0)
              return;
            dropdown.SetValueWithoutNotify(itemIndex);
          }));
        }

        void onValueChangedInternal(int index)
        {
          inputScheduler.ScheduleInputCmd<StorageAlertSetThresholdCmd>(new StorageAlertSetThresholdCmd((IEntity) entityProvider(), options[index], isAbove));
        }
      }
    }

    public float GetToggleBtnWidth() => this.m_toggleOpenBtn.GetWidth();

    private void onToggleOpenClick()
    {
      this.m_innerContainer.SetVisibility<Panel>(!this.m_innerContainer.IsVisible());
    }

    public void HidePanel() => this.m_innerContainer.Hide<Panel>();

    private void onAlertToggle(bool isEnabled, bool isAbove)
    {
      if (isAbove)
        this.m_aboveToggle.SetIsOn(isEnabled);
      else
        this.m_belowToggle.SetIsOn(isEnabled);
      this.m_inputScheduler.ScheduleInputCmd<StorageAlertSetEnabledCmd>(new StorageAlertSetEnabledCmd((IEntity) this.m_entityProvider(), isEnabled, isAbove));
    }
  }
}
