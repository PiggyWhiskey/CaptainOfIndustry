// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.UpgradeToolWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  internal class UpgradeToolWindowView : WindowView
  {
    private StackContainer m_itemsContainer;
    private ViewsCacheHomogeneous<UpgradeToolEntityRow> m_entityViewCache;
    private Panel m_upgradeContainer;
    private Btn m_upgradeBtn;
    private readonly LazyResolve<UpgradeToolInputController> m_controller;
    private readonly IInputScheduler m_inputScheduler;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly UpgradeCostResolver m_upgradeCostResolver;
    private readonly IAssetTransactionManager m_assetsManager;
    private CostTooltip m_costTooltip;
    private ScrollableStackContainer m_scrollableStackContainer;

    internal UpgradeToolWindowView(
      LazyResolve<UpgradeToolInputController> controller,
      IInputScheduler inputScheduler,
      UpgradeCostResolver upgradeCostResolver,
      IAssetTransactionManager assetsManager,
      UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("UpgradeTool", WindowView.FooterStyle.None);
      this.m_controller = controller;
      this.m_inputScheduler = inputScheduler;
      this.m_upgradeCostResolver = upgradeCostResolver;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_assetsManager = assetsManager;
    }

    protected override void BuildWindowContent()
    {
      UiStyle style = this.Builder.Style;
      this.SetTitle((LocStrFormatted) TrCore.UpgradeTool);
      this.PositionSelfToCenter();
      this.MakeMovable();
      this.m_itemsContainer = this.Builder.NewStackContainer("SelectUpgrades").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).SetInnerPadding(Offset.TopBottom(10f)).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      this.m_scrollableStackContainer = new ScrollableStackContainer(this.Builder, (float) ((double) this.ResolveWindowSize().y - 50.0 - 200.0), this.m_itemsContainer);
      this.m_scrollableStackContainer.PutToTopOf<ScrollableStackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      this.m_scrollableStackContainer.SizeChanged += (Action<IUiElement>) (e =>
      {
        float width = this.m_entityViewCache.AllExistingOnes().IsEmpty<UpgradeToolEntityRow>() ? 200f : this.m_entityViewCache.AllExistingOnes().First<UpgradeToolEntityRow>().GetDynamicWidth();
        if (this.m_scrollableStackContainer.IsScrollingNeeded)
          width += 20f;
        this.SetContentSize(width, e.GetHeight() + 50f);
      });
      this.m_upgradeContainer = this.Builder.NewPanel("UpgradeContainer").SetBackground(style.Panel.ItemOverlay).PutToBottomOf<Panel>((IUiElement) this.GetContentPanel(), 50f);
      this.m_upgradeBtn = this.Builder.NewBtnPrimary("UpgradeBtn", (IUiElement) this.m_upgradeContainer).SetText((LocStrFormatted) Tr.Upgrade).SetIcon("Assets/Unity/UserInterface/Toolbar/Upgrade.svg", 16.Vector2()).PlayErrorSoundWhenDisabled().OnClick(new Action(this.onUpgradeClick));
      this.m_upgradeBtn.PutToLeftMiddleOf<Btn>((IUiElement) this.m_upgradeContainer, this.m_upgradeBtn.GetOptimalSize(), Offset.Left(15f));
      this.m_costTooltip = new CostTooltip(this.Builder, this.m_assetsManager);
      this.m_costTooltip.AttachTo<Btn>((IUiElementWithHover<Btn>) this.m_upgradeBtn);
      this.AddUpdater(this.m_costTooltip.CreateUpdater());
      this.m_entityViewCache = new ViewsCacheHomogeneous<UpgradeToolEntityRow>((Func<UpgradeToolEntityRow>) (() => new UpgradeToolEntityRow((IUiElement) this.m_itemsContainer, this.Builder, this.m_assetsManager, this.m_unlockedProtosDb, new Action(this.updateCost))));
      this.AddUpdater(this.m_entityViewCache.Updater);
    }

    public void SetEntities(IIndexable<IAreaSelectableEntity> entities)
    {
      this.m_itemsContainer.StartBatchOperation();
      this.m_itemsContainer.ClearAll();
      this.m_entityViewCache.ReturnAll();
      foreach (IGrouping<StaticEntityProto.ID, IUpgradableEntity> source in entities.AsEnumerable().OfType<IUpgradableEntity>().GroupBy<IUpgradableEntity, StaticEntityProto.ID, IUpgradableEntity>((Func<IUpgradableEntity, StaticEntityProto.ID>) (x => x.Prototype.Id), (Func<IUpgradableEntity, IUpgradableEntity>) (x => x)))
      {
        UpgradeToolEntityRow view = this.m_entityViewCache.GetView();
        view.SetEntity(source.AsEnumerable<IUpgradableEntity>());
        this.m_itemsContainer.Append((IUiElement) view, new float?(view.GetHeight()));
      }
      this.m_itemsContainer.FinishBatchOperation();
      this.updateCost();
    }

    private void onUpgradeClick()
    {
      foreach (UpgradeToolEntityRow allExistingOne in this.m_entityViewCache.AllExistingOnes())
        allExistingOne.OnUpgrade(this.m_inputScheduler);
      this.m_controller.Value.DeactivateSelf();
    }

    private void updateCost()
    {
      AssetValue empty = AssetValue.Empty;
      bool enabled = false;
      foreach (UpgradeToolEntityRow allExistingOne in this.m_entityViewCache.AllExistingOnes())
      {
        empty += allExistingOne.GetCost();
        enabled |= allExistingOne.IsUpgradeSelected();
      }
      this.m_costTooltip.SetCost(empty);
      this.m_upgradeBtn.SetEnabled(enabled);
    }
  }
}
