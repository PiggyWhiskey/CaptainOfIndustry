// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.MapEditorToolbar
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.MainMenu;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UiToolkit.Library.ObjectEditor;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class MapEditorToolbar : Mafi.Unity.UiToolkit.Library.Column
  {
    public static readonly Px PANEL_OFFSET;
    private static readonly Px INNER_TOOLBAR_PADDING;
    private static readonly Px TOP_TOOLBAR_HEIGHT;
    private readonly Mafi.Unity.MapEditor.MapEditor m_editor;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IUnityInputMgr m_inputManager;
    private readonly ErrorWindow m_errorWindow;
    private readonly IconClickable m_errorButton;
    private readonly Dropdown<ObjEditor> m_moreMenu;
    private readonly Mafi.Unity.UiToolkit.Library.Row m_topToolbar;
    private readonly Mafi.Unity.UiToolkit.Library.Row m_viewToolbar;
    private readonly Mafi.Unity.UiToolkit.Library.Row m_bottomToolbar;
    private readonly ControlsToolbox m_controlsToolbox;
    private readonly Mafi.Unity.UiToolkit.Library.Row m_progressPanel;
    private readonly UiComponent m_progressBar;
    private readonly Mafi.Unity.UiToolkit.Library.Label m_progressLabel;
    private readonly Mafi.Unity.UiToolkit.Library.Button m_cancelButton;
    private readonly ButtonText m_copyButton;
    private readonly IconClickable m_closeButton;
    private Option<IVisualElementScheduledItem> m_hideProgressTask;
    private readonly ProtosDb m_protosDb;
    private readonly XRayTool m_xRayTool;
    private readonly IUiUpdater m_updater;
    private bool m_hasUnseenErrors;

    private Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig Config
    {
      get => this.m_editor.ToolbarConfig;
    }

    public MapEditorToolbar(
      Mafi.Unity.MapEditor.MapEditor editor,
      XRayTool xRayTool,
      ProtosDb protosDb,
      ShortcutsManager shortcutsManager,
      IUnityInputMgr inputManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_editor = editor;
      this.m_shortcutsManager = shortcutsManager;
      this.m_inputManager = inputManager;
      this.m_xRayTool = xRayTool;
      this.m_protosDb = protosDb;
      this.m_errorWindow = new ErrorWindow();
      UpdaterBuilder updater = UpdaterBuilder.Start();
      this.AlignItemsStretch<MapEditorToolbar>();
      UiComponent[] uiComponentArray = new UiComponent[5];
      Px? gap1 = new Px?();
      Mafi.Unity.UiToolkit.Library.Row component1 = new Mafi.Unity.UiToolkit.Library.Row(gap: gap1);
      component1.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c =>
      {
        Mafi.Unity.UiToolkit.Library.Row component2 = c;
        Px? nullable = new Px?(0.px());
        Px? top = new Px?(MapEditorToolbar.TOP_TOOLBAR_HEIGHT);
        Px? right = new Px?(0.px());
        Px? bottom = new Px?();
        Px? left = nullable;
        component2.AbsolutePosition<Mafi.Unity.UiToolkit.Library.Row>(top, right, bottom, left).JustifyItemsCenter<Mafi.Unity.UiToolkit.Library.Row>().IgnoreInputPicking<Mafi.Unity.UiToolkit.Library.Row>();
      }));
      Mafi.Unity.UiToolkit.Library.Row component3 = new Mafi.Unity.UiToolkit.Library.Row(Outer.WindowBackground, gap: new Px?(1.pt()));
      component3.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.Width<Mafi.Unity.UiToolkit.Library.Row>(new Px?(400.px())).Padding<Mafi.Unity.UiToolkit.Library.Row>(MapEditorToolbar.PANEL_OFFSET + 3.pt(), 3.pt(), 14.px(), 3.pt()).AlignItemsEnd<Mafi.Unity.UiToolkit.Library.Row>().WrapClass(Cls.slideIn).WrapClass(Cls.fromTop)));
      component3.Add((UiComponent) (this.m_progressLabel = new Mafi.Unity.UiToolkit.Library.Label().FlexGrow<Mafi.Unity.UiToolkit.Library.Label>(1f)));
      component3.Add((UiComponent) (this.m_cancelButton = (Mafi.Unity.UiToolkit.Library.Button) new ButtonText("Cancel".AsLoc(), (Action) (() =>
      {
        this.m_editor.StopRegeneration();
        this.ShowNotification("Cancelled");
      }))));
      component3.Add((UiComponent) (this.m_copyButton = new ButtonText("Copy error".AsLoc())));
      component3.Add((UiComponent) (this.m_closeButton = new IconClickable("Assets/Unity/UserInterface/General/Close.svg", new Action(this.closeProgressPanel)).Small()));
      UiComponent component4 = new UiComponent().Name<UiComponent>("ProgressBar");
      gap1 = new Px?(0.px());
      Px? nullable1 = new Px?(0.px());
      Px? top1 = new Px?();
      Px? right1 = new Px?();
      Px? bottom1 = nullable1;
      Px? left1 = gap1;
      component3.Add(this.m_progressBar = component4.AbsolutePosition<UiComponent>(top1, right1, bottom1, left1).Background<UiComponent>(new ColorRgba?(Theme.PrimaryColor)).Height<UiComponent>(new Px?(4.px())));
      Mafi.Unity.UiToolkit.Library.Row child1 = component3;
      this.m_progressPanel = component3;
      component1.Add((UiComponent) child1);
      uiComponentArray[0] = (UiComponent) component1;
      Mafi.Unity.UiToolkit.Library.Row component5 = new Mafi.Unity.UiToolkit.Library.Row();
      component5.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c =>
      {
        Mafi.Unity.UiToolkit.Library.Row component6 = c;
        Px? nullable2 = new Px?(0.px());
        Px? top2 = new Px?(-MapEditorToolbar.PANEL_OFFSET);
        Px? right2 = new Px?(0.px());
        Px? bottom2 = new Px?();
        Px? left2 = nullable2;
        component6.AbsolutePosition<Mafi.Unity.UiToolkit.Library.Row>(top2, right2, bottom2, left2).JustifyItemsCenter<Mafi.Unity.UiToolkit.Library.Row>().IgnoreInputPicking<Mafi.Unity.UiToolkit.Library.Row>();
      }));
      Mafi.Unity.UiToolkit.Library.Row component7 = new Mafi.Unity.UiToolkit.Library.Row(1.px());
      component7.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.Padding<Mafi.Unity.UiToolkit.Library.Row>(MapEditorToolbar.PANEL_OFFSET, 2.pt(), MapEditorToolbar.INNER_TOOLBAR_PADDING, 2.pt()).AlignItemsStart<Mafi.Unity.UiToolkit.Library.Row>()));
      component7.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Row(Outer.WindowBackground).AbsolutePositionFillParent<Mafi.Unity.UiToolkit.Library.Row>());
      component7.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/Undo.svg").OnClick<IconClickable>((Action) (() => this.m_editor.Undo())).Apply<IconClickable>((Action<IconClickable>) (btn => updater.Observe<int>((Func<int>) (() => this.m_editor.UndoManager.PossibleUndoCount)).Do((Action<int>) (undoCount =>
      {
        btn.Enabled<IconClickable>(undoCount > 0);
        btn.Tooltip<IconClickable>(new LocStrFormatted?(string.Format("[{0}] Undo ({1})", (object) this.m_shortcutsManager.Undo.ToNiceString(), (object) undoCount).AsLoc()));
      })))));
      component7.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/Redo.svg").OnClick<IconClickable>((Action) (() => this.m_editor.Redo())).Apply<IconClickable>((Action<IconClickable>) (btn => updater.Observe<int>((Func<int>) (() => this.m_editor.UndoManager.PossibleRedoCount)).Do((Action<int>) (redoCount =>
      {
        btn.Enabled<IconClickable>(redoCount > 0);
        btn.Tooltip<IconClickable>(new LocStrFormatted?(string.Format("[{0}] Redo ({1})", (object) this.m_shortcutsManager.Redo.ToNiceString(), (object) redoCount).AsLoc()));
      })))));
      Outer panel = Outer.Panel;
      Px? nullable3 = new Px?();
      Px? gap2 = nullable3;
      Mafi.Unity.UiToolkit.Library.Row row = new Mafi.Unity.UiToolkit.Library.Row(panel, gap: gap2);
      row.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.Padding<Mafi.Unity.UiToolkit.Library.Row>(MapEditorToolbar.PANEL_OFFSET + 1.pt(), 4.pt(), 3.pt(), 3.pt()).MarginLeft<Mafi.Unity.UiToolkit.Library.Row>(3.pt()).MarginRight<Mafi.Unity.UiToolkit.Library.Row>(2.pt()).MarginTopBottom<Mafi.Unity.UiToolkit.Library.Row>(-MapEditorToolbar.PANEL_OFFSET)));
      row.Add((UiComponent) (this.m_moreMenu = new IconDropdown<ObjEditor>("Assets/Unity/UserInterface/Toolbar/Menu.svg").MarginRight<IconDropdown<ObjEditor>>(2.pt()).SetOptionViewFactory((Func<ObjEditor, int, bool, UiComponent>) ((ed, _1, _2) => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(ed.Title))).SetSearchStringLookup((Func<ObjEditor, string>) (ed => ed.Title.Value)).HideCheckmarks().OnValueChanged((Action<Dropdown<ObjEditor>, ObjEditor, int>) ((dropdown, ed, _) => this.m_editor.EditorScreen.ShowEditor(ed)))));
      row.Add((UiComponent) new ButtonPrimary((LocStrFormatted) Tr.ApplyChanges, (Action) (() => this.m_editor.ApplyChangesNow())).AlignSelf<ButtonPrimary>(Mafi.Unity.UiToolkit.Component.Align.Center).FlipNotches<ButtonPrimary>().Apply<ButtonPrimary>((Action<ButtonPrimary>) (btn => updater.Observe<RectangleTerrainArea2i?>((Func<RectangleTerrainArea2i?>) (() => this.Config.AreaToRegenerate)).Do((Action<RectangleTerrainArea2i?>) (areaToRegenerate => btn.Enabled<ButtonPrimary>(areaToRegenerate.HasValue).Tooltip<ButtonPrimary>(new LocStrFormatted?(("[" + this.m_shortcutsManager.EditorApplyChanges.ToNiceString() + "] " + (areaToRegenerate.HasValue ? this.Config.GetRegeneratedAreaSummary() : "Regenerate custom area with Ctrl + Shift and left mouse drag.")).AsLoc())))))));
      row.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Rotate128.png").MarginLeft<IconClickable>(1.pt()).Tooltip<IconClickable>(new LocStrFormatted?("Regenerate the entire map\n\nPartial map regeneration using 'Apply changes' is fast but it may result in glitches or terrain seams. Use this to preview the map exactly as it would appear in the game.".AsLoc())).OnClick<IconClickable>((Action) (() => this.m_editor.RegenerateEntireMap())));
      row.Add((UiComponent) (this.m_errorButton = new IconClickable("Assets/Unity/UserInterface/General/Warning128.png").Tooltip<IconClickable>(new LocStrFormatted?("View errors".AsLoc())).OnClick<IconClickable>((Action) (() =>
      {
        this.m_hasUnseenErrors = false;
        if (this.m_errorWindow.Parent.IsNone)
          this.RunWithBuilder((Action<UiBuilder>) (b => b.AddComponent((UiComponent) this.m_errorWindow)));
        else
          this.m_errorWindow.Visible<ErrorWindow>(true);
      })).Enabled<IconClickable>(false)));
      component7.Add((UiComponent) row);
      component7.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Save.svg").Medium().Padding<IconClickable>(2.pt()).OnClick<IconClickable>((Action) (() => this.m_editor.Save())).Apply<IconClickable>((Action<IconClickable>) (icon => updater.Observe<string>((Func<string>) (() => this.Config.MapNameWip)).Do((Action<string>) (mapName => icon.Tooltip<IconClickable>(new LocStrFormatted?(("Save map as '" + this.Config.MapNameWip + "'. \nSaves the map as work-in-progress version. To make the map final or change the file name, see the Publishing tab.").AsLoc())))))));
      component7.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/OpenInFolder.svg").Tooltip<IconClickable>(new LocStrFormatted?("Load map".AsLoc())).OnClick<IconClickable>((Action) (() => this.m_editor.ShowLoadTab())));
      component7.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Publish.svg").Tooltip<IconClickable>(new LocStrFormatted?("Publish map".AsLoc())).OnClick<IconClickable>((Action) (() => this.m_editor.ShowPublishTab())));
      Mafi.Unity.UiToolkit.Library.Row child2 = component7;
      this.m_topToolbar = component7;
      component5.Add((UiComponent) child2);
      uiComponentArray[1] = (UiComponent) component5;
      ControlsToolbox component8 = this.m_controlsToolbox = new ControlsToolbox(this.m_shortcutsManager);
      nullable3 = new Px?(0.px());
      gap1 = new Px?(0.px());
      Px? top3 = new Px?();
      Px? right3 = gap1;
      Px? bottom3 = nullable3;
      Px? left3 = new Px?();
      ControlsToolbox component9 = component8.AbsolutePosition<ControlsToolbox>(top3, right3, bottom3, left3);
      Percent? nullable4 = new Percent?(50.Percent());
      Percent? top4 = new Percent?();
      Percent? right4 = new Percent?();
      Percent? bottom4 = new Percent?();
      Percent? left4 = nullable4;
      uiComponentArray[2] = (UiComponent) component9.AbsolutePosition<ControlsToolbox>(top4, right4, bottom4, left4).JustifyItemsStart<ControlsToolbox>().PaddingLeft<ControlsToolbox>(240.px());
      gap1 = new Px?();
      Mafi.Unity.UiToolkit.Library.Row component10 = new Mafi.Unity.UiToolkit.Library.Row(gap: gap1);
      component10.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c =>
      {
        Mafi.Unity.UiToolkit.Library.Row component11 = c;
        Px? nullable5 = new Px?(-MapEditorToolbar.PANEL_OFFSET);
        Px? nullable6 = new Px?(0.px());
        Px? nullable7 = new Px?(0.px());
        Px? top5 = new Px?();
        Px? right5 = nullable7;
        Px? bottom5 = nullable5;
        Px? left5 = nullable6;
        component11.AbsolutePosition<Mafi.Unity.UiToolkit.Library.Row>(top5, right5, bottom5, left5).JustifyItemsCenter<Mafi.Unity.UiToolkit.Library.Row>().IgnoreInputPicking<Mafi.Unity.UiToolkit.Library.Row>();
      }));
      Outer windowBackground = Outer.WindowBackground;
      gap1 = new Px?();
      Px? gap3 = gap1;
      Mafi.Unity.UiToolkit.Library.Row component12 = new Mafi.Unity.UiToolkit.Library.Row(windowBackground, gap: gap3);
      component12.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.Padding<Mafi.Unity.UiToolkit.Library.Row>(MapEditorToolbar.INNER_TOOLBAR_PADDING, 2.pt(), MapEditorToolbar.PANEL_OFFSET, 2.pt()).AlignItemsEnd<Mafi.Unity.UiToolkit.Library.Row>().IgnoreInputPicking<Mafi.Unity.UiToolkit.Library.Row>()));
      component12.Add(this.buildBottomToolbar(updater));
      Mafi.Unity.UiToolkit.Library.Row child3 = component12;
      this.m_bottomToolbar = component12;
      component10.Add((UiComponent) child3);
      uiComponentArray[3] = (UiComponent) component10;
      gap1 = new Px?();
      Mafi.Unity.UiToolkit.Library.Row component13 = new Mafi.Unity.UiToolkit.Library.Row(gap: gap1);
      component13.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c =>
      {
        Mafi.Unity.UiToolkit.Library.Row component14 = c;
        Px? nullable8 = new Px?(-MapEditorToolbar.PANEL_OFFSET);
        Px? nullable9 = new Px?(-MapEditorToolbar.PANEL_OFFSET);
        Px? top6 = new Px?();
        Px? right6 = new Px?();
        Px? bottom6 = nullable9;
        Px? left6 = nullable8;
        component14.AbsolutePosition<Mafi.Unity.UiToolkit.Library.Row>(top6, right6, bottom6, left6).IgnoreInputPicking<Mafi.Unity.UiToolkit.Library.Row>();
      }));
      Mafi.Unity.UiToolkit.Library.Row component15 = new Mafi.Unity.UiToolkit.Library.Row(Outer.WindowBackground, gap: new Px?(1.px()));
      component15.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.Padding<Mafi.Unity.UiToolkit.Library.Row>(MapEditorToolbar.INNER_TOOLBAR_PADDING, 2.pt(), MapEditorToolbar.PANEL_OFFSET, MapEditorToolbar.PANEL_OFFSET).AlignItemsEnd<Mafi.Unity.UiToolkit.Library.Row>()));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/Resources128.png").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>(this.notifyOfChanges((Action<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig>) (e => e.ShowResourcesBars = !e.ShowResourcesBars))).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.Config.ShowResourcesBars)).Tooltip<IconClickable>(new LocStrFormatted?("Show resource bars".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/TerrainGrid.svg").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>(this.notifyOfChanges((Action<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig>) (e => e.ShowTerrainGrid = !e.ShowTerrainGrid))).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.Config.ShowTerrainGrid)).Tooltip<IconClickable>(new LocStrFormatted?("Show terrain grid".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Layers.svg").OnClick<IconClickable>((Action) (() => this.m_editor.ShowLayersTab())).Tooltip<IconClickable>(new LocStrFormatted?("Inspect terrain layers".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/Fog.svg").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>(this.notifyOfChanges((Action<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig>) (e => e.DisableFog = !e.DisableFog))).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.Config.DisableFog)).Tooltip<IconClickable>(new LocStrFormatted?("Hide fog".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/NoGoal.svg").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>((Action) (() => this.m_editor.SuppressHandles = !this.m_editor.SuppressHandles)).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.m_editor.SuppressHandles)).Tooltip<IconClickable>(new LocStrFormatted?("Hide handles".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/HidePreviews.svg").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>((Action) (() => this.m_editor.SuppressPreviews = !this.m_editor.SuppressPreviews)).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.m_editor.SuppressPreviews)).Tooltip<IconClickable>(new LocStrFormatted?("Hide previews".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/TerrainDetails.svg").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>(this.notifyOfChanges((Action<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig>) (e => e.DisableTerrainDetails = !e.DisableTerrainDetails))).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.Config.DisableTerrainDetails)).Tooltip<IconClickable>(new LocStrFormatted?("Hide terrain details".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/Wave.svg").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>(this.notifyOfChanges((Action<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig>) (e => e.TransparentOcean = !e.TransparentOcean))).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.Config.TransparentOcean)).Tooltip<IconClickable>(new LocStrFormatted?("Hide ocean".AsLoc())));
      component15.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/Toolbar/XRayView.svg").Class<IconClickable>(Cls.toggle).OnClick<IconClickable>((Action) (() => this.m_inputManager.ToggleController((IUnityInputController) this.m_xRayTool))).SelectedObserve<IconClickable>(updater, (Func<bool>) (() => this.m_xRayTool.IsEnabled)).Tooltip<IconClickable>(new LocStrFormatted?("X-ray view\n\nSee underground around the cursor".AsLoc())));
      component15.Add((UiComponent) new IconDropdown<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>("Assets/Unity/UserInterface/General/Path.svg").SelectedObserve<IconDropdown<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>>(updater, (Func<bool>) (() => this.Config.Overlay == Mafi.Unity.MapEditor.MapEditor.OverlayMode.Pathability)).InnerDecorationClass(Cls.toggle).Tooltip<Dropdown<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>>(new LocStrFormatted?("Show vehicle pathability".AsLoc())).IncludeClearOption().SetOptions(this.Config.PathabilityClasses.IsEmpty ? Enumerable.Empty<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>() : this.Config.PathabilityClasses.AsEnumerable()).SetOptionViewFactory((Func<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption, int, bool, UiComponent>) ((group, idx, _) => (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(group.Name).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?(group.Tooltip)))).OnValueChanged((Action<Dropdown<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>, Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption, int>) ((btn, group, _) =>
      {
        this.Config.PathabilityClass = group.Class;
        this.Config.Overlay = group.IsEmpty ? Mafi.Unity.MapEditor.MapEditor.OverlayMode.None : Mafi.Unity.MapEditor.MapEditor.OverlayMode.Pathability;
        this.m_editor.OnToolbarConfigChanged(false);
        (btn as IconDropdown<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>).Selected<IconDropdown<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>>(!group.IsEmpty);
      })).ValueIndexObserve<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig.PathabilityClassOption>(updater, (Func<int>) (() => this.Config.Overlay != Mafi.Unity.MapEditor.MapEditor.OverlayMode.None ? this.Config.PathabilityClass : -1)));
      Mafi.Unity.UiToolkit.Library.Row child4 = component15;
      this.m_viewToolbar = component15;
      component13.Add((UiComponent) child4);
      uiComponentArray[4] = (UiComponent) component13;
      this.Add(uiComponentArray);
      updater.Observe<bool>((Func<bool>) (() => !this.m_editor.IsEditorBusy)).Do((Action<bool>) (notBusy =>
      {
        this.m_topToolbar.Enabled<Mafi.Unity.UiToolkit.Library.Row>(notBusy);
        this.m_viewToolbar.Enabled<Mafi.Unity.UiToolkit.Library.Row>(notBusy);
        this.m_bottomToolbar.Enabled<Mafi.Unity.UiToolkit.Library.Row>(notBusy);
      }));
      updater.Observe<bool>((Func<bool>) (() => this.m_xRayTool.IsEnabled)).Observe<Option<LayoutEntityProto>>((Func<Option<LayoutEntityProto>>) (() => this.m_editor.EntityTesterPreviewController.ProtoBeingTested)).Do((Action<bool, Option<LayoutEntityProto>>) ((xRay, testProto) => this.m_controlsToolbox.ShowTools(xRay ? MapEditorControls.XRay : (testProto.HasValue ? MapEditorControls.TestEntityPlacement : MapEditorControls.None))));
      this.m_updater = updater.Build();
    }

    private IEnumerable<UiComponent> buildBottomToolbar(UpdaterBuilder updater)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<UiComponent>) new MapEditorToolbar.\u003CbuildBottomToolbar\u003Ed__28(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__updater = updater
      };
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Undo))
      {
        this.m_editor.Undo();
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Redo))
      {
        this.m_editor.Redo();
        return true;
      }
      if (!this.m_shortcutsManager.IsDown(this.m_shortcutsManager.EditorApplyChanges))
        return false;
      this.m_editor.ApplyChangesNow();
      return true;
    }

    public void AddMenuEditor(ObjEditor editor) => this.m_moreMenu.AddOption(editor);

    public void UpdateProgress(RegenProgress progress)
    {
      bool enabled = progress.Progress.IsPositive && progress.Progress < Percent.Hundred;
      string str = !enabled || !(progress.Progress > Percent.One) ? progress.Message ?? "" : progress.Message + " (" + progress.Progress.ToStringRounded() + ")";
      this.m_progressBar.Width<UiComponent>(progress.Progress.Clamp0To100());
      Mafi.Unity.UiToolkit.Library.Label component = this.m_progressLabel.Text<Mafi.Unity.UiToolkit.Library.Label>(str.AsLoc());
      string valueOrNull = progress.ErrorTip.ValueOrNull;
      LocStrFormatted? text = valueOrNull != null ? new LocStrFormatted?(valueOrNull.AsLoc()) : new LocStrFormatted?();
      int num = progress.IsError ? 1 : 0;
      component.Tooltip<Mafi.Unity.UiToolkit.Library.Label>(text, num != 0).ClassIff<Mafi.Unity.UiToolkit.Library.Label>(Cls.warningText, progress.IsError);
      this.m_cancelButton.Visible<Mafi.Unity.UiToolkit.Library.Button>(true).Enabled<Mafi.Unity.UiToolkit.Library.Button>(enabled);
      this.m_copyButton.Visible<ButtonText>(false);
      this.m_closeButton.Visible<IconClickable>(false);
      this.m_hideProgressTask.ValueOrNull?.Pause();
      this.m_progressPanel.WrapClass(Cls.show);
      if (progress.IsError)
      {
        this.m_cancelButton.Visible<Mafi.Unity.UiToolkit.Library.Button>(false);
        this.m_copyButton.Visible<ButtonText>(true).OnClick<ButtonText>((Action) (() => GUIUtility.systemCopyBuffer = progress.Message + ":\n" + progress.ErrorTip.Value));
        this.m_closeButton.Visible<IconClickable>(true);
        this.m_errorWindow.AddErrorThreadSafe(progress.Message, progress.ErrorTip.Value);
        this.m_hasUnseenErrors = true;
      }
      else
      {
        if (enabled)
          return;
        if (this.m_hideProgressTask.IsNone)
          this.m_hideProgressTask = this.Schedule.Execute(new Action(this.closeProgressPanel)).SomeOption<IVisualElementScheduledItem>();
        this.m_hideProgressTask.Value.ExecuteLater(2000L);
      }
    }

    public void ShowNotification(string message) => this.UpdateProgress(new RegenProgress(message));

    public void ShowErrorNotification(string message, string errorTip)
    {
      string message1 = message;
      Option<string> option = (Option<string>) errorTip;
      Percent progress = new Percent();
      Option<string> errorTip1 = option;
      this.UpdateProgress(new RegenProgress(message1, progress, errorTip1));
    }

    public void LogErrorSilentlyAndThreadSafe(string message, string errorTip)
    {
      this.m_errorWindow.AddErrorThreadSafe(message, errorTip);
      this.m_hasUnseenErrors = true;
    }

    private void closeProgressPanel()
    {
      this.m_progressPanel.WrapClass(Cls.show, false);
      this.m_hideProgressTask.ValueOrNull?.Pause();
    }

    public void RenderUpdate() => this.m_updater.RenderUpdate();

    public void SyncUpdate(GameTime time)
    {
      if (this.m_errorWindow.HasErrors)
        this.m_errorButton.Enabled<IconClickable>(true).Color<IconClickable>(new ColorRgba?(this.m_hasUnseenErrors ? Theme.WarningText : Theme.Text));
      this.m_updater.SyncUpdate();
    }

    private Action<IconClickable> notifyOfChanges(Action<Mafi.Unity.MapEditor.MapEditor.EditorToolbarConfig> apply)
    {
      return (Action<IconClickable>) (button =>
      {
        apply(this.Config);
        this.m_editor.OnToolbarConfigChanged(false);
      });
    }

    static MapEditorToolbar()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      MapEditorToolbar.PANEL_OFFSET = 16.px();
      MapEditorToolbar.INNER_TOOLBAR_PADDING = 6.px();
      MapEditorToolbar.TOP_TOOLBAR_HEIGHT = 26.px();
    }
  }
}
