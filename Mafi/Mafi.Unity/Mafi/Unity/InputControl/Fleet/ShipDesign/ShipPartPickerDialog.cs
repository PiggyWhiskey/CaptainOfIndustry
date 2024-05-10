// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipPartPickerDialog
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Fleet;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.ShipDesign
{
  public class ShipPartPickerDialog : WindowView
  {
    private GridContainer m_gridContainer;
    private ShipSlotIconContent m_noPartView;
    private ViewsCacheHomogeneous<ShipSlotContent> m_viewCache;
    private readonly Set<FleetEntityPartProto> m_unlockedParts;

    public ShipPartPickerDialog(UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_unlockedParts = new Set<FleetEntityPartProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector("PartPicker", WindowView.FooterStyle.Flat);
      ShipPartPickerDialog partPickerDialog = this;
      this.m_unlockedParts.AddRange(unlockedProtosDb.AllUnlocked<FleetEntityPartProto>());
      unlockedProtosDb.OnUnlockedSetChangedForUi += (Action) (() => partPickerDialog.m_unlockedParts.AddRange(unlockedProtosDb.AllUnlocked<FleetEntityPartProto>()));
    }

    protected override void BuildWindowContent()
    {
      this.m_viewCache = new ViewsCacheHomogeneous<ShipSlotContent>((Func<ShipSlotContent>) (() => new ShipSlotContent(this.Builder)));
      this.m_noPartView = new ShipSlotIconContent(this.Builder, "Assets/Unity/UserInterface/General/Empty128.png");
      this.m_noPartView.SetGroupName(Tr.Empty.TranslatedString);
      this.m_gridContainer = this.Builder.NewGridContainer("Grid").SetInnerPadding(Offset.All(4f)).SetCellSpacing(4f).SetDynamicHeightMode(4).SetCellSize(new Vector2(100f, 125f)).PutToLeftTopOf<GridContainer>((IUiElement) this.GetContentPanel(), Vector2.zero);
    }

    public void SetUpgrades(
      ShipSlotWrapper slotGroup,
      bool isReadOnly,
      Action<ShipSlotWrapper, Option<FleetEntityPartProto>> onItemClick)
    {
      this.m_gridContainer.ClearAll();
      this.m_viewCache.ReturnAll();
      Option<FleetEntityPartProto> existingPart = slotGroup.Group.ExistingPart;
      Option<FleetEntityPartProto> selectedPart = slotGroup.SelectedPart;
      if (slotGroup.ForbidDowngrade && existingPart.HasValue || isReadOnly && selectedPart.HasValue)
      {
        this.m_noPartView.Hide<ShipSlotIconContent>();
      }
      else
      {
        this.m_noPartView.Show<ShipSlotIconContent>();
        this.m_noPartView.AppendTo<ShipSlotIconContent>(this.m_gridContainer);
        this.m_noPartView.SetOnClick((Action) (() => onItemClick(slotGroup, (Option<FleetEntityPartProto>) Option.None)));
        this.m_noPartView.SetHighlighted(selectedPart.IsNone);
      }
      bool isNone = existingPart.IsNone;
      foreach (FleetEntityPartProto eligibleItem in slotGroup.Group.Proto.EligibleItems)
      {
        FleetEntityPartProto item = eligibleItem;
        if (this.m_unlockedParts.Contains(item))
        {
          ShipSlotContent view = this.m_viewCache.GetView();
          view.SetItem(item, (Action) (() => onItemClick(slotGroup, (Option<FleetEntityPartProto>) item)));
          this.m_gridContainer.Append((IUiElement) view);
          if (item == selectedPart)
            view.SetHighlighted(true);
          isNone |= item == existingPart;
          bool isDisabled = slotGroup.ForbidDowngrade && !isNone || isReadOnly && item != selectedPart;
          view.SetDisabled(isDisabled);
        }
      }
      this.SetTitle((LocStrFormatted) (existingPart.HasValue ? Tr.ReplaceShipPart : Tr.AddNewShipPart));
      this.SetContentSize(this.m_gridContainer.GetRequiredWidth(), this.m_gridContainer.GetHeight());
    }
  }
}
