// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.RocketLaunchPadWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.SpaceProgram;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class RocketLaunchPadWindowView : StaticEntityInspectorBase<RocketLaunchPad>
  {
    private readonly RocketLaunchPadInspector m_controller;

    protected override RocketLaunchPad Entity => this.m_controller.SelectedEntity;

    public RocketLaunchPadWindowView(RocketLaunchPadInspector controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_controller = controller.CheckNotNull<RocketLaunchPadInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      UpdaterBuilder updater = UpdaterBuilder.Start();
      Txt txt = this.Builder.NewTxt("countdown").SetText((LocStrFormatted) Tr.LaunchPad_Launch__LiftOff);
      TextStyle title = this.Builder.Style.Global.Title;
      ref TextStyle local1 = ref title;
      int? nullable = new int?(30);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local1.Extend(color, fontStyle, fontSize, isCapitalized);
      Txt countdownTxt = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(itemContainer, new float?(40f));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.LaunchPad_UnityPerLaunch);
      Panel parent = this.AddOverlayPanel(itemContainer);
      TextWithIcon textWithIcon = new TextWithIcon(this.Builder).SetIcon("Assets/Unity/UserInterface/General/UnitySmall.svg").SetPrefixText(RocketLaunchManager.UNITY_PER_LAUNCH.Format1Dec()).SetSuffixText(string.Format(" / {0}", (object) TrCore.NumberOfMonths.Format(RocketLaunchManager.UNITY_PER_LAUNCH_DURATION_MONTHS)));
      textWithIcon.PutToLeftOf<TextWithIcon>((IUiElement) parent, textWithIcon.GetWidth(), Offset.Left(15f));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.LaunchPad_FuelTitle);
      BufferView fuelBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.LaunchPad_WaterBufferTitle);
      BufferView waterBuffer = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.LaunchPad_Launch__Title);
      Btn launchBtn = this.Builder.NewBtnPrimary("launch").SetText((LocStrFormatted) Tr.LaunchPad_Launch__Start).PlayErrorSoundWhenDisabled().OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<LaunchRocketCmd>(new LaunchRocketCmd(this.Entity))));
      launchBtn.AppendTo<Btn>(itemContainer, new Vector2?(launchBtn.GetOptimalSize() + new Vector2(20f, 5f)), ContainerPosition.LeftOrTop, Offset.Left(this.Builder.Style.Panel.Indent) + Offset.Bottom(5f));
      this.AddSwitch(itemContainer, Tr.LaunchPad_Launch__AutoStart.TranslatedString, (Action<bool>) (x => this.m_controller.InputScheduler.ScheduleInputCmd<SetRocketAutoLaunchCmd>(new SetRocketAutoLaunchCmd(this.Entity, x))), updater, (Func<bool>) (() => this.Entity.AutoLaunch));
      updater.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.AttachedRocket.ValueOrNull?.FuelBuffer.Product)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.AttachedRocket.ValueOrNull?.FuelBuffer.Capacity)).Observe<Quantity?>((Func<Quantity?>) (() => this.Entity.AttachedRocket.ValueOrNull?.FuelBuffer.Quantity)).Do((Action<ProductProto, Quantity?, Quantity?>) ((p, c, q) =>
      {
        if ((Proto) p == (Proto) null)
          fuelBuffer.UpdateState(Option<ProductProto>.None, Quantity.Zero, Quantity.Zero);
        else
          fuelBuffer.UpdateState(p, c.Value, q.Value);
      }));
      updater.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.WaterBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.WaterBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.WaterBuffer.Quantity)).Do((Action<ProductProto, Quantity, Quantity>) ((p, c, q) => waterBuffer.UpdateState(p, c, q)));
      updater.Observe<bool>((Func<bool>) (() => this.Entity.CanStartLaunchCountdown())).Do((Action<bool>) (canLaunch => launchBtn.SetEnabled(canLaunch)));
      updater.Observe<RocketLaunchPadState>((Func<RocketLaunchPadState>) (() => this.Entity.State)).Do((Action<RocketLaunchPadState>) (state =>
      {
        if (state == RocketLaunchPadState.RocketLaunching)
          countdownTxt.SetText((LocStrFormatted) Tr.LaunchPad_Launch__LiftOff);
        itemContainer.SetItemVisibility((IUiElement) countdownTxt, state == RocketLaunchPadState.LaunchCountdown || state == RocketLaunchPadState.RocketLaunching);
      }));
      updater.Observe<int?>((Func<int?>) (() =>
      {
        Duration? launchCountdown = this.Entity.LaunchCountdown;
        ref Duration? local2 = ref launchCountdown;
        return !local2.HasValue ? new int?() : new int?(local2.GetValueOrDefault().Seconds.ToIntCeiled());
      })).Do((Action<int?>) (countdown =>
      {
        if (!countdown.HasValue)
          return;
        countdownTxt.SetText(countdown.Value.ToString());
      }));
      this.AddUpdater(updater.Build());
    }
  }
}
