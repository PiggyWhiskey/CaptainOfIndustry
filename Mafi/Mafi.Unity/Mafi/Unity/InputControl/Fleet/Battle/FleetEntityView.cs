// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.Battle.FleetEntityView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Fleet;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.Battle
{
  public class FleetEntityView : IUiElementWithUpdater, IUiElement
  {
    private readonly Func<bool> m_isRunningInBackground;
    public const int WIDTH = 360;
    public const int FLAG_HEIGHT = 25;
    public static readonly ColorRgba BACKGROUND;
    public static readonly ColorRgba BACKGROUND_DISABLED;
    public static readonly ColorRgba RED_HP;
    private readonly GridContainer m_weaponsGrid;
    private readonly Panel m_container;
    private readonly Panel m_hullContainer;
    private readonly IconContainer m_shipIcon;
    private readonly SimpleProgressBar m_hullHpBar;
    private readonly IconContainer m_pirateIcon;
    private readonly ViewsCacheHomogeneous<WeaponView> m_weaponsCache;
    public float XPos;
    public float YPos;
    public float LastSimPosition;
    public float PivotX;
    public FleetEntity Ship;
    private bool m_isFacingLeft;
    private bool m_isEscaping;
    private readonly Txt m_customTitle;
    private readonly Panel m_customTitleHolder;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; private set; }

    public FleetEntityView(UiBuilder builder, Func<bool> isRunningInBackground)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      FleetEntityView fleetEntityView = this;
      this.m_isRunningInBackground = isRunningInBackground;
      this.m_weaponsCache = new ViewsCacheHomogeneous<WeaponView>((Func<WeaponView>) (() => new WeaponView(builder, isRunningInBackground)));
      this.m_container = builder.NewPanel(nameof (Ship));
      int leftOffset = 35;
      this.m_weaponsGrid = builder.NewGridContainer("Weapons").SetDynamicHeightMode(2).SetCellSize(WeaponView.SIZE).SetCellSpacing(4f).PutToLeftTopOf<GridContainer>((IUiElement) this.m_container, new Vector2(0.0f, 0.0f), Offset.Left((float) leftOffset));
      Panel connector = builder.NewPanel("Connector").SetBackground(FleetEntityView.BACKGROUND).PutToLeftOf<Panel>((IUiElement) this.m_container, 3f, Offset.Left((float) leftOffset) + Offset.Bottom(30f) + Offset.Top(-8f));
      this.m_pirateIcon = builder.NewIconContainer("Pirate").SetIcon("Assets/Unity/UserInterface/WorldMap/PirateIcon128.png").SetColor(FleetEntityView.BACKGROUND).PutToCenterTopOf<IconContainer>((IUiElement) connector, 25.Vector2(), Offset.Top(-24f));
      this.m_hullContainer = builder.NewPanel("Hull").PutToBottomOf<Panel>((IUiElement) this.m_container, 100f);
      this.m_shipIcon = builder.NewIconContainer("ShipIcon").SetColor(FleetEntityView.BACKGROUND).PutToTopOf<IconContainer>((IUiElement) this.m_hullContainer, 90f);
      this.m_hullHpBar = new SimpleProgressBar((IUiElement) this.m_hullContainer, builder).AddBorder(new BorderStyle(ColorRgba.Black));
      Txt hullText = builder.NewTxt("HullText").SetAlignment(TextAnchor.MiddleCenter).SetColor(new ColorRgba(16777215)).SetTextStyle(builder.Style.Global.Title).PutTo<Txt>((IUiElement) this.m_hullHpBar);
      this.m_customTitleHolder = builder.NewPanel("TitleHolder").SetBackground(ColorRgba.Black).PutToBottomOf<Panel>((IUiElement) this.m_hullHpBar, 20f, Offset.Bottom(-20f));
      this.m_customTitle = builder.NewTxt("CustomTitle").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(builder.Style.Global.Title).BestFitEnabled(12).PutTo<Txt>((IUiElement) this.m_customTitleHolder);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<int>((Func<int>) (() => fleetEntityView.Ship.Hull.MaxHp.GetValue())).Observe<int>((Func<int>) (() => fleetEntityView.Ship.Hull.CurrentHp)).Observe<bool>((Func<bool>) (() => fleetEntityView.Ship.IsInBattle)).Do((Action<int, int, bool>) ((maxHp, hp, isInBattle) =>
      {
        if (!isInBattle)
          return;
        hullText.SetText(string.Format("{0} / {1}", (object) hp, (object) maxHp));
        closure_0.m_hullHpBar.SetProgress((float) hp / (float) maxHp);
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => fleetEntityView.Ship.IsDestroyed)).Do((Action<bool>) (isDestroyed =>
      {
        ColorRgba color = isDestroyed ? FleetEntityView.BACKGROUND_DISABLED : FleetEntityView.BACKGROUND;
        closure_0.m_shipIcon.SetColor(color);
        connector.SetBackground(color);
        closure_0.m_pirateIcon.SetColor(color);
        closure_0.m_hullHpBar.SetBackgroundColor(isDestroyed ? FleetEntityView.RED_HP.SetA((byte) 200) : FleetEntityView.RED_HP);
      }));
      AudioSource shipAlarm = builder.AudioDb.GetClonedAudio(builder.Audio.ShipAlarm);
      updaterBuilder.Observe<bool>((Func<bool>) (() => fleetEntityView.Ship.IsEscaping)).Do((Action<bool>) (isEscaping =>
      {
        if (closure_0.m_isEscaping)
          return;
        closure_0.updateFacingOrientation(isEscaping);
        if (!isEscaping || closure_0.m_isRunningInBackground())
          return;
        shipAlarm.Play();
      }));
      this.Updater = updaterBuilder.Build();
      this.Updater.AddChildUpdater(this.m_weaponsCache.Updater);
    }

    public void SetShip(FleetEntity ship, bool faceLeft)
    {
      this.Ship = ship;
      this.m_isFacingLeft = faceLeft;
      this.m_customTitleHolder.Hide<Panel>();
      if (ship.Fleet.IsHuman)
        this.m_shipIcon.SetIcon("Assets/Unity/UserInterface/Ship/PlayerBridgeI512x128.png");
      else
        this.m_shipIcon.SetIcon(ship.Hull.Proto.Graphics.IconPath);
      this.m_pirateIcon.SetVisibility<IconContainer>(!ship.Fleet.IsHuman);
      this.m_hullHpBar.PutToBottomOf<SimpleProgressBar>((IUiElement) this.m_hullContainer, 20f, Offset.Left(10f) + Offset.Right((float) (370 - ship.Hull.Proto.Graphics.IconContentWidth.Apply(360))));
      this.m_hullHpBar.SetProgress(ship.Hull.HpPercent);
      this.m_weaponsGrid.ClearAll();
      this.m_weaponsCache.ReturnAll();
      foreach (FleetWeapon weapon in this.Ship.Weapons)
      {
        WeaponView view = this.m_weaponsCache.GetView();
        view.SetWeapon(weapon);
        this.m_weaponsGrid.Append((IUiElement) view);
        view.SetScale<WeaponView>(1.Vector2());
      }
      float num = (float) ship.Hull.Proto.Graphics.IconContentTopOffset.Apply(90);
      this.SetSize<FleetEntityView>(new Vector2(360f, (float) ((double) this.m_hullContainer.GetHeight() + (double) this.m_weaponsGrid.GetHeight() - (double) num + 15.0)));
      this.PivotX = ship.Hull.Proto.Graphics.IconContentWidth.ToFloat() / 2f;
      this.SetPivot<FleetEntityView>(new Vector2(this.PivotX, 1f));
      this.updateFacingOrientation();
    }

    public void SetCustomTitle(string title)
    {
      this.m_customTitleHolder.Show<Panel>();
      this.m_customTitle.SetText(title);
    }

    private void updateFacingOrientation(bool isEscaping = false)
    {
      this.m_isEscaping = isEscaping;
      bool faceLeft = isEscaping ? !this.m_isFacingLeft : this.m_isFacingLeft;
      this.SetScale<FleetEntityView>(faceLeft ? new Vector2(1f, 1f) : new Vector2(-1f, 1f));
      this.m_hullHpBar.SetScale<SimpleProgressBar>(faceLeft ? new Vector2(1f, 1f) : new Vector2(-1f, 1f));
      foreach (WeaponView allExistingOne in this.m_weaponsCache.AllExistingOnes())
        allExistingOne.SetIsFacingLeft(faceLeft);
    }

    static FleetEntityView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FleetEntityView.BACKGROUND = new ColorRgba(0);
      FleetEntityView.BACKGROUND_DISABLED = new ColorRgba(0, 200);
      FleetEntityView.RED_HP = new ColorRgba(11553357);
    }
  }
}
