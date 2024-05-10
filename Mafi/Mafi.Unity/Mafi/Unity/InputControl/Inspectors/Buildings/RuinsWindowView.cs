// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.RuinsWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Buildings.RuinedBuildings;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class RuinsWindowView : StaticEntityInspectorBase<Mafi.Core.Buildings.RuinedBuildings.Ruins>
  {
    private readonly RuinsInspector m_controller;
    private readonly ProtosDb m_protosDb;

    protected override Mafi.Core.Buildings.RuinedBuildings.Ruins Entity
    {
      get => this.m_controller.SelectedEntity;
    }

    public RuinsWindowView(RuinsInspector controller, ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_protosDb = protosDb;
      this.m_controller = controller.CheckNotNull<RuinsInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Panel parent = this.AddOverlayPanel(itemContainer, 70, Offset.Bottom(10f));
      LocStrFormatted text = TrCore.RuinsRecycleFormatted__Tooltip.Format(this.m_protosDb.GetOrThrow<ProductProto>((Proto.ID) Ids.Products.IronScrap).Strings.Name, this.m_protosDb.GetOrThrow<MachineProto>((Proto.ID) Ids.Machines.SmeltingFurnaceT1).Strings.Name);
      this.Builder.NewTxt("Desc").SetTextStyle(this.Builder.Style.Global.TextInc).BestFitEnabled(this.Builder.Style.Global.TextInc.FontSize).SetAlignment(TextAnchor.MiddleCenter).SetText(text).PutTo<Txt>((IUiElement) parent, Offset.LeftRight(5f));
      Panel initialInfo = this.Builder.NewPanel("Info").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(80f));
      StackContainer initialInfoContainer = this.Builder.NewStackContainer("InfoCont").SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(10f).PutToLeftOf<StackContainer>((IUiElement) initialInfo, 0.0f, Offset.Left(20f));
      Txt objectToPlace1 = this.Builder.NewTxt("Provides").SetText((LocStrFormatted) Tr.Provides).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft);
      objectToPlace1.AppendTo<Txt>(initialInfoContainer, new float?(objectToPlace1.GetPreferedWidth()));
      ProtoWithIcon<ProductProto>[] productIcons = new ProtoWithIcon<ProductProto>[3]
      {
        new ProtoWithIcon<ProductProto>((IUiElement) initialInfoContainer, this.Builder),
        new ProtoWithIcon<ProductProto>((IUiElement) initialInfoContainer, this.Builder),
        new ProtoWithIcon<ProductProto>((IUiElement) initialInfoContainer, this.Builder)
      };
      foreach (ProtoWithIcon<ProductProto> objectToPlace2 in productIcons)
        objectToPlace2.AppendTo<ProtoWithIcon<ProductProto>>(initialInfoContainer, new float?(80f));
      Btn objectToPlace3 = this.Builder.NewBtnPrimary("StartRecycling").SetText((LocStrFormatted) TrCore.RuinsRecycle__Action).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<RuinsScrapCmd>(new RuinsScrapCmd(this.Entity.Id, !this.Entity.IsScrapping))));
      objectToPlace3.AppendTo<Btn>(initialInfoContainer, new Vector2?(new Vector2(objectToPlace3.GetOptimalWidth() + 10f, 30f)), ContainerPosition.MiddleOrCenter);
      ConstructionProgressView scrapView = new ConstructionProgressView((IUiElement) itemContainer, this.Builder, (Func<Option<IConstructionProgress>>) (() => this.Entity.ScrapProgress.SomeOption<IConstructionProgress>()));
      scrapView.SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<ConstructionProgressView>(itemContainer, new float?(95f)).Hide<ConstructionProgressView>();
      Btn deconstrBtn = this.Builder.NewBtnPrimary("ScrapBtn").SetButtonStyle(this.Style.Global.GeneralBtn).SetText("X");
      Vector2 size = deconstrBtn.GetOptimalSize() + new Vector2(40f, 0.0f);
      deconstrBtn.SetSize<Btn>(size).AppendTo<Btn>(itemContainer, new Vector2?(size), ContainerPosition.MiddleOrCenter, Offset.Top(10f)).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<RuinsScrapCmd>(new RuinsScrapCmd(this.Entity.Id, !this.Entity.IsScrapping))));
      updaterBuilder.Observe<AssetValue>((Func<AssetValue>) (() => this.Entity.Prototype.ProductsGiven)).Do((Action<AssetValue>) (assetValue =>
      {
        foreach (IUiElement element in productIcons)
          initialInfoContainer.HideItem(element);
        for (int index = 0; index < assetValue.Products.Length; ++index)
        {
          if (index >= productIcons.Length)
          {
            Log.Error("Too many products, not supported in the UI");
            break;
          }
          productIcons[index].SetProto((Option<ProductProto>) assetValue.Products[index].Product);
          initialInfoContainer.ShowItem((IUiElement) productIcons[index]);
        }
        if ((double) this.Width >= (double) initialInfoContainer.GetDynamicWidth() + 40.0)
          return;
        this.SetWidth(initialInfoContainer.GetDynamicWidth() + 40f);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsScrapping)).Observe<Percent>((Func<Percent>) (() => this.Entity.ScrapProgress.Progress)).Do((Action<bool, Percent>) ((isDeconstructing, progress) =>
      {
        bool isVisible = !isDeconstructing && progress == Percent.Hundred;
        itemContainer.SetItemVisibility((IUiElement) initialInfo, isVisible);
        itemContainer.SetItemVisibility((IUiElement) scrapView, !isVisible);
        itemContainer.SetItemVisibility((IUiElement) deconstrBtn, !isVisible);
        deconstrBtn.SetText((LocStrFormatted) (isDeconstructing ? Tr.Pause : Tr.Continue));
        deconstrBtn.SetWidth<Btn>(deconstrBtn.GetOptimalWidth());
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.AddUpdater(scrapView.Updater);
    }
  }
}
