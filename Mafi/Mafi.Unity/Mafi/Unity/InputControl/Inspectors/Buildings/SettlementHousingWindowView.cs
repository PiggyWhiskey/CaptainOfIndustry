// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.SettlementHousingWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Population;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class SettlementHousingWindowView : StaticEntityInspectorBase<SettlementHousingModule>
  {
    private readonly DependencyResolver m_resolver;
    private readonly SettlementHousingInspector m_controller;
    private ViewsCacheHomogeneous<SettlementHousingWindowView.UnityIncreaseView> m_unityIncreasesCache;
    private ViewsCacheHomogeneous<IconContainer> m_upgradeIconsCache;
    private ViewsCacheHomogeneous<SettlementHousingWindowView.NeedIncreaseView> m_needsIncreasesCache;

    protected override SettlementHousingModule Entity => this.m_controller.SelectedEntity;

    public SettlementHousingWindowView(
      SettlementHousingInspector controller,
      DependencyResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_resolver = resolver;
      this.m_controller = controller.CheckNotNull<SettlementHousingInspector>();
      this.SetWindowOffsetGroup(ItemDetailWindowView.WindowOffsetGroup.LargeScreen);
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_unityIncreasesCache = new ViewsCacheHomogeneous<SettlementHousingWindowView.UnityIncreaseView>((Func<SettlementHousingWindowView.UnityIncreaseView>) (() => new SettlementHousingWindowView.UnityIncreaseView(this.Builder, (Func<SettlementHousingModule>) (() => this.Entity))));
      this.m_upgradeIconsCache = new ViewsCacheHomogeneous<IconContainer>((Func<IconContainer>) (() => this.Builder.NewIconContainer("Icon")));
      this.m_needsIncreasesCache = new ViewsCacheHomogeneous<SettlementHousingWindowView.NeedIncreaseView>((Func<SettlementHousingWindowView.NeedIncreaseView>) (() => new SettlementHousingWindowView.NeedIncreaseView(this.Builder)));
      SettlementWindow element = this.m_resolver.Instantiate<SettlementWindow>();
      element.SetSettlementProvider((Func<Settlement>) (() => this.Entity.Settlement.Value));
      element.BuildUi(this.Builder, (IUiElement) this);
      this.AttachSidePanel((IWindow) element);
      element.Show();
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Occupants__Title);
      Panel parent = this.AddOverlayPanel(itemContainer);
      SomethingOutOfSomethingView occupantsView = new SomethingOutOfSomethingView((IUiElement) parent, this.Builder);
      occupantsView.First.SetIcon("Assets/Unity/UserInterface/General/Population.svg");
      occupantsView.Second.SetIcon("Assets/Unity/UserInterface/Toolbar/Settlement.svg");
      occupantsView.PutToLeftOf<SomethingOutOfSomethingView>((IUiElement) parent, 0.0f, Offset.Left(15f));
      Txt decorationsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) TrCore.UpointsCategory__DecorationsLong);
      Panel decorationsContainer = this.AddOverlayPanel(itemContainer);
      TextWithIcon decorationsUnity = new TextWithIcon(this.Builder).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetColor(this.Builder.Style.Global.GreenForDark);
      decorationsUnity.PutToLeftOf<TextWithIcon>((IUiElement) decorationsContainer, 0.0f, Offset.Left(15f));
      updaterBuilder.Observe<Upoints>((Func<Upoints>) (() => this.Entity.GetUpointsForDecorations())).Do((Action<Upoints>) (decorationUpoints =>
      {
        itemContainer.SetItemVisibility((IUiElement) decorationsTitle, decorationUpoints.IsNotZero);
        itemContainer.SetItemVisibility((IUiElement) decorationsContainer, decorationUpoints.IsNotZero);
        decorationsUnity.SetPrefixText(decorationUpoints.Format1Dec());
      }));
      Txt requirementsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.HousingUnityBonus, new LocStrFormatted?((LocStrFormatted) Tr.HousingUnityBonus__Tooltip));
      StackContainer unityIncreaseContainer = this.Builder.NewStackContainer("UnityIncreasesContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).AppendTo<StackContainer>(itemContainer);
      Txt increasedNeedsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.HousingDemandIncrease, new LocStrFormatted?((LocStrFormatted) Tr.HousingDemandIncrease__Tooltip));
      StackContainer increasesContainer = this.Builder.NewStackContainer("IncreasesContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).AppendTo<StackContainer>(itemContainer);
      updaterBuilder.Observe<KeyValuePair<PopNeedProto, Percent>>((Func<IEnumerable<KeyValuePair<PopNeedProto, Percent>>>) (() => (IEnumerable<KeyValuePair<PopNeedProto, Percent>>) this.Entity.Prototype.NeedsIncreases), (ICollectionComparator<KeyValuePair<PopNeedProto, Percent>, IEnumerable<KeyValuePair<PopNeedProto, Percent>>>) CompareFixedOrder<KeyValuePair<PopNeedProto, Percent>>.Instance).Do((Action<Lyst<KeyValuePair<PopNeedProto, Percent>>>) (needIncreases =>
      {
        increasesContainer.StartBatchOperation();
        increasesContainer.ClearAll();
        this.m_needsIncreasesCache.ReturnAll();
        foreach (KeyValuePair<PopNeedProto, Percent> needIncrease in needIncreases)
        {
          SettlementHousingWindowView.NeedIncreaseView view = this.m_needsIncreasesCache.GetView();
          view.SetIcon(needIncrease.Key.Graphics.IconPath);
          view.SetTitle(needIncrease.Key.Strings.Name.TranslatedString);
          view.SetPercent(needIncrease.Value);
          increasesContainer.Append((IUiElement) view, new float?(30f));
        }
        increasesContainer.FinishBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) increasedNeedsTitle, needIncreases.IsNotEmpty);
      }));
      updaterBuilder.Observe<SettlementHousingModuleProto>((Func<SettlementHousingModuleProto>) (() => this.Entity.Prototype)).Do((Action<SettlementHousingModuleProto>) (proto =>
      {
        unityIncreaseContainer.StartBatchOperation();
        unityIncreaseContainer.ClearAll();
        this.m_unityIncreasesCache.ReturnAll();
        for (int index = 0; index < proto.UnityIncreases.Length; ++index)
        {
          SettlementHousingWindowView.UnityIncreaseView view = this.m_unityIncreasesCache.GetView();
          view.SetIncreaseIndex(index);
          view.AppendTo<SettlementHousingWindowView.UnityIncreaseView>(unityIncreaseContainer, new float?(30f));
        }
        unityIncreaseContainer.FinishBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) requirementsTitle, proto.UnityIncreases.Length > 0);
        itemContainer.SetItemVisibility((IUiElement) unityIncreaseContainer, proto.UnityIncreases.Length > 0);
      }));
      updaterBuilder.Observe<int>((Func<int>) (() => this.Entity.Population)).Observe<int>((Func<int>) (() => this.Entity.Capacity)).Do((Action<int, int>) ((pops, cap) =>
      {
        occupantsView.First.SetPrefixText(pops.ToString());
        occupantsView.Second.SetPrefixText(cap.ToString());
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.AddUpdater(this.m_unityIncreasesCache.Updater);
    }

    internal class UnityIncreaseView : IUiElementWithUpdater, IUiElement
    {
      private readonly StackContainer m_container;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public int IncreaseIndex { get; private set; }

      public IUiUpdater Updater { get; }

      public UnityIncreaseView(UiBuilder builder, Func<SettlementHousingModule> entityProvider)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        SettlementHousingWindowView.UnityIncreaseView unityIncreaseView = this;
        ViewsCacheHomogeneous<IconContainer> iconsCache = new ViewsCacheHomogeneous<IconContainer>((Func<IconContainer>) (() => builder.NewIconContainer("Icon")));
        UiStyle style = builder.Style;
        this.m_container = builder.NewStackContainer("RequirementsContainer").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetBackground(style.Panel.ItemOverlay).SetItemSpacing(5f).SetInnerPadding(Offset.Left(10f));
        IconContainer arrow = builder.NewIconContainer("Arrow").SetIcon("Assets/Unity/UserInterface/General/Transform128.png");
        arrow.SetWidth<IconContainer>(20f);
        Txt increaseStatus = builder.NewTxt("unityIncreaseStatus").SetTextStyle(style.Global.Text).SetAlignment(TextAnchor.MiddleLeft);
        TextWithIcon extraUnity = new TextWithIcon(builder);
        extraUnity.SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg");
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        updaterBuilder.Observe<int>((Func<int>) (() => unityIncreaseView.IncreaseIndex)).Observe<SettlementHousingModuleProto>((Func<SettlementHousingModuleProto>) (() => entityProvider().Prototype)).Do((Action<int, SettlementHousingModuleProto>) ((index, proto) =>
        {
          if (index >= proto.UnityIncreases.Length)
            return;
          KeyValuePair<ImmutableArray<PopNeedProto>, Percent> unityIncrease = proto.UnityIncreases[index];
          Percent percent = unityIncrease.Value;
          closure_0.m_container.StartBatchOperation();
          closure_0.m_container.ClearAll();
          iconsCache.ReturnAll();
          foreach (PopNeedProto popNeedProto in unityIncrease.Key)
            iconsCache.GetView().SetIcon(popNeedProto.Graphics.IconPath).AppendTo<IconContainer>(closure_0.m_container, new float?(30f), Offset.TopBottom(5f));
          if (percent.IsPositive)
          {
            extraUnity.SetPrefixText(string.Format("+ {0}", (object) percent));
            closure_0.m_container.Append((IUiElement) arrow, new float?(arrow.GetWidth()), Offset.Left(5f));
            closure_0.m_container.Append((IUiElement) extraUnity, new float?(extraUnity.GetWidth()), Offset.Left(5f));
            closure_0.m_container.Append((IUiElement) increaseStatus, new float?(100f));
          }
          closure_0.m_container.FinishBatchOperation();
          extraUnity.SetVisibility<TextWithIcon>(percent.IsPositive);
          arrow.SetVisibility<IconContainer>(percent.IsPositive);
          increaseStatus.SetVisibility<Txt>(percent.IsPositive);
        }));
        updaterBuilder.Observe<int>((Func<int>) (() => entityProvider().AchievedUnityIncreaseIndexLastUpdate)).Do((Action<int>) (indexSatisfied =>
        {
          ColorRgba color;
          if (indexSatisfied < closure_0.IncreaseIndex)
          {
            color = ColorRgba.LightGray;
            increaseStatus.SetText("");
          }
          else if (indexSatisfied == closure_0.IncreaseIndex)
          {
            color = style.Global.GreenForDark;
            increaseStatus.SetText("(active)");
          }
          else
          {
            color = style.Global.OrangeText;
            increaseStatus.SetText("(not active)");
          }
          extraUnity.SetColor(color);
          increaseStatus.SetColor(color);
        }));
        this.Updater = updaterBuilder.Build();
      }

      public void SetIncreaseIndex(int index) => this.IncreaseIndex = index;
    }

    internal class NeedIncreaseView : IUiElement
    {
      public const int HEIGHT = 35;
      private readonly Panel m_container;
      private readonly Txt m_title;
      private readonly IconContainer m_icon;
      private readonly Txt m_percent;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public NeedIncreaseView(UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_container = builder.NewPanel("NeedInfo").SetBackground(builder.Style.Panel.ItemOverlay);
        int size = 25;
        this.m_icon = builder.NewIconContainer("ProductIcon").PutToLeftOf<IconContainer>((IUiElement) this.m_container, (float) size, Offset.Left(10f)).Hide<IconContainer>();
        this.m_title = builder.NewTxt("Title").SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleLeft).PutToLeftOf<Txt>((IUiElement) this.m_container, 200f, Offset.Left((float) (size + 10 + 5)));
        this.m_percent = builder.NewTxt("Percent").SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleRight).PutToRightOf<Txt>((IUiElement) this.m_container, 50f, Offset.Right(10f));
      }

      public void SetTitle(string title) => this.m_title.SetText(title);

      public void SetIcon(string iconPath) => this.m_icon.SetIcon(iconPath).Show<IconContainer>();

      public void SetPercent(Percent percent)
      {
        this.m_percent.SetText(string.Format("+{0}", (object) percent));
      }
    }
  }
}
