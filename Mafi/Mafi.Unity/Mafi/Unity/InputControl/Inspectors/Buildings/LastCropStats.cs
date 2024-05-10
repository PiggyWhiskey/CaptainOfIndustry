// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.LastCropStats
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class LastCropStats : WindowView
  {
    private Panel m_container;
    private ProductQuantityWithIcon m_cropIcon;
    private StackContainer m_statsContainer;
    private ViewsCacheHomogeneous<Txt> m_statsCache;

    public LastCropStats()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (LastCropStats));
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.CropHarvestStats__Title);
      this.m_statsCache = new ViewsCacheHomogeneous<Txt>((Func<Txt>) (() => this.Builder.NewTxt("Stat").SetTextStyle(this.Builder.Style.Global.TextInc).SetAlignment(TextAnchor.MiddleLeft)));
      this.m_container = this.Builder.NewPanel("Container").SetBackground(this.Builder.Style.Panel.ItemOverlay).PutToTopOf<Panel>((IUiElement) this.GetContentPanel(), 0.0f, Offset.Top(10f));
      this.m_cropIcon = new ProductQuantityWithIcon((IUiElement) this.m_container, this.Builder);
      this.m_cropIcon.PutToLeftOf<ProductQuantityWithIcon>((IUiElement) this.m_container, 60f, Offset.Left(10f) + Offset.TopBottom(10f));
      this.m_statsContainer = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f).SetInnerPadding(Offset.TopBottom(10f)).PutToLeftTopOf<StackContainer>((IUiElement) this.m_container, new Vector2(200f, 0.0f), Offset.Left(this.m_cropIcon.GetWidth() + 40f));
    }

    public void SetCrop(Crop crop)
    {
      this.m_cropIcon.SetProduct(crop.Yield);
      this.m_cropIcon.SetIcon(crop.Prototype.Graphics.IconPath);
      this.m_statsContainer.StartBatchOperation();
      this.m_statsContainer.ClearAll();
      this.m_statsCache.ReturnAll();
      if (crop.HarvestReason == CropHarvestReason.PrematureLackOfMaintenance)
        this.m_statsCache.GetView().SetText((LocStrFormatted) Tr.CropState__DeadNoMaintenance).SetColor(this.Builder.Style.Global.OrangeText).AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      else if (crop.HarvestReason == CropHarvestReason.PrematureNoWater)
        this.m_statsCache.GetView().SetText((LocStrFormatted) Tr.CropState__DeadNoWater).SetColor(this.Builder.Style.Global.OrangeText).AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      else if (crop.HarvestReason == CropHarvestReason.PrematureNoFertility)
        this.m_statsCache.GetView().SetText((LocStrFormatted) Tr.CropState__DeadNoFertility).SetColor(this.Builder.Style.Global.OrangeText).AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      else if (crop.HarvestReason == CropHarvestReason.PrematureClearedByPlayer)
        this.m_statsCache.GetView().SetText((LocStrFormatted) Tr.CropState__RemovedForChange).SetColor(this.Builder.Style.Global.OrangeText).AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      Percent percent;
      if (!crop.YieldDeltaDueToFertility.IsNear(Percent.Zero, Percent.One))
      {
        Txt view = this.m_statsCache.GetView();
        percent = crop.YieldDeltaDueToFertility;
        if (percent.IsPositive)
        {
          Txt txt = view;
          ref readonly LocStr1 local = ref Tr.CropStats__MoreDueToFertility;
          percent = crop.YieldDeltaDueToFertility;
          string stringRounded = percent.ToStringRounded();
          LocStrFormatted text = local.Format(stringRounded);
          txt.SetText(text);
          view.SetColor(this.Builder.Style.Global.GreenForDark);
        }
        else
        {
          Txt txt = view;
          ref readonly LocStr1 local = ref Tr.CropStats__LessDueToFertility;
          percent = -crop.YieldDeltaDueToFertility;
          string stringRounded = percent.ToStringRounded();
          LocStrFormatted text = local.Format(stringRounded);
          txt.SetText(text);
          view.SetColor(this.Builder.Style.Global.OrangeText);
        }
        view.AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      }
      percent = crop.YieldDeltaDueToBonusMultiplier;
      if (!percent.IsNear(Percent.Zero, Percent.One))
      {
        Txt view = this.m_statsCache.GetView();
        percent = crop.YieldDeltaDueToBonusMultiplier;
        if (percent.IsPositive)
        {
          Txt txt = view;
          ref readonly LocStr1 local = ref Tr.CropStats__MoreDueToBonus;
          percent = crop.YieldDeltaDueToBonusMultiplier;
          string stringRounded = percent.ToStringRounded();
          LocStrFormatted text = local.Format(stringRounded);
          txt.SetText(text);
          view.SetColor(this.Builder.Style.Global.GreenForDark);
        }
        view.AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      }
      percent = crop.YieldLostDueToLackOfWater;
      if (!percent.IsNear(Percent.Zero, Percent.One))
      {
        Txt view = this.m_statsCache.GetView();
        Txt txt = view;
        ref readonly LocStr1 local = ref Tr.CropStats__LessDueToWater;
        percent = crop.YieldLostDueToLackOfWater;
        string stringRounded = percent.ToStringRounded();
        LocStrFormatted text = local.Format(stringRounded);
        txt.SetText(text);
        view.SetColor(this.Builder.Style.Global.OrangeText);
        view.AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      }
      percent = crop.YieldLostDueToPrematureHarvest;
      if (!percent.IsNear(Percent.Zero, Percent.One))
      {
        Txt view = this.m_statsCache.GetView();
        Txt txt = view;
        ref readonly LocStr1 local = ref Tr.CropStats__LessDueEarlyHarvest;
        percent = crop.YieldLostDueToPrematureHarvest;
        string stringRounded = percent.ToStringRounded();
        LocStrFormatted text = local.Format(stringRounded);
        txt.SetText(text);
        view.SetColor(this.Builder.Style.Global.OrangeText);
        view.AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      }
      if (crop.DaysWaitingForWaterBeforeGrowthStart > 0)
      {
        Txt view = this.m_statsCache.GetView();
        Fix64 quantity = crop.DaysWaitingForWaterBeforeGrowthStart / 30.ToFix64();
        string str = TrCore.NumberOfMonths.Format(quantity.ToStringRounded(1), quantity).Value;
        view.SetText(Tr.CropStats__DelayedDueToWater.Format(str));
        view.SetColor(this.Builder.Style.Global.OrangeText);
        view.AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      }
      if (crop.TotalDaysWithoutWater > 0)
      {
        Fix64 quantity = crop.TotalDaysWithoutWater / 30.ToFix64();
        Txt view = this.m_statsCache.GetView();
        string str = TrCore.NumberOfMonths.Format(quantity.ToStringRounded(1), quantity).Value;
        view.SetText(Tr.CropStats__MonthsWithoutWater.Format(str));
        view.SetColor(this.Builder.Style.Global.OrangeText);
        view.AppendTo<Txt>(this.m_statsContainer, new float?(20f));
      }
      this.m_statsContainer.FinishBatchOperation();
      float height = this.m_statsContainer.GetDynamicHeight().Max(90f);
      this.m_container.SetHeight<Panel>(height);
      this.SetContentSize(350f, height + 10f);
    }
  }
}
