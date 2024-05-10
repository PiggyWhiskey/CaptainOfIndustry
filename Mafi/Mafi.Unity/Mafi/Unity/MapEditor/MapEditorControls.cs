// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.MapEditorControls
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.InputControl;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public static class MapEditorControls
  {
    public static readonly IEnumerable<ControlsToolboxEntry> None;
    public static readonly IEnumerable<ControlsToolboxEntry> TestEntityPlacement;
    public static readonly IEnumerable<ControlsToolboxEntry> XRay;

    static MapEditorControls()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MapEditorControls.None = (IEnumerable<ControlsToolboxEntry>) Array.Empty<ControlsToolboxEntry>();
      MapEditorControls.TestEntityPlacement = (IEnumerable<ControlsToolboxEntry>) new \u003C\u003Ez__ReadOnlyArray<ControlsToolboxEntry>(new ControlsToolboxEntry[2]
      {
        new ControlsToolboxEntry((Func<ShortcutsManager, KeyBindings>) (mgr => mgr.Rotate), "Assets/Unity/UserInterface/General/Rotate128.png"),
        new ControlsToolboxEntry((Func<ShortcutsManager, KeyBindings>) (mgr => mgr.Flip), "Assets/Unity/UserInterface/General/Flip128.png")
      });
      MapEditorControls.XRay = (IEnumerable<ControlsToolboxEntry>) new \u003C\u003Ez__ReadOnlyArray<ControlsToolboxEntry>(new ControlsToolboxEntry[1]
      {
        new ControlsToolboxEntry(KeyBindings.FromPrimaryKeys(KbCategory.MapEditor, ShortcutMode.MapEditor, KeyCode.LeftControl, (KeyCode) 10077), "Assets/Unity/UserInterface/General/PlatformFlat128.png")
      });
    }
  }
}
