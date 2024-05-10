// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.RainwaterHarvesterWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.RainwaterHarvesters;
using Mafi.Core.Entities;
using Mafi.Core.Environment;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class RainwaterHarvesterWindowView : StaticEntityInspectorBase<RainwaterHarvester>
  {
    private readonly RainwaterHarvesterInspector m_controller;
    private readonly WeatherManager m_weatherManager;

    protected override RainwaterHarvester Entity => this.m_controller.SelectedEntity;

    public RainwaterHarvesterWindowView(
      RainwaterHarvesterInspector controller,
      WeatherManager weatherManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_weatherManager = weatherManager;
      this.m_controller = controller.CheckNotNull<RainwaterHarvesterInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddStorageLogisticsPanel(updaterBuilder, (Func<IEntityWithSimpleLogisticsControl>) (() => (IEntityWithSimpleLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.EstimatedWaterYieldTitle);
      Panel parent = this.AddOverlayPanel(this.ItemsContainer);
      TextWithIcon estimateTxt = new TextWithIcon(this.Builder).SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").SetSuffixText("/ 60").PutToLeftOf<TextWithIcon>((IUiElement) parent, 0.0f, Offset.Left(20f));
      updaterBuilder.Observe<FluidProductProto>((Func<FluidProductProto>) (() => this.Entity.Prototype.WaterProto)).Observe<PartialQuantity>((Func<PartialQuantity>) (() => this.m_weatherManager.GetSumOfRainyFortnights().Apply(14) * this.Entity.WaterCollectedPerDayFullRain / 12)).Do((Action<FluidProductProto, PartialQuantity>) ((waterProto, avgCollectionPerMonth) =>
      {
        estimateTxt.SetPrefixText(avgCollectionPerMonth.ToStringRounded(1));
        estimateTxt.SetIcon(waterProto.Graphics.IconPath);
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.WaterCollected);
      BufferView buffer = this.Builder.NewBufferView((IUiElement) itemContainer).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      updaterBuilder.Observe<FluidProductProto>((Func<FluidProductProto>) (() => this.Entity.Prototype.WaterProto)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.StoredWater)).Do((Action<FluidProductProto, Quantity, Quantity>) ((product, capacity, quantity) => buffer.UpdateState((ProductProto) product, capacity, quantity)));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
