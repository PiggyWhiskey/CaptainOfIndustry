// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.IconsWithTitlesView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.ResearchLab;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Syncers;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  /// <summary>
  /// Side panel component that displays units of type <see cref="T:Mafi.Core.UnlockingTree.IUnlockUnitWithTitleAndIcon" />.
  /// </summary>
  public class IconsWithTitlesView
  {
    private readonly UiBuilder m_builder;
    private readonly StackContainer m_parent;
    private readonly Txt m_unlocksTitle;
    private readonly ResearchPopupController m_popupController;
    private readonly StackContainer m_unlocksContainer;
    private readonly Lyst<IconsWithTitlesView.ItemsData> m_itemsDataCache;

    internal IconsWithTitlesView(
      UiBuilder builder,
      StackContainer parent,
      Txt title,
      ResearchPopupController popupController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_itemsDataCache = new Lyst<IconsWithTitlesView.ItemsData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_parent = parent;
      this.m_unlocksTitle = title;
      this.m_popupController = popupController;
      this.m_unlocksContainer = this.m_builder.NewStackContainer("Unlocks", (IUiElement) this.m_parent).SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(5f).AppendTo<StackContainer>(this.m_parent, new float?(0.0f));
    }

    public IUiUpdater CreateUpdater(Func<IUnlockingNode> nodeProvider)
    {
      return UpdaterBuilder.Start().Observe<IUnlockingNode>(nodeProvider).Do((Action<IUnlockingNode>) (node => this.update(node.Units.Where((Func<IUnlockNodeUnit, bool>) (x => !x.HideInUI)).OfType<IUnlockUnitWithTitleAndIcon>().Select<IUnlockUnitWithTitleAndIcon, IconsWithTitlesView.ItemsData>((Func<IUnlockUnitWithTitleAndIcon, IconsWithTitlesView.ItemsData>) (x =>
      {
        Option<IProto> prototype = Option<IProto>.None;
        if (x is ProtoUnlock protoUnlock2)
          prototype = protoUnlock2.UnlockedProtos.FirstOrDefault().CreateOption<IProto>();
        return new IconsWithTitlesView.ItemsData(x.Title, x.Description, x.IconPath.ValueOr(""), true, prototype);
      }))))).Build();
    }

    public IUiUpdater CreateUpdaterForRequiredProtos(
      Func<ResearchNode> nodeProvider,
      UnlockedProtosDbForUi unlockedProtosDb)
    {
      Lyst<Pair<IResearchNodeUnlockingCondition, bool>> conditions = new Lyst<Pair<IResearchNodeUnlockingCondition, bool>>();
      return UpdaterBuilder.Start().Observe<Pair<IResearchNodeUnlockingCondition, bool>>((Func<IIndexable<Pair<IResearchNodeUnlockingCondition, bool>>>) (() =>
      {
        ResearchNode researchNode = nodeProvider();
        conditions.Clear();
        foreach (IResearchNodeUnlockingCondition unlockingCondition in researchNode.Proto.UnlockingConditions)
          conditions.Add(new Pair<IResearchNodeUnlockingCondition, bool>(unlockingCondition, researchNode.LockedByConditions.Contains(unlockingCondition)));
        return (IIndexable<Pair<IResearchNodeUnlockingCondition, bool>>) conditions;
      }), (ICollectionComparator<Pair<IResearchNodeUnlockingCondition, bool>, IIndexable<Pair<IResearchNodeUnlockingCondition, bool>>>) CompareFixedOrder<Pair<IResearchNodeUnlockingCondition, bool>>.Instance).Observe<ResearchNode>(nodeProvider).Do((Action<Lyst<Pair<IResearchNodeUnlockingCondition, bool>>, ResearchNode>) ((unlocksData, node) =>
      {
        this.m_itemsDataCache.Clear();
        foreach (Pair<IResearchNodeUnlockingCondition, bool> pair in unlocksData)
        {
          IResearchNodeUnlockingCondition first = pair.First;
          bool second = pair.Second;
          switch (first)
          {
            case UnlockingConditionProtoRequired conditionProtoRequired2:
              string icon = !(conditionProtoRequired2.ProtoRequired is ResearchNodeProto) ? (conditionProtoRequired2.ProtoRequired is IProtoWithIcon protoRequired2 ? protoRequired2.IconPath : (string) null) ?? "" : "Assets/Unity/UserInterface/Toolbar/Research.svg";
              this.m_itemsDataCache.Add(new IconsWithTitlesView.ItemsData((LocStrFormatted) conditionProtoRequired2.ProtoRequired.Strings.Name, (LocStrFormatted) conditionProtoRequired2.ProtoRequired.Strings.DescShort, icon, !second, Option<IProto>.None));
              continue;
            case UnlockingConditionGlobalStats conditionGlobalStats2:
              this.m_itemsDataCache.Add(new IconsWithTitlesView.ItemsData(new LocStrFormatted(string.Format("{0} / {1}", (object) conditionGlobalStats2.CurrentQuantity, (object) conditionGlobalStats2.QuantityRequired)), (LocStrFormatted) conditionGlobalStats2.Tooltip, conditionGlobalStats2.ProductToTrack.Graphics.IconPath, !second, Option<IProto>.None));
              continue;
            default:
              Log.Error(string.Format("Unknown condition implementation {0}", (object) first.GetType()));
              continue;
          }
        }
        if (node.LabRequired.HasValue)
        {
          ResearchLabProto researchLabProto = node.LabRequired.Value;
          this.m_itemsDataCache.Add(new IconsWithTitlesView.ItemsData(new LocStrFormatted(string.Format("{0} or better", (object) researchLabProto.Strings.Name)), (LocStrFormatted) researchLabProto.Strings.DescShort, researchLabProto.Graphics.IconPath, unlockedProtosDb.IsUnlocked((IProto) researchLabProto), Option<IProto>.None));
        }
        this.update((IEnumerable<IconsWithTitlesView.ItemsData>) this.m_itemsDataCache);
      })).Build();
    }

    private void update(
      IEnumerable<IconsWithTitlesView.ItemsData> pathToIconMap)
    {
      this.m_unlocksContainer.StartBatchOperation();
      this.m_unlocksContainer.ClearAndDestroyAll();
      int num = 0;
      foreach (IconsWithTitlesView.ItemsData pathToIcon in pathToIconMap)
      {
        ++num;
        this.createUnlockIcon((IUiElement) this.m_unlocksContainer, pathToIcon).AppendTo<Panel>(this.m_unlocksContainer, new float?((float) this.m_builder.Style.Research.NodeDetailUnlockStripeHeight));
      }
      this.m_unlocksContainer.FinishBatchOperation();
      this.m_parent.SetItemVisibility((IUiElement) this.m_unlocksTitle, num > 0);
    }

    private Panel createUnlockIcon(IUiElement parent, IconsWithTitlesView.ItemsData data)
    {
      UiStyle style = this.m_builder.Style;
      Panel unlockIcon = this.m_builder.NewPanel("Unlock", parent).SetBackground(data.UseGreenBg ? style.Research.NodeDetailUnlockBgStripe : style.Research.NodeDetailUnlockBgStripeRed);
      IconContainer icon = this.m_builder.NewIconContainer("Icon", (IUiElement) unlockIcon).PutToLeftMiddleOf<IconContainer>((IUiElement) unlockIcon, style.Research.NodeDetailUnlockIconSize, Offset.Left(style.Panel.Padding));
      if (data.IconPath.IsNotEmpty())
        icon.SetIcon(data.IconPath);
      else
        icon.Hide<IconContainer>();
      this.m_builder.NewTxt("Text", (IUiElement) unlockIcon).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.Panel.Text).SetText(data.Title).PutTo<Txt>((IUiElement) unlockIcon, Offset.Left(style.Research.NodeDetailUnlockIconSize.x + 2f * style.Panel.Padding));
      if (data.Prototype.HasValue)
      {
        unlockIcon.OnMouseEnter((Action) (() => this.m_popupController.ItemHovered(data.Prototype, (IUiElement) icon)));
        unlockIcon.OnMouseLeave((Action) (() => this.m_popupController.ItemHovered((Option<IProto>) Option.None, (IUiElement) icon)));
      }
      else if (data.Description.Value.IsNotEmpty())
        this.m_builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) unlockIcon).SetText(data.Description);
      return unlockIcon;
    }

    private struct ItemsData
    {
      public LocStrFormatted Title;
      public LocStrFormatted Description;
      public string IconPath;
      public bool UseGreenBg;
      public Option<IProto> Prototype;

      public ItemsData(
        LocStrFormatted title,
        LocStrFormatted description,
        string icon,
        bool useGreenBg,
        Option<IProto> prototype)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Title = title;
        this.Description = description;
        this.IconPath = icon;
        this.UseGreenBg = useGreenBg;
        this.Prototype = prototype;
      }
    }
  }
}
