// Decompiled with JetBrains decompiler
// Type: RTG.Hotkeys
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class Hotkeys
  {
    private static List<KeyCode> _availableKeys;
    private static List<string> _availableKeyNames;
    [SerializeField]
    private bool _isEnabled;
    [SerializeField]
    private KeyCode _key;
    [SerializeField]
    private bool _lCtrl;
    [SerializeField]
    private bool _lCmd;
    [SerializeField]
    private bool _lAlt;
    [SerializeField]
    private bool _lShift;
    [SerializeField]
    private bool _useStrictModifierCheck;
    [SerializeField]
    private bool _lMouseBtn;
    [SerializeField]
    private bool _rMouseBtn;
    [SerializeField]
    private bool _mMouseBtn;
    [SerializeField]
    private bool _useStrictMouseCheck;
    [SerializeField]
    private string _name;
    [NonSerialized]
    private List<Hotkeys> _potentialOverlaps;
    [SerializeField]
    private HotkeysStaticData _staticData;

    static Hotkeys()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Hotkeys._availableKeys = new List<KeyCode>();
      Hotkeys._availableKeys.Add(KeyCode.Space);
      Hotkeys._availableKeys.Add(KeyCode.Backspace);
      Hotkeys._availableKeys.Add(KeyCode.Return);
      Hotkeys._availableKeys.Add(KeyCode.Tab);
      Hotkeys._availableKeys.Add(KeyCode.Delete);
      Hotkeys._availableKeys.Add(KeyCode.LeftBracket);
      Hotkeys._availableKeys.Add(KeyCode.RightBracket);
      for (int index = 97; index <= 122; ++index)
        Hotkeys._availableKeys.Add((KeyCode) index);
      for (int index = 48; index <= 57; ++index)
        Hotkeys._availableKeys.Add((KeyCode) index);
      Hotkeys._availableKeys.Add(KeyCode.None);
      Hotkeys._availableKeyNames = new List<string>();
      for (int index = 0; index < Hotkeys._availableKeys.Count; ++index)
        Hotkeys._availableKeyNames.Add(Hotkeys._availableKeys[index].ToString());
    }

    public static List<KeyCode> AvailableKeys
    {
      get => new List<KeyCode>((IEnumerable<KeyCode>) Hotkeys._availableKeys);
    }

    public static List<string> AvailableKeyNames
    {
      get => new List<string>((IEnumerable<string>) Hotkeys._availableKeyNames);
    }

    public bool IsEnabled
    {
      get => this._isEnabled;
      set => this._isEnabled = value;
    }

    public string Name => this._name;

    public KeyCode Key
    {
      get => this._key;
      set
      {
        if (!Hotkeys._availableKeys.Contains(value))
          return;
        this._key = value;
      }
    }

    public bool LCtrl
    {
      get => this._lCtrl;
      set => this._lCtrl = value;
    }

    public bool LCmd
    {
      get => this._lCmd;
      set => this._lCmd = value;
    }

    public bool LAlt
    {
      get => this._lAlt;
      set => this._lAlt = value;
    }

    public bool LShift
    {
      get => this._lShift;
      set => this._lShift = value;
    }

    public bool LMouseButton
    {
      get => this._lMouseBtn;
      set => this._lMouseBtn = value;
    }

    public bool RMouseButton
    {
      get => this._rMouseBtn;
      set => this._rMouseBtn = value;
    }

    public bool MMouseButton
    {
      get => this._mMouseBtn;
      set => this._mMouseBtn = value;
    }

    public bool UseStrictMouseCheck
    {
      get => this._useStrictMouseCheck;
      set => this._useStrictMouseCheck = value;
    }

    public bool UseStrictModifierCheck
    {
      get => this._useStrictModifierCheck;
      set => this._useStrictModifierCheck = value;
    }

    public Hotkeys(string name)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isEnabled = true;
      this._useStrictModifierCheck = true;
      this._name = nameof (Hotkeys);
      this._potentialOverlaps = new List<Hotkeys>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._name = name;
      this._key = KeyCode.None;
      this._staticData = new HotkeysStaticData()
      {
        CanHaveMouseButtons = true
      };
    }

    public Hotkeys(string name, HotkeysStaticData staticData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isEnabled = true;
      this._useStrictModifierCheck = true;
      this._name = nameof (Hotkeys);
      this._potentialOverlaps = new List<Hotkeys>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._name = name;
      this._key = KeyCode.None;
      this._staticData = staticData;
    }

    public static void EstablishPotentialOverlaps(List<Hotkeys> hotkeysCollection)
    {
      foreach (Hotkeys hotkeys1 in hotkeysCollection)
      {
        foreach (Hotkeys hotkeys2 in hotkeysCollection)
          hotkeys1.AddPotentialOverlap(hotkeys2);
      }
    }

    public int GetNumModifiers()
    {
      int numModifiers = 0;
      if (this.LAlt)
        ++numModifiers;
      if (this.LCtrl)
        ++numModifiers;
      if (this.LShift)
        ++numModifiers;
      return numModifiers;
    }

    public int GetNumMouseButtons()
    {
      int numMouseButtons = 0;
      if (this.LMouseButton)
        ++numMouseButtons;
      if (this.RMouseButton)
        ++numMouseButtons;
      if (this.MMouseButton)
        ++numMouseButtons;
      return numMouseButtons;
    }

    public List<MouseButton> GetAllUsedMouseButtons()
    {
      if (this.GetNumMouseButtons() == 0)
        return new List<MouseButton>();
      List<MouseButton> usedMouseButtons = new List<MouseButton>(3);
      if (this.LMouseButton)
        usedMouseButtons.Add(MouseButton.Left);
      if (this.RMouseButton)
        usedMouseButtons.Add(MouseButton.Right);
      if (this.MMouseButton)
        usedMouseButtons.Add(MouseButton.Middle);
      return usedMouseButtons;
    }

    public bool UsesMouseButtons(List<MouseButton> buttons)
    {
      List<MouseButton> usedMouseButtons = this.GetAllUsedMouseButtons();
      foreach (MouseButton button in buttons)
      {
        if (!usedMouseButtons.Contains(button))
          return false;
      }
      return true;
    }

    public List<KeyCode> GetAllUsedModifiers()
    {
      if (this.GetNumMouseButtons() == 0)
        return new List<KeyCode>();
      List<KeyCode> allUsedModifiers = new List<KeyCode>(3);
      if (this.LAlt)
        allUsedModifiers.Add(KeyCode.LeftAlt);
      if (this.LShift)
        allUsedModifiers.Add(KeyCode.LeftShift);
      if (this.LCtrl)
        allUsedModifiers.Add(KeyCode.LeftControl);
      return allUsedModifiers;
    }

    public bool UsesModifiers(List<KeyCode> modifiers)
    {
      List<KeyCode> allUsedModifiers = this.GetAllUsedModifiers();
      foreach (KeyCode modifier in modifiers)
      {
        if (!allUsedModifiers.Contains(modifier))
          return false;
      }
      return true;
    }

    public void AddPotentialOverlap(Hotkeys hotkeys)
    {
      if (hotkeys == null || hotkeys == this || this.ContainsPotentialOverlap(hotkeys))
        return;
      this._potentialOverlaps.Add(hotkeys);
    }

    public bool ContainsPotentialOverlap(Hotkeys hotkeys)
    {
      return this._potentialOverlaps.Contains(hotkeys);
    }

    public bool IsOverlappedBy(Hotkeys hotkeys)
    {
      return hotkeys != null && hotkeys != this && this.GetNumModifiers() <= hotkeys.GetNumModifiers() && this.GetNumMouseButtons() <= hotkeys.GetNumMouseButtons() && hotkeys.Key == this.Key && hotkeys.UsesModifiers(this.GetAllUsedModifiers()) && hotkeys.UsesMouseButtons(this.GetAllUsedMouseButtons());
    }

    public bool IsActive(bool checkForOverlaps = true)
    {
      if (!this.IsEnabled || this.IsEmpty() || this.Key != KeyCode.None && !RTInput.IsKeyPressed(this.Key) || this.UseStrictModifierCheck && this.HasNoModifiers() && this.IsAnyModifierKeyPressed() || this._lCtrl && !RTInput.IsKeyPressed(KeyCode.LeftControl) || this._lCmd && !RTInput.IsKeyPressed(KeyCode.LeftMeta) || this._lAlt && !RTInput.IsKeyPressed(KeyCode.LeftAlt) || this._lShift && !RTInput.IsKeyPressed(KeyCode.LeftShift) || this.UseStrictMouseCheck && this.HasNoMouseButtons() && this.IsAnyMouseButtonPressed() || this._lMouseBtn && !RTInput.IsLeftMouseButtonPressed() || this._rMouseBtn && !RTInput.IsRightMouseButtonPressed() || this._mMouseBtn && !RTInput.IsMiddleMouseButtonPressed())
        return false;
      if (checkForOverlaps)
      {
        foreach (Hotkeys potentialOverlap in this._potentialOverlaps)
        {
          if (potentialOverlap.IsActive(false) && this.IsOverlappedBy(potentialOverlap))
            return false;
        }
      }
      return true;
    }

    public bool IsActiveInFrame(bool checkForOverlaps = true)
    {
      if (!this.IsEnabled || this.IsEmpty() || this.Key != KeyCode.None && !RTInput.WasKeyPressedThisFrame(this.Key) || this.UseStrictModifierCheck && this.HasNoModifiers() && this.IsAnyModifierKeyPressed() || this._lCtrl && !RTInput.IsKeyPressed(KeyCode.LeftControl) || this._lCmd && !RTInput.IsKeyPressed(KeyCode.LeftMeta) || this._lAlt && !RTInput.IsKeyPressed(KeyCode.LeftAlt) || this._lShift && !RTInput.IsKeyPressed(KeyCode.LeftShift) || this.UseStrictMouseCheck && this.HasNoMouseButtons() && this.IsAnyMouseButtonPressed() || this._lMouseBtn && !RTInput.WasLeftMouseButtonPressedThisFrame() || this._rMouseBtn && !RTInput.WasRightMouseButtonPressedThisFrame() || this._mMouseBtn && !RTInput.WasMiddleMouseButtonPressedThisFrame())
        return false;
      if (checkForOverlaps)
      {
        foreach (Hotkeys potentialOverlap in this._potentialOverlaps)
        {
          if (potentialOverlap.IsActiveInFrame(false) && this.IsOverlappedBy(potentialOverlap))
            return false;
        }
      }
      return true;
    }

    public bool HasNoKeys() => this.Key == KeyCode.None;

    /// <summary>
    /// Checks if the shortcut has any modifiers assigned to it.
    /// </summary>
    public bool HasNoModifiers() => !this._lAlt && !this._lCmd && !this._lCtrl && !this._lShift;

    /// <summary>
    /// Checks if the shortcut has any mouse buttons assigned to it.
    /// </summary>
    public bool HasNoMouseButtons() => !this._lMouseBtn && !this._rMouseBtn && !this._mMouseBtn;

    /// <summary>
    /// Checks if no keys, mouse buttons or modifier keys are available for this shortcut.
    /// </summary>
    public bool IsEmpty() => this.HasNoKeys() && this.HasNoModifiers() && this.HasNoMouseButtons();

    /// <summary>Checks if at least one modifier key is pressed.</summary>
    private bool IsAnyModifierKeyPressed()
    {
      return RTInput.IsKeyPressed(KeyCode.LeftControl) || RTInput.IsKeyPressed(KeyCode.LeftMeta) || RTInput.IsKeyPressed(KeyCode.LeftAlt) || RTInput.IsKeyPressed(KeyCode.LeftShift);
    }

    /// <summary>Checks if at least one mouse button is pressed.</summary>
    private bool IsAnyMouseButtonPressed()
    {
      return RTInput.IsLeftMouseButtonPressed() || RTInput.IsRightMouseButtonPressed() || RTInput.IsMiddleMouseButtonPressed();
    }
  }
}
