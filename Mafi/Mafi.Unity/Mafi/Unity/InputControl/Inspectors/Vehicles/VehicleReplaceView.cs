// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.VehicleReplaceView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles.Commands;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  public class VehicleReplaceView : IUiElementWithUpdater, IUiElement
  {
    private readonly InspectorContext m_context;
    private DrivingEntityProto m_currentProto;
    private Mafi.Core.Entities.Dynamic.Vehicle m_currentVehicle;
    private readonly PanelWithShadow m_container;
    private readonly ProtoPicker<DrivingEntityProto> m_protoPicker;
    private Option<DrivingEntityProto> m_replacementProto;
    private readonly Btn m_plusBtn;
    private readonly Btn m_replaceBtn;
    private readonly Offset m_iconOffset;
    private readonly IconContainer m_vehicleIcon;
    private readonly TitleTooltip m_btnTooltip;
    private readonly IconContainer m_fuelIcon;

    public IUiUpdater Updater { get; }

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public VehicleReplaceView(
      UiBuilder builder,
      InspectorContext context,
      IUiElement parent,
      WindowView parentWindow,
      Panel overlay,
      Func<Mafi.Core.Entities.Dynamic.Vehicle> entityProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      VehicleReplaceView vehicleReplaceView = this;
      this.m_context = context;
      this.m_protoPicker = new ProtoPicker<DrivingEntityProto>(new Action<DrivingEntityProto>(this.onVehicleSelected), useBigIcons: true);
      this.m_protoPicker.BuildUi(builder);
      this.m_protoPicker.SetTitle((LocStrFormatted) Tr.SelectVehicle_Title);
      this.m_container = builder.NewPanelWithShadow("ReplaceContainer", parent).SetBackground((ColorRgba) 2236962).AddShadowRightBottom();
      Btn btn = builder.NewBtn("Replace");
      BtnStyle generalBtn = builder.Style.Global.GeneralBtn;
      ref BtnStyle local = ref generalBtn;
      bool? nullable = new bool?(false);
      TextStyle? text = new TextStyle?();
      BorderStyle? border = new BorderStyle?();
      ColorRgba? backgroundClr = new ColorRgba?();
      ColorRgba? normalMaskClr = new ColorRgba?();
      ColorRgba? hoveredClr = new ColorRgba?();
      ColorRgba? pressedClr = new ColorRgba?();
      ColorRgba? disabledMaskClr = new ColorRgba?();
      ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
      ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
      bool? shadow = nullable;
      int? width = new int?();
      int? height = new int?();
      int? sidePaddings = new int?();
      Offset? iconPadding = new Offset?();
      BtnStyle buttonStyle = local.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      this.m_replaceBtn = btn.SetButtonStyle(buttonStyle).SetIcon("Assets/Unity/UserInterface/General/Repeat.svg").PlayErrorSoundWhenDisabled().OnClick(new Action(this.onReplaceClick));
      this.m_replaceBtn.PutToLeftOf<Btn>((IUiElement) this.m_container, 40f);
      Tooltip replaceBtnTooltip = this.m_replaceBtn.AddToolTipAndReturn();
      Panel plusBtnHolder = builder.NewPanel("PlusBtnContainer", (IUiElement) this.m_container).PutToLeftOf<Panel>((IUiElement) this.m_container, 40f, Offset.Left(this.m_replaceBtn.GetWidth() + 10f));
      this.m_plusBtn = builder.NewBtn("PlusBtn", parent).OnClick(new Action(this.plusBtnClicked)).PlayErrorSoundWhenDisabled().SetOnMouseEnterLeaveActions(new Action(this.onIconMouseEnter), new Action(this.onIconMouseLeave)).PutTo<Btn>((IUiElement) plusBtnHolder);
      this.m_btnTooltip = new TitleTooltip(builder);
      this.m_btnTooltip.SetMaxWidthOverflow(40);
      this.m_btnTooltip.AttachTo<Btn>((IUiElementWithHover<Btn>) this.m_plusBtn);
      Tooltip plusBtnTooltip = this.m_plusBtn.AddToolTipAndReturn();
      this.m_iconOffset = Offset.All(1f) + Offset.Left(3f);
      this.m_vehicleIcon = builder.NewIconContainer("Icon", (IUiElement) this.m_plusBtn).PutTo<IconContainer>((IUiElement) this.m_plusBtn, this.m_iconOffset);
      this.m_fuelIcon = builder.NewIconContainer("FuelIcon", (IUiElement) this.m_vehicleIcon).PutToLeftBottomOf<IconContainer>((IUiElement) this.m_vehicleIcon, 16.Vector2(), Offset.Left(-8f));
      IconContainer tickIcon = builder.NewIconContainer("Tick").SetIcon("Assets/Unity/UserInterface/General/Tick128.png", (ColorRgba) 1219584).PutToRightBottomOf<IconContainer>((IUiElement) this.m_container, 20.Vector2(), Offset.All(1f));
      this.m_container.SetWidth<PanelWithShadow>((float) ((double) this.m_plusBtn.GetWidth() + 10.0 + 5.0 + 40.0));
      this.m_protoPicker.PutToLeftTopOf<ProtoPicker<DrivingEntityProto>>((IUiElement) plusBtnHolder, this.m_protoPicker.GetSize(), Offset.Left(plusBtnHolder.GetWidth() - 1f));
      this.m_protoPicker.OnShowStart += (Action) (() =>
      {
        overlay.Show<Panel>();
        plusBtnHolder.SetParent<Panel>((IUiElement) parentWindow);
      });
      this.m_protoPicker.OnHide += new Action(onPickerHide);
      overlay.OnClick(new Action(((View) this.m_protoPicker).Hide));
      parentWindow.OnHide += (Action) (() =>
      {
        if (!vehicleReplaceView.m_protoPicker.IsVisible)
          return;
        vehicleReplaceView.m_protoPicker.Hide();
      });
      this.setVehicleProto(Option<DrivingEntityProto>.None);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Mafi.Core.Entities.Dynamic.Vehicle>(entityProvider).Observe<Option<DrivingEntityProto>>((Func<Option<DrivingEntityProto>>) (() => entityProvider().ReplacementProto)).Do((Action<Mafi.Core.Entities.Dynamic.Vehicle, Option<DrivingEntityProto>>) ((vehicle, replacement) =>
      {
        closure_0.m_currentProto = vehicle.Prototype;
        closure_0.m_currentVehicle = vehicle;
        closure_0.m_plusBtn.SetEnabled(replacement.IsNone);
        tickIcon.SetVisibility<IconContainer>(replacement.HasValue);
        plusBtnTooltip.SetText(replacement.HasValue ? Tr.ReplaceVehicle__WaitingForReplace.TranslatedString : "");
        if (replacement.IsNone)
        {
          replacement = closure_0.m_context.VehiclesReplacementMap.GetReplacementFor(closure_0.m_currentProto);
          if (replacement.HasValue && closure_0.m_context.UnlockedProtosDbForUi.IsLocked((Proto) replacement.Value))
            replacement = Option<DrivingEntityProto>.None;
        }
        closure_0.setVehicleProto(replacement);
      }));
      string mainTooltip = string.Format("\n\n{0}", (object) Tr.ReplaceVehicle__MainTooltip);
      updaterBuilder.Observe<bool>((Func<bool>) (() => entityProvider().IsOnWayToDepotForScrap)).Observe<bool>((Func<bool>) (() => entityProvider().IsOnWayToDepotForReplacement)).Observe<bool>((Func<bool>) (() => entityProvider().ReplaceQueued)).Observe<bool>((Func<bool>) (() => (Proto) vehicleReplaceView.m_currentProto != (Proto) null && vehicleReplaceView.m_replacementProto.HasValue && vehicleReplaceView.m_context.VehiclesManager.CanUpgradeVehicle(vehicleReplaceView.m_currentProto, vehicleReplaceView.m_replacementProto.Value))).Observe<bool>((Func<bool>) (() => vehicleReplaceView.m_replacementProto.HasValue)).Do((Action<bool, bool, bool, bool, bool>) ((isOnWayForScrap, isOnWayForReplace, isReplacementPending, canReplaceCurrentVehicle, hasReplacementSelected) =>
      {
        closure_0.m_replaceBtn.SetEnabled(((isOnWayForScrap || isOnWayForReplace ? 0 : (!isReplacementPending ? 1 : 0)) & (canReplaceCurrentVehicle ? 1 : 0)) != 0);
        if (isOnWayForScrap)
          replaceBtnTooltip.SetText(string.Format("{0}{1}", (object) Tr.ScrapVehicle__InProgress, (object) mainTooltip));
        else if (isOnWayForReplace)
          replaceBtnTooltip.SetText(string.Format("{0}{1}", (object) Tr.ReplaceVehicle__OnItsWay, (object) mainTooltip));
        else if (isReplacementPending)
          replaceBtnTooltip.SetText(string.Format("{0}{1}", (object) Tr.ReplaceVehicle__WaitingForReplace, (object) mainTooltip));
        else if (!hasReplacementSelected)
          replaceBtnTooltip.SetText(string.Format("{0}{1}", (object) Tr.ReplaceVehicle__NoVehicleSelected, (object) mainTooltip));
        else if (!canReplaceCurrentVehicle)
          replaceBtnTooltip.SetText(string.Format("{0}{1}", (object) Tr.ReplaceVehicle__NoDepot, (object) mainTooltip));
        else
          replaceBtnTooltip.SetText((LocStrFormatted) Tr.ReplaceVehicle__MainTooltip);
      }));
      this.Updater = updaterBuilder.Build();

      void onPickerHide()
      {
        overlay.Hide<Panel>();
        plusBtnHolder.PutToLeftOf<Panel>((IUiElement) closure_0.m_container, 40f, Offset.Left(closure_0.m_replaceBtn.GetWidth() + 10f));
      }
    }

    private void onReplaceClick()
    {
      if (this.m_currentVehicle == null || !this.m_replacementProto.HasValue)
        return;
      this.m_context.InputScheduler.ScheduleInputCmd<ReplaceVehicleCmd>(new ReplaceVehicleCmd(this.m_currentVehicle.Id, this.m_replacementProto.Value.Id));
    }

    private void onVehicleSelected(DrivingEntityProto proto)
    {
      this.setVehicleProto((Option<DrivingEntityProto>) proto);
      this.m_context.VehiclesReplacementMap.SetReplacementFor(this.m_currentProto, proto);
      this.m_protoPicker.Hide();
    }

    private void setVehicleProto(Option<DrivingEntityProto> proto)
    {
      this.m_replacementProto = proto;
      this.m_fuelIcon.SetVisibility<IconContainer>(proto.HasValue && proto.Value.FuelTankProto.HasValue);
      if (this.m_replacementProto.HasValue)
      {
        this.m_vehicleIcon.SetIcon(this.m_replacementProto.Value.Graphics.IconPath, ColorRgba.White);
        this.m_fuelIcon.SetIcon(this.m_replacementProto.Value.FuelTankProto.ValueOrNull?.Product.IconPath ?? "");
        this.m_btnTooltip.SetText((LocStrFormatted) proto.Value.Strings.Name);
      }
      else
      {
        this.m_vehicleIcon.SetIcon("Assets/Unity/UserInterface/General/NoVehicleSelected.svg", (ColorRgba) 11447982);
        this.m_btnTooltip.SetText(LocStrFormatted.Empty);
      }
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_protoPicker.SetVisibleProtos((IEnumerable<DrivingEntityProto>) this.m_context.UnlockedProtosDbForUi.FilterUnlocked<DrivingEntityProto>(this.m_context.VehiclesReplacementMap.AllVehicles.Where<DrivingEntityProto>((Func<DrivingEntityProto, bool>) (x => (Proto) x != (Proto) this.m_currentProto))).OrderBy<DrivingEntityProto, float>((Func<DrivingEntityProto, float>) (p => p.UIOrder)), true);
        this.m_protoPicker.Show();
      }
    }

    private void onIconMouseEnter()
    {
      if (!this.m_plusBtn.IsEnabled)
        return;
      this.m_vehicleIcon.PutTo<IconContainer>((IUiElement) this.m_plusBtn, this.m_iconOffset - Offset.All(1f));
    }

    private void onIconMouseLeave()
    {
      this.m_vehicleIcon.PutTo<IconContainer>((IUiElement) this.m_plusBtn, this.m_iconOffset);
    }
  }
}
