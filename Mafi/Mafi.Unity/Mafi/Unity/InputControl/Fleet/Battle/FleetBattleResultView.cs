// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.Battle.FleetBattleResultView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Fleet;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.Battle
{
  public class FleetBattleResultView : WindowView
  {
    private Txt m_playerDmgDone;
    private Txt m_enemyDmgDone;
    private Panel m_defeatPanel;
    private Panel m_victoryPanel;
    private StackContainer m_container;
    private Option<Action> m_onAccept;

    public FleetBattleResultView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("BattleResultView", WindowView.FooterStyle.None);
    }

    public void SetOnAccept(Action onAccept) => this.m_onAccept = (Option<Action>) onAccept;

    protected override void BuildWindowContent()
    {
      this.SetHeaderHeight(0.0f);
      this.m_container = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(15f).PutTo<StackContainer>((IUiElement) this.GetContentPanel(), Offset.Top(10f) + Offset.Bottom(10f));
      int num = 50;
      this.m_defeatPanel = this.Builder.NewPanel("DefeatedPanel").SetBackground(new ColorRgba(11272192)).AppendTo<Panel>(this.m_container, new float?((float) num));
      Txt txt1 = this.Builder.NewTxt("Defeated").SetText(Tr.BattleResult__Defeat.TranslatedString.ToUpper(LocalizationManager.CurrentCultureInfo));
      TextStyle title1 = this.Builder.Style.Global.Title;
      ref TextStyle local1 = ref title1;
      int? nullable = new int?(14);
      ColorRgba? color1 = new ColorRgba?();
      FontStyle? fontStyle1 = new FontStyle?();
      int? fontSize1 = nullable;
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
      txt1.SetTextStyle(textStyle1).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_defeatPanel);
      this.m_victoryPanel = this.Builder.NewPanel("VictoryPanel").SetBackground(new ColorRgba(36895)).AppendTo<Panel>(this.m_container, new float?((float) num));
      Txt txt2 = this.Builder.NewTxt("Victory").SetText(Tr.BattleResult__Victory.TranslatedString.ToUpper(LocalizationManager.CurrentCultureInfo));
      TextStyle title2 = this.Builder.Style.Global.Title;
      ref TextStyle local2 = ref title2;
      nullable = new int?(14);
      ColorRgba? color2 = new ColorRgba?();
      FontStyle? fontStyle2 = new FontStyle?();
      int? fontSize2 = nullable;
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
      txt2.SetTextStyle(textStyle2).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_victoryPanel);
      Panel parent = this.Builder.NewPanel("Stats").AppendTo<Panel>(this.m_container, new float?(60f));
      Panel leftOf1 = this.Builder.NewPanel("PlayerStatsBar").PutToLeftOf<Panel>((IUiElement) parent, 200f);
      this.Builder.NewTxt("PlayerTitle").SetText((LocStrFormatted) Tr.BattleResult__ShipTitle).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.Builder.Style.Global.Title).PutToTopOf<Txt>((IUiElement) leftOf1, 30f);
      this.m_playerDmgDone = this.Builder.NewTxt("PlayerDamageDone").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.Builder.Style.Global.Text).PutToTopOf<Txt>((IUiElement) leftOf1, 30f, Offset.Top(30f));
      Panel leftOf2 = this.Builder.NewPanel("EnemyStatsBar").PutToLeftOf<Panel>((IUiElement) parent, 200f, Offset.Left(200f));
      this.Builder.NewTxt("EnemyTitle").SetText((LocStrFormatted) Tr.Enemy).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.Builder.Style.Global.Title).PutToTopOf<Txt>((IUiElement) leftOf2, 30f);
      this.m_enemyDmgDone = this.Builder.NewTxt("EnemyDamageDone").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.Builder.Style.Global.Text).PutToTopOf<Txt>((IUiElement) leftOf2, 30f, Offset.Top(30f));
      Btn btn = this.Builder.NewReturnBtn().OnClick((Action) (() =>
      {
        if (this.m_onAccept.HasValue)
          this.m_onAccept.Value();
        this.Hide();
      }));
      btn.AppendTo<Btn>(this.m_container, new Vector2?(btn.GetSize()), ContainerPosition.MiddleOrCenter, Offset.Left(20f));
      this.SetContentSize(new Vector2(400f, this.m_container.GetDynamicHeight()));
    }

    public void SetBattleState(BattleResult result)
    {
      bool playerWon = result.PlayerResult.Value.PlayerWon;
      this.m_container.SetItemVisibility((IUiElement) this.m_victoryPanel, playerWon);
      this.m_container.SetItemVisibility((IUiElement) this.m_defeatPanel, !playerWon);
      FleetBattleResultStats playerFleetStats = result.PlayerResult.Value.PlayerFleetStats;
      this.m_playerDmgDone.SetText(Tr.BattleResult__DamageDone.Format(playerFleetStats.DamageDone.ToString()));
      FleetBattleResultStats opponentFleetStats = result.PlayerResult.Value.OpponentFleetStats;
      this.m_enemyDmgDone.SetText(Tr.BattleResult__DamageDone.Format(opponentFleetStats.DamageDone.ToString()));
      this.Show();
    }
  }
}
