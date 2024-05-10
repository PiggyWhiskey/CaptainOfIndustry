// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.PowerGenerationPriorityPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  public class PowerGenerationPriorityPanel : BasePriorityPanel
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly Func<IElectricityGeneratingEntity> m_generatorProvider;
    private static string ICON_PATH;

    public PowerGenerationPriorityPanel(
      IUiElement parent,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      Func<IElectricityGeneratingEntity> generatorProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(parent, builder, 14, Tr.Production.TranslatedString);
      this.m_inputScheduler = inputScheduler;
      this.m_generatorProvider = generatorProvider;
      this.SetTooltip((LocStrFormatted) Tr.PowerGenerationPriorityTooltip);
      this.SetCustomIcon(PowerGenerationPriorityPanel.ICON_PATH);
      Panel panel = builder.NewPanel("Options");
      SwitchBtn backUpMode = builder.NewSwitchBtn().SetText((LocStrFormatted) Tr.ProvideSurplusPower__Toggle).AddTooltip((LocStrFormatted) Tr.ProvideSurplusPower__Tooltip).SetOnToggleAction((Action<bool>) (isOn => this.m_inputScheduler.ScheduleInputCmd<SetIsElectricitySurplusGeneratorCmd>(new SetIsElectricitySurplusGeneratorCmd(this.m_generatorProvider(), isOn)))).PutTo<SwitchBtn>((IUiElement) panel);
      panel.SetSize<Panel>(new Vector2(backUpMode.GetWidth() + 10f, 25f));
      this.AddOptionsPanel((IUiElement) panel);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_generatorProvider().ElectricityGenerator.IsSurplusGenerator)).Do((Action<bool>) (isBackup =>
      {
        backUpMode.SetIsOn(isBackup);
        this.SetHasOptionsSet(isBackup);
      }));
      this.Updater.AddChildUpdater(updaterBuilder.Build());
    }

    public PowerGenerationPriorityPanel(
      IUiElement parent,
      UiBuilder builder,
      int priority,
      bool isSurplusGenerator)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(parent, builder, 14, Tr.Production.TranslatedString);
      this.SetTooltip((LocStrFormatted) Tr.PowerGenerationPriorityTooltip);
      this.SetCustomIcon(PowerGenerationPriorityPanel.ICON_PATH);
      Panel panel = builder.NewPanel("Options").SetSize<Panel>(new Vector2(220f, 25f));
      SwitchBtn switchBtn = builder.NewSwitchBtn().SetText((LocStrFormatted) Tr.ProvideSurplusPower__Toggle).AddTooltip((LocStrFormatted) Tr.ProvideSurplusPower__Tooltip).PutTo<SwitchBtn>((IUiElement) panel);
      this.AddOptionsPanel((IUiElement) panel);
      switchBtn.SetIsEnabled(false);
      switchBtn.SetIsOn(isSurplusGenerator);
      this.SetHasOptionsSet(isSurplusGenerator);
      this.SetPriorityWithoutNotify(priority);
      this.SetReadonly();
    }

    protected override void OnValueChanged(int index)
    {
      this.m_inputScheduler.ScheduleInputCmd<SetElectricityGenerationPriorityCmd>(new SetElectricityGenerationPriorityCmd(this.m_generatorProvider().Id, index));
    }

    protected override int GetCurrentPriority()
    {
      return this.m_generatorProvider().ElectricityGenerator.GenerationPriority;
    }

    protected override bool IsPrioritySupported() => true;

    static PowerGenerationPriorityPanel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      PowerGenerationPriorityPanel.ICON_PATH = "Assets/Base/Products/Icons/Electricity.svg";
    }
  }
}
