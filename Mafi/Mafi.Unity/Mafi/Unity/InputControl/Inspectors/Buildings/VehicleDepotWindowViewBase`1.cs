// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.VehicleDepotWindowViewBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Commands;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class VehicleDepotWindowViewBase<T> : StaticEntityInspectorBase<T> where T : VehicleDepotBase
  {
    private readonly IEntityInspector<T> m_controller;
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly IAssetTransactionManager m_assetsManager;
    private readonly Lyst<VehicleDepotWindowViewBase<T>.PerFuelView> m_perFuelViews;
    private readonly Set<DynamicGroundEntityProto> m_lastSeenUnlockedProtos;
    private VehicleDepotWindowViewBase<T>.BuildQueueView m_buildQueueView;
    private TabsContainer m_tabsContainer;

    protected override T Entity => this.m_controller.SelectedEntity;

    public VehicleDepotWindowViewBase(
      IEntityInspector<T> controller,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      IVehiclesManager vehiclesManager,
      IAssetTransactionManager assetsManager,
      IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_perFuelViews = new Lyst<VehicleDepotWindowViewBase<T>.PerFuelView>();
      this.m_lastSeenUnlockedProtos = new Set<DynamicGroundEntityProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      VehicleDepotWindowViewBase<T> depotWindowViewBase = this;
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_vehiclesManager = vehiclesManager;
      this.m_assetsManager = assetsManager;
      this.m_controller = controller.CheckNotNull<IEntityInspector<T>>();
      unlockedProtosDb.OnUnlockedSetChanged.AddNonSaveable<VehicleDepotWindowViewBase<T>>(this, (Action) (() => depotWindowViewBase.m_lastSeenUnlockedProtos.AddRange(unlockedProtosDb.AllUnlocked<DynamicGroundEntityProto>())));
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
    }

    private void initState()
    {
      this.m_lastSeenUnlockedProtos.AddRange(this.m_unlockedProtosDb.AllUnlocked<DynamicGroundEntityProto>());
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      IInputScheduler inputScheduler = this.m_controller.Context.InputScheduler;
      int width = 790;
      this.SetWidth((float) width);
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddLogisticsPanel(updaterBuilder, (Func<IEntityWithLogisticsControl>) (() => (IEntityWithLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      updaterBuilder.Observe<VehicleDepotBase.State>((Func<VehicleDepotBase.State>) (() => this.Entity.CurrentState)).Do((Action<VehicleDepotBase.State>) (state =>
      {
        switch (state)
        {
          case VehicleDepotBase.State.Idle:
            statusInfo.SetStatus(Tr.EntityStatus__Idle);
            break;
          case VehicleDepotBase.State.Paused:
            statusInfo.SetStatusPaused();
            break;
          case VehicleDepotBase.State.NotEnoughWorkers:
            statusInfo.SetStatusNoWorkers();
            break;
          case VehicleDepotBase.State.NotEnoughPower:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          case VehicleDepotBase.State.NotEnoughComputing:
            statusInfo.SetStatus(TrCore.EntityStatus__NoComputing, StatusPanel.State.Critical);
            break;
          case VehicleDepotBase.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          default:
            Log.Warning(string.Format("Unknown state {0}", (object) state));
            break;
        }
      }));
      this.m_buildQueueView = new VehicleDepotWindowViewBase<T>.BuildQueueView(this.Builder, this.m_controller, this.m_assetsManager, this.m_vehiclesManager);
      this.m_buildQueueView.AppendTo<VehicleDepotWindowViewBase<T>.BuildQueueView>(itemContainer, new float?(200f), Offset.LeftRight(10f) + Offset.Top(10f));
      DrivingEntityProto[] array1 = this.m_protosDb.All<DrivingEntityProto>().OrderBy<DrivingEntityProto, float>((Func<DrivingEntityProto, float>) (p => p.UIOrder)).ToArray<DrivingEntityProto>();
      ProductProto[] array2 = ((IEnumerable<DrivingEntityProto>) array1).Select<DrivingEntityProto, ProductProto>(new Func<DrivingEntityProto, ProductProto>(resolveFuelFor)).Distinct<ProductProto>().ToArray<ProductProto>();
      int height = 310;
      this.m_tabsContainer = this.Builder.NewTabsContainer(width, height, (IUiElement) itemContainer);
      this.m_tabsContainer.AppendTo<TabsContainer>(itemContainer, new float?((float) height), Offset.Top(5f));
      foreach (ProductProto productProto in array2)
      {
        VehicleDepotWindowViewBase<T>.PerFuelView perFuelView = new VehicleDepotWindowViewBase<T>.PerFuelView();
        this.m_perFuelViews.Add(perFuelView);
        this.m_tabsContainer.AddTab((LocStrFormatted) productProto.Strings.Name, new IconStyle?(new IconStyle(productProto.IconPath)), (Tab) perFuelView);
        foreach (DrivingEntityProto drivingEntityProto in array1)
        {
          if (!((Proto) resolveFuelFor(drivingEntityProto) != (Proto) productProto))
          {
            VehicleDepotWindowViewBase<T>.CardView element = new VehicleDepotWindowViewBase<T>.CardView(this.m_vehiclesManager, this.m_assetsManager, this.Builder, drivingEntityProto);
            element.BuildUi((Action<DrivingEntityProto>) (vehicle =>
            {
              bool flag = NotMappedShortcuts.IsBuildMultiple();
              inputScheduler.ScheduleInputCmd<AddVehicleToBuildQueueCmd>(new AddVehicleToBuildQueueCmd(vehicle, (VehicleDepotBase) this.Entity, flag ? 5 : 1));
            }));
            this.AddUpdater(element.Updater);
            perFuelView.GridContainer.Append((IUiElement) element);
            perFuelView.Cards.Add(element);
          }
        }
      }
      this.MakeMovable();
      this.PositionSelfToCenter();
      updaterBuilder.Observe<VehicleDepotBaseProto>((Func<VehicleDepotBaseProto>) (() => this.Entity.Prototype)).Observe<int>((Func<int>) (() => this.m_lastSeenUnlockedProtos.Count)).Do((Action<VehicleDepotBaseProto, int>) ((entityProto, unlockedProtosCount) => this.updateUnlockedProtos(entityProto)));
      this.AddUpdater(updaterBuilder.Build());
      this.AddUpdater(this.m_buildQueueView.Updater);

      static ProductProto resolveFuelFor(DrivingEntityProto proto)
      {
        return proto is RocketTransporterProto transporterProto ? (transporterProto.RocketProto is RocketProto rocketProto ? rocketProto.LaunchFuel.Product : (ProductProto) null) ?? ProductProto.Phantom : proto.FuelTankProto.ValueOrNull?.Product ?? ProductProto.Phantom;
      }
    }

    private void updateUnlockedProtos(VehicleDepotBaseProto depotProto)
    {
      foreach (VehicleDepotWindowViewBase<T>.PerFuelView perFuelView in this.m_perFuelViews)
      {
        int num = 0;
        foreach (VehicleDepotWindowViewBase<T>.CardView card in perFuelView.Cards)
        {
          bool isVisible = depotProto.BuildableEntities.Contains((DynamicGroundEntityProto) card.Vehicle) && this.m_lastSeenUnlockedProtos.Contains((DynamicGroundEntityProto) card.Vehicle);
          if (isVisible)
            ++num;
          perFuelView.GridContainer.SetItemVisibility((IUiElement) card, isVisible);
        }
        this.m_tabsContainer.SetTabVisibility((Tab) perFuelView, num > 0);
      }
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      this.m_tabsContainer.RenderUpdate(gameTime);
      base.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      this.m_tabsContainer.SyncUpdate(gameTime);
      base.SyncUpdate(gameTime);
    }

    private class PerFuelView : Tab
    {
      internal GridContainer GridContainer;
      public Lyst<VehicleDepotWindowViewBase<T>.CardView> Cards;

      public PerFuelView()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Cards = new Lyst<VehicleDepotWindowViewBase<T>.CardView>();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (PerFuelView<>));
      }

      protected override void BuildUi()
      {
        this.GridContainer = this.Builder.NewGridContainer("Cars container").SetCellSize(new Vector2(180f, 100f)).SetCellSpacing(10f).SetDynamicHeightMode(4);
        this.GridContainer.PutToCenterTopOf<GridContainer>((IUiElement) this, new Vector2(this.GridContainer.GetRequiredWidth(), 0.0f), Offset.TopBottom(8f));
        this.GridContainer.SizeChanged += (Action<IUiElement>) (e => this.SetHeight<VehicleDepotWindowViewBase<T>.PerFuelView>(this.GridContainer.GetRequiredHeight() + 16f));
      }
    }

    private class BuildQueueView : IUiElement
    {
      private readonly IEntityInspector<T> m_controller;
      private readonly Panel m_container;
      private readonly Lyst<IconContainer> m_iconContainers;
      private readonly Lyst<IconContainer> m_replaceIconContainers;
      private int m_queueLength;

      private VehicleDepotBase Entity => (VehicleDepotBase) this.m_controller.SelectedEntity;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public IUiUpdater Updater { get; }

      public BuildQueueView(
        UiBuilder builder,
        IEntityInspector<T> controller,
        IAssetTransactionManager assetManager,
        IVehiclesManager vehiclesManager)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_iconContainers = new Lyst<IconContainer>();
        this.m_replaceIconContainers = new Lyst<IconContainer>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        VehicleDepotWindowViewBase<T>.BuildQueueView buildQueueView = this;
        this.m_controller = controller;
        IInputScheduler inputScheduler = controller.Context.InputScheduler;
        ColorRgba color = (ColorRgba) 4144959;
        BorderStyle borderStyle = new BorderStyle((ColorRgba) 3487029);
        int coord = 60;
        int leftPanelSize = 100;
        ColorRgba iconColor = (ColorRgba) 15839548;
        this.m_container = builder.NewPanel("MainContainer").SetBackground(color).SetBorderStyle(borderStyle);
        Panel parent1 = builder.NewPanel("InnerContainer").PutTo<Panel>((IUiElement) this.m_container, Offset.All(5f));
        Panel leftTopOf = builder.NewPanel("IconBtn").PutToLeftTopOf<Panel>((IUiElement) parent1, new Vector2((float) (leftPanelSize - 5), (float) (coord + 30)));
        IconContainer icon1 = builder.NewIconContainer("Icon").PutToCenterTopOf<IconContainer>((IUiElement) leftTopOf, coord.Vector2());
        IconContainer replaceVehicleIcon = builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Upgrade.svg", iconColor).PutToLeftBottomOf<IconContainer>((IUiElement) icon1, 18.Vector2(), Offset.BottomLeft(5f, -4f)).Hide<IconContainer>();
        Btn cancelBtn = builder.NewBtnDanger("Cancel").SetText((LocStrFormatted) Tr.Cancel).OnClick((Action) (() => inputScheduler.ScheduleInputCmd<RemoveVehicleFromBuildQueueCmd>(new RemoveVehicleFromBuildQueueCmd(0, closure_1.Entity))));
        cancelBtn.PutToCenterBottomOf<Btn>((IUiElement) leftTopOf, cancelBtn.GetOptimalSize());
        ConstructionProgressView constructionView = new ConstructionProgressView((IUiElement) this.m_container, builder, (Func<Option<IConstructionProgress>>) (() => controller.SelectedEntity.VehicleConstructionProgress));
        constructionView.PutToBottomOf<ConstructionProgressView>((IUiElement) this.m_container, 135f, Offset.Left((float) leftPanelSize) + Offset.Bottom(5f)).Hide<ConstructionProgressView>();
        constructionView.AddQuickBuildBtn(assetManager, this.m_controller.Context.UpointsManager, (Action) (() => inputScheduler.ScheduleInputCmd<QuickBuildCurrentVehicleCmd>(new QuickBuildCurrentVehicleCmd(closure_1.Entity))));
        Txt objectToPlace = builder.NewTitle(Tr.StoredProduct__Title);
        float storedTitleSize = objectToPlace.GetPreferedWidth();
        ProductQuantitiesView storedProductsView = new ProductQuantitiesView((IUiElement) this.m_container, builder);
        objectToPlace.PutToLeftOf<Txt>((IUiElement) storedProductsView, storedTitleSize, Offset.Left(-storedTitleSize));
        storedProductsView.SizeChanged += (Action<IUiElement>) (element => storedProductsView.PutToLeftBottomOf<ProductQuantitiesView>((IUiElement) closure_1.m_container, element.GetSize(), Offset.Left((float) ((double) leftPanelSize + (double) storedTitleSize + 10.0))));
        storedProductsView.Hide<ProductQuantitiesView>();
        StackContainer topOf = builder.NewStackContainer("BuildQueue").SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(5f).PutToTopOf<StackContainer>((IUiElement) parent1, 40f, Offset.Left((float) leftPanelSize));
        for (int index1 = 0; index1 < 10; ++index1)
        {
          int index = index1;
          Btn parent2 = builder.NewBtn("PlaceHolder").SetButtonStyle(new BtnStyle(border: new BorderStyle?(builder.Style.Panel.Border))).OnClick((Action) (() =>
          {
            if (index >= closure_0.m_queueLength)
              return;
            inputScheduler.ScheduleInputCmd<RemoveVehicleFromBuildQueueCmd>(new RemoveVehicleFromBuildQueueCmd(index + 1, closure_0.Entity));
          })).AppendTo<Btn>(topOf, new float?(40f));
          this.m_iconContainers.Add(builder.NewIconContainer("Icon").PutTo<IconContainer>((IUiElement) parent2, Offset.All(1f)));
          this.m_replaceIconContainers.Add(builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Upgrade.svg", iconColor).PutToLeftBottomOf<IconContainer>((IUiElement) parent2, 12.Vector2(), Offset.BottomLeft(2f, 2f)).Hide<IconContainer>());
        }
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        Panel limitHolder = builder.NewPanel("limitReached").SetBackground(builder.Style.Panel.ItemOverlay).Hide<Panel>();
        Txt txt = builder.NewTxt("limitReached").SetText((LocStrFormatted) Tr.VehicleLimitReached).SetAlignment(TextAnchor.MiddleLeft).SetBackground(builder.Style.Panel.ItemOverlay).SetTextStyle(builder.Style.Global.TextInc.Extend(new ColorRgba?(builder.Style.Global.OrangeText))).PutTo<Txt>((IUiElement) limitHolder, Offset.Left(5f));
        limitHolder.PutToLeftTopOf<Panel>((IUiElement) constructionView, txt.GetPreferedSize() + 10.Vector2());
        updaterBuilder.Observe<bool>((Func<bool>) (() => (buildQueueView.Entity.BuildQueue.IsNotEmpty<DrivingEntityProto>() || buildQueueView.Entity.ReplaceQueue.IsNotEmpty<DrivingEntityProto>()) && !vehiclesManager.CanBuildVehicle((DynamicEntityProto) buildQueueView.Entity.CurrentlyBuildVehicle.Value))).Do((Action<bool>) (isOutOfLimit =>
        {
          limitHolder.SetVisibility<Panel>(isOutOfLimit);
          if (!limitHolder.IsVisible())
            return;
          limitHolder.SendToFront<Panel>();
        }));
        updaterBuilder.Observe<DrivingEntityProto>((Func<IIndexable<DrivingEntityProto>>) (() => buildQueueView.Entity.ReplaceQueue), (ICollectionComparator<DrivingEntityProto, IIndexable<DrivingEntityProto>>) CompareByCount<DrivingEntityProto>.Instance).Observe<DrivingEntityProto>((Func<IIndexable<DrivingEntityProto>>) (() => buildQueueView.Entity.BuildQueue), (ICollectionComparator<DrivingEntityProto, IIndexable<DrivingEntityProto>>) CompareByCount<DrivingEntityProto>.Instance).Do(new Action<Lyst<DrivingEntityProto>, Lyst<DrivingEntityProto>>(this.updateBuildQueue));
        updaterBuilder.Observe<int>((Func<int>) (() => buildQueueView.Entity.Buffers.Sum<IProductBufferReadOnly>((Func<IProductBufferReadOnly, int>) (x => x.Quantity.Value)))).Observe<Option<DrivingEntityProto>>((Func<Option<DrivingEntityProto>>) (() => buildQueueView.Entity.CurrentlyBuildVehicle)).Observe<bool>((Func<bool>) (() => buildQueueView.Entity.ReplaceQueue.IsNotEmpty<DrivingEntityProto>())).Do((Action<int, Option<DrivingEntityProto>, bool>) ((buffers, proto, isReplaceVehicle) =>
        {
          icon1.SetIcon(proto.IsNone ? builder.Style.Icons.Empty : proto.Value.Graphics.IconPath);
          icon1.SetColor(proto.IsNone ? (ColorRgba) 7763574 : ColorRgba.White);
          cancelBtn.SetVisibility<Btn>(proto.HasValue);
          constructionView.SetVisibility<ConstructionProgressView>(proto.HasValue);
          storedProductsView.SetVisibility<ProductQuantitiesView>(proto.IsNone);
          replaceVehicleIcon.SetVisibility<IconContainer>(isReplaceVehicle);
          if (!proto.IsNone)
            return;
          ImmutableArray<ProductQuantity> immutableArray = closure_1.Entity.Buffers.Where<IProductBufferReadOnly>((Func<IProductBufferReadOnly, bool>) (x => x.Quantity.IsPositive)).Select<IProductBufferReadOnly, ProductQuantity>((Func<IProductBufferReadOnly, ProductQuantity>) (x => new ProductQuantity(x.Product, x.Quantity))).ToImmutableArray<ProductQuantity>();
          storedProductsView.SetProducts(immutableArray);
          storedProductsView.SetVisibility<ProductQuantitiesView>(immutableArray.Length > 0);
        }));
        this.Updater = updaterBuilder.Build();
        this.Updater.AddChildUpdater(constructionView.Updater);
      }

      private void updateBuildQueue(
        Lyst<DrivingEntityProto> replaceQueue,
        Lyst<DrivingEntityProto> buildQueue)
      {
        int replacementsCount = replaceQueue.Count - 1;
        this.setProtosToQueue(replaceQueue.Concat<DrivingEntityProto>((IEnumerable<DrivingEntityProto>) buildQueue).Skip<DrivingEntityProto>(1), replacementsCount);
      }

      private void setProtosToQueue(
        IEnumerable<DrivingEntityProto> vehiclesProtos,
        int replacementsCount)
      {
        int index1 = 0;
        foreach (DrivingEntityProto vehiclesProto in vehiclesProtos)
        {
          if (index1 != 10)
          {
            this.m_replaceIconContainers[index1].SetVisibility<IconContainer>(index1 < replacementsCount);
            this.m_iconContainers[index1].SetIcon(vehiclesProto.Graphics.IconPath).Show<IconContainer>();
            ++index1;
          }
          else
            break;
        }
        this.m_queueLength = index1;
        for (int index2 = index1; index2 < 10; ++index2)
        {
          this.m_iconContainers[index2].Hide<IconContainer>();
          this.m_replaceIconContainers[index2].Hide<IconContainer>();
        }
      }
    }

    private class CardView : IUiElement
    {
      public readonly DrivingEntityProto Vehicle;
      private readonly IVehiclesManager m_vehiclesManager;
      private readonly IAssetTransactionManager m_assetManager;
      private readonly UiBuilder m_builder;
      private Panel m_card;
      private StackContainer m_stackContainer;
      private VehicleTooltip m_costTooltip;

      public GameObject GameObject => this.m_card.GameObject;

      public RectTransform RectTransform => this.m_card.RectTransform;

      public IUiUpdater Updater { get; private set; }

      public CardView(
        IVehiclesManager vehiclesManager,
        IAssetTransactionManager assetManager,
        UiBuilder builder,
        DrivingEntityProto vehicle)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehiclesManager = vehiclesManager;
        this.m_assetManager = assetManager;
        this.m_builder = builder;
        this.Vehicle = vehicle;
      }

      public IUiElement BuildUi(Action<DrivingEntityProto> onBuy)
      {
        UiStyle style = this.m_builder.Style;
        ColorRgba color = (ColorRgba) 4144959;
        ColorRgba colorRgba = (ColorRgba) 3487029;
        BorderStyle borderStyle = new BorderStyle(colorRgba);
        Vector2 vector2 = new Vector2(80f, 80f);
        this.m_card = this.m_builder.NewPanel("Card - " + this.Vehicle.Strings.Name.ToString()).SetBackground(color).SetBorderStyle(borderStyle);
        Panel topOf = this.m_builder.NewPanel("Header").SetBackground(colorRgba).PutToTopOf<Panel>((IUiElement) this.m_card, 25f);
        Txt title = this.m_builder.NewTxt("Title").SetText((LocStrFormatted) this.Vehicle.Strings.Name).SetTextStyle(style.Panel.SectionTitle).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) topOf, Offset.Left(style.Panel.PaddingCompact));
        this.m_stackContainer = this.m_builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutTo<StackContainer>((IUiElement) this.m_card, Offset.Top(topOf.GetHeight()) + Offset.Left(5f));
        this.m_builder.NewIconContainer("Icon").SetIcon(this.Vehicle.Graphics.IconPath).AppendTo<IconContainer>(this.m_stackContainer, new Vector2?(vector2), ContainerPosition.MiddleOrCenter, Offset.Right(10f));
        this.m_builder.NewPanel("Border").SetBackground(colorRgba).AppendTo<Panel>(this.m_stackContainer, new float?(1f));
        Btn btn1 = this.m_builder.NewBtn("Build");
        BtnStyle primaryBtn = this.m_builder.Style.Global.PrimaryBtn;
        ref BtnStyle local = ref primaryBtn;
        Offset? nullable = new Offset?(Offset.All(8f));
        TextStyle? text = new TextStyle?();
        BorderStyle? border = new BorderStyle?();
        ColorRgba? backgroundClr = new ColorRgba?();
        ColorRgba? normalMaskClr = new ColorRgba?();
        ColorRgba? hoveredClr = new ColorRgba?();
        ColorRgba? pressedClr = new ColorRgba?();
        ColorRgba? disabledMaskClr = new ColorRgba?();
        ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
        ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
        bool? shadow = new bool?();
        int? width = new int?();
        int? height = new int?();
        int? sidePaddings = new int?();
        Offset? iconPadding = nullable;
        BtnStyle buttonStyle = local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
        Btn btn2 = btn1.SetButtonStyle(buttonStyle).SetIcon("Assets/Unity/UserInterface/General/Build.svg", 20.Vector2()).OnClick((Action) (() => onBuy(this.Vehicle)));
        btn2.PutToRightMiddleOf<Btn>((IUiElement) this.m_stackContainer, new Vector2(btn2.GetOptimalSize().x, btn2.GetOptimalSize().y + 12f), Offset.Right(18f));
        this.m_costTooltip = new VehicleTooltip(this.m_builder, this.m_assetManager);
        this.m_costTooltip.SetData(this.Vehicle.CostToBuild, (LocStrFormatted) this.Vehicle.Strings.DescShort, this.Vehicle.GetFuelConsumedPer60());
        this.m_costTooltip.AttachTo<Btn>((IUiElementWithHover<Btn>) btn2);
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<VehicleStats>((Func<VehicleStats>) (() => this.m_vehiclesManager.GetStats((DynamicEntityProto) this.Vehicle))).Do((Action<VehicleStats>) (stats => title.SetText(string.Format("{0} ({1})", (object) this.Vehicle.Strings.Name, (object) stats.Owned))));
        this.Updater = updaterBuilder.Build();
        this.Updater.AddChildUpdater(this.m_costTooltip.CreateUpdater());
        return (IUiElement) this.m_card;
      }
    }
  }
}
