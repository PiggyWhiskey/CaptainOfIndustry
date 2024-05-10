// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipStatsPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Fleet;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.ShipDesign
{
  public class ShipStatsPanel : WindowView
  {
    private StackContainer m_container;
    private Txt m_hpVal;
    private Txt m_armorVal;
    private Txt m_dpsVal;
    private Txt m_crewVal;
    private Txt m_weaponRangeVal;
    private Txt m_fuelCapVal;
    private Txt m_radarRangeVal;
    private Txt m_battleScoreVal;

    public ShipStatsPanel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("StatsView", WindowView.FooterStyle.None);
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.ShipStats);
      this.m_container = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetInnerPadding(Offset.TopBottom(5f) + Offset.LeftRight(10f)).PutTo<StackContainer>((IUiElement) this.GetContentPanel());
      this.m_hpVal = this.addStat("hp", Tr.HitPoints);
      this.m_armorVal = this.addStat("armor", Tr.Armor);
      this.m_crewVal = this.addStat("crew", Tr.ShipCrew);
      this.m_dpsVal = this.addStat("dps", Tr.AvgDamage);
      this.m_weaponRangeVal = this.addStat("range", Tr.MaxWeaponRange);
      this.m_fuelCapVal = this.addStat("fuelCap", Tr.FuelTank_Title);
      this.m_radarRangeVal = this.addStat("radar", Tr.RadarRange);
      this.m_battleScoreVal = this.addStat("battleScore", Tr.BattleScore);
      this.SetContentSize(180f, this.m_container.GetDynamicHeight());
    }

    private Txt addStat(string id, LocStr name)
    {
      Panel parent = this.Builder.NewPanel(id + "Container").AppendTo<Panel>(this.m_container, new float?(22f));
      this.Builder.NewTxt("Title").SetTextStyle(this.Builder.Style.Global.Title).SetText(name.TranslatedString + ":").SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) parent);
      Txt txt = this.Builder.NewTxt("Value");
      TextStyle title = this.Builder.Style.Global.Title;
      ref TextStyle local = ref title;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      return txt.SetTextStyle(textStyle).SetText("-").SetAlignment(TextAnchor.MiddleRight).PutTo<Txt>((IUiElement) parent);
    }

    public void SetStats(FleetEntityStats oldStats, FleetEntityStats newStats)
    {
      this.m_hpVal.SetText(newStats.HitPoints.ToString());
      this.setIsChanged(this.m_hpVal, oldStats.HitPoints != newStats.HitPoints);
      this.m_armorVal.SetText(newStats.Armor.ToString());
      this.setIsChanged(this.m_armorVal, oldStats.Armor != newStats.Armor);
      this.m_crewVal.SetText(newStats.Crew.ToString());
      this.setIsChanged(this.m_crewVal, oldStats.Crew != newStats.Crew);
      this.m_dpsVal.SetText(newStats.AvgDamage.ToString());
      this.setIsChanged(this.m_dpsVal, oldStats.AvgDamage != newStats.AvgDamage);
      this.m_weaponRangeVal.SetText(newStats.MaxWeaponRange.ToString());
      this.setIsChanged(this.m_weaponRangeVal, oldStats.MaxWeaponRange != newStats.MaxWeaponRange);
      this.m_fuelCapVal.SetText(newStats.FuelTankCapacity.ToString());
      this.setIsChanged(this.m_fuelCapVal, oldStats.FuelTankCapacity != newStats.FuelTankCapacity);
      this.m_radarRangeVal.SetText(newStats.RadarRange.ToString());
      this.setIsChanged(this.m_radarRangeVal, oldStats.RadarRange != newStats.RadarRange);
      this.m_battleScoreVal.SetText(newStats.GetBattleScore().ToString());
      this.setIsChanged(this.m_battleScoreVal, oldStats.GetBattleScore() != newStats.GetBattleScore());
    }

    private void setIsChanged(Txt text, bool isChanged)
    {
      text.SetColor(isChanged ? this.Builder.Style.Global.OrangeText : this.Builder.Style.Global.Title.Color);
    }
  }
}
