// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.Battle.BattlePreviewWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Fleet;
using Mafi.Core.World;
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
  public class BattlePreviewWindow : WindowView
  {
    private readonly Action m_onBattleReset;
    private readonly TravelingFleetManager m_fleetManager;
    private FleetBattleProgressView m_battleProgressView;

    public BattlePreviewWindow(Action onBattleReset, TravelingFleetManager fleetManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("BattleView");
      this.m_onBattleReset = onBattleReset;
      this.m_fleetManager = fleetManager;
      this.ShowAfterSync = true;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle("Battle in progress!");
      this.m_battleProgressView = new FleetBattleProgressView(this.Builder, this.m_fleetManager, (Func<bool>) (() => false));
      this.m_battleProgressView.PutTo<FleetBattleProgressView>((IUiElement) this.GetContentPanel());
      this.Builder.NewBtnPrimary("Reset battle").SetText("Reset battle").OnClick(this.m_onBattleReset).PutToRightBottomOf<Btn>((IUiElement) this.GetContentPanel(), new Vector2(180f, 35f));
      this.PositionSelfToFullscreen();
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_battleProgressView.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_battleProgressView.SyncUpdate(gameTime);
    }

    public bool InputUpdate() => this.m_battleProgressView.InputUpdate();

    public void SetBattle(IBattleState battleState)
    {
      this.m_battleProgressView.SetBattle(battleState);
      this.Show();
    }
  }
}
