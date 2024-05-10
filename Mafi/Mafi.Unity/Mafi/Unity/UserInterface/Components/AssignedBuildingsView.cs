// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.AssignedBuildingsView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components.VehiclesAssigner;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class AssignedBuildingsView : IUiElement, IDynamicSizeElement
  {
    private readonly CameraController m_cameraController;
    private readonly IInputScheduler m_inputScheduler;
    private static readonly Vector2 ICON_SIZE;
    private Lyst<IEntityAssignedAsInput> m_assignedInputBuildings;
    private Lyst<IEntityAssignedAsOutput> m_assignedOutputBuildings;
    private IEntity m_entity;
    private readonly GridContainer m_grid;
    private readonly AssignedElementIcon.Cache m_iconsCache;
    private readonly UiUnLockerForTech m_uiLocker;
    private readonly bool m_isForInputs;
    private readonly Panel m_prefixIcon;
    private readonly Panel m_btnHolder;
    private readonly Panel m_container;
    private readonly Txt m_noItemsText;
    private readonly AssignedElementIcon m_entityIcon;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; private set; }

    public AssignedBuildingsView(
      IUiElement parent,
      CameraController cameraController,
      IInputScheduler inputScheduler,
      UnlockedProtosDbForUi unlockedProtosDb,
      ProtosDb protosDb,
      UiBuilder builder,
      Action onBtnClick,
      Func<IEntityAssignedAsOutput> outputEntityProvider,
      Func<IEntityAssignedAsInput> inputEntityProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      AssignedBuildingsView element = this;
      this.m_cameraController = cameraController;
      this.m_inputScheduler = inputScheduler;
      this.m_uiLocker = new UiUnLockerForTech(unlockedProtosDb, protosDb);
      this.m_iconsCache = new AssignedElementIcon.Cache(builder, "", new Action<int>(this.onItemClick), new Action<int>(this.onItemRightClick));
      this.m_container = builder.NewPanel("GridContainerWithBtn", parent).SetBackground(builder.Style.Panel.ItemOverlay);
      this.m_grid = builder.NewGridContainer("Items").SetDynamicHeightMode().SetCellSpacing(3f).SetCellSize(AssignedBuildingsView.ICON_SIZE).SetInnerPadding(Offset.TopBottom(3f)).PutToTopOf<GridContainer>((IUiElement) this.m_container, 0.0f, Offset.Left(10f));
      this.m_entityIcon = new AssignedElementIcon(builder, "", (Action<int>) (_ => { }));
      this.m_entityIcon.PutToLeftTopOf<AssignedElementIcon>((IUiElement) this, 0.Vector2());
      this.m_btnHolder = builder.NewPanel("Prefix").PutTo<Panel>((IUiElement) this);
      Btn btn = builder.NewBtnGeneral("PlusBtn").OnClick(onBtnClick).SetIcon("Assets/Unity/UserInterface/General/PlusMinus.svg").PutTo<Btn>((IUiElement) this.m_btnHolder, Offset.All(5f));
      builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) btn);
      this.m_noItemsText = builder.NewTxt("NoItemsInfo").SetTextStyle(builder.Style.Global.Text).SetText(string.Format("({0})", (object) Tr.AssignedForLogistics__Empty)).SetAlignment(TextAnchor.MiddleLeft);
      this.m_noItemsText.PutToRightOf<Txt>((IUiElement) btn, this.m_noItemsText.GetPreferedWidth(), Offset.Right((float) (-(double) this.m_noItemsText.GetPreferedWidth() - 20.0))).Hide<Txt>();
      this.m_grid.SizeChanged += (Action<IUiElement>) (x =>
      {
        element.SetHeight<AssignedBuildingsView>(Mathf.Max(element.m_grid.ComputeHeightFor(1), element.m_grid.GetHeight()));
        Action<IUiElement> sizeChanged = element.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) element);
      });
      this.m_prefixIcon = builder.NewPanel("Prefix").PutTo<Panel>((IUiElement) this);
      IconContainer iconContainer = builder.NewIconContainer("Prefix").PutTo<IconContainer>((IUiElement) this.m_prefixIcon, Offset.All(10f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      if (outputEntityProvider != null)
      {
        iconContainer.SetIcon("Assets/Unity/UserInterface/General/DoubleArrowsRight.svg", (ColorRgba) 4300609);
        updaterBuilder.Observe<IEntityAssignedAsInput>((Func<IReadOnlyCollection<IEntityAssignedAsInput>>) (() => (IReadOnlyCollection<IEntityAssignedAsInput>) outputEntityProvider().AssignedInputs), (ICollectionComparator<IEntityAssignedAsInput, IReadOnlyCollection<IEntityAssignedAsInput>>) CompareByCount<IEntityAssignedAsInput>.Instance).Observe<IEntityAssignedAsOutput>((Func<IEntityAssignedAsOutput>) (() => outputEntityProvider())).Do(new Action<Lyst<IEntityAssignedAsInput>, IEntityAssignedAsOutput>(this.rebuildAssignedInputs));
      }
      else if (inputEntityProvider != null)
      {
        iconContainer.SetIcon("Assets/Unity/UserInterface/General/DoubleArrowsLeft.svg", (ColorRgba) 12320768);
        updaterBuilder.Observe<IEntityAssignedAsOutput>((Func<IReadOnlyCollection<IEntityAssignedAsOutput>>) (() => (IReadOnlyCollection<IEntityAssignedAsOutput>) inputEntityProvider().AssignedOutputs), (ICollectionComparator<IEntityAssignedAsOutput, IReadOnlyCollection<IEntityAssignedAsOutput>>) CompareByCount<IEntityAssignedAsOutput>.Instance).Observe<IEntityAssignedAsInput>((Func<IEntityAssignedAsInput>) (() => inputEntityProvider())).Do(new Action<Lyst<IEntityAssignedAsOutput>, IEntityAssignedAsInput>(this.rebuildAssignedOutputs));
      }
      this.Updater = updaterBuilder.Build();
    }

    public void SetupVisibilityHook(Txt title, StackContainer parentContainer)
    {
      this.m_uiLocker.SetupVisibilityHook(IdsCore.Technology.CustomRoutes, parentContainer, (IUiElement) this, (IUiElement) title);
    }

    private void onItemClick(int index)
    {
      if (this.m_assignedInputBuildings != null)
      {
        if (index < 0 || index >= this.m_assignedInputBuildings.Count)
          return;
        this.m_cameraController.PanTo(this.m_assignedInputBuildings[index].Position2f);
      }
      else
      {
        if (this.m_assignedOutputBuildings == null || index < 0 || index >= this.m_assignedOutputBuildings.Count)
          return;
        this.m_cameraController.PanTo(this.m_assignedOutputBuildings[index].Position2f);
      }
    }

    private void onItemRightClick(int index)
    {
      if (this.m_entity == null)
        return;
      if (this.m_assignedInputBuildings != null)
      {
        this.m_inputScheduler.ScheduleInputCmd<UnassignStaticEntityCmd>(new UnassignStaticEntityCmd((IEntityAssignedAsOutput) this.m_entity, this.m_assignedInputBuildings[index]));
      }
      else
      {
        if (this.m_assignedOutputBuildings == null)
          return;
        this.m_inputScheduler.ScheduleInputCmd<UnassignStaticEntityCmd>(new UnassignStaticEntityCmd(this.m_assignedOutputBuildings[index], (IEntityAssignedAsInput) this.m_entity));
      }
    }

    private void rebuildAssignedInputs(
      Lyst<IEntityAssignedAsInput> assignedBuildings,
      IEntityAssignedAsOutput entity)
    {
      this.m_assignedInputBuildings = assignedBuildings;
      this.m_entity = (IEntity) entity;
      this.m_grid.StartBatchOperation();
      this.m_grid.ClearAll();
      this.m_iconsCache.ReturnAll();
      this.m_entityIcon.SetIcon(entity.Prototype.Graphics.IconPath).AppendTo<AssignedElementIcon>(this.m_grid);
      this.m_prefixIcon.AppendTo<Panel>(this.m_grid);
      for (int index = 0; index < assignedBuildings.Count; ++index)
      {
        LayoutEntityProto prototype = assignedBuildings[index].Prototype;
        this.m_iconsCache.GetView().SetIcon(prototype.Graphics.IconPath).AppendTo<AssignedElementIcon>(this.m_grid).Index = index;
      }
      this.m_btnHolder.AppendTo<Panel>(this.m_grid);
      this.m_grid.FinishBatchOperation();
      this.m_noItemsText.SetVisibility<Txt>(assignedBuildings.IsEmpty);
    }

    private void rebuildAssignedOutputs(
      Lyst<IEntityAssignedAsOutput> assignedBuildings,
      IEntityAssignedAsInput entity)
    {
      this.m_assignedOutputBuildings = assignedBuildings;
      this.m_entity = (IEntity) entity;
      this.m_grid.StartBatchOperation();
      this.m_grid.ClearAll();
      this.m_iconsCache.ReturnAll();
      this.m_entityIcon.SetIcon(entity.Prototype.Graphics.IconPath).AppendTo<AssignedElementIcon>(this.m_grid);
      this.m_prefixIcon.AppendTo<Panel>(this.m_grid);
      for (int index = 0; index < assignedBuildings.Count; ++index)
      {
        LayoutEntityProto prototype = assignedBuildings[index].Prototype;
        this.m_iconsCache.GetView().SetIcon(prototype.Graphics.IconPath).AppendTo<AssignedElementIcon>(this.m_grid).Index = index;
      }
      this.m_btnHolder.AppendTo<Panel>(this.m_grid);
      this.m_grid.FinishBatchOperation();
      this.m_noItemsText.SetVisibility<Txt>(assignedBuildings.IsEmpty);
    }

    static AssignedBuildingsView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AssignedBuildingsView.ICON_SIZE = new Vector2(46f, 46f);
    }
  }
}
