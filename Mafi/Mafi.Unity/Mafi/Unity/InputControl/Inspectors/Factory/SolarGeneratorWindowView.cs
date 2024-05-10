// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.SolarGeneratorWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Prototypes.Machines;
using Mafi.Core.Environment;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  internal class SolarGeneratorWindowView : StaticEntityInspectorBase<SolarElectricityGenerator>
  {
    private readonly SolarGeneratorInspector m_controller;
    private readonly IWeatherManager m_weatherManager;
    private readonly ISolarPanelsManager m_solarPanelsManager;

    protected override SolarElectricityGenerator Entity => this.m_controller.SelectedEntity;

    public SolarGeneratorWindowView(
      SolarGeneratorInspector controller,
      IWeatherManager weatherManager,
      ISolarPanelsManager solarPanelsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller;
      this.m_weatherManager = weatherManager;
      this.m_solarPanelsManager = solarPanelsManager;
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddMaxElectricityOutputPanel(updaterBuilder, (Func<Electricity>) (() => this.m_solarPanelsManager.GetMaxPowerOutputFor((ISolarPanelEntity) this.Entity)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Production);
      Panel parent = this.AddOverlayPanel(itemContainer);
      TextWithIcon producedPerMonth = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TitleBig).SetIcon("Assets/Unity/UserInterface/General/Electricity.svg").PutToLeftOf<TextWithIcon>((IUiElement) parent, 0.0f, Offset.Left(20f));
      producedPerMonth.Icon.SetColor((ColorRgba) 16763686);
      updaterBuilder.Observe<Electricity>((Func<Electricity>) (() => this.m_solarPanelsManager.GetCurrentPowerOutputFor((ISolarPanelEntity) this.Entity))).Observe<Electricity>((Func<Electricity>) (() => this.m_solarPanelsManager.GetMaxPowerOutputFor((ISolarPanelEntity) this.Entity))).Do((Action<Electricity, Electricity>) ((powerOut, maxCapacity) => producedPerMonth.SetSuffixText(string.Format("{0} / {1}", (object) powerOut.Value, (object) maxCapacity.Format()) + string.Format("  -  {0}", (object) this.m_weatherManager.CurrentWeather.Strings.Name))));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
