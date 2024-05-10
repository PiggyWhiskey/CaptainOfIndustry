// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.LayoutEntityToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  public class LayoutEntityToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
  {
    private readonly AudioSource m_invalidSound;
    private readonly AudioSource m_upSound;
    private readonly AudioSource m_downSound;
    private readonly AudioSource m_rotateSound;
    private Option<Action> m_onRotate;
    private Option<Action> m_onFlip;
    private Option<Func<bool?>> m_onUp;
    private Option<Func<bool?>> m_onDown;
    private Option<Action> m_onTogglePricePopup;
    private Option<Action> m_onToggleZipperPlacement;
    private Option<Action> m_onToggleSnapping;
    private ToggleBtn m_pricePopupBtn;
    private ToggleBtn m_snappingBtn;
    private ToggleBtn m_zipperBtn;

    public ToggleBtn PlaceMultipleBtn { get; private set; }

    public ToggleBtn DoNotCopyConfigBtn { get; private set; }

    public LayoutEntityToolbox(
      ToolbarController toolbar,
      UiBuilder builder,
      AudioDb audioDb,
      UiAudio audio)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(toolbar, builder);
      this.m_invalidSound = audioDb.GetSharedAudio(audio.InvalidOp);
      this.m_upSound = audioDb.GetSharedAudio(audio.Up);
      this.m_downSound = audioDb.GetSharedAudio(audio.Down);
      this.m_rotateSound = audioDb.GetSharedAudio(audio.Rotate);
    }

    public void SetOnRotate(Action action)
    {
      Assert.That<Option<Action>>(this.m_onRotate).IsNone<Action>();
      this.m_onRotate = (Option<Action>) action;
    }

    public void SetOnFlip(Action action)
    {
      Assert.That<Option<Action>>(this.m_onFlip).IsNone<Action>();
      this.m_onFlip = (Option<Action>) action;
    }

    public void SetOnUp(Func<bool?> action)
    {
      Assert.That<Option<Func<bool?>>>(this.m_onUp).IsNone<Func<bool?>>();
      this.m_onUp = (Option<Func<bool?>>) action;
    }

    public void SetOnDown(Func<bool?> action)
    {
      Assert.That<Option<Func<bool?>>>(this.m_onDown).IsNone<Func<bool?>>();
      this.m_onDown = (Option<Func<bool?>>) action;
    }

    public void SetOnToggleZipperPlacement(Action action)
    {
      this.m_onToggleZipperPlacement = (Option<Action>) action;
    }

    public void SetOnTogglePricePopup(Action action)
    {
      this.m_onTogglePricePopup = (Option<Action>) action;
    }

    public void SetOnToggleSnapping(Action action)
    {
      this.m_onToggleSnapping = (Option<Action>) action;
    }

    public void DisplayPricePopupDisabled(bool isDisabled)
    {
      this.BuildIfNeeded();
      this.m_pricePopupBtn.SetIsOn(isDisabled);
    }

    protected override void BuildCustomItems(UiBuilder builder)
    {
      this.AddButton("Rotate", "Assets/Unity/UserInterface/General/Rotate128.png", (Action) (() =>
      {
        if (!this.m_onRotate.HasValue)
          return;
        this.m_onRotate.Value();
      }), (Func<ShortcutsManager, KeyBindings>) (m => m.Rotate), (LocStrFormatted) Tr.RotateShortcut__Tooltip);
      this.AddButton("Flip", "Assets/Unity/UserInterface/General/Flip128.png", (Action) (() =>
      {
        if (!this.m_onFlip.HasValue)
          return;
        this.m_onFlip.Value();
      }), (Func<ShortcutsManager, KeyBindings>) (m => m.Flip), (LocStrFormatted) Tr.FlipShortcut__Tooltip);
      this.PlaceMultipleBtn = this.AddToggleButton("PlaceMultiple", "Assets/Unity/UserInterface/General/Repeat.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PlaceMultiple), (LocStrFormatted) Tr.PlaceMultipleTooltip);
      this.DoNotCopyConfigBtn = this.AddToggleButton("DoNotCopy", "Assets/Unity/UserInterface/Toolbox/DoNotCopy.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.CopyExcludingSettings), (LocStrFormatted) Tr.CopyTool__NoCopyTooltip);
      this.AddButton("Up", "Assets/Unity/UserInterface/General/PlatformUp128.png", new Action(this.OnUp), (Func<ShortcutsManager, KeyBindings>) (m => m.RaiseUp));
      this.AddButton("Down", "Assets/Unity/UserInterface/General/PlatformDown128.png", new Action(this.OnDown), (Func<ShortcutsManager, KeyBindings>) (m => m.LowerDown));
      this.m_pricePopupBtn = this.AddToggleButton("PriceHide", "Assets/Unity/UserInterface/Toolbox/NoPopup.svg", (Action<bool>) (_ => this.OnTogglePricePopup()), (Func<ShortcutsManager, KeyBindings>) (m => m.TogglePricePopup), (LocStrFormatted) Tr.Toolbox__HideCosts);
      this.m_snappingBtn = this.AddToggleButton("PortSnap", "Assets/Unity/UserInterface/Toolbox/NoSnap.svg", (Action<bool>) (_ => this.OnTogglePortSnapping()), (Func<ShortcutsManager, KeyBindings>) (m => m.LiftSnapping), (LocStrFormatted) Tr.TransportTool__PortSnapTooltip);
      this.m_snappingBtn.Hide<ToggleBtn>();
      this.m_zipperBtn = this.AddToggleButton("PlaceZippers", "TODO", (Action<bool>) (_ => this.OnZipperPlacement()), (Func<ShortcutsManager, KeyBindings>) (m => m.TransportPortsBlocking), (LocStrFormatted) Tr.TransportTool__PortSnapTooltip);
      this.m_zipperBtn.Hide<ToggleBtn>();
      this.AddToToolbar();
    }

    public void SetDoNotCopyConfigVisibility(bool isVisible)
    {
      this.BuildIfNeeded();
      this.SetBtnVisibility((IUiElement) this.DoNotCopyConfigBtn, isVisible);
    }

    public void OnUp()
    {
      if (this.m_onUp.IsNone)
        return;
      this.PlayUpSound(this.m_onUp.Value());
    }

    public void OnDown()
    {
      if (this.m_onDown.IsNone)
        return;
      this.PlayDownSound(this.m_onDown.Value());
    }

    public void PlayUpSound(bool? success)
    {
      bool? nullable1 = success;
      bool flag1 = true;
      if (nullable1.GetValueOrDefault() == flag1 & nullable1.HasValue)
      {
        this.m_upSound.Play();
      }
      else
      {
        bool? nullable2 = success;
        bool flag2 = false;
        if (!(nullable2.GetValueOrDefault() == flag2 & nullable2.HasValue))
          return;
        this.m_invalidSound.Play();
      }
    }

    public void PlayDownSound(bool? success)
    {
      bool? nullable1 = success;
      bool flag1 = true;
      if (nullable1.GetValueOrDefault() == flag1 & nullable1.HasValue)
      {
        this.m_downSound.Play();
      }
      else
      {
        bool? nullable2 = success;
        bool flag2 = false;
        if (!(nullable2.GetValueOrDefault() == flag2 & nullable2.HasValue))
          return;
        this.m_invalidSound.Play();
      }
    }

    public void OnTogglePricePopup()
    {
      if (this.m_onTogglePricePopup.IsNone)
        return;
      this.m_onTogglePricePopup.Value();
      this.m_rotateSound.Play();
    }

    public void DisplaySnappingDisabled(bool isDisabled)
    {
      this.BuildIfNeeded();
      this.m_snappingBtn.SetIsOn(isDisabled);
    }

    public void OnTogglePortSnapping()
    {
      if (!this.m_snappingBtn.IsVisible() || this.m_onToggleSnapping.IsNone)
        return;
      this.m_onToggleSnapping.Value();
      this.m_rotateSound.Play();
    }

    public void OnZipperPlacement()
    {
      if (!this.m_zipperBtn.IsVisible() || this.m_onToggleZipperPlacement.IsNone)
        return;
      this.m_onToggleZipperPlacement.Value();
      this.m_rotateSound.Play();
    }

    public void SetPortSnappingButtonEnabled(bool enabled)
    {
      if (enabled)
        this.m_snappingBtn.Show<ToggleBtn>();
      else
        this.m_snappingBtn.Hide<ToggleBtn>();
    }

    public void SetZipperPlacementButtonEnabled(bool enabled)
    {
      if (enabled)
        this.m_zipperBtn.Show<ToggleBtn>();
      else
        this.m_zipperBtn.Hide<ToggleBtn>();
    }
  }
}
