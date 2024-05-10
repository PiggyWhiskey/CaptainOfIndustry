// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.Settlements.HospitalWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings.Settlements
{
  internal class HospitalWindowView : StaticEntityInspectorBase<Hospital>
  {
    private readonly HospitalInspector m_controller;
    private readonly PopsHealthManager m_popsHealthManager;
    private HospitalWindowView.MedicalSuppliesView.Cache m_inputBuffersCache;
    private StackContainer m_inputBuffersContainer;

    protected override Hospital Entity => this.m_controller.SelectedEntity;

    public HospitalWindowView(
      HospitalInspector controller,
      PopsHealthManager popsHealthManager,
      SettlementsManager settlementsManager,
      Action openSettlementWindow)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_popsHealthManager = popsHealthManager;
      this.m_controller = controller.CheckNotNull<HospitalInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      int num = 550;
      this.m_inputBuffersCache = new HospitalWindowView.MedicalSuppliesView.Cache(this.Builder, this.m_controller.Context.UnlockedProtosDbForUi, this.m_popsHealthManager, (ItemDetailWindowView) this, new Action<ProductProto, int>(onProductSet), new Action<int>(onProductRemove), (Func<Hospital>) (() => this.Entity));
      this.AddUpdater(this.m_inputBuffersCache.Updater);
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(this.ItemsContainer, (LocStrFormatted) Tr.CurrentDisease__Title, new LocStrFormatted?((LocStrFormatted) Tr.CurrentDisease__Tooltip));
      Panel parent1 = this.Builder.AddOverlayPanel(this.ItemsContainer, 40);
      Txt diseaseName = this.Builder.NewTxt("DiseaseName").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Builder.Style.Global.TextInc);
      diseaseName.PutToLeftTopOf<Txt>((IUiElement) parent1, new Vector2((float) num, 20f), Offset.Left(40f));
      Panel diseaseNameDesc = this.Builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png", new ColorRgba?(this.Builder.Style.Global.DangerClr)).PutToLeftMiddleOf<Panel>((IUiElement) diseaseName, 14.Vector2(), Offset.Left(-18f));
      Tooltip diseaseTooltip = this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) diseaseNameDesc);
      Txt diseaseInfo = this.Builder.NewTxt("DiseaseInfo").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Builder.Style.Global.TextInc).SetColor(this.Builder.Style.Global.DangerClr);
      diseaseInfo.PutToLeftBottomOf<Txt>((IUiElement) parent1, new Vector2((float) num, 20f), Offset.Left(20f));
      updaterBuilder.Observe<Option<DiseaseProto>>((Func<Option<DiseaseProto>>) (() => this.m_popsHealthManager.CurrentDisease)).Observe<int>((Func<int>) (() => this.m_popsHealthManager.CurrentDiseaseMonthsLeft)).Observe<Percent>((Func<Percent>) (() => this.m_popsHealthManager.CurrentDiseaseMortality)).Do((Action<Option<DiseaseProto>, int, Percent>) ((currentDisease, monthsLeft, mortality) =>
      {
        if (currentDisease.HasValue)
        {
          DiseaseProto diseaseProto = currentDisease.Value;
          diseaseName.SetText((LocStrFormatted) diseaseProto.Strings.Name);
          diseaseTooltip.SetText((LocStrFormatted) diseaseProto.Strings.DescShort);
          diseaseInfo.SetText(Tr.CurrentDisease__Info.Format(monthsLeft.ToString(), (-diseaseProto.HealthPenalty.ToIntPercentRounded()).ToString(), mortality.ToStringRounded(1)));
        }
        else
          diseaseName.SetText((LocStrFormatted) Tr.CurrentDisease__NoDisease);
        diseaseName.SetColor(currentDisease.HasValue ? this.Builder.Style.Global.DangerClr : this.Builder.Style.Global.Text.Color);
        diseaseInfo.SetVisibility<Txt>(currentDisease.HasValue);
        diseaseNameDesc.SetVisibility<Panel>(currentDisease.HasValue);
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle, new LocStrFormatted?((LocStrFormatted) Tr.Hospital_InputsTooltip));
      Panel parent2 = this.Builder.NewPanel("Info").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(24f));
      TextWithIcon inputNeeded = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftOf<TextWithIcon>((IUiElement) parent2, 200f, Offset.Left(30f) + Offset.Top(4f));
      this.m_inputBuffersContainer = this.Builder.NewStackContainer("Buffers").SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(5f).AppendTo<StackContainer>(itemContainer, new float?(0.0f));
      updaterBuilder.Observe<Hospital.State>((Func<Hospital.State>) (() => this.Entity.CurrentState)).Do((Action<Hospital.State>) (state =>
      {
        switch (state)
        {
          case Hospital.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case Hospital.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case Hospital.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case Hospital.State.MissingInput:
            statusInfo.SetStatus(Tr.EntityStatus__WaitingForProducts, StatusPanel.State.Critical);
            break;
          case Hospital.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case Hospital.State.NotEnoughPower:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
        }
      }));
      updaterBuilder.Observe<PartialQuantity>((Func<PartialQuantity>) (() => this.Entity.GetSettlementSuppliesNeedPerMonth())).Do((Action<PartialQuantity>) (totalNeeded => inputNeeded.SetPrefixText(string.Format("{0}: {1} / 60", (object) Tr.TotalSettlementNeed, (object) totalNeeded.ToStringRounded(1)))));
      updaterBuilder.Observe<Option<ProductBuffer>>((Func<IIndexable<Option<ProductBuffer>>>) (() => this.Entity.BuffersPerSlot), (ICollectionComparator<Option<ProductBuffer>, IIndexable<Option<ProductBuffer>>>) CompareFixedOrder<Option<ProductBuffer>>.Instance).Do((Action<Lyst<Option<ProductBuffer>>>) (buffers =>
      {
        this.m_inputBuffersContainer.StartBatchOperation();
        this.m_inputBuffersContainer.ClearAll();
        this.m_inputBuffersCache.ReturnAll();
        int slot = 0;
        foreach (Option<ProductBuffer> buffer in buffers)
        {
          HospitalWindowView.MedicalSuppliesView view = this.m_inputBuffersCache.GetView();
          view.Show<HospitalWindowView.MedicalSuppliesView>();
          view.AppendTo<HospitalWindowView.MedicalSuppliesView>(this.m_inputBuffersContainer, new float?(view.RequiredHeight));
          view.SetSlot(slot);
          ++slot;
        }
        this.m_inputBuffersContainer.FinishBatchOperation();
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth((float) num);

      void onProductSet(ProductProto product, int slot)
      {
        this.m_controller.InputScheduler.ScheduleInputCmd<AssignProductToSlotCmd>(new AssignProductToSlotCmd((IEntityWithMultipleProductsToAssign) this.Entity, (Option<ProductProto>) product, slot));
      }

      void onProductRemove(int slot)
      {
        this.m_controller.InputScheduler.ScheduleInputCmd<AssignProductToSlotCmd>(new AssignProductToSlotCmd((IEntityWithMultipleProductsToAssign) this.Entity, Option<ProductProto>.None, slot));
      }
    }

    private class MedicalSuppliesView : IUiElementWithUpdater, IUiElement
    {
      private readonly Panel m_container;
      private readonly ProductBufferViewWithPicker m_buffer;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public float RequiredHeight { get; }

      public IUiUpdater Updater { get; private set; }

      public MedicalSuppliesView(
        UiBuilder builder,
        UnlockedProtosDbForUi unlockedProtosDb,
        PopsHealthManager popsHealthManager,
        ItemDetailWindowView owner,
        Action<ProductProto, int> onSetProductToStore,
        Action<int> onProductRemove,
        Func<Hospital> entityProvider)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        HospitalWindowView.MedicalSuppliesView medicalSuppliesView = this;
        this.RequiredHeight = builder.Style.BufferView.HeightWithSlider;
        this.m_container = builder.NewPanel("Product").SetBackground(builder.Style.Panel.ItemOverlay);
        UiStyle style = builder.Style;
        int size = 300;
        this.m_buffer = new ProductBufferViewWithPicker(builder, unlockedProtosDb, owner, onSetProductToStore, onProductRemove, (Func<IEntityWithMultipleProductsToAssign>) entityProvider);
        this.m_buffer.PutToLeftOf<ProductBufferViewWithPicker>((IUiElement) this.m_container, (float) size, Offset.Bottom(builder.Style.BufferView.HeightWithSlider - builder.Style.BufferView.Height));
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        TextWithIcon upointsGivenTxt = new TextWithIcon(builder).SetTextStyle(style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(0.0f, 20f), Offset.Top(6f) + Offset.Left((float) (size + 18)));
        TextWithIcon healthGivenTxt = new TextWithIcon(builder).SetTextStyle(style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/Health.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(0.0f, 20f), Offset.Top(30f) + Offset.Left((float) (size + 18)));
        TextWithIcon mortalityReduceTxt = new TextWithIcon(builder).SetTextStyle(style.Global.TextInc).SetIcon("Assets/Unity/UserInterface/General/PopulationSmall.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(0.0f, 20f), Offset.Top(54f) + Offset.Left((float) (size + 18)));
        Panel mortalityTxtHelp = builder.NewPanel("Help").SetBackground("Assets/Unity/UserInterface/General/Info128.png").PutToLeftMiddleOf<Panel>((IUiElement) mortalityReduceTxt, 14.Vector2(), Offset.Left(-18f));
        builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) mortalityTxtHelp).SetText((LocStrFormatted) Tr.Hospital_MortalityReductionTooltip);
        updaterBuilder.Observe<MedicalSuppliesParam>((Func<MedicalSuppliesParam>) (() =>
        {
          ProductProto product = entityProvider().GetBuffer(medicalSuppliesView.m_buffer.Slot).ValueOrNull?.Product;
          if (!((Proto) product != (Proto) null))
            return (MedicalSuppliesParam) null;
          MedicalSuppliesParam paramValue;
          return !product.TryGetParam<MedicalSuppliesParam>(out paramValue) ? (MedicalSuppliesParam) null : paramValue;
        })).Observe<Percent>((Func<Percent>) (() => entityProvider().UnityProductionMultiplier)).Do((Action<MedicalSuppliesParam, Percent>) ((supplies, unityMultiplier) =>
        {
          if (supplies != null)
          {
            upointsGivenTxt.SetPrefixText(string.Format("{0}: +{1}", (object) TrCore.Unity, (object) supplies.GetUnityWhenProvided(unityMultiplier).Format()));
            healthGivenTxt.SetPrefixText(string.Format("{0}: +{1}", (object) Tr.Health, (object) supplies.HealthPointsWhenProvided.ToIntPercentRounded()));
            mortalityReduceTxt.SetPrefixText(string.Format("{0}: {1}", (object) Tr.Hospital_MortalityReduction, (object) supplies.MortalityDeductionWhenProvided));
          }
          upointsGivenTxt.SetVisibility<TextWithIcon>(supplies != null);
          healthGivenTxt.SetVisibility<TextWithIcon>(supplies != null);
          mortalityReduceTxt.SetVisibility<TextWithIcon>(supplies != null);
        }));
        updaterBuilder.Observe<bool>((Func<bool>) (() => popsHealthManager.IsDiseaseMortalityIgnored)).Do((Action<bool>) (isMortalityIgnored =>
        {
          mortalityReduceTxt.SetVisibility<TextWithIcon>(!isMortalityIgnored);
          mortalityTxtHelp.SetVisibility<Panel>(!isMortalityIgnored);
        }));
        this.Updater = updaterBuilder.Build();
        this.Updater.AddChildUpdater(this.m_buffer.Updater);
      }

      public void SetSlot(int slot) => this.m_buffer.SetSlot(slot);

      public class Cache : ViewsCacheHomogeneous<HospitalWindowView.MedicalSuppliesView>
      {
        public Cache(
          UiBuilder builder,
          UnlockedProtosDbForUi unlockedProtosDb,
          PopsHealthManager popsHealthManager,
          ItemDetailWindowView owner,
          Action<ProductProto, int> onSetProductToStore,
          Action<int> onProductRemove,
          Func<Hospital> entityProvider)
        {
          xxhJUtQyC9HnIshc6H.OukgcisAbr();
          // ISSUE: explicit constructor call
          base.\u002Ector((Func<HospitalWindowView.MedicalSuppliesView>) (() => new HospitalWindowView.MedicalSuppliesView(builder, unlockedProtosDb, popsHealthManager, owner, onSetProductToStore, onProductRemove, entityProvider)));
        }
      }
    }
  }
}
