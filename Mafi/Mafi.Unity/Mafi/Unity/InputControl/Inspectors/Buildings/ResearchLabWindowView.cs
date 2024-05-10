// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ResearchLabWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Research;
using Mafi.Core.Syncers;
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
  internal class ResearchLabWindowView : StaticEntityInspectorBase<Mafi.Core.Buildings.ResearchLab.ResearchLab>
  {
    private readonly ResearchLabInspector m_controller;
    private readonly ResearchManager m_researchManager;
    private readonly Action m_onOpenResearchWindow;

    protected override Mafi.Core.Buildings.ResearchLab.ResearchLab Entity
    {
      get => this.m_controller.SelectedEntity;
    }

    public ResearchLabWindowView(
      ResearchLabInspector controller,
      ResearchManager researchManager,
      Action onOpenResearchWindow)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_researchManager = researchManager;
      this.m_onOpenResearchWindow = onOpenResearchWindow;
      this.m_controller = controller.CheckNotNull<ResearchLabInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddUnityCostPanel(updaterBuilder, (Func<IUnityConsumingEntity>) (() => (IUnityConsumingEntity) this.Entity));
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ResearchSpeed__Title);
      Panel parent = this.Builder.NewPanel("SpeedContainer").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(35f));
      TextWithIcon speedView = new TextWithIcon(this.Builder).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftOf<TextWithIcon>((IUiElement) parent, 200f, Offset.Left(20f));
      updaterBuilder.Observe<Fix32>((Func<Fix32>) (() => this.Entity.ResearchPointsPerMonth)).Do((Action<Fix32>) (pointPerMonth => speedView.SetPrefixText(pointPerMonth.ToStringRounded(1) + " / 60")));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CurrentResearch);
      Panel statusContainer = this.Builder.NewPanel("RecipeContainer").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(50f));
      SimpleProgressBar researchProgressBar = new SimpleProgressBar((IUiElement) statusContainer, this.Builder).PutToBottomOf<SimpleProgressBar>((IUiElement) statusContainer, 6f);
      Btn resStartBtn = this.Builder.NewBtnPrimary("NewResearchBtn").SetText((LocStrFormatted) Tr.StartNewResearch_Action).OnClick(this.m_onOpenResearchWindow);
      resStartBtn.AppendTo<Btn>(itemContainer, new Vector2?(resStartBtn.GetOptimalSize()), ContainerPosition.MiddleOrCenter, Offset.TopBottom(8f));
      this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/Toolbar/Research.svg").PutToLeftMiddleOf<IconContainer>((IUiElement) statusContainer, 20.Vector2(), Offset.Left(20f));
      Txt researchTitle = this.Builder.NewTxt("Title").SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) statusContainer, Offset.Left(50f));
      Txt inputBufferTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle);
      BufferView inputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      TextWithIcon consumedPerMonth = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) inputBuffer, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.InputBuffer.ValueOrNull?.Product)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.InputBuffer.ValueOrNull?.Capacity)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.InputBuffer.ValueOrNull?.Quantity)).Do((Action<ProductProto, Quantity?, Quantity?>) ((product, capacity, quantity) =>
      {
        itemContainer.SetItemVisibility((IUiElement) inputBuffer, capacity.HasValue);
        itemContainer.SetItemVisibility((IUiElement) inputBufferTitle, capacity.HasValue);
        if (!capacity.HasValue || !quantity.HasValue)
          return;
        inputBuffer.UpdateState(product, capacity.Value, quantity.Value);
      }));
      updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => this.Entity.Prototype.ConsumedPerRecipe)).Observe<Duration>((Func<Duration>) (() => this.Entity.Prototype.DurationForRecipe)).Do((Action<ProductQuantity, Duration>) ((consumerPerRecipe, duration) =>
      {
        if (!consumerPerRecipe.IsNotEmpty)
          return;
        Fix32 fix32 = 60.ToFix32() / duration.Seconds.ToFix32() * consumerPerRecipe.Quantity.Value;
        consumedPerMonth.SetPrefixText(Tr.Needs.TranslatedString + " " + fix32.ToStringRounded(1) + " / 60");
      }));
      Txt outputBufferTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle);
      BufferView outputBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      TextWithIcon producedPerMonth = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) outputBuffer, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.OutputBuffer.ValueOrNull?.Product)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.OutputBuffer.ValueOrNull?.Capacity)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.OutputBuffer.ValueOrNull?.Quantity)).Do((Action<ProductProto, Quantity?, Quantity?>) ((product, capacity, quantity) =>
      {
        itemContainer.SetItemVisibility((IUiElement) outputBuffer, capacity.HasValue);
        itemContainer.SetItemVisibility((IUiElement) outputBufferTitle, capacity.HasValue);
        if (!capacity.HasValue || !quantity.HasValue)
          return;
        outputBuffer.UpdateState(product, capacity.Value, quantity.Value);
      }));
      updaterBuilder.Observe<ProductQuantity>((Func<ProductQuantity>) (() => this.Entity.Prototype.ProducedPerRecipe)).Observe<Duration>((Func<Duration>) (() => this.Entity.Prototype.DurationForRecipe)).Do((Action<ProductQuantity, Duration>) ((producedPerRecipe, duration) =>
      {
        if (!producedPerRecipe.IsNotEmpty)
          return;
        Fix32 fix32 = 60.ToFix32() / duration.Seconds.ToFix32() * producedPerRecipe.Quantity.Value;
        producedPerMonth.SetPrefixText(string.Format("{0} {1} / 60", (object) Tr.Provides, (object) fix32.ToStringRounded(1)));
      }));
      updaterBuilder.Observe<Mafi.Core.Buildings.ResearchLab.ResearchLab.State>((Func<Mafi.Core.Buildings.ResearchLab.ResearchLab.State>) (() => this.Entity.CurrentState)).Do((Action<Mafi.Core.Buildings.ResearchLab.ResearchLab.State>) (state =>
      {
        itemContainer.SetItemVisibility((IUiElement) statusContainer, this.Entity.CurrentState != Mafi.Core.Buildings.ResearchLab.ResearchLab.State.MissingInput);
        switch (state)
        {
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.Idle:
            statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.MissingInput:
            statusInfo.SetStatus(Tr.EntityStatus__MissingInput, StatusPanel.State.Critical);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.NotEnoughUpoints:
            statusInfo.SetStatus(Tr.EntityStatus__NoUnity, StatusPanel.State.Critical);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.NotEnoughPower:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.NotEnoughComputing:
            statusInfo.SetStatus(TrCore.EntityStatus__NoComputing, StatusPanel.State.Critical);
            break;
          case Mafi.Core.Buildings.ResearchLab.ResearchLab.State.ResearchTooDifficult:
            statusInfo.SetStatus(Tr.EntityStatus__ResearchTooAdvanced, StatusPanel.State.Critical);
            break;
        }
      }));
      updaterBuilder.Observe<Option<ResearchNodeProto>>((Func<Option<ResearchNodeProto>>) (() => this.Entity.CurrentResearch)).Do((Action<Option<ResearchNodeProto>>) (currentResearch =>
      {
        researchTitle.SetText(currentResearch.ValueOrNull?.Strings.Name.TranslatedString ?? "--");
        resStartBtn.SetButtonStyle(currentResearch.HasValue ? this.Builder.Style.Global.GeneralBtn : this.Builder.Style.Global.PrimaryBtn);
        resStartBtn.SetText((LocStrFormatted) (currentResearch.HasValue ? Tr.OpenResearch_Action : Tr.StartNewResearch_Action));
        resStartBtn.SetWidth<Btn>(resStartBtn.GetOptimalWidth());
      }));
      updaterBuilder.Observe<Percent?>((Func<Percent?>) (() => this.m_researchManager.CurrentResearch.ValueOrNull?.ProgressInPerc)).Do((Action<Percent?>) (progressMaybe =>
      {
        if (progressMaybe.HasValue)
        {
          researchProgressBar.Show<SimpleProgressBar>();
          researchProgressBar.SetProgress(progressMaybe.Value);
        }
        else
          researchProgressBar.Hide<SimpleProgressBar>();
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth(450f);
    }
  }
}
