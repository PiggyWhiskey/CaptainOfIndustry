// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Ships.CargoShipFuelReplaceView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Economy;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Ships
{
  public class CargoShipFuelReplaceView : IUiElementWithUpdater, IUiElement
  {
    private readonly InspectorContext m_context;
    private readonly Func<CargoShip> m_entityProvider;
    private ProductProto m_currentProto;
    private readonly PanelWithShadow m_container;
    private readonly ProtoPicker<ProductProto> m_protoPicker;
    private readonly Btn m_plusBtn;
    private readonly Btn m_replaceBtn;
    private readonly Offset m_iconOffset;
    private readonly IconContainer m_vehicleIcon;
    private readonly TitleTooltip m_btnTooltip;
    private readonly CostTooltipCompact m_costTooltip;

    public IUiUpdater Updater { get; }

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public CargoShipFuelReplaceView(
      UiBuilder builder,
      InspectorContext context,
      IUiElement parent,
      WindowView parentWindow,
      Panel overlay,
      Func<CargoShip> entityProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      CargoShipFuelReplaceView shipFuelReplaceView = this;
      this.m_context = context;
      this.m_entityProvider = entityProvider;
      this.m_protoPicker = new ProtoPicker<ProductProto>(new Action<ProductProto>(this.onFuelSelected), new Func<ProductProto, LocStrFormatted>(this.getTooltipFor));
      this.m_protoPicker.BuildUi(builder);
      this.m_protoPicker.SetTitle((LocStrFormatted) Tr.SelectFuel_Title);
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
      this.m_costTooltip = new CostTooltipCompact(builder);
      this.m_costTooltip.AttachTo<Btn>((IUiElementWithHover<Btn>) this.m_replaceBtn);
      Panel plusBtnHolder = builder.NewPanel("PlusBtnContainer", (IUiElement) this.m_container).PutToLeftOf<Panel>((IUiElement) this.m_container, 40f, Offset.Left(this.m_replaceBtn.GetWidth() + 10f));
      this.m_plusBtn = builder.NewBtn("PlusBtn", parent).OnClick(new Action(this.plusBtnClicked)).PlayErrorSoundWhenDisabled().SetOnMouseEnterLeaveActions(new Action(this.onIconMouseEnter), new Action(this.onIconMouseLeave)).PutTo<Btn>((IUiElement) plusBtnHolder);
      this.m_btnTooltip = new TitleTooltip(builder);
      this.m_btnTooltip.SetMaxWidthOverflow(40);
      this.m_btnTooltip.AttachTo<Btn>((IUiElementWithHover<Btn>) this.m_plusBtn);
      this.m_iconOffset = Offset.All(5f);
      this.m_vehicleIcon = builder.NewIconContainer("Icon", (IUiElement) this.m_plusBtn).PutTo<IconContainer>((IUiElement) this.m_plusBtn, this.m_iconOffset);
      IconContainer tickIcon = builder.NewIconContainer("Tick").SetIcon("Assets/Unity/UserInterface/General/Tick128.png", (ColorRgba) 1219584).PutToRightBottomOf<IconContainer>((IUiElement) this.m_container, 20.Vector2(), Offset.All(1f));
      this.m_container.SetWidth<PanelWithShadow>((float) ((double) this.m_plusBtn.GetWidth() + 10.0 + 5.0 + 40.0));
      this.m_protoPicker.PutToLeftTopOf<ProtoPicker<ProductProto>>((IUiElement) plusBtnHolder, this.m_protoPicker.GetSize(), Offset.Left(plusBtnHolder.GetWidth() - 1f));
      this.m_protoPicker.OnShowStart += (Action) (() =>
      {
        overlay.Show<Panel>();
        plusBtnHolder.SetParent<Panel>((IUiElement) parentWindow);
      });
      this.m_protoPicker.OnHide += new Action(onPickerHide);
      overlay.OnClick(new Action(((View) this.m_protoPicker).Hide));
      parentWindow.OnHide += (Action) (() =>
      {
        if (!shipFuelReplaceView.m_protoPicker.IsVisible)
          return;
        shipFuelReplaceView.m_protoPicker.Hide();
      });
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => entityProvider().PendingFuelToChangeTo)).Observe<Tupple<bool, AssetValue, LocStrFormatted>>((Func<Tupple<bool, AssetValue, LocStrFormatted>>) (() =>
      {
        CargoShip cargoShip = entityProvider();
        if (cargoShip.PendingFuelToChangeTo.HasValue)
        {
          shipFuelReplaceView.m_currentProto = cargoShip.PendingFuelToChangeTo.Value;
          return Tupple.Create<bool, AssetValue, LocStrFormatted>(false, AssetValue.Empty, LocStrFormatted.Empty);
        }
        if (shipFuelReplaceView.m_currentProto == null)
          shipFuelReplaceView.m_currentProto = entityProvider().FuelProto;
        AssetValue cost;
        LocStrFormatted errorMessage;
        return Tupple.Create<bool, AssetValue, LocStrFormatted>(cargoShip.CanChangeFuelTo(shipFuelReplaceView.m_currentProto, out cost, out errorMessage), cost, errorMessage);
      })).Do((Action<Option<ProductProto>, Tupple<bool, AssetValue, LocStrFormatted>>) ((pendingFuel, fuelCheck) =>
      {
        bool first = fuelCheck.First;
        AssetValue second = fuelCheck.Second;
        LocStrFormatted third = fuelCheck.Third;
        LocStrFormatted fuelSwitchTooltip = (LocStrFormatted) TrCore.ShipFuelSwitch__Tooltip;
        closure_0.setFuelProto(pendingFuel.ValueOrNull ?? closure_0.m_currentProto);
        closure_0.m_replaceBtn.SetEnabled(pendingFuel.HasValue | first);
        closure_0.m_plusBtn.SetEnabled(pendingFuel.IsNone);
        tickIcon.SetVisibility<IconContainer>(pendingFuel.HasValue);
        closure_0.m_replaceBtn.SetIcon(pendingFuel.HasValue ? "Assets/Unity/UserInterface/General/Cancel.svg" : "Assets/Unity/UserInterface/General/Repeat.svg");
        closure_0.m_costTooltip.SetCost(second.Products.ToImmutableArray().AsIndexable);
        if (third.IsNotEmpty)
          closure_0.m_costTooltip.SetText(string.Format("{0}\n\n<b>{1}</b>", (object) fuelSwitchTooltip, (object) third).AsLoc(), false);
        else if (pendingFuel.HasValue)
          closure_0.m_costTooltip.SetText(string.Format("{0}\n\n<b>{1}</b>", (object) fuelSwitchTooltip, (object) TrCore.ShipFuelSwitch__InProgress).AsLoc(), false);
        else
          closure_0.m_costTooltip.SetText(fuelSwitchTooltip, false);
      }));
      this.Updater = updaterBuilder.Build();

      void onPickerHide()
      {
        overlay.Hide<Panel>();
        plusBtnHolder.PutToLeftOf<Panel>((IUiElement) closure_0.m_container, 40f, Offset.Left(closure_0.m_replaceBtn.GetWidth() + 10f));
      }
    }

    private LocStrFormatted getTooltipFor(ProductProto product)
    {
      CargoShip cargoShip = this.m_entityProvider();
      CargoShipProto.FuelData fuelData = cargoShip.Prototype.AvailableFuels.FirstOrDefault((Func<CargoShipProto.FuelData, bool>) (x => (Proto) x.FuelProto == (Proto) product));
      if (fuelData == null)
        return LocStrFormatted.Empty;
      string str = cargoShip.FuelPerJourneyNeeded(fuelData).ToString();
      return string.Format("{0}: {1} / {2}", (object) Tr.Consumption, (object) str, (object) Tr.FuelPerJourneySuffix).AsLoc();
    }

    private void onReplaceClick()
    {
      CargoShip cargoShip = this.m_entityProvider();
      ProductProto productProto = cargoShip.PendingFuelToChangeTo.HasValue ? cargoShip.FuelProto : this.m_currentProto;
      this.m_context.InputScheduler.ScheduleInputCmd<CargoShipReplaceFuelCmd>(new CargoShipReplaceFuelCmd(cargoShip.Id, productProto.Id));
    }

    private void onFuelSelected(ProductProto proto)
    {
      this.setFuelProto(proto);
      this.m_protoPicker.Hide();
    }

    private void setFuelProto(ProductProto proto)
    {
      this.m_currentProto = proto;
      this.m_vehicleIcon.SetIcon(this.m_currentProto.Graphics.IconPath, ColorRgba.White);
      this.m_btnTooltip.SetText((LocStrFormatted) proto.Strings.Name);
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        this.m_protoPicker.SetVisibleProtos(this.m_entityProvider().Prototype.AvailableFuels.Where((Func<CargoShipProto.FuelData, bool>) (x =>
        {
          if (!((Proto) x.FuelProto != (Proto) this.m_currentProto))
            return false;
          return x.LockingProto.IsNone || this.m_context.UnlockedProtosDbForUi.IsUnlocked((IProto) x.LockingProto.Value);
        })).Select<CargoShipProto.FuelData, ProductProto>((Func<CargoShipProto.FuelData, ProductProto>) (x => x.FuelProto)));
        this.m_protoPicker.Show();
      }
    }

    private void onIconMouseEnter()
    {
      if (!this.m_plusBtn.IsEnabled)
        return;
      this.m_vehicleIcon.PutTo<IconContainer>((IUiElement) this.m_plusBtn, this.m_iconOffset - Offset.All(2f));
    }

    private void onIconMouseLeave()
    {
      this.m_vehicleIcon.PutTo<IconContainer>((IUiElement) this.m_plusBtn, this.m_iconOffset);
    }
  }
}
