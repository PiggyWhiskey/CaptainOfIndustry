// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.Battle.WeaponView
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
  public class WeaponView : IUiElementWithUpdater, IUiElement
  {
    public static readonly Vector2 SIZE;
    private readonly Panel m_container;
    private readonly IconContainer m_icon;
    private readonly SimpleProgressBar m_hpBar;
    private readonly Txt m_dpsInfo;
    private readonly Txt m_rangeInfo;
    private FleetWeapon m_weapon;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; private set; }

    public WeaponView(UiBuilder builder, Func<bool> isRunningInBackground)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      WeaponView weaponView = this;
      this.m_container = builder.NewPanel("Weapon").SetBackground(FleetEntityView.BACKGROUND);
      this.m_icon = builder.NewIconContainer("Icon").PutToLeftOf<IconContainer>((IUiElement) this.m_container, 50f, Offset.Left(3f) + Offset.Top(-5f));
      IconContainer fireIcon = builder.NewIconContainer("Fire").SetIcon("Assets/Unity/UserInterface/Ship/Fire128.png").PutToRightTopOf<IconContainer>((IUiElement) this.m_icon, 23.Vector2(), Offset.Top(-5f) + Offset.Right(5f)).Hide<IconContainer>();
      Offset offset = Offset.Left((float) ((double) this.m_icon.GetWidth() + 3.0 + 5.0));
      Txt txt1 = builder.NewTxt("DPS").SetAlignment(TextAnchor.MiddleCenter).SetColor(new ColorRgba(16777215));
      TextStyle boldText1 = builder.Style.Global.BoldText;
      ref TextStyle local1 = ref boldText1;
      int? nullable = new int?(11);
      ColorRgba? color1 = new ColorRgba?();
      FontStyle? fontStyle1 = new FontStyle?();
      int? fontSize1 = nullable;
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
      this.m_dpsInfo = txt1.SetTextStyle(textStyle1).PutToTopOf<Txt>((IUiElement) this.m_container, 20f, offset);
      Txt txt2 = builder.NewTxt("Range").SetAlignment(TextAnchor.MiddleCenter).SetColor(new ColorRgba(16777215));
      TextStyle boldText2 = builder.Style.Global.BoldText;
      ref TextStyle local2 = ref boldText2;
      nullable = new int?(11);
      ColorRgba? color2 = new ColorRgba?();
      FontStyle? fontStyle2 = new FontStyle?();
      int? fontSize2 = nullable;
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
      this.m_rangeInfo = txt2.SetTextStyle(textStyle2).PutToBottomOf<Txt>((IUiElement) this.m_container, 20f, offset + Offset.Bottom(10f));
      this.m_hpBar = new SimpleProgressBar((IUiElement) this.m_container, builder).SetBackgroundColor(FleetEntityView.RED_HP).AddBorder(new BorderStyle(ColorRgba.Black)).PutToBottomOf<SimpleProgressBar>((IUiElement) this.m_container, 7f, Offset.Bottom(3f) + Offset.LeftRight(15f));
      SimpleProgressBar reloadBar = new SimpleProgressBar((IUiElement) this.m_container, builder, true).SetBackgroundColor(new ColorRgba(4672256)).SetColor(builder.Style.Global.OrangeText).PutToLeftOf<SimpleProgressBar>((IUiElement) this.m_container, 5f, Offset.TopBottom(15f) + Offset.Left(this.m_icon.GetWidth() + 3f));
      AudioSource turretShot = builder.AudioDb.GetClonedAudio(builder.Audio.TurretShot);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Percent>((Func<Percent>) (() => weaponView.m_weapon.HpPercent)).Observe<bool>((Func<bool>) (() => weaponView.m_weapon.OwningEntity.IsInBattle)).Do((Action<Percent, bool>) ((hpPercent, inBattle) =>
      {
        if (!inBattle)
          return;
        weaponView.m_hpBar.SetProgress(hpPercent);
      }));
      updaterBuilder.Observe<int>((Func<int>) (() => weaponView.m_weapon.Proto.ReloadRounds)).Observe<int>((Func<int>) (() => weaponView.m_weapon.RoundsUntilReloaded)).Observe<bool>((Func<bool>) (() => weaponView.m_weapon.IsDestroyed || weaponView.m_weapon.OwningEntity.IsDestroyed)).Do((Action<int, int, bool>) ((reloadRounds, roundUntilReloaded, isDisabled) =>
      {
        if (isDisabled)
        {
          reloadBar.SetProgress(Percent.Zero);
          closure_0.m_container.SetBackground(FleetEntityView.BACKGROUND_DISABLED);
        }
        else
        {
          reloadBar.SetProgress((float) (reloadRounds - roundUntilReloaded) / (float) reloadRounds);
          closure_0.m_container.SetBackground(FleetEntityView.BACKGROUND);
        }
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => weaponView.m_weapon.FiredLastSim)).Do((Action<bool>) (firedLastSim =>
      {
        if (firedLastSim && !isRunningInBackground())
          turretShot.Play();
        closure_0.m_icon.SetColor(firedLastSim ? new ColorRgba(16764416) : new ColorRgba(16777215));
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => weaponView.m_weapon.IsDestroyed)).Do((Action<bool>) (isDestroyed => fireIcon.SetVisibility<IconContainer>(isDestroyed)));
      updaterBuilder.Observe<bool>((Func<bool>) (() => weaponView.m_weapon.OwningEntity.IsDestroyed)).Do((Action<bool>) (isDestroyed => weaponView.m_container.SetBackground(isDestroyed ? FleetEntityView.BACKGROUND_DISABLED : FleetEntityView.BACKGROUND)));
      this.Updater = updaterBuilder.Build();
      this.m_container.SetSize<Panel>(WeaponView.SIZE);
    }

    public void SetWeapon(FleetWeapon weapon)
    {
      this.m_weapon = weapon;
      this.m_icon.SetIcon(weapon.Proto.Graphics.IconPath);
      this.m_dpsInfo.SetText(string.Format("{0} DPS", (object) weapon.AvgDamagePer10Rounds));
      this.m_rangeInfo.SetText(string.Format("{0}km", (object) weapon.Range));
    }

    public void SetIsFacingLeft(bool faceLeft)
    {
      Vector2 scale = faceLeft ? new Vector2(1f, 1f) : new Vector2(-1f, 1f);
      this.m_dpsInfo.SetScale<Txt>(scale);
      this.m_rangeInfo.SetScale<Txt>(scale);
      this.m_hpBar.SetScale<SimpleProgressBar>(scale);
    }

    static WeaponView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WeaponView.SIZE = new Vector2(120f, 45f);
    }
  }
}
