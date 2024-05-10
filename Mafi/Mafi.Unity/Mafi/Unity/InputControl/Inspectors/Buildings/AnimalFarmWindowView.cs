// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.AnimalFarmWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
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
  internal class AnimalFarmWindowView : StaticEntityInspectorBase<AnimalFarm>
  {
    private readonly AnimalFarmInspector m_controller;
    private StatusPanel m_statusInfo;

    protected override AnimalFarm Entity => this.m_controller.SelectedEntity;

    public AnimalFarmWindowView(AnimalFarmInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<AnimalFarmInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.SetWidth(600f);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_statusInfo = this.AddStatusInfoPanel();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.AnimalFarm_Title, new LocStrFormatted?((LocStrFormatted) Tr.AnimalFarm_TitleTooltip));
      SwitchBtn parent1 = this.AddSwitch(itemContainer, Tr.AnimalSlaughtering__Title.TranslatedString, (Action<bool>) (x => this.m_controller.ScheduleInputCmd<AnimalFarmSetSlaughterLimitCmd>(new AnimalFarmSetSlaughterLimitCmd(this.Entity, new int?()))), updaterBuilder, (Func<bool>) (() => this.Entity.IsSlaughteringEnabled), Tr.AnimalSlaughtering__Tooltip.TranslatedString);
      SwitchBtn growthSwitch = this.Builder.NewSwitchBtn().SetText((LocStrFormatted) Tr.AnimalFarm_PauseGrowth__Title).SetOnToggleAction((Action<bool>) (x => this.m_controller.ScheduleInputCmd<AnimalFarmToggleGrowthPauseCmd>(new AnimalFarmToggleGrowthPauseCmd(this.Entity)))).PutToLeftOf<SwitchBtn>((IUiElement) parent1, this.Style.Panel.LineHeight, Offset.Left(parent1.GetWidth() + 60f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsGrowthPaused)).Do((Action<bool>) (s => growthSwitch.SetIsOn(s)));
      growthSwitch.AddTooltip((LocStrFormatted) Tr.AnimalFarm_PauseGrowth__Tooltip);
      BufferViewOneSlider animalBuffer = this.Builder.NewBufferWithOneSlider((IUiElement) itemContainer, new Action<float>(sliderValueChange), 10, "", customColor: new ColorRgba?(this.Builder.Style.Global.DangerClr), useImportSlider: false).AppendTo<BufferViewOneSlider>(itemContainer, new float?(this.Style.BufferView.HeightWithSlider));
      animalBuffer.ShowProductTitleOnHoverOnly(true);
      updaterBuilder.Observe<VirtualProductProto>((Func<VirtualProductProto>) (() => this.Entity.Prototype.Animal)).Observe<int>((Func<int>) (() => this.Entity.Prototype.AnimalsCapacity)).Observe<int>((Func<int>) (() => this.Entity.AnimalsCount)).Do((Action<VirtualProductProto, int, int>) ((animal, capacity, quantity) => animalBuffer.UpdateState((ProductProto) animal, capacity.Quantity(), quantity.Quantity())));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Entity.SlaughterStep)).Observe<bool>((Func<bool>) (() => this.Entity.IsSlaughteringEnabled)).Do((Action<int, bool>) ((slaughterStep, isSlaughteringEnabled) =>
      {
        animalBuffer.UpdateSlider(slaughterStep);
        animalBuffer.SetSliderVisibility(isSlaughteringEnabled);
      }));
      Panel parent2 = this.Builder.NewPanel("Overlay").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(35f));
      TextWithIcon animalGrowthDesc = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftTopOf<TextWithIcon>((IUiElement) parent2, new Vector2(200f, 25f), Offset.Left(20f) + Offset.Top(-15f));
      updaterBuilder.Observe<Fix32>((Func<Fix32>) (() => this.Entity.AnimalsBornPerMonth)).Do((Action<Fix32>) (bornPerMonth => animalGrowthDesc.SetPrefixText("+" + bornPerMonth.ToStringRounded(1) + " / 60")));
      StackContainer centerOf = this.Builder.NewStackContainer("Btns").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).PutToCenterOf<StackContainer>((IUiElement) parent2, 0.0f, Offset.Bottom(5f) + Offset.Right(20f));
      int numberToMove = 50;
      Btn add10 = this.Builder.NewBtnGeneral("Add").SetText(string.Format("+ {0}", (object) numberToMove)).AddToolTip(Tr.AnimalFarm_AddAnimals_Tooltip).OnClick((Action) (() => this.m_controller.Context.InputScheduler.ScheduleInputCmd<MoveAnimalsCmd>(new MoveAnimalsCmd(this.Entity, numberToMove))));
      add10.AppendTo<Btn>(centerOf, new float?(add10.GetOptimalWidth()));
      Btn objectToPlace1 = this.Builder.NewBtnGeneral("Remove").SetText(string.Format("- {0}", (object) numberToMove)).AddToolTip(Tr.AnimalFarm_RemoveAnimals_Tooltip).OnClick((Action) (() => this.m_controller.Context.InputScheduler.ScheduleInputCmd<MoveAnimalsCmd>(new MoveAnimalsCmd(this.Entity, -numberToMove))));
      objectToPlace1.AppendTo<Btn>(centerOf, new float?(objectToPlace1.GetOptimalWidth()));
      Btn objectToPlace2 = this.Builder.NewBtnGeneral("Remove").SetText((LocStrFormatted) Tr.AnimalFarm_RemoveAllAnimals).AddToolTip(Tr.AnimalFarm_RemoveAnimals_Tooltip).OnClick((Action) (() => this.m_controller.Context.InputScheduler.ScheduleInputCmd<MoveAnimalsCmd>(new MoveAnimalsCmd(this.Entity, -10000))));
      objectToPlace2.AppendTo<Btn>(centerOf, new float?(objectToPlace2.GetOptimalWidth()));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsEnabled)).Do((Action<bool>) (isEnabled => add10.SetEnabled(isEnabled)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.InputsTitle);
      BufferView foodBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new Vector2?(new Vector2(300f, this.Style.BufferView.CompactHeight)), ContainerPosition.LeftOrTop);
      TextWithIcon foodDesc = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) foodBuffer, new Vector2(200f, 25f), Offset.Left(100f));
      BufferView waterBuffer = this.Builder.NewBufferView((IUiElement) foodBuffer, isCompact: true).PutToLeftOf<BufferView>((IUiElement) foodBuffer, 300f, Offset.Left(300f));
      TextWithIcon waterDesc = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) foodBuffer, new Vector2(200f, 25f), Offset.Left(400f));
      updaterBuilder.Observe<AnimalFarmProto>((Func<AnimalFarmProto>) (() => this.Entity.Prototype)).Observe<int>((Func<int>) (() => this.Entity.AnimalsCount)).Do((Action<AnimalFarmProto, int>) ((proto, animalsCount) =>
      {
        Fix32 fix32_1 = proto.FoodPerAnimalPerMonth.Quantity.Value * animalsCount;
        Fix32 fix32_2 = proto.WaterPerAnimalPerMonth.Quantity.Value * animalsCount;
        foodDesc.SetPrefixText(Tr.Needs.TranslatedString + " " + fix32_1.ToStringRounded(1) + " / 60");
        waterDesc.SetPrefixText(Tr.Needs.TranslatedString + " " + fix32_2.ToStringRounded(1) + " / 60");
      }));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.FoodInputBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FoodInputBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FoodInputBuffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => foodBuffer.UpdateState(product, capacity, quantity)));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.WaterInputBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.WaterInputBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.WaterInputBuffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => waterBuffer.UpdateState(product, capacity, quantity)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.OutputsTitle);
      StackContainer outputsContainer = this.Builder.NewStackContainer("Output").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).AppendTo<StackContainer>(itemContainer);
      ViewsCacheHomogeneous<AnimalFarmWindowView.OutputBufferView> buffersCache = new ViewsCacheHomogeneous<AnimalFarmWindowView.OutputBufferView>((Func<AnimalFarmWindowView.OutputBufferView>) (() => new AnimalFarmWindowView.OutputBufferView((IUiElement) outputsContainer, this.Builder)));
      this.AddUpdater(buffersCache.Updater);
      updaterBuilder.Observe<IProductBuffer>((Func<ImmutableArray<IProductBuffer>>) (() => this.Entity.OutputBuffers), (ICollectionComparator<IProductBuffer, ImmutableArray<IProductBuffer>>) CompareFixedOrder<IProductBuffer>.Instance).Observe<AnimalFarm>((Func<AnimalFarm>) (() => this.Entity)).Do((Action<Lyst<IProductBuffer>, AnimalFarm>) ((buffers, entity) =>
      {
        outputsContainer.ClearAll();
        buffersCache.ReturnAll();
        outputsContainer.StartBatchOperation();
        foreach (IProductBuffer buffer in buffers)
          buffersCache.GetView().SetBuffer(buffer, entity).AppendTo<AnimalFarmWindowView.OutputBufferView>(outputsContainer);
        outputsContainer.FinishBatchOperation();
      }));
      BufferView carcassBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      TextWithIcon carcassDesc = new TextWithIcon(this.Builder).SetTextStyle(this.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) carcassBuffer, new Vector2(200f, 25f), Offset.Left(100f));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.CarcassBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CarcassBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CarcassBuffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((product, capacity, quantity) => carcassBuffer.UpdateState(product, capacity, quantity)));
      updaterBuilder.Observe<Fix32>((Func<Fix32>) (() => this.Entity.CarcassPerMonth)).Do((Action<Fix32>) (carcassPerMonth => carcassDesc.SetPrefixText(string.Format("{0}: {1} / 60", (object) Tr.Production, (object) carcassPerMonth.ToStringRounded(1)))));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.CarcassBuffer.Quantity.IsPositive || this.Entity.IsSlaughteringEnabled)).Do((Action<bool>) (isBufferVisible => itemContainer.SetItemVisibility((IUiElement) carcassBuffer, isBufferVisible)));
      updaterBuilder.Observe<AnimalFarm.State>((Func<AnimalFarm.State>) (() => this.Entity.CurrentState)).Do((Action<AnimalFarm.State>) (state =>
      {
        switch (state)
        {
          case AnimalFarm.State.Paused:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case AnimalFarm.State.Working:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case AnimalFarm.State.MissingWorkers:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case AnimalFarm.State.MissingFood:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__AnimalFarm_NoFood, StatusPanel.State.Critical);
            break;
          case AnimalFarm.State.MissingWater:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Farm_NoWater, StatusPanel.State.Critical);
            break;
          case AnimalFarm.State.NoAnimals:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__AnimalFarm_NoAnimals, StatusPanel.State.Warning);
            break;
          case AnimalFarm.State.FullOutput:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__FullOutput, StatusPanel.State.Critical);
            break;
        }
      }));
      this.AddUpdater(updaterBuilder.Build());

      void sliderValueChange(float value)
      {
        int num = (int) value;
        Assert.That<int>(num).IsWithinIncl(0, 10);
        this.m_controller.Context.InputScheduler.ScheduleInputCmd<AnimalFarmSetSlaughterLimitCmd>(new AnimalFarmSetSlaughterLimitCmd(this.Entity, new int?(num)));
      }
    }

    private class OutputBufferView : IUiElementWithUpdater, IUiElement
    {
      private BufferView m_bufferView;

      public GameObject GameObject => this.m_bufferView.GameObject;

      public RectTransform RectTransform => this.m_bufferView.RectTransform;

      private AnimalFarm Entity { get; set; }

      private IProductBuffer Buffer { get; set; }

      public IUiUpdater Updater { get; }

      public OutputBufferView(IUiElement parent, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_bufferView = builder.NewBufferView(parent, isCompact: true);
        TextWithIcon outputDesc = new TextWithIcon(builder, (IUiElement) this.m_bufferView).SetTextStyle(builder.Style.Panel.Text).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftBottomOf<TextWithIcon>((IUiElement) this.m_bufferView, new Vector2(200f, 25f), Offset.Left(100f));
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<AnimalFarmProto>((Func<AnimalFarmProto>) (() => this.Entity.Prototype)).Observe<int>((Func<int>) (() => this.Entity.AnimalsCount)).Do((Action<AnimalFarmProto, int>) ((proto, animalsCount) =>
        {
          if (proto.ProducedPerAnimalPerMonth.HasValue)
          {
            Fix32 fix32 = proto.ProducedPerAnimalPerMonth.Value.Quantity.Value * animalsCount;
            outputDesc.SetPrefixText(Tr.Production.TranslatedString + ": " + fix32.ToStringRounded(1) + " / 60");
          }
          else
            outputDesc.SetPrefixText("");
        }));
        updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Buffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Buffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Buffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(this.m_bufferView.UpdateState));
        this.Updater = updaterBuilder.Build();
        this.SetHeight<AnimalFarmWindowView.OutputBufferView>(builder.Style.BufferView.CompactHeight);
      }

      public AnimalFarmWindowView.OutputBufferView SetBuffer(
        IProductBuffer buffer,
        AnimalFarm entity)
      {
        this.Buffer = buffer;
        this.Entity = entity;
        return this;
      }
    }
  }
}
