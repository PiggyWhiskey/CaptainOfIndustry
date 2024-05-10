// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CaptainOfficeWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.Input;
using Mafi.Core.Population.Edicts;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Population;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class CaptainOfficeWindowView : StaticEntityInspectorBase<CaptainOffice>
  {
    private readonly CaptainOfficeInspector m_controller;
    private readonly EdictsManager m_edictsManager;
    private readonly IInputScheduler m_inputScheduler;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private StatusPanel m_statusInfo;
    private TabsContainer m_tabsContainer;

    protected override CaptainOffice Entity => this.m_controller.SelectedEntity;

    public CaptainOfficeWindowView(
      CaptainOfficeInspector controller,
      EdictsManager edictsManager,
      IInputScheduler inputScheduler,
      UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_edictsManager = edictsManager;
      this.m_inputScheduler = inputScheduler;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_controller = controller.CheckNotNull<CaptainOfficeInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_statusInfo = this.AddStatusInfoPanel();
      int height = this.ResolveWindowSize().y.Clamp(412.3f, 545.3f).CeilToInt();
      int num = 3;
      int width = ((float) ((double) (num * 345) + (double) (num - 1) * 5.0 + 5.0 + 20.0)).CeilToInt();
      this.m_tabsContainer = this.Builder.NewTabsContainer(width, height, (IUiElement) itemContainer);
      this.m_tabsContainer.AppendTo<TabsContainer>(itemContainer, new float?((float) height), Offset.Top(5f));
      foreach (EdictCategoryProto edictCategory in this.m_controller.Context.ProtosDb.All<EdictCategoryProto>())
      {
        CaptainOfficeWindowView.EdictCategoryTab edictCategoryTab = new CaptainOfficeWindowView.EdictCategoryTab(this.m_controller, this.m_edictsManager, this.m_inputScheduler, this.m_unlockedProtosDb, edictCategory);
        this.m_tabsContainer.AddTab(edictCategory.Strings.Name.TranslatedString, new IconStyle?(), (Tab) edictCategoryTab);
      }
      updaterBuilder.Observe<CaptainOffice.State>((Func<CaptainOffice.State>) (() => this.Entity.CurrentState)).Do((Action<CaptainOffice.State>) (state =>
      {
        switch (state)
        {
          case CaptainOffice.State.None:
          case CaptainOffice.State.Working:
            this.m_statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case CaptainOffice.State.Paused:
            this.m_statusInfo.SetStatusPaused();
            break;
          case CaptainOffice.State.NotEnoughWorkers:
            this.m_statusInfo.SetStatusNoWorkers();
            break;
          case CaptainOffice.State.NotEnoughPower:
            this.m_statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
        }
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth((float) width);
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

    internal class EdictCategoryTab : Tab
    {
      public const float CELL_SPACING = 5f;
      public const float LEFT_OFFSET = 5f;
      public const float RIGHT_OFFSET = 20f;
      private readonly CaptainOfficeInspector m_controller;
      private readonly EdictsManager m_edictsManager;
      private readonly IInputScheduler m_inputScheduler;
      private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
      private readonly EdictCategoryProto m_edictCategory;
      private GridContainer m_edictsContainer;
      private readonly Lyst<EdictTogglePanel> m_hiddenEdicts;

      public EdictCategoryTab(
        CaptainOfficeInspector controller,
        EdictsManager edictsManager,
        IInputScheduler inputScheduler,
        UnlockedProtosDbForUi unlockedProtosDb,
        EdictCategoryProto edictCategory)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_hiddenEdicts = new Lyst<EdictTogglePanel>();
        // ISSUE: explicit constructor call
        base.\u002Ector("EdictsTab");
        this.m_edictsManager = edictsManager;
        this.m_inputScheduler = inputScheduler;
        this.m_unlockedProtosDb = unlockedProtosDb;
        this.m_edictCategory = edictCategory;
        this.m_controller = controller.CheckNotNull<CaptainOfficeInspector>();
      }

      protected override void BuildUi()
      {
        this.m_edictsContainer = this.Builder.NewGridContainer("Edicts").SetCellSpacing(5f).SetDynamicHeightMode(3).SetInnerPadding(Offset.Left(5f) + Offset.Right(20f) + Offset.Top(5f)).PutToLeftTopOf<GridContainer>((IUiElement) this, 0.Vector2());
        ImmutableArray<Edict> allEdicts = this.m_edictsManager.AllEdicts;
        this.m_edictsContainer.StartBatchOperation();
        bool flag = true;
        foreach (Edict firstEdict in allEdicts)
        {
          if (!firstEdict.Prototype.PreviousTier.HasValue && !((Proto) firstEdict.Prototype.Category != (Proto) this.m_edictCategory))
          {
            EdictTogglePanel element = new EdictTogglePanel((IUiElement) this.m_edictsContainer, this.Builder, this.m_inputScheduler, this.m_unlockedProtosDb, firstEdict, allEdicts);
            if (flag)
            {
              this.m_edictsContainer.SetCellSize(element.GetSize());
              flag = false;
            }
            this.m_edictsContainer.Append((IUiElement) element);
            this.m_edictsContainer.HideItem((IUiElement) element);
            this.m_hiddenEdicts.Add(element);
            this.AddUpdater(element.Updater);
          }
        }
        this.m_edictsContainer.FinishBatchOperation();
        this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.updateEdictsVisibility);
        this.updateEdictsVisibility();
      }

      private void updateEdictsVisibility()
      {
        foreach (EdictTogglePanel element in this.m_hiddenEdicts.ToArray())
        {
          if (this.m_unlockedProtosDb.IsUnlocked((IProto) element.FirstEdict.Prototype))
          {
            this.m_hiddenEdicts.Remove(element);
            this.m_edictsContainer.ShowItem((IUiElement) element);
          }
        }
        this.SetSize<CaptainOfficeWindowView.EdictCategoryTab>(new Vector2(this.m_edictsContainer.GetRequiredWidth(), this.m_edictsContainer.GetRequiredHeight()));
        if (!this.m_hiddenEdicts.IsEmpty)
          return;
        this.m_unlockedProtosDb.OnUnlockedSetChangedForUi -= new Action(this.updateEdictsVisibility);
      }
    }
  }
}
