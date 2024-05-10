// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.TransportMenuToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TransportMenuToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
  {
    private readonly AudioSource m_popupSound;
    private readonly AudioSource m_invalidSound;
    private readonly AudioSource m_upSound;
    private readonly AudioSource m_downSound;
    private readonly AudioSource m_rotateSound;
    private readonly AudioSource m_snapChangeSound;
    private Option<Func<bool>> m_onUp;
    private Option<Func<bool>> m_onDown;
    private Option<Action> m_onTieBreak;
    private Option<Action> m_toggleOnlyStraight;
    private Option<Action<bool>> m_setOnlyStraight;
    private Option<Action> m_onToggleSnapping;
    private Option<Action> m_onTogglePortsBlocking;
    private Option<Action> m_onTogglePricePopup;
    private ToggleBtn m_tieBreakBtn;
    private ToggleBtn m_noTurnBtn;
    private ToggleBtn m_snappingBtn;
    private ToggleBtn m_portBlockBtn;
    private ToggleBtn m_pricePopupBtn;

    public TransportMenuToolbox(
      ToolbarController toolbar,
      UiBuilder builder,
      AudioDb audioDb,
      UiAudio audio)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(toolbar, builder);
      this.m_popupSound = audioDb.GetSharedAudio(audio.Unassign);
      this.m_invalidSound = audioDb.GetSharedAudio(audio.InvalidOp);
      this.m_upSound = audioDb.GetSharedAudio(audio.Up);
      this.m_downSound = audioDb.GetSharedAudio(audio.Down);
      this.m_rotateSound = audioDb.GetSharedAudio(audio.Rotate);
      this.m_snapChangeSound = audioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/TransportSnapChange.prefab");
    }

    public void SetOnTransportUp(Option<Func<bool>> action) => this.m_onUp = action;

    public void SetOnTransportDown(Option<Func<bool>> action) => this.m_onDown = action;

    public void SetOnTieBreak(Option<Action> action) => this.m_onTieBreak = action;

    public void DisplayTieBreakActive(bool isDisabled)
    {
      this.BuildIfNeeded();
      this.m_tieBreakBtn.SetIsOn(isDisabled);
    }

    public void SetOnOnlyStraight(Option<Action<bool>> action) => this.m_setOnlyStraight = action;

    public void SetOnToggleOnlyStraight(Option<Action> action)
    {
      this.m_toggleOnlyStraight = action;
    }

    public void DisplayOnlyStraightActive(bool isDisabled)
    {
      this.BuildIfNeeded();
      this.m_noTurnBtn.SetIsOn(isDisabled);
    }

    public void SetOnToggleSnapping(Option<Action> action) => this.m_onToggleSnapping = action;

    public void DisplaySnappingDisabled(bool isDisabled)
    {
      this.BuildIfNeeded();
      this.m_snappingBtn.SetIsOn(isDisabled);
    }

    public void SetOnTogglePortsBlocking(Option<Action> action)
    {
      this.m_onTogglePortsBlocking = action;
    }

    public void DisplayPortsBlockingDisabled(bool isDisabled)
    {
      this.BuildIfNeeded();
      this.m_portBlockBtn.SetIsOn(isDisabled);
    }

    public void SetOnTogglePricePopup(Option<Action> action) => this.m_onTogglePricePopup = action;

    public void DisplayPricePopupDisabled(bool isDisabled)
    {
      this.BuildIfNeeded();
      this.m_pricePopupBtn.SetIsOn(isDisabled);
    }

    protected override void BuildCustomItems(UiBuilder builder)
    {
      this.AddButton("Up", "Assets/Unity/UserInterface/General/PlatformUp128.png", new Action(this.OnUp), (Func<ShortcutsManager, KeyBindings>) (m => m.RaiseUp));
      this.AddButton("Down", "Assets/Unity/UserInterface/General/PlatformDown128.png", new Action(this.OnDown), (Func<ShortcutsManager, KeyBindings>) (m => m.LowerDown));
      this.m_tieBreakBtn = this.AddToggleButton("TieBreak", "Assets/Unity/UserInterface/Toolbox/TieBreaking.svg", (Action<bool>) (_ => this.OnTieBreak()), (Func<ShortcutsManager, KeyBindings>) (m => m.TransportTieBreak), (LocStrFormatted) Tr.TransportTool__TieBreakTooltip);
      this.m_noTurnBtn = this.AddToggleButton("NoTurn", "Assets/Unity/UserInterface/Toolbox/NoTurn.svg", (Action<bool>) (_ => this.OnToggleOnlyStraight()), (Func<ShortcutsManager, KeyBindings>) (m => m.TransportNoTurn), (LocStrFormatted) Tr.TransportTool__NoTurnTooltip);
      this.m_snappingBtn = this.AddToggleButton("PortSnap", "Assets/Unity/UserInterface/Toolbox/NoSnap.svg", (Action<bool>) (_ => this.OnPortSnapping()), (Func<ShortcutsManager, KeyBindings>) (m => m.TransportSnapping), (LocStrFormatted) Tr.TransportTool__PortSnapTooltip);
      this.m_portBlockBtn = this.AddToggleButton("PortBlock", "Assets/Core/Ports/PortBlocked128.png", (Action<bool>) (_ => this.OnTogglePortsBlocking()), (Func<ShortcutsManager, KeyBindings>) (m => m.TransportPortsBlocking), (LocStrFormatted) Tr.TransportTool__PortBlockTooltip);
      this.m_pricePopupBtn = this.AddToggleButton("PriceHide", "Assets/Unity/UserInterface/Toolbox/NoPopup.svg", (Action<bool>) (_ => this.OnTogglePricePopup()), (Func<ShortcutsManager, KeyBindings>) (m => m.TogglePricePopup), (LocStrFormatted) Tr.Toolbox__HideCosts);
      this.AddToToolbar();
    }

    public void OnUp()
    {
      if (this.m_onUp.IsNone)
        return;
      if (this.m_onUp.Value())
        this.m_upSound.Play();
      else
        this.m_invalidSound.Play();
    }

    public void OnDown()
    {
      if (this.m_onDown.IsNone)
        return;
      if (this.m_onDown.Value())
        this.m_downSound.Play();
      else
        this.m_invalidSound.Play();
    }

    public void OnTieBreak()
    {
      if (this.m_onTieBreak.IsNone)
        return;
      this.m_onTieBreak.Value();
      this.m_rotateSound.Play();
    }

    public void OnToggleOnlyStraight()
    {
      if (this.m_setOnlyStraight.IsNone)
        return;
      this.m_toggleOnlyStraight.Value();
      this.m_rotateSound.Play();
    }

    public void OnSetOnlyStraight(bool enabled)
    {
      if (this.m_setOnlyStraight.IsNone)
        return;
      this.m_setOnlyStraight.Value(enabled);
      this.m_rotateSound.Play();
    }

    public void OnPortSnapping()
    {
      if (this.m_onToggleSnapping.IsNone)
        return;
      this.m_onToggleSnapping.Value();
      this.m_snapChangeSound.Play();
    }

    public void OnTogglePortsBlocking()
    {
      if (this.m_onTogglePortsBlocking.IsNone)
        return;
      this.m_onTogglePortsBlocking.Value();
      this.m_snapChangeSound.Play();
    }

    public void OnTogglePricePopup()
    {
      if (this.m_onTogglePricePopup.IsNone)
        return;
      this.m_onTogglePricePopup.Value();
      this.m_popupSound.Play();
    }
  }
}
