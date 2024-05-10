// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Map.MapCellWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Map;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Map
{
  internal class MapCellWindowView : ItemDetailWindowView
  {
    private Panel m_buyRegionContainer;
    private readonly MapCellInspector m_controller;

    private MapCell MapCell => this.m_controller.MapCell;

    public MapCellWindowView(MapCellInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("MapCellInspector");
      this.m_controller = controller.CheckNotNull<MapCellInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.SetTitle("Region detail");
      this.m_buyRegionContainer = this.Builder.NewPanel("UnlockContainer").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(70f));
      Txt buyRegionTitle = this.Builder.NewTxt("Name").SetText("Unlock this region").SetTextStyle(this.Style.Panel.SectionTitle).SetAlignment(TextAnchor.MiddleCenter).PutToTopOf<Txt>((IUiElement) this.m_buyRegionContainer, 30f);
      Txt regionUnlockedTxt = this.Builder.NewTxt("Done").SetText("This region is yours!").SetTextStyle(this.Style.Panel.SectionTitle).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_buyRegionContainer);
      Btn buyRegionBtn = this.Builder.NewBtn("Unlock").SetButtonStyle(this.Builder.Style.Global.PrimaryBtn).OnClick(new Action(this.m_controller.UnlockCurrentCell));
      buyRegionBtn.PutToCenterTopOf<Btn>((IUiElement) this.m_buyRegionContainer, buyRegionBtn.GetOptimalSize(), Offset.Top(buyRegionTitle.GetHeight()));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.MapCell.IsUnlocked)).Do((Action<bool>) (isUnlocked =>
      {
        regionUnlockedTxt.SetVisibility<Txt>(isUnlocked);
        buyRegionTitle.SetVisibility<Txt>(!isUnlocked);
        buyRegionBtn.SetVisibility<Btn>(!isUnlocked);
      }));
      this.AddUpdater(updaterBuilder.Build());
    }
  }
}
