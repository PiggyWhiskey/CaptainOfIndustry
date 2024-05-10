// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.MineTowerWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
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
  internal class MineTowerWindowView : StaticEntityInspectorBase<MineTower>
  {
    private readonly ImmutableArray<ProductProto> m_allDumpableProducts;
    private readonly MineTowerInspector m_controller;
    private readonly VehiclesAssignerView<ExcavatorProto> m_excavatorsAssigner;
    private readonly VehiclesAssignerView<TruckProto> m_trucksAssigner;
    private ProtosFilterEditor<ProductProto> m_filterView;
    private ProtosFilterEditor<ProductProto> m_notificationsFilterView;
    private Txt m_areaInfo;
    private Panel m_startEditCont;
    private Panel m_stopEditCont;
    private readonly string m_mineTowerName;

    protected override MineTower Entity => this.m_controller.SelectedEntity;

    public MineTowerWindowView(
      MineTowerInspector controller,
      VehiclesAssignerView<ExcavatorProto> excavatorsAssigner,
      VehiclesAssignerView<TruckProto> trucksAssigner,
      ImmutableArray<ProductProto> allDumpableProducts)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_allDumpableProducts = allDumpableProducts;
      this.m_excavatorsAssigner = excavatorsAssigner.CheckNotNull<VehiclesAssignerView<ExcavatorProto>>();
      this.m_trucksAssigner = trucksAssigner.CheckNotNull<VehiclesAssignerView<TruckProto>>();
      this.m_controller = controller.CheckNotNull<MineTowerInspector>();
      this.m_mineTowerName = controller.Context.ProtosDb.All<MineTowerProto>().FirstOrDefault<MineTowerProto>()?.Strings.Name.TranslatedString ?? "Mine tower";
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.MakeScrollableWithHeightLimit();
      this.AddHeaderTutorialButton(this.m_controller.Context, IdsCore.Messages.TutorialOnMineTower);
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Panel parent = this.Builder.NewPanel("ManagerArea").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(this.ItemsContainer, new float?(40f));
      this.m_startEditCont = this.Builder.NewPanel("StartEdit").PutTo<Panel>((IUiElement) parent, Offset.LeftRight(this.Style.Panel.Indent));
      this.m_areaInfo = this.Builder.NewTxt("AreaInfo").SetTextStyle(this.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).SetText("").PutTo<Txt>((IUiElement) this.m_startEditCont, Offset.Right(120f));
      Btn btn = this.Builder.NewBtnPrimary("Btn").EnableDynamicSize().SetText((LocStrFormatted) TrCore.ManagedArea__EditAction).SetIcon("Assets/Unity/UserInterface/Toolbox/SelectArea.svg", 22.Vector2()).OnClick(new Action(this.m_controller.ToggleAreaEditing));
      btn.PutToRightMiddleOf<Btn>((IUiElement) this.m_startEditCont, btn.GetSize());
      this.m_stopEditCont = this.Builder.NewPanel("StopEdit").Hide<Panel>().PutTo<Panel>((IUiElement) parent, Offset.LeftRight(this.Style.Panel.Indent));
      this.Builder.NewTxt("AreaInfo").SetTextStyle(this.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).SetText("Click and drag with mouse to change.").PutTo<Txt>((IUiElement) this.m_stopEditCont, Offset.Right(120f));
      this.Builder.NewBtnDanger("Btn").SetText("Cancel").SetButtonStyle(this.Style.Global.GeneralBtn).OnClick(new Action(this.m_controller.ToggleAreaEditing)).PutToRightMiddleOf<Btn>((IUiElement) this.m_stopEditCont, new Vector2(100f, 26f));
      this.AddVehiclesAssigner<ExcavatorProto>(this.m_excavatorsAssigner, (LocStrFormatted) Tr.AssignedExcavators__Title, new LocStrFormatted?((LocStrFormatted) Tr.AssignedExcavators__MineTower_Title));
      this.AddVehiclesAssigner<TruckProto>(this.m_trucksAssigner, (LocStrFormatted) Tr.AssignedTrucks__Title, new LocStrFormatted?((LocStrFormatted) Tr.AssignedTrucks__MineTower_Tooltip));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.DumpingFilter__Title, new LocStrFormatted?((LocStrFormatted) Tr.DumpingFilter__Tooltip));
      this.m_filterView = new ProtosFilterEditor<ProductProto>(this.Builder, (IWindowWithInnerWindowsSupport) this, this.ItemsContainer, new Action<ProductProto>(this.m_controller.RemoveDumpableProduct), new Action<ProductProto>(this.m_controller.AddDumpableProduct), new Func<IEnumerable<ProductProto>>(this.m_allDumpableProducts.AsEnumerable), (Func<IEnumerable<ProductProto>>) (() => (IEnumerable<ProductProto>) this.m_controller.SelectedEntity.DumpableProducts), usePrimaryBtnStyle: false);
      this.m_filterView.SetTextToShowWhenEmpty(string.Format("({0})", (object) Tr.DumpingFilter__Empty));
      this.SetWidth(this.m_filterView.GetRequiredWidth() + 30f);
      updaterBuilder.Observe<ProductProto>((Func<IReadOnlyCollection<ProductProto>>) (() => (IReadOnlyCollection<ProductProto>) this.Entity.DumpableProducts), (ICollectionComparator<ProductProto, IReadOnlyCollection<ProductProto>>) CompareByCount<ProductProto>.Instance).Do(new Action<Lyst<ProductProto>>(this.m_filterView.UpdateFilteredProtos));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.MineTowerNotifyFilter__Title, new LocStrFormatted?((LocStrFormatted) Tr.MineTowerNotifyFilter__Tooltip));
      this.m_notificationsFilterView = new ProtosFilterEditor<ProductProto>(this.Builder, (IWindowWithInnerWindowsSupport) this, this.ItemsContainer, (Action<ProductProto>) (p =>
      {
        if (p != null)
          this.m_controller.Context.InputScheduler.ScheduleInputCmd<RemoveProductToNotifyIfCannotDumpCmd>(new RemoveProductToNotifyIfCannotDumpCmd(this.Entity, p));
        else
          Log.Error(string.Format("Trying to add UI product which isn't a ProductProto '{0}'", (object) p));
      }), (Action<ProductProto>) (p =>
      {
        if (p != null)
          this.m_controller.Context.InputScheduler.ScheduleInputCmd<AddProductToNotifyIfCannotDumpCmd>(new AddProductToNotifyIfCannotDumpCmd(this.Entity, p));
        else
          Log.Error(string.Format("Trying to add UI product which isn't a ProductProto '{0}'", (object) p));
      }), new Func<IEnumerable<ProductProto>>(this.m_allDumpableProducts.AsEnumerable), (Func<IEnumerable<ProductProto>>) (() => (IEnumerable<ProductProto>) this.m_controller.SelectedEntity.ProductsToNotifyIfCannotGetRidOf), usePrimaryBtnStyle: false);
      this.m_notificationsFilterView.SetTextToShowWhenEmpty(string.Format("({0})", (object) Tr.MineTowerNotifyFilter__Empty));
      this.SetWidth((this.m_filterView.GetRequiredWidth() + 30f).Max(this.m_notificationsFilterView.GetRequiredWidth().Max(480f)));
      updaterBuilder.Observe<ProductProto>((Func<IReadOnlyCollection<ProductProto>>) (() => (IReadOnlyCollection<ProductProto>) this.Entity.ProductsToNotifyIfCannotGetRidOf), (ICollectionComparator<ProductProto, IReadOnlyCollection<ProductProto>>) CompareByCount<ProductProto>.Instance).Do(new Action<Lyst<ProductProto>>(this.m_notificationsFilterView.UpdateFilteredProtos));
      this.AddBuildingsAssignerForExport(this.m_controller.Context, new Action(((EntityInspector<MineTower, MineTowerWindowView>) this.m_controller).EditInputBuildingsClicked), (Func<IEntityAssignedAsOutput>) (() => (IEntityAssignedAsOutput) this.Entity), Tr.AssignedForLogistics__ExportTooltipMineTower.Format(this.m_mineTowerName));
      this.AddBuildingsAssignerForImport(this.m_controller.Context, new Action(((EntityInspector<MineTower, MineTowerWindowView>) this.m_controller).EditOutputBuildingsClicked), (Func<IEntityAssignedAsInput>) (() => (IEntityAssignedAsInput) this.Entity), Tr.AssignedForLogistics__ImportTooltipMineTower.Format(this.m_mineTowerName), updaterBuilder);
      updaterBuilder.Observe<RectangleTerrainArea2i>((Func<RectangleTerrainArea2i>) (() => this.Entity.Area)).Do(new Action<RectangleTerrainArea2i>(this.updateAreaInfo));
      this.AddUpdater(updaterBuilder.Build());
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_startEditCont.SetVisibility<Panel>(!this.m_controller.AreaEditInProgress);
      this.m_stopEditCont.SetVisibility<Panel>(this.m_controller.AreaEditInProgress);
    }

    private void updateAreaInfo(RectangleTerrainArea2i area)
    {
      int num1 = area.Size.X * 2;
      int num2 = area.Size.Y * 2;
      this.m_areaInfo.SetText(Tr.ManagedArea__Info.Format(string.Format("{0}m x {1}m", (object) num1, (object) num2)));
    }
  }
}
