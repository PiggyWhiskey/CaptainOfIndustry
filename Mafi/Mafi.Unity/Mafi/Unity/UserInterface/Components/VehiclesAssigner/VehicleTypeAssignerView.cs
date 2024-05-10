// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.VehiclesAssigner.VehicleTypeAssignerView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.VehiclesAssigner
{
  /// <summary>Handles assignment of exactly one prototype.</summary>
  public class VehicleTypeAssignerView : IUiElement
  {
    private readonly Panel m_container;
    private readonly UiBuilder m_builder;
    private readonly DynamicGroundEntityProto m_protoToAssign;
    private readonly CameraController m_cameraController;
    private Lyst<Mafi.Core.Entities.Dynamic.Vehicle> m_assignedVehicles;
    private VehicleStats m_vehicleStats;
    private int m_assignedVehiclesCount;
    private readonly Btn m_minusButton;
    private bool m_canBeAssigned;
    public const int HEIGHT = 60;
    public const int WIDTH = 139;
    public bool IsHiddenBecauseOwnedIsZero;
    private int m_lastClickedIndex;

    public event Action<bool> VisibilityChanged;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public VehicleTypeAssignerView(
      UiBuilder builder,
      DrivingEntityProto proto,
      UnlockedProtosDb unlockedProtosDb,
      CameraController cameraController,
      IVehiclesManager vehiclesManager,
      Func<IEntityAssignedWithVehicles> entityProvider,
      Action<DrivingEntityProto> onAssignCmd,
      Action<DrivingEntityProto> onUnassignCmd)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      VehicleTypeAssignerView typeAssignerView = this;
      this.m_protoToAssign = (DynamicGroundEntityProto) proto;
      this.m_cameraController = cameraController;
      this.m_builder = builder;
      UiStyle style = this.m_builder.Style;
      AudioSource sharedAudio1 = this.m_builder.AudioDb.GetSharedAudio(this.m_builder.Audio.Assign);
      AudioSource sharedAudio2 = this.m_builder.AudioDb.GetSharedAudio(this.m_builder.Audio.Unassign);
      this.m_container = builder.NewPanel("Assigner");
      Panel icon = builder.NewPanel("Icon").SetBackground(proto.Graphics.IconPath).OnClick(new Action(this.onIconClick)).PutToLeftOf<Panel>((IUiElement) this.m_container, 60f);
      TitleTooltip iconTooltip = new TitleTooltip(this.m_builder);
      iconTooltip.SetMaxWidthOverflow(40);
      iconTooltip.SetText((LocStrFormatted) proto.Strings.Name);
      if (proto.FuelTankProto.HasValue)
        builder.NewPanel("Icon").SetBackground(proto.FuelTankProto.Value.Product.IconPath).OnClick(new Action(this.onIconClick)).PutToLeftBottomOf<Panel>((IUiElement) icon, 18.Vector2(), Offset.Bottom(4f));
      IconContainer showIcon = builder.NewIconContainer("ShowIcon").SetIcon("Assets/Unity/UserInterface/General/Hide128.png").PutToRightTopOf<IconContainer>((IUiElement) icon, 12.Vector2(), Offset.Top(2f));
      icon.SetOnMouseEnterLeaveActions((Action) (() =>
      {
        if (closure_0.m_assignedVehiclesCount > 0)
          showIcon.Show<IconContainer>();
        iconTooltip.Show((IUiElement) icon);
      }), (Action) (() =>
      {
        showIcon.Hide<IconContainer>();
        iconTooltip.Hide();
      }));
      showIcon.Hide<IconContainer>();
      float y = 22.5f;
      Offset offset1 = Offset.Left(icon.GetWidth() + 5f);
      Txt txt1 = this.m_builder.NewTxt("Assigned");
      TextStyle boldText = style.Global.BoldText;
      ref TextStyle local1 = ref boldText;
      int? nullable1 = new int?(16);
      ColorRgba? color1 = new ColorRgba?();
      FontStyle? fontStyle1 = new FontStyle?();
      int? fontSize1 = nullable1;
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
      Txt assignedCount = txt1.SetTextStyle(textStyle1).SetAlignment(TextAnchor.LowerRight).PutToLeftTopOf<Txt>((IUiElement) this.m_container, new Vector2(32f, y), Offset.Top(4f) + offset1);
      Txt txt2 = this.m_builder.NewTxt("Available");
      TextStyle text = style.Global.Text;
      ref TextStyle local2 = ref text;
      ColorRgba? color2 = new ColorRgba?((ColorRgba) 12369084);
      FontStyle? fontStyle2 = new FontStyle?();
      int? nullable2 = new int?();
      int? fontSize2 = nullable2;
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
      Txt availableCount = txt2.SetTextStyle(textStyle2).SetAlignment(TextAnchor.UpperRight).PutToLeftTopOf<Txt>((IUiElement) this.m_container, new Vector2(32f, y), Offset.Top((float) (4.0 + (double) y + 4.0)) + offset1);
      Offset offset2 = offset1 + Offset.Left(assignedCount.GetWidth() + 10f);
      Btn btn1 = this.m_builder.NewBtn("PlusButton");
      BtnStyle primaryBtn = style.Global.PrimaryBtn;
      ref BtnStyle local3 = ref primaryBtn;
      nullable2 = new int?(16);
      ColorRgba? color3 = new ColorRgba?();
      FontStyle? fontStyle3 = new FontStyle?();
      int? fontSize3 = nullable2;
      bool? isCapitalized3 = new bool?();
      BtnStyle buttonStyle1 = local3.ExtendText(color3, fontStyle3, fontSize3, isCapitalized3);
      Btn plusButton = btn1.SetButtonStyle(buttonStyle1).PlayErrorSoundWhenDisabled().SetText("+").OnClick((Action) (() => onAssignCmd(proto)), sharedAudio1).PutToLeftTopOf<Btn>((IUiElement) this.m_container, new Vector2(32f, y), Offset.Top(5f) + offset2);
      Tooltip plusBtnTooltip = plusButton.AddToolTipAndReturn();
      Btn btn2 = this.m_builder.NewBtn("MinusButton");
      BtnStyle minusPrimaryBtn = style.Global.MinusPrimaryBtn;
      ref BtnStyle local4 = ref minusPrimaryBtn;
      nullable2 = new int?(16);
      ColorRgba? color4 = new ColorRgba?();
      FontStyle? fontStyle4 = new FontStyle?();
      int? fontSize4 = nullable2;
      bool? isCapitalized4 = new bool?();
      BtnStyle buttonStyle2 = local4.ExtendText(color4, fontStyle4, fontSize4, isCapitalized4);
      this.m_minusButton = btn2.SetButtonStyle(buttonStyle2).SetText("-").OnClick((Action) (() => onUnassignCmd(proto)), sharedAudio2).PutToLeftTopOf<Btn>((IUiElement) this.m_container, new Vector2(32f, y), Offset.Top((float) (5.0 + (double) y + 5.0)) + offset2);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Mafi.Core.Entities.Dynamic.Vehicle>((Func<IIndexable<Mafi.Core.Entities.Dynamic.Vehicle>>) (() => entityProvider().AllVehiclesWithProto((DynamicEntityProto) typeAssignerView.m_protoToAssign)), (ICollectionComparator<Mafi.Core.Entities.Dynamic.Vehicle, IIndexable<Mafi.Core.Entities.Dynamic.Vehicle>>) CompareFixedOrder<Mafi.Core.Entities.Dynamic.Vehicle>.Instance).Do((Action<Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>) (assignedVehicles =>
      {
        closure_0.m_lastClickedIndex = 0;
        closure_0.m_assignedVehicles = assignedVehicles;
        assignedCount.SetText(assignedVehicles.Count.ToStringCached());
        closure_0.m_assignedVehiclesCount = assignedVehicles.Count;
        closure_0.updateVisibility();
      }));
      updaterBuilder.Observe<VehicleStats>((Func<VehicleStats>) (() => vehiclesManager.GetStats((DynamicEntityProto) typeAssignerView.m_protoToAssign))).Observe<bool>((Func<bool>) (() => entityProvider().CanVehicleBeAssigned((DynamicEntityProto) typeAssignerView.m_protoToAssign))).Observe<bool>((Func<bool>) (() => unlockedProtosDb.IsUnlocked((Proto) typeAssignerView.m_protoToAssign))).Do((Action<VehicleStats, bool, bool>) ((stats, canBeAssigned, isUnlocked) =>
      {
        closure_0.m_canBeAssigned = canBeAssigned && stats.Owned > 0;
        closure_0.IsHiddenBecauseOwnedIsZero = canBeAssigned & isUnlocked && stats.Owned == 0;
        availableCount.SetText(stats.Assignable.ToStringCached());
        plusButton.SetEnabled(stats.Assignable > 0);
        plusBtnTooltip.SetText((LocStrFormatted) (stats.Assignable > 0 ? Tr.AssignVehicleBtn__Tooltip : Tr.AssignVehicleBtn__NotAvailable));
        closure_0.m_vehicleStats = stats;
        closure_0.updateVisibility();
      }));
      this.Updater = updaterBuilder.Build();
    }

    private void updateVisibility()
    {
      bool flag = this.m_assignedVehiclesCount > 0 || this.m_canBeAssigned;
      this.m_minusButton.SetVisibility<Btn>(this.m_assignedVehiclesCount > 0);
      if (flag == this.IsVisible())
        return;
      Action<bool> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged(flag);
    }

    private void onIconClick()
    {
      if (this.m_assignedVehicles.Count == 0)
        return;
      this.m_lastClickedIndex %= this.m_assignedVehicles.Count;
      if (this.m_lastClickedIndex >= this.m_assignedVehicles.Count)
        return;
      this.m_cameraController.PanTo(this.m_assignedVehicles[this.m_lastClickedIndex].Position2f);
      ++this.m_lastClickedIndex;
    }
  }
}
