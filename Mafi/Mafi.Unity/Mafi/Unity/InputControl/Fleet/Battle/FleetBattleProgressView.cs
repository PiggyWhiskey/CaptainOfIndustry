// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.Battle.FleetBattleProgressView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Fleet;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.Battle
{
  public class FleetBattleProgressView : IUiElement
  {
    private float m_targetScale;
    private Vector2 m_zoomPoint;
    private float m_zoomVelocity;
    private readonly Panel m_container;
    private readonly ScrollableContainer m_scrollableContainer;
    private readonly Panel m_bgContainer;
    private readonly FleetBattleResultView m_battleResultView;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly IUiUpdater m_updater;
    private readonly ViewsCacheHomogeneous<FleetEntityView> m_fleetsViewsCache;
    private readonly Lyst<FleetEntityView> m_leftFleet;
    private readonly Lyst<FleetEntityView> m_rightFleet;
    private IBattleState m_battleState;
    private float m_yOffsetLeftFleet;
    private float m_yOffsetRightFleet;
    private float m_xOffsetFleet;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public FleetBattleProgressView(
      UiBuilder builder,
      TravelingFleetManager fleetManager,
      Func<bool> isRunningInBackground)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_targetScale = 1f;
      this.m_leftFleet = new Lyst<FleetEntityView>();
      this.m_rightFleet = new Lyst<FleetEntityView>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      FleetBattleProgressView battleProgressView = this;
      this.m_fleetManager = fleetManager;
      this.m_battleResultView = new FleetBattleResultView();
      this.m_fleetsViewsCache = new ViewsCacheHomogeneous<FleetEntityView>((Func<FleetEntityView>) (() => new FleetEntityView(builder, isRunningInBackground)));
      this.m_container = builder.NewPanel("BattleView");
      this.m_scrollableContainer = builder.NewScrollableContainer("ScrollableContainer").DisableScrollByMouseWheel().SetDecelerationRate(0.0f).PutTo<ScrollableContainer>((IUiElement) this.m_container);
      this.m_battleResultView.BuildUi(builder);
      this.m_battleResultView.PutToCenterBottomOf<FleetBattleResultView>((IUiElement) this.m_container, this.m_battleResultView.GetSize());
      this.m_bgContainer = builder.NewPanel("BattleMapContainer").SetBackground(builder.AssetsDb.GetSharedSprite("Assets/Unity/UserInterface/WorldMap/WaterBackground2.png"), isTiled: true);
      this.m_scrollableContainer.AddItem((IUiElement) this.m_bgContainer);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<BattleResult>>((Func<Option<BattleResult>>) (() => battleProgressView.m_battleState.Result)).Do((Action<Option<BattleResult>>) (result =>
      {
        battleProgressView.m_battleResultView.SetVisibility<FleetBattleResultView>(result.HasValue);
        if (!result.HasValue)
          return;
        battleProgressView.m_battleResultView.SetBattleState(result.Value);
      }));
      this.m_updater = updaterBuilder.Build();
      this.m_updater.AddChildUpdater(this.m_fleetsViewsCache.Updater);
    }

    public void RenderUpdate(GameTime gameTime)
    {
      this.m_updater.RenderUpdate();
      this.updateFleetsPositions((Option<GameTime>) gameTime);
      float x = this.m_bgContainer.RectTransform.localScale.x;
      float num = Mathf.SmoothDamp(x, this.m_targetScale, ref this.m_zoomVelocity, 0.1f);
      this.m_bgContainer.RectTransform.localScale = new Vector3(num, num, 1f);
      Vector2 vector2_1 = this.m_zoomPoint * x;
      Vector2 vector2_2 = this.m_zoomPoint * num - vector2_1;
      RectTransform rectTransform = this.m_bgContainer.RectTransform;
      rectTransform.localPosition = rectTransform.localPosition - new Vector3(vector2_2.x, vector2_2.y, 0.0f);
      this.m_scrollableContainer.FixScroll();
    }

    public void SetOnResultAccepted(Action onResultAccepted)
    {
      this.m_battleResultView.SetOnAccept(onResultAccepted);
    }

    public void SetBattle(IBattleState battleState)
    {
      this.m_battleState = battleState;
      this.m_fleetsViewsCache.ReturnAll();
      this.m_leftFleet.Clear();
      this.m_rightFleet.Clear();
      BattleFleet battleFleet1 = battleState.Attacker.IsHuman ? battleState.Attacker : battleState.Defender;
      BattleFleet battleFleet2 = battleState.Attacker.IsHuman ? battleState.Defender : battleState.Attacker;
      float self1 = 0.0f;
      float num = 0.0f;
      foreach (FleetEntity entity in battleFleet1.Entities)
      {
        FleetEntityView view = this.m_fleetsViewsCache.GetView();
        view.SetShip(entity, false);
        this.m_leftFleet.Add(view);
        self1 += view.GetHeight() + 65f;
        if (this.m_fleetManager.HasFleet && this.m_fleetManager.TravelingFleet.FleetEntity == entity && this.m_fleetManager.TravelingFleet.HasCustomTitle())
          view.SetCustomTitle(this.m_fleetManager.TravelingFleet.GetTitle());
      }
      foreach (FleetEntity entity in battleFleet2.Entities)
      {
        FleetEntityView view = this.m_fleetsViewsCache.GetView();
        view.SetShip(entity, true);
        this.m_rightFleet.Add(view);
        num += view.GetHeight() + 65f;
      }
      float self2 = self1.Max(num);
      float x = ((float) (720.0 + 60.0 * 40.0)).Max((float) Screen.width / 0.7f) + 500f;
      float y = self2.Max((float) Screen.height / 0.7f) + 500f;
      this.m_bgContainer.SetSize<Panel>(new Vector2(x, y));
      this.m_bgContainer.RectTransform.localPosition = new Vector3((float) -((double) x / 2.0), y / 2f);
      this.m_xOffsetFleet = x / 2f;
      this.m_yOffsetLeftFleet = (float) (((double) y - (double) self2) / 2.0);
      this.m_yOffsetRightFleet = this.m_yOffsetLeftFleet;
      if ((double) self1 < (double) num)
        this.m_yOffsetLeftFleet += (float) (((double) num - (double) self1) / 2.0);
      else
        this.m_yOffsetRightFleet += (float) (((double) self1 - (double) num) / 2.0);
      this.updateFleetsPositions(Option<GameTime>.None);
    }

    private void updateFleetsPositions(Option<GameTime> gameTime)
    {
      this.updateRelativePos(this.m_leftFleet, this.m_yOffsetLeftFleet, gameTime);
      this.updateRelativePos(this.m_rightFleet, this.m_yOffsetRightFleet, gameTime);
    }

    private void updateRelativePos(
      Lyst<FleetEntityView> fleet,
      float yOffset,
      Option<GameTime> gameTime)
    {
      float num = yOffset;
      foreach (FleetEntityView fleetEntityView in fleet)
      {
        if (gameTime.HasValue)
        {
          fleetEntityView.XPos = fleetEntityView.XPos.Lerp(fleetEntityView.LastSimPosition * 60f, gameTime.Value.RelativeT);
        }
        else
        {
          fleetEntityView.LastSimPosition = fleetEntityView.Ship.Position.ToFloat();
          fleetEntityView.XPos = fleetEntityView.LastSimPosition * 60f;
        }
        fleetEntityView.YPos = num;
        fleetEntityView.PutToLeftTopOf<FleetEntityView>((IUiElement) this.m_bgContainer, fleetEntityView.GetSize(), Offset.TopLeft(fleetEntityView.YPos, this.m_xOffsetFleet + fleetEntityView.XPos));
        fleetEntityView.SetPivot<FleetEntityView>(new Vector2(fleetEntityView.PivotX, 1f));
        num += fleetEntityView.GetHeight() + 65f;
      }
    }

    public void SyncUpdate(GameTime gameTime)
    {
      this.m_updater.SyncUpdate();
      foreach (FleetEntityView fleetEntityView in this.m_leftFleet)
        fleetEntityView.LastSimPosition = fleetEntityView.Ship.Position.ToFloat();
      foreach (FleetEntityView fleetEntityView in this.m_rightFleet)
        fleetEntityView.LastSimPosition = fleetEntityView.Ship.Position.ToFloat();
    }

    public bool InputUpdate()
    {
      float num = 1f * Input.GetAxis("Mouse ScrollWheel");
      if (num.IsNearZero())
        return false;
      Vector3 mousePosition = Input.mousePosition;
      if (RectTransformUtility.RectangleContainsScreenPoint(this.m_scrollableContainer.RectTransform, (Vector2) mousePosition))
      {
        this.m_zoomPoint = (Vector2) this.m_bgContainer.RectTransform.InverseTransformPoint(mousePosition);
        this.m_targetScale = (this.m_targetScale + num).Clamp(0.7f, 1.2f);
      }
      return true;
    }
  }
}
