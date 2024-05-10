// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.ExplorationResultView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Research;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class ExplorationResultView : WindowView
  {
    private readonly ShortcutsManager m_shortcutsManager;
    private StackContainer m_stackContainer;
    private ProductRewardsView m_rewardsView;
    private Txt m_exploreResult;
    private Txt m_refugeesResult;
    private Panel m_refCountContainer;
    private Panel m_entityFoundContainer;
    private IconContainer m_entityIcon;
    private Txt m_entityInfoText;
    private Panel m_technologyFoundContainer;
    private Txt m_technologyText;
    private IconContainer m_technologyIcon;

    public ExplorationResultView(ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("ExplorationResult", WindowView.FooterStyle.Flat);
      this.m_shortcutsManager = shortcutsManager;
    }

    protected override void BuildWindowContent()
    {
      this.m_rewardsView = new ProductRewardsView(this.Builder);
      this.m_stackContainer = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(10f).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      this.m_exploreResult = this.Builder.NewTxt("ExploreResult").SetTextStyle(this.Style.Global.Text).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_stackContainer, new float?(30f), Offset.Left(5f));
      this.m_refCountContainer = this.Builder.NewPanel("RefAmountContainer").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(this.m_stackContainer, new float?(60f));
      Txt txt1 = this.Builder.NewTxt("AmountOfRefugees").SetText(Tr.AmountOfPops.Format("+10", 10));
      TextStyle text1 = this.Builder.Style.Global.Text;
      ref TextStyle local1 = ref text1;
      int? nullable1 = new int?(14);
      ColorRgba? color1 = new ColorRgba?();
      FontStyle? fontStyle1 = new FontStyle?();
      int? fontSize1 = nullable1;
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
      this.m_refugeesResult = txt1.SetTextStyle(textStyle1).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) this.m_refCountContainer, Offset.Left(10f));
      this.Builder.NewIconContainer("PopIcon").SetIcon("Assets/Unity/UserInterface/General/PopulationSmall.svg").PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_refCountContainer, new Vector2(40f, 40f), Offset.Left((float) ((double) this.m_refugeesResult.GetPreferedWidth() + 10.0 + 5.0)));
      this.m_entityFoundContainer = this.Builder.NewPanel("EntityFoundContainer").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(this.m_stackContainer, new float?(70f), Offset.Top(10f));
      this.m_entityIcon = this.Builder.NewIconContainer("EntityIcon").PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_entityFoundContainer, 50.Vector2(), Offset.Left(10f));
      Txt txt2 = this.Builder.NewTxt("EntityInfo");
      TextStyle text2 = this.Builder.Style.Global.Text;
      ref TextStyle local2 = ref text2;
      nullable1 = new int?(14);
      ColorRgba? color2 = new ColorRgba?();
      FontStyle? nullable2 = new FontStyle?();
      FontStyle? fontStyle2 = nullable2;
      int? fontSize2 = nullable1;
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
      this.m_entityInfoText = txt2.SetTextStyle(textStyle2).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) this.m_entityFoundContainer, Offset.Left(70f) + Offset.Top(10f));
      this.m_rewardsView.AppendTo<ProductRewardsView>(this.m_stackContainer, new float?(0.0f));
      this.m_technologyFoundContainer = this.Builder.NewPanel("TechnologyFoundContainer").AppendTo<Panel>(this.m_stackContainer, new float?(85f), Offset.Top(10f));
      this.m_technologyText = this.Builder.NewTxt("TechnologyName").SetText((LocStrFormatted) Tr.NewDiscovery).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.m_technologyFoundContainer, 25f, Offset.Left(10f));
      Panel parent = this.Builder.NewPanel("TechnologyFoundContainer").SetBackground((ColorRgba) 11840768).PutTo<Panel>((IUiElement) this.m_technologyFoundContainer, Offset.Top(25f));
      this.m_technologyIcon = this.Builder.NewIconContainer("TechnologyIcon").PutToLeftMiddleOf<IconContainer>((IUiElement) parent, 40.Vector2(), Offset.Left(10f));
      Txt txt3 = this.Builder.NewTxt("TechnologyName");
      TextStyle text3 = this.Builder.Style.Global.Text;
      ref TextStyle local3 = ref text3;
      nullable1 = new int?(14);
      nullable2 = new FontStyle?(FontStyle.Bold);
      ColorRgba? color3 = new ColorRgba?((ColorRgba) 5837184);
      FontStyle? fontStyle3 = nullable2;
      int? fontSize3 = nullable1;
      bool? isCapitalized3 = new bool?();
      TextStyle textStyle3 = local3.Extend(color3, fontStyle3, fontSize3, isCapitalized3);
      this.m_technologyText = txt3.SetTextStyle(textStyle3).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) parent, Offset.Left(60f));
      this.Builder.NewBtnPrimary("Accept").OnClick(new Action(this.onAcceptClick)).SetText((LocStrFormatted) Tr.ConfirmGreatNews).AppendTo<Btn>(this.m_stackContainer, new Vector2?(new Vector2(120f, 30f)), ContainerPosition.MiddleOrCenter);
      this.PositionSelfToCenter();
    }

    public void OnLocationExplored(LocationExploredMessage exploredMessage)
    {
      WorldMapLocation location = exploredMessage.Location;
      Option<WorldMapLoot> loot = exploredMessage.Loot;
      this.SetTitle((LocStrFormatted) Tr.ExplorationResult__Title);
      if (location.EntityProto.HasValue)
      {
        this.m_entityInfoText.SetText(Tr.ExplorationResult__Entity.Format(location.EntityProto.Value.Strings.Name.TranslatedString));
        this.m_entityIcon.SetIcon(location.EntityProto.Value.Graphics.IconPath);
      }
      else if (loot.IsNone)
        this.m_exploreResult.SetText((LocStrFormatted) Tr.ExplorationResult__Nothing);
      else
        this.m_exploreResult.SetText((LocStrFormatted) Tr.ExplorationResult__Loot);
      if (loot.HasValue)
      {
        WorldMapLoot worldMapLoot = loot.Value;
        this.m_rewardsView.SetReward(worldMapLoot.Products);
        this.m_refugeesResult.SetText(Tr.AmountOfPops.Format(string.Format("+{0}", (object) worldMapLoot.People), worldMapLoot.People));
      }
      if (exploredMessage.UnlockedProtos.IsNotEmpty)
      {
        TechnologyProto first = exploredMessage.UnlockedProtos.First;
        this.m_technologyIcon.SetIcon(first.Graphics.IconPath);
        this.m_technologyText.SetText((LocStrFormatted) first.Strings.Name);
      }
      this.m_stackContainer.StartBatchOperation();
      this.m_stackContainer.SetItemVisibility((IUiElement) this.m_exploreResult, location.EntityProto.IsNone);
      this.m_stackContainer.SetItemVisibility((IUiElement) this.m_refCountContainer, loot.HasValue && loot.Value.People > 0);
      this.m_stackContainer.SetItemVisibility((IUiElement) this.m_rewardsView, loot.HasValue && loot.Value.Products.IsNotEmpty);
      this.m_stackContainer.SetItemVisibility((IUiElement) this.m_technologyFoundContainer, exploredMessage.UnlockedProtos.IsNotEmpty);
      this.m_stackContainer.SetItemVisibility((IUiElement) this.m_entityFoundContainer, location.EntityProto.HasValue);
      this.m_stackContainer.FinishBatchOperation();
      this.SetContentSize(400f, this.m_stackContainer.GetDynamicHeight());
      this.Show();
    }

    private void onAcceptClick() => this.Hide();

    public bool InputUpdate()
    {
      if (!Input.GetKeyDown(this.m_shortcutsManager.Cancel) || !this.IsVisible)
        return false;
      this.Hide();
      return true;
    }
  }
}
