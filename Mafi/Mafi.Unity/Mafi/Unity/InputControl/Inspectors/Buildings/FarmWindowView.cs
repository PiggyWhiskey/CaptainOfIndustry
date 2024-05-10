// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.FarmWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class FarmWindowView : StaticEntityInspectorBase<Farm>
  {
    private readonly FarmInspector m_controller;
    private readonly IPropertiesDb m_propsDb;
    private StatusPanel m_statusInfo;
    private CropPicker m_cropPicker;
    private FertilizersOverview m_fertilizersOverview;
    private LastCropStats m_lastCropStats;
    private Option<Crop> m_lastCrop;
    private readonly TechnologyProto m_cropRotationTech;
    private int m_lastSlotIndex;
    private Panel m_windowOverlay;

    protected override Farm Entity => this.m_controller.SelectedEntity;

    private bool AnyFertilizerUnlocked { get; set; }

    public FarmWindowView(FarmInspector controller, IPropertiesDb propsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      FarmWindowView farmWindowView = this;
      this.m_propsDb = propsDb;
      this.m_controller = controller.CheckNotNull<FarmInspector>();
      this.m_cropRotationTech = controller.Context.ProtosDb.GetOrThrow<TechnologyProto>(IdsCore.Technology.CropRotation);
      ImmutableArray<ProductProto> allFertilizers = controller.Context.ProtosDb.All<ProductProto>().Where<ProductProto>((Func<ProductProto, bool>) (x => x.TryGetParam<FertilizerProductParam>(out FertilizerProductParam _))).ToImmutableArray<ProductProto>();
      this.AnyFertilizerUnlocked = allFertilizers.Any(new Func<ProductProto, bool>(this.m_controller.Context.UnlockedProtosDbForUi.IsUnlocked));
      if (this.AnyFertilizerUnlocked)
        return;
      this.m_controller.Context.UnlockedProtosDbForUi.OnUnlockedSetChangedForUi += new Action(unlockedCFertilizersCheck);

      void unlockedCFertilizersCheck()
      {
        farmWindowView.AnyFertilizerUnlocked = allFertilizers.Any(new Func<ProductProto, bool>(farmWindowView.m_controller.Context.UnlockedProtosDbForUi.IsUnlocked));
        if (!farmWindowView.AnyFertilizerUnlocked)
          return;
        farmWindowView.m_controller.Context.UnlockedProtosDbForUi.OnUnlockedSetChangedForUi -= new Action(unlockedCFertilizersCheck);
      }
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.MakeScrollableWithHeightLimit();
      base.AddCustomItems(itemContainer);
      this.SetWidth(660f);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_statusInfo = this.AddStatusInfoPanel();
      StackContainer parent1 = this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      TextWithIcon textWithIcon = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.Text).SetPrefixText(Tr.MonthDurationLegend.Format(60.ToString()).Value).SetIcon("Assets/Unity/UserInterface/General/Clock.svg");
      textWithIcon.PutToLeftMiddleOf<TextWithIcon>((IUiElement) parent1, new Vector2(textWithIcon.GetWidth(), 20f), Offset.Left((float) (660.0 - (double) textWithIcon.GetWidth() - 20.0)));
      this.m_windowOverlay = this.Builder.NewPanel("Overlay").SetBackground(new ColorRgba(4408131, 110)).PutTo<Panel>((IUiElement) this.GetContentPanel()).Hide<Panel>();
      this.m_cropPicker = new CropPicker(this.m_controller.Context.UnlockedProtosDbForUi, this.m_controller.Context.ProtosDb, this.m_propsDb, (IUiElement) this.m_windowOverlay, new Action<Option<CropProto>>(this.onCropPicked));
      this.m_cropPicker.SetOnCloseButtonClickAction((Action) (() =>
      {
        this.m_cropPicker.Hide();
        this.m_windowOverlay.Hide<Panel>();
      }));
      this.m_cropPicker.BuildUi(this.Builder);
      this.m_fertilizersOverview = new FertilizersOverview(this.m_controller.Context.ProtosDb);
      this.m_fertilizersOverview.SetOnCloseButtonClickAction((Action) (() =>
      {
        this.m_fertilizersOverview.Hide();
        this.m_windowOverlay.Hide<Panel>();
      }));
      this.m_fertilizersOverview.BuildUi(this.Builder);
      this.m_lastCropStats = new LastCropStats();
      this.m_lastCropStats.SetOnCloseButtonClickAction((Action) (() =>
      {
        this.m_lastCropStats.Hide();
        this.m_windowOverlay.Hide<Panel>();
      }));
      this.m_lastCropStats.BuildUi(this.Builder);
      Panel scheduleHolder = this.Builder.NewPanel("ScheduleHolder").AppendTo<Panel>(itemContainer, new float?(65f + this.Builder.Style.Panel.LineHeight));
      Txt cropScheduleTitle = this.Builder.CreateSectionTitle((IUiElement) scheduleHolder, (LocStrFormatted) Tr.CropSchedule, new LocStrFormatted?((LocStrFormatted) Tr.CropSchedule__Tooltip));
      cropScheduleTitle.PutToTopOf<Txt>((IUiElement) scheduleHolder, this.Builder.Style.Panel.LineHeight, Offset.Left(this.Builder.Style.Panel.Padding) + Offset.Top(5f));
      CropScheduleView cropSchedule = new CropScheduleView(this.Builder, this.m_controller.InputScheduler, (Func<Farm>) (() => this.Entity), new Action<int>(this.showCropPicker));
      cropSchedule.PutToLeftBottomOf<CropScheduleView>((IUiElement) scheduleHolder, cropSchedule.GetSize(), Offset.Left(15f));
      this.AddUpdater(cropSchedule.Updater);
      float avgDataOffset = cropSchedule.GetWidth() + 30f;
      TextWithIcon avgProductionTitle = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.Title).SetIcon("Assets/Unity/UserInterface/General/Clock.svg");
      avgProductionTitle.SetPrefixText(Tr.AverageProduction.TranslatedString + " / 60");
      Panel panel = this.Builder.NewPanel("Help");
      ColorRgba? nullable1 = new ColorRgba?();
      ColorRgba? color1 = nullable1;
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) panel.SetBackground("Assets/Unity/UserInterface/General/Info128.png", color1).PutToLeftMiddleOf<Panel>((IUiElement) avgProductionTitle, 16.Vector2(), Offset.Left(-20f))).SetText((LocStrFormatted) Tr.FarmAvgProduction__Tooltip);
      StackContainer yieldInfoContainer = this.Builder.NewStackContainer("AvgYields").SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(5f).SetSizeMode(StackContainer.SizeMode.Dynamic);
      int? nullable2;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        Btn btn = this.Builder.NewBtn("MonthlyProduction");
        nullable1 = new ColorRgba?(this.Builder.Style.Global.ControlsBgColor);
        TextStyle title = this.Builder.Style.Global.Title;
        ref TextStyle local = ref title;
        ColorRgba? color2 = new ColorRgba?(ColorRgba.White);
        FontStyle? fontStyle = new FontStyle?();
        nullable2 = new int?();
        int? fontSize = nullable2;
        bool? isCapitalized = new bool?();
        TextStyle? text = new TextStyle?(local.Extend(color2, fontStyle, fontSize, isCapitalized));
        ColorRgba? nullable3 = new ColorRgba?(ColorRgba.White);
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = nullable1;
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredMaskClr = new ColorRgba?();
        ColorRgba? pressedMaskClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = nullable3;
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        Offset? iconPadding = new Offset?();
        BtnStyle buttonStyle = new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, true, iconPadding: iconPadding);
        Btn monthlyStatus = btn.SetButtonStyle(buttonStyle).SetText("").SetEnabled(false).AppendTo<Btn>(yieldInfoContainer, new float?(0.0f));
        IconContainer iconContainer = this.Builder.NewIconContainer("Icon");
        monthlyStatus.SetIcon(iconContainer, new Vector2?(28.Vector2()));
        int index = index1;
        updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => this.Entity.AvgYieldPerYear[index])).Do((Action<ProductQuantity>) (pq =>
        {
          yieldInfoContainer.SetItemVisibility((IUiElement) monthlyStatus, pq.IsNotEmpty);
          if (!pq.IsNotEmpty)
            return;
          iconContainer.SetIcon(pq.Product.Graphics.IconPath);
          monthlyStatus.SetText((pq.Quantity.Value.ToFix32() / 12).ToStringRounded(1));
          yieldInfoContainer.UpdateItemWidth((IUiElement) monthlyStatus, monthlyStatus.GetOptimalWidth());
        }));
      }
      Txt clickToStartTxt = this.Builder.NewTxt("Text").SetText(string.Format("<- {0}", (object) Tr.FarmPlantCropHelp)).SetTextStyle(this.Builder.Style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleLeft).PutToLeftTopOf<Txt>((IUiElement) cropSchedule, new Vector2(200f, 20f), Offset.Left(avgDataOffset) + Offset.Top(10f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.AvgYieldPerYear.First.IsNotEmpty)).Do((Action<bool>) (hasEstimates =>
      {
        avgProductionTitle.SetVisibility<TextWithIcon>(hasEstimates);
        clickToStartTxt.SetVisibility<Txt>(!hasEstimates);
      }));
      if (this.m_controller.Context.UnlockedProtosDbForUi.IsUnlocked((IProto) this.m_cropRotationTech))
      {
        positionSchedule(true);
      }
      else
      {
        positionSchedule(false);
        this.m_controller.Context.UnlockedProtosDbForUi.OnUnlockedSetChangedForUi += new Action(onProtoUnlocked);
      }
      Txt activeCropTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Farm_PlantedCrop);
      CropView activeCropView = new CropView(this.Builder, this.m_controller.Context.ProtosDb, this.m_propsDb, alignLeft: true).AppendTo<CropView>(itemContainer);
      Panel increaseFertilityWarning = this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?(this.Style.Global.OrangeText)).PutToLeftMiddleOf<Panel>((IUiElement) activeCropTitle, 16.Vector2(), Offset.Left(activeCropTitle.GetPreferedWidth() + 4f)).Hide<Panel>();
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) increaseFertilityWarning).SetText((LocStrFormatted) Tr.FarmFertilityPenaltyNoRotation);
      int leftOffset1 = 350;
      Txt txt1 = this.Builder.NewTxt("Text");
      TextStyle text1 = this.Builder.Style.Global.Text;
      ref TextStyle local1 = ref text1;
      nullable2 = new int?(13);
      ColorRgba? color3 = new ColorRgba?(this.Builder.Style.Global.GreenForDark);
      FontStyle? fontStyle1 = new FontStyle?();
      int? fontSize1 = nullable2;
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local1.Extend(color3, fontStyle1, fontSize1, isCapitalized1);
      Txt timeLeftTxt = txt1.SetTextStyle(textStyle1).SetAlignment(TextAnchor.MiddleLeft).PutToLeftTopOf<Txt>((IUiElement) activeCropView, new Vector2((float) (660 - leftOffset1), 20f), Offset.Top(10f) + Offset.Left((float) leftOffset1));
      Panel timeLeftHelp = this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?(this.Builder.Style.Global.GreenForDark)).PutToRightOf<Panel>((IUiElement) timeLeftTxt, 0.0f);
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) timeLeftHelp).SetText((LocStrFormatted) Tr.Crop_DurationLeft__Tooltip);
      Txt txt2 = this.Builder.NewTxt("Text");
      TextStyle text2 = this.Builder.Style.Global.Text;
      ref TextStyle local2 = ref text2;
      nullable2 = new int?(13);
      ColorRgba? color4 = new ColorRgba?(this.Builder.Style.Global.OrangeText);
      FontStyle? fontStyle2 = new FontStyle?();
      int? fontSize2 = nullable2;
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local2.Extend(color4, fontStyle2, fontSize2, isCapitalized2);
      Txt overdueTxt = txt2.SetTextStyle(textStyle2).SetAlignment(TextAnchor.MiddleLeft).PutToLeftTopOf<Txt>((IUiElement) activeCropView, new Vector2((float) (660 - leftOffset1), 20f), Offset.Top(40f) + Offset.Left((float) leftOffset1));
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?(this.Builder.Style.Global.OrangeText)).PutToRightOf<Panel>((IUiElement) overdueTxt, 0.0f)).SetText((LocStrFormatted) Tr.CropOverdue__Tooltip);
      Txt txt3 = this.Builder.NewTxt("Text");
      TextStyle text3 = this.Builder.Style.Global.Text;
      ref TextStyle local3 = ref text3;
      nullable2 = new int?(13);
      ColorRgba? color5 = new ColorRgba?(this.Builder.Style.Global.OrangeText);
      FontStyle? fontStyle3 = new FontStyle?();
      int? fontSize3 = nullable2;
      bool? isCapitalized3 = new bool?();
      TextStyle textStyle3 = local3.Extend(color5, fontStyle3, fontSize3, isCapitalized3);
      Txt notStarterTxt = txt3.SetTextStyle(textStyle3).SetAlignment(TextAnchor.MiddleLeft).PutToLeftTopOf<Txt>((IUiElement) activeCropView, new Vector2((float) (660 - leftOffset1), 20f), Offset.Top(10f) + Offset.Left((float) leftOffset1));
      updaterBuilder.Observe<FarmProto>((Func<FarmProto>) (() => this.Entity.Prototype)).Observe<Percent?>((Func<Percent?>) (() => this.Entity.CurrentCrop.ValueOrNull?.ConsumedFertilityPerDay)).Observe<CropProto>((Func<CropProto>) (() => this.Entity.CurrentCrop.ValueOrNull?.Prototype ?? (CropProto) null)).Do((Action<FarmProto, Percent?, CropProto>) ((entityProto, realFertilityConsumed, cropProto) =>
      {
        if ((Proto) cropProto != (Proto) null)
        {
          CropView cropView = activeCropView;
          CropProto crop = cropProto;
          FarmProto farmProto = entityProto;
          Percent? nullable4 = realFertilityConsumed;
          Percent? realFertilityConsumedPerMonth = nullable4.HasValue ? new Percent?(nullable4.GetValueOrDefault() * 30) : new Percent?();
          cropView.SetCrop(crop, farmProto, realFertilityConsumedPerMonth);
        }
        itemContainer.SetItemVisibility((IUiElement) activeCropView, (Proto) cropProto != (Proto) null);
        itemContainer.SetItemVisibility((IUiElement) activeCropTitle, (Proto) cropProto != (Proto) null);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() =>
      {
        Crop valueOrNull = this.Entity.CurrentCrop.ValueOrNull;
        return valueOrNull != null && valueOrNull.HasFertilityPenalty;
      })).Do((Action<bool>) (hasFertilityPenalty => increaseFertilityWarning.SetVisibility<Panel>(hasFertilityPenalty)));
      updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => this.Entity.GetCurrentYieldEstimate())).Observe<bool>((Func<bool>) (() =>
      {
        Crop valueOrNull = this.Entity.CurrentCrop.ValueOrNull;
        return valueOrNull != null && valueOrNull.IsStarted;
      })).Observe<int>((Func<int>) (() =>
      {
        Crop valueOrNull = this.Entity.CurrentCrop.ValueOrNull;
        return valueOrNull == null ? 0 : valueOrNull.DaysRemaining;
      })).Do((Action<ProductQuantity, bool, int>) ((yieldEstimate, isStarted, daysRemaining) =>
      {
        if (yieldEstimate.IsNotEmpty)
        {
          Fix32 fix32 = daysRemaining.Over(30);
          string str = TrCore.NumberOfMonths.Format(fix32.ToStringRounded(1), fix32.ToFix64()).Value;
          timeLeftTxt.SetText(Tr.Crop_DurationLeft.Format(yieldEstimate.Quantity.ToString(), str));
          timeLeftHelp.PutToLeftMiddleOf<Panel>((IUiElement) timeLeftTxt, 18.Vector2(), Offset.Left(timeLeftTxt.GetPreferedWidth() + 5f));
        }
        timeLeftTxt.SetVisibility<Txt>(isStarted && yieldEstimate.IsNotEmpty);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() =>
      {
        Crop valueOrNull = this.Entity.CurrentCrop.ValueOrNull;
        return valueOrNull != null && valueOrNull.IsMissingWater;
      })).Observe<int>((Func<int>) (() =>
      {
        Crop valueOrNull = this.Entity.CurrentCrop.ValueOrNull;
        return valueOrNull == null ? 0 : valueOrNull.DaysMissingWater;
      })).Observe<CropProto>((Func<CropProto>) (() => this.Entity.CurrentCrop.ValueOrNull?.Prototype ?? (CropProto) null)).Do((Action<bool, int, CropProto>) ((isMissingWater, daysMissingWater, cropProto) =>
      {
        if ((Proto) cropProto == (Proto) null)
          return;
        overdueTxt.SetVisibility<Txt>(isMissingWater);
        if (!isMissingWater || !cropProto.DaysToSurviveWithNoWater.HasValue)
          return;
        Fix32 fix32_1 = daysMissingWater.Over(30);
        Fix32 fix32_2 = cropProto.DaysToSurviveWithNoWater.Value.Over(30);
        string str1 = TrCore.NumberOfMonths.Format(fix32_1.ToStringRounded(1), fix32_1.ToFix64()).Value;
        string str2 = TrCore.NumberOfMonths.Format(fix32_2.ToStringRounded(1), fix32_2.ToFix64()).Value;
        overdueTxt.SetText(Tr.CropOverdue.Format(str1, str2));
      }));
      updaterBuilder.Observe<bool?>((Func<bool?>) (() => this.Entity.CurrentCrop.ValueOrNull?.IsStarted)).Observe<Farm.State>((Func<Farm.State>) (() => this.Entity.CurrentState)).Do((Action<bool?, Farm.State>) ((isStarted, state) =>
      {
        bool? nullable5;
        if (isStarted.HasValue)
        {
          nullable5 = isStarted;
          bool flag = false;
          if (nullable5.GetValueOrDefault() == flag & nullable5.HasValue)
          {
            switch (state)
            {
              case Farm.State.NotEnoughWater:
                notStarterTxt.SetText((LocStrFormatted) Tr.CropWaiting__Water);
                break;
              case Farm.State.LowFertility:
                notStarterTxt.SetText((LocStrFormatted) Tr.CropWaiting__Fertility);
                break;
              default:
                notStarterTxt.SetText((LocStrFormatted) Tr.CropWaiting__NoReason);
                break;
            }
          }
        }
        Txt element = notStarterTxt;
        int num;
        if (isStarted.HasValue)
        {
          nullable5 = isStarted;
          bool flag = false;
          num = nullable5.GetValueOrDefault() == flag & nullable5.HasValue ? 1 : 0;
        }
        else
          num = 0;
        element.SetVisibility<Txt>(num != 0);
      }));
      Btn harvestNowBtn = this.Builder.NewBtn("HarvestNow").SetText((LocStrFormatted) Tr.CropHarvestNow__Action).SetButtonStyle(this.Builder.Style.Global.DangerBtn).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<FarmHarvestNowCmd>(new FarmHarvestNowCmd(this.Entity.Id))));
      Tooltip harvestTooltip = this.Builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) harvestNowBtn);
      float leftOffset2 = (float) (660.0 - (double) harvestNowBtn.GetOptimalWidth() - 20.0);
      harvestNowBtn.PutToLeftBottomOf<Btn>((IUiElement) activeCropTitle, harvestNowBtn.GetOptimalSize(), Offset.Left(leftOffset2) + Offset.Bottom(5f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.CurrentCrop.HasValue && this.Entity.CurrentCrop.Value.HarvestWillYieldProducts)).Observe<Quantity>((Func<Quantity>) (() =>
      {
        Crop valueOrNull = this.Entity.CurrentCrop.ValueOrNull;
        return valueOrNull == null ? Quantity.Zero : valueOrNull.GetHarvestEstimate();
      })).Do((Action<bool, Quantity>) ((isEnabled, harvestEstimate) =>
      {
        harvestNowBtn.SetEnabled(isEnabled);
        harvestTooltip.SetText(isEnabled ? Tr.CropHarvestNow__Tooltip.Format(harvestEstimate.ToString()) : LocStrFormatted.Empty);
      }));
      Btn previousHarvestStatsBtn = this.Builder.NewBtnGeneral("HarvestStats").SetText((LocStrFormatted) Tr.CropHarvestStats__Open).OnClick(new Action(this.showLastCropStats));
      previousHarvestStatsBtn.PutToLeftBottomOf<Btn>((IUiElement) activeCropTitle, previousHarvestStatsBtn.GetOptimalSize(), Offset.Left((float) ((double) leftOffset2 - (double) previousHarvestStatsBtn.GetOptimalWidth() - 10.0)) + Offset.Bottom(5f));
      updaterBuilder.Observe<Option<Crop>>((Func<Option<Crop>>) (() => this.Entity.PreviousCrop)).Do((Action<Option<Crop>>) (lastCrop =>
      {
        this.m_lastCrop = lastCrop;
        previousHarvestStatsBtn.SetEnabled(this.m_lastCrop.HasValue && !this.m_lastCrop.Value.Prototype.IsEmptyCrop);
      }));
      int size = 120;
      int num1 = (660 - size) / 2;
      Txt soilBufferTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FarmWater__Title, new LocStrFormatted?((LocStrFormatted) Tr.FarmWater__Tooltip));
      BufferView soilBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).SetAsSuperCompact().AppendTo<BufferView>(itemContainer, new Vector2?(new Vector2((float) num1, this.Style.BufferView.SuperCompactHeight)), ContainerPosition.LeftOrTop);
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.SoilWaterBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.SoilWaterBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.SoilWaterBuffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) =>
      {
        soilBuffer.UpdateState(product, capacity, quantity);
        itemContainer.SetItemVisibility((IUiElement) soilBuffer, capacity.IsPositive);
        itemContainer.SetItemVisibility((IUiElement) soilBufferTitle, capacity.IsPositive);
      }));
      this.Builder.CreateSectionTitle((IUiElement) soilBufferTitle, (LocStrFormatted) Tr.FarmWater__AvgNeed).PutToLeftOf<Txt>((IUiElement) soilBufferTitle, (float) size, Offset.Left((float) (num1 - 20)));
      Panel leftOf = this.Builder.NewPanel("AvgWaterContainer").SetBackground(this.Style.Panel.ItemOverlay).PutToLeftOf<Panel>((IUiElement) soilBuffer, (float) size, Offset.Left((float) num1));
      TextWithIcon avgWaterNeedTxt = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftMiddleOf<TextWithIcon>((IUiElement) leftOf, new Vector2(0.0f, 20f));
      updaterBuilder.Observe<PartialQuantity>((Func<PartialQuantity>) (() => this.Entity.AvgWaterNeedPerMonth)).Do((Action<PartialQuantity>) (avgNeed => avgWaterNeedTxt.SetPrefixText(avgNeed.ToStringRounded(1) + " / 60")));
      Txt waterTankTitle = this.Builder.CreateSectionTitle((IUiElement) soilBufferTitle, (LocStrFormatted) Tr.FarmIrrigation__Title, new LocStrFormatted?((LocStrFormatted) Tr.FarmIrrigation__Tooltip)).PutToLeftOf<Txt>((IUiElement) soilBufferTitle, 200f, Offset.Left((float) (num1 + size)));
      BufferView waterBuffer = this.Builder.NewBufferView((IUiElement) soilBuffer, isCompact: true).SetAsSuperCompact().PutToLeftOf<BufferView>((IUiElement) soilBuffer, (float) num1, Offset.Left((float) (num1 + size)));
      Panel noIrrigationPlaceholder = this.Builder.NewPanel("NoIrrigationPlaceholder").SetBackground(this.Style.Panel.ItemOverlay).PutToLeftOf<Panel>((IUiElement) soilBuffer, (float) num1, Offset.Left((float) (num1 + size))).Hide<Panel>();
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.ImportedWaterBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.ImportedWaterBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.ImportedWaterBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(waterBuffer.UpdateState));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.ImportedWaterBuffer.Capacity.IsPositive)).Do((Action<bool>) (hasWaterTank =>
      {
        waterTankTitle.SetVisibility<Txt>(hasWaterTank);
        waterBuffer.SetVisibility<BufferView>(hasWaterTank);
        noIrrigationPlaceholder.SetVisibility<Panel>(!hasWaterTank);
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FarmFertilityTitle, new LocStrFormatted?((LocStrFormatted) Tr.FarmFertility__Tooltip));
      Panel parent2 = this.Builder.NewPanel("InfoContainer").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new Vector2?(new Vector2(310f, this.Style.BufferView.HeightWithSlider)), ContainerPosition.RightOrBottom);
      TextWithIcon fertilizerNeededTxt = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Fertility128.png").SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) parent2, new Vector2(0.0f, 20f), Offset.Top(6f) + Offset.Left(18f));
      fertilizerNeededTxt.SetSuffixText("/ 60");
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToLeftMiddleOf<Panel>((IUiElement) fertilizerNeededTxt, 14.Vector2(), Offset.Left(-18f))).SetText((LocStrFormatted) Tr.FarmFertility__NeedTooltip);
      TextWithIcon naturalReplenishTxt = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Fertility128.png").SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) parent2, new Vector2(0.0f, 20f), Offset.Top(30f) + Offset.Left(18f));
      naturalReplenishTxt.SetSuffixText("/ 60");
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToLeftMiddleOf<Panel>((IUiElement) naturalReplenishTxt, 14.Vector2(), Offset.Left(-18f))).SetText((LocStrFormatted) Tr.FarmFertility__NaturalReplenishTooltip);
      TextWithIcon equilibriumTxt = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Fertility128.png").SetTextStyle(this.Style.Global.TextInc.Extend(new ColorRgba?(this.Builder.Style.Global.BlueForDark))).PutToLeftTopOf<TextWithIcon>((IUiElement) parent2, new Vector2(0.0f, 20f), Offset.Top(54f) + Offset.Left(18f));
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?(this.Builder.Style.Global.BlueForDark)).PutToLeftMiddleOf<Panel>((IUiElement) equilibriumTxt, 14.Vector2(), Offset.Left(-18f))).SetText((LocStrFormatted) Tr.FarmFertility__EquilibriumTooltip);
      BufferViewOneSlider fertilizerBuffer = this.Builder.NewBufferWithOneSlider((IUiElement) parent2, new Action<float>(sliderValueChange), 14, "").PutToRightOf<BufferViewOneSlider>((IUiElement) parent2, 350f, Offset.Right(310f));
      fertilizerBuffer.SetCustomIcon("Assets/Unity/UserInterface/General/Fertility128.png");
      fertilizerBuffer.SetCustomIconText(Tr.FarmFertility.TranslatedString);
      fertilizerBuffer.ShowSliderForNoProduct();
      fertilizerBuffer.Bar.SetColor((ColorRgba) 10116118);
      fertilizerBuffer.SetLabelFunc((Func<int, string>) (step => Tr.FarmFertility__Target.Format((step * Farm.FERTILITY_PER_SLIDER_STEP).ToStringRounded()).Value));
      QuantityBar.Marker equilibriumMarker = fertilizerBuffer.AddMarker(Percent.Zero, this.Builder.Style.Global.BlueForDark);
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.Fertility)).Do((Action<Percent>) (fertility => fertilizerBuffer.UpdateBar(Percent.FromRatio(fertility.ToIntPercentRounded(), 150), new LocStrFormatted(fertility.ToString()))));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Entity.FertilityTargetIndex)).Do((Action<int>) (fertilityStep => fertilizerBuffer.UpdateSlider(fertilityStep)));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.FertilityNeededPerDay)).Do((Action<Percent>) (fertility =>
      {
        string stringRounded = (fertility * 30).ToStringRounded(1);
        fertilizerNeededTxt.SetPrefixText(Tr.FarmFertility__Need.Format(stringRounded).Value);
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.NaturalReplenishPerDay)).Do((Action<Percent>) (replenish =>
      {
        string stringRounded = (replenish * 30).ToStringRounded(1);
        naturalReplenishTxt.SetPrefixText(string.Format("{0}: {1}", (object) Tr.FarmFertility__NaturalReplenish, (object) stringRounded));
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.NaturalFertilityEquilibrium)).Do((Action<Percent>) (equilibrium =>
      {
        string stringRounded = equilibrium.ToStringRounded(1);
        equilibriumTxt.SetPrefixText(Tr.FarmFertility__Equilibrium.Format(stringRounded).Value);
        equilibriumMarker.SetPosition(Percent.FromRatio(equilibrium.ToIntPercentRounded(), 150));
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.StoredFertilizerCapacity.IsPositive && this.AnyFertilizerUnlocked)).Do((Action<bool>) (hasFertilizerCapacity => fertilizerBuffer.SetSliderVisibility(hasFertilizerCapacity)));
      Txt fertilizerTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FarmFertilizer__Title, new LocStrFormatted?((LocStrFormatted) Tr.FarmFertilizer__Tooltip));
      Panel container = this.Builder.NewPanel("InfoContainer").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(60f));
      Btn leftMiddleOf = this.Builder.NewBtnGeneral("FertilizersOverview").SetText((LocStrFormatted) Tr.FarmFertilizersOverview__Open).AllowMultilineText().OnClick(new Action(this.showFertilizersOverview)).PutToLeftMiddleOf<Btn>((IUiElement) container, new Vector2(110f, 35f), Offset.Left(10f));
      float x = (float) (350.0 - (double) leftMiddleOf.GetWidth() - 20.0 - 24.0);
      QuantityBar storedFertilizedBar = new QuantityBar(this.Builder);
      storedFertilizedBar.SetColor((ColorRgba) 9861686).PutToLeftMiddleOf<QuantityBar>((IUiElement) container, new Vector2(x, 30f), Offset.Left(leftMiddleOf.GetWidth() + 20f));
      updaterBuilder.Observe<Quantity>((Func<Quantity>) (() => this.Entity.StoredFertilizerCount)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.StoredFertilizerCapacity)).Do((Action<Quantity, Quantity>) ((stored, capacity) => storedFertilizedBar.UpdateValues(capacity, stored)));
      float leftOffset3 = 368f;
      TextWithIcon maxFertilizationTxt = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Fertility128.png").PutToLeftTopOf<TextWithIcon>((IUiElement) container, new Vector2(660f - leftOffset3, 20f), Offset.Top(7f) + Offset.Left(leftOffset3));
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToLeftMiddleOf<Panel>((IUiElement) maxFertilizationTxt, 14.Vector2(), Offset.Left(-18f))).SetText((LocStrFormatted) Tr.FarmFertilizer__MaxFertilityTooltip);
      TextWithIcon fertilizerConversionTxt = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Fertility128.png").PutToLeftTopOf<TextWithIcon>((IUiElement) container, new Vector2(660f - leftOffset3, 20f), Offset.Top(33f) + Offset.Left(leftOffset3));
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToLeftMiddleOf<Panel>((IUiElement) fertilizerConversionTxt, 14.Vector2(), Offset.Left(-18f))).SetText((LocStrFormatted) Tr.FarmFertilizer__FertilizerConversionTooltip);
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.MaxFertilityProvidedByFertilizer)).Do((Action<Percent>) (maxFertility =>
      {
        string stringRounded = maxFertility.ToStringRounded();
        maxFertilizationTxt.SetPrefixText(Tr.FarmFertilizer__MaxFertility.Format(stringRounded).Value);
      }));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.FertilityPerFertilizer)).Do((Action<Percent>) (fertility =>
      {
        string stringRounded = fertility.ToStringRounded(1);
        fertilizerConversionTxt.SetPrefixText(Tr.FarmFertilizer__FertilizerConversion.Format(stringRounded).Value);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.StoredFertilizerCapacity.IsPositive && this.AnyFertilizerUnlocked)).Do((Action<bool>) (hasFertilizerCapacity =>
      {
        itemContainer.StartBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) fertilizerTitle, hasFertilizerCapacity);
        itemContainer.SetItemVisibility((IUiElement) container, hasFertilizerCapacity);
        itemContainer.FinishBatchOperation();
      }));
      Txt outputsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle);
      SwitchBtn switchBtn = this.Builder.NewSwitchBtn().SetText(Tr.NotifyIfFarmBufferFull.TranslatedString).SetOnToggleAction((Action<bool>) (_ => this.m_controller.Context.InputScheduler.ScheduleInputCmd<ToggleFullBufferNotificationCmd>(new ToggleFullBufferNotificationCmd(this.Entity))));
      switchBtn.PutToLeftOf<SwitchBtn>((IUiElement) outputsTitle, switchBtn.GetWidth(), Offset.Left(outputsTitle.GetPreferedWidth() + 20f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.NotifyOnFullBuffer)).Do((Action<bool>) (notifyOn => switchBtn.SetIsOn(notifyOn)));
      StackContainer outputsContainer = this.Builder.NewStackContainer("Output").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).AppendTo<StackContainer>(itemContainer);
      ViewsCacheHomogeneous<FarmWindowView.OutputBufferView> buffersCache = new ViewsCacheHomogeneous<FarmWindowView.OutputBufferView>((Func<FarmWindowView.OutputBufferView>) (() => new FarmWindowView.OutputBufferView((IUiElement) outputsContainer, this.Builder)));
      this.AddUpdater(buffersCache.Updater);
      updaterBuilder.Observe<IProductBuffer>((Func<IEnumerable<IProductBuffer>>) (() => (IEnumerable<IProductBuffer>) this.Entity.OutputBuffers), (ICollectionComparator<IProductBuffer, IEnumerable<IProductBuffer>>) CompareFixedOrder<IProductBuffer>.Instance).Do((Action<Lyst<IProductBuffer>>) (buffers =>
      {
        outputsContainer.ClearAll();
        buffersCache.ReturnAll();
        outputsContainer.StartBatchOperation();
        foreach (IProductBuffer buffer in buffers)
          buffersCache.GetView().SetBuffer(buffer).AppendTo<FarmWindowView.OutputBufferView>(outputsContainer);
        outputsContainer.FinishBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) outputsTitle, buffers.IsNotEmpty);
      }));
      updaterBuilder.Observe<Farm.State>((Func<Farm.State>) (() => this.Entity.CurrentState)).Do((Action<Farm.State>) (state =>
      {
        switch (state)
        {
          case Farm.State.None:
          case Farm.State.NoCropSelected:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Farm_NoCrop, StatusPanel.State.Critical);
            break;
          case Farm.State.Paused:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case Farm.State.Broken:
            this.m_statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case Farm.State.NotEnoughWorkers:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case Farm.State.NotEnoughWater:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Farm_NoWater, StatusPanel.State.Critical);
            break;
          case Farm.State.LowFertility:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Farm_LowFertility, StatusPanel.State.Critical);
            break;
          case Farm.State.Growing:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Farm_Growing);
            break;
        }
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.m_windowOverlay.SendToFront<Panel>();
      this.m_cropPicker.PutTo<CropPicker>((IUiElement) this.m_windowOverlay);
      this.m_cropPicker.Hide();
      this.m_fertilizersOverview.PutToCenterMiddleOf<FertilizersOverview>((IUiElement) this.m_windowOverlay, this.m_fertilizersOverview.GetSize());
      this.m_fertilizersOverview.Hide();
      this.m_lastCropStats.PutTo<LastCropStats>((IUiElement) this.m_windowOverlay);
      this.m_lastCropStats.Hide();
      this.OnHide += (Action) (() =>
      {
        this.m_windowOverlay.Hide<Panel>();
        this.m_cropPicker.Hide();
        this.m_fertilizersOverview.Hide();
        this.m_lastCropStats.Hide();
      });

      void sliderValueChange(float value)
      {
        int num = (int) value;
        Assert.That<int>(num).IsWithinIncl(0, 14);
        this.m_controller.Context.InputScheduler.ScheduleInputCmd<FarmSetFertilityTargetCmd>(new FarmSetFertilityTargetCmd(this.Entity, num * Farm.FERTILITY_PER_SLIDER_STEP));
      }

      void onProtoUnlocked()
      {
        if (!this.m_controller.Context.UnlockedProtosDbForUi.IsUnlocked((IProto) this.m_cropRotationTech))
          return;
        this.m_controller.Context.UnlockedProtosDbForUi.OnUnlockedSetChangedForUi -= new Action(onProtoUnlocked);
        positionSchedule(true);
      }

      void positionSchedule(bool isRotationUnlocked)
      {
        if (isRotationUnlocked)
        {
          avgProductionTitle.PutToLeftOf<TextWithIcon>((IUiElement) cropScheduleTitle, 200f, Offset.Left(avgDataOffset + 20f));
          yieldInfoContainer.PutToLeftTopOf<StackContainer>((IUiElement) cropSchedule, new Vector2(yieldInfoContainer.GetDynamicWidth(), 46f), Offset.Left(avgDataOffset + 10f));
        }
        else
        {
          avgProductionTitle.PutToLeftOf<TextWithIcon>((IUiElement) cropScheduleTitle, 200f, Offset.Left(20f));
          yieldInfoContainer.PutToLeftTopOf<StackContainer>((IUiElement) cropSchedule, new Vector2(yieldInfoContainer.GetDynamicWidth(), 46f));
          avgProductionTitle.SetParent<TextWithIcon>((IUiElement) scheduleHolder);
          yieldInfoContainer.SetParent<StackContainer>((IUiElement) scheduleHolder);
        }
        cropScheduleTitle.SetVisibility<Txt>(isRotationUnlocked);
        cropSchedule.SetVisibility<CropScheduleView>(isRotationUnlocked);
      }
    }

    private void showCropPicker(int slotIndex)
    {
      this.m_lastSlotIndex = slotIndex;
      this.m_cropPicker.RefreshData(this.Entity.Prototype);
      this.m_cropPicker.PutToCenterMiddleOf<CropPicker>((IUiElement) this.m_windowOverlay, this.m_cropPicker.GetSize());
      this.m_cropPicker.Show();
      this.m_windowOverlay.Show<Panel>();
    }

    private void onCropPicked(Option<CropProto> crop)
    {
      this.m_controller.ScheduleInputCmd<FarmAssignCropCmd>(new FarmAssignCropCmd(this.Entity, crop, this.m_lastSlotIndex));
      this.m_cropPicker.Hide();
      this.m_windowOverlay.Hide<Panel>();
    }

    private void showFertilizersOverview()
    {
      this.m_fertilizersOverview.Show();
      this.m_windowOverlay.Show<Panel>();
    }

    private void showLastCropStats()
    {
      if (this.m_lastCrop.IsNone || this.m_lastCrop.Value.Prototype.IsEmptyCrop)
        return;
      this.m_lastCropStats.SetCrop(this.m_lastCrop.Value);
      this.m_lastCropStats.PutToCenterMiddleOf<LastCropStats>((IUiElement) this.m_windowOverlay, this.m_lastCropStats.GetSize());
      this.m_lastCropStats.Show();
      this.m_windowOverlay.Show<Panel>();
    }

    private class OutputBufferView : IUiElementWithUpdater, IUiElement
    {
      private readonly BufferView m_bufferView;

      public GameObject GameObject => this.m_bufferView.GameObject;

      public RectTransform RectTransform => this.m_bufferView.RectTransform;

      private IProductBuffer Buffer { get; set; }

      public IUiUpdater Updater { get; }

      public OutputBufferView(IUiElement parent, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_bufferView = builder.NewBufferView(parent, isCompact: true).SetAsSuperCompact();
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Buffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Buffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(this.m_bufferView.UpdateState));
        this.Updater = updaterBuilder.Build();
        this.SetHeight<FarmWindowView.OutputBufferView>(builder.Style.BufferView.SuperCompactHeight);
      }

      public FarmWindowView.OutputBufferView SetBuffer(IProductBuffer buffer)
      {
        this.Buffer = buffer;
        return this;
      }
    }
  }
}
