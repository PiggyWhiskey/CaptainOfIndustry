// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorForDict
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  internal class ObjEditorForDict : Column, IObjEditor
  {
    private readonly ObjEditor m_editor;
    private IDictNonGeneric m_dict;
    private readonly Lyst<object> m_keys;
    private readonly Lyst<IObjEditor> m_innerEditors;
    private readonly Column m_items;
    private readonly ObjEditorLabel m_label;
    private readonly ComponentsCache<ObjEditorForDict.DictItem> m_itemsCache;
    private readonly IconClickable m_collapseChildrenBtn;
    private ObjEditorData m_data;

    public UiComponent Component => (UiComponent) this;

    public ObjEditorForDict(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_keys = new Lyst<object>();
      this.m_innerEditors = new Lyst<IObjEditor>();
      Px? gap = new Px?();
      // ISSUE: explicit constructor call
      base.\u002Ector(gap: gap);
      this.m_editor = editor;
      this.m_itemsCache = new ComponentsCache<ObjEditorForDict.DictItem>((Func<ObjEditorForDict.DictItem>) (() => new ObjEditorForDict.DictItem()));
      this.AlignItemsStretch<ObjEditorForDict>();
      UiComponent[] uiComponentArray = new UiComponent[2];
      ObjEditorLabel objEditorLabel1 = new ObjEditorLabel(new Action<bool>(this.onCollapse), new Action<bool>(this.onCollapseClicked));
      IconClickable component1 = new IconClickable("Assets/Unity/UserInterface/General/CollapseAll.svg", new Action(this.onCollapseChildren)).GrowOnHover();
      gap = new Px?(1.pt());
      Px? top = new Px?();
      Px? right = gap;
      Px? bottom = new Px?();
      Px? left = new Px?();
      objEditorLabel1.Add((UiComponent) (this.m_collapseChildrenBtn = component1.AbsolutePosition<IconClickable>(top, right, bottom, left).Class<IconClickable>(Cls.primary).Tooltip<IconClickable>(new LocStrFormatted?("Toggle collapse all".AsLoc()))));
      ObjEditorLabel objEditorLabel2 = objEditorLabel1;
      this.m_label = objEditorLabel1;
      uiComponentArray[0] = (UiComponent) objEditorLabel2;
      Column component2 = new Column(ObjEditor.LIST_GAP);
      component2.Add<Column>((Action<Column>) (c => c.AlignItemsStretch<Column>().Padding<Column>(ObjEditor.NESTING_OFFSET)));
      Column column = component2;
      this.m_items = component2;
      uiComponentArray[1] = (UiComponent) column;
      this.Add(uiComponentArray);
    }

    private void onCollapse(bool isCollapsed)
    {
      this.m_items.SetVisible(!isCollapsed);
      this.m_collapseChildrenBtn.SetVisible(!isCollapsed);
    }

    private void onCollapseClicked(bool isCollapsed)
    {
      this.m_editor.SetCollapsed((object) this.m_dict, this.m_data.LabelForCollapse, isCollapsed);
      this.onCollapse(isCollapsed);
    }

    private void onCollapseChildren()
    {
      bool isCollapsed = this.m_innerEditors.OfType<ObjEditorComposite>().Any<ObjEditorComposite>((Func<ObjEditorComposite, bool>) (x => !x.IsCollapsed));
      foreach (IObjEditor innerEditor in this.m_innerEditors)
      {
        if (innerEditor is ObjEditorComposite objEditorComposite)
          objEditorComposite.SetCollapsed(isCollapsed);
      }
    }

    public void SetData(ObjEditorData data)
    {
      this.m_dict = (IDictNonGeneric) data.Value;
      this.m_data = data;
      this.m_innerEditors.Clear();
      this.m_keys.Clear();
      this.m_itemsCache.ReturnAll();
      this.m_items.Clear();
      this.m_label.SetText(data.Label + string.Format(" [{0}]", (object) this.m_dict.Count), data.Tooltip, data.IsLabelHeader, data.NestingLevel);
      MemberInfo valueOrNull = data.Member.ValueOrNull;
      bool defaultValue = (object) valueOrNull != null && valueOrNull.HasAttribute<EditorCollapseObjectAttribute>();
      this.m_label.SetCollapsed(this.m_editor.IsCollapsed((object) this.m_dict, data.LabelForCollapse, defaultValue));
      ObjEditor.ApplyClassForNesting((UiComponent) this.m_items, data.NestingLevel);
      foreach (KeyValuePair<object, object> keyValuePair in this.m_dict)
      {
        KeyValuePair<object, object> pair = keyValuePair;
        Type type = pair.Value.GetType();
        Option<IObjEditor> editorFor = this.m_editor.EditorsRegistry.CreateEditorFor(this.m_editor, new ObjEditorData(type, pair.Value, data.NestingLevel + 2, ObjEditorUtils.ProcessCamelCase(pair.Key.ToString())));
        if (editorFor.HasValue)
        {
          this.m_innerEditors.Add(editorFor.Value);
          this.m_keys.Add(pair.Key);
          ObjEditorForDict.DictItem child = this.m_itemsCache.GetView().SetData(editorFor.Value, data.NestingLevel + 1);
          this.m_items.Add((UiComponent) child);
          ObjEditorsRegistry.RegisteredAction registeredAction;
          if (this.m_editor.EditorsRegistry.TryGetRegisteredAction(type, out registeredAction))
          {
            ObjEditorForAction editorInstance = this.m_editor.GetEditorInstance<ObjEditorForAction>();
            editorInstance.SetData(registeredAction, pair.Value, (Action<object>) (v => this.m_dict[pair.Key] = v));
            child.AddActionButton(editorInstance);
          }
        }
        else
          Log.Error("Editor not found!");
      }
      if (this.m_dict.Count <= 0)
      {
        Column items = this.m_items;
        Label label = new Label("Dictionary is empty".AsLoc());
        label.Add<Label>((Action<Label>) (c => c.AlignText<Label>(TextAlign.CenterMiddle)));
        items.Add((UiComponent) label);
      }
      this.m_collapseChildrenBtn.SetVisible(this.m_dict.Count > 0 && this.m_innerEditors.Any<IObjEditor>((Predicate<IObjEditor>) (x => x is ObjEditorComposite objEditorComposite && objEditorComposite.CanBeCollapsed)));
    }

    public bool TryGetValue(out object value)
    {
      for (int index = 0; index < this.m_innerEditors.Count; ++index)
      {
        object obj;
        if (this.m_innerEditors[index].TryGetValue(out obj))
          this.m_dict[this.m_keys[index]] = obj;
      }
      value = (object) this.m_dict;
      return true;
    }

    protected override void SetEnabledInternal(bool enabled)
    {
      base.SetEnabledInternal(enabled);
      foreach (IObjEditor innerEditor in this.m_innerEditors)
        innerEditor.Component.Enabled<UiComponent>(enabled);
    }

    private class DictItem : Column
    {
      public DictItem()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.AlignItemsStretch<ObjEditorForDict.DictItem>().Padding<ObjEditorForDict.DictItem>(ObjEditor.NESTING_OFFSET);
      }

      public ObjEditorForDict.DictItem SetData(IObjEditor editor, int nestingLevel)
      {
        this.Add(editor.Component);
        ObjEditor.ApplyClassForNesting((UiComponent) this, nestingLevel);
        return this;
      }

      public void AddActionButton(ObjEditorForAction actionBtn)
      {
        this.Add((UiComponent) actionBtn);
      }
    }
  }
}
