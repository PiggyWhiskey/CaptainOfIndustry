// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ShortcutsManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  /// <summary>
  /// Note that this map is not complete. There can be another shortcuts defined in mods (e.g. base mode).
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ShortcutsManager
  {
    private Lyst<KeyCode> m_keysOn;
    private KeyCode m_keyDown;
    private KeyCode m_keyUp;
    private Lyst<KeyCode> m_newKeysOn;
    private Lyst<KeyCode> m_tempKeys;
    private Dict<KeyCode, Lyst<KeyBinding>> m_collisionLookup;
    private int m_lastSeenRevision;
    private readonly KeyCode[] m_keys;
    private readonly Mafi.Event m_onKeyBindingsChanged;

    public KeyBindings PrimaryAction => ShortcutsStorage.Current.PrimaryAction;

    public bool IsPrimaryActionDown => this.IsDown(this.PrimaryAction);

    public bool IsPrimaryActionUp => this.IsUp(this.PrimaryAction);

    public bool IsPrimaryActionOn => this.IsOn(this.PrimaryAction);

    public KeyBindings SecondaryAction => ShortcutsStorage.Current.SecondaryAction;

    public bool IsSecondaryActionDown => this.IsDown(this.SecondaryAction);

    public bool IsSecondaryActionUp => this.IsUp(this.SecondaryAction);

    public bool IsSecondaryActionOn => this.IsOn(this.SecondaryAction);

    public KeyBindings FreeLookMode => ShortcutsStorage.Current.FreeLookMode;

    public KeyBindings MoveUp => ShortcutsStorage.Current.MoveUp;

    public KeyBindings MoveLeft => ShortcutsStorage.Current.MoveLeft;

    public KeyBindings MoveDown => ShortcutsStorage.Current.MoveDown;

    public KeyBindings MoveRight => ShortcutsStorage.Current.MoveRight;

    public KeyBindings PanSpeedBoost => ShortcutsStorage.Current.PanSpeedBoost;

    public KeyBindings ZoomIn => ShortcutsStorage.Current.ZoomIn;

    public KeyBindings ZoomOut => ShortcutsStorage.Current.ZoomOut;

    public KeyBindings PanCamera => ShortcutsStorage.Current.PanCamera;

    public KeyBindings RotateClockwise => ShortcutsStorage.Current.RotateClockwise;

    public KeyBindings RotateCounterClockwise => ShortcutsStorage.Current.RotateCounterClockwise;

    public KeyBindings CameraSave1 => ShortcutsStorage.Current.SaveCameraPosition1;

    public KeyBindings CameraSave2 => ShortcutsStorage.Current.SaveCameraPosition2;

    public KeyBindings CameraSave3 => ShortcutsStorage.Current.SaveCameraPosition3;

    public KeyBindings CameraLoad1 => ShortcutsStorage.Current.JumpToCameraPosition1;

    public KeyBindings CameraLoad2 => ShortcutsStorage.Current.JumpToCameraPosition2;

    public KeyBindings CameraLoad3 => ShortcutsStorage.Current.JumpToCameraPosition3;

    public KeyBindings PauseGame => ShortcutsStorage.Current.PauseGame;

    public KeyBindings IncreaseGameSpeed => ShortcutsStorage.Current.IncreaseGameSpeed;

    public KeyBindings DecreaseGameSpeed => ShortcutsStorage.Current.DecreaseGameSpeed;

    public KeyBindings SetGameSpeedTo0 => ShortcutsStorage.Current.SetGameSpeedTo0;

    public KeyBindings SetGameSpeedTo1 => ShortcutsStorage.Current.SetGameSpeedTo1;

    public KeyBindings SetGameSpeedTo2 => ShortcutsStorage.Current.SetGameSpeedTo2;

    public KeyBindings SetGameSpeedTo3 => ShortcutsStorage.Current.SetGameSpeedTo3;

    public KeyBindings ToggleDeleteTool => ShortcutsStorage.Current.ToggleDeleteTool;

    public KeyBindings ToggleInstaCopyTool => ShortcutsStorage.Current.ToggleInstaCopyTool;

    public KeyBindings ToggleCopyTool => ShortcutsStorage.Current.ToggleCopyTool;

    public KeyBindings ToggleInstaCutTool => ShortcutsStorage.Current.ToggleInstaCutTool;

    public KeyBindings ToggleCutTool => ShortcutsStorage.Current.ToggleCutTool;

    public KeyBindings ToggleCloneConfigTool => ShortcutsStorage.Current.ToggleCloneConfigTool;

    public KeyBindings ToggleUpointsTool => ShortcutsStorage.Current.ToggleUnityTool;

    public KeyBindings TogglePropsRemovalTool => ShortcutsStorage.Current.TogglePropsRemovalTool;

    public KeyBindings TogglePauseTool => ShortcutsStorage.Current.TogglePauseTool;

    public KeyBindings ToggleUpgradeTool => ShortcutsStorage.Current.ToggleUpgradeTool;

    public KeyBindings ToggleMiningTool => ShortcutsStorage.Current.ToggleMiningTool;

    public KeyBindings ToggleDumpingTool => ShortcutsStorage.Current.ToggleDumpingTool;

    public KeyBindings ToggleLevelingTool => ShortcutsStorage.Current.ToggleLevelingTool;

    public KeyBindings ToggleSurfacingTool => ShortcutsStorage.Current.ToggleSurfacingTool;

    public KeyBindings ToggleTreeHarvestingTool
    {
      get => ShortcutsStorage.Current.ToggleTreeHarvestingTool;
    }

    public KeyBindings TogglePlanningMode => ShortcutsStorage.Current.TogglePlanningMode;

    public KeyBindings TogglePricePopup => ShortcutsStorage.Current.TogglePricePopup;

    public KeyBindings ClearDesignation => ShortcutsStorage.Current.ClearDesignation;

    public KeyBindings RaiseUp => ShortcutsStorage.Current.RaiseUp;

    public KeyBindings LowerDown => ShortcutsStorage.Current.LowerDown;

    public KeyBindings Rotate => ShortcutsStorage.Current.Rotate;

    public KeyBindings Flip => ShortcutsStorage.Current.Flip;

    public KeyBindings PlaceMultiple => ShortcutsStorage.Current.PlaceMultiple;

    public KeyBindings TransportTieBreak => ShortcutsStorage.Current.TransportTieBreak;

    public KeyBindings TransportNoTurn => ShortcutsStorage.Current.TransportNoTurn;

    public KeyBindings TransportSnapping => ShortcutsStorage.Current.TransportSnapping;

    public KeyBindings LiftSnapping => ShortcutsStorage.Current.LiftSnapping;

    public KeyBindings TransportPortsBlocking => ShortcutsStorage.Current.TransportPortsBlocking;

    public KeyBindings DeleteEntireTransport => ShortcutsStorage.Current.DeleteEntireTransport;

    public KeyBindings DeleteWithQuickRemove => ShortcutsStorage.Current.DeleteWithQuickRemove;

    public KeyBindings CopyExcludingSettings => ShortcutsStorage.Current.CopyExcludingSettings;

    public KeyBindings PauseMore => ShortcutsStorage.Current.PauseMore;

    public KeyBindings ToggleTutorials => ShortcutsStorage.Current.ToggleTutorials;

    public KeyBindings ToggleRecipeBook => ShortcutsStorage.Current.ToggleRecipesBook;

    public KeyBindings ToggleConsole => ShortcutsStorage.Current.ToggleConsole;

    public KeyBindings ToggleMap => ShortcutsStorage.Current.ToggleMap;

    public KeyBindings ToggleResearchWindow => ShortcutsStorage.Current.ToggleResearchWindow;

    public KeyBindings ToggleStats => ShortcutsStorage.Current.ToggleStats;

    public KeyBindings ToggleBlueprints => ShortcutsStorage.Current.ToggleBlueprints;

    public KeyBindings ToggleTradePanel => ShortcutsStorage.Current.ToggleTradePanel;

    public KeyBindings ToggleResVis => ShortcutsStorage.Current.ToggleResVis;

    public KeyBindings ToggleTransportMenu => ShortcutsStorage.Current.ToggleTransportMenu;

    public KeyBindings ToggleCaptainsOffice => ShortcutsStorage.Current.ToggleCaptainsOffice;

    public KeyBindings TogglePhotoMode => ShortcutsStorage.Current.TogglePhotoMode;

    public KeyBindings TakePicture => ShortcutsStorage.Current.PhotoModeTakePicture;

    public KeyBindings PhotoModeRotation => ShortcutsStorage.Current.PhotoModeRotation;

    public KeyBindings Search => ShortcutsStorage.Current.Search;

    public KeyBindings EditorApplyChanges => ShortcutsStorage.Current.ApplyChanges;

    public KeyBindings Undo => ShortcutsStorage.Current.Undo;

    public KeyBindings Redo => ShortcutsStorage.Current.Redo;

    public Mafi.Event OnKeyBindingsChanged => this.m_onKeyBindingsChanged;

    public ShortcutMode CurrentMode { get; private set; }

    public ShortcutsManager(IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_keysOn = new Lyst<KeyCode>();
      this.m_newKeysOn = new Lyst<KeyCode>();
      this.m_tempKeys = new Lyst<KeyCode>();
      this.m_collisionLookup = new Dict<KeyCode, Lyst<KeyBinding>>();
      this.m_lastSeenRevision = -1;
      this.m_onKeyBindingsChanged = new Mafi.Event(ThreadType.Main);
      this.Cancel = KeyCode.Escape;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      gameLoopEvents.InputUpdate.AddNonSaveable<ShortcutsManager>(this, new Action<GameTime>(this.inputUpdate));
      this.m_keys = Enum.GetValues(typeof (KeyCode)).Cast<KeyCode>().ToArray<KeyCode>();
      this.CurrentMode = ShortcutMode.Game;
    }

    public void SetShortcutsMode(ShortcutMode mode) => this.CurrentMode = mode;

    internal void NotifyKeyBindingsChanged() => this.m_onKeyBindingsChanged.Invoke();

    private void onMapChange()
    {
      this.m_lastSeenRevision = ShortcutsStorage.LastVersion;
      this.m_collisionLookup.Clear();
      foreach (KeyValuePair<PropertyInfo, KbAttribute> allAttribute in ShortcutsStorage.GetAllAttributes())
      {
        KeyBindings keyBindings = (KeyBindings) allAttribute.Key.GetValue((object) ShortcutsStorage.Current);
        addToCollision(keyBindings.Primary);
        addToCollision(keyBindings.Secondary);
      }
      this.m_collisionLookup = this.m_collisionLookup.Where<KeyValuePair<KeyCode, Lyst<KeyBinding>>>((Func<KeyValuePair<KeyCode, Lyst<KeyBinding>>, bool>) (x => x.Value.Count > 1)).ToDict<KeyValuePair<KeyCode, Lyst<KeyBinding>>, KeyCode, Lyst<KeyBinding>>((Func<KeyValuePair<KeyCode, Lyst<KeyBinding>>, KeyCode>) (x => x.Key), (Func<KeyValuePair<KeyCode, Lyst<KeyBinding>>, Lyst<KeyBinding>>) (x => x.Value));

      void addToCollision(KeyBinding binding)
      {
        foreach (KeyCode key in binding.Keys)
          this.m_collisionLookup.GetOrAdd<KeyCode, Lyst<KeyBinding>>(key, (Func<KeyCode, Lyst<KeyBinding>>) (_ => new Lyst<KeyBinding>())).Add(binding);
      }
    }

    private void inputUpdate(GameTime time)
    {
      if (this.m_lastSeenRevision != ShortcutsStorage.LastVersion)
        this.onMapChange();
      this.m_newKeysOn.Clear();
      this.m_keyDown = KeyCode.None;
      this.m_keyUp = KeyCode.None;
      foreach (KeyCode key in this.m_keys)
      {
        if (Input.GetKeyDown(key))
          this.m_keyDown = key;
        if (Input.GetKeyUp(key))
          this.m_keyUp = key;
        if (Input.GetKey(key))
          this.m_newKeysOn.Add(key);
      }
      this.m_tempKeys.Clear();
      foreach (KeyCode keyCode in this.m_keysOn)
      {
        if (this.m_newKeysOn.Contains(keyCode))
          this.m_tempKeys.Add(keyCode);
      }
      foreach (KeyCode keyCode in this.m_newKeysOn)
      {
        if (!this.m_tempKeys.Contains(keyCode))
          this.m_tempKeys.Add(keyCode);
      }
      Swap.Them<Lyst<KeyCode>>(ref this.m_keysOn, ref this.m_tempKeys);
    }

    /// <summary>Whether key was just released in last frame.</summary>
    public bool IsUp(KeyBindings bindings)
    {
      return (bindings.Mode & this.CurrentMode) != (ShortcutMode) 0 && (this.isUp(bindings.Primary) || this.isUp(bindings.Secondary));
    }

    /// <summary>Whether key was just pressed down in last frame.</summary>
    public bool IsDown(KeyBindings bindings)
    {
      return (bindings.Mode & this.CurrentMode) != (ShortcutMode) 0 && (this.isDown(bindings.Primary) || this.isDown(bindings.Secondary));
    }

    /// <summary>Whether key is being held down.</summary>
    public bool IsOn(KeyBindings bindings)
    {
      return (bindings.Mode & this.CurrentMode) != (ShortcutMode) 0 && (this.isBeingHeld(bindings.Primary) || this.isBeingHeld(bindings.Secondary));
    }

    private bool isBeingHeld(KeyBinding binding)
    {
      if (binding.Keys.IsEmpty)
        return false;
      bool flag = isBeingHeldInternal(binding);
      Lyst<KeyBinding> lyst;
      if (flag && binding.Category != KbCategory.General && this.m_collisionLookup.TryGetValue(binding.Keys.First, out lyst))
      {
        foreach (KeyBinding keyBinding in lyst)
        {
          if (keyBinding.Keys.Length > binding.Keys.Length && isBeingHeldInternal(keyBinding) && this.areInConflict(binding, keyBinding))
            return false;
        }
      }
      return flag;

      bool isBeingHeldInternal(KeyBinding b)
      {
        if (this.m_keysOn.Count < b.Keys.Length)
          return false;
        int num = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < this.m_keysOn.Count && index1 < b.Keys.Length; ++index2)
        {
          if (this.m_keysOn[index2] == b.Keys[index1])
          {
            ++index1;
            ++num;
          }
        }
        return num == b.Keys.Length;
      }
    }

    private bool isDown(KeyBinding binding)
    {
      if (binding.Keys.IsEmpty)
        return false;
      bool flag = isDownInternal(binding);
      Lyst<KeyBinding> lyst;
      if (flag && binding.Category != KbCategory.General && this.m_collisionLookup.TryGetValue(binding.Keys.First, out lyst))
      {
        foreach (KeyBinding keyBinding in lyst)
        {
          if (keyBinding.Keys.Length > binding.Keys.Length && isDownInternal(keyBinding) && this.areInConflict(binding, keyBinding))
            return false;
        }
      }
      return flag;

      bool isDownInternal(KeyBinding b)
      {
        if (this.m_keyDown != b.Keys.Last)
          return false;
        if (b.Keys.Length == 1)
          return true;
        if (this.m_keysOn.Count < b.Keys.Length - 1)
          return false;
        int num = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < this.m_keysOn.Count && index1 < b.Keys.Length; ++index2)
        {
          if (this.m_keysOn[index2] == b.Keys[index1])
          {
            ++index1;
            ++num;
          }
        }
        return num >= b.Keys.Length - 1;
      }
    }

    private bool isUp(KeyBinding binding)
    {
      if (binding.Keys.IsEmpty)
        return false;
      bool flag = isUpInternal(binding);
      Lyst<KeyBinding> lyst;
      if (flag && binding.Category != KbCategory.General && this.m_collisionLookup.TryGetValue(binding.Keys.First, out lyst))
      {
        foreach (KeyBinding keyBinding in lyst)
        {
          if (keyBinding.Keys.Length > binding.Keys.Length && isUpInternal(keyBinding) && this.areInConflict(binding, keyBinding))
            return false;
        }
      }
      return flag;

      bool isUpInternal(KeyBinding b)
      {
        if (this.m_keyUp != b.Keys.Last)
          return false;
        if (b.Keys.Length == 1)
          return true;
        if (this.m_keysOn.Count < b.Keys.Length - 1)
          return false;
        int num = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < this.m_keysOn.Count && index1 < b.Keys.Length; ++index2)
        {
          if (this.m_keysOn[index2] == b.Keys[index1])
          {
            ++index1;
            ++num;
          }
        }
        return num >= b.Keys.Length - 1;
      }
    }

    public KeyCode Cancel { get; }

    internal string ResolveShortcutToString(
      string name,
      Func<ShortcutsManager, KeyBindings> shortcut = null)
    {
      string str = "";
      if (shortcut != null)
      {
        try
        {
          str = shortcut(this).ToNiceString();
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception when resolving a shortcut for " + name + "!");
        }
      }
      return str;
    }

    private bool areInConflict(KeyBinding first, KeyBinding second)
    {
      return first.Category == second.Category || !KbCategoriesData.MutuallyExclusiveCategories.Contains(first.Category) || !KbCategoriesData.MutuallyExclusiveCategories.Contains(second.Category);
    }
  }
}
