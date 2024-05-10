// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.ResourceStatView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  internal class ResourceStatView : IUiElement
  {
    public const int HEIGHT = 60;
    private static readonly ColorRgba UtilBarClr;
    private static readonly ColorRgba GlobalBarDemandClrs;
    private static readonly ColorRgba ConsumedClr;
    private static readonly ColorRgba ProducedClr;
    private static readonly ColorRgba ProducedAndWastedClr;
    private readonly Panel m_container;
    private readonly Txt m_protoName;
    private IEntityProto m_proto;
    private readonly IconContainer m_icon;
    private readonly DualBar m_globalRatioBar;
    private readonly QuantityBar m_localRatioBar;
    private readonly Txt m_count;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ResourceStatView(
      UiBuilder builder,
      bool isConsumption,
      int ratioBarWidth = 150,
      Action<IEntityProto, bool> openStatsAction = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ResourceStatView resourceStatView = this;
      this.m_container = builder.NewPanel("ConsumerView").SetBackground(builder.Style.Panel.ItemOverlay);
      int size1 = 50;
      int size2 = openStatsAction != null ? 30 : 0;
      int rightOffset = 30;
      this.m_icon = builder.NewIconContainer("Icon", (IUiElement) this.m_container).PutToLeftOf<IconContainer>((IUiElement) this.m_container, (float) size1, Offset.Left(10f));
      this.m_protoName = builder.NewTitle("Title", (IUiElement) this.m_container).PutToLeftTopOf<Txt>((IUiElement) this.m_container, new Vector2(200f, 25f), Offset.Left(this.m_icon.GetWidth() + 20f));
      Panel parent = builder.NewPanel("Holder", (IUiElement) this.m_container).PutTo<Panel>((IUiElement) this.m_container, Offset.Top(25f) + Offset.Bottom(5f) + Offset.Left(this.m_icon.GetWidth() + 20f) + Offset.Right(10f));
      Panel panel = builder.NewPanel("CountContainer", (IUiElement) parent).SetBackground(builder.Style.QuantityBar.BackgroundColor).SetBorderStyle(new BorderStyle(ColorRgba.Black));
      panel.PutToLeftOf<Panel>((IUiElement) parent, 60f);
      this.m_count = builder.NewTitle("Title", (IUiElement) panel).SetTextStyle(builder.Style.Global.TextBigBold).SetAlignment(TextAnchor.MiddleRight).PutTo<Txt>((IUiElement) panel, Offset.Right(5f));
      this.m_globalRatioBar = new DualBar(builder, (IUiElement) parent).SetInnerBarColor(isConsumption ? ResourceStatView.ConsumedClr : ResourceStatView.ProducedClr).SetOuterBarColor(isConsumption ? ResourceStatView.GlobalBarDemandClrs : ResourceStatView.ProducedAndWastedClr).PutTo<DualBar>((IUiElement) parent, Offset.Left(panel.GetWidth() + 10f) + Offset.Right((float) (ratioBarWidth + size2 + rightOffset + 20)));
      this.m_globalRatioBar.AlignTextToLeft(Offset.Left(10f));
      this.m_localRatioBar = new QuantityBar(builder).SetColor(ResourceStatView.UtilBarClr).PutToRightOf<QuantityBar>((IUiElement) parent, (float) ratioBarWidth, Offset.Right((float) (size2 + rightOffset + 10)));
      if (openStatsAction == null)
        return;
      builder.NewBtnGeneral("OpenStats").SetIcon("Assets/Unity/UserInterface/Toolbar/Stats.svg").PutToRightOf<Btn>((IUiElement) parent, (float) size2, Offset.Right((float) rightOffset)).OnClick((Action) (() => openStatsAction(resourceStatView.m_proto, isConsumption)));
    }

    public void SetData(ElectricityManager.ConsumptionPerProto stats, Electricity maxDemand)
    {
      this.setProto(stats.ConsumerProto, stats.EntitiesTotal);
      Electricity consumed = stats.LastTick.Consumed;
      Electricity demand = stats.LastTick.Demand;
      this.updateGlobalBar(consumed.Value, demand.Value, maxDemand.Value, !consumed.IsZero || !demand.IsPositive ? consumed.Format().Value : string.Format("({0})", (object) demand.Format()));
      Electricity possibleConsumption = stats.LastTick.MaxPossibleConsumption;
      this.updateRelativeBar(consumed.Value, possibleConsumption.Value, string.Format("{0} / {1}", (object) this.currentToStringAvoidUnitsIfCan(consumed, possibleConsumption), (object) possibleConsumption.Format()));
    }

    public void SetData(
      ElectricityManager.ProductionPerProto stats,
      Electricity maxProductionPlusWasted)
    {
      this.setProto(stats.ProducerProto, stats.EntitiesTotal);
      Electricity produced = stats.LastTick.Produced;
      this.updateGlobalBar(produced.Value, stats.LastTick.ProduceAndWasted.Value, maxProductionPlusWasted.Value, produced.Format().Value);
      Electricity generationCapacity = stats.LastTick.MaxGenerationCapacity;
      this.updateRelativeBar(produced.Value, generationCapacity.Value, string.Format("{0} / {1}", (object) this.currentToStringAvoidUnitsIfCan(produced, generationCapacity), (object) generationCapacity.Format()));
    }

    public void SetData(ComputingManager.ConsumptionPerProto stats, Computing maxDemand)
    {
      this.setProto(stats.ConsumerProto, stats.EntitiesTotal);
      Computing consumed = stats.LastTick.Consumed;
      Computing demand = stats.LastTick.Demand;
      this.updateGlobalBar(consumed.Value, demand.Value, maxDemand.Value, !consumed.IsZero || !demand.IsPositive ? consumed.Format().Value : string.Format("({0})", (object) demand.Format()));
      Computing possibleConsumption = stats.LastTick.MaxPossibleConsumption;
      this.updateRelativeBar(consumed.Value, possibleConsumption.Value, string.Format("{0} / {1}", (object) this.currentToStringAvoidUnitsIfCan(consumed, possibleConsumption), (object) possibleConsumption.Format()));
    }

    public void SetData(ComputingManager.ProductionPerProto stats, Computing maxProduction)
    {
      this.setProto(stats.ProducerProto, stats.EntitiesTotal);
      Computing produced = stats.LastTick.Produced;
      this.updateGlobalBar(produced.Value, produced.Value, maxProduction.Value, produced.Format().Value);
      Computing generationCapacity = stats.LastTick.MaxGenerationCapacity;
      this.updateRelativeBar(produced.Value, generationCapacity.Value, string.Format("{0} / {1}", (object) this.currentToStringAvoidUnitsIfCan(produced, generationCapacity), (object) generationCapacity.Format()));
    }

    public void SetData(MaintenanceManager.ConsumptionPerProto stats, PartialQuantity maxDemand)
    {
      this.setProto(stats.Proto, stats.EntitiesTotal);
      PartialQuantity demand = stats.LastTick.Demand;
      int num = demand.ToQuantityRounded().Value;
      int max = maxDemand.ToQuantityRounded().Value;
      this.updateGlobalBar(num, num, max, demand.ToStringRounded(1));
      PartialQuantity possibleConsumption = stats.LastTick.MaxPossibleConsumption;
      this.updateRelativeBar(num, possibleConsumption.ToQuantityRounded().Value, demand.ToStringRounded(1) + " / " + possibleConsumption.ToStringRounded(1));
    }

    public void SetData(WorkersManager.WorkersStatsPerProto stats, int maxDemand)
    {
      this.setProto(stats.Proto, stats.EntitiesTotal);
      int workersAssigned = stats.WorkersAssigned;
      int workersNeeded = stats.WorkersNeeded;
      this.updateGlobalBar(workersAssigned, workersNeeded, maxDemand, workersAssigned != 0 || workersNeeded <= 0 ? workersAssigned.ToStringCached() : "(" + workersNeeded.ToStringCached() + ")");
      this.updateRelativeBar(workersAssigned, workersNeeded, string.Format("{0} / {1}", (object) workersAssigned, (object) workersNeeded));
    }

    private string currentToStringAvoidUnitsIfCan(Electricity current, Electricity max)
    {
      return current.IsPositive && current.Value < 1000 && max.Value >= 1000 ? current.Format().Value : current.FormatNoUnits().Value;
    }

    private string currentToStringAvoidUnitsIfCan(Computing current, Computing max)
    {
      return current.IsPositive && current.Value < 1000 && max.Value >= 1000 ? current.Format().Value : current.FormatNoUnits().Value;
    }

    private void setProto(IEntityProto proto, int count)
    {
      this.m_proto = proto;
      this.m_count.SetText(count.ToStringCached());
      this.m_protoName.SetText((LocStrFormatted) this.m_proto.Strings.Name);
      if (!(this.m_proto is IProtoWithIcon proto1))
        return;
      this.m_icon.SetIcon(proto1.IconPath);
    }

    private void updateGlobalBar(int currentInner, int currentOuter, int max, string formattedStr)
    {
      Percent percentOuter = max > 0 ? Percent.FromRatio(currentOuter, max) : Percent.Zero;
      Percent percentInner = max > 0 ? Percent.FromRatio(currentInner, max) : Percent.Zero;
      if (currentInner > 0 && percentInner < 2.Percent())
        percentInner = 2.Percent();
      if (currentOuter > 0 && percentOuter < 2.Percent())
        percentOuter = 2.Percent();
      this.m_globalRatioBar.UpdateValues(percentInner, percentOuter, formattedStr);
    }

    private void updateRelativeBar(int current, int max, string formattedStr)
    {
      this.m_localRatioBar.UpdateValues(max > 0 ? Percent.FromRatio(current, max) : Percent.Zero, formattedStr);
    }

    static ResourceStatView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ResourceStatView.UtilBarClr = (ColorRgba) 2711155;
      ResourceStatView.GlobalBarDemandClrs = (ColorRgba) 10438969;
      ResourceStatView.ConsumedClr = (ColorRgba) 8287041;
      ResourceStatView.ProducedClr = (ColorRgba) 4750913;
      ResourceStatView.ProducedAndWastedClr = (ColorRgba) 8883816;
    }
  }
}
