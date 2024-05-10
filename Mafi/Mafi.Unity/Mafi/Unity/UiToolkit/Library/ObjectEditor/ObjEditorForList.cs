// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.ObjectEditor.ObjEditorForList
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Component.Manipulators;
using Mafi.Unity.UiToolkit.Library.ObjectEditor.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.ObjectEditor
{
  internal class ObjEditorForList : Mafi.Unity.UiToolkit.Library.Column, IObjEditor
  {
    private readonly ObjEditor m_editor;
    private ILystNonGeneric m_list;
    private readonly Lyst<IObjEditor> m_innerEditors;
    private readonly ComponentsCache<ObjEditorForList.ListItem> m_itemsCache;
    private readonly IconClickable m_collapseChildrenBtn;
    private readonly ObjEditorLabel m_label;
    private readonly Mafi.Unity.UiToolkit.Library.Row m_btnsContainer;
    private readonly Mafi.Unity.UiToolkit.Library.Column m_items;
    private System.Type m_innerType;
    private ObjEditorData m_data;

    public UiComponent Component => (UiComponent) this;

    public ObjEditorForList(ObjEditor editor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_innerEditors = new Lyst<IObjEditor>();
      Px? gap = new Px?();
      // ISSUE: explicit constructor call
      base.\u002Ector(gap: gap);
      this.m_editor = editor;
      this.m_itemsCache = new ComponentsCache<ObjEditorForList.ListItem>((Func<ObjEditorForList.ListItem>) (() => new ObjEditorForList.ListItem(this)));
      this.AlignItemsStretch<ObjEditorForList>();
      26.px();
      UiComponent[] uiComponentArray = new UiComponent[3];
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
      Mafi.Unity.UiToolkit.Library.Column component2 = new Mafi.Unity.UiToolkit.Library.Column(ObjEditor.LIST_GAP);
      component2.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().Padding<Mafi.Unity.UiToolkit.Library.Column>(ObjEditor.NESTING_OFFSET)));
      Mafi.Unity.UiToolkit.Library.Column column = component2;
      this.m_items = component2;
      uiComponentArray[1] = (UiComponent) column;
      Mafi.Unity.UiToolkit.Library.Row component3 = new Mafi.Unity.UiToolkit.Library.Row();
      component3.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.JustifyItemsEnd<Mafi.Unity.UiToolkit.Library.Row>().Gap<Mafi.Unity.UiToolkit.Library.Row>(new Px?(2.px()))));
      Mafi.Unity.UiToolkit.Library.Row row = component3;
      this.m_btnsContainer = component3;
      uiComponentArray[2] = (UiComponent) row;
      this.Add(uiComponentArray);
    }

    private void onOrderChanged(int oldIndex, int newIndex)
    {
      if (oldIndex < 0 || newIndex < 0 || oldIndex >= this.m_list.Count || newIndex >= this.m_list.Count)
      {
        Log.Error(string.Format("Indices '{0}','{1}' out of range: 0 - {2}", (object) oldIndex, (object) newIndex, (object) this.m_list.Count));
      }
      else
      {
        object obj = this.m_list[oldIndex];
        this.m_list.RemoveAt(oldIndex);
        this.m_list.Insert(newIndex, obj);
        this.m_editor.ReportValueChanged(rebuildUi: true);
      }
    }

    private void onCollapse(bool isCollapsed)
    {
      this.m_items.SetVisible(!isCollapsed);
      this.m_btnsContainer.SetVisible(!isCollapsed);
      this.m_collapseChildrenBtn.SetVisible(!isCollapsed);
    }

    private void onCollapseClicked(bool isCollapsed)
    {
      this.m_editor.SetCollapsed((object) this.m_list, this.m_data.LabelForCollapse, isCollapsed);
      this.onCollapse(isCollapsed);
    }

    public void SetData(ObjEditorData data)
    {
      this.m_list = (ILystNonGeneric) data.Value;
      this.m_data = data;
      this.m_innerType = data.Type.GetGenericArguments()[0];
      this.m_innerEditors.Clear();
      this.m_items.Clear();
      this.m_itemsCache.ReturnAll();
      this.m_label.SetText(string.Format("{0} ({1} items)", (object) data.Label, (object) this.m_list.Count), data.Tooltip, data.IsLabelHeader, data.NestingLevel);
      MemberInfo valueOrNull = data.Member.ValueOrNull;
      this.m_label.SetCollapsed(this.m_editor.IsCollapsed((object) this.m_list, this.m_data.LabelForCollapse, (object) valueOrNull != null && valueOrNull.HasAttribute<EditorCollapseObjectAttribute>()));
      EditorCollectionAttribute customAttribute = data.Member.Value.GetCustomAttribute<EditorCollectionAttribute>();
      bool allowReorder = customAttribute != null && customAttribute.AllowReorder;
      bool allowRemoval = customAttribute != null && customAttribute.AllowRemoval;
      ObjEditor.ApplyClassForNesting((UiComponent) this.m_items, data);
      for (int index1 = 0; index1 < this.m_list.Count; ++index1)
      {
        object obj = this.m_list[index1];
        string str = string.Format("{0}.", (object) (index1 + 1));
        string labelForCollapse = this.resolveClassName(obj);
        string label = labelForCollapse.IsNotEmpty() ? str + " " + labelForCollapse : str;
        Option<IObjEditor> editorFor = this.m_editor.EditorsRegistry.CreateEditorFor(this.m_editor, new ObjEditorData(obj.GetType(), obj, data.NestingLevel + 2, label, labelForCollapse));
        if (editorFor.HasValue)
        {
          this.m_innerEditors.Add(editorFor.Value);
          ObjEditorForList.ListItem child = this.m_itemsCache.GetView().SetData(editorFor.Value, index1, data.NestingLevel + 1, allowReorder, allowRemoval);
          this.m_items.Add((UiComponent) child);
          ObjEditorsRegistry.RegisteredAction registeredAction;
          if (this.m_editor.EditorsRegistry.TryGetRegisteredAction(obj.GetType(), out registeredAction))
          {
            ObjEditorForAction editorInstance = this.m_editor.GetEditorInstance<ObjEditorForAction>();
            int index = index1;
            editorInstance.SetData(registeredAction, obj, (Action<object>) (v => this.m_list[index] = v));
            child.AddActionButton(editorInstance);
          }
        }
        else
          Log.Error("Editor not found!");
      }
      if (this.m_list.Count <= 0)
      {
        Mafi.Unity.UiToolkit.Library.Column items = this.m_items;
        Mafi.Unity.UiToolkit.Library.Label label = new Mafi.Unity.UiToolkit.Library.Label("List is empty".AsLoc());
        label.Add<Mafi.Unity.UiToolkit.Library.Label>((Action<Mafi.Unity.UiToolkit.Library.Label>) (c => c.AlignText<Mafi.Unity.UiToolkit.Library.Label>(TextAlign.CenterMiddle)));
        items.Add((UiComponent) label);
      }
      this.m_btnsContainer.Visible<Mafi.Unity.UiToolkit.Library.Row>(!this.m_label.IsCollapsed);
      this.m_collapseChildrenBtn.SetVisible(this.m_list.Count > 0 && this.m_innerEditors.Any<IObjEditor>((Predicate<IObjEditor>) (x => x is ObjEditorComposite objEditorComposite && objEditorComposite.CanBeCollapsed)));
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

    private string resolveClassName(object val)
    {
      if (val == null)
        return string.Empty;
      string str = (string) null;
      MemberInfo memberInfo = ((IEnumerable<MemberInfo>) val.GetType().GetMembers()).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (x => x.HasAttribute<EditorClassNameAttribute>()));
      PropertyInfo propertyInfo = memberInfo as PropertyInfo;
      if ((object) propertyInfo != null)
      {
        str = propertyInfo.GetValue(val) as string;
      }
      else
      {
        FieldInfo fieldInfo = memberInfo as FieldInfo;
        if ((object) fieldInfo != null)
          str = fieldInfo.GetValue(val) as string;
      }
      return str ?? string.Empty;
    }

    public bool TryGetValue(out object value)
    {
      foreach (ObjEditorForList.ListItem allExistingOne in this.m_itemsCache.AllExistingOnes())
      {
        object obj;
        if (allExistingOne.Editor.TryGetValue(out obj))
          this.m_list[allExistingOne.Index] = obj;
      }
      value = (object) this.m_list;
      return true;
    }

    protected override void SetEnabledInternal(bool enabled)
    {
      base.SetEnabledInternal(enabled);
      foreach (IObjEditor innerEditor in this.m_innerEditors)
        innerEditor.Component.Enabled<UiComponent>(enabled);
    }

    private void deleteItem(ObjEditorForList.ListItem item)
    {
      int index = item.Index;
      if (index < 0 || index >= this.m_list.Count)
        return;
      this.m_list.RemoveAt(index);
      this.m_editor.ReportValueChanged(rebuildUi: true);
    }

    private class ListItem : Mafi.Unity.UiToolkit.Library.Column
    {
      public IObjEditor Editor;
      public int Index;
      private readonly ObjEditorForList m_editor;
      private readonly Mafi.Unity.UiToolkit.Library.Column m_editorContainer;
      private readonly Reorderable m_reorderManipulator;
      private readonly Mafi.Unity.UiToolkit.Library.Column m_handle;
      private Option<IconButton> m_removeButton;

      public ListItem(ObjEditorForList parent)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_editor = parent;
        this.AlignItemsEnd<ObjEditorForList.ListItem>();
        Mafi.Unity.UiToolkit.Library.Row row = new Mafi.Unity.UiToolkit.Library.Row();
        row.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (e => e.Fill<Mafi.Unity.UiToolkit.Library.Row>().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Row>()));
        Mafi.Unity.UiToolkit.Library.Column component1 = new Mafi.Unity.UiToolkit.Library.Column();
        component1.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.Class<Mafi.Unity.UiToolkit.Library.Column>(Cls.dragHandle).MarginRight<Mafi.Unity.UiToolkit.Library.Column>(2.px())));
        Mafi.Unity.UiToolkit.Library.Column child1 = component1;
        this.m_handle = component1;
        row.Add((UiComponent) child1);
        Mafi.Unity.UiToolkit.Library.Column component2 = new Mafi.Unity.UiToolkit.Library.Column();
        component2.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.Fill<Mafi.Unity.UiToolkit.Library.Column>().AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().Padding<Mafi.Unity.UiToolkit.Library.Column>(ObjEditor.NESTING_OFFSET)));
        Mafi.Unity.UiToolkit.Library.Column child2 = component2;
        this.m_editorContainer = component2;
        row.Add((UiComponent) child2);
        this.Add((UiComponent) row);
        this.m_reorderManipulator = new Reorderable(this.m_handle.RootElement);
        this.m_reorderManipulator.OnOrderChanged += new Action<int, int>(parent.onOrderChanged);
      }

      public ObjEditorForList.ListItem SetData(
        IObjEditor editor,
        int index,
        int nestingLevel,
        bool allowReorder,
        bool allowRemoval)
      {
        this.Editor = editor;
        this.Index = index;
        this.m_editorContainer.SetChildren(new UiComponent[1]
        {
          editor.Component
        });
        ObjEditor.ApplyClassForNesting((UiComponent) this, nestingLevel);
        if (allowReorder)
          this.RootElement.AddManipulator((IManipulator) this.m_reorderManipulator);
        else
          this.RootElement.RemoveManipulator((IManipulator) this.m_reorderManipulator);
        if (allowRemoval && this.m_removeButton.IsNone)
        {
          this.m_removeButton = (Option<IconButton>) new IconButton("Assets/Unity/UserInterface/General/Trash128.png", (Action) (() => this.m_editor.deleteItem(this))).AlignSelf<IconButton>(Mafi.Unity.UiToolkit.Component.Align.End).MarginRight<IconButton>(ObjEditor.NESTING_OFFSET).MarginBottom<IconButton>(1.pt());
          this.Add((UiComponent) this.m_removeButton.Value);
        }
        this.m_handle.SetVisible(allowReorder);
        this.m_removeButton.ValueOrNull?.SetVisible(allowRemoval);
        return this;
      }

      public void AddActionButton(ObjEditorForAction actionBtn)
      {
        this.Add((UiComponent) actionBtn);
      }
    }
  }
}
