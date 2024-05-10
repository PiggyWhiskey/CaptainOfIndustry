// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.ControlsToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.InputControl;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class ControlsToolbox : Row
  {
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly Row m_container;

    public ControlsToolbox(ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.JustifyItemsCenter<ControlsToolbox>();
      this.Visible<ControlsToolbox>(false);
      this.Add((UiComponent) (this.m_container = new Row(4.pt())).AlignItemsStretch<Row>());
    }

    public void ShowTools(IEnumerable<ControlsToolboxEntry> entries)
    {
      this.m_container.SetChildren((IEnumerable<UiComponent>) entries.Select<ControlsToolboxEntry, ControlsToolboxEntry>((Func<ControlsToolboxEntry, ControlsToolboxEntry>) (entry => entry.Update(this.m_shortcutsManager))));
      this.Visible<ControlsToolbox>(this.m_container.Count > 0);
    }
  }
}
