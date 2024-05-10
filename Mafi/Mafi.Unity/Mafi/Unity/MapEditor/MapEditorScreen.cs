// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.MapEditorScreen
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UiToolkit.Library.ObjectEditor;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class MapEditorScreen : Row
  {
    public static readonly Px PADDING_TOP_BOTTOM;
    public readonly ObjEditorsDock LeftDock;
    public readonly ObjEditorsDock RightDock;
    public readonly MapEditorToolbar Toolbar;
    private readonly IUiUpdater m_updater;

    public MapEditorScreen(Mafi.Unity.MapEditor.MapEditor mapEditor, MapEditorToolbar mapEditorToolbar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      MapEditorScreen mapEditorScreen = this;
      this.AbsolutePositionFillParent<MapEditorScreen>().IgnoreInputPicking<MapEditorScreen>().AlignItemsStretch<MapEditorScreen>();
      UiComponent[] uiComponentArray = new UiComponent[3];
      Column component1 = new Column();
      component1.Add<Column>((Action<Column>) (c =>
      {
        Column component2 = c;
        Px? nullable1 = new Px?(0.px());
        Px? top = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? nullable2 = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? right = new Px?();
        Px? bottom = nullable2;
        Px? left = nullable1;
        component2.AbsolutePosition<Column>(top, right, bottom, left).AlignItemsStretch<Column>().IgnoreInputPicking<Column>();
      }));
      component1.Add((UiComponent) (this.LeftDock = new ObjEditorsDock(true)));
      uiComponentArray[0] = (UiComponent) component1;
      Column component3 = new Column();
      component3.Add<Column>((Action<Column>) (c =>
      {
        Column component4 = c;
        Px? nullable = new Px?(0.px());
        Px? top = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? right = nullable;
        Px? bottom = new Px?(MapEditorScreen.PADDING_TOP_BOTTOM);
        Px? left = new Px?();
        component4.AbsolutePosition<Column>(top, right, bottom, left).AlignItemsStretch<Column>().IgnoreInputPicking<Column>();
      }));
      component3.Add((UiComponent) (this.RightDock = new ObjEditorsDock(false)));
      uiComponentArray[1] = (UiComponent) component3;
      uiComponentArray[2] = (UiComponent) (this.Toolbar = mapEditorToolbar.AbsolutePositionFillParent<MapEditorToolbar>().IgnoreInputPicking<MapEditorToolbar>());
      this.Add(uiComponentArray);
      this.LeftDock.AddDockingReference(this.RightDock, "Assets/Unity/UserInterface/General/DoubleArrowsRight.svg", "Dock right".AsLoc());
      this.RightDock.AddDockingReference(this.LeftDock, "Assets/Unity/UserInterface/General/DoubleArrowsLeft.svg", "Dock left".AsLoc());
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<bool>((Func<bool>) (() => !mapEditor.IsEditorBusy)).Do((Action<bool>) (notBusy =>
      {
        mapEditorScreen.LeftDock.Enabled<ObjEditorsDock>(notBusy);
        mapEditorScreen.RightDock.Enabled<ObjEditorsDock>(notBusy);
      }));
      this.m_updater = updaterBuilder.Build();
    }

    public void RenderUpdate(GameTime time)
    {
      this.m_updater.RenderUpdate();
      this.LeftDock.RenderUpdate(time);
      this.RightDock.RenderUpdate(time);
    }

    public void SyncUpdate(GameTime time) => this.m_updater.SyncUpdate();

    public void ShowEditor(ObjEditor editor, ObjEditorDock dock = ObjEditorDock.None)
    {
      foreach (UiComponent allTab in this.LeftDock.GetAllTabs())
      {
        if (allTab == editor)
        {
          this.LeftDock.SwitchToTab(allTab);
          return;
        }
      }
      foreach (UiComponent allTab in this.RightDock.GetAllTabs())
      {
        if (allTab == editor)
        {
          this.RightDock.SwitchToTab(allTab);
          return;
        }
      }
      editor.DockSelfTo(dock == ObjEditorDock.Right ? this.RightDock : this.LeftDock);
    }

    /// <summary>
    /// Add an editor to the set shown in the menu. Editors can be removed from dock panels
    /// but will still be available to re-add if shown in the menu.
    /// </summary>
    /// <param name="editor">Persistent editor to add</param>
    /// <param name="dock">Where to dock the editor</param>
    public void AddEditor(ObjEditor editor, ObjEditorDock dock, bool showInMenu)
    {
      if (showInMenu && editor.EditedObject.HasValue)
        this.Toolbar.AddMenuEditor(editor);
      if (dock == ObjEditorDock.None)
        return;
      editor.DockSelfTo(dock == ObjEditorDock.Left ? this.LeftDock : this.RightDock);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      return this.LeftDock.InputUpdate(inputScheduler) || this.RightDock.InputUpdate(inputScheduler) || this.Toolbar.InputUpdate(inputScheduler);
    }

    static MapEditorScreen()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MapEditorScreen.PADDING_TOP_BOTTOM = 50.px();
    }
  }
}
