// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipSlotView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Fleet;
using Mafi.Core.Prototypes;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.ShipDesign
{
  public class ShipSlotView : IUiElement
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly PanelWithShadow m_container;
    private readonly ShipSlotContent m_content;
    private readonly ShipSlotIconContent m_plus;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ShipSlotView(UiBuilder builder, UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_container = builder.NewPanelWithShadow("Slot").AddShadowRightBottom();
      this.m_content = new ShipSlotContent(builder).PutTo<ShipSlotContent>((IUiElement) this.m_container);
      this.m_plus = new ShipSlotIconContent(builder, "Assets/Unity/UserInterface/General/Plus.svg").PutTo<ShipSlotIconContent>((IUiElement) this.m_container);
    }

    public void SetGroup(
      ShipSlotWrapper groupWrapper,
      Action<ShipSlotWrapper> onPlusClick,
      Action<ShipSlotWrapper> onItemClick)
    {
      this.m_content.Hide<ShipSlotContent>();
      this.m_plus.Hide<ShipSlotIconContent>();
      Option<FleetEntityPartProto> selectedPart = groupWrapper.SelectedPart;
      FleetEntityPartProto fleetEntityPartProto = (FleetEntityPartProto) null;
      foreach (FleetEntityPartProto eligibleItem in groupWrapper.Group.Proto.EligibleItems)
      {
        if (this.m_unlockedProtosDb.IsUnlocked((IProto) eligibleItem))
          fleetEntityPartProto = eligibleItem;
      }
      if (selectedPart.HasValue)
      {
        this.m_content.SetItem(selectedPart.Value, (Action) (() => onItemClick(groupWrapper)));
        this.m_content.SetHighlighted(groupWrapper.Group.ExistingPart != selectedPart);
        this.m_content.SetUpgradeAvailable((Proto) fleetEntityPartProto != (Proto) null && selectedPart != fleetEntityPartProto);
        this.m_content.PutTo<ShipSlotContent>((IUiElement) this.m_container);
        this.m_content.Show<ShipSlotContent>();
      }
      else
      {
        this.m_plus.SetOnClick((Action) (() => onPlusClick(groupWrapper)));
        this.m_plus.SetHighlighted(groupWrapper.Group.ExistingPart.HasValue);
        this.m_plus.PutTo<ShipSlotIconContent>((IUiElement) this.m_container);
        this.m_plus.SetGroupName(groupWrapper.Group.Proto.Strings.Name.TranslatedString);
        this.m_plus.Show<ShipSlotIconContent>();
        this.m_plus.SetEnabled((Proto) fleetEntityPartProto != (Proto) null);
      }
    }
  }
}
