// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ShortcutsMap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public class ShortcutsMap
  {
    public static readonly KeyCode[] AllKeys;
    public const ShortcutMode ANY = ShortcutMode.Game | ShortcutMode.MapEditor;
    public const ShortcutMode GAME = ShortcutMode.Game;
    public const ShortcutMode EDITOR = ShortcutMode.MapEditor;

    public static IEnumerable<KeyValuePair<KbCategory, LocStrFormatted>> Categories
    {
      get
      {
        ShortcutsMap.\u003Cget_Categories\u003Ed__1 categories = new ShortcutsMap.\u003Cget_Categories\u003Ed__1(-2);
        return (IEnumerable<KeyValuePair<KbCategory, LocStrFormatted>>) categories;
      }
    }

    public static IEnumerable<KeyValuePair<PropertyInfo, KbAttribute>> GetKeybindings(
      KbCategory category)
    {
      return (IEnumerable<KeyValuePair<PropertyInfo, KbAttribute>>) ((IEnumerable<PropertyInfo>) typeof (ShortcutsMap).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.HasAttribute<KbAttribute>())).Select<PropertyInfo, KeyValuePair<PropertyInfo, KbAttribute>>((Func<PropertyInfo, KeyValuePair<PropertyInfo, KbAttribute>>) (x => Make.Kvp<PropertyInfo, KbAttribute>(x, (KbAttribute) x.GetCustomAttributes(typeof (KbAttribute), false).First<object>()))).Where<KeyValuePair<PropertyInfo, KbAttribute>>((Func<KeyValuePair<PropertyInfo, KbAttribute>, bool>) (x => x.Value.Category == category)).ToArray<KeyValuePair<PropertyInfo, KbAttribute>>();
    }

    [Kb(KbCategory.Camera, "MoveUp", "Move up", null, false, false)]
    public KeyBindings MoveUp { get; set; }

    [Kb(KbCategory.Camera, "MoveLeft", "Move left", null, false, false)]
    public KeyBindings MoveLeft { get; set; }

    [Kb(KbCategory.Camera, "MoveDown", "Move down", null, false, false)]
    public KeyBindings MoveDown { get; set; }

    [Kb(KbCategory.Camera, "MoveRight", "Move right", null, false, false)]
    public KeyBindings MoveRight { get; set; }

    [Kb(KbCategory.Camera, "RotateClockwise", "Rotate clock-wise", null, false, false)]
    public KeyBindings RotateClockwise { get; set; }

    [Kb(KbCategory.Camera, "RotateCounterClockwise", "Rotate counter-clock-wise", null, false, false)]
    public KeyBindings RotateCounterClockwise { get; set; }

    [Kb(KbCategory.Camera, "PanSpeedBoost", "Increase pan speed", null, true, false)]
    public KeyBindings PanSpeedBoost { get; set; }

    [Kb(KbCategory.Camera, "FreeLookMode", "Free look mode", null, true, false)]
    public KeyBindings FreeLookMode { get; set; }

    [Kb(KbCategory.Camera, "ZoomIn", "Zoom in", null, false, false)]
    public KeyBindings ZoomIn { get; set; }

    [Kb(KbCategory.Camera, "ZoomOut", "Zoom out", null, false, false)]
    public KeyBindings ZoomOut { get; set; }

    [Kb(KbCategory.Camera, "PanCamera", "Move camera (hold)", null, true, false)]
    public KeyBindings PanCamera { get; set; }

    [Kb(KbCategory.Camera, "SaveCameraPosition1", "Save camera position {0}", "1", "", false, false)]
    public KeyBindings SaveCameraPosition1 { get; set; }

    [Kb(KbCategory.Camera, "SaveCameraPosition2", "Save camera position {0}", "2", "", false, false)]
    public KeyBindings SaveCameraPosition2 { get; set; }

    [Kb(KbCategory.Camera, "SaveCameraPosition3", "Save camera position {0}", "3", "", false, false)]
    public KeyBindings SaveCameraPosition3 { get; set; }

    [Kb(KbCategory.Camera, "JumpToCameraPosition1", "Jump to saved camera position {0}", "1", "", false, false)]
    public KeyBindings JumpToCameraPosition1 { get; set; }

    [Kb(KbCategory.Camera, "JumpToCameraPosition2", "Jump to saved camera position {0}", "2", "", false, false)]
    public KeyBindings JumpToCameraPosition2 { get; set; }

    [Kb(KbCategory.Camera, "JumpToCameraPosition3", "Jump to saved camera position {0}", "3", "", false, false)]
    public KeyBindings JumpToCameraPosition3 { get; set; }

    [Kb(KbCategory.Speed, "PauseGame", "Toggle pause", null, false, false)]
    public KeyBindings PauseGame { get; set; }

    [Kb(KbCategory.Speed, "IncreaseGameSpeed", "Increase speed", null, false, false)]
    public KeyBindings IncreaseGameSpeed { get; set; }

    [Kb(KbCategory.Speed, "DecreaseGameSpeed", "Decrease speed", null, false, false)]
    public KeyBindings DecreaseGameSpeed { get; set; }

    [Kb(KbCategory.Speed, "SetGameSpeedTo0", "Set game speed to {0}x", "0", "", false, false)]
    public KeyBindings SetGameSpeedTo0 { get; set; }

    [Kb(KbCategory.Speed, "SetGameSpeedTo1", "Set game speed to {0}x", "1", "", false, false)]
    public KeyBindings SetGameSpeedTo1 { get; set; }

    [Kb(KbCategory.Speed, "SetGameSpeedTo2", "Set game speed to {0}x", "2", "", false, false)]
    public KeyBindings SetGameSpeedTo2 { get; set; }

    [Kb(KbCategory.Speed, "SetGameSpeedTo3", "Set game speed to {0}x", "3", "", false, false)]
    public KeyBindings SetGameSpeedTo3 { get; set; }

    [Kb(KbCategory.Tools, "ToggleDeleteTool", "Demolish / remove", null, false, false)]
    public KeyBindings ToggleDeleteTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleInstaCopyTool", "Copy (insta-copy)", "Will also insta-copy on press while you are hovering over an entity", false, false)]
    public KeyBindings ToggleInstaCopyTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleCopyTool", "Copy", null, false, false)]
    public KeyBindings ToggleCopyTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleInstaCutTool", "Cut (insta-cut)", "Will also insta-cut on press while you are hovering over an entity", false, false)]
    public KeyBindings ToggleInstaCutTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleCutTool", "Cut", null, false, false)]
    public KeyBindings ToggleCutTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleCloneConfigTool", "Clone", null, false, false)]
    public KeyBindings ToggleCloneConfigTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleUnityTool", "Unity tool", null, false, false)]
    public KeyBindings ToggleUnityTool { get; set; }

    [Kb(KbCategory.Tools, "TogglePropsRemovalTool", "Debris removal tool", null, false, false)]
    public KeyBindings TogglePropsRemovalTool { get; set; }

    [Kb(KbCategory.Tools, "TogglePauseTool", "Pause tool", null, false, false)]
    public KeyBindings TogglePauseTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleUpgradeTool", "Upgrade tool", null, false, false)]
    public KeyBindings ToggleUpgradeTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleMiningTool", "Mining designations", null, false, false)]
    public KeyBindings ToggleMiningTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleDumpingTool", "Dumping designations", null, false, false)]
    public KeyBindings ToggleDumpingTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleLevelingTool", "Leveling designations", null, false, false)]
    public KeyBindings ToggleLevelingTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleSurfacingTool", "Surface designations", null, false, false)]
    public KeyBindings ToggleSurfacingTool { get; set; }

    [Kb(KbCategory.Tools, "ToggleTreeHarvestingTool", "Harvesting designations", null, false, false)]
    public KeyBindings ToggleTreeHarvestingTool { get; set; }

    [Kb(KbCategory.Tools, "TogglePlanningMode", "Toggle planning mode", null, false, false)]
    public KeyBindings TogglePlanningMode { get; set; }

    [Kb(KbCategory.Tools, "TogglePricePopup", "Toggle price popup", null, false, false)]
    public KeyBindings TogglePricePopup { get; set; }

    [Kb(KbCategory.Build, "RaiseUp", "Raise up", null, false, false)]
    public KeyBindings RaiseUp { get; set; }

    [Kb(KbCategory.Build, "LowerDown", "Lower down", null, false, false)]
    public KeyBindings LowerDown { get; set; }

    [Kb(KbCategory.Build, "Rotate", "Rotate", null, false, false)]
    public KeyBindings Rotate { get; set; }

    [Kb(KbCategory.Build, "Flip", "Flip / Toggle mode", null, false, false)]
    public KeyBindings Flip { get; set; }

    [Kb(KbCategory.Build, "PlaceMultiple", "Place multiple", null, true, false)]
    public KeyBindings PlaceMultiple { get; set; }

    [Kb(KbCategory.Designation, "ClearDesignation", "Clear designation", null, true, true)]
    public KeyBindings ClearDesignation { get; set; }

    [Kb(KbCategory.Transport, "TransportTieBreak", "Use alternative route", null, true, false)]
    public KeyBindings TransportTieBreak { get; set; }

    [Kb(KbCategory.Transport, "TransportNoTurn", "Disallow turns", null, true, false)]
    public KeyBindings TransportNoTurn { get; set; }

    [Kb(KbCategory.Transport, "TransportSnapping", "Toggle snapping", null, false, false)]
    public KeyBindings TransportSnapping { get; set; }

    [Kb(KbCategory.Transport, "LiftSnapping", "Toggle snapping", null, false, false)]
    public KeyBindings LiftSnapping { get; set; }

    [Kb(KbCategory.Transport, "TransportPortsBlocking", "Toggle port avoidance", null, false, false)]
    public KeyBindings TransportPortsBlocking { get; set; }

    [Kb(KbCategory.Demolish, "DeleteEntireTransport", "Demolish entire transport", null, true, false)]
    public KeyBindings DeleteEntireTransport { get; set; }

    [Kb(KbCategory.Demolish, "DeleteWithQuickRemove", "With quick remove (using Unity)", null, true, false)]
    public KeyBindings DeleteWithQuickRemove { get; set; }

    [Kb(KbCategory.CopyTool, "CopyExcludingSettings", "Copy without configuration", "Hold while copying structures to exclude copying their configuration", true, false)]
    public KeyBindings CopyExcludingSettings { get; set; }

    [Kb(KbCategory.PauseTool, "PauseMore", "Pause only", "Hold to make the pause tool to only pause things instead of automatically toggling pause.", true, false)]
    public KeyBindings PauseMore { get; set; }

    [Kb(KbCategory.Windows, "ToggleTutorials", "Tutorials", null, false, false)]
    public KeyBindings ToggleTutorials { get; set; }

    [Kb(KbCategory.Windows, "ToggleRecipesBook", "Recipes book", null, false, false)]
    public KeyBindings ToggleRecipesBook { get; set; }

    [Kb(KbCategory.Windows, "ToggleConsole", "Console", null, false, false)]
    public KeyBindings ToggleConsole { get; set; }

    [Kb(KbCategory.Windows, "ToggleMap", "World map", null, false, false)]
    public KeyBindings ToggleMap { get; set; }

    [Kb(KbCategory.Windows, "ToggleResearchWindow", "Research", null, false, false)]
    public KeyBindings ToggleResearchWindow { get; set; }

    [Kb(KbCategory.Windows, "ToggleStats", "Statistics", null, false, false)]
    public KeyBindings ToggleStats { get; set; }

    [Kb(KbCategory.Windows, "ToggleTradePanel", "Trading", null, false, false)]
    public KeyBindings ToggleTradePanel { get; set; }

    [Kb(KbCategory.Windows, "ToggleBlueprints", "Blueprints", null, false, false)]
    public KeyBindings ToggleBlueprints { get; set; }

    [Kb(KbCategory.Windows, "ToggleResVis", "Resources visualization", null, false, false)]
    public KeyBindings ToggleResVis { get; set; }

    [Kb(KbCategory.Windows, "ToggleTransportMenu", "Transports menu", null, false, false)]
    public KeyBindings ToggleTransportMenu { get; set; }

    [Kb(KbCategory.Windows, "ToggleCaptainsOffice", "Captain's office (if constructed)", null, false, false)]
    public KeyBindings ToggleCaptainsOffice { get; set; }

    [Kb(KbCategory.General, "PrimaryAction", "Primary action / select", null, false, true)]
    public KeyBindings PrimaryAction { get; set; }

    [Kb(KbCategory.General, "SecondaryAction", "Alternative action / deselect", null, false, true)]
    public KeyBindings SecondaryAction { get; set; }

    [Kb(KbCategory.General, "Search", "Search", null, false, false)]
    public KeyBindings Search { get; set; }

    [Kb(KbCategory.PhotoMode, "TogglePhotoMode", "Toggle photo mode", "Photo mode allows taking high-quality screenshots (better quality than default screen-grab).", false, false)]
    public KeyBindings TogglePhotoMode { get; set; }

    [Kb(KbCategory.PhotoMode, "PhotoModeTakePicture", "Take screenshot", null, true, false)]
    public KeyBindings PhotoModeTakePicture { get; set; }

    [Kb(KbCategory.PhotoMode, "PhotoModeRotation", "Camera auto-rotation", null, true, false)]
    public KeyBindings PhotoModeRotation { get; set; }

    [Kb(KbCategory.MapEditor, "ApplyChanges", "Apply changes", null, false, false)]
    public KeyBindings ApplyChanges { get; set; }

    [Kb(KbCategory.MapEditor, "Undo", "Undo", null, false, false)]
    public KeyBindings Undo { get; set; }

    [Kb(KbCategory.MapEditor, "Redo", "Redo", null, false, false)]
    public KeyBindings Redo { get; set; }

    public ShortcutsMap()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMoveUp\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.W, KeyCode.UpArrow);
      // ISSUE: reference to a compiler-generated field
      this.\u003CMoveLeft\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.A, KeyCode.LeftArrow);
      // ISSUE: reference to a compiler-generated field
      this.\u003CMoveDown\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.S, KeyCode.DownArrow);
      // ISSUE: reference to a compiler-generated field
      this.\u003CMoveRight\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.D, KeyCode.RightArrow);
      // ISSUE: reference to a compiler-generated field
      this.\u003CRotateClockwise\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftAlt, KeyCode.E);
      // ISSUE: reference to a compiler-generated field
      this.\u003CRotateCounterClockwise\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftAlt, KeyCode.Q);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPanSpeedBoost\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftShift, KeyCode.RightShift);
      // ISSUE: reference to a compiler-generated field
      this.\u003CFreeLookMode\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Mouse2, KeyCode.LeftAlt, KeyCode.Mouse0);
      // ISSUE: reference to a compiler-generated field
      this.\u003CZoomIn\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftBracket, KeyCode.KeypadDivide);
      // ISSUE: reference to a compiler-generated field
      this.\u003CZoomOut\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.RightBracket, KeyCode.KeypadMultiply);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPanCamera\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Mouse1);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSaveCameraPosition1\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftControl, KeyCode.Alpha4);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSaveCameraPosition2\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftControl, KeyCode.Alpha5);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSaveCameraPosition3\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftControl, KeyCode.Alpha6);
      // ISSUE: reference to a compiler-generated field
      this.\u003CJumpToCameraPosition1\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Alpha4);
      // ISSUE: reference to a compiler-generated field
      this.\u003CJumpToCameraPosition2\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Alpha5);
      // ISSUE: reference to a compiler-generated field
      this.\u003CJumpToCameraPosition3\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Camera, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Alpha6);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPauseGame\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Speed, ShortcutMode.Game, KeyCode.Space, KeyCode.Pause);
      // ISSUE: reference to a compiler-generated field
      this.\u003CIncreaseGameSpeed\u003Ek__BackingField = new KeyBindings(ShortcutMode.Game, KeyBinding.FromKeys(KbCategory.Speed, KeyCode.LeftShift, KeyCode.Equals), KeyBinding.FromKey(KbCategory.Speed, KeyCode.KeypadPlus));
      // ISSUE: reference to a compiler-generated field
      this.\u003CDecreaseGameSpeed\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Speed, ShortcutMode.Game, KeyCode.Minus, KeyCode.KeypadMinus);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSetGameSpeedTo0\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Speed, ShortcutMode.Game, KeyCode.Alpha0, KeyCode.Keypad0);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSetGameSpeedTo1\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Speed, ShortcutMode.Game, KeyCode.Alpha1, KeyCode.Keypad1);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSetGameSpeedTo2\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Speed, ShortcutMode.Game, KeyCode.Alpha2, KeyCode.Keypad2);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSetGameSpeedTo3\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Speed, ShortcutMode.Game, KeyCode.Alpha3, KeyCode.Keypad3);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleDeleteTool\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Tools, ShortcutMode.Game, KeyCode.Delete, KeyCode.LeftControl, KeyCode.D);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleInstaCopyTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.C);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleCopyTool\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Tools, ShortcutMode.Game, KeyCode.LeftControl, KeyCode.C);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleInstaCutTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.X);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleCutTool\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Tools, ShortcutMode.Game, KeyCode.LeftControl, KeyCode.X);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleCloneConfigTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.V);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleUnityTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.U);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTogglePropsRemovalTool\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.Tools, ShortcutMode.Game, KeyCode.LeftControl, KeyCode.U);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTogglePauseTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.P);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleUpgradeTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.I);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleMiningTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.M);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleDumpingTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.Z);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleLevelingTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.N);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleSurfacingTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.Less);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleTreeHarvestingTool\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.H);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTogglePlanningMode\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.B);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTogglePricePopup\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Build, ShortcutMode.Game, KeyCode.Y);
      // ISSUE: reference to a compiler-generated field
      this.\u003CRaiseUp\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Build, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.E);
      // ISSUE: reference to a compiler-generated field
      this.\u003CLowerDown\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Build, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Q);
      // ISSUE: reference to a compiler-generated field
      this.\u003CRotate\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Build, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.R);
      // ISSUE: reference to a compiler-generated field
      this.\u003CFlip\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Build, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.F);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPlaceMultiple\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Build, ShortcutMode.Game, KeyCode.LeftShift, KeyCode.RightShift);
      // ISSUE: reference to a compiler-generated field
      this.\u003CClearDesignation\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Build, ShortcutMode.Game, KeyCode.Mouse1, KeyCode.LeftControl, KeyCode.Mouse0);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTransportTieBreak\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Transport, ShortcutMode.Game, KeyCode.LeftControl, KeyCode.RightControl);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTransportNoTurn\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Transport, ShortcutMode.Game, KeyCode.LeftShift, KeyCode.RightShift);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTransportSnapping\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Transport, ShortcutMode.Game, KeyCode.R);
      // ISSUE: reference to a compiler-generated field
      this.\u003CLiftSnapping\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Transport, ShortcutMode.Game, KeyCode.J);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTransportPortsBlocking\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Transport, ShortcutMode.Game, KeyCode.F);
      // ISSUE: reference to a compiler-generated field
      this.\u003CDeleteEntireTransport\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Demolish, ShortcutMode.Game, KeyCode.LeftShift, KeyCode.RightShift);
      // ISSUE: reference to a compiler-generated field
      this.\u003CDeleteWithQuickRemove\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.Demolish, ShortcutMode.Game, KeyCode.LeftControl, KeyCode.RightControl);
      // ISSUE: reference to a compiler-generated field
      this.\u003CCopyExcludingSettings\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.CopyTool, ShortcutMode.Game, KeyCode.LeftControl, KeyCode.RightControl);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPauseMore\u003Ek__BackingField = KeyBindings.FromKeys(KbCategory.PauseTool, ShortcutMode.Game, KeyCode.LeftShift, KeyCode.RightShift);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleTutorials\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.F2);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleRecipesBook\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.F1);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleConsole\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.BackQuote);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleMap\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.Tab);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleResearchWindow\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.G);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleStats\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.O);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleTradePanel\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.F3);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleBlueprints\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.F4);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleResVis\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.L);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleTransportMenu\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.T);
      // ISSUE: reference to a compiler-generated field
      this.\u003CToggleCaptainsOffice\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.Windows, ShortcutMode.Game, KeyCode.F5);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPrimaryAction\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.General, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Mouse0);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSecondaryAction\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.General, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.Mouse1);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSearch\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.General, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.LeftControl, KeyCode.F);
      // ISSUE: reference to a compiler-generated field
      this.\u003CTogglePhotoMode\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.PhotoMode, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.F11);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPhotoModeTakePicture\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.PhotoMode, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.P);
      // ISSUE: reference to a compiler-generated field
      this.\u003CPhotoModeRotation\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.PhotoMode, ShortcutMode.Game | ShortcutMode.MapEditor, KeyCode.O);
      // ISSUE: reference to a compiler-generated field
      this.\u003CApplyChanges\u003Ek__BackingField = KeyBindings.FromKey(KbCategory.MapEditor, ShortcutMode.MapEditor, KeyCode.Space);
      // ISSUE: reference to a compiler-generated field
      this.\u003CUndo\u003Ek__BackingField = KeyBindings.FromPrimaryKeys(KbCategory.MapEditor, ShortcutMode.MapEditor, KeyCode.LeftControl, KeyCode.Z);
      // ISSUE: reference to a compiler-generated field
      this.\u003CRedo\u003Ek__BackingField = new KeyBindings(ShortcutMode.MapEditor, KeyBinding.FromKeys(KbCategory.MapEditor, KeyCode.LeftControl, KeyCode.Y), KeyBinding.FromKeys(KbCategory.MapEditor, KeyCode.LeftControl, KeyCode.LeftShift, KeyCode.Z));
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ShortcutsMap()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ShortcutsMap.AllKeys = Enum.GetValues(typeof (KeyCode)).Cast<KeyCode>().ToArray<KeyCode>();
    }
  }
}
