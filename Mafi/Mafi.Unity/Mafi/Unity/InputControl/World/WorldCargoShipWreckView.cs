// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldCargoShipWreckView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  public class WorldCargoShipWreckView : ItemDetailWindowView
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly Action<WorldMapLocation, LocationVisitReason> m_onGoToClick;

    private WorldMapCargoShipWreck Entity { get; set; }

    public WorldCargoShipWreckView(
      IInputScheduler inputScheduler,
      TravelingFleetManager fleetManager,
      Action onClose,
      Action<WorldMapLocation, LocationVisitReason> onGoToClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("WorldMapCargoShipWreckInspector", false);
      this.m_inputScheduler = inputScheduler;
      this.m_fleetManager = fleetManager;
      this.m_onGoToClick = onGoToClick;
      this.SetOnCloseButtonClickAction(onClose);
      this.EnableClippingPrevention();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.SetOnHideSound((Option<AudioSource>) Option.None);
      this.SetOnShowSound((Option<AudioSource>) Option.None);
      this.MakeMovable();
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Txt desc = this.Builder.NewTxt("Desc").SetTextStyle(this.Builder.Style.Panel.Text).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.ItemsContainer, new float?(20f));
      WorldMapRepairView repairPanel = new WorldMapRepairView(this.Builder, this.m_inputScheduler, (Func<WorldMapRepairableEntity>) (() => (WorldMapRepairableEntity) this.Entity));
      repairPanel.AppendTo<WorldMapRepairView>(this.ItemsContainer, new float?(repairPanel.GetHeight()));
      this.AddUpdater(repairPanel.Updater);
      WorldEntityShipOrdersView entityShipOrdersView = new WorldEntityShipOrdersView(this.Builder, this.m_fleetManager, (Func<WorldMapRepairableEntity>) (() => (WorldMapRepairableEntity) this.Entity), this.m_onGoToClick);
      entityShipOrdersView.AppendTo<WorldEntityShipOrdersView>(this.ItemsContainer, new float?(entityShipOrdersView.GetHeight()));
      this.AddUpdater(entityShipOrdersView.Updater);
      updaterBuilder.Observe<WorldMapCargoShipWreckProto>((Func<WorldMapCargoShipWreckProto>) (() => this.Entity.Prototype)).Observe<bool>((Func<bool>) (() => this.Entity.IsRepaired)).Do((Action<WorldMapCargoShipWreckProto, bool>) ((proto, isRepaired) =>
      {
        this.SetTitle((LocStrFormatted) proto.Strings.Name);
        desc.SetText(Tr.NeedsRepairsDesc__Parametrized.Format(proto.Strings.Name.TranslatedString.ToLower()).Value);
        this.ItemsContainer.SetItemVisibility((IUiElement) repairPanel, !isRepaired);
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetWidth(400f);
    }

    public void SetEntity(WorldMapCargoShipWreck entity) => this.Entity = entity;
  }
}
