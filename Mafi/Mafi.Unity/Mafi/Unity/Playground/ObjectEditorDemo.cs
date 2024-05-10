// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.ObjectEditorDemo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using Mafi.Random.Noise;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UiToolkit.Library.ObjectEditor;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  public class ObjectEditorDemo : IDemoSetup
  {
    public UiComponent Content;
    private int m_lastSeen;
    private TestObjForEditor m_obj;

    public ObjectEditorDemo(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void Run()
    {
      ShortcutsManager shortcutsManager = new ShortcutsManager((IGameLoopEvents) new GameLoopEvents());
      Column component = new Column();
      component.Add<Column>((Action<Column>) (c => c.Fill<Column>().AlignItemsStretch<Column>().Name<Column>("DemoRoot")));
      DemoScreen screen;
      component.Add((UiComponent) (screen = new DemoScreen().OverflowHidden<DemoScreen>()));
      this.Content = (UiComponent) component;
      ObjEditorsRegistry registry = new ObjEditorsRegistry();
      registry.RegisterCustomEditor("Custom editor", typeof (Rect), (ICustomObjEditorFactory) new CustomTestEditorFactory());
      registry.RegisterActionPerType<SimplexNoise2dSeed>(new Func<SimplexNoise2dSeed, SimplexNoise2dSeed>(this.randomize), ObjEditorIcon.Randomize);
      this.Content.Schedule.Execute((Action) (() =>
      {
        screen.LeftDock.RenderUpdate(new GameTime());
        screen.RightDock.RenderUpdate(new GameTime());
      })).Every(100L);
      this.m_obj = new TestObjForEditor();
      ObjEditor objEditor = this.addTab((object) this.m_obj, "Editor 1", screen.LeftDock, registry);
      this.m_lastSeen = this.m_obj.IncreaseToRebuild;
      objEditor.OnValueChanged += new Action<bool>(this.editorOnOnValueChanged);
      this.addTab((object) new TestObjForEditor2(), "Medium Title", screen.LeftDock, registry);
      this.addTab((object) new TestObjForEditor(), "Short", screen.LeftDock, registry);
      this.addTab((object) new TestObjForEditor2(), "Editor With a Pretty Long Title", screen.LeftDock, registry);
      this.addTab((object) new TestObjForEditor(), "Editor 3", screen.LeftDock, registry);
      this.addTab((object) new TestObjForEditor2(), "Slightly longer title here", screen.LeftDock, registry);
      this.addTab((object) new TestObjForEditor(), "Short 2", screen.LeftDock, registry);
      this.addTab((object) new TestObjForEditor2(), "Editor with a pretty and even a bit longer title", screen.LeftDock, registry);
      this.addTab((object) new TestObjForEditor2(), "Editor 2", screen.RightDock, registry);
    }

    private void editorOnOnValueChanged(bool _)
    {
      if (this.m_lastSeen >= this.m_obj.IncreaseToRebuild)
        return;
      this.m_lastSeen = this.m_obj.IncreaseToRebuild;
      ++this.m_obj.CustomRebuildsDone;
      this.m_obj.RebuildIfTrue = true;
    }

    private ObjEditor addTab(
      object obj,
      string title,
      ObjEditorsDock dock,
      ObjEditorsRegistry registry)
    {
      ObjEditor objEditor = new ObjEditor(registry, new ProtosDb(), title.AsLoc());
      objEditor.DockSelfTo(dock);
      objEditor.SetObjectToEdit((Option<object>) obj);
      return objEditor;
    }

    private SimplexNoise2dSeed randomize(SimplexNoise2dSeed input)
    {
      return new SimplexNoise2dSeed(0.11.ToFix32(), 0.11.ToFix32());
    }
  }
}
