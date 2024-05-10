// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ForestryTowerWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
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
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class ForestryTowerWindowView : StaticEntityInspectorBase<ForestryTower>
  {
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly ImmutableArray<TreePlantingGroupProto> m_allTreeGroups;
    private readonly ForestryTowerInspector m_controller;
    private readonly VehiclesAssignerView<TreePlanterProto> m_treePlantersAssigner;
    private readonly VehiclesAssignerView<TreeHarvesterProto> m_treeHarvestersAssigner;
    private Txt m_areaInfo;
    private Panel m_startEditCont;
    private Lyst<ForestryTowerWindowView.CutAtBtn> m_cutAtBtns;

    protected override ForestryTower Entity => this.m_controller.SelectedEntity;

    public ForestryTowerWindowView(
      ForestryTowerInspector controller,
      VehiclesAssignerView<TreePlanterProto> treePlantersAssigner,
      VehiclesAssignerView<TreeHarvesterProto> treeHarvestersAssigner,
      UnlockedProtosDb unlockedProtosDb,
      ImmutableArray<TreePlantingGroupProto> allTreeGroups)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_allTreeGroups = allTreeGroups;
      this.m_treePlantersAssigner = treePlantersAssigner.CheckNotNull<VehiclesAssignerView<TreePlanterProto>>();
      this.m_treeHarvestersAssigner = treeHarvestersAssigner.CheckNotNull<VehiclesAssignerView<TreeHarvesterProto>>();
      this.m_controller = controller.CheckNotNull<ForestryTowerInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.AddHeaderTutorialButton(this.m_controller.Context, IdsCore.Messages.TutorialOnTreesPlanting);
      base.AddCustomItems(itemContainer);
      this.SetWidth(520f);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Panel parent1 = this.Builder.NewPanel("ManagerArea").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(this.ItemsContainer, new float?(40f));
      this.m_startEditCont = this.Builder.NewPanel("StartEdit").PutTo<Panel>((IUiElement) parent1, Offset.LeftRight(this.Style.Panel.Indent));
      this.m_areaInfo = this.Builder.NewTxt("AreaInfo").SetTextStyle(this.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).SetText("").PutTo<Txt>((IUiElement) this.m_startEditCont, Offset.Right(120f));
      Btn btn1 = this.Builder.NewBtnPrimary("Btn").EnableDynamicSize().SetText((LocStrFormatted) TrCore.ManagedDesignation__EditAction).SetIcon("Assets/Unity/UserInterface/EntityIcons/Sapling.svg", 22.Vector2()).OnClick(new Action(this.m_controller.ToggleDesignationEditing));
      btn1.PutToRightMiddleOf<Btn>((IUiElement) this.m_startEditCont, btn1.GetSize());
      Btn btn2 = this.Builder.NewBtnGeneral("Btn").EnableDynamicSize().SetText((LocStrFormatted) TrCore.ManagedArea__EditAction).SetIcon("Assets/Unity/UserInterface/Toolbox/SelectArea.svg", 22.Vector2()).OnClick(new Action(this.m_controller.ToggleAreaEditing));
      btn2.PutToRightMiddleOf<Btn>((IUiElement) this.m_startEditCont, btn2.GetSize(), Offset.Right(btn1.GetWidth() + 10f));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Trees__HarvestingOptions, new LocStrFormatted?((LocStrFormatted) Tr.Trees__HarvestingOptionsTooltip));
      Panel parent2 = this.Builder.NewPanel("InnerContainer", (IUiElement) itemContainer).AppendTo<Panel>(itemContainer, new float?(60f));
      StackContainer leftOf = this.Builder.NewStackContainer("CutOptions").SetItemSpacing(5f).SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.RightToLeft).PutToLeftOf<StackContainer>((IUiElement) parent2, 0.0f, Offset.Left(10f));
      string iconPath = this.m_controller.Context.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Wood).Graphics.IconPath;
      this.m_cutAtBtns = new Lyst<ForestryTowerWindowView.CutAtBtn>()
      {
        new ForestryTowerWindowView.CutAtBtn(this.Builder, ForestryTower.NO_CUT_AT, new Action<Percent>(this.setCutPercentage), (Option<string>) Option.None),
        new ForestryTowerWindowView.CutAtBtn(this.Builder, 40.Percent(), new Action<Percent>(this.setCutPercentage), (Option<string>) iconPath),
        new ForestryTowerWindowView.CutAtBtn(this.Builder, 60.Percent(), new Action<Percent>(this.setCutPercentage), (Option<string>) iconPath),
        new ForestryTowerWindowView.CutAtBtn(this.Builder, 80.Percent(), new Action<Percent>(this.setCutPercentage), (Option<string>) iconPath),
        new ForestryTowerWindowView.CutAtBtn(this.Builder, 100.Percent(), new Action<Percent>(this.setCutPercentage), (Option<string>) iconPath)
      };
      foreach (ForestryTowerWindowView.CutAtBtn cutAtBtn in this.m_cutAtBtns)
        cutAtBtn.AppendTo<ForestryTowerWindowView.CutAtBtn>(leftOf, new float?(60f));
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => this.Entity.TargetHarvestPercent)).Do((Action<Percent>) (cutAt =>
      {
        foreach (ForestryTowerWindowView.CutAtBtn cutAtBtn in this.m_cutAtBtns)
          cutAtBtn.SetSelectedIffEquals(cutAt);
      }));
      this.AddVehiclesAssigner<TreePlanterProto>(this.m_treePlantersAssigner, (LocStrFormatted) Tr.AssignedTreePlanters__Title, new LocStrFormatted?((LocStrFormatted) Tr.AssignedTreePlanters__ForestryTower_Title));
      this.AddVehiclesAssigner<TreeHarvesterProto>(this.m_treeHarvestersAssigner, (LocStrFormatted) Tr.AssignedTreeHarvesters__Title, new LocStrFormatted?((LocStrFormatted) Tr.AssignedTreeHarvesters__ForestryTower_Title));
      this.AddBuildingsAssignerForExport(this.m_controller.Context, new Action(((EntityInspector<ForestryTower, ForestryTowerWindowView>) this.m_controller).EditInputBuildingsClicked), (Func<IEntityAssignedAsOutput>) (() => (IEntityAssignedAsOutput) this.Entity), (LocStrFormatted) Tr.AssignedForLogistics__ExportTooltipForestryTower);
      this.AddBuildingsAssignerForImport(this.m_controller.Context, new Action(((EntityInspector<ForestryTower, ForestryTowerWindowView>) this.m_controller).EditOutputBuildingsClicked), (Func<IEntityAssignedAsInput>) (() => (IEntityAssignedAsInput) this.Entity), (LocStrFormatted) Tr.AssignedForLogistics__ImportTooltipForestryTower, updaterBuilder);
      updaterBuilder.Observe<RectangleTerrainArea2i>((Func<RectangleTerrainArea2i>) (() => this.Entity.Area)).Do(new Action<RectangleTerrainArea2i>(this.updateAreaInfo));
      this.AddUpdater(updaterBuilder.Build());
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_startEditCont.SetVisibility<Panel>(!this.m_controller.AreaEditInProgress);
    }

    private void updateAreaInfo(RectangleTerrainArea2i area)
    {
      int num1 = area.Size.X * 2;
      int num2 = area.Size.Y * 2;
      this.m_areaInfo.SetText(Tr.ManagedArea__Info.Format(string.Format("{0}m x {1}m", (object) num1, (object) num2)));
    }

    private void plusBtnClicked()
    {
    }

    private void setCutPercentage(Percent value) => this.m_controller.SetCutPercentage(value);

    public void SetTreeProtoFromInspector(IProtoWithIconAndName product)
    {
      this.setTreeProtoFromProductPicker(product);
    }

    private void setTreeProtoFromProductPicker(IProtoWithIconAndName product)
    {
      if (!(product is TreePlantingGroupProto product1))
      {
        Log.Error(string.Format("Trying to set UI product which isn't a TreeProto '{0}'", (object) product));
      }
      else
      {
        this.m_controller.SetTreePlantingGroupProto((Option<TreePlantingGroupProto>) product1);
        this.m_cutAtBtns[1].SetData(((float) product1.ProductWhenHarvested.Quantity.Value * 0.4f).FloorToInt().ToString(), product1.TimeTo40PercentGrowth);
        this.m_cutAtBtns[2].SetData(((float) product1.ProductWhenHarvested.Quantity.Value * 0.6f).FloorToInt().ToString(), product1.TimeTo60PercentGrowth);
        ForestryTowerWindowView.CutAtBtn cutAtBtn1 = this.m_cutAtBtns[3];
        int num = ((float) product1.ProductWhenHarvested.Quantity.Value * 0.8f).FloorToInt();
        string yield1 = num.ToString();
        Duration to80PercentGrowth = product1.TimeTo80PercentGrowth;
        cutAtBtn1.SetData(yield1, to80PercentGrowth);
        ForestryTowerWindowView.CutAtBtn cutAtBtn2 = this.m_cutAtBtns[4];
        num = product1.ProductWhenHarvested.Quantity.Value;
        string yield2 = num.ToString();
        Duration to100PercentGrowth = product1.TimeTo100PercentGrowth;
        cutAtBtn2.SetData(yield2, to100PercentGrowth);
        this.SetTreePlantingGroupProto((Option<TreePlantingGroupProto>) product1);
      }
    }

    public void SetTreePlantingGroupProto(Option<TreePlantingGroupProto> product)
    {
    }

    public class CutAtBtn : IUiElement
    {
      private readonly UiBuilder m_builder;
      private readonly Percent m_cutAt;
      private readonly Btn m_btn;
      private readonly Option<TextWithIcon> m_textWithIcon;
      private readonly Tooltip m_tooltip;
      private readonly BtnStyle m_selectedStyle;

      public GameObject GameObject => this.m_btn.GameObject;

      public RectTransform RectTransform => this.m_btn.RectTransform;

      public CutAtBtn(
        UiBuilder builder,
        Percent cutAt,
        Action<Percent> onClick,
        Option<string> iconPath)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_builder = builder;
        this.m_cutAt = cutAt;
        BtnStyle listMenuBtnSelected = builder.Style.Global.ListMenuBtnSelected;
        ref BtnStyle local = ref listMenuBtnSelected;
        BorderStyle? nullable = new BorderStyle?(new BorderStyle((ColorRgba) 10123554));
        TextStyle? text = new TextStyle?();
        BorderStyle? border = nullable;
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
        Offset? iconPadding = new Offset?();
        this.m_selectedStyle = local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
        this.m_btn = builder.NewBtn(nameof (CutAtBtn)).OnClick((Action) (() => onClick(cutAt))).SetButtonStyle(builder.Style.Global.ListMenuBtnDarker);
        this.m_tooltip = this.m_btn.AddToolTipAndReturn();
        Txt topOf = builder.NewTitle("Text").SetText(cutAt.ToStringRounded()).SetAlignment(TextAnchor.MiddleCenter).PutToTopOf<Txt>((IUiElement) this.m_btn, 25f, Offset.Top(5f));
        if (iconPath.HasValue)
        {
          topOf.PutToTopOf<Txt>((IUiElement) this.m_btn, 25f, Offset.Top(5f));
          TextWithIcon objectToPlace = new TextWithIcon(builder, 18);
          this.m_textWithIcon = (Option<TextWithIcon>) objectToPlace;
          objectToPlace.SetIcon(iconPath.Value).SetTextStyle(builder.Style.Global.Title);
          objectToPlace.PutToBottomOf<TextWithIcon>((IUiElement) this.m_btn, 25f, Offset.Bottom(5f));
        }
        else
        {
          builder.NewIconContainer("NoCut").SetIcon("Assets/Unity/UserInterface/General/NoTreeCut.svg").PutTo<IconContainer>((IUiElement) this.m_btn, Offset.All(10f));
          topOf.Hide<Txt>();
          this.m_tooltip.SetText((LocStrFormatted) Tr.Trees__NoCut);
        }
      }

      public void SetData(string yield, Duration growthDuration)
      {
        ref readonly LocStr1Plural local = ref TrCore.NumberOfYears;
        Fix32 years = growthDuration.Years;
        string stringRoundedAdaptive = years.ToStringRoundedAdaptive();
        years = growthDuration.Years;
        Fix64 fix64 = years.ToFix64();
        string str = local.Format(stringRoundedAdaptive, fix64).Value;
        this.m_tooltip.SetText(string.Format("{0}: ", (object) Tr.Trees__CutAfter) + str);
        this.m_textWithIcon.ValueOrNull?.SetPrefixText(yield);
      }

      public void SetSelected(bool isSelected)
      {
        this.m_btn.SetButtonStyle(isSelected ? this.m_selectedStyle : this.m_builder.Style.Global.ListMenuBtnDarker);
        this.m_btn.SetEnabled(!isSelected);
      }

      public void SetSelectedIffEquals(Percent cutAt) => this.SetSelected(this.m_cutAt == cutAt);
    }
  }
}
